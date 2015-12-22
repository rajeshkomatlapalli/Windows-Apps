using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
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
using System.ComponentModel;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class VideoFavorite : UserControl
    {
        public string movieid = string.Empty;
        public string title = string.Empty;
        bool check = false;
        ShowLinks selecteditem = null;
        public VideoFavorite()
        {
            this.InitializeComponent();
            AppSettings.LinkUrl = string.Empty;
            progressbar.IsActive = true;
            Loaded += VideoFavorite_Loaded;
        }

        private void VideoFavorite_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFavoriteVideos();
            //GetPageDataInBackground();
        }

        private void lstvwfavoriteVideosongs_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstvwfavoriteVideosongs.SelectedIndex == -1)
                    return;
                if (check == true)
                {
                    check = false;
                    selecteditem = (sender as Selector).SelectedItem as ShowLinks;
                    Constants.selecteditem = selecteditem;
                    Constants.Favoritesselecteditem = selecteditem;
                    AppSettings.LinkType = selecteditem.LinkType;
                    AppSettings.ShowID = (lstvwfavoriteVideosongs.SelectedItem as ShowLinks).ShowID.ToString();
                    AppSettings.FavoritesSelectedIndex = lstvwfavoriteVideosongs.SelectedIndex.ToString();
                    lstvwfavoriteVideosongs.SelectedIndex = -1;
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
                AppSettings.ShowID = (lstvwfavoriteVideosongs.SelectedItem as ShowLinks).ShowID.ToString();
                AppSettings.FavTitle = (lstvwfavoriteVideosongs.SelectedItem as ShowLinks).Title;
                AppSettings.LinkUrl = (lstvwfavoriteVideosongs.SelectedItem as ShowLinks).LinkUrl;
                string Url = (lstvwfavoriteVideosongs.SelectedItem as ShowLinks).LinkUrl;
                AppSettings.LinkUrl = Url.Split(',')[1];
                AppSettings.PlayVideoTitle = (lstvwfavoriteVideosongs.SelectedItem as ShowLinks).Title;
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("Youtube").Invoke(p, null);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstvwfavoriteVideosongs_SelectionChanged_2 Event In VideoFavorite.cs file", ex);
            }
        }

        private void GetPageDataInBackground()
        {
            lstvwfavoriteVideosongs.ItemsSource = FavoritesManager.GetFavoriteByType(LinkType.Songs);
            BackgroundHelper bwHelper = new BackgroundHelper();

            bwHelper.AddBackgroundTask(
                                        (object s, DoWorkEventArgs a) =>
                                        {
                                            a.Result = FavoritesManager.GetFavoriteByType(LinkType.Songs);
                                        },
                                        (object s, RunWorkerCompletedEventArgs a) =>
                                        {
                                            lstvwfavoriteVideosongs.ItemsSource = (List<ShowLinks>)a.Result;
                                        }
                                      );
            bwHelper.RunBackgroundWorkers();
        }

        public void LoadFavoriteVideos()
        {
            if (AppSettings.FavoritesSelectedIndex != "")
            {
                lstvwfavoriteVideosongs.ItemsSource = null;
                lstvwfavoriteVideosongs.Items.Clear();
                AppSettings.FavoritesSelectedIndex = "";
            }
            List<ShowLinks> objfavlist = new List<ShowLinks>();
            //objfavlist = FavoritesManager.GetFavoriteByType(LinkType.Songs);
            objfavlist = FavoritesManager.GetFavoriteLinks(LinkType.Songs);
            if (objfavlist.Count != 0)
            {
                lstvwfavoriteVideosongs.ItemsSource = objfavlist;
                progressbar.IsActive = false;
            }
            else
            {
                progressbar.IsActive = false;
                txtmsg.Text = "No videos  available";
                txtmsg.Visibility = Visibility.Visible;
            }
        }

        public int GetCount()
        {
            int count = 0;
            try
            {
                count = lstvwfavoriteVideosongs.Items.Count;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetCount method In VideoFavorite.cs file", ex);
            }
            return count;
        }

        private void tblkChapter_RightTapped_1(object sender, RightTappedRoutedEventArgs e)
        {
            Constants.selecteditem = null;
            check = true;
            Constants.Favoriteappbarvisible = true;
            Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
            p.GetType().GetTypeInfo().GetDeclaredMethod("appbar").Invoke(p, new object[] { true });
        }
    }
}