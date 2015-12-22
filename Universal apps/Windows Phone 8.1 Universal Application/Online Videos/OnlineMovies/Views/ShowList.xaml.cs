using Common.Library;
using Common.Utilities;
using OnlineMovies.Views;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.UI;
using OnlineVideos.View_Models;
using OnlineVideos.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineVideos
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Shows : Page
    {
        //AppInsights insights = new AppInsights();
        public ValueSet AudioTrackName { get; set; }
        #region Global
        bool IsDataLoaded;
        List<ShowList> objtoprated = null;
        List<ShowList> objrecent = null;
        List<ShowList> objtamil = null;
        List<ShowList> objupcoming = null;
        List<DownloadStatus> download = new List<DownloadStatus>();
        int topratedCount;
        int recentCount;
        int tamilCount;
        int upcomingCount;
        int rateindex;
        string index = string.Empty;
        string youtubeurl;       
        private HttpClient webClient1 = new HttpClient();
        string name = string.Empty;
        private MediaPlayer _mediaPlayer;

        public static dynamic group = default(dynamic);
        public static dynamic grouptelugu = default(dynamic);
        public static dynamic grouptamil = default(dynamic);
        public static dynamic groupupcoming = default(dynamic);
        #endregion

        #region Constructor
        public Shows()
        {
            try
            {
            this.InitializeComponent();
            topratedCount = Constants.PageSize;
            recentCount = Constants.PageSize;
            tamilCount = Constants.PageSize;
            upcomingCount = Constants.PageSize;
            rateindex = 0;

            LoadPivotThemes();
            LoadDownLoadTheme();
            IsDataLoaded = false;
            }
            catch (Exception ex)
            {                
               Exceptions.SaveOrSendExceptions("Exception in Shows Method In Shows file.", ex);
            }
            HideStatusBar();
        }
        #endregion
        public async void HideStatusBar()
        {
            var statusBar = StatusBar.GetForCurrentView();
            await statusBar.HideAsync();
        }
        private void LoadPivotThemes()
        {

            if (AppSettings.ProjectName == "Indian Cinema" || AppSettings.ProjectName == "Indian Cinema Pro")
            {
                pvttop.Header = "hindi";
                pvtitmrecent.Header = "telugu";
                pvttamil.Header = "tamil";
                pvttamil.Visibility = Visibility.Visible;                
                pvtupcoming.Visibility = Visibility.Visible;
            }
            else
            {
                pvttop.Header = "top rated";
                pvtitmrecent.Header = "recent";                
                pvttamil.Visibility = Visibility.Collapsed;
                pvtupcoming.Visibility = Visibility.Collapsed;
                pvtVideos.Items.Remove(pvttamil);                
                pvtVideos.Items.Remove(pvtupcoming);
            }
            if (AppResources.ShowCategoryPivot != true)
            {
                pvtcat.Visibility = Visibility.Collapsed;
                pvtVideos.Items.Remove(pvtcat);
            }
            else
            {
                pvtcat.Header = AppResources.ShowCategoryPivotTitle.Split(',')[1];
                pvtcat.Visibility = Visibility.Visible;
            }
        }

        private void LoadDownLoadTheme()
        {
            if (AppSettings.AddNewShowIconVisibility == true)
            {
                if (ResourceHelper.ProjectName == "Web Tile")
                {
                    pvtdownload.Visibility = Visibility.Collapsed;
                    pvtVideos.Items.Remove(pvtdownload);
                }
                else
                {
                    pvtdownload.Visibility = Visibility.Visible;
                    pvtdownload.Header = "downloads";
                }
            }
            else
            {
                pvtdownload.Visibility = Visibility.Collapsed;
                pvtVideos.Items.Remove(pvtdownload);
            }
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {         
            name = (string)e.Parameter;
            _mediaPlayer = BackgroundMediaPlayer.Current;
            try
            {
                              
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In Shows.cs file.", ex);
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
               
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in OnNavigatingFrom Method In Shows.cs file.", ex);
            }            
        }

        private void menuflyout_Opened_1(object sender, object e)
        {
            try
            {               
                MenuFlyout menu = sender as MenuFlyout;
                MenuFlyoutItem menu1 = sender as MenuFlyoutItem;
                ListBoxItem selectedListBoxItem = lbxTopRated.ItemContainerGenerator.ContainerFromItem(menu1.DataContext) as ListBoxItem;
                int showid = (selectedListBoxItem.Content as ShowList).ShowID;               
                if (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result.Status != "Custom" && ResourceHelper.AppName != Apps.Web_Tile.ToString())
                {                    
                    menu.Hide();                    
                }                
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ContextMenu_Opened_1 Method In Shows.cs file.", ex);
            }
        }

        private void deletetoprated_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowDetail detailModel = new ShowDetail();
                detailModel.DeleteDownloadedShow(lbxTopRated, sender as MenuFlyoutItem);
                GetPageDataInBackground();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in deletetoprated_Click Method In Shows.cs file.", ex);
            }
        }

        private void menuflyout1_Opened(object sender, object e)
        {
            try
            {
                MenuFlyout menu = sender as MenuFlyout;
                MenuFlyoutItem menu1 = sender as MenuFlyoutItem;
                ListBoxItem selectedListBoxItem = lbxRecentlyAdded.ItemContainerGenerator.ContainerFromItem(menu1.DataContext) as ListBoxItem;
                int showid = (selectedListBoxItem.Content as ShowList).ShowID;                
                if (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result.Status != "Custom" && ResourceHelper.AppName != Apps.Web_Tile.ToString())
                {                    
                    menu.Hide();
                }               
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ContextMenu_Opened_2 Method In Shows.cs file.", ex);
            }
        }

        private void deleterecent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowDetail detailModel = new ShowDetail();
                detailModel.DeleteDownloadedShow(lbxRecentlyAdded, sender as MenuFlyoutItem);
                GetPageDataInBackground();
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in deleterecent_Click Method In Shows.cs file.", ex);
            }
        }

        private void GetPageDataInBackground()
        {
            BackgroundHelper bwHelper = new BackgroundHelper();
            performanceProgressBar.IsIndeterminate = true;
            bwHelper.AddBackgroundTask(
                                        (object s, DoWorkEventArgs a) =>
                                        {                                           
                                            a.Result = group;
                                        },
                                        (object s, RunWorkerCompletedEventArgs a) =>
                                        {
                                            objtoprated = (List<ShowList>)a.Result;
                                            if (objtoprated.Count != 0)
                                            {
                                                lbxTopRated.ItemsSource = (List<ShowList>)a.Result;                                              
                                            }
                                            else
                                            {
                                                lbxTopRated.ItemsSource = null;
                                                tblkToprated.Visibility = Visibility.Visible;
                                            }                                            
                                            if (ResourceHelper.ProjectName == "VideoMix")
                                            {
                                                tblkToprated.Text = "No Video mixes Available";
                                            }
                                            performanceProgressBar.IsIndeterminate = false;
                                        }
                                      );

            bwHelper.AddBackgroundTask(
                                        (object s, DoWorkEventArgs a) =>
                                        {                                            
                                            a.Result = grouptelugu;
                                        },
                                        (object s, RunWorkerCompletedEventArgs a) =>
                                        {
                                            objrecent = (List<ShowList>)a.Result;
                                            if (objrecent.Count != 0)
                                            {
                                                lbxRecentlyAdded.ItemsSource = (List<ShowList>)a.Result;
                                                tblkrecent.Visibility = Visibility.Collapsed;
                                            }
                                            else
                                            {
                                                lbxRecentlyAdded.ItemsSource = null;
                                                tblkrecent.Visibility = Visibility.Visible;
                                            }                                           
                                            if (ResourceHelper.ProjectName == "VideoMix")
                                            {
                                                tblkrecent.Text = "No Video mixes Available";
                                            }
                                            performanceProgressBar.IsIndeterminate = false;
                                        }
                                      );
            if (AppSettings.ProjectName == "Indian Cinema" || AppSettings.ProjectName == "Indian Cinema Pro")
            {
                bwHelper.AddBackgroundTask(
                                          (object s, DoWorkEventArgs a) =>
                                          {                                            
                                             a.Result = grouptamil;
                                          },
                                          (object s, RunWorkerCompletedEventArgs a) =>
                                          {
                                              objtamil = (List<ShowList>)a.Result;
                                              if (objtamil.Count != 0)
                                              {
                                                  lbxTamilAdded.ItemsSource = (List<ShowList>)a.Result;                                               
                                              }
                                              else
                                              {
                                                  lbxTamilAdded.ItemsSource = null;                                                 
                                              }                                            
                                              performanceProgressBar.IsIndeterminate = false;
                                          }
                                        );
            }
            if (AppSettings.ProjectName == "Indian Cinema" || AppSettings.ProjectName == "Indian Cinema Pro")
            {
                bwHelper.AddBackgroundTask(
                    (object s, DoWorkEventArgs a) =>
                    {
                        a.Result = groupupcoming;
                    },
                        (object s, RunWorkerCompletedEventArgs a) =>
                        {
                            objupcoming = (List<ShowList>)a.Result;
                            if (objupcoming.Count != 0)
                            {
                                lbxUpcomingAdded.ItemsSource = (List<ShowList>)a.Result;
                            }
                            else
                            {
                                lbxUpcomingAdded.ItemsSource = null;
                            }
                            performanceProgressBar.IsIndeterminate = false;
                        }
                    );
            }
            
            if (AppResources.ShowCategoryPivot)
            {
                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                if (ResourceHelper.AppName == Apps.Web_Tile.ToString())
                                                {
                                                    a.Result = OnlineShow.LoadCategoryListForWebTile();
                                                }
                                                else if (ResourceHelper.AppName == Apps.Cricket_Videos.ToString())
                                                {
                                                    a.Result = OnlineShow.LoadCategoryListForCricketVideos();
                                                }
                                                else
                                                {
                                                    a.Result = OnlineShow.LoadCategoryList();
                                                }
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                if (ResourceHelper.AppName == Apps.Web_Tile.ToString())
                                                {
                                                    lbxCategory.ItemsSource = (List<ShowCategories>)a.Result;
                                                    performanceProgressBar.IsIndeterminate = false;
                                                }
                                                else if (ResourceHelper.AppName == Apps.Cricket_Videos.ToString())
                                                {
                                                    lbxCategory.ItemsSource = (List<ShowCategories>)a.Result;
                                                    performanceProgressBar.IsIndeterminate = false;
                                                }
                                                else
                                                {
                                                    lbxCategory.ItemsSource = (List<CatageoryTable>)a.Result;
                                                    performanceProgressBar.IsIndeterminate = false;
                                                }
                                            }
                                          );
            }
            if (AppSettings.AddNewShowIconVisibility == true)
            {
                List<DownloadInfo> objVideoList = new List<DownloadInfo>();
                performanceProgressBar.IsIndeterminate = true;
                bwHelper.AddBackgroundTask(
                                           (object s, DoWorkEventArgs a) =>
                                           {
                                               objVideoList = DownloadManager.LoadDownloadUrls();
                                           },
                                           (object s, RunWorkerCompletedEventArgs a) =>
                                           {
                                               if (objVideoList.Count != 0)
                                               {
                                                   DownLoadList();
                                               }
                                               else
                                               {
                                                   Constants.SaveDownloadList.Clear();
                                                   tblkNoDownloads.Visibility = Visibility.Visible;
                                                   lbxdownloading.Visibility = Visibility.Collapsed;
                                               }
                                               performanceProgressBar.IsIndeterminate = false;
                                           }
                                         );

            }
            bwHelper.RunBackgroundWorkers();
        }

        public async void DownLoadList()
        {
            try
            {
                List<DownloadInfo> objVideoList = new List<DownloadInfo>();
                download.Clear();
                download.AddRange(Constants.SaveDownloadList);
                if (Constants.DownloadStatus == false)
                    objVideoList = DownloadManager.LoadDownloadUrls();
                int countlist = objVideoList.Count();
                if (countlist > 0)
                {
                    List<DownloadInfo> info = new List<DownloadInfo>();
                    var xquery = (from q in objVideoList select q);
                    foreach (var itm in xquery)
                    {
                        var listitm = Constants.SaveDownloadList.Where(k => k.Id == itm.FolderID).FirstOrDefault();
                        if (listitm == null)
                        {
                            List<DownloadStatus> downloadstatuslist = new List<DownloadStatus>();
                            string link = "";
                            link = itm.LinkUrl;
                            string name = "";
                            name = itm.LinkUrl.ToString().Substring(itm.LinkUrl.ToString().LastIndexOf('/') + 1);
                            if (itm.LinkUrl.StartsWith("http://www.youtube.com/watch?v="))
                            {
                                link = itm.LinkUrl.Replace("http://www.youtube.com/watch?v=", "");
                                name = itm.title;
                                name = name.Replace(":", "").Replace("|", "").Replace(".flv", "").Replace(".", "").Replace("&", "").Replace("#", "").Replace(";", "").Replace("title=", "");
                            }
                            var ln = link;
                            HttpClient wb = new HttpClient();
                            Windows.Web.Http.HttpClient ss = new Windows.Web.Http.HttpClient();                           
                            downloadstatuslist = DownloadManagerHelper.LoadList(name, link, int.Parse(itm.FolderID.ToString()));
                            download.AddRange(downloadstatuslist);
                            Constants.SaveDownloadList.AddRange(downloadstatuslist);
                            Constants.DownloadStatus = true;
                        }
                    }
                }
                else if (Constants.SaveDownloadList.Count == 0)
                {
                    tblkNoDownloads.Visibility = Visibility.Visible;
                    lbxdownloading.Visibility = Visibility.Collapsed;
                }
                lbxdownloading.ItemsSource = null;
                lbxdownloading.ItemsSource = Constants.SaveDownloadList;
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in AddList Method In showlist file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                Exceptions.SaveExceptionInLocalStorage(excepmess);
                Exceptions.SaveOrSendExceptions("Exception in DownLoadList Method In Shows file.", ex);
            }
        }
       
        private void menuflyout2_Opened(object sender, object e)
        {
            try
            {
                MenuFlyout menu = sender as MenuFlyout;
                MenuFlyoutItem m = sender as MenuFlyoutItem;
                ListBoxItem selectedListBoxItem = lbxTamilAdded.ItemContainerGenerator.ContainerFromItem(m.DataContext) as ListBoxItem;
                int showid = (selectedListBoxItem.Content as ShowList).ShowID;                
                if (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result.Status != "Custom" && ResourceHelper.AppName != Apps.Web_Tile.ToString())
                {                  
                    menu.Hide();                  
                }                
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ContextMenu_Opened_3 Method In Shows.cs file.", ex);
            }
        }

        private void deletetamil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowDetail detailModel = new ShowDetail();
                detailModel.DeleteDownloadedShow(lbxTamilAdded, sender as MenuFlyoutItem);
                GetPageDataInBackground();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in deletetamil_Click_1 Method In Shows.cs file.", ex);
            }
        }

        private void btnadd_Click(object sender, RoutedEventArgs e)
        {
            Constants.DownloadTimer.Stop();
            if (ResourceHelper.AppName == Apps.Cricket_Videos.ToString())
            {                
                Frame.Navigate(typeof(UserUpload));
            }
            else               
                Frame.Navigate(typeof(UserUpload));
        }

        private void lbxTopRated_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxTopRated.SelectedIndex != -1)
                {
                    AppSettings.ShowID = (lbxTopRated.SelectedItem as ShowList).ShowID.ToString();
                    AppSettings.ShowRating = Convert.ToDouble((lbxTopRated.SelectedItem as ShowList).Rating);
                    string title = (lbxTopRated.SelectedItem as ShowList).Title;
                    AppSettings.Title = (lbxTopRated.SelectedItem as ShowList).Title;
                    if (title == Constants.getMoreLabel)
                    {
                        performanceProgressBar.IsIndeterminate = true;
                        Constants.UIThread = true;
                        topratedCount = topratedCount + Constants.PageSize;
                        lbxTopRated.ItemsSource = OnlineShow.GetTopRatedShows(topratedCount);
                        lbxTopRated.ScrollIntoView(lbxTopRated.Items[topratedCount - Constants.PageSize]);
                        performanceProgressBar.IsIndeterminate = false;
                    }
                    else
                    {
                        if (ResourceHelper.ProjectName == Apps.DownloadManger.ToString())
                        {
                            GetDownloadExtensions();
                        }
                        else
                        {
                            AppState.searchtitle = "";
                            string[] parameters = new string[2];
                            parameters[0] = AppSettings.ShowID.ToString();
                            parameters[1] = null;
                            Frame.Navigate(typeof(Details), parameters);
                        }
                    }
                    lbxTopRated.SelectedIndex = -1;
                    State.BackStack = "hindi";
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxTopRated_SelectionChanged event In Shows.cs file.", ex);
            }
        }

        private void GetDownloadExtensions()
        {
            try
            {
                string ext = string.Empty;
                try
                {
                    ext = System.IO.Path.GetExtension((lbxRecentlyAdded.SelectedItem as ShowList).Title);
                }
                catch (Exception ex)
                {
                    ext = System.IO.Path.GetExtension((lbxTopRated.SelectedItem as ShowList).Title);
                }
                if (ext == ".3gp" || ext == ".3g2" || ext == ".mp4" || ext == ".m4v" || ext == ".wmv" || (lbxTopRated.SelectedItem as ShowList).TileImage == "videos.jpg")
                {
                   
                }
                else if (ext == ".mp3" || ext == "=low" || ext == ".wav" || ext == ".aac" || ext == ".amr" || ext == ".wma")
                {
                    try
                    {
                        LoadAudioSongd((lbxRecentlyAdded.SelectedItem as ShowList).Title);
                    }
                    catch (Exception ex)
                    {
                        LoadAudioSongd((lbxTopRated.SelectedItem as ShowList).Title);
                    }
                }
                else if (ext == ".jpg" || ext == ".png")
                {
                    PageHelper.NavigateToDownloadedImagePage(AppResources.DownloadImagePageName);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetDownloadExtensions Method In Shows.cs file.", ex);
            }
        }

        private void LoadAudioSongd(string title)
        {
            try
            {
                int Showid = Convert.ToInt32(AppSettings.ShowID);
                string linktype = LinkType.Audio.ToString();                
                var ShowLinksByType = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == Showid && i.LinkType == linktype).ToListAsync()).Result;
                foreach (var linkinfo in ShowLinksByType)
                {
                    AppSettings.AudioTitle = linkinfo.Title;
                    if (linkinfo.Title == title)
                    {                        
                        AudioTrackName = new ValueSet
                                {
                                    {
                                        "Play",
                                        linkinfo.LinkUrl
                                    }
                                };
                        BackgroundMediaPlayer.SendMessageToBackground(AudioTrackName);
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
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAudioSongd Method In Shows file.", ex);
            }
        }

        void Current_CurrentStateChanged(MediaPlayer sender, object args)
        {
            try
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
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Instance_PlayStateChanged Method In Shows.cs file.", ex);
            }
        }
        private void lbxRecentlyAdded_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxRecentlyAdded.SelectedIndex != -1)
                {
                    AppSettings.ShowID = (lbxRecentlyAdded.SelectedItem as ShowList).ShowID.ToString();
                    AppSettings.ShowRating = Convert.ToDouble((lbxRecentlyAdded.SelectedItem as ShowList).Rating);
                    string title = (lbxRecentlyAdded.SelectedItem as ShowList).Title;
                    AppSettings.Title = (lbxRecentlyAdded.SelectedItem as ShowList).Title;
                    if (title == Constants.getMoreLabel)
                    {
                        performanceProgressBar.IsIndeterminate = true;
                        Constants.UIThread = true;
                        recentCount = recentCount + Constants.PageSize;
                        lbxRecentlyAdded.ItemsSource = OnlineShow.GetRecentlyAddedShows(recentCount);
                        lbxRecentlyAdded.ScrollIntoView(lbxRecentlyAdded.Items[recentCount - Constants.PageSize]);
                        performanceProgressBar.IsIndeterminate = false;
                    }
                    else
                    {
                        if (ResourceHelper.ProjectName == Apps.DownloadManger.ToString())
                        {
                            GetDownloadExtensions();
                        }
                        else
                        {
                            AppState.searchtitle = "";
                            string[] parameters = new string[2];
                            parameters[0] = AppSettings.ShowID.ToString();
                            parameters[1] = null;
                            Frame.Navigate(typeof(Details), parameters);
                        }
                    }
                    lbxRecentlyAdded.SelectedIndex = -1;
                    State.BackStack = "telugu";
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxRecentlyAdded_SelectionChanged Method In Shows.cs file.", ex);
            }
        }

        private void lbxTamilAdded_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxTamilAdded.SelectedIndex != -1)
                {
                    AppSettings.ShowID = (lbxTamilAdded.SelectedItem as ShowList).ShowID.ToString();
                    AppSettings.ShowRating = Convert.ToDouble((lbxTamilAdded.SelectedItem as ShowList).Rating);
                    string title = (lbxTamilAdded.SelectedItem as ShowList).Title;
                    AppSettings.Title = (lbxTamilAdded.SelectedItem as ShowList).Title;
                    if (title == Constants.getMoreLabel)
                    {
                        performanceProgressBar.IsIndeterminate = true;
                        Constants.UIThread = true;
                        tamilCount = tamilCount + Constants.PageSize;
                        lbxTamilAdded.ItemsSource = OnlineShow.GetTamilAddedShows(tamilCount);
                        lbxTamilAdded.ScrollIntoView(lbxTamilAdded.Items[tamilCount - Constants.PageSize]);
                        performanceProgressBar.IsIndeterminate = false;
                    }
                    else
                    {
                        if (ResourceHelper.ProjectName == Apps.DownloadManger.ToString())
                        {
                            GetDownloadExtensions();
                        }
                        else
                        {
                            AppState.searchtitle = "";
                            string[] parameters = new string[2];
                            parameters[0] = AppSettings.ShowID.ToString();
                            parameters[1] = null;
                            Frame.Navigate(typeof(Details), parameters);
                        }
                    }
                    lbxTamilAdded.SelectedIndex = -1;
                    State.BackStack = "tamil";
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxTamilAdded_SelectionChanged_1 event In Shows.cs file.", ex);
            }
        }

        private void lbxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ResourceHelper.AppName == Apps.Web_Tile.ToString())
                {
                    if (lbxCategory.SelectedIndex != -1)
                    {                                                              
                        string[] parameter = new string[2];
                        parameter[0] = (lbxCategory.SelectedItem as ShowCategories).CategoryName;
                        parameter[1] = (lbxCategory.SelectedItem as ShowCategories).CategoryID.ToString();

                        Frame.Navigate(typeof(CategoryDetails), parameter);
                        lbxCategory.SelectedIndex = -1;
                    }
                }
                else if (ResourceHelper.AppName == Apps.Cricket_Videos.ToString())
                {
                    if (lbxCategory.SelectedIndex != -1)
                    {                      
                        string[] parameter = new string[2];
                        parameter[0] = (lbxCategory.SelectedItem as ShowCategories).CategoryName;
                        parameter[1] = (lbxCategory.SelectedItem as ShowCategories).CategoryID.ToString();

                        Frame.Navigate(typeof(CategoryDetails), parameter);
                        lbxCategory.SelectedIndex = -1;
                    }
                }

                else
                {
                    if (lbxCategory.SelectedIndex != -1)
                    {                        
                        string[] parameter = new string[2];
                        parameter[0] = (lbxCategory.SelectedItem as CatageoryTable).CategoryName;
                        parameter[1] = (lbxCategory.SelectedItem as CatageoryTable).CategoryID.ToString();

                        Frame.Navigate(typeof(CategoryDetails), parameter);
                        lbxCategory.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxTopRated_SelectionChanged Method In Videos file", ex);
            }
        }

        #region PageLoad
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (State.BackStack == "hindi")
            {
                pvtVideos.SelectedIndex = 1;
                State.BackStack = "";
            }
            else if (State.BackStack == "telugu")
            {
                pvtVideos.SelectedIndex = 2;
                State.BackStack = "";
            }
            else if (State.BackStack == "tamil")
            {
                pvtVideos.SelectedIndex = 3;
                State.BackStack = "";
            }
            else if (State.BackStack == "upcoming movies")
            {
                pvtVideos.SelectedIndex = 4;
                State.BackStack = "";
            }
            LoadAds();
                Constants.UIThread = true;
                group = OnlineShow.GetTopRatedShows(topratedCount);
                grouptelugu = OnlineShow.GetRecentlyAddedShows(recentCount);
            grouptamil = OnlineShow.GetTamilAddedShows(tamilCount);
            groupupcoming = OnlineShow.GetUpComingAddedShows(upcomingCount);
                Constants.UIThread = false;
            if (ResourceHelper.AppName != Apps.Indian_Cinema_Pro.ToString() && ResourceHelper.AppName != Apps.Kids_TV_Pro.ToString() && ResourceHelper.AppName != Apps.Story_Time_Pro.ToString())
               // LoadAds();
            try
            {               
                if (ResourceHelper.ProjectName != "Web Tile" && ResourceHelper.ProjectName != "Video Mix")
                {                   
                    AppBarButton b = new AppBarButton();                   
                    b.Label = "Add" + " " + AppResources.Type;                                        
                }
                else
                {
                    BottomAppBar.Visibility = Visibility.Collapsed;                          
                }
                if (IsDataLoaded == false)
                {
                    if (rateindex != 0)
                    {
                        pvtVideos.SelectedIndex = rateindex;
                        rateindex = 0;
                    }

                    PageHelper.RemoveEntryFromBackStack("Videos");                    
                   tblkVideosTitle.Text = name.ToUpper(); 
                                    
                    try
                    {
                        GetPageDataInBackground();
                    }
                    catch (Exception ex)
                    {
                        Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In Shows file.", ex);
                    }
                }
                performanceProgressBar.IsIndeterminate = false;

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In Shows file.", ex);
            }
        }
        #endregion

        private void LoadAds()
        {
            try
            {
                LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 2);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In SongDetails file", ex);
                string excepmess = "Exception in LoadAds Method In SongDetails file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }

        private void imgTitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {                      
            Frame.Navigate(typeof(MainPage));        
        }

        private void pvtVideos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (State.BackStack == "hindi")
            {
                pvtVideos.SelectedIndex = 2;
            }
            else if (State.BackStack == "telugu")
            {
                pvtVideos.SelectedIndex = 3;
            }
            else if (State.BackStack == "tamil")
            {
                pvtVideos.SelectedIndex = 4;
            }
            else if (State.BackStack == "hindi")
            {
                pvtVideos.SelectedIndex = 4;
            }

            string pvtindex = pvtVideos.SelectedIndex.ToString();
            string pvtitem = string.Empty;
            switch (pvtindex)
            {
                case "0": pvtitem = "MovieList genre"; break;
                case "1": pvtitem = "MovieList hindi"; break;
                case "2": pvtitem = "MovieList telugu"; break;
                case "3": pvtitem = "MovieList tamil"; break;
            }
            //insights.Event(pvtitem + "View");
        }

        private void lbxUpcomingAdded_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxUpcomingAdded.SelectedIndex != -1)
                {
                    AppSettings.ShowID = (lbxUpcomingAdded.SelectedItem as ShowList).ShowID.ToString();
                    AppSettings.ShowRating = Convert.ToDouble((lbxUpcomingAdded.SelectedItem as ShowList).Rating);
                    string title = (lbxUpcomingAdded.SelectedItem as ShowList).Title;
                    AppSettings.Title = (lbxUpcomingAdded.SelectedItem as ShowList).Title;
                    if (title == Constants.getMoreLabel)
                    {
                        performanceProgressBar.IsIndeterminate = true;
                        Constants.UIThread = true;
                        upcomingCount = upcomingCount + Constants.PageSize;
                        lbxUpcomingAdded.ItemsSource = OnlineShow.GetUpComingAddedShows(upcomingCount);
                        lbxUpcomingAdded.ScrollIntoView(lbxUpcomingAdded.Items[upcomingCount - Constants.PageSize]);
                        performanceProgressBar.IsIndeterminate = false;
                    }
                    else
                    {
                        if (ResourceHelper.ProjectName == Apps.DownloadManger.ToString())
                        {
                            GetDownloadExtensions();
                        }
                        else
                        {
                            AppState.searchtitle = "";
                            //PageHelper.NavigateToDetailPage(AppResources.DetailPageName, AppSettings.ShowID);
                            String[] array = new string[2];
                            array[0] = AppSettings.ShowID.ToString();
                            array[1] = null;
                            Frame.Navigate(typeof(Details), array);
                        }
                    }

                    lbxUpcomingAdded.SelectedIndex = -1;
                    State.BackStack = "upcoming movies";
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxUpcomingAdded_SelectionChanged_1 Method In Shows.cs file.", ex);
            }
        }
    }
}
