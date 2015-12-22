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
    public sealed partial class VideosSearchList : UserControl
    {
        public string ShowID = string.Empty;
        ShowLinks selecteditem = null;
        bool check = false;
        public VideosSearchList()
        {
            try
            {
            this.InitializeComponent();
            progressbar.IsActive = true;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in VideosSearchList Method In VideosSearchList.cs file", ex);
            }
        }

        private void lstvwsongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                AppSettings.ShowID = (lstvwsongs.SelectedItem as ShowLinks).ShowID.ToString();
                AppSettings.FavTitle = (lstvwsongs.SelectedItem as ShowLinks).Title;
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("DetailPage").Invoke(p, null);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstvwsongs_SelectionChanged Method In VideosSearchList.cs file", ex);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GetPageDataInBackground();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in VideosSearchList_Loaded_1 Method In VideosSearchList.cs file", ex);
            }
        }

        private void GetPageDataInBackground()
        {
            try
            {
                List<ShowLinks> objlinks = new List<ShowLinks>();
                BackgroundHelper bwHelper = new BackgroundHelper();

                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                a.Result = SearchManager.GetShowsLinksBySearch(AppSettings.SearchText, LinkType.Songs);
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                objlinks = (List<ShowLinks>)a.Result;
                                                if (objlinks.Count != 0)
                                                {
                                                    progressbar.IsActive = false;
                                                    lstvwsongs.ItemsSource = objlinks;
                                                }
                                                else
                                                {
                                                    progressbar.IsActive = false;
                                                    txtmsg.Visibility = Visibility.Visible;
                                                    txtmsg.Text = "No videos available";

                                                }

                                            }
                                            );
                bwHelper.RunBackgroundWorkers();

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In VideosSearchList.cs file", ex);
            }

        }
        public int GetCount()
        {
            int count = 0;
            try
            {
                count = lstvwsongs.Items.Count;

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetCount method In VideoFavorite.cs file", ex);

            }
            return count;
        }

    }
}
