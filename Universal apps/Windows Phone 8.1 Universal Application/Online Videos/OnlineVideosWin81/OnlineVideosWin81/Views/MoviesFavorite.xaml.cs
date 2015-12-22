using AdRotator;
using Common.Library;
using OnlineVideos.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
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
    public sealed partial class MoviesFavorite : Page
    {
        bool check = false;
        DispatcherTimer timer = new DispatcherTimer();
        public DispatcherTimer appbartimer;
        public TextBlock songblock = null;

        public MoviesFavorite()
        {
            this.InitializeComponent();
            //songblock = this.songtitle;
            Loaded += MoviesFavorite_Loaded;
            Unloaded += MoviesFavorite_Unloaded;
            //App.rootMediaElement.MediaOpened += rootMediaElement_MediaOpened;
        }

        void MoviesFavorite_Unloaded(object sender, RoutedEventArgs e)
        {
            App.AdCollapasedPageNavigation = true;
            if (Constants.CloseLyricspopup.IsOpen == true)
                Constants.CloseLyricspopup.IsOpen = false;
        }

        void MoviesFavorite_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerWheelChanged += CoreWindow_PointerWheelChanged;
            App.AdCollapasedPageNavigation = false;
            //AdRotatorWin8.Invalidate();
            SettingsPane.GetForCurrentView().CommandsRequested += onCommandsRequested;
            if (Constants.FavouritesMessageBox == false && AppSettings.SkyDriveLogin == false)
            {
                Constants.FavouritesMessageBox = true;
                MessageDialog msg = new MessageDialog("Please Sign in,in order to Sync preferences across app update");
                bool? result = null;
                msg.Commands.Add(new UICommand("OK", new UICommandInvokedHandler((cmd) => result = true)));
                msg.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler((cmd) => result = false)));
                Task.Run(async () => await msg.ShowAsync());
                if (result == true)
                {
                    //SyncStories.SyncButton ss = new SyncStories.SyncButton();
                    //Task.Run(async () => await ss.SetNameField(true));
                }
            }
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

        private void BottomAppBar_Opened(object sender, object e)
        {
            //Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
            //AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
            //adcontrol.IsAdRotatorEnabled = false;
            //adcontrol.Visibility = Visibility.Collapsed;
            if (Constants.Favoriteappbarvisible == false)
            {
                BottomAppBar.IsOpen = false;
            }
            else
            {

                Constants.Favoriteappbarvisible = false;
            }
        }

        private void BottomAppBar_Closed(object sender, object e)
        {
            if (!App.AdCollapasedPageNavigation)
            {
                //Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                //AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
                //adcontrol.IsAdRotatorEnabled = false;
                //adcontrol.Visibility = Visibility.Visible;
            }
        }

        private void Fav_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Constants.selecteditem != null)
                {
                    FavoritesManager.RemoveFavoriteSongs(Constants.selecteditem.LinkType);
                    //videofavorite.LoadFavoriteVideos();
                    //AudioFavorite.GetPageDataInBackground();

                }
                else
                {
                    FavoritesManager.RemoveFavoriteMovies();
                    //Moviefavorite.GetPageDataInBackground();
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Fav_Click  event In MoviesFavorite", ex);
            }
        }

        private void BackButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (App.rootFrame.CanGoBack)
                App.rootFrame.GoBack();
        }

        public void Youtube()
        {
            App.rootFrame.Navigate(typeof(Youtube));
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }
        public void appbar(bool value)
        {

            BottomAppBar.IsOpen = value;

            if (value == true)
            {
                appbartimer = new DispatcherTimer();
                appbartimer.Interval = TimeSpan.FromSeconds(6);
                appbartimer.Tick += appbartimer_Tick;
                appbartimer.Start();
            }

        }

        void appbartimer_Tick(object sender, object e)
        {
            BottomAppBar.IsOpen = false;
            appbartimer.Stop();

        }

        private void Grid_Tapped_1(object sender, TappedRoutedEventArgs e)
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        public void DetailPage()
        {
            App.rootFrame.Navigate(typeof(Detail));
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }
    }
}
