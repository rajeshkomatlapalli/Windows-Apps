using AdRotator;
using Common.Library;
using OnlineVideos.Common;
using OnlineVideosWin8.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
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
    public sealed partial class Youtube : Page
    {
        DispatcherTimer dt = null;
        DispatcherTimer adTimer = new DispatcherTimer();
        MediaElement RootMediaElement1 = default(MediaElement);
        Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);

        public Youtube()
        {
            this.InitializeComponent();
            RootMediaElement1 = (MediaElement)border.FindName("MediaPlayer");
            AppSettings.YoutubeError = "0";
            Unloaded += Youtube_Unloaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                App.Adcollapased = true;
                Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
                adcontrol.IsAdRotatorEnabled = false;
                adcontrol.Visibility = Visibility.Collapsed;
                if (RootMediaElement1.CurrentState == MediaElementState.Playing || RootMediaElement1.CurrentState == MediaElementState.Paused)
                {
                    RootMediaElement1.Stop();
                }
                if (Constants.NavigationFromWebView == true)
                {
                    Constants.NavigationFromWebView = false;
                    if (App.rootFrame.CanGoBack)
                    {

                        App.rootFrame.GoBack();

                    }
                }
                else
                {
                    BackButton.SetValue(Canvas.TopProperty, (Window.Current.Bounds.Top + 20));
                    rate.SetValue(Canvas.LeftProperty, (Window.Current.Bounds.Right - 200));
                    rate.SetValue(Canvas.TopProperty, (Window.Current.Bounds.Top + 80));
                    if (AppSettings.Status == "Offline Available")
                    {
                        Offlinevideos();

                    }
                    else
                    {
                        youtub();
                    }

                }

            }
            catch (Exception)
            {
                tblkvideos.Visibility = Visibility.Visible;
                tblkvideos.Text = " Video link not  available";
            }
            try
            {
                if (App.RootAdForYoutube60X292 == null)
                {
                    AdRotatorWin8.Invalidate(true);
                    App.RootAdForYoutube60X292 = AdRotatorWin8;
                }
                else
                {
                    AdRotatorWin8.FetchAdSettingsFile(App.RootAdForYoutube60X292);
                    AdRotatorWin8.Invalidate();

                }
            }
            catch (Exception)
            {
            }
        }

        void dt_Tick(object sender, object e)
        {
            BackButton.Visibility = Visibility.Collapsed;
            dt.Stop();
        }

        public async void Offlinevideos()
        {
            try
            {
                StorageFolder sf = Task.Run(async () => await KnownFolders.VideosLibrary.GetFolderAsync(AppSettings.ProjectName)).Result;
                StorageFile file = default(StorageFile);
                IReadOnlyList<StorageFile> file1 = Task.Run(async () => await sf.GetFilesAsync()).Result;

                foreach (StorageFile t in file1.ToList())
                {
                    string s = t.Name;

                    if (s == AppSettings.MovieTitle + "_" + AppSettings.Title + ".mp4")
                    {
                        Uri source = new Uri(t.Path, UriKind.RelativeOrAbsolute);
                        // var read = Windows.Storage.FileIO.ReadBufferAsync(t);
                        IRandomAccessStream writeStream = await t.OpenAsync(FileAccessMode.Read);
                        Stream outStream = Task.Run(() => writeStream.AsStreamForRead()).Result;
                        if (writeStream.Size != 0)
                        {
                            videoMediaElement.SetSource(writeStream, "");

                        }
                        else
                        {
                            BackButton.Visibility = Visibility.Visible;
                            tblkvideos.Visibility = Visibility.Visible;
                            tblkvideos.Text = " Video downloading please wait!  paly video  again!";
                            if (App.rootFrame.CanGoBack)
                                App.rootFrame.GoBack();

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                BackButton.Visibility = Visibility.Visible;
                tblkvideos.Visibility = Visibility.Visible;
                tblkvideos.Text = " Video downloading please wait!  paly video try again!";
                if (App.rootFrame.CanGoBack)
                    App.rootFrame.GoBack();

            }
        }
        void Youtube_Unloaded(object sender, RoutedEventArgs e)
        {

        }
        public void youtub()
        {
            try
            {
                AppSettings.YoutubeUri = string.Empty;
                Task.Run(async () => await YouTube.GetVideoUriAsync(AppSettings.LinkUrl, AppSettings.YoutubeQuality));
                SyncAgentState.auto.WaitOne();
                if (!string.IsNullOrEmpty(AppSettings.YoutubeUri))
                {
                    Uri source = new Uri(AppSettings.YoutubeUri, UriKind.RelativeOrAbsolute);
                    videoMediaElement.Source = source;
                }
                else
                {
                    App.rootFrame.Navigate(typeof(webview));
                    Window.Current.Content = App.rootFrame;
                    Window.Current.Activate();


                };
            }
            catch (Exception)
            {
                App.rootFrame.Navigate(typeof(webview));
                Window.Current.Content = App.rootFrame;
                Window.Current.Activate();

            }
        }

        private void BackButton_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                AdVisiable();
                Constants.MediaElementPosition = new TimeSpan();

                if (App.rootFrame.CanGoBack)
                {
                    while (App.rootFrame.CurrentSourcePageType == typeof(Youtube))
                    {
                        App.rootFrame.GoBack();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        

        //private void videoMediaElement_PositionChanged_1(object sender, RoutedPropertyChangedEventArgs<TimeSpan> e)
        //{
        //    if ((TimeSpan)e.OldValue < (TimeSpan)e.NewValue)
        //        Constants.MediaElementPosition = (TimeSpan)e.NewValue;
        //}

        private void videoMediaElement_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (dt != null)
                dt.Stop();
            dt = new DispatcherTimer();
            dt.Interval = (sender as Microsoft.PlayerFramework.MediaPlayer).AutoHideInterval.Add(new TimeSpan(0, 0, 0, 0, 300));
            dt.Tick += dt_Tick;
            dt.Start();
            BackButton.Visibility = Visibility.Visible;
        }

        private void videoMediaElement_RateChanged(object sender, RateChangedRoutedEventArgs e)
        {
            if (videoMediaElement.PlaybackRate.ToString() == "1")
            {
                rate.Visibility = Visibility.Collapsed;
            }
            else
            {
                rate.Visibility = Visibility.Visible;
                if (videoMediaElement.PlaybackRate.ToString().Contains("-"))
                {
                    rate.Text = "Rewind " + videoMediaElement.PlaybackRate.ToString() + "X";
                }
                else
                {
                    rate.Text = "FastForward " + videoMediaElement.PlaybackRate.ToString() + "X";
                }
            }
        }

        private void videoMediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            try
            {
                Constants.MediaElementPosition = new TimeSpan();

                if (AppResources.advisible == false)
                {
                    if (Constants.YoutubePlayList.Count > 0)
                    {
                        AppSettings.LinkUrl = Constants.YoutubePlayList.FirstOrDefault().Key;
                        AppSettings.PlayVideoTitle = Constants.YoutubePlayList.FirstOrDefault().Value;
                        Constants.YoutubePlayList.Remove(Constants.YoutubePlayList.FirstOrDefault().Key);
                        youtub();
                    }


                    else
                    {
                        if (App.rootFrame.CanGoBack)
                            App.rootFrame.GoBack();
                        AdVisiable();
                    }

                }
                else
                {
                    if (Constants.YoutubePlayList.Count > 0)
                    {
                        AppSettings.LinkUrl = Constants.YoutubePlayList.FirstOrDefault().Key;
                        AppSettings.PlayVideoTitle = Constants.YoutubePlayList.FirstOrDefault().Value;
                        Constants.YoutubePlayList.Remove(Constants.YoutubePlayList.FirstOrDefault().Key);
                        App.rootFrame.Navigate(typeof(Advertisement));
                        Window.Current.Content = App.rootFrame;
                        Window.Current.Activate();
                        //youtub();
                    }
                    else
                    {
                        if (App.rootFrame.CanGoBack)
                            App.rootFrame.GoBack();
                        AdVisiable();
                    }
                }
            }
            catch
            {
                if (AppResources.advisible == true)
                {
                    if (App.rootFrame.CanGoBack)
                        App.rootFrame.GoBack();
                    AdVisiable();
                }
            }
        }

        private static void AdVisiable()
        {
            Border border1 = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
            AdRotatorControl adcontrol1 = (AdRotatorControl)border1.FindName("AdRotatorWin8");
            adcontrol1.IsAdRotatorEnabled = true;
            adcontrol1.IsEnabled = true;
            adcontrol1.Visibility = Visibility.Visible;
            adcontrol1.Visibility = Visibility.Visible;
        }

        private void videoMediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            App.rootFrame.Navigate(typeof(webview));
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }

        private void videoMediaElement_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Escape)
            {
                if (e.Key == VirtualKey.Escape)
                {

                    if (App.rootFrame.CanGoBack)
                        App.rootFrame.GoBack();

                }
            }

            e.Handled = true;
        }

        private void videoMediaElement_LayoutUpdated(object sender, object e)
        {
            ChangeWidth();
        }

        private void videoMediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            videoMediaElement.Position = Constants.MediaElementPosition;
        }

        private void Image_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            if (adstack.Visibility == Visibility.Visible)
            {
                adstack.Visibility = Visibility.Collapsed;
                ChangeWidth();
                adTimer.Interval = TimeSpan.FromMinutes(15);
                adTimer.Tick += adTimer_Tick;
                adTimer.Start();
            }
        }

        void adTimer_Tick(object sender, object e)
        {
            adTimer.Stop();
            adstack.Visibility = Visibility.Visible;
            ChangeWidth();
        }

        public void ChangeWidth()
        {
            try
            {

                IEnumerable<DependencyObject> cboxes = GetChildsRecursive(videoMediaElement);
                foreach (DependencyObject obj in cboxes.OfType<Grid>())
                {
                    Type type = obj.GetType();
                    if (type.Name == "Grid" && (obj as Grid).Name == "TimelineContainerGrid")
                    {
                        Grid gd = obj as Grid;
                        if (adstack.Visibility == Visibility.Visible)
                        {
                            try
                            {
                                videoMediaElement.LayoutUpdated -= videoMediaElement_LayoutUpdated;
                            }
                            catch (Exception)
                            {
                            }
                            gd.Width = Window.Current.Bounds.Width - 302;
                        }
                        else
                            gd.Width = Window.Current.Bounds.Width;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ChangeWidth Method In Youtube.xaml.cs file", ex);
            }
        }
        public static IEnumerable<DependencyObject> GetChildsRecursive(DependencyObject root)
        {
            List<DependencyObject> elts = new List<DependencyObject>();
            try
            {
                elts.Add(root);
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
                {
                    elts.AddRange(GetChildsRecursive(VisualTreeHelper.GetChild(root, i)));
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetChildsRecursive Method In Youtube.xaml.cs file", ex);
            }
            return elts;

        }

    }
}
