using Common.Library;
using Common.Utilities;
using Indian_Cinema;
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
    public sealed partial class AudioFavorite : UserControl
    {
        #region GlobalDeclaration
        List<ShowLinks> showFavaudiosongs = null;
        List<ShowLinks> objAudioList;
        ShowAudio audio;
        #endregion

        #region Constructor
        public AudioFavorite()
        {
            this.InitializeComponent();
            objAudioList = new List<ShowLinks>();
            audio = new ShowAudio();
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
                                                     a.Result = FavoritesManager.GetFavoriteAudioLinks();
                                                 },
                                                 (object s, RunWorkerCompletedEventArgs a) =>
                                                 {
                                                     showFavaudiosongs = (List<ShowLinks>)a.Result;
                                                     if (showFavaudiosongs.Count > 0)
                                                     {
                                                         lbxFavoriteaudio.ItemsSource = showFavaudiosongs;
                                                         tblkFavNoAudio.Visibility = Visibility.Collapsed;
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
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In AudioFavorite.cs file.", ex);
            }
        }
        #endregion

        private void lbxFavoriteaudio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxFavoriteaudio.SelectedIndex == -1)
                return;
            audio.LoadDownLoads(lbxFavoriteaudio.SelectedItem as ShowLinks);
            AppSettings.SongID = (lbxFavoriteaudio.SelectedItem as ShowLinks).LinkID.ToString();
            AppSettings.AudioImage = (lbxFavoriteaudio.SelectedItem as ShowLinks).Songplay;
            AppSettings.LinkTitle = (lbxFavoriteaudio.SelectedItem as ShowLinks).Title.ToString();
            AppSettings.LinkType = "Audio";
            objAudioList = FavoritesManager.GetFavoriteAudioLinks();
            lbxFavoriteaudio.ItemsSource = objAudioList;
            lbxFavoriteaudio.SelectedIndex = -1;
            State.BackStack = "audio";
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OnlineVideos.View_Models.ShowDetail detailModel = new OnlineVideos.View_Models.ShowDetail();                
                detailModel.AddToFavorites(lbxFavoriteaudio, sender as MenuFlyoutItem, LinkType.Audio);                
                Frame frame = Window.Current.Content as Frame;
                Page p = frame.Content as Page;
                p.Frame.Navigate(typeof(MainPage));
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in btnAudioFavSong_Click Method In AudioFavorite.cs file.", ex);
            }
        }

        private void StackPanel_Holding(object sender, HoldingRoutedEventArgs e)
        {           
            if (e.HoldingState != HoldingState.Started) return;
            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;            
            FlyoutBase.ShowAttachedFlyout(element);
        }
    }
}
