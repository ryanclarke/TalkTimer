using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Input;
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
    public sealed partial class ColorTimerControl : UserControl, IPlayable
    {
        private int _minutes = 10;
        private Clock _clock;
        private DispatcherTimer _timer;

        public event EventHandler Finished;

        public ColorTimerControl()
        {
            this.InitializeComponent();

            VersionNumber.Text = CustomAttributeExtensions.GetCustomAttribute<AssemblyFileVersionAttribute>(typeof(App).GetTypeInfo().Assembly).Version;

            _clock = new Clock(_minutes);
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Tick += timer_Tick;

            LargeNumberGroup.RenderTransform = new TranslateTransform();

            PlayerControls.Setup(this);
            Finished += (s, e) => { };

            UpdateCounterUI();
        }

        void timer_Tick(object sender, object e)
        {
            _clock.ElapseSecond();
            UpdateCounterUI();
            if (_clock.IsInOvertime()) Finished(this, new EventArgs());
        }

        private void UpdateCounterUI()
        {
            var clockColor = _timer.IsEnabled ? Colors.White : new Color { A = 68, R = 255, G = 255, B = 255 };
            LargeNumber.Foreground = new SolidColorBrush(clockColor);
            SmallNumber.Foreground = new SolidColorBrush(clockColor);

            if (_clock.IsAt(_minutes))
            {
                var backgroundColor = _timer.IsEnabled ? Colors.DeepSkyBlue : Colors.Black;
                ColorBox.Background = new SolidColorBrush(backgroundColor);
            }
            else if (_clock.JustPassed(2))
            {
                ColorBox.Background = new SolidColorBrush(Colors.Purple);
            }
            else if (_clock.JustPassed(1))
            {
                ColorBox.Background = new SolidColorBrush(Colors.Red);
            }
            else if (_clock.IsAt(0))
            {
                ColorBox.Background = new SolidColorBrush(Colors.Black);
            }

            if (_clock.IsLastMinute())
            {
                LargeNumber.Text = _clock.Seconds;
                SmallNumber.Text = "";
            }
            else if (_clock.IsInOvertime())
            {
                LargeNumber.Text = _clock.Minutes;
                SmallNumber.Text = "";
            }
            else
            {
                LargeNumber.Text = _clock.Minutes;
                SmallNumber.Text = _clock.Seconds;
            }
        }

        private void LargeNumberGroup_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (_timer.IsEnabled) return;

            var x = e.Cumulative.Translation.X;
            var y = e.Cumulative.Translation.Y;

            if (Math.Abs(y) > Math.Abs(x))
            {
                if (Math.Abs(y) > Window.Current.Bounds.Height / 10)
                {
                    e.Complete();
                    if (y < 0)
                    {
                        _minutes = _minutes + 1;
                    }
                    if (y > 0)
                    {
                        _minutes = Math.Max(0, _minutes - 1);
                    }
                    _clock.Set(_minutes);
                    UpdateCounterUI();
                }
                else
                {
                    ((TranslateTransform)(sender as UIElement).RenderTransform).Y = y;
                }
            }
        }

        private void LargeNumberGroup_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var element = sender as UIElement;
            ((TranslateTransform)element.RenderTransform).Y = element.RenderTransformOrigin.Y;
        }

        public void Play()
        {
            _clock.Set(_minutes);
            _timer.Start();
            LargeNumberUpArrow.Visibility = Visibility.Collapsed;
            LargeNumberDownArrow.Visibility = Visibility.Collapsed;
            UpdateCounterUI();
        }

        public void Pause()
        {
            _timer.Stop();
            UpdateCounterUI();
        }

        public void Resume()
        {
            _timer.Start();
            UpdateCounterUI();
        }

        public void Stop()
        {
            _timer.Stop();
            _clock.Set(_minutes);
            LargeNumberUpArrow.Visibility = Visibility.Visible;
            LargeNumberDownArrow.Visibility = Visibility.Visible;
            UpdateCounterUI();
        }

        private void Twitter_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("https://twitter.com/RyanSClarke", UriKind.Absolute));
        }

        private void Email_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("mailto://ryan+crosswind@ryanclarke.net?Crosswind", UriKind.Absolute));
        }

        private void iIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (AboutDetail.Visibility == Visibility.Visible)
            {
                AboutDetail.Visibility = Visibility.Collapsed;
                AboutPanel.Background = AboutSubtleBackground;
            }
            else
            {
                AboutDetail.Visibility = Visibility.Visible;
                AboutPanel.Background = AboutBackground;
            }
        }
    }

    public interface IPlayable
    {
        void Play();
        void Pause();
        void Resume();
        void Stop();
        event EventHandler Finished;
    } 
}
