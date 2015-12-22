using Common.Library;
using Common.Utilities;
using CommonControls;
using Indian_Cinema.UserControls;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.UI;
using OnlineVideos.View_Models;
using OnlineVideos.Views;
using System;
using System.Collections.Generic;
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
using Windows.Media.Playback;
using Windows.System.Threading;
using Windows.UI.Input;
using Windows.UI.Popups;
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
    public sealed partial class ShowAudio : UserControl
    {
        #region GlobalDeclaration
        AppInsights insights = new AppInsights();
        Stopwatch stopwatch = new Stopwatch();
        public ValueSet message { get; set; }
        List<ShowLinks> objAudioList;
        Indian_Cinema.UserControls.PersonGalleryPopup galleryPopup = null;
        private bool IsDataLoaded;
        ShowDetail detailModel;
        bool ringtoneimageclicked = false;
        private MediaPlayer _mediaPlayer;
        IEnumerable<DependencyObject> cboxes;
        string sharemsg = string.Empty;
        ListView sharelistBox;
        MenuFlyoutItem sharemfselectedItem;
        string sharetitle;
        ListViewItem selectedListBoxItem;

        #endregion

        #region Constructor

        public ShowAudio()
        {            
            this.InitializeComponent();
            objAudioList = new List<ShowLinks>();
            IsDataLoaded = false;
            _mediaPlayer = BackgroundMediaPlayer.Current;
            DataTransferManager dtManager = DataTransferManager.GetForCurrentView();
            dtManager.DataRequested += dtManager_DataRequested;
        }

        void dtManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            try
            {
                //ListViewItem selectedListBoxItem = sharelistBox.ContainerFromItem(sharemfselectedItem.DataContext) as ListViewItem;
                string lnk = sharemsg;
                //string ss = (selectedListBoxItem.Content as ShowLinks).Title;
                lnk += "'" + sharetitle + "', Get the app at \n";
                DataRequest request = args.Request;
                request.Data.Properties.Title = ResourceHelper.ProjectName + " App";
                request.Data.SetWebLink(ResourceHelper.AppMarketplaceWebLink);
                request.Data.Properties.Description = lnk;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
        #endregion        

        #region Events
        private async void lbxAudioList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxAudioList.SelectedIndex == -1)
                    return;

                if (ringtoneimageclicked == true)
                {
                    lbxAudioList.SelectedIndex = -1;
                    return;
                }
                tblkAudioError.Visibility = Visibility.Collapsed;
                if (Constants.Synthesizer != null)
                {                   
                    Constants.Synthesizer.Dispose();
                }
                performanceProgressBar.Visibility = Visibility.Visible;
                performanceProgressBar.IsIndeterminate = true;
             
                string showid = (lbxAudioList.SelectedItem as ShowLinks).ShowID.ToString();
                AppSettings.LinkTitle = (lbxAudioList.SelectedItem as ShowLinks).Title.ToString();
                AppSettings.LinkType = (lbxAudioList.SelectedItem as ShowLinks).LinkType.ToString();
                insights.Event(AppSettings.LinkTitle + "Played");
                LoadDownLoads(lbxAudioList.SelectedItem as ShowLinks);

                //await Task.Delay(5000);
                //if (BackgroundMediaPlayer.Current.CurrentState == MediaPlayerState.Playing)
                //{
                    AppSettings.SongID = (lbxAudioList.SelectedItem as ShowLinks).LinkID.ToString();
                    AppSettings.AudioImage = (lbxAudioList.SelectedItem as ShowLinks).Songplay;
                //}
                //else
                //{
                //    AppSettings.SongID = (lbxAudioList.SelectedItem as ShowLinks).LinkID.ToString();
                //    AppSettings.AudioImage = (lbxAudioList.SelectedItem as ShowLinks).SongNO;
                    //tblkAudioError.Visibility = Visibility.Visible;
                    //tblkAudioError.Text = "Error In Audio Link";
                //}
                               
                objAudioList = OnlineShow.GetShowLinksByTypeForWp8(showid, LinkType.Audio/*, false*/);
                lbxAudioList.ItemsSource = objAudioList;
                lbxAudioList.SelectedIndex = -1;
                State.BackStack = "audio";
                
                performanceProgressBar.IsIndeterminate = false;
                performanceProgressBar.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxAudioList_SelectionChanged method In ShowAudio.cs file.", ex);
            }
        }

        private void Pin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string showLinkTitle;                
                Frame frame = Windows.UI.Xaml.Window.Current.Content as Frame;
                Page p = frame.Content as Page;
                Pivot p1 = (Pivot)(p.FindName("pvtMainDetails"));
                if (p1 != null)
                    Constants.topsongnavigation = p1.SelectedIndex.ToString();
                ListViewItem selectedListBoxItem = this.lbxAudioList.ContainerFromItem((sender as MenuFlyoutItem).DataContext) as ListViewItem;
                if (selectedListBoxItem == null)
                    return;
                showLinkTitle = (selectedListBoxItem.Content as ShowLinks).Title;
                AppSettings.ShowLinkTitle = AppSettings.ShowID + (selectedListBoxItem.Content as ShowLinks).Title;
                ShellTileHelper_New.PinVideoLinkToStartScreen(AppSettings.ShowID, selectedListBoxItem.Content as ShowLinks);
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in btnpintostart_Click method In ShowAudio.cs file.", ex);
            }
        }

        private void MenuFlyout_Opened(object sender, object e)
        {
            try
            {                
                MenuFlyout mainmenu = sender as MenuFlyout;
                foreach (MenuFlyoutItem contextMenuItem in mainmenu.Items)
                {                   
                    if (selectedListBoxItem == null)
                        return;
                    if (contextMenuItem.Name == "Pin")
                    {
                        string name = AppSettings.ShowID + (selectedListBoxItem.Content as ShowLinks).Title;
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
                        if (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid1 && i.LinkID == linkid && i.LinkType == linktype).FirstOrDefaultAsync()).Result.IsFavourite == false)
                        {                            
                            contextMenuItem.Text = "add to favorites";
                        }
                        else                            
                            contextMenuItem.Text = "remove from favorites";
                    }
                }
               
                int showid = AppSettings.ShowUniqueID;
                if (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid && i.Status == "Custom").FirstOrDefaultAsync()).Result != null)
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

                Exceptions.SaveOrSendExceptions("Exception in ContextMenu_Opened method In ShowAudio.cs file.", ex);
            }
        }

        private void favorites_Click(object sender, RoutedEventArgs e)
        {
            detailModel.AddToFavorites(lbxAudioList, sender as MenuFlyoutItem, LinkType.Audio);
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            //detailModel.ReportBrokenLink(lbxAudioList, sender as MenuFlyoutItem);
            try
            {
                ListView listBox = lbxAudioList;
                MenuFlyoutItem selectedItem = sender as MenuFlyoutItem;
                ListViewItem selectedListBoxItem = listBox.ContainerFromItem(selectedItem.DataContext) as ListViewItem;
                if (selectedListBoxItem != null)
                {
                    ShowLinks showLinkInfo = selectedListBoxItem.Content as OnlineVideos.Entities.ShowLinks;
                    string[] parameters = new string[5];
                    parameters[0] = showLinkInfo.ShowID.ToString();//ShowID
                    parameters[1] = showLinkInfo.LinkOrder.ToString();//chno
                    parameters[2] = showLinkInfo.Title;//title
                    parameters[3] = showLinkInfo.LinkType;//linktype
                    parameters[4] = showLinkInfo.LinkUrl;//uri

                    Frame frame = Window.Current.Content as Frame;
                    Page p = frame.Content as Page;
                    p.Frame.Navigate(typeof(Feedback), parameters);
                }                       
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in btnBrokenSong_Click Method In ShowVideos.cs file.", ex);
            }
        }

        private void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {
           // detailModel.PostAppLinkToSocialNetworks(lbxAudioList, sender as MenuFlyoutItem, " Listen to Bollywood Song ");
            sharemsg = " Listen to Bollywood Song ";
            sharelistBox = lbxAudioList;
            sharemfselectedItem = sender as MenuFlyoutItem;
            ListViewItem selectedListBoxItem1 = sharelistBox.ContainerFromItem(sharemfselectedItem.DataContext) as ListViewItem;
            sharetitle = (selectedListBoxItem1.Content as ShowLinks).Title;
            Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
        }

        private void Rating_Click(object sender, RoutedEventArgs e)
        {
            detailModel.RateThisLink(lbxAudioList, sender as MenuFlyoutItem, AppResources.DetailPageName);
        }

        private async void del_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuFlyoutItem menu = sender as MenuFlyoutItem;
                ListViewItem selectedListBoxItem = lbxAudioList.ContainerFromItem(menu.DataContext) as ListViewItem;                
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
                Exceptions.SaveOrSendExceptions("Exception in del_Click_1 method In ShowAudio.cs file.", ex);
            }
        }
        Image ringtoneimage;
        //private void ringtone_PointerPressed(object sender, PointerRoutedEventArgs e)
        //{            
        //    try
        //    {
        //        ringtoneimageclicked = true;
        //        Image ringtoneimage = sender as Image;               
        //        if(BackgroundMediaPlayer.Current.CurrentState==MediaPlayerState.Playing)
        //        {                    
        //            var msgbox = new MessageDialog("This app needs permision to stop the Background Music.Is it ok to stop currently playing music?","");
        //            msgbox.Commands.Add(new UICommand("OK", new UICommandInvokedHandler(this.TriggerThisFunctionForOK)));                    
        //        }
        //        else
        //        {                   
        //            AppSettings.Chapterno = ringtoneimage.Tag.ToString().Split(',')[0];
        //            if (AppSettings.Chapterno.Contains('\n'))
        //                AppSettings.Chapterno = AppSettings.Chapterno.Replace("\n", "");
        //            AppSettings.ShowLinkTitle = ringtoneimage.Tag.ToString().Split(',')[1];
                   
        //            Frame frame = Window.Current.Content as Frame;
        //            Page p = frame.Content as Page;
        //            string[] parameters=new string[2];
        //            parameters[0]=AppSettings.ShowID.ToString();
        //            parameters[1]=AppSettings.ShowLinkTitle;
        //            //p.Frame.Navigate(typeof(RingTone), parameters);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Exceptions.SaveOrSendExceptions("Exception in ringtone_MouseLeftButtonDown method In ShowAudio.cs file.", ex);
        //    }
        //}

        private void TriggerThisFunctionForOK(IUICommand command)
        {           
            BackgroundMediaPlayer.Shutdown();            
            AppSettings.Chapterno = ringtoneimage.Tag.ToString().Split(',')[0];
            if (AppSettings.Chapterno.Contains('\n'))
                AppSettings.Chapterno = AppSettings.Chapterno.Replace("\n", "");
            AppSettings.ShowLinkTitle = ringtoneimage.Tag.ToString().Split(',')[1];
            PageHelper.NavigateToRingTonePage(AppResources.RingTonePageName, AppSettings.ShowLinkTitle, AppSettings.ShowID, AppSettings.Chapterno);
        }

        #endregion
        
        #region Page Load
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
            
            try
            {
                insights.Event("Audio List Viewed");
                if (IsDataLoaded == false)
                {
                    GetPageDataInBackground();
                    detailModel = new ShowDetail();
                    IsDataLoaded = true;         
                }
                ringtoneimageclicked = false;
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in UserControl_Loaded event In ShowAudio.cs file.", ex);
            }
        }
        #endregion

        #region Common Methods
        private void GetPageDataInBackground()
        {
            BackgroundHelper bwHelper = new BackgroundHelper();

            bwHelper.AddBackgroundTask(
                                        (object s, DoWorkEventArgs a) =>
                                        {
                                            a.Result = OnlineShow.GetShowLinksByTypeForWp8(AppSettings.ShowID, LinkType.Audio/*, false*/);
                                        },
                                        (object s, RunWorkerCompletedEventArgs a) =>
                                        {
                                            objAudioList = (List<ShowLinks>)a.Result;
                                            if (objAudioList.Count > 0)
                                            {
                                                lbxAudioList.ItemsSource = objAudioList;
                                                tblkFavNoAudio.Visibility = Visibility.Collapsed;
                                                lbxAudioList.Loaded += new RoutedEventHandler(lbxAudioList_Loaded);
                                            }
                                            else
                                            {
                                                lbxAudioList.ItemsSource = null;
                                                tblkFavNoAudio.Text = "No " + AppResources.ShowFavoriteAudioPivotTitle + " available";
                                                tblkFavNoAudio.Visibility = Visibility.Visible;
                                            }
                                        }
                                      );
            bwHelper.RunBackgroundWorkers();
        }

        void lbxAudioList_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {                
                if (Application.Current.Resources.Keys.Contains("searchtitle"))
                {
                    PageHelper.SetSelectedItemForegroundColor(this.lbxAudioList, AppState.searchtitle);
                }
                else
                {
                    PageHelper.SetSelectedItemForegroundColor(this.lbxAudioList, "");
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxAudioList_Loaded method In ShowAudio.cs file.", ex);
            }
        }        
               
        public void LoadDownLoads(LinkHistory linkinfo)
        {
            try
            {
                string linktype = "";
                AppSettings.ShowLinkTitle = linkinfo.LinkUrl;
                AppSettings.AudioTitle = linkinfo.Title;
                if (linktype == ".jpg" || linktype == ".png")
                {
                    galleryPopup.Show(linkinfo.ID.ToString());
                }
                else
                {
                    if (linkinfo.Songplay == Constants.PlayImagePath)
                    {
                        IEnumerable<DependencyObject> cboxes = PageHelper.GetChildsRecursive(lbxAudioList);
                        foreach (DependencyObject obj in cboxes.OfType<ProgressBar>())
                        {
                            Type type = obj.GetType();
                            if (type.Name == "PerformanceProgressBar")
                            {
                                ProgressBar cb = obj as ProgressBar;
                                if (cb.Tag.ToString() == linkinfo.LinkUrl)
                                {
                                    cb.IsIndeterminate = true;
                                }
                            }
                        }
                        if (lbxAudioList.ItemsSource != null)
                        {
                            History objHistory = new History();
                            objHistory.SaveAudioHistory((lbxAudioList.SelectedItem as ShowLinks).ShowID.ToString(), (lbxAudioList.SelectedItem as ShowLinks).Title);
                        }                        
                        string url = string.Empty;
                        if (ResourceHelper.AppName == Apps.Web_Media.ToString())
                            url = ResourceHelper.ProjectName + "/Songs/" + linkinfo.LinkUrl;
                        else
                            url = linkinfo.LinkUrl;

                        message = new ValueSet
                           {
                              {
                                 "Play",
                                 url
                              }
                           };
                        BackgroundMediaPlayer.SendMessageToBackground(message);
                        BackgroundMediaPlayer.Current.CurrentStateChanged += Current_CurrentStateChanged;
                        AppSettings.AudioShowID = AppSettings.ShowID;
                    }
                    else
                    {
                        BackgroundMediaPlayer.Current.Pause();
                        AppSettings.SongPlayImage = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadDownLoads method In ShowAudio.cs file.", ex);
            }
        }

        public void LoadDownLoads(ShowLinks linkinfo)
        {
            try
            {
                string linktype = "";
                AppSettings.ShowLinkTitle = linkinfo.LinkUrl;
                AppSettings.AudioTitle = linkinfo.Title;
                if (linktype == ".jpg" || linktype == ".png")
                {
                    galleryPopup.Show(linkinfo.LinkID.ToString());
                }
                else
                {
                    if (linkinfo.Songplay == Constants.PlayImagePath)
                    {                       
                        cboxes = PageHelper.GetChildsRecursive(lbxAudioList);
                        foreach (DependencyObject obj in cboxes.OfType<ProgressBar>())
                        {
                            Type type = obj.GetType();
                            if (type.Name == "PerformanceProgressBar")
                            {
                                ProgressBar cb = obj as ProgressBar;
                                if (cb.Tag.ToString() == linkinfo.LinkUrl)
                                {
                                    cb.IsIndeterminate = true;
                                }
                            }
                        }
                        if (lbxAudioList.ItemsSource != null)
                        {
                            History objHistory = new History();
                            objHistory.SaveAudioHistory((lbxAudioList.SelectedItem as ShowLinks).ShowID.ToString(), (lbxAudioList.SelectedItem as ShowLinks).Title);
                        }                        
                        string url = string.Empty;
                        if (ResourceHelper.AppName == Apps.Web_Media.ToString())
                            url = ResourceHelper.ProjectName + "/Songs/" + linkinfo.LinkUrl;
                        else
                            url = linkinfo.LinkUrl;

                        message = new ValueSet
                           {
                              {
                                 "Play",
                                 url
                              }
                           };
                        BackgroundMediaPlayer.SendMessageToBackground(message);
                        BackgroundMediaPlayer.Current.CurrentStateChanged+=Current_CurrentStateChanged;                       
                        AppSettings.AudioShowID = AppSettings.ShowID;
                    }
                    else
                    {
                        BackgroundMediaPlayer.Current.Pause();
                        AppSettings.SongPlayImage = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadDownLoads Method In ShowAudio file.", ex);
            }
        }
        void Current_CurrentStateChanged(MediaPlayer sender, object args)
        {
            MediaPlayerState playState = BackgroundMediaPlayer.Current.CurrentState;
            switch (playState)
            {
                case MediaPlayerState.Paused:
                    IEnumerable<DependencyObject> cboxes22 = PageHelper.GetChildsRecursive(lbxAudioList);
                    foreach (DependencyObject obj in cboxes22.OfType<ProgressBar>())
                    {
                        Type type = obj.GetType();
                        if (type.Name == "PerformanceProgressBar")
                        {
                            ProgressBar cb = obj as ProgressBar;
                            if (cb.Tag.ToString() == AppSettings.ShowLinkTitle)
                            {
                                cb.IsIndeterminate = false;
                            }
                        }
                    }
                    break;

                case MediaPlayerState.Playing:
                                      
                    foreach (DependencyObject obj in cboxes.OfType<ProgressBar>())
                    {
                        Type type = obj.GetType();
                        if (type.Name == "PerformanceProgressBar")
                        {
                            ProgressBar cb = obj as ProgressBar;
                            if (cb.Tag.ToString() == AppSettings.ShowLinkTitle)
                            {
                                cb.IsIndeterminate = false;
                            }
                        }
                    }
                    break;
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

            FrameworkElement element1 = (FrameworkElement)e.OriginalSource;
            var datacontext = element1.DataContext;
            selectedListBoxItem = this.lbxAudioList.ContainerFromItem(datacontext) as ListViewItem;
        }
        #endregion                
    }
}