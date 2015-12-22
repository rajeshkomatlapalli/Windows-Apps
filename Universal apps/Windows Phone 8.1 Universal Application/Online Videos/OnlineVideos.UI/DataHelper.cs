#define DEBUG_AGENT
using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Linq;
using OnlineVideos.Library;
using Common.Library;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage.Streams;

namespace OnlineVideos
{
    public class DataHelper
    {
        public void CopyDefaultImage()
        {
            try
            {
                string gg = ResourceHelper.ProjectName;
                if (ResourceHelper.AppName == "Indian_Cinema.WindowsPhone" ||ResourceHelper.AppName=="Indian Cinema" || ResourceHelper.AppName == Apps.Indian_Cinema_Pro.ToString() || ResourceHelper.AppName == Apps.Story_Time.ToString() || ResourceHelper.AppName == Apps.Online_Education.ToString() || ResourceHelper.AppName == Apps.Kids_TV_Shows.ToString() || ResourceHelper.AppName == Apps.Kids_TV_Pro.ToString() || ResourceHelper.AppName == Apps.Kids_TV.ToString() || ResourceHelper.AppName == Apps.Animation_Planet.ToString() || ResourceHelper.AppName == Apps.Bollywood_Movies.ToString() || ResourceHelper.AppName == Apps.Bollywood_Music.ToString() || ResourceHelper.AppName == Apps.World_Dance.ToString() || ResourceHelper.AppName == Apps.Yoga_and_Health.ToString().Replace("and", "&") || ResourceHelper.AppName == Apps.Fitness_Programs.ToString() || ResourceHelper.AppName == Apps.Recipe_Shows.ToString() || ResourceHelper.AppName == Apps.Vedic_Library.ToString() || ResourceHelper.AppName == Apps.DrivingTest.ToString())
                {

                        StorageFile databaseFile = Task.Run(async () => await Package.Current.InstalledLocation.GetFileAsync("Images\\Default.jpg")).Result;
                        StorageFolder sf = ApplicationData.Current.LocalFolder;
                        StorageFile sf1 = Task.Run(async () => await sf.CreateFileAsync("Images\\Default.jpg", CreationCollisionOption.OpenIfExists)).Result;
                        Task.Run(async () => await databaseFile.CopyAndReplaceAsync(sf1));
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in CopyDefaultImage Method In DataHelper.cs file", ex);
            }
        }
        public void MoveReferenceDatabase()
        {

            try
            {
                StorageFile databaseFile1 = Task.Run(async () => await Package.Current.InstalledLocation.GetFileAsync("Claps.mp3")).Result;
                StorageFolder sf2 = ApplicationData.Current.LocalFolder;
                StorageFile sf3 = Task.Run(async () => await sf2.CreateFileAsync("Claps.mp3", CreationCollisionOption.OpenIfExists)).Result;
                Task.Run(async () => await databaseFile1.CopyAndReplaceAsync(sf3));
            }
            catch (Exception ex)
            {
                
               
            }
        }
      
        public void CopyDatabase()
        {
            try
            {
                StorageFile databaseFile = Task.Run(async () => await Package.Current.InstalledLocation.GetFileAsync("OnlineMoviesDb.sqlite")).Result;
                StorageFolder sf = ApplicationData.Current.LocalFolder;
                StorageFile sf1 = Task.Run(async () => await sf.CreateFileAsync("OnlineMoviesDb.sqlite", CreationCollisionOption.OpenIfExists)).Result;
                Task.Run(async () => await databaseFile.CopyAndReplaceAsync(sf1));
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in CopyDatabase Method In DataHelper.cs file", ex);
            }

        }

        public void Deletexml()
        {
            Storage.DeleteFile(ResourceHelper.ProjectName + "/Videos.xml");
            Storage.DeleteFile(ResourceHelper.ProjectName + "/Description.xml");
            Storage.DeleteFile(ResourceHelper.ProjectName + "/PersonProfile.xml");
            Storage.DeleteFile(ResourceHelper.ProjectName + "/TempCast.xml");
            Storage.DeleteFile(ResourceHelper.ProjectName + "/TempRuns.xml");
            Storage.DeleteFile(ResourceHelper.ProjectName + "/Roles.xml");
            Storage.DeleteFile(ResourceHelper.ProjectName + "/Songs.xml");
            Storage.DeleteFile(ResourceHelper.ProjectName + "/Countries.xml");
            Storage.DeleteFile(ResourceHelper.ProjectName + "/Result.xml");
            Storage.DeleteFile(ResourceHelper.ProjectName + "/Catageory.xml");
        }
       

        public  void IsLowMemDevice()
        {
            try
            {
                // Check the working set limit and set the IsLowMemDevice flag accordingly.
               // Int64 result = (Int64)DeviceExtendedProperties.GetValue("ApplicationWorkingSetLimit");

                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                Int64 result = Convert.ToInt64(localSettings.Values["ApplicationWorkingSetLimit"]);
                if (result == null)
                {
                    result =Convert.ToInt64(Guid.NewGuid());
                    localSettings.Values["ApplicationWorkingSetLimit"] = Convert.ToInt64(result);
                }
                
                if (result < 94371840L)
                Storage.IsLowMemDevice = true;
                else
                  Storage.IsLowMemDevice= false;
            }
            catch (ArgumentOutOfRangeException)
            {
                // Windows Phone OS update not installed, which indicates a 512-MB device. 
                Storage.IsLowMemDevice= false;
            }
        }

        public void DeleteImageFolder()
        {
            try
            {
                StorageFolder mv = Task.Run(async () => await ApplicationData.Current.LocalFolder.GetFolderAsync("Images")).Result;
                Task.Run(async () => await mv.DeleteAsync());
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in DeleteImageFolder Method In DataHelper.cs file", ex);
            }
        }

        public void SaveHelp()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation as StorageFolder;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"DefaultData\Help.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"DefaultData\Help.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
                var fquery1 = Task.Run(async () => await file1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                var outputStream = fquery1.GetOutputStreamAt(0);
                var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                writer.WriteString(Data);
                var fi = Task.Run(async () => await writer.StoreAsync()).Result;
                writer.DetachStream();
                var oi = Task.Run(async () => await outputStream.FlushAsync()).Result;
                outputStream.Dispose();
                fquery1.Dispose();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SaveHelp Method In DataHelper.cs file", ex);
            }
        }

        public void SaveHelpMenu()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation as StorageFolder;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"DefaultData\HelpMenu.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"DefaultData\HelpMenu.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
                var fquery1 = Task.Run(async () => await file1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                var outputStream = fquery1.GetOutputStreamAt(0);
                var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                writer.WriteString(Data);
                var fi = Task.Run(async () => await writer.StoreAsync()).Result;
                writer.DetachStream();
                var oi = Task.Run(async () => await outputStream.FlushAsync()).Result;
                outputStream.Dispose();
                fquery1.Dispose();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SaveHelpMenu Method In DataHelper.cs file", ex);
            }
        }
        public void SaveContactUs()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation as StorageFolder;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"XmlData\ContactUs.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\ContactUs.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
                var fquery1 = Task.Run(async () => await file1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                var outputStream = fquery1.GetOutputStreamAt(0);
                var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                writer.WriteString(Data);
                var fi = Task.Run(async () => await writer.StoreAsync()).Result;
                writer.DetachStream();
                var oi = Task.Run(async () => await outputStream.FlushAsync()).Result;
                outputStream.Dispose();
                fquery1.Dispose();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SaveContactUs Method In DataHelper.cs file", ex);
            }

        }


    }
}

