using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Reflection;
using Common.Library;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class RingtoneEditor : UserControl
    {
        #region GlobalDeclaration
        BackgroundHelper bwHelper = new BackgroundHelper();
        static Stream DownloadStream;
       // SaveRingtoneTask saveRingtoneChooser;
        bool IsPreviewMode = false;
        Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
        //MediaElement RootMediaElement1 = default(MediaElement);

        #endregion
        public RingtoneEditor()
        {
            this.InitializeComponent();
           // RootMediaElement1 = (MediaElement)border.FindName("MediaPlayer");

            Loaded += RingtoneEditor_Loaded;
        }

        void RingtoneEditor_Loaded(object sender, object e)
        {
            if (RootMediaElement.CurrentState == MediaElementState.Playing)
            {
                Constants.IsAudioPlaying = true;
                RootMediaElement.Pause();
            }
            progressstack.Visibility = Visibility.Visible;
            progressbar.IsActive = true;
            tbbegin.Text = "";
            tbend.Text = "";
            ControlVisibility(Visibility.Collapsed);
            txterror.Visibility = Visibility.Collapsed;
            RootMediaElement.Source = null;
            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += bg_DoWork;
            bg.RunWorkerCompleted += bg_RunWorkerCompleted;
            bg.RunWorkerAsync();
        }

        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressstack.Visibility = Visibility.Collapsed;
            progressbar.IsActive = false;
            if (DownloadStream != null)
            {
                songReady();
                AppSettings.IsRingtone = true;
                RootMediaElement.Source = new Uri(Constants.selecteditem.LinkUrl, UriKind.Absolute);
                playimage.Source = LoadImage(Constants.PlayerPauseImage) as ImageSource;
            }
        }

        void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    HttpClient http = new HttpClient();
                    HttpResponseMessage response = Task.Run(async () => await http.GetAsync(Constants.selecteditem.LinkUrl)).Result;
                    if (response != null)
                    {
                        DownloadStream = Task.Run(async () => await response.Content.ReadAsStreamAsync()).Result;
                    }
                    else
                    {
                        ErrorMessage(Constants.NetworkErrorMessageForRingTone, Visibility.Visible);
                    }

                }
                else
                {
                    //ApplicationBar.IsVisible = false;
                    ErrorMessage(Constants.NetworkErrorMessageForRingTone, Visibility.Visible);
                }
            }
            catch (Exception ex)
            {
            }
        }

        object LoadImage(string path)
        {
            object objImage = null;
            BitmapImage bi = new BitmapImage();
            bi.UriSource = new Uri("ms-appx://" + path, UriKind.Absolute);
            objImage = bi;
            return objImage;
        }

        void TrimCompleted()
        {
            float fileSize = Storage.GetFileSize(ResourceHelper.RingToneFileName(Constants.selecteditem.Title), FileSizeUnit.MB);

            //RingTone should not be greater than 1MB
            if (fileSize > 1)
            {
                ErrorMessage(Constants.SongSizeErrorMessage, Visibility.Collapsed);
                if (Task.Run(async () => await Storage.RingToneFileExists(ResourceHelper.RingToneFileName(Constants.selecteditem.Title))).Result)
                    Storage.DeleteRingToneFile(ResourceHelper.RingToneFileName(Constants.selecteditem.Title));
            }
            else
            {
                txtfilesize.Text = fileSize.ToString("N2") + " MB";

                txtfilesize.Visibility = Visibility.Visible;
            }
        }
        public void ErrorMessage(string message, Visibility visible)
        {
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                txterror.Text = message;
                txterror.Visibility = visible;
            });
        }
        void TrimForRingTone()
        {
            try
            {
                StorageFolder store = KnownFolders.MusicLibrary;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(ResourceHelper.RingToneFileName(Constants.selecteditem.Title).Substring(ResourceHelper.RingToneFileName(Constants.selecteditem.Title).IndexOf('/') + 1), Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.ReadWrite)).Result;
                var outputStream = fquery.GetOutputStreamAt(0);
                var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                //using (var reader = new Mp3FileReader(DownloadStream))
                //{
                //    Mp3Frame frame = new Mp3Frame();
                //    while ((frame = reader.ReadNextFrame()) != null)
                //    {
                //        if ((int)reader.CurrentTime.TotalSeconds <= Convert.ToDouble(tbend.Text) && (int)reader.CurrentTime.TotalSeconds >= Convert.ToDouble(tbbegin.Text))
                //        {
                //            writer.WriteBytes(frame.RawData);
                //        }
                //    }
                //    var fi = Task.Run(async () => await writer.StoreAsync()).Result;
                //    writer.DetachStream();
                //    var oi = Task.Run(async () => await outputStream.FlushAsync()).Result;
                //    outputStream.Dispose();
                //    fquery.Dispose();
                //}
            }
            catch (Exception ex)
            {
            }
        }
        void songReady()
        {
            playimage.Source = LoadImage(Constants.PlayerPlayImage) as ImageSource;
            ControlVisibility(Visibility.Visible);
            txtTitle.Text = Constants.selecteditem.Title;
        }
        private void Playsong()
        {
            RootMediaElement.Play();
            playimage.Source = LoadImage(Constants.PlayerPauseImage) as ImageSource;
        }

        public void ControlVisibility(Visibility visible)
        {
            foreach (UIElement c in Playergrid.Children)
            {
                Type type = c.GetType();
                if (type.Name == "TextBox")
                {
                    TextBox t = c as TextBox;
                    if (t.Name != "txterror" && t.Name != "txtfilesize")
                        t.Visibility = visible;
                }
                else
                {
                    c.Visibility = visible;
                }
            }
        }

        void UpdateSlider(object sender, object e)
        {
            updatesliderValue(RootMediaElement.Position);
        }
        public void updatesliderValue(TimeSpan s)
        {
            ringtoneslider.Value = s.TotalSeconds;
            string TotalTime = s.ToString();
            string minutes = TotalTime.Substring(TotalTime.IndexOf(':') + 1, 2);
            string seconds = TotalTime.Substring(TotalTime.LastIndexOf(':') + 1, 2);
            TimeSpan ts = new TimeSpan(0, Convert.ToInt32(minutes), Convert.ToInt32(seconds));
            TimeSpan tot = RootMediaElement.NaturalDuration.TimeSpan;
            string actualtime = tot.ToString();
            string min = actualtime.Substring(actualtime.IndexOf(':') + 1, 2);
            string sec = actualtime.Substring(actualtime.LastIndexOf(':') + 1, 2);
            TimeSpan tstot = new TimeSpan(0, Convert.ToInt32(min), Convert.ToInt32(sec));
            txtduration.Text = ts.ToString().Substring(ts.ToString().IndexOf(':') + 1) + "/" + tstot.ToString().Substring(tstot.ToString().IndexOf(':') + 1);
        }
        void Rewind(object sender, object e)
        {
            RootMediaElement.Pause();
            int SliderValue = (int)ringtoneslider.Value;
            TimeSpan ts = new TimeSpan(0, 0, 0, SliderValue - 3, 0);
            RootMediaElement.Position = ts;
        }
        private void TrimSongForRingTone()
        {
            if (IsPreviewMode == false)
            {
                string Range = string.Empty;

                if (tbbegin.Text != "")
                {
                    tbend.Text = RootMediaElement.Position.TotalSeconds.ToString();
                    if (Convert.ToDouble(tbbegin.Text) < Convert.ToDouble(tbend.Text) && tbbegin.Text != "" && tbend.Text != "")
                    {
                        txterror.Visibility = Visibility.Collapsed;
                        Range = (Convert.ToDouble(tbend.Text) - Convert.ToDouble(tbbegin.Text)).ToString();

                        if (Convert.ToDouble(Range) <= 40)
                        {
                            bwHelper.AddBackgroundTask(
                                       (object s, DoWorkEventArgs a) =>
                                       {
                                           Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                           {
                                               TrimForRingTone();
                                           });
                                       },
                                       (object s, RunWorkerCompletedEventArgs a) =>
                                       {
                                           TrimCompleted();
                                       }
                                     );
                            bwHelper.RunBackgroundWorkers();
                            txttimerange.Text = Range + " secs";
                        }
                        else
                        {
                            ErrorMessage(Constants.SongDurationErrorMessageForRingTone, Visibility.Visible);
                        }
                    }
                }
                else
                {
                    ErrorMessage(Constants.BeginTimeErrorMessageForRingTone, Visibility.Visible);
                }
            }
            else
            {
                ErrorMessage(Constants.PreviewModeErrorMessageForRingTone, Visibility.Visible);
            }
        }

        void forward(object sender, object e)
        {
            RootMediaElement.Pause();
            int SliderValue = (int)ringtoneslider.Value;
            TimeSpan ts = new TimeSpan(0, 0, 0, SliderValue + 3, 0);
            RootMediaElement.Position = ts;
        }

        private void imgclose_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Popup p = (Popup)OnlineVideos.Common.PageNavigationManager.GetParentOfType(this, typeof(Popup));            
            object pg = p.GetType().GetTypeInfo().BaseType.GetRuntimeProperty("Tag").GetValue(p);
            ((Page)pg).GetType().GetTypeInfo().GetDeclaredMethod("ringtoneclose").Invoke(pg, null);
        }

        private void imgTitle_MouseLeftButtonDown(object sender, KeyRoutedEventArgs e)
        {
            //NavigationService.Navigate(NavigationHelper.MainPanoramaPage);            
        }

        private void ringtoneslider_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            bwHelper.StopScheduledTasks(0);
        }

        private void ringtoneslider_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            int SliderValue = (int)ringtoneslider.Value;
            TimeSpan ts = new TimeSpan(0, 0, 0, SliderValue, 0);
            RootMediaElement.Position = ts;
            bwHelper.StartScheduledTasks(0);
        }

        private void mymedia_MediaEnded(object sender, RoutedEventArgs e)
        {
            playimage.Source = LoadImage(Constants.PlayerPlayImage) as ImageSource;
            IsPreviewMode = false;
            AppSettings.IsRingtone = true;
            RootMediaElement.Source = new Uri(Constants.selecteditem.LinkUrl, UriKind.Absolute);
        }

        private void mymedia_MediaOpened(object sender, RoutedEventArgs e)
        {
            var value = RootMediaElement.NaturalDuration.TimeSpan.TotalSeconds;
            ringtoneslider.Maximum = value;
            //RootMediaElement.AutoPlay = false;
            bwHelper.AddScheduledBackgroundTask(UpdateSlider, TimeSpan.FromSeconds(1), 0);
            bwHelper.StartScheduledTasks(0);
        }

        private void Rewindimage_PointerPressed_1(object sender, PointerRoutedEventArgs e)
        {
            bwHelper.AddScheduledBackgroundTask(Rewind, TimeSpan.FromMilliseconds(5), 1);
            bwHelper.StartScheduledTasks(1);
        }

        private void Rewindimage_PointerReleased_1(object sender, PointerRoutedEventArgs e)
        {
            bwHelper.StopScheduledTasks(1);
            RootMediaElement.Play();
        }

        private void playimage_MouseLeftButtonDown(object sender, PointerRoutedEventArgs e)
        {
            BitmapImage bi = new BitmapImage();
            bi = playimage.Source as BitmapImage;
            if (bi.UriSource.ToString().Contains("play.png"))
            {
                Playsong();
                playimage.Source = LoadImage(Constants.PlayerPauseImage) as ImageSource;
            }
            else
            {
                RootMediaElement.Pause();
                playimage.Source = LoadImage(Constants.PlayerPlayImage) as ImageSource;
            }
        }

        private void forwardimage_PointerPressed_1(object sender, PointerRoutedEventArgs e)
        {
            bwHelper.AddScheduledBackgroundTask(forward, TimeSpan.FromMilliseconds(5), 2);
            bwHelper.StartScheduledTasks(2);
        }

        private void forwardimage_PointerReleased_1(object sender, PointerRoutedEventArgs e)
        {
            bwHelper.StopScheduledTasks(2);
            RootMediaElement.Play();
        }

        private void btnbegin_Click(object sender, RoutedEventArgs e)
        {
            if (IsPreviewMode == false)
            {
                txterror.Visibility = Visibility.Collapsed;
                tbbegin.Text = RootMediaElement.Position.TotalSeconds.ToString();
                tbend.Text = "";
            }
            else
            {
                ErrorMessage(Constants.PreviewModeErrorMessageForRingTone, Visibility.Visible);
            }
        }

        private void tbbegin_LostFocus(object sender, RoutedEventArgs e)
        {
            if (IsPreviewMode == false)
            {
                txterror.Visibility = Visibility.Collapsed;
            }
            else
            {
                ErrorMessage(Constants.PreviewModeErrorMessageForRingTone, Visibility.Visible);
            }
        }

        private void btnend_Click(object sender, RoutedEventArgs e)
        {
            TrimSongForRingTone();
        }

        private void tbend_LostFocus(object sender, RoutedEventArgs e)
        {
            TrimSongForRingTone();
        }

        private void btnpreview_PointerPressed_1(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                if (Task.Run(async () => await Storage.RingToneFileExists(ResourceHelper.RingToneFileName(Constants.selecteditem.Title))).Result)
                {
                    IsPreviewMode = true;
                    txterror.Visibility = Visibility.Collapsed;
                    RootMediaElement.Stop();
                    BitmapImage bi = new BitmapImage();
                    bi = playimage.Source as BitmapImage;
                    if (bi.UriSource.ToString().Contains("pause.png"))
                    {
                        playimage.Source = LoadImage(Constants.PlayerPlayImage) as ImageSource;
                    }
                    StorageFolder store = Windows.Storage.KnownFolders.MusicLibrary;
                    StorageFile file = Task.Run(async () => await store.CreateFileAsync(ResourceHelper.RingToneFileName(Constants.selecteditem.Title), Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                    var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.ReadWrite)).Result;
                    RootMediaElement.SetSource(fquery, file.ContentType);
                    RootMediaElement.Play();
                    playimage.Source = LoadImage(Constants.PlayerPauseImage) as ImageSource;
                }
                else
                {
                    ErrorMessage(Constants.TimeErrorMessageForRingTone, Visibility.Visible);
                }
            }
            catch(Exception ex)
            {
                string excp = ex.Message;
            }
        }

        private void btncancel_PointerPressed_1(object sender, PointerRoutedEventArgs e)
        {
            if (Task.Run(async () => await Storage.RingToneFileExists(ResourceHelper.RingToneFileName(Constants.selecteditem.Title))).Result)
                Storage.DeleteRingToneFile(ResourceHelper.RingToneFileName(Constants.selecteditem.Title));
            tbend.Text = "";
            tbbegin.Text = "";
            txttimerange.Text = "";
            txtfilesize.Text = "";
        }
    }
}
