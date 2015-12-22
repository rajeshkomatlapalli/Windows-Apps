using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
#if NOTANDROID || WINDOWS_PHONE_APP
using Windows.Storage;
using Windows.Storage.Streams;
#endif
using System.Reflection;
using System.Collections;

namespace Common.Library
{
    public enum StorageType
    {
        Roaming, Local, Temporary
    }
    public class StorageHelper<T>
    {
		#if NOTANDROID
        private DataContractSerializer Dserializer;
        private ApplicationData appData = Windows.Storage.ApplicationData.Current;
        private XmlSerializer serializer;
        private StorageType storageType;
        public StorageHelper(StorageType StorageType)
        {
            if (typeof(T).Name.Contains("Dictionary"))
            {
                Dserializer = new DataContractSerializer(typeof(T));
            }
            else
            {
                try
                {
                    serializer = new XmlSerializer(typeof(T));
                }
                catch (Exception ex)
                {
                }
            }
            storageType = StorageType;
        }
        public T ConvertXelementToEntity(XElement val)
        {
            try
            {
                serializer = new XmlSerializer(typeof(T));
                System.IO.MemoryStream ms1 = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(val.ToString()));
                var ss = (T)serializer.Deserialize(ms1);
                ms1.Dispose();
                return ss;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
      
        public async void DeleteASync(string FileName)
        {
            FileName = FileName + ".xml";
            try
            {
                StorageFolder folder = GetFolder(storageType);

                var file = await GetFileIfExistsAsync(folder, FileName);
                if (file != null)
                {
                    await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }
            }
            catch (Exception)
            {
                
            }
        }
        public async Task<bool> SaveDictionaryASync(T Obj, string FileName)
        {
            FileName = FileName + ".xml";
            try
            {
                if (Obj != null)
                {
                    StorageFile file = null;
                    StorageFolder folder = GetFolder(storageType);
                    file = await folder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
                    IRandomAccessStream writeStream = await file.OpenAsync(FileAccessMode.ReadWrite);
                    Stream outStream = Task.Run(() => writeStream.AsStreamForWrite()).Result;
                    Dserializer.WriteObject(outStream, Obj);
                    var oi = Task.Run(async () => await writeStream.FlushAsync()).Result;
                    writeStream.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> SaveASync(T Obj, string FileName)
        {

            FileName = FileName + ".xml";

            try
            {


                StorageFile file = null;
                StorageFolder folder = GetFolder(storageType);
                file = await folder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
                IRandomAccessStream writeStream = await file.OpenAsync(FileAccessMode.ReadWrite);
                Stream outStream = Task.Run(() => writeStream.AsStreamForWrite()).Result;
                serializer.Serialize(outStream, Obj);
                var oi = Task.Run(async () => await writeStream.FlushAsync()).Result;
                writeStream.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<T> LoadDictionaryASync(string FileName)
        {
            FileName = FileName + ".xml";
            try
            {
                T result=default(T);
                StorageFile file = null;
                StorageFolder folder = GetFolder(storageType);
                file = await folder.CreateFileAsync(FileName,CreationCollisionOption.OpenIfExists);
                var readStream = Task.Run(async () => await file.OpenAsync(FileAccessMode.ReadWrite)).Result;
                Stream inStream = Task.Run(() => readStream.AsStreamForRead()).Result;
                if(inStream.Length>0)
                 result = (T)Dserializer.ReadObject(inStream);
                await inStream.FlushAsync();
                inStream.Dispose();
                readStream.Dispose();
                if (Convert.ToInt32(result.GetType().GetRuntimeProperty("Count").GetValue(result)) > 0)
                    return result;
                else
                    return default(T);
            }
            catch (FileNotFoundException)
            {
                return (T)Activator.CreateInstance(typeof(T));               
            }
            catch (Exception ex)
            {               
                return default(T);
            }
        }
        public async Task<T> LoadASync(string FileName)
        {
            FileName = FileName + ".xml";
            try
            {
                T result = (T)Activator.CreateInstance(typeof(T));
                StorageFile file = null;
                StorageFolder folder = GetFolder(storageType);
                file = await folder.CreateFileAsync(FileName,CreationCollisionOption.OpenIfExists);
                var readStream = Task.Run(async () => await file.OpenAsync(FileAccessMode.ReadWrite)).Result;
                Stream inStream = Task.Run(() => readStream.AsStreamForRead()).Result;
                if (inStream.Length>0)
                 result = (T)serializer.Deserialize(inStream);
                await inStream.FlushAsync();
                inStream.Dispose();                           
                readStream.Dispose();
                if (Convert.ToInt32(result.GetType().GetRuntimeProperty("Count").GetValue(result)) > 0)
                    return result;
                else
                    return result;
            }
            catch (FileNotFoundException)
            {               
                return default(T);
            }
            catch (Exception ex)
            {               
                return default(T);
            }
        }

        private StorageFolder GetFolder(StorageType storageType)
        {
            StorageFolder folder;
            switch (storageType)
            {
                case StorageType.Roaming:
                    folder = appData.RoamingFolder;
                    break;
                case StorageType.Local:
                    folder = appData.LocalFolder;
                    break;
                case StorageType.Temporary:
                    folder = appData.TemporaryFolder;
                    break;
                default:
                    throw new Exception(String.Format("Unknown StorageType: {0}", storageType));
            }
            return folder;
        }

        private async Task<StorageFile> GetFileIfExistsAsync(StorageFolder folder, string fileName)
        {
            try
            {
                return await folder.GetFileAsync(fileName);
            }
            catch
            {
                return null;
            }
        }
#endif
		#if ANDROID
		private XmlSerializer serializer;
		public T ConvertXelementToEntity(XElement val)
		{
			serializer = new XmlSerializer(typeof(T));
			System.IO.MemoryStream ms1 = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(val.ToString()));
			return (T)serializer.Deserialize(ms1);
		}
		#endif
    }
}
