using AdRotator;
using Common.Library;
using Indian_Cinema.Views;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.ApplicationSettings;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GameControl = OnlineVideosWin8.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public class valuess : INotifyPropertyChanged
    {

        private bool _Status;
        public bool Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
                OnPropertyChanged("Status");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }

    public sealed partial class Detail : Page
    {
        valuess buttonvisibility = new valuess();
        ComboBox pickr = default(ComboBox);
        Popup pop = new Popup();
        bool check = false;
        Popup ringpop = new Popup();
        Popup ratingpopup = new Popup();

        GameControl.RingtoneEditor ring = new GameControl.RingtoneEditor();
        public DispatcherTimer storyboardtimer;
        public DispatcherTimer appbartimer;
        public static Detail current;
        public TextBlock songblock = null;
        DataTransferManager dataTransferManager;
        public string ShowID = string.Empty;
        public string TileImage = string.Empty;
        public static string title = string.Empty;
        public static string image = null;
        public string favoriteTitle = string.Empty;
        public static bool onnavigate = false;
        public bool BackKeyPressed = false;
        string selecteditem = string.Empty;
        private StorageFile imageFile;
        DispatcherTimer timer = new DispatcherTimer();
        BackgroundHelper bwHelper1 = new BackgroundHelper();
        string ShowId = AppSettings.ShowID;
        ShowList objvideos = new ShowList();
        private DownloadOperation activeDownload;
        private Rect windowBounds;
        Popup DwnloadPop = new Popup();
        public bool RatingPopupIsOpen = false;
        public Detail()
        {
            try
            {
            this.InitializeComponent();
                //App.rootMediaElement.MediaOpened += rootMediaElement_MediaOpened;

                gamecontrol.Loaded += gamecontrol_Loaded;
                Constants.ringtoneinstance = this;
                current = this;
                //songblock = this.songtitle;
                Loaded += Detail_Loaded;
                this.DataContext = buttonvisibility;
                Window.Current.SizeChanged += Current_SizeChanged;
                checkstate();
                AppSettings.YoutubeID = "0";
                //tblkTitle.Text = OnlineShow.GetVideoDetail(AppSettings.ShowID).Title;
                //AppSettings.MovieTitle = tblkTitle.Text;
                Unloaded += Detail_Unloaded;
                AppSettings.MovieRateShowStatus = false;
            }
            catch (Exception ex)
            {
            }
        }

        void Detail_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                App.AdCollapasedPageNavigation = true;

                if (Constants.CloseLyricspopup.IsOpen == true)
                    Constants.CloseLyricspopup.IsOpen = false;
                this.dataTransferManager.DataRequested -= new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnDataRequested);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Detail_Unloaded  Method In Detailpage", ex);
            }
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            ApplicationViewState currentState = Windows.UI.ViewManagement.ApplicationView.Value;
            if (currentState == ApplicationViewState.Snapped)
            {
                App.Adcollapased = true;
                FullScreenLandscape.Visibility = Visibility.Collapsed;
                snapscrollview.Visibility = Visibility.Visible;
                Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
                adcontrol.IsAdRotatorEnabled = false;
                adcontrol.Visibility = Visibility.Collapsed;
                BottomAppBar.Visibility = Visibility.Collapsed;
                TopAppBar.Visibility = Visibility.Collapsed;
                AddControlvisablesnap.Visibility = Visibility.Visible;
                SnapViewAdRotatorWin8.Invalidate();
            }
            else
            {
                FullScreenLandscape.Visibility = Visibility.Visible;
                snapscrollview.Visibility = Visibility.Collapsed;
                Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
                adcontrol.IsAdRotatorEnabled = true;
                adcontrol.Visibility = Visibility.Visible;
                BottomAppBar.Visibility = Visibility.Visible;
                TopAppBar.Visibility = Visibility.Visible;
                AddControlvisablesnap.Visibility = Visibility.Collapsed;
            }
        }

        void gamecontrol_Loaded(object sender, RoutedEventArgs e)
        {
            addgame();
        }

        private void checkstate()
        {
            ApplicationViewState currentState = Windows.UI.ViewManagement.ApplicationView.Value;
            if (currentState == ApplicationViewState.Snapped)
            {
                SnapViewAdRotatorWin8.Invalidate();
                App.Adcollapased = true;
                FullScreenLandscape.Visibility = Visibility.Collapsed;
                snapscrollview.Visibility = Visibility.Visible;
                BottomAppBar.Visibility = Visibility.Collapsed;
                TopAppBar.Visibility = Visibility.Collapsed;
                Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
                adcontrol.IsAdRotatorEnabled = false;
                adcontrol.Visibility = Visibility.Collapsed;

            }
            else
            {
                FullScreenLandscape.Visibility = Visibility.Visible;
                snapscrollview.Visibility = Visibility.Collapsed;
                BottomAppBar.Visibility = Visibility.Visible;
                TopAppBar.Visibility = Visibility.Visible;
                Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
                adcontrol.IsAdRotatorEnabled = true;
                adcontrol.Visibility = Visibility.Visible;

            }
        }

        void Detail_Loaded(object sender, RoutedEventArgs e)
        {
            int showid = AppSettings.ShowUniqueID;
            tblkTitle.Text = OnlineShow.GetVideoDetail(AppSettings.ShowID).Title;
            AppSettings.MovieTitle = tblkTitle.Text;
            if (ContentPanel.Children.Count < 2)
            {

                if (Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync().Result.Status != "Custom")
                {
                    buttonvisibility.Status = false;

                }
                else
                {
                    buttonvisibility.Status = true;
                }
                //string sharedstatus = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result.ShareStatus;
                //if (sharedstatus == "Shared To Blog")
                //    ((ApplicationBarMenuItem)ApplicationBar.MenuItems[0]).Text = "share this" + " " + AppResources.Type + "(" + "Shared)";
                //else
                //    ((ApplicationBarMenuItem)ApplicationBar.MenuItems[0]).Text = "share this" + " " + AppResources.Type + "(" + "Not Shared)";
                Window.Current.CoreWindow.PointerWheelChanged += CoreWindow_PointerWheelChanged;
                checkstate();
                App.AdCollapasedPageNavigation = false;
                AdRotatorWin8.Invalidate();
                SettingsPane.GetForCurrentView().CommandsRequested += onCommandsRequested;
                onnavigate = false;
                if (Constants.scrollwidth != 0 && Constants.ScrollPosition != 0)
                {
                    //this.IsEnabled = false;
                }
                if (ShowCastManager.ShowGameControl(AppSettings.ShowID))
                {
                    gamecontrol.Loaded -= gamecontrol_Loaded;
                    int index = detailgrid.Children.IndexOf(GameGrid);
                    if (index > 0)
                        detailgrid.Children.RemoveAt(index);
                }

                //BackButton.Visibility = Visibility.Visible;
                //Detail page = new Detail();
                //page.Visibility = Visibility.Visible;

                try
                {

                    if (this.dataTransferManager == default(DataTransferManager))
                    {
                        this.dataTransferManager = DataTransferManager.GetForCurrentView();
                        this.dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnDataRequested);
                    }
                }
                catch (Exception ex)
                {
                    this.dataTransferManager.DataRequested -= new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnDataRequested);
                }

            }
        }

        private void CoreWindow_PointerWheelChanged(CoreWindow sender, PointerEventArgs args)
        {
            if (args.CurrentPoint.Properties.MouseWheelDelta == (-120))
            {
                //MouseWheel Backward scroll
                FullScreenLandscape.ScrollToHorizontalOffset(FullScreenLandscape.HorizontalOffset + Window.Current.CoreWindow.Bounds.Width / 10);
            }
            if (args.CurrentPoint.Properties.MouseWheelDelta == (120))
            {
                //MouseWheel Forward scroll
                FullScreenLandscape.ScrollToHorizontalOffset(FullScreenLandscape.HorizontalOffset - Window.Current.CoreWindow.Bounds.Width / 10);

            }
        }

        private void onCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            AddControlvisable1.Visibility = Visibility.Collapsed;
        }

        public void addgame()
        {
            storyboardtimer = new DispatcherTimer();
            storyboardtimer.Interval = TimeSpan.FromSeconds(6);
            storyboardtimer.Tick += storyboardtimer_Tick;
            storyboardtimer.Start();
        }

        void storyboardtimer_Tick(object sender, object e)
        {

            if (OnlineVideos.Game.GameInstance.gameopened == false)
            {
                foreach (OnlineVideos.Game.MemoryCard m in OnlineVideos.Game.GameInstance.stopstoryboard)
                {
                    m.storyboardfromx = "3";
                    m.storyboardtox = "-3";
                    m.storyboardfromy = "3";
                    m.storyboardtoy = "-3";
                    m.storyboardfromz = "3";
                    m.storyboardtoz = "-3";
                }
                storyboardtimer.Stop();
            }


        }
        public void CastPanorama()
        {
            App.rootFrame.Navigate(typeof(CastPanorama));
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }
        public void DownLoadVideoHelp()
        {
            DwnloadPop.Height = 400;
            DwnloadPop.Width = 400;
            DwnloadPop.Margin = new Thickness(220, 70, 0, 55);
            DownLoadVideoPopup DLVP = new DownLoadVideoPopup();
            DwnloadPop.Child = DLVP;
            DwnloadPop.IsOpen = true;

        }
        public void Youtube()
        {
            App.rootFrame.Navigate(typeof(Youtube));
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }

        public bool checklink(string linktype)
        {
            bool exists = false;
            try
            {
                if (Task.Run(async () => await Storage.FavFileExists("Favourites.xml")).Result)
                {
                    int sid = int.Parse(AppSettings.ShowID);
                    XDocument xdoc = new XDocument();
                    StorageFolder store = ApplicationData.Current.LocalFolder;

                    StorageFolder file = Task.Run(async () => await store.CreateFolderAsync(AppSettings.ProjectName, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                    StorageFile file1 = Task.Run(async () => await file.CreateFileAsync("Favourites.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                    var f = Task.Run(async () => await file1.OpenAsync(FileAccessMode.Read)).Result;
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



                    var data = xdoc.Root.Elements("Show").Where(i => i.Attribute("id").Value == AppSettings.ShowID.ToString() && i.Element(linktype) != null).Elements(linktype).Where(i => i.Attribute("no").Value == ((Constants.selecteditem == null) ? Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(j => j.ShowID == sid && j.LinkType == "Movies").FirstOrDefaultAsync()).Result.LinkOrder.ToString() : Constants.selecteditem.LinkOrder.ToString())).ToList();
                    if (data.Count() > 0)
                    {
                        if (data[0].Value == "1")
                            exists = true;

                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Checklink  Method In Detailpage", ex);
            }
            return exists;

        }
        public void changetext(string linktype)
        {

            if (checklink(linktype))
            {
                btnfav.Style = (Application.Current.Resources["FavoriteAppBarButtonStyle1"] as Style);

            }
            else
            {
                btnfav.Style = (Application.Current.Resources["FavoriteAppBarButtonStyle"] as Style);
            }
        }
        public void appbar(bool value)
        {
            AddControlvisable1.Visibility = Visibility.Visible;
            //AddControlvisable.Visibility = Visibility.Collapsed;
            BottomAppBar.IsOpen = value;
            TopAppBar.IsOpen = value;

            if (Constants.AppbaritemVisbility == "visbil")
            {
                BottomAppBar.Visibility = Visibility.Visible;
                Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
                adcontrol.IsAdRotatorEnabled = false;
                adcontrol.Visibility = Visibility.Collapsed;
                if (buttonvisibility.Status == true)
                {
                    btnonlineshare.Visibility = Visibility.Visible;
                    btneditdes.Visibility = Visibility.Visible;
                    Share.Visibility = Visibility.Collapsed;
                    Homeappbar.Visibility = Visibility.Visible;
                    btnreportbroken.Visibility = Visibility.Collapsed;
                    btnpin.Visibility = Visibility.Collapsed;
                    btnfav.Visibility = Visibility.Collapsed;
                    btncast.Visibility = Visibility.Visible;
                    btnsongs.Visibility = Visibility.Visible;
                    btnaudios.Visibility = Visibility.Visible;
                    btndeletecast.Visibility = Visibility.Collapsed;
                    ringtone.Visibility = Visibility.Collapsed;
                    btnRatetheVideo.Visibility = Visibility.Collapsed;
                    btnRatetheshow.Visibility = Visibility.Collapsed;
                    btnRatetheaudio.Visibility = Visibility.Collapsed;
                    btndellink.Visibility = Visibility.Collapsed;
                    btnvideos.Visibility = Visibility.Visible;

                }
                else
                {
                    btnonlineshare.Visibility = Visibility.Collapsed;
                    btneditdes.Visibility = Visibility.Collapsed;
                    Share.Visibility = Visibility.Visible;
                    Homeappbar.Visibility = Visibility.Visible;
                    btnreportbroken.Visibility = Visibility.Collapsed;
                    btnpin.Visibility = Visibility.Visible;
                    btnfav.Visibility = Visibility.Visible;
                    btncast.Visibility = Visibility.Collapsed;
                    btnsongs.Visibility = Visibility.Collapsed;
                    btnaudios.Visibility = Visibility.Collapsed;
                    btndeletecast.Visibility = Visibility.Collapsed;
                    ringtone.Visibility = Visibility.Collapsed;
                    btnRatetheVideo.Visibility = Visibility.Collapsed;
                    btnRatetheshow.Visibility = Visibility.Visible;
                    btnRatetheaudio.Visibility = Visibility.Collapsed;
                    btndellink.Visibility = Visibility.Collapsed;
                    btnvideos.Visibility = Visibility.Collapsed;
                }

                Constants.AppbaritemVisbility = "not visbil";


            }
            else
            {
                if (AppSettings.LinkType == LinkType.Movies.ToString())
                {
                    BottomAppBar.Visibility = Visibility.Visible;
                    if (buttonvisibility.Status == true)
                    {
                        btndellink.Visibility = Visibility.Visible;
                        btnvideos.Visibility = Visibility.Visible;

                    }
                    else
                    {
                        btndellink.Visibility = Visibility.Collapsed;
                        btnvideos.Visibility = Visibility.Collapsed;
                    }
                    Share.Visibility = Visibility.Visible;
                    Homeappbar.Visibility = Visibility.Visible;
                    btnreportbroken.Visibility = Visibility.Visible;
                    btnpin.Visibility = Visibility.Visible;
                    btnfav.Visibility = Visibility.Collapsed;
                    btneditdes.Visibility = Visibility.Collapsed;
                    btncast.Visibility = Visibility.Collapsed;
                    btnsongs.Visibility = Visibility.Collapsed;
                    btnaudios.Visibility = Visibility.Collapsed;
                    btndeletecast.Visibility = Visibility.Collapsed;
                    btnonlineshare.Visibility = Visibility.Collapsed;
                    ringtone.Visibility = Visibility.Collapsed;
                    btnRatetheVideo.Visibility = Visibility.Collapsed;
                    btnRatetheshow.Visibility = Visibility.Visible;
                    btnRatetheaudio.Visibility = Visibility.Collapsed;
                    Constants.AppbaritemVisbility = "not visbil";

                }
                else if (AppSettings.LinkType == LinkType.Songs.ToString())
                {
                    BottomAppBar.Visibility = Visibility.Visible;
                    if (buttonvisibility.Status == true)
                    {
                        btndellink.Visibility = Visibility.Visible;
                        btnsongs.Visibility = Visibility.Visible;

                    }
                    else
                    {
                        btndellink.Visibility = Visibility.Collapsed;
                        btnsongs.Visibility = Visibility.Collapsed;
                    }
                    Share.Visibility = Visibility.Visible;
                    Homeappbar.Visibility = Visibility.Visible;
                    btnreportbroken.Visibility = Visibility.Visible;
                    btnpin.Visibility = Visibility.Visible;
                    btnfav.Visibility = Visibility.Visible;
                    btneditdes.Visibility = Visibility.Collapsed;
                    btncast.Visibility = Visibility.Collapsed;
                    btnvideos.Visibility = Visibility.Collapsed;
                    btnaudios.Visibility = Visibility.Collapsed;
                    btndeletecast.Visibility = Visibility.Collapsed;
                    btnonlineshare.Visibility = Visibility.Collapsed;
                    ringtone.Visibility = Visibility.Collapsed;
                    btnRatetheVideo.Visibility = Visibility.Visible;
                    btnRatetheshow.Visibility = Visibility.Collapsed;
                    btnRatetheaudio.Visibility = Visibility.Collapsed;
                    Constants.AppbaritemVisbility = "not visbil";

                }
                else if (AppSettings.LinkType == LinkType.Audio.ToString())
                {
                    BottomAppBar.Visibility = Visibility.Visible;
                    if (buttonvisibility.Status == true)
                    {
                        btndellink.Visibility = Visibility.Visible;
                        btnaudios.Visibility = Visibility.Visible;

                    }
                    else
                    {
                        btndellink.Visibility = Visibility.Collapsed;
                        btnaudios.Visibility = Visibility.Collapsed;
                    }
                    Share.Visibility = Visibility.Visible;
                    Homeappbar.Visibility = Visibility.Visible;
                    btnreportbroken.Visibility = Visibility.Visible;
                    btnpin.Visibility = Visibility.Visible;
                    btnfav.Visibility = Visibility.Visible;
                    btneditdes.Visibility = Visibility.Collapsed;
                    btncast.Visibility = Visibility.Collapsed;
                    btnvideos.Visibility = Visibility.Collapsed;
                    btnsongs.Visibility = Visibility.Collapsed;
                    btndeletecast.Visibility = Visibility.Collapsed;
                    btnonlineshare.Visibility = Visibility.Collapsed;
                    ringtone.Visibility = Visibility.Visible;
                    btnRatetheVideo.Visibility = Visibility.Collapsed;
                    btnRatetheshow.Visibility = Visibility.Collapsed;
                    btnRatetheaudio.Visibility = Visibility.Visible;
                    Constants.AppbaritemVisbility = "not visbil";

                }
                else
                {

                    if (buttonvisibility.Status == true)
                    {
                        btndeletecast.Visibility = Visibility.Visible;
                        btncast.Visibility = Visibility.Visible;
                        btnaudios.Visibility = Visibility.Collapsed;
                        Share.Visibility = Visibility.Collapsed;
                        btneditdes.Visibility = Visibility.Collapsed;
                        BottomAppBar.Visibility = Visibility.Visible;
                        btnRatetheVideo.Visibility = Visibility.Collapsed;
                        btnRatetheshow.Visibility = Visibility.Collapsed;
                        btnRatetheaudio.Visibility = Visibility.Collapsed;
                        ringtone.Visibility = Visibility.Collapsed;
                        btnonlineshare.Visibility = Visibility.Collapsed;
                        btndellink.Visibility = Visibility.Collapsed;
                        btnreportbroken.Visibility = Visibility.Collapsed;
                        btnpin.Visibility = Visibility.Collapsed;
                        btnfav.Visibility = Visibility.Collapsed;
                        btnvideos.Visibility = Visibility.Collapsed;
                        btnsongs.Visibility = Visibility.Collapsed;
                    }
                    else
                    {

                        BottomAppBar.Visibility = Visibility.Collapsed;
                    }
                    Homeappbar.Visibility = Visibility.Visible;
                    btnreportbroken.Visibility = Visibility.Collapsed;
                    Share.Visibility = Visibility.Collapsed;
                    Constants.AppbaritemVisbility = "not visbil";
                }
            }

            if (value == true)
            {
                StorageFolder sf = Task.Run(async () => await KnownFolders.VideosLibrary.GetFolderAsync(AppSettings.ProjectName)).Result;
                IReadOnlyList<StorageFile> file1 = Task.Run(async () => await sf.GetFilesAsync()).Result;
                string CompareTitle = string.Empty;
                string MovieTitleandSongTitle = string.Empty;
                MovieTitleandSongTitle = AppSettings.MovieTitle + "_" + AppSettings.Title + ".mp4";
                foreach (StorageFile t in file1.ToList())
                {

                    if (MovieTitleandSongTitle == t.Name)
                    {
                        CompareTitle = t.Name;
                    }

                }

                var sf1 = Windows.Storage.ApplicationData.Current.LocalFolder;
                IReadOnlyList<StorageFile> file2 = Task.Run(async () => await sf1.GetFilesAsync()).Result;

                foreach (StorageFile t in file2.ToList())
                {
                    string s = t.Name;
                    bool Comp = s.EndsWith(".mp4");

                }
            }
            appbartimer = new DispatcherTimer();
            appbartimer.Interval = TimeSpan.FromSeconds(6);
            appbartimer.Tick += appbartimer_Tick;
            appbartimer.Start();

        }
        void appbartimer_Tick(object sender, object e)
        {
            BottomAppBar.IsOpen = false;
            TopAppBar.IsOpen = false;
            appbartimer.Stop();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (App.rootFrame.BackStackDepth > 2)
            {
                Constants.scrollwidth = 0;
                Constants.ScrollPosition = 0;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Frame f = App.rootFrame;

            if (BackKeyPressed == false)
            {
                Constants.scrollwidth = FullScreenLandscape.ExtentWidth;
                Constants.ScrollPosition = FullScreenLandscape.HorizontalOffset;
                onnavigate = true;
            }
        }

        public void ResetPageCache()
        {
            var cacheSize = Frame.CacheSize;
            Frame.CacheSize = 0;
            Frame.CacheSize = cacheSize;
        }
        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataRequestDeferral def = e.Request.GetDeferral();
            ShowList showlist = new ShowList();
            showlist = OnlineShow.GetVideoDetail(AppSettings.ShowID);

            string dataPackageTitle = showlist.Title;

            if (!String.IsNullOrEmpty(dataPackageTitle))
            {
                DataPackage requestData = e.Request.Data;
                requestData.Properties.Title = dataPackageTitle;
                requestData.SetText(dataPackageTitle);

                StorageFolder store = Package.Current.InstalledLocation;
                BitmapImage bitmapImage = new BitmapImage();

                this.imageFile = null;

                if (imageFile == null)
                {


                    string dataPackageDescription = showlist.Description;
                    if (dataPackageDescription != null)
                    {
                        requestData.Properties.Description = dataPackageDescription;

                        requestData.SetText(dataPackageDescription);

                    }

                    // It's recommended to use both SetBitmap and SetStorageItems for sharing a single image
                    // since the target app may only support one or the other.
                    List<IStorageItem> imageItems = new List<IStorageItem>();
                    imageItems.Add(this.imageFile);

                    def.Complete();
                }
            }
        }

        private void Homeappbar_Click(object sender, RoutedEventArgs e)
        {
            if (RatingPopupIsOpen == true)
            {
                RatingpopUp rp = RatingpopUp.current;
                rp.close();

            }
            if (App.rootFrame.CanGoBack)
            {
                if (ContentPanel.Children.Count < 2)
                    ResetPageCache();
                App.rootFrame.GoBack();
            }
        }

        private void Share_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }

        public void SnapLyricsPopup(bool value)
        {

            App.rootFrame.Navigate(typeof(SnapLyrics));
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }

        private void btnreportbroken_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Constants.selecteditem != null)
                {

                    if (Constants.selecteditem != null)
                    {
                        Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                        AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
                        adcontrol.IsAdRotatorEnabled = false;
                        adcontrol.Visibility = Visibility.Collapsed;
                        OnlineShow.UpdateReportBrokenCount();

                        App.rootFrame.Navigate(typeof(ContactUs), "context");
                        Window.Current.Content = App.rootFrame;
                        Window.Current.Activate();
                    }

                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in feedback_Click  event In Detailpage", ex);
            }
        }

        public string ChecksmallLogo(string FileName)
        {

            if (Task.Run(async () => await Storage.FileExists("Images\\TileImages\\30-30\\" + FileName + ".jpg")).Result)
            {
                return "ms-appdata:///local/Images/TileImages/30-30/" + FileName + ".jpg";
            }
            else
            {
                return "ms-appx:///Images/TileImages/30-30/" + FileName + ".jpg";
            }
        }
        public string ChecksquareLogo(string FileName)
        {

            if (Task.Run(async () => await Storage.FileExists("Images\\TileImages\\150-150\\" + FileName + ".jpg")).Result)
            {
                return "ms-appdata:///local/Images/TileImages/150-150/" + FileName + ".jpg";
            }
            else
            {
                return "ms-appx:///Images/TileImages/150-150/" + FileName + ".jpg";
            }
        }

        public string CheckWideLogo(string FileName)
        {

            if (Task.Run(async () => await Storage.FileExists("Images\\TileImages\\310-150\\" + FileName + ".jpg")).Result)
            {
                return "ms-appdata:///local/Images/TileImages/310-150/" + FileName + ".jpg";
            }
            else
            {
                return "ms-appx:///Images/TileImages/310-150/" + FileName + ".jpg";
            }
        }

        private void BottomAppBar_Closed(object sender, object e)
        {
            if (!App.AdCollapasedPageNavigation)
            {
                Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
                adcontrol.IsAdRotatorEnabled = true;
                adcontrol.Visibility = Visibility.Visible;
            }
        }

        private void BottomAppBar_Opened(object sender, object e)
        {
            Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
            AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
            adcontrol.IsAdRotatorEnabled = false;
            adcontrol.Visibility = Visibility.Collapsed;

            // Musicstak.Visibility = Visibility.Collapsed;

            if (Constants.appbarvisible == false)
            {
                BottomAppBar.IsOpen = false;
                TopAppBar.IsOpen = false;

            }
            else
            {

                Constants.appbarvisible = false;

            }
        }

        private void Pin_Click(object sender, RoutedEventArgs e)
        {
            string abc = Constants.pintomovie;
            if (Constants.selecteditem != null || Constants.pintomovie == "ok")
            {
                string title = string.Empty;
                string SongID = string.Empty;
                if (Constants.pintomovie == "ok")
                {
                    title = tblkTitle.Text;

                }
                else
                {
                    title = Constants.selecteditem.Title;
                    SongID = Constants.selecteditem.LinkOrder.ToString();
                }

                this.BottomAppBar.IsSticky = true;
                if (!Windows.UI.StartScreen.SecondaryTile.Exists(AppSettings.ShowID + "-" + SongID))
                {
                    string imagename = AppSettings.TileImage.Substring(0, AppSettings.TileImage.LastIndexOf('.'));
                    Uri Squarelogo = new Uri(ChecksquareLogo(imagename), UriKind.RelativeOrAbsolute);
                    string tileActivationArguments = "timeTileWasPinned=" + DateTime.Now.ToLocalTime().ToString();
                    Uri Widelogo = new Uri(CheckWideLogo(imagename), UriKind.RelativeOrAbsolute);
                    Uri smalllogo = new Uri(ChecksmallLogo(imagename), UriKind.RelativeOrAbsolute);
                    SecondaryTile secondaryTile = new SecondaryTile(ShowId + "-" + SongID, title, "Indian Cinema", tileActivationArguments, TileOptions.ShowNameOnLogo, Widelogo);
                    secondaryTile.ForegroundText = ForegroundText.Dark;
                    secondaryTile.WideLogo = Widelogo;
                    secondaryTile.Logo = Squarelogo;
                    secondaryTile.SmallLogo = new Uri("ms-appx:///Images/TileImages/30-30/" + imagename + ".jpg", UriKind.RelativeOrAbsolute);
                    bool isPinned = await secondaryTile.RequestCreateAsync();
                    Constants.pintomovie = "not ok";
                }
                else
                {
                    SecondaryTile secondaryTile = new SecondaryTile(AppSettings.ShowID + "-" + SongID);
                    bool isDeleted = await secondaryTile.RequestDeleteAsync();
                    Constants.pintomovie = "not ok";
                }

                this.BottomAppBar.IsSticky = false;
                Constants.selecteditem = null;
                Constants.pintomovie = "not ok";
            }
        }

        public void apbars()
        {
            BottomAppBar.IsOpen = true;
            TopAppBar.IsOpen = true;
        }

        private void Fav_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Constants.selecteditem != null)
                {
                    FavoritesManager.SaveFavoriteSongs(Constants.selecteditem.LinkType);
                }
                else
                {
                    FavoritesManager.Savemovieasfavorite(LinkType.Movies.ToString());
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Fav_Click  event In Detailpage", ex);
            }
        }

        private void btndellink_Click_1(object sender, RoutedEventArgs e)
        {
            int showid = AppSettings.ShowUniqueID;
            int linkid = Constants.LinkID;
            string linktype = AppSettings.LinkType;
            var aa = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(id => id.ShowID == showid && id.LinkID == linkid && id.LinkType == linktype).FirstOrDefaultAsync()).Result;
            if (aa != null)
            {
                if (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid && i.Status == "Custom").FirstOrDefaultAsync()).Result != null)
                {
                    Task.Run(async () => await Constants.connection.DeleteAsync(aa));
                    if (linktype == "Movies")
                    {
                        if (Constants.selectedMovielinklist.Count() > 0)
                            Constants.selectedMovielinklist.Remove(Constants.selectedMovielinklist.Where(i => i.LinkID == linkid).FirstOrDefault());
                        if (Constants.selectedMovielinklist.Count() == 0)
                        {
                            OnlineVideosWin8.Controls.ShowChapters.currents.tblkvisible();

                        }

                    }
                    if (linktype == "Songs")
                    {
                        if (Constants.selecteditemshowlinklist.Count() > 0)
                            Constants.selecteditemshowlinklist.Remove(Constants.selecteditemshowlinklist.Where(i => i.LinkID == linkid).FirstOrDefault());
                        if (Constants.selecteditemshowlinklist.Count() == 0)
                        {

                            OnlineVideosWin8.Controls.ShowVideos.currents.tblkvisible();

                        }
                    }
                    if (linktype == "Audio")
                    {

                        OnlineVideosWin8.Controls.ShowAudio.currents.GetPageDataInBackground();
                    }
                    //if (Constants.selectedaudiolinklist.Count() > 0)
                    //    Constants.selectedaudiolinklist.Remove(Constants.selectedaudiolinklist.Where(i => i.LinkID == linkid).FirstOrDefault());
                    //if (OnlineVideosWin8.Controls.ShowAudio.currents.links.Count() == 0)
                    //    ((TextBlock)(OnlineVideosWin8.Controls.ShowAudio.currents).FindName("txtmsg")).Visibility = Visibility.Visible;  
                }

            }
        }

        private void btneditdes_Click_1(object sender, RoutedEventArgs e)
        {
            Constants.editdescription = true;
            App.rootFrame.Navigate(typeof(Description));
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }

        private void btncast_Click_1(object sender, RoutedEventArgs e)
        {
            Viewbox vb = new Viewbox();
            vb.Name = "popupviewbox";
            vb.Margin = new Thickness(0, 20, 0, 20);
            Add_Cast upload = new Add_Cast();
            upload.Tag = this.showcast;
            PopupManager.CopyControl(this.LayoutRoot, this.BottomAppBar, "popupviewbox", this.ContentPanel);
            vb.Child = upload;
            ContentPanel.Children.Add(vb);
            PopupManager.DisableControls();
            Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
            AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
            adcontrol.IsAdRotatorEnabled = false;
            adcontrol.Visibility = Visibility.Collapsed;
        }

        private void btnvideo_Click_1(object sender, RoutedEventArgs e)
        {
            Constants.Linkstype = "Movies";
            App.rootFrame.Navigate(typeof(LinksFromOnline), string.Empty);
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }

        private void btnsongs_Click_1(object sender, RoutedEventArgs e)
        {
            Constants.Linkstype = "Songs";
            App.rootFrame.Navigate(typeof(LinksFromOnline), string.Empty);
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }

        private void btnaudios_Click_1(object sender, RoutedEventArgs e)
        {
            Constants.Linkstype = "Audio";

            if (AppSettings.ProjectName == "Indian Cinema")
            {

                StackPanel messagestk = new StackPanel();
                messagestk.Background = new SolidColorBrush(Colors.Black);
                messagestk.Opacity = 0.8;
                messagestk.Margin = new Thickness(70, 130, 0, 0);
                messagestk.HorizontalAlignment = HorizontalAlignment.Center;
                messagestk.VerticalAlignment = VerticalAlignment.Center;
                messagestk.Orientation = Orientation.Vertical;
                messagestk.Height = 300;
                messagestk.Width = 300;

                TextBlock tblklinktype = new TextBlock();
                tblklinktype.Height = 60;
                tblklinktype.Width = 200;
                tblklinktype.Foreground = new SolidColorBrush(Colors.White);
                tblklinktype.Margin = new Thickness(-10, 60, 0, 0);
                tblklinktype.FontSize = 28;
                tblklinktype.Text = "Select Language";
                pickr = new ComboBox();
                pickr.Width = 170;
                pickr.Margin = new Thickness(-20, -10, 0, 0);

                pickr.Items.Add("Hindi");
                pickr.Items.Add("Telugu");
                pickr.Items.Add("Tamil");
                pickr.SelectedIndex = 0;
                StackPanel imgstck = new StackPanel();
                imgstck.Orientation = Orientation.Horizontal;
                //imgstck.Height = 100;
                //imgstck.Width = 300;
                imgstck.Margin = new Thickness(45, 30, 0, 0);
                Image saveimg = new Image();
                saveimg.Height = 48;
                saveimg.Width = 48;
                saveimg.Source = new BitmapImage(new Uri(this.BaseUri, @"/Images/Rating/appbar.check.rest.png"));
                saveimg.Tapped += saveimg_Tapped;
                Image cancelimg = new Image();
                cancelimg.Height = 48;
                cancelimg.Width = 48;
                cancelimg.Margin = new Thickness(40, 0, 0, 0);
                cancelimg.Source = new BitmapImage(new Uri(this.BaseUri, @"/Images/Rating/appbar.cancel.rest.png"));
                cancelimg.Tapped += cancelimg_Tapped;
                imgstck.Children.Add(saveimg);
                imgstck.Children.Add(cancelimg);
                messagestk.Children.Add(tblklinktype);
                messagestk.Children.Add(pickr);
                messagestk.Children.Add(imgstck);

                pop.Child = messagestk;
                pop.IsOpen = true;
                pickr.SelectionChanged += pickr_SelectionChanged;
            }
            else
            {
                App.rootFrame.Navigate(typeof(LinksFromOnline), "Hindi");
                Window.Current.Content = App.rootFrame;
                Window.Current.Activate();

            }
        }

        private void btndeletecast_Click_1(object sender, RoutedEventArgs e)
        {
            int personid = Convert.ToInt32(AppSettings.PersonID);
            int showid = AppSettings.ShowUniqueID;
            ss.ShowCast pid = Task.Run(async () => await Constants.connection.Table<ss.ShowCast>().Where(i => i.ShowID == showid && i.PersonID == personid).FirstOrDefaultAsync()).Result;
            int castcount = 0;
            var aa = Task.Run(async () => await Constants.connection.Table<ss.ShowCast>().Where(id => id.PersonID == pid.PersonID).ToListAsync()).Result.GroupBy(id => id.PersonID).OrderByDescending(id => id.Count()).Select(g => new { Count = g.Count() });
            if (aa.Count() > 0)
            {

                foreach (var itm in aa)
                {
                    castcount = itm.Count;

                    if (castcount == 1 && Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid && i.Status == "Custom").FirstOrDefaultAsync()).Result != null)
                    {
                        CastProfile ds1 = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID == pid.PersonID).FirstOrDefaultAsync()).Result;
                        Task.Run(async () => await Constants.connection.DeleteAsync(ds1));
                    }

                }
            }

            Task.Run(async () => await Constants.connection.DeleteAsync(pid));
            string FolderName = "PersonImages";
            if (Task.Run(async () => await Storage.FileExists("Images\\" + FolderName + "\\" + pid.PersonID + ".jpg")).Result)
            {
                var isoStore = Task.Run(async () => await ApplicationData.Current.LocalFolder.GetFolderAsync("Images")).Result;
                var isoStore1 = Task.Run(async () => await isoStore.GetFolderAsync(FolderName)).Result;
                //var isoStore2 = Task.Run(async () => await isoStore1.GetFolderAsync(showid.ToString())).Result;
                var isoStore3 = Task.Run(async () => await isoStore1.GetFileAsync(pid.PersonID + ".jpg")).Result;
                Task.Run(async () => await isoStore3.DeleteAsync());
            }
            OnlineVideosWin8.Controls.ShowCast.current.GetPageDataInBackground();
        }

        private void btnonlineshare_Click_1(object sender, RoutedEventArgs e)
        {
            UploadToBlog upload = new UploadToBlog(AppSettings.ShowUniqueID, page1: this);
        }

        private void Rating_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void ContentPanel_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            check = true;
            Constants.appbarvisible = true;
            Constants.pintomovie = "ok";
            Constants.AppbaritemVisbility = "visbil";
            appbar(false);
            changetext(LinkType.Movies.ToString());
            Constants.selecteditem = null;
            btnRatetheVideo.Visibility = Visibility.Collapsed;
            btnRatetheaudio.Visibility = Visibility.Collapsed;
            btnfav.Visibility = Visibility.Visible;
        }

        private void BackButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (RatingPopupIsOpen == true)
            {
                RatingpopUp rp = RatingpopUp.current;
                rp.close();

            }

            BackKeyPressed = true;
            DwnloadPop.IsOpen = false;
            try
            {
                this.dataTransferManager.DataRequested -= new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnDataRequested);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in BackButton_Click_1  event In Detailpage", ex);
            }
            if (App.rootFrame.CanGoBack)
            {
                if (ContentPanel.Children.Count < 2)
                    ResetPageCache();
                App.rootFrame.GoBack();
            }
            if (Constants.CloseLyricspopup.IsOpen == true)
                Constants.CloseLyricspopup.IsOpen = false;
        }

        private void FullScreenLandscape_ViewChanged_1(object sender, ScrollViewerViewChangedEventArgs e)
        {

        }

        void pickr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string language = (sender as ComboBox).SelectedItem.ToString();
        }

        void cancelimg_Tapped(object sender, TappedRoutedEventArgs e)
        {
            pop.IsOpen = false;
        }

        void saveimg_Tapped(object sender, TappedRoutedEventArgs e)
        {
            pop.IsOpen = false;
            App.rootFrame.Navigate(typeof(LinksFromOnline), pickr.SelectedItem.ToString());
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }
    }
}
