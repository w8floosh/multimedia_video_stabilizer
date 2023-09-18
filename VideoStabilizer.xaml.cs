using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Forms;
using ComboBox = System.Windows.Controls.ComboBox;
using System.Windows.Markup;
using Binding = System.Windows.Data.Binding;
using Multimedia.CustomMarkupExtensions;
using Multimedia.CustomControls;
using System.ComponentModel;
using Python.Runtime;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Button = System.Windows.Controls.Button;
using System.Threading;
using System.Windows.Threading;
using Application = System.Windows.Application;
using OpenCvSharp;
using Window = System.Windows.Window;
using System.Windows.Forms.VisualStyles;

namespace Multimedia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class VideoStabilizer : Window, INotifyPropertyChanged
    {
        private App global { get; set; }
        private dynamic instance { get; set; }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputPathProperty = DependencyProperty.Register(
            "InputPath", typeof(string), typeof(VideoStabilizer), new PropertyMetadata("")
        );


        public static readonly DependencyProperty OutputPathProperty = DependencyProperty.Register(
            "OutputPath", typeof(string), typeof(VideoStabilizer), new PropertyMetadata("")
        );

        public static readonly DependencyProperty ReadyProperty = DependencyProperty.Register(
            "Ready", typeof(bool), typeof(VideoStabilizer), new PropertyMetadata(false)
        );
        public static readonly DependencyProperty StabilizedProperty = DependencyProperty.Register(
            "Stabilized", typeof(bool), typeof(VideoStabilizer), new PropertyMetadata(false)
        );
        public static readonly DependencyProperty CompletedProperty = DependencyProperty.Register(
            "Completed", typeof(Visibility), typeof(VideoStabilizer), new PropertyMetadata(Visibility.Hidden)
        );
        public bool Ready
        {
            get { return (bool)GetValue(ReadyProperty); }
            set
            {
                SetValue(ReadyProperty, value);
                UpdatedProperty(nameof(Ready));
            }
        }
        public bool Stabilized
        {
            get { return (bool)GetValue(StabilizedProperty); }
            set
            {
                SetValue(StabilizedProperty, value);
                UpdatedProperty(nameof(Stabilized));
            }
        }

        public string InputPath
        {
            get { return (string)GetValue(InputPathProperty); }
            set {
                SetValue(InputPathProperty, value);
                UpdatedProperty(nameof(InputPath)); 
            }
        }

        public string OutputPath
        {
            get { return (string)GetValue(OutputPathProperty); }
            set { 
                SetValue(OutputPathProperty, value);
                UpdatedProperty(nameof(OutputPath));
            }
        }
        public Visibility Completed
        {
            get { return (Visibility)GetValue(CompletedProperty); }
            set
            {
                SetValue(CompletedProperty, value);
                UpdatedProperty(nameof(Completed));
            }
        }
        public MetricCalculator Metric { get; private set; }
        public KeyPointDetectorObject KeypointDetector { get; private set; }
        public VideoStabilizer()
        {
            global = (App)System.Windows.Application.Current;
            DataContext = this;
            InitializeComponent();

            InputPath = " ";
            OutputPath = " ";
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void UpdatedProperty(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        private void SwitchKPDAlgorithm(object sender, SelectionChangedEventArgs e)
        {    
            KeypointDetector = global.KeypointDetectors.GetValueOrDefault((KPDAlgorithm)((ComboBox)sender).SelectedItem);
        }

        private void SwitchVSQAMetric(object sender, SelectionChangedEventArgs e)
        {
            Metric = global.Metrics.GetValueOrDefault((VSQAMetric)((ComboBox)sender).SelectedItem);
        }

        private void ChooseInputPath(object sender, RoutedEventArgs e)
        {
            using OpenFileDialog dialog = new();
            dialog.Filter = "All video files|*.mp4;*.wmv;*.avi;*.mov;*.mpg;*.mpeg;*.flv";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            dialog.RestoreDirectory = true;
            DialogResult choice = dialog.ShowDialog();
            if (choice == System.Windows.Forms.DialogResult.Cancel) return;

            InputPath = dialog.FileName;
            InputPathTextBox.Text = InputPath;
            NotStabilizedVideoPlayer.Video.Source = new Uri(InputPath);
        }
        private void ChooseOutputPath(object sender, RoutedEventArgs e)
        {
            using SaveFileDialog dialog = new();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            dialog.CheckPathExists = true;
            dialog.RestoreDirectory = true;
            dialog.AddExtension = true;
            dialog.DefaultExt = Path.GetExtension(InputPath);
            dialog.Filter = $"Input video format|*{Path.GetExtension(InputPath)}";
            DialogResult choice = dialog.ShowDialog();
            if (choice == System.Windows.Forms.DialogResult.Cancel) return; 
            if (dialog.FileName == "") OutputPath = dialog.InitialDirectory + "/output.avi";
            OutputPath = dialog.FileName;
            OutputPathTextBox.Text = OutputPath;
            if (InputPath != string.Empty) Ready = true;
        }

        private async void Stabilize(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() => { 
                Ready = false;
                StabilizingProgressBar.IsIndeterminate = true;
                Stabilized = false;
                Completed = Visibility.Hidden;
                StabilizeButton.Content = "Stabilizing video...";
                StabilizedVideoPlayer.Video.Source = null;
            });
            try
            {
                string Input = InputPath.Replace("\\", "/");
                string Output = OutputPath.Replace("\\", "/");
                string KPDetector = KeypointDetector.Name;
                dynamic module;
                using (Py.GIL())
                {
                    module = Py.Import("video_stabilizer");
                    instance = module.VideoStabilizer(Input, Output);
                }
                Task stabilize = Task.Run(() =>
                {
                    using (Py.GIL())
                    {
                        instance.stabilize(kpd_algorithm: KPDetector);
                    }
                });
                await stabilize;
                Dispatcher.Invoke(() =>
                {
                    StabilizedVideoPlayer.Video.Source = new Uri(OutputPath);
                    StabilizedVideoPlayer.RestartButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    NotStabilizedVideoPlayer.RestartButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    StabilizedVideoPlayer.PlayButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    NotStabilizedVideoPlayer.PlayButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                });
                Func<
                    string, Feature2D, MetricCalculator, List<List<Point2f>>, 
                    (double value, List<List<Point2f>> tracks)
                > computeMetric = (inputPath, detector, metric, tracks) => metric.Compute(inputPath, detector, tracks);
                string inputPathCopy = InputPath;
                string outputPathCopy = OutputPath;
                MetricCalculator metricCopy = Metric;
                Feature2D detectorCopy = KeypointDetector.Detector;

                Task computeMetrics = Task.Run(() =>
                {
                    Dispatcher.Invoke(() => { StabilizeButton.Content = "Computing selected metric for input video..."; });
                    (double metricInput, List<List<Point2f>> inputTracks) = computeMetric(inputPathCopy, detectorCopy, metricCopy, null);
                    Dispatcher.Invoke(() => { StabilizeButton.Content = "Computing selected metric for output video..."; });
                    (double metricOutput, _) = computeMetric(outputPathCopy, detectorCopy, metricCopy, inputTracks);
                    Dispatcher.Invoke(() =>
                    {
                        MetricInputValue.Text = String.Format("{0} metric value: {1}", Metric.Name, metricInput.ToString("0.000") + Metric.Unit);
                        MetricOutputValue.Text = String.Format("{0} metric value: {1}", Metric.Name, metricOutput.ToString("0.000") + Metric.Unit);
                        StabilizeButton.Content = "Stabilize video";
                        StabilizingProgressBar.IsIndeterminate = false;
                        Ready = true;
                        Stabilized = true;
                        Completed = Visibility.Visible;
                    });
                });
                await computeMetrics;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }
        private void PlotTrajectory(object sender, RoutedEventArgs e)
        {
            using (Py.GIL())
            {
                if (instance)
                {
                    dynamic mpl = Py.Import("matplotlib");
                    dynamic plt = Py.Import("matplotlib.pyplot");
                    mpl.use("TkAgg");
                    instance.stabilizer.plot_trajectory();
                    plt.show();
                    
                }
            }
        }
        private void PlotTransforms(object sender, RoutedEventArgs e)
        {
            using (Py.GIL())
            {
                if (instance)
                {
                    dynamic mpl = Py.Import("matplotlib");
                    dynamic plt = Py.Import("matplotlib.pyplot");
                    mpl.use("TkAgg");
                    instance.stabilizer.plot_transforms();
                    plt.show();
                }
            }
        }
    }
}
