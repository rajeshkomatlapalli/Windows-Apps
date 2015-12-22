using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.IO;
using Microsoft.Live;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
//using OnlineVideos.Common;
using Common.Library;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Storage.FileProperties;
namespace SyncStories
{
    public class UploadStory
    {
        public List<object> list = default(List<object>);
        public StorageFile StorageFile;
        public bool checktime = false;
        string skyDriveFolderID = string.Empty;
        string FolderID = string.Empty;
        string mainFolderID = string.Empty;
        string childFolderID = string.Empty;
        private string folderid = string.Empty;
        public static LiveAuthClient authClient;
        public static LiveConnectSession session = default(LiveConnectSession);
        public bool LoginStatus()
        {
            if ((LiveConnectSession)SyncButton.session == null)
                return false;
            else return true;
        }
        public async Task CreateFolder(string foldername)
        {
            try
            {
                session = (LiveConnectSession)SyncButton.session;
                LiveConnectClient client = new LiveConnectClient(session);
                LiveOperationResult liveOpResult =await client.GetAsync("me/skydrive/files?filter=folders");
                dynamic appResult = liveOpResult.Result;
                List<object> folderData = appResult.data;
                foreach (dynamic folder in folderData)
                {
                    string name = folder.name;
                    if (name == foldername)
                    {
                        skyDriveFolderID = folder.id;
                    }
                }

                if (string.IsNullOrEmpty(skyDriveFolderID))
                {
                    var skyDriveFolderData = new Dictionary<string, object>();
                    LiveConnectClient parentclient = new LiveConnectClient(session);
                    skyDriveFolderData.Add("name", foldername);
                    LiveOperationResult operationResult =await parentclient.PostAsync("me/skydrive", skyDriveFolderData);
                    dynamic Results = operationResult.Result;

                    skyDriveFolderID = Results.id;

                    List<object> list = RetrieveSubfolders();
                    foreach (Dictionary<string, object> item in list)
                    {
                        CreateSubFolder(skyDriveFolderID, item["name"].ToString());
                    }
                }
                else
                {
                    List<object> list = RetrieveSubfolders();
                    foreach (Dictionary<string, object> item in list)
                    {
                        string fid = await GetFolderId(skyDriveFolderID + "/files", item["name"].ToString());
                        if (string.IsNullOrEmpty(fid))
                        {

                            CreateSubFolder(skyDriveFolderID, item["name"].ToString());

                        }
                        else
                        {
                            updatefile(fid, item["name"].ToString());
                        }
                    }
                }
                if (checktime == false)
                    AppSettings.StoryUploadedDate = System.DateTime.Now;
            }
            catch (Exception ex)
            {
                checktime = true;
                Exceptions.SaveOrSendExceptions("Exception in CreateFolder Method In UploadStory.cs file", ex);
            }
            finally
            {
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadFavourites;
            }
        }

        public void updatefile(string folderid, string foldername)
        {
            try
            {
                Dictionary<string, IRandomAccessStream> stream = GetFiles(foldername);
                foreach (KeyValuePair<string, IRandomAccessStream> strm in stream)
                {                    
                    //string a = Convert.ToString(strm);
                    LiveConnectClient fileclient = new LiveConnectClient(session);
                    string conv = strm.Key.ToString();
                    IInputStream strem = strm.Value.GetInputStreamAt(0);
                    fileclient.BackgroundUploadAsync(folderid, conv, strem, OverwriteOption.Overwrite);
                    //fileclient.CreateBackgroundUploadAsync(folderid, conv, strem, OverwriteOption.Overwrite); //Wp SL 8.1
                }
            }
            catch (Exception ex)
            {
                checktime = true;
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadFavourites;
                Exceptions.SaveOrSendExceptions("Exception in updatefile Method In UploadStory.cs file", ex);
            }
        }
        public async Task<string> GetFolderId(string folderid, string foldername)
        {
            try
            {
                LiveConnectClient copyclient = new LiveConnectClient(session);
                LiveOperationResult result = default(LiveOperationResult);

                result = await copyclient.GetAsync(folderid);

                dynamic Result = result.Result;
                List<object> folder = Result.data;
                foreach (dynamic folders in folder)
                {
                    string name = folders.name;
                    if (name == foldername)
                    {
                        FolderID = folders.id;
                    }
                }
                return FolderID;
            }
            catch (Exception ex)
            {
                checktime = true;
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadFavourites;
                return FolderID;
             
              //  Exceptions.SaveOrSendExceptions("Exception in GetFolderId Method In UploadStory.cs file", ex);
            }
        }

        public async void CreateSubFolder(string folderid, string foldername)
        {
            try
            {
                Dictionary<string, object> folder = new Dictionary<string, object>();
                LiveConnectClient childclient = new LiveConnectClient(session);
                folder.Add("name", foldername);
                LiveOperationResult operationResult =await childclient.PostAsync(folderid, folder);
                dynamic Result = operationResult.Result;
                childFolderID = Result.id;

                Dictionary<string, IRandomAccessStream> stream = GetFiles(foldername);
                foreach (KeyValuePair<string, IRandomAccessStream> strm in stream)
                {
                    LiveConnectClient fileclient = new LiveConnectClient(session);
                    IInputStream strem = strm.Value.GetInputStreamAt(0);
                    LiveOperationResult result1 = await fileclient.BackgroundUploadAsync(childFolderID, strm.Key.ToString(), strem, OverwriteOption.Overwrite);
                }
            }
            catch (Exception ex)
            {
                checktime = true;
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadFavourites;
                Exceptions.SaveOrSendExceptions("Exception in CreateSubFolder Method In UploadStory.cs file", ex);
            }
        }
        public List<object> RetrieveSubfolders()
        {
            list = new List<object>();
            try
            {
                var isoStore = ApplicationData.Current.LocalFolder;
                var story = Task.Run(async () => await isoStore.CreateFolderAsync("StoryRecordings", CreationCollisionOption.OpenIfExists)).Result;
                foreach (var foldername in Task.Run(async () => await story.GetFoldersAsync()).Result)
                {
                    Dictionary<string, object> folder = new Dictionary<string, object>();
                    folder.Add("name", foldername.Name);
                    list.Add(folder);
                }
            }
            catch (Exception ex)
            {
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadFavourites;
                Exceptions.SaveOrSendExceptions("Exception in RetrieveSubfolders Method In UploadStory.cs file", ex);
            }
            return list;
        }
        public Dictionary<string, IRandomAccessStream> GetFiles(string foldername)
        {
            Dictionary<string, IRandomAccessStream> streams = new Dictionary<string, IRandomAccessStream>();
            try
            {
                List<object> list = new List<object>();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                var story = Task.Run(async () => await store1.GetFolderAsync("StoryRecordings")).Result;
                var story1 = Task.Run(async () => await story.GetFolderAsync(foldername)).Result;
                foreach (var filename in Task.Run(async () => await story1.GetFilesAsync()).Result)
                {

                    BasicProperties props = Task.Run(async () => await filename.GetBasicPropertiesAsync()).Result;
                    DateTimeOffset dateModified = (DateTimeOffset)props.DateModified;

                    if (dateModified.ToLocalTime() > Convert.ToDateTime(AppSettings.StoryUploadedDate))
                    {
                        IRandomAccessStream stream = Task.Run(async () => await filename.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;

                        streams.Add(filename.Name, stream);
                    }
                }
            }
            catch (Exception ex)
            {
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadFavourites;
                Exceptions.SaveOrSendExceptions("Exception in GetFiles Method In UploadStory.cs file", ex);
            }
            return streams;
        }
    }
}
