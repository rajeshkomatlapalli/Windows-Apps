using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using comm = OnlineVideos.Common;
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
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Reflection;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using System.Diagnostics;
using OnlineVideos.Views;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public class valchanged : INotifyPropertyChanged
    {
        private Visibility _visible = Visibility.Collapsed;
        public Visibility visible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
                OnPropertyChanged("visible");
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

    public sealed partial class ShowVideos : UserControl
    {
        //public  valchanged change = new valchanged();

        public static ShowVideos currents = default(ShowVideos);
        AppInsights insights = new AppInsights();
        Stopwatch stopwatch = new Stopwatch();
        public List<ShowLinks> Oflinelist1 = new List<ShowLinks>();
        public string ShowID = string.Empty;
        ShowLinks selecteditem = null;
        ObservableCollection<ShowLinks> objlist = new ObservableCollection<ShowLinks>();
        List<ShowLinks> StatusImageCode = new List<ShowLinks>();
        bool check = false;

        public ShowVideos()
        {
            try
            {
            this.InitializeComponent();
            progressbar.IsActive = true;

            currents = this;
            AppSettings.YoutubeID = "1";
            AppSettings.LinkUrl = string.Empty;
            Loaded += ShowVideos_Loaded;
            Window.Current.SizeChanged += Current_SizeChanged;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowVideos Method In ShowVideos.cs file", ex);
                insights.Exception(ex);
            }
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            try
            {
                ApplicationViewState currentState = Windows.UI.ViewManagement.ApplicationView.Value;
                if (currentState == ApplicationViewState.Snapped)
                {
                    VisualStateManager.GoToState(this, "Snapped", false);
                }
                else
                {
                    VisualStateManager.GoToState(this, "Filled", false);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Current_SizeChanged Method In ShowVideos.cs file", ex);
            }
        }

        void ShowVideos_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AppSettings.ProjectName == "Indian Cinema" || AppSettings.ProjectName=="Indian_Cinema")
                {
                    objlist = OnlineShow.GetShowLinksByType(AppSettings.ShowID, LinkType.Songs);

                    if (Constants.selecteditemshowlinklist.Count > 0)
                    {
                        Oflinelist1 = Constants.selecteditemshowlinklist.ToList();
                        lstvwsongs.ItemsSource = Constants.selecteditemshowlinklist;
                        itemlistview.ItemsSource = Constants.selecteditemshowlinklist;
                        if (NetworkHelper.IsNetworkAvailable())
                        {
                            lstvwsongs.Loaded += lstvwsongs_Loaded;
                        }
                        progressbar.IsActive = false;
                        progressbarsnap.IsActive = false;
                        txtmsg.Visibility = Visibility.Collapsed;
                        txtmsg1.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        lstvwsongs.ItemsSource = null;
                        itemlistview.ItemsSource = null;
                        progressbar.IsActive = false;
                        progressbarsnap.IsActive = false;
                        txtmsg.Text = "No videos  available";
                        txtmsg1.Text = "No videos  available";
                        txtmsg.Visibility = Visibility.Visible;
                        txtmsg1.Visibility = Visibility.Visible;
                    }
                    // lstvwsongs.ItemsSource = (List<ShowLinks>)a.Result;
                }
                else if (AppSettings.ProjectName == "Video Mix")
                {
                    GetVideomixVideosInBackground();
                }
                else
                {
                    GetPageDataInBackground();
                }
                checkstate();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowVideos_Loaded Method In ShowVideos.cs file", ex);
                insights.Exception(ex);
            }
        }
        private void checkstate()
        {
            try
            {
                ApplicationViewState currentState = Windows.UI.ViewManagement.ApplicationView.Value;
                if (currentState == ApplicationViewState.Snapped)
                {
                    VisualStateManager.GoToState(this, "Snapped", false);
                }
                else
                {
                    VisualStateManager.GoToState(this, "Filled", false);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in checkstate Method In ShowVideos.cs file", ex);
            }
        }

        public void tblkvisible()
        {
            txtmsg.Visibility = Visibility.Visible;
            txtmsg.Text = "No videos available";
        }

        private void tblkChapter_RightTapped_1(object sender, RightTappedRoutedEventArgs e)
        {
            try
            {
                check = true;
                //Constants.selecteditem = selecteditem;
                Constants.appbarvisible = true;
                //AppSettings.LinkType = Constants.selecteditem.LinkType;
                //AppSettings.Title = Constants.selecteditem.Title;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in tblkChapter_RightTapped_1 Method In ShowVideos.cs file", ex);
            }
        }

        public void GetPageDataInBackground()
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();
                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                a.Result = OnlineShow.GetShowLinksByType(AppSettings.ShowID, LinkType.Songs);
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                if (Constants.selecteditemshowlinklist.Count > 0)
                                                {
                                                    if (NetworkHelper.IsNetworkAvailable())
                                                    {
                                                        Oflinelist1 = Constants.selecteditemshowlinklist.ToList();

                                                        lstvwsongs.ItemsSource = Constants.selecteditemshowlinklist;
                                                        itemlistview.ItemsSource = Constants.selecteditemshowlinklist;
                                                        lstvwsongs.Loaded += lstvwsongs_Loaded;
                                                        progressbar.IsActive = false;
                                                        progressbarsnap.IsActive = false;
                                                        txtmsg.Visibility = Visibility.Collapsed;
                                                    }
                                                    else
                                                    {
                                                        progressbar.IsActive = false;
                                                        progressbarsnap.IsActive = false;
                                                        txtmsg.Text = "No Network  available";
                                                        txtmsg1.Text = "No Network  available";
                                                        txtmsg.Visibility = Visibility.Visible;
                                                        txtmsg1.Visibility = Visibility.Visible;
                                                    }
                                                }
                                                else
                                                {
                                                    progressbar.IsActive = false;
                                                    progressbarsnap.IsActive = false;
                                                    txtmsg.Text = "No videos  available";
                                                    txtmsg1.Text = "No videos  available";
                                                    txtmsg.Visibility = Visibility.Visible;
                                                    txtmsg1.Visibility = Visibility.Visible;
                                                }
                                                // lstvwsongs.ItemsSource = (List<ShowLinks>)a.Result;
                                            }
                                          );
                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In ShowVideos.cs file", ex);
                insights.Exception(ex);
            }
        }

        public void GetVideomixVideosInBackground()
        {
            List<ShowLinks> showlinkslist = new List<ShowLinks>();
            BackgroundHelper bwHelper = new BackgroundHelper();
            bwHelper.AddBackgroundTask(
                                        (object s, DoWorkEventArgs a) =>
                                        {
                                            a.Result = OnlineShow.GetMixVideos();
                                        },
                                        (object s, RunWorkerCompletedEventArgs a) =>
                                        {
                                            if (Constants.selecteditemshowlinklist.Count != 0)
                                            {
                                                lstvwsongs.ItemsSource = Constants.selecteditemshowlinklist;
                                                itemlistview.ItemsSource = Constants.selecteditemshowlinklist;
                                                progressbar.IsActive = false;
                                                progressbarsnap.IsActive = false;
                                            }
                                            else
                                            {
                                                Constants.VideoMixHelpLineVisibility = true;
                                                progressbar.IsActive = false;
                                                progressbarsnap.IsActive = false;
                                                txtmsg.Text = "Please add some videos to the mix and create bookmarks in each video.";
                                                txtmsg1.Text = "Please add some videos to the mix and create bookmarks in each video.";
                                                txtmsg.Visibility = Visibility.Visible;
                                                txtmsg1.Visibility = Visibility.Visible;
                                            }
                                        });
            bwHelper.RunBackgroundWorkers();
        }

        public void CheckinFolder()
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();
                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                a.Result = CheckFolder();
                                                InsertIntoDatabase((List<int>)a.Result);
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                List<int> linkorder = (List<int>)a.Result;
                                                List<ShowLinks> TempList = Constants.selecteditemshowlinklist.ToList();
                                                if (linkorder != null)
                                                {
                                                    if (linkorder.Count() > 0)
                                                    {
                                                        foreach (ShowLinks ln in TempList)
                                                        {
                                                            if (linkorder.Contains(ln.LinkOrder))
                                                            {
                                                                if (ln.DownloadStatus != "Offline Available")
                                                                {
                                                                    ShowLinks ss = Constants.selecteditemshowlinklist.ToList().Find(i => i.LinkOrder == ln.LinkOrder);
                                                                    int index = Constants.selecteditemshowlinklist.IndexOf(ss);
                                                                    ss.DownloadStatus = "Offline Available";
                                                                    Constants.selecteditemshowlinklist.Remove(Constants.selecteditemshowlinklist.Where(i => i.LinkOrder == ss.LinkOrder).FirstOrDefault());
                                                                    Constants.selecteditemshowlinklist.Insert(index, ss);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (!string.IsNullOrEmpty(ln.DownloadStatus))
                                                                {
                                                                    ShowLinks ss = Constants.selecteditemshowlinklist.ToList().Find(i => i.LinkOrder == ln.LinkOrder);
                                                                    int index = Constants.selecteditemshowlinklist.IndexOf(ss);
                                                                    ss.DownloadStatus = "Download";
                                                                    Constants.selecteditemshowlinklist.Remove(Constants.selecteditemshowlinklist.Where(i => i.LinkOrder == ss.LinkOrder).FirstOrDefault());
                                                                    Constants.selecteditemshowlinklist.Insert(index, ss);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                          );
                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in CheckinFolder Method In ShowVideos.cs file", ex);
                insights.Exception(ex);
            }
        }

        void lstvwsongs_Loaded(object sender, RoutedEventArgs e)
        {
            if (!NetworkHelper.IsNetworkAvailable())
            {
                List<ShowLinks> Oflinelist = new List<ShowLinks>();
                Oflinelist = Constants.selecteditemshowlinklist.ToList().FindAll(i => i.DownloadStatus != "Offline Available");
                foreach (ShowLinks ss in Oflinelist)
                {
                    Constants.selecteditemshowlinklist.Remove(ss);
                }
            }
            CheckinFolder();
        }

        public void InsertIntoDatabase(List<int> linkorder)
        {
            try
            {
                List<ShowLinks> TempList = Constants.selecteditemshowlinklist.ToList();
                if (linkorder.Count() > 0)
                {
                    DataManager<ShowLinks> datamanager1 = new DataManager<ShowLinks>();
                    int showid = int.Parse(AppSettings.ShowID);
                    foreach (ShowLinks ln in TempList)
                    {
                        ShowLinks objdetail = datamanager1.GetFromList(i => i.ShowID == showid && i.LinkOrder == ln.LinkOrder);
                        if (linkorder.Contains(ln.LinkOrder))
                        {
                            if (objdetail.DownloadStatus != "Offline Available")
                            {
                                objdetail.DownloadStatus = "Offline Available";
                                datamanager1.Savemovies(objdetail, "");
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(objdetail.DownloadStatus))
                            {
                                objdetail.DownloadStatus = "";
                                datamanager1.Savemovies(objdetail, "");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in InsertIntoDatabase Method In ShowVideos.cs file", ex);
                insights.Exception(ex);
            }
        }

        public List<int> CheckFolder()
        {
            try
            {
                List<int> linkorder = new List<int>();
                var folder = Windows.Storage.KnownFolders.VideosLibrary;
                var folder1 = Task.Run(async () => await folder.CreateFolderAsync(AppSettings.ProjectName, CreationCollisionOption.OpenIfExists)).Result;
                bool Exists = false;
              
                foreach (ShowLinks sl in Oflinelist1)
                {
                    try
                    {
                        var file1 = Task.Run(async () => await folder1.GetFileAsync(AppSettings.MovieTitle + "_" + sl.Title + ".mp4")).Result;
                        if (file1 != null)
                            Exists = true;
                    }                       
                    catch (Exception)
                    {
                        Exists = false;
                    }
                    if (Exists == true)
                    {
                        linkorder.Add(sl.LinkOrder);
                        Exists = false;
                    }
                }
                return linkorder;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in CheckFolder Method In ShowVideos.cs file", ex);
                insights.Exception(ex);
                return default(List<int>);
            }
        }

        public void DownLoad(List<ShowLinks> ListDownLoad)
        {
            lstvwsongs.ItemsSource = ListDownLoad;
            //lstvwsongs.ItemsSource = ListDownLoad;
        }

        private void lstvwsongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstvwsongs.SelectedIndex == -1)
                    return;
                if (check == true)
                {
                    check = false;
                    selecteditem = (sender as Selector).SelectedItem as ShowLinks;
                    Constants.selecteditem = selecteditem;
                    AppSettings.LinkType = selecteditem.LinkType;
                    Constants.LinkID = selecteditem.LinkID;
                    AppSettings.Title = selecteditem.Title;
                    AppSettings.PlayVideoTitle = selecteditem.Title;
                    AppSettings.Status = selecteditem.DownloadStatus;
                    Page p1 = (Page)comm.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                    p1.GetType().GetTypeInfo().GetDeclaredMethod("appbar").Invoke(p1, new object[] { true });
                    p1.GetType().GetTypeInfo().GetDeclaredMethod("changetext").Invoke(p1, new object[] { "Songs" });
                    lstvwsongs.SelectedIndex = -1;
                    return;
                }
                var selecteditem1 = (sender as Selector).SelectedItem as ShowLinks;
                //if (AppResources.advisible ==false)
                //{
                //    var Itemcollection = (sender as GridView).Items.ToList();
                //    Constants.YoutubePlayList = new List<string>();
                //    foreach (var item in Itemcollection.Cast<ShowLinks>().Where(i => i.LinkOrder > selecteditem1.LinkOrder))
                //    {
                //        Constants.YoutubePlayList.Add(item.LinkUrl);
                //    }
                //}
                //else
                //{
                var Itemcollection = (sender as GridView).Items.ToList();
                Constants.YoutubePlayList = new Dictionary<string, string>();
                foreach (var item in Itemcollection.Cast<ShowLinks>().Where(i => i.LinkOrder > selecteditem1.LinkOrder))
                {
                    Constants.YoutubePlayList.Add(item.LinkUrl, item.Title);
                }
                foreach (var item in Itemcollection.Cast<ShowLinks>().Where(i => i.LinkOrder < selecteditem1.LinkOrder))
                {
                    Constants.YoutubePlayList.Add(item.LinkUrl, item.Title);
                }
                //}
                //if (Constants.Gettileimage == false)
                //{
                //    Constants.TileImageTitle = selecteditem1.Title;
                //    Constants.TileImageUrl = selecteditem1.ThumbnailImage;
                //}
                History history = new History();
                history.SaveVideoHistory((lstvwsongs.SelectedItem as ShowLinks).ShowID.ToString(), (lstvwsongs.SelectedItem as ShowLinks).Title, (lstvwsongs.SelectedItem as ShowLinks).LinkUrl);
                AppSettings.LinkUrl = string.Empty;
                AppSettings.LinkUrl = selecteditem1.LinkUrl;
                AppSettings.ShowID = selecteditem1.ShowID.ToString();
                AppSettings.Status = selecteditem1.DownloadStatus;
                AppSettings.Title = selecteditem1.Title;
                AppSettings.PlayVideoTitle = selecteditem1.Title;
                Page p = (Page)comm.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("Youtube").Invoke(p, null);
                lstvwsongs.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstvwsongs_SelectionChanged Method In ShowVideos.cs file", ex);
            }
        }

        private void Image_ImageFailed_1(object sender, ExceptionRoutedEventArgs e)
        {
            List<ShowLinks> RemovieImagelist = new List<ShowLinks>();
            RemovieImagelist.AddRange(Constants.selecteditemshowlinklist);
            if (NetworkHelper.IsNetworkAvailable())
            {
                foreach (ShowLinks item in RemovieImagelist)
                {
                    if (Constants.selecteditemshowlinklist.Count != 0)
                    {
                        if (!item.LinkUrl.StartsWith("http://"))
                        {
                            Constants.selecteditemshowlinklist.Remove(Constants.selecteditemshowlinklist.Where(i => i.ThumbnailImage == ((BitmapImage)(sender as Image).Source).UriSource.ToString()).FirstOrDefault());
                            if (Constants.selecteditemshowlinklist.Count == 0)
                            {
                                txtmsg.Text = "No videos  available";
                                txtmsg1.Text = "No videos  available";
                                txtmsg.Visibility = Visibility.Visible;
                                txtmsg1.Visibility = Visibility.Visible;
                            }
                        }
                    }
                }
            }
        }

        private void tblkChapter1_Click_2(object sender, RoutedEventArgs e)
        {            
            if (AppSettings.ProjectName != "Video Mix")
            {
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("DownLoadVideoHelp").Invoke(p, null);                
            }
        }

        private void snaplstvwsongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (itemlistview.SelectedIndex == -1)
                    return;
                //stopwatch = System.Diagnostics.Stopwatch.StartNew();
                //var properties = new Dictionary<string, string> { { AppSettings.LinkType, AppSettings.Title } };
                //var metrics = new Dictionary<string, double> { { "Processing Time", stopwatch.Elapsed.TotalMilliseconds } };
                //insights.Event("Songs Page Time", properties, metrics);
                var success = false;
                var startTime = DateTime.UtcNow;
                var timer = System.Diagnostics.Stopwatch.StartNew();
                insights.Dependency("Songs", "Duration", startTime, timer.Elapsed, success);
                insights.Event(AppSettings.LinkType + AppSettings.Title + "View");
                if (check == true)
                {
                    check = false;
                    selecteditem = (sender as Selector).SelectedItem as ShowLinks;
                    Constants.selecteditem = selecteditem;
                    AppSettings.LinkType = selecteditem.LinkType;
                    itemlistview.SelectedIndex = -1;
                    return;
                }
                var sssl = (sender as ListView).Items.ToList();
                var selecteditem1 = (sender as Selector).SelectedItem as ShowLinks;
                History history = new History();
                history.SaveVideoHistory((itemlistview.SelectedItem as ShowLinks).ShowID.ToString(), (itemlistview.SelectedItem as ShowLinks).Title, (itemlistview.SelectedItem as ShowLinks).LinkUrl);
                AppSettings.LinkUrl = selecteditem1.LinkUrl;
                AppSettings.ShowID = selecteditem1.ShowID.ToString();
                itemlistview.SelectedIndex = -1;
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("Youtube").Invoke(p, null);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in snaplstvwsongs_SelectionChanged Method In ShowVideos.cs file", ex);
                insights.Exception(ex);
            }
        }       
    }
}