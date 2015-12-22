using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using Common.Library;
using Common;
 #if NOTANDROID
using Windows.Storage;
using Windows.Storage.Streams;
#endif
#if WINDOWS_PHONE_APP && NOTANDROID
//using System.ServiceModel.Syndication;
#endif
using System.Threading.Tasks;
using System.Text;
using Windows.Web.Syndication;
//using System.ServiceModel.Syndication;
#if (WINDOWS_PHONE_APP && NOTANDROID) || ANDROID
//using System.IO.IsolatedStorage;
#endif
#if WINDOWS_APP
using Windows.Web.Syndication;
#endif
namespace OnlineVideos.Library
{

    public class ShowDownloadForBlogs
    {
		#if ANDROID
		System.Collections.ObjectModel.ObservableCollection<FeedItem> currentFeed = new System.Collections.ObjectModel.ObservableCollection<FeedItem> ();
		#endif
        public static List<DateTimeOffset> PublishedDate = new List<DateTimeOffset>();
        public static System.Threading.AutoResetEvent auto = new System.Threading.AutoResetEvent(false);
       
        public void StartDownload()
        {
            if (AppSettings.BlogStatus == 0)
            {
#if WINDOWS_PHONE_APP
                if (!String.IsNullOrEmpty(AppSettings.Secondaryshowsfromblog))
                { 
#endif
                if (!String.IsNullOrEmpty(AppSettings.Secondaryshowsfromblogwin81))
                {
                    if (Task.Run(async () => await Storage.FileExists("Movies.xml")).Result)
                    Storage.DeleteFile("Movies.xml");
#if WINDOWS_PHONE_APP
                    ReadRss(new Uri(AppSettings.Secondaryshowsfromblog, UriKind.Absolute)); 
#endif
                    ReadRss(new Uri(AppSettings.Secondaryshowsfromblogwin81, UriKind.Absolute));
#if WINDOWS_PHONE_APP || ANDROID
                    auto.WaitOne();
#endif
                    AppSettings.BlogStatus = 1;
                }
                else
                {
                    if (Task.Run(async () => await Storage.FileExists("Movies.xml")).Result)
                    Storage.DeleteFile("Movies.xml");
                    AppSettings.BlogStatus = 1;
                }
            }
            else
            {
#if WINDOWS_PHONE_APP
                if (!String.IsNullOrEmpty(AppSettings.Primaryshowsfromblog))
                {
                    ReadRss(new Uri(AppSettings.Primaryshowsfromblog, UriKind.Absolute)); 
#endif
                if (!String.IsNullOrEmpty(AppSettings.Primaryshowsfromblogwin81))
                {
                    ReadRss(new Uri(AppSettings.Primaryshowsfromblogwin81, UriKind.Absolute));
#if WINDOWS_PHONE_APP || ANDROID
                    auto.WaitOne();
#endif
                }
            }
        }
       
        public async static void ReadRss(Uri rssUri)
        {
            try
            {
                XDocument xdoc = new XDocument();
                string ttitle = string.Empty;

                if (Task.Run(async () => await Storage.FileExists("Movies.xml")).Result)
                {
#if ( WINDOWS_APP && NOTANDROID)

                StorageFolder store = ApplicationData.Current.LocalFolder;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"Movies.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var f = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = f.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
                string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
                System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlData));
                xdoc = XDocument.Load(ms);
                ttitle = xdoc.ToString();
                dataReader.DetachStream();
                dataReader.Dispose();
                inputStream.Dispose();
#endif
#if ANDROID || WINDOWS_PHONE_APP                    

                    try
                    {
                        StorageFolder store = ApplicationData.Current.LocalFolder;
                        StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"Movies.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                        var f = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                        IInputStream inputStream = f.GetInputStreamAt(0);
                        DataReader dataReader = new DataReader(inputStream);
                        uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
                        string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
                        System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlData));
                        xdoc = XDocument.Load(ms);
                        ttitle = xdoc.ToString();
                        dataReader.DetachStream();
                        dataReader.Dispose();
                        inputStream.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Exceptions.SaveOrSendExceptions("Exception in ReadRss Method In ShowDownloadFromBlog.cs file", ex);
                    }
#endif
                }
                else
                {
                    ttitle = string.Empty;
                }
#if( WINDOWS_APP && NOTANDROID)
                SyndicationClient client = new SyndicationClient();
                client.BypassCacheOnRetrieve = true;
			   var currentFeed = Task.Run(async () => await client.RetrieveFeedAsync(rssUri)).Result.Items;
#endif
#if  ANDROID || WINDOWS_PHONE_APP
                
#if ANDROID
		var	currentFeed= Task.Run(async () => await  SyndicationFeed.Load(str)).Result;	
#endif
#if NOTANDROID
                
                SyndicationClient client = new SyndicationClient();
                client.BypassCacheOnRetrieve = true;
                var currentFeed = Task.Run(async () => await client.RetrieveFeedAsync(rssUri)).Result.Items;
#endif
#endif
                if (currentFeed == null)
                    return;
                string stringtomatch = "<NewDataSet>";
#if ANDROID || WINDOWS_PHONE_APP
                foreach (var f in currentFeed)
#endif
#if WINDOWS_APP
            foreach (var f in currentFeed)
#endif
                {
#if (WINDOWS_PHONE_APP && NOTANDROID) ||( WINDOWS_APP && NOTANDROID)
                    string SummaryText = f.Summary.Text;
#endif
#if ANDROID
				string SummaryText=f.Description;
#endif
#if ( WINDOWS_APP && NOTANDROID)
                if ((DateTimeOffset)f.PublishedDate.DateTime > AppSettings.LastUpdatedDate)
#endif
#if ( WINDOWS_PHONE_APP && NOTANDROID) || ANDROID
                    //if ((DateTimeOffset)f.PublishedDate.Date > AppSettings.LastUpdatedDate.Date)
                    if ((DateTimeOffset)f.PublishedDate.DateTime > AppSettings.LastUpdatedDate)
#endif
                    {
                        if (ttitle == "")

                            ttitle = WebUtility.HtmlDecode(Regex.Replace(SummaryText, "<[^>]+?>", ""));
                        else
                            ttitle = ttitle.Substring(0, ttitle.LastIndexOf('<')) + (WebUtility.HtmlDecode(Regex.Replace(SummaryText, "<[^>]+?>", ""))).Substring(stringtomatch.Length);

                    }
#if ( WINDOWS_APP && NOTANDROID)
                PublishedDate.Add((DateTimeOffset)f.PublishedDate.DateTime);
#endif
#if ( WINDOWS_PHONE_APP && NOTANDROID)
                    PublishedDate.Add((DateTimeOffset)f.PublishedDate.DateTime);
#endif
#if ANDROID
				PublishedDate.Add((DateTimeOffset)f.PublishDate);
#endif
                }


                if (ttitle != "")
                {
#if ( WINDOWS_APP && NOTANDROID)
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"Movies.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
                var fquery1 = Task.Run(async () => await file1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                var outputStream =  fquery1.GetOutputStreamAt(0);
                var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                writer.WriteString(ttitle);
                var fi = Task.Run(async () => await writer.StoreAsync()).Result;
                var oi = Task.Run(async () => await outputStream.FlushAsync()).Result;
                writer.DetachStream();
                outputStream.Dispose();
                fquery1.Dispose();
#endif
#if ANDROID || WINDOWS_PHONE_APP                    
                    try
                    {
                        StorageFolder store1 = ApplicationData.Current.LocalFolder;
                        StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"Movies.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
                        var fquery1 = Task.Run(async () => await file1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                        var outputStream = fquery1.GetOutputStreamAt(0);
                        var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                        writer.WriteString(ttitle);
                        var fi = Task.Run(async () => await writer.StoreAsync()).Result;
                        var oi = Task.Run(async () => await outputStream.FlushAsync()).Result;
                        writer.DetachStream();
                        outputStream.Dispose();
                        fquery1.Dispose();
                    }
                    catch (Exception ex)
                    {
                       Exceptions.SaveOrSendExceptions("Exception in ReadRss Method In ShowDownloadFromBlog.cs file", ex);
                    }
#endif
                    if (AppSettings.BlogStatus == 1)
                    {
                        if (PublishedDate.Count > 0)
                            AppSettings.LastPublishedDate = (from p in PublishedDate orderby p descending select p).FirstOrDefault();
                        PublishedDate.Clear();
                        AppSettings.BackgroundAgentStatus = SyncAgentStatus.UpdateShows;
                        AppSettings.BlogStatus = 0;
                    }
                    else
                    {
                        AppSettings.BlogStatus = 1;
                    }

# if WINDOWS_PHONE_APP || ANDROID
                    auto.Set();
#endif
                }
                else
                {
                    if (AppSettings.BlogStatus == 1)
                    {
                        AppSettings.BackgroundAgentStatus = SyncAgentStatus.UpdateShows;

                    }
                    if (AppSettings.BlogStatus == 1)
                    {
                        AppSettings.BlogStatus = 0;
                    }
                    else
                    {
                        AppSettings.BlogStatus = 1;
                    }
# if WINDOWS_PHONE_APP || ANDROID
                    auto.Set();
#endif
                }
            }
            catch (Exception ex)
            {               
            }
        }
    }
}
