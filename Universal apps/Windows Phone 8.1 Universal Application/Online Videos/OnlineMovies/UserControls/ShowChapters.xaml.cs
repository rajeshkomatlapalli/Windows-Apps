using Common.Library;
using CommonControls;
using OnlineMovies.Views;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.View_Models;
using OnlineVideos.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class ShowChapters : UserControl
    {
        AppInsights insights = new AppInsights();
        Stopwatch stopwatch = new Stopwatch();
        ShowDetail detailModel;
        private bool IsDataLoaded;
        ObservableCollection<ShowLinks> objChapterList;
        public ListView SClistbox { get; set; }
        public ShowChapters()
        {
            
            this.InitializeComponent();
            objChapterList = new ObservableCollection<ShowLinks>();
            detailModel = new ShowDetail();
            IsDataLoaded = false;            
        }

        #region Events
        private void lbxChapterLinks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SClistbox = lbxChapterLinks;
            try
            {
                if (lbxChapterLinks.SelectedIndex == -1)
                    return;
                AppSettings.LinkType = (lbxChapterLinks.SelectedItem as ShowLinks).LinkType.ToString();
                AppSettings.LinkTitle = (lbxChapterLinks.SelectedItem as ShowLinks).Title.ToString();
                stopwatch = System.Diagnostics.Stopwatch.StartNew();
                var properties = new Dictionary<string, string> { { AppSettings.LinkType, AppSettings.LinkTitle } };
                var metrics = new Dictionary<string, double> { { "Processing Time", stopwatch.Elapsed.TotalMilliseconds } };
                insights.Event("Chapters page Time", properties, metrics);
                var selecteditem1 = (sender as Selector).SelectedItem as ShowLinks;
                var Itemcollection = (sender as ListView).Items.ToList();
                Constants.YoutubePlayList = new Dictionary<string, string>();
                foreach (var item in Itemcollection.Cast<ShowLinks>().Where(i => i.LinkOrder > selecteditem1.LinkOrder))
                {
                    if (!Constants.YoutubePlayList.ContainsKey(item.LinkUrl))
                        Constants.YoutubePlayList.Add(item.LinkUrl, item.Title);
                }
                foreach (var item in Itemcollection.Cast<ShowLinks>().Where(i => i.LinkOrder < selecteditem1.LinkOrder))
                {
                    if (!Constants.YoutubePlayList.ContainsKey(item.LinkUrl))
                        Constants.YoutubePlayList.Add(item.LinkUrl, item.Title);
                }
                History objHistory = new History();
               objHistory.SaveMovieHistory((lbxChapterLinks.SelectedItem as ShowLinks).ShowID.ToString(), (lbxChapterLinks.SelectedItem as ShowLinks).Title);


                if (ResourceHelper.AppName == Apps.Indian_Cinema_Pro.ToString() || ResourceHelper.AppName == Apps.Kids_TV_Pro.ToString() || ResourceHelper.AppName == Apps.Story_Time_Pro.ToString())
                    AppSettings.startplayingforpro = true;
                else
                    AppSettings.startplaying = true;
                AppSettings.PlayVideoTitle = Constants.YoutubePlayList.FirstOrDefault().Value;
                State.BackStack = "chapters";
                Frame frame = Window.Current.Content as Frame;
                Page p = frame.Content as Page;                        
                string myid=(lbxChapterLinks.SelectedItem as ShowLinks).LinkUrl;
                p.Frame.Navigate(typeof(Youtube), myid);                
                lbxChapterLinks.SelectedIndex = -1;
                lbxChapterLinks.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxChapterLinks_SelectionChanged method In ShowChapters.cs file.", ex);
            }
        }

        private void MenuFlyout_Opened(object sender, object e)
        {
            try
            {
                MenuFlyout menu = sender as MenuFlyout;               
                int showid = AppSettings.ShowUniqueID;
                if (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid && i.Status == "Custom").FirstOrDefaultAsync()).Result != null)
                {
                    var item = menu.Items.OfType<MenuFlyoutItem>().First(m => (string)m.Name == "del");
                    item.IsEnabled = true;
                }
                else
                {
                    var item = menu.Items.OfType<MenuFlyoutItem>().First(m => (string)m.Name == "del");
                    item.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in ContextMenu_Opened_1 method In ShowChapters.cs file.", ex);
            }
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            //detailModel.ReportBrokenLink(lbxChapterLinks, sender as MenuFlyoutItem);
            try
            {
                ListView listBox = lbxChapterLinks;
                MenuFlyoutItem selectedItem = sender as MenuFlyoutItem;
                ListViewItem selectedListBoxItem = listBox.ContainerFromItem(selectedItem.DataContext) as ListViewItem;
                if (selectedListBoxItem != null)
                {
                    ShowLinks showLinkInfo = selectedListBoxItem.Content as OnlineVideos.Entities.ShowLinks;
                    string[] parameters=new string[5];
                    parameters[0]=showLinkInfo.ShowID.ToString();//ShowID
                    parameters[1]=showLinkInfo.LinkOrder.ToString();//chno
                    parameters[2]=showLinkInfo.Title;//title
                    parameters[3]=showLinkInfo.LinkType;//linktype
                    parameters[4]=showLinkInfo.LinkUrl;//uri

                    Frame frame = Window.Current.Content as Frame;
                    Page p = frame.Content as Page;
                    p.Frame.Navigate(typeof(Feedback), parameters);
                }

                //detailModel.ReportBrokenLink(lbxSongsList, sender as MenuFlyoutItem);                
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in btnBrokenSong_Click Method In ShowVideos.cs file.", ex);
            }
        }

        private async void del_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuFlyoutItem menu = sender as MenuFlyoutItem;
                ListBoxItem selectedListBoxItem = lbxChapterLinks.ItemContainerGenerator.ContainerFromItem(menu.DataContext) as ListBoxItem;               
                int showid = (selectedListBoxItem.Content as ShowLinks).ShowID;
                int linkid = (selectedListBoxItem.Content as ShowLinks).LinkID;
                string linktype = (selectedListBoxItem.Content as ShowLinks).LinkType;
                var xquery = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(id => id.ShowID == showid && id.LinkID == linkid && id.LinkType == linktype).FirstOrDefaultAsync()).Result;
                if (xquery != null)
                {
                    if (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid && i.Status == "Custom").FirstOrDefaultAsync()).Result != null)
                    {
                        await Constants.connection.DeleteAsync(xquery);
                    }
                }
                GetPageDataInBackground();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in MenuItem_Click_1 method In ShowChapters.cs file.", ex);
            }
        }
        #endregion

        #region Page Load
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                stopwatch.Stop();
                insights.Event("Chapters Loaded");
                if (IsDataLoaded == false)
                {
                    GetPageDataInBackground();
                    IsDataLoaded = true;
                }
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in UserControl_Loaded method In ShowChapters.cs file.", ex);
            }
        }
        #endregion

        private void GetPageDataInBackground()
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();

                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {

                                                a.Result = OnlineShow.GetShowLinksByType(AppSettings.ShowID, LinkType.Movies/*, false*/);
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {

                                                objChapterList = (ObservableCollection<ShowLinks>)a.Result;
                                                if (objChapterList.Count > 0)
                                                {
                                                    lbxChapterLinks.ItemsSource = objChapterList;
                                                    tblkFavNoMovies.Visibility = Visibility.Collapsed;

                                                }
                                                else
                                                {
                                                    lbxChapterLinks.ItemsSource = null;
                                                    tblkFavNoMovies.Text = "No " + AppResources.ShowDetailPageChaptersPivotTitle + " available";
                                                    tblkFavNoMovies.Visibility = Visibility.Visible;
                                                }

                                            }
                                          );

                bwHelper.RunBackgroundWorkers();

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground method In ShowChapters.cs file.", ex);
            }
        }

        private void StackPanel_Holding(object sender, HoldingRoutedEventArgs e)
        {
            // this event is fired multiple times. We do not want to show the menu twice
            if (e.HoldingState != HoldingState.Started) return;

            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            // If the menu was attached properly, we just need to call this handy method
            FlyoutBase.ShowAttachedFlyout(element);
        }

        private void Image_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            Page p = frame.Content as Page;
            p.Frame.Navigate(typeof(LinksFromOnline), null);
        }
    }
}
