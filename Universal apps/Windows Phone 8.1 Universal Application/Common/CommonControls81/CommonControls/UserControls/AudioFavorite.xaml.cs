using Common.Library;
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
using System.Reflection;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class AudioFavorite : UserControl
    {
        public TextBlock songblock = null;
        bool check = false;
        ShowLinks selecteditem = null;
        MediaElement RootMediaElement;
        public AudioFavorite()
        {
            this.InitializeComponent();
            songblock = new TextBlock();
            Loaded += AudioFavorite_Loaded;
            progressbar.IsActive = true;
        }

        void AudioFavorite_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                RootMediaElement = (MediaElement)border.FindName("MediaPlayer");
                //DependencyObject rootGrid = VisualTreeHelper.GetChild(Window.Current.Content, 0);
                //var foregroundPlayer = (MediaElement)VisualTreeHelper.GetChild(rootGrid, 0) as MediaElement;
                //RootMediaElement = (MediaElement)VisualTreeHelper.GetChild(rootGrid, 1) as MediaElement;
                GetPageDataInBackground();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in AudioFavorite_Loaded Method In AudioFavorite.cs file", ex);
            }
        }

        public void GetPageDataInBackground()
        {
            try
            {
                if (AppSettings.FavoritesSelectedIndex != "")
                {
                    lstvwfavoriteaudiosongs.ItemsSource = null;
                    lstvwfavoriteaudiosongs.Items.Clear();
                    AppSettings.FavoritesSelectedIndex = "";
                }
                List<ShowLinks> objfavlinks = new List<ShowLinks>();
                BackgroundHelper bwHelper = new BackgroundHelper();
                bwHelper.AddBackgroundTask(
                                                  (object s, DoWorkEventArgs a) =>
                                                  {
                                                      //a.Result = FavoritesManager.GetFavoriteByType(LinkType.Audio);
                                                      a.Result = FavoritesManager.GetFavoriteLinks(LinkType.Audio);
                                                  },
                                                  (object s, RunWorkerCompletedEventArgs a) =>
                                                  {
                                                      objfavlinks = (List<ShowLinks>)a.Result;
                                                      foreach (ShowLinks s1 in objfavlinks)
                                                      {
                                                          s1.RatingBitmapImage = ImageHelper.LoadRatingImage(s1.Rating.ToString());

                                                      }
                                                      if (objfavlinks.Count != 0)
                                                      {
                                                          lstvwfavoriteaudiosongs.ItemsSource = objfavlinks;
                                                          progressbar.IsActive = false;
                                                      }
                                                      else
                                                      {
                                                          txtmsg.Text = "No audio songs available";
                                                          txtmsg.Visibility = Visibility.Visible;
                                                          lstvwfavoriteaudiosongs.Visibility = Visibility.Collapsed;
                                                          progressbar.IsActive = false;
                                                      }

                                                  }
                                                );
                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In AudioFavorite.cs file", ex);
            }
        }

        private void lstvwfavoriteaudiosongs_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstvwfavoriteaudiosongs.SelectedIndex == -1)

                    return;
                if (check == true)
                {
                    check = false;
                    selecteditem = (sender as Selector).SelectedItem as ShowLinks;
                    Constants.selecteditem = selecteditem;
                    AppSettings.LinkType = selecteditem.LinkType;
                    AppSettings.ShowID = (lstvwfavoriteaudiosongs.SelectedItem as ShowLinks).ShowID.ToString();
                    AppSettings.FavoritesSelectedIndex = lstvwfavoriteaudiosongs.SelectedIndex.ToString();
                    lstvwfavoriteaudiosongs.SelectedIndex = -1;
                    return;
                }
                AppSettings.LiricsLink = (lstvwfavoriteaudiosongs.SelectedItem as ShowLinks).Title;
                Constants.AppbarTitle = (lstvwfavoriteaudiosongs.SelectedItem as ShowLinks).Title;                
                string Url = (lstvwfavoriteaudiosongs.SelectedItem as ShowLinks).LinkUrl;
                Url = Url.Split(',')[1];
                Constants.Link = (lstvwfavoriteaudiosongs.SelectedItem as ShowLinks).Title + "$" + Url;
                AppSettings.ShowID = (lstvwfavoriteaudiosongs.SelectedItem as ShowLinks).ShowID.ToString();
                AppSettings.AudioFavTitle = (lstvwfavoriteaudiosongs.SelectedItem as ShowLinks).Title;
                songblock.Text = Constants.AppbarTitle;
                LoadLyrics();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstvwfavoriteaudiosongs_SelectionChanged_2 Method In AudioFavorite.cs file", ex);
            }
        }

        private void tblkAudioSongs_RightTapped_1(object sender, RightTappedRoutedEventArgs e)
        {
            try
            {
                Constants.selecteditem = null;
                check = true;
                Constants.Favoriteappbarvisible = true;
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("appbar").Invoke(p, new object[] { true });
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in tblkAudioSongs_RightTapped_1 Method In AudioFavorite.cs file", ex);
            }
        }

        private void LoadLyrics()
        {
            try
            {
                if (Constants.CloseLyricspopup.IsOpen == true)
                    Constants.CloseLyricspopup.IsOpen = false;
                Constants._PlayList.Clear();
                foreach (ShowLinks s in lstvwfavoriteaudiosongs.Items.Cast<ShowLinks>())
                {
                    Constants._PlayList.Add(s.Title + "$" + s.LinkUrl, "Stop");
                }
                var p = new LyricsPopUp();
                Constants.CloseLyricspopup = new Popup();
                Constants.CloseLyricspopup.Child = p;
                Constants.CloseLyricspopup.IsOpen = true;
                RootMediaElement.Source = new Uri(Constants.Link.Split('$')[1], UriKind.Absolute);
                Constants._PlayList[Constants.Link] = "Play";
                TextBlock song = (TextBlock)p.GetType().GetTypeInfo().GetDeclaredField("songblock").GetValue(p);
                song.Text = Constants.Link.Split('$')[0];
                RootMediaElement.Play();
                p.GetType().GetTypeInfo().GetDeclaredMethod("changeimage").Invoke(p, null);

                // Page p1 = (Page)PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("LyricsPopup").Invoke(p, new object[] { true });


                //OLD CODE
                //Page p = (Page)PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                //RootMediaElement.Source = new Uri(Constants.Link.Split('$')[1], UriKind.Absolute);
                //Constants._PlayList[Constants.Link] = "Play";
                //TextBlock song = (TextBlock)p.GetType().GetTypeInfo().GetDeclaredField("songblock").GetValue(p);
                //song.Text = Constants.Link.Split('$')[0];
                //RootMediaElement.Play();
                //p.GetType().GetTypeInfo().GetDeclaredMethod("changeimage").Invoke(p, null);

                //Page p1 = (Page)PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                //p1.GetType().GetTypeInfo().GetDeclaredMethod("LyricsPopup").Invoke(p1, new object[] { Constants.Link.Split('$')[1] });
                //Page p1 = (Page)PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                //popgallimages.Child = (UIElement)p1.GetType().GetTypeInfo().GetDeclaredMethod("Popup").Invoke(p1, null);

                //popgallimages.IsOpen = true;
                //popgallimages.Height = 1000;




            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadLyrics Method In ShowAudio.cs file", ex);
            }

        }

    }
}
