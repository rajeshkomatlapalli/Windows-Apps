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
    public sealed partial class AudioSearch : UserControl
    {
        public AudioSearch()
        {
            try
            {
            this.InitializeComponent();
            progressbar.IsActive = true;
            Loaded += AudioSearch_Loaded;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in AudioSearch Method In AudioSearch.cs file", ex);
            }
        }

        void AudioSearch_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GetPageDataInBackground();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetParentOfTypePage Method In PageNavigationManager.cs file", ex);
            }
        }

        private void GetPageDataInBackground()
        {
            try
            {
                List<ShowLinks> objsearchlist = new List<ShowLinks>();
                BackgroundHelper bhhelper = new BackgroundHelper();
                bhhelper.AddBackgroundTask((object s, DoWorkEventArgs a) =>
                {
                    a.Result = SearchManager.GetShowsLinksBySearch(AppSettings.SearchText, LinkType.Audio);
                },
                    (object s, RunWorkerCompletedEventArgs a) =>
                    {
                        objsearchlist = (List<ShowLinks>)a.Result;
                        if (objsearchlist.Count != 0)
                        {
                            progressbar.IsActive = false;
                            lstvwsearchaudiosongs.ItemsSource = objsearchlist; //SearchManager.GetShowsLinksBySearch(AppSettings.SearchText, LinkType.Audio);
                        }
                        else
                        {
                            progressbar.IsActive = false;
                            txtmsg.Text = "No audio songs available";
                            txtmsg.Visibility = Visibility.Visible;
                        }
                    }
                    );

                bhhelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In AudioSearch.cs file", ex);
            }

        }

        private void lstvwsearchaudiosongs_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                AppSettings.ShowID = (lstvwsearchaudiosongs.SelectedItem as ShowLinks).ShowID.ToString();
                AppSettings.FavTitle = (lstvwsearchaudiosongs.SelectedItem as ShowLinks).Title;
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("DetailPage").Invoke(p, null);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstvwsearchaudiosongs_SelectionChanged_1 Method In AudioSearch.cs file", ex);
            }
        }
    }
}
