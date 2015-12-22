using Common.Library;
using OnlineMovies.Views;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideos.UserControls
{
    public sealed partial class ComedyHistory : UserControl
    {

        History objHistory = new History();
        List<LinkHistory> showHistoryVideos = null;
        string link = string.Empty;
        private bool IsDataLoaded;
        int songLastIndex = 1;
        int songFirstIndex = 1;
        int songsHisCount = 0;

        public ComedyHistory()
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
            songLastIndex = objHistory.GetLastHisId(Constants.SongHistoryFile);
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
                                                           lbxComedyHistory.ItemsSource = showHistoryVideos;
                                                           tblkHistoryNoComedy.Visibility = Visibility.Collapsed;
                                                       }
                                                       else
                                                       {
                                                           tblkHistoryNoComedy.Text = "No " + AppResources.ComedyHistoryPivotTitle + " available";
                                                           tblkHistoryNoComedy.Visibility = Visibility.Visible;
                                                       }
                                                   }
                                                   else
                                                   {
                                                       tblkHistoryNoComedy.Text = "No " + AppResources.ComedyHistoryPivotTitle + " available";
                                                       tblkHistoryNoComedy.Visibility = Visibility.Visible;
                                                   }
                                               }
                                             );
            bwHelper.RunBackgroundWorkers();
        }

        private void lbxComedyHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxComedyHistory.SelectedIndex == -1)
                    return;
                AppSettings.LinkTitle = (lbxComedyHistory.SelectedItem as LinkHistory).Title.ToString();
                AppSettings.LinkType = "Comedy";
                State.BackStack = "comedy";
                if (ResourceHelper.AppName == Apps.Web_Media.ToString())
                {
                    
                }
                else
                {
                    Frame frame = Windows.UI.Xaml.Window.Current.Content as Frame;
                    Page p = frame.Content as Page;                   
                    LinkHistory myid= lbxComedyHistory.SelectedItem as LinkHistory;
                    p.Frame.Navigate(typeof(Youtube), myid);                   
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxSongHistory_SelectionChanged Method In VideosHistory.cs file.", ex);
            }
            lbxComedyHistory.SelectedIndex = -1;
        }
    }
}