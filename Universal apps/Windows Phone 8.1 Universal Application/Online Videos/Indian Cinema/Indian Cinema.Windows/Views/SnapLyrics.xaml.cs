using Common.Library;
using Microsoft.PlayerFramework;
using OnlineVideos;
using OnlineVideos.Data;
using OnlineVideos.Entities;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Indian_Cinema.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SnapLyrics : Page
    {
        bool check = false;
        DispatcherTimer timer = new DispatcherTimer();
        public static SnapLyrics current;
        public TextBlock songblock = null;
        public SnapLyrics()
        {
            this.InitializeComponent();
            App.rootMediaElement.MediaOpened += rootMediaElement_MediaOpened;
            Loaded += SnapLyrics_Loaded;
            current = this;
            songblock = this.songtitle;
        }

        private void rootMediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (playpause.Style == App.Current.Resources["PlayAppBarButtonStyle"] as Style)
            {
                changeimage();
            }
            //BottomAppBar.IsOpen = true;
            Autoslider1.Maximum = App.rootMediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
            timer.Tick += timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Start();
        }

        void SnapLyrics_Loaded(object sender, RoutedEventArgs e)
        {
            //SnapViewAdRotatorWin8.Invalidate();
            Window.Current.CoreWindow.PointerWheelChanged += CoreWindow_PointerWheelChanged;
            Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
            App.rootMediaElement = (MediaElement)border.FindName("MediaPlayer");
            if (App.rootMediaElement.CurrentState.ToString() == "Playing" && playpause.Style == App.Current.Resources["PlayAppBarButtonStyle"] as Style)
            {
                changeimage();
            }
            else if (App.rootMediaElement.CurrentState.ToString() == "Paused" && playpause.Style == App.Current.Resources["PauseAppBarButtonStyle"] as Style)
            {
                changeimage();
            }
            if (App.rootMediaElement.CurrentState.ToString() == "Playing" || App.rootMediaElement.CurrentState.ToString() == "Paused")
            {
                Autoslider1.Maximum = App.rootMediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
                timer.Tick += timer_Tick;
                timer.Interval = TimeSpan.FromMilliseconds(1000);
                timer.Start();
            }

            LyricsPopup(true);

        }

        private void CoreWindow_PointerWheelChanged(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.PointerEventArgs args)
        {
            if (args.CurrentPoint.Properties.MouseWheelDelta == (-120))
            {
                //MouseWheel Backward scroll
                lyricsscroll.ScrollToHorizontalOffset(lyricsscroll.HorizontalOffset + Window.Current.CoreWindow.Bounds.Width / 10);
            }
            if (args.CurrentPoint.Properties.MouseWheelDelta == (120))
            {
                //MouseWheel Forward scroll
                lyricsscroll.ScrollToHorizontalOffset(lyricsscroll.HorizontalOffset - Window.Current.CoreWindow.Bounds.Width / 10);

            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private void BackButton_click_1(object sender, RoutedEventArgs e)
        {
            //var pag = new Detail();
            //Window.Current.Content = pag;
            //Window.Current.Activate();

            if (App.rootFrame.CanGoBack)
                App.rootFrame.GoBack();
        }

        private void Previous_Click(object sender, TappedRoutedEventArgs e)
        {
            if (Constants._PlayList != null)
            {
                string[] arr = new string[2];
                int index = 0;

                foreach (KeyValuePair<string, string> dic in Constants._PlayList)
                {
                    if (dic.Value == "Play")
                    {
                        break;
                    }
                    else
                    {
                        index++;
                    }
                }
                Constants._PlayList[Constants._PlayList.ElementAt(index).Key] = "Stop";
                if (index == 0)
                {
                    index = Constants._PlayList.Count - 1;
                    Constants._PlayList[Constants._PlayList.ElementAt(index).Key] = "Play";
                    arr = Constants._PlayList.ElementAt(index).Key.ToString().Split('$');
                    App.rootMediaElement.Source = new Uri(arr[1].ToString(), UriKind.Absolute);
                    Constants.AppbarTitle = arr[0].ToString();
                }
                else
                {
                    Constants._PlayList[Constants._PlayList.ElementAt(index - 1).Key] = "Play";
                    arr = Constants._PlayList.ElementAt(index - 1).Key.ToString().Split('$');
                    App.rootMediaElement.Source = new Uri(arr[1].ToString(), UriKind.Absolute);
                    Constants.AppbarTitle = arr[0].ToString();
                }
                AppSettings.LiricsLink = Constants.AppbarTitle;
                songtitle.Text = Constants.AppbarTitle;
                App.rootMediaElement.Play();
                check = true;
                LyricsPopup(true);
            }
        }

        public void changeimage()
        {
            //if (playpause.Style == App.Current.Resources["PauseAppBarButtonStyle"] as Style)
            //{
            //    playpause.Style = App.Current.Resources["PlayAppBarButtonStyle"] as Style;
            //   txtplaypause.Text = "play";
            //}
            //else
            //{
            //    txtplaypause.Text = "pause";
            //    playpause.Style = App.Current.Resources["PauseAppBarButtonStyle"] as Style;
            //}
            if (playpause.Label == "play")
            {
                playpause.Label = "pause";
               // playpause.Source = new BitmapImage(new Uri(this.BaseUri, @"/Images/PlayerImages/pause.png"));
            }
            else
            {
                //playpause.Source = new BitmapImage(new Uri(this.BaseUri, @"/Images/PlayerImages/play.png"));
                playpause.Label = "play";
            }
        }

        private void Playpause_click(object sender, TappedRoutedEventArgs e)
        {
            if (App.rootMediaElement.Source != null)
            {
                if (App.rootMediaElement.CurrentState.ToString() == "Playing")
                {
                    timer.Stop();
                    App.rootMediaElement.Pause();
                    changeimage();
                }
                else
                {
                    timer.Start();
                    App.rootMediaElement.Play();
                    changeimage();
                }
            }
        }

        public void LyricsPopup(bool value)
        {
            try
            {
                List<ShowLinks> links = OnlineShow.GetLyrics(AppSettings.ShowID, AppSettings.LiricsLink);
                if (links.FirstOrDefault().Description != "")
                {
                    lyrics.Text = links.FirstOrDefault().Description;
                    songtitle.Text = Constants.AppbarTitle;
                }
                else
                {
                    lyrics.Text = "No Lyrics Available";
                    lyrics.VerticalAlignment = VerticalAlignment.Center;
                }
                check = true;
                changeimage();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LyricsPopup  Method In SnapLyrics", ex);
            }

        }
        void timer_Tick(object sender, object e)
        {
            Autoslider1.Value = App.rootMediaElement.Position.TotalMilliseconds;
        }

        private void Next_click(object sender, TappedRoutedEventArgs e)
        {
            if (Constants._PlayList != null)
            {
                string[] arr = new string[2];
                int index = 0;
                foreach (KeyValuePair<string, string> dic in Constants._PlayList)
                {
                    if (dic.Value == "Play")
                    {
                        break;
                    }
                    else
                    {
                        index++;
                    }
                }
                Constants._PlayList[Constants._PlayList.ElementAt(index).Key] = "Stop";
                int k = Constants._PlayList.Count;
                if (Constants._PlayList.Count - 1 == index)
                {
                    index = 0;
                    Constants._PlayList[Constants._PlayList.ElementAt(index).Key] = "Play";
                    arr = Constants._PlayList.ElementAt(index).Key.ToString().Split('$');
                    App.rootMediaElement.Source = new Uri(arr[1].ToString(), UriKind.Absolute);
                    Constants.AppbarTitle = arr[0].ToString();
                }
                else
                {
                    Constants._PlayList[Constants._PlayList.ElementAt(index + 1).Key] = "Play";
                    string ph = Constants._PlayList.ElementAt(index + 1).Key.ToString();
                    arr = Constants._PlayList.ElementAt(index + 1).Key.ToString().Split('$');
                    App.rootMediaElement.Source = new Uri(arr[1].ToString(), UriKind.Absolute);
                    Constants.AppbarTitle = arr[0].ToString();
                }
                AppSettings.LiricsLink = Constants.AppbarTitle;
                songtitle.Text = Constants.AppbarTitle;
                App.rootMediaElement.Play();
                LyricsPopup(true);
                check = true;
            }
        }
    }
}
