using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
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
    public sealed partial class AudioHistory : UserControl
    {
        LinkHistory selecteditem = null;
        bool check = false;
        public TextBlock songblock = null;
        MediaElement RootMediaElement;

        public AudioHistory()
        {
            this.InitializeComponent();
            songblock = new TextBlock();
            progressbar.IsActive = true;
            Loaded += AudioHistory_Loaded;
        }

        void AudioHistory_Loaded(object sender, RoutedEventArgs e)
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

        public int GetCount()
        {
            int count = 0;
            try
            {
                count = lstvwaudiosongshistory.Items.Count;

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetCount method In VideoHistory.cs file", ex);

            }
            return count;
        }
        private void GetPageDataInBackground()
        {
            try
            {

                History history = new History();
                List<LinkHistory> objlist = new List<LinkHistory>();
                objlist = history.GetHistoryList(Constants.AudioHistoryFile);
                foreach (LinkHistory s1 in objlist)
                {

                    s1.ImageRating = ImageHelper.LoadRatingImage(s1.ImageRating);


                }
                if (objlist.Count != 0)
                {
                    progressbar.IsActive = false;
                    lstvwaudiosongshistory.ItemsSource = objlist;
                }
                else
                {
                    progressbar.IsActive = false;
                    txtmsg.Text = "No audio songs available";
                    txtmsg.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In AudioHistory.cs file", ex);
            }
        }

        private void lstvwaudiosongshistory_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstvwaudiosongshistory.SelectedIndex == -1)
                    return;
                if (check == true)
                {
                    check = false;
                    selecteditem = (sender as Selector).SelectedItem as LinkHistory;
                    Constants.histroryselecteditem = selecteditem;
                    AppSettings.LiricsLink = selecteditem.LinkUrl;
                    AppSettings.ShowID = (lstvwaudiosongshistory.SelectedItem as LinkHistory).ShowID.ToString();
                    AppSettings.FavoritesSelectedIndex = lstvwaudiosongshistory.SelectedIndex.ToString();
                    lstvwaudiosongshistory.SelectedIndex = -1;
                    return;
                }
                selecteditem = (sender as Selector).SelectedItem as LinkHistory;
                AppSettings.LiricsLink = (lstvwaudiosongshistory.SelectedItem as LinkHistory).Title;
                Constants.AppbarTitle = (lstvwaudiosongshistory.SelectedItem as LinkHistory).Title;
                Constants.Link = (lstvwaudiosongshistory.SelectedItem as LinkHistory).Title + "$" + (lstvwaudiosongshistory.SelectedItem as LinkHistory).LinkUrl;

                AppSettings.ShowID = (lstvwaudiosongshistory.SelectedItem as LinkHistory).ShowID.ToString();
                AppSettings.AudioFavTitle = (lstvwaudiosongshistory.SelectedItem as LinkHistory).Title;
                songblock.Text = Constants.AppbarTitle;
                LoadLyrics();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstvwaudiosongshistory_SelectionChanged_1 Event In AudioHistory.cs file", ex);
            }
        }

        private void LoadLyrics()
        {
            try
            {
                if (Constants.CloseLyricspopup.IsOpen == true)
                    Constants.CloseLyricspopup.IsOpen = false;
                Constants._PlayList.Clear();
                foreach (LinkHistory s in lstvwaudiosongshistory.Items.Cast<LinkHistory>())
                {
                    if (Constants.Link.Split('$')[0].ToString() != s.Title)
                    {
                        Constants._PlayList.Add(s.Title + "$" + s.LinkUrl, "Stop");
                    }
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
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadLyrics Method In ShowAudio.cs file", ex);
            }
        }
    }
}
