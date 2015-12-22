using Common.Library;
using OnlineMovies.Views;
using OnlineVideos.Entities;
using OnlineVideos.UI;
using OnlineVideos.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShowFavorites : Page
    {
        ShowLinks desToSelect = new ShowLinks();
        private SolidColorBrush adcontrolborder = new SolidColorBrush(Colors.Transparent);        
        public ShowFavorites()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void imgTitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (ResourceHelper.AppName != Apps.Indian_Cinema_Pro.ToString() && ResourceHelper.AppName != Apps.Kids_TV_Pro.ToString() && ResourceHelper.AppName != Apps.Story_Time_Pro.ToString())
            {
                if (ShellTileHelper_New.IsPinned("99988") == true)
                {
                    Pin_to_start.Label = "unpin from start";
                    Pin_to_start.Icon = new SymbolIcon(Symbol.UnPin);
                }
                else
                {
                    Pin_to_start.Label = "pin to start";
                    Pin_to_start.Icon = new SymbolIcon(Symbol.Pin);
                }
                LoadAds();
            }
        }
        private void LoadAds()
        {
            try
            {
                LoadAdds.LoadAdControl_New(LayoutRoot, adstaSongs, 1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In ShowFavorites file", ex);
                string excepmess = "Exception in LoadAds Method In ShowFavorites file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }

        private async void download_all_Click(object sender, RoutedEventArgs e)
        {
            Constants.YoutubePlayList = new Dictionary<string, string>();
            var Itemcollection = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.IsFavourite == true && i.LinkType == "Songs").ToListAsync()).Result;

            foreach (var item in Itemcollection)
            {
                if (NetworkHelper.IsNetworkAvailableForDownloads())
                {
                    //if (!Constants.YoutubePlayList.ContainsKey(item.LinkUrl))
                    //    Constants.YoutubePlayList.Add(item.LinkUrl, item.Title);
                    if (item.DownloadStatus == null || item.DownloadStatus == "0")
                    {
                        string Url = item.LinkUrl;
                        var finalurl = await MyToolkit.Multimedia.YouTube.GetVideoUriAsync(Url, MyToolkit.Multimedia.YouTubeQuality.Quality480P);
                        Url = finalurl.Uri.AbsoluteUri.ToString();
                        Uri download_uri = new Uri(Url);
                        download_uri = new Uri(download_uri.AbsoluteUri);
                        Constants.YoutubePlayList.Add(Url, item.Title);
                        var httpclient = new HttpClient();
                        var response = await httpclient.GetAsync(download_uri);
                        if (response.IsSuccessStatusCode)
                        {
                            var file = await response.Content.ReadAsInputStreamAsync();
                            Stream str = file.AsStreamForRead();
                            byte[] mybyte = ReadToEnd(str);
                            StorageFile destinationFile = await KnownFolders.VideosLibrary.CreateFileAsync(item.Title + ".mp4", CreationCollisionOption.ReplaceExisting);

                            Windows.Storage.Streams.IRandomAccessStream stream = await destinationFile.OpenAsync(FileAccessMode.ReadWrite);
                            IOutputStream output = stream.GetOutputStreamAt(0);
                            DataWriter writer = new DataWriter(output);
                            writer.WriteBytes(mybyte);
                            await writer.StoreAsync();
                            await output.FlushAsync();

                            int linkid = item.LinkID;
                            desToSelect = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkID == linkid).FirstOrDefaultAsync()).Result;
                            desToSelect.DownloadStatus = "1";
                            await Task.Run(async () => await Constants.connection.UpdateAsync(desToSelect));
                        }
                    }
                }
                else
                {
                    //if (item.DownloadStatus == "1")
                    //{
                    //    string Url = item.Title + ".mp4";
                    //    Constants.YoutubePlayList.Add(Url, item.Title);
                    //}
                }
            }
            favvideos.GetPageDataInBackground();
            //int linkid = Convert.ToInt32(AppSettings.LinkID);
            //desToSelect = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkID == linkid).FirstOrDefaultAsync()).Result;
            //if(Constants.YoutubePlayList.Count>0)
            //{
            //if(desToSelect.DownloadStatus=="0")
            //{

            //}
            //var httpclient = new HttpClient();
            //}
        }

        private void play_all_Click(object sender, RoutedEventArgs e)
        {
            Constants.YoutubePlayList = new Dictionary<string, string>();
            var Itemcollection = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.IsFavourite == true && i.LinkType == "Songs").ToListAsync()).Result;

            foreach (var item in Itemcollection)
            {
                if (NetworkHelper.IsNetworkAvailableForDownloads())
                {
                    if (!Constants.YoutubePlayList.ContainsKey(item.LinkUrl))
                        Constants.YoutubePlayList.Add(item.LinkUrl, item.Title);
                }
                else
                {
                    if (item.DownloadStatus == "1")
                    {
                        string Url = item.Title + ".mp4";
                        Constants.YoutubePlayList.Add(Url, item.Title);
                    }
                }
            }
            if (Constants.YoutubePlayList.Count > 0)
            {
                if (ResourceHelper.AppName == Apps.Indian_Cinema_Pro.ToString() || ResourceHelper.AppName == Apps.Kids_TV_Pro.ToString() || ResourceHelper.AppName == Apps.Story_Time_Pro.ToString())
                    AppSettings.startplayingforpro = true;
                else
                    AppSettings.startplaying = true;
                AppSettings.LinkUrl = Constants.YoutubePlayList.FirstOrDefault().Key;
                AppSettings.LinkTitle = Constants.YoutubePlayList.FirstOrDefault().Value;
                Constants.YoutubePlayList.Remove(Constants.YoutubePlayList.FirstOrDefault().Key);
                AppSettings.PlayVideoTitle = Constants.YoutubePlayList.FirstOrDefault().Value;
                if (NetworkHelper.IsNetworkAvailableForDownloads())
                {
                    Frame.Navigate(typeof(Youtube), AppSettings.LinkUrl);
                }
                else
                    Frame.Navigate(typeof(OfflineVideoPlayer), AppSettings.LinkUrl);
            }
        }

        private void Pin_to_start_Click(object sender, RoutedEventArgs e)
        {
            if (ShellTileHelper_New.IsPinned("99988") == true)
            {
                ShellTileHelper_New.UnPin("99988");
                Pin_to_start.Label = "pin to start";
                Pin_to_start.Icon = new SymbolIcon(Symbol.Pin);
            }
            else
            {
                SecondaryTile initialData = new SecondaryTile();
                initialData = new SecondaryTile(
                            "99988",
                            "Favorites",
                            "NoArguments",
                            new Uri("ms-appx:///Images/Hub/Logo.png"),
                            TileSize.Square150x150);
                initialData.VisualElements.ShowNameOnSquare150x150Logo = true;
                ShellTileHelper_New.Pin(initialData);
                Pin_to_start.Label = "unpin from start";
                Pin_to_start.Icon = new SymbolIcon(Symbol.UnPin);
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
                byte[] readBuffer = new byte[4096];
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
    }
}
