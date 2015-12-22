using Common.Library;
using Common.Utilities;
using MyToolkit.Multimedia;
using OnlineVideos;
using OnlineVideos.Data;
using OnlineVideos.UserControls;
using OnlineVideos.View_Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineMovies.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VideoGamesDetail : Page
    {
        string background = "0";
        public VideoGamesDetail()
        {
            this.InitializeComponent();
            StatusBar statusbar = StatusBar.GetForCurrentView();
            statusbar.HideAsync();
            Loaded += VideoGamesDetail_Loaded;
            if (AppSettings.ShowID != "0")
            {
                LoadPivotThemes(AppSettings.ShowUniqueID);
                //TODO: Save title also in settings or pass through query string
                tblkVideosTitle.Text = OnlineShow.GetShowDetail(AppSettings.ShowUniqueID).Title;
                background = "1";
                if (!ShowCastManager.ShowGamePivot(AppSettings.ShowID))
                {
                    pvtMainDetails.Items.Remove(gamepivot);
                }
            }
        }

        private async void LoadPivotThemes(long ShowID)
        {
            pvtMainDetails.Background =await ShowDetail.LoadShowPivotBackground(ShowID);
        }

        void VideoGamesDetail_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //FlurryWP8SDK.Api.LogEvent("VideoGamesDetail Page", true);
                performanceProgressBar.IsIndeterminate = true;

                string id = "";                
                if (background == "0")
                {
                    LoadPivotThemes(AppSettings.ShowUniqueID);
                    tblkVideosTitle.Text = OnlineShow.GetShowDetail(AppSettings.ShowUniqueID).Title;
                }
                performanceProgressBar.IsIndeterminate = false;
                PageHelper.RemoveEntryFromBackStack("VideoGameDetail");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In VideoGamesDetail.cs file.", ex);
            }
        }
       
        showvideolbx ShowVideoslbx;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                //FlurryWP8SDK.Api.LogPageView();               
                if (ShowVideoslbx.SVListBox.IsEnabled == false)
                    ShowVideoslbx.SVListBox.IsEnabled = true;               
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In VideoGamesDetail.cs file.", ex);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
                //FlurryWP8SDK.Api.EndTimedEvent("VideoGamesDetail Page");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom Method In VideoGamesDetail.cs file.", ex);
            }
        }

        private void imgTitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
