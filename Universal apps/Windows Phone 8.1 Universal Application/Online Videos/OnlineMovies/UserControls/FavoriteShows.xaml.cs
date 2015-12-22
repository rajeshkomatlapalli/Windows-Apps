using Common.Library;
using Common.Utilities;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.UI;
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
using OnlineVideos.Views;
using OnlineMovies.Views;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideos.UserControls
{
    public sealed partial class FavoriteShows : UserControl
    {

        #region GlobalDeclaration

        List<ShowList> showFavShows = null;
        string link = string.Empty;
        public static dynamic GetFavoriteShows = default(dynamic);
        #endregion

        #region Constructor

        public FavoriteShows()
        {
            this.InitializeComponent();
            showFavShows = new List<ShowList>();
        }
        #endregion

        #region Events
        private void lbxMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] parameters = new string[2];
            Frame frame = Window.Current.Content as Frame;
            Page p = frame.Content as Page;
            try
            {
                if (lbxVidoes.SelectedIndex == -1)
                    return;                
                if (ResourceHelper.ProjectName == "VideoMix" || ResourceHelper.AppName == Apps.Web_Tile.ToString())
                {
                    Constants.navigationfrom ="FavoriteShows";                                                           
                    parameters[0] = AppSettings.ShowID;
                    parameters[1] = null;
                    p.Frame.Navigate(typeof(Details), parameters);
                }
                else
                {
                    if (!AppResources.ShowFavoritesPageMoviesPivot)
                    {
                        List<ShowLinks> showLinks = OnlineShow.GetShowLinksByTypeForWp8((lbxVidoes.SelectedItem as ShowLinks).ShowID.ToString(), LinkType.Movies);
                        foreach (ShowLinks objLinks in showLinks)
                        {
                            link = objLinks.LinkUrl;
                        }                        
                        p.Frame.Navigate(typeof(Browser), link);                                                
                    }
                    else
                    {
                        if (!AppResources.IsSimpleDetailPage)
                        {
                            Pivot p1 = (Pivot)(p.FindName("PvtMainFavorites"));
                            if (p1 != null)
                            {
                                parameters[0] = (lbxVidoes.SelectedItem as ShowList).ShowID.ToString();
                                parameters[1] = p1.SelectedItem.GetType().GetRuntimeProperty("Header").GetValue((object)p1.SelectedItem, null).ToString();

                                p.Frame.Navigate(typeof(Details),parameters);                              
                            }
                        }
                        else
                        {                            
                            Pivot p1 = (Pivot)(p.FindName("pvtMainFavorites"));
                            if (p1 != null)
                            {
                                if (ResourceHelper.AppName == Apps.World_Dance.ToString())
                                {
                                    parameters[0]=(lbxVidoes.SelectedItem as ShowList).ShowID.ToString();
                                    parameters[1]=p1.SelectedItem.GetType().GetRuntimeProperty("Header").GetValue((object)p1.SelectedItem, null).ToString();

                                     p.Frame.Navigate(typeof(DanceDetailPage),parameters);
                                }                                    
                                    else if(ResourceHelper.AppName==Apps.Video_Games.ToString())
                                    {
                                        parameters[0]=(lbxVidoes.SelectedItem as ShowList).ShowID.ToString();
                                        parameters[1]=p1.SelectedItem.GetType().GetRuntimeProperty("Header").GetValue((object)p1.SelectedItem, null).ToString();
                                    }                                    
                                }
                                else
                                {
                                    parameters[0]=(lbxVidoes.SelectedItem as ShowList).ShowID.ToString();
                                    parameters[1]=p1.SelectedItem.GetType().GetRuntimeProperty("Header").GetValue((object)p1.SelectedItem, null).ToString();
                                    p.Frame.Navigate(typeof(MusicDetail),parameters);                                    
                                }
                            }
                        }
                    }
                }            
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxMovies_SelectionChanged Method In FavoriteShows.cs file.", ex);
            }
            lbxVidoes.SelectedIndex = -1;
        }
        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Constants.UIThread = true;
            GetFavoriteShows = FavoritesManager.GetFavoriteShows();
            Constants.UIThread = false;
            GetPageDataInBackground();
        }

        #region "Common Methods"

        private void GetPageDataInBackground()
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();
                bwHelper.AddBackgroundTask(
                                                   (object s, DoWorkEventArgs a) =>
                                                   {                                                                                                              
                                                       a.Result = GetFavoriteShows;
                                                   },
                                                   (object s, RunWorkerCompletedEventArgs a) =>
                                                   {
                                                       showFavShows = (List<ShowList>)a.Result;
                                                       if (showFavShows.Count > 0)
                                                       {
                                                           lbxVidoes.ItemsSource = showFavShows;

                                                           tblkFavShows.Visibility = Visibility.Collapsed;
                                                       }
                                                       else
                                                       {
                                                           tblkFavShows.Text = "No " + AppResources.FavoriteMoviesPivotTitle + " available";
                                                           tblkFavShows.Text = "No " + "Movies" + " available";
                                                           tblkFavShows.Visibility = Visibility.Visible;
                                                       }
                                                   }
                                                 );
                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In FavoriteShows.cs file.", ex);
            }
        }
        #endregion

    }
}
