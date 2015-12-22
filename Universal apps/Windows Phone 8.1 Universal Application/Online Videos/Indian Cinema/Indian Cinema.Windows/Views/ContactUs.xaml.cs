//using AdRotator;
using Common.Library;
using Indian_Cinema;
using OnlineVideos.Data;
using OnlineVideosWin81.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
    public sealed partial class ContactUs : Page
    {
        public static string Navigate = string.Empty;
        public ContactUs()
        {
            this.InitializeComponent();
            Loaded += ContactUs_Loaded;
        }

        void ContactUs_Loaded(object sender, RoutedEventArgs e)
        {
            Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
            //AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
            //adcontrol.IsAdRotatorEnabled = false;
            //adcontrol.Visibility = Visibility.Collapsed;
            App.Adcollapased = true;
            if (Navigate == "context")
            {
                string MovieTitle = OnlineShow.GetMovieTitle(Constants.selecteditem.ShowID.ToString());
                var asmName = this.GetType().AssemblyQualifiedName;
                var versionExpression = new System.Text.RegularExpressions.Regex("Version=(?<version>[0-9.]*)");
                var m = versionExpression.Match(asmName);
                string version = String.Empty;
                if (m.Success)
                {
                    version = m.Groups["version"].Value;
                }

                ((TextBox)((Feedback)contactuscontrol.FindName("FeedBackControl")).FindName("txtboxFeedback")).Text = "Movie Name : " + MovieTitle + "\n" + Constants.selecteditem.Title + "\n" + "http://www.youtube.com/watch?v=" + Constants.selecteditem.LinkUrl + "\n" + "Application Name: " + AppSettings.ProjectName + "\n" + "CultureInfo: " + CultureInfo.CurrentCulture.Name + "\n" + "Time : " + System.DateTimeOffset.UtcNow + "\n" + "Standard Time :" + System.TimeZoneInfo.Local.StandardName + "\n" + "Version: " + version + "\n";
                ((WebView)contactuscontrol.FindName("wb")).Visibility = Visibility.Collapsed;
                ((Control)contactuscontrol.FindName("FeedBackControl")).Visibility = Visibility.Visible;
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                Navigate = e.Parameter.ToString();
            }
        }
        public void MainPage()
        {
            App.rootFrame.Navigate(typeof(MainPage));
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }
    }
}
