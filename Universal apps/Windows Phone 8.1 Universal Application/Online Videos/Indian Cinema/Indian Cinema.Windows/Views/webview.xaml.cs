using Common.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class webview : Page
    {
        public string link = string.Empty;
        public string movieid = string.Empty;
        public webview()
        {
            this.InitializeComponent();
            yutubeMedia.Source = new Uri("http://www.youtube.com/embed/" + AppSettings.LinkUrl);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private void Youtube_Loaded_1(object sender, RoutedEventArgs e)
        {
            Uri source = new Uri("http://www.youtube.com/embed/" + AppSettings.LinkUrl);
            yutubeMedia.Source = source;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.rootFrame.CanGoBack)
            {
                App.rootFrame.GoBack();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Constants.NavigationFromWebView = true;
        }

        private void yutubeMedia_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                if (App.rootFrame.CanGoBack)
                {
                    App.rootFrame.GoBack();
                }
            }
        }
    }
}