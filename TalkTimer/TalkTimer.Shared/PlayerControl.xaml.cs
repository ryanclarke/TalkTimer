using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public PlayerControl()
        {
            this.InitializeComponent();
        }

        private void PlayControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //if (_clock.IsInOvertime())
            //{
            //    _timer.Stop();
            //    _clock.Set(_minutes);
            //    UpdateCounterUI();
            //}
            //else if (_clock.IsAt(_minutes))
            //{
            //    StartTimer();
            //}
        }
    }
}
