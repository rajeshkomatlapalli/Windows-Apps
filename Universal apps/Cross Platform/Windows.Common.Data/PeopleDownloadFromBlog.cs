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
#endif
using System.Threading.Tasks;

namespace OnlineVideos.Library
{
     #if WINDOWS_APP
    public class PeopleDownloadFromBlog
    {
       
        SyndicationFeed currentFeed = null;
        public static List<DateTimeOffset> PublishedDate = new List<DateTimeOffset>();
        public void StartDownload()
        {
            if (AppSettings.BlogStatus == 0)
            {
                if (!String.IsNullOrEmpty(AppSettings.Secondarypeoplefromblog))
                {
                    if (Task.Run(async () => await Storage.FileExists("peoples.xml")).Result)
                    Storage.DeleteFile("peoples.xml");
                    ReadRss(new Uri(AppSettings.Secondarypeoplefromblog, UriKind.Absolute));
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
                if (!String.IsNullOrEmpty(AppSettings.Primarypeoplefromblog))
                {
                    ReadRss(new Uri(AppSettings.Primarypeoplefromblog, UriKind.Absolute));
                }

            }

        }

        public  void ReadRss(Uri rssUri)
        {
            XDocument xdoc = new XDocument();

            string ttitle = string.Empty;
            try
            {
                if (Task.Run(async () => await Storage.FileExists("peoples.xml")).Result)
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
                else
                {
                    ttitle = string.Empty;
                }
                SyndicationClient client = new SyndicationClient();
                client.BypassCacheOnRetrieve = true;
                currentFeed = Task.Run(async () => await client.RetrieveFeedAsync(rssUri)).Result;

                if (currentFeed == null)
                    return;



                string stringtomatch = "<NewDataSet>";
                foreach (SyndicationItem f in currentFeed.Items)
                {
                    if ((DateTimeOffset)f.PublishedDate.DateTime > AppSettings.PeopleLastUpdatedDate)
                    {
                        if (ttitle == "")

                            ttitle = WebUtility.HtmlDecode(Regex.Replace(f.Summary.Text, "<[^>]+?>", ""));
                        else
                            ttitle = ttitle.Substring(0, ttitle.LastIndexOf('<')) + (WebUtility.HtmlDecode(Regex.Replace(f.Summary.Text, "<[^>]+?>", ""))).Substring(stringtomatch.Length);

                    }

                    PublishedDate.Add((DateTimeOffset)f.PublishedDate.DateTime);
                }
                if (ttitle != "")
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
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ReadRss Method In PeopleDownloadFromBlog.cs file", ex);
            }

        }

    }
#endif
}
