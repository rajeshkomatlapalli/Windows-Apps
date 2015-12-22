using Common;
using Common.Library;
using Common.Utilities;
using Online_Education;
using OnlineMovies.Views;
using OnlineVideos;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.Library;
using OnlineVideos.UserControls;
using OnlineVideos.Utilities;
using OnlineVideos.ViewModels;
using OnlineVideos.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.Media.SpeechRecognition;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OnlineVideos
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region GlobalDeclaration
        AppInsights insights = new AppInsights();
        string Ins_Event = string.Empty;
        DispatcherTimer dt = new DispatcherTimer();
        ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
        public ObservableCollection<ShowList> objtoprated = null;
        ObservableCollection<ShowList> objRecent = null;
        ObservableCollection<ShowList> objtamil = null;
        ObservableCollection<ShowList> objupcoming = null;
        Dictionary<string, Uri> Navigation;
        SpeechRecognizer speechRecognizer;
        string link;
        public ValueSet message { get; set; }
        private MediaPlayer _mediaPlayer;
        BackgroundHelper bwHelper1 = new BackgroundHelper();

        string[] parameters = new string[2];
        #endregion
        public MainPage()
        {
            this.InitializeComponent();
            RununderLockscreen();
            InitializeUI();
            HideStatusBar();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            insights.pageview("MainPage");
            _mediaPlayer = BackgroundMediaPlayer.Current;
            base.OnNavigatedTo(e);
            try
            {
                if (AppSettings.ProjectName == "Indian Cinema.WindowsPhone" || AppSettings.ProjectName == "Indian Cinema Pro")
                {
                    tblkToprated.Text = "hindi";
                    tblkrecent.Text = "telugu";
                    hubsection_tamil.Visibility = Visibility.Visible;
                    hubsection_upcomming.Visibility = Visibility.Visible;
                }
                else
                {
                    tblkToprated.Text = "top rated";
                    tblkrecent.Text = "recent";
                    hubsection_tamil.Visibility = Visibility.Collapsed;
                    hubsection_upcomming.Visibility = Visibility.Collapsed;
                }
                if (UtilitiesResources.ShowAdRotator)
                {
                    Border frameBorder = (Border)VisualTreeHelper.GetChild(Window.Current.Content, 0);

                }
                if (AppSettings.AddNewShowIconVisibility)
                {
                    string backentryid = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNaviagtedTo Method In MainPage file.", ex);
            }
        }
        public async void HideStatusBar()
        {
            var statusBar = StatusBar.GetForCurrentView();
            await statusBar.HideAsync();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Constants.UIThread = false;
            try
            {
                insights.Trace("Main Page Loaded");
                //SyncButton.Login();
                if (ResourceHelper.ProjectName == "Web Tile")
                {
                    dt.Interval = TimeSpan.FromSeconds(40);
                    dt.Tick += dt_Tick;
                    dt.Start();
                }
                SearchPanel.Visibility = Visibility.Visible;
                GetPageDataInBackground();
                if (!AppSettings.LiveTileBackgroundAgentStatus)
                {
                    GetLiveTileImagesInBackGround();
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In MainPage file.", ex);
                insights.Exception(ex);
            }
        }
        private void GetLiveTileImagesInBackGround()
        {
            BackgroundHelper bwHelper = new BackgroundHelper();
            bwHelper.AddBackgroundTask(
                                        (object s, DoWorkEventArgs a) =>
                                        {
                                            Constants.LiveTileBackgroundAgentStatus = false;
                                            LiveTileUpDate.LiveTileForCycle();
                                        },
                                        (object s, RunWorkerCompletedEventArgs a) =>
                                        {

                                        }
                                      );
            bwHelper.RunBackgroundWorkers();
        }
        private void GetPageDataInBackground()
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();

                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {

                                                a.Result = App.group;
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                objtoprated = (ObservableCollection<ShowList>)a.Result;
                                                if (objtoprated.Count != 0)
                                                {
                                                    lbxTopRated.ItemsSource = objtoprated;
                                                }
                                                else
                                                {
                                                    lbxTopRated.ItemsSource = null;
                                                    if (ResourceHelper.ProjectName == "Web Tile")
                                                        tblkToprated.Text = AppResources.TopRatedMsg;
                                                    if (ResourceHelper.AppName == Apps.Video_Mix.ToString() || ResourceHelper.AppName == Apps.Web_Tile.ToString())
                                                        tblkToprated.Visibility = Visibility.Visible;
                                                }
                                            }
                                          );

                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                a.Result = App.grouptelugu;
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                objRecent = (ObservableCollection<ShowList>)a.Result;
                                                if (objRecent.Count != 0)
                                                {
                                                    lbxRecentlyAdded.ItemsSource = objRecent;
                                                }
                                                else
                                                {
                                                    lbxRecentlyAdded.ItemsSource = null;
                                                    if (ResourceHelper.ProjectName == "Web Tile")
                                                        tblkrecent.Text = AppResources.RecentdMsg;
                                                    if (ResourceHelper.AppName == Apps.Video_Mix.ToString() || ResourceHelper.AppName == Apps.Web_Tile.ToString())
                                                        tblkrecent.Visibility = Visibility.Visible;
                                                }
                                            }
                                          );
                if (AppSettings.ProjectName == "Indian Cinema.WindowsPhone" || AppSettings.ProjectName == "Indian Cinema Pro")
                {
                    bwHelper.AddBackgroundTask(
                                               (object s, DoWorkEventArgs a) =>
                                               {
                                                   a.Result = App.grouptamil;
                                               },
                                               (object s, RunWorkerCompletedEventArgs a) =>
                                               {
                                                   objtamil = (ObservableCollection<ShowList>)a.Result;
                                                   if (objtamil.Count != 0)
                                                   {
                                                       lbxtamil.ItemsSource = objtamil;
                                                   }
                                                   else
                                                   {
                                                       lbxtamil.ItemsSource = null;
                                                   }
                                               }
                                             );
                }
                if (AppSettings.ProjectName == "Indian Cinema.WindowsPhone" || AppSettings.ProjectName == "Indian Cinema Pro")
                {
                    bwHelper.AddBackgroundTask(
                                              (object s, DoWorkEventArgs a) =>
                                              {
                                                  a.Result = App.upcomingmovies;
                                              },
                                              (object s, RunWorkerCompletedEventArgs a) =>
                                              {
                                                  objupcoming = (ObservableCollection<ShowList>)a.Result;
                                                  if (objupcoming.Count != 0)
                                                  {
                                                      lbxupcoming.ItemsSource = objupcoming;
                                                  }
                                                  else
                                                  {
                                                      lbxupcoming.ItemsSource = null;
                                                  }
                                              }
                                            );
                }
                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                a.Result = OnlineVideos.UI.MenuHelper.GetMainMenuItems((string)a.Argument);
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                List<MenuProperties> MainmenuItems = (List<MenuProperties>)a.Result;
                                                try
                                                {
                                                    if (!AppResources.ShowonlinewebTile)
                                                    {
                                                        MainmenuItems.RemoveAt(1);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    string msg = ex.Message;
                                                    MainmenuItems.RemoveAt(1);
                                                }
                                                lbxMenu.ItemsSource = MainmenuItems;
                                            },
                                            (object)AppResources.MainMenuItemShowListName
                                          );

                if (AppResources.ShowTopRatedLinks)
                {
                    bwHelper.AddBackgroundTask(
                                                (object s, DoWorkEventArgs a) =>
                                                {
                                                    a.Result = OnlineShow.GetTopRatedLinks();
                                                },
                                                (object s, RunWorkerCompletedEventArgs a) =>
                                                {
                                                    lbxSongsList.ItemsSource = (List<ShowLinks>)a.Result;
                                                }
                                              );
                }
                //if (AppResources.ShowTopRatedAudioLinks)
                //{
                //    bwHelper.AddBackgroundTask(
                //                                (object s, DoWorkEventArgs a) =>
                //                                {
                //                                    a.Result = OnlineShow.GetTopRatedAudioLinksforXml();
                //                                },
                //                                (object s, RunWorkerCompletedEventArgs a) =>
                //                                {
                //                                    lbxAudioList.ItemsSource = (List<ShowLinks>)a.Result;
                //                                }
                //                              );
                //}
                bwHelper.RunBackgroundWorkers();

                if (!Storage.IsLowMemDevice)
                {
                    if (AppResources.RegisterDownloadAgent)
                    {
                        bwHelper.AddScheduledBackgroundTask(RegisterShowDownloadAgent, TimeSpan.FromSeconds(10));
                    }
                    bwHelper.StartScheduledTasks();
                }
            }
            catch (Exception ex)
            {

            }
        }

        void dt_Tick(object sender, object e)
        {
            BackGroundCheck bc = new BackGroundCheck();
            AddReminder ar = new AddReminder();
            object[] ob = new object[2];
            ob[0] = ar;
            ob[1] = bc;
            ar.timer = dt;
            bc.timer = dt;
            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += bg_DoWork;
            bg.RunWorkerAsync(ob);
        }

        void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] ob = (object[])e.Argument;
            ((BackGroundCheck)ob[1]).CheckData();
            ((AddReminder)ob[0]).Add();
        }

        void RegisterShowDownloadAgent(object sender, object e)
        {
            try
            {
                RegisterAgent objbackagent = new RegisterAgent();
                objbackagent.backagent();
                (sender as DispatcherTimer).Stop();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowDownloadAgentTimer_Tick Method In MainPage file.", ex);
                insights.Exception(ex);
            }
        }

        private void LaunchSearchPage()
        {
            if (tbxSearch.Text != "")
            {
                Frame.Navigate(typeof(Search), tbxSearch.Text.ToLower());
                SearchManager.SaveSearchHistoryIntoDatabase(tbxSearch.Text.ToLower());
            }
            else
            {

            }
        }

        private async void RununderLockscreen()
        {
            bool AppLevelState = true;
            if (settings.Containers.ContainsKey("AppLevelState"))
                AppLevelState = Convert.ToBoolean(settings.Containers["AppLevelState"]);
            if (settings.Containers.ContainsKey("runUnderLock"))
                ApplicationIdleModeHelper.Current.RunsUnderLock = SettingsHelper.getBoolValue("runUnderLock");

            if (AppLevelState == true)
            {
                MessageDialog resultLocService = new MessageDialog("In order to deliver location tracking services and relevent Ads for your location, application needs permission to use the location services on the device.\n\n\n Do you want to allow application to use location service?", "Location Service");

                resultLocService.Commands.Add(new UICommand("Ok", new UICommandInvokedHandler(this.TriggerThisFunctionForOk)));
                resultLocService.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler(this.TriggerThisFunctionForCancel)));
                var res = await resultLocService.ShowAsync();
            }
        }

        private void TriggerThisFunctionForCancel(IUICommand command)
        {
            SettingsHelper.Save("GeoLocationStatus", "false");
            SettingsHelper.Save("AppLevelState", "false");
        }

        private async void TriggerThisFunctionForOk(IUICommand command)
        {
            SettingsHelper.Save("GeoLocationStatus", "true");
            MessageDialog resultRunUnderLock = new MessageDialog("In order to provide background audio/video playback and download new links in the background, application needs permission to run under the phone lock screen. \n\n\n Do you want to allow application to run under lock screen?", "Run Under Lock Screen");

            resultRunUnderLock.Commands.Add(new UICommand("Ok", new UICommandInvokedHandler(this.TriggerThisFunctionForOk1)));
            resultRunUnderLock.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler(this.TriggerThisFunctionForCancel1)));
            var res = await resultRunUnderLock.ShowAsync();
        }

        private void TriggerThisFunctionForCancel1(IUICommand command)
        {
            ApplicationIdleModeHelper.Current.RunsUnderLock = false;
            SettingsHelper.Save("runUnderLock", "false");
        }

        private void TriggerThisFunctionForOk1(IUICommand command)
        {
            ApplicationIdleModeHelper.Current.HasUserAgreedToRunUnderLock = true;
            SettingsHelper.Save("runUnderLock", "true");
            ApplicationIdleModeHelper.Current.RunsUnderLock = true;
        }

        private void GetPageDataSynchronously()
        {
            lbxTopRated.ItemsSource = OnlineShow.GetTopRatedShows().Items;
            lbxRecentlyAdded.ItemsSource = OnlineShow.GetRecentlyAddedShows().Items;
            lbxMenu.ItemsSource = OnlineVideos.UI.MenuHelper.GetMainMenuItems(AppResources.MainMenuItemShowListName);
            if (AppResources.ShowTopRatedLinks)
            {
                lbxSongsList.ItemsSource = OnlineShow.GetTopRatedLinks();
            }
        }

        private void InitializeUI()
        {
            if (AppResources.ShowTopRatedLinks && AppResources.ShowDetailPageAudioPivot)
            {
                hubsection_Topsongs.Visibility = Visibility.Visible;
                tblkMusicHeader.Text = "top music";
                hubsection_TopAudio.Visibility = Visibility.Visible;
                tblkHeader.Text = AppResources.TopRatedLinksTitle;
                AppSettings.musicvisibility = hubsection_TopAudio.Visibility.ToString();
            }
            else if (!AppResources.ShowTopRatedLinks && AppResources.ShowDetailPageAudioPivot)
            {
                if (AppSettings.AddNewShowIconVisibility)
                {
                    hubsection_Topsongs.Visibility = Visibility.Collapsed;
                    hubsection_TopAudio.Visibility = Visibility.Collapsed;
                }
                else
                {
                    hubsection_Topsongs.Visibility = Visibility.Collapsed;
                    tblkMusicHeader.Text = "top music";
                    hubsection_TopAudio.Visibility = Visibility.Visible;
                }
            }
            else
            {
                hubsection_Topsongs.Visibility = Visibility.Collapsed;
                hubsection_TopAudio.Visibility = Visibility.Collapsed;
            }

            if (AppSettings.AddNewShowIconVisibility)
            {
                downloadimg.Visibility = Visibility;
            }
            if (AppResources.ShowTopRatedLinks)
                hubsection_Topsongs.Visibility = Visibility.Visible;
            else
                hubsection_Topsongs.Visibility = Visibility.Collapsed;
        }

#endregion
        private void LoadAudioSongd(string title)
        {
            int Showid = Convert.ToInt32(AppSettings.ShowID);
            string linktpe = LinkType.Audio.ToString();
            var ShowLinksByType = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == Showid && i.LinkType == linktpe).ToListAsync()).Result;
            foreach (var linkinfo in ShowLinksByType)
            {
                if (linkinfo.Title == title)
                {
                    message = new ValueSet
                    {
                        {
                            "Play",
                            linkinfo.LinkUrl
                        }
                    };
                    BackgroundMediaPlayer.SendMessageToBackground(message);
                    BackgroundMediaPlayer.Current.CurrentStateChanged += Current_CurrentStateChanged;
                    AppSettings.AudioShowID = AppSettings.ShowID;
                }
                else
                {
                    BackgroundMediaPlayer.Current.Pause();
                    AppSettings.SongPlayImage = string.Empty;
                }
            }
        }

        void Current_CurrentStateChanged(MediaPlayer sender, object args)
        {
            MediaPlayerState playState = BackgroundMediaPlayer.Current.CurrentState;
            switch (playState)
            {
                case MediaPlayerState.Paused:
                    IEnumerable<DependencyObject> cboxes = PageHelper.GetChildsRecursive(lbxRecentlyAdded);
                    foreach (DependencyObject obj in cboxes.OfType<ProgressBar>())
                    {
                        Type type = obj.GetType();
                        if (type.Name == "PerformanceProgressBar")
                        {
                            ProgressBar cb = obj as ProgressBar;
                            if (cb.Tag.ToString() == AppSettings.ShowLinkTitle)
                            {
                                cb.IsIndeterminate = false;
                            }
                        }
                    }
                    break;

                case MediaPlayerState.Playing:

                    IEnumerable<DependencyObject> cboxes1 = PageHelper.GetChildsRecursive(lbxRecentlyAdded);
                    foreach (DependencyObject obj in cboxes1.OfType<ProgressBar>())
                    {
                        Type type = obj.GetType();
                        if (type.Name == "PerformanceProgressBar")
                        {
                            ProgressBar cb = obj as ProgressBar;
                            if (cb.Tag.ToString() == AppSettings.ShowLinkTitle)
                            {
                                cb.IsIndeterminate = false;
                            }
                        }
                    }
                    break;
            }
        }

        private void tbxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.VirtualKey.ToString() == "Enter")
            {
                LaunchSearchPage();
            }
        }

        private void lbxSongsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxSongsList.SelectedIndex == -1)
                    return;
                AppState.searchtitle = (lbxSongsList.SelectedItem as ShowLinks).Title;
                AppSettings.ShowID = (lbxSongsList.SelectedItem as ShowLinks).ShowID.ToString();
                Ins_Event = AppSettings.LinkType + " " + AppState.searchtitle + " " + "Viewed";
                insights.Event(Ins_Event);
                Constants.topsongnavigation = "3";
                parameters[0] = AppSettings.ShowID;
                parameters[1] = null;
                Frame.Navigate(typeof(SubjectDetail), parameters);
                lbxSongsList.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxSongsList_SelectionChanged event In MainPage file.", ex);
            }
        }

        private void lbxAudioList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxAudioList.SelectedIndex == -1)
                return;
            AppSettings.ShowID = (lbxAudioList.SelectedItem as ShowLinks).ShowID.ToString();
            AppState.searchtitle = (lbxAudioList.SelectedItem as ShowLinks).Title.ToString();
            Ins_Event = AppState.searchtitle + " " + "played";
            insights.Event(Ins_Event);
            if (ResourceHelper.AppName == Apps.Bollywood_Music.ToString())
                Constants.topsongnavigation = "1";
            else
                Constants.topsongnavigation = "4";

            parameters[0] = AppSettings.ShowID;
            parameters[1] = null;
            Frame.Navigate(typeof(SubjectDetail), parameters);
            lbxAudioList.SelectedIndex = -1;
        }

        private void lbxTopRated_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListView MainPageListBox = sender as ListView;

                if (MainPageListBox.SelectedIndex == -1)
                    return;
                AppSettings.ShowID = (MainPageListBox.SelectedItem as ShowList).ShowID.ToString();
                AppSettings.Title = (MainPageListBox.SelectedItem as ShowList).Title;
                Ins_Event = tblkToprated.Text + " " + AppSettings.Title + " " + "Viewed";
                insights.Event(Ins_Event);
                lbxRecentlyAdded_SelectionChanged(sender, e);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxTopRated_SelectionChanged event In MainPage file.", ex);
            }
        }

        private void lbxRecentlyAdded_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListView MainPageListBox = sender as ListView;

                if (MainPageListBox.SelectedIndex == -1)
                    return;
                if ((MainPageListBox.SelectedItem as ShowList).Rating != null)
                    AppSettings.ShowRating = (double)Math.Round(Convert.ToDecimal((MainPageListBox.SelectedItem as ShowList).Rating));
                AppSettings.ShowID = (MainPageListBox.SelectedItem as ShowList).ShowID.ToString();
                AppSettings.Title = (MainPageListBox.SelectedItem as ShowList).Title;
                Ins_Event = tblkrecent.Text + " " + AppSettings.Title + "viewded";
                insights.Event(Ins_Event);
                AppState.searchtitle = "";
                string ext = System.IO.Path.GetExtension((MainPageListBox.SelectedItem as ShowList).Title);
                if (ext == ".3gp" || ext == ".3g2" || ext == ".mp4" || ext == ".m4v" || ext == ".wmv" || (MainPageListBox.SelectedItem as ShowList).TileImage == "videos.jpg")
                {

                }
                else if (ext == ".mp3" || ext == ".wav" || ext == ".aac" || ext == ".amr" || ext == ".wma")
                {
                    LoadAudioSongd((MainPageListBox.SelectedItem as ShowList).Title);
                }
                else if (ext == ".jpg" || ext == ".png")
                {
                    PageHelper.NavigateToDownloadedImagePage(AppResources.DownloadImagePageName);
                }
                else
                {
                    parameters[0] = AppSettings.ShowID;
                    parameters[1] = null;
                    Frame.Navigate(typeof(SubjectDetail), parameters);
                }
                MainPageListBox.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxRecentlyAdded_SelectionChanged event In MainPage file.", ex);
                insights.Exception(ex);
            }
        }

        private void lbxtamil_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView MainPageListBox = sender as ListView;

            if (MainPageListBox.SelectedIndex == -1)
                return;
            AppSettings.ShowID = (MainPageListBox.SelectedItem as ShowList).ShowID.ToString();
            AppSettings.Title = (MainPageListBox.SelectedItem as ShowList).Title;
            Ins_Event = "Tamil" + " " + AppSettings.Title + " " + "Viewed";
            insights.Event(Ins_Event);
            lbxRecentlyAdded_SelectionChanged(sender, e);
        }

        private void lbxupcoming_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView MainPageListBox = sender as ListView;

            if (MainPageListBox.SelectedIndex == -1)
                return;
            AppSettings.ShowID = (MainPageListBox.SelectedItem as ShowList).ShowID.ToString();
            AppSettings.Title = (MainPageListBox.SelectedItem as ShowList).Title;
            Ins_Event = "upcoming movies" + " " + AppSettings.Title + " " + "Viewed";
            insights.Event(Ins_Event);
            lbxRecentlyAdded_SelectionChanged(sender, e);
        }

        private async void microphone_Tapped(object sender, TappedRoutedEventArgs e) //It is an "async" menthod,Add async after private keyword.............
        {
            try
            {
                SpeechRecognizer recognizer = new SpeechRecognizer();

                SpeechRecognitionTopicConstraint topicConstraint = new SpeechRecognitionTopicConstraint(SpeechRecognitionScenario.Dictation, "Cities");

                recognizer.Constraints.Add(topicConstraint);
                await recognizer.CompileConstraintsAsync(); // Required

                // Open the UI.
                var recognitionResult = await recognizer.RecognizeWithUIAsync();
                if (recognitionResult.Confidence != SpeechRecognitionConfidence.Rejected)
                {
                    tbxSearch.Text = recognitionResult.Text.Replace(".", string.Empty);
                    string searchText = tbxSearch.Text;
                    if (searchText != string.Empty)
                    {
                        List<ShowList> showLinks = SearchManager.GetShowsBySearch(searchText);
                        foreach (ShowList objLinks in showLinks)
                        {
                            if (tbxSearch.Text == objLinks.Title)
                            {
                                AppSettings.ShowID = (objLinks.ShowID).ToString();
                                Frame.Navigate(typeof(SubjectDetail), AppSettings.ShowID);
                            }
                        }
                        string SearchText = tbxSearch.Text.Replace(".", string.Empty);
                        if (SearchText == "Favorites")
                        {
                            Frame.Navigate(typeof(SubjectsHistory));
                        }
                        if (SearchText == "Movie list" || SearchText == "vedic library" || SearchText == "yoga shows")
                        {
                            //NavigationService.Navigate(new Uri("/Views/ShowList.xaml", UriKind.Relative));
                        }
                        if (SearchText == "History")
                        {
                            Frame.Navigate(typeof(SubjectsHistory));
                        }
                        if (SearchText == "Help")
                        {
                            Frame.Navigate(typeof(Help));
                        }
                        if (recognitionResult.Confidence != SpeechRecognitionConfidence.Rejected)
                        {
                            Frame.Navigate(typeof(Search), tbxSearch.Text);
                        }
                    }
                    else
                    {
                        recognizer.Dispose();
                    }
                }
                else
                {
                    recognizer.Dispose();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        private void findbtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            LaunchSearchPage();
        }

        private void lbxMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxMenu.SelectedIndex == -1)
                    return;

                switch ((lbxMenu.SelectedItem as MenuProperties).Id)
                {
                    case "1":
                        Frame.Navigate(typeof(Shows), (lbxMenu.SelectedItem as MenuProperties).Name);
                        break;
                    case "2":
                        PageHelper.NavigateToOnlineWebTile(AppResources.OnlinewebTilePageName);
                        break;
                    case "3":
                        string name = AppResources.FavoritePageName;
                        var dd = AppResources.FavoritePageName;
                        Frame.Navigate(typeof(Favoritesubjects));
                        break;
                    case "4":
                        string name1 = AppResources.HistoryPageName;
                        Frame.Navigate(typeof(SubjectsHistory));
                        break;
                    case "5":
                        Frame.Navigate(typeof(HelpMenu));
                        break;
                    case "6":
                        Frame.Navigate(typeof(Settings));
                        break;
                }

                lbxMenu.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxMenu_SelectionChanged event In MainPage file.", ex);
                insights.Exception(ex);
            }
        }

        private void downloadimg_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ResourceHelper.ProjectName == AppResources.VideoMix)
            {
                Constants.UpdatePlayListTitleAndDescrption = false;
            }
            else
            {
                Frame.Navigate(typeof(Browser)); // To be Modified................................................
            }
        }

        private void playfav_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Constants.YoutubePlayList = new Dictionary<string, string>();
            var Itemcollection = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.IsFavourite == true && i.LinkType == "Songs").ToListAsync()).Result;
            foreach (var item in Itemcollection)
            {
                if (!Constants.YoutubePlayList.ContainsKey(item.LinkUrl))
                    Constants.YoutubePlayList.Add(item.LinkUrl, item.Title);
            }
            if (Constants.YoutubePlayList.Count > 0)
            {
                if (ResourceHelper.AppName == Apps.Indian_Cinema_Pro.ToString() || ResourceHelper.AppName == Apps.Kids_TV_Pro.ToString() || ResourceHelper.AppName == Apps.Story_Time_Pro.ToString())
                    AppSettings.startplayingforpro = true;
                else
                    AppSettings.startplaying = true;
                AppSettings.LinkUrl = Constants.YoutubePlayList.FirstOrDefault().Key;
                AppSettings.LinkTitle = Constants.YoutubePlayList.FirstOrDefault().Value;
                Constants.YoutubePlayList.Remove(Constants.YoutubePlayList.FirstOrDefault().Key);
                AppSettings.PlayVideoTitle = Constants.YoutubePlayList.FirstOrDefault().Value;

                Frame.Navigate(typeof(Youtube), AppSettings.LinkUrl);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
