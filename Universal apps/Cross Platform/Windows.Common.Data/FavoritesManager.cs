using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Collections.Generic;
using OnlineVideos.Data;
using OnlineVideos.Entities;
#if NOTANDROID
using Windows.Storage;
using Windows.Storage.Streams;
#endif
using System.Xml.Linq;
using System.Threading.Tasks;
using Common.Library;
using Common;
using System.Text;
using System.IO;
using Windows.Common.Data;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;
#if WP8 && NOTANDROID
using System.Windows.Media.Imaging;
using Windows.Common.Data;
#endif
#if WINDOWS_APP
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

#endif
namespace OnlineVideos.Data
{
    public static class FavoritesManager
    {
#if ANDROID
		public static string path = Path.Combine (System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal), "Favourites.xml");
#endif
        private static void savexml(string linktype)
        {
            try
            {
                int sid = int.Parse(AppSettings.ShowID);
#if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
                if (Task.Run(async () => await Storage.FavFileExists("Favourites.xml")).Result)
#endif
#if ANDROID
				if (File.Exists (path))
#endif
                {
                    if (checkxml())
                    {
                        updatexml(linktype);
                    }
                    else
                    {
                        XDocument xdoc = new XDocument();
#if ANDROID
						xdoc = XDocument.Load (path);
#endif
#if NOTANDROID
                        StorageFolder store = ApplicationData.Current.LocalFolder;
                        StorageFolder file = Task.Run(async () => await store.CreateFolderAsync(AppSettings.ProjectName, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                        StorageFile file1 = Task.Run(async () => await file.CreateFileAsync("Favourites.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                        var f = Task.Run(async () => await file1.OpenAsync(FileAccessMode.Read)).Result;
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
#endif
                        if (AppSettings.ProjectName == "Video Games" || linktype == "Movies")
                        {
                            xdoc.Root.Add(
                                new XElement("Show",
                                                             new XAttribute("id", AppSettings.ShowID),
                                                             new XElement(linktype, "1",
                                                                           new XAttribute("no", (linktype == "Movies" && Constants.selecteditem == null) ? 0 : Constants.selecteditem.LinkOrder))
                            )
                            );
                        }
                        else
                        {
                            if (linktype == "Movies")
                            {
                                xdoc.Root.Add(
                                    new XElement("Show",
                                                                    new XAttribute("id", AppSettings.ShowID),
                                                                    new XElement(linktype, "1",
                                                                                  new XAttribute("no", (linktype == "Movies" && Constants.selecteditem == null) ? Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == sid).FirstOrDefaultAsync()).Result.LinkOrder : Constants.selecteditem.LinkOrder))
                                )
                                );
                            }
                            else
                            {
                                xdoc.Root.Add(
                                    new XElement("Show",
                                                                  new XAttribute("id", AppSettings.ShowID),
                                                                  new XElement(linktype, "1",
                                                                                new XAttribute("no", (linktype == "Movies" && Constants.selecteditem == null) ? Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == sid && i.LinkType == "Movies").FirstOrDefaultAsync()).Result.LinkOrder : Constants.selecteditem.LinkOrder))
                                )
                                );
                            }
                        }
#if NOTANDROID
                        StorageFolder store1 = ApplicationData.Current.LocalFolder;

                        StorageFolder file4 = Task.Run(async () => await store.CreateFolderAsync(AppSettings.ProjectName, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
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
#endif
#if ANDROID
						xdoc.Save (path);
#endif
                    }
                }
                else
                {
                    XDocument xdoc = new XDocument();
                    if (AppSettings.ProjectName == "Video Games" || linktype == "Movies")
                    {
                        xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                                                               new XElement("Favourites",
                                                            new XElement("Show",
                                                                         new XAttribute("id", AppSettings.ShowID),
                                                                         new XElement(linktype, "1",
                                                                             new XAttribute("no", (linktype == "Movies" && Constants.selecteditem == null) ? 0 : Constants.selecteditem.LinkOrder))
                        )));
                    }
                    else
                    {
                        if (linktype == "Movies")
                        {
                            xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                                                                      new XElement("Favourites",
                                                            new XElement("Show",
                                                                         new XAttribute("id", AppSettings.ShowID),
                                                                         new XElement(linktype, "1",
                                                                             new XAttribute("no", (linktype == "Movies" && Constants.selecteditem == null) ? Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == sid).FirstOrDefaultAsync()).Result.LinkOrder : Constants.selecteditem.LinkOrder))
                            )));
                        }
                        else
                        {
                            xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                                                                      new XElement("Favourites",
                                                                  new XElement("Show",
                                                                               new XAttribute("id", AppSettings.ShowID),
                                                                               new XElement(linktype, "1",
                                                                                   new XAttribute("no", (linktype == "Movies" && Constants.selecteditem == null) ? Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == sid && i.LinkType == "Movies").FirstOrDefaultAsync()).Result.LinkOrder : Constants.selecteditem.LinkOrder))
                            )));

                        }
                    }

#if NOTANDROID
                    StorageFolder store1 = ApplicationData.Current.LocalFolder;

                    StorageFolder file4 = Task.Run(async () => await store1.CreateFolderAsync(AppSettings.ProjectName, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
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
#endif
#if ANDROID
					xdoc.Save (path);
#endif
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Savexml Method In FavoriteManager.cs file", ex);
            }
        }

        public async static void AddOrRemoveFavorite(int ShowID, int LinkID)
        {
            try
            {
                var selectedRow = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == ShowID && i.LinkID == LinkID).ToListAsync()).Result;
                ShowLinks selectedLink = selectedRow.FirstOrDefault();
                if (selectedLink != null)
                {
                    if (selectedLink.IsFavourite == false)
                    {
                        selectedLink.IsFavourite = true;
                    }
                    else
                    {
                        selectedLink.IsFavourite = false;
                    }
                    selectedLink.ClientShowUpdated = DateTime.Now;

                    await Constants.connection.UpdateAsync(selectedLink);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Show ID", ShowID);
                ex.Data.Add("Link ID", LinkID);
                Exceptions.SaveOrSendExceptions("Exception in SaveFavoriteSongs Method In OnlineShow.cs file", ex);
            }
        }

        public static void QuizAddOrRemoveFavorite(int ShowID, int Sno)
        {
            try
            {
                var selectedRow = Task.Run(async () => await Constants.connection.Table<QuizList>().Where(i => i.ShowID == ShowID && i.QuizID == Sno).ToListAsync()).Result;
                QuizList selectedLink = selectedRow.FirstOrDefault();
                if (selectedLink != null)
                {
                    if (selectedLink.IsFavourite == false)
                    {
                        selectedLink.IsFavourite = true;
                    }
                    else
                    {
                        selectedLink.IsFavourite = false;
                    }

                    Task.Run(async () => await Constants.connection.UpdateAsync(selectedLink));
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Show ID", ShowID);
                ex.Data.Add("Link ID", Sno);
                Exceptions.SaveOrSendExceptions("Exception in SaveFavoriteSongs Method In OnlineShow.cs file", ex);
            }
        }

        public static void RemoveFavoriteMovies()
        {
            try
            {
                XDocument xdoc = new XDocument();
#if ANDROID
				xdoc = XDocument.Load (path);
#endif
#if NOTANDROID
                StorageFolder store = ApplicationData.Current.LocalFolder;

                StorageFolder file = Task.Run(async () => await store.CreateFolderAsync(AppSettings.ProjectName, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                StorageFile file1 = Task.Run(async () => await file.CreateFileAsync("Favourites.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var f = Task.Run(async () => await file1.OpenAsync(FileAccessMode.Read)).Result;
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
#endif
                xdoc.Root.Elements("Show").Where(i => i.Attribute("id").Value == AppSettings.ShowID.ToString()).Elements().Where(i => i.Name == "Movies").ElementAt(0).Remove();


#if NOTANDROID
                StorageFolder store2 = ApplicationData.Current.LocalFolder;
                StorageFolder file2 = Task.Run(async () => await store2.CreateFolderAsync(AppSettings.ProjectName, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
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
#endif
#if ANDROID
				xdoc.Save (path);
#endif
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in RemoveFavoriteMovies Method In FavoriteManager.cs file", ex);
            }
        }

        public static void RemoveFavoriteSongs(string linktype)
        {
            try
            {
                XDocument xdoc = new XDocument();
#if ANDROID
				xdoc = XDocument.Load (path);
#endif
#if NOTANDROID
                StorageFolder store = ApplicationData.Current.LocalFolder;

                StorageFolder file = Task.Run(async () => await store.CreateFolderAsync(AppSettings.ProjectName, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                StorageFile file1 = Task.Run(async () => await file.CreateFileAsync("Favourites.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var f = Task.Run(async () => await file1.OpenAsync(FileAccessMode.Read)).Result;
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
#endif
                xdoc.Root.Elements("Show").Where(i => i.Attribute("id").Value == AppSettings.ShowID.ToString()).Elements(linktype).Where(p => p.Attribute("no").Value.ToString() == Constants.selecteditem.LinkOrder.ToString()).ElementAt(0).Remove();

#if NOTANDROID
                StorageFolder store2 = ApplicationData.Current.LocalFolder;
                StorageFolder file2 = Task.Run(async () => await store2.CreateFolderAsync(AppSettings.ProjectName, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
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
#endif
#if ANDROID
				xdoc.Save (path);
#endif
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in RemoveFavoriteSongs Method In FavoriteManager.cs file", ex);
            }
        }

        public static void SaveFavoriteSongs(string linktype)
        {
            try
            {
                savexml(linktype);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SaveFavoriteSongs Method In FavoriteManager.cs file", ex);
            }
        }

        public static bool checkxml()
        {
            bool exists = false;
            try
            {
                XDocument xdoc = new XDocument();
#if NOTANDROID
                if (Task.Run(async () => await Storage.FavFileExists("Favourites.xml")).Result)
                {
                    StorageFolder store = ApplicationData.Current.LocalFolder;
                    StorageFolder file = Task.Run(async () => await store.CreateFolderAsync(AppSettings.ProjectName, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                    StorageFile file1 = Task.Run(async () => await file.CreateFileAsync("Favourites.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                    var f = Task.Run(async () => await file1.OpenAsync(FileAccessMode.Read)).Result;
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
#endif
#if ANDROID
				if (File.Exists (path)) {
					xdoc = XDocument.Load (path);
#endif
                    var data = (from p in xdoc.Descendants("Favourites").Elements() where p.Attribute("id").Value == AppSettings.ShowID select p).FirstOrDefault();
                    if (data != null)
                        exists = true;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in checkxml Method In FavoriteManager.cs file", ex);
            }
            return exists;
        }

        public static void updatexml(string linktype)
        {
            try
            {
                int sid = int.Parse(AppSettings.ShowID);
                XDocument xdoc = new XDocument();
#if NOTANDROID
                StorageFolder store = ApplicationData.Current.LocalFolder;

                StorageFolder file = Task.Run(async () => await store.CreateFolderAsync(AppSettings.ProjectName, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                StorageFile file1 = Task.Run(async () => await file.CreateFileAsync("Favourites.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var f = Task.Run(async () => await file1.OpenAsync(FileAccessMode.Read)).Result;
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
#endif
#if ANDROID
				xdoc = XDocument.Load (path);
#endif
                var data = default(List<XElement>);
                if (AppSettings.ProjectName == "Video Games" || linktype == "Movies")
                {
                    data = xdoc.Root.Elements("Show").Where(i => i.Attribute("id").Value == AppSettings.ShowID.ToString() && i.Element(linktype) != null).Elements(linktype).Where(i => i.Attribute("no").Value == ((Constants.selecteditem == null) ? "0" : Constants.selecteditem.LinkOrder.ToString())).ToList();
                }
                else
                {
                    data = xdoc.Root.Elements("Show").Where(i => i.Attribute("id").Value == AppSettings.ShowID.ToString() && i.Element(linktype) != null).Elements(linktype).Where(i => i.Attribute("no").Value == ((Constants.selecteditem == null) ? Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(j => j.ShowID == sid && j.LinkType == "Movies").FirstOrDefaultAsync()).Result.LinkOrder.ToString() : Constants.selecteditem.LinkOrder.ToString())).ToList();
                }
                if (data.Count() > 0)
                {
                    data[0].Value = (data[0].Value == "0" ? "1" : "0");
                }
                else
                {
                    XElement data1 = (from p in xdoc.Descendants("Favourites").Elements() where p.Attribute("id").Value == AppSettings.ShowID select p).FirstOrDefault();
                    if (AppSettings.ProjectName == "Video Games" || linktype == "Movies")
                    {
                        data1.Add(new XElement(linktype, "1",new XAttribute("no", (linktype == "Movies" && Constants.selecteditem == null) ? 0 : Constants.selecteditem.LinkOrder)));
                    }
                    else
                    {
                        data1.Add(new XElement(linktype, "1",new XAttribute("no", (linktype == "Movies" && Constants.selecteditem == null) ? Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == sid && i.LinkType == "Movies").FirstOrDefaultAsync()).Result.LinkOrder : Constants.selecteditem.LinkOrder)));
                    }
                }
#if NOTANDROID
                StorageFolder store2 = ApplicationData.Current.LocalFolder;
                StorageFolder file2 = Task.Run(async () => await store.CreateFolderAsync(AppSettings.ProjectName, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
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
#endif
#if ANDROID
				xdoc.Save (path);
#endif
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Updatexml Method In FavoriteManager.cs file", ex);
            }
        }

        public static List<ShowLinks> GetFavoriteByType(LinkType link)
        {
            List<ShowLinks> objMovieList1 = new List<ShowLinks>();
            try
            {
                XDocument xdoc = new XDocument();
                string lintype = link.ToString();
#if ANDROID
				if (File.Exists (path)) {
					xdoc = XDocument.Load (path);
#endif
#if NOTANDROID
                if (Task.Run(async () => await Storage.FavFileExists("Favourites.xml")).Result)
                {

                    StorageFolder store = ApplicationData.Current.LocalFolder;
                    StorageFolder file = Task.Run(async () => await store.CreateFolderAsync(AppSettings.ProjectName, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                    StorageFile file1 = Task.Run(async () => await file.CreateFileAsync("Favourites.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                    var f = Task.Run(async () => await file1.OpenAsync(FileAccessMode.Read)).Result;
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
#endif
                    foreach (var fk in xdoc.Descendants("Favourites").FirstOrDefault().Elements("Show"))
                    {
                        foreach (var bb in fk.Elements(lintype).Where(i => i.Value == "1"))
                        {
                            List<ShowLinks> objLinkList = new List<ShowLinks>();
                            int showid = int.Parse(fk.Attribute("id").Value);
                            int linkorder = int.Parse(bb.Attribute("no").Value);
                            objLinkList = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkOrder == linkorder && i.LinkType == lintype).OrderBy(j => j.LinkOrder).ToListAsync()).Result;
                            foreach (ShowLinks sl in objLinkList)
                            {
                                if (lintype != "Audio")
                                {
#if ANDROID
									sl.Thumbnail=  "http://img.youtube.com/vi/" +sl.LinkUrl+ "/default.jpg";
#endif
#if NOTANDROID
                                    if (AppResources.ShowthumbnailTitle == "true")
                                    {
                                        string ShowTitle = OnlineShow.GetMovieTitle(sl.ShowID.ToString());
                                        sl.Title = ShowTitle + "-" + sl.Title;
                                        if (sl.LinkUrl.StartsWith("http://"))
                                        {

                                            sl.ThumbnailImage = sl.LinkUrl.Replace(".wmv", "") + "_512.jpg";
                                        }
                                        else
                                        {
                                            sl.Thumbnail = new BitmapImage(new Uri("http://img.youtube.com/vi/" + sl.LinkUrl + "/default.jpg", UriKind.Absolute));
                                        }
                                    }
                                    else
                                    {
                                        if (sl.LinkUrl.StartsWith("http://"))
                                        {

                                            sl.ThumbnailImage = sl.LinkUrl.Replace(".wmv", "") + "_512.jpg";
                                        }
                                        else
                                        {
                                            sl.Thumbnail = new BitmapImage(new Uri("http://img.youtube.com/vi/" + sl.LinkUrl + "/default.jpg", UriKind.Absolute));
                                        }
                                    }
#endif
                                }
                                objMovieList1.Add(sl);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetFavoriteByType Method In FavoriteManager.cs file", ex);
            }
            return objMovieList1;
        }

        public static void Savemovieasfavorite(string linktype)
        {
            savexml(linktype);
        }

        public static List<ShowList> GetFavoriteByMovie(LinkType type)
        {
            List<ShowList> objMovieList = new List<ShowList>();
            List<ShowList> objMovieList1 = new List<ShowList>();
            try
            {
                XDocument xdoc = new XDocument();
                string lintype = type.ToString();
#if ANDROID
                if (File.Exists(path))
                {
                    xdoc = XDocument.Load(path);
#endif
#if NOTANDROID
                if (Task.Run(async () => await Storage.FavFileExists("Favourites.xml")).Result)
                {

                    StorageFolder store = ApplicationData.Current.LocalFolder;
                    StorageFolder file = Task.Run(async () => await store.CreateFolderAsync(AppSettings.ProjectName, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                    StorageFile file1 = Task.Run(async () => await file.CreateFileAsync("Favourites.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                    var f = Task.Run(async () => await file1.OpenAsync(FileAccessMode.Read)).Result;
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
#endif
                    foreach (var fk in xdoc.Descendants("Favourites").FirstOrDefault().Elements("Show"))
                    {
                        foreach (var bb in fk.Elements(lintype).Where(i => i.Value == "1"))
                        {
                            int showid = int.Parse(fk.Attribute("id").Value);
                            objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).OrderBy(j => j.Rating).ToListAsync()).Result;
                            int count = 1;
                            foreach (var item in objMovieList)
                            {
#if NOTANDROID
                                item.LandingImage = ResourceHelper.getViewAllImageFromStorageOrInstalledFolder(item.TileImage);
                                item.RatingBitmapImage = ImageHelper.LoadRatingImage(item.Rating.ToString());
#endif
#if ANDROID
								try
								{
									string releasedate = item.ReleaseDate;
									string Year = releasedate.Substring(releasedate.LastIndexOf(' ') + 1);
									item.ReleaseDate = Year;
									}
								catch
								{
								}
#endif
                                item.SubTitle = "Subtitle: " + item.SubTitle;
                                objMovieList1.Add(item);
                                count++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetFavoriteByMovie Method In FavoriteManager.cs file", ex);
            }
            return objMovieList1;
        }

        public static List<ShowLinks> GetFavoriteAudioLinks()
        {
            List<ShowLinks> objMovieList = new List<ShowLinks>();
            try
            {
                var ShowLinksList = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.IsFavourite == true && i.LinkType == "Audio").ToListAsync()).Result;
                foreach (ShowLinks song in ShowLinksList)
                {
                    ShowLinks favoriteSong = new ShowLinks();

                    favoriteSong.ShowID = song.ShowID;
                    favoriteSong.LinkID = song.LinkID;
                    string Movietitle = OnlineShow.GetShowDetail(song.ShowID).Title;
                    favoriteSong.Title = Movietitle + " - " + song.Title;
                    favoriteSong.LinkOrder = song.LinkOrder;
                    favoriteSong.LinkUrl = song.LinkUrl;
                    if (AppSettings.SongID == song.LinkID.ToString() && AppSettings.AudioImage == Constants.PlayImagePath)
                        favoriteSong.Songplay = Constants.SongPlayPath;
                    else
                        favoriteSong.Songplay = Constants.PlayImagePath;

                    objMovieList.Add(favoriteSong);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetFavoriteAudioLinks Method ", ex);
            }
            return objMovieList;
        }

        public static List<ShowLinks> GetFavoriteImages()
        {
            List<ShowLinks> objMovieList = new List<ShowLinks>();
            try
            {
                var ShowLinksList = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.IsFavourite == true && i.LinkType == "Images").ToListAsync()).Result;
                foreach (ShowLinks song in ShowLinksList)
                {
                    ShowLinks favoriteSong = new ShowLinks();
                    favoriteSong.ShowID = song.ShowID;
                    favoriteSong.LinkID = song.LinkID;
                    string Movietitle = OnlineShow.GetShowDetail(song.ShowID).Title;
                    favoriteSong.Title = Movietitle + " - " + song.Title;
#if NOTANDROID
                    favoriteSong.DownloadedImage = ResourceHelper.getShowTileImage(song.LinkUrl);
#endif
                    favoriteSong.LinkOrder = song.LinkOrder;
                    favoriteSong.LinkUrl = song.LinkUrl;

                    objMovieList.Add(favoriteSong);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadFavoritesongs Method In Vidoes.cs file", ex);
            }
            return objMovieList;
        }

        public static void SavehideandshowSongs()
        {
            try
            {
                string command = string.Empty;
                int showid = int.Parse(AppSettings.ShowID);
                Constants.selecteditemshowlinkslist = new List<ShowLinks>();
                //Constants.selecteditemshowlinkslist = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == AppSettings.LinkType).OrderBy(j => j.LinkOrder).ToListAsync()).Result; //(datamanager.GetList(i => i.ShowID.ToString() == AppSettings.ShowID && i.LinkType == AppSettings.LinkType.ToString(), j => Convert.ToInt32(j.LinkOrder.ToString()), "R"));
                Constants.selecteditemshowlinkslist = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid).ToListAsync()).Result;
                foreach (ShowLinks links in Constants.selecteditemshowlinkslist)
                {
                    if (links.Title == Constants.selecteditem.Title)
                    {
                        if (links.IsHidden == false)
                        {
                            command = "UPDATE Showlinks SET IsHidden=" + 1 + " " + "WHERE  ShowID=" + links.ShowID + " And   LinkType='" + links.LinkType + "' And  LinkOrder=" + links.LinkOrder;
                        }
                        else
                        {
                            command = "UPDATE Showlinks SET IsHidden=" + 0 + " " + "WHERE  ShowID=" + links.ShowID + " And   LinkType='" + links.LinkType + "' And  LinkOrder=" + links.LinkOrder;
                        }

                        Task.Run(async () => await Constants.connection.QueryAsync<ShowLinks>(command));
                    }
                }
                Constants.selecteditem = null;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SavehideandshowSongs Method In FavoriteManager.cs file", ex);
            }
        }

        public static void Saveandremovemovies()
        {
            string command = string.Empty;
            int showid = int.Parse(AppSettings.ShowID);
            ShowList showlist = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result;
            if (showlist.IsHidden == false)
            {
                command = "UPDATE ShowList SET IsHidden=" + 1 + " " + "WHERE  ShowID=" + showlist.ShowID;
            }
            else
            {
                command = "UPDATE ShowList SET IsHidden=" + 0 + " " + "WHERE  ShowID=" + showlist.ShowID;
            }
            Task.Run(async () => await Constants.connection.QueryAsync<ShowList>(command));
        }

        public static List<ShowList> GetFavoriteShows()
        {
            List<ShowList> objMovieList = null;
            try
            {
                var ShowList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false && i.IsFavourite == true).OrderByDescending(j => j.ShowID).ToListAsync()).Result;
                objMovieList = new List<ShowList>();
                foreach (ShowList item in ShowList)
                {
                    ShowList disprop = new ShowList();
                    disprop.ShowID = Convert.ToInt32(item.ShowID.ToString());
#if NOTANDROID
                    disprop.Image = ResourceHelper.getShowTileImage(item.TileImage);
#endif
                    disprop.Title = item.Title;
                    disprop.Rating = item.Rating;
                    disprop.RatingImage = ImageHelper.LoadRatingImage(item.Rating.ToString()).ToString();
                    if (item.ReleaseDate != null)
                    {
#if WINDOWS_PHONE_APP
                        if (item.ReleaseDate.Contains(','))
                        {
                            disprop.ReleaseDate = item.ReleaseDate.Substring(item.ReleaseDate.LastIndexOf(',') + 1);
                        }
                        else
                        {
                            disprop.ReleaseDate = item.ReleaseDate;
                        }
#endif
                    }
                    disprop.Genre = item.Genre;


                    if (item.SubTitle != null)
                    {
                        disprop.SubTitle = item.SubTitle;
                    }

                    objMovieList.Add(disprop);
                }

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadFavoriteVideos Method In Vidoes.cs file", ex);
            }
            return objMovieList;
        }

        public static void SaveFavoriteQuiz()
        {
            try
            {
                string command = string.Empty;
                int showid = int.Parse(AppSettings.ShowID);
                int quizeid = int.Parse(AppSettings.ShowQuizId);
                List<QuizList> qlist = new List<QuizList>();
                qlist = Task.Run(async () => await Constants.connection.Table<QuizList>().Where(i => i.ShowID == showid && i.QuizID == quizeid).OrderBy(j => j.QuizID).ToListAsync()).Result;
                foreach (QuizList links in qlist)
                {
                    if (links.Name == Constants.Quizselecteditem.Name)
                    {
                        if (links.IsFavourite == false)
                            command = "UPDATE QuizList SET IsFavourite=" + 1 + " " + "WHERE  ShowID=" + links.ShowID + " And   QuizID=" + links.QuizID;
                        else
                            command = "UPDATE QuizList SET IsFavourite=" + 0 + " " + "WHERE  ShowID=" + links.ShowID + " And   QuizID=" + links.QuizID;

                        Task.Run(async () => await Constants.connection.QueryAsync<QuizList>(command));
                    }
                }
                Constants.Quizselecteditem = null;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SaveFavoriteQuiz Method In FavoriteManager.cs file", ex);
            }
        }

        public static List<QuizList> GetFavoritequiz()
        {
            List<QuizList> objMovieList = new List<QuizList>();
            try
            {

                List<QuizList> objLinkList = new List<QuizList>();
                objLinkList = Task.Run(async () => await Constants.connection.Table<QuizList>().Where(i => i.IsFavourite == true).OrderBy(j => j.ShowID).ToListAsync()).Result;
                foreach (QuizList song in objLinkList)
                {
                    QuizList favoriteSong = new QuizList();

                    favoriteSong.ShowID = Convert.ToInt32(song.ShowID);
                    string ShowTitle = OnlineShow.GetMovieTitle(song.ShowID.ToString());
                    favoriteSong.Name = song.Name;
                    favoriteSong.QuizID = Convert.ToInt32(song.QuizID);
                    objMovieList.Add(favoriteSong);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetFavoritequiz Method In FavoriteManager.cs file", ex);
            }
            return objMovieList;
        }

        public static void RemoveFavoriteQuiz()
        {
            try
            {
                string command = string.Empty;
                int showid = int.Parse(AppSettings.ShowID);
                int quizeid = int.Parse(AppSettings.ShowQuizId);
                List<QuizList> qlist = new List<QuizList>();
                qlist = Task.Run(async () => await Constants.connection.Table<QuizList>().Where(i => i.ShowID == showid && i.QuizID == quizeid).OrderBy(j => j.QuizID).ToListAsync()).Result;
                foreach (QuizList links in qlist)
                {
                    if (links.Name == Constants.Quizselecteditem.Name)
                    {
                        if (links.IsFavourite == false)
                            command = "UPDATE QuizList SET IsFavourite=" + 1 + " " + "WHERE  ShowID=" + links.ShowID + " And   QuizID=" + links.QuizID;
                        else
                            command = "UPDATE QuizList SET IsFavourite=" + 0 + " " + "WHERE  ShowID=" + links.ShowID + " And   QuizID=" + links.QuizID;

                        Task.Run(async () => await Constants.connection.QueryAsync<QuizList>(command));
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in RemoveFavoriteQuiz Method In FavoriteManager.cs file", ex);
            }
        }

        public static void Removemovieasfavorite()
        {
            int showid = int.Parse(AppSettings.ShowID);
            string command = string.Empty;
            ShowList showlist = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result;
            if (showlist.IsFavourite == true)
                command = "UPDATE ShowList SET IsFavourite=" + 0 + " " + "WHERE  ShowID=" + showlist.ShowID;
            Task.Run(async () => await Constants.connection.QueryAsync<ShowList>(command));
        }

        public static IQueryable<ShowList> GetCountForLastUpdatedFavouritesForVideos()
        {

            return default(IQueryable<ShowList>);
        }

        public static IQueryable<ShowLinks> GetCountForLastUpdatedFavouritesForLinks()
        {

            return default(IQueryable<ShowLinks>);
        }

        public static List<ShowLinks> GetLastUpdatedFavouriteLinks()
        {

            return default(IQueryable<ShowLinks>).ToList();

        }

        public static List<ShowList> GetLastUpdatedFavouriteShows()
        {

            return default(IQueryable<ShowList>).ToList();
        }

        public static void UpdateFavouritesByShowID(List<ShowList> CheckMovieIDForFavouritesDownload)
        {
            string command = string.Empty;
            ShowList RowToUpdate = CheckMovieIDForFavouritesDownload.FirstOrDefault();
            command = "UPDATE ShowList SET IsFavourite=" + 1 + " " + "WHERE  ShowID=" + RowToUpdate.ShowID;
            Task.Run(async () => await Constants.connection.QueryAsync<ShowList>(command));
        }

        public static List<ShowLinks> CheckShowIDForFavouriteSongDownload(string ID, string songno)
        {
            int id = int.Parse(ID);
            int linkorder = int.Parse(songno);
            var list = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == id & i.LinkOrder == linkorder).OrderBy(j => j.LinkOrder).ToListAsync()).Result;
            return list.AsQueryable<ShowLinks>().ToList();
        }

        public static List<ShowList> CheckShowIDForFavouritesDownload(string MovieID)
        {

            int showid = Convert.ToInt32(MovieID);
            var list = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).OrderBy(j => j.ShowID).ToListAsync()).Result;
            return list.AsQueryable<ShowList>().ToList();
        }

        public static void UpdateFavouritesForSongByShowID(List<ShowLinks> CheckMovieIDForFavouriteSongDownload)
        {
            ShowLinks RowToUpdate = CheckMovieIDForFavouriteSongDownload.FirstOrDefault();
            string command = string.Empty;
            command = "UPDATE ShowLinks SET IsFavourite=" + 1 + " " + "WHERE  ShowID=" + RowToUpdate.ShowID + "    And   LinkOrder=" + RowToUpdate.LinkOrder;
            Task.Run(async () => await Constants.connection.QueryAsync<ShowList>(command));

        }
        //Code for Wp8
        public static List<ShowLinks> GetFavoriteLinks(LinkType t)
        {
            List<ShowLinks> objMovieList = new List<ShowLinks>();
            try
            {
                string Ltype = t.ToString();
                var ShowLinksList = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.IsFavourite == true && i.LinkType == Ltype).ToListAsync()).Result;
                foreach (ShowLinks song in ShowLinksList)
                {

                    song.LinkUrl = song.Title + "," + song.LinkUrl + "," + song.LinkID;
                    ShowLinks favoriteSong = new ShowLinks();
                    favoriteSong.ShowID = song.ShowID;
                    favoriteSong.LinkID = song.LinkID;
                    string Movietitle = OnlineShow.GetShowDetail(song.ShowID).Title;
                    favoriteSong.movietitle = Movietitle;
                    //favoriteSong.Title = Movietitle + " - " + song.Title;
                    favoriteSong.Title = song.Title;
                    favoriteSong.LinkOrder = song.LinkOrder;
                    favoriteSong.LinkUrl = song.LinkUrl;
                    favoriteSong.LinkType = song.LinkType;
                    favoriteSong.DownloadStatus = song.DownloadStatus;
                    if (song.DownloadStatus == null)
                    {
                        favoriteSong.DownloadStatus = "Download";
                    }

                    if (song.LinkUrl.StartsWith("http://"))
                    {
                        favoriteSong.ThumbnailImage = song.LinkUrl.Split(',')[1].Replace(".wmv", "") + "_512.jpg";
                    }
                    else
                    {
                        favoriteSong.ThumbnailImage = "http://img.youtube.com/vi/" + song.LinkUrl.Split(',')[1] + "/default.jpg";
                    }
                    if (song.DownloadStatus == "1")
                    {
                        favoriteSong.DownloadIconVisible = Visibility.Collapsed;
                        favoriteSong.txtboxVisibility = Visibility.Visible;
                    }
                    else
                    {
                        favoriteSong.DownloadIconVisible = Visibility.Visible;
                        favoriteSong.txtboxVisibility = Visibility.Collapsed;
                    }
                    objMovieList.Add(favoriteSong);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadFavoritesongs Method In Vidoes.cs file", ex);
            }
            return objMovieList;
        }

        public static void SaveFavorites(long ShowID)
        {
            try
            {
                var selectedRow = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == ShowID).ToListAsync()).Result;
                ShowList selectedLink = selectedRow.FirstOrDefault();
                if (selectedLink != null)
                {
                    if (selectedLink.IsFavourite == false)
                    {
                        selectedLink.IsFavourite = true;
                    }
                    else
                    {
                        selectedLink.IsFavourite = false;
                    }
                    selectedLink.ClientShowUpdated = DateTime.Now;

                    Task.Run(async () => await Constants.connection.UpdateAsync(selectedLink));
                    int sid = Convert.ToInt32(AppSettings.ShowID);
#if WINDOWS_PHONE_APP && NOTANDROID
                    SyncFavouritesManager.savexml("Movies", Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == sid && i.LinkType == "Movies").FirstOrDefaultAsync()).Result != null ? Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == sid && i.LinkType == "Movies").FirstOrDefaultAsync()).Result.LinkID.ToString() : "1");
#endif
                    //context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Show ID", ShowID);
                Exceptions.SaveOrSendExceptions("Exception in SaveFavoriteSongs Method In OnlineShow.cs file", ex);
            }
        }

        public static bool IsFavoriteShow(long showID)
        {
            try
            {
                var shows = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showID).ToListAsync()).Result;
                return shows.FirstOrDefault().IsFavourite == true ? true : false;
            }
            catch (Exception ex)
            {
                ex.Data.Add("Show ID", showID);
                Exceptions.SaveOrSendExceptions("Exception in IsFavoriteMovie Method In OnlineShow.cs file", ex);
            }
            return false;
        }
    }
}
