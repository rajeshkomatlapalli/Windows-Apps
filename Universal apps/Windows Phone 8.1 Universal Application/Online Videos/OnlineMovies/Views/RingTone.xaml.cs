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
using System.Windows;
using System.Net.NetworkInformation;
using Windows.Networking.Connectivity;
using System.Net.Http;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using Windows.Storage;
using Common.Library;
using Common.Utilities;
using Windows.UI.Popups;
using System.ComponentModel;
using OnlineVideos.RingToneEditor;
using Windows.Storage.Streams;
using Windows.Media.Playback;
using Windows.Networking.BackgroundTransfer;
using Windows.UI.Core;
using Windows.Storage.Provider;
using Windows.Storage.Pickers;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RingTone : Page
    {
        #region GlobalDeclaration
        BackgroundHelper bwHelper = new BackgroundHelper();
        static Stream DownloadStream;
        bool IsPreviewMode = false;
        public ValueSet message { get; set; }
        private MediaPlayer _mediaPlayer;
        public Uri urlofaudio;
        public static bool DownlodeOvere { get; set; }
        #endregion

        #region Constructor
        public RingTone()
        {
            this.InitializeComponent();
            Loaded += RingTone_Loaded;
        }
        #endregion

        #region PageLoad
        void RingTone_Loaded(object sender, RoutedEventArgs e)
        {
            _mediaPlayer = BackgroundMediaPlayer.Current;
            try
            {
                //FlurryWP8SDK.Api.LogEvent("RingTone Page", true);
                LoadAds();
                if (NetworkInterface.GetIsNetworkAvailable() && NetworkInformation.GetConnectionProfiles().ToString()!=null)
                {                    
                    txterror.Visibility = Visibility.Collapsed;
                    performanceProgressBar.IsIndeterminate = true;
                    if (!AppSettings.ShowLinkTitle.ToLower().StartsWith("http://"))
                    {
                        if (AppSettings.ShowLinkTitle.ToLower().Contains("www."))
                            AppSettings.ShowLinkTitle = "http://" + AppSettings.ShowLinkTitle;
                        else
                            AppSettings.ShowLinkTitle = "http://www." + AppSettings.ShowLinkTitle;
                    }
                    playaudio(AppSettings.ShowLinkTitle.ToString());                               
                    HttpClient web = new HttpClient();
                }
                else
                {                                 
                    BottomAppBar.Visibility = Visibility.Collapsed;
                    ErrorMessage(Constants.NetworkErrorMessageForRingTone, Visibility.Visible);
                }
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in RingTone_Loaded Method In RingTone.cs file.", ex);
            }
        }
         #endregion

        public void playaudio(string Audiourl)
        {
            urlofaudio = new Uri(AppSettings.ShowLinkTitle, UriKind.Absolute);
            try
            {                
                if (Audiourl != null)
                {
                    mymedia.Source = new Uri(AppSettings.ShowLinkTitle, UriKind.Absolute);
                    songReady();
                    playimage.Source = LoadImage(Constants.PlayerPauseImage) as ImageSource;
                    ringtoneloadingmessage.Visibility = Visibility.Collapsed;
                    performanceProgressBar.IsIndeterminate = false;
                }    
                    else
                {
                    ErrorMessage(Constants.NetworkErrorMessageForRingTone, Visibility.Visible);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadDownLoads method In ShowAudio.cs file.", ex);
            }
        }

        void songReady()
        {
            playimage.Source = LoadImage(Constants.PlayerPlayImage) as ImageSource;
            ControlVisibility(Visibility.Visible);
            txtTitle.Text = AppSettings.Chapterno;
        }

        private void LoadAds()
        {
            try
            {
                PageHelper.LoadAdControl_New(LayoutRoot, adstaRing, 1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In SongDetails file", ex);
                string excepmess = "Exception in LoadAds Method In SongDetails file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }

        }

        public void ErrorMessage(string message, Visibility visible)
        {
            txterror.Text = message;
            txterror.Visibility = visible;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                DownlodeOvere = false;           
                if (Application.Current.Resources.Keys.Contains("RingtoneStatus"))
                {
                    if (AppState.RingtoneStatus == "TombStoned" || AppState.RingtoneStatus == "RingToneTask")
                    {
                        AppState.RingtoneStatus = "";                                          
                    }
                    else
                    {
                        txtfilesize.Visibility = Visibility.Collapsed;
                        txterror.Visibility = Visibility.Collapsed;
                        ControlVisibility(Visibility.Collapsed);
                    }
                }
                else
                {
                    txtfilesize.Visibility = Visibility.Collapsed;
                    txterror.Visibility = Visibility.Collapsed;
                    ControlVisibility(Visibility.Collapsed);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In RingTone.cs file.", ex);
            }
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

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
               // FlurryWP8SDK.Api.EndTimedEvent("RingTone Page");               
                var backkey = Frame.BackStack.ToList();                
                foreach (var itm in backkey)
                {                    
                    if (itm.SourcePageType.ToString().Contains(AppResources.DetailPageName) || itm.SourcePageType.ToString().Contains("RingTone.xaml"))
                    {                       
                        Frame.BackStack.RemoveAt(-1);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom Method In RingTone.cs file.", ex);
            }
        }
        private void imgTitle_Tapped(object sender, TappedRoutedEventArgs e)
        {            
            Frame.Navigate(typeof(MainPage));
        }

        private void ringtoneslider_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            try
            {
                bwHelper.StopScheduledTasks(0);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ringtoneslider_ManipulationStarted Method In RingTone.cs file.", ex);
            }
        }

        private void ringtoneslider_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            try
            {
                int SliderValue = (int)ringtoneslider.Value;
                TimeSpan ts = new TimeSpan(0, 0, 0, SliderValue, 0);
                mymedia.Position = ts;
                bwHelper.StartScheduledTasks(0);
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in ringtoneslider_ManipulationCompleted Method In RingTone.cs file.", ex);
            }
        }

        private void mymedia_MediaEnded(object sender, RoutedEventArgs e)
        {
            try
            {
                playimage.Source = LoadImage(Constants.PlayerPlayImage) as ImageSource;
                IsPreviewMode = false;
                mymedia.Source = new Uri(AppSettings.ShowLinkTitle, UriKind.Absolute);
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in mymedia_MediaEnded Method In RingTone.cs file.", ex);
            }
        }

        private void mymedia_MediaOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                var value = mymedia.NaturalDuration.TimeSpan.TotalSeconds;
                ringtoneslider.Value = value;
                mymedia.AutoPlay = false; 
                TimeSpan interval= TimeSpan.FromSeconds(1);
                bwHelper.AddScheduledBackgroundTask(UpdateSlider, TimeSpan.FromSeconds(1), 0);
                bwHelper.StartScheduledTasks(0);
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in mymedia_MediaOpened Method In RingTone.cs file.", ex);
            }
        }
        public void UpdateSlider(object sender, object e)
        {
            try
            {                               
              updatesliderValue(mymedia.Position);               
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in UpdateSlider Method In RingTone.cs file.", ex);
            }
        }        
        public void updatesliderValue(TimeSpan s)
        {
            ringtoneslider.Value = s.TotalSeconds;
            string TotalTime = s.ToString();
            string minutes = TotalTime.Substring(TotalTime.IndexOf(':') + 1, 2);
            string seconds = TotalTime.Substring(TotalTime.LastIndexOf(':') + 1, 2);
            TimeSpan ts = new TimeSpan(0, Convert.ToInt32(minutes), Convert.ToInt32(seconds));
            TimeSpan tot = mymedia.NaturalDuration.TimeSpan;
            string actualtime = tot.ToString();
            string min = actualtime.Substring(actualtime.IndexOf(':') + 1, 2);
            string sec = actualtime.Substring(actualtime.LastIndexOf(':') + 1, 2);
            TimeSpan tstot = new TimeSpan(0, Convert.ToInt32(min), Convert.ToInt32(sec));
            txtduration.Text = ts.ToString().Substring(ts.ToString().IndexOf(':') + 1) + "/" + tstot.ToString().Substring(tstot.ToString().IndexOf(':') + 1);
        }
        private void btnbegin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsPreviewMode == false)
                {
                    txterror.Visibility = Visibility.Collapsed;
                    tbbegin.Text = mymedia.Position.TotalSeconds.ToString();
                    tbend.Text = "";
                }
                else
                {
                    ErrorMessage(Constants.PreviewModeErrorMessageForRingTone, Visibility.Visible);
                }
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in btnbegin_Click Method In RingTone.cs file.", ex);
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
            try
            {
                TrimSongForRingTone();
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in btnend_Click Method In RingTone.cs file.", ex);
            }
        }

        private void tbend_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                TrimSongForRingTone();
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in tbend_LostFocus Method In RingTone.cs file.", ex);
            }
        }

       async private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Task.Run(async () => await Storage.FileExists(ResourceHelper.RingToneFileName(AppSettings.Chapterno))).Result)
                {
                    IsPreviewMode = true;
                    txterror.Visibility = Visibility.Collapsed;
                    mymedia.Stop();                    
                    BitmapImage bi = new BitmapImage();
                    bi = playimage.Source as BitmapImage;
                    if (bi.UriSource.ToString().Contains("pause.png"))
                    {
                        playimage.Source = LoadImage(Constants.PlayerPlayImage) as ImageSource;
                    }                   
                    StorageFolder myIsolatedStorage = ApplicationData.Current.LocalFolder;
                    IStorageFile local = await myIsolatedStorage.GetFileAsync(ResourceHelper.RingToneFileName(AppSettings.Chapterno));
                    Stream stream11 = (await local.OpenAsync(FileAccessMode.ReadWrite)).AsStream();                                       
                    using (BinaryReader reader = new BinaryReader(stream11))                    
                    {
                        byte[] buffer = reader.ReadBytes((int)stream11.Length);
                        IRandomAccessStream nstream = new MemoryStream(buffer).AsRandomAccessStream();
                        mymedia.SetSource(nstream, "");
                    }

                    mymedia.AutoPlay = true;
                    playimage.Source = LoadImage(Constants.PlayerPauseImage) as ImageSource;

                }
                else
                {
                    ErrorMessage(Constants.TimeErrorMessageForRingTone, Visibility.Visible);
                }
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in btnPreview_Click Method In RingTone.cs file.", ex);
            }
        }

        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Task.Run(async () => await Storage.FileExists(ResourceHelper.RingToneFileName(AppSettings.Chapterno))).Result)
                {
                    AppState.RingtoneStatus = "RingToneTask";

                    //StorageFolder store = KnownFolders.MusicLibrary;
                    string filename = ResourceHelper.RingToneFileName(AppSettings.Chapterno);
                    //StorageFile file = Task.Run(async () => await store.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists)).Result;

                    StorageFolder storage = ApplicationData.Current.LocalFolder;
                    //StorageFile file1 = Task.Run(async () => await storage.GetFileAsync(filename)).Result;


                    StorageFolder folder = ApplicationData.Current.LocalFolder;
                    StorageFile file = await folder.GetFileAsync(filename);

                    var st = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

                    var size = st.Size;

                    FileSavePicker savePicker = new FileSavePicker();
                    //savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                    savePicker.SuggestedSaveFile = file;
                    savePicker.FileTypeChoices.Add("MP3", new List<string>() { ".mp3" });
                    savePicker.ContinuationData.Add("SourceSound", file.Path);
                    savePicker.SuggestedFileName = "DeviPrasad";

                    savePicker.PickSaveFileAndContinue(); 



                    //Stream stream111 = (await file1.OpenAsync(FileAccessMode.ReadWrite)).AsStream();                 
                    //using(Stream stream11 = (await file.OpenAsync(FileAccessMode.ReadWrite)).AsStream())
                    //{                        
                    //    using (var reader = new Mp3FileReader(stream111))
                    //    {
                    //        Mp3Frame frame = new Mp3Frame();
                    //        {
                    //            while ((frame = reader.ReadNextFrame()) != null)
                    //            {
                    //                stream11.Write(frame.RawData, 0, frame.RawData.Length);
                    //            }
                    //        }
                    //    }

                    //}                    
                }
                else
                {
                    ErrorMessage(Constants.TimeErrorMessageForSaveRingTone, Visibility.Visible);
                }                
            }               
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in ApplicationBarIconButton_Click Method In RingTone.cs file.", ex);
            }
            Frame.GoBack();
        }
        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {            
            try
            {
                mymedia.Source = null;
                if (Task.Run(async () => await Storage.FileExists(ResourceHelper.RingToneFileName(AppSettings.Chapterno))).Result)
                    Storage.DeleteFile(ResourceHelper.RingToneFileName(AppSettings.Chapterno));
                AppState.RingtoneStatus = "";
                string[] parameters=new string[2];
                parameters[0]=AppSettings.ShowID;
                parameters[1]=null;
                Frame.Navigate(typeof(Details), parameters);               
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ApplicationBarIconButton_Click_1 Method In RingTone.cs file.", ex);
            }
        }
               
        private void Rewindimage_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                bwHelper.AddScheduledBackgroundTask(Rewind, TimeSpan.FromMilliseconds(5), 1);
                bwHelper.StartScheduledTasks(1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Rewindimage_MouseLeftButtonDown Method In RingTone.cs file.", ex);
            }
        }

        void Rewind(object sender, object e)
        {
            mymedia.Pause();
            int SliderValue = (int)ringtoneslider.Value;
            TimeSpan ts = new TimeSpan(0, 0, 0, SliderValue - 3, 0);
            mymedia.Position = ts;
        }

        private void Rewindimage_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                bwHelper.StopScheduledTasks(1);
                mymedia.Play();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Rewindimage_MouseLeftButtonUp Method In RingTone.cs file.", ex);
            }
        }

        private void forwardimage_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                bwHelper.AddScheduledBackgroundTask(forward, TimeSpan.FromMilliseconds(5), 2);
                bwHelper.StartScheduledTasks(2);
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in forwardimage_MouseLeftButtonDown Method In RingTone.cs file.", ex);
            }
        }

        void forward(object sender, object e)
        {
            mymedia.Pause();
            int SliderValue = (int)ringtoneslider.Value;
            TimeSpan ts = new TimeSpan(0, 0, 0, SliderValue + 3, 0);
            mymedia.Position = ts;
        }

        private void forwardimage_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                bwHelper.StopScheduledTasks(2);
                mymedia.Play();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in forwardimage_MouseLeftButtonUp Method In RingTone.cs file.", ex);
            }
        }

        private void playimage_PointerPressed(object sender, PointerRoutedEventArgs e)
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
                mymedia.Pause();
                playimage.Source = LoadImage(Constants.PlayerPlayImage) as ImageSource;
            }
        }

        private void Playsong()
        {
            mymedia.Play();
            playimage.Source = LoadImage(Constants.PlayerPauseImage) as ImageSource;
        }

        object LoadImage(string path)
        {
            object objImage = null;
            BitmapImage bi = new BitmapImage();
            bi.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            objImage = bi;
            return objImage;
        }
        private readonly CoreDispatcher dispatcher;
        private void TrimSongForRingTone()
        {
            if (IsPreviewMode == false)
            {
                string Range = string.Empty;
                tbend.Text = mymedia.Position.TotalSeconds.ToString();
                if (tbbegin.Text != "")
                {
                    if (Convert.ToDouble(tbbegin.Text) < Convert.ToDouble(tbend.Text) && tbbegin.Text != "" && tbend.Text != "")
                    {
                        txterror.Visibility = Visibility.Collapsed;
                        Range = (Convert.ToDouble(tbend.Text) - Convert.ToDouble(tbbegin.Text)).ToString();
                        string NewRange;
                        if(Range.Length>5)
                        {
                           NewRange= Range.Remove(Range.Length - 4);
                           txttimerange.Text = NewRange + " secs";
                        }
                        if (Convert.ToDouble(Range) <= 40)
                        {                           
                            TrimForRingTone();                           
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

       async void TrimForRingTone()
        {
            try
            {
                var downloader = new BackgroundDownloader();
                StorageFolder myIsolatedStorage = ApplicationData.Current.LocalFolder;
                string filename = ResourceHelper.RingToneFileName(AppSettings.Chapterno).Substring(ResourceHelper.RingToneFileName(AppSettings.Chapterno).IndexOf('/') + 1);
                StorageFile local1 = await myIsolatedStorage.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);

                if (DownlodeOvere == false)
                {
                    StorageFolder Istorage = ApplicationData.Current.LocalFolder;
                    StorageFile local11 = await Istorage.CreateFileAsync("CutterMP3.mp3", CreationCollisionOption.OpenIfExists);

                    var download = downloader.CreateDownload(urlofaudio, local11);
                    await download.StartAsync();
                    DownlodeOvere = true;
                }
                //IRandomAccessStream accessStream = await local1.OpenReadAsync();
                //Stream local = accessStream.AsStreamForRead((int)accessStream.Size);

                StorageFile sampleFile1 = await myIsolatedStorage.GetFileAsync("CutterMP3.mp3");
                var stream1 = (await sampleFile1.OpenAsync(FileAccessMode.ReadWrite)).AsStream();
                StorageFile sampleFile = await myIsolatedStorage.GetFileAsync(filename);
                using (var stream = (await sampleFile.OpenAsync(FileAccessMode.ReadWrite)).AsStream())
                {

                    using (var reader = new Mp3FileReader(stream1))
                    {
                        Mp3Frame frame = new Mp3Frame();
                        while ((frame = reader.ReadNextFrame()) != null)
                        {                            
                            if ((int)reader.CurrentTime.TotalSeconds <= Convert.ToDouble(tbend.Text) && (int)reader.CurrentTime.TotalSeconds >= Convert.ToDouble(tbbegin.Text))                          
                            stream.Write(frame.RawData, 0, frame.RawData.Length);
                        }                       
                        stream.Dispose();
                    }
                }
                TrimCompleted();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        void TrimCompleted()
        {
            prev.Visibility = Visibility.Visible;
            save.Visibility = Visibility.Visible;
            cancel.Visibility = Visibility.Visible;

            float fileSize = Storage.GetFileSize(ResourceHelper.RingToneFileName(AppSettings.Chapterno), FileSizeUnit.MB);
            if (fileSize > 1)
            {
                ErrorMessage(Constants.SongSizeErrorMessage, Visibility.Collapsed);
                if (Task.Run(async () => await Storage.FileExists(ResourceHelper.RingToneFileName(AppSettings.Chapterno))).Result)
                  Storage.DeleteFile(ResourceHelper.RingToneFileName(AppSettings.Chapterno));
            }
            else
            {
                txtfilesize.Text = fileSize.ToString("N2") + " MB";
                txtfilesize.Visibility = Visibility.Visible;               
            }
            
        }

        internal async void ContinueFileOpenPicker(Windows.ApplicationModel.Activation.FileSavePickerContinuationEventArgs e)
        {
            var file = e.File;
            string soundPath = (string)e.ContinuationData["SourceSound"];
            //var ff= file.Properties;
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);

                StorageFile srcFile = await StorageFile.GetFileFromPathAsync(soundPath);

                await srcFile.CopyAndReplaceAsync(file);

                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
            }
        }
    }
}
