using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.Views;
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

namespace Indian_Cinema.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShowLyrics : Page
    {
        public DispatcherTimer appbartimer;
        DispatcherTimer timer = new DispatcherTimer();
        public TextBlock songblock = null;
        public static ShowLyrics current;
        public ShowLyrics()
        {
            this.InitializeComponent();
            current = this;
            // progressbar.IsActive = true;
            songblock = this.songtitle;
            Loaded += ShowLyrics_Loaded;
            App.rootMediaElement.MediaOpened += rootMediaElement_MediaOpened;
        }

        private void rootMediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (playpause.Style == App.Current.Resources["PlayAppBarButtonStyle"] as Style)
            {
                changeimage();
            }

            audioslider1.Maximum = App.rootMediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
            timer.Tick += timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Start();
        }

        void ShowLyrics_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Window.Current.CoreWindow.PointerWheelChanged += CoreWindow_PointerWheelChanged;
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

                    audioslider1.Maximum = App.rootMediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
                    timer.Tick += timer_Tick;
                    timer.Interval = TimeSpan.FromMilliseconds(1000);
                    timer.Start();

                }
                //  progressbar.IsActive = false;
                List<ShowLinks> links = OnlineShow.GetLyrics(AppSettings.ShowID, AppSettings.LiricsLink);
                if (links.FirstOrDefault().Description != "")

                    lyrics.Text = links.FirstOrDefault().Description;
                else
                {
                    lyrics.Text = "No Lyrics Available";
                    lyrics.VerticalAlignment = VerticalAlignment.Center;
                }
                songtitle.Text = Constants.AppbarTitle;
            }
            catch (Exception ex)
            {
                songtitle.Text = Constants.AppbarTitle;
                lyrics.Text = "No Lyrics Available";
                lyrics.VerticalAlignment = VerticalAlignment.Center;
            }
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

        public void changeimage()
        {
            if (playpause.Style == App.Current.Resources["PauseAppBarButtonStyle"] as Style)
            {
                playpause.Style = App.Current.Resources["PlayAppBarButtonStyle"] as Style;
            }
            else
            {
                playpause.Style = App.Current.Resources["PauseAppBarButtonStyle"] as Style;
            }

        }

        void timer_Tick(object sender, object e)
        {

            audioslider1.Value = App.rootMediaElement.Position.TotalMilliseconds;

        }

        private void previous_click(object sender, RoutedEventArgs e)
        {
            try
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
                        List<ShowLinks> links = OnlineShow.GetLyrics(AppSettings.ShowID, AppSettings.LiricsLink);
                        if (links.FirstOrDefault().Description != "")

                            lyrics.Text = links.FirstOrDefault().Description;
                        else
                        {
                            lyrics.Text = "No Lyrics Available";
                            lyrics.VerticalAlignment = VerticalAlignment.Center;
                        }
                    }
                    else
                    {
                        Constants._PlayList[Constants._PlayList.ElementAt(index - 1).Key] = "Play";
                        arr = Constants._PlayList.ElementAt(index - 1).Key.ToString().Split('$');
                        App.rootMediaElement.Source = new Uri(arr[1].ToString(), UriKind.Absolute);
                        Constants.AppbarTitle = arr[0].ToString();
                        List<ShowLinks> links = OnlineShow.GetLyrics(AppSettings.ShowID, arr[0].ToString());
                        if (links.FirstOrDefault().Description != "")

                            lyrics.Text = links.FirstOrDefault().Description;
                        else
                        {
                            lyrics.Text = "No Lyrics Available";
                            lyrics.VerticalAlignment = VerticalAlignment.Center;
                        }
                    }
                    songtitle.Text = Constants.AppbarTitle;
                    App.rootMediaElement.Play();
                }
            }
            catch (Exception ex)
            {
                songtitle.Text = Constants.AppbarTitle;
                lyrics.Text = "No Lyrics Available";
                lyrics.VerticalAlignment = VerticalAlignment.Center;
            }
        }

        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            try
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
                        List<ShowLinks> links = OnlineShow.GetLyrics(AppSettings.ShowID, arr[0].ToString());
                        if (links.FirstOrDefault().Description != "")

                            lyrics.Text = links.FirstOrDefault().Description;
                        else
                        {
                            lyrics.Text = "No Lyrics Available";
                            lyrics.VerticalAlignment = VerticalAlignment.Center;
                        }
                    }
                    else
                    {
                        Constants._PlayList[Constants._PlayList.ElementAt(index + 1).Key] = "Play";
                        string ph = Constants._PlayList.ElementAt(index + 1).Key.ToString();
                        arr = Constants._PlayList.ElementAt(index + 1).Key.ToString().Split('$');
                        App.rootMediaElement.Source = new Uri(arr[1].ToString(), UriKind.Absolute);
                        Constants.AppbarTitle = arr[0].ToString();
                        List<ShowLinks> links = OnlineShow.GetLyrics(AppSettings.ShowID, arr[0].ToString());
                        if (links.FirstOrDefault().Description != "")

                            lyrics.Text = links.FirstOrDefault().Description;
                        else
                        {
                            lyrics.Text = "No Lyrics Available";
                            lyrics.VerticalAlignment = VerticalAlignment.Center;
                        }

                    }
                    songtitle.Text = Constants.AppbarTitle;
                    App.rootMediaElement.Play();
                }
            }
            catch (Exception ex)
            {
                songtitle.Text = Constants.AppbarTitle;
                lyrics.Text = "No Lyrics Available";
                lyrics.VerticalAlignment = VerticalAlignment.Center;
            }
        }

        public void closepopup()
        {
            LayoutRoot.Visibility = Visibility.Collapsed;
        }

        private void imgclose_tapped(object sender, TappedRoutedEventArgs e)
        {
            closepopup();
            Detail dt = Detail.current;
            //dt.changeopacity();
        }
    }
}
