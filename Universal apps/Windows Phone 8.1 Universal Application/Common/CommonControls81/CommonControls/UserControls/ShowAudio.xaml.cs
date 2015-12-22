using Common.Library;
using comm = OnlineVideos.Common;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
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
using Microsoft.PlayerFramework;
using OnlineVideos.Views;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class ShowAudio : UserControl
    {
        AppInsights insights = new AppInsights();
        public static ShowAudio currents = null;
        public TextBlock songblock = null;
        // Popup popgallimages;
        bool check = false;
        bool checksong = false;
        ShowLinks selecteditem = null;
        DispatcherTimer timer = new DispatcherTimer();
        //MediaPlayer RootMediaElement = new MediaPlayer();
        //public static MediaElement RootMediaElement = new MediaElement();
        //public static MediaElement MediaPlayer = default(MediaElement);
        List<ShowLinks> links = new List<ShowLinks>();
        //MediaElement RootMediaElement = new MediaElement();
        public ShowAudio()
        {
            try
            {
            this.InitializeComponent();
            currents = this;
            songblock = new TextBlock();
            Loaded += ShowAudio_Loaded;
            progressbar.IsActive = true;
            Window.Current.SizeChanged += Current_SizeChanged;
                // popgallimages = new Popup();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowAudio Method In ShowAudio.cs file", ex);
            }
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
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
                Exceptions.SaveOrSendExceptions("Exception in Current_SizeChanged Method In ShowAudio.cs file", ex);
            }
        }

        void ShowAudio_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //DependencyObject rootGrid = VisualTreeHelper.GetChild(Window.Current.Content, 0);
                //var foregroundPlayer = (MediaElement)VisualTreeHelper.GetChild(rootGrid, 0) as MediaElement;
                //RootMediaElement = (MediaElement)VisualTreeHelper.GetChild(rootGrid, 1) as MediaElement;
                Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                RootMediaElement = (MediaElement)border.FindName("MediaPlayer");
                //RootMediaElement = (MediaPlayer)border.FindName("MediaPlayer");
                checkstate();
                GetPageDataInBackground();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowAudio_Loaded Method In ShowAudio.cs file", ex);
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
                Exceptions.SaveOrSendExceptions("Exception in controlplay Method In ShowAudio.cs file", ex);
            }
        }
        public async void controlImage()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation as StorageFolder;
                string imgpth = AppSettings.TileImage.Substring(0, AppSettings.TileImage.LastIndexOf('.'));
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"Images\TileImages\150-150\" + imgpth + ".png", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                AudioTrack Track = new AudioTrack();
                string filename = AppSettings.ChannelImage + ".png";
                Track.ThumbUri = new Uri("ms-appdata:///local/" + filename);
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
                MediaControl.AlbumArt = Track.ThumbUri;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }
        void MediaControl_PreviousTrackPressed(object sender, object e)
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
                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            RootMediaElement.Source = new Uri(arr[1].ToString(), UriKind.Absolute);
                        });
                        Constants.AppbarTitle = arr[0].ToString();
                    }
                    else
                    {
                        Constants._PlayList[Constants._PlayList.ElementAt(index - 1).Key] = "Play";
                        arr = Constants._PlayList.ElementAt(index - 1).Key.ToString().Split('$');
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
                Exceptions.SaveOrSendExceptions("Exception in MediaControl_PreviousTrackPressed Method In ShowAudio.cs file", ex);
            }
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
                Exceptions.SaveOrSendExceptions("Exception in checkstate Method In ShowAudio.cs file", ex);
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
                Exceptions.SaveOrSendExceptions("Exception in MediaControl_NextTrackPressed Method In ShowAudio.cs file", ex);
            }
        }
        void MediaControl_StopPressed(object sender, object e)
        {
            try
            {
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => RootMediaElement.Stop());
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

        private void lstvwaudiosongs_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstvwaudiosongs.SelectedIndex == -1)

                    return;
                if (check == true)
                {
                    check = false;
                    selecteditem = (sender as Selector).SelectedItem as ShowLinks;
                    Constants.selecteditem = selecteditem;
                    Constants.LinkID = selecteditem.LinkID;
                    AppSettings.LinkType = LinkType.Audio.ToString();
                    Page p = (Page)comm.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                    p.GetType().GetTypeInfo().GetDeclaredMethod("changetext").Invoke(p, new object[] { "Audio" });
                    lstvwaudiosongs.SelectedIndex = -1;
                    return;
                }
                AppSettings.LiricsLink = (lstvwaudiosongs.SelectedItem as ShowLinks).Title;
                AppSettings.LinkType = (lstvwaudiosongs.SelectedItem as ShowLinks).LinkType;
                insights.Event(AppSettings.LinkType+AppSettings.LiricsLink+"Play");
                Constants.AppbarTitle = (lstvwaudiosongs.SelectedItem as ShowLinks).Title;
                AppSettings.ChannelTitle = (lstvwaudiosongs.SelectedItem as ShowLinks).Title;
                Constants.Link = (lstvwaudiosongs.SelectedItem as ShowLinks).Title + "$" + (lstvwaudiosongs.SelectedItem as ShowLinks).LinkUrl;
                History history = new History();
                history.SaveAudioHistory((lstvwaudiosongs.SelectedItem as ShowLinks).ShowID.ToString(), (lstvwaudiosongs.SelectedItem as ShowLinks).Title, (lstvwaudiosongs.SelectedItem as ShowLinks).LinkUrl, (lstvwaudiosongs.SelectedItem as ShowLinks).Rating.ToString());
                songblock.Text = Constants.AppbarTitle;
                LoadLyrics();
                controlplay();
                controlImage();
                lstvwaudiosongs.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstvwaudiosongs_SelectionChanged_1 Method In ShowAudio.cs file", ex);
            }
        }

        private void LoadLyrics()
        {
            try
            {
                if (Constants.CloseLyricspopup.IsOpen == true)
                    Constants.CloseLyricspopup.IsOpen = false;
                Constants._PlayList.Clear();
                foreach (ShowLinks s in lstvwaudiosongs.Items.Cast<ShowLinks>())
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
                // Page p = (Page)PageNavigationManager.GetParentOfTypePage(this, typeof(Page));

                var p = new LyricsPopUp();
                Constants.CloseLyricspopup = new Popup();
                Constants.CloseLyricspopup.Child = p;
                Constants.CloseLyricspopup.IsOpen = true;
                RootMediaElement.Source = new Uri(Constants.Link.Split('$')[1], UriKind.Absolute);
                Constants._PlayList[Constants.Link] = "Play";
                TextBlock song = (TextBlock)p.GetType().GetTypeInfo().GetDeclaredField("songblock").GetValue(p);
                song.Text = Constants.Link.Split('$')[0];
                //RootMediaElement.Play();
                p.GetType().GetTypeInfo().GetDeclaredMethod("changeimage").Invoke(p, null);
                p.GetType().GetTypeInfo().GetDeclaredMethod("LyricsPopup").Invoke(p, new object[] { true });

                //Page p1 = (Page)PageNavigationManager.GetParentOfTypePage(this, typeof(Page));                
                //Page p1 = (Page)PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                //popgallimages.Child = (UIElement)p1.GetType().GetTypeInfo().GetDeclaredMethod("Popup").Invoke(p1, null);
                //popgallimages.IsOpen = true;
                //popgallimages.Height = 1000;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadLyrics Method In ShowAudio.cs file", ex);
            }
        }

        private void tblkAudioSongs_RightTapped_1(object sender, RightTappedRoutedEventArgs e)
        {
            try
            {
                check = true;
                Constants.appbarvisible = true;
                AppSettings.LinkType = LinkType.Audio.ToString();
                Page p = (Page)comm.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("appbar").Invoke(p, new object[] { true });
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in tblkAudioSongs_RightTapped_1 Method In ShowAudio.cs file", ex);
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
                Constants.AppbarTitle = (itemlistview.SelectedItem as ShowLinks).Title;
                Constants.Link = (itemlistview.SelectedItem as ShowLinks).Title + "$" + (itemlistview.SelectedItem as ShowLinks).LinkUrl;
                History history = new History();
                history.SaveAudioHistory((itemlistview.SelectedItem as ShowLinks).ShowID.ToString(), (itemlistview.SelectedItem as ShowLinks).Title, (itemlistview.SelectedItem as ShowLinks).LinkUrl, (itemlistview.SelectedItem as ShowLinks).Rating.ToString());
                songblock.Text = Constants.AppbarTitle;
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
                Constants._PlayList.Clear();
                foreach (ShowLinks s in itemlistview.Items.Cast<ShowLinks>())
                {
                    try
                    {
                        Constants._PlayList.Add(s.Title + "$" + s.LinkUrl, "Stop");
                    }
                    catch (Exception ex)
                    {
                        Exceptions.SaveOrSendExceptions("Exception in tblkAudioSongs_RightTapped_1 Method In ShowAudio.cs file", ex);
                    }
                }

                Page p = (Page)comm.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                RootMediaElement = (MediaElement)border.FindName("MediaPlayer");
                RootMediaElement.Source = new Uri(Constants.Link.Split('$')[1], UriKind.Absolute);               
                Constants._PlayList[Constants.Link] = "Play";
                //TextBlock song = (TextBlock)p.GetType().GetTypeInfo().GetDeclaredField("songblock").GetValue(p);
                //song.Text = Constants.Link.Split('$')[0];
                //RootMediaElement.Play();
                // p.GetType().GetTypeInfo().GetDeclaredMethod("changeimage").Invoke(p, null);
                // Page p1 = (Page)PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("SnapLyricsPopup").Invoke(p, new object[] { true });
                //Page p1 = (Page)PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                //popgallimages.Child = (UIElement)p1.GetType().GetTypeInfo().GetDeclaredMethod("Popup").Invoke(p1, null);

                //popgallimages.IsOpen = true;
                //popgallimages.Height = 1000;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadLyrics Method In ShowAudio.cs file", ex);
            }
        }
        public void GetPageDataInBackground()
        {
            try
            {
                int count = 1;
                BackgroundHelper bwHelper = new BackgroundHelper();
                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                a.Result = OnlineShow.GetShowAudioByType(AppSettings.ShowID);
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                links = (List<ShowLinks>)a.Result;
                                                foreach (ShowLinks link in links)
                                                {
                                                    link.RatingBitmapImage = ImageHelper.LoadRatingImage(link.Rating.ToString());
                                                    link.SongNO = count.ToString() + ".";
                                                    count++;
                                                }
                                                if (links.Count > 0)
                                                {
                                                    lstvwaudiosongs.ItemsSource = links;
                                                    itemlistview.ItemsSource = links;
                                                    progressbar.IsActive = false;
                                                    progressbarsnap.IsActive = false;
                                                    txtmsg.Visibility = Visibility.Collapsed;
                                                    txtmsg1.Visibility = Visibility.Collapsed;
                                                }
                                                else
                                                {
                                                    lstvwaudiosongs.ItemsSource = null;
                                                    itemlistview.ItemsSource = null;
                                                    txtmsg.Visibility = Visibility.Visible;
                                                    progressbar.IsActive = false;
                                                    progressbarsnap.IsActive = false;
                                                    txtmsg1.Visibility = Visibility.Visible;
                                                }
                                                // lstvwsongs.ItemsSource = (List<ShowLinks>)a.Result;
                                            }
                                          );
                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In ShowVideos.cs file", ex);
            }
        }
    }
}