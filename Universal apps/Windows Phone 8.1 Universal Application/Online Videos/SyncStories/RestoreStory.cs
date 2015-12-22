using System;
using System.Net;
using System.Windows;
using Microsoft.Live;
using System.Windows.Input;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Linq;
using System.Threading;
//using OnlineVideos.Common;
using Common.Library;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace SyncStories
{   
    public class RestoreStory
    {      
        string skyDriveFolderID = string.Empty;
        string skyDrivechildFolderID = string.Empty;
        private LiveConnectSession session;
        private LiveConnectSession newsession;
        private static string _skyDriveFolderId = string.Empty;
        public bool LoginStatus()
        {
            if ((LiveConnectSession)SyncButton.session == null)
                return false;
            else return true;
        }
        public async Task RestoreFolder(string skydrivefoldername)
        {
            try
            {
                session = (LiveConnectSession)SyncButton.session;
                await GetFolderId(skydrivefoldername);
                //GetSubFolderId(skyDriveFolderID);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in RestoreFolder Method In RestoreStory.cs file", ex);
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadStory;
            }

        }
        public async Task<string> GetFolderId(string skyDriveFolderName)
        {
            string skyDriveFolderid = string.Empty;
            try
            {
                newsession = session;
                LiveConnectClient client = new LiveConnectClient(newsession);
                LiveOperationResult liveOpResult = await client.GetAsync("me/skydrive/files?filter=folders");
                if (liveOpResult.Result != null)
                {
                    dynamic appResult = liveOpResult.Result;
                    List<object> folderData = appResult.data;
                    foreach (dynamic folder in folderData)
                    {
                        string name = folder.name;
                        if (name == skyDriveFolderName)
                        {
                            _skyDriveFolderId = folder.id;

                        }
                    }
                    await GetSubFolderId(_skyDriveFolderId);
                }
                skyDriveFolderid = _skyDriveFolderId;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetFolderId Method In RestoreStory.cs file", ex);
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadStory;
            }
            return skyDriveFolderid;
        }
        public async Task GetSubFolderId(string skyDriveFolderID)
        {
            try
            {
                LiveConnectClient client = new LiveConnectClient(newsession);
                LiveOperationResult liveOpResult =await client.GetAsync(skyDriveFolderID + "/files");
                dynamic appResult = liveOpResult.Result;
                List<object> folderData = appResult.data;
                foreach (dynamic folder in folderData)
                {
                    string name = folder.name;
                    skyDrivechildFolderID = folder.id;
                    LiveOperationResult liveOpResult1 = Task.Run(async () => await client.GetAsync(skyDrivechildFolderID + "/files")).Result;
                    dynamic appResult1 = liveOpResult1.Result;
                    List<object> folderData1 = appResult1.data;
                    foreach (dynamic file in folderData1)
                    {
                        string filenames = file.name;
                        if (!filenames.Contains("-8"))
                         await   Downloadmp3(file.id.ToString() + "/content", filenames, name);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetSubFolderId Method In RestoreStory.cs file", ex);
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadStory;
            }
        }
        private async Task Downloadmp3(string fileid, string filename, string foldername)
        {
            try
            {
                LiveConnectClient client = new LiveConnectClient(newsession);
                Stream stream = default(Stream);
                LiveDownloadOperationResult reslut = await client.BackgroundDownloadAsync(fileid);

                if (reslut != null)
                {
                    stream = (Stream)reslut.Stream;
                    StorageFolder f = ApplicationData.Current.LocalFolder;
                    var story = Task.Run(async () => await f.CreateFolderAsync("StoryRecordings", CreationCollisionOption.OpenIfExists)).Result;
                    var story1 = Task.Run(async () => await story.CreateFolderAsync(foldername, CreationCollisionOption.OpenIfExists)).Result;
                    StorageFile file = Task.Run(async () => await story1.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists)).Result;
                    using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        using (IOutputStream outputStream4 = fileStream.GetOutputStreamAt(0))
                        {
                            using (DataWriter dataWriter = new DataWriter(outputStream4))
                            {
                                MemoryStream sty = new MemoryStream();
                                await stream.CopyToAsync(sty);
                                dataWriter.WriteBytes(sty.ToArray());
                                await dataWriter.StoreAsync();
                                await outputStream4.FlushAsync();
                                dataWriter.DetachStream();
                            }
                            outputStream4.Dispose();
                        }
                        fileStream.Dispose();
                    }

                }
                AppSettings.DownloadStoryCompleted = true;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Downloadmp3 Method In RestoreStory.cs file", ex);
            }
            finally
            {
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadStory;
            }
        }             
    }
}