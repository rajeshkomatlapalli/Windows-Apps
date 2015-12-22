using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class AudioHistory : UserControl
    {
        #region GlobalDeclaration
        History objHistory = new History();
        List<LinkHistory> objAudioList;
        int movieLastIndex = 1;
        int movieFirstIndex = 1;
        int moviesHisCount = 0;
        #endregion

        #region Constructor
        public AudioHistory()
        {
            this.InitializeComponent();
            GetPageDataInBackground();
        }
        #endregion

        #region "Common Methods"
        private void GetPageDataInBackground()
        {

            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();

                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                a.Result = LoadAudioHistory();
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                objAudioList = (List<LinkHistory>)a.Result;
                                                if (objAudioList != null)
                                                {
                                                    if (objAudioList.Count > 0)
                                                    {
                                                        lbxAudioHistory.ItemsSource = objAudioList;
                                                        tblkFavNoAudio.Visibility = Visibility.Collapsed;
                                                    }
                                                    else
                                                    {
                                                        tblkFavNoAudio.Visibility = Visibility.Visible;
                                                        tblkFavNoAudio.Text = "No " + AppResources.ShowFavoriteAudioPivotTitle + " available";
                                                    }
                                                }
                                                else
                                                {
                                                    tblkFavNoAudio.Visibility = Visibility.Visible;
                                                    tblkFavNoAudio.Text = "No " + AppResources.ShowFavoriteAudioPivotTitle + " available";
                                                }
                                            }
                                          );

                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In AudioHistory.cs file.", ex);
            }

        }
        public List<LinkHistory> LoadAudioHistory()
        {
            List<LinkHistory> objmovie = new List<LinkHistory>();
            try
            {
                movieLastIndex = objHistory.GetLastHisId(Constants.AudioHistoryFile);
                movieFirstIndex = movieFirstIndex - 14;
                if (movieFirstIndex < 0)
                    movieFirstIndex = 1;
                moviesHisCount = 15;                
                objmovie =objHistory.LoadAudioHistory(movieFirstIndex, movieLastIndex, moviesHisCount);                
                return objmovie;
            }

            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAudioHistory Method In AudioHistory.cs file.", ex);
                return null;
            }
        }
        #endregion

        #region Events
        private  void lbxAudioHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxAudioHistory.SelectedIndex == -1)
                return;
            ShowAudio audio = new ShowAudio();
            var nn=  lbxAudioHistory.SelectedItem as LinkHistory;
            audio.LoadDownLoads(lbxAudioHistory.SelectedItem as LinkHistory);
            AppSettings.SongID = (lbxAudioHistory.SelectedItem as LinkHistory).ID.ToString();
            AppSettings.AudioImage = (lbxAudioHistory.SelectedItem as LinkHistory).Songplay;
            AppSettings.LinkTitle = (lbxAudioHistory.SelectedItem as LinkHistory).Title.ToString();
            AppSettings.LinkType = "Audio";
            objAudioList = LoadAudioHistory();
            lbxAudioHistory.ItemsSource = objAudioList;
            lbxAudioHistory.SelectedIndex = -1;
            State.BackStack = "audio";
        }
        #endregion

    }
}
