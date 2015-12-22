using Common.Library;
using System;
using System.Collections.Generic;
using System.Linq;
#if WINDOWS_APP || ANDROID
using System.Net.Http;
#endif
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Windows;

namespace OnlineVideos.Data
{
    public enum dailyQuality
    {
        ld, sd, hd720, hd1080,hd
    }
    public static class DownloadManagerDailyMotion
    {
        public static EventWaitHandle asyncWait = new ManualResetEvent(false);
       public static  AutoResetEvent autoevent=new AutoResetEvent(false);
        //get the dailymotion video title
        public static string GetDailymotionVideoTitle(string html)
        {
            string Title = string.Empty;
            Match GetTitle = Regex.Match(html, @"\<title\s?.*?\>((.|\r\n)+?)\<\/title\>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Title = GetTitle.Groups[1].Value;
            return Title;
        }
//        public static void dailyDownloadwp8(string LinkUrl, dailyQuality quality)
//        {

           
//#if WP8
//            WebClient web=new WebClient();
//            web.DownloadStringAsync(new Uri(LinkUrl,UriKind.RelativeOrAbsolute),quality);
//            web.DownloadStringCompleted+=web_DownloadStringCompleted;
//#endif
           
//        }
//#if WP8
//static void web_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
//{
//    string video = null;
//    dailyQuality quality =(dailyQuality) e.UserState;
//    try
//    {
//        switch (quality)
//        {
//            case dailyQuality.ld:
//                video = Regex.Split(e.Result.ToString(), "video_url%22%3A%22")[1];
//                break;
//            case dailyQuality.sd:
//                video = Regex.Split(e.Result.ToString(), "video_url%22%3A%22")[1];
//                break;
//            case dailyQuality.hd:
//                video = Regex.Split(e.Result.ToString(), "video_url%22%3A%22")[1];
//                break;
//            case dailyQuality.hd720:
//                video = Regex.Split(e.Result.ToString(), "video_url%22%3A%22")[1];
//                break;
//            default:
//                break;
//        }


//    }
//    catch (Exception ex)
//    {
//        video = Regex.Split(e.Result.ToString(), "video_url%22%3A%22")[1];
//        //video = Regex.Split(pgdata, "sdURL%22%3A%22")[1];
//       // ex.Data.Add("LinkUrl", LinkUrl);
//        Exceptions.SaveOrSendExceptions("Exception in dailyDownload Method In DownloadManagerDailyMotion.cs file", ex);

//    }

//    video = Regex.Split(video, "%22%2C%22")[0];

//     NormalDaily(video);
//}
//        #endif
private static void NormalDaily(string data)
{
    string astring = data.Replace("%3A", ":");
    astring = astring.Replace("%5C%2F", "/");
    astring = astring.Replace("%3F", "?");
    astring = astring.Replace("%3D", "=");
    astring = astring.Replace("%253D", "=");
    astring = astring.Replace("%253F", "?");
    astring = astring.Replace("%253A", ":");
    astring = astring.Replace("%252F", "/");
    AppSettings.YoutubeUri = astring;
   
}
public static string dailyDownloadwp8(string LinkUrl, dailyQuality quality)
{
#if WINDOWS_PHONE_APP && NOTANDROID
    string pgdata = LinkUrl;

#else
  string pgdata = GetHtmlContent(LinkUrl);
#endif

    string video = null;

    try
    {
        switch (quality)
        {
            case dailyQuality.ld:
                video = Regex.Split(pgdata, "stream_h264_ld_url")[1];
                break;
            case dailyQuality.sd:
                video = Regex.Split(pgdata, "video_url%22%3A%22")[1];
                break;
            case dailyQuality.hd:
                video = Regex.Split(pgdata, "video_url%22%3A%22")[1];
                break;
            case dailyQuality.hd720:
                video = Regex.Split(pgdata, "video_url%22%3A%22")[1];
                break;
            default:
                return "...";
        }


    }
    catch (Exception ex)
    {
        video = Regex.Split(pgdata, "video_url%22%3A%22")[1];
        //video = Regex.Split(pgdata, "sdURL%22%3A%22")[1];
        ex.Data.Add("LinkUrl", LinkUrl);
        Exceptions.SaveOrSendExceptions("Exception in dailyDownload Method In DownloadManagerDailyMotion.cs file", ex);

    }
#if WINDOWS_PHONE_APP
    video = Regex.Split(video, ",")[0];

#else
            video = Regex.Split(video, "%22%2C%22")[0];
#endif
   

    return makeNormalDaily(video);
}
        //get the  download video url from dailymotion site.
        public static string dailyDownload(string LinkUrl, dailyQuality quality)
        {
			#if WINDOWS_PHONE_APP && NOTANDROID
            string pgdata = LinkUrl;

#else
  string pgdata = GetHtmlContent(LinkUrl);
#endif

            string video = null;

            try
            {
                switch (quality)
                {
                    case dailyQuality.ld:
                        video = Regex.Split(pgdata, "video_url%22%3A%22")[1];
                        break;
                    case dailyQuality.sd:
                        video = Regex.Split(pgdata, "video_url%22%3A%22")[1];
                        break;
                    case dailyQuality.hd:
                        video = Regex.Split(pgdata, "video_url%22%3A%22")[1];
                        break;
                    case dailyQuality.hd720:
                        video = Regex.Split(pgdata, "video_url%22%3A%22")[1];
                        break;
                    default:
                        return "...";
                }

               
            }
            catch (Exception ex)
            {
                video = Regex.Split(pgdata, "video_url%22%3A%22")[1];
                //video = Regex.Split(pgdata, "sdURL%22%3A%22")[1];
                ex.Data.Add("LinkUrl", LinkUrl);
                Exceptions.SaveOrSendExceptions("Exception in dailyDownload Method In DownloadManagerDailyMotion.cs file", ex);

            }
#if WINDOWS_PHONE_APP
            video = Regex.Split(video, ",")[0];

#else
            video = Regex.Split(video, "%22%2C%22")[0];
#endif
            return makeNormalDaily(video);
        }

        //make the normalDaily download video url
        private static string makeNormalDaily(string data)
        {
            string astring = data.Replace("%3A", ":");
            astring = astring.Replace("%5C%2F", "/");
            astring = astring.Replace("%3F", "?");
            astring = astring.Replace("%3D", "=");
            astring = astring.Replace("%253D", "=");
            astring = astring.Replace("%253F", "?");
            astring = astring.Replace("%253A", ":");
            astring = astring.Replace("%252F", "/");
#if WINDOWS_PHONE_APP
            astring = astring.Replace(@"\/", "/");
            astring = astring.Substring(astring.IndexOf("http://") + 7);
            astring = "http://" + astring.Substring(0, astring.Length - 1);
#endif
            AppSettings.YoutubeUri = astring;
            return astring;
        }

        //get the page html content by using link url
        private static string GetHtmlContent(string LinkUrl)
        {
            string pgdata = string.Empty;
			#if WINDOWS_APP || ANDROID
            try
            {
                var httpClient = new HttpClient();
                var url = new Uri(LinkUrl);
                var accessToken = "1234";
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                httpRequestMessage.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken));
                httpRequestMessage.Headers.Add("User-Agent", "My user-Agent");
                var response = Task.Run(async () => await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead)).Result;
                pgdata = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
                Constants.VideofileSize = Convert.ToInt32(response.Content.Headers.ContentLength);
                if (response.StatusCode.ToString() != "OK")
                {
                    Exceptions.SaveOrSendHttpclientExceptions("Exception in GetHtmlContent Method In DownloadManagerDailyMotion.cs file", response.ReasonPhrase, response.RequestMessage.RequestUri.ToString());
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("LinkUrl", LinkUrl);
                Exceptions.SaveOrSendExceptions("Exception in GetHtmlContent Method In DownloadManagerDailyMotion.cs file", ex);


            }
#endif
            return pgdata;
        }

        //get the  download video url from vimeo site.
        public static string GetDownloadvideoUrlforVimeoSite(string videolink)
        {
            string downloadUrl = string.Empty;
			#if WINDOWS_APP || ANDROID
            try
            {

                string pageData = GetHtmlContent(videolink);
				if(videolink.StartsWith("http://player.vimeo.com/v2/video"))
				{

					try {
						downloadUrl=Regex.Split(pageData,@"""url"":""")[2].Split('"')[0].ToString();
					} catch (Exception ex) {
						try {
							downloadUrl=Regex.Split(pageData,@"""url"":""")[1].Split('"')[0].ToString();
						} catch (Exception ex1) {
							
						}
					}
				}
				else
				{
                string clipId = null;
                if (Regex.Match(pageData, @"clip_id=(\d+)", RegexOptions.Singleline).Success)
                {
                    clipId = Regex.Match(pageData, @"clip_id=(\d+)", RegexOptions.Singleline).Groups[1].ToString();
                }
                else if (Regex.Match(pageData, @"(\d+)", RegexOptions.Singleline).Success)
                {
                    clipId = Regex.Match(pageData, @"(\d+)", RegexOptions.Singleline).Groups[1].ToString();
                }

                string sig = Regex.Match(pageData, "\"signature\":\"(.+?)\"", RegexOptions.Singleline).Groups[1].ToString();
                string timestamp = Regex.Match(pageData, "\"timestamp\":(\\d+)", RegexOptions.Singleline).Groups[1].ToString();
                string videoUrl = string.Format("http://player.vimeo.com/play_redirect?clip_id={0}&sig={1}&time={2}", clipId, sig, timestamp);
                var httpClient1 = new HttpClient();
                var url1 = new Uri(videoUrl);
                var accessToken1 = "1234";
                var httpRequestMessage1 = new HttpRequestMessage(HttpMethod.Get, url1);
                httpRequestMessage1.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken1));
                httpRequestMessage1.Headers.Add("User-Agent", "My user-Agent");
                var response1 = Task.Run(async () => await httpClient1.SendAsync(httpRequestMessage1, HttpCompletionOption.ResponseHeadersRead)).Result;
                Constants.VideofileSize = Convert.ToInt32(response1.Content.Headers.ContentLength);
                downloadUrl = response1.RequestMessage.RequestUri.ToString();
            }
			}
            catch (Exception ex)
            {
                ex.Data.Add("videoLink", videolink);
                Exceptions.SaveOrSendExceptions("Exception in GetDownloadvideoUrlforVimeoSite Method In DownloadManagerDailyMotion.cs file", ex);

              
            }
#endif
			# if WINDOWS_PHONE_APP && NOTANDROID
            try
            {
                string clipId = null;
                if (Regex.Match(videolink, @"clip_id=(\d+)", RegexOptions.Singleline).Success)
                {
                    clipId = Regex.Match(videolink, @"clip_id=(\d+)", RegexOptions.Singleline).Groups[1].ToString();
                }
                else if (Regex.Match(videolink, @"(\d+)", RegexOptions.Singleline).Success)
                {
                    clipId = Regex.Match(videolink, @"(\d+)", RegexOptions.Singleline).Groups[1].ToString();
                }

                string sig = Regex.Match(videolink, "\"signature\":\"(.+?)\"", RegexOptions.Singleline).Groups[1].ToString();
                string timestamp = Regex.Match(videolink, "\"timestamp\":(\\d+)", RegexOptions.Singleline).Groups[1].ToString();
                string videoUrl = string.Format("http://player.vimeo.com/play_redirect?clip_id={0}&sig={1}&time={2}", clipId, sig, timestamp);
                downloadUrl = videoUrl;
            }
            catch (Exception ex)
            {
                ex.Data.Add("videoLink", videolink);
                Exceptions.SaveOrSendExceptions("Exception in GetDownloadvideoUrlforVimeoSite Method In DownloadManagerDailyMotion.cs file", ex);


            }
#endif
            return downloadUrl;

        }

        //get the download video url from veoh site.
        public static string GetVideoUrlForVeohSite(string VideoUrl)
        {
            string video = string.Empty;

            try
            {

				#if WINDOWS_APP || ANDROID
                string html = GetHtmlContent(VideoUrl);
#else
                string html = VideoUrl;
#endif
                try
                {
                    if (html.Contains("fullPreviewHashHighPath"))
                        video = Regex.Split(html, "fullPreviewHashHighPath")[1].Split(',')[0].Replace(@"\", "").Replace(@"""", "").TrimStart(new char[] { ':' });
                    else
                    {
                        string href;
                        string[] regexImgSrc = { @"<a[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", @"<embed[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", @"<iframe[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", @"<video[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", @"<link[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>" };
                        for (int i = 0; i < regexImgSrc.Count(); i++)
                        {
                            MatchCollection matchesImgSrc = Regex.Matches(html, regexImgSrc[i], RegexOptions.IgnoreCase | RegexOptions.Singleline);
                            foreach (Match m in matchesImgSrc)
                            {
                                href = m.Groups[1].Value;

                                if (href.ToString().EndsWith(".3gp") || href.ToString().StartsWith("http://serials.") || href.ToString().EndsWith(".3g2") || href.ToString().StartsWith("http://www.youtube.com/v/") || href.ToString().StartsWith("#/watch?feature=related&amp;v=") || href.ToString().StartsWith("/watch?v=") || href.ToString().StartsWith("#/watch?feature=relmfu&amp;v=") || href.ToString().EndsWith(".mp4") || href.ToString().EndsWith(".m4v") || href.ToString().EndsWith(".wmv") || href.StartsWith("/watch?v=") || href.ToString().StartsWith("http://vipjatt.com/album/video") || href.ToString().StartsWith("http://vipjatt.com/album/video") || href.ToString().StartsWith("https://www.youtube.com"))
                                {


                                    if (href.ToString().StartsWith("#/watch?feature=related&amp;v=") || href.ToString().EndsWith("&hl=en&fs=1&rel=0") || href.ToString().StartsWith("/watch?v=")  || ((href.ToString().StartsWith("http://www.youtube.com/v/")) && (href.ToString().EndsWith("&amp;hl=en&amp;fs=1&amp;rel=0"))) || href.ToString().StartsWith("#/watch?feature=relmfu&amp;v=") || href.ToString().StartsWith("/watch?v=") || href.StartsWith("/watch?v=") || ((href.ToString().StartsWith("#/watch?v=")) && (href.ToString().EndsWith("&amp;feature=relmfu"))))
                                    {
                                        string youtubeid;

                                        youtubeid = href.ToString().Replace("#/watch?feature=related&amp;v=", "").Replace("/watch?v=", "").Replace("&amp;feature=relmfu", "").Replace("#/watch?feature=relmfu&amp;v=", "").Replace("http://www.youtube.com/v/", "").Replace("&amp;hl=en&amp;fs=1&amp;rel=0", "").Replace("&hl=en&fs=1&rel=0", "").Replace("?version=3&hl=en_US&rel=0?version=3&iv_load_policy=3&hl=en_US&rel=0", "").Replace("&amp;wide=1", "").Replace("http://serials.telugudailyserials.com/cv.php?url=", "").Replace("&source=youtube", "");
                                       
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
                                        Constants.SaveVideos.Add(new Uri(href));

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
                                          
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        string mess = "Exception in OnNavigatedTo Method In DownloadPivots file\n\n" + ex.Message + " \n\n Stack Trace \n" + ex.StackTrace;
                                        Exceptions.SaveOrSendExceptions(mess, ex);
                                    }

                                }
                            }
                        }
#if WINDOWS_PHONE_APP
                        Match sourcematch = Regex.Match(html, @"<source[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>");
                        string veohurl = sourcematch.Groups[1].Value;
                        if (veohurl.Contains(".mp4"))
                        {
                            video = veohurl;
                        }
#endif

                    }
                }
                catch (Exception ex)
                {
                    ex.Data.Add("Linkurl", VideoUrl);
                    Exceptions.SaveOrSendExceptions("Exception in GetVideoUrlForVeohSite Method In DownloadManagerDailyMotion.cs file", ex);

                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Linkurl", VideoUrl);
                Exceptions.SaveOrSendExceptions("Exception in GetVideoUrlForVeohSite Method In DownloadManagerDailyMotion.cs file", ex);
            }
            return video;
        }
      
        //get the download video url from MetaCafe Site
        public static string GetDownloadVideoUrlFromMetacafeSite(string LinkUrl)
        {
			#if WINDOWS_APP || ANDROID
           string pgdata = GetHtmlContent(LinkUrl);
#else
            string pgdata = LinkUrl;
#endif
            string video = null;
            string VideoUrl = string.Empty;
            try
            {
                if (pgdata.Contains("mediaURL%22%3A%22"))
                {
                    video = Regex.Split(pgdata, "mediaURL%22%3A%22")[2];

                    VideoUrl = Regex.Split(video, "%22%7D%5D%7D%7D")[0].Replace("%3A", ":").Replace("%5C%2F", "/").Replace("%25", "%");
                }
                else
                {
                    string[] videourl = Regex.Split(pgdata, "src");
                     foreach (string item in videourl)
                     {
                         if (item.Contains("http://www.youtube.com/embed/") || item.Contains("https://www.youtube.com/embed") || item.Contains("//www.youtube-nocookie.com/embed/") || item.Contains("www.youtube.com/embed/"))
                         {
                             string videoid = Regex.Split(item, @"""")[1].Replace("http://www.youtube.com/embed/", "").Replace("https://www.youtube.com/embed/", "").Replace("//www.youtube.com/embed/", "").Replace("www.youtube.com/embed/", "").Replace("//www.youtube-nocookie.com/embed/", "");
                             string orgyoutubeid = "http://www.youtube.com/v/" + videoid + "&amp;hl=en&amp;fs=1&amp;rel=0";
                             AppSettings.Youtubelink = orgyoutubeid;
#if WINDOWS_PHONE_APP
                             VideoUrl = orgyoutubeid;
# else
                             VideoUrl = videoid;
#endif
                         }
                     }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetDownloadVideoUrlFromMetacafeSite Method In DownloadManagerDailyMotion.cs file", ex);

            }
            if (VideoUrl.Contains(".14"))
            {
                VideoUrl = Regex.Split(video, "%22%7D%5D%7D%7D")[0].Replace("%3A", ":").Replace("%5C%2F", "/").Replace("%25", "%");
            }
            else
            {
                try
                {
                    if (pgdata.Contains("mediaURL%22%3A%22"))
                    {
                        video = Regex.Split(pgdata, "mediaURL%22%3A%22")[1];
                        VideoUrl = Regex.Split(video, "%22%7D%5D%7D%7D")[0].Replace("%3A", ":").Replace("%5C%2F", "/").Replace("%25", "%");
                    }
                    else
                    {
                        string[] videourl = Regex.Split(pgdata, "src");
                        foreach (string item in videourl)
                        {
                            if (item.Contains("http://www.youtube.com/embed/") || item.Contains("https://www.youtube.com/embed") || item.Contains("//www.youtube-nocookie.com/embed/") || item.Contains("www.youtube.com/embed/"))
                            {
                                string videoid = Regex.Split(item, @"""")[1].Replace("http://www.youtube.com/embed/", "").Replace("https://www.youtube.com/embed/", "").Replace("//www.youtube.com/embed/", "").Replace("www.youtube.com/embed/", "").Replace("//www.youtube-nocookie.com/embed/", "");
                                string orgyoutubeid = "http://www.youtube.com/v/" + videoid + "&amp;hl=en&amp;fs=1&amp;rel=0";
                                AppSettings.Youtubelink = orgyoutubeid;
#if WINDOWS_PHONE_APP
                                VideoUrl = orgyoutubeid;
# else
                             VideoUrl = videoid;
#endif
                            }
                        }
                    }
                    }
                catch (Exception ex)
                {
                    ex.Data.Add("LinkUrl", LinkUrl);
                    Exceptions.SaveOrSendExceptions("Exception in GetDownloadVideoUrlFromMetacafeSite Method In DownloadManagerDailyMotion.cs file", ex);

                }
            }
            if (VideoUrl.Contains("metacafe") && VideoUrl.Contains(".mp4"))
               VideoUrl= VideoUrl.Substring(0, VideoUrl.IndexOf(".mp4") + ".mp4".Length);
            return VideoUrl;
        }

        //get the video title from metacafe site
        public static string GetVideoTitleForMetaCafeSite(string url)
        {
            string VideoTitle = string.Empty;
            try
            {
                #if WINDOWS_APP
                string html = GetHtmlContent(url);
#else
                string html = url;
#endif
                VideoTitle = Regex.Match(html, @"\<title\s?.*?\>((.|\r\n)+?)\<\/title\>").Groups[1].Value;
            }
            catch (Exception ex)
            {
                ex.Data.Add("Linkurl", url);
                Exceptions.SaveOrSendExceptions("Exception in GetVideoTitleForMetaCafeSite Method In DownloadManagerDailyMotion.cs file", ex);

            }
            return VideoTitle;
        }
        //get the video url from schooltube site
        public static string GetvideoUrlFromSchooltube(string linkurl)
        {
            string videourl = string.Empty;
            try
            {
                 #if WINDOWS_APP
                string pgdata = GetHtmlContent(linkurl);
#else
                string pgdata = linkurl;
#endif
                videourl = Regex.Split(pgdata, " file:")[1].Split('"')[1];
            }
            catch (Exception ex)
            {
                ex.Data.Add("Linkurl", linkurl);
                Exceptions.SaveOrSendExceptions("Exception in GetvideoUrlFromSchooltube Method In DownloadManagerDailyMotion.cs file", ex);

            }
            return videourl;
        }

    }
}
