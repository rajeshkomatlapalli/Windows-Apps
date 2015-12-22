using Common.Library;
using Common.Utilities;
using CommonControls;
using OnlineMovies.Views;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.UI;
using OnlineVideos.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
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
    public class showvideolbx
    {
        public ListView SVListBox { get; set; }
    }
    public sealed partial class ShowVideos : UserControl
    {        
        OnlineVideos.View_Models.ShowDetail detailModel;
        public static ShowVideos current = default(ShowVideos);
        //AppInsights insights = new AppInsights();
        string video = string.Empty;
        Stopwatch stopwatch = new Stopwatch();
        ObservableCollection<ShowLinks> objSongList = null;
        private bool IsDataLoaded;       
        string sharemsg = string.Empty;
        ListView sharelistBox;
        MenuFlyoutItem sharemfselectedItem;

        public ShowVideos()
        {            
            this.InitializeComponent();
            current = this;
            IsDataLoaded = false;
            detailModel = new OnlineVideos.View_Models.ShowDetail();
            objSongList = new ObservableCollection<ShowLinks>();
            DataTransferManager dtManager = DataTransferManager.GetForCurrentView();
            dtManager.DataRequested += dtManager_DataRequested;
        }

        void dtManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            try
            {
                ListViewItem selectedListBoxItem = sharelistBox.ContainerFromItem(sharemfselectedItem.DataContext) as ListViewItem;
                string lnk = sharemsg;
                lnk += "'" + (selectedListBoxItem.Content as ShowLinks).Title + "', Get the app at \n";
                DataRequest request = args.Request;
                request.Data.Properties.Title = ResourceHelper.ProjectName + " App";
                request.Data.SetWebLink(ResourceHelper.AppMarketplaceWebLink);
                request.Data.Properties.Description = lnk;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in dtManager_DataRequested Method In ShowVideos.cs file.", ex);
            }
        }
        #region Events
        private void lbxSongsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxSongsList.SelectedIndex == -1)
                    return;
                AppSettings.LinkTitle = (lbxSongsList.SelectedItem as ShowLinks).Title.ToString();
                AppSettings.LinkType = (lbxSongsList.SelectedItem as ShowLinks).LinkType.ToString();
                video = AppSettings.LinkType + " " + AppSettings.Title;
                stopwatch = System.Diagnostics.Stopwatch.StartNew();
                var properties = new Dictionary<string, string> { { video, "Viewed" } };
                var metrics = new Dictionary<string, double> { { "Processing Time", stopwatch.Elapsed.TotalMilliseconds } };
                //insights.Event("Songs Page Time", properties, metrics);

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
                AppSettings.PlayVideoTitle = Constants.YoutubePlayList.FirstOrDefault().Value;

                History objHistory = new History();
                objHistory.SaveSongHistory((lbxSongsList.SelectedItem as ShowLinks).ShowID.ToString(), (lbxSongsList.SelectedItem as ShowLinks).Title);

                if (ResourceHelper.AppName == Apps.Indian_Cinema_Pro.ToString() || ResourceHelper.AppName == Apps.Kids_TV_Pro.ToString() || ResourceHelper.AppName == Apps.Story_Time_Pro.ToString())
                    AppSettings.startplayingforpro = true;
                else
                    AppSettings.startplaying = true;
                if (ResourceHelper.ProjectName == "VideoMix")
                {
                    var item = lbxSongsList.SelectedItem as ShowLinks;
                    AppSettings.ShowID = item.ShowID.ToString();
                    AppSettings.LinkUrl = item.LinkUrl;
                    AppSettings.Title = item.Title;
                    Constants.TileImageUrl = item.UrlType;
                    AppSettings.LinkOrder = item.LinkOrder.ToString();
                    PageHelper.NavigateTo(NavigationHelper.MixVideosPopupPage);
                }
                else
                {
                    State.BackStack = "songs";
                    Frame frame = Window.Current.Content as Frame;
                    Page p = frame.Content as Page;
                    p.Frame.Navigate(typeof(Youtube), (lbxSongsList.SelectedItem as ShowLinks).LinkUrl);
                }

                lbxSongsList.SelectedIndex = -1;
                lbxSongsList.IsEnabled = true;
                lbxSongsList.ItemsSource = OnlineShow.GetShowLinksByType(AppSettings.ShowID, LinkType.Songs/*, false*/);
            }
            catch (Exception ex)
            {
                string excep = ex.Message;
                UtilitiesManager.LoadBrowserTask((lbxSongsList.SelectedItem as ShowLinks).LinkUrl);
            }
        }
        ListViewItem selectedListBoxItem;

        private void StackPanel_Holding(object sender, HoldingRoutedEventArgs e)
        {
            // this event is fired multiple times. We do not want to show the menu twice
            if (e.HoldingState != HoldingState.Started) return;

            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            // If the menu was attached properly, we just need to call this handy method
            FlyoutBase.ShowAttachedFlyout(element);

            FrameworkElement element1 = (FrameworkElement)e.OriginalSource;
            var datacontext = element1.DataContext;
            selectedListBoxItem = this.lbxSongsList.ContainerFromItem(datacontext) as ListViewItem;                     

        }
        
        private void MenuFlyout_Opened(object sender, object e)
        {
            try
            {
                string Name = AppSettings.ShowID + (selectedListBoxItem.Content as ShowLinks).Title;
                //insights.Event(Name);
                MenuFlyout mainmenu = sender as MenuFlyout;
                foreach (MenuFlyoutItem contextMenuItem in mainmenu.Items)
                {                    
                    if (selectedListBoxItem == null)
                        return;
                    
                    if (contextMenuItem.Name == "Pin")
                    {
                        string name=AppSettings.ShowID+(selectedListBoxItem.Content as ShowLinks).Title;
                        string ID = Regex.Replace(name, @"\s", "");
                        if (ShellTileHelper_New.IsPinned(ID)==true)
                        {
                            contextMenuItem.Text = Constants.UnpinFromStartScreen;
                        }
                        else
                        {
                            contextMenuItem.Text = Constants.PinToStartScreen;
                        }
                    }
                    if (contextMenuItem.Name == "Rating")
                    {
                        contextMenuItem.Text = AppResources.AllowRatingLinkContextMenuLabel;
                    }
                    if (contextMenuItem.Name.Contains("favorites"))
                    {
                        int showid1 = Convert.ToInt32(AppSettings.ShowID);

                        int linkid = (selectedListBoxItem.Content as ShowLinks).LinkID;
                        string linktype = (selectedListBoxItem.Content as ShowLinks).LinkType;
                        
                        if (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid1 && i.LinkID == linkid && i.LinkType == linktype).FirstOrDefaultAsync()).Result != null)
                        {
                            if (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid1 && i.LinkID == linkid && i.LinkType == linktype).FirstOrDefaultAsync()).Result.IsFavourite == false)
                            {                                                                
                                contextMenuItem.Text = "add to favorites";
                            }
                            else                                
                                contextMenuItem.Text = "remove from favorites";
                        }
                    }
                }                
                int showid = AppSettings.ShowUniqueID;
                if (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid && i.Status == "Custom").FirstOrDefaultAsync()).Result != null || ResourceHelper.AppName == Apps.Video_Mix.ToString())
                {
                    var item = mainmenu.Items.OfType<MenuFlyoutItem>().First(m => (string)m.Name == "del");
                    item.IsEnabled = true;
                }
                else
                {
                    var item = mainmenu.Items.OfType<MenuFlyoutItem>().First(m => (string)m.Name == "del");
                    item.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ContextMenu_Opened Method In ShowVideos.cs file.", ex);
            }
        }

        private void Pin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string showLinkTitle;
                Frame frame = Window.Current.Content as Frame;
                Page p = frame.Content as Page;
                Pivot p1 = (Pivot)(p.FindName("pvtMainDetails"));
                if (p1 != null)
                    Constants.topsongnavigation = p1.SelectedIndex.ToString();
                ListViewItem selectedListBoxItem = this.lbxSongsList.ContainerFromItem((sender as MenuFlyoutItem).DataContext) as ListViewItem;
                if (selectedListBoxItem == null)
                    return;
                showLinkTitle = (selectedListBoxItem.Content as ShowLinks).Title;
                AppSettings.ShowLinkTitle = AppSettings.ShowID + (selectedListBoxItem.Content as ShowLinks).Title;
                ShellTileHelper_New.PinVideoLinkToStartScreen(AppSettings.ShowID, selectedListBoxItem.Content as ShowLinks);
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in btnpintostart_Click Method In ShowVideos.cs file.", ex);
            }
        }

        private void favorites_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                detailModel.AddToFavorites(lbxSongsList, sender as MenuFlyoutItem, LinkType.Songs);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in btnFavSong_Click Method In ShowVideos.cs file.", ex);
            }
        }
        
        private void report_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ListView listBox = lbxSongsList;
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

        private void sharelink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AppSettings.ProjectName == "Indian Cinema.WindowsPhone" || AppSettings.ProjectName == "Indian Cinema Pro")
                {
                    sharemsg = " Watch Bollywood Song ";
                }
                else if (AppSettings.ProjectName == "Online Education.WindowsPhone")
                {
                    sharemsg = " Watch Online Education Video ";
                }
                else if (AppSettings.ProjectName == "Vedic Library")
                {
                    sharemsg = " Watch Vedic Library Video ";
                }
                
                sharelistBox = lbxSongsList;
                sharemfselectedItem = sender as MenuFlyoutItem;
                detailModel.PostAppLinkToSocialNetworks(lbxSongsList, sender as MenuFlyoutItem, sharemsg);
                Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in sharelink_Click Method In ShowVideos.cs file.", ex);
            }
        }

        private void Rating_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                detailModel.RateThisLink(lbxSongsList, sender as MenuFlyoutItem, "Details");                
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ratelink_Click Method In ShowVideos.cs file.", ex);
            }
        }

        private async void del_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuFlyoutItem menu = sender as MenuFlyoutItem;
                ListViewItem selectedListBoxItem = lbxSongsList.ContainerFromItem(menu.DataContext) as ListViewItem;               
                int showid = (selectedListBoxItem.Content as ShowLinks).ShowID;
                int linkid = (selectedListBoxItem.Content as ShowLinks).LinkID;
                string linktype = (selectedListBoxItem.Content as ShowLinks).LinkType;
                var xquery = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(id => id.ShowID == showid && id.LinkID == linkid && id.LinkType == linktype).FirstOrDefaultAsync()).Result;
                if (xquery != null)
                {
                    if (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid && i.Status == "Custom").FirstOrDefaultAsync()).Result != null || ResourceHelper.AppName == Apps.Video_Mix.ToString())
                    {
                        await Constants.connection.DeleteAsync(xquery);
                    }
                }
                GetPageDataInBackground();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in del_Click_1 Method In ShowVideos.cs file.", ex);
            }
        }
        #endregion
        
        #region PageLoad
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {                       
            try
            {
                //insights.Event("Videos list viewed");
                if (IsDataLoaded == false)
                {
                    MyProgressBar1.IsIndeterminate = true;
                    GetPageDataInBackground();
                    IsDataLoaded = true;
                    MyProgressBar1.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in UserControl_Loaded Method In ShowVideos.cs file.", ex);
            }
        }
        #endregion

        public void GetPageDataInBackground()
        {
            BackgroundHelper bwHelper = new BackgroundHelper();
            bwHelper.AddBackgroundTask(
                                        (object s, DoWorkEventArgs a) =>
                                        {
                                            a.Result = OnlineShow.GetShowLinksByType(AppSettings.ShowID, LinkType.Songs/*, false*/);
                                        },
                                        (object s, RunWorkerCompletedEventArgs a) =>
                                        {
                                            objSongList = (ObservableCollection<ShowLinks>)a.Result;
                                            if (objSongList.Count > 0)
                                            {
                                                lbxSongsList.ItemsSource = objSongList;
                                                tblkFavNoSongs.Visibility = Visibility.Collapsed;
                                                lbxSongsList.Loaded += LbxSongsList_Loaded;
                                            }
                                            else
                                            {                                                
                                                if (ResourceHelper.ProjectName == "Video Mix")
                                                {
                                                    Constants.VisiablehelpImage = true;
                                                    tblkFavNoSongs.Visibility = Visibility.Visible;
                                                    tblkFavNoSongsForVideoMix.Visibility = Visibility.Visible;
                                                    tblkFavNoSongs.Text = "Please add some videos to the mix ";
                                                    tblkFavNoSongsForVideoMix.Text = "and create bookmarks in each video.";
                                                }
                                                else
                                                {
                                                    lbxSongsList.ItemsSource = null;
                                                    tblkFavNoSongs.Visibility = Visibility.Visible;
                                                    tblkFavNoSongs.Text = "No " + AppResources.ShowDetailPageSongsPivotTitle as string + " available";                                                   
                                                }
                                            }
                                            MyProgressBar1.IsIndeterminate = false;
                                        }
                                      );
            bwHelper.RunBackgroundWorkers();
        }

        private void LbxSongsList_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        void lbxSongsList_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                lbxSongsList.IsEnabled = true;
                //insights.Event("Songs list loaded");
                if (Application.Current.Resources.Keys.Contains("searchtitle"))
                {
                    PageHelper.SetSelectedItemForegroundColor(this.lbxSongsList, AppState.searchtitle);
                }
                else
                {
                    PageHelper.SetSelectedItemForegroundColor(this.lbxSongsList, "");
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxSongsList_Loaded Method In ShowVideos.cs file.", ex);
            }
        }
        
        private void tblkchno_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

    }
}
