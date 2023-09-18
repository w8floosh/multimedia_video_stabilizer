using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multimedia
{
    public struct KeyPointDetectorObject
    {
        readonly public string Name { get; }
        readonly public Feature2D Detector { get; }

        public KeyPointDetectorObject(string name, Feature2D detector)
        {
            Name = name;
            Detector = detector;
        }
    }
    public class MetricCalculator
    {
        public string Name { get; }
        public string Unit { get; }
        //public KeyPointDetectorObject KeyPointDetector { get; }
        public Func<string, Feature2D?, List<List<Point2f>>?, (double, List<List<Point2f>>)> Compute { get; }

        public MetricCalculator(string name, Func<string, Feature2D?, List<List<Point2f>>, (double, List<List<Point2f>>)> computer, string unit)
        {
            Name = name;
            //KeyPointDetector = detector;
            Compute = computer;
            Unit = unit;
        }
    }
}
