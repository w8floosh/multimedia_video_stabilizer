using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Multimedia.CustomControls
{
    /// <summary>
    /// Logica di interazione per VideoPlayer.xaml
    /// </summary>

    public partial class VideoPlayer : UserControl
    {
        private readonly TimeSpan Frame = new(333333);
        public VideoPlayer()
        {

            InitializeComponent();
            DataContext = this;
            Video.ScrubbingEnabled = true;
        }

        public void PlayOrPause(object sender, RoutedEventArgs e)
        {
            Video.LoadedBehavior =
                Video.LoadedBehavior == MediaState.Play ?
                MediaState.Pause :
                MediaState.Play;
            Debug.WriteLine(Video.Position);

        }
        public void GoToFirstFrame(object sender, RoutedEventArgs e)
        {
            Video.LoadedBehavior = MediaState.Pause;
            Video.Position = new TimeSpan(0);
            Debug.WriteLine(Video.Position);

        }
        public void GoToLastFrame(object sender, RoutedEventArgs e)
        {
            Video.LoadedBehavior = MediaState.Pause;
            Video.Position = Video.NaturalDuration.TimeSpan.Subtract(Frame);
            Debug.WriteLine(Video.Position);

        }
        public void GoToNextFrame(object sender, RoutedEventArgs e)
        {
            // update to the next frame which is 33.3333 milliseconds away with 30fps;
            // 1 millisecond = 10000 ticks => 33.3333 milliseconds = 33333 ticks
            if (Video.Position.Ticks <= Video.NaturalDuration.TimeSpan.Subtract(Frame).Ticks)
            {
                Video.LoadedBehavior = MediaState.Pause;
                Video.Position = Video.Position.Add(Frame);
            }
            Debug.WriteLine(Video.Position);
        }
        public void GoToPrevFrame(object sender, RoutedEventArgs e)
        {
            // update to the next frame which is 33.3333 milliseconds away with 30fps;
            // 1 millisecond = 10000 ticks => 33.3333 milliseconds = 333333 ticks
            if (Video.Position.Ticks >= 333333)
            {
                Video.LoadedBehavior = MediaState.Pause;
                Video.Position = Video.Position.Subtract(Frame);
            }
            Debug.WriteLine(Video.Position);
        }
    }
}
