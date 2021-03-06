﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
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

        public bool AlarmIsSet { get; set; }

        public event EventHandler Finished;

        public ColorTimerControl()
        {
            this.InitializeComponent();

            _clock = new Clock(_minutes);
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            _timer.Tick += timer_Tick;

            LargeNumberGroup.RenderTransform = new TranslateTransform();

            AlarmIsSet = true;
            PlayerControls.Setup(this);
            Finished += (s, e) => { };

            DisplayInformation.GetForCurrentView().OrientationChanged += (s, e) => ArrangeViewForOrientation();
            
            ArrangeViewForOrientation();

            UpdateCounterUI();
        }

        private void ArrangeViewForOrientation()
        {
            var orientation = DisplayInformation.GetForCurrentView().CurrentOrientation;
            PlayerControls.ArrangeViewForOrientation(orientation);
            if (orientation == DisplayOrientations.Portrait || orientation == DisplayOrientations.PortraitFlipped)
            {
                PlayerControlViewbox.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                PlayerControlViewbox.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Bottom;
            }
            else
            {
                PlayerControlViewbox.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Right;
                PlayerControlViewbox.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            }
        }

        void timer_Tick(object sender, object e)
        {
            _clock.UpdateClock();
            UpdateCounterUI();
            if (_clock.IsInOvertime()) Finished(this, new EventArgs());
        }

        private void UpdateCounterUI()
        {
            var clockColor = _timer.IsEnabled ? Colors.White : new Color { A = 102, R = 255, G = 255, B = 255 };
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
                if (AlarmIsSet) Alarm.Play();
            }
            else if (_clock.IsInOvertime())
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
            LargeNumberUpArrow.Visibility = Visibility.Collapsed;
            LargeNumberDownArrow.Visibility = Visibility.Collapsed;
            _timer.Start();
            _clock.Set(_minutes);
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
            _clock.Resume();
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
    }

    public interface IPlayable
    {
        bool AlarmIsSet { get; set; }
        void Play();
        void Pause();
        void Resume();
        void Stop();
        event EventHandler Finished;
    } 
}
