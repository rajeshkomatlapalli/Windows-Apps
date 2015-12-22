using Common.Library;
using Microsoft.Advertising.WinRT.UI;
using OnlineVideos;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.ApplicationSettings;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Reflection;
using System.Threading.Tasks;
using OnlineVideos.Views;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class Shows : UserControl
    {
        bool check = false;
        ShowLinks selecteditem = null;
        public static Shows current = null;
        public ListGroups Groups = default(ListGroups);
        public ListGroups GroupsForTamilAndUpcomming = default(ListGroups);
        List<ShowLinks> links = new List<ShowLinks>();
        List<ShowLinks> objlist = new List<ShowLinks>();
        MediaElement RootMediaElement = default(MediaElement);
        //MediaPlayer RootMediaElement = new MediaPlayer();
        List<ShowLinks> StatusImageCode = new List<ShowLinks>();
        IEnumerable<DependencyObject> cboxes = null;
        AppInsights insights = new AppInsights();
        MediaElement MediaPlayer;
        public Shows()
        {
            try
            {
                this.InitializeComponent();
                this.Tag = this;
                current = this;
                progressbar.IsActive = true;
                Window.Current.SizeChanged += Current_SizeChanged;

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Shows Method In Shows.cs file", ex);                
            }
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            try
            {
                ApplicationViewState currentState = Windows.UI.ViewManagement.ApplicationView.Value;
                if (currentState == ApplicationViewState.Snapped)
                {
                    VisualStateManager.GoToState(this, "Snapped", false);
                }
                else
                {
                    VisualStateManager.GoToState(this, "Filled", false);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Current_SizeChanged Method In Shows.cs file", ex);                
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {                
                insights.Event("Shows Loaded");
                Groups = new ListGroups();
                GroupsForTamilAndUpcomming = new ListGroups();
                Window.Current.CoreWindow.PointerWheelChanged += CoreWindow_PointerWheelChanged;

                if (AppSettings.MainPageAudioCategory != 0 && AppSettings.MainPageAudioCategory != null)
                    musiccategory.SelectedIndex = AppSettings.MainPageAudioCategory;
                else
                    musiccategory.SelectedIndex = 0;

                if (AppSettings.MainPageSongCategory != 0 && AppSettings.MainPageSongCategory != null)
                    categorys.SelectedIndex = AppSettings.MainPageSongCategory;
                else
                    categorys.SelectedIndex = 0;

                SettingsPane.GetForCurrentView().CommandsRequested += onCommandsRequested;
                GetTopRatedinBackground();
                checkstate();
                groupedItemsViewSource.Source = Groups.Items;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in UserControl_Loaded Method In Shows.cs file", ex); 
            }
        }

        private void CoreWindow_PointerWheelChanged(CoreWindow sender, PointerEventArgs args)
        {
            if (args.CurrentPoint.Properties.MouseWheelDelta == (-120))
            {
                //MouseWheel Backward scroll
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset + Window.Current.CoreWindow.Bounds.Width / 10);
            }
            if (args.CurrentPoint.Properties.MouseWheelDelta == (120))
            {
                //MouseWheel Forward scroll
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - Window.Current.CoreWindow.Bounds.Width / 10);
            }
        }

        //private void AdVisible()
        //{
        //    if (AppResources.advisible == true)
        //    {
        //        //AdControl.ApplicationId = AppResources.adApplicationId;
        //        //AdControl.AdUnitId = AppResources.adUnitId;
        //        AdControl adctrl = new AdControl();
        //        adctrl.ApplicationId = AppResources.adApplicationId;
        //        adctrl.AdUnitId = AppResources.adUnitId;
        //        adctrl.ApplicationId = AppResources.adApplicationId1;
        //        adctrl.AdUnitId = AppResources.adUnitId1;
        //        //AdControl1.ApplicationId = AppResources.adApplicationId1;
        //        //AdControl1.AdUnitId = AppResources.adUnitId1;

        //        if (AppSettings.ProjectName == "Indian_Cinema.Windows")
        //        {
        //            advisibleForTamil.Visibility = Visibility.Visible;
        //            advisibleForTamil.Margin = new Thickness(-20, 60, 0, 0);
        //            //AdControlupcomming.Width = 250;
        //            //AdControlupcomming.Height = 250;
        //            //AdRotatorupcommingduplex.Width = 250;
        //            //AdRotatorupcommingduplex.Height = 250;
        //            advisible12.Margin = new Thickness(-47, 60, 0, 0);
        //            //AdControl12.Width = 250;
        //            //AdControl12.Height = 250;
        //            //AdRotator12duplex.Invalidate();
        //            //AdRotator12duplex.Width = 250;
        //            //AdRotator12duplex.Height = 250;
        //            advisible12.Visibility = Visibility.Visible;
        //        }
        //    }
        //    else
        //    {
        //        ColumnDefinition mycol = MusicAndSongsGrid.ColumnDefinitions[2];
        //        MusicAndSongsGrid.ColumnDefinitions.Remove(mycol);
        //        RowDefinition myrow = snapgrid.RowDefinitions[2];
        //        snapgrid.RowDefinitions.Remove(myrow);
        //    }
        //}

        private void onCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            advisible.Visibility = Visibility.Collapsed;
        }

        private void checkstate()
        {
            try
            {
                ApplicationViewState currentState = Windows.UI.ViewManagement.ApplicationView.Value;
                if (currentState == ApplicationViewState.Snapped)
                {
                    VisualStateManager.GoToState(this, "Snapped", false);
                }
                else
                {
                    VisualStateManager.GoToState(this, "Filled", false);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in checkstate Method In Shows.cs file", ex);                
            }
        }

        private void snaplstvwaudiosongs_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {                
                if (itemlistview.SelectedIndex == -1)
                    return;
                if (check == true)
                {
                    check = false;
                    selecteditem = (sender as Selector).SelectedItem as ShowLinks;
                    Constants.selecteditem = selecteditem;
                    AppSettings.LinkType = selecteditem.LinkType;
                    itemlistview.SelectedIndex = -1;
                    return;
                }
                AppSettings.LiricsLink = (itemlistview.SelectedItem as ShowLinks).Title;
                AppSettings.ShowID = (itemlistview.SelectedItem as ShowLinks).ShowID.ToString();
                Constants.AppbarTitle = (itemlistview.SelectedItem as ShowLinks).Title;
                insights.Event(AppSettings.LiricsLink + "play");
                Constants.Link = (itemlistview.SelectedItem as ShowLinks).Title + "$" + (itemlistview.SelectedItem as ShowLinks).LinkUrl;
                History history = new History();
                history.SaveAudioHistory((itemlistview.SelectedItem as ShowLinks).ShowID.ToString(), (itemlistview.SelectedItem as ShowLinks).Title, (itemlistview.SelectedItem as ShowLinks).LinkUrl, (itemlistview.SelectedItem as ShowLinks).Rating.ToString());
                //songblock.Text = Constants.AppbarTitle;
                LoadLSnapyrics();
                controlplay();
                controlImage();
                itemlistview.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in snaplstvwaudiosongs_SelectionChanged_1 Method In ShowAudio.cs file", ex);                
            }
        }

        private void LoadLSnapyrics()
        {
            try
            {
                if (Constants.CloseLyricspopup.IsOpen == true)
                    Constants.CloseLyricspopup.IsOpen = false;
                Constants._PlayList.Clear();
                foreach (ShowLinks s in itemlistview.Items.Cast<ShowLinks>())
                {
                    try
                    {
                        Constants._PlayList.Add(s.Title + "$" + s.LinkUrl, "Stop");
                    }
                    catch (Exception ex)
                    {
                        Exceptions.SaveOrSendExceptions("Exception in LoadLyrics Method In ShowAudio.cs file", ex);                        
                    }
                }
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                var page = new LyricsPopUp();
                Constants.CloseLyricspopup = new Popup();
                Constants.CloseLyricspopup.Child = page;
                Constants.CloseLyricspopup.IsOpen = true;
                Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                MediaPlayer = (MediaElement)border.FindName("MediaPlayer");
                //DependencyObject rootGrid = VisualTreeHelper.GetChild(Window.Current.Content, 0);
                //MediaPlayer = (MediaElement)VisualTreeHelper.GetChild(rootGrid, 0) as MediaElement;
                //MediaPlayer = (MediaElement)VisualTreeHelper.GetChild(rootGrid, 1) as MediaElement;

                MediaPlayer.Source = new Uri(Constants.Link.Split('$')[1], UriKind.Absolute);                               
                MediaPlayer.Play();
                Constants._PlayList[Constants.Link] = "Play";
                TextBlock song = (TextBlock)page.GetType().GetTypeInfo().GetDeclaredField("songblock").GetValue(page);
                song.Text = Constants.Link.Split('$')[0];
                page.GetType().GetTypeInfo().GetDeclaredMethod("changeimage").Invoke(page, null);
                page.GetType().GetTypeInfo().GetDeclaredMethod("LyricsPopup").Invoke(page, new object[] { true });
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadLyrics Method In ShowAudio.cs file", ex);                
            }
        }
        public void controlplay()
        {
            try
            {
                MediaControl.PlayPressed += MediaControl_PlayPressed;
                MediaControl.PausePressed += MediaControl_PausePressed;
                MediaControl.PlayPauseTogglePressed += MediaControl_PlayPauseTogglePressed;
                MediaControl.StopPressed += MediaControl_StopPressed;
                MediaControl.NextTrackPressed += MediaControl_NextTrackPressed;
                MediaControl.PreviousTrackPressed += MediaControl_PreviousTrackPressed;
            }
            catch (Exception ex)
            {
                string exc = ex.Message;
                Exceptions.SaveOrSendExceptions("Exception in controlplay Method In ShowAudio.cs file", ex);                
            }
        }
       
        async void MediaControl_PreviousTrackPressed(object sender, object e)
        {
            try
            {
                if (Constants._PlayList != null)
                {
                    string[] arr = new string[2];
                    int index = 0;
                    foreach (KeyValuePair<string, string> dic in Constants._PlayList)
                    {
                        if (dic.Value == "Play")
                        {
                            break;
                        }
                        else
                        {
                            index++;
                        }
                    }
                    Constants._PlayList[Constants._PlayList.ElementAt(index).Key] = "Stop";
                    if (index == 0)
                    {
                        index = Constants._PlayList.Count - 1;
                        Constants._PlayList[Constants._PlayList.ElementAt(index).Key] = "Play";
                        arr = Constants._PlayList.ElementAt(index).Key.ToString().Split('$');
                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            RootMediaElement.Source = new Uri(arr[1].ToString(), UriKind.Absolute);
                        });
                        Constants.AppbarTitle = arr[0].ToString();
                    }
                    else
                    {
                        Constants._PlayList[Constants._PlayList.ElementAt(index - 1).Key] = "Play";
                        arr = Constants._PlayList.ElementAt(index - 1).Key.ToString().Split('$');
                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            RootMediaElement.Source = new Uri(arr[1].ToString(), UriKind.Absolute);
                        });
                        Constants.AppbarTitle = arr[0].ToString();
                    }
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        RootMediaElement.Play();
                        AppSettings.ChannelTitle = Constants.AppbarTitle;
                        controlImage();
                    });
                }
            }
            catch (Exception ex)
            {
                string exc = ex.Message;
            }
        }

        async void MediaControl_StopPressed(object sender, object e)
        {
            try
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => RootMediaElement.Stop());
                controlImage();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in MediaControl_StopPressed Method In ShowAudio.cs file", ex);               
            }
        }
        async void MediaControl_PlayPauseTogglePressed(object sender, object e)
        {
            try
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (RootMediaElement.CurrentState == MediaElementState.Paused)
                    {
                        RootMediaElement.Play();
                    }
                    else
                    {
                        RootMediaElement.Pause();
                    }
                });
                controlImage();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in MediaControl_PlayPauseTogglePressed Method In ShowAudio.cs file", ex);                
            }
        }
        async void MediaControl_PausePressed(object sender, object e)
        {
            try
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => RootMediaElement.Pause());
                controlImage();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in MediaControl_PausePressed Method In ShowAudio.cs file", ex);
                
            }
        }
        async void MediaControl_PlayPressed(object sender, object e)
        {
            try
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => RootMediaElement.Play());
                controlImage();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in MediaControl_PlayPressed Method In ShowAudio.cs file", ex);
                
            }
        }

        public async void controlImage()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation as StorageFolder;
                string imgpth = AppSettings.TileImage.Substring(0, AppSettings.TileImage.LastIndexOf('.'));
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"Images\TileImages\150-150\" + imgpth + ".png",Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                //AudioTrack Track = new AudioTrack();
                string filename = AppSettings.ChannelImage + ".png";
                //Track.ThumbUri = new Uri("ms-appdata:///local/" + filename);
                using (StorageItemThumbnail storageItemThumbnail = await file.GetThumbnailAsync(ThumbnailMode.SingleItem, 500, ThumbnailOptions.ResizeThumbnail))
                using (IInputStream inputStream = storageItemThumbnail.GetInputStreamAt(0))
                using (var Reader = new DataReader(inputStream))
                {
                    uint u = await Reader.LoadAsync((uint)storageItemThumbnail.Size);
                    IBuffer Buffer = Reader.ReadBuffer(u);
                    var local = ApplicationData.Current.LocalFolder;
                    var File = await local.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                    using (IRandomAccessStream randomAccessStream = await File.OpenAsync(FileAccessMode.ReadWrite))
                    using (IOutputStream outputStream = randomAccessStream.GetOutputStreamAt(0))
                    {
                        await outputStream.WriteAsync(Buffer);
                        await outputStream.FlushAsync();
                    }
                }
                MediaControl.TrackName = AppSettings.ChannelTitle;
                MediaControl.ArtistName = AppSettings.ChannelImage;
                //MediaControl.AlbumArt = Track.ThumbUri;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }

        private void tblkAudioSongs_RightTapped_1(object sender, RightTappedRoutedEventArgs e)
        {

        }

        private void tbgetmore_Click_1(object sender, RoutedEventArgs e)
        {
            Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
            p.GetType().GetTypeInfo().GetDeclaredMethod("TopMusic").Invoke(p, null);
        }

        private void view_PointerEntered_1(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                if (AppSettings.ProjectName == "Online Education")
                {
                    ((Canvas)((TextBlock)sender).Parent).Background = new SolidColorBrush(Colors.White);
                    ((TextBlock)sender).Foreground = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    //((Canvas)((TextBlock)sender).Parent).Background = new SolidColorBrush(Colors.White);
                    //((TextBlock)sender).Foreground = new SolidColorBrush(Colors.Black);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in view_PointerEntered_1 Method In Shows.cs file", ex);
                
            }
        }

        public void GetRecentinBackground()
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();
                bwHelper.AddBackgroundTask(
                                          (object s, DoWorkEventArgs a) =>
                                          {
                                              a.Result = OnlineShow.GetRecentlyAddedShows();
                                              if (!AppSettings.LiveTileBackgroundAgentStatus)
                                              {
                                                  //SendLocalImageTile sendlocalimagetile = new SendLocalImageTile();
                                                  //Constants.LiveTileBackgroundAgentStatus = false;
                                                  //sendlocalimagetile.UpdateTileWithImage();
                                                  //AppSettings.LiveTileBackgroundAgentStatus = true;
                                              }
                                          },
                                          (object s, RunWorkerCompletedEventArgs a) =>
                                          {
                                              Groups.Items.Add((ListGroup)a.Result);

                                              progressbar.IsActive = false;
                                              if (AppSettings.ProjectName == "Indian Cinema" || AppSettings.ProjectName == "Indian Cinema Pro" || AppSettings.ProjectName == "Indian_Cinema")
                                              {
                                                  groupedItemsViewSourceForTamilAndUpcomming.Source = GroupsForTamilAndUpcomming.Items;
                                                  GetTamilShows();
                                                  //Addvisible();
                                              }
                                              else
                                              {
                                                  GetListviewinBackground();
                                                  if (AppSettings.ProjectName == "Bollywood Music")
                                                  {
                                                      GetPageDataForTopAudiosInBackground(Int32.Parse(((ComboBoxItem)musiccategory.SelectedItem).Tag.ToString()));
                                                      topmusicsongs.Visibility = Visibility.Visible;
                                                      musiccategory.Visibility = Visibility.Collapsed;
                                                      snapgrid1.Visibility = Visibility.Collapsed;
                                                  }
                                                  if (AppResources.advisible == true)
                                                  {
                                                      advisible.Visibility = Visibility.Visible;
                                                  }
                                              }
                                              if (AppSettings.ProjectName == "Web Tile")
                                              {
                                                  if (Groups.Items[0].Items.Count() == 0)
                                                  {
                                                      viewbx.Visibility = Visibility.Collapsed;
                                                      advisible.Visibility = Visibility.Collapsed;
                                                      Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                                                      ((TextBlock)p.FindName("notile")).Visibility = Visibility.Visible;
                                                      //viewbx.MaxWidth = 1200;
                                                      //viewbx.MaxHeight = 1200;
                                                      //viewbx.Margin=new Thickness(0,0,0,40);
                                                      //viewbx.VerticalAlignment = VerticalAlignment.Top;
                                                  }
                                                  else if (Groups.Items[0].Items.Count() == 1)
                                                  {
                                                      viewbx.Visibility = Visibility.Visible;
                                                      advisible.Visibility = Visibility.Visible;
                                                      viewbx.MaxWidth = Double.MaxValue;
                                                      viewbx.MaxHeight = Double.MaxValue;
                                                      viewbx.Margin = new Thickness(0, 0, 30, 40);
                                                      viewbx.VerticalAlignment = VerticalAlignment.Top;
                                                  }
                                                  else
                                                  {
                                                      viewbx.Visibility = Visibility.Visible;
                                                      advisible.Visibility = Visibility.Visible;
                                                      Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                                                      ((TextBlock)p.FindName("notile")).Visibility = Visibility.Collapsed;
                                                  }
                                              }
                                          }
                                        );
                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetRecentinBackground Method In Shows.cs file", ex);
                
            }
        }
        public void GetTamilShows()
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();
                bwHelper.AddBackgroundTask(
                                          (object s, DoWorkEventArgs a) =>
                                          {
                                              a.Result = OnlineShow.GetTamilAddedShows();
                                          },
                                          (object s, RunWorkerCompletedEventArgs a) =>
                                          {
                                              GroupsForTamilAndUpcomming.Items.Add((ListGroup)a.Result);
                                              progressbar.IsActive = false;
                                              GetUpComingMoviesBackground();
                                          }
                                        );
                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetRecentinBackground Method In Shows.cs file", ex);
                
            }
        }

        //public void Addvisible()
        //{
        //    var ad = new Microsoft.Advertising.WinRT.UI.AdControl();
        //    AdControl.Height = 600;
        //    AdControl.Width = 160;
        //    itemGridView.Margin = new Thickness(0, 0, 68, 0);
        //    MusicAndSongsGrid.Margin = new Thickness(50, 0, 0, 0);
        //}
        public void GetUpComingMoviesBackground()
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();
                bwHelper.AddBackgroundTask(
                                          (object s, DoWorkEventArgs a) =>
                                          {
                                              a.Result = OnlineShow.GetUpComingMovies();
                                          },
                                          (object s, RunWorkerCompletedEventArgs a) =>
                                          {
                                              GroupsForTamilAndUpcomming.Items.Add((ListGroup)a.Result);
                                              if (Groups.Items.Count == 1)
                                              {
                                                  txtmsg1.Text = "no images,no audios,no video plz download";
                                                  txtmsg1.Visibility = Visibility.Visible;
                                              }
                                              progressbar.IsActive = false;
                                              AppSettings.MainPageAudioCategory = musiccategory.SelectedIndex;

                                              if (AppSettings.TopAudio.Count > 0 && AppSettings.TopSong.Count > 0)
                                              {
                                                  topmusicsongs.Visibility = Visibility.Visible;
                                                  itemlistview.ItemsSource = AppSettings.TopAudio;

                                                  if (NetworkHelper.IsNetworkAvailable())
                                                  {
                                                      itemlistview1.ItemsSource = AppSettings.TopSong;
                                                  }
                                                  else
                                                  {
                                                      itemlistview1.ItemsSource = objlist;
                                                      progressbarFortopsongs.IsActive = false;
                                                      txtmsg2.Text = "Network not Available";
                                                      txtmsg2.Visibility = Visibility.Visible;
                                                  }
                                                  progressbarForTopmusic.IsActive = false;
                                                  progressbarFortopsongs.IsActive = false;
                                                  musiccategory.SelectionChanged += ComboBox_SelectionChanged;
                                                  categorys.SelectionChanged += Categorys_SelectionChanged;
                                                  GetListviewinBackground();
                                              }
                                              else
                                              {
                                                  GetPageDataForTopAudiosInBackground(Int32.Parse(((ComboBoxItem)musiccategory.SelectedItem).Tag.ToString()));
                                              }
                                          }
                                        );
                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetRecentinBackground Method In Shows.cs file", ex);
                
            }
        }

        public void GetTopRatedinBackground()
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();

                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                a.Result = OnlineShow.GetTopRatedShows();
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                if (AppSettings.ProjectName == "Web Tile" && Groups.Items.Count == 1)
                                                    Groups.Items.Clear();
                                                Groups.Items.Add((ListGroup)a.Result);
                                                GetRecentinBackground();
                                            }
                                          );
                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetTopRatedinBackground Method In Shows.cs file", ex);
                
            }
        }
        private void GetPageDataForTopAudiosInBackground(int catid)
        {
            try
            {

                int count = 1;
                BackgroundHelper bwHelper = new BackgroundHelper();
                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                a.Result = OnlineShow.GetTopRatedAudio(catid);
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                links = (List<ShowLinks>)a.Result;
                                                foreach (ShowLinks link in links.Take(10))
                                                {
                                                    link.RatingBitmapImage = ImageHelper.LoadRatingImage(link.Rating.ToString());
                                                    link.SongNO = count.ToString() + ".";
                                                    count++;
                                                }
                                                if (links.Count > 0)
                                                {
                                                    AppSettings.TopAudio = links;
                                                    itemlistview.ItemsSource = links;
                                                    progressbarForTopmusic.IsActive = false;
                                                }
                                                else
                                                {
                                                    progressbarForTopmusic.IsActive = false;
                                                    txtmsg1.Visibility = Visibility.Visible;
                                                }
                                                AppSettings.MainPageSongCategory = categorys.SelectedIndex;
                                                if (AppSettings.ProjectName != "Bollywood Music")
                                                {
                                                    GetPageDataForTopSongsInBackground(Int32.Parse(((ComboBoxItem)categorys.SelectedItem).Tag.ToString()));
                                                }
                                                musiccategory.SelectionChanged += ComboBox_SelectionChanged;
                                            }
                                          );
                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In ShowVideos.cs file", ex);
                
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          AppSettings.MainPageAudioCategory = (sender as ComboBox).SelectedIndex;
          GetPageDataForTopAudios(Int32.Parse(((ComboBoxItem)(sender as ComboBox).SelectedItem).Tag.ToString()));
        }

        private void GetPageDataForTopSongsInBackground(int catid)
        {
            try
            {
                topmusicsongs.Visibility = Visibility.Visible;
                int count = 1;
                BackgroundHelper bwHelper = new BackgroundHelper();
                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                a.Result = OnlineShow.GetTopRatedSongs(catid);
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                if (NetworkHelper.IsNetworkAvailable())
                                                {
                                                    objlist = (List<ShowLinks>)a.Result;
                                                    StatusImageCode = objlist.Where(i => i.StatusCode == true).Take(10).ToList();
                                                    int songno = 1;
                                                    foreach (ShowLinks link in StatusImageCode.Take(10))
                                                    {
                                                        link.RatingBitmapImage = ImageHelper.LoadRatingImage(link.Rating.ToString());
                                                        link.SongNO = songno.ToString() + ".";
                                                        songno++;
                                                    }
                                                    if (objlist.Count != 0)
                                                    {
                                                        AppSettings.TopSong = StatusImageCode;
                                                        itemlistview1.ItemsSource = StatusImageCode;
                                                        progressbarFortopsongs.IsActive = false;
                                                    }
                                                    else
                                                    {
                                                        progressbarFortopsongs.IsActive = false;
                                                        txtmsg2.Text = "No videos  available";
                                                        txtmsg2.Visibility = Visibility.Visible;
                                                    }
                                                }
                                                else
                                                {
                                                    itemlistview1.ItemsSource = objlist;
                                                    progressbarFortopsongs.IsActive = false;
                                                    txtmsg2.Text = "Network not Available";
                                                    txtmsg2.Visibility = Visibility.Visible;
                                                }
                                                categorys.SelectionChanged += Categorys_SelectionChanged;
                                                GetListviewinBackground();
                                            }
                                          );
                advisible.Visibility = Visibility.Visible;
                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In ShowVideos.cs file", ex);
                
            }
        }

        private void Categorys_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AppSettings.MainPageSongCategory = (sender as ComboBox).SelectedIndex;
            GetPageDataForTopSongs(Int32.Parse(((ComboBoxItem)(sender as ComboBox).SelectedItem).Tag.ToString()));
        }

        private void GetPageDataForTopSongs(int catid)
        {
            try
            {
                topmusicsongs.Visibility = Visibility.Visible;
                progressbarFortopsongs.IsActive = true;
                objlist.Clear();
                objlist = OnlineShow.GetTopRatedSongs(catid);
                StatusImageCode = objlist.Where(i => i.StatusCode == true).Take(10).ToList();
                int songno = 1;
                foreach (ShowLinks link in StatusImageCode.Take(10))
                {
                    link.RatingBitmapImage = ImageHelper.LoadRatingImage(link.Rating.ToString());
                    link.SongNO = songno.ToString() + ".";
                    songno++;
                }
                if (objlist.Count != 0)
                {
                    AppSettings.TopSong = StatusImageCode;
                    itemlistview1.ItemsSource = StatusImageCode;
                    progressbarFortopsongs.IsActive = false;
                }
                else
                {
                    progressbarFortopsongs.IsActive = false;
                    txtmsg2.Text = "No videos  available";
                    txtmsg2.Visibility = Visibility.Visible;
                }
                if (AppSettings.ProjectName == "Indian Cinema Pro")
                {
                    advisible.Visibility = Visibility.Collapsed;
                }
                else
                    advisible.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataForTopSongs Method In ShowVideos.cs file", ex);
                
            }
        }

        private void GetPageDataForTopAudios(int catid)
        {
            try
            {
                progressbarForTopmusic.IsActive = true;
                int count = 1;
                links.Clear();
                links = OnlineShow.GetTopRatedAudio(catid);
                foreach (ShowLinks link in links.Take(10))
                {
                    link.RatingBitmapImage = ImageHelper.LoadRatingImage(link.Rating.ToString());
                    link.SongNO = count.ToString() + ".";
                    count++;
                }
                if (links.Count > 0)
                {
                    AppSettings.TopAudio = links;
                    itemlistview.ItemsSource = links.Take(10);
                    progressbarForTopmusic.IsActive = false;
                }
                else
                {
                    progressbarForTopmusic.IsActive = false;
                    txtmsg1.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataForTopAudios Method In ShowVideos.cs file", ex);
                
            }
        }

        private void GetListviewinBackground()
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();
                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                a.Result = OnlineShow.GetTopRatedListViewShows();
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                ItemListView.ItemsSource = a.Result;
                                                progressbarsnap.IsActive = false;
                                            }
                                          );
                //AdVisible();

                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetListviewinBackground Method In Shows.cs file", ex);
                
            }
        }
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                itemclicked(sender, e);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ItemView_ItemClick Method In Shows.cs file", ex);
                
            }
        }

        private void itemclicked(object sender, ItemClickEventArgs e)
        {
            try
            {
                Constants.AppbarImage = (e.ClickedItem as ShowList).Image;
                AppSettings.ChannelImage = (e.ClickedItem as ShowList).Title;
                AppSettings.TileImage = (e.ClickedItem as ShowList).TileImage;
                AppSettings.ShowID = (e.ClickedItem as ShowList).ShowID.ToString();
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("DetailPage").Invoke(p, null);
                if (check == true)
                {
                    check = false;
                    Constants.selecteditem = e.ClickedItem as ShowLinks;
                    AppSettings.LinkType = (e.ClickedItem as ShowLinks).LinkType;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in itemclicked Method In Shows.cs file", ex);
                
            }
        }

        private void view_PointerExited_1(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                if (AppSettings.ProjectName == "Online Education")
                {
                    ((Canvas)((TextBlock)sender).Parent).Background = new SolidColorBrush(Colors.Transparent);
                    ((TextBlock)sender).Foreground = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    //((Canvas)((TextBlock)sender).Parent).Background = new SolidColorBrush(Colors.Transparent);
                    //((TextBlock)sender).Foreground = new SolidColorBrush(Colors.White);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in view_PointerExited_1 Method In Shows.cs file", ex);
                
            }
        }

        private void view_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                AppSettings.ViewAllTitle = ((TextBlock)sender).Tag.ToString();
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("VideoListPage").Invoke(p, null);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in view_Tapped_1 Method In Shows.cs file", ex);
                
            }
        }

        private void ItemListView_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            try
            {
                itemclicked(sender, e);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ItemListView_ItemClick_1 Method In Shows.cs file", ex);                
            }
        }

        private void snaplstvwsongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int Count = itemlistview1.SelectedIndex;
                AppSettings.LinkType = string.Empty;
                AppSettings.Status = string.Empty;
                if (itemlistview1.SelectedIndex == -1)
                    return;
                if (check == true)
                {
                    check = false;
                    selecteditem = (sender as Selector).SelectedItem as ShowLinks;
                    Constants.selecteditem = selecteditem;
                    AppSettings.LinkType = selecteditem.LinkType;
                    insights.Event(AppSettings.PlayVideoTitle + "view");
                    itemlistview1.SelectedIndex = -1;
                    return;
                }

                var selecteditem1 = (sender as Selector).SelectedItem as ShowLinks;
                History history = new History();
                history.SaveVideoHistory((itemlistview1.SelectedItem as ShowLinks).ShowID.ToString(), (itemlistview1.SelectedItem as ShowLinks).Title, (itemlistview1.SelectedItem as ShowLinks).LinkUrl);
                AppSettings.LinkUrl = selecteditem1.LinkUrl;
                AppSettings.ShowID = selecteditem1.ShowID.ToString();
               
                itemlistview1.SelectedIndex = -1;
                //if (AppSettings.ProjectName == "Indian Cinema Pro")
                //{
                //    var Itemcollection = (sender as ListView).Items.ToList();
                //    Constants.YoutubePlayList = new List<string>();
                //    foreach (var item in Itemcollection.Cast<ShowLinks>())
                //    {
                //        Constants.YoutubePlayList.Add(item.LinkUrl);
                //    }
                //    Constants.YoutubePlayList.Remove(selecteditem1.LinkUrl);
                //    Constants.YoutubePlayList.RemoveRange(0, Count);
                //}
                //else
                //{

                var Itemcollection = (sender as ListView).Items.ToList();
                
                Constants.YoutubePlayList = new Dictionary<string, string>();
                foreach (var item in Itemcollection.Cast<ShowLinks>().Where(i => i.LinkOrder != selecteditem1.LinkOrder))
                {
                    Constants.YoutubePlayList.Add(item.LinkUrl, item.Title);
                }
                //}
                AppSettings.PlayVideoTitle = selecteditem1.Title;
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("Youtube").Invoke(p, null);
            }
            catch (Exception ex)
            {
                string exc = ex.Message;
                Exceptions.SaveOrSendExceptions("Exception in snaplstvwsongs_SelectionChanged Method In ShowVideos.cs file", ex);
                
            }
        }
        
        void MediaControl_NextTrackPressed(object sender, object e)
        {
            try
            {
                if (Constants._PlayList != null)
                {
                    string[] arr = new string[2];
                    int index = 0;

                    foreach (KeyValuePair<string, string> dic in Constants._PlayList)
                    {
                        if (dic.Value == "Play")
                        {
                            break;
                        }
                        else
                        {
                            index++;
                        }
                    }
                    Constants._PlayList[Constants._PlayList.ElementAt(index).Key] = "Stop";
                    int k = Constants._PlayList.Count;
                    if (Constants._PlayList.Count - 1 == index)
                    {
                        index = 0;
                        Constants._PlayList[Constants._PlayList.ElementAt(index).Key] = "Play";
                        arr = Constants._PlayList.ElementAt(index).Key.ToString().Split('$');
                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            RootMediaElement.Source = new Uri(arr[1].ToString(), UriKind.Absolute);
                        });
                        Constants.AppbarTitle = arr[0].ToString();
                    }
                    else
                    {
                        Constants._PlayList[Constants._PlayList.ElementAt(index + 1).Key] = "Play";
                        string ph = Constants._PlayList.ElementAt(index + 1).Key.ToString();
                        arr = Constants._PlayList.ElementAt(index + 1).Key.ToString().Split('$');
                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            RootMediaElement.Source = new Uri(arr[1].ToString(), UriKind.Absolute);
                        });
                        Constants.AppbarTitle = arr[0].ToString();
                    }
                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        RootMediaElement.Play();
                        AppSettings.ChannelTitle = Constants.AppbarTitle;
                        controlImage();
                    });
                }
            }
            catch (Exception ex)
            {
                string exc = ex.Message;
            }
        }

        private void tblkChapter_RightTapped_1(object sender, RightTappedRoutedEventArgs e)
        {

        }

        private void tbgetmore1_Click_1(object sender, RoutedEventArgs e)
        {
            Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
            p.GetType().GetTypeInfo().GetDeclaredMethod("TopSongs").Invoke(p, null);
        }
    }
}