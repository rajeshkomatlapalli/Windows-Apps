using Common.Library;
using Microsoft.Live;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SyncStories
{
    public class RestoreStory
    {
        string skyDriveFolderID = string.Empty;
        string skyDrivechildFolderID = string.Empty;
        private LiveConnectSession session;
        private LiveConnectSession newsession;
        public void RestoreFolder(string skydrivefoldername)
        {
            try
            {
                session = (LiveConnectSession)SyncButton.session;
                skyDriveFolderID = GetFolderId(skydrivefoldername);
                GetSubFolderId(skyDriveFolderID);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in RestoreFolder Method In RestoreStory.cs file", ex);
            }

        }
        public void RestorefavFolder(string skydrivefoldername)
        {
            try
            {
                session = (LiveConnectSession)SyncButton.session;
                skyDriveFolderID = GetFolderId(skydrivefoldername);
                GetFavSubFolderId(skyDriveFolderID);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in RestoreFolder Method In RestoreStory.cs file", ex);
            }

        }
        public string GetFolderId(string skyDriveFolderName)
        {
            string skyDriveFolderid = string.Empty;
            try
            {
                newsession = session;
                LiveConnectClient client = new LiveConnectClient(newsession);
                LiveOperationResult liveOpResult = Task.Run(async () => await client.GetAsync("me/skydrive/files?filter=folders")).Result;
                dynamic appResult = liveOpResult.Result;
                List<object> folderData = appResult.data;
                foreach (dynamic folder in folderData)
                {
                    string name = folder.name;
                    if (name == skyDriveFolderName)
                    {
                        skyDriveFolderid = folder.id;

                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetFolderId Method In RestoreStory.cs file", ex);
            }
            return skyDriveFolderid;
        }
        public void GetSubFolderId(string skyDriveFolderID)
        {
            try
            {

                LiveConnectClient client = new LiveConnectClient(newsession);
                LiveOperationResult liveOpResult = Task.Run(async () => await client.GetAsync(skyDriveFolderID + "/files")).Result;
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
                        if (filenames.Contains("-8"))
                            Downloadmp3(file.id.ToString() + "/content", filenames, name);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetSubFolderId Method In RestoreStory.cs file", ex);
            }
        }
        public void GetFavSubFolderId(string skyDriveFolderID)
        {
            try
            {
                LiveConnectClient client = new LiveConnectClient(newsession);

                LiveOperationResult liveOpResult1 = Task.Run(async () => await client.GetAsync(skyDriveFolderID + "/files")).Result;
                dynamic appResult1 = liveOpResult1.Result;
                List<object> folderData1 = appResult1.data;
                foreach (dynamic file in folderData1)
                {
                    string filenames = file.name;
                    Downloadxml(file.id.ToString() + "/content", filenames);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetFavSubFolderId Method In RestoreStory.cs file", ex);
            }
        }
        private void Downloadmp3(string fileid, string filename, string foldername)
        {
            LiveConnectClient client = new LiveConnectClient(newsession);
            StorageFolder f = ApplicationData.Current.LocalFolder;
            var story = Task.Run(async () => await f.CreateFolderAsync("StoryRecordings", CreationCollisionOption.OpenIfExists)).Result;
            var story1 = Task.Run(async () => await story.CreateFolderAsync(foldername, CreationCollisionOption.OpenIfExists)).Result;
            StorageFile file = Task.Run(async () => await story1.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists)).Result;

            client.BackgroundDownloadAsync(fileid, file);
            AppSettings.DownloadStoryCompleted = true;
        }
        private void Downloadxml(string fileid, string filename)
        {
            LiveConnectClient client = new LiveConnectClient(newsession);
            StorageFolder f = ApplicationData.Current.LocalFolder;
            var story = Task.Run(async () => await f.CreateFolderAsync(AppSettings.ProjectName, CreationCollisionOption.OpenIfExists)).Result;

            StorageFile file = Task.Run(async () => await story.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists)).Result;

            client.BackgroundDownloadAsync(fileid, file);
            AppSettings.DownloadFavCompleted = true;
        }
    }
}
