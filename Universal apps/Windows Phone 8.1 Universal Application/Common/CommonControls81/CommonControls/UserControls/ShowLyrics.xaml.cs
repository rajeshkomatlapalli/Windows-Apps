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
    public sealed partial class ShowLyrics : UserControl
    {
        public ShowLyrics()
        {
            try
            {
            this.InitializeComponent();
            progressbar.IsActive = true;
            Loaded += ShowLyrics_Loaded;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowLyrics Method In ShowLyrics.cs file", ex);
            }
        }

        void ShowLyrics_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<ShowLinks> lyriclist = new List<ShowLinks>();
                lyriclist = OnlineShow.Getlyricsofsong(AppSettings.ShowID);
                lstvwlyrics.ItemsSource = lyriclist;
                if (lyriclist.Count > 0)
                {
                    progressbar.IsActive = false;
                    List<ShowLinks> links = OnlineShow.GetLyrics(AppSettings.ShowID, AppSettings.LiricsLink);
                    if (links.Count > 0)

                        lyrics.Text = links.FirstOrDefault().Description;
                    else
                        lyrics.Text = "No Lyrics Available";

                }
                else
                {
                    progressbar.IsActive = false;
                    lyrics.Text = "No Lyrics Available";
                    lyrics.HorizontalAlignment = HorizontalAlignment.Center;
                    lyrics.VerticalAlignment = VerticalAlignment.Center;

                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowLyrics_Loaded Method In ShowLyrics.cs file", ex);
            }
        }

        private void tblklyrics_RightTapped_1(object sender, RightTappedRoutedEventArgs e)
        {

        }

        private void lstvwlyrics_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                if (lstvwlyrics.SelectedIndex == -1)
                    return;

                AppSettings.LiricsLink = (lstvwlyrics.SelectedItem as ShowLinks).Title;
                List<ShowLinks> links = OnlineShow.GetLyrics(AppSettings.ShowID, AppSettings.LiricsLink);
                if (links != null)
                    lyrics.Text = links.FirstOrDefault().Description;
                else
                    lyrics.Text = "No Lyrics Available";
                lstvwlyrics.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstvwlyrics_SelectionChanged_1 Method In ShowLyrics.cs file", ex);
            }
        }
    }
}
