using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TalkTimer
{
    public sealed partial class PlayerControl : UserControl
    {
        private IPlayable Timer;
        private bool _isFinished;

        public PlayerControl()
        {
            this.InitializeComponent();
        }

        public void Setup(IPlayable timer)
        {
            Timer = timer;
            Timer.Finished += (s, e) =>
            {
                _isFinished = true;
                StopButton.Visibility = Visibility.Visible;
            };
        }

        public void ArrangeViewForOrientation(DisplayOrientations orientation)
        {
            if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
            {
                PlayerStackPanel.Orientation = Orientation.Horizontal;
            }
            else
            {
                PlayerStackPanel.Orientation = Orientation.Vertical;
            }
        }

        private void PlayButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PlayButton.Visibility = Visibility.Collapsed;
            PauseButton.Visibility = Visibility.Visible;
            ResumeButton.Visibility = Visibility.Collapsed;
            StopButton.Visibility = _isFinished ? Visibility.Visible : Visibility.Collapsed;
            Timer.Play();
        }

        private void PauseButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PlayButton.Visibility = Visibility.Collapsed;
            PauseButton.Visibility = Visibility.Collapsed;
            ResumeButton.Visibility = Visibility.Visible;
            StopButton.Visibility = Visibility.Visible;
            Timer.Pause();
        }

        private void ResumeButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PlayButton.Visibility = Visibility.Collapsed;
            PauseButton.Visibility = Visibility.Visible;
            ResumeButton.Visibility = Visibility.Collapsed;
            StopButton.Visibility = _isFinished ? Visibility.Visible : Visibility.Collapsed;
            Timer.Resume();
        }

        private void StopButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PlayButton.Visibility = Visibility.Visible;
            PauseButton.Visibility = Visibility.Collapsed;
            ResumeButton.Visibility = Visibility.Collapsed;
            StopButton.Visibility = Visibility.Collapsed;
            Timer.Stop();
            _isFinished = false;
        }

        private void AlarmOn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AlarmOn.Visibility = Visibility.Collapsed;
            AlarmOff.Visibility = Visibility.Visible;
            Timer.AlarmIsSet = false;
        }

        private void AlarmOff_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AlarmOn.Visibility = Visibility.Visible;
            AlarmOff.Visibility = Visibility.Collapsed;
            Timer.AlarmIsSet = true;
        }
    }
}
