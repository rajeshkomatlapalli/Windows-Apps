using Common.Library;
using Common.Utilities;
using OnlineMovies.Views;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.UI;
using OnlineVideos.Views;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideos.UserControls
{
    public sealed partial class ShowHistory : UserControl
    {
        History objHistory = new History();
        List<LinkHistory> showHistoryVideos = null;
        string link = string.Empty;
        int movieLastIndex = 1;
        int movieFirstIndex = 1;
        public static dynamic LoadMovieHistory = default(dynamic);

        int moviesHisCount = 0;
        public ShowHistory()
        {
            this.InitializeComponent();
            showHistoryVideos = new List<LinkHistory>();
        }

        private void lbxVidoes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Frame frame = Windows.UI.Xaml.Window.Current.Content as Frame;
            Page p = frame.Content as Page;
            try
            {
                if (lbxVidoes.SelectedIndex == -1)
                    return;
                AppSettings.LinkTitle = (lbxVidoes.SelectedItem as LinkHistory).Title.ToString();
                AppSettings.LinkType = "Songs";
                if (!AppResources.ShowFavoritesPageMoviesPivot)
                {
                    List<ShowLinks> objsong = OnlineShow.GetShowLinksByTypeForWp8((lbxVidoes.SelectedItem as ShowLinks).ShowID.ToString(), LinkType.Movies);
                    foreach (ShowLinks objmovie in objsong)
                    {
                        link = objmovie.LinkUrl;
                    }                   
                    string myid=link;
                    p.Frame.Navigate(typeof(Youtube), myid);                   
                }
                else
                {
                    string[] parameter = new string[2];
                    if (!AppResources.IsSimpleDetailPage)
                    {
                        parameter[0] = (lbxVidoes.SelectedItem as LinkHistory).ShowID.ToString();
                        parameter[1] = "movies";                      
                        p.Frame.Navigate(typeof(Details), parameter);
                    }
                    else
                    {
                        if (ResourceHelper.AppName == Apps.Video_Mix.ToString() || ResourceHelper.AppName == Apps.Web_Tile.ToString())
                        {                            
                            if (ResourceHelper.AppName == Apps.Video_Mix.ToString())
                            {
                                parameter[0] = Convert.ToString((lbxVidoes.SelectedItem as LinkHistory).ShowID);
                                parameter[1] = null;                                
                            }
                            else
                            {
                                parameter[0] = Convert.ToString((lbxVidoes.SelectedItem as LinkHistory).ShowID);
                                parameter[1] = null;
                                p.Frame.Navigate(typeof(Details), parameter);
                            }                            
                        }
                        else                        
                        parameter[0] = Convert.ToString((lbxVidoes.SelectedItem as LinkHistory).ShowID);
                        parameter[1] = "movies";
                        p.Frame.Navigate(typeof(Details), parameter);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxVidoes_SelectionChanged Method In ShowHistory file.", ex);
            }
            lbxVidoes.SelectedIndex = -1;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {            
            try
            {
                movieLastIndex = objHistory.GetLastHisId(Constants.MovieHistoryFile);
                movieFirstIndex = movieFirstIndex - 14;
                if (movieFirstIndex < 0)
                    movieFirstIndex = 1;
                moviesHisCount = 15;
                Constants.UIThread = true;
                LoadMovieHistory = objHistory.LoadMovieHistory(movieFirstIndex, movieLastIndex, moviesHisCount);
                Constants.UIThread = false;
                BackgroundHelper bwHelper = new BackgroundHelper();
                bwHelper.AddBackgroundTask(
                                                   (object s, DoWorkEventArgs a) =>
                                                   {
                                                       //a.Result = objHistory.LoadMovieHistory(movieFirstIndex, movieLastIndex, moviesHisCount);
                                                       //a.Result = LoadMovieHistory;
                                                       a.Result = LoadMovieHistory;
                                                   },
                                                   (object s, RunWorkerCompletedEventArgs a) =>
                                                   {
                                                       showHistoryVideos = (List<LinkHistory>)a.Result;
                                                       if (showHistoryVideos != null)
                                                       {
                                                           LinkHistory linkhistory = new LinkHistory();

                                                           if (showHistoryVideos.Count > 0)
                                                           {
                                                               lbxVidoes.ItemsSource = showHistoryVideos;
                                                               tblkMoviesHistory.Visibility = Visibility.Collapsed;
                                                           }
                                                           else
                                                           {
                                                               tblkMoviesHistory.Text = "No " + AppResources.FavoriteMoviesPivotTitle + " available";
                                                               tblkMoviesHistory.Visibility = Visibility.Visible;
                                                           }
                                                       }
                                                       else
                                                       {
                                                           tblkMoviesHistory.Text = "No " + AppResources.FavoriteMoviesPivotTitle + " available";
                                                           tblkMoviesHistory.Visibility = Visibility.Visible;
                                                       }
                                                   }
                                                 );
                bwHelper.RunBackgroundWorkers();
               
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in UserControl_Loaded Method In ShowHistory file.", ex);
            }
        }
    }
}
