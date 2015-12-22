using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
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
    public sealed partial class MoviesHistory : Page
    {
        bool check = false;
        DispatcherTimer timer = new DispatcherTimer();
        string link = "";
        public TextBlock songblock = null;
        public MoviesHistory()
        {           
            this.InitializeComponent();
            //songblock = this.songtitle;
            Loaded += MoviesFavorite_Loaded;
            // App.rootMediaElement.MediaOpened += rootMediaElement_MediaOpened;
        }
        private void MoviesFavorite_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerWheelChanged += CoreWindow_PointerWheelChanged;
            //AdRotatorWin8.Invalidate();
            SettingsPane.GetForCurrentView().CommandsRequested += onCommandsRequested;
            //if (App.rootMediaElement.CurrentState.ToString() == "Playing" && playpause.Style == App.Current.Resources["PlayAppBarButtonStyle"] as Style)
            //{
            //    changeimage();
            //}
            //else if (App.rootMediaElement.CurrentState.ToString() == "Paused" && playpause.Style == App.Current.Resources["PauseAppBarButtonStyle"] as Style)
            //{
            //    changeimage();
            //}
            //if (App.rootMediaElement.CurrentState.ToString() == "Playing" || App.rootMediaElement.CurrentState.ToString() == "Paused")
            //{

            //    audioslider1.Maximum = App.rootMediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
            //    timer.Tick += timer_Tick;
            //    timer.Interval = TimeSpan.FromMilliseconds(1000);
            //    timer.Start();

            //}

        }

        private void CoreWindow_PointerWheelChanged(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.PointerEventArgs args)
        {
            if (args.CurrentPoint.Properties.MouseWheelDelta == (-120))
            {
                //MouseWheel Backward scroll
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset + Window.Current.CoreWindow.Bounds.Width / 10);
            }
            if (args.CurrentPoint.Properties.MouseWheelDelta == (120))
            {
                //MouseWheel Forward scroll
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - Window.Current.CoreWindow.Bounds.Width / 10);

            }
        }

        private void onCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            AddControlvisable1.Visibility = Visibility.Collapsed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private void LayoutRoot_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //if (LayoutRoot.Opacity == 0.20000000298023224)
            //{
            if (check == false)
            {
                LayoutRoot.Opacity = 1;
                //Lyricspopup.Visibility = Visibility.Collapsed;
                check = false;
            }
            check = false;
            //}
        }

        private void BackButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (App.rootFrame.CanGoBack)
                App.rootFrame.GoBack();
        }
        public void DetailPage()
        {
            App.rootFrame.Navigate(typeof(Detail));
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }
        public void Youtube()
        {
            App.rootFrame.Navigate(typeof(Youtube));
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }
    }
}
