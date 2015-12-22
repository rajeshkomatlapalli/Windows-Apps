using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using System.Reflection;
using Microsoft.ApplicationInsights;
#if WINDOWS_PHONE_APP
using OnlineVideos.Entities;
using Windows.Phone.UI.Input;
using OnlineVideos.Views;
using Common.Library;
using System.ComponentModel;
using SyncStories;
using OnlineVideos.Library;
using OnlineVideos.Data;
#endif
#if WINDOWS_APP
using SyncStories;
using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.UI;
using OnlineVideos.Views;
using Windows.ApplicationModel.Search;
#endif

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace OnlineVideos
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
        private List<EventHandler<BackPressedEventArgs>> listOfHandlers = new List<EventHandler<BackPressedEventArgs>>();
#endif
        public static dynamic group = default(dynamic);
        public static dynamic grouptelugu = default(dynamic);
        public static dynamic grouptamil = default(dynamic);
        public static dynamic upcomingmovies = default(dynamic);
        public static ShowList webinfo = default(ShowList);
        public static WebInformation webinfotable = new WebInformation();
        AppInitialize objCustomSetting;
        ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
        DispatcherTimer timer;
        StorageFolder isoStore = ApplicationData.Current.LocalFolder;
        public static bool AdStatus = false;
        public static Frame rootFrame = null;
        //public static MediaElement rootMediaElement = default(MediaElement);
        //public static MediaElement rootMediaElement = new MediaElement();
        public static MediaElement rootMediaElement = null;
        public static bool AddLoaded = false;
        public static bool AddUnLoaded = false;
        public static bool Adcollapased = false;
        public static bool AdCollapasedPageNavigation = false;
        Stopwatch stopwatch = new Stopwatch();
        AppInsights insights = new AppInsights();

#if WINDOWS_APP
        private SearchPane _searchPane;
#endif
        bool searchitem = false;
        public static string Youtubeid = "";

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            try
            {
                this.InitializeComponent();
                this.Suspending += this.OnSuspending;
                WindowsAppInitializer.InitializeAsync();
                this.UnhandledException += App_UnhandledException;
                if(System.Diagnostics.Debugger.IsAttached)
                {
                    Application.Current.DebugSettings.EnableFrameRateCounter = true;
                }                
                AppSettings.NavigationID = false;
#if WINDOWS_PHONE_APP
                objCustomSetting = new AppInitialize();
                timer = new DispatcherTimer();
                Constants.DownloadTimer = timer;
#endif
#if WINDOWS_APP
                //Border frameBorder = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                //Popup Adc = frameBorder.FindName("pop") as Popup;
                //Adc.Opened += Adc_Opened;
#endif
            }
            catch(Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Constructor in App.xaml.cs file", ex);
            }
        }

#if WINDOWS_APP
        void Adc_Opened(object sender, object e)
        {
            DispatcherTimer timer1 = new DispatcherTimer();
            timer1.Interval = TimeSpan.FromSeconds(5);
            timer1.Tick += timer1_Tick;
            timer1.Start();
        }

        void timer1_Tick(object sender, object e)
        {
            (sender as DispatcherTimer).Stop();
            Border frameBorder = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
            Popup Adc = frameBorder.FindName("pop") as Popup;
            Adc.IsOpen = false;
        }
#endif
        void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
            else
            {
                e.Handled = true;
            }
        }

#if WINDOWS_PHONE_APP
        private void InvokingMethod(object sender, BackPressedEventArgs e)
        {
            for (int i = 0; i < listOfHandlers.Count; i++)
                listOfHandlers[i](sender, e);
        }
        public event EventHandler<BackPressedEventArgs> myBackKeyEvent
        {
            add { listOfHandlers.Add(value); }
            remove { listOfHandlers.Remove(value); }
        }
        public void AddToTop(EventHandler<BackPressedEventArgs> eventToAdd) { listOfHandlers.Insert(0, eventToAdd); }

        void StartDownloadingShows()
        {
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        async void timer_Tick(object sender, object e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerAsync();
            try
            {
                switch (AppSettings.BackgroundAgentStatus)
                {
                    case SyncAgentStatus.DownloadFavourites:
                        if (AppSettings.DownloadFavCompleted == false && AppSettings.SkyDriveLogin == true)
                        {
                            await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                            {
                                timer.Stop();
                            });
                            ReStoreFavourites reStoreFav = new ReStoreFavourites();
                            await reStoreFav.RestorefavFolder(ResourceHelper.ProjectName);
                            await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                            {
                                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadFavourites;
                                timer.Start();
                            });
                        }
                        else
                            AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadFavourites;
                        break;
                    case SyncAgentStatus.UploadFavourites:
                        bool result = Task.Run(async () => await Storage.FavouriteFileExists("Favourites.xml")).Result;
                        if (AppSettings.SkyDriveLogin == true && result)
                        {
                            await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                            {
                                timer.Stop();
                            });
                            UploadFavourites upLoad = new UploadFavourites();
                            await upLoad.CreateFolderForFav(ResourceHelper.ProjectName);
                            await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                            {
                                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadParentalControlPreferences;
                                timer.Start();
                            });
                        }
                        else
                            AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadParentalControlPreferences;
                        break;

                    case SyncAgentStatus.RestoreStory:
                        if (AppSettings.DownloadStoryCompleted == false && AppSettings.SkyDriveLogin == true && (ResourceHelper.ProjectName == "Story Time" || ResourceHelper.ProjectName == "Vedic Library"))
                        {
                            await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                            {
                                timer.Stop();
                            });
                            RestoreStory restore = new RestoreStory();
                            //if (ResourceHelper.ProjectName == "Story Time")
                            await restore.RestoreFolder("StoryRecordings");
                            //else
                            //    await restore.RestoreFolder("VedicRecordings");
                            await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                            {
                                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadStory;
                                timer.Start();
                            });
                        }
                        else
                            AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadStory;
                        break;
                    case SyncAgentStatus.UploadStory:
                        if (AppSettings.SkyDriveLogin == true && (ResourceHelper.ProjectName == "Story Time" || ResourceHelper.ProjectName == "Vedic Library"))
                        {
                            await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                            {
                                timer.Stop();
                            });
                            UploadStory st = new UploadStory();
                            //if (ResourceHelper.ProjectName == "Story Time")
                            await st.CreateFolder("StoryRecordings");
                            //else
                            //    await st.CreateFolder("VedicRecordings");

                            await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                            {
                                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadFavourites;
                                timer.Start();
                            });
                        }
                        else
                            AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadFavourites;
                        break;
                    default:
                        ShowDownloader.StartBackgroundDownload(timer);
                        break;
                }                
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception Occured", ex);
            }
        }
       
        private void pop_Opened_1(object sender, EventArgs e)
        {
            DispatcherTimer timer1 = new DispatcherTimer();
            timer1.Interval = TimeSpan.FromSeconds(5);
            timer1.Tick += timer1_Tick;
            timer1.Start();
        }

        void timer1_Tick(object sender, object e)
        {
            (sender as DispatcherTimer).Stop();
            Border frameborder = (Border)VisualTreeHelper.GetChild(Window.Current.Content, 0);
            Popup Adc = frameborder.FindName("pop") as Popup;
            Adc.IsOpen = false;
        }
#endif
        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            stopwatch = Stopwatch.StartNew();
            var properties = new Dictionary<string, string> { { "", "" } };
            var metrics = new Dictionary<string, double> { { "Duration", stopwatch.Elapsed.Seconds } };
            insights.Event("start time", properties, metrics);
#if WINDOWS_APP
            await EnsureMainPageActivatedAsync(e);
#endif
#if WINDOWS_PHONE_APP
            HardwareButtons.BackPressed += InvokingMethod;
            (App.Current as App).myBackKeyEvent += App_myBackKeyEvent;

            SQLite.SQLiteAsyncConnection conn = new SQLite.SQLiteAsyncConnection(Constants.DataBaseConnectionstringForSqlite);
            Constants.connection = conn;
            AppSettings.AppStatus = ApplicationStatus.Launching;
            AppSettings.StopTimer = "False";
            objCustomSetting.CheckElementSection();
            SyncButton.Login();
            if (AppSettings.IsNewVersion == true)
            {

                AppSettings.RatingUserName = AppResources.RatingUserName;
                AppSettings.RatingPassword = AppResources.RatingPassword;
                AppSettings.ShowsRatingBlogUrl = AppResources.ShowsRatingBlogUrl;
                if (ResourceHelper.AppName == Apps.Online_Education.ToString() || ResourceHelper.AppName == Apps.DrivingTest.ToString())
                    AppSettings.QuizRatingBlogUrl = AppResources.QuizRatingBlogUrl;
                AppSettings.LinksRatingBlogUrl = AppResources.LinksRatingBlogUrl;
                AppSettings.ShowsRatingBlogName = AppResources.ShowsRatingBlogName;
                AppSettings.LinksRatingBlogName = AppResources.LinksRatingBlogName;
                if (ResourceHelper.AppName == Apps.Online_Education.ToString() || ResourceHelper.AppName == Apps.DrivingTest.ToString())
                    AppSettings.QuizLinksRatingBlogName = AppResources.QuizLinksRatingBlogName;

            }
            Constants.UIThread = true;
            group = OnlineShow.GetTopRatedShows().Items;
            grouptelugu = OnlineShow.GetRecentlyAddedShows().Items;
            grouptamil = OnlineShow.GetTamilAddedShows().Items;
            upcomingmovies = OnlineShow.GetUpComingMovies().Items;
            Constants.UIThread = false;
            StartDownloadingShows();

            AppSettings.AppStatus = ApplicationStatus.Active;

            if (ResourceHelper.AppName == Apps.Kids_TV_Pro.ToString())
            {
                AppSettings.ShowAdControl = false;
            }
#endif
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();                
                rootFrame.Style = Resources["RootFrameStyle"] as Style;
                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter

                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

#if WINDOWS_PHONE_APP
        private void App_myBackKeyEvent(object sender, BackPressedEventArgs e)
        {
            if (!e.Handled)
            {                
                Frame rootFrame = Window.Current.Content as Frame;
                if (rootFrame.CanGoBack && rootFrame!=null)
                {
                    rootFrame.GoBack();
                    e.Handled = true;
                }
            }
        } 
#endif
#if WINDOWS_APP
        async private Task EnsureMainPageActivatedAsync(IActivatedEventArgs args)
        {
            try
            {
                SQLite.SQLiteAsyncConnection conn = new SQLite.SQLiteAsyncConnection(Constants.DataBaseConnectionstringForSqlite);
                Constants.connection = conn;
                AppSettings.YoutubeQuality = 0;
                AppSettings.ComboYoutube = 0;
                AppInitialize a = new AppInitialize();
                //Window.Current.VisibilityChanged += Current_VisibilityChanged;
                Window.Current.Activated += Current_Activated;
                DataHelper helper = new DataHelper();
                AppSettings.IsRingtone = false;
                AppSettings.ProjectName = DeviceHelp.GetAppAttribute("DisplayName").Result;
                string version = String.Empty;
                version = DeviceHelp.GetVersion("Version").Result;
                //version = "1.0.0.0";
                if (version != SettingsHelper.getStringValue("Version"))
                {
                    helper.CopyDatabase();
                    helper.DeleteImageFolder();
                    AppSettings.SaveTopSongId = "20";
                    AppSettings.SaveTopMusicId = "20";
                    helper.SaveHelp();
                    helper.SaveHelpMenu();
                    helper.SaveContactUs();
                    a.CheckElementSection();
                    helper.CopyDefaultImage();
                }
                //SyncButton.login();
                // Do not repeat app initialization when already running, just ensure that
                // the window is active
                rootFrame = Window.Current.Content as Frame;
                var page = new MainPage();
                Constants.baseUri = page.BaseUri;
                if (args.PreviousExecutionState == ApplicationExecutionState.Running)
                {
                    Window.Current.Activate();
                    return;
                }

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Create a Frame to act navigation context and navigate to the first page
                rootFrame = new Frame();
                rootFrame.Style = Resources["RootFrameStyle"] as Style;
                if (!rootFrame.Navigate(typeof(MainPage)))
                {
                    throw new Exception("Failed to create initial page");
                }

                //Window.Current.VisibilityChanged += Current_VisibilityChanged;
                // Place the frame in the current Window and ensure that it is active
                Window.Current.Content = rootFrame;
                Window.Current.Activate();
            }
            catch(Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in EnsureMainPageActivatedAsync method in app.xaml.cs page", ex);
            }
        }

        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            // Register QuerySubmitted handler for the window at window creation time and only registered once
            // so that the app can receive user queries at any time.
            try
            {
                SearchPane.GetForCurrentView().QuerySubmitted += new TypedEventHandler<SearchPane, SearchPaneQuerySubmittedEventArgs>(OnQuerySubmitted);
            }
            catch(Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnWindowCreated method in app.xaml.cs page", ex);
            }
        }

        void Current_Activated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            if (rootMediaElement != null && rootMediaElement.CurrentState == MediaElementState.Paused && Constants.IsAudioPlaying == true)
            {
                rootMediaElement.Play();
                Constants.IsAudioPlaying = false;
            }
        }
        async protected override void OnSearchActivated(Windows.ApplicationModel.Activation.SearchActivatedEventArgs args)
        {
            try
            {
                await EnsureMainPageActivatedAsync(args);
                if (args.QueryText == "")
                {
                    // navigate to landing page.
                }
                else
                {
                    searchitem = false;
                    App.rootFrame.Navigate(typeof(Search));
                    Window.Current.Content = App.rootFrame;
                    Window.Current.Activate();
                    AppSettings.SearchText = args.QueryText;
                    SearchManager.SaveSearchHistoryIntoDatabase(args.QueryText);
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void OnQuerySubmitted(SearchPane sender, SearchPaneQuerySubmittedEventArgs args)
        {
            //if (searchitem == true)
            //{
            try
            {
                searchitem = false;
                App.rootFrame.Navigate(typeof(Search));
                Window.Current.Content = App.rootFrame;
                Window.Current.Activate();
                AppSettings.SearchText = args.QueryText;
                //SearchManager.SaveSearchHistory(args.QueryText);
                SearchManager.SaveSearchHistoryIntoDatabase(args.QueryText);
            }
            catch(Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnQuerySUbmitted Event in App.xaml.cs page", ex);
            }
            //}
        }
#endif
#if WINDOWS_PHONE_APP
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            if (AppSettings.IsRingtone == true && Constants.ringtoneinstance != default(object))
            {
                ((Page)Constants.ringtoneinstance).GetType().GetTypeInfo().GetDeclaredMethod("ringtoneclose").Invoke(Constants.ringtoneinstance, null);
            }
            var deferral = e.SuspendingOperation.GetDeferral();
            // TODO: Save application state and stop any background activity
            deferral.Complete();

            //stopwatch = System.Diagnostics.Stopwatch.StartNew();
            //var properties = new Dictionary<string, string> { { "", "" } };
            //var metrics = new Dictionary<string, double> { { "", stopwatch.Elapsed.TotalMilliseconds } };
            //insights.Event("Application Exited with duration", properties, metrics);
            stopwatch.Stop();
        }
    }
}