using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Library;
//using Common.Library;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;

namespace OnlineVideos.UI
{
   public class DataHelper
    {
       public void CopyDefaultImage()
       {
           try
           {
               string gg1 = AppSettings.ProjectName;
               string gg = ResourceHelper.ProjectName;
               if (ResourceHelper.AppName == "Indian_Cinema.Windows" || ResourceHelper.AppName == Apps.Story_Time.ToString() || ResourceHelper.AppName == Apps.Online_Education.ToString() || ResourceHelper.AppName == Apps.Kids_TV_Shows.ToString() || ResourceHelper.AppName == "Yoga_&_Health" || ResourceHelper.AppName == Apps.Fitness_Programs.ToString() || ResourceHelper.AppName == Apps.World_Dance.ToString() || ResourceHelper.AppName == Apps.Story_Time_Pro.ToString() || ResourceHelper.AppName == Apps.Kids_TV_Pro.ToString() || ResourceHelper.AppName == Apps.Vedic_Library.ToString() || ResourceHelper.AppName == Apps.Driving_Exam.ToString() || ResourceHelper.AppName == Apps.Animation_Planet.ToString() || ResourceHelper.AppName == Apps.Bollywood_Music.ToString() || ResourceHelper.AppName == Apps.Recipe_Shows.ToString() || ResourceHelper.AppName == Apps.Video_Games.ToString() || ResourceHelper.AppName == Apps.Cricket_Videos.ToString())
               {
                   List<string> ImageDic = new List<string>();
                   ImageDic.Add("scale-100");
                   ImageDic.Add("scale-140");
                   ImageDic.Add("scale-180");
                   ImageDic.Add("ListImages");
                   ImageDic.Add("TileImages\\30-30");
                   ImageDic.Add("TileImages\\150-150");
                   foreach (string foldername in ImageDic)
                   {
                       StorageFile databaseFile = Task.Run(async () => await Package.Current.InstalledLocation.GetFileAsync("Images\\" + foldername.ToString()+"\\"+"Default.jpg")).Result;
                       StorageFolder sf = ApplicationData.Current.LocalFolder;
                       StorageFile sf1 = Task.Run(async () => await sf.CreateFileAsync("Images\\" + foldername.ToString() + "\\" + "Default.jpg", CreationCollisionOption.OpenIfExists)).Result;
                       Task.Run(async () => await databaseFile.CopyAndReplaceAsync(sf1));
                   }
               }
               if (AppSettings.ProjectName == "Video Mix")
               {
                     List<string> ImageDic = new List<string>();
                     ImageDic.Add("Images");
                   ImageDic.Add("Images\\scale-100");
                   foreach (string foldername in ImageDic)
                   {
                       StorageFile databaseFile = Task.Run(async () => await Package.Current.InstalledLocation.GetFileAsync(foldername.ToString() + "\\" + "Vlogo.jpg")).Result;
                       StorageFolder sf = ApplicationData.Current.LocalFolder;
                       StorageFile sf1 = Task.Run(async () => await sf.CreateFileAsync(foldername.ToString() + "\\" + "Vlogo.jpg", CreationCollisionOption.OpenIfExists)).Result;
                       Task.Run(async () => await databaseFile.CopyAndReplaceAsync(sf1));
                   }
               }
           }
           catch (Exception ex)
           {
               Exceptions.SaveOrSendExceptions("Exception in CopyDefaultImage Method In DataHelper.cs file", ex);
           }
       }
       public void CopyDatabase()
       {
          try
           {
               StorageFile databaseFile =Task.Run(async()=> await Package.Current.InstalledLocation.GetFileAsync("OnlineMoviesDb.sqlite")).Result;
               StorageFolder sf = ApplicationData.Current.LocalFolder;
               StorageFile sf1 = Task.Run(async()=>await sf.CreateFileAsync("OnlineMoviesDb.sqlite", CreationCollisionOption.OpenIfExists)).Result;
               Task.Run(async()=>  await databaseFile.CopyAndReplaceAsync(sf1));
           }
           catch (Exception ex)
           {
              Exceptions.SaveOrSendExceptions("Exception in CopyDatabase Method In DataHelper.cs file", ex);
           }
       }

       public  void DeleteImageFolder()
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
        public  void SaveCastProfile()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation as StorageFolder;
                StorageFile file = Task.Run(async()=>await  store.CreateFileAsync(@"XmlData\CastProfile.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async()=>await  file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async()=>await  dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async()=>await store1.CreateFileAsync(@"XmlData\CastProfile.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
                var fquery1 = Task.Run(async()=>await file1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                var outputStream = fquery1.GetOutputStreamAt(0);
                var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                writer.WriteString(Data);
                var fi = Task.Run(async()=>await writer.StoreAsync()).Result;
                writer.DetachStream();
                var oi = Task.Run(async()=>await outputStream.FlushAsync()).Result;
                outputStream.Dispose();
                fquery1.Dispose();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SaveCastProfile Method In DataHelper.cs file", ex);
            }
        }
        public void SaveMatchBattingScore()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation as StorageFolder;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"XmlData\MatchBattingScore.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\MatchBattingScore.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
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
                Exceptions.SaveOrSendExceptions("Exception in SaveMatchBattingScore Method In DataHelper.cs file", ex);
            }
        }

        public void SavMatchBowlingScore()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation as StorageFolder;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"XmlData\MatchBowlingScore.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\MatchBowlingScore.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
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
                Exceptions.SaveOrSendExceptions("Exception in SavMatchBowlingScore Method In DataHelper.cs file", ex);
            }
        }
        public void SaveMatchExtras()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation as StorageFolder;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"XmlData\MatchExtras.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\MatchExtras.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
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
                Exceptions.SaveOrSendExceptions("Exception in SaveMatchExtras Method In DataHelper.cs file", ex);
            }

        }
        public void SaveShowCategories()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation as StorageFolder;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"XmlData\ShowCategories.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\ShowCategories.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
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
                Exceptions.SaveOrSendExceptions("Exception in SaveShowCategories Method In DataHelper.cs file", ex);
            }
        }
        public  void SaveCastRoles()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation as StorageFolder;
                StorageFile file = Task.Run(async()=>await store.CreateFileAsync(@"XmlData\CastRoles.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async()=>await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async()=>await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\CastRoles.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result ;
                var fquery1 = Task.Run(async()=>await file1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                var outputStream = fquery1.GetOutputStreamAt(0);
                var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                writer.WriteString(Data);
                var fi = Task.Run(async()=>await writer.StoreAsync()).Result;
                writer.DetachStream();
                var oi = Task.Run(async()=>await outputStream.FlushAsync()).Result;
                outputStream.Dispose();
                fquery1.Dispose();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SaveCastRoles Method In DataHelper.cs file", ex);
            }
        }

        public  void SaveCategoriesByShowID()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation as StorageFolder;
                StorageFile file = Task.Run(async()=>await store.CreateFileAsync(@"XmlData\CategoriesByShowID.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async()=>await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async()=>await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\CategoriesByShowID.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
                var fquery1 = Task.Run(async()=>await file1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                var outputStream = fquery1.GetOutputStreamAt(0);
                var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                writer.WriteString(Data);
                var fi = Task.Run(async()=>await writer.StoreAsync()).Result;
                writer.DetachStream();
                var oi = Task.Run(async()=>await outputStream.FlushAsync()).Result;
                outputStream.Dispose();
                fquery1.Dispose();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SaveCategoriesByShowID Method In DataHelper.cs file", ex);
            }
        }

        public  void SaveShowCast()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation;
                StorageFile file = Task.Run(async()=>await store.CreateFileAsync(@"XmlData\ShowCast.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async()=>await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async()=>await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async()=>await store1.CreateFileAsync(@"XmlData\ShowCast.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
                var fquery1 = Task.Run(async () => await file1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                var outputStream = fquery1.GetOutputStreamAt(0);
                var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                writer.WriteString(Data);
                var fi = Task.Run(async()=>await writer.StoreAsync()).Result;
                writer.DetachStream();
                var oi = Task.Run(async()=>await outputStream.FlushAsync()).Result;
                outputStream.Dispose();
                fquery1.Dispose();

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SaveShowCast Method In DataHelper.cs file", ex);
            }
        }

        public  void SaveShowLinks()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"XmlData\ShowLinks.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\ShowLinks.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
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
                Exceptions.SaveOrSendExceptions("Exception in SaveShowLinks Method In DataHelper.cs file", ex);
            }
        }
        public  void SaveShowList()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"XmlData\ShowList.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\ShowList.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
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
                Exceptions.SaveOrSendExceptions("Exception in SaveShowList Method In DataHelper.cs file", ex);
            }
        }

        public void SaveQuiz()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation as StorageFolder;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"XmlData\QuizList.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\QuizList.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
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
                Exceptions.SaveOrSendExceptions("Exception in SaveQuiz Method In DataHelper.cs file", ex);
            }
        }

        public void SaveQuestions()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation as StorageFolder;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"XmlData\QuizQuestions.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\QuizQuestions.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
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
                Exceptions.SaveOrSendExceptions("Exception in SaveQuestions Method In DataHelper.cs file", ex);
            }
        }
        public void SaveCatageoryTable()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation as StorageFolder;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"XmlData\CatageoryTable.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\CatageoryTable.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
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
                Exceptions.SaveOrSendExceptions("Exception in SaveCatageoryTable Method In DataHelper.cs file", ex);
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


        public void SavePersonGallery()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"XmlData\PersonGallery.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\PersonGallery.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
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
                Exceptions.SaveOrSendExceptions("Exception in SavePersonGallery Method In DataHelper.cs file", ex);
            }
        }
        public void SaveStory()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation as StorageFolder;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"XmlData\Stories.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\Stories.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
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
                Exceptions.SaveOrSendExceptions("Exception in SaveStory Method In DataHelper.cs file", ex);
            }
        }
        public void SaveStoryLanguage()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation as StorageFolder;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"XmlData\StoryLanguage.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\StoryLanguage.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
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
                Exceptions.SaveOrSendExceptions("Exception in SaveStoryLanguage Method In DataHelper.cs file", ex);
            }
        }


        public void SaveGameWeapons()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"XmlData\GameWeapons.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\GameWeapons.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
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
                Exceptions.SaveOrSendExceptions("Exception in SaveGameWeapons Method In DataHelper.cs file", ex);
            }
        }

        public void SaveGameVehicles()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"XmlData\GameVehicles.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\GameVehicles.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
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
                Exceptions.SaveOrSendExceptions("Exception in SaveGameVehicles Method In DataHelper.cs file", ex);
            }
        }

        public void SaveGameMisions()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"XmlData\GameMissions.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\GameMissions.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
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
                Exceptions.SaveOrSendExceptions("Exception in SaveGameMisions Method In DataHelper.cs file", ex);
            }
        }

        public void SaveGameAchievement()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"XmlData\GameAchievement.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\GameAchievement.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
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
                Exceptions.SaveOrSendExceptions("Exception in SaveGameAchievement Method In DataHelper.cs file", ex);
            }
        }

        public void SaveGameCheats()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"XmlData\GameCheats.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\GameCheats.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
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
                Exceptions.SaveOrSendExceptions("Exception in SaveGameCheats Method In DataHelper.cs file", ex);
            }
        }

        public void SaveGameControls()
        {
            try
            {
                StorageFolder store = Package.Current.InstalledLocation;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"XmlData\GameControls.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var fquery = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = fquery.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)fquery.Size)).Result;
                string Data = (dataReader.ReadString(numBytesLoaded)).ToString();
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\GameControls.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
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
                Exceptions.SaveOrSendExceptions("Exception in SaveGameControls Method In DataHelper.cs file", ex);
            }
        }
    }
}
