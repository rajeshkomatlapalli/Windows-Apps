using Common.Library;
using Indian_Cinema;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BackAgentError : Page
    {
        public BackAgentError()
        {
            this.InitializeComponent();
            Loaded += BackAgentError_Loaded;
        }

        void BackAgentError_Loaded(object sender, RoutedEventArgs e)
        {
            string backgroundAgentError = AppSettings.BackgroundAgenError;

            if (!string.IsNullOrEmpty(backgroundAgentError))
            {
                NoError.Visibility = Visibility.Visible;
                NoError.Text = backgroundAgentError;
            }
            else
            {
                NoError.Visibility = Visibility.Visible;
                NoError.Text = "No errors in background agent.";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            App.rootFrame.Navigate(typeof(MainPage));
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }
    }
}
