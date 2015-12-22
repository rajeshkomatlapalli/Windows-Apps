using Common.Library;
using Common.Utilities;
using Indian_Cinema;
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
using Windows.UI.Input;
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
    public sealed partial class FavouriteComedy : UserControl
    {
        #region GLobal Declaration

        List<ShowLinks> showFavComedy = null;
        private bool IsDataLoaded;

        #endregion

        #region Constructor
        public FavouriteComedy()
        {
            this.InitializeComponent();
            IsDataLoaded = false;
            showFavComedy = new List<ShowLinks>();
        }
        #endregion

        #region Events
        private void lbxFavoritecomedy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxFavoritecomedy.SelectedIndex == -1)
                    return;
                AppSettings.LinkTitle = (lbxFavoritecomedy.SelectedItem as ShowLinks).Title.ToString();
                AppSettings.LinkType = (lbxFavoritecomedy.SelectedItem as ShowLinks).LinkType.ToString();
                string Url = (lbxFavoritecomedy.SelectedItem as ShowLinks).LinkUrl;                
                if (ResourceHelper.AppName == Apps.Video_Mix.ToString())
                {
                    PageHelper.NavigateToDetailPage(AppResources.DetailPageName, AppSettings.ShowID);
                }
                else if (ResourceHelper.AppName == Apps.Web_Media.ToString())
                {
                    
                }
                else
                {
                    State.BackStack = "comedy";
                    Frame frame = Window.Current.Content as Frame;
                    Page p = frame.Content as Page;                    
                    string myid=Url;
                    p.Frame.Navigate(typeof(Youtube), myid);                    
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxFavoritecomedy_SelectionChanged Method In FavouriteComedy.cs file.", ex);
            }
            lbxFavoritecomedy.SelectedIndex = -1;
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OnlineVideos.View_Models.ShowDetail detailModel = new OnlineVideos.View_Models.ShowDetail();
                detailModel.AddToFavorites(lbxFavoritecomedy, sender as MenuFlyoutItem, LinkType.Comedy);                
                Frame frame = Window.Current.Content as Frame;
                Page p = frame.Content as Page;
                p.Frame.Navigate(typeof(MainPage));
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in btnFavComedy_Click Method In FavoriteVideos.cs file.", ex);
            }
        }
        #endregion

        #region PageLoad
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
                Exceptions.SaveOrSendExceptions("Exception in UserControl_Loaded Method In FavoriteVideos.cs file.", ex);
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
                                                 a.Result = FavoritesManager.GetFavoriteLinks(LinkType.Comedy);
                                             },
                                                   (object s, RunWorkerCompletedEventArgs a) =>
                                                   {
                                                       showFavComedy = (List<ShowLinks>)a.Result;
                                                       if (showFavComedy.Count > 0)
                                                       {
                                                           lbxFavoritecomedy.ItemsSource = showFavComedy;

                                                           tblkFavNoComedy.Visibility = Visibility.Collapsed;
                                                       }
                                                       else
                                                       {                                                           
                                                           tblkFavNoComedy.Text = "No " + AppResources.FavoriteComedyPivotTitle + " available";
                                                           tblkFavNoComedy.Text="NO" +AppResources.ShowFavouritesPageComedyPivot+ "available";
                                                           tblkFavNoComedy.Visibility = Visibility.Visible;
                                                       }
                                                   }
                                                 );
                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In FavoriteComedy.cs file.", ex);
            }
        }

        #endregion

        private void StackPanel_Holding(object sender, HoldingRoutedEventArgs e)
        {
            // this event is fired multiple times. We do not want to show the menu twice
            if (e.HoldingState != HoldingState.Started) return;

            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            // If the menu was attached properly, we just need to call this handy method
            FlyoutBase.ShowAttachedFlyout(element);
        }

    }
}
