using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StoryReading : Page
    {
        int slidestatus = 0;
        int pagenumber = 1;
        long oldPos1 = 0;
        int previouspage = 0;
        private bool recordclicked = false;
        private bool buttonpressed = false;
        private bool buttontapped = false;
        private bool backpressed = false;
        private bool stopclicked = false;
        private bool recordstopped = false;
        bool CancelEvents = false;
        static bool pagenavigatedback = false;
        int rerecordindex = 0;
        bool recording = false;
        bool paused = false;
        private MediaCapture _mediaCaptureManager;
        private StorageFile _recordStorageFile;
        private bool _userRequestedRaw;
        private bool _rawAudioSupported;
        public bool recstarted = false;

        public StoryReading()
        {
            this.InitializeComponent();
            InitializeAudioRecording();
        }

        private async void InitializeAudioRecording()
        {

            _mediaCaptureManager = new MediaCapture();
            var settings = new MediaCaptureInitializationSettings();
            settings.StreamingCaptureMode = StreamingCaptureMode.Audio;
            settings.MediaCategory = MediaCategory.Other;
            settings.AudioProcessing = (_rawAudioSupported && _userRequestedRaw) ? AudioProcessing.Raw : AudioProcessing.Default;

            await _mediaCaptureManager.InitializeAsync(settings);

            //Debug.WriteLine("Device initialised successfully");

            //_mediaCaptureManager.RecordLimitationExceeded += new RecordLimitationExceededEventHandler(RecordLimitationExceeded);
            //_mediaCaptureManager.Failed += new MediaCaptureFailedEventHandler(Failed);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ShowList objlist = OnlineShow.GetShowDetail(AppSettings.ShowUniqueID);
            txttitle.Text = objlist.Title;
            if (ResourceHelper.AppName == Apps.Vedic_Library.ToString() && objlist.SubTitle == Constants.MovieCategoryTelugu)
            {
                txtDescription.FontFamily = new FontFamily("/Pothana2000.ttf#Pothana2000");
                txtDescription.FontSize = 31;
            }
            if (objlist.SubTitle == Constants.MovieCategoryEnglish)
            {
                if (ResourceHelper.AppName == Apps.Story_Time.ToString())
                {
                    txtDescription.FontFamily = new FontFamily("/COM4NRG_.TTF#COM4t Nuvu Regular");
                    txtDescription.FontSize = 30;
                }
                else
                {
                    txtDescription.FontSize = 30;
                }
            }
            if (ResourceHelper.AppName == Apps.Vedic_Library.ToString() && objlist.SubTitle == Constants.MovieCategoryHindi)
            {
                txtDescription.FontFamily = new FontFamily("/CDACOTYGB.TTF#CDAC-GISTYogesh");
                txtDescription.FontSize = 30;
            }
            try
            {
                loadingblock.Visibility = Visibility.Visible;
                //_performanceProgressBar.Visibility = Visibility.Visible;
                //_performanceProgressBar.IsIndeterminate = true;

                if (ResourceHelper.AppName == Apps.Vedic_Library.ToString() && Constants.mode == "Listenmp3")
                {
                    string title = StoryManager.GetTitle(AppSettings.ShowUniqueID);
                    if (!Task.Run(async () => await Storage.FileExists(title + ".mp3")).Result)
                    {

                        //if (NetworkInterface.GetIsNetworkAvailable() && NetworkInterface.NetworkInterfaceType != NetworkInterfaceType.None)
                        //{
                        //    ShowLinks objlinks = new ShowLinks();
                        //    Constants.Downloadedstream = default(Stream);
                        //    WebClient web = new WebClient();
                        //    objlinks = (OnlineShow.GetAudioLinks(AppSettings.ShowID, LinkType.Audio));
                        //    web.OpenReadAsync(new Uri(objlinks.LinkUrl, UriKind.Absolute));
                        //    web.OpenReadCompleted += new OpenReadCompletedEventHandler(web_OpenReadCompleted);
                        //}


                        //else
                        //{
                        //    Constants.mode = "Read";
                        //    Loaded += new RoutedEventHandler(StoryReading_Loaded);
                        //}
                    }


                    else
                    {
                        Loaded += new RoutedEventHandler(StoryReading_Loaded);
                    }
                }
                else
                {

                    Loaded += new RoutedEventHandler(StoryReading_Loaded);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In StoryReading file", ex);
                string excepmess = "Exception in OnNavigatedTo Method In StoryReading file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }
        private void LoadAds()
        {
            try
            {
                LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In StoryReading file", ex);
                string excepmess = "Exception in LoadAds Method In StoryReading file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }
        private void StoryReading_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadAds();
                // txttitle.Text = StoryManager.GetTitle(AppSettings.ShowUniqueID);
                StoryReadingExperience.StartRetriving(this.ContentPanel);
                if (ResourceHelper.AppName == Apps.Vedic_Library.ToString() && Constants.mode == "Listenmp3")
                    StoryReadingExperience.RetriveFromStorage(this.imgstory, txtDescription, txtpage, pagenumber, this.ContentPanel, Constants.mode == "Listenmp3" ? this.me : null, buttontapped);
                else
                    StoryReadingExperience.RetriveFromStorage(this.imgstory, txtDescription, txtpage, pagenumber, this.ContentPanel, Constants.mode == "Listen" ? this.me:null, buttontapped);
                if (ResourceHelper.AppName == Apps.Vedic_Library.ToString() && Constants.mode == "Listenmp3")
                { }
                else
                {

                }
                loadingblock.Visibility = Visibility.Collapsed;
                //_performanceProgressBar.IsIndeterminate = false;
                //_performanceProgressBar.Visibility = Visibility.Collapsed;
                if (Constants.mode == "Rec")
                {
                    showstack();
                }

                if (Constants.mode == "Rec" && StoryReadingExperience.storyDictionary.Count == getcount())
                {
                    List<Stories> stories = Constants.connection.Table<Stories>().OrderByDescending(k => k.ID).ToListAsync().Result.Take(1).ToList();
                    //IQueryable<Stories> stories = (from k in context.storyTable
                    //                               orderby k.ID descending

                    //                               select k).Take(1);

                    foreach (Stories s in stories)
                    {
                        s.RecordCompleted = false;
                        Constants.connection.UpdateAsync(s);
                    }


                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in StoryReading_Loaded In StoryReading file", ex);
                string excepmess = "Exception in StoryReading_Loaded In StoryReading file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }

        private void LayoutRoot_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            try
            {
                if (slidestatus > 0)
                {
                    if (Constants.mode == "Listen")
                    {

                        me.Stop();
                        me.Source = null;

                    }
                    if (Constants.mode != "Listenmp3")
                    {
                        slidestatus = StoryReadingExperience.storystackmanipulationCompleted(this.imgstory, this.txtDescription, this.txtpage, pagenumber, this.ContentPanel, Constants.mode == "Listen" ? this.me : null, buttontapped);
                        this.messagestk.Visibility = Visibility.Collapsed;
                    }

                    if (Constants.mode == "Listenmp3")
                    {

                        me.Stop();
                        me.Source = null;
                        slidestatus = StoryReadingExperience.storystackmanipulationCompleted(this.imgstory, this.txtDescription, this.txtpage, pagenumber, this.ContentPanel, Constants.mode == "Listenmp3" ? this.me : null, buttontapped);
                        this.messagestk.Visibility = Visibility.Collapsed;
                    }
                    if (Constants.mode == "Rec")
                    {
                        buttonpressed = false;
                        showstack();
                    }

                }
                else
                {
                    if (Constants.mode != "Read")
                    {
                        buttonpressed = false;
                        showstack();
                        //if (Constants.mode == "Listen")
                        //{
                        //    DispatcherTimer buttonTimer = new DispatcherTimer();
                        //    buttonTimer.Interval = TimeSpan.FromSeconds(10);
                        //    buttonTimer.Tick += new EventHandler(buttonTimer_Tick);
                        //    buttonTimer.Start();
                        //}
                    }

                }

            }

            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in StackPanel_ManipulationCompleted method In StoryReading file", ex);
                string excepmess = "Exception in StackPanel_ManipulationCompleted method In StoryReading file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }

        private void LayoutRoot_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            try
            {
                slidestatus++;
                if (slidestatus == 1)
                {
                    if (Constants.mode == "Rec")
                    {
                        if (recordclicked == true)
                        {
                            stop_Click_1();
                        }
                    }
                    pagenumber = StoryReadingExperience.storystackmanipulationdelta(e, pagenumber, slidestatus, this.mainstk, myStoryboard);
                    if (Constants.mode == "Rec" && previouspage > pagenumber && recordclicked == true)
                    {
                        pagenavigatedback = true;
                    }

                    previouspage = pagenumber;
                    return;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in StackPanel_ManipulationDelta method In StoryReading file", ex);
                string excepmess = "Exception in StackPanel_ManipulationDelta method In StoryReading file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }
        private void showstack()
        {
            try
            {
                if (buttonpressed == false)
                {
                    if (Constants.mode == "Rec" && Convert.ToInt32(txtpage.Text.Substring(0, txtpage.Text.IndexOf('/'))) <= getcount() && recordclicked == false)
                    {
                        this.Resume.Source = new BitmapImage(new Uri("ms-appx:///Images/Record Icons/record.png", UriKind.RelativeOrAbsolute));
                        message.Text = "Tap to re-record voice for page " + txtpage.Text.Substring(0, txtpage.Text.IndexOf('/'));
                        StoryReadingExperience.storystackmouseenter(this.messagestk);
                    }
                    else if (Constants.mode == "Rec" && Convert.ToInt32(txtpage.Text.Substring(0, txtpage.Text.IndexOf('/'))) <= getcount() && recordclicked == true)
                    {
                        this.Resume.Source = new BitmapImage(new Uri("ms-appx:///Images/Record Icons/stop.png", UriKind.RelativeOrAbsolute));
                        message.Text = "Tap to stop recording voice for page " + txtpage.Text.Substring(0, txtpage.Text.IndexOf('/'));
                        StoryReadingExperience.storystackmouseenter(this.messagestk);
                    }
                    else if (Constants.mode == "Rec" && Convert.ToInt32(txtpage.Text.Substring(0, txtpage.Text.IndexOf('/'))) == getcount() + 1 && recordclicked == false)
                    {
                        this.Resume.Source = new BitmapImage(new Uri("ms-appx:///Images/Record Icons/record.png", UriKind.RelativeOrAbsolute));
                        message.Text = "Tap to record voice for page " + txtpage.Text.Substring(0, txtpage.Text.IndexOf('/'));
                        StoryReadingExperience.storystackmouseenter(this.messagestk);
                    }
                    else if (Constants.mode == "Rec" && Convert.ToInt32(txtpage.Text.Substring(0, txtpage.Text.IndexOf('/'))) == getcount() + 1 && recordclicked == true)
                    {
                        this.Resume.Source = new BitmapImage(new Uri("ms-appx:///Images/Record Icons/stop.png", UriKind.RelativeOrAbsolute));
                        message.Text = "Tap to  stop recording voice for page " + txtpage.Text.Substring(0, txtpage.Text.IndexOf('/'));
                        StoryReadingExperience.storystackmouseenter(this.messagestk);
                    }
                    else if (Constants.mode == "Listen" || Constants.mode == "Listenmp3")
                    {
                        StoryReadingExperience.storystackmouseenter(this.messagestk);
                    }

                }
                else
                {
                    buttonpressed = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in showstack In StoryReading file", ex);
                string excepmess = "Exception in showstack In StoryReading file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }
        public int getcount()
        {
            int count = 0;
            if (Storage.FileExistsInMusicLibrary("StoryRecordings.xml"))
            {
                XDocument xdoc = new XDocument();
                StorageFolder store = ApplicationData.Current.LocalFolder;
                var story = Task.Run(async () => await store.CreateFolderAsync("StoryRecordings", CreationCollisionOption.OpenIfExists)).Result;
                var story1 = Task.Run(async () => await story.CreateFolderAsync(AppSettings.ShowID, CreationCollisionOption.OpenIfExists)).Result;
                StorageFile file = Task.Run(async () => await story1.CreateFileAsync("StoryRecordings.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var f = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = f.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
                string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
                System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlData));
                xdoc = XDocument.Load(ms);
                dataReader.DetachStream();
                inputStream.Dispose();
                f.Dispose();
                ms.Dispose();
                var data = xdoc.Descendants("Stories").Elements("Story").Elements("filename").Select(i => i.Value).ToList();
                if (data.Count() > 0)
                {
                    count = data.Count();
                }
            }
            //if (Task.Run(async () => await Storage.FileExists("/StoryRecordings/" + AppSettings.ShowID + "/StoryRecordings.xml")).Result)
            //{
            //    XDocument xdoc = Storage.ReadFileAsDocument("/StoryRecordings/" + AppSettings.ShowID + "/StoryRecordings.xml");
            //    var data = xdoc.Descendants("Stories").Elements("Story").Elements("End").Select(i => i.Value).ToList();

            //    count = data.Count();
            //}
            return count;
        }

        private void Resume_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Constants.mode == "Rec" && recordclicked == true)
                {
                    this.Resume.Source = new BitmapImage(new Uri("ms-appx:///Images/Record Icons/stop.png", UriKind.RelativeOrAbsolute));

                }
                else if (Constants.mode == "Rec" && recordclicked == false)
                {
                    this.Resume.Source = new BitmapImage(new Uri("ms-appx:///Images/Record Icons/record.png", UriKind.RelativeOrAbsolute));
                }
                else if (Constants.mode == "Listen" && me.CurrentState != MediaElementState.Playing)
                {
                    this.Resume.Margin = new Thickness(0, 50, 0, 10);
                    this.message.Margin = new Thickness(35, 0, 10, 0);
                    this.Resume.Source = new BitmapImage(new Uri("ms-appx:///Images/Record Icons/play.png", UriKind.RelativeOrAbsolute));
                }
                else if (Constants.mode == "Listen")
                {
                    this.Resume.Margin = new Thickness(0, 50, 0, 10);
                    this.message.Margin = new Thickness(30, 0, 10, 0);
                    this.Resume.Source = new BitmapImage(new Uri("ms-appx:///Images/Record Icons/pause.png", UriKind.RelativeOrAbsolute));
                }
                else if (Constants.mode == "Listenmp3" && me.CurrentState != MediaElementState.Playing)
                {
                    this.Resume.Margin = new Thickness(0, 50, 0, 10);
                    this.message.Margin = new Thickness(35, 0, 10, 0);
                    this.Resume.Source = new BitmapImage(new Uri("ms-appx:///Images/Record Icons/play.png", UriKind.RelativeOrAbsolute));
                }
                else if (Constants.mode == "Listenmp3")
                {
                    this.Resume.Margin = new Thickness(0, 50, 0, 10);
                    this.message.Margin = new Thickness(30, 0, 10, 0);
                    this.Resume.Source = new BitmapImage(new Uri("ms-appx:///Images/Record Icons/pause.png", UriKind.RelativeOrAbsolute));
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Resume_Loaded In StoryReading file", ex);
                string excepmess = "Exception in Resume_Loaded In StoryReading file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }
        private void Resume_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                buttonpressed = true;

                if (Constants.mode == "Listen" || Constants.mode == "Listenmp3")
                {
                    if (me.CurrentState == MediaElementState.Playing)
                    {
                        me.Pause();
                        buttontapped = true;
                        this.messagestk.Visibility = Visibility.Collapsed;
                        return;

                    }

                    else
                    {
                        me.Play();
                        buttontapped = false;
                        this.messagestk.Visibility = Visibility.Collapsed;
                        return;
                    }

                }

                if (Constants.mode == "Rec")
                {
                    if (((BitmapImage)this.Resume.Source).UriSource.ToString().Contains("record.png"))
                    {
                        record_Click();
                        this.Resume.Source = new BitmapImage(new Uri("ms-appx:///Images/Record Icons/stop.png", UriKind.RelativeOrAbsolute));
                        this.messagestk.Visibility = Visibility.Collapsed;
                        return;
                    }
                    else
                    {
                        recordstopped = true;
                        stop_Click_1();
                        this.Resume.Source = new BitmapImage(new Uri("ms-appx:///Images/Record Icons/record.png", UriKind.RelativeOrAbsolute));
                        this.messagestk.Visibility = Visibility.Collapsed;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in resume_Click In StoryReading file", ex);
                string excepmess = "Exception in resume_Click In StoryReading file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }

        private async void stop_Click_1()
        {
            try
            {
                long beginp = oldPos1;

                if (recstarted == true)
                {
                    await _mediaCaptureManager.StopRecordAsync();
                    recstarted = false;
                    stopclicked = true;
                }
                if (Constants.mode == "Rec" && recordclicked == true)
                {
                    savexml(pagenumber, AppSettings.Title + pagenumber + ".wav".Trim());
                    recordclicked = false;
                }
                if (Constants.mode == "Rec" && StoryReadingExperience.storyDictionary.Count == getcount())
                {
                    int showid = Convert.ToInt32(AppSettings.ShowID);
                    List<Stories> stories = Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == showid).OrderByDescending(k => k.ID).ToListAsync()).Result.Take(1).ToList();

                    foreach (Stories s in stories)
                    {
                        s.RecordCompleted = true;
                        Constants.connection.UpdateAsync(s);
                    }

                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in stop method In StoryReading file", ex);
                string excepmess = "Exception in stop method In StoryReading file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }
        private void savexml(int pagenum, string filename)
        {
            try
            {
                int pageno = 0;
                if (recordstopped == true)
                {
                    recordstopped = false;
                }
                pageno = pagenum;

                if (Storage.FileExistsInMusicLibrary("StoryRecordings.xml"))
                {
                    if (checkxml(pageno))
                    {
                        updatexml(pageno, filename);
                    }
                    else
                    {
                        XDocument xdoc = new XDocument();
                        StorageFolder store = ApplicationData.Current.LocalFolder;
                        var story = Task.Run(async () => await store.CreateFolderAsync("StoryRecordings", CreationCollisionOption.OpenIfExists)).Result;
                        var story1 = Task.Run(async () => await story.CreateFolderAsync(AppSettings.ShowID, CreationCollisionOption.OpenIfExists)).Result;
                        StorageFile file = Task.Run(async () => await story1.CreateFileAsync("StoryRecordings.xml", CreationCollisionOption.OpenIfExists)).Result;
                        var f = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                        IInputStream inputStream = f.GetInputStreamAt(0);
                        DataReader dataReader = new DataReader(inputStream);
                        uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
                        string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
                        System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlData));
                        xdoc = XDocument.Load(ms);
                        dataReader.DetachStream();
                        inputStream.Dispose();
                        f.Dispose();
                        ms.Dispose();

                        xdoc.Root.Add(new XElement("Story",
                           new XAttribute("Id", pageno),
                            new XElement("filename", filename),
                             new XElement("RecordCompleted", "true")));

                        StorageFolder store1 = ApplicationData.Current.LocalFolder;
                        var story3 = Task.Run(async () => await store1.CreateFolderAsync("StoryRecordings", CreationCollisionOption.OpenIfExists)).Result;
                        var story4 = Task.Run(async () => await story3.CreateFolderAsync(AppSettings.ShowID, CreationCollisionOption.OpenIfExists)).Result;
                        StorageFile file1 = Task.Run(async () => await story4.CreateFileAsync("StoryRecordings.xml", CreationCollisionOption.OpenIfExists)).Result;
                        var fquery1 = Task.Run(async () => await file1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                        var outputStream = fquery1.GetOutputStreamAt(0);
                        var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                        StringBuilder sb = new StringBuilder();
                        TextWriter tx = new StringWriter(sb);
                        xdoc.Save(tx);
                        string text = tx.ToString();
                        text = text.Replace("utf-16", "utf-8");
                        writer.WriteString(text);
                        var fi = Task.Run(async () => await writer.StoreAsync()).Result;
                        writer.DetachStream();
                        var oi = Task.Run(async () => await outputStream.FlushAsync()).Result;
                        outputStream.Dispose();
                        fquery1.Dispose();
                    }
                }
                else
                {
                    XDocument xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                               new XElement("Stories",
                                            new XElement("Story",
                                                new XAttribute("Id", pageno),
                                                new XElement("filename", filename),
                                                  new XElement("RecordCompleted", "false"))));

                    StorageFolder store1 = ApplicationData.Current.LocalFolder;
                    var story = Task.Run(async () => await store1.CreateFolderAsync("StoryRecordings", CreationCollisionOption.OpenIfExists)).Result;
                    var story1 = Task.Run(async () => await story.CreateFolderAsync(AppSettings.ShowID, CreationCollisionOption.OpenIfExists)).Result;
                    StorageFile file1 = Task.Run(async () => await story1.CreateFileAsync("StoryRecordings.xml", CreationCollisionOption.OpenIfExists)).Result;
                    var fquery1 = Task.Run(async () => await file1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                    var outputStream = fquery1.GetOutputStreamAt(0);
                    var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                    StringBuilder sb = new StringBuilder();
                    TextWriter tx = new StringWriter(sb);
                    xdoc.Save(tx);
                    string text = tx.ToString();
                    text = text.Replace("utf-16", "utf-8");
                    writer.WriteString(text);
                    var fi = Task.Run(async () => await writer.StoreAsync()).Result;
                    writer.DetachStream();
                    var oi = Task.Run(async () => await outputStream.FlushAsync()).Result;
                    outputStream.Dispose();
                    fquery1.Dispose();
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in savexml method In StoryReading file", ex);
                string excepmess = "Exception in savexml method In StoryReading file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }

        private void updatexml(int pageno, string filename)
        {
            try
            {
                int count = 0;
                double replaceendtime = 0;
                int difference = 0;
                XDocument xdoc = new XDocument();
                StorageFolder store = ApplicationData.Current.LocalFolder;
                var story = Task.Run(async () => await store.CreateFolderAsync("StoryRecordings", CreationCollisionOption.OpenIfExists)).Result;
                var story1 = Task.Run(async () => await story.CreateFolderAsync(AppSettings.ShowID, CreationCollisionOption.OpenIfExists)).Result;
                StorageFile file = Task.Run(async () => await story1.CreateFileAsync("StoryRecordings.xml", CreationCollisionOption.OpenIfExists)).Result;
                var f = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = f.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
                string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
                System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlData));
                xdoc = XDocument.Load(ms);
                dataReader.DetachStream();
                inputStream.Dispose();
                f.Dispose();
                ms.Dispose();
                var data = xdoc.Descendants("Stories").Elements("Story").Where(i => Convert.ToInt32(i.Attribute("Id").Value) >= pageno).OrderBy(i => Convert.ToInt32(i.Attribute("Id").Value));
                if (data != null)
                {
                    foreach (XElement itemElement in data)
                    {
                        if (count == 0)
                        {
                            //itemElement.SetElementValue("Begin", BeginTime);
                            //itemElement.SetElementValue("End", EndTime);
                            //replaceendtime = EndTime;
                            itemElement.SetElementValue("filename", filename);
                            count++;
                        }
                        else
                        {
                            itemElement.SetElementValue("filename", filename);
                            //difference = Convert.ToInt32(itemElement.Element("End").Value) - Convert.ToInt32(itemElement.Element("Begin").Value);
                            //itemElement.SetElementValue("Begin", replaceendtime);
                            //itemElement.SetElementValue("End", replaceendtime + difference);
                            //replaceendtime = replaceendtime + difference;
                        }
                    }
                }
                StorageFolder store2 = ApplicationData.Current.LocalFolder;
                var story3 = Task.Run(async () => await store2.CreateFolderAsync("StoryRecordings", CreationCollisionOption.OpenIfExists)).Result;
                var story4 = Task.Run(async () => await story3.CreateFolderAsync(AppSettings.ShowID, CreationCollisionOption.OpenIfExists)).Result;
                StorageFile file2 = Task.Run(async () => await story4.CreateFileAsync("StoryRecordings.xml", CreationCollisionOption.ReplaceExisting)).Result;
                var fquery2 = Task.Run(async () => await file2.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                var outputStream2 = fquery2.GetOutputStreamAt(0);
                var writer2 = new Windows.Storage.Streams.DataWriter(outputStream2);
                StringBuilder sb2 = new StringBuilder();
                TextWriter tx2 = new StringWriter(sb2);
                xdoc.Save(tx2);
                string text = tx2.ToString();
                text = text.Replace("utf-16", "utf-8");
                writer2.WriteString(text);
                var fi = Task.Run(async () => await writer2.StoreAsync()).Result;
                writer2.DetachStream();
                var oi = Task.Run(async () => await outputStream2.FlushAsync()).Result;
                outputStream2.Dispose();
                fquery2.Dispose();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in CreateFolder UpdateXML In StoryReadingpage.cs file", ex);
            }
        }

        private async void record_Click()
        {
            try
            {
                stopclicked = false;
                
                //if (!Storage.FileExistsInMusicLibrary(AppSettings.Title + txtpage.Text.IndexOf('/') + ".wav".Trim()))
                //{
                if (!checkxml(pagenumber))
                {
                    var isoStore = ApplicationData.Current.LocalFolder;
                    var story = Task.Run(async () => await isoStore.CreateFolderAsync("StoryRecordings", CreationCollisionOption.OpenIfExists)).Result;
                    var story1 = Task.Run(async () => await story.CreateFolderAsync(AppSettings.ShowID, CreationCollisionOption.OpenIfExists)).Result;

                    _recordStorageFile = await story1.CreateFileAsync(AppSettings.Title + pagenumber + ".wav".Trim(), CreationCollisionOption.ReplaceExisting);

                    MediaEncodingProfile recordProfile = MediaEncodingProfile.CreateWav(AudioEncodingQuality.Auto);

                    await _mediaCaptureManager.StartRecordToStorageFileAsync(recordProfile, this._recordStorageFile);

                    recstarted = true;

                    recordclicked = true;
                }
                else
                {
                    var isoStore = ApplicationData.Current.LocalFolder;
                    var story = Task.Run(async () => await isoStore.CreateFolderAsync("StoryRecordings", CreationCollisionOption.OpenIfExists)).Result;
                    var story1 = Task.Run(async () => await story.CreateFolderAsync(AppSettings.ShowID, CreationCollisionOption.OpenIfExists)).Result;

                    _recordStorageFile = await story1.CreateFileAsync(AppSettings.Title + pagenumber + ".wav".Trim(), CreationCollisionOption.ReplaceExisting);

                    MediaEncodingProfile recordProfile = MediaEncodingProfile.CreateWav(AudioEncodingQuality.Auto);

                    await _mediaCaptureManager.StartRecordToStorageFileAsync(recordProfile, this._recordStorageFile);

                    recstarted = true;

                    recordclicked = true;
                }
                //}
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in record_Click() method In StoryReading file", ex);
                string excepmess = "Exception in record_Click() method In StoryReading file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }

        public bool checkxml(int pagenumbercheck)
        {
            bool exists = false;
            try
            {
                if (Storage.FileExistsInMusicLibrary("StoryRecordings.xml"))
                {
                    XDocument xdoc = new XDocument();
                    StorageFolder store = ApplicationData.Current.LocalFolder;
                    var story = Task.Run(async () => await store.CreateFolderAsync("StoryRecordings", CreationCollisionOption.OpenIfExists)).Result;
                    var story1 = Task.Run(async () => await story.CreateFolderAsync(AppSettings.ShowID, CreationCollisionOption.OpenIfExists)).Result;
                    StorageFile file = Task.Run(async () => await story1.CreateFileAsync("StoryRecordings.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                    var f = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                    IInputStream inputStream = f.GetInputStreamAt(0);
                    DataReader dataReader = new DataReader(inputStream);
                    uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
                    string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlData));
                    xdoc = XDocument.Load(ms);
                    dataReader.DetachStream();
                    inputStream.Dispose();
                    f.Dispose();
                    ms.Dispose();
                    var data = (from p in xdoc.Descendants("Stories").Elements() where p.Attribute("Id").Value == pagenumbercheck.ToString() select p).ToList();
                    if (data.Count() > 0)
                        exists = true;
                }
            }
            catch (Exception ex)
            {

            }
            return exists;
        }

        private void LayoutRoot_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if (slidestatus > 0)
                {
                    if (Constants.mode == "Listen")
                    {

                        me.Stop();
                        me.Source = null;

                    }
                    if (Constants.mode != "Listenmp3")
                    {
                        slidestatus = StoryReadingExperience.storystackmanipulationCompleted(this.imgstory, this.txtDescription, this.txtpage, pagenumber, this.ContentPanel, Constants.mode == "Listen" ? this.me : null, buttontapped);
                        this.messagestk.Visibility = Visibility.Collapsed;
                    }

                    if (Constants.mode == "Listenmp3")
                    {

                        me.Stop();
                        me.Source = null;
                        slidestatus = StoryReadingExperience.storystackmanipulationCompleted(this.imgstory, this.txtDescription, this.txtpage, pagenumber, this.ContentPanel, Constants.mode == "Listenmp3" ? this.me : null, buttontapped);
                        this.messagestk.Visibility = Visibility.Collapsed;
                    }
                    if (Constants.mode == "Rec")
                    {
                        buttonpressed = false;
                        showstack();
                    }

                }
                else
                {
                    if (Constants.mode != "Read")
                    {
                        buttonpressed = false;
                        showstack();
                        //if (Constants.mode == "Listen")
                        //{
                        //    DispatcherTimer buttonTimer = new DispatcherTimer();
                        //    buttonTimer.Interval = TimeSpan.FromSeconds(10);
                        //    buttonTimer.Tick += new EventHandler(buttonTimer_Tick);
                        //    buttonTimer.Start();
                        //}
                    }

                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Doubletapped method In StoryReading file", ex);
            }
        }

        private void me_MediaEnded(object sender, RoutedEventArgs e)
        {
            XDocument xdoc = new XDocument();
            StorageFolder store = ApplicationData.Current.LocalFolder;
            var story = Task.Run(async () => await store.CreateFolderAsync("StoryRecordings", CreationCollisionOption.OpenIfExists)).Result;
            var story1 = Task.Run(async () => await story.CreateFolderAsync(AppSettings.ShowID, CreationCollisionOption.OpenIfExists)).Result;
            StorageFile file = Task.Run(async () => await story1.CreateFileAsync("StoryRecordings.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
            var f = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
            IInputStream inputStream = f.GetInputStreamAt(0);
            DataReader dataReader = new DataReader(inputStream);
            uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
            string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
            System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlData));
            xdoc = XDocument.Load(ms);
            dataReader.DetachStream();
            inputStream.Dispose();
            f.Dispose();
            ms.Dispose();
            var data = from f1 in xdoc.Descendants("Stories").Elements("Story")
                       where (Convert.ToInt32(f1.Attribute("Id").Value)) == pagenumber + 1
                       select f1;
            if (data.Count() > 0)
            {
                try
                {
                    StorageFolder store1 = ApplicationData.Current.LocalFolder;
                    var story11 = Task.Run(async () => await store1.CreateFolderAsync("StoryRecordings", CreationCollisionOption.OpenIfExists)).Result;
                    var story111 = Task.Run(async () => await story11.CreateFolderAsync(AppSettings.ShowID, CreationCollisionOption.OpenIfExists)).Result;
                    int num = pagenumber + 1;
                    var stream = Task.Run(async () => await story111.GetFileAsync(AppSettings.Title + num.ToString() + ".wav".Trim())).Result;
                    LayoutRoot_ManipulationDelta(null, null);
                    LayoutRoot_ManipulationCompleted(null, null);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
