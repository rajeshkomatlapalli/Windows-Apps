using Common.Library;
using Common.Utilities;
using OnlineMovies.Views;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.View_Models;
using PicasaMobileInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.Media.SpeechRecognition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using OnlineVideos.UserControls;
using Windows.Phone.UI.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public  partial class ShowDetails : Page
    {
        #region Global
        string navigationvalue = string.Empty;
        string pivotindex = null;
        PivotItem currentItem = default(PivotItem);
        bool appbarvisible = true;
        string background = "0";
        string chapter = string.Empty;
        public DispatcherTimer storyboardtimer;        
        Shows playerobj;
        string id;
        private MediaPlayer _mediaPlayer; 
        #endregion

        public ShowDetails()
        {
            this.InitializeComponent();
            try
            {
            if(AppSettings.ShowID != "0")
               {
                   LoadPivotThemes(AppSettings.ShowUniqueID);
                   tblkVideosTitle.Text=OnlineShow.GetShowDetail(AppSettings.ShowUniqueID).Title;
                   background="1";
               }
               Loaded +=new RoutedEventHandler(Page_Loaded);
           }
            catch(Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception In ShowDetails Method In ShowDetails.cs file",ex);
            }
        }

        private void LoadAds()
        {
            try
            {
                LoadAdds.LoadAdControl_New(MainGrid, adstasongs, 1);
            }
            catch(Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception In LoadAds Method In SongDetail File", ex);
                string excepmess = "Exception in LoadAds Method In SongDetails file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }

        private async void LoadPivotThemes(long ShowID)
        {
            try
            {
                pvtMainDetails.Background =await ShowDetail.LoadShowPivotBackground(ShowID);
                if(!ShowCastManager.ShowGamePivot(AppSettings.ShowID))
                {
                    pvtMainDetails.Items.Remove(gamepivot);
                }
                if(ResourceHelper.AppName==Apps.Fitness_Programs.ToString()|| ResourceHelper.AppName == Apps.World_Dance.ToString())
                {
                    pvtitmSongs.Header = "";
                    pvtMainDetails.Items.Remove(pvtitmCast);
                }
                if(ResourceHelper.AppName=="Yoga_&_Health")
                {
                    pvtitmCast.Header = "asanas";
                    pvtMainDetails.Items.Remove(gamepivot);
                    pvtMainDetails.SelectedIndex = 1;
                }
                else
                {
                    pvtitmCast.Header = "characters";
                }
                Loaded += new RoutedEventHandler(Page_Loaded);
            }
            catch(Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadPivotThemes Method In ShowDetails.cs file", ex);
            }
        }        
        private void Page_Loaded(object sender,RoutedEventArgs e)
        {
            LoadAds();
            AppBarButton addButton = new AppBarButton();
            try
            {                                
                int showid = AppSettings.ShowUniqueID;
                string sharedstatus = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result.ShareStatus;
                if (sharedstatus == "Shared To Blog")                   
                    addButton.Label = "share this" + " " + AppResources.Type + "(" + "Shared)";
                else                    
                    addButton.Label = "share this" + " " + AppResources.Type + "(" + "Not Shared)";
                progressbar.IsIndeterminate = true;

                string id = "";                
                if (background == "0")
                {
                    LoadPivotThemes(AppSettings.ShowUniqueID);
                    tblkVideosTitle.Text = OnlineShow.GetShowDetail(AppSettings.ShowUniqueID).Title;
                }
                progressbar.IsIndeterminate = false;
                PageHelper.RemoveEntryFromBackStack("ShowDetails");
                
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In ShowDetails.cs file", ex);
            }
        }

        #region Events
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {            
            string[] parameters = (string[])e.Parameter;
            if (parameters[0] != null)
            {
                id = parameters[0];
                AppSettings.ShowID = id;
            }
            if (parameters[1] != null)
            {
                pivotindex = parameters[1];
            }
            _mediaPlayer = BackgroundMediaPlayer.Current;
            //HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            try
            {
                //FlurryWP8SDK.Api.LogPageView();
                int showid = AppSettings.ShowUniqueID;
                if (Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync().Result.Status != "Custom")
                {
                    this.BottomAppBar.Visibility = Visibility.Collapsed;
                    appbarvisible = false;                   
                }
                else
                {
                    this.BottomAppBar.Visibility = Visibility.Visible;
                    while (Frame.BackStack.Count() > 1)
                    {
                        if (!Frame.BackStack.FirstOrDefault().SourcePageType.Equals("ShowDetails"))
                        {
                            Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
                        }
                        else
                        {
                            //Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
                            //break;
                        }
                    }
                }
                if (navigationvalue == "1")
                {
                    if (pivotindex != null)
                        pvtMainDetails.SelectedIndex = Convert.ToInt32(pivotindex);
                    Constants.navigationvalue++;
                }
                else
                {
                    if (pivotindex != null)
                        pvtMainDetails.SelectedIndex = Convert.ToInt32(pivotindex);
                    Constants.navigationvalue--;
                }
                //}
                if (Frame.CanGoBack)
                {
                    AppState.searchtitle = "";
                }
                else
                {

                }
                if (ShowVideos.current.lbxSongsList.IsEnabled == false)
                {
                    ShowVideos.current.lbxSongsList.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In ShowDetails.cs file", ex);
            }

        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
                AppState.searchtitle = "";               
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom Method In ShowDetails.cs file", ex);
            }
        }
       
        private void pvtMainDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            CommandBar bottomCommandBar = this.BottomAppBar as CommandBar;
            //AppBarButton addButton = new AppBarButton();
            try
            {
                currentItem = e.AddedItems[0] as PivotItem;
                if (appbarvisible == true)
                {
                    if (currentItem != null)
                    {
                        if (currentItem.Header.ToString() == "game")
                        {
                            if (bottomCommandBar.SecondaryCommands.Count == 2)
                                bottomCommandBar.SecondaryCommands.RemoveAt(1);
                        }
                        else
                        {
                            if (bottomCommandBar.SecondaryCommands.Count < 2)
                            {
                                btnadd.Label = "Add" + " " + currentItem.Header.ToString();
                            }
                            else
                            {
                                btnadd.Label = "Add" + " " + currentItem.Header.ToString();
                            }
                        }
                    }
                }
                if (pvtMainDetails.SelectedIndex == 2)
                {
                    BackgroundHelper bwhelper = new BackgroundHelper();
                    bwhelper.AddBackgroundTask(
                    (object s, DoWorkEventArgs a) =>
                    {
                        //addgame();
                    },
                    (object s, RunWorkerCompletedEventArgs a) =>
                    {
                    }
                    );
                    bwhelper.RunBackgroundWorkers();
                    bwhelper.StartScheduledTasks();
                }
                else
                {
                    if (BackgroundMediaPlayer.Current.CurrentState == MediaPlayerState.Playing)
                    {
                        object name;
                        var TrackName = playerobj.AudioTrackName;
                        TrackName.TryGetValue("Play", out name);
                        string Tracksource = Convert.ToString(name);
                        if (BackgroundMediaPlayer.Current != null && Tracksource.Contains("Claps.mp3"))
                        {
                            BackgroundMediaPlayer.Current.Pause();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in pvtMainDetails_SelectionChanged Method In ShowDetails.cs file", ex);
            }
        }
       
        public async void addgame()
        {
            try
            {                
                if (!ShowCastManager.ShowGameControl(AppSettings.ShowID))
                {
                    storyboardtimer = new DispatcherTimer();
                    storyboardtimer.Interval = TimeSpan.FromSeconds(2);                    
                    storyboardtimer.Tick += storyboardtimer_Tick;
                    storyboardtimer.Start();
                }            
            }
            catch (Exception ex)
            {
                   Exceptions.SaveOrSendExceptions("Exception in addgame Method In ShowDetails.cs file", ex);
            }
        }

        void storyboardtimer_Tick(object sender, object e)
        {
            try
            {
                if (pvtMainDetails.SelectedIndex == 2)
                {
                    if (OnlineVideosCardGame.GameInstance.gameopened == false)
                    {
                        foreach (OnlineVideosCardGame.Model.MemoryCard m in OnlineVideosCardGame.GameInstance.stopstoryboard)
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
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in storyboardtimer_Tick Method In ShowDetails.cs file", ex);
            }
        }
        
        #endregion

        private void imgTitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {                          
            Frame.Navigate(typeof(MainPage));
        }
        
        private void share_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (NetworkHelper.IsNetworkAvailableForDownloads())
                {
                    UploadToBlog upload = new UploadToBlog(AppSettings.ShowUniqueID, page1: this);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in btnshare_Click_1 Method In ShowDetails.cs file", ex);
            }
        }

        private void btnadd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((sender as AppBarButton).Label.Contains("characters"))
                {
                    Constants.DownloadTimer.Stop();
                    Frame.Navigate(typeof(AddCast_New));
                }
                else if ((sender as AppBarButton).Label.Contains("asanas"))
                {
                    Constants.DownloadTimer.Stop();
                    Frame.Navigate(typeof(AddCast_New));
                }
                else
                {
                    if ((sender as AppBarButton).Label.Contains("videos"))
                    {
                        Constants.DownloadTimer.Stop();
                        Constants.Linkstype = "Songs";
                        string[] parame = new string[3];
                        parame[0] = id;
                        parame[1] = string.Empty;
                        parame[2] = "chapters";

                        Frame.Navigate(typeof(LinksFromOnline), parame);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in btnadd_Click_1 Method In ShowDetails.cs file", ex);
            }
        }
    }
}
