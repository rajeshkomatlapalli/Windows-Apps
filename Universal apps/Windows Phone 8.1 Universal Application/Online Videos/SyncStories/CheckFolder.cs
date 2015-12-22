using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Microsoft.Live;
using System.Threading;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Library;

namespace SyncStories
{
    public class CheckFolder
    {
        private string foldername = string.Empty;
        private string folderid = string.Empty;
        private LiveConnectSession session;
       
        public async Task<string> GetFolderId(string skyDriveFolderName, string folder)
        {
            try
            {
                session = (LiveConnectSession)SyncButton.session;
                LiveConnectClient copyclient = new LiveConnectClient(session);
                if (folder != "")
                    foldername = folder;
                else
                    foldername = skyDriveFolderName;
                LiveOperationResult result = default(LiveOperationResult);
                if (foldername == skyDriveFolderName)
                  result=await copyclient.GetAsync("me/skydrive/files?filter=folders");
                else
                    result = await copyclient.GetAsync(skyDriveFolderName);
                if (result != null)
                {
                    Dictionary<string, object> folderData = (Dictionary<string, object>)result.Result;
                    //var d = folderData.Where(i => i.Key == "data").Select(i => i.Value).ToList().First().AsQueryable().Cast<Dictionary<string, object>>().Where(i => i.Where(j => j.Key == "name").Select(j => j.Value).ToString() == foldername).Select(i => i.Where(j => j.Key == "id").Select(j => j.Value)).Select(i => i).ToString();
                    //var d = folderData.Where(i => i.Key == "data").Select(i=>i.Value);

                    List<object> folders = (List<object>)folderData["data"];
                    foreach (object item in folders)
                    {
                        Dictionary<string, object> folder2 = (Dictionary<string, object>)item;
                        if (folder2["name"].ToString() == foldername)
                            folderid = folder2["id"].ToString();
                    }
                }
                return folderid;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetFolderId Method In CheckFolder.cs file", ex);
                return folderid;
            }
        }       
    }
}
