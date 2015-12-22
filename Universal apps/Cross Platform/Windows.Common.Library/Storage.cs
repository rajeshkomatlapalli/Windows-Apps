using System;
using System.Net;
using System.Xml.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Windows.Storage;
using Windows.Storage.Streams;
#if WINDOWS_PHONE_APP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Devices.Enumeration;
//using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
using Windows.Storage.Pickers;
using System.Collections.Generic;
using Windows.Storage.Search;
#endif
#if WP8
#if NOTANDROID
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Media;
#endif
using System.IO.IsolatedStorage;
#endif
namespace Common.Library
{
    public static class Storage
    {
#if NOTANDROID || WINDOWS_PHONE_APP
        public static XDocument ReadFileAsDocumentFromLibrary(string filename)
        {
            XDocument xdoc = new XDocument();
            StorageFolder store = ApplicationData.Current.LocalFolder;
            var story = Task.Run(async () => await store.CreateFolderAsync("StoryRecordings", CreationCollisionOption.OpenIfExists)).Result;
            var story1 = Task.Run(async () => await story.CreateFolderAsync(AppSettings.ShowID, CreationCollisionOption.OpenIfExists)).Result;
            StorageFile file = Task.Run(async () => await story1.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists)).Result;
            var f = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
            IInputStream inputStream = f.GetInputStreamAt(0);
            DataReader dataReader = new DataReader(inputStream);
            uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
            string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
            System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlData));
            xdoc = XDocument.Load(ms);
            dataReader.DetachStream();
            inputStream.Dispose();
            f.Dispose();
            ms.Dispose();
            return xdoc;
        }

        public static bool FileExistsInMusicLibrary(string path)
        {
            var isoStore = ApplicationData.Current.LocalFolder;
            var story = Task.Run(async () => await isoStore.CreateFolderAsync("StoryRecordings", CreationCollisionOption.OpenIfExists)).Result;
            var story1 = Task.Run(async () => await story.CreateFolderAsync(AppSettings.ShowID, CreationCollisionOption.OpenIfExists)).Result;
            bool Exists = false;
            try
            {
                var FileExists = Task.Run(async () => await story1.GetFileAsync(path)).Result;
                if (FileExists != null)
                    Exists = true;
            }
            catch (Exception ex)
            {
                Exists = false;
            }
            return Exists;
        }

        public static bool FolderExistsInMusicLibrary(string filename)
        {
            var isoStore = ApplicationData.Current.LocalFolder;
            var story = Task.Run(async () => await isoStore.CreateFolderAsync("StoryRecordings", CreationCollisionOption.OpenIfExists)).Result;

            bool Exists = false;
            try
            {
                var FolderExists = Task.Run(async () => await story.GetFolderAsync(filename)).Result;
                if (FolderExists != null)
                    Exists = true;
            }
            catch (Exception ex)
            {
                Exists = false;
            }
            return Exists;
        }

        public static void DeleteInMusicLibrary(string foldername)
        {
            var isoStore = Task.Run(async () => await ApplicationData.Current.LocalFolder.GetFolderAsync("StoryRecordings")).Result;
            var isoStore1 = Task.Run(async () => await isoStore.GetFolderAsync(foldername)).Result;
            Task.Run(async () => await isoStore1.DeleteAsync());
        }

        public static void DeleteFile(string filename)
        {
            try
            {
                StorageFile isoStore = Task.Run(async () => await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(filename)).Result;
                Task.Run(async () => await isoStore.DeleteAsync());
                //StorageFile isoStore =await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(filename);
                //await isoStore.DeleteAsync();
            }
            catch (Exception ex)
            {
            }
        }

        public async static Task<bool> RingToneFileExists(string filename)
        {
            var isoStore = Windows.Storage.KnownFolders.MusicLibrary;
            bool Exists = false;
            try
            {
                var FileExists = await isoStore.GetFileAsync(filename);
                if (FileExists != null)
                    Exists = true;
            }
            catch (Exception ex)
            {
                Exists = false;
            }
            return Exists;
        }

        public static void DeleteRingToneFile(string filename)
        {
            StorageFile isoStore = Task.Run(async () => await Windows.Storage.KnownFolders.MusicLibrary.GetFileAsync(filename)).Result;
            Task.Run(async () => await isoStore.DeleteAsync());
        }

        public static float GetFileSize(string filename, FileSizeUnit unit)
        {
            float fileSize = -1;
            try
            {
                if (Task.Run(async () => await RingToneFileExists(filename)).Result)
                {
                    StorageFolder store = KnownFolders.MusicLibrary;
                    StorageFile file = Task.Run(async () => await store.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists)).Result;
                    var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.ReadWrite)).Result;
                    fileSize = fquery.Size / (float)unit;
                    Task.Run(async () => await fquery.FlushAsync());
                    fquery.Dispose();
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            return fileSize;
        }

        public async static Task<bool> FolderExists(string path)
        {
            var isoStore = Windows.Storage.ApplicationData.Current.LocalFolder;
            bool Exists = false;
            try
            {
                var FolderExists = await isoStore.GetFolderAsync(path);
                if (FolderExists != null)
                    Exists = true;
            }
            catch (Exception ex)
            {
                Exists = false;
            }
            return Exists;            
        }
#if WINDOWS_APP
        public static bool FileExistsForWp8(string filename)
        {
            StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            //StorageFile folder1 = Task.Run(async () => await folder.GetFileAsync(filename)).Result;

            //if(folder1!=null)
            //{
            //    return Convert.ToBoolean(folder1);
            //}
            //else
            //{
            //    return false;
            //}            
            try
            {
                StorageFile folder1 = Task.Run(async () => await folder.GetFileAsync(filename)).Result;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

            //IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
            //return isoStore.FileExists(filename);
        } 
#endif
#if WINDOWS_PHONE_APP

        public static bool FileExistsForWp8(string filename)
        {
            StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            //StorageFile folder1 = Task.Run(async () => await folder.GetFileAsync(filename)).Result;

            //if(folder1!=null)
            //{
            //    return Convert.ToBoolean(folder1);
            //}
            //else
            //{
            //    return false;
            //}            
            try
            {
                StorageFile folder1 = Task.Run(async () => await folder.GetFileAsync(filename)).Result;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }                      
            
            //IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
            //return isoStore.FileExists(filename);
        }
        public static async Task<bool> DirectoryExists(string filePath)
        {            
            //IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
            //return isoStore.DirectoryExists(filePath);

            try
            {
                StorageFolder folder = ApplicationData.Current.LocalFolder;

                var folder1 = await StorageFolder.GetFolderFromPathAsync(filePath);
                var files = await folder1.GetFilesAsync(CommonFileQuery.OrderByName);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            // StorageFolder folder = Task.Run(async () => await isoStore.GetFolderAsync(AppSettings.ProjectName)).Result;
            //try
            //{
            //    StorageFolder FileExists = await folder.GetFolderAsync(filePath);
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}
        }

        public static void CreateDirectory(string filePath)
        {
            StorageFolder store11 = ApplicationData.Current.LocalFolder;
            var story12 = Task.Run(async () => await store11.CreateFolderAsync(filePath, CreationCollisionOption.OpenIfExists)).Result;
            var story13 = Task.Run(async () => await story12.CreateFolderAsync(AppSettings.ShowID, CreationCollisionOption.OpenIfExists)).Result;

            //IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
            //isoStore.CreateDirectory(filePath);
        }
          public async static Task<bool> FavouriteFileExists(string filename)
        {
            var isoStore = Windows.Storage.ApplicationData.Current.LocalFolder;
            bool Exists = false;
            try
            {
                var folder = Task.Run(async () => await isoStore.GetFolderAsync("Favourites")).Result;
                var FileExists = await folder.GetFileAsync(filename);
                if (FileExists != null)
                    Exists = true;

            }
            catch (Exception ex)
            {
                Exists = false;
            }
            return Exists;
        }
#endif

#if WINDOWS_APP
        public async static Task<bool> FavouriteFileExists(string filename)
        {
            var isoStore = Windows.Storage.ApplicationData.Current.LocalFolder;
            bool Exists = false;
            try
            {
                string name = AppSettings.ProjectName;
                var folder = Task.Run(async () => await isoStore.GetFolderAsync(name)).Result;
                var FileExists = await folder.GetFileAsync(filename);
                if (FileExists != null)
                    Exists = true;

            }
            catch (Exception ex)
            {
                Exists = false;
            }


            return Exists;
        }
#endif
        public async static Task<bool> FavFileExists(string filename)
        {
            var isoStore = Windows.Storage.ApplicationData.Current.LocalFolder;
            bool Exists = false;
            try
            {
                var folder = Task.Run(async () => await isoStore.GetFolderAsync(AppSettings.ProjectName)).Result;
                var FileExists = await folder.GetFileAsync(filename);
                if (FileExists != null)
                    Exists = true;

            }
            catch (Exception ex)
            {
                Exists = false;
            }

            return Exists;
        }

        public async static Task<bool> FolderExist(string FolderName)
        {
            var isoStore = KnownFolders.VideosLibrary;
            bool Exists = true;
            try
            {
                var FileExists = await isoStore.GetFolderAsync(FolderName);
                if (FileExists != null)
                    Exists = false;

            }
            catch (Exception ex)
            {
                Exists = true;
            }
            return Exists;
        }
        public async static Task<bool> MusicFolderExist(string FolderName)
        {
            var isoStore = ApplicationData.Current.LocalFolder;
            bool Exists = true;
            try
            {
                var FileExists = await isoStore.GetFolderAsync(FolderName);
                if (FileExists != null)
                    Exists = false;

            }
            catch (Exception ex)
            {
                Exists = true;
            }
            return Exists;
        }

        public async static Task<bool> MusicFileExists(string filename)
        {
            var isoStore = KnownFolders.MusicLibrary;
            bool Exists = false;
            try
            {
                var sf = Task.Run(async () => await KnownFolders.MusicLibrary.GetFolderAsync(AppSettings.ProjectName)).Result;
                var FileExists = await sf.GetFileAsync(filename);
                if (FileExists != null)
                    Exists = true;

            }
            catch (Exception ex)
            {
                Exists = false;
            }
            return Exists;
        }

        public static async Task<XDocument> ReadFileAsDocument(string filename)
        {
#if WINDOWS_APP || WINDOWS_PHONE_APP
            XDocument xdoc = new XDocument();
            try
            {
                StorageFolder store = ApplicationData.Current.LocalFolder;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(filename, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
//#if WP8
//                StorageFile file = Task.Run(async () => await store.CreateFileAsync(filename.Replace(AppSettings.ProjectName, string.Empty).Replace("/", string.Empty), Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
//#endif
                var f = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = f.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
                string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
                System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlData));
                try
                {
                    xdoc = XDocument.Load(ms);
                }
                catch (Exception ex)
                {
                }
                dataReader.DetachStream();
                inputStream.Dispose();
                f.Dispose();
                ms.Dispose();
                return xdoc;
            }
            catch (Exception ex)
            {
            }
            return xdoc;
#endif

#if WINDOWS_PHONE_APP
            XDocument xdoc1 = null;
            try
            {
                string FileName=string.Empty;
                //IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                StorageFolder isoStore = Windows.Storage.ApplicationData.Current.LocalFolder;
                //if (isoStore.FileExists(filename))
                //{
                if (filename.Contains(AppSettings.ProjectName))
                    FileName = filename.Replace(AppSettings.ProjectName, string.Empty).Replace("/", string.Empty);
                else
                    FileName = filename;
                try
                {
                    //await isoStore.GetFileAsync(FileName);
                    //StorageFolder store = ApplicationData.Current.LocalFolder;
                    StorageFile file = await isoStore.GetFileAsync(FileName);
                    IRandomAccessStream accessStream = await file.OpenReadAsync();
                    Stream stream = accessStream.AsStreamForRead((int)accessStream.Size);
                    //  IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(FileName, FileMode.Open, isoStore);
                    if (stream.Length > 0)
                    {
                        xdoc1 = new XDocument();
                        xdoc1 = XDocument.Load(stream);
                    }                    
                    stream.Dispose();                    
                }
                catch(Exception ex)
                { 
                }
            }
            catch (Exception ex)
            {

            }
            return xdoc1;
#endif
        }

        public static XDocument ReadFileAsDocument1(string filename)
        {
            XDocument xdoc = new XDocument();
            try
            {
                //if (Task.Run(async () => await Storage.FileExists("DownLoadVideo.xml")).Result)
                //{
                //    StorageFolder store = ApplicationData.Current.LocalFolder;
                //    StorageFile file = Task.Run(async () => await store.GetFileAsync(filename)).Result;
                //    var f = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                //    IInputStream inputStream = f.GetInputStreamAt(0);
                //    DataReader dataReader = new DataReader(inputStream);
                //    uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
                //    string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
                //    System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlData));
                //    xdoc = XDocument.Load(ms);
                //    dataReader.DetachStream();
                //    inputStream.Dispose();
                //    f.Dispose();
                //    ms.Dispose();
                //}
            }
            catch (Exception ex)
            {
            }
            return xdoc;
        }
        public async static void SaveFileFromDocument(string filename, XDocument xdoc)
        {
#if WINDOWS_APP || WINDOWS_PHONE_APP
            try
            {
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async()=> await store1.CreateFileAsync(filename, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
//#if WP8
//                string FileName = filename.Replace(AppSettings.ProjectName, string.Empty).Replace("/", string.Empty);

//                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(FileName, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
//#endif
                var fquery1 = Task.Run(async () => await file1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                var outputStream = fquery1.GetOutputStreamAt(0);
                var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                StringBuilder sb = new StringBuilder();
                TextWriter tx = new StringWriter(sb);
                xdoc.Save(tx);
                string text = tx.ToString();
                text = text.Replace("utf-16", "utf-8");
                writer.WriteString(text);
                var fi = Task.Run(async () => await writer.StoreAsync()).Result;
                var oi = Task.Run(async () => await outputStream.FlushAsync()).Result;
                writer.DetachStream();
                outputStream.Dispose();
                fquery1.Dispose();
            }
            catch (Exception ex)
            {
                string exp = ex.ToString();
            }
#endif
#if WINDOWS_PHONE_APP 

            StorageFolder isoStore = Windows.Storage.ApplicationData.Current.LocalFolder;
//            IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
            string FileName = string.Empty;
            if (filename.Contains(AppSettings.ProjectName))
                FileName = filename.Replace(AppSettings.ProjectName, string.Empty).Replace("/", string.Empty);
            else
                FileName = filename;

            try
            {
                StorageFile isoStream = Task.Run(async () => await isoStore.CreateFileAsync(FileName, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                //IRandomAccessStream accessStream = await isoStream.OpenReadAsync();
                //Stream stream = accessStream.AsStreamForRead((int)accessStream.Size);

                var fquery1 = Task.Run(async () => await isoStream.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                var outputStream = fquery1.GetOutputStreamAt(0);
                var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                StringBuilder sb = new StringBuilder();
                TextWriter tx = new StringWriter(sb);
                //  IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(FileName, FileMode.Create, isoStore);
                xdoc.Save(tx);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            //isoStream.Close();
#endif

        }
        public static async Task<bool> WriteToFile(StorageFolder folder, string fileName, String content)
        {
            StorageFile file = await folder.CreateFileAsync(fileName,
            CreationCollisionOption.ReplaceExisting);
            IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);
            IOutputStream outputFileStream = fileStream.GetOutputStreamAt(0);
            DataWriter dataWriter = new DataWriter(outputFileStream);
            dataWriter.WriteString(content);
            await dataWriter.StoreAsync();
            bool result = await outputFileStream.FlushAsync();
            return result;
        }
        public static XElement ReadFileElements(string filename)
        {
            XElement xdoc = default(XElement);
            try
            {
                StorageFolder store = ApplicationData.Current.LocalFolder;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(filename, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var f = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = f.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
                string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
                System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlData));
                xdoc = XElement.Load(ms);
                dataReader.DetachStream();
                inputStream.Dispose();
                f.Dispose();
                ms.Dispose();
            }
            catch (Exception ex)
            {
            }
            return xdoc;
        }
        public static async void SaveFileFromDocument1(string filename, XDocument xdoc)
        {
            try
            {
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = await store1.CreateFileAsync(filename, Windows.Storage.CreationCollisionOption.ReplaceExisting);
                var fquery1 = await file1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
                var outputStream = fquery1.GetOutputStreamAt(0);
                var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                StringBuilder sb = new StringBuilder();
                TextWriter tx = new StringWriter(sb);
                xdoc.Save(tx);
                string text = tx.ToString();
                text = text.Replace("utf-16", "utf-8");
                writer.WriteString(text);
                var fi = await writer.StoreAsync();
                var oi = await outputStream.FlushAsync();
                outputStream.Dispose();
                fquery1.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
        public static async Task<string> ReadFileToString(StorageFolder folder, string fileName)
        {
            string documentText = string.Empty;
            StorageFile file = await folder.GetFileAsync(fileName);
            IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);
            IInputStream inputStream = fileStream.GetInputStreamAt(0);
            uint fileSize = (uint)fileStream.Size;
            DataReader dataReader = new DataReader(inputStream);
            await dataReader.LoadAsync(fileSize);
            documentText = dataReader.ReadString(fileSize);
            return documentText;
        }

        public static void saveSettings(string keyname, string keyvalue)
        {
            try
            {
                var applicationData = Windows.Storage.ApplicationData.Current;
                var localSettings = applicationData.LocalSettings;
                localSettings.Values[keyname] = keyvalue;
                applicationData.SignalDataChanged();
            }
            catch (Exception ex)
            {
                Exception exNew = new Exception("Exception in saveSettings Method In IsolatedSettings file\n\n" + ex.Message + " \n\n Stack Trace \n" + ex.StackTrace);
                throw exNew;
            }
        }
#if WP8 || WINDOWS_PHONE_APP
        public static BitmapImage ReadBitmapImageFromFileForWp8(string imagefile, BitmapCreateOptions options)
        {
            BitmapImage image = null;           
            try
            {                 
                //StorageFile.GetUserStoreForApplication();
                //StorageFile.GetFileFromApplicationUriAsync();
                Windows.Storage.StorageFolder localfolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                string[] FoldersNames = imagefile.TrimStart('/').Split('/');
                StorageFolder GetFolder =ApplicationData.Current.LocalFolder;
                foreach (string foldername in FoldersNames)
                {
                    if (foldername != FoldersNames[FoldersNames.Length - 1].ToString())
                        GetFolder = Task.Run(async () => await GetFolder.GetFolderAsync(foldername)).Result;
                }
              StorageFile GetFile = Task.Run(async () =>await GetFolder.GetFileAsync(FoldersNames[FoldersNames.Length -1].ToString())).Result;

              //  IRandomAccessStream stre = await GetFile.OpenAsync(Windows.Storage.FileAccessMode.Read);
                Stream stre = Task.Run(async () => await GetFile.OpenStreamForReadAsync()).Result;
              // Stream stre = Task.Run(async () => await GetFile.OpenStreamForReadAsync()).Result;
                StreamReader reader = new StreamReader(stre);
                //Windows.Storage.Streams.IInputStream read = stre.AsInputStream();
                image = new BitmapImage();
                image.CreateOptions = options;
                image.SetSource(stre.AsRandomAccessStream());
            }
            catch (Exception ex)
            {
            }
            return image;
        }

        public static ImageSource ReadBitmapImageFromFileInBackground(string imagefile, BitmapCreateOptions options)
        {
           BitmapImage image = null;
            AutoResetEvent wait = new AutoResetEvent(false);
            if (Constants.UIThread == false)
            {     
             Task.Run(async()=>{ await Windows.UI.Xaml.Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                //Deployment.Current.Dispatcher.BeginInvoke(delegate()
                {
                    //string uri = "ms-appx://" + imagefile;
                    image = ReadBitmapImageFromFileForWp8(imagefile, options);
                    wait.Set();
                });
            });
                wait.WaitOne();
            }
            else
            {
                //string uri = "ms-appx://" + imagefile;
                image = ReadBitmapImageFromFileForWp8(imagefile, options);
                //image = ReadBitmapImageFromFileForWp8(imagefile, options);
            }
            return image;
        }
        //public static void ResizeImageForLiveTile(ImageSource bitmap, string ImagePath)
        //{
        //    WriteableBitmap wbOutput;
        //    Image imgTemp = new Image();
        //    imgTemp.Source = bitmap;

        //    //wbOutput = new WriteableBitmap(imgTemp,null);
        //    using(MemoryStream stream = new MemoryStream())
        //    {
        //        wbOutput.SaveJpeg(stream, 173, 173, 0, 100);
        //        using (Windows.Storage.StorageFolder myIsolatedStorage = Windows.Storage.ApplicationData.Current.LocalFolder)
        //       // using (StorageFile myIsolatedStorage = StorageFile.GetUserStoreForApplication())
        //        {
        //            Storage.DeleteSecondaryTileImage(ImagePath);
        //            if (!myIsolatedStorage.DirectoryExists(Constants.SecondaryTileImagePath))
        //                myIsolatedStorage.CreateFolderAsync(Constants.SecondaryTileImagePath);//.CreateDirectory(Constants.SecondaryTileImagePath);

        //            using (IolatedStorageFileStream local = myIsolatedStorage.CreateFile(Constants.SecondaryTileImagePath + "/" + ImagePath))
        //            {
        //                local.Write(stream.GetBuffer(), 0, stream.GetBuffer().Length);
        //                local.Close();
        //            }
        //        }
        //    }
        //}


        public async static void ResizeImageForLiveTile(ImageSource bitmap, string ImagePath)
        {          
            //WriteableBitmap wbOutput;
            //Image imgTemp = new Image();
            //imgTemp.Source = bitmap;

            //wbOutput = new WriteableBitmap(imgTemp, null);
            //using (MemoryStream stream = new MemoryStream())
            //{
            //    wbOutput.SaveJpeg(stream, 173, 173, 0, 100);
            //    using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            //    {
            //        Storage.DeleteSecondaryTileImage(ImagePath);
            //        if (!myIsolatedStorage.DirectoryExists(Constants.SecondaryTileImagePath))
            //            myIsolatedStorage.CreateDirectory(Constants.SecondaryTileImagePath);

            //        using (IsolatedStorageFileStream local = myIsolatedStorage.CreateFile(Constants.SecondaryTileImagePath + "/" + ImagePath))
            //        {
            //            local.Write(stream.GetBuffer(), 0, stream.GetBuffer().Length);
            //            local.Close();
            //        }
            //    }
            //}

            WriteableBitmap writeableBitmap = new WriteableBitmap(300, 300); 
            StorageFile file = await StorageFile.GetFileFromPathAsync(bitmap.ToString());
            using(IRandomAccessStream fileStream=await file.OpenAsync(FileAccessMode.Read))
            {
                await writeableBitmap.SetSourceAsync(fileStream);
            }
            FileSavePicker picker = new FileSavePicker();
            picker.FileTypeChoices.Add("JPG File", new List<string>() { ".jpg" });
            StorageFile savefile = await picker.PickSaveFileAsync();
            if (savefile == null)
                return;
            IRandomAccessStream stream1 = await savefile.OpenAsync(FileAccessMode.ReadWrite);
            BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream1);
            // Get pixels of the WriteableBitmap object 
            Stream pixelStream = writeableBitmap.PixelBuffer.AsStream();
            byte[] pixels = new byte[pixelStream.Length];
            await pixelStream.ReadAsync(pixels, 0, pixels.Length);
            // Save the image file with jpg extension 
            encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)writeableBitmap.PixelWidth, (uint)writeableBitmap.PixelHeight, 96.0, 96.0, pixels);
            await encoder.FlushAsync();
        }

        public static BitmapImage ReadBitmapImageFromFile(string imagefile, BitmapCreateOptions options)
        {
            BitmapImage image = null;
            string image1 = null;
            try
            {

                string[] FoldersNames = imagefile.TrimStart('/').Split('/');
                StorageFolder GetFolder1 = Task.Run(async () => await ApplicationData.Current.LocalFolder.GetFolderAsync(FoldersNames[0])).Result;
                StorageFolder GetFolder2 = Task.Run(async () => await GetFolder1.GetFolderAsync(FoldersNames[1])).Result;
                StorageFile GetFile = Task.Run(async () => await GetFolder2.GetFileAsync(FoldersNames[2])).Result;
                Stream stre = Task.Run(async () => await GetFile.OpenStreamForReadAsync()).Result;
                StreamReader reader = new StreamReader(stre);
                image = new BitmapImage();
                image.CreateOptions = options;
                image.SetSource(stre.AsRandomAccessStream());

            }
            catch (Exception ex)
            {
            }
            return image;

        }
        public static void DeleteSecondaryTileImage(string TileImage)
        {
            DeleteFile(Constants.SecondaryTileImagePath + TileImage);
        }


       public async static Task<BitmapImage> ReadImageFromLocalStorage(string ImageFile)
        {
            BitmapImage isoImage = null;
            //StorageFolder isoStore1 = ApplicationData.Current.LocalFolder;

           // Windows.Storage.StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
           // StorageFile isostore = local.GetFileAsync();
            //IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
            Windows.Storage.StorageFolder isoStore = Windows.Storage.ApplicationData.Current.LocalFolder;

            try
            {
                var file = Task.Run(async () => await isoStore.GetFileAsync(ImageFile)).Result;
                isoImage = new BitmapImage();
                isoImage.CreateOptions = BitmapCreateOptions.None;
                //using(ApplicationData.Current.LocalFolder local=isoStore.GetFileAsync(ImageFile))
                //StorageFile file1 = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(ImageFile);
                using (var local = Task.Run(async () => await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result)
                {
                    isoImage.SetSource(local);
                }
            }
            catch (Exception ex)
            {
                return isoImage;
            }
            return isoImage;
        } private static bool isLowMemDeviceValue;

        //public static IsolatedStorageFileStream CreateFile(string filename)
        //{
        //    //IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
        //    Windows.Storage.StorageFile isoStore = Windows.Storage.StorageFile.GetFileFromApplicationUriAsync();
        //    return isoStore.CreateFile(filename);
        //}

       public async static Task<StorageFile> CreateFile(string filename)
        {
            //IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
           // StorageFolder isoStore = ApplicationData.Current.LocalFolder;

           //Stream dd= isoStore.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
           //return dd;
            // return isoStore.CreateFile(filename);
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile storageFileIsolated = await localFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            return storageFileIsolated;
            //Stream GetFolder1 = Task.Run(async () => await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(filename)).Result;
            //return GetFolder1;
        }

       public static Stream OpenFile(string filename)
        {

            Stream GetFolder1 = Task.Run(async () => await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(filename)).Result;
            return GetFolder1;

            //StorageFile file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(filename);
            //return await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);

            //var stream = await StorageFolder.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
            //StorageFile isoStore = StorageFile
            //return isoStore.OpenFile(filename, mode, access);
        }
        public async static void DeleteIsolatedFoldersAndFilesFile()
        {
            
            StorageFolder GetFolder1 = Task.Run(async () => await ApplicationData.Current.LocalFolder.CreateFolderAsync(ResourceHelper.ProjectName)).Result;
            await GetFolder1.DeleteAsync(StorageDeleteOption.PermanentDelete);


            StorageFolder GetFolder2 = Task.Run(async () => await ApplicationData.Current.LocalFolder.GetFolderAsync(ResourceHelper.IsolatedDirectoryName)).Result;
            await GetFolder2.DeleteAsync(StorageDeleteOption.PermanentDelete);


          //  Windows.Storage.StorageFolder isoStore = Windows.Storage.ApplicationData.Current.LocalFolder;
          ////  StorageFolder isoStore = ApplicationData.Current.LocalFolder; 
           
          //  StorageFolder ss1 = isoStore.GetFolderAsync(ResourceHelper.ProjectName);
          //  await ss1.DeleteAsync(StorageDeleteOption.PermanentDelete);
          //  var ss2 = isoStore.GetFolderAsync(ResourceHelper.IsolatedDirectoryName);
          //  await isoStore.DeleteAsync(StorageDeleteOption.PermanentDelete);

            //if (isoStore.CreateFolderAsync(ResourceHelper.ProjectName))
            //{
            //    isoStore.DeleteDirectory(ResourceHelper.ProjectName);
            //}
            //if (isoStore.DirectoryExists(ResourceHelper.IsolatedDirectoryName))
            //{
            //    isoStore.DeleteDirectory(ResourceHelper.IsolatedDirectoryName);
            //}
        }

        //public static bool IsLowMemDevice
        //{
        //    get
        //    {
        //        //if (ApplicationData.Current.RoamingSettings.Values.Contains("IsLowmemDevice")) isLowMemDeviceValue = (bool)ApplicationData.Current.RoamingSettings["IsLowMemDevice"];
        //        if (IsolatedStorageSettings.ApplicationSettings.Contains("IsLowMemDevice"))
        //            isLowMemDeviceValue = (bool)IsolatedStorageSettings.ApplicationSettings["IsLowMemDevice"];
        //        return isLowMemDeviceValue;
        //    }

        //    set
        //    {
        //        if (value != IsLowMemDevice)
        //            IsolatedStorageSettings.ApplicationSettings["IsLowMemDevice"] = value;
        //    }
        //}

        public static bool IsLowMemDevice
        {
            get
            {
                //if (ApplicationData.Current.RoamingSettings.Values.Contains("IsLowmemDevice")) isLowMemDeviceValue = (bool)ApplicationData.Current.RoamingSettings["IsLowMemDevice"];
                if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey("IsLowMemDevice"))
                    isLowMemDeviceValue = (bool)Windows.Storage.ApplicationData.Current.LocalSettings.Values["IsLowMemDevice"];
                return isLowMemDeviceValue;
            }

            set
            {
                if (value != IsLowMemDevice)
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values["IsLowMemDevice"] = value;
            }
        }

#endif
        public static string getSettingsStringValue(string keyname)
        {
            try
            {
                var applicationData = Windows.Storage.ApplicationData.Current;
                var localSettings = applicationData.LocalSettings;
                string keyvalue = "";
                var value = localSettings.Values[keyname];
                if (value == null)
                    keyvalue = "";
                else
                    keyvalue = value.ToString();

                return keyvalue;
            }
            catch (Exception ex)
            {
                Exception exNew = new Exception("Exception in getSettingsStringValue Method In Common file\n\n" + ex.Message + " \n\n Stack Trace \n" + ex.StackTrace);
                throw exNew;
            }
        }
        #region HomeBuyer
        public static XDocument ReadFileAsDocument2(string filename)
        {
            XDocument xdoc = new XDocument();
            try
            {
                if (Task.Run(async () => await Storage.FileExists(@"XmlData\TotalExpensive.xml")).Result)
                {
                    StorageFolder store = ApplicationData.Current.LocalFolder;
                    StorageFile file = Task.Run(async () => await store.GetFileAsync(filename)).Result;
                    var f = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                    IInputStream inputStream = f.GetInputStreamAt(0);
                    DataReader dataReader = new DataReader(inputStream);
                    uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
                    string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlData));
                    xdoc = XDocument.Load(ms);
                    dataReader.DetachStream();
                    Task.Run(async () => await f.FlushAsync());
                    inputStream.Dispose();
                    f.Dispose();
                    ms.Dispose();
                }
            }
            catch (Exception ex)
            {
            }
            return xdoc;
        }
        #endregion
#endif

#if ANDROID
		public static void CreateDirectory(string directoryName)
		{
			IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
			if (!isoStore.DirectoryExists(directoryName))
				isoStore.CreateDirectory(directoryName);
		}

		public static void DeleteSecondaryTileImage(string TileImage)
		{
			DeleteFile(Constants.SecondaryTileImagePath + TileImage);
		}

		public static void DeleteFile(string filename)
		{
			try
			{
				IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();

				if (isoStore.FileExists(filename))
				{
					isoStore.DeleteFile(filename);
				}
			}
			catch (Exception ex)
			{

			}
		}
		public static void DeleteIsolatedFoldersAndFilesFile()
		{
			IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();

			if (isoStore.DirectoryExists(ResourceHelper.ProjectName))
			{
				isoStore.DeleteDirectory(ResourceHelper.ProjectName);
			}
			if (isoStore.DirectoryExists(ResourceHelper.IsolatedDirectoryName))
			{
				isoStore.DeleteDirectory(ResourceHelper.IsolatedDirectoryName);
			}
		}
		public static IsolatedStorageFileStream CreateFile(string filename)
		{
			IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
			return isoStore.CreateFile(filename);
		}

		public static float GetFileSize(string filename, FileSizeUnit unit)
		{
			float fileSize = -1;
			if (Task.Run(async()=> await  FileExists(filename)).Result)
			{
				using (IsolatedStorageFileStream filestream = OpenFile(filename, FileMode.Open, FileAccess.Read))
				{
					fileSize = filestream.Length / (float)unit;
				}
			}
			return fileSize;
		}

		public static IsolatedStorageFileStream OpenFile(string filename, FileMode mode, FileAccess access)
		{
			IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
			return isoStore.OpenFile(filename, mode, access);
		}

		public static bool DirectoryExists(string filename)
		{
			IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
			return isoStore.DirectoryExists(filename);
		}

		public static XDocument ReadFileAsDocument(string filename)
		{
			XDocument xdoc = null;

			IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
			if (isoStore.FileExists(filename))
			{
				IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(filename, FileMode.Open, isoStore);
				if (isoStream.Length > 0)
				{
					xdoc = new XDocument();
					xdoc = XDocument.Load(isoStream);
				}
				isoStream.Close();
			}
			return xdoc;
		}

		public static void SaveFileFromDocument(string filename, XDocument xdoc)
		{
			IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
			IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(filename, FileMode.Create, isoStore);
			xdoc.Save(isoStream);
			isoStream.Close();
		}

#endif
        public async static Task<bool> FileExists(string filename)
        {            
            bool Exists = false;
#if WINDOWS_APP
            var isoStore = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                var FileExists =await  isoStore.GetFileAsync(filename);
                if (FileExists != null)
                    Exists = true;
                    
            }
            catch (Exception ex)
            {
                Exists = false;
            }
         return Exists;
#endif
#if WP8 || ANDROID || WINDOWS_PHONE_APP
            //try
            //{               
            //    IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
            //    Exists = isoStore.FileExists(filename);
            //}
            //catch (Exception ex)
            //{
            //    Exists = false;
            //}
            
            try
            {
                StorageFolder isoStore = ApplicationData.Current.LocalFolder;
                var ss=await isoStore.GetFileAsync(filename);
                Exists = true;
            }
            catch (Exception ex)
            {
                Exists = false;
            }

            return Exists;
#endif
        }
    }
}
