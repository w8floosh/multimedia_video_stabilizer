using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
using OpenCvSharp;

namespace Multimedia
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 
    public enum VSQAMetric
    {
        [Description("Interframe Transformation Fidelity Index (ITF)")]
        ITF, 
        [Description("Average Acceleration (AvA)")]
        AvA,
        [Description("Interframe Similarity Index (ISI)")]
        ISI
    }
    public enum KPDAlgorithm
    {
        [Description("Features from Accelerated Segment Test (FAST)")]
        FAST,
        [Description("Good Features To Track (GFTT/Harris)")]
        GFTT,
        [Description("Binary Robust Invariant Scalable Keypoints (BRISK)")]
        BRISK,
        [Description("Oriented FAST and Rotated BRIEF (ORB)")]
        ORB
    }
    public partial class App : Application
    {
        public IntPtr PythonThreadState;
        public dynamic sys;
        public Dictionary<VSQAMetric, MetricCalculator> Metrics;
        public Dictionary<KPDAlgorithm, KeyPointDetectorObject> KeypointDetectors;

        public App()
        {
            Metrics = new();
            KeypointDetectors = new();
            Metrics.Add(VSQAMetric.ITF, new(name: "ITF", computer: MetricComputationAlgorithms.InterframeTransformationFidelity, unit: "dB"));
            Metrics.Add(VSQAMetric.AvA, new(name: "AvA", computer: MetricComputationAlgorithms.AverageAcceleration, unit: "px/fr²"));
            Metrics.Add(VSQAMetric.ISI, new(name: "ISI", computer: MetricComputationAlgorithms.InterframeSimilarityIndex, unit: ""));
            KeypointDetectors.Add(KPDAlgorithm.FAST, new(name: "FAST", detector: FastFeatureDetector.Create(42)));
            KeypointDetectors.Add(KPDAlgorithm.GFTT, new(name: "GFTT", detector: GFTTDetector.Create(qualityLevel: 0.3, minDistance: 1, blockSize: 4)));
            KeypointDetectors.Add(KPDAlgorithm.BRISK, new(name: "BRISK", detector: BRISK.Create()));
            KeypointDetectors.Add(KPDAlgorithm.ORB, new(name: "ORB", detector: ORB.Create()));
            (PythonThreadState, sys) = DependencyManager.InitializePythonEnvironment();
            DependencyManager.CheckVidStabDependency();
        }
    }
}
