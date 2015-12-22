using Common.Library;
using HtmlAgilityPack;
using MyToolkit.Multimedia;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Data.Xml.Dom;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace OnlineVideos.UI
{
    public static class DownloadManagerHelper
    {
        public static List<Uri> GetUrlForsharesixAndPromptfile(List<Uri> Video)
        {
            string htmlforvideoslasher = string.Empty;
            try
            {
                var httpClient = new HttpClient();
                var url = new Uri(AppSettings.NavigationUrl);
                var accessToken = "1234";
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                httpRequestMessage.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken));
                httpRequestMessage.Headers.Add("User-Agent", "My user-Agent");
                var response = Task.Run(async () => await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead)).Result;
                string html = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
                string pat = @"<frame[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
                MatchCollection fram = Regex.Matches(html, pat, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match m in fram)
                {
                    string href = m.Groups[1].Value;
                    if (href.ToString().StartsWith("http://"))
                    {
                        var httpClient1 = new HttpClient();
                        var url1 = new Uri(href);
                        var accessToken1 = "1234";
                        var httpRequestMessage1 = new HttpRequestMessage(HttpMethod.Get, url1);
                        httpRequestMessage1.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken1));
                        httpRequestMessage1.Headers.Add("User-Agent", "My user-Agent");
                        var response1 = Task.Run(async () => await httpClient.SendAsync(httpRequestMessage1, HttpCompletionOption.ResponseHeadersRead)).Result;
                        string html1 = Task.Run(async () => await response1.Content.ReadAsStringAsync()).Result;
                        htmlforvideoslasher = html1;
                        string pat1 = @"\<script\s?.*?\>((.|\r\n)+?)\<\/script\>";
                        Match mth = Regex.Match(html1, @"\<Title\s?.*?\>((.|\r\n)+?)\<\/Title\>");
                        MatchCollection fram1 = Regex.Matches(html1, pat1, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        foreach (Match m1 in fram1)
                        {
                            string rep = m1.Groups[1].Value.Replace("</script>\n", "");
                            if (rep.StartsWith("<script type='text/javascript'>eval"))
                            {
                                SharesixVidoeurl(rep);
                            }
                        }
                    }
                    if (href.ToString().StartsWith("http://www.promptfile.com"))
                    {
                        var httpClient1 = new HttpClient();
                        var url1 = new Uri(href);
                        var accessToken1 = "1234";
                        var httpRequestMessage1 = new HttpRequestMessage(HttpMethod.Get, url1);
                        httpRequestMessage1.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken1));
                        httpRequestMessage1.Headers.Add("User-Agent", "My user-Agent");
                        var response1 = Task.Run(async () => await httpClient.SendAsync(httpRequestMessage1, HttpCompletionOption.ResponseHeadersRead)).Result;
                        string html1 = Task.Run(async () => await response1.Content.ReadAsStringAsync()).Result;
                        string pat1 = @"\<div\s?.*?\>((.|\r\n)+?)\<\/div\>";
                        MatchCollection fram1 = Regex.Matches(html1, pat1, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        foreach (Match m1 in fram1)
                        {

                            Match GetUrl = Regex.Match(m1.ToString(), @"<a[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                            if (GetUrl.ToString().Contains("http://www.promptfile.com"))
                            {
                                string Url = GetUrl.Groups[1].Value.Replace("==", "");
                                Match Title = Regex.Match(html, @"\<title\s?.*?\>((.|\r\n)+?)\<\/title\>");
                                string OriginalTitle = Title.Groups[1].Value;
                                string OriginalUrl = Url + "=" + OriginalTitle + ".mkv";
                                Video.Add(new Uri(OriginalUrl));
                            }
                        }
                    }

                    if (href.StartsWith("http://www.videoslasher.com"))
                    {
                        string title = Regex.Match(htmlforvideoslasher, @"\<title\s?.*?\>((.|\r\n)+?)\<\/title\>").Groups[1].Value;
                        Video.Add(new Uri(href + "/" + title));
                    }
                    if (href.StartsWith("http://royalvids.eu"))
                    {
                        string title = Regex.Match(htmlforvideoslasher, @"\<Title\s?.*?\>((.|\r\n)+?)\<\/Title\>").Groups[1].Value;
                        Video.Add(new Uri(href + "/" + title));
                    }
                }
                if (response.RequestMessage.RequestUri.ToString().StartsWith("http://www.putlocker.com/file/"))
                {
                    Match Title = Regex.Match(html, @"\<title\s?.*?\>((.|\r\n)+?)\<\/title\>");
                    string OriginalTitle = Title.Groups[1].Value;
                    Video.Add(new Uri(response.RequestMessage.RequestUri.ToString() + "/" + OriginalTitle));
                }
                SharesixVidoeurl(html);
                royalvidsvideourl(html);
                videoslashervideourls(html);
                if (response.StatusCode.ToString() != "OK")
                {
                    // Exceptions.SaveOrSendHttpclientExceptions("Exception in GetHtmlContent Method In DownloadManagerDailyMotion.cs file", response.ReasonPhrase, response.RequestMessage.RequestUri.ToString());
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in GetUrlForsharesixAndPromptfile Method In DownloadManagerHelper file", ex);

            }
            return Video;
        }

        private static void SharesixVidoeurl(string html)
        {
            try
            {
                string sharesixvideourl = Regex.Split(html, "player_code")[2];
                Match mth = Regex.Match(html, @"\<Title\s?.*?\>((.|\r\n)+?)\<\/Title\>");
                string rep = sharesixvideourl;
                string rate = rep.Substring(rep.LastIndexOf("addParam|") + 9).Replace("player|", "");
                string PlayerUrl = rate.Substring(rate.LastIndexOf("video|") + 6).Split('|')[0];
                string PlayerID = rate.Split('f')[0].Replace("|", "._").Replace("._._", "");
                string[] arrayid = PlayerID.Split('_');
                string orignalid = null;
                int i = 0;
                foreach (string s in arrayid.Reverse())
                {
                    i++;
                    if (arrayid.Count() != i)
                        orignalid = orignalid + s + ".";
                    else
                        orignalid = orignalid + s.Replace(".", "");
                }
                string DownloadUrl = "http://" + orignalid.Replace("..", ".") + "/d/" + PlayerUrl + "/" + mth.Groups[1].Value;
                Constants.SaveVideos.Add(new Uri(DownloadUrl));
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in SharesixVidoeurl Method In DownloadManagerHelper file", ex);

            }

        }

        private static void royalvidsvideourl(string html)
        {
            try
            {
                string videoslasher = Regex.Split(html, "SRC=")[1].Split('"')[1];
                if (videoslasher.StartsWith("http://royalvids.eu/embed"))
                {
                    string title = Regex.Match(html, @"\<Title\s?.*?\>((.|\r\n)+?)\<\/Title\>").Groups[1].Value;
                    Constants.SaveVideos.Add(new Uri(videoslasher + "/" + title));
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in royalvidsvideourl Method In DownloadManagerHelper file", ex);

            }
        }

        private static void videoslashervideourls(string html)
        {
            try
            {
                string videoslasher = Regex.Split(html, "http://www.videoslasher.com/embed/")[1].Split('"')[0];
                string title = Regex.Match(html, @"\<title\s?.*?\>((.|\r\n)+?)\<\/title\>").Groups[1].Value.Replace("| VideoSlasher.com", "");
                Constants.SaveVideos.Add(new Uri("http://www.videoslasher.com/file/" + videoslasher + "/" + title));
            }
            catch (Exception ex)
            {
                ex.Data.Add("navigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in videoslashervideourls Method In DownloadManagerHelper file", ex);


            }
        }
        public static DownloadPivot gettimeandviews(string hh, DownloadPivot d)
        {
             HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(hh);
            if (Constants.NavigatedUri.Contains("v="))
            {
                foreach (HtmlNode node in doc.DocumentNode.Descendants().Where(n => n.Name == "li"))
                {
                    try
                    {
                        //http://www.youtube.com/watch?v=uIDx3eUZ-vw
                        if (node.Attributes.Where(i => i.Name == "class").FirstOrDefault() != null)
                        {
                            if (node.Attributes["class"].Value.ToString().Contains("video-list-item"))
                            {

                                d.title = WebUtility.HtmlDecode(node.Descendants("a").Where(i => i.Attributes["class"] != null && i.Attributes["class"].Value != null).FirstOrDefault().Attributes["title"].Value);

                                //d.time = node.Descendants("span").Where(i => i.Attributes["class"] != null && i.Attributes["class"].Value == "video-time").FirstOrDefault().InnerText;
                                //d.views = node.Descendants("span").Where(i => i.Attributes["class"] != null && i.Attributes["class"].Value == "stat view-count").FirstOrDefault().InnerText.Trim();


                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        Exceptions.SaveOrSendExceptions("Exception in OnlineLinks_Loaded event In OnlineLinks.xaml.cs file", ex);
                    }
                }
            }
            //else if (Constants.NavigatedUri.Contains("search_query="))
            //{
            //    foreach (HtmlNode node in doc.DocumentNode.Descendants().Where(n => n.Name == "li"))
            //    {
            //        //http://www.youtube.com/results?search_query=yevadu
            //        if (node.Attributes.Where(i => i.Name == "class").FirstOrDefault() != null)
            //        {
            //            try
            //            {
            //                if (node.Attributes["class"].Value.ToString().Contains("yt-lockup clearfix yt-uix-tile result-item-padding yt-lockup-video yt-lockup-tile"))
            //                {

            //                    string title = node.Descendants("a").Where(i => i.Attributes["class"] != null && i.Attributes["class"].Value == "yt-uix-sessionlink yt-uix-tile-link  yt-ui-ellipsis yt-ui-ellipsis-2").FirstOrDefault().InnerText;
            //                    d.title = WebUtility.HtmlDecode(title).Trim();
            //                    d.time = node.Descendants("span").Where(i => i.Attributes["class"] != null && i.Attributes["class"].Value == "video-time").FirstOrDefault().InnerText;
            //                    //d.views = node.Descendants("div").Where(i => i.Attributes["class"].Value == "yt-lockup-meta").FirstOrDefault().Descendants("ul").FirstOrDefault().Elements("li").Skip(2).FirstOrDefault().InnerHtml;

            //                }
            //            }
            //            catch (Exception ex)
            //            {

            //                Exceptions.SaveOrSendExceptions("Exception in OnlineLinks_Loaded event In OnlineLinks.xaml.cs file", ex);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    foreach (HtmlNode node in doc.DocumentNode.Descendants().Where(n => n.Name == "div"))
            //    {

            //        try
            //        {
            //            if (node.Attributes.Where(i => i.Name == "class").FirstOrDefault() != null)
            //            {

            //                if (node.Attributes["class"].Value.ToString() == "yt-lockup clearfix  yt-lockup-video yt-lockup-grid vve-check")
            //                {
            //                    string title = node.Descendants("a").Where(i => i.Attributes["class"] != null && i.Attributes["class"].Value == "yt-uix-sessionlink yt-uix-tile-link  yt-ui-ellipsis yt-ui-ellipsis-2").FirstOrDefault().InnerText;
            //                    d.title = WebUtility.HtmlDecode(title).Trim();
            //                    d.time = node.Descendants("span").Where(i => i.Attributes["class"] != null && i.Attributes["class"].Value == "video-time").FirstOrDefault().InnerText;
            //                    //d.views = node.Descendants("div").Where(i => i.Attributes["class"].Value == "yt-lockup-meta").FirstOrDefault().Descendants("ul").FirstOrDefault().Elements("li").Skip(1).FirstOrDefault().InnerHtml;

            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {

            //            Exceptions.SaveOrSendExceptions("Exception in OnlineLinks_Loaded event In OnlineLinks.xaml.cs file", ex);
            //        }
            //    }
            //}
                    return d;
        }
        public static string gettitle(string data, string key, int i, string stop)
        {
            string source = data.ToString();
            string extract = source.Substring(source.IndexOf(key) + i);
            string result = extract.Substring(0, extract.IndexOf(stop));
            return result;
        }
        public static ObservableCollection<DownloadPivot> LoadVideos(List<Uri> Video)
        {
            ObservableCollection<DownloadPivot> download = new ObservableCollection<DownloadPivot>();
            try
            {

                if (Video.Count() != 0)
                {
                    string path = "";
                    int j = 0;
                    bool fal = false;
                    foreach (Uri u in Video)
                    {
                        if (u.ToString().StartsWith("http://") || u.ToString().StartsWith("https://"))
                        {
                            if (u.ToString().StartsWith("http://www.youtube.com/v/") || u.ToString().StartsWith("https://www.youtube.com/v/") || u.ToString().StartsWith("http://www.youtube.com/embed/") || u.ToString().StartsWith("https://www.youtube.com/embed/"))
                            {
                                string youtubeurl = string.Empty;
                                if (u.ToString().Contains("www.youtube.com/embed/"))
                                    youtubeurl = u.ToString().Substring(u.ToString().LastIndexOf("embed/") + "embed/".Length);
                                else
                                    youtubeurl = u.ToString().Substring(u.ToString().LastIndexOf("v/") + 2);
                                string yurl = youtubeurl.Replace("&amp;hl=en&amp;fs=1&amp;rel=0", "").Replace("?version=3&hl=en_US&rel=0?version=3&iv_load_policy=3&hl=en_US&rel=0", "").Replace("?version=3&amp;hl=en_GB", "").Replace("&hl=hi_IN&fs=1", "").Replace("&hl=en&fs=1&rel=0", "");
                                HttpClient wcl = new HttpClient();
                                wcl.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; Trident/5.0)");
                                HttpResponseMessage response = Task.Run(async () => await wcl.GetAsync("http://www.youtube.com/watch?v=" + yurl)).Result;
                                string htmlData = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
                                //AppSettings.LinkUrl = yurl;
                                //string title = GetTitle().ToString();
                                Constants.NavigatedUri = "http://www.youtube.com/watch?v=" + yurl;
                                DownloadPivot d = new DownloadPivot();
                                string title = gettitle(htmlData, "eow-title", 49, "\">");
                                if (title.Contains("\""))
                                    title = gettitle(htmlData, "eow-title", 60, "\">");
                                //gettimeandviews(htmlData, d);
                                d.title = WebUtility.HtmlDecode(title);
                                d.Downloaduri = u;                                
                                string youtubeID = string.Empty;
                                if (yurl.Contains("&amp;list="))
                                {
                                    //string s = "http://img.youtube.com/vi/3uxe_dAKjrM&amp;list=PLCC3DD92BA7AB726C&amp;index=2/default.jpg";
                                    //string f = yurl.Substring(yurl.IndexOf("vi/") + 3);
                                    youtubeID = yurl.Substring(0, yurl.IndexOf("&"));
                                }
                                else
                                    youtubeID = yurl;
                                d.DownLoadVideoImage = "http://img.youtube.com/vi/" + youtubeID + "/default.jpg";
                                if(d.title!="")
                                download.Add(d);
                            }
                            else if (u.ToString().StartsWith("http://www.dailymotion.com"))
                            {
                                DownloadPivot d = new DownloadPivot();
                                string title = u.ToString().Substring(u.ToString().LastIndexOf("/") + 1);
                                d.title = WebUtility.HtmlDecode(title);
                                d.Downloaduri = u;
                                d.DownLoadVideoImage = "http://www.dailymotion.com/thumbnail/160x120/video/" + u.ToString().Substring(u.ToString().LastIndexOf("/") + 1);
                                if (d.title != "")
                                    download.Add(d);
                            }
                            else if (u.ToString().StartsWith("http://pluralsight.com"))
                            {
                                DownloadPivot d = new DownloadPivot();
                                string title = u.ToString().Substring(u.ToString().LastIndexOf("/") + 1);
                                d.title = WebUtility.HtmlDecode(title);
                                string url = u.ToString().Replace(u.ToString().Substring(u.ToString().LastIndexOf("/")), "");
                                d.Downloaduri = new Uri(url);
                                d.DownLoadVideoImage = "http://s.pluralsight.com/mn/img/cs/courselibrary-header-bg-v1.png";
                                if (d.title != "")
                                    download.Add(d);
                            }
                            else if (u.ToString().StartsWith("http://vimeo.com/") || u.ToString().StartsWith("http://www.veoh.com"))
                            {
                                DownloadPivot d = new DownloadPivot();
                                d.DownLoadVideoImage = Regex.Split(u.ToString(), "&imageurl=")[1];
                                string title = Regex.Split(u.ToString(), "&title=")[1].Replace(d.DownLoadVideoImage, "").Replace("&amp;", "").Replace("&imageurl=", "");
                                d.title = WebUtility.HtmlDecode(title);
                                d.Downloaduri = new Uri(u.ToString());
                                if (d.title != "")
                                    download.Add(d);
                            }
                            else if (u.ToString().StartsWith("http://www.metacafe.com") || u.ToString().StartsWith("http://www.sevenload.com"))
                            {
                                DownloadPivot d = new DownloadPivot();
                                d.DownLoadVideoImage = Regex.Split(u.ToString(), "/ImageUrl=")[1];
                                d.Downloaduri = new Uri(u.ToString().Replace(d.DownLoadVideoImage, "").Replace("/ImageUrl=", "").TrimEnd(new char[] { '/' }));
                                string title = d.Downloaduri.ToString().Substring(d.Downloaduri.ToString().LastIndexOf("/") + 1);
                                d.title = WebUtility.HtmlDecode(title);
                                if (d.title != "")
                                    download.Add(d);
                            }
                            else if (u.ToString().StartsWith("http://www.c-sharpcorner.com"))
                            {
                                DownloadPivot d = new DownloadPivot();
                                d.DownLoadVideoImage = u.ToString().Replace(".wmv", "").Replace(".mp4", "") + ".jpg";
                                d.Downloaduri = u;
                                string title = d.Downloaduri.ToString().Substring(d.Downloaduri.ToString().LastIndexOf("/") + 1);
                                d.title = WebUtility.HtmlDecode(title);
                                if (d.title != "")
                                    download.Add(d);
                            }
                            else if (u.ToString().Contains(".ch9"))
                            {
                                if (u.ToString().EndsWith(".wmv"))
                                {
                                    DownloadPivot d = new DownloadPivot();
                                    string title = u.ToString().Substring(u.ToString().LastIndexOf("/") + 1);
                                    d.title = WebUtility.HtmlDecode(title);
                                    d.Downloaduri = u;
                                    d.DownLoadVideoImage = u.ToString().Replace(".wmv", "").Replace(".mp4", "") + "_512.jpg";
                                    if (d.title != "")
                                        download.Add(d);
                                }
                            }
                            else
                            {
                                DownloadPivot d = new DownloadPivot();
                                string title = u.ToString().Substring(u.ToString().LastIndexOf("/") + 1);
                                d.title = WebUtility.HtmlDecode(title);
                                d.Downloaduri = u;
                                if (u.ToString().StartsWith("http://www.promptfile.com"))
                                {
                                    string[] split = u.ToString().Split('=');
                                    d.title = split[1];
                                }
                                if (d.title != "")
                                    download.Add(d);
                            }
                        }
                        else
                        {
                            DownloadPivot d = new DownloadPivot();
                            path = AppSettings.starturidownloadmanger + u.AbsolutePath.ToString();
                            string title = path.Substring(path.ToString().LastIndexOf("/") + 1);
                            d.title = WebUtility.HtmlDecode(title);
                            d.Downloaduri = new Uri(path);

                            if (d.title != "")
                                download.Add(d);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string mess = "Exception in LoadDownLoadVideos Method In DownloadManagerHelper file\n\n" + ex.Message + " \n\n Stack Trace \n" + ex.StackTrace;
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions(mess, ex);
            }
            return download;
        }

        public static ObservableCollection<DownloadPivot> LoadImages(List<Uri> Image)
        {
            ObservableCollection<DownloadPivot> download = new ObservableCollection<DownloadPivot>();
            try
            {
                string path;
                if (Image.Count() != 0)
                {
                    foreach (Uri u in Image)
                    {
                        if (u.ToString().StartsWith("http://"))
                        {
                            DownloadPivot d = new DownloadPivot();
                            d.Downloadimage = new BitmapImage(new Uri(u.ToString(), UriKind.Absolute));
                            if (u.ToString().StartsWith("http://img.youtube.com/vi/"))
                                d.title = u.ToString().Replace("http://img.youtube.com/vi/", "").Replace("/", "");
                            else
                                d.title = u.ToString().Substring(u.ToString().LastIndexOf("/") + 1);
                            d.Downloaduri = u;
                            if (d.title != "")
                                download.Add(d);
                        }
                        else
                        {
                            DownloadPivot d = new DownloadPivot();
                            path = u.AbsolutePath.ToString();
                            d.Downloadimage = new BitmapImage(new Uri(path, UriKind.Absolute));
                            d.title = path.Substring(path.ToString().LastIndexOf("/") + 1);

                            d.Downloaduri = new Uri(path);
                            if (d.title != "")
                                download.Add(d);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in LoadImages Method In DownloadManagerHelper file", ex);
            }
            return download;
        }

        public static ObservableCollection<DownloadPivot> LoadAudios(List<Uri> Audio)
        {
            ObservableCollection<DownloadPivot> download = new ObservableCollection<DownloadPivot>();
            try
            {
                if (Audio.Count() != 0)
                {
                    string path;
                    foreach (Uri u in Audio)
                    {
                        if (u.ToString().StartsWith("http://"))
                        {
                            DownloadPivot d = new DownloadPivot();
                            d.title = u.ToString().Substring(u.ToString().LastIndexOf("/") + 1);
                            d.Downloaduri = u;
                            if (d.title != "")
                                download.Add(d);
                        }
                        else
                        {
                            DownloadPivot d = new DownloadPivot();
                            path = AppSettings.starturidownloadmanger + u.AbsolutePath.ToString();
                            d.title = path.Substring(path.ToString().LastIndexOf("/") + 1);
                            d.Downloaduri = new Uri(path);
                            if (d.title != "")
                                download.Add(d);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string mess = "Exception in LoadDownloadAudios Method In DownloadManagerHelper file\n\n" + ex.Message + " \n\n Stack Trace \n" + ex.StackTrace;
                ex.Data.Add("movieId", AppSettings.ShowID);
                Exceptions.SaveOrSendExceptions(mess, ex);
            }
            return download;
        }

        public static ObservableCollection<DownloadPivot> LinksForChannelch(ObservableCollection<DownloadPivot> download)
        {
            try
            {
                var httpClient = new HttpClient();
                var url = new Uri(AppSettings.NavigationUrl);
                var accessToken = "1234";
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                httpRequestMessage.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken));
                httpRequestMessage.Headers.Add("User-Agent", "My user-Agent");
                var response = Task.Run(async () => await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead)).Result;
                string html = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
                MatchCollection Gettbody = Regex.Matches(html, @"\<tbody\s?.*?\>((.|\r\n)+?)\<\/tbody\>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match body in Gettbody)
                {
                    if (body.ToString().StartsWith("<tbody>"))
                    {
                        Match GetUrl = Regex.Match(body.ToString(), @"<a[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        string LinkUrl = GetUrl.Groups[1].Value;
                        if (!LinkUrl.StartsWith("http://"))
                        {
                            LinkUrl = "http://www.1channel.ch" + LinkUrl;
                        }
                        Match GetTitle = Regex.Match(body.ToString(), @"\<script\s?.*?\>((.|\r\n)+?)\<\/script\>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        string title = GetTitle.Groups[1].Value.Replace("document.writeln", "").Replace("(", "").Replace(")", "").Replace("'", "").Replace(";", "");
                        DownloadPivot d = new DownloadPivot();
                        d.title = WebUtility.HtmlDecode(title);
                        d.Downloaduri = new Uri(LinkUrl);
                        d.height = "44";
                        if (d.title != "")
                            download.Add(d);
                    }
                }
                MatchCollection GetHerf = Regex.Matches(html, @"<a[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

                foreach (Match her in GetHerf)
                {
                    if (her.ToString().StartsWith("<a href"))
                    {
                        string split = her.ToString().Substring(her.ToString().LastIndexOf("title") + 5).Replace("=", "").Replace("\"", "").Replace(">", "");
                        string href = her.Groups[1].Value;
                        if (!href.StartsWith("http://"))
                        {
                            href = "http://www.1channel.ch" + href;
                        }
                        DownloadPivot d = new DownloadPivot();
                        d.title = split;
                        d.Downloaduri = new Uri(href);
                        d.height = "44";
                        if (d.title != "")
                            download.Add(d);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in LinksForChannelch Method In DownloadManagerHelper file", ex);
            }
            return download;
        }

        public static ObservableCollection<DownloadPivot> GetLinkTitleForDailymotionSite(ObservableCollection<DownloadPivot> download, Uri u)
        {
            try
            {
                string title = string.Empty;
                DownloadPivot d = new DownloadPivot();
                if (u.ToString().EndsWith("/1") || u.ToString().EndsWith("/in") || u.ToString().EndsWith("/2") || u.ToString().EndsWith("/3") || u.ToString().EndsWith("/4") || u.ToString().EndsWith("/5") || u.ToString().EndsWith("/6") || u.ToString().EndsWith("/7") || u.ToString().EndsWith("/8"))
                    title = u.ToString().Replace(u.ToString().Substring(u.ToString().LastIndexOf("/")), "").Replace(u.ToString().Substring(u.ToString().LastIndexOf("/")), "").Substring(u.ToString().Replace(u.ToString().Substring(u.ToString().LastIndexOf("/")), "").Replace(u.ToString().Substring(u.ToString().LastIndexOf("/")), "").LastIndexOf("/") + 1);
                else
                    title = u.ToString().Substring(u.ToString().LastIndexOf("/"));
                title = title.Replace("/", "");
                d.title = WebUtility.HtmlDecode(title);
                d.Downloaduri = u;
                d.height = "44";
                if (d.title != "")
                    download.Add(d);
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in GetLinkTitleForDailymotionSite Method In DownloadManagerHelper file", ex);
            }
            return download;
        }

        public static ObservableCollection<DownloadPivot> LoadLinks(List<Uri> Link)
        {
            ObservableCollection<DownloadPivot> download = new ObservableCollection<DownloadPivot>();
            try
            {
                if (AppSettings.NavigationUrl.ToString().StartsWith("http://www.1channel.ch/"))
                {
                    download = DownloadManagerHelper.LinksForChannelch(download);
                }
                else
                {
                    if (Link.Count() != 0)
                    {
                        string path;
                        foreach (Uri u in Link)
                        {
                            if (u.ToString().StartsWith("http://"))
                            {
                                if (u.ToString().StartsWith("http://www.dailymotion.com"))
                                {
                                    download = DownloadManagerHelper.GetLinkTitleForDailymotionSite(download, u);
                                }
                                else if (u.ToString().StartsWith("http://vimeo.com"))
                                {
                                    DownloadPivot d = new DownloadPivot();
                                    string title = u.ToString().Replace("http://vimeo.com/", "").Replace("/", "-").Replace("http:", "").Replace("--", "");
                                    d.title = WebUtility.HtmlDecode(title);
                                    d.Downloaduri = u;
                                    d.height = "44";
                                    if (d.title != "")
                                        download.Add(d);
                                }
                                else
                                {
                                    DownloadPivot d = new DownloadPivot();
                                    string title = u.ToString().Substring(u.ToString().LastIndexOf("/") + 1).Replace("-", "").Replace("_", "").Replace(".html", "");
                                    d.title = WebUtility.HtmlDecode(title);
                                    d.Downloaduri = u;
                                    d.height = "44";
                                    if (d.title != "")
                                        download.Add(d);
                                }
                            }
                            else
                            {
                                DownloadPivot d = new DownloadPivot();
                                path = u.AbsoluteUri.ToString();
                                string title = path.Substring(path.ToString().LastIndexOf("/") + 1).Replace("-", "").Replace("_", "").Replace(".html", "");
                                d.title = WebUtility.HtmlDecode(title);
                                d.Downloaduri = new Uri(path);
                                d.height = "44";
                                if (d.title != "")
                                    download.Add(d);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in LoadLinks Method In DownloadManagerHelper.cs file", ex);
            }
            return download;
        }

        public static List<Uri> GetLinksForPluralsight(List<Uri> Link)
        {
            try
            {
                var httpClient = new HttpClient();
                var url = new Uri(AppSettings.NavigationUrl);
                var accessToken = "1234";
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                httpRequestMessage.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken));
                httpRequestMessage.Headers.Add("User-Agent", "My user-Agent");
                var response = Task.Run(async () => await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead)).Result;
                string html = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;

                MatchCollection Gettbody = Regex.Matches(html, @"<a[^>]*?class\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match body in Gettbody)
                {
                    if (!body.ToString().StartsWith("<tbody>"))
                    {
                        Match GetUrl = Regex.Match(body.ToString(), @"<a[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        string LinkUrl = GetUrl.Groups[1].Value;
                        if (LinkUrl.StartsWith("/training/Courses"))
                            Link.Add(new Uri("http://pluralsight.com" + LinkUrl));
                    }
                }
                Constants.Savehtml = html;
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in GetLinksForPluralsight Method In DownloadManagerHelper.cs file", ex);
            }
            return Link;
        }

        public static List<Uri> GetPlayerUrlForPluralsight(List<Uri> Video)
        {

            try
            {
                string AuthorName = string.Empty;
                string TopicTitle = string.Empty;
                MatchCollection Gettbody = Regex.Matches(Constants.Savehtml, @"<a[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match body in Gettbody)
                {
                    if (!body.ToString().StartsWith("<tbody>"))
                    {
                        Match GetUrl = Regex.Match(body.ToString(), @"<a[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        string LinkUrl = GetUrl.Groups[0].Value;
                        if (LinkUrl.Contains("http://pluralsight.com/training/Authors/Details"))
                        {
                            string Suburl = LinkUrl.Replace("<a href=", "").Replace(">", "").Split('"')[1];
                            AuthorName = Suburl.Substring(Suburl.LastIndexOf("/") + 1);
                            //videourl.Add(LinkUrl.Replace("<a href=", "").Replace(">", ""));
                        }
                    }
                }
                MatchCollection Gettbody1 = Regex.Matches(Constants.Savehtml, @"<meta[^>]*?property\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match body in Gettbody1)
                {
                    if (!body.ToString().StartsWith("<tbody>"))
                    {
                        Match GetUrl = Regex.Match(body.ToString(), @"<meta[^>]*?property\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        string LinkUrl = GetUrl.Groups[0].Value;
                        if (LinkUrl.Contains("http://pluralsight.com/courses"))
                        {
                            string Suburl = LinkUrl.Replace("<meta property=", "").Replace(">", "").Split('"')[3];
                            TopicTitle = Suburl.Substring(Suburl.LastIndexOf("/") + 1);
                            //GetVideosId.Add(LinkUrl.Replace("<a href=", "").Replace(">", ""));
                        }

                    }
                }
                MatchCollection Gettbody2 = Regex.Matches(Constants.Savehtml, @"<tr[^>]*?id\s?.*?\>((.|\r\n)+?)\<\/tr\>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match body in Gettbody2)
                {
                    if (!body.ToString().StartsWith("<tbody>"))
                    {
                        Match GetUrl = Regex.Match(body.ToString(), @"<tr[^>]*?id\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        string LinkUrl = GetUrl.Groups[1].Value;
                        Match GetUrlTitle = Regex.Match(body.ToString(), @"<div[^>]*?class\s?.*?\>((.|\r\n)+?)\<\/div\>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        string Title = GetUrlTitle.Groups[1].Value.Split('>')[1];
                        string PlayerUrl = "http://pluralsight.com/training/Player?author=" + AuthorName + "&name=" + LinkUrl + "&mode = live & clip =" + 0 + "&course=" + TopicTitle + "/" + Title;
                        Video.Add(new Uri(PlayerUrl.Replace(" ", "")));
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in GetPlayerUrlForPluralsight Method In DownloadManagerHelper.cs file", ex);
            }
            return Video;
        }

        public static void GetWebContent(string[] regexImgSrc)
        {
            try
            {
                if (AppSettings.NavigationUrl.StartsWith("http://www.1channel.ch") || AppSettings.NavigationUrl.StartsWith("http://vodly.to"))
                {
                    Constants.SaveVideos = DownloadManagerHelper.GetUrlForsharesixAndPromptfile(Constants.SaveVideos);
                }
                if (AppSettings.NavigationUrl.StartsWith("http://www.dailymotion.com"))
                {
                    DownloadManagerHelper.GetVideoLinksFromDailymotionSite();
                }
                if (AppSettings.NavigationUrl.Contains("pluralsight"))
                {
                    Constants.SaveLinks = DownloadManagerHelper.GetLinksForPluralsight(Constants.SaveLinks);
                    Constants.SaveVideos = DownloadManagerHelper.GetPlayerUrlForPluralsight(Constants.SaveVideos);
                }

                var httpClient = new HttpClient();
                var url = new Uri(AppSettings.NavigationUrl);
                var accessToken = "1234";
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                httpRequestMessage.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken));
                httpRequestMessage.Headers.Add("User-Agent", "My user-Agent");
                var response = Task.Run(async () => await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead)).Result;
                string htmlDataReload = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
                if (AppSettings.NavigationUrl.Contains("in.com"))
                {
                    GetVideoUrlsFromINCom(htmlDataReload);
                }
                if (AppSettings.NavigationUrl.Contains("vimeo.com"))
                {
                    GetVideoUrlsForVimeo(htmlDataReload);
                }
                if (AppSettings.NavigationUrl.Contains("veoh.com"))
                {
                    GetvideoUrlForVeohSite(htmlDataReload);
                }
                if (AppSettings.NavigationUrl.Contains("bharatmovies.com"))
                {
                    GetvideourlsFromBharthMovies(htmlDataReload);
                }
                if (AppSettings.NavigationUrl.Contains("mobilesongs.audio.pk") || AppSettings.NavigationUrl.Contains("hindisongs.audio.pk"))
                {
                    GetAudiosFromMobileSongssite(htmlDataReload);
                }
                if (AppSettings.NavigationUrl.Contains("teluguone"))
                {
                    GetTeluguOneSiteMovies(htmlDataReload);
                }
                if (AppSettings.NavigationUrl.Contains("cinevedika"))
                {
                    GetCinevedikaVideoUrls(htmlDataReload);
                }
                if (AppSettings.NavigationUrl.Contains("songslover.pk"))
                {
                    GetLoveOngsPkSiteVideoUrls(htmlDataReload);
                }
                if (AppSettings.NavigationUrl.Contains("onlinewatchmovies") || AppSettings.NavigationUrl.Contains("manatelugumovies") || AppSettings.NavigationUrl.Contains("c-sharpcorner"))
                {
                    GetVideoUrlsFromOnlinewatchMoviesSite(htmlDataReload);
                }
                if (AppSettings.NavigationUrl.Contains("metacafe"))
                {
                    GetLinksAndVideoLinksFromMetaCafeSite(htmlDataReload);
                }
                if (AppSettings.NavigationUrl.Contains("sevenload"))
                {
                    GetVideoLinksForSevenloadSite(htmlDataReload);
                }
                if (response.StatusCode.ToString() != "OK")
                {
                    // Exceptions.SaveOrSendHttpclientExceptions("Exception in GetHtmlContent Method In DownloadManagerDailyMotion.cs file", response.ReasonPhrase, response.RequestMessage.RequestUri.ToString());
                }
                Match mth = Regex.Match(htmlDataReload, @"\<Title\s?.*?\>((.|\r\n)+?)\<\/Title\>");
                string href;
                string hrefforvip;
                for (int i = 0; i < regexImgSrc.Count(); i++)
                {
                    MatchCollection matchesImgSrc = Regex.Matches(htmlDataReload, regexImgSrc[i], RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    foreach (Match m in matchesImgSrc)
                    {
                        href = m.Groups[1].Value;

                        if (href.ToString().EndsWith(".3gp") || href.ToString().StartsWith("http://serials.") || href.ToString().EndsWith(".3g2") || href.ToString().StartsWith("http://www.youtube.com/v/") || href.ToString().StartsWith("#/watch?feature=related&amp;v=") || href.ToString().StartsWith("/watch?v=") || href.ToString().StartsWith("#/watch?feature=relmfu&amp;v=") || href.ToString().EndsWith(".mp4") || href.ToString().EndsWith(".m4v") || href.ToString().EndsWith(".wmv") || href.StartsWith("/watch?v=") || href.ToString().StartsWith("http://vipjatt.com/album/video") || href.ToString().StartsWith("http://vipjatt.com/album/video") || href.ToString().StartsWith("https://www.youtube.com") || href.ToString().Contains("//www.youtube.com/embed/"))
                        {

                            if (href.ToString().StartsWith("//www.youtube.com/embed/"))
                                href = href.ToString().Replace(@"//www.youtube.com/embed/", "http://www.youtube.com/embed/");
                            if (href.ToString().StartsWith("http://serials.") || href.ToString().StartsWith("#/watch?feature=related&amp;v=") || href.ToString().EndsWith("&hl=en&fs=1&rel=0") || href.ToString().StartsWith("/watch?v=") || href.ToString().StartsWith("http://vipjatt.com/album/video") || ((href.ToString().StartsWith("http://www.youtube.com/v/")) && (href.ToString().EndsWith("&amp;hl=en&amp;fs=1&amp;rel=0"))) || href.ToString().StartsWith("#/watch?feature=relmfu&amp;v=") || href.ToString().StartsWith("/watch?v=") || href.StartsWith("/watch?v=") || ((href.ToString().StartsWith("#/watch?v=")) && (href.ToString().EndsWith("&amp;feature=relmfu"))))
                            {
                                string youtubeid;
                                if (href.StartsWith("http://vipjatt.com/album/video"))
                                {
                                    youtubeid = href.Substring(href.LastIndexOf("/") + 1).Replace(".html", "");
                                }
                                else
                                {
                                    youtubeid = href.ToString().Replace("#/watch?feature=related&amp;v=", "").Replace("/watch?v=", "").Replace("&amp;feature=relmfu", "").Replace("#/watch?feature=relmfu&amp;v=", "").Replace("http://www.youtube.com/v/", "").Replace("&amp;hl=en&amp;fs=1&amp;rel=0", "").Replace("&hl=en&fs=1&rel=0", "").Replace("&amp;wide=1", "").Replace("http://serials.telugudailyserials.com/cv.php?url=", "").Replace("&source=youtube", "").Replace("?version=3&hl=en_US&rel=0?version=3&iv_load_policy=3&hl=en_US&rel=0", "");
                                }
                                string orgyoutubeid = "http://www.youtube.com/v/" + youtubeid + "&amp;hl=en&amp;fs=1&amp;rel=0";
                                if (!Constants.SaveVideos.Contains(new Uri(orgyoutubeid)))
                                {
                                    Constants.SaveVideos.Add(new Uri(orgyoutubeid));
                                    string imgur = "http://img.youtube.com/vi/" + youtubeid + "/default.jpg";
                                    Constants.SaveImages.Add(new Uri(imgur));
                                }
                            }
                            else
                            {
                                if (href.ToString().StartsWith("//www.youtube.com/embed/"))
                                    href = href.ToString().Replace(@"//www.youtube.com/embed/", "http://www.youtube.com/embed/");
                                if (!Constants.SaveVideos.Contains(new Uri(href)))
                                    Constants.SaveVideos.Add(new Uri(href));
                            }
                        }
                        href = m.Groups[1].Value;
                        if (href.ToString().EndsWith(".mp3") || href.StartsWith("http://link1.songspk.pk/") || href.StartsWith("http://link1.songspk.name/") || href.EndsWith("=low") || href.ToString().EndsWith(".wav") || href.ToString().EndsWith(".aac") || href.ToString().EndsWith(".amr") || href.ToString().EndsWith(".wma"))
                        {
                            if (AppSettings.NavigationUrl.Contains("ibiblio"))
                            { href = "http://www.ibiblio.org/ram/" + href; }
                            if (href.StartsWith("http://"))
                            {
                                if (href.EndsWith("=low"))
                                {
                                    string hrf = href + ".mp3";
                                    href = hrf;
                                }
                                if (href.StartsWith("http://link1.songspk.pk/") || href.StartsWith("http://link1.songspk.name/"))
                                {
                                    href = GetLovePkAudiosongs(href);
                                }
                                if (!Constants.SaveAudios.Contains(new Uri(href)))
                                    Constants.SaveAudios.Add(new Uri(href));
                            }
                            else
                            {
                                string validurl = AppSettings.NavigationUrl + href;
                                if (!Constants.SaveAudios.Contains(new Uri(validurl)))
                                    Constants.SaveAudios.Add(new Uri(validurl));
                            }
                        }
                        if (i == 3)
                        {
                            href = m.Groups[1].Value;
                            if (href.StartsWith("http"))
                            {
                                try
                                {
                                    Constants.SaveImages.Add(new Uri(href));
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                        if (i == 0 || i == 5)
                        {
                            try
                            {
                                href = m.Groups[1].Value;
                                if (href.Contains("www."))
                                {
                                    if (!href.Contains(".xml"))
                                    {
                                        Constants.SaveLinks.Add(new Uri(href));
                                    }
                                    if (href.ToString().StartsWith("http://www.dailymotion.com") && href.ToString().ToLower().Contains("autoPlay="))
                                    {
                                        if (!Constants.SaveVideos.Contains(new Uri(href)))
                                            Constants.SaveVideos.Add(new Uri(href));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                string mess = "Exception in OnNavigatedTo Method In DownloadPivots file\n\n" + ex.Message + " \n\n Stack Trace \n" + ex.StackTrace;
                                Exceptions.SaveOrSendExceptions(mess, ex);
                            }
                        }
                        if (Constants.SaveVideos.Count == 10)
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in GetWebContent Method In DownloadManagerHelper.cs file", ex);
            }
        }

        public static void GetVideoLinksFromDailymotionSite()
        {
            try
            {
                List<string> videolink = new List<string>();
                var httpClient = new HttpClient();
                var url = new Uri(AppSettings.NavigationUrl);
                var accessToken = "1234";
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                httpRequestMessage.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken));
                httpRequestMessage.Headers.Add("User-Agent", "My user-Agent");
                var response = Task.Run(async () => await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead)).Result;
                string html = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
                MatchCollection Gettbody = Regex.Matches(html, @"<a[^>]*?class\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match body in Gettbody)
                {
                    Match GetUrl = Regex.Match(body.ToString(), @"<a[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    string LinkUrl = GetUrl.Groups[1].Value;
                    if (LinkUrl.StartsWith("/video"))
                    {
                        if (!videolink.Contains(LinkUrl))
                        {
                            videolink.Add(LinkUrl);
                            Constants.SaveVideos.Add(new Uri("http://www.dailymotion.com" + LinkUrl));
                        }
                    }
                    if (!LinkUrl.StartsWith("http://") && LinkUrl.StartsWith("/in"))
                    {
                        LinkUrl = "http://www.dailymotion.com" + LinkUrl;
                        Constants.SaveLinks.Add(new Uri(LinkUrl));
                    }


                }
                if (response.StatusCode.ToString() != "OK")
                {
                    Exceptions.SaveOrSendHttpclientExceptions("Exception in GetHtmlContent Method In DownloadManagerDailyMotion.cs file", response.ReasonPhrase, response.RequestMessage.RequestUri.ToString());
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in GetVideoLinksFromDailymotionSite Method In DownloadManagerHelper.cs file", ex);

            }
        }

        public static IEnumerable<DependencyObject> GetChildsRecursive(DependencyObject root)
        {
            List<DependencyObject> elts = new List<DependencyObject>();
            try
            {
                elts.Add(root);

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
                {
                    elts.AddRange(GetChildsRecursive(VisualTreeHelper.GetChild(root, i)));
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetChildsRecursive Method In DownloadManagerHelper.cs file", ex);
            }
            return elts;
        }

        public static DownloadPivot GetTitleForHorzentalList()
        {
            DownloadPivot dn = new DownloadPivot();
            try
            {

                dn.Downloaduri = new Uri(AppSettings.NavigationUrl);
                var httpClient = new HttpClient();
                var url1 = new Uri(AppSettings.NavigationUrl);
                var accessToken1 = "1234";
                var httpRequestMessage1 = new HttpRequestMessage(HttpMethod.Get, url1);
                httpRequestMessage1.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken1));
                httpRequestMessage1.Headers.Add("User-Agent", "My user-Agent");
                var response1 = Task.Run(async () => await httpClient.SendAsync(httpRequestMessage1, HttpCompletionOption.ResponseHeadersRead)).Result;
                string html1 = Task.Run(async () => await response1.Content.ReadAsStringAsync()).Result;
                dn.title = Regex.Match(html1, @"\<title\s?.*?\>((.|\r\n)+?)\<\/title\>", RegexOptions.IgnoreCase).ToString().Replace("<title>", "").Replace("</title>", "");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetTitleForHorzentalList Method In DownloadManagerHelper.cs file", ex);

            }
            return dn;
        }

        public static List<string> CheckBoxCheckedItemList(GridView GridviewName)
        {
            List<string> chkboxes = new List<string>();
            try
            {
                IEnumerable<DependencyObject> cboxes = null;
                cboxes = DownloadManagerHelper.GetChildsRecursive(GridviewName);
                foreach (DependencyObject obj in cboxes.OfType<CheckBox>())
                {
                    Type type = obj.GetType();
                    if (type.Name == "CheckBox")
                    {
                        CheckBox cb = obj as CheckBox;
                        if (cb.IsChecked == true)
                        {
                            if (!chkboxes.Contains(cb.Tag.ToString()))
                                chkboxes.Add(cb.Tag.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in CheckedItemList Method In DownloadManagerHelper.cs file", ex);

            }
            return chkboxes;
        }

        public static List<string> CheckBoxUnCheckedItemList(GridView GridviewName)
        {
            List<string> chkboxes = new List<string>();
            try
            {
                IEnumerable<DependencyObject> cboxes = null;
                cboxes = DownloadManagerHelper.GetChildsRecursive(GridviewName);
                foreach (DependencyObject obj in cboxes.OfType<CheckBox>())
                {
                    Type type = obj.GetType();
                    if (type.Name == "CheckBox")
                    {
                        CheckBox cb = obj as CheckBox;
                        if (cb.IsChecked == true)
                        {
                            if (!chkboxes.Contains(cb.Tag.ToString()))
                                chkboxes.Remove(cb.Tag.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in CheckBoxUnCheckedItemList Method In DownloadManagerHelper.cs file", ex);

            }
            return chkboxes;
        }
        //public void  Gettimeandviews(string RssDoc)
        //{
        //    HtmlDocument doc = new HtmlDocument();
        //    doc.LoadHtml(RssDoc);
        //    foreach (HtmlNode node in doc.DocumentNode.DescendantNodes().Where(n => n.Name == "li"))
        //    {
        //        try
        //        {
        //            //http://www.youtube.com/watch?v=uIDx3eUZ-vw
        //            if (node.Attributes.Where(i => i.Name == "class").FirstOrDefault() != null)
        //            {
        //                if (node.Attributes["class"].Value.ToString().Contains("video-list-item"))
        //                {
        //                    //Prop p = new Prop();
        //                    //string hrefvalue = node.Descendants("a").FirstOrDefault().Attributes.Where(i => i.Name == "href").FirstOrDefault().Value.ToString();
        //                    //p.VideoID = hrefvalue.Substring(hrefvalue.LastIndexOf('=') + 1);
        //                    //p.VideoTitle = WebUtility.HtmlDecode(node.Descendants("span").Where(i => i.Attributes["title"] != null).FirstOrDefault().Attributes["title"].Value);
        //                    p.Duration = node.Descendants("span").Where(i => i.Attributes["class"] != null && i.Attributes["class"].Value == "video-time").FirstOrDefault().InnerText;
        //                    p.Views = node.Descendants("span").Where(i => i.Attributes["class"] != null && i.Attributes["class"].Value == "stat view-count").FirstOrDefault().InnerText.Trim();
        //                    //p.ImageUrl = "http://i1.ytimg.com/vi/" + p.VideoID + "/default.jpg";
        //                    listprop.Add(p);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            Exceptions.SaveOrSendExceptions("Exception in OnlineLinks_Loaded event In OnlineLinks.xaml.cs file", ex);
        //        }
        //    }
        //}
        
        
        //public static string GetTitle(string RssDoc)
        //{
        //    string str14 = string.Empty;
        //    try
        //    {
        //        str14 = YoutubeTitle.GetTxtBtwn(RssDoc, "'VIDEO_TITLE': '", "'", 0);
        //        if (str14 == "") str14 = YoutubeTitle.GetTxtBtwn(RssDoc, "\"title\" content=\"", "\"", 0);
        //        if (str14 == "") str14 = YoutubeTitle.GetTxtBtwn(RssDoc, "&title=", "&", 0);
        //        str14 = str14.Replace(@"\", "").Replace("'", "&#39;").Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("+", " ");
        //        AppSettings.ShowLinkTitle = str14;
        //    }
        //    catch (Exception ex)
        //    {
        //        Exceptions.SaveOrSendExceptions("Exception in GetTitle Method In DownloadManagerHelper.cs file", ex);
        //    }
        //    return str14;
        //}

        public static Task<string> GetTitle()
        {            
            Task<string> str14 = MyToolkit.Multimedia.YouTube.GetVideoTitleAsync(AppSettings.LinkUrl, CancellationToken.None);
            AppSettings.ShowLinkTitle = str14.ToString();
            return str14;
        }

        public static void GetDownLoadTableInformation()
        {
            int size;
            try
            {
                int i = 0;
                List<DownloadInfo> objVideoList = new List<DownloadInfo>();
                objVideoList = DownloadManager.LoadDownloadUrls();
                int countlist = objVideoList.Count();
                if (countlist > 0)
                {
                    var xquery = (from q in objVideoList select q);
                    foreach (var itm in xquery)
                    {
                        if (!itm.LinkUrl.StartsWith("http://www.dailymotion.com") && !itm.LinkUrl.StartsWith("http://www.youtube.com/watch?v=") && !itm.LinkUrl.StartsWith("http://vimeo.com/") && !itm.LinkUrl.StartsWith("http://www.veoh.com") && !itm.LinkUrl.StartsWith("http://www.metacafe.com") && !itm.LinkUrl.StartsWith("http://www.sevenload.com"))
                        {
                            i++;
                            string link = "";
                            link = itm.LinkUrl;
                            string name = "";
                            string Title = "";
                            if (itm.LinkUrl.ToString().StartsWith("http://img.youtube.com/vi/"))
                                Title = itm.LinkUrl.ToString().Replace("http://img.youtube.com/vi/", "").Replace("/", "");
                            else
                                Title = itm.LinkUrl.ToString().Substring(itm.LinkUrl.ToString().LastIndexOf('/') + 1);
                            if (name.Contains(".jpg"))
                            {
                                //tblkNoDownloads.Visibility = Visibility.Visible;
                            }
                            var ln = link;
                            if (itm.LinkUrl.StartsWith("http://www.promptfile.com"))
                            {
                                string[] split = itm.LinkUrl.Split('=');
                                link = split[0];
                                Title = split[1];
                            }
                            var httpClient = new HttpClient();
                            var url = new Uri(link);
                            var accessToken = "1234";
                            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                            httpRequestMessage.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken));
                            httpRequestMessage.Headers.Add("User-Agent", "My user-Agent");
                            var response = Task.Run(async () => await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead)).Result;
                            size = Convert.ToInt32(response.Content.Headers.ContentLength);
                            string foldername = DownloadManager.GetFolderName(Title);
                            name = Title.Replace(":", "").Replace("|", "").Replace("&", "").Replace("#", "").Replace(";", "").Replace("/", "").Replace("?", "").Replace("=", "");
                            Constants.LoadDownloadList = LoadList(name, link, Convert.ToInt32(itm.id), size, foldername);
                            DownloadClick(link, name, itm.id);
                            DownloadManager.DeleteDownloadUrls(Convert.ToInt32(itm.id));
                        }
                        else if (itm.LinkUrl.StartsWith("http://www.youtube.com/watch?v="))
                        {
                            try
                            {
                                string link = "";
                                var httpClient = new HttpClient();
                                var url = new Uri(itm.LinkUrl);
                                var accessToken = "1234";
                                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                                httpRequestMessage.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken));
                                httpRequestMessage.Headers.Add("User-Agent", "My user-Agent");
                                var response = Task.Run(async () => await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead)).Result;
                                string html = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
                                GetYotubeUrl(html);
                                size = Convert.ToInt32(response.Content.Headers.ContentLength);
                                GetYouTubeVideoTitle();
                                link = AppSettings.YoutubeUri + "&title=" + AppSettings.ShowLinkTitle;
                                string name = "";
                                name = AppSettings.ShowLinkTitle.Replace(":", "").Replace("|", "").Replace(".flv", "").Replace(".", "").Replace("&", "").Replace("#", "").Replace(";", "").Replace("/", "").Replace("?", "").Replace("=", "");
                                string foldername = DownloadManager.GetFolderName(name + ".mp4");
                                Constants.LoadDownloadList = LoadList(name + ".mp4", link, Convert.ToInt32(itm.id), size, foldername);
                                DownloadClick(link, name + ".mp4", itm.id);
                                DownloadManager.DeleteDownloadUrls(Convert.ToInt32(itm.id));
                            }
                            catch (Exception ex)
                            {
                                Exceptions.SaveOrSendExceptions("Exception in GetDownLoadTableInformation Method In DownloadManagerHelper.cs file", ex);
                            }
                        }
                        else if (itm.LinkUrl.StartsWith("http://www.dailymotion.com"))
                        {
                            string link = "";
                            DownloadManagerDailyMotion.dailyDownload(itm.LinkUrl, dailyQuality.hd720);
                            size = Constants.VideofileSize;
                            link = AppSettings.YoutubeUri + "&title=" + itm.LinkUrl.Substring(itm.LinkUrl.LastIndexOf("/") + 1);
                            string name = "";
                            name = itm.LinkUrl.Substring(itm.LinkUrl.LastIndexOf("/")).Replace(":", "").Replace("|", "").Replace(".flv", "").Replace(".", "").Replace("&", "").Replace("#", "").Replace(";", "").Replace("/", "").Replace("?", "").Replace("=", "");
                            string foldername = DownloadManager.GetFolderName(name + ".mp4");
                            Constants.LoadDownloadList = LoadList(name + ".mp4", link, Convert.ToInt32(itm.id), size, foldername);
                            DownloadClick(link, name + ".mp4", itm.id);
                            DownloadManager.DeleteDownloadUrls(Convert.ToInt32(itm.id));
                        }
                        else if (itm.LinkUrl.StartsWith("http://vimeo.com/") || itm.LinkUrl.StartsWith("http://www.veoh.com"))
                        {
                            string link = "";
                            String playurl = Regex.Split(itm.LinkUrl.ToString(), "&imageurl=")[1];
                            string downloadurl = Regex.Split(itm.LinkUrl.ToString(), "&title=")[0].Replace(playurl, "");
                            if (itm.LinkUrl.StartsWith("http://vimeo.com/"))
                                AppSettings.YoutubeUri = DownloadManagerDailyMotion.GetDownloadvideoUrlforVimeoSite(downloadurl);
                            if (itm.LinkUrl.StartsWith("http://www.veoh.com"))
                                AppSettings.YoutubeUri = DownloadManagerDailyMotion.GetVideoUrlForVeohSite(downloadurl);
                            size = Constants.VideofileSize;
                            link = AppSettings.YoutubeUri;
                            string name = Regex.Split(itm.LinkUrl.ToString().ToString(), "&title=")[1].Replace(playurl, "").Replace("&amp;", "").Replace("&imageurl=", "").Replace(@"""", "");
                            name = name.Replace(":", "").Replace("|", "").Replace(".flv", "").Replace(".", "").Replace("&", "").Replace("#", "").Replace(";", "").Replace("/", "").Replace("?", "").Replace("=", "");
                            string foldername = DownloadManager.GetFolderName(name + ".mp4");
                            Constants.LoadDownloadList = LoadList(name + ".mp4", link, Convert.ToInt32(itm.id), size, foldername);
                            DownloadClick(link, name + ".mp4", itm.id);
                            DownloadManager.DeleteDownloadUrls(Convert.ToInt32(itm.id));
                        }
                        else if (itm.LinkUrl.StartsWith("http://www.metacafe.com") || itm.LinkUrl.StartsWith("http://www.sevenload.com"))
                        {
                            string link = "";
                            string name = string.Empty;
                            string downloadurl = itm.LinkUrl;
                            if (itm.LinkUrl.StartsWith("http://www.metacafe.com"))
                                AppSettings.YoutubeUri = DownloadManagerDailyMotion.GetDownloadVideoUrlFromMetacafeSite(downloadurl);
                            else
                                AppSettings.YoutubeUri = "http://ak.cdn.sevenload.net/slcom-production/cdn/" + itm.LinkUrl.Substring(itm.LinkUrl.LastIndexOf("-") + 1) + "/mobile-h264.main-md.mp4";
                            if (AppSettings.Youtubelink.Contains("youtube"))
                            {
                                var httpClient = new HttpClient();
                                if (AppSettings.Youtubelink.Contains("youtube.com/v/"))
                                {
                                    if (AppSettings.Youtubelink.Contains("http://www.youtube.com/v/") || AppSettings.Youtubelink.Contains("https://www.youtube.com/v") || AppSettings.Youtubelink.Contains("www.youtube.com/v/"))
                                    {
                                        string videoid = AppSettings.Youtubelink.Replace("http://www.youtube.com/v/", "").Replace("https://www.youtube.com/v/", "").Replace("//www.youtube.com/v/", "").Replace("www.youtube.com/v/", "").Replace("//www.youtube-nocookie.com/v/", "");
                                        string orgyoutubeid = "http://www.youtube.com/v/" + videoid + "&amp;hl=en&amp;fs=1&amp;rel=0";
                                        AppSettings.Youtubelink = "http://www.youtube.com/watch?v="+videoid;
                                    }
                                }
                                var url = new Uri(AppSettings.Youtubelink);
                                AppSettings.Youtubelink = string.Empty;
                                var accessToken = "1234";
                                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                                httpRequestMessage.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken));
                                httpRequestMessage.Headers.Add("User-Agent", "My user-Agent");
                                var response = Task.Run(async () => await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead)).Result;
                                string html = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
                                GetYotubeUrl(html);
                                size = Convert.ToInt32(response.Content.Headers.ContentLength);
                                GetYouTubeVideoTitle();
                                link = AppSettings.YoutubeUri + "&title=" + AppSettings.ShowLinkTitle;
                                name = AppSettings.ShowLinkTitle.Replace(":", "").Replace("|", "").Replace(".flv", "").Replace(".", "").Replace("&", "").Replace("#", "").Replace(";", "").Replace("/", "").Replace("?", "").Replace("=", "");
                            }
                            else
                            {
                                name = DownloadManagerDailyMotion.GetVideoTitleForMetaCafeSite(itm.LinkUrl);
                                size = Constants.VideofileSize;
                                link = AppSettings.YoutubeUri;
                                name = name.Replace(":", "").Replace("|", "").Replace(".flv", "").Replace(".", "").Replace("&", "").Replace("#", "").Replace(";", "").Replace("/", "").Replace("?", "").Replace("=", "").Replace("'","");
                            }
                            string foldername = DownloadManager.GetFolderName(name + ".mp4");
                            Constants.LoadDownloadList = LoadList(name + ".mp4", link, Convert.ToInt32(itm.id), size, foldername);
                            DownloadClick(link, name + ".mp4", itm.id);
                            DownloadManager.DeleteDownloadUrls(Convert.ToInt32(itm.id));
                        }
                    }
                    //ListofDownLoads = Constants.DownLoadList;
                }
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in GetDownLoadTableInformation Method In showlist file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }

        public static List<DownloadStatus> LoadList(string Title, string link, int i, int size, string Foldename)
        {
            List<DownloadStatus> download = new List<DownloadStatus>();
            try
            {
                DownloadStatus d = new DownloadStatus();

                List<DownloadInfo> objVideoList = new List<DownloadInfo>();
                string folderStatus = "";
                objVideoList = DownloadManager.LoadDownloadUrls();
                foreach (var itm in objVideoList)
                {
                    folderStatus = itm.DownloadStatus;
                }
                if (objVideoList.Count != 0)
                {
                    d.Id = i;
                    d.Title = Title;
                    d.FolderStatus = folderStatus;
                    d.ChapterProgressPosition = 0;
                    d.RequestUri = link;
                    d.FolderName = Foldename;
                    //d.Downsc = guidid;
                    d.FileSize = size.ToString();
                    download.Add(d);
                    DownloadManager.InsertDownloadItem(d);

                }
            }

            catch (Exception ex)
            {
                DownloadManager.DeleteDownloadUrls(Convert.ToInt32(i));
                Exceptions.SaveOrSendExceptions("Exception in LoadList Method In DownloadManagerHelper.cs file", ex);
            }
            return download;
        }

        public static void GetYotubeUrl(String html)
        {
            try
            {
                var urls = new List<YouTubeUri>();
                var urlsfor480p = new List<YouTubeUri>();
                var entry = default(YouTubeUri);
                try
                {

                    var match = Regex.Match(html, "url_encoded_fmt_stream_map\": \"(.*?)\"");
                    var data = Uri.UnescapeDataString(match.Groups[1].Value);

                    var arr = Regex.Split(data, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"); // split by comma but outside quotes
                    foreach (var d in arr)
                    {
                        var url = "";
                        var signature = "";
                        var tuple = new YouTubeUri();
                        foreach (var p in d.Replace("\\u0026", "\t").Split('\t'))
                        {
                            var index = p.IndexOf('=');
                            if (index != -1 && index < p.Length)
                            {
                                try
                                {
                                    var key = p.Substring(0, index);
                                    var value = Uri.UnescapeDataString(p.Substring(index + 1));
                                    if (key == "url")
                                        url = value;
                                    else if (key == "itag")
                                        tuple.Itag = int.Parse(value);
                                    else if (key == "type" && value.Contains("video/mp4"))
                                        tuple.Type = value;
                                    else if (key == "sig")
                                        signature = value;
                                }
                                catch { }
                            }
                        }

                        tuple.url = url + "&signature=" + signature;
                        if (tuple.IsValid)
                            urls.Add(tuple);
                    }

                    var minTag = GetQualityIdentifier(Common.Library.YouTubeQuality.Quality720P);
                    var maxTag = GetQualityIdentifier(Common.Library.YouTubeQuality.Quality720P);
                    urlsfor480p.AddRange(urls);
                    foreach (var u in urls.Where(u => u.Itag < minTag || u.Itag > maxTag).ToArray())
                        urls.Remove(u);

                    entry = urls.OrderByDescending(u => u.Itag).FirstOrDefault();
                    if (entry == null)
                    {
                        minTag = GetQualityIdentifier(Common.Library.YouTubeQuality.Quality480P);
                        maxTag = GetQualityIdentifier(Common.Library.YouTubeQuality.Quality480P);
                        foreach (var u in urlsfor480p.Where(u => u.Itag < minTag || u.Itag > maxTag).ToArray())
                            urlsfor480p.Remove(u);

                        entry = urlsfor480p.OrderByDescending(u => u.Itag).FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    return;
                }

                if (entry != null)
                {
                    AppSettings.YoutubeUri = entry.url.ToString();
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetYotubeUrl Method In DownloadManagerHelper.cs file", ex);
            }
        }

        private static int GetQualityIdentifier(Common.Library.YouTubeQuality quality)
        {
            switch (quality)
            {
                case Common.Library.YouTubeQuality.Quality480P: return 18;
                case Common.Library.YouTubeQuality.Quality720P: return 22;
                case Common.Library.YouTubeQuality.Quality1080P: return 37;
            }
            throw new ArgumentException("maxQuality");
        }

        //private static string GetYouTubeVideoTitle(string RssDoc)
        //{
        //    string str14 = string.Empty;
        //    try
        //    {
        //        str14 = YoutubeTitle.GetTxtBtwn(RssDoc, "'VIDEO_TITLE': '", "'", 0);
        //        if (str14 == "") str14 = YoutubeTitle.GetTxtBtwn(RssDoc, "\"title\" content=\"", "\"", 0);
        //        if (str14 == "") str14 = YoutubeTitle.GetTxtBtwn(RssDoc, "&title=", "&", 0);
        //        str14 = str14.Replace(@"\", "").Replace("'", "&#39;").Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("+", " ");
        //        AppSettings.ShowLinkTitle = str14;
        //    }
        //    catch (Exception ex)
        //    {
        //        Exceptions.SaveOrSendExceptions("Exception in GetYouTubeVideoTitle Method In DownloadManagerHelper.cs file", ex);
        //    }
        //    return str14;
        //}

        private static Task<string> GetYouTubeVideoTitle()
        {
            Task<string> str14 = MyToolkit.Multimedia.YouTube.GetVideoTitleAsync(AppSettings.LinkUrl);
            AppSettings.ShowLinkTitle = str14.ToString();
            return str14;
        }

        private static async void DownloadClick(string url, string title, string id)
        {
            try
            {
                string ext = System.IO.Path.GetExtension(title);
                var uri = new Uri(url);
                var downloader = new BackgroundDownloader();
                var file = default(StorageFolder);
                StorageFile file1 = default(StorageFile);
                string FolderName = string.Empty;
                if (ext == ".mp3" || ext == ".wav" || ext == ".aac" || ext == ".amr" || ext == ".wma")
                {
                    file = Task.Run(async () => await ApplicationData.Current.LocalFolder.GetFolderAsync("Audio")).Result;
                    FolderName = "Audio";
                }
                else if (ext == ".3gp" || ext == ".3g2" || ext == ".mp4" || ext == ".m4v" || ext == ".wmv" || ext == ".mkv" || ext == ".flv")
                {
                    file = Task.Run(async () => await ApplicationData.Current.LocalFolder.GetFolderAsync("Video")).Result;
                    FolderName = "Video";
                }
                else if (ext == ".doc")
                {
                    file = Task.Run(async () => await ApplicationData.Current.LocalFolder.GetFolderAsync("Documents")).Result;
                    FolderName = "Documents";
                }
                else
                {
                    file = Task.Run(async () => await ApplicationData.Current.LocalFolder.GetFolderAsync("Images")).Result;
                    FolderName = "Images";
                }
                try
                {
                    file1 = await file.CreateFileAsync(title,
                        CreationCollisionOption.ReplaceExisting);
                    DownloadOperation download = downloader.CreateDownload(uri, file1);
                    await StartDownloadAsync(download);
                }
                catch (Exception ex)
                {
                    ex.Data.Add("Url", url);
                    ex.Data.Add("Title", title);
                    Exceptions.SaveOrSendExceptions("Exception in DownloadClick Method In DownloadManagerHelper.cs file", ex);
                    DownloadManager.DeleteDownloadUrls(Convert.ToInt32(id));
                    DownloadManager.DeleteDownloadStatus(Convert.ToInt32(id));
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Url", url);
                ex.Data.Add("Title", title);
                Exceptions.SaveOrSendExceptions("Exception in DownloadClick Method In DownloadManagerHelper.cs file", ex);
            }
        }

        private static async Task StartDownloadAsync(DownloadOperation downloadOperation)
        {
            try
            {
                var progress = new Progress<DownloadOperation>(ProgressCallback1);
                await downloadOperation.StartAsync().AsTask(progress);
            }
            catch (Exception ex)
            {
                ex.Data.Add("RequestUri", downloadOperation.RequestedUri);
                Exceptions.SaveOrSendExceptions("Exception in StartDownloadAsync Method In DownloadManagerHelper.cs file", ex);
            }
        }

        private static void ProgressCallback1(DownloadOperation obj)
        {
            try
            {
                double progress
                    = ((double)obj.Progress.BytesReceived / obj.Progress.TotalBytesToReceive);
                double df = progress * 100;
                string title = obj.ResultFile.Name;
                foreach (DownloadStatus dld in Constants.DownLoadList)
                {
                    if (dld.Title == title)
                    {
                        dld.ChapterProgressPosition = df;
                        int totalbytes = (int)obj.Progress.TotalBytesToReceive;
                        string totalreceivebytes = DownloadManager.GetFileSize(totalbytes);
                        dld.TotalBytesToRecieve = totalreceivebytes;
                    }
                    if (100 == df && dld.Title == title)
                    {
                        try
                        {
                            DownloadManager.DownloadingStatus(dld.FolderName + "/" + dld.Title, "", Convert.ToInt32(obj.Progress.TotalBytesToReceive));
                            DownloadManager.DeleteDownloadStatus(dld.Id);
                            Constants.DownLoadList.Remove(dld);
                        }
                        catch (Exception ex)
                        {
                            ex.Data.Add("Title", dld.Title);
                            ex.Data.Add("Url", dld.RequestUri);
                            Exceptions.SaveOrSendExceptions("Exception in ProgressCallback Method In DownloadManagerHelper.cs file", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("RequestedUri", obj.RequestedUri);
                Exceptions.SaveOrSendExceptions("Exception in ProgressCallback Method In DownloadManagerHelper.cs file", ex);
            }
        }

        public static async Task LoadActiveDownloadsAsync()
        {
            try
            {
                //ListofDownLoads = Constants.DownLoadList;
                IReadOnlyList<DownloadOperation> downloads = null;
                downloads = await BackgroundDownloader.GetCurrentDownloadsAsync();
                if (downloads.Count > 0)
                {
                    await ResumeDownloadAsync(downloads);
                }
                else
                {
                    //DownloadManager.DeleteAll();
                    //ListofDownLoads = Task.Run(async () => await Constants.connection.Table<DownloadStatus>().ToListAsync()).Result;
                }
                if (downloads.Count == 0)
                {
                    DownloadManager.DeleteAll();
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadActiveDownloadsAsync Method In DownloadManagerHelper.cs file", ex);
            }
        }

        private static async Task ResumeDownloadAsync(IReadOnlyList<DownloadOperation> downloadOperation)
        {
            try
            {

                foreach (DownloadOperation oneByOneDownLoad in downloadOperation)
                {
                    var progress = new Progress<DownloadOperation>(ProgressCallback1);
                    await oneByOneDownLoad.AttachAsync().AsTask(progress);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ResumeDownloadAsync Method In DownloadManagerHelper.cs file", ex);
            }
        }

        private static void GetVideoUrlsForVimeo(string html)
        {
            try
            {
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);
                foreach (HtmlNode node in doc.DocumentNode.Descendants().Where(n => n.Name == "ol"))
                {
                    if (node.Attributes.Where(i => i.Name == "class").FirstOrDefault() != null)
                    {
                        if (node.Attributes["class"].Value.ToString().Contains("js-browse_list clearfix browse browse_videos browse_videos_thumbnails kane"))
                        {
                            if (node.Element("li").Attributes["id"].Value.ToString().Contains("clip_"))
                            {
                                string gg = node.Element("li").Element("a").Attributes["href"].Value.ToString();
                            }
                        }
                    }
                }
                MatchCollection Gettbody = Regex.Matches(html, @"<li id\s?.*?\>((.|\r\n)+?)\<\/li\>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match body in Gettbody)
                {
                    if (!body.ToString().StartsWith("<link rel"))
                    {
                        Match geturl = Regex.Match(body.ToString(), @"\<hgroup\s?.*?\>((.|\r\n)+?)\<\/hgroup\>");
                        if (geturl.Value != "")
                        {
                            Match image = Regex.Match(body.ToString(), @"<div[^>]*?class\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>");
                            string imageurl = ";" + image.ToString().Split('(')[1].Replace("'", "").Replace(">", "").Replace(")", "").Split(';')[0].Replace(";", "");
                            Match videourl = Regex.Match(geturl.ToString(), @"\<a\s?.*?\>((.|\r\n)+?)\<\/a\>");
                            string title = videourl.Groups[1].Value;
                            string GetUrl = Regex.Split(videourl.ToString(), "a")[1].Replace("href=", "").Replace("cl", "");
                            Constants.SaveVideos.Add(new Uri(GetUrl + "&title=" + title + "&Imageurl=" + imageurl));
                        }
                        else if (body.ToString().StartsWith("<li id"))
                        {
                            try
                            {
                                Match videourl = Regex.Match(body.ToString(), @"<a[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>");
                                string Geturl = videourl.Groups[1].Value;
                                Geturl = Geturl.Substring(Geturl.LastIndexOf("/") + 1);
                                string Title = Regex.Split(videourl.ToString(), "title")[1].Replace(">", "").Replace("=", "");
                                string imageurl = Regex.Match(body.ToString(), @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>").Groups[1].Value;
                                Constants.SaveVideos.Add(new Uri("http://vimeo.com/" + Geturl + "&title=" + Title + "&imageurl=" + imageurl));
                            }
                            catch (Exception ex)
                            {
                                Exceptions.SaveOrSendExceptions("Exception in GetVideoUrlsForVimeo Method In DownloadManagerHelper.cs file", ex);
                            }
                        }
                    }
                }
                MatchCollection GettbodyLinks = Regex.Matches(html, @"<a[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match body in GettbodyLinks)
                {
                    if (body.ToString().StartsWith("<a href="))
                    {
                        string link = "http://vimeo.com" + body.Groups[1].Value;
                        if (!link.StartsWith("http://vimeo.comjavascript"))
                            Constants.SaveLinks.Add(new Uri(link));
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetVideoUrlsForVimeo Method In DownloadManagerHelper.cs file", ex);
            }
        }

        private static void GetvideoUrlForVeohSite(string html)
        {
            try
            {
                List<string> linkslist = new List<string>();
                MatchCollection Gettbody = Regex.Matches(html, @"<a[^>]*?href\s?.*?\>((.|\r\n)+?)\<\/a\>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match body in Gettbody)
                {
                    Match ch = Regex.Match(body.ToString(), @"<a[^>]*?id\s?.*?\>((.|\r\n)+?)\<\/a\>");
                    if (ch.ToString().Contains("<a id"))
                    {
                        string[] Linkurl = ch.ToString().Split('"');
                        foreach (string sd in Linkurl)
                        {
                            if (sd.StartsWith("http:"))
                            {
                                linkslist.Add(sd);
                                Constants.SaveLinks.Add(new Uri(sd));
                            }
                        }
                    }

                    if (body.ToString().Contains("http://www.veoh.com/list"))
                    {
                        string[] LinkUrl = body.ToString().Split('"');
                        foreach (string sd in LinkUrl)
                        {
                            if (sd.StartsWith("http:"))
                            {
                                if (!linkslist.Contains(sd))
                                    Constants.SaveLinks.Add(new Uri(sd));
                            }
                        }
                    }
                    if (body.ToString().Contains("http://www.veoh.com/watch/"))
                    {
                        string VideoUrl = string.Empty;
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(body.ToString());
                        foreach (HtmlNode node in doc.DocumentNode.Descendants().Where(n => n.Name == "a"))
                        {
                            if (node.Attributes["href"].Value.Contains("www.veoh.com/watch/"))
                                VideoUrl = node.Attributes["href"].Value;
                        }
                        //string VideoUrl = body.ToString().Split('"')[1];
                        string Title = GetVideoTitleForVeohsite(VideoUrl);
                        if (!Constants.SaveVideos.Contains(new Uri(VideoUrl + "&title=" + Title)))
                            Constants.SaveVideos.Add(new Uri(VideoUrl + "&title=" + Title));
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in GetvideoUrlForVeohSite Method In DownloadManagerHelper.cs file", ex);
            }
        }

        private static string GetVideoTitleForVeohsite(string videolinkurl)
        {
            string VideoTitle = string.Empty;
            try
            {
                var httpClient = new HttpClient();
                var url = new Uri(videolinkurl);
                var accessToken = "1234";
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                httpRequestMessage.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken));
                httpRequestMessage.Headers.Add("User-Agent", "My user-Agent");
                var response = Task.Run(async () => await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead)).Result;
                string html = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
                VideoTitle = Regex.Match(html, @"\<title\s?.*?\>((.|\r\n)+?)\<\/title\>").Groups[1].Value.Replace("Veoh.com", "").Replace("|", "").Replace("&amp", "").Replace("#", "").Replace("0", "").Replace("1", "").Replace("2", "").Replace("3", "").Replace("4", "").Replace("5", "").Replace(";", "").Replace("6", "").Replace("7", "").Replace("8", "").Replace("9", "");
                string Image = Regex.Split(html, "image")[1];
                string imageurl = Image.Split('>')[0].Replace("content=", "").Replace(@"""", "");
                VideoTitle = VideoTitle + "&imageurl=" + imageurl;
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", videolinkurl);
                Exceptions.SaveOrSendExceptions("Exception in GetVideoTitleForVeohsite Method In DownloadManagerHelper.cs file", ex);
            }
            return VideoTitle;
        }

        public static void GetSelectedVideoLink(DownloadPivot selecteditem)
        {
            try
            {
                if (selecteditem.Downloaduri.ToString().StartsWith("http://www.youtube.com"))
                {
                    AppSettings.LinkUrl = selecteditem.Downloaduri.ToString().Replace("&amp;hl=en&amp;fs=1&amp;rel=0", "").Replace("http://www.youtube.com/v/", "").Replace("&amp;wide=1", "");
                }
                else if (selecteditem.Downloaduri.ToString().StartsWith("http://www.dailymotion.com"))
                {
                    AppSettings.YoutubeUri = "http://www.dailymotion.com/embed/video" + selecteditem.Downloaduri.ToString().Substring(selecteditem.Downloaduri.ToString().LastIndexOf("/"));
                    AppSettings.LinkUrl = selecteditem.Downloaduri.ToString();
                }
                else if (selecteditem.Downloaduri.ToString().StartsWith("http://pluralsight.com"))
                {
                    AppSettings.YoutubeUri = selecteditem.Downloaduri.ToString();
                    AppSettings.LinkUrl = selecteditem.Downloaduri.ToString();
                }
                else if (selecteditem.Downloaduri.ToString().StartsWith("http://vimeo.com/") || selecteditem.Downloaduri.ToString().StartsWith("http://www.veoh.com"))
                {
                    String playurl = Regex.Split(selecteditem.Downloaduri.ToString(), "&imageurl=")[1];
                    AppSettings.LinkUrl = Regex.Split(selecteditem.Downloaduri.ToString(), "&title=")[0].Replace(playurl, "");
                }
                else if (selecteditem.Downloaduri.ToString().StartsWith("http://www.putlocker.com/file/") || selecteditem.Downloaduri.ToString().StartsWith("http://www.videoslasher.com"))
                {
                    string RemoveTitle = selecteditem.Downloaduri.ToString().Substring(selecteditem.Downloaduri.ToString().LastIndexOf("/"));
                    AppSettings.YoutubeUri = selecteditem.Downloaduri.ToString().Replace(RemoveTitle, "").Replace("file", "embed");
                    AppSettings.LinkUrl = selecteditem.Downloaduri.ToString();
                }
                else if (selecteditem.Downloaduri.ToString().StartsWith("http://royalvids.eu"))
                {
                    string RemoveTitle = selecteditem.Downloaduri.ToString().Substring(selecteditem.Downloaduri.ToString().LastIndexOf("/"));

                    string videoId = selecteditem.Downloaduri.ToString().Replace(RemoveTitle, "").Replace("-650x430.html", "-1200x730.html");
                    AppSettings.YoutubeUri = videoId;
                    AppSettings.LinkUrl = selecteditem.Downloaduri.ToString();

                }
                else if (selecteditem.Downloaduri.ToString().StartsWith("http://www.metacafe.com"))
                {
                    AppSettings.LinkUrl = selecteditem.Downloaduri.ToString();

                }
                else if (selecteditem.Downloaduri.ToString().StartsWith("http://www.sevenload.com"))
                {
                    AppSettings.LinkUrl = selecteditem.Downloaduri.ToString();
                }
                else
                {
                    AppSettings.LinkUrl = string.Empty;
                    AppSettings.YoutubeUri = selecteditem.Downloaduri.ToString();
                    AppSettings.LinkUrl = selecteditem.Downloaduri.ToString();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Selectedurl", selecteditem.Downloaduri);
                Exceptions.SaveOrSendExceptions("Exception in GetSelectedVideoLink Method In DownloadManagerHelper.cs file", ex);
            }
        }

        private static void GetvideourlsFromBharthMovies(string html)
        {
            MatchCollection Gettbody = Regex.Matches(html, @"<a[^>]*?href\s?.*?\>((.|\r\n)+?)\<\/a\>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (Match body in Gettbody)
            {
                try
                {
                    string[] links = Regex.Split(body.ToString(), ">");
                    string link = links[0].Replace("<a href=", "").Replace(@"""", "");
                    Constants.SaveLinks.Add(new Uri("http://www.bharatmovies.com" + link));
                }
                catch (Exception ex)
                {
                    ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                    Exceptions.SaveOrSendExceptions("Exception in GetvideourlsFromBharthMovies Method In DownloadManagerHelper.cs file", ex);
                }
            }
            try
            {
                string GetUrl = Regex.Split(html.ToString(), "var config")[1].Split('[')[1];
                string[] videourl = Regex.Split(GetUrl, "}");
                foreach (string s in videourl)
                {
                    if (s.StartsWith("{id"))
                    {
                        string[] k = s.Split(',');
                        string videoId = k[0].Replace(@"{id:'", "").Replace("'", "");
                        string videoTitle = k[1].Replace(@"title:'", "").Replace("'", "");
                        Constants.SaveVideos.Add(new Uri("http://www.youtube.com/v/" + videoId + "&amp;hl=en&amp;fs=1&amp;rel=0"));
                        string imgur = "http://img.youtube.com/vi/" + videoId + "/default.jpg";
                        Constants.SaveImages.Add(new Uri(imgur));
                    }
                    if (s.StartsWith(",{id"))
                    {
                        string[] k = s.Split(',');
                        string videoId = k[1].Replace(@"{id:'", "").Replace("'", "");
                        string videoTitle = k[2].Replace(@"title:'", "").Replace("'", "");
                        Constants.SaveVideos.Add(new Uri("http://www.youtube.com/v/" + videoId + "&amp;hl=en&amp;fs=1&amp;rel=0"));
                        string imgur = "http://img.youtube.com/vi/" + videoId + "/default.jpg";
                        Constants.SaveImages.Add(new Uri(imgur));
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private static string GetLovePkAudiosongs(string AudioUrl)
        {
            string AudioDownloadUrl = string.Empty;
            try
            {
                var httpClient = new HttpClient();
                var url = new Uri(AudioUrl);
                var accessToken = "1234";
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                httpRequestMessage.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken));
                httpRequestMessage.Headers.Add("User-Agent", "My user-Agent");
                var response = Task.Run(async () => await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead)).Result;
                AudioDownloadUrl = response.RequestMessage.RequestUri.ToString();
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AudioUrl);
                Exceptions.SaveOrSendExceptions("Exception in GetLovePkAudiosongs Method In DownloadManagerHelper.cs file", ex);
            }
            return AudioDownloadUrl;
        }

        private static void GetVideoUrlsFromINCom(string html)
        {
            try
            {
                string[] mp4formatevideos = Regex.Split(html, "video_src");
                foreach (string videourl in mp4formatevideos)
                {
                    string replacevideo = videourl.Replace(@"""", "");
                    if (replacevideo.Contains(".mp4"))
                    {
                        string downloadurl = Regex.Split(replacevideo, "type")[0].Replace("src=", "").Replace(@"""", "");
                        string ext = downloadurl.Substring(downloadurl.LastIndexOf(".") + 1);
                        if (ext.StartsWith("mp4"))
                        {
                            Constants.SaveVideos.Add(new Uri(downloadurl));
                        }
                    }
                }

                MatchCollection matchesImgSrc = Regex.Matches(html, @"<a[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match m1 in matchesImgSrc)
                {
                    string hrefs = m1.Groups[1].Value;
                    if (hrefs.ToString().StartsWith("http://www.in.com/videos/") && hrefs.ToString().EndsWith(".html#block1"))
                    {
                        string[] regex = { @"<a[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", @"<embed[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", @"<iframe[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>" };
                        var httpClient = new HttpClient();
                        var url = new Uri(hrefs.ToString());
                        var accessToken = "1234";
                        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                        httpRequestMessage.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken));
                        httpRequestMessage.Headers.Add("User-Agent", "My user-Agent");
                        var response = Task.Run(async () => await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead)).Result;
                        string htmldata = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
                        string href;

                        for (int i = 0; i < regex.Count(); i++)
                        {
                            MatchCollection matches = Regex.Matches(htmldata, regex[i], RegexOptions.IgnoreCase | RegexOptions.Singleline);
                            foreach (Match m in matches)
                            {
                                href = m.Groups[1].Value;

                                if (href.ToString().EndsWith(".mp4") || href.ToString().EndsWith(".wmv") || href.ToString().Contains("www.youtube.com/v/") || href.ToString().EndsWith(".m4v") || href.ToString().Contains("www.youtube.com/embed/") || href.ToString().Contains("www.youtube-nocookie.com/embed/") || href.ToString().Contains("www.youtube-nocookie.com/v/"))
                                {
                                    if (href.ToString().StartsWith("//www.youtube"))
                                        href = href.ToString().Replace("//www.youtube", "http://www.youtube");
                                    if (!Constants.SaveVideos.Contains(new Uri(href.ToString())))
                                        Constants.SaveVideos.Add(new Uri(href.ToString()));
                                }
                                if (href.ToString().StartsWith("http://www.dailymotion.com") && href.ToString().ToLower().Contains("autoplay="))
                                {
                                    if (!Constants.SaveVideos.Contains(new Uri(href.ToString())))
                                        Constants.SaveVideos.Add(new Uri(href.ToString()));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in GetVideoUrlsFromINCom Method In DownloadManagerHelper.cs file", ex);
            }
        }

        private static void GetAudiosFromMobileSongssite(string html)
        {
            try
            {
                string audiolink = Regex.Split(html, "Director")[1];
                MatchCollection Gettbody = Regex.Matches(audiolink, @"<a[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match body in Gettbody)
                {
                    string audiourl = body.Groups[1].Value;
                    if (audiourl.Contains(".mp3"))
                    {
                        Constants.SaveAudios.Add(new Uri("http://mobilesongs.audio.pk" + audiourl.Replace("/file10", "").Replace(".html", "")));
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in GetAudiosFromMobileSongssite Method In DownloadManagerHelper.cs file", ex);
            }
        }

        private static void GetTeluguOneSiteMovies(string html)
        {
            try
            {
                if (AppSettings.NavigationUrl.Contains("http://www.teluguone.com/movies/moviesPlayer"))
                {
                    GetYoutubeUrlFromTeluguOneSite(AppSettings.NavigationUrl);
                }
                List<string> comparevideosId = new List<string>();
                string[] getMovieIds = Regex.Split(html, "moviesPlayer");
                foreach (string s in getMovieIds)
                {
                    string videourl = Regex.Split(s, "'")[0];
                    if (videourl.StartsWith(".jsp?movieId"))
                    {
                        if (!comparevideosId.Contains(videourl))
                        {
                            // GetYoutubeUrlFromTeluguOneSite("http://www.teluguone.com/movies/moviesPlayer" + videourl);
                            Constants.SaveLinks.Add(new Uri("http://www.teluguone.com/movies/moviesPlayer" + videourl));
                            comparevideosId.Add(videourl);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in GetTeluguOneSiteMovies Method In DownloadManagerHelper.cs file", ex);
            }
            GetLinksForCinevedik(html);
            GetTeleguOneVideo(html);
        }

        private static void GetYoutubeUrlFromTeluguOneSite(string videourl)
        {
            try
            {
                var httpClient = new HttpClient();
                var url = new Uri(videourl);
                var accessToken = "1234";
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                httpRequestMessage.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken));
                httpRequestMessage.Headers.Add("User-Agent", "My user-Agent");
                var response = Task.Run(async () => await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead)).Result;
                string html = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
                string videoid = Regex.Split(html, "http://www.youtube.com/embed/")[1];
                videoid = Regex.Split(videoid, @"""")[0];
                string orgyoutubeid = "http://www.youtube.com/v/" + videoid + "&amp;hl=en&amp;fs=1&amp;rel=0";
                Constants.SaveVideos.Add(new Uri(orgyoutubeid));
                string imgur = "http://img.youtube.com/vi/" + videoid + "/default.jpg";
                Constants.SaveImages.Add(new Uri(imgur));
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in GetYoutubeUrlFromTeluguOneSite Method In DownloadManagerHelper.cs file", ex);
            }
        }

        private static void GetCinevedikaVideoUrls(string html)
        {
            List<string> comparevideosId = new List<string>();
            try
            {
                string[] videourl = Regex.Split(html, "src");
                foreach (string s in videourl)
                {
                    if (s.Contains("i.ytimg.com"))
                    {
                        string videoid = Regex.Split(s, @"""")[1].Replace("http://i.ytimg.com/vi/", "").Replace("/default.jpg", "").Replace(@"""", "");
                        string orgyoutubeid = "http://www.youtube.com/v/" + videoid + "&amp;hl=en&amp;fs=1&amp;rel=0";
                        if (!comparevideosId.Contains(orgyoutubeid))
                        {
                            Constants.SaveVideos.Add(new Uri(orgyoutubeid));
                            string imgur = "http://img.youtube.com/vi/" + videoid + "/default.jpg";
                            Constants.SaveImages.Add(new Uri(imgur));
                            comparevideosId.Add(orgyoutubeid);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in GetCinevedikaVideoUrls Method In DownloadManagerHelper.cs file", ex);
            }
            GetcinevedikaUrls(html, comparevideosId);
        }

        private static void GetcinevedikaUrls(string html, List<string> comparevideosId)
        {
            try
            {
                string[] videourl = Regex.Split(html, "http://www.youtube.com/v/");
                foreach (string s in videourl)
                {
                    if (s.Contains("player_embedded"))
                    {
                        string videoid = Regex.Split(s, "fs")[0].Replace("?", "");
                        string orgyoutubeid = "http://www.youtube.com/v/" + videoid + "&amp;hl=en&amp;fs=1&amp;rel=0";
                        if (!comparevideosId.Contains(orgyoutubeid))
                        {
                            Constants.SaveVideos.Add(new Uri(orgyoutubeid));
                            string imgur = "http://img.youtube.com/vi/" + videoid + "/default.jpg";
                            Constants.SaveImages.Add(new Uri(imgur));
                            comparevideosId.Add(orgyoutubeid);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in GetcinevedikaUrls Method In DownloadManagerHelper.cs file", ex);
            }
            GetLinksForCinevedik(html);
        }

        private static void GetLinksForCinevedik(string html)
        {
            try
            {
                MatchCollection Gettbody = Regex.Matches(html, @"<a[^>]*? href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match body in Gettbody)
                {
                    try
                    {
                        string LinkUrl = body.Groups[1].Value.TrimEnd(new char[] { '/' });
                        Constants.SaveLinks.Add(new Uri(LinkUrl));
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in GetLinksForCinevedik Method In DownloadManagerHelper.cs file", ex);
            }
            GetDailymotionvideosurlsFromCinevedik(html);
        }

        private static void GetDailymotionvideosurlsFromCinevedik(string html)
        {
            try
            {
                string[] videourl = Regex.Split(html, "http://www.dailymotion.com/embed");

                foreach (string s in videourl)
                {
                    if (s.StartsWith("/video"))
                    {
                        string videoid = Regex.Split(s, ">")[0].Replace("/video/", "").Replace("'", "");
                        string VideoTitle = Regex.Match(html, @"\<title\s?.*?\>((.|\r\n)+?)\<\/title\>").Groups[1].Value.Replace("Cinevedika.com", "").Replace("|", "");
                        string orgyoutubeid = "http://www.dailymotion.com/video/" + videoid + "_" + VideoTitle;
                        Constants.SaveVideos.Add(new Uri(orgyoutubeid));
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in GetDailymotionvideosurlsFromCinevedik Method In DownloadManagerHelper.cs file", ex);
            }
        }

        private static void GetTeleguOneVideo(string html)
        {
            try
            {
                string[] videourl = Regex.Split(html, "http://img.youtube.com");
                foreach (string s in videourl)
                {
                    try
                    {
                        if (s.Contains("/vi/"))
                        {
                            string videoid = Regex.Split(s, "/")[2].Replace("http://i.ytimg.com/vi/", "").Replace("/default.jpg", "").Replace(@"""", "");
                            string orgyoutubeid = "http://www.youtube.com/v/" + videoid + "&amp;hl=en&amp;fs=1&amp;rel=0";
                            Constants.SaveVideos.Add(new Uri(orgyoutubeid));
                            string imgur = "http://img.youtube.com/vi/" + videoid + "/default.jpg";
                            Constants.SaveImages.Add(new Uri(imgur));
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                        Exceptions.SaveOrSendExceptions("Exception in GetTeleguOneVideo Method In DownloadManagerHelper.cs file", ex);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            GetTeluguOneShortFilems(html);
        }

        private static void GetTeluguOneShortFilems(string html)
        {
            try
            {
                string[] videourl = Regex.Split(html, "http://www.youtube.com");
                foreach (string s in videourl)
                {
                    try
                    {
                        if (s.StartsWith("/embed"))
                        {
                            string videoid = Regex.Split(s, @"""")[0].Replace("http://i.ytimg.com/vi/", "").Replace("/default.jpg", "").Replace(@"""", "").Replace("/embed/", "");
                            string orgyoutubeid = "http://www.youtube.com/v/" + videoid + "&amp;hl=en&amp;fs=1&amp;rel=0";
                            Constants.SaveVideos.Add(new Uri(orgyoutubeid));
                            string imgur = "http://img.youtube.com/vi/" + videoid + "/default.jpg";
                            Constants.SaveImages.Add(new Uri(imgur));
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                        Exceptions.SaveOrSendExceptions("Exception in GetTeluguOneShortFilems Method In DownloadManagerHelper.cs file", ex);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private static void GetLiveTvUrlForTeluguOneSite(string html)
        {
            try
            {
                string[] videourl = Regex.Split(html, "src");

                foreach (string s in videourl)
                {
                    try
                    {
                        if (s.Contains("http://objectinfo.live"))
                        {
                            string videoid = Regex.Split(s, @"""")[1];
                            string orgyoutubeid = videoid;
                            Constants.SaveVideos.Add(new Uri(orgyoutubeid));
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                        Exceptions.SaveOrSendExceptions("Exception in GetLiveTvUrlForTeluguOneSite Method In DownloadManagerHelper.cs file", ex);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private static void GetLoveOngsPkSiteVideoUrls(string html)
        {
            List<string> comparevideosId = new List<string>();
            try
            {
                string[] videourl = Regex.Split(html, "src");
                foreach (string s in videourl)
                {
                    if (s.Contains("i.ytimg.com"))
                    {
                        string videoid = Regex.Split(s, @"""")[1].Replace("http://i.ytimg.com/vi/", "").Replace("/default.jpg", "").Replace(@"""", "").Replace(".jpg", "").Split('/')[0];
                        string orgyoutubeid = "http://www.youtube.com/v/" + videoid + "&amp;hl=en&amp;fs=1&amp;rel=0";
                        if (!comparevideosId.Contains(orgyoutubeid))
                        {
                            Constants.SaveVideos.Add(new Uri(orgyoutubeid));
                            string imgur = "http://img.youtube.com/vi/" + videoid + "/default.jpg";
                            Constants.SaveImages.Add(new Uri(imgur));
                            comparevideosId.Add(orgyoutubeid);
                        }
                    }
                    if (s.Contains("img.youtube.com"))
                    {
                        string videoid = Regex.Split(s, "/")[4].Replace("http://i.ytimg.com/vi/", "").Replace("/default.jpg", "").Replace(@"""", "").Replace(".jpg", "").Split('/')[0];
                        string orgyoutubeid = "http://www.youtube.com/v/" + videoid + "&amp;hl=en&amp;fs=1&amp;rel=0";
                        if (!comparevideosId.Contains(orgyoutubeid))
                        {
                            Constants.SaveVideos.Add(new Uri(orgyoutubeid));
                            string imgur = "http://img.youtube.com/vi/" + videoid + "/default.jpg";
                            Constants.SaveImages.Add(new Uri(imgur));
                            comparevideosId.Add(orgyoutubeid);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in GetLoveOngsPkSiteVideoUrls Method In DownloadManagerHelper.cs file", ex);
            }
        }

        private static void GetVideoUrlsFromOnlinewatchMoviesSite(string html)
        {
            try
            {
                string[] videourl = Regex.Split(html, "src");
                foreach (string item in videourl)
                {
                    if (item.Contains("http://www.youtube.com/embed/") || item.Contains("https://www.youtube.com/embed") || item.Contains("//www.youtube-nocookie.com/embed/") || item.Contains("www.youtube.com/embed/"))
                    {
                        string videoid = Regex.Split(item, @"""")[1].Replace("http://www.youtube.com/embed/", "").Replace("https://www.youtube.com/embed/", "").Replace("//www.youtube.com/embed/", "").Replace("www.youtube.com/embed/", "").Replace("//www.youtube-nocookie.com/embed/", "");
                        string orgyoutubeid = "http://www.youtube.com/v/" + videoid + "&amp;hl=en&amp;fs=1&amp;rel=0";
                        Constants.SaveVideos.Add(new Uri(orgyoutubeid));
                        string imgur = "http://img.youtube.com/vi/" + videoid + "/default.jpg";
                        Constants.SaveImages.Add(new Uri(imgur));
                    }
                    if (AppSettings.NavigationUrl.Contains("c-sharpcorner"))
                    {
                        if (item.Contains("http://channel9.msdn.com/Blogs/"))
                        {
                            //var start = item.IndexOf("\"") + 6;
                            //var match2 = input.Substring(start, input.IndexOf("-") - start);
                            string link = Regex.Split(item, @"""")[1];
                            string[] regex = { @"<a[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", @"<embed[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", @"<iframe[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>" };
                            var httpClient = new HttpClient();
                            var url = new Uri(link);
                            var accessToken = "1234";
                            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                            httpRequestMessage.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken));
                            httpRequestMessage.Headers.Add("User-Agent", "My user-Agent");
                            var response = Task.Run(async () => await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead)).Result;
                            string htmldata = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
                            string href;

                            for (int i = 0; i < regex.Count(); i++)
                            {
                                MatchCollection matches = Regex.Matches(htmldata, regex[i], RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                foreach (Match m in matches)
                                {
                                    href = m.Groups[1].Value;

                                    if (href.ToString().EndsWith(".mp4") || href.ToString().EndsWith(".wmv") || href.ToString().StartsWith("http://media.ch9.ms/ch9/") || href.ToString().EndsWith(".m4v"))
                                    {
                                        if (!Constants.SaveVideos.Contains(new Uri(href.ToString())))
                                            Constants.SaveVideos.Add(new Uri(href.ToString()));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in GetVideoUrlsFromOnlinewatchMoviesSite Method In DownloadManagerHelper.cs file", ex);
            }
        }

        private static void GetLinksAndVideoLinksFromMetaCafeSite(string html)
        {
            try
            {
                List<string> linksList = new List<string>();
                string[] linksarray = Regex.Split(html, @"href=");
                foreach (string link in linksarray)
                {
                    try
                    {
                        string orglink = Regex.Split(link, @"""")[1];
                        if (orglink.StartsWith("/"))
                            Constants.SaveLinks.Add(new Uri("http://www.metacafe.com" + orglink.TrimEnd(new char[] { '/' })));
                        if (orglink.StartsWith("http://"))
                            Constants.SaveLinks.Add(new Uri(orglink.TrimEnd(new char[] { '/' })));
                        if (orglink.StartsWith("/watch"))
                        {
                            string imgurl = Regex.Split(link, "src=")[1].Split('"')[1];
                            if (orglink.Contains("?br"))
                            {
                                string title = orglink.Substring(orglink.LastIndexOf("?"));
                                orglink = orglink.Replace(title, "");
                            }
                            Constants.SaveVideos.Add(new Uri("http://www.metacafe.com" + orglink + "/ImageUrl=" + imgurl));
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                        Exceptions.SaveOrSendExceptions("Exception in GetLinksAndVideoLinksFromMetaCafeSite Method In DownloadManagerHelper.cs file", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in GetLinksAndVideoLinksFromMetaCafeSite Method In DownloadManagerHelper.cs file", ex);
            }
        }

        private static void GetVideoLinksForSevenloadSite(string html)
        {
            try
            {
                List<string> linksList = new List<string>();
                string[] linksarray = Regex.Split(html, @"href=");
                foreach (string link in linksarray)
                {
                    try
                    {
                        string orglink = Regex.Split(link, @"""")[1];
                        if (orglink.StartsWith("/"))
                            Constants.SaveLinks.Add(new Uri("http://www.sevenload.com" + orglink.TrimEnd(new char[] { '/' })));
                        if (orglink.StartsWith("http://"))
                            Constants.SaveLinks.Add(new Uri(orglink.TrimEnd(new char[] { '/' })));
                        if (orglink.StartsWith("/videos"))
                        {
                            string imgurl = Regex.Split(link, "src=")[1].Split('"')[1];
                            if (orglink.Contains("?br"))
                            {
                                string title = orglink.Substring(orglink.LastIndexOf("?"));
                                orglink = orglink.Replace(title, "");
                            }
                            Constants.SaveVideos.Add(new Uri("http://www.sevenload.com" + orglink + "/ImageUrl=" + imgurl));
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                        Exceptions.SaveOrSendExceptions("Exception in GetVideoLinksForSevenloadSite Method In DownloadManagerHelper.cs file", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
                Exceptions.SaveOrSendExceptions("Exception in GetVideoLinksForSevenloadSite Method In DownloadManagerHelper.cs file", ex);
            }
        }

        public static TextBox GetNavigateurlForTextBox(GridView itemGridView)
        {
            TextBox cb = default(TextBox);
            try
            {
                IEnumerable<DependencyObject> cboxes = null;
                cboxes = DownloadManagerHelper.GetChildsRecursive(itemGridView);
                foreach (DependencyObject obj in cboxes.OfType<TextBox>())
                {
                    Type type = obj.GetType();
                    if (type.Name == "TextBox")
                    {
                        cb = obj as TextBox;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in CheckedItemList Method In DownloadManagerHelper.cs file", ex);
            }
            return cb;
        }

        public static ListBox GetNavigateurlForListBox(GridView itemGridView)
        {
            ListBox cb = default(ListBox);
            try
            {
                IEnumerable<DependencyObject> cboxes = null;
                cboxes = DownloadManagerHelper.GetChildsRecursive(itemGridView);
                foreach (DependencyObject obj in cboxes.OfType<ListBox>())
                {
                    Type type = obj.GetType();
                    if (type.Name == "ListBox")
                    {
                        cb = obj as ListBox;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in CheckedItemList Method In DownloadManagerHelper.cs file", ex);
            }
            return cb;
        }

        private static void GetLinksForSchooltube(string html)
        {
            try
            {
                MatchCollection Gettbody = Regex.Matches(html, @"<a[^>]*? href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match body in Gettbody)
                {
                    string LinkUrl = body.Groups[1].Value;
                    if (LinkUrl.StartsWith("http://www.schooltube.com/video"))
                    {
                        Constants.SaveVideos.Add(new Uri(LinkUrl));
                    }
                    if (!LinkUrl.StartsWith("http://www.schooltube.com"))
                    {
                        Constants.SaveLinks.Add(new Uri("http://www.schooltube.com" + LinkUrl));
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetLinksForSchooltube Method In DownloadManagerHelper.cs file", ex);
            }
        }

        public static void InsertFavoriteLinks(DownloadPivot item)
        {
            try
            {
                //AppSettings.NavigationUrl= item.Downloaduri.ToString();
                int count = ChildLinkCount(item.Downloaduri.ToString());
                DownloadManager.InsertFavoriteLinks(item, count);
            }
            catch (Exception ex)
            {
            }
        }

        public static int ChildLinkCount(string LinkUrl1)
        {
            int count = 0;
            string[] regexImgSrc = { @"<a[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", 
                                     @"<embed[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>",
                                     @"<iframe[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", 
                                     @"<video[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", @"<link[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>" };
            List<Uri> savelinks = new List<Uri>();
            string htmlDataReload = "";
            try
            {
                var httpClient = new HttpClient();
                var url = new Uri(LinkUrl1);
                var accessToken = "1234";
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                httpRequestMessage.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken));
                httpRequestMessage.Headers.Add("User-Agent", "My user-Agent");
                var response = Task.Run(async () => await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead)).Result;
                htmlDataReload = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
            }
            catch (Exception ex)
            {

            }
            if (LinkUrl1.StartsWith("http://www.dailymotion.com"))
            {
                try
                {
                    MatchCollection Gettbody = Regex.Matches(htmlDataReload, @"<a[^>]*?class\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    foreach (Match body in Gettbody)
                    {
                        Match GetUrl = Regex.Match(body.ToString(), @"<a[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        string LinkUrl = GetUrl.Groups[1].Value;
                        if (!LinkUrl.StartsWith("http://") && LinkUrl.StartsWith("/in"))
                        {
                            LinkUrl = "http://www.dailymotion.com" + LinkUrl;
                            savelinks.Add(new Uri(LinkUrl));
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (LinkUrl1.Contains("pluralsight"))
            {
                try
                {
                    MatchCollection Gettbody = Regex.Matches(htmlDataReload, @"<a[^>]*?class\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    foreach (Match body in Gettbody)
                    {
                        if (!body.ToString().StartsWith("<tbody>"))
                        {
                            Match GetUrl = Regex.Match(body.ToString(), @"<a[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                            string LinkUrl = GetUrl.Groups[1].Value;
                            if (LinkUrl.StartsWith("/training/Courses"))
                                savelinks.Add(new Uri("http://pluralsight.com" + LinkUrl));
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            if (LinkUrl1.Contains("vimeo.com"))
            {
                try
                {
                    MatchCollection GettbodyLinks = Regex.Matches(htmlDataReload, @"<a[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    foreach (Match body in GettbodyLinks)
                    {
                        if (body.ToString().StartsWith("<a href="))
                        {
                            string link = "http://vimeo.com" + body.Groups[1].Value;
                            if (!link.StartsWith("http://vimeo.comjavascript"))
                                savelinks.Add(new Uri(link));
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            if (LinkUrl1.Contains("veoh.com"))
            {
                List<string> linkslist = new List<string>();
                try
                {
                    MatchCollection Gettbody = Regex.Matches(htmlDataReload, @"<a[^>]*?href\s?.*?\>((.|\r\n)+?)\<\/a\>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    foreach (Match body in Gettbody)
                    {
                        Match ch = Regex.Match(body.ToString(), @"<a[^>]*?id\s?.*?\>((.|\r\n)+?)\<\/a\>");
                        if (ch.ToString().Contains("<a id"))
                        {
                            string[] Linkurl = ch.ToString().Split('"');
                            foreach (string sd in Linkurl)
                            {
                                if (sd.StartsWith("http:"))
                                {
                                    linkslist.Add(sd);
                                    savelinks.Add(new Uri(sd));
                                }
                            }
                        }

                        if (body.ToString().Contains("http://www.veoh.com/list"))
                        {
                            string[] LinkUrl = body.ToString().Split('"');
                            foreach (string sd in LinkUrl)
                            {
                                if (sd.StartsWith("http:"))
                                {
                                    if (!linkslist.Contains(sd))
                                        savelinks.Add(new Uri(sd));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            if (LinkUrl1.Contains("bharatmovies.com"))
            {
                MatchCollection Gettbody = Regex.Matches(htmlDataReload, @"<a[^>]*?href\s?.*?\>((.|\r\n)+?)\<\/a\>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match body in Gettbody)
                {
                    try
                    {
                        string[] links = Regex.Split(body.ToString(), ">");
                        string link = links[0].Replace("<a href=", "").Replace(@"""", "");
                        savelinks.Add(new Uri("http://www.bharatmovies.com" + link));
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            if (LinkUrl1.Contains("teluguone"))
            {
                try
                {
                    List<string> comparevideosId = new List<string>();
                    string[] getMovieIds = Regex.Split(htmlDataReload, "moviesPlayer");
                    foreach (string s in getMovieIds)
                    {
                        string videourl = Regex.Split(s, "'")[0];
                        if (videourl.StartsWith(".jsp?movieId"))
                        {
                            if (!comparevideosId.Contains(videourl))
                            {
                                // GetYoutubeUrlFromTeluguOneSite("http://www.teluguone.com/movies/moviesPlayer" + videourl);
                                savelinks.Add(new Uri("http://www.teluguone.com/movies/moviesPlayer" + videourl));
                                comparevideosId.Add(videourl);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                try
                {

                    MatchCollection Gettbody = Regex.Matches(htmlDataReload, @"<a[^>]*? href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    foreach (Match body in Gettbody)
                    {
                        try
                        {
                            string LinkUrl = body.Groups[1].Value.TrimEnd(new char[] { '/' });
                            savelinks.Add(new Uri(LinkUrl));
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            if (LinkUrl1.Contains("cinevedika"))
            {
                try
                {
                    MatchCollection Gettbody = Regex.Matches(htmlDataReload, @"<a[^>]*? href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    foreach (Match body in Gettbody)
                    {
                        try
                        {
                            string LinkUrl = body.Groups[1].Value.TrimEnd(new char[] { '/' });
                            savelinks.Add(new Uri(LinkUrl));
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            if (LinkUrl1.Contains("metacafe"))
            {
                List<string> linksList = new List<string>();
                string[] linksarray = Regex.Split(htmlDataReload, @"href=");
                foreach (string link in linksarray)
                {
                    try
                    {
                        string orglink = Regex.Split(link, @"""")[1];
                        if (orglink.StartsWith("/"))
                            savelinks.Add(new Uri("http://www.metacafe.com" + orglink.TrimEnd(new char[] { '/' })));
                        if (orglink.StartsWith("http://"))
                            savelinks.Add(new Uri(orglink.TrimEnd(new char[] { '/' })));
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            if (LinkUrl1.Contains("sevenload"))
            {
                try
                {
                    List<string> linksList = new List<string>();
                    string[] linksarray = Regex.Split(htmlDataReload, @"href=");
                    foreach (string link in linksarray)
                    {
                        string orglink = Regex.Split(link, @"""")[1];
                        if (orglink.StartsWith("/"))
                            savelinks.Add(new Uri("http://www.sevenload.com" + orglink.TrimEnd(new char[] { '/' })));
                        if (orglink.StartsWith("http://"))
                            savelinks.Add(new Uri(orglink.TrimEnd(new char[] { '/' })));
                    }
                }
                catch (Exception ex)
                {

                }
            }
            string href = "";
            for (int i = 0; i < regexImgSrc.Count(); i++)
            {
                MatchCollection matchesImgSrc = Regex.Matches(htmlDataReload, regexImgSrc[i], RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match m in matchesImgSrc)
                {
                    if (i == 0 || i == 5)
                    {
                        try
                        {
                            href = m.Groups[1].Value;
                            if (href.Contains("www."))
                            {
                                if (!href.Contains(".xml"))
                                {
                                    savelinks.Add(new Uri(href));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            count = savelinks.Count;
            return count;
        }
    }
}
#region YouTubeUriClass
public class YouTubeUri
{
    internal string url;

    public Uri Uri { get { return new Uri(url, UriKind.Absolute); } }
    public int Itag { get; set; }
    public string Type { get; set; }

    public bool IsValid
    {
        get { return url != null && Itag > 0 && Type != null; }
    }
}
#endregion
