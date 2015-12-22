using Common.Library;
using Microsoft.Live;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;

namespace SyncStories
{
    public class ReStoreFavourites
    {
        string skyDriveFolderID = string.Empty;
        string skyDrivechildFolderID = string.Empty;
        private LiveConnectSession session;
        private LiveConnectSession newsession;
        private static string _folderName = string.Empty;
        private static string _skyDriveFolderId = string.Empty;
        AutoResetEvent auto = new AutoResetEvent(false); 
        public bool LoginStatus()
        {
            if ((LiveConnectSession)SyncButton.session == null)
                return false;
            else return true;
        }
        public async Task RestorefavFolder(string skydrivefoldername)
        {
            try
            {
                session = (LiveConnectSession)SyncButton.session;
                await GetFolderId(skydrivefoldername);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in RestoreFolder Method In RestoreFavourites.cs file", ex);
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadFavourites;
            }
        }

        public async Task<string> GetFolderId(string skyDriveFolderName)
        {
            string skyDriveFolderid = string.Empty;
            try
            {
                newsession = session;
                LiveConnectClient client = new LiveConnectClient(newsession);
                _folderName = skyDriveFolderName;
                LiveOperationResult result = await client.GetAsync("me/skydrive/files?filter=folders");
                if (result.Result != null)
                {
                    dynamic appResult = result.Result;
                    List<object> folderData = appResult.data;
                    foreach (dynamic folder in folderData)
                    {
                        string name = folder.name;
                        if (name == _folderName)
                        {
                            _skyDriveFolderId = folder.id;
                        }
                    }
                    await GetFavSubFolderId(_skyDriveFolderId);
                }
                skyDriveFolderid = _skyDriveFolderId;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetFolderId Method In RestoreFavourites.cs file", ex);
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadFavourites;
            }
            return skyDriveFolderid;
        }

        public async Task GetFavSubFolderId(string skyDriveFolderID)
        {
            try
            {
                LiveConnectClient client = new LiveConnectClient(newsession);
                LiveOperationResult result = await client.GetAsync(skyDriveFolderID + "/files");
                if (result != null)
                {
                    dynamic appResult1 = result.Result;
                    List<object> folderData1 = appResult1.data;
                    foreach (dynamic file in folderData1)
                    {
                        string filenames = file.name;
                        await Downloadxml(file.id+ "/content", filenames);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetFavSubFolderId Method In RestoreFavourites.cs file", ex);
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadFavourites;
            }
        }

        public async Task Downloadxml(string fileid, string filename)
        {
            try
            {
                LiveConnectClient client = new LiveConnectClient(this.session);
                //Stream stream = default(Stream);
                StorageFolder f = ApplicationData.Current.LocalFolder;
                var story = await f.CreateFolderAsync("Favourites", CreationCollisionOption.OpenIfExists);
                StorageFile storageFile = await story.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
                LiveDownloadOperationResult result = await client.BackgroundDownloadAsync(fileid, storageFile);
                //var result = await opr.StartAsync();
                
                //LiveDownloadOperationResult result = await client.BackgroundDownloadAsync(fileid);
                //var result=await Dresult.GetRandomAccessStreamAsync();

                //if (result != null)
                //{
                //    stream = (Stream)result.Stream;
                //    //StorageFolder f = ApplicationData.Current.LocalFolder;
                //    //var story = await f.CreateFolderAsync("Favourites", CreationCollisionOption.OpenIfExists);
                //    //StorageFile storageFile = await story.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
                //    using (IRandomAccessStream fileStream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
                //    {
                //        using (IOutputStream outputStream4 = fileStream.GetOutputStreamAt(0))
                //        {
                //            using (DataWriter dataWriter = new DataWriter(outputStream4))
                //            {
                //                MemoryStream sty = new MemoryStream();
                //                await stream.CopyToAsync(sty);
                //                dataWriter.WriteBytes(sty.ToArray());
                //                await dataWriter.StoreAsync();
                //                await outputStream4.FlushAsync();
                //                dataWriter.DetachStream();
                //            }
                //            outputStream4.Dispose();
                //        }
                //        fileStream.Dispose();
                //    }
                //}
                AppSettings.DownloadFavCompleted = true;
                if (await Storage.FavouriteFileExists("Favourites.xml"))
                {
                    StorageFolder f1 = ApplicationData.Current.LocalFolder;
                    var story1 = await f1.CreateFolderAsync("Favourites", CreationCollisionOption.OpenIfExists);

                    StorageFile storageFile1 = await story1.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
                    IRandomAccessStream fileStream = await storageFile1.OpenAsync(FileAccessMode.Read);
                    try
                    {
                        XDocument xdoc = new XDocument();
                        xdoc = XDocument.Load(fileStream.AsStream());
                        var xquery = from i in xdoc.Descendants("Favourites") select i;
                        foreach (var item in xquery)
                        {
                            foreach (var element in item.Elements("Show"))
                            {
                                foreach (var ss in element.Descendants())
                                {
                                    if (ss.Name.ToString() == "Movies")
                                    {
                                        int showid = Convert.ToInt32(element.Attribute("id").Value);
                                        await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                                            {
                                                try
                                                {
                                                    ShowList showlist = Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync().Result;
                                                    showlist.IsFavourite = Convert.ToBoolean(Convert.ToInt32(ss.Value));
                                                    Task.Run(async () => await Constants.connection.UpdateAsync(showlist));
                                                }
                                                finally
                                                {
                                                    auto.Set();
                                                }
                                            });
                                        auto.WaitOne();
                                    }
                                    else
                                    {
                                        int showid = Convert.ToInt32(element.Attribute("id").Value);
                                        int linkid = Convert.ToInt32(ss.Attribute("no").Value);
                                        string linktype = ss.Name.ToString();
                                        await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                                        {
                                            try
                                            {
                                                ShowLinks links = Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkID == linkid && i.LinkType == linktype).FirstOrDefaultAsync().Result;
                                                links.IsFavourite = Convert.ToBoolean(Convert.ToInt32(ss.Value));
                                                Task.Run(async () => await Constants.connection.UpdateAsync(links));
                                            }
                                            finally
                                            {
                                                auto.Set();
                                            }
                                        });
                                        auto.WaitOne();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        fileStream.Dispose();
                    }
                    finally
                    {
                        fileStream.Dispose();
                        AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadFavourites;
                    }
                }
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadFavourites;
                Exceptions.SaveOrSendExceptions("Exception in Downloadxml Method In RestoreFavourites.cs file", ex);
            }
            //LiveOperationResult reslut=await client.BackgroundDownloadAsync(fileid, new Uri("/shared/transfers/"+filename, UriKind.RelativeOrAbsolute));
        }
    }
}
