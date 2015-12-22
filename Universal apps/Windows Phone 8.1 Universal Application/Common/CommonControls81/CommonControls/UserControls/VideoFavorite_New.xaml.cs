using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Reflection;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    //public class valchanged : INotifyPropertyChanged
    //{
    //    private Visibility _visible = Visibility.Collapsed;
    //    public Visibility visible
    //    {
    //        get
    //        {
    //            return _visible;
    //        }
    //        set
    //        {
    //            _visible = value;
    //            OnPropertyChanged("visible");
    //        }
    //    }
        
    //    public event PropertyChangedEventHandler PropertyChanged;
    //    private void OnPropertyChanged(String name)
    //    {
    //        if (PropertyChanged != null)
    //        {
    //            PropertyChanged(this, new PropertyChangedEventArgs(name));
    //        }
    //    }
    //}
    public sealed partial class VideoFavorite_New : UserControl
    {
        bool check = false;
        ShowLinks selecteditem = null;
        public static VideoFavorite_New currents = default(VideoFavorite_New);
        public List<ShowLinks> Oflinelist1 = new List<ShowLinks>();
        
        public VideoFavorite_New()
        {
            Constants.downstate = 4;
            this.InitializeComponent();
            currents = this;
            Loaded += VideoFavorite_New_Loaded;
        }

        private void VideoFavorite_New_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadFavoriteVideos();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in CheckFolder Method In ShowVideos.cs file", ex);
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
                        var file1 = Task.Run(async () => await folder1.GetFileAsync(sl.movietitle + "_" + sl.Title + ".mp4")).Result;
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
                return default(List<int>);
            }
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
            }
        }
        public void LoadFavoriteVideos()
        {
            //if(Constants.downstate==1)
            //{
            //    Constants.downstate = Constants.downstate + 1;
            //}
            //if (Constants.downstate == 4)
            //{
                //if (AppSettings.FavoritesSelectedIndex != "")
                //{
                //    lstvwsongs.ItemsSource = null;
                //    lstvwsongs.Items.Clear();
                //    AppSettings.FavoritesSelectedIndex = "";
                //}
                List<ShowLinks> objfavlist = new List<ShowLinks>();
                //objfavlist = FavoritesManager.GetFavoriteByType(LinkType.Songs);
                objfavlist = FavoritesManager.GetFavoriteLinks(LinkType.Songs);


                Constants.selecteditemshowlinklist = new ObservableCollection<ShowLinks>(objfavlist);
                Oflinelist1 = Constants.selecteditemshowlinklist.ToList();

                if (objfavlist.Count != 0)
                {
                    if(lstvwsongs.Items.Count>0)
                    {
                        lstvwsongs.Items.Clear();
                    }
                    lstvwsongs.ItemsSource = objfavlist;
                    lstvwsongs.Loaded += Lstvwsongs_Loaded;
                    progressbar.IsActive = false;
                }
                else
                {
                    progressbar.IsActive = false;
                    txtmsg.Text = "No videos  available";
                    txtmsg.Visibility = Visibility.Visible;
                }
            //}
        }

        private void Lstvwsongs_Loaded(object sender, RoutedEventArgs e)
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

        private void snaplstvwsongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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

        private void tblkChapter_RightTapped_1(object sender, RightTappedRoutedEventArgs e)
        {
            Constants.selecteditem = null;
            check = true;
            Constants.Favoriteappbarvisible = true;
            Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
            p.GetType().GetTypeInfo().GetDeclaredMethod("appbar").Invoke(p, new object[] { true });
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
                    Constants.Favoritesselecteditem = selecteditem;
                    AppSettings.LinkType = selecteditem.LinkType;
                    AppSettings.ShowID = (lstvwsongs.SelectedItem as ShowLinks).ShowID.ToString();
                    AppSettings.FavoritesSelectedIndex = lstvwsongs.SelectedIndex.ToString();
                    lstvwsongs.SelectedIndex = -1;
                    return;

                }
                //if (AppResources.advisible == false)
                //{
                //    var selecteditem1 = (sender as Selector).SelectedItem as ShowLinks;
                //    var Itemcollection = (sender as GridView).Items.ToList();
                //    Constants.YoutubePlayList = new List<string>();
                //    foreach (var item in Itemcollection.Cast<ShowLinks>())
                //    {
                //        Constants.YoutubePlayList.Add(item.LinkUrl);
                //    }
                //    Constants.YoutubePlayList.Remove(selecteditem1.LinkUrl);
                //    foreach (var item in Itemcollection.Cast<ShowLinks>())
                //    {
                //        if (selecteditem1.LinkUrl != item.LinkUrl)
                //        {
                //            Constants.YoutubePlayList.Remove(item.LinkUrl);
                //        }
                //        else
                //        {
                //            break;
                //        }
                //    }
                //}
                //else
                //{
                var selecteditem1 = (sender as Selector).SelectedItem as ShowLinks;
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
                AppSettings.ShowID = (lstvwsongs.SelectedItem as ShowLinks).ShowID.ToString();
                AppSettings.FavTitle = (lstvwsongs.SelectedItem as ShowLinks).Title;
                AppSettings.LinkUrl = (lstvwsongs.SelectedItem as ShowLinks).LinkUrl;
                string Url = (lstvwsongs.SelectedItem as ShowLinks).LinkUrl;
                AppSettings.LinkUrl = Url.Split(',')[1];
                AppSettings.PlayVideoTitle = (lstvwsongs.SelectedItem as ShowLinks).Title;
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("Youtube").Invoke(p, null);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstvwfavoriteVideosongs_SelectionChanged_2 Event In VideoFavorite.cs file", ex);
            }
        }
    }
}
