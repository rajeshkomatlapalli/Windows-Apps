using Common.Library;
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
using System.Reflection;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class LyricsPopUp : UserControl
    {       
        MediaElement rootMediaElement = default(MediaElement);
        DispatcherTimer timer = new DispatcherTimer();
        bool check = false;
        public TextBlock songblock = null;

        public LyricsPopUp()
        {
            this.InitializeComponent();
            Loaded += LyricsPopUp_Loaded;
            songblock = this.songtitle;
        }

        void LyricsPopUp_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Border border1 = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                MediaPlayer = (MediaElement)border1.FindName("MediaPlayer");

                if (MediaPlayer == null)
                {
                    //Border border1 = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                    MediaPlayer = (MediaElement)border1.FindName("MediaPlayer");
                }
                MediaPlayer.Source = new Uri(Constants.Link.Split('$')[1], UriKind.Absolute);
                MediaPlayer.Play();
                
                MediaPlayer.MediaOpened += rootMediaElement_MediaOpened;
                MediaPlayer.MediaEnded += rootMediaElement_MediaEnded;
                MediaPlayer.MediaFailed += rootMediaElement_MediaFailed;
                if (AppSettings.ProjectName == "Indian Cinema Pro")
                {
                    MediaPlayer.MediaEnded += rootMediaElement_MediaEnded;
                }
                if (MediaPlayer.CurrentState.ToString() == "Playing" && txtplaypause.Text == "play")
                {
                    changeimage();
                }
                else if (MediaPlayer.CurrentState.ToString() == "Paused" && txtplaypause.Text == "pause")
                {
                    changeimage();
                }
                if (MediaPlayer.CurrentState.ToString() == "Playing")
                {
                    audioslider1.Maximum = MediaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
                    timer.Tick += timer_Tick;
                    timer.Interval = TimeSpan.FromMilliseconds(1000);
                    timer.Start();
                }
                //if(MediaPlayer.CurrentState.ToString()=="Playing")
                //{
                    //if(audioslider1.Value==0.0)
                    //{
                    //    TextBlock txterror = new TextBlock();
                    //    txterror.Text = "Audio file is not playing, Report it as Broken Link";
                    //}
                //}
            }
            catch(Exception ex)
            {
                string excp = ex.Message;
                string[] arry1 = new string[2];
                arry1[0] = ex.Source.ToString();
                arry1[1] = ex.StackTrace.ToString();
            }
        }

        void rootMediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            // NextSongPlay();
        }

        void rootMediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (txtplaypause.Text == "play")
            {
                changeimage();
            }
            audioslider1.Maximum = MediaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
            timer.Tick += timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Start();
        }
        void timer_Tick(object sender, object e)
        {
            audioslider1.Value = MediaPlayer.Position.TotalMilliseconds;
        }
        void rootMediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            NextSongPlay();
        }
        public void changeimage()
        {
            if (txtplaypause.Text == "play")
            {
                txtplaypause.Text = "pause";
                playpause.Source = new BitmapImage(new Uri(this.BaseUri, @"/Images/PlayerImages/pause.png"));
            }
            else
            {
                playpause.Source = new BitmapImage(new Uri(this.BaseUri, @"/Images/PlayerImages/play.png"));
                txtplaypause.Text = "play";
            }
        }
        private void NextSongPlay()
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
                    MediaPlayer.Source = new Uri(arr[1].ToString(), UriKind.Absolute);
                    Constants.AppbarTitle = arr[0].ToString();
                }
                else
                {
                    Constants._PlayList[Constants._PlayList.ElementAt(index + 1).Key] = "Play";
                    string ph = Constants._PlayList.ElementAt(index + 1).Key.ToString();
                    arr = Constants._PlayList.ElementAt(index + 1).Key.ToString().Split('$');
                    MediaPlayer.Source = new Uri(arr[1].ToString(), UriKind.Absolute);
                    Constants.AppbarTitle = arr[0].ToString();
                }
                AppSettings.LiricsLink = Constants.AppbarTitle;
                songtitle.Text = Constants.AppbarTitle;
                MediaPlayer.Play();
                LyricsPopup(true);
                check = true;
                int movieId = OnlineShow.GetMovieID(Constants.AppbarTitle);
                string MovieTitle = OnlineShow.GetMovieTitle(movieId.ToString());
                songMovieTitle.Text = MovieTitle + " " + "-" + " ";
            }
        }
        public void LyricsPopup(bool value)
        {
            try
            {
                int movieId = OnlineShow.GetMovieID(Constants.AppbarTitle);
                string MovieTitle = OnlineShow.GetMovieTitle(movieId.ToString());
                songMovieTitle.Text = MovieTitle + " " + "-" + " ";
                //LayoutRoot.Opacity = 0.2;
                //Popup myPopup = new Popup();
                //myPopup.IsOpen = true;
                //if (myPopup.IsOpen == true)
                //{
                //    Lyricspopup.Visibility = Visibility.Visible;
                //}
                //else
                //{
                Lyricspopup.Visibility = Visibility.Visible;
                //}
                List<ShowLinks> links = OnlineShow.GetLyrics(AppSettings.ShowID, AppSettings.LiricsLink);
                if (links.FirstOrDefault().Description != "")
                {
                    lyrics.Text = links.FirstOrDefault().Description;
                }
                else
                {
                    lyrics.Text = "No Lyrics Available";
                    lyrics.VerticalAlignment = VerticalAlignment.Center;
                }
                check = true;
                if (MediaPlayer.CurrentState.ToString() == "Playing")
                {
                    changeimage();
                }
                else if (MediaPlayer.CurrentState.ToString() == "Paused")
                {
                    changeimage();
                }
                if (MediaPlayer.CurrentState.ToString() == "Playing")
                {
                    audioslider1.Maximum = MediaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
                    timer.Tick += timer_Tick;
                    timer.Interval = TimeSpan.FromMilliseconds(1000);
                    timer.Start();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void imgclose_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Popup myPopup = new Popup();
            myPopup.IsOpen = false;            
            Lyricspopup.Visibility = Visibility.Collapsed;
            if (LayoutRoot.Opacity == 0.20000000298023224)
            {
                LayoutRoot.Opacity = 1;
                Lyricspopup.Visibility = Visibility.Collapsed;
            }
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
                    MediaPlayer.Source = new Uri(arr[1].ToString(), UriKind.Absolute);
                    Constants.AppbarTitle = arr[0].ToString();
                }
                else
                {
                    Constants._PlayList[Constants._PlayList.ElementAt(index - 1).Key] = "Play";
                    arr = Constants._PlayList.ElementAt(index - 1).Key.ToString().Split('$');
                    MediaPlayer.Source = new Uri(arr[1].ToString(), UriKind.Absolute);
                    Constants.AppbarTitle = arr[0].ToString();
                }
                AppSettings.LiricsLink = Constants.AppbarTitle;
                songtitle.Text = Constants.AppbarTitle;
                MediaPlayer.Play();
                check = true;
                LyricsPopup(true);
            }
        }

        private void PlayPause_Click(object sender, TappedRoutedEventArgs e)
        {
            if (MediaPlayer.Source != null)
            {
                if (MediaPlayer.CurrentState.ToString() == "Playing")
                {
                    timer.Stop();
                    MediaPlayer.Pause();
                    changeimage();
                }
                else
                {
                    timer.Start();
                    MediaPlayer.Play();
                    changeimage();
                }
            }
        }

        private void Next_Click(object sender, TappedRoutedEventArgs e)
        {
            NextSongPlay();
        }
    }
}