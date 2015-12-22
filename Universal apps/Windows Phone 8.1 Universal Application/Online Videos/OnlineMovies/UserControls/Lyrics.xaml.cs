using Common.Library;
using Common.Utilities;
using OnlineMovies.Views;
using OnlineVideos.Data;
using OnlineVideos.Entities;
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
    public sealed partial class Lyrics : UserControl
    {
        #region GlobalDeclaration
        AppInsights insights = new AppInsights();
        List<ShowLinks> objLiricsList;
        private bool IsDataLoaded;
        #endregion

        #region Constructor
        public Lyrics()
        {
            this.InitializeComponent();
            objLiricsList = new List<ShowLinks>();
            IsDataLoaded = false;
        }
        #endregion

        #region Events
        
        private void lbxLyricsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxLyricsList.SelectedIndex == -1)
                return;
            AppSettings.LiricsLink = (lbxLyricsList.SelectedItem as ShowLinks).Title;
            insights.Event(AppSettings.LiricsLink + "Viewed");
            Frame frame = Window.Current.Content as Frame;
            Page p = frame.Content as Page;

            string name = AppResources.LiricsDetailPageName;//To Know the page name for navigate....(ShowLiricsDetail--Page)
            //PageHelper.NavigateToLiricsShowPage(AppResources.LiricsDetailPageName, AppSettings.LiricsLink);
            p.Frame.Navigate(typeof(ShowLiricsDetail), AppSettings.LiricsLink);
        }
        #endregion

        #region PageLoad
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            insights.Event("lyrics loaded");
            if (IsDataLoaded == false)
            {
                GetPageDataInBackground();
                IsDataLoaded = true;
            }
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
                                                a.Result = OnlineShow.GetShowLinksByTypeForWp8(AppSettings.ShowID, LinkType.Audio/*, false*/);
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                objLiricsList = (List<ShowLinks>)a.Result;
                                                List<ShowLinks> lyricshowlink = new List<ShowLinks>();
                                                foreach (ShowLinks songlyrics in objLiricsList)
                                                {
                                                    if (songlyrics.Description != "")
                                                    {
                                                        lyricshowlink.Add(songlyrics);
                                                    }
                                                    else
                                                    {
                                                    }
                                                }
                                                if (lyricshowlink.Count > 0)
                                                {
                                                    lbxLyricsList.ItemsSource = lyricshowlink;

                                                    tblkFavNoLyrics.Visibility = Visibility.Collapsed;
                                                }
                                                else
                                                {
                                                    tblkFavNoLyrics.Text = "Lyrics not available";
                                                    tblkFavNoLyrics.Visibility = Visibility.Visible;
                                                }
                                            }
                                          );

                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In Lyrics.cs file.", ex);
            }
        }
        #endregion
    }
}
