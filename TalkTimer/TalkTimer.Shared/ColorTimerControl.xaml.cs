using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class ColorTimerControl : UserControl
    {
        private const int _minutes = 10;
        private Clock _clock;
        private DispatcherTimer _timer;

        public ColorTimerControl()
        {
            this.InitializeComponent();

            _clock = new Clock(_minutes);
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Tick += timer_Tick;

            UpdateCounterUI();
        }

        private void ColorBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_clock.IsInOvertime())
            {
                _timer.Stop();
                _clock.Set(_minutes);
                UpdateCounterUI();
            }
            else if (_clock.IsAt(_minutes))
            {
                StartTimer();
            }
        }

        private void StartTimer()
        {
            _clock.Set(_minutes);

            _timer.Start();

            UpdateCounterUI();
        }

        void timer_Tick(object sender, object e)
        {
            _clock.ElapseSecond();
            UpdateCounterUI();
        }

        private void UpdateCounterUI()
        {
            var clockColor = _timer.IsEnabled ? Colors.White : Colors.DarkSlateGray;
            LargeNumber.Foreground = new SolidColorBrush(clockColor);
            SmallNumber.Foreground = new SolidColorBrush(clockColor);

            if (_clock.IsAt(_minutes))
            {
                var backgroundColor = _timer.IsEnabled ? Colors.Green : Colors.Black;
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
    }
}
