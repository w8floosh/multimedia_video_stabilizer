using OpenCvSharp;
using OpenCvSharp.Internal.Vectors;
using OpenCvSharp.Quality;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace Multimedia
{
    public struct FrameObject
    {
        public static ResourcesTracker Tracker { get; private set; }
        public Mat? RGB { get; set; }
        public Mat? Gray { get; private set; }
        public Point2f[] KeyPoints { get; private set; }
        public FrameObject(ResourcesTracker tracker, Feature2D detector, VideoCapture capture)
        {
            RGB = tracker.T(tracker.NewMat());
            Gray = tracker.T(tracker.NewMat());
            if (!capture.Read(RGB))
            {
                KeyPoints = Array.Empty<Point2f>();
                return;
            }
            Cv2.CvtColor(RGB, Gray, ColorConversionCodes.BGR2GRAY);
            KeyPoints = detector.Detect(Gray).Select(keypoint => keypoint.Pt).ToArray();
        }
    }

    public static class MetricComputationAlgorithms
    {
        public static (double, List<List<Point2f>>) InterframeTransformationFidelity(string videoPath, Feature2D? detector, List<List<Point2f>>? _)
        {
            VideoCapture capture = new(videoPath);
            double PSNRSum = 0;
            int nf = capture.FrameCount;
            using (var tracker = new ResourcesTracker()) 
            {
                Mat PrevFrame = tracker.T(tracker.NewMat());
                while (capture.Read(PrevFrame))
                {
                    Mat CurrentFrame = tracker.T(tracker.NewMat());
                    capture.Read(CurrentFrame);
                    if (CurrentFrame.Empty()) break;
                    PSNRSum += (double)QualityPSNR.Compute(InputArray.Create(PrevFrame),InputArray.Create(CurrentFrame), null);
                    PrevFrame = CurrentFrame;
                }
            }
            return (PSNRSum / (nf - 1), null);
        }
        public static (double, List<List<Point2f>>) InterframeSimilarityIndex(string videoPath, Feature2D? detector, List<List<Point2f>>? _)
        {
            VideoCapture capture = new(videoPath);
            double SSIMSum = 0;
            int NoFrames = capture.FrameCount;
            using (var tracker = new ResourcesTracker())
            {
                Mat PrevFrame = tracker.T(tracker.NewMat());
                while (capture.Read(PrevFrame))
                {
                    Mat CurrentFrame = tracker.T(tracker.NewMat());
                    if (capture.Read(CurrentFrame))
                    {
                        SSIMSum += (double)QualitySSIM.Compute(InputArray.Create(PrevFrame), InputArray.Create(CurrentFrame), null);
                        PrevFrame = CurrentFrame;
                    }
                }
            }
            return (SSIMSum / (NoFrames - 1), null);

        }
        public static (double, List<List<Point2f>>) AverageAcceleration(string videoPath, Feature2D? detector, List<List<Point2f>>? inputTracks)
        {
            detector ??= GFTTDetector.Create(qualityLevel: 0.3, minDistance: 1, blockSize: 4);
            VideoCapture capture = new(videoPath);
            List<List<Point2f>> OutputTracks;
            int NoFrames = capture.FrameCount;
            int ProcessedFrames = 0;
            int NoKeyPoints = 0;
            double TotalAccelerations = 0;
            using (var tracker = new ResourcesTracker())
            {
                List<List<Point2f>> tracks = new();
                FrameObject CurrentFrame = new(tracker, detector, capture);
                if (inputTracks == null)
                {
                    //Mat KeyPointsMaskImage = tracker.T(tracker.NewMat());
                    for (int i = 0; i < CurrentFrame.KeyPoints.Length; i++)
                    {
                        List<Point2f> track = new() { CurrentFrame.KeyPoints[i] };
                        tracks.Add(track);
                        //Cv2.Circle(InputOutputArray.Create(KeyPointsMaskImage), (Point)CurrentFrame.KeyPoints[i], 5, Scalar.RandomColor());
                    }
                }
                else tracks = inputTracks;
                NoKeyPoints = tracks.Count;

                OutputTracks = tracks;
                Point2f[] PrevGoodPoints = null;
                Point2f[] CurrentGoodPoints = Array.Empty<Point2f>();
                Point2f[] NextGoodPoints = Array.Empty<Point2f>();
                byte[] status = Array.Empty<byte>();
                float[] err = Array.Empty<float>();
                while (!CurrentFrame.RGB.Empty())
                {
                    //Mat VisualizedFrame = tracker.T(tracker.NewMat());
                    ProcessedFrames++;
                    FrameObject NextFrame = new(tracker, detector, capture);
                    Point2f[] LastPointsTracked = tracks.Select(track => track.Last()).ToArray();

                    List<List<Point2f>> NewTracks = new();
                    if (!NextFrame.RGB.Empty())
                    {
                        // we need to calculate feature points optical flow forwards and backwards to search matching features
                        Cv2.CalcOpticalFlowPyrLK(
                            InputArray.Create(CurrentFrame.Gray),
                            InputArray.Create(NextFrame.Gray),
                            LastPointsTracked,
                            ref NextGoodPoints,
                            out status,
                            out err);

                        Cv2.CalcOpticalFlowPyrLK(
                            InputArray.Create(NextFrame.Gray),
                            InputArray.Create(CurrentFrame.Gray),
                            NextGoodPoints,
                            ref LastPointsTracked,
                            out status,
                            out err);

                        List<Point2f> NextPointsTracked = NextGoodPoints.Where((point, index) => point.DistanceTo(LastPointsTracked[index]) < 1).ToList();
                        var enumerator = NextPointsTracked.GetEnumerator();
                        //(int tr = 0, pt = 0; tr < tracks.Count && pt < NextGoodPoints.Length; tr++, pt++) 
                        foreach (var track in tracks)
                        {
                            if (enumerator.MoveNext())
                            {
                                track.Add(enumerator.Current);
                                //Cv2.Line(KeyPointsMaskImage, (Point)track.Last(), (Point)enumerator.Current, Scalar.RandomColor());
                            }
                        }


                        //Console.WriteLine(String.Format("Current good points: {0}, next good points: {1}", CurrentGoodPoints.Length, NextGoodPoints.Length));
                        if (ProcessedFrames > 2 && ProcessedFrames < NoFrames)
                        {
                            // average acceleration metric formula needs to calculate the norm of acceleration vectors of points, which take into account the frames I(t-1), I(t), I(t+1)
                            IEnumerable<double> Accelerations = CurrentGoodPoints.Select((current, index) =>
                            {
                                try
                                {
                                    Point2f next = NextGoodPoints[index];
                                    Point2f prev = PrevGoodPoints[index];
                                    // acceleration vector formula
                                    Point2f accelerationVector = next - (current.Multiply(2.0)) + prev;
                                    return Math.Sqrt(accelerationVector.X * accelerationVector.X + accelerationVector.Y * accelerationVector.Y);
                                }
                                catch (Exception)
                                {
                                    Debug.WriteLine("Feature points array mismatch: this is likely due to a feature point found in the current frame but not in the previous one");
                                    return 0;
                                }
                            });
                            
                            TotalAccelerations += Accelerations.Sum();
                        }
                    }
                    //CurrentFrame.RGB.CopyTo(VisualizedFrame, KeyPointsMaskImage);
                    PrevGoodPoints = CurrentGoodPoints;
                    CurrentGoodPoints = NextGoodPoints;
                    CurrentFrame = NextFrame;
                    //Console.WriteLine(string.Format("Processed frames: {0} | Keypoints found in last frame: {1} ", ProcessedFrames, CurrentGoodPoints.Length));
                    //Cv2.ImShow("Live computation of optical flow", VisualizedFrame);
                    //Cv2.WaitKey(5);
                }
                tracker.Dispose();
            }
            double result = TotalAccelerations / (NoKeyPoints * (NoFrames - 2));

            Console.WriteLine(string.Format(@"Computation results for {0}: 
                Total accelerations: {1}px/fr²
                Number of keypoints: {2}
                Number of frames: {3}
                Average acceleration value: {4}px/fr²", videoPath, TotalAccelerations, NoKeyPoints, NoFrames, result));
            return (result, OutputTracks);
        }
    }
}
