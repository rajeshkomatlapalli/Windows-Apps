using Common.Library;
using OnlineMovies.Views;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideos.UserControls
{
    public sealed partial class VideosHistory : UserControl
    {
        History objHistory = new History();
        List<LinkHistory> showHistoryVideos = null;
        string link = string.Empty;
        private bool IsDataLoaded;
        int songLastIndex = 1;
        int songFirstIndex = 1;
        int songsHisCount = 0;

        public VideosHistory()
        {
            this.InitializeComponent();
            IsDataLoaded = false;
            showHistoryVideos = new List<LinkHistory>();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsDataLoaded == false)
                {
                    GetPageDataInBackground();
                    IsDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in UserControl_Loaded Method In VideosHistory.cs file.", ex);
            }
        }

        private void GetPageDataInBackground()
        {
            songLastIndex =objHistory.GetLastHisId(Constants.SongHistoryFile);
            songFirstIndex = songFirstIndex - 14;
            if (songFirstIndex < 0)
                songFirstIndex = 1;
            songsHisCount = 15;
            BackgroundHelper bwHelper = new BackgroundHelper();
            bwHelper.AddBackgroundTask(
                                               (object s, DoWorkEventArgs a) =>
                                               {
                                                   a.Result = objHistory.LoadSongHistory(songFirstIndex, songLastIndex, songsHisCount);
                                               },
                                               (object s, RunWorkerCompletedEventArgs a) =>
                                               {
                                                   showHistoryVideos = (List<LinkHistory>)a.Result;
                                                   if (showHistoryVideos != null)
                                                   {
                                                       if (showHistoryVideos.Count > 0)
                                                       {
                                                           lbxSongHistory.ItemsSource = showHistoryVideos;
                                                           tblkHistoryNoSongs.Visibility = Visibility.Collapsed;
                                                       }
                                                       else
                                                       {
                                                           tblkHistoryNoSongs.Text = "No " + AppResources.FavoriteSongsPivotTitle + " available";
                                                           tblkHistoryNoSongs.Visibility = Visibility.Visible;
                                                       }
                                                   }
                                                   else
                                                   {
                                                       tblkHistoryNoSongs.Text = "No " + AppResources.FavoriteSongsPivotTitle + " available";
                                                       tblkHistoryNoSongs.Visibility = Visibility.Visible;
                                                   }
                                               }
                                             );
            bwHelper.RunBackgroundWorkers();
        }
        private void lbxSongHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxSongHistory.SelectedIndex == -1)
                    return;
                AppSettings.LinkTitle = (lbxSongHistory.SelectedItem as LinkHistory).Title.ToString();
                AppSettings.LinkType = "Songs";
                State.BackStack = "songs";
                Frame frame = Window.Current.Content as Frame;
                Page p = frame.Content as Page;                
                p.Frame.Navigate(typeof(Youtube),(lbxSongHistory.SelectedItem as LinkHistory).LinkUrl);               
            }
            catch (Exception ex)
            {
              Exceptions.SaveOrSendExceptions("Exception in lbxSongHistory_SelectionChanged Method In VideosHistory.cs file.", ex);
            }
            lbxSongHistory.SelectedIndex = -1;
        }
    }
}
