using Common.Library;
using Common.Utilities;
using OnlineMovies.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OfflineVideoPlayer : Page
    {
        string path;
        public OfflineVideoPlayer()
        {
            this.InitializeComponent();
            Loaded += OfflineVideoPlayer_Loaded;
        }

       void OfflineVideoPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                play(path);                
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OfflineVideo Platyer", ex);
            }
        }

        public async void play(string path)
       {
           try
           {
               StorageFile SourceFile = await KnownFolders.VideosLibrary.GetFileAsync(path);
               IRandomAccessStream stream = await SourceFile.OpenAsync(FileAccessMode.Read);
               player.SetSource(stream, SourceFile.ContentType);
               //player.Source = new Uri(SourceFile.Path);
               //player.Play();
           }
           catch (Exception ex)
           {
               Exceptions.SaveOrSendExceptions("Exception in OfflineVideo Platyer", ex);
           }
       }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                path = (string)e.Parameter;               
            }
            catch(Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OfflineVideo Platyer", ex);
            }
        }

        private void player_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (AppSettings.startplayingforpro == true && (ResourceHelper.AppName == Apps.Indian_Cinema_Pro.ToString() || ResourceHelper.AppName != Apps.Kids_TV_Pro.ToString() || ResourceHelper.AppName != Apps.Story_Time_Pro.ToString()))
            {
                if (Constants.YoutubePlayList.Count > 0)
                {
                    AppSettings.LinkUrl = Constants.YoutubePlayList.FirstOrDefault().Key;
                    AppSettings.LinkTitle = Constants.YoutubePlayList.FirstOrDefault().Value;
                    Constants.YoutubePlayList.Remove(Constants.YoutubePlayList.FirstOrDefault().Key);
                    AppSettings.PlayVideoTitle = Constants.YoutubePlayList.FirstOrDefault().Value;
                    play(AppSettings.LinkUrl);
                }
                else if (Constants.YoutubePlayList.Count == 0)
                {
                    AppSettings.startplayingforpro = false;
                    Frame.GoBack();
                }
            }
            else if (AppSettings.startplaying == true && ResourceHelper.AppName != Apps.Indian_Cinema_Pro.ToString() && ResourceHelper.AppName != Apps.Kids_TV_Pro.ToString() && ResourceHelper.AppName != Apps.Story_Time_Pro.ToString())
            {
                if (Constants.YoutubePlayList != null)
                {
                    if (Constants.YoutubePlayList.Count > 0)
                    {
                        PageHelper.RootApplicationFrame.Navigate(typeof(Advertisement));
                    }
                    else if (Constants.YoutubePlayList.Count == 0)
                    {
                        AppSettings.startplaying = false;
                        while (Frame.BackStack.Count() > 1)
                        {
                            if (Frame.BackStack.FirstOrDefault().SourcePageType.Equals("Youtube") || Frame.BackStack.FirstOrDefault().SourcePageType.Equals("Advertisement"))
                            {
                                Frame.BackStack.RemoveAt(-1);
                            }
                            else
                                break;
                        }
                        Frame.GoBack();
                    }
                }
                else
                {
                    Frame.GoBack();
                }
            }
            else
                Frame.GoBack();
        }
    }
}
