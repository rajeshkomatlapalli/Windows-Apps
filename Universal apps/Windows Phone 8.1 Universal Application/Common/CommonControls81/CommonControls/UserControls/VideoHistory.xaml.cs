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
using OnlineVideosWin81.Controls;
using OnlineVideos.Views;


// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class VideoHistory : UserControl
    {
        AppInsights insights = new AppInsights();
        public VideoHistory()
        {
            
            try
            {
            this.InitializeComponent();
            AppSettings.LinkUrl = string.Empty;
            Loaded += VideoHistory_Loaded;
            progressbar.IsActive = true;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in VideoHistory Method In VideoHistory.cs file", ex);
                insights.Exception(ex);
            }
        }
        
        void VideoHistory_Loaded(object sender, RoutedEventArgs e)
        {
           
            try
            {
                insights.Event("video history loaded");
                GetPageDataInBackground();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in VideoHistory_Loaded Method In VideoHistory.cs file", ex);
            }
        }

        private void lstvwvideohistory_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selecteditem1 = (sender as Selector).SelectedItem as LinkHistory;
                AppSettings.LinkUrl = selecteditem1.LinkUrl;
                AppSettings.ShowID = selecteditem1.ShowID.ToString();
                insights.Event(AppSettings.LinkType);
                insights.Event(AppSettings.LinkUrl);
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("Youtube").Invoke(p, null);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstvwvideohistory_SelectionChanged_1 Event In VideoHistory.cs file", ex);
            }
        }

        private void tblkvideo_RightTapped_1(object sender, RightTappedRoutedEventArgs e)
        {

        }
        public int GetCount()
        {
            int count = 0;
            try
            {
                count = lstvwvideohistory.Items.Count;

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
                objlist = history.GetHistoryList(Constants.VideoHistoryFile);
                if (objlist.Count != 0)
                {
                    lstvwvideohistory.ItemsSource = objlist;
                    progressbar.IsActive = false;
                }
                else
                {
                    progressbar.IsActive = false;
                    txtmsg.Text = "No videos available";
                    txtmsg.Visibility = Visibility.Visible;
                }
                //BackgroundHelper bwHelper = new BackgroundHelper();
                //
                //bwHelper.AddBackgroundTask(
                //                            (object s, DoWorkEventArgs a) =>
                //                            {
                //                                a.Result = history.GetHistoryList(Constants.VideoHistoryFile);
                //                            },
                //                            (object s, RunWorkerCompletedEventArgs a) =>
                //                            {
                //                                lstvwvideohistory.ItemsSource = (List<LinkHistory>)a.Result;
                //                            }
                //                          );

                //bwHelper.RunBackgroundWorkers();

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In VideoHistory.cs file", ex);
            }
        }

    }
}
