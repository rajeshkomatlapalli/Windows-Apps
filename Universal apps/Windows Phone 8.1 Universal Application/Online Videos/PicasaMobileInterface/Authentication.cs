/* 
    Author: Manuel Conti
    WebSite : www.sintax.org
*/

using System;
using System.Net;
using System.Windows;
using System.Text;
using System.IO;
using System.Windows.Input;
using Common.Library;
using Windows.Storage;
using System.Threading.Tasks;

namespace PicasaMobileInterface
{
    public class Authentication
    {

        private static string _password = "{17953F82-41EA-4439-A355-D024C604ED41}";
        private static byte[] _salt = Encoding.UTF8.GetBytes("o6806642kbM7c5");
        private static string _filePath = "PicasaReader\\credential.txt";
        private static string _directoryName = "PicasaReader";
        private static char _separator = '§';

        public static void GetCredential(out string UserName, out string Password)
        {
            UserName = String.Empty;
            Password = String.Empty;

            //IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
            StorageFolder store = ApplicationData.Current.LocalFolder;
            StreamReader file = null;

            try
            {
              //  file = new StreamReader(new IsolatedStorageFileStream(_filePath, FileMode.Open, store));

                StorageFolder isoStore = ApplicationData.Current.LocalFolder;
                var fs= Task.Run(() =>  ApplicationData.Current.LocalFolder.GetFileAsync(_filePath));
                //var stream = fs.OpenAsync(FileAccessMode.ReadWrite);

                //string[] rslt = Decrypt(file.ReadToEnd(),_password).Split(_separator);
                //UserName = rslt[0];
                //Password = rslt[1];
                //file.Close();
            }
            catch(Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetCredential Method In Authentication.cs file", ex);
            }
        }
        
        public async static void SaveCredential(string UserName, string Password)
        {
            try
            {
               // string rslt = Encrypt(UserName + _separator + Password, _password);
                //IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
                StorageFolder store = ApplicationData.Current.LocalFolder;

                if (store.GetFolderAsync(_directoryName)==null)
                  await store.CreateFolderAsync(_directoryName);

              //  StreamWriter file = new StreamWriter(new IsolatedStorageFileStream(_filePath, FileMode.OpenOrCreate, store));
                StorageFolder isoStore = ApplicationData.Current.LocalFolder;
                var fs = await ApplicationData.Current.LocalFolder.CreateFileAsync(_filePath);
                var file = fs.OpenAsync(FileAccessMode.ReadWrite);

                //file.WriteLine(rslt);                
                //file.Close();
            }
            catch (Exception ex)
            {                
                Exceptions.SaveOrSendExceptions("Exception in SaveCredential Method In Authentication.cs file", ex);
            }
        }

        public async static void Logout()
        {
            try
            {
                //IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
                StorageFile store = await ApplicationData.Current.LocalFolder.GetFileAsync(_filePath);
                await store.DeleteAsync();
            }
            catch(Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Logout Method In Authentication.cs file", ex);
            }
        }
       
        //private static string Encrypt(string dataToEncrypt, string password)
        //{
        //    AesManaged aes = null;
        //    MemoryStream memoryStream = null;
        //    CryptoStream cryptoStream = null;

        //    try
        //    {
        //        //Generate a Key based on a Password and HMACSHA1 pseudo-random number generator 
        //        Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(password, _salt);

        //        //Create AES algorithm with 256 bit key and 128-bit block size 
        //        aes = new AesManaged();
        //        aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
        //        aes.IV = rfc2898.GetBytes(aes.BlockSize / 8);

        //        //Create Memory and Crypto Streams 
        //        memoryStream = new MemoryStream();
        //        cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

        //        //Encrypt Data 
        //        byte[] data = Encoding.UTF8.GetBytes(dataToEncrypt);
        //        cryptoStream.Write(data, 0, data.Length);
        //        cryptoStream.FlushFinalBlock();

        //        //Return Base 64 String 
        //        return Convert.ToBase64String(memoryStream.ToArray());
        //    }
        //    finally
        //    {
        //        if (cryptoStream != null)
        //            cryptoStream.Close();

        //        if (memoryStream != null)
        //            memoryStream.Close();

        //        if (aes != null)
        //            aes.Clear();
        //    }
        //}

        //private static string Decrypt(string dataToDecrypt, string password)
        //{
        //    AesManaged aes = null;
        //    MemoryStream memoryStream = null;
        //    CryptoStream cryptoStream = null;

        //    try
        //    {
        //        //Generate a Key based on a Password and HMACSHA1 pseudo-random number generator 
        //        Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(password, _salt);

        //        //Create AES algorithm with 256 bit key and 128-bit block size 
        //        aes = new AesManaged();
        //        aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
        //        aes.IV = rfc2898.GetBytes(aes.BlockSize / 8);

        //        //Create Memory and Crypto Streams 
        //        memoryStream = new MemoryStream();
        //        cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);

        //        //Decrypt Data 
        //        byte[] data = Convert.FromBase64String(dataToDecrypt);
        //        cryptoStream.Write(data, 0, data.Length);
        //        cryptoStream.FlushFinalBlock();

        //        //Return Decrypted String 
        //        byte[] decryptBytes = memoryStream.ToArray();
        //        return Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);
        //    }
        //    finally
        //    {
        //        if (cryptoStream != null)
        //            cryptoStream.Close();

        //        if (memoryStream != null)
        //            memoryStream.Close();

        //        if (aes != null)
        //            aes.Clear();
        //    }
        //}
    }
}
