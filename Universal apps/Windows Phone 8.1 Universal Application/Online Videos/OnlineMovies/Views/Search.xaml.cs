using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using System.Reflection;
using OnlineMovies.Views;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineVideos
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Search : Page
    {

        #region Global

        List<CastProfile> matchedPeople = null;
        List<ShowList> matchedMovies = null;
        List<ShowLinks> matchedLinks = null;
        List<ShowLinks> matchedaudioLinks = null;
        List<QuizList> matchedSubjects = null;
        string searchText;

        #endregion

        #region Constructor
        public Search()
        {
            try
            {
            this.InitializeComponent();
            matchedMovies = new List<ShowList>();
            matchedLinks = new List<ShowLinks>();
            matchedaudioLinks = new List<ShowLinks>();
            matchedPeople = new List<CastProfile>();
            matchedSubjects = new List<QuizList>();
            LoadPivotThemes();
            if (ResourceHelper.AppName != Apps.Indian_Cinema_Pro.ToString() && ResourceHelper.AppName != "Kids_TV.WindowsPhone" && ResourceHelper.AppName != Apps.Story_Time_Pro.ToString())
            { 
               
            }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Search Method In Search file.", ex);
            }
        }
        #endregion

        private void LoadAds()
        {
            try
            {
                LoadAdds.LoadAdControl_New(LayoutRoot, adstSearch, 1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In SongDetails file", ex);
                string excepmess = "Exception in LoadAds Method In SongDetails file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }

        }

        private void LoadPivotThemes()
        {
            try
            {
                //if (AppResources.ShowCricketDetailPage)
                //{
                    //pvtitmVideos.Visibility = Visibility.Visible;
                    //pvtitmVideos.Header = AppResources.FavoriteMoviesPivotTitle;
                    //pvtitmCast.Visibility = Visibility.Visible;
                    //pvtitmCast.Header = AppResources.FavoritePeoplePivotTitle;
                    //pvtitemAudioSongs.Visibility = Visibility.Collapsed;
                    //pvtMainSearch.Items.Remove(pvtitemAudioSongs);
                    //pvtitemSongs.Visibility = Visibility.Collapsed;
                    //pvtMainSearch.Items.Remove(pvtitemSongs);
                    //pvtitemquiz.Visibility = Visibility.Collapsed;
                    //pvtMainSearch.Items.Remove(pvtitemquiz);
                //}
                //else
                //{
                    if (ResourceHelper.AppName == Apps.Web_Tile.ToString())
                    {
                        pvtitmVideos.Visibility = Visibility.Visible;
                        pvtitmVideos.Header = AppResources.FavoriteMoviesPivotTitle;
                        pvtitemSongs.Visibility = Visibility.Collapsed;
                        pvtMainSearch.Items.Remove(pvtitemSongs);
                        pvtitemAudioSongs.Visibility = Visibility.Collapsed;
                        pvtMainSearch.Items.Remove(pvtitemAudioSongs);
                        pvtitemquiz.Visibility = Visibility.Collapsed;
                        pvtMainSearch.Items.Remove(pvtitemquiz);
                    }
                    else
                    {
                        if (AppResources.ShowDetailPageAudioPivot)
                        {
                            pvtitemAudioSongs.Visibility = Visibility.Visible;
                            pvtitemAudioSongs.Header = AppResources.FavoriteAudioPivotTitle;
                        }
                        else
                        {
                            pvtitemAudioSongs.Visibility = Visibility.Collapsed;
                            pvtMainSearch.Items.Remove(pvtitemAudioSongs);
                        }
                        if (!AppResources.ShowFavoritesPageMoviesPivot || !AppResources.ShowDetailPageAudioPivot)
                        {
                            pvtitmVideos.Visibility = Visibility.Collapsed;
                            pvtMainSearch.Items.Remove(pvtitmVideos);
                        }
                        else
                        {
                            pvtitmVideos.Visibility = Visibility.Visible;
                            pvtitmVideos.Header = AppResources.FavoriteMoviesPivotTitle;
                        }
                        
                        if (AppResources.ShowCastPanoramaPage)
                        {
                            pvtitmCast.Visibility = Visibility.Visible;
                            pvtitmCast.Header = AppResources.FavoritePeoplePivotTitle;
                        }
                        else
                        {
                            pvtitmCast.Visibility = Visibility.Collapsed;
                            pvtMainSearch.Items.Remove(pvtitmCast);
                        }
                    if (!AppResources.ShowQuiz)
                    {
                        pvtitemquiz.Visibility = Visibility.Collapsed;
                        pvtMainSearch.Items.Remove(pvtitemquiz);
                    }
                    else
                    {
                        pvtitemquiz.Visibility = Visibility.Visible;
                        pvtitemquiz.Header = AppResources.ShowQuizPivotTitle;
                        pvtitemAudioSongs.Visibility = Visibility.Collapsed;
                        pvtMainSearch.Items.Remove(pvtitemAudioSongs);
                    }
                }
                //}
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In Search file.", ex);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            searchText = (string)e.Parameter;
            try
            {
               // FlurryWP8SDK.Api.LogPageView();
                _performanceProgressBar.IsIndeterminate = true;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In Search file.", ex);
            }            
            //HardwareButtons.BackPressed += HardwareButtons_BackPressed;
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
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom Method In Search file.", ex);
            }
        }

        private void imgTitle_Tapped(object sender, TappedRoutedEventArgs e)
        {            
            Frame.Navigate(typeof(MainPage));
        }

        private void lbxVideos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxVidoes.SelectedIndex == -1)
                    return;
                AppState.searchtitle = string.Empty;
                AppSettings.ShowID = (lbxVidoes.SelectedItem as ShowList).ShowID.ToString();
                AppSettings.Title = (lbxVidoes.SelectedItem as ShowList).Title.ToString();
                string[] parameters = new string[2];
                parameters[0] = (lbxVidoes.SelectedItem as ShowList).ShowID.ToString();
                parameters[1] = pvtMainSearch.SelectedItem.GetType().GetRuntimeProperty("Header").GetValue((object)this.pvtMainSearch.SelectedItem, null).ToString();
                //Frame.Navigate(typeof(Details), parameters);
                lbxVidoes.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In Search file.", ex);
            }
        }

        private void lbxSearchSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxSearchSongs.SelectedIndex == -1)
                    return;
                AppSettings.LinkTitle = (lbxSearchSongs.SelectedItem as ShowLinks).Title.ToString();
                AppSettings.LinkType = (lbxSearchSongs.SelectedItem as ShowLinks).LinkType.ToString();
                if (ResourceHelper.AppName == Apps.Web_Media.ToString())
                {

                }
                else if (!AppResources.IsSimpleDetailPage)
                {
                    AppState.searchtitle = (lbxSearchSongs.SelectedItem as ShowLinks).Title;
                    string[] parameters = new string[2];
                    parameters[0] = (lbxSearchSongs.SelectedItem as ShowLinks).ShowID.ToString();
                    parameters[1] = pvtMainSearch.SelectedItem.GetType().GetRuntimeProperty("Header").GetValue((object)this.pvtMainSearch.SelectedItem, null).ToString();
                    //Frame.Navigate(typeof(Details), parameters);
               }
                else
                {
                    AppSettings.ShowID = (lbxSearchSongs.SelectedItem as ShowLinks).ShowID.ToString();
                    AppState.searchtitle = (lbxSearchSongs.SelectedItem as ShowLinks).Title;
                    if (ResourceHelper.AppName == Apps.World_Dance.ToString())
                    {
                       
                    }
                    else
                    {                           
                        string[] parameters = new string[2];
                        parameters[0] = AppSettings.ShowID.ToString();
                        parameters[1] = null;
                        //Frame.Navigate(typeof(SubjectDetail), parameters);
                    }
                } lbxSearchSongs.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                lbxSearchSongs.SelectedIndex = -1;
                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In Search file.", ex);
            }
        }

        private void lbxSearchAudioSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] parameters = new string[2];
            try
            {
                if (lbxSearchAudioSongs.SelectedIndex == -1)
                    return;
                AppSettings.ShowID = (lbxSearchAudioSongs.SelectedItem as ShowLinks).ShowID.ToString();
                AppState.searchtitle = (lbxSearchAudioSongs.SelectedItem as ShowLinks).Title.ToString();
                AppSettings.LinkTitle = (lbxSearchAudioSongs.SelectedItem as ShowLinks).Title.ToString();
                AppSettings.LinkType = (lbxSearchAudioSongs.SelectedItem as ShowLinks).LinkType.ToString();
                if (!AppResources.IsSimpleDetailPage)
                {
                    Constants.topsongnavigation = "4";                    
                    parameters[0] = (lbxSearchAudioSongs.SelectedItem as ShowLinks).ShowID.ToString();
                    parameters[1] = pvtMainSearch.SelectedItem.GetType().GetRuntimeProperty("Header").GetValue((object)this.pvtMainSearch.SelectedItem, null).ToString();
                   // Frame.Navigate(typeof(Details), parameters);                    
                }
                else if (ResourceHelper.AppName == Apps.Web_Media.ToString())
                {                    
                    AppSettings.SongID = (lbxSearchAudioSongs.SelectedItem as ShowLinks).LinkID.ToString();
                    AppSettings.AudioImage = (lbxSearchAudioSongs.SelectedItem as ShowLinks).Songplay;
                }
                else
                {
                    Constants.topsongnavigation = "1";
                    parameters[0] = (lbxSearchAudioSongs.SelectedItem as ShowLinks).ShowID.ToString();
                    parameters[1] = (lbxSearchAudioSongs.SelectedItem as ShowLinks).Title;
                   // Frame.Navigate(typeof(Details), parameters);                    
                }
                lbxSearchAudioSongs.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                lbxSearchAudioSongs.SelectedIndex = -1;
                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In Search file.", ex);
            }
        }

        private void lbxComedy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxComedy.SelectedIndex == -1)
                    return;
                AppState.searchtitle = string.Empty;
                AppSettings.ShowID = (lbxComedy.SelectedItem as ShowList).ShowID.ToString();
                AppSettings.Title = (lbxComedy.SelectedItem as ShowList).Title.ToString();                
                lbxVidoes.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In Search file.", ex);
            }
        }

        private void lbxCast_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxCast.SelectedIndex == -1)
                    return;               
                AppSettings.PersonID = (lbxCast.SelectedItem as CastProfile).PersonID.ToString();

                if (AppResources.ShowCastPanoramaPage && (ResourceHelper.AppName != Apps.Kids_TV_Shows.ToString() && ResourceHelper.AppName != Apps.Kids_TV_Pro.ToString() && ResourceHelper.AppName != Apps.Kids_TV.ToString()))
                {                  
                    string[] parameters = new string[2];
                    parameters[0] = AppSettings.PersonID;
                    parameters[1] = "search";
                    //Frame.Navigate(typeof(CastHub), parameters);
                }
                else if (ResourceHelper.AppName == Apps.Kids_TV_Shows.ToString() || ResourceHelper.AppName == Apps.Kids_TV_Pro.ToString() || ResourceHelper.AppName == Apps.Kids_TV.ToString())
                {                    
                    Frame.Navigate(typeof(NavigationHelper), NavigationHelper.getCharacterDetailPage());                   
                }
                lbxCast.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In Search file.", ex);
            }
        }

        private void lbxSearchQuiz_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxSearchQuiz.SelectedIndex == -1)
                    return;
                AppSettings.ShowID = (lbxSearchQuiz.SelectedItem as QuizList).ShowID.ToString();
                string QuizTitle = (lbxSearchQuiz.SelectedItem as QuizList).Name.ToString();
                AppState.searchtitle = QuizTitle;
                //Frame.Navigate(typeof(NavigationHelper), NavigationHelper.getSubjectDetailPage((lbxSearchQuiz.SelectedItem as QuizList), QuizTitle));
                string[] parame = new string[2];
                parame[0] = AppSettings.ShowID.ToString();
                parame[1] = "Quiz";
                //Frame.Navigate(typeof(SubjectDetail), parame);
                lbxSearchQuiz.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                lbxSearchQuiz.SelectedIndex = -1;
                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In Search file.", ex);
            }
        }

        #region PageLoad
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadAds();
                GetPageDataInBackground();
                if (pvtMainSearch.Items.Count >= 2)
                    pvtMainSearch.SelectedIndex = 0;
                _performanceProgressBar.IsIndeterminate = false;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In Search file.", ex);
            }
        }
        #endregion

        private void GetPageDataInBackground()
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();
                //if (!AppResources.ShowCricketDetailPage)
                //{
                bwHelper.AddBackgroundTask(
                                           (object s, DoWorkEventArgs a) =>
                                           {

                                               a.Result = SearchManager.GetShowsLinksBySearch(searchText.ToLower(), LinkType.Songs);
                                           },
                                           (object s, RunWorkerCompletedEventArgs a) =>
                                           {
                                               matchedLinks = (List<ShowLinks>)a.Result;
                                               if (matchedLinks.Count > 0)
                                               {
                                                   lbxSearchSongs.ItemsSource = matchedLinks;
                                                   tblkFavNoSongs.Visibility = Visibility.Collapsed;
                                               }
                                               else
                                                   tblkFavNoSongs.Visibility = Visibility.Visible;
                                               if (ResourceHelper.ProjectName == "VideoMix")
                                               {
                                                   tblkFavNoSongs.Text = "No Video mixes Available";
                                               }
                                               else
                                               {
                                                   tblkFavNoSongs.Text = "No Videos Available";
                                               }
                                           }
                                         );
                //}

                if (AppResources.ShowFavoritesPageMoviesPivot)
                {
                    bwHelper.AddBackgroundTask(
                                                (object s, DoWorkEventArgs a) =>
                                                {
                                                    a.Result = SearchManager.GetShowsBySearch(searchText.ToLower());
                                                },
                                                (object s, RunWorkerCompletedEventArgs a) =>
                                                {
                                                    matchedMovies = (List<ShowList>)a.Result;
                                                    if (matchedMovies.Count > 0)
                                                    {
                                                        lbxVidoes.ItemsSource = matchedMovies;
                                                    }
                                                    else
                                                    {
                                                        tblk.Text = "No " + AppResources.FavoriteMoviesPivotTitle + " available";
                                                        tblk.Visibility = Visibility.Visible;
                                                    }
                                                }
                                              );
                }


                if (AppResources.ShowCastPanoramaPage)
                {
                    bwHelper.AddBackgroundTask(
                                                (object s, DoWorkEventArgs a) =>
                                                {
                                                    a.Result = SearchManager.GetPeopleBySearch(searchText.ToLower());
                                                },
                                                (object s, RunWorkerCompletedEventArgs a) =>
                                                {
                                                    matchedPeople = (List<CastProfile>)a.Result;
                                                    if (matchedPeople.Count > 0)
                                                    {
                                                        lbxCast.ItemsSource = matchedPeople;
                                                    }
                                                    else
                                                    {
                                                        tblkcast.Text = "No " + AppResources.FavoritePeoplePivotTitle + " available";
                                                        tblkcast.Visibility = Visibility.Visible;
                                                    }

                                                }
                                              );
                }

                if (AppResources.ShowQuiz)
                {
                    bwHelper.AddBackgroundTask(
                    (object s, DoWorkEventArgs a) =>
                    {
                        a.Result = SearchManager.GetSubjectsBySearch(searchText.ToLower());
                    },
                    (object s, RunWorkerCompletedEventArgs a) =>
                    {
                        matchedSubjects = (List<QuizList>)a.Result;
                        if (matchedSubjects.Count > 0)
                        {
                            lbxSearchQuiz.ItemsSource = matchedSubjects;
                        }
                        else
                        {
                            tblkquiz.Text = "No " + AppResources.ShowQuizPivotTitle + " available";
                            tblkquiz.Visibility = Visibility.Visible;
                        }

                    }
                    );
                }

                if (AppResources.ShowDetailPageAudioPivot)
                {
                    bwHelper.AddBackgroundTask(
                                           (object s, DoWorkEventArgs a) =>
                                           {

                                               a.Result = SearchManager.GetShowsLinksBySearch(searchText.ToLower(), LinkType.Audio);
                                           },
                                           (object s, RunWorkerCompletedEventArgs a) =>
                                           {
                                               matchedaudioLinks = (List<ShowLinks>)a.Result;
                                               if (matchedaudioLinks.Count > 0)
                                               {
                                                   lbxSearchAudioSongs.ItemsSource = matchedaudioLinks;
                                                   tblkFavNoAudioSongs.Visibility = Visibility.Collapsed;
                                               }
                                               else
                                                   tblkFavNoAudioSongs.Visibility = Visibility.Visible;
                                               tblkFavNoAudioSongs.Text = "No " + "Audio" + " available";
                                           }
                                         );
                }
                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Error in Getdatamethod in Search file", ex);
            }
        }
    
    }
}
