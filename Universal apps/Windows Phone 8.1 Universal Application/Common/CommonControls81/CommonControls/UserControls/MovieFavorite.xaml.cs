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
    public sealed partial class MovieFavorite : UserControl
    {
        bool check = false;
        ShowList selecteditem = null;
        public MovieFavorite()
        {
            try
            {
            this.InitializeComponent();
            progressbar.IsActive = true;
            Loaded += MovieFavorite_Loaded;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in MovieFavorite Method In MovieFavorite.cs file", ex);
            }
        }

        void MovieFavorite_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                GetPageDataInBackground();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in MovieFavorite_Loaded Method In MovieFavorite.cs file", ex);
            }
        }

        public void GetPageDataInBackground()
        {
            try
            {
                if (AppSettings.FavoritesSelectedIndex != "")
                {
                    grdvwfavoriteMovies.ItemsSource = null;
                    grdvwfavoriteMovies.Items.Clear();
                    AppSettings.FavoritesSelectedIndex = "";
                }
                List<ShowList> objfavlist = new List<ShowList>();
                //if (AppSettings.ProjectName =="Video Mix")
                //{
                //    objfavlist = DownloadManager.GetMoviefav();
                //}
                //else
                //{
                objfavlist = FavoritesManager.GetFavoriteByMovie(LinkType.Movies);
                //}
                if (objfavlist.Count != 0)
                {
                    progressbar.IsActive = false;
                    grdvwfavoriteMovies.ItemsSource = objfavlist;
                }
                else
                {
                    progressbar.IsActive = false;

                    if (AppSettings.ProjectName == "Video Mix")
                        txtmsg.Text = "No video mixes  available";
                    else if (AppSettings.ProjectName == "Web Tile")
                        txtmsg.Text = "No tiles available";
                    else
                        txtmsg.Text = "No movies  available";

                    txtmsg.Visibility = Visibility.Visible;
                }
                //BackgroundHelper bwHelper = new BackgroundHelper();
                //bwHelper.AddBackgroundTask(
                //                                  (object s, DoWorkEventArgs a) =>
                //                                  {
                //                                      a.Result = FavoritesManager.GetFavoriteByMovie(LinkType.Movies);
                //                                  },
                //                                  (object s, RunWorkerCompletedEventArgs a) =>
                //                                  {

                //                                      grdvwfavoriteMovies.ItemsSource = (List<ShowList>)a.Result;

                //                                  }
                //                                );
                //bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In MovieFavorite.cs file", ex);
            }
        }

        private void StackPanel_RightTapped_1(object sender, RightTappedRoutedEventArgs e)
        {
            try
            {
                Constants.selecteditem = null;
                check = true;
                Constants.Favoriteappbarvisible = true;
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("appbar").Invoke(p, new object[] { true });
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in StackPanel_RightTapped_1 Method In MovieFavorite.cs file", ex);
            }
        }

        private void grdvwRelatedMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (grdvwfavoriteMovies.SelectedIndex == -1)
                    return;
                if (check == true)
                {
                    check = false;
                    selecteditem = (sender as Selector).SelectedItem as ShowList;

                    AppSettings.ShowID = (grdvwfavoriteMovies.SelectedItem as ShowList).ShowID.ToString();
                    AppSettings.FavoritesSelectedIndex = grdvwfavoriteMovies.SelectedIndex.ToString();
                    grdvwfavoriteMovies.SelectedIndex = -1;
                    return;

                }
                AppSettings.ShowID = (grdvwfavoriteMovies.SelectedItem as ShowList).ShowID.ToString();
                AppSettings.FavTitle = (grdvwfavoriteMovies.SelectedItem as ShowList).Title;
                AppSettings.TileImage = (grdvwfavoriteMovies.SelectedItem as ShowList).TileImage;
                AppSettings.ChannelImage = (grdvwfavoriteMovies.SelectedItem as ShowList).Title;
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("DetailPage").Invoke(p, null);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in grdvwRelatedMovies_SelectionChanged Method In MovieFavorite.cs file", ex);
            }
        }
    }
}
