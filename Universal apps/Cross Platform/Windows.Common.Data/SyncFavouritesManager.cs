using ss=Common.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Storage.Streams;
using OnlineVideos.Entities;



namespace Windows.Common.Data
{
  public  class SyncFavouritesManager
    {
      public static void savexml(string linktype,string linkid)
      {
          try
          {
              int sid = int.Parse(ss.AppSettings.ShowID);
              if (Task.Run(async () => await ss.Storage.FavouriteFileExists("Favourites.xml")).Result)
              {

                  if (checkxml())
                  {
                      updatexml(linktype,linkid);
                  }
                  else
                  {
                      XDocument xdoc = new XDocument();

                      StorageFolder store = ApplicationData.Current.LocalFolder;
                      StorageFolder file = Task.Run(async () => await store.CreateFolderAsync("Favourites", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                      StorageFile file1 = Task.Run(async () => await file.CreateFileAsync("Favourites.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                      var f = Task.Run(async () => await file1.OpenAsync(FileAccessMode.Read)).Result;
                      IInputStream inputStream = f.GetInputStreamAt(0);
                      DataReader dataReader = new DataReader(inputStream);
                      uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
                      string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
                      MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlData));
                      xdoc = XDocument.Load(ms);
                      dataReader.DetachStream();
                      inputStream.Dispose();
                      f.Dispose();
                      ms.Dispose();

                     
                              xdoc.Root.Add(
                                  new XElement("Show",
                                                                new XAttribute("id", ss.AppSettings.ShowID),
                                                                new XElement(linktype, "1",
                                                                              new XAttribute("no", linkid))
                              )
                              );
                         


                      StorageFolder store1 = ApplicationData.Current.LocalFolder;

                      StorageFolder file4 = Task.Run(async () => await store.CreateFolderAsync("Favourites", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                      StorageFile file5 = Task.Run(async () => await file4.CreateFileAsync("Favourites.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                      var fquery1 = Task.Run(async () => await file5.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                      var outputStream = fquery1.GetOutputStreamAt(0);
                      var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                      StringBuilder sb = new StringBuilder();
                      TextWriter tx = new StringWriter(sb);
                      xdoc.Save(tx);
                      string text = tx.ToString();
                      text = text.Replace("utf-16", "utf-8");
                      writer.WriteString(text);
                      var fi = Task.Run(async () => await writer.StoreAsync()).Result;
                      writer.DetachStream();
                      var oi = Task.Run(async () => await outputStream.FlushAsync()).Result;
                      outputStream.Dispose();
                      fquery1.Dispose();

                  }
              }
              else
              {
                  XDocument xdoc = new XDocument();
                  
                      
                          xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                                                                    new XElement("Favourites",
                                                                new XElement("Show",
                                                                             new XAttribute("id", ss.AppSettings.ShowID),
                                                                             new XElement(linktype, "1",
                                                                                 new XAttribute("no", linkid))
                          )));

                     
                

                  StorageFolder store1 = ApplicationData.Current.LocalFolder;

                  StorageFolder file4 = Task.Run(async () => await store1.CreateFolderAsync("Favourites", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                  StorageFile file5 = Task.Run(async () => await file4.CreateFileAsync("Favourites.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                  var fquery1 = Task.Run(async () => await file5.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                  var outputStream = fquery1.GetOutputStreamAt(0);
                  var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                  StringBuilder sb = new StringBuilder();
                  TextWriter tx = new StringWriter(sb);
                  xdoc.Save(tx);
                  string text = tx.ToString();
                  text = text.Replace("utf-16", "utf-8");
                  writer.WriteString(text);
                  var fi = Task.Run(async () => await writer.StoreAsync()).Result;
                  writer.DetachStream();
                  var oi = Task.Run(async () => await outputStream.FlushAsync()).Result;
                  outputStream.Dispose();
                  fquery1.Dispose();

              }
          }
          catch (Exception ex)
          {
              ss.Exceptions.SaveOrSendExceptions("Exception in Savexml Method In FavoriteManager.cs file", ex);
          }
      }

      public static bool checkxml()
      {
          bool exists = false;
          try
          {
              XDocument xdoc = new XDocument();

             
                  StorageFolder store = ApplicationData.Current.LocalFolder;
                  StorageFolder file = Task.Run(async () => await store.CreateFolderAsync("Favourites", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                  StorageFile file1 = Task.Run(async () => await file.CreateFileAsync("Favourites.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                  var f = Task.Run(async () => await file1.OpenAsync(FileAccessMode.Read)).Result;
                  IInputStream inputStream = f.GetInputStreamAt(0);
                  DataReader dataReader = new DataReader(inputStream);
                  uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
                  string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
                  MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlData));
                  xdoc = XDocument.Load(ms);
                  dataReader.DetachStream();
                  inputStream.Dispose();
                  f.Dispose();
                  ms.Dispose();

                  var data = (from p in xdoc.Descendants("Favourites").Elements() where p.Attribute("id").Value == ss.AppSettings.ShowID select p).FirstOrDefault();
                  if (data != null)
                      exists = true;
              }
         
          catch (Exception ex)
          {
              ss.Exceptions.SaveOrSendExceptions("Exception in checkxml Method In FavoriteManager.cs file", ex);
          }
          return exists;
      }

      public static void updatexml(string linktype,string linkid)
      {

          try
          {

              int sid = int.Parse(ss.AppSettings.ShowID);
              XDocument xdoc = new XDocument();

              StorageFolder store = ApplicationData.Current.LocalFolder;

              StorageFolder file = Task.Run(async () => await store.CreateFolderAsync("Favourites", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
              StorageFile file1 = Task.Run(async () => await file.CreateFileAsync("Favourites.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
              var f = Task.Run(async () => await file1.OpenAsync(FileAccessMode.Read)).Result;
              IInputStream inputStream = f.GetInputStreamAt(0);
              DataReader dataReader = new DataReader(inputStream);
              uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
              string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
              MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlData));
              xdoc = XDocument.Load(ms);
              dataReader.DetachStream();
              inputStream.Dispose();
              f.Dispose();
              ms.Dispose();

              var data = default(List<XElement>);
             
                  data = xdoc.Root.Elements("Show").Where(i => i.Attribute("id").Value == ss.AppSettings.ShowID.ToString() && i.Element(linktype) != null).Elements(linktype).Where(i => i.Attribute("no").Value == linkid).ToList();
             
              if (data.Count() > 0)
              {


                  data[0].Value = (data[0].Value == "0" ? "1" : "0");




              }
              else
              {
                  XElement data1 = (from p in xdoc.Descendants("Favourites").Elements() where p.Attribute("id").Value == ss.AppSettings.ShowID select p).FirstOrDefault();
                 
                      data1.Add(new XElement(linktype, "1",
                                                               new XAttribute("no", linkid)));
                  

              }

              StorageFolder store2 = ApplicationData.Current.LocalFolder;
              StorageFolder file2 = Task.Run(async () => await store.CreateFolderAsync("Favourites", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
              StorageFile file3 = Task.Run(async () => await file2.CreateFileAsync("Favourites.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
              var fquery2 = Task.Run(async () => await file3.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
              var outputStream2 = fquery2.GetOutputStreamAt(0);
              var writer2 = new Windows.Storage.Streams.DataWriter(outputStream2);
              StringBuilder sb2 = new StringBuilder();
              TextWriter tx2 = new StringWriter(sb2);
              xdoc.Save(tx2);
              string text = tx2.ToString();
              text = text.Replace("utf-16", "utf-8");
              writer2.WriteString(text);
              var fi = Task.Run(async () => await writer2.StoreAsync()).Result;
              writer2.DetachStream();
              var oi = Task.Run(async () => await outputStream2.FlushAsync()).Result;
              outputStream2.Dispose();
              fquery2.Dispose();

          }
          catch (Exception ex)
          {
              ss.Exceptions.SaveOrSendExceptions("Exception in Updatexml Method In FavoriteManager.cs file", ex);
          }
      }
		
    }
}
