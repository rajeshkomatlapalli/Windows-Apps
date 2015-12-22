using Common.Library;
using comm = OnlineVideos.Common;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Reflection;
using Windows.UI.Xaml.Media.Imaging;
using OnlineVideos.Views;
using System.Diagnostics;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class ShowChapters : UserControl
    {
        AppInsights insights = new AppInsights();
        Stopwatch stopwatch = new Stopwatch();
        public static ShowChapters currents = default(ShowChapters);
        public List<ShowLinks> Oflinelist1 = new List<ShowLinks>();
        public string ShowID = string.Empty;
        ShowLinks selecteditem = null;
        List<ShowLinks> objlist = new List<ShowLinks>();
        List<ShowLinks> StatusImageCode = new List<ShowLinks>();
        bool check = false;
        public ShowChapters()
        {
            try
            {
            this.InitializeComponent();
            AppSettings.YoutubeID = "1";
            currents = this;
            progressbar.IsActive = true;
            Loaded += ShowChapters_Loaded;
            Window.Current.SizeChanged += Current_SizeChanged;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowChapters Method In ShowChapters.cs file", ex);
                insights.Exception(ex);
            }
        }

        public void tblkvisible()
        {
            txtmsg.Visibility = Visibility.Visible;
            txtmsg.Text = "No chapters available";
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
                Exceptions.SaveOrSendExceptions("Exception in Current_SizeChanged Method In ShowChapters.cs file", ex);
                insights.Exception(ex);
            }
        }

        void ShowChapters_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                insights.Event("Chapters view");
                checkstate();
                GetPageDataInBackground();

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowChapters_Loaded Method In ShowChapters.cs file", ex);
                insights.Exception(ex);
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

                                                      a.Result = OnlineShow.GetShowLinksByMovies(AppSettings.ShowID);
                                                  },
                                                  (object s, RunWorkerCompletedEventArgs a) =>
                                                  {


                                                      if (Constants.selectedMovielinklist.Count > 0)
                                                      {
                                                          Oflinelist1 = Constants.selectedMovielinklist.ToList();
                                                          lstvwmovies.ItemsSource = Constants.selectedMovielinklist;
                                                          itemlistview.ItemsSource = Constants.selectedMovielinklist;
                                                          if (NetworkHelper.IsNetworkAvailable())
                                                          {
                                                              lstvwmovies.Loaded += lstvwmovies_Loaded;
                                                          }
                                                          progressbar.IsActive = false;
                                                          txtmsg.Visibility = Visibility.Collapsed;
                                                          txtmsg1.Visibility = Visibility.Collapsed;
                                                      }
                                                      else
                                                      {
                                                          lstvwmovies.ItemsSource = null;
                                                          itemlistview.ItemsSource = null;
                                                          progressbar.IsActive = false;
                                                          txtmsg.Text = "No chapters available";
                                                          txtmsg.Visibility = Visibility.Visible;
                                                          txtmsg1.Visibility = Visibility.Visible;
                                                      }


                                                  }
                                                );
                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In ShowChapters.cs file", ex);
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
                    else
                        Exists = false;
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
                                                List<ShowLinks> TempList = Constants.selectedMovielinklist.ToList();

                                                if (linkorder.Count() > 0)
                                                {

                                                    foreach (ShowLinks ln in TempList)
                                                    {
                                                        if (linkorder.Contains(ln.LinkOrder))
                                                        {
                                                            if (ln.DownloadStatus != "Offline Available")
                                                            {
                                                                ShowLinks ss = new ShowLinks();
                                                                ss = Constants.selectedMovielinklist.ToList().Find(i => i.LinkOrder == ln.LinkOrder);
                                                                int index = Constants.selectedMovielinklist.IndexOf(ss);
                                                                ss.DownloadStatus = "Offline Available";
                                                                Constants.selectedMovielinklist.Remove(Constants.selectedMovielinklist.Where(i => i.LinkOrder == ss.LinkOrder).FirstOrDefault());
                                                                Constants.selectedMovielinklist.Insert(index, ss);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (!string.IsNullOrEmpty(ln.DownloadStatus))
                                                            {
                                                                ShowLinks ss = new ShowLinks();
                                                                ss = Constants.selectedMovielinklist.ToList().Find(i => i.LinkOrder == ln.LinkOrder);
                                                                int index = Constants.selectedMovielinklist.IndexOf(ss);
                                                                ss.DownloadStatus = "";
                                                                Constants.selectedMovielinklist.Remove(Constants.selectedMovielinklist.Where(i => i.LinkOrder == ss.LinkOrder).FirstOrDefault());
                                                                Constants.selectedMovielinklist.Insert(index, ss);
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
            }
        }
        void lstvwmovies_Loaded(object sender, RoutedEventArgs e)
        {
            //insights.Event("list view movies loaded");
            if (!NetworkHelper.IsNetworkAvailable())
            {
                List<ShowLinks> Oflinelist = new List<ShowLinks>();
                Oflinelist = Constants.selectedMovielinklist.ToList().FindAll(i => i.DownloadStatus != "Offline Available");
                foreach (ShowLinks ss in Oflinelist)
                {
                    Constants.selectedMovielinklist.Remove(ss);
                }
            }
            CheckinFolder();
        }
        public void InsertIntoDatabase(List<int> linkorder)
        {
            try
            {
                List<ShowLinks> TempList = Constants.selectedMovielinklist.ToList();

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

        private void lstvwChapters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstvwmovies.SelectedIndex == -1)
                    return;
                AppSettings.LinkTitle = (lstvwmovies.SelectedItem as ShowLinks).Title.ToString();
                //stopwatch = System.Diagnostics.Stopwatch.StartNew();
                //var properties = new Dictionary<string, string> { { AppSettings.LinkTitle, "View" } };
                //var metrics = new Dictionary<string, double> { { "Duration", stopwatch.Elapsed.Seconds } };
                //insights.Event("Chapters", properties, metrics);
                var success = false;
                var startTime = DateTime.UtcNow;
                var timer = System.Diagnostics.Stopwatch.StartNew();
                insights.Dependency("Chapters", "Duration", startTime, timer.Elapsed, success);
                insights.Event(AppSettings.LinkTitle + "View");
                if (check == true)
                {
                    check = false;
                    selecteditem = (sender as Selector).SelectedItem as ShowLinks;
                    Constants.selecteditem = selecteditem;
                    AppSettings.LinkType = selecteditem.LinkType;
                    Constants.LinkID = selecteditem.LinkID;
                    lstvwmovies.SelectedIndex = -1;
                    AppSettings.LinkType = Constants.selecteditem.LinkType;
                    AppSettings.Title = Constants.selecteditem.Title;
                    AppSettings.PlayVideoTitle = Constants.selecteditem.Title;
                    Page p1 = (Page)comm.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                    p1.GetType().GetTypeInfo().GetDeclaredMethod("appbar").Invoke(p1, new object[] { true });
                    p1.GetType().GetTypeInfo().GetDeclaredMethod("changetext").Invoke(p1, new object[] { "Movies" });
                    return;
                }

                var selecteditem1 = (sender as Selector).SelectedItem as ShowLinks;
                //if (AppResources.advisible == false)
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
                History history = new History();
                history.SaveMovieHistory((lstvwmovies.SelectedItem as ShowLinks).ShowID.ToString(), (lstvwmovies.SelectedItem as ShowLinks).Title, (lstvwmovies.SelectedItem as ShowLinks).LinkUrl);
                AppSettings.LinkUrl = selecteditem1.LinkUrl;
                AppSettings.ShowID = selecteditem1.ShowID.ToString();
                AppSettings.Status = selecteditem1.Status;
                AppSettings.Title = selecteditem1.Title;
                AppSettings.PlayVideoTitle = selecteditem1.Title;
                
                Page p = (Page)comm.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("Youtube").Invoke(p, null);
               

                lstvwmovies.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstvwChapters_SelectionChanged Method In ShowChapters.cs file", ex);
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
                Exceptions.SaveOrSendExceptions("Exception in checkstate Method In ShowChapters.cs file", ex);
                insights.Exception(ex);
            }
        }

        private void tblkChapter_RightTapped_1(object sender, RightTappedRoutedEventArgs e)
        {
            try
            {
                check = true;
                Constants.appbarvisible = true;

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in tblkChapter_RightTapped_1 Method In ShowChapters.cs file", ex);
                insights.Exception(ex);
            }
        }

        private void Image_ImageFailed_1(object sender, ExceptionRoutedEventArgs e)
        {
            if (NetworkHelper.IsNetworkAvailable())
            {
                Constants.selectedMovielinklist.Remove(Constants.selectedMovielinklist.Where(i => i.ThumbnailImage == ((BitmapImage)(sender as Image).Source).UriSource.ToString()).FirstOrDefault());
            }
            if (Constants.selectedMovielinklist.Count == 0)
            {
                txtmsg.Visibility = Visibility.Visible;
                txtmsg1.Visibility = Visibility.Visible;
            }
        }

        private void tblkChapter1_Click_2(object sender, RoutedEventArgs e)
        {
            Page p = (Page)comm.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
            p.GetType().GetTypeInfo().GetDeclaredMethod("DownLoadVideoHelp").Invoke(p, null);
        }

        private void snaplstvwChapters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (itemlistview.SelectedIndex == -1)
                    return;
                if (check == true)
                {
                    check = false;
                    selecteditem = (sender as Selector).SelectedItem as ShowLinks;
                    Constants.selecteditem = selecteditem;
                    AppSettings.LinkType = selecteditem.LinkType;
                    insights.Event(AppSettings.LinkType + "view");
                    itemlistview.SelectedIndex = -1;
                    return;
                }

                var selecteditem1 = (sender as Selector).SelectedItem as ShowLinks;
                History history = new History();
                history.SaveMovieHistory((itemlistview.SelectedItem as ShowLinks).ShowID.ToString(), (itemlistview.SelectedItem as ShowLinks).Title, (itemlistview.SelectedItem as ShowLinks).LinkUrl);
                AppSettings.LinkUrl = selecteditem1.LinkUrl;

                AppSettings.ShowID = selecteditem1.ShowID.ToString();

                Page p = (Page)comm.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("Youtube").Invoke(p, null);


                itemlistview.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in snaplstvwChapters_SelectionChanged Method In ShowChapters.cs file", ex);
                insights.Exception(ex);
            }
        }
    }
}
