using Common.Library;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
  #if NOTANDROID
using Windows.Storage;
using Windows.Storage.Streams;
#endif
#if WINDOWS_APP
using System.Net.Http;
using Windows.Networking.BackgroundTransfer;
using Windows.UI.Xaml.Controls;
#endif
namespace OnlineVideos.Data
{
    public static class DownLoadVideos
    {
      #if WINDOWS_APP
        public static async void DownloadVideos(string YoutubeUri, string Title, string MovieTitle)
        {

            try
            {

                var httpClient = new HttpClient();
                var url = new Uri(YoutubeUri);
                var accessToken = "1234";
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url + "&title=" + Title);
                httpRequestMessage.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken));
                httpRequestMessage.Headers.Add("User-Agent", "My user-Agent");
                var response = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);
                int size = Convert.ToInt32(response.Content.Headers.ContentLength);
                byte[] b = new byte[size];
                b = Task.Run(async () => await response.Content.ReadAsByteArrayAsync()).Result;
                var imageFile = await KnownFolders.VideosLibrary.CreateFileAsync(MovieTitle + "_" + Title + ".mp4", CreationCollisionOption.ReplaceExisting);
                var fs = Task.Run(async () => await imageFile.OpenAsync(FileAccessMode.ReadWrite)).Result;
                var writer = fs.GetOutputStreamAt(0);
                var writer1 = new Windows.Storage.Streams.DataWriter(writer);
                writer1.WriteBytes(b);
                var fi = Task.Run(async () => await writer1.StoreAsync()).Result;
                await writer1.FlushAsync();
                writer1.DetachStream();
                writer1.Dispose();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in DownloadVideos Method In DownLoadVideos.cs file", ex);
            }
        }
        public static ObservableCollection<ShowLinks> sample()
        {
            ObservableCollection<ShowLinks> dv = new ObservableCollection<ShowLinks>();
            try
            {
                if (dv.Count == 0)
                {
                    dv = OnlineShow.GetShowLinksByType(AppSettings.ShowID, LinkType.Songs);
                }
                foreach (ShowLinks sl in dv)
                {
                    if (AppSettings.Title == sl.Title)
                    {
                        sl.Status = "statues" + AppSettings.DownloadStatues;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in sample Method In DownLoadVideos.cs file", ex);
            }
            return dv;
        }

        public static async void DownloadClick(string YoutubeUri, string Title, string MovieTitle)
        {
            try
            {
              
                var uri = new Uri(YoutubeUri + "&title=" + Title);
                var downloader = new BackgroundDownloader();
                var folder = Windows.Storage.KnownFolders.VideosLibrary;
                var AppFolder = await folder.CreateFolderAsync(AppSettings.ProjectName, CreationCollisionOption.OpenIfExists);
                StorageFile file = await AppFolder.CreateFileAsync(MovieTitle + "_" + Title + ".mp4", CreationCollisionOption.ReplaceExisting);
                DownloadOperation download = downloader.CreateDownload(uri, file);
                await StartDownloadAsync(download);
            }
            catch (Exception ex)
            {
                //Storage.DeleteFile("DownLoadVideo.xml");
                Exceptions.SaveOrSendExceptions("Exception in DownloadClick Method In DownLoadVideos.cs file", ex);
            }
        }


        private static void ProgressCallback(DownloadOperation obj)
        {
            try
            {
                double progress
                    = ((double)obj.Progress.BytesReceived / obj.Progress.TotalBytesToReceive);
                double df = progress * 100;
                if (100 == df)
                {
                    DataManager<ShowLinks> datamanager1 = new DataManager<ShowLinks>();
                    ShowLinks objdetail = new ShowLinks();
                    string uniqueID = obj.Guid.ToString();
                    objdetail = datamanager1.GetFromList(i => i.UniqueDownloadID == uniqueID);
                    objdetail.DownloadStatus = "Offline Available";
                    int Linkorder = objdetail.LinkOrder;
                    datamanager1.Savemovies(objdetail, "");
                    if (Constants.selecteditem != null && Constants.Instance!=null)
                    {
                        if (Constants.selecteditem.LinkType == LinkType.Songs.ToString())
                        {
                            Constants.Instance.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                 {
                     ShowLinks ss = new ShowLinks();
                     ss = Constants.selecteditemshowlinklist.ToList().Find(i => i.LinkOrder == Linkorder);
                     int index = Constants.selecteditemshowlinklist.IndexOf(ss);
                     ss.DownloadStatus = "Offline Available";
                     Constants.selecteditemshowlinklist.Remove(Constants.selecteditemshowlinklist.Where(i => i.LinkOrder == ss.LinkOrder).FirstOrDefault());
                     Constants.selecteditemshowlinklist.Insert(index, ss);
                          
                 }).AsTask().Wait();
                        }
                        else
                        {
                            Constants.Instance.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                            {
                                ShowLinks ss = new ShowLinks();
                                ss = Constants.selectedMovielinklist.ToList().Find(i => i.LinkOrder == Linkorder);
                                int index = Constants.selectedMovielinklist.IndexOf(ss);
                                ss.DownloadStatus = "Offline Available";
                                Constants.selectedMovielinklist.Remove(Constants.selectedMovielinklist.Where(i => i.LinkOrder == ss.LinkOrder).FirstOrDefault());
                                Constants.selectedMovielinklist.Insert(index, ss);
                        
                             }).AsTask().Wait();
                        }
                    }


                }
                if (progress >= 1.0)
                {


                }                             
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ProgressCallback Method In DownLoadVideos.cs file", ex);
            }            
        }
        private static async Task StartDownloadAsync(DownloadOperation downloadOperation)
        {
            try
            {

                DataManager<ShowLinks> datamanager1 = new DataManager<ShowLinks>();
                ShowLinks objdetail = new ShowLinks();
                int showid = int.Parse(AppSettings.ShowID);
                int Linkorder = Constants.selecteditem.LinkOrder;
                objdetail = datamanager1.GetFromList(i => i.ShowID == showid && i.LinkOrder == Linkorder);
                objdetail.DownloadStatus = "Downloading";
                objdetail.UniqueDownloadID = downloadOperation.Guid.ToString();
                datamanager1.Savemovies(objdetail, "");
                if (Constants.selecteditem != null && Constants.Instance != null)
                {
                    if (Constants.selecteditem.LinkType == LinkType.Songs.ToString())
                    {
                        Constants.Instance.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                 {
                     ShowLinks ss = new ShowLinks();
                     ss = Constants.selecteditemshowlinklist.ToList().Find(i => i.LinkOrder == Linkorder);
                     int index = Constants.selecteditemshowlinklist.IndexOf(ss);
                     ss.DownloadStatus = "Downloading";
                     Constants.selecteditemshowlinklist.Remove(Constants.selecteditemshowlinklist.Where(i => i.LinkOrder==ss.LinkOrder).FirstOrDefault());
                     Constants.selecteditemshowlinklist.Insert(index, ss);
                 }).AsTask().Wait();

                    }
                    else
                    {
                        Constants.Instance.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    ShowLinks ss = new ShowLinks();
                    ss = Constants.selectedMovielinklist.ToList().Find(i => i.LinkOrder == Linkorder);
                    int index = Constants.selectedMovielinklist.IndexOf(ss);
                    ss.DownloadStatus = "Downloading";
                    Constants.selectedMovielinklist.Remove(Constants.selectedMovielinklist.Where(i => i.LinkOrder == ss.LinkOrder).FirstOrDefault());
                    Constants.selectedMovielinklist.Insert(index, ss);
                  
                }).AsTask().Wait();

                    }
                }
                var progress = new Progress<DownloadOperation>(ProgressCallback);
                await downloadOperation.StartAsync().AsTask(progress);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in StartDownloadAsync Method In DownLoadVideos.cs file", ex);
            }
        }
        public static async Task LoadActiveDownloadsAsync()
        {
            try
            {

                IReadOnlyList<DownloadOperation> downloads = null;
                downloads = await BackgroundDownloader.GetCurrentDownloadsAsync();
                if (downloads.Count > 0)
                {

                    await ResumeDownloadAsync(downloads.First());
                }
                if (downloads.Count == 0)
                {
                    if(AppSettings.ProjectName=="Web Media")
                    DownloadManager.DeleteAll();
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadActiveDownloadsAsync Method In DownLoadVideos.cs file", ex);
            }
        }
        private static async Task ResumeDownloadAsync(DownloadOperation downloadOperation)
        {
            try
            {
                var progress = new Progress<DownloadOperation>(ProgressCallback);
                await downloadOperation.AttachAsync().AsTask(progress);
            }
            catch (Exception ex)
            {
            }
        }

#endif
    }

}












