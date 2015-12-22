using Common.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class AudioControl : UserControl
    {
        //public static MediaElement rootMediaElement = default(MediaElement);        
        DispatcherTimer timer = new DispatcherTimer();
        MediaElement MediaPlayer;
        public AudioControl()
        {
            this.InitializeComponent();
            Loaded += AudioControl_Loaded;
        }

        void AudioControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //DependencyObject rootGrid = VisualTreeHelper.GetChild(Window.Current.Content, 0);
                //MediaPlayer = (MediaElement)VisualTreeHelper.GetChild(rootGrid, 1) as MediaElement;

                Border border1 = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                MediaPlayer = (MediaElement)border1.FindName("MediaPlayer");


                if (MediaPlayer == null)
                {
                    Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                    MediaPlayer = (MediaElement)border.FindName("MediaPlayer");
                }
                MediaPlayer.MediaOpened += rootMediaElement_MediaOpened;
                MediaPlayer.MediaEnded += rootMediaElement_MediaEnded;
                if (MediaPlayer.CurrentState.ToString() == "Playing" && txtplaypause.Text == "play")
                {
                    changeimage();
                }
                else if (MediaPlayer.CurrentState.ToString() == "Paused" && txtplaypause.Text == "pause")
                {
                    changeimage();
                }
                if (MediaPlayer.CurrentState.ToString() == "Playing" || MediaPlayer.CurrentState.ToString() == "Paused")
                {
                    songtitle.Text = Constants.AppbarTitle;
                    audioslider1.Maximum = MediaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
                    timer.Tick += timer_Tick;
                    timer.Interval = TimeSpan.FromMilliseconds(1000);
                    timer.Start();
                }
            }
            catch(Exception ex)
            {
                string excp = ex.Message;
            }
        }

        public void changeimage()
        {
            try
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
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in changeimage event In ShowList.xaml.cs file", ex);
            }
        }

        void rootMediaElement_MediaEnded(object sender, RoutedEventArgs e)
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
                        StorageFolder file = ApplicationData.Current.LocalFolder;
                        string title1 = arr[1].ToString().Substring(arr[1].ToString().LastIndexOf("/") + 1);
                        songtitle.Text = title1;
                        //App.AudioSongTitle = title1;
                        StorageFile file1 = Task.Run(async () => await file.GetFileAsync(@"Audio\" + title1)).Result;
                        IRandomAccessStream writeStream = Task.Run(async () => await file1.OpenAsync(FileAccessMode.Read)).Result;
                        Stream outStream = Task.Run(() => writeStream.AsStreamForRead()).Result;
                        if (writeStream.Size != 0)
                        {
                            MediaPlayer.SetSource(writeStream, "");
                            MediaPlayer.Play();
                        }
                        Constants.AppbarTitle = arr[0].ToString();
                    }
                    else
                    {
                        Constants._PlayList[Constants._PlayList.ElementAt(index + 1).Key] = "Play";
                        string ph = Constants._PlayList.ElementAt(index + 1).Key.ToString();
                        arr = Constants._PlayList.ElementAt(index + 1).Key.ToString().Split('$');
                        StorageFolder file = ApplicationData.Current.LocalFolder;
                        string title1 = arr[1].ToString().Substring(arr[1].ToString().LastIndexOf("/") + 1);
                        songtitle.Text = title1;
                        //App.AudioSongTitle = title1;
                        StorageFile file1 = Task.Run(async () => await file.GetFileAsync(@"Audio\" + title1)).Result;
                        IRandomAccessStream writeStream = Task.Run(async () => await file1.OpenAsync(FileAccessMode.Read)).Result;
                        Stream outStream = Task.Run(() => writeStream.AsStreamForRead()).Result;
                        if (writeStream.Size != 0)
                        {
                            MediaPlayer.SetSource(writeStream, "");
                            MediaPlayer.Play();
                        }
                        Constants.AppbarTitle = arr[0].ToString();
                    }
                    AppSettings.LiricsLink = Constants.AppbarTitle;
                    MediaPlayer.Play();
                }
                changeimage();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Next_Click event In ShowList.xaml.cs file", ex);
            }
        }

        private void Previous_Click(object sender, TappedRoutedEventArgs e)
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
                }
                changeimage();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Previous_Click event In ShowList.xaml.cs file", ex);
            }
        }

        private void PlayPause_Click(object sender, TappedRoutedEventArgs e)
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
            if (MediaPlayer.CurrentState.ToString() == "Closed")
            {
                playpause.Source = new BitmapImage(new Uri(this.BaseUri, @"/Images/PlayerImages/play.png"));
                txtplaypause.Text = "play";
            }
        }

        private void Next_Click(object sender, TappedRoutedEventArgs e)
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
                }
                changeimage();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Next_Click event In ShowList.xaml.cs file", ex);
            }
        }

        void timer_Tick(object sender, object e)
        {
            audioslider1.Value = MediaPlayer.Position.TotalMilliseconds;
        }

        private void rootMediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (txtplaypause.Text == "play")
            {
                changeimage();
            }
            //BottomAppBar.IsOpen = true;
            audioslider1.Maximum = MediaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
            timer.Tick += timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Start();
        }
    }
}