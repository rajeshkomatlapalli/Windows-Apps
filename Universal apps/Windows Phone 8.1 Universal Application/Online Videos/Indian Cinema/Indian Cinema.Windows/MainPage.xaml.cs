using AdRotator;
using Bollywood_Music.Views;
using Common.Library;
using Microsoft.ApplicationInsights;
using Mvvm;
using Mvvm.Services.Sound;
using Online_Education.Views;
using OnlineVideos.Entities;
using OnlineVideos.Library;
using OnlineVideos.Views;
using SyncStories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Search;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.ApplicationSettings;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OnlineVideos
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Global Declaration
        AppInsights insights = new AppInsights();        
        private SearchPane searchPane;
        bool check = false;
        private Popup settingsPopup;
        // Used to determine the correct height to ensure our custom UI fills the screen.
        private Rect windowBounds;
        const int ContentAnimationOffset = 100;
        // Desired width for the settings UI. UI guidelines specify this should be 346 or 646 depending on your needs.
        private double settingsWidth = 646;
        private SearchPane _searchPane;
        bool searchitem = false;

        private DataTransferManager dataTransferManager;
        private StorageFile imageFile;
        BackgroundHelper bwHelper1 = new BackgroundHelper();
        DispatcherTimer timer = new DispatcherTimer();
        public TextBlock songblock = null;
        #endregion

        #region Constructor
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
            Window.Current.SizeChanged += Current_SizeChanged;
            AppSettings.CategoryVisible = "true";
            searchPane = SearchPane.GetForCurrentView();
            checkstate();
            windowBounds = Window.Current.Bounds;
            Unloaded += MainPage_Unloaded;
        }
        #endregion

        #region Page Load
        void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (App.Adcollapased == false)
                {
                    if (App.AddUnLoaded == false)
                    {
                        if (VisualTreeHelper.GetChildrenCount(Window.Current.Content as Frame) > 0)
                        {
                            Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                            //AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
                            //adcontrol.IsAdRotatorEnabled = true;
                            //adcontrol.IsEnabled = true;
                            //adcontrol.Visibility = Visibility.Visible;
                            //adcontrol.Visibility = Visibility.Visible;
                            //adcontrol.Invalidate(true);
                            //adcontrol.Invalidate(AdRotator.Model.AdProvider);
                            App.AddUnLoaded = true;
                        }
                    }
                    else
                    {
                        Border border1 = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                        //AdRotatorControl adcontrol1 = (AdRotatorControl)border1.FindName("AdRotatorWin8");
                        //adcontrol1.IsAdRotatorEnabled = true;
                        //adcontrol1.IsEnabled = true;
                        //adcontrol1.Visibility = Visibility.Visible;
                        //adcontrol1.Visibility = Visibility.Visible;
                    }
                }
                App.Adcollapased = false;
                if (Constants.CloseLyricspopup.IsOpen == true)
                    Constants.CloseLyricspopup.IsOpen = false;
                // searchPane.SuggestionsRequested -= new TypedEventHandler<SearchPane, SearchPaneSuggestionsRequestedEventArgs>(OnSearchPaneSuggestionsRequested);
                //this.dataTransferManager.DataRequested -= new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnDataRequested);
            }
            catch (Exception ex)
            {
                //this.dataTransferManager.DataRequested -= new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnDataRequested);
            }
        }
        private void GetLiveTileImagesInBackGround()
        {
            BackgroundHelper bwHelper = new BackgroundHelper();
            //PrimaryTileUpdate primarytileupdate = new PrimaryTileUpdate();
            bwHelper.AddBackgroundTask(
                                        (object s, DoWorkEventArgs a) =>
                                        {
                                            Constants.LiveTileBackgroundAgentStatus = false;
                                            //primarytileupdate.LiveTileUpdate(ResourceHelper.ProjectName);
                                            LiveTileUpDate.LiveTileForCycle();
                                            //LiveTileUpDate.CreateOrUpDateTile();
                                        },
                                        (object s, RunWorkerCompletedEventArgs a) =>
                                        {

                                        }
                                      );
            bwHelper.RunBackgroundWorkers();
        }
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            SoundPlayer.Instance.Initialize();
            insights.Trace("MainPage loaded");
            try
            {
                if (!AppSettings.LiveTileBackgroundAgentStatus)
                {
                    GetLiveTileImagesInBackGround();
                }
                if (!Constants.isEventRegistered)
                {
                    SettingsPane.GetForCurrentView().CommandsRequested += onCommandsRequested;
                    Constants.isEventRegistered = true;
                }
                BackgroundHelper bwHelper = new BackgroundHelper();

                if (App.rootMediaElement == null)
                {
                    Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                    App.rootMediaElement = (MediaElement)border.FindName("MediaPlayer");
                }
                if (AppSettings.BackGroundAgentRegistered == false)
                {
                    bwHelper.AddScheduledBackgroundTask(RegisterShowDownloadAgent, TimeSpan.FromSeconds(10));
                }
                bwHelper.StartScheduledTasks();
                _searchPane = SearchPane.GetForCurrentView();
                if (Constants.BackgroundDownloadStatus == false)
                {
                    bwHelper1.AddScheduledBackgroundTask(StartBackgroundDownload, TimeSpan.FromSeconds(5));
                    bwHelper1.StartScheduledTasks();
                    Constants.BackgroundDownloadStatus = true;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in MainPage_Loaded event In mainpage.cs file", ex);
                insights.Exception(ex);
            }
        }
        #endregion

        #region Common Methods & Events
        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            ApplicationViewState currentState = Windows.UI.ViewManagement.ApplicationView.Value;
            if (currentState == ApplicationViewState.Snapped)
            {
                bottomAppBar.Visibility = Visibility.Collapsed;
            }
            else
            {
                bottomAppBar.Visibility = Visibility.Visible;
                Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                //AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
                //adcontrol.IsAdRotatorEnabled = false;
                //adcontrol.Visibility = Visibility.Collapsed;
            }
        }

        private void checkstate()
        {
            ApplicationViewState currentState = Windows.UI.ViewManagement.ApplicationView.Value;
            if (currentState == ApplicationViewState.Snapped)
            {
                bottomAppBar.Visibility = Visibility.Collapsed;
            }
            else
            {
                bottomAppBar.Visibility = Visibility.Visible;
            }
        }

        public void musintoplyricpopup()
        {
            //Lyricspopup.Visibility = Visibility.Visible;
        }

        public void PhotoChooser()
        {
            App.rootFrame.Navigate(typeof(PhotoChooser));
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }
        public void DetailPage()
        {
            if (LayoutRoot.Children.Count < 2)
                ResetPageCache();
            App.rootFrame.Navigate(typeof(Detail));
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }
        public void VideoListPage()
        {
            App.rootFrame.Navigate(typeof(VideoList));
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }

        private void StartBackgroundDownload(object sender, object e)
        {
            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += bg_DoWork;
            bg.RunWorkerAsync();
        }

        async void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            switch (AppSettings.BackgroundAgentStatus)
            {
                case SyncAgentStatus.DownloadFavourites:
                    if (AppSettings.DownloadFavCompleted == false && AppSettings.SkyDriveLogin == true)
                    {
                        await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            bwHelper1.StopScheduledTasks(0);
                        });
                        RestoreFav();
                        AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadFavourites;
                        await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            bwHelper1.StartScheduledTasks(0);
                        });
                    }
                    else
                        AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadFavourites;
                    break;
                case SyncAgentStatus.UploadFavourites:
                    if (AppSettings.SkyDriveLogin == true && Task.Run(async () => await Storage.FavouriteFileExists("Favourites.xml")).Result)
                    {
                        await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            bwHelper1.StopScheduledTasks(0);
                        });
                        UploadFav();
                        AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadParentalControlPreferences;
                        await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            bwHelper1.StartScheduledTasks(0);
                        });
                    }
                    else
                        AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadParentalControlPreferences;
                    break;
                case SyncAgentStatus.RestoreStory:
                    if (AppSettings.DownloadStoryCompleted == false && AppSettings.SkyDriveLogin == true && (AppSettings.ProjectName == "Story Time" || AppSettings.ProjectName == "Vedic Library"))
                    {
                        await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            bwHelper1.StopScheduledTasks(0);
                        });
                        RestoreStory restore = new RestoreStory();
                        //if (AppSettings.ProjectName == "Story Time")
                        restore.RestoreFolder("StoryRecordings");
                        //else
                        //    restore.RestoreFolder("VedicRecordings");

                        await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadStory;
                            bwHelper1.StartScheduledTasks(0);
                        });
                    }
                    else
                        AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadStory;
                    break;
                case SyncAgentStatus.UploadStory:
                    if (AppSettings.SkyDriveLogin == true && (AppSettings.ProjectName == "Story Time" || AppSettings.ProjectName == "Vedic Library"))
                    {
                        await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            bwHelper1.StopScheduledTasks(0);
                        });
                        UploadStory st = new UploadStory();
                        //if (AppSettings.ProjectName == "Story Time")
                        st.CreateFolder("StoryRecordings");
                        //else
                        //    st.CreateFolder("VedicRecordings");

                        await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadFavourites;
                            bwHelper1.StartScheduledTasks(0);
                        });
                    }
                    else
                        AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadFavourites;
                    break;
                default:
                    ShowDownloader.StartBackgroundDownload(bwHelper1, this);
                    break;
            }
        }

        private void RestoreFav()
        {
            try
            {
                if (AppSettings.IsNewVersion)
                {
                    RestoreStory restore = new RestoreStory();
                    restore.RestorefavFolder(AppSettings.ProjectName);
                }
            }
            catch (Exception ex)
            {
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadFavourites;
            }
        }

        private void UploadFav()
        {
            try
            {
                if (Task.Run(async () => await Storage.FavFileExists("Favourites.xml")).Result)
                {
                    UploadStory upload = new UploadStory();
                    upload.CreateFolderForFav(AppSettings.ProjectName);
                }
            }
            catch (Exception ex)
            {
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadParentalControlPreferences;
            }
        }

        private void RegisterShowDownloadAgent(object sender, object e)
        {
            try
            {
                RegisterTimeTriggerBackgroundTask();
                (sender as DispatcherTimer).Stop();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowDownloadAgentTimer_Tick Method In MainPage file.", ex);
                insights.Exception(ex);
            }
        }

        private void RegisterTimeTriggerBackgroundTask()
        {
            try
            {
                foreach (var cur in BackgroundTaskRegistration.AllTasks)
                {
                    if (cur.Value.Name == "TileUpdate")
                    {
                        cur.Value.Unregister(true);
                    }
                }
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
                BackgroundTaskRegistration task = default(BackgroundTaskRegistration);
                BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
                builder.Name = "TileUpdate";
                builder.TaskEntryPoint = "Tasks.SampleBackgroundTask";
                IBackgroundTrigger trigger = new SystemTrigger(SystemTriggerType.InternetAvailable, false);
                IBackgroundCondition condition = new SystemCondition(SystemConditionType.UserPresent);
                builder.AddCondition(condition);
                builder.SetTrigger(trigger);
                task = builder.Register();
                AppSettings.BackGroundAgentRegistered = true;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in RegisterTimeTriggerBackgroundTask Method In mainpage.cs file", ex);
                insights.Exception(ex);
            }
        }

        private void onCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            UICommandInvokedHandler handler = new UICommandInvokedHandler(onSettingsCommand);
            UICommandInvokedHandler handler1 = new UICommandInvokedHandler(onPrivacyCommand);
            UICommandInvokedHandler handler2 = new UICommandInvokedHandler(onAccountCommand);

            SettingsCommand generalCommand = new SettingsCommand("DefaultsId", "App Settings", handler);
            SettingsCommand generalCommand1 = new SettingsCommand("DefaultsId", "Privacy Statement", handler1);
            SettingsCommand generalCommand2 = new SettingsCommand("DefaultsId", "Cloud Login", handler2);

            args.Request.ApplicationCommands.Add(generalCommand);
            args.Request.ApplicationCommands.Add(generalCommand1);
            args.Request.ApplicationCommands.Add(generalCommand2);
        }

        private void onAccountCommand(IUICommand command)
        {
            settingsPopup = new Popup();
            settingsPopup.Closed += OnPopupClosed;
            Window.Current.Activated += OnWindowActivated;
            settingsPopup.IsLightDismissEnabled = true;
            settingsPopup.Width = settingsWidth;
            settingsPopup.Height = windowBounds.Height;
            settingsPopup.ChildTransitions = new TransitionCollection();
            settingsPopup.ChildTransitions.Add(new PaneThemeTransition()
            {
                Edge = (SettingsPane.Edge == SettingsEdgeLocation.Right) ?
                       EdgeTransitionLocation.Right :
                       EdgeTransitionLocation.Left
            });

            SyncButton mypane = new SyncButton();
            mypane.Width = settingsWidth;
            mypane.Height = windowBounds.Height;
            settingsPopup.Child = mypane;
            settingsPopup.SetValue(Canvas.LeftProperty, SettingsPane.Edge == SettingsEdgeLocation.Right ? (windowBounds.Width - settingsWidth) : 0);
            settingsPopup.SetValue(Canvas.TopProperty, 0);
            settingsPopup.IsOpen = true;
        }

        private void onPrivacyCommand(IUICommand command)
        {
            // Create a Popup window which will contain our flyout.
            settingsPopup = new Popup();
            settingsPopup.Closed += OnPopupClosed;
            Window.Current.Activated += OnWindowActivated;
            settingsPopup.IsLightDismissEnabled = true;
            settingsPopup.Width = settingsWidth;
            settingsPopup.Height = windowBounds.Height;

            // Add the proper animation for the panel.
            settingsPopup.ChildTransitions = new TransitionCollection();
            settingsPopup.ChildTransitions.Add(new PaneThemeTransition()
            {
                Edge = (SettingsPane.Edge == SettingsEdgeLocation.Right) ?
                       EdgeTransitionLocation.Right :
                       EdgeTransitionLocation.Left
            });

            // Create a SettingsFlyout the same dimenssions as the Popup.
            PrivacyStatement mypane = new PrivacyStatement();
            mypane.Width = settingsWidth;
            mypane.Height = windowBounds.Height;

            // Place the SettingsFlyout inside our Popup window.
            settingsPopup.Child = mypane;

            // Let's define the location of our Popup.
            settingsPopup.SetValue(Canvas.LeftProperty, SettingsPane.Edge == SettingsEdgeLocation.Right ? (windowBounds.Width - settingsWidth) : 0);
            settingsPopup.SetValue(Canvas.TopProperty, 0);
            settingsPopup.IsOpen = true;
        }

        private void OnWindowActivated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
            {
                settingsPopup.IsOpen = false;
            }
        }

        private void OnPopupClosed(object sender, object e)
        {
            Window.Current.Activated -= OnWindowActivated;
        }

        private void onSettingsCommand(IUICommand command)
        {
            settingsPopup = new Popup();
            settingsPopup.Closed += OnPopupClosed;
            Window.Current.Activated += OnWindowActivated;
            settingsPopup.IsLightDismissEnabled = true;
            settingsPopup.Width = settingsWidth;
            settingsPopup.Height = windowBounds.Height;

            // Add the proper animation for the panel.
            settingsPopup.ChildTransitions = new TransitionCollection();
            settingsPopup.ChildTransitions.Add(new PaneThemeTransition()
            {
                Edge = (SettingsPane.Edge == SettingsEdgeLocation.Right) ?
                       EdgeTransitionLocation.Right :
                       EdgeTransitionLocation.Left
            });

            // Create a SettingsFlyout the same dimenssions as the Popup.
            SettingPage mypane = new SettingPage();
            mypane.Width = settingsWidth;
            mypane.Height = 700;
            mypane.Width = settingsWidth;
            mypane.Height = windowBounds.Height;

            // Place the SettingsFlyout inside our Popup window.
            settingsPopup.Child = mypane;
            //setting26.Visibility = Visibility.Visible;
            // Let's define the location of our Popup.
            settingsPopup.SetValue(Canvas.LeftProperty, SettingsPane.Edge == SettingsEdgeLocation.Right ? (windowBounds.Width - settingsWidth) : 0);
            settingsPopup.SetValue(Canvas.TopProperty, 0);
            settingsPopup.IsOpen = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            insights.pageview("Mainpage");
            try
            {
                //Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                //AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
                //adcontrol.IsAdRotatorEnabled = false;
                //adcontrol.Visibility = Visibility.Collapsed;
                // searchPane.SuggestionsRequested += new TypedEventHandler<SearchPane, SearchPaneSuggestionsRequestedEventArgs>(OnSearchPaneSuggestionsRequested);
                this.dataTransferManager = DataTransferManager.GetForCurrentView();
                //this.dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnDataRequested);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In MainPage file.", ex);
                insights.Exception(ex);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {

        }

        private void ResetPageCache()
        {
            var cacheSize = Frame.CacheSize;
            Frame.CacheSize = 0;
            Frame.CacheSize = cacheSize;
        }

        private async void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"Assets\Logo.Png", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                IRandomAccessStream displayStream = await file.OpenAsync(FileAccessMode.Read);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(displayStream);
                this.imageFile = file;
                string dataPackageTitle = "Indian Cinema App";
                if (!String.IsNullOrEmpty(dataPackageTitle))
                {
                    string dataPackageText = "I have recently found a very good entertainment app that I would like to share with you, Get the app at ";
                    if (!String.IsNullOrEmpty(dataPackageText))
                    {
                        DataPackage requestData = e.Request.Data;
                        requestData.Properties.Title = dataPackageTitle;
                        string dataPackageDescription = "I have recently found a very good entertainment app that I would like to share with you, Get the app at ";
                        if (dataPackageDescription != null)
                        {
                            requestData.Properties.Description = dataPackageText;
                        }
                        requestData.SetText(dataPackageText);
                        requestData.SetText(dataPackageTitle);
                        if (imageFile != null)
                        {
                            List<IStorageItem> imageItems = new List<IStorageItem>();
                            imageItems.Add(this.imageFile);
                            requestData.SetStorageItems(imageItems);
                            string s = imageItems.ToString();
                            RandomAccessStreamReference imageStreamRef = RandomAccessStreamReference.CreateFromFile(this.imageFile);
                            requestData.Properties.Thumbnail = imageStreamRef;
                            requestData.SetBitmap(imageStreamRef);
                        }
                        else
                        {
                            e.Request.FailWithDisplayText("Enter the text you would like to share and try again.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnDataRequested Method In MainPage file.", ex);
                insights.Exception(ex);
            }
        }
        #endregion

        #region Events
        private void searchbutton_Click(object sender, RoutedEventArgs e)
        {
            insights.Event(AppSettings.Title + "Movie Search");
            searchitem = true;
            _searchPane.Show();
        }

        private void Fav_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                insights.Event(AppSettings.Title + "Movie Favorite View");
                App.rootFrame.Navigate(typeof(MoviesFavorite));
                Window.Current.Content = App.rootFrame;
                Window.Current.Activate();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Fav_Click event In MainPage file.", ex);
                insights.Exception(ex);
            }
        }

        private void palyallfav_Click(object sender, RoutedEventArgs e)
        {
            XDocument xdoc = new XDocument();
            Constants.YoutubePlayList = new Dictionary<string, string>();
            if (Task.Run(async () => await Storage.FavFileExists("Favourites.xml")).Result)
            {
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

                foreach (var fk in xdoc.Descendants("Favourites").FirstOrDefault().Elements("Show"))
                {
                    foreach (var bb in fk.Elements("Songs").Where(i => i.Value == "1"))
                    {
                        List<ShowLinks> objLinkList = new List<ShowLinks>();
                        int showid = int.Parse(fk.Attribute("id").Value);
                        int linkorder = int.Parse(bb.Attribute("no").Value);
                        objLinkList = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkOrder == linkorder && i.LinkType == "Songs").OrderBy(j => j.LinkOrder).ToListAsync()).Result;
                        foreach (var item in objLinkList)
                        {
                            Constants.YoutubePlayList.Add(item.LinkUrl, item.Title);
                        }
                    }
                }
            }
            //var Itemcollection = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.IsFavourite == true && i.LinkType == "Songs").ToListAsync()).Result;
            //foreach (var item in Itemcollection)
            //{
            //    Constants.YoutubePlayList.Add(item.LinkUrl, item.Title);
            //}
            if (Constants.YoutubePlayList.Count > 0)
            {
                AppSettings.LinkUrl = Constants.YoutubePlayList.FirstOrDefault().Key;
                //AppSettings.PlayVideoTitle = Constants.YoutubePlayList.FirstOrDefault().Value;
                Constants.YoutubePlayList.Remove(Constants.YoutubePlayList.FirstOrDefault().Key);
                AppSettings.PlayVideoTitle = Constants.YoutubePlayList.FirstOrDefault().Value;
                Youtube();
            }
        }

        private void Youtube()
        {
            App.rootFrame.Navigate(typeof(Youtube));
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.rootFrame.Navigate(typeof(MoviesHistory));
                Window.Current.Content = App.rootFrame;
                Window.Current.Activate();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in History_Click event In MainPage file.", ex);
                insights.Exception(ex);
            }
        }

        private void share_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                insights.Event(AppSettings.Title + "Movie Share View");
                DataTransferManager.ShowShareUI();
                ShareSourceLoad();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Share_Click event In MainPage file.", ex);
                insights.Exception(ex);
            }
        }

        private void ShareSourceLoad()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.DataRequested);
        }

        void DataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;
            request.Data.Properties.Title = "Indian Cinema App";
            request.Data.Properties.Description = "I have recently found a very good entertainment app that I would like to share with you, Get the app at";
            request.Data.SetText("I have recently found a very good entertainment app that I would like to share with you, Get the app at http://apps.microsoft.com/windows/en-US/app/indian-cinema/c091e512-46e8-4fbb-8f28-157057fdcecb");
        }

        private void addshow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Frame.Navigate(typeof(UserUpload_New));
                //Viewbox vb = new Viewbox();
                //vb.Name = "popupviewbox";
                //vb.Margin = new Thickness(0, 20, 0, 20);
                //UserUpload upload = new UserUpload();
                //upload.Tag = showlist.Tag;
                //PopupManager.CopyControl(this.showlistgrd, this.bottomAppBar, "popupviewbox", this.LayoutRoot);
                //vb.Child = upload;
                //LayoutRoot.Children.Add(vb);
                //PopupManager.DisableControls();
                //Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                //AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
                //adcontrol.IsAdRotatorEnabled = false;
                //adcontrol.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                string exc = ex.Message;
            }
        }

        private async void Rating_Click(object sender, RoutedEventArgs e)
        {
            insights.Event(AppSettings.Title + "Movie Rating View");
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:REVIEW?PFN=4379LARTSOFT.IndianCinema_bhgh8tx796sj4"));
        }

        private void feedback_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                insights.Event(AppSettings.Title + "Movie feedback View");
                App.rootFrame.Navigate(typeof(OnlineVideos.Views.ContactUs));
                Window.Current.Content = App.rootFrame;
                Window.Current.Activate();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in feedback_Click event In MainPage file.", ex);
                insights.Exception(ex);
            }
        }

        private void help_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                insights.Event(AppSettings.Title + "Movie help View");
                App.rootFrame.Navigate(typeof(OnlineVideos.Views.HelpMenu));
                Window.Current.Content = App.rootFrame;
                Window.Current.Activate();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in help_Click event In MainPage file.", ex);
                insights.Exception(ex);
            }
        } 
        #endregion
    }
}
