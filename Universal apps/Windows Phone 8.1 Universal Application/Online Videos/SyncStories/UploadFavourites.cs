using Common.Library;
using Microsoft.Live;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;
//using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;

namespace SyncStories
{
    public class UploadFavourites
    {
        #region "Global Declaration"
        public List<object> list = default(List<object>);
        public StorageFile StorageFile;
        public bool checktime = false;
        string skyDriveFolderID = string.Empty;
        string FolderID = string.Empty;
        string mainFolderID = string.Empty;
        string childFolderID = string.Empty;
        private string folderid = string.Empty;
        public static LiveAuthClient authClient;
        LiveConnectClient client = null;
        public static LiveConnectSession session = default(LiveConnectSession);
        private static string _folderName = string.Empty;
        //private IsolatedStorageFileStream readStream = null;
        private static string _getFolderName = string.Empty;


        #endregion

        #region Methods
        public bool LoginStatus()
        {
            if ((LiveConnectSession)SyncButton.session == null)
                return false;
            else 
                return true;
        }
        public async void CreateFolder(string foldername)
        {
            try
            {
                session = (LiveConnectSession)SyncButton.session;
                _folderName = foldername;
                client = new LiveConnectClient(session);

                LiveOperationResult result = await client.GetAsync("me/skydrive/files?filter=folders");
                if (result != null)
                {
                    dynamic appResult = result.Result;
                    List<object> folderData = appResult["data"];
                    foreach (dynamic folder in folderData)
                    {
                        string name = folder["name"];
                        if (name == _folderName)
                        {
                            skyDriveFolderID = folder["id"];
                        }
                    }

                    if (string.IsNullOrEmpty(skyDriveFolderID))
                    {
                        var skyDriveFolderData = new Dictionary<string, object>();
                        LiveConnectClient parentclient = new LiveConnectClient(session);
                        skyDriveFolderData.Add("name", _folderName);
                        LiveOperationResult result1 = await parentclient.PostAsync("me/skydrive", skyDriveFolderData);
                        if (result1 != null)
                        {
                            dynamic Results = result1.Result;

                            skyDriveFolderID = Results["id"];

                            List<object> list = RetrieveSubfolders();
                            foreach (Dictionary<string, object> item in list)
                            {
                                CreateSubFolder(skyDriveFolderID, item["name"].ToString());
                            }
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
                                Upload(fid, item["name"].ToString());
                            }
                        }
                    }
                    if (checktime == false)
                        AppSettings.StoryUploadedDate = System.DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                checktime = true;
                Exceptions.SaveOrSendExceptions("Exception in CreateFolder Method In UploadStory.cs file", ex);
            }
        }

        public async void Upload(string folderid,string foldername)
        {
            try
            {
                var authclient = new LiveAuthClient();
                var authresult = await authclient.LoginAsync(new string[] { "wl.skydrive", "wl.skydrive_update" });

                if (authresult.Session != null)
                {
                    Dictionary<string, IRandomAccessStream> stream = GetFavFiles("Favourites");
                    foreach (KeyValuePair<string, IRandomAccessStream> strm in stream)
                    {
                        IInputStream strem = strm.Value.GetInputStreamAt(0);
                        var liveclient = new LiveConnectClient(authresult.Session);
                        LiveOperationResult upld = await liveclient.BackgroundUploadAsync(skyDriveFolderID, strm.Key.ToString(), strem, OverwriteOption.Overwrite);
                        //LiveUploadOperation upload = await liveclient.CreateBackgroundUploadAsync(skyDriveFolderID, strm.Key.ToString(), strem, OverwriteOption.Overwrite);
                        //LiveOperationResult uploadresult = await upload.StartAsync();
                        //HandleUploadResult(uploadresult);
                    }
                }
            }
            catch(LiveAuthException ex)
            {
                checktime = true;
            }
        }

        //public void updatefile(string folderid, string foldername)
        //{
        //    try
        //    {   
        //        //InMemoryRandomAccessStream stream = GetFiles(foldername);
        //        Dictionary<string, IRandomAccessStream> stream = GetFiles(foldername);
        //        foreach (KeyValuePair<string, IRandomAccessStream> strm in stream)
        //        {
        //            LiveConnectClient fileclient = new LiveConnectClient(session);
        //            IInputStream strem = strm.Value.GetInputStreamAt(0);
        //            fileclient.BackgroundUploadAsync(folderid, strm.Key.ToString(), strem, OverwriteOption.Overwrite);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        checktime = true;
        //        Exceptions.SaveOrSendExceptions("Exception in updatefile Method In UploadStory.cs file", ex);
        //    }
        //}

        public async Task<string> GetFolderId(string folderid, string foldername)
        {
            try
            {
                LiveConnectClient copyclient = new LiveConnectClient(session);
               LiveOperationResult result = default(LiveOperationResult);
                _getFolderName = foldername;
                LiveOperationResult result1 = await copyclient.GetAsync(folderid);
                if (result1 != null)
                {
                    dynamic Result = result1.Result;
                    List<object> folder = Result["data"];
                    foreach (dynamic folders in folder)
                    {
                        string name = folders["name"];
                        if (name == _getFolderName)
                        {
                            FolderID = folders.id;
                        }
                    }
                }

                return FolderID;
            }
            catch (Exception ex)
            {
                checktime = true;
                return FolderID;
               // Exceptions.SaveOrSendExceptions("Exception in GetFolderId Method In UploadStory.cs file", ex);
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
                    var check = new List<string>();
                    check.Add("System.DateModified");
                    var props = Task.Run(async () => await filename.Properties.RetrievePropertiesAsync(check)).Result;
                    DateTimeOffset dateModified = (DateTimeOffset)props.SingleOrDefault().Value;

                    if (dateModified.ToLocalTime() > AppSettings.StoryUploadedDate)
                    {
                        IRandomAccessStream stream = Task.Run(async () => await filename.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;

                        streams.Add(filename.Name, stream);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetFiles Method In UploadStory.cs file", ex);
            }
            return streams;
        }

        public async void CreateSubFolder(string folderid, string foldername)
        {
            try
            {                
                Dictionary<string, object> folder = new Dictionary<string, object>();
                LiveConnectClient childclient = new LiveConnectClient(session);
                folder.Add("name", foldername);
                LiveOperationResult result = await childclient.PostAsync(folderid, folder);
                if (result != null)
                {
                    dynamic Result = result.Result;
                    childFolderID = Result["id"];

                    Dictionary<string, IRandomAccessStream> stream = GetFiles(_folderName);
                    foreach (KeyValuePair<string, IRandomAccessStream> strm in stream)
                    {
                        LiveConnectClient fileclient = new LiveConnectClient(session);
                        IInputStream strem = strm.Value.GetInputStreamAt(0);
                        LiveOperationResult result1 = await fileclient.BackgroundUploadAsync(childFolderID, strm.Key.ToString(), strem, OverwriteOption.Overwrite);
                    }
                }
            }
            catch (Exception ex)
            {
                checktime = true;
                Exceptions.SaveOrSendExceptions("Exception in CreateSubFolder Method In UploadStory.cs file", ex);
            }
        }

        public async Task CreateFolderForFav(string foldername)
        {
            try
            {
                //SyncButton.Login();
                session = (LiveConnectSession)SyncButton.session;
                LiveConnectClient client = new LiveConnectClient(session);
                _folderName = foldername;

                LiveOperationResult result = await client.GetAsync("me/skydrive/files?filter=folders");
                if (result != null)
                {
                    IDictionary<string, object> appResult = result.Result;
                    List<object> folderData = (List<object>)appResult["data"];
                    foreach (IDictionary<string, object> folder in folderData)
                    {
                        string name = folder["name"].ToString();
                        if (name == _folderName)
                        {
                            skyDriveFolderID = folder["id"].ToString();
                        }
                    }

                    if (string.IsNullOrEmpty(skyDriveFolderID))
                    {
                        var skyDriveFolderData = new Dictionary<string, object>();
                        LiveConnectClient parentclient = new LiveConnectClient(session);
                        skyDriveFolderData.Add("name", _folderName);

                        LiveOperationResult result1 = await parentclient.PostAsync("me/skydrive", skyDriveFolderData);
                        if (result1 != null)
                        {
                            IDictionary<string, object> Results = result1.Result;
                            skyDriveFolderID = Results["id"].ToString();
                            Dictionary<string, IRandomAccessStream> stream = GetFavFiles("Favourites");
                            foreach (KeyValuePair<string, IRandomAccessStream> strm in stream)
                            {
                                LiveConnectClient fileclient = new LiveConnectClient(session);
                                IInputStream strem = strm.Value.GetInputStreamAt(0);
                                LiveOperationResult result2 = await fileclient.BackgroundUploadAsync(skyDriveFolderID, strm.Key.ToString(), strem, OverwriteOption.Overwrite);
                            }
                        }
                    }
                    else
                    {
                        Dictionary<string, IRandomAccessStream> stream = GetFavFiles("Favourites");
                        foreach (KeyValuePair<string, IRandomAccessStream> strm in stream)
                        {
                            //try
                            //{
                                LiveConnectClient fileclient = new LiveConnectClient(session);
                                IInputStream strem = strm.Value.GetInputStreamAt(0);
                              //  LiveUploadOperation upld = await fileclient.CreateBackgroundUploadAsync(skyDriveFolderID, strm.Key.ToString(), strem, OverwriteOption.Overwrite);
                                //LiveOperationResult res = await upld.StartAsync();
                            //}
                            //catch (Exception ex)
                            //{
                            //    Exceptions.SaveOrSendExceptions("Exception in CreateFolder Method In UploadStory.cs file", ex);
                            //}
                           LiveOperationResult result3 = await fileclient.BackgroundUploadAsync(skyDriveFolderID, strm.Key.ToString(), strem, OverwriteOption.Overwrite);
                        }
                     }
                    if (checktime == false)
                        AppSettings.LastUpdatedDateforFavourites = System.DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                checktime = true;
                Exceptions.SaveOrSendExceptions("Exception in CreateFolder Method In UploadStory.cs file", ex);                
            }
            finally
            {
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadParentalControlPreferences;
            }
        }

        public Dictionary<string, IRandomAccessStream> GetFavFiles(string foldername)
         {
            Dictionary<string, IRandomAccessStream> streams = new Dictionary<string, IRandomAccessStream>();
            try
             {
                //StorageFolder store11 = ApplicationData.Current.LocalFolder;

                //var file = Task.Run(async () => await store11.GetFileAsync(""));





                List<object> list = new List<object>();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                var story = Task.Run(async () => await store1.GetFolderAsync(foldername)).Result;

                foreach (var filename in Task.Run(async () => await story.GetFilesAsync()).Result)
                {                    
                    BasicProperties props = Task.Run(async () => await filename.GetBasicPropertiesAsync()).Result;
                    //DateTimeOffset dateModified = (DateTimeOffset)props.DateModified;
                    //if (dateModified.ToLocalTime() > Convert.ToDateTime(AppSettings.LastUpdatedDateforFavourites))
                    //{
                        IRandomAccessStream stream = Task.Run(async () => await filename.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                        streams.Add(filename.Name, stream);
                    //}
                }
             }
            catch (Exception ex)
            {               
                string excep = ex.Message;
                Exception e1 = ex.InnerException;
                string e2 = ex.StackTrace;
                Exceptions.SaveOrSendExceptions("Exception in GetFavFiles Method In UploadStory.cs file", ex);
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadParentalControlPreferences;
            }
            return streams;
        }
        #endregion

        //private async void Upload()
        //{
        //    var authclient=new LiveAuthClient();
        //    var authresult=await authclient.LoginAsync(new string[] {"wl.skydrive", "wl.skydrive_update" });
            
        //    if(authresult.Session!=null)
        //    {
        //        Dictionary<string, IRandomAccessStream> stream = GetFavFiles("Favourites");
        //        foreach (KeyValuePair<string, IRandomAccessStream> strm in stream)
        //        {
        //            IInputStream strem = strm.Value.GetInputStreamAt(0);
        //            var liveclient = new LiveConnectClient(authresult.Session);
        //            LiveUploadOperation upload = await liveclient.CreateBackgroundUploadAsync(skyDriveFolderID, strm.Key.ToString(), strem, OverwriteOption.Overwrite);
        //            LiveOperationResult uploadresult = await upload.StartAsync();
        //            //HandleUploadResult(uploadresult);                    
        //        }
        //    }

        //    var pendingoperations = await LiveConnectClient.GetCurrentBackgroundUploadsAsync();
        //    foreach(LiveDownloadOperation pendingoprtn in pendingoperations)
        //    {
        //        try
        //        {
        //            var opres = await pendingoprtn.AttachAsync();
        //        }
        //        catch(Exception ex)
        //        {
        //        }
        //    }
        //}
    }
}


