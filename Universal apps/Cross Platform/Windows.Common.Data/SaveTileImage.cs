using Common.Library;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;


#if NOTANDROID
using Windows.Storage;
#endif
#if WINDOWS_APP
using System.Net.Http;
#endif
namespace OnlineVideos.Data
{
 public static   class SaveTileImage
    {
     public static string title = string.Empty;
     public async static void SaveShowlistImage(string imageUrl, string ImageTitle)
     {
            #region WINDOWS_PHONE_APP
            try
            {
                var file = default(StorageFolder);
                file = Task.Run(async () => await ApplicationData.Current.LocalFolder.CreateFolderAsync("Images" ,CreationCollisionOption.OpenIfExists)).Result;
                var httpClient = new HttpClient();
                var url = new Uri(imageUrl);
                var accessToken = "1234";
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                httpRequestMessage.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken));
                httpRequestMessage.Headers.Add("User-Agent", "My user-Agent");
                var response = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);
                int size = Convert.ToInt32(response.Content.Headers.ContentLength);
                byte[] b = new byte[size];
                b = Task.Run(async () => await response.Content.ReadAsByteArrayAsync()).Result;
                var imageFile = await file.CreateFileAsync(ImageTitle + ".jpg", CreationCollisionOption.ReplaceExisting);
                var fs = Task.Run(async () => await imageFile.OpenAsync(FileAccessMode.ReadWrite)).Result;
                var writer = fs.GetOutputStreamAt(0);
                var writer1 = new Windows.Storage.Streams.DataWriter(writer);
                writer1.WriteBytes(b);
                var fi = Task.Run(async () => await writer1.StoreAsync()).Result;
                await writer1.FlushAsync();
                writer1.DetachStream();
                writer1.Dispose();

                int showid = Convert.ToInt32(AppSettings.ShowID);
                List<ShowList> playlist = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).ToListAsync()).Result;
                var showlist = playlist.FirstOrDefault();
                DownloadManager.UpdateTileImage(showlist.Title + ".jpg");
            }
            catch (Exception ex)
            {

            } 
            #endregion

#if ANDROID
			System.Net.WebClient _wc=new System.Net.WebClient();
			System.IO.Stream _stream= _wc.OpenRead(imageUrl);
			var destinationPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), ImageTitle+".jpg");
			if(!System.IO.File.Exists(destinationPath))
			{
				using (var destination = System.IO.File.Create(destinationPath))
				{
					_stream.CopyTo(destination);
				}
			}
#endif
#if WINDOWS_APP
         try
         {
             var file = default(StorageFolder);
             file = Task.Run(async () => await ApplicationData.Current.LocalFolder.GetFolderAsync("Images\\scale-100")).Result;
             var httpClient = new HttpClient();
             var url = new Uri(imageUrl);
             var accessToken = "1234";
             var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
             httpRequestMessage.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken));
             httpRequestMessage.Headers.Add("User-Agent", "My user-Agent");
             var response = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);
             int size = Convert.ToInt32(response.Content.Headers.ContentLength);
             byte[] b = new byte[size];
             b = Task.Run(async () => await response.Content.ReadAsByteArrayAsync()).Result;
             var imageFile = await file.CreateFileAsync(ImageTitle+".jpg", CreationCollisionOption.ReplaceExisting);
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
             
            
         }

#endif
#if WP8 && NOTANDROID
          System.Net.WebClient wc = new System.Net.WebClient();
         wc.OpenReadAsync(new Uri(imageUrl));
         wc.OpenReadCompleted += wc_OpenReadCompleted;
         title = ImageTitle;
#endif
        }
    
      #if WP8 &&  NOTANDROID
     static void wc_OpenReadCompleted(object sender, System.Net.OpenReadCompletedEventArgs e)
     {
         try
         {
             string filepath = string.Empty;
             System.IO.IsolatedStorage.IsolatedStorageFile isoStore = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication();
             if (!isoStore.DirectoryExists("/Images"))
                 isoStore.CreateDirectory("/Images");
             filepath = "/Images/" + title + ".jpg";
             long RequiredSize = e.Result.Length;
             byte[] SaveFile = new byte[RequiredSize];
             System.IO.IsolatedStorage.IsolatedStorageFileStream isoFile = new System.IO.IsolatedStorage.IsolatedStorageFileStream(filepath, System.IO.FileMode.Create, isoStore);
             e.Result.Read(SaveFile, 0, SaveFile.Length);
             isoFile.Write(SaveFile, 0, SaveFile.Length);
             int showid = Convert.ToInt32(AppSettings.ShowID);
             List<ShowList> playlist = Task.Run(async()=> await Constants.connection.Table<ShowList>().Where(i=>i.ShowID==showid).ToListAsync()).Result;   
             var showlist = playlist.FirstOrDefault();
             DownloadManager.UpdateTileImage(showlist.Title + ".jpg");
         }
         catch (Exception ex)
         {


         }
     }
#endif
    }
}
