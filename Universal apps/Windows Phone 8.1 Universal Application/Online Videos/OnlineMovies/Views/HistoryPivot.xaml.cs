using Common.Library;
using OnlineVideos;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OnlineMovies.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HistoryPivot : Page
    {
        #region Global
        private SolidColorBrush adcontrolborder = new SolidColorBrush(Colors.Transparent);
        History objHistory = new History();
        string link = string.Empty;
        int movieLastIndex = 1;
        int movieFirstIndex = 1;
        int songLastIndex = 1;
        int songFirstIndex = 1;
        int moviesHisCount = 0;
        int songsHisCount = 0;
        bool newPageInstance = false;
        #endregion

        public HistoryPivot()
        {
            this.InitializeComponent();
            newPageInstance = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {               
                LoadPivotThemes();
                LoadSongHistory();

                if (newPageInstance)
                {
                    LoadVideoHistory();
                }

                newPageInstance = false;
                if (pvtMainHistory.Items.Count >= 2)
                    pvtMainHistory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In HistoryPivot.cs file.", ex);
            }
        }

        private void imgTitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void lbxVidoes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxSongHistory.SelectedIndex == -1)
                    return;

                //YoutubeApi.Youtube.PlayYoutubeVideo((lbxSongHistory.SelectedItem as ShowLinks).LinkUrl);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxSongHistory_SelectionChanged Method In HistoryPivot.cs file.", ex);
            }
            lbxSongHistory.SelectedIndex = -1;
        }

        private void LoadVideoHistory()
        {
            try
            {
                //if (!AppResources.ShowFavoritesPageMoviesPivot)
                //{
                //    pvtitmVideos.Visibility = Visibility.Collapsed;
                //    pvtMainHistory.Items.Remove(pvtitmVideos);
                //}
                //else
                //{
                    pvtitmVideos.Visibility = Visibility.Visible;

                    pvtitmVideos.Header = AppResources.FavoriteMoviesPivotTitle;
                    movieLastIndex = objHistory.GetLastHisId(Constants.MovieHistoryFile);
                    movieFirstIndex = movieFirstIndex - 14;
                    if (movieFirstIndex < 0)
                        movieFirstIndex = 1;
                    moviesHisCount = 15;
                    List<LinkHistory> objmovie = objHistory.LoadMovieHistory(movieFirstIndex, movieLastIndex, moviesHisCount);
                    if (objmovie.Count > 0)
                        lbxVidoes.ItemsSource = objmovie;
                    else
                        tblk.Text = "No " + AppResources.FavoriteMoviesPivotTitle + " available";
                    tblk.Visibility = Visibility.Visible;
                //}
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadVideoHistory Method In HistoryPivot.cs file.", ex);
            }
        }

        private void lbxSongHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxSongHistory.SelectedIndex == -1)
                    return;

                //YoutubeApi.Youtube.PlayYoutubeVideo((lbxSongHistory.SelectedItem as ShowLinks).LinkUrl);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxSongHistory_SelectionChanged Method In HistoryPivot.cs file.", ex);
            }
            lbxSongHistory.SelectedIndex = -1;
        }
        private void LoadSongHistory()
        {
            try
            {
                songLastIndex = objHistory.GetLastHisId(Constants.SongHistoryFile);
                songFirstIndex = songFirstIndex - 14;
                if (songFirstIndex < 0)
                    songFirstIndex = 1;
                songsHisCount = 15;
                List<LinkHistory> objsong = objHistory.LoadSongHistory(songFirstIndex, songLastIndex, songsHisCount);
                if (objsong.Count > 0)
                {
                    lbxSongHistory.ItemsSource = objsong;
                    tblkFavNoSongs.Visibility = Visibility.Collapsed;
                }
                else
                    tblkFavNoSongs.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadSongHistory Method In HistoryPivot.cs file.", ex);
            }
        }

        private void LoadPivotThemes()
        {
            try
            {
                if (AppResources.ShowDetailPageAudioVideoHeaders)
                {
                    pvtitemSongs.Header = AppResources.FavoriteSongsPivotTitle;
                }
                else
                    pvtitemSongs.Header = "";
                tblkFavNoSongs.Text = "No " + AppResources.FavoriteSongsPivotTitle + " available";
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadPivotThemes Method In HistoryPivot.cs file.", ex);
            }
        }
    }
}
