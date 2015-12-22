using Common.Library;
using Common.Utilities;
using Microsoft.PlayerFramework;
using OnlineMovies.Views;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.UI;
using OnlineVideos.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideos.UserControls
{
    public sealed partial class FavoriteVideos : UserControl
    {
        #region GlobalDeclaration
        ListViewItem selectedListBoxItem;
        List<ShowLinks> showFavVideos = null;
        private bool IsDataLoaded;
        string Url;
        string Youtube_Url = string.Empty;
        string play_url = string.Empty;
        string Vid_title = string.Empty;
        string linkId = string.Empty;
        Uri url;
        ShowLinks desToUpdate = new ShowLinks();
        //StorageFile SourceFile;
        ShowLinks desToSelect = new ShowLinks();
        int linkid;
        #endregion

        #region Constructor
        public FavoriteVideos()
        {
            this.InitializeComponent();
            IsDataLoaded = false;
            showFavVideos = new List<ShowLinks>();
        }
        #endregion

        private async void lbxFavoritesongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string Title = (lbxFavoritesongs.SelectedItem as ShowLinks).Title.ToString();            
            try
            {
                if (lbxFavoritesongs.SelectedIndex == -1)
                    return;
                //AppBarButton a = (lbxFavoritesongs.SelectedItem as AppBarButton);
                AppSettings.LinkTitle = (lbxFavoritesongs.SelectedItem as ShowLinks).Title.ToString();
                AppSettings.LinkType = (lbxFavoritesongs.SelectedItem as ShowLinks).LinkType.ToString();
                //AppSettings.LinkUrl = (lbxFavoritesongs.SelectedItem as ShowLinks).LinkUrl.ToString();
                int linkid = (lbxFavoritesongs.SelectedItem as ShowLinks).LinkID;
                Url = (lbxFavoritesongs.SelectedItem as ShowLinks).LinkUrl;
                string LUrl = Url.Split(',')[1];

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

                if (ResourceHelper.AppName == Apps.Indian_Cinema_Pro.ToString() || ResourceHelper.AppName == Apps.Kids_TV_Pro.ToString() || ResourceHelper.AppName == Apps.Story_Time_Pro.ToString())
                    AppSettings.startplayingforpro = true;
                else
                    AppSettings.startplaying = true;

                if (ResourceHelper.AppName == Apps.Video_Mix.ToString())
                {
                    ////PageHelper.NavigateToDetailPage(AppResources.DetailPageName, AppSettings.ShowID);
                    //Frame frame = Window.Current.Content as Frame;
                    //Page p = frame.Content as Page;
                    //p.Frame.Navigate(typeof(Youtube), LUrl);
                    State.BackStack = "songs";
                    if (NetworkHelper.IsNetworkAvailableForDownloads() == true)
                    {
                        LUrl = LUrl.Replace("&amp;hl=en&amp;fs=1&amp;rel=0", "").Replace("http://www.youtube.com/v/", "").Replace("https://www.youtube.com/v/", "");
                        Frame frame = Window.Current.Content as Frame;
                        Page p = frame.Content as Page;                        
                        p.Frame.Navigate(typeof(Youtube), LUrl);
                    }
                    else
                    {
                        //p.Frame.Navigate(typeof(OfflineVideoPlayer), url);
                        //MessageDialog dialog = new MessageDialog("No Internet Connection available, Please connect to Internet to avail services","Play Offline Videos");
                        //await dialog.ShowAsync();

                        desToSelect = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkID == linkid).FirstOrDefaultAsync()).Result;
                        if (desToSelect.DownloadStatus == "1")
                        {
                            string showLinkTitle = string.Empty;
                            string Video_Title_offline = string.Empty;
                            //if (selectedListBoxItem == null)
                            //    return;
                            showLinkTitle = AppSettings.LinkTitle;
                            if (ResourceHelper.AppName == Apps.Video_Mix.ToString())
                            {
                                showLinkTitle = showLinkTitle.Split('-')[1].Trim();
                                Video_Title_offline = showLinkTitle.Split('|')[0].Trim() + ".mp4";
                            }
                            else
                            {
                                showLinkTitle = showLinkTitle.Split('-')[1].Trim();
                                Video_Title_offline = showLinkTitle + ".mp4";
                            }                                
                            Frame frame = Window.Current.Content as Frame;
                            Page p = frame.Content as Page;
                            p.Frame.Navigate(typeof(OfflineVideoPlayer), Video_Title_offline);
                        }
                        else
                        {
                            MessageDialog dialog = new MessageDialog("Video not available on your phone or No Internet Connection available.", "Warning");
                            await dialog.ShowAsync();
                        }
                    }
                }
                else if (ResourceHelper.AppName == Apps.Web_Media.ToString())
                {
                    
                }
                else
                {                    
                    if (NetworkHelper.IsNetworkAvailableForDownloads() == true)
                    {
                        Frame frame = Window.Current.Content as Frame;
                        Page p = frame.Content as Page;
                        p.Frame.Navigate(typeof(Youtube), LUrl);
                    }
                    else
                    {
                       //p.Frame.Navigate(typeof(OfflineVideoPlayer), url);
                       //MessageDialog dialog = new MessageDialog("No Internet Connection available, Please connect to Internet to avail services","Play Offline Videos");
                       //await dialog.ShowAsync();

                        desToSelect = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkID == linkid).FirstOrDefaultAsync()).Result;
                        if(desToSelect.DownloadStatus=="1")
                        {
                            string showLinkTitle = string.Empty;
                            //if (selectedListBoxItem == null)
                            //    return;
                            showLinkTitle = AppSettings.LinkTitle;
                            showLinkTitle = showLinkTitle.Split('-')[1].Trim();
                            string Video_Title_offline = showLinkTitle + ".mp4";
                            Frame frame = Window.Current.Content as Frame;
                            Page p = frame.Content as Page;
                            p.Frame.Navigate(typeof(OfflineVideoPlayer), Video_Title_offline);
                        }
                        else
                        {
                            MessageDialog dialog = new MessageDialog("Video not available on your phone or No Internet Connection available.", "Warning");
                            await dialog.ShowAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxFavoritesongs_SelectionChanged Method In FavoriteVideos.cs file.", ex);
                UtilitiesManager.LoadBrowserTask((lbxFavoritesongs.SelectedItem as ShowLinks).LinkUrl);
            }           
                //lbxFavoritesongs.SelectedIndex = -1;            
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OnlineVideos.View_Models.ShowDetail detailModel = new OnlineVideos.View_Models.ShowDetail();
                detailModel.AddToFavorites(lbxFavoritesongs, sender as MenuFlyoutItem, LinkType.Songs);

                lbxFavoritesongs.Visibility = Visibility.Collapsed;
                GetPageDataInBackground();
                //Frame frame = Window.Current.Content as Frame;
                //Page p = frame.Content as Page;
                //p.Frame.Navigate(typeof(StoryTimeFavorite));
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in btnFavSong_Click Method In FavoriteVideos.cs file.", ex);
            }
        }

        #region PageLoad
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {            
            try
            {
                if (IsDataLoaded == false)
                {                    
                    GetPageDataInBackground();
                    IsDataLoaded = true;                    
                }                
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in UserControl_Loaded Method In FavoriteVideos.cs file.", ex);
            }
        }
        #endregion

        #region "Common Methods"
        public void GetPageDataInBackground()
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();
                bwHelper.AddBackgroundTask(
                                             (object s, DoWorkEventArgs a) =>
                                             {
                                                 a.Result = FavoritesManager.GetFavoriteLinks(LinkType.Songs);
                                             },
                                                   (object s, RunWorkerCompletedEventArgs a) =>
                                                   {
                                                       showFavVideos = (List<ShowLinks>)a.Result;
                                                       if (showFavVideos.Count > 0)
                                                       {
                                                           lbxFavoritesongs.ItemsSource = showFavVideos;
                                                           lbxFavoritesongs.Visibility = Visibility.Visible;
                                                           tblkFavNoSongs.Visibility = Visibility.Collapsed;
                                                       }
                                                       else
                                                       {
                                                           tblkFavNoSongs.Text = "No " + AppResources.FavoriteSongsPivotTitle + " available";
                                                           tblkFavNoSongs.Visibility = Visibility.Visible;
                                                       }
                                                   }
                                                 );
                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In FavoriteVideos.cs file.", ex);
            }
        }
        #endregion

        private void StackPanel_Holding(object sender, HoldingRoutedEventArgs e)
        {            
            if (e.HoldingState != HoldingState.Started) return;

            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;
            
            FlyoutBase.ShowAttachedFlyout(element);
            FrameworkElement element1 = (FrameworkElement)e.OriginalSource;
            var datacontext = element1.DataContext;
            selectedListBoxItem = this.lbxFavoritesongs.ContainerFromItem(datacontext) as ListViewItem;  
        }

        //private void pin_to_start_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        //insights.Event("Video pin" + AppSettings.LinkTitle + "View");
        //        string showLinkTitle;
        //        Frame frame = Window.Current.Content as Frame;
        //        Page p = frame.Content as Page;
        //        Pivot p1 = (Pivot)(p.FindName("pvtMainDetails"));
        //        if (p1 != null)
        //            Constants.topsongnavigation = p1.SelectedIndex.ToString();
        //        ListViewItem selectedListBoxItem = this.lbxFavoritesongs.ContainerFromItem((sender as MenuFlyoutItem).DataContext) as ListViewItem;
        //        if (selectedListBoxItem == null)
        //            return;
        //        showLinkTitle = (selectedListBoxItem.Content as ShowLinks).Title;
        //        AppSettings.ShowLinkTitle = AppSettings.ShowID + (selectedListBoxItem.Content as ShowLinks).Title;
        //        ShellTileHelper_New.PinVideoLinkToStartScreen(AppSettings.ShowID, selectedListBoxItem.Content as ShowLinks);
        //    }
        //    catch (Exception ex)
        //    {
        //        Exceptions.SaveOrSendExceptions("Exception in btnpintostart_Click Method In ShowVideos.cs file.", ex);
        //    }
        //}

        //private void flyout_fav_Opened(object sender, object e)
        //{
        //    try
        //    {                
        //        MenuFlyout mainmenu = sender as MenuFlyout;
                
        //        foreach (MenuFlyoutItem contextMenuItem in mainmenu.Items)
        //        {
        //            if (selectedListBoxItem == null)
        //                return;
        //            if (contextMenuItem.Name == "pin_to_start")
        //            {
        //                string name = AppSettings.ShowID + (selectedListBoxItem.Content as ShowLinks).Title;
        //                string ID = Regex.Replace(name, @"\s", "");
        //                if (ShellTileHelper_New.IsPinned(ID) == true)
        //                {
        //                    contextMenuItem.Text = Constants.UnpinFromStartScreen;                            
        //                }
        //                else
        //                {
        //                    contextMenuItem.Text = Constants.PinToStartScreen;
        //                }
        //            }                   
        //        }
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //}

        private async void Download_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //ProgressBar ring = new ProgressBar();
                //ring.IsIndeterminate = true;
                //P_ring.IsActive = true;
                AppBarButton btn = sender as AppBarButton;
                Url = btn.Tag.ToString().Split(',')[1];                
                Vid_title = btn.Tag.ToString().Split(',')[0];
                
                linkId = btn.Tag.ToString().Split(',')[2];
                linkid = Convert.ToInt32(linkId);
                MyToolkit.Multimedia.YouTubeUri fianl_url;
               
                fianl_url =Task.Run(async()=> await MyToolkit.Multimedia.YouTube.GetVideoUriAsync(Url, MyToolkit.Multimedia.YouTubeQuality.Quality480P)).Result;
                
                string finale = fianl_url.Uri.AbsoluteUri.ToString();
                //string fin = fianl_url.ToString();
                url = new Uri(finale);
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(url);
                //ClosePopup();
                if (response.IsSuccessStatusCode)
                {
                    var file = await response.Content.ReadAsInputStreamAsync();
                    Stream str = file.AsStreamForRead();
                    byte[] mybyte = ReadToEnd(str);
                    StorageFile destinationFile = await KnownFolders.VideosLibrary.CreateFileAsync(Vid_title + ".mp4", CreationCollisionOption.ReplaceExisting);

                    Windows.Storage.Streams.IRandomAccessStream stream = await destinationFile.OpenAsync(FileAccessMode.ReadWrite);
                    IOutputStream output = stream.GetOutputStreamAt(0);

                    DataWriter writer = new DataWriter(output);
                    writer.WriteBytes(mybyte);
                    await writer.StoreAsync();
                    await output.FlushAsync();

                    //P_ring.IsActive = false;
                    //MessageDialog dialog = new MessageDialog(Vid_title + ".mp4 Downloaded successfully , and added to video library..!!","Download Completed");
                    //await dialog.ShowAsync();
                    //P_ring.IsActive = false;
                    //string command = "update ShowLinks set DownloadStatus=1 where LinkId='" + linkid + "'";
                    
                    desToUpdate = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkID == linkid).FirstOrDefaultAsync()).Result;
                    desToUpdate.DownloadStatus = "1";
                    await Task.Run(async () => await Constants.connection.UpdateAsync(desToUpdate));

                    GetPageDataInBackground();

                    desToSelect = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkID == linkid).FirstOrDefaultAsync()).Result;
                }
            }
            catch (Exception ex)
            {
                string excp = ex.Message;
                Exceptions.SaveOrSendExceptions("Exception in Download_Click Method In FavoriteVideos.xaml.cs file.", ex);
            }
        }     

        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;
            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }
            try
            {
                byte[] readBuffer = new byte[5120];
                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            System.Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            System.Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }
                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    System.Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }            
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }            
        }

        private async void delete_offline_video_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                AppBarButton btn = sender as AppBarButton;
                Url = btn.Tag.ToString().Split(',')[1];
                Vid_title = btn.Tag.ToString().Split(',')[0];
                linkId = btn.Tag.ToString().Split(',')[2];
                linkid = Convert.ToInt32(linkId);

                StorageFile SourceFile = await KnownFolders.VideosLibrary.GetFileAsync(Vid_title + ".mp4");
                await SourceFile.DeleteAsync();

                desToUpdate = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkID == linkid).FirstOrDefaultAsync()).Result;
                desToUpdate.DownloadStatus = "0";
                await Task.Run(async () => await Constants.connection.UpdateAsync(desToUpdate));

                GetPageDataInBackground();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in delete_offline_video_Click event in FavoriteVideos.xaml.cs file", ex);
            }
        }

        private void flyout_fav_Opened(object sender, object e)
        {

        }
    }
}