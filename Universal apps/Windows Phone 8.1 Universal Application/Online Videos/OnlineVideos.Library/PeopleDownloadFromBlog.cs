using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using Common.Library;
using System.Linq;
using System.Xml.Linq;
using Common;
#if WINDOWS_APP
using Windows.Web.Syndication;
#endif
#if NOTANDROID
using Windows.Storage;
using Windows.Storage.Streams;
#if (WINDOWS_PHONE_APP && NOTANDROID)
//using System.ServiceModel.Syndication;
#endif
#endif
using System.Threading.Tasks;
using Windows.Web.Syndication;
//using Windows.Web.Syndication;
//using System.ServiceModel.Syndication;
#if (WINDOWS_PHONE_APP && NOTANDROID) || ANDROID
//using System.IO.IsolatedStorage;
#endif
namespace OnlineVideos.Library
{

    public class PeopleDownloadFromBlog
    {
        public static System.Threading.AutoResetEvent auto = new System.Threading.AutoResetEvent(false);
#if ANDROID
		System.Collections.ObjectModel.ObservableCollection<FeedItem> currentFeed = new System.Collections.ObjectModel.ObservableCollection<FeedItem> ();
#endif
        public static List<DateTimeOffset> PublishedDate = new List<DateTimeOffset>();
        public void StartDownload()
        {
            if (AppSettings.BlogStatus == 0)
            {
                if (!String.IsNullOrEmpty(AppSettings.Secondarypeoplefromblog) && AppSettings.Secondarypeoplefromblog!="false")
                {
                    if (Task.Run(async () => await Storage.FileExists("peoples.xml")).Result)
                        Storage.DeleteFile("peoples.xml");
                    ReadRss(new Uri(AppSettings.Secondarypeoplefromblog, UriKind.Absolute));
# if WINDOWS_PHONE_APP || ANDROID
                    auto.WaitOne();
#endif
                   AppSettings.BlogStatus = 1;
                }
                else
                {
                    if (Task.Run(async () => await Storage.FileExists("peoples.xml")).Result)
                        Storage.DeleteFile("peoples.xml");
                    AppSettings.BlogStatus = 1;
                }

            }
            else
            {
                if (!String.IsNullOrEmpty(AppSettings.Primarypeoplefromblog) && AppSettings.Primarypeoplefromblog!="false")
                {
                    ReadRss(new Uri(AppSettings.Primarypeoplefromblog, UriKind.Absolute));
# if WINDOWS_PHONE_APP || ANDROID
                    auto.WaitOne();
#endif

                }

            }

        }

        public async void ReadRss(Uri rssUri)
        {
            XDocument xdoc = new XDocument();

            string ttitle = string.Empty;
            try
            {
                if (Task.Run(async () => await Storage.FileExists("peoples.xml")).Result)
                {
#if ( WINDOWS_APP && NOTANDROID)
                    StorageFolder store = ApplicationData.Current.LocalFolder;
                    StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"peoples.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
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
                        StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"peoples.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
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
                var  currentFeed = Task.Run(async () => await client.RetrieveFeedAsync(rssUri)).Result.Items;
               
#endif
#if  ANDROID || WINDOWS_PHONE_APP
                HttpWebRequest request = (HttpWebRequest)
                    WebRequest.Create(rssUri);
				request.Method = HttpMethod.Get;
                HttpWebResponse response1 = (HttpWebResponse) Task.Run(async()=> await request.GetResponseAsync()).Result;
                Stream str = response1.GetResponseStream(); 
#if ANDROID
				var currentFeed=Task.Run(async () => await  SyndicationFeed.Load(str)).Result;	

#endif
#if NOTANDROID              
                SyndicationClient client = new SyndicationClient();
                client.BypassCacheOnRetrieve = true;
                var currentFeed = Task.Run(async () => await client.RetrieveFeedAsync(rssUri)).Result.Items;                
               // var currentFeed = SyndicationFeed.Load(response).Items;
#endif
#endif
                if (currentFeed == null)
                    return;
                string stringtomatch = "<NewDataSet>";

#if NOTANDROID && WINDOWS_PHONE_APP
                foreach (var f in currentFeed)
#endif
#if WINDOWS_APP
                foreach (var f in currentFeed)
#endif
                {
#if (WINDOWS_PHONE_APP && NOTANDROID) ||( WINDOWS_APP && NOTANDROID)
                    string SummaryText=f.Summary.Text;
                     #endif
                    #if ANDROID
					string SummaryText=f.Description;
                     #endif
                    #if ( WINDOWS_APP && NOTANDROID)

                    if ((DateTimeOffset)f.PublishedDate.DateTime > AppSettings.PeopleLastUpdatedDate)
                    #endif
#if ( WINDOWS_PHONE_APP && NOTANDROID) || ANDROID

                    if ((DateTimeOffset)f.PublishedDate.DateTime > AppSettings.PeopleLastUpdatedDate)                    
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
                    StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"peoples.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
                    var fquery1 = Task.Run(async () => await file1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                    var outputStream = fquery1.GetOutputStreamAt(0);
                    var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                    writer.WriteString(ttitle);
                    var fi = Task.Run(async () => await writer.StoreAsync()).Result;
                    writer.DetachStream();
                   var oi = Task.Run(async () => await outputStream.FlushAsync()).Result;
                    outputStream.Dispose();
                    fquery1.Dispose();
#endif
#if ANDROID || WINDOWS_PHONE_APP                   

                    try
                    {
                        StorageFolder store1 = ApplicationData.Current.LocalFolder;
                        StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"peoples.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
                        var fquery1 = Task.Run(async () => await file1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                        var outputStream = fquery1.GetOutputStreamAt(0);
                        var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                        writer.WriteString(ttitle);
                        var fi = Task.Run(async () => await writer.StoreAsync()).Result;
                        writer.DetachStream();
                        var oi = Task.Run(async () => await outputStream.FlushAsync()).Result;
                        outputStream.Dispose();
                        fquery1.Dispose();
                    }
                    catch (Exception ex)
                    {
                        
                    }
#endif
                    if (AppSettings.BlogStatus == 1)
                    {
                        if (PublishedDate.Count > 0)
                            AppSettings.LastPeoplePublishedDate = (from p in PublishedDate orderby p descending select p).FirstOrDefault();
                        PublishedDate.Clear();
                        AppSettings.BackgroundAgentStatus = SyncAgentStatus.UpdatePeople;
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
                        AppSettings.BackgroundAgentStatus = SyncAgentStatus.UpdatePeople;
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
                Exceptions.SaveOrSendExceptions("Exception in ReadRss Method In PeopleDownloadFromBlog.cs file", ex);
            }

        }

    }
}