using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
    public sealed partial class AboutControl : UserControl
    {
        public AboutControl()
        {
            this.InitializeComponent();

            VersionNumber.Text = CustomAttributeExtensions.GetCustomAttribute<AssemblyFileVersionAttribute>(typeof(App).GetTypeInfo().Assembly).Version;
        }

        private void Twitter_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("https://twitter.com/RyanSClarke", UriKind.Absolute));
        }

        private void Email_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("mailto:ryan+talktimer@ryanclarke.net?Subject=TalkTimer", UriKind.Absolute));
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
}
