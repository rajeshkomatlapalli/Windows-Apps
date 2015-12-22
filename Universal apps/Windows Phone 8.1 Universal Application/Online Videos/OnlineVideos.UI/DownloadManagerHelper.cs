using Common.Library;
//using OnlineVideos.Common;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Media;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace OnlineVideos.UI
{
 public static class    DownloadManagerHelper
    {
     public static  List<string> downloadtitle = new List<string>();
     public static List<Uri> Savevideos = new List<Uri>();
     public static ObservableCollection<DownloadPivot> SavevLinks = new ObservableCollection<DownloadPivot>();
     public static int k = 0;
     public static ObservableCollection<DownloadPivot> downloadVideoList = new ObservableCollection<DownloadPivot>();
     public static List<string> RemovieCheckobxItem = new List<string>();
     public static EventWaitHandle asyncWait = new ManualResetEvent(false);

     public static ObservableCollection<DownloadPivot> LoadImages(List<Uri> Image)
     {
         string path;
         ObservableCollection<DownloadPivot> download = new ObservableCollection<DownloadPivot>();
         try
         {
             foreach (Uri u in Image)
             {

                 if (u.ToString().StartsWith("http://"))
                 {

                     DownloadPivot d = new DownloadPivot();
                     d.Downloadimage = new BitmapImage(new Uri(u.ToString(), UriKind.Absolute));
                     if (u.ToString().StartsWith("http://img.youtube.com/vi/"))
                         d.title = u.ToString().Replace("http://img.youtube.com/vi/", "");
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
         catch (Exception ex)
         {
             Exceptions.SaveOrSendExceptions("Exception in LoadImages Method In DownloadManagerHelper.cs file", ex);
             
             
         }
         return download;
     }

     public static ObservableCollection<DownloadPivot> LoadAudios(List<Uri> Audio)
     {
         string path;
         ObservableCollection<DownloadPivot> download = new ObservableCollection<DownloadPivot>();
         try
         {
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
         catch (Exception ex)
         {
             Exceptions.SaveOrSendExceptions("Exception in LoadAudios Method In DownloadManagerHelper.cs file", ex);
             
         }
         return download;
     }

     public static ObservableCollection<DownloadPivot> LoadLinks(List<Uri> Link)
     {
         try
         {
             SavevLinks.Clear();
            
                 if (Link.Count() != 0)
                 {
                     string path;
                     foreach (Uri u in Link)
                     {
                         if (u.ToString().StartsWith("http://"))
                         {
                             if (u.ToString().StartsWith("http://www.dailymotion.com"))
                             {
                                 SavevLinks = DownloadManagerHelper.GetLinkTitleForDailymotionSite(SavevLinks, u);
                             }
                             else if (u.ToString().StartsWith("http://www.1channel.ch/"))
                             {
                                 DownloadPivot d = new DownloadPivot();
                                 string channellinktitle=u.ToString().Substring(u.ToString().LastIndexOf("/") + 1);
                                 d.title = channellinktitle;
                                 d.Downloaduri = new Uri(u.ToString().Replace(channellinktitle,""));
                                 if (d.title != "")
                                     SavevLinks.Add(d);
                             }

                             else
                             {
                                 DownloadPivot d = new DownloadPivot();
                                 d.title = u.ToString().Substring(u.ToString().LastIndexOf("/") + 1);
                                 d.Downloaduri = u;

                                 if (d.title != "")
                                     SavevLinks.Add(d);
                             }
                         }
                         else
                         {
                             DownloadPivot d = new DownloadPivot();
                             path = u.AbsoluteUri.ToString();
                             d.title = path.Substring(path.ToString().LastIndexOf("/") + 1);
                             d.Downloaduri = new Uri(path);

                             if (d.title != "")
                                 SavevLinks.Add(d);
                         }
                     }
                 }
                
             
         }
         catch (Exception ex)
         {
             Exceptions.SaveOrSendExceptions("Exception in LoadLinks Method In DownloadManagerHelper.cs file", ex);
         }

         return SavevLinks;
     }

     private static void Get1channelchLinks(string linkurl)
     {
         try
         {
             asyncWait.Reset();
             HttpClient wcb = new HttpClient();             
             //wcb.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; de; rv:1.9.2.12) Gecko/20101026 Firefox/3.6.12";
             //wcb.DownloadStringAsync(new Uri(linkurl));
             //wcb.DownloadStringCompleted += wcb_DownloadStringCompleted;
             asyncWait.WaitOne();
         }
         catch (Exception ex)
         {
             
            Exceptions.SaveOrSendExceptions("Exception in Get1channelchLinks Method In DownloadManagerHelper.cs file", ex);
         }                         
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
             d.title = title.Replace("/", "");
             d.Downloaduri = u;
             if (d.title != "")
                 download.Add(d);
            
         }
         catch (Exception ex)
         {
             
            Exceptions.SaveOrSendExceptions("Exception in GetLinkTitleForDailymotionSite Method In DownloadManagerHelper.cs file", ex);
         }
         return download;
     }

        public static void GetWebContent(string[] regexImgSrc)
        {
            try
            {
                if (AppSettings.NavigationUrl.Contains("dailymotion.com"))
                {
                    if (AppSettings.NavigationUrl == "http://touch.dailymotion.com/#/")
                        AppSettings.NavigationUrl = "http://dailymotion.com";
                    //DownloadManagerHelper.GetVideoLinksFromDailymotionSite(AppSettings.NavigationUrl);                 
                }
                if (AppSettings.NavigationUrl.ToString().StartsWith("http://www.1channel.ch/"))
                {

                    Get1channelchLinks(AppSettings.NavigationUrl);

                }
                if (AppSettings.NavigationUrl.StartsWith("http://pluralsight.com"))
                {
                    // Constants.SaveLinks = DownloadManagerHelper.GetLinksForPluralsight(Constants.SaveLinks);
                    //Constants.SaveVideos = DownloadManagerHelper.GetPlayerUrlForPluralsight(Constants.SaveVideos);
                }

                string htmlDataReload = AppSettings.htmltextfordownloadmanger;
                var httpClient = new HttpClient();
                var url = new Uri(AppSettings.NavigationUrl);
                var accessToken = "1234";
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                httpRequestMessage.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken));
                httpRequestMessage.Headers.Add("User-Agent", "My user-Agent");
                var response = Task.Run(async () => await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead)).Result;
                htmlDataReload = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
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
                //if (AppSettings.NavigationUrl.Contains("teluguone"))
                //{
                //    GetVideoLinksForteluguone(htmlDataReload, regexImgSrc);
                //}
                string href;
                string hrefforvip;

                for (int i = 0; i < regexImgSrc.Count(); i++)
                {
                    string text = AppSettings.htmltext;
                    MatchCollection matchesImgSrc = Regex.Matches(htmlDataReload, regexImgSrc[i], RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    foreach (Match m in matchesImgSrc)
                    {
                        href = m.Groups[1].Value;

                        if (href.ToString().EndsWith(".3gp") || href.ToString().EndsWith(".3g2") || href.ToString().StartsWith("http://www.youtube.com/v/") || href.ToString().StartsWith("#/watch?feature=related&amp;v=") || href.ToString().StartsWith("#/watch?v=") || href.StartsWith("/watch?v=") || href.ToString().StartsWith("#/watch?feature=relmfu&amp;v=") || href.ToString().EndsWith(".mp4") || href.ToString().EndsWith(".m4v") || href.ToString().EndsWith(".wmv"))
                        {
                            if (href.ToString().StartsWith("#/watch?feature=related&amp;v=") || ((href.ToString().StartsWith("http://www.youtube.com/v/")) && (href.ToString().EndsWith("&amp;hl=en&amp;fs=1&amp;rel=0"))) || href.ToString().StartsWith("#/watch?feature=relmfu&amp;v=") || href.ToString().StartsWith("#/watch?v=") || href.StartsWith("/watch?v=") || ((href.ToString().StartsWith("#/watch?v=")) && (href.ToString().EndsWith("&amp;feature=relmfu"))))
                            {

                                string youtubeid = href.ToString().Replace("#/watch?feature=related&amp;v=", "").Replace("#/watch?v=", "").Replace("/watch?v=", "").Replace("&amp;feature=relmfu", "").Replace("#/watch?feature=relmfu&amp;v=", "").Replace("http://www.youtube.com/v/", "").Replace("&amp;hl=en&amp;fs=1&amp;rel=0", "").Replace("&amp;feature=g-logo-xit", "");
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
                        href = m.Groups[1].Value;
                        if (href.ToString().EndsWith(".mp3") || href.EndsWith("=low") || href.ToString().EndsWith(".wav") || href.ToString().EndsWith(".aac") || href.ToString().EndsWith(".amr") || href.ToString().EndsWith(".wma"))
                        {
                            if (href.StartsWith("http://"))
                            {
                                if (href.EndsWith("=low"))
                                {
                                    string hrf = href + ".mp3";
                                    href = hrf;
                                }
                                Constants.SaveAudios.Add(new Uri(href));
                            }
                            else
                            {
                                string validurl = AppSettings.NavigationUrl + href;
                                Constants.SaveAudios.Add(new Uri(validurl));
                            }
                        }
                        if (i == 3)
                        {
                            href = m.Groups[1].Value;
                            if (href.StartsWith("http"))
                            {
                                Constants.SaveImages.Add(new Uri(href));
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
                                string mess = "Exception in GetWebContent Method In DownloadManagerHelper file\n\n" + ex.Message + " \n\n Stack Trace \n" + ex.StackTrace;
                                Exceptions.SaveOrSendExceptions(mess, ex);
                            }

                        }
                        if (Constants.count == 25)
                        {
                            if (Constants.SaveVideos.Count == 25)
                                break;
                        }
                        else
                        {
                            if (Constants.SaveVideos.Count == 12)
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string mess = "Exception in GetWebContent Method In DownloadManagerHelper file\n\n" + ex.Message + " \n\n Stack Trace \n" + ex.StackTrace;
                Exceptions.SaveOrSendExceptions(mess, ex);
            }
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
        public static string GetTitle(string RssDoc)
     {
         string str14 = string.Empty;
         try
         {
             str14 =GetTxtBtwn(RssDoc, "'VIDEO_TITLE': '", "'", 0);
             if (str14 == "") str14 = GetTxtBtwn(RssDoc, "\"title\" content=\"", "\"", 0);
             if (str14 == "") str14 =GetTxtBtwn(RssDoc, "&title=", "&", 0);
             str14 = str14.Replace(@"\", "").Replace("'", "&#39;").Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("+", " ");
           

         }
         catch (Exception ex)
         {
             Exceptions.SaveOrSendExceptions("Exception in GetTitle Method In DownloadManagerHelper.cs file", ex);
         }
         return str14;
     }
     
     public static string GetTxtBtwn(string input, string start, string end, int startIndex)
        {
            try
            {
                return GetTxtBtwn(input, start, end, startIndex, false);
            }
            catch (Exception ex)
            {
                
               Exceptions.SaveOrSendExceptions("Exception in GetTxtBtwn Method In DownloadManagerHelper.cs file", ex);
               return string.Empty;
            }
        }

     private static string GetTxtBtwn(string input, string start, string end, int startIndex, bool UseLastIndexOf)
        {
            int index1 = UseLastIndexOf ? input.LastIndexOf(start, startIndex) :
                                          input.IndexOf(start, startIndex);
            if (index1 == -1) return "";
            index1 += start.Length;
            int index2 = input.IndexOf(end, index1);
            if (index2 == -1) return input.Substring(index1);
            return input.Substring(index1, index2 - index1);
        }
    
    //static void wcb_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
    // {
    //     ObservableCollection<DownloadPivot> download = new ObservableCollection<DownloadPivot>();
    //     try
    //     {
    //         string html = e.Result.ToString();
    //         MatchCollection Gettbody = Regex.Matches(html, @"\<tbody\s?.*?\>((.|\r\n)+?)\<\/tbody\>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    //         foreach (Match body in Gettbody)
    //         {
    //             if (body.ToString().StartsWith("<tbody>"))
    //             {
    //                 Match GetUrl = Regex.Match(body.ToString(), @"<a[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    //                 string LinkUrl = GetUrl.Groups[1].Value;
    //                 if (!LinkUrl.StartsWith("http://"))
    //                 {
    //                     LinkUrl = "http://www.1channel.ch" + LinkUrl;
    //                 }
    //                 Match GetTitle = Regex.Match(body.ToString(), @"\<script\s?.*?\>((.|\r\n)+?)\<\/script\>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    //                 string title = GetTitle.Groups[1].Value.Replace("document.writeln", "").Replace("(", "").Replace(")", "").Replace("'", "").Replace(";", "");
    //                 string linkurl=string.Empty, Title = string.Empty;
    //                 Title = title;
    //                 linkurl = LinkUrl;
    //                 if (Title != "")
    //                     Constants.SaveLinks.Add(new Uri(linkurl +"/"+Title));
    //             }
    //         }
    //         MatchCollection GetHerf = Regex.Matches(html, @"<a[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

    //         foreach (Match her in GetHerf)
    //         {
    //             if (her.ToString().StartsWith("<a href"))
    //             {
    //                 string split = her.ToString().Substring(her.ToString().LastIndexOf("title") + 5).Replace("=", "").Replace("\"", "").Replace(">", "");
    //                 string href = her.Groups[1].Value;
    //                 if (!href.StartsWith("http://"))
    //                 {
    //                     href = "http://www.1channel.ch" + href;
    //                 }
    //                 string linkurl = string.Empty, Title = string.Empty;
    //                 Title = split;
    //                 linkurl = href;
    //                 if (Title != "")
    //                     Constants.SaveLinks.Add(new Uri(linkurl+"/"+ Title));
                     
    //             }

    //         }
          
    //     }
    //     catch (Exception ex)
    //     {
    //         Exceptions.SaveOrSendExceptions("Exception in wcb_DownloadStringCompleted Method In DownloadManagerHelper.cs file", ex);

    //     }
    //     asyncWait.Set();
    // }

    public static IEnumerable<DependencyObject> GetChildsRecursive(DependencyObject root)
    {
        List<DependencyObject> elts = new List<DependencyObject>();
        try
        {
            elts.Add(root);

            //for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            //{
            //    elts.AddRange(GetChildsRecursive(VisualTreeHelper.GetChild(root, i)));
            //}
        }
        catch (Exception ex)
        {
            Exceptions.SaveOrSendExceptions("Exception in GetChildsRecursive Method In DownloadManagerHelper.cs file", ex);
        }
        return elts;
    }

    public static List<string> CheckBoxCheckedItemList(ListView GridviewName)
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

    public static List<string> CheckBoxUnCheckedItemList(ListView GridviewName)
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

    //public async static void ReadDownloadStream(OpenReadCompletedEventArgs e, List<DownloadStatus> download)
    //{
    //    string path = "";
    //    try
    //    {
    //        if (e.Result != null)
    //        {
    //            string filepath = "";
    //            string FileName = "";

    //            string FolderStatus = "";

    //            IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
    //            bool Check = CheckSpace(e.Result.Length);
    //            if (Check == true)
    //            {
    //                long RequiredSize = e.Result.Length;
    //                byte[] SaveFile = new byte[RequiredSize];
    //                var xquery = from p in download where p.Id == Convert.ToInt32(e.UserState) select p;
    //                foreach (var itm in xquery)
    //                {
    //                    FileName = itm.RequestUri.ToString().Substring(itm.RequestUri.ToString().LastIndexOf('/') + 1);
    //                    path = itm.RequestUri.ToString();
    //                    FolderStatus = itm.FolderStatus;
    //                    AppSettings.ShowLinkTitle = itm.Title;
    //                }
    //                string ext = System.IO.Path.GetExtension(FileName);
    //                if (ext == ".doc")
    //                {

    //                    if (!isoStore.DirectoryExists(ResourceHelper.ProjectName + "/Documents"))
    //                        isoStore.CreateDirectory(ResourceHelper.ProjectName + "/Documents");
    //                    filepath = ResourceHelper.ProjectName + "/Documents" + FileName;
    //                }
    //                else if (ext == ".3gp" || ext == ".3g2" || ext == ".mp4" || ext == ".m4v" || ext == ".wmv")
    //                {

    //                    if (!isoStore.DirectoryExists(ResourceHelper.ProjectName + "/Videos"))
    //                        isoStore.CreateDirectory(ResourceHelper.ProjectName + "/Videos");
    //                    filepath = "/" + ResourceHelper.ProjectName + "/Videos/" + FileName;
    //                }
    //                else if (ext == ".mp3" || ext == ".wav" || ext == ".aac" || ext == ".amr" || ext == ".wma")
    //                {

    //                    if (!isoStore.DirectoryExists(ResourceHelper.ProjectName + "/Songs"))
    //                        isoStore.CreateDirectory(ResourceHelper.ProjectName + "/Songs");

    //                    filepath = ResourceHelper.ProjectName + "/Songs/" + FileName.Replace("&", "").Replace("=", "").Replace("|", "").Replace("?", "").Replace(":", "").Replace(";", "");
    //                }
    //                else
    //                {

    //                    if (!isoStore.DirectoryExists(ResourceHelper.ProjectName + "/Images"))
    //                        isoStore.CreateDirectory(ResourceHelper.ProjectName + "/Images");
    //                    filepath = "/" + ResourceHelper.ProjectName + "/Images/" + FileName;
    //                }
    //                if (!isoStore.FileExists(filepath))
    //                {
    //                    IsolatedStorageFileStream isoFile = new IsolatedStorageFileStream(filepath, System.IO.FileMode.Create, isoStore);
    //                    e.Result.Read(SaveFile, 0, SaveFile.Length);
    //                    isoFile.Write(SaveFile, 0, SaveFile.Length);
    //                    DownloadManager.DownloadingStatus(filepath, FolderStatus, RequiredSize);
    //                    DownloadManager.DeleteYoutubeDownloadUrls(int.Parse(e.UserState.ToString()));
    //                    isoFile.Close();
    //                }
    //                else
    //                {
    //                    DownloadManager.DeleteYoutubeDownloadUrls(int.Parse(e.UserState.ToString()));
    //                }
    //            }
    //            else
    //            {
    //                MessageDialog msgbox = new MessageDialog("No Space Available ");
    //                await msgbox.ShowAsync();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        DownloadManager.DeleteYoutubeDownloadUrls(int.Parse(e.UserState.ToString()));
    //        MessageDialog msgbox = new MessageDialog("Download faild :  " + AppSettings.ShowLinkTitle);
    //        //await msgbox.ShowAsync();
    //        Exceptions.SaveOrSendExceptions("Exception in wb_OpenReadCompleted Method In showlist.xaml.cs file", ex);
    //    }
    //}

    //public static void ProgressChangeStatus(DownloadProgressChangedEventArgs e, List<DownloadStatus> download)
    //{
    //    try
    //    {
    //        foreach (DownloadStatus d in download)
    //        {
    //            if (d.Id.ToString() == e.UserState.ToString())
    //            {
    //                d.ChapterProgressPosition = e.ProgressPercentage;
    //                d.TotalBytesToRecieve = e.TotalBytesToReceive.ToString();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
            
    //        Exceptions.SaveOrSendExceptions("Exception in ProgressChangeStatus Method In DownloadManagerHelper.cs file", ex);
    //    }
    //}

    public async static Task<bool> CheckSpace(int size)
    {
        bool SpaceAvailable = false;
        try
        {
            StorageFolder isoStore = ApplicationData.Current.LocalFolder;
            //long AvailableSpace = isoStore.AvailableFreeSpace;
            int AvailableSpace = await GetFreeSpace();
            if (size > AvailableSpace)
            {
                //if (!isoStore.IncreaseQuotaTo(isoStore.Quota + size))
                //{
                //    SpaceAvailable = false;
                //}
                //else
                //{
                //    SpaceAvailable = true;
                //}
            }
            else
            {
                SpaceAvailable = true;
            }
        }
        catch (Exception ex)
        {
            string excepmess = "Exception in CheckSpace Method In showlist file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            Exceptions.SaveExceptionInLocalStorage(excepmess);
        }
        return SpaceAvailable;
    }

    public async static Task<int> GetFreeSpace()
    {
        StorageFolder local = ApplicationData.Current.LocalFolder;
        var retrivedProperties = await local.Properties.RetrievePropertiesAsync(new string[] { "System.FreeSpace" });
        return (int)retrivedProperties["System.FreeSpace"];
    }

    //public async static void YoutubeWriteStream(OpenReadCompletedEventArgs e, List<DownloadStatus> download)
    //{
    //    var itm = download.Where(i => i.Id == int.Parse(e.UserState.ToString())).FirstOrDefault();
      
    //    string filepath = string.Empty;
    //    try
    //    {
    //        if (itm.Title != null)
    //            AppSettings.ShowLinkTitle = itm.Title;
    //        bool Check = DownloadManagerHelper.CheckSpace(e.Result.Length);
    //        if (Check == true)
    //        {
    //            var file = IsolatedStorageFile.GetUserStoreForApplication();
    //            filepath = AppSettings.ShowLinkTitle + ".mp4";
    //            if (!file.FileExists(filepath))
    //            {
    //                byte[] buffer = new byte[e.Result.Length];
    //                using (var stream = new IsolatedStorageFileStream(AppSettings.ShowLinkTitle + ".mp4", FileMode.Create, file))
    //                {
    //                    int bytesRead;
    //                    while ((bytesRead = e.Result.Read(buffer, 0, buffer.Length)) > 0)
    //                    {
    //                        stream.Write(buffer, 0, bytesRead);
    //                    }
    //                }
    //                DownloadManager.DownloadingStatus(filepath, "", buffer.Length);
    //                DownloadManager.DeleteYoutubeDownloadUrls(int.Parse(e.UserState.ToString()));

    //            }
    //        }
    //        else
    //        {
    //            MessageDialog msgbox = new MessageDialog("No Space Available ");
    //            await msgbox.ShowAsync();
    //        }

    //    }
    //    catch (NullReferenceException ex)
    //    {

    //    }
    //    catch (Exception ex)
    //    {
    //        MessageDialog msgbox = new MessageDialog("Download faild :  " + AppSettings.ShowLinkTitle + ex.Message);
    //        //await msgbox.ShowAsync();
    //        //MessageBox.Show("Download faild :  " + AppSettings.ShowLinkTitle + ex.Message);
    //        DownloadManager.DeleteYoutubeDownloadUrls(int.Parse(e.UserState.ToString()));
    //        Exceptions.SaveOrSendExceptions("Exception in YoutubeWriteStream Method In DownloadManagerHelper.cs file", ex);
    //    }
        
    //}

    public static string  GethtmlData(string rawHtml)
    {
        string title = string.Empty;
        var urls = new List<YouTubeUri>();
        var urlsfor480p = new List<YouTubeUri>();
        var uri = default(Uri);
        try
        {
            var match = Regex.Match(rawHtml, "url_encoded_fmt_stream_map\": \"(.*?)\"");
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

            var minTag = GetQualityIdentifier(YouTubeQuality.Quality720P);
            var maxTag = GetQualityIdentifier(YouTubeQuality.Quality720P);
            urlsfor480p.AddRange(urls);
            foreach (var u in urls.Where(u => u.Itag < minTag || u.Itag > maxTag).ToArray())
                urls.Remove(u);

            var entry = urls.OrderByDescending(u => u.Itag).FirstOrDefault();
            if (entry == null)
            {
                minTag = GetQualityIdentifier(YouTubeQuality.Quality480P);
                maxTag = GetQualityIdentifier(YouTubeQuality.Quality480P);
                foreach (var u in urlsfor480p.Where(u => u.Itag < minTag || u.Itag > maxTag).ToArray())
                    urlsfor480p.Remove(u);

                entry = urlsfor480p.OrderByDescending(u => u.Itag).FirstOrDefault();
            }


            if (entry != null)
            {

                uri = new Uri(entry.url);

            }
            else
            {
               uri=new Uri("http://www.youtube.com/Faild"); 
            }
        }
        catch (Exception ex)
        {
            Exceptions.SaveOrSendExceptions("Exception in GethtmlData Method In DownloadManagerHelper.cs file", ex);
           
        }
        return uri.ToString();

       
    }

    public static string GetYoutuVidoesUrlFor3Gpp(string rawHtml)
    {
        string title = string.Empty;
        var urls = new List<YouTubeUri>();
        var urlsfor480p = new List<YouTubeUri>();
        var uri = default(Uri);
        try
        {
            var match = Regex.Match(rawHtml, "url_encoded_fmt_stream_map\": \"(.*?)\"");
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
                            else if (key == "type" && value.Contains("video/3gpp"))
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

            var entry = urls.FirstOrDefault();


            if (entry != null)
            {

                uri = new Uri(entry.url);

            }
            else
            {
                uri = new Uri("http://www.youtube.com/Faild");
            }
        }
        catch (Exception ex)
        {

            Exceptions.SaveOrSendExceptions("Exception in GetYoutuVidoesUrlFor3Gpp Method In DownloadManagerHelper.cs file", ex);
        }
        return uri.ToString();


    }

    public  static int GetQualityIdentifier(YouTubeQuality quality)
    {
        switch (quality)
        {
            case YouTubeQuality.Quality480P: return 18;
            case YouTubeQuality.Quality720P: return 22;
            case YouTubeQuality.Quality1080P: return 37;
        }
        throw new ArgumentException("maxQuality");
    }

    public static List<DownloadStatus> LoadList(string Title, string link, int i )
    {
        List<DownloadStatus> download = new List<DownloadStatus>();
        try
        {
            List<DownloadInfo> objVideoList = new List<DownloadInfo>();
            string folderStatus = "";
            objVideoList = DownloadManager.LoadDownloadUrls();
            foreach (var itm in objVideoList)
            {
                folderStatus = itm.DownloadStatus;
            }

            if (objVideoList.Count() != 0)
            {

                DownloadStatus d = new DownloadStatus();
                d.Id = i;
                d.Title = Title;
                d.FolderStatus = folderStatus;
                d.ChapterProgressPosition = 0;
                d.RequestUri = link;
                download.Add(d);



            }
        }


        catch (Exception ex)
        {
            string excepmess = "Exception in LoadList Method In showlist file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            Exceptions.SaveExceptionInLocalStorage(excepmess);
            Exceptions.SaveOrSendExceptions("Exception in LoadList Method In DownloadManagerHelper.cs file", ex);
        }
        return download;
    }

    //public static void Playvideo(int ShowId)
    //{
    //    try
    //    {
    //        string linktype = LinkType.Songs.ToString();
    //        var ShowLinksByType = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == ShowId && i.LinkType == linktype).ToListAsync()).Result;
    //        foreach (var linkinfo in ShowLinksByType)
    //        {
    //            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
    //            {
    //                using (var isfs = new IsolatedStorageFileStream(linkinfo.LinkUrl, FileMode.Open, isf))
    //                {
    //                    MediaPlayerLauncher M;
    //                    M = new MediaPlayerLauncher();
    //                    M.Media = new Uri(linkinfo.LinkUrl, UriKind.RelativeOrAbsolute);
    //                    M.Controls = MediaPlaybackControls.All;
    //                    M.Show();
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Exceptions.SaveOrSendExceptions("Exception in Playvideo event In Playdownloadedvideo file.", ex);
    //    }
    //}

    public static void AppbarDownloadClick(List<string> chkboxes, List<DownloadPivot> YoutubeDownloadUrlList)
    {
        try
        {

            if (chkboxes.Count() != 0)
            {
                foreach (string s in chkboxes)
                {
                    if (s.StartsWith("http://www.youtube.com"))
                    {
                        string youtubeurl = s.Substring(s.LastIndexOf("v/") + 2);
                        string yurl = youtubeurl.Replace("&amp;hl=en&amp;fs=1&amp;rel=0", "");
                        var itm = YoutubeDownloadUrlList.Where(i => i.Downloaduri == new Uri(s)).FirstOrDefault();
                        string url = itm.YoutubeUrl;
                        DownloadManager.SaveDownloadingInfo(itm.title, "http://www.youtube.com/watch?v=" + url);
                       
                    }
                    else if (s.StartsWith("http://www.dailymotion.com") || s.StartsWith("http://vimeo.com") || s.Contains("veoh.com") || s.Contains("metacafe.com") || s.Contains("sevenload.com"))
                    {
                        if (s.EndsWith(".jpg") || s.EndsWith(".png") || s.EndsWith(".gif"))
                        {
                            DownloadManager.SaveDownloadingInfo("", s);
                        }
                        else
                        {
                            var itm = YoutubeDownloadUrlList.Where(i => i.Downloaduri == new Uri(s)).FirstOrDefault();
                            string url = itm.YoutubeUrl;
                            DownloadManager.SaveDownloadingInfo(itm.title, url);
                        }
                    }
                    else
                    {
                        DownloadManager.SaveDownloadingInfo("", s);

                    }
                }
            }
        }
        catch (Exception ex)
        {
            string mess = "Exception in chk_Checked Method In DownloadPivots.xaml file\n\n" + ex.Message + " \n\n Stack Trace \n" + ex.StackTrace;
            Exceptions.SaveOrSendExceptions(mess, ex);
        }
    }

    //public static void PlayStreamVideo(string url)
    //{
    //    var launcher = new MediaPlayerLauncher
    //    {
    //        Controls = MediaPlaybackControls.All,
    //        Media = new Uri(url.ToString(), UriKind.Absolute)
    //    };
    //    launcher.Show();
    //}

    //public static void GetVideoLinksFromDailymotionSite(string linkurl)
    //{
    //        asyncWait.Reset();
    //        HttpClient WcDailymotion = new HttpClient();
    //        WcDailymotion.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; de; rv:1.9.2.12) Gecko/20101026 Firefox/3.6.12";
    //        WcDailymotion.DownloadStringAsync(new Uri(linkurl));
    //        WcDailymotion.DownloadStringCompleted += WcDailymotion_DownloadStringCompleted;
    //        asyncWait.WaitOne();       
    //}

    //static void WcDailymotion_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
    //{
    //    List<string> videolink = new List<string>();
    //    MatchCollection Gettbody = Regex.Matches(e.Result, @"<a[^>]*?class\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    //    foreach (Match body in Gettbody)
    //    {
    //        Match GetUrl = Regex.Match(body.ToString(), @"<a[^>]*?href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    //        string LinkUrl = GetUrl.Groups[1].Value;
    //        if (LinkUrl.StartsWith("/video"))
    //        {
    //            if (!videolink.Contains(LinkUrl))
    //            {
    //                videolink.Add(LinkUrl);
    //                Constants.SaveVideos.Add(new Uri("http://www.dailymotion.com" + LinkUrl));
    //            }
    //        }
    //        if (!LinkUrl.StartsWith("http://") && LinkUrl.StartsWith("/in"))
    //        {
    //            LinkUrl = "http://www.dailymotion.com" + LinkUrl;
    //            Constants.SaveLinks.Add(new Uri(LinkUrl));
    //        }
    //    }
    //    asyncWait.Set();        
    //}

    private static void GetVideoUrlsForVimeo(string html)
    {

        try
        {
            MatchCollection Gettbody = Regex.Matches(html, @"<li[^>]*?id\s?.*?\>((.|\r\n)+?)\<\/li\>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
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
                    string VideoUrl = body.ToString().Split('"')[1];
                    Constants.SaveVideos.Add(new Uri(VideoUrl));
                }
            }
        }
        catch (Exception ex)
        {
            ex.Data.Add("NavigationUrl", AppSettings.NavigationUrl);
            Exceptions.SaveOrSendExceptions("Exception in GetvideoUrlForVeohSite Method In DownloadManagerHelper.cs file", ex);

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
            Exceptions.SaveOrSendExceptions("Exception in GetvideourlsFromBharthMovies Method In DownloadManagerHelper.cs file", ex);

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
                if (item.Contains("http://www.youtube.com/embed/") || item.Contains("https://www.youtube.com/embed"))
                {
                    string videoid = Regex.Split(item, @"""")[1].Replace("http://www.youtube.com/embed/", "");
                    string orgyoutubeid = "http://www.youtube.com/v/" + videoid + "&amp;hl=en&amp;fs=1&amp;rel=0";
                    Constants.SaveVideos.Add(new Uri(orgyoutubeid));
                    string imgur = "http://img.youtube.com/vi/" + videoid + "/default.jpg";
                    Constants.SaveImages.Add(new Uri(imgur));
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
            string title = string.Empty;
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
                            title = orglink.Substring(orglink.LastIndexOf("?"));
                            orglink = orglink.Replace(title, "");

                        }
                        Constants.SaveVideos.Add(new Uri("http://www.metacafe.com" + orglink + "&title=" + orglink.Split('/')[3] + "&imageurl=" + imgurl));
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

    private static void GetVideoLinksForteluguone(string html, string[] regexImgSrc)
    {
        try
        {
            string href;
            for (int i = 0; i < regexImgSrc.Count(); i++)
            {
                MatchCollection matchesImgSrc = Regex.Matches(AppSettings.htmltextfordownloadmanger, regexImgSrc[i], RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match m in matchesImgSrc)
                {
                    href = m.Groups[1].Value;


                    if (href.ToString().StartsWith("http://www.youtube.com/embed/"))
                    {

                        if (!Constants.SaveVideos.Contains(new Uri(href)))
                        {
                            string youtubeid = href.ToString().Replace("#/watch?feature=related&amp;v=", "").Replace("#/watch?v=", "").Replace("/watch?v=", "").Replace("&amp;feature=relmfu", "").Replace("#/watch?feature=relmfu&amp;v=", "").Replace("http://www.youtube.com/v/", "").Replace("http://www.youtube.com/embed/", "").Replace("&amp;hl=en&amp;fs=1&amp;rel=0", "").Replace("&amp;feature=g-logo-xit", "");
                            string orgyoutubeid = "http://www.youtube.com/v/" + youtubeid + "&amp;hl=en&amp;fs=1&amp;rel=0";
                            Constants.SaveVideos.Add(new Uri(href));
                            string imgur = "http://img.youtube.com/vi/" + youtubeid + "/default.jpg";
                            Constants.SaveImages.Add(new Uri(imgur));
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Exceptions.SaveOrSendExceptions("Exception in GetVideoLinksForteluguone Method In DownloadManagerHelper.cs file", ex);
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
                        Constants.SaveVideos.Add(new Uri("http://www.sevenload.com" + orglink + "&title=" + orglink.Split('/')[2] + "&imageurl=" + imgurl));
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

    }
 public  class YouTubeUri
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

}
