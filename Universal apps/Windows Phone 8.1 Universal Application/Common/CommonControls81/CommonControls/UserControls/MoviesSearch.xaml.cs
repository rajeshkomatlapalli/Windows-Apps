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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class MoviesSearch : UserControl
    {
        public MoviesSearch()
        {
            try
            {
            this.InitializeComponent();
            progressbar.IsActive = true;
            Loaded += MoviesSearch_Loaded;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in MoviesSearch Method In MoviesSearch.cs file", ex);
            }
        }

        void MoviesSearch_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GetPageDataInBackground();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in MoviesSearch_Loaded Method In MoviesSearch.cs file", ex);
            }
        }

        private void GetPageDataInBackground()
        {
            try
            {
                List<ShowList> objlist = new List<ShowList>();
                BackgroundHelper bwHelper = new BackgroundHelper();

                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                a.Result = SearchManager.GetShowsBySearch(AppSettings.SearchText);
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                objlist = (List<ShowList>)a.Result;
                                                if (objlist.Count != 0)
                                                {
                                                    progressbar.IsActive = false;
                                                    grdvwsearchMovies.ItemsSource = objlist; //SearchManager.GetShowsBySearch(AppSettings.SearchText);
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

                                            }
                                          );


                bwHelper.RunBackgroundWorkers();

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In MoviesSearch.cs file", ex);
            }
        }

        private void grdvwsearchMovies_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                AppSettings.ShowID = (grdvwsearchMovies.SelectedItem as ShowList).ShowID.ToString();
                AppSettings.FavTitle = (grdvwsearchMovies.SelectedItem as ShowList).Title;
                AppSettings.TileImage = (grdvwsearchMovies.SelectedItem as ShowList).TileImage;
                AppSettings.ChannelImage = (grdvwsearchMovies.SelectedItem as ShowList).Title;
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("DetailPage").Invoke(p, null);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in grdvwsearchMovies_SelectionChanged_1  Method In MoviesSearch.cs file", ex);
            }
        }
    }
}
