using Common.Library;
using OnlineVideos.Data;
using PicasaMobileInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
#if WINDOWS_PHONE_APP
//using System.ServiceModel.Syndication;
#endif
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Windows.Web.Syndication;
//using System.ServiceModel.Syndication;
#if WINDOWS_APP
using Windows.Web.Syndication;
#endif

namespace OnlineVideos.Library
{
    public class BlogDetails
    {
        public string title
        {
            get;
            set;
        }
        public string url
        {
            get;
            set;
        }

    }
    public class UpdateRatingInBlog
    {
        public string DefaultUrl = string.Empty;
        string PostTitle = string.Empty;
        double PostRating = 0.0;
        string blogtype = string.Empty;
        public AutoResetEvent auto = new AutoResetEvent(false);

        public async void getshow(string showid, double rating, string type)
        {
            //double returnrating = 0.0;
            try
            {
                blogtype = type;
                PostTitle = showid;
                PostRating = rating;
#if WINDOWS_PHONE_APP
                HttpWebRequest request = default(HttpWebRequest);
                if (blogtype == "show")
                    request = (HttpWebRequest)WebRequest.Create(AppSettings.ShowsRatingBlogUrl+"&category=" + PostTitle.Trim());
                    else if(blogtype=="Quiz")
                    request = (HttpWebRequest)WebRequest.Create(AppSettings.QuizRatingBlogUrl + "&category=" + PostTitle.Trim());
                else
                    request = (HttpWebRequest)WebRequest.Create(AppSettings.LinksRatingBlogUrl + "&category=" + PostTitle.Trim());
                request.Method = HttpMethod.Get;
                HttpWebResponse response1 = (HttpWebResponse)Task.Run(async () => await request.GetResponseAsync()).Result;
                Stream str = response1.GetResponseStream();
                XmlReader response = XmlReader.Create(str);                
              //  SyndicationFeed feeds = SyndicationFeed.Load(response);

                SyndicationFeed feeds = null;
                SyndicationClient currentFeed = new SyndicationClient();
                currentFeed.BypassCacheOnRetrieve = true;

                feeds = await currentFeed.RetrieveFeedAsync(new Uri(AppSettings.ShowsRatingBlogUrl + "&category=" + PostTitle.Trim()));

                if (feeds == null)
                    return;
                if (feeds.Items.Count() > 0)
                {
                    foreach (SyndicationItem f in feeds.Items)
                    {
                        string id = f.Id;
                        double Rate = Convert.ToDouble(WebUtility.HtmlDecode(Regex.Replace(f.Summary.Text, "<[^>]+?>", "")));
                        double finalrating = (PostRating + Rate) / 2;
                        updatepost(id, finalrating, PostTitle);
                    }
                }
                else
                {
                    insertpost();
                }
#endif
#if WINDOWS_APP

                Uri uri;
                if (blogtype == "show")
                  uri = new Uri("http://"+Constants.Uploadshowrating+".blogspot.com/feeds/posts/default?&category=" + PostTitle.Trim() + "&alt=rss", UriKind.RelativeOrAbsolute);
                else if (blogtype == "Quiz")
                {
                    uri = new Uri("http://" + Constants.Uploadquizrating + ".blogspot.com/feeds/posts/default?&category=" + PostTitle.Trim() + "&alt=rss", UriKind.RelativeOrAbsolute);
                }
                else
                {
                    uri = new Uri("http://" + Constants.UploadLinksrating + ".blogspot.com/feeds/posts/default?&category=" + PostTitle.Trim() + "&alt=rss", UriKind.RelativeOrAbsolute);
                }
               
                SyndicationClient client = new SyndicationClient();
                client.BypassCacheOnRetrieve = true;
                SyndicationFeed feeds = Task.Run(async () => await client.RetrieveFeedAsync(uri)).Result;
                if (feeds.Items.Count() > 0)
                {
                    foreach (SyndicationItem f in feeds.Items)
                    {
                        string id = f.Id;
                        double Rate = Convert.ToDouble(System.Net.WebUtility.HtmlDecode(Regex.Replace(f.Summary.Text, "<[^>]+?>", "")));
                        double finalrating = (PostRating + Rate) / 2;
                        //returnrating=finalrating;
                        updatepost(id, finalrating, PostTitle);
                    }
                }
                else
                {
                    insertpost();
                }
#endif
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in getshow Method In UpdateRatingInBlog.cs file", ex);
                string gg = ex.Message;
                //return returnrating;
            }
        }
        public void insertpost()
        {

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.blogger.com/feeds/default/blogs/");
                request.Method = "GET";
                request.Headers["Authorization"] = CONST.PIC_AUTH + AppSettings.blogtoken;

                request.BeginGetResponse(new AsyncCallback(RequestBlogCompleted), request);
                auto.WaitOne();
            }
            catch (Exception ex)
            {
                 Exceptions.SaveOrSendExceptions("Exception in insertpost Method In UpdateRatingInBlog.cs file", ex);
            }
        }

        private void RequestBlogCompleted(IAsyncResult result)
        {
            try
            {
                var request = (HttpWebRequest)result.AsyncState;
                var response = (HttpWebResponse)request.EndGetResponse(result);
                StreamReader responseReader = new StreamReader(response.GetResponseStream());
                string Title = string.Empty;
                if (blogtype == "show")
                {

#if WINDOWS_PHONE_APP
                    Title = AppSettings.ShowsRatingBlogName;
#endif
#if WINDOWS_APP
                Title = Constants.Uploadshowrating;
#endif
                }
                else if (blogtype == "Quiz")
                {

#if WINDOWS_PHONE_APP
                    Title = AppSettings.QuizLinksRatingBlogName;
#endif
#if WINDOWS_APP
                Title = Constants.Uploadquizrating;
#endif

                }
                else
                {
#if WINDOWS_PHONE_APP
                    Title = AppSettings.LinksRatingBlogName;
#endif
#if WINDOWS_APP
                Title = Constants.UploadLinksrating;
#endif

                }
                string AttributeValue = "http://schemas.google.com/g/2005#post";
                XElement MyXMLConfig = XElement.Load(responseReader);
                XNamespace atomNS = "http://www.w3.org/2005/Atom";
                IEnumerable<BlogDetails> blogdetail = from item in MyXMLConfig.Descendants(atomNS + "entry")
                                                      select new BlogDetails
                                                      {
                                                          title = item.Element(atomNS + "title").Value,
                                                          url = item.Elements(atomNS + "link").Where(a => a.Attribute("rel").Value == AttributeValue).Select(a => a.Attribute("href").Value).FirstOrDefault(),
                                                      };
                foreach (BlogDetails bb in blogdetail)
                {
                    if (Title == bb.title)
                    {
                        DefaultUrl = bb.url;
                        break;
                    }
                }
                UploadPost();
            }
            catch (Exception ex)
            {
                
                 Exceptions.SaveOrSendExceptions("Exception in RequestBlogCompleted Method In UpdateRatingInBlog.cs file", ex);
            }
        }
        public void UploadPost()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(DefaultUrl);
                request.Method = "POST";
                request.ContentType = "application/atom+xml";
                request.Headers["Authorization"] = CONST.PIC_AUTH + AppSettings.blogtoken;
                List<object> UserState = new List<object>();
                UserState.Add(request);
                UserState.Add(PostRating);
                UserState.Add(PostTitle);
                request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallbackItem), UserState);
            }
            catch (Exception ex)
            {
                
                Exceptions.SaveOrSendExceptions("Exception in UploadPost Method In UpdateRatingInBlog.cs file", ex);
            }
        }


        public void updatepost(string ID, double Rate, string title)
        {
            try
            {
                string BlogID = ID.Substring(ID.IndexOf('-') + 1, (ID.LastIndexOf('.') - (ID.IndexOf('-') + 1)));
                string PostID = ID.Substring(ID.LastIndexOf('-') + 1);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.blogger.com/feeds/" + BlogID + "/posts/default/" + PostID);
                request.Method = "PUT";
                request.ContentType = "application/atom+xml";
                request.Headers["Authorization"] = CONST.PIC_AUTH + AppSettings.blogtoken;
                List<object> UserState = new List<object>();
                UserState.Add(request);
                UserState.Add(Rate);
                UserState.Add(title);
                request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallbackItem), UserState);
                auto.WaitOne();
            }
            catch (Exception ex)
            {

                auto.Set();
                Exceptions.SaveOrSendExceptions("Exception in updatepost Method In UpdateRatingInBlog.cs file", ex);
            }
        }
        private void GetRequestStreamCallbackItem(IAsyncResult asynchronousResult)
        {
            try
            {
                List<object> UserState = new List<object>();

                UserState = (List<object>)asynchronousResult.AsyncState;

                BlogData(asynchronousResult, (HttpWebRequest)UserState[0], (double)UserState[1], (string)UserState[2]);
            }
            catch(Exception ex)
            {

                auto.Set();
                Exceptions.SaveOrSendExceptions("Exception in GetRequestStreamCallbackItem Method In UpdateRatingInBlog.cs file", ex);
            }
        }
        private void BlogData(IAsyncResult asynchronousResult, HttpWebRequest request, double rating, string title)
        {
            try
            {
                Stream postStream = new MemoryStream();

                string contents = "<entry xmlns='http://www.w3.org/2005/Atom'>" +
                          "<title type='text'>" + title.Trim() + "</title>" +
                          "<category scheme='http://www.blogger.com/atom/ns#' term='" + title.Trim() + "'/>" +
                          "<content type='xhtml'>" +
                            rating +
                          "</content>" +
                          "</entry>";

                Stream ms = new MemoryStream(Encoding.UTF8.GetBytes(contents));
                postStream = request.EndGetRequestStream(asynchronousResult);
                byte[] buffer = new byte[ms.Length / 4];
                int bytesRead = -1;
                ms.Position = 0;
                while ((bytesRead = ms.Read(buffer, 0, buffer.Length)) > 0)
                {
                    postStream.Write(buffer, 0, bytesRead);
                }
                ms.Flush();
#if WINDOWS_PHONE_APP
                //ms.Close();
                postStream.Flush();
                //postStream.Close();
#endif
#if WINDOWS_APP
                ms.Dispose();
                postStream.Flush();
                postStream.Dispose();
#endif

                request.BeginGetResponse(new AsyncCallback(RequestAlbumCompletedItem), request);
            }
            catch(Exception ex)
            {

                auto.Set();
                Exceptions.SaveOrSendExceptions("Exception in BlogData Method In UpdateRatingInBlog.cs file", ex);
            }
        }
        private void RequestAlbumCompletedItem(IAsyncResult result)
        {
            try
            {
                var request = (HttpWebRequest)result.AsyncState;
                var response = (HttpWebResponse)request.EndGetResponse(result);
                StreamReader responseReader = new StreamReader(response.GetResponseStream());
                string responseStr = responseReader.ReadToEnd();

                auto.Set();
            }
            catch(Exception ex)
            {

                auto.Set();
                Exceptions.SaveOrSendExceptions("Exception in RequestAlbumCompletedItem Method In UpdateRatingInBlog.cs file", ex);
            }
        }
        public async void downloadrating(DateTimeOffset lastDownloadedRatingDate, string type1)
        {
            try
            {
                string lastDownloadedRatingDate1 = lastDownloadedRatingDate.ToString("yyyy-MM-dd'T'HH:mm:ss");
                string lastDownloadedRatingDate2 = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
                //WebRequest.Create("http://indiancinemarating.blogspot.com/feeds/posts/default?published-min=" + lastDownloadedRatingDate + "T00:00:00&published-max=28-10-2013T23:59:59");
                SyndicationFeed currentFeed = null;
                HttpWebRequest request = default(HttpWebRequest);
#if WINDOWS_PHONE_APP
                if (type1 == "show")
                    request = (HttpWebRequest)WebRequest.Create(AppResources.ShowsRatingBlogUrl + "&updated-min=" + lastDownloadedRatingDate1 + "&updated-max=" + lastDownloadedRatingDate2 + "&orderby=updated");
                else if (type1 == "Quiz")
                    request = (HttpWebRequest)WebRequest.Create(AppResources.QuizRatingBlogUrl + "&updated-min=" + lastDownloadedRatingDate1 + "&updated-max=" + lastDownloadedRatingDate2 + "&orderby=updated");
                else
                    request = (HttpWebRequest)WebRequest.Create(AppResources.LinksRatingBlogUrl + "&updated-min=" + lastDownloadedRatingDate1 + "&updated-max=" + lastDownloadedRatingDate2 + "&orderby=updated");
#endif
#if WINDOWS_APP
            if (type1 == "show")
                request = (HttpWebRequest)WebRequest.Create("http://"+Constants.Uploadshowrating+".blogspot.com/feeds/posts/default?updated-min=" + lastDownloadedRatingDate1 + "&updated-max=" + lastDownloadedRatingDate2 + "&orderby=updated&alt=rss");
            else  if (type1 == "Quiz")
            {
                request = (HttpWebRequest)WebRequest.Create("http://" + Constants.Uploadquizrating + ".blogspot.com/feeds/posts/default?updated-min=" + lastDownloadedRatingDate1 + "&updated-max=" + lastDownloadedRatingDate2 + "&orderby=updated&alt=rss");
            }
            else
                request = (HttpWebRequest)WebRequest.Create("http://"+Constants.UploadLinksrating+".blogspot.com/feeds/posts/default?updated-min=" + lastDownloadedRatingDate1 + "&updated-max=" + lastDownloadedRatingDate2 + "&orderby=updated&alt=rss");
#endif
                request.Method = HttpMethod.Get;
                HttpWebResponse response1 = (HttpWebResponse)Task.Run(async () => await request.GetResponseAsync()).Result;
                Stream str = response1.GetResponseStream();
                XmlReader response = XmlReader.Create(str);
#if WINDOWS_PHONE_APP
                Uri uri;
                if (type1 == "show")
                    uri = new Uri(AppResources.ShowsRatingBlogUrl.ToString(), UriKind.RelativeOrAbsolute);
                else if (type1 == "Quiz")
                {
                    uri = new Uri(AppResources.QuizRatingBlogUrl, UriKind.RelativeOrAbsolute);
                }
                else
                    uri = new Uri(AppResources.LinksRatingBlogUrl, UriKind.RelativeOrAbsolute);

                SyndicationClient client = new SyndicationClient();
                client.BypassCacheOnRetrieve = true;
                currentFeed = Task.Run(async () => await client.RetrieveFeedAsync(uri)).Result;
#endif
#if WINDOWS_PHONE_APP
              //  currentFeed = SyndicationFeed.Load(response);

                //SyndicationFeed current = null;
                //SyndicationClient currentFeed1 = new SyndicationClient();
                //currentFeed1.BypassCacheOnRetrieve = true;

                //current = await currentFeed1.RetrieveFeedAsync();

#endif
#if WINDOWS_APP
            Uri uri;
            if (blogtype == "show")
                uri = new Uri("http://"+Constants.Uploadshowrating+".blogspot.com/feeds/posts/default?&category=" + PostTitle.Trim() + "&alt=rss", UriKind.RelativeOrAbsolute);
            else if (blogtype == "Quiz")
            {
                uri = new Uri("http://" + Constants.Uploadquizrating + ".blogspot.com/feeds/posts/default?&category=" + PostTitle.Trim() + "&alt=rss", UriKind.RelativeOrAbsolute);
            }
            else
                uri = new Uri("http://"+Constants.UploadLinksrating+".blogspot.com/feeds/posts/default?&category=" + PostTitle.Trim() + "&alt=rss", UriKind.RelativeOrAbsolute);
            
            SyndicationClient client = new SyndicationClient();
            client.BypassCacheOnRetrieve = true;
            currentFeed = Task.Run(async () => await client.RetrieveFeedAsync(uri)).Result;
#endif
                if (currentFeed == null)
                    return;
                foreach (SyndicationItem f in currentFeed.Items)
                {
#if WINDOWS_PHONE_APP                    
                    if ((DateTimeOffset)f.PublishedDate.DateTime > lastDownloadedRatingDate)
#endif
#if WINDOWS_APP
                if ((DateTimeOffset)f.PublishedDate.Date > lastDownloadedRatingDate.Date)
#endif
                    {
#if WINDOWS_PHONE_APP
                        double rating = Convert.ToDouble(WebUtility.HtmlDecode(Regex.Replace(f.Summary.Text, "<[^>]+?>", "")));
#endif
#if WINDOWS_APP
                    double rating = Convert.ToDouble(System.Net.WebUtility.HtmlDecode(Regex.Replace(f.Summary.Text, "<[^>]+?>", "")));
#endif
                        string title = f.Title.Text;
                        if (type1 == "show")
                            RatingManager.UpdateRatingForShows(title, rating);
                        else
                            RatingManager.UpdateRatingForVideos(title, rating);
                        if (type1 == "show")
                            AppSettings.lastDownloadedRatingDate = DateTime.Now;
                        else
                            AppSettings.lastVideoDownloadedRatingDate = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in downloadrating Method In UpdateRatingInBlog.cs file", ex);
            }
        }
    }
}
