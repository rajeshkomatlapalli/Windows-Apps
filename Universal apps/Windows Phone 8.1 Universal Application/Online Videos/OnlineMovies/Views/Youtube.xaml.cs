using Common.Library;
//using Common.Library;
using Common.Utilities;
using OnlineVideos;
using OnlineVideos.Data;
using OnlineVideos.Views;
//using MyToolkit.Multimedia;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.Phone.UI.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineMovies.Views
{
    public class YouTubeUri
    {
        internal string url;
        public Uri Uri { get { return new Uri(url, UriKind.Absolute); } }
        public int Itag { get; set; }
        public string Type { get; set; }
        string myid;

        public bool IsValid
        {
            get { return url != null && Itag > 0 && Type != null; }
        }
    }
   
    public sealed partial class Youtube : Page
    {
        AppInsights insights = new AppInsights();
        Stopwatch stopwatch = new Stopwatch();
        public string YoutubeID = string.Empty;
         string ShowTitle = String.Empty;
         string titleval = string.Empty;
        public Youtube()
        {
            this.InitializeComponent();            
        }      

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            insights.pageview("Youtube viewed");
            string myid = (string)e.Parameter;          
            if (myid != null)
            {
                PlayYoutubeVideo(myid);
            }
           HardwareButtons.BackPressed += HardwareButtons_BackPressed;           
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
           if(Frame.CanGoBack)
           {
               var lpt = Frame.BackStack.Last().ToString();
               Frame.GoBack();
           }
        }

        public async void PlayYoutubeVideo(string url)
        {
            YoutubeID = url;
            MyToolkit.Multimedia.YouTubeUri uri;
            try
            {
                uri = await MyToolkit.Multimedia.YouTube.GetVideoUriAsync(YoutubeID, MyToolkit.Multimedia.YouTubeQuality.Quality480P);

                if (uri != null)
                {
                    mediaplayer.Source = uri.Uri;
                }
                else
                {
                    Frame.Navigate(typeof(Browser), YoutubeID);
                    //throw new Exception("no_video_urls_found");
                }
            }
            catch (Exception ex)
            {
                Frame.Navigate(typeof(Browser), YoutubeID);
                Exceptions.SaveOrSendExceptions("Exception in GethtmlData Method In Youtube.cs file", ex);
            }
        }

        async void DownloadStringCompleted(string uri)
        {
            try
            {
                HttpClient client = new HttpClient();
                var data = await client.GetStringAsync(uri);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }           
        }

        public void GethtmlData(string rawHtml)
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
                if (uri != null)
                {                    
                    LoadVideoPlayer(uri.ToString());
                }

                else
                {
                    AppSettings.startplaying = false;
                    AppSettings.startplayingforpro = false;
                    UtilitiesManager.LoadBrowserTask(YoutubeID);                   
                    //insights.Trace("Youtube Links are not working");
                }
            }
            catch (Exception ex)
            {
                AppSettings.startplaying = false;
                AppSettings.startplayingforpro = false;
                UtilitiesManager.LoadBrowserTask(YoutubeID);
                Exceptions.SaveOrSendExceptions("Exception in GethtmlData Method In Youtube.cs file", ex);
                //insights.Exception(ex);
            }
        }

        private void LoadVideoPlayer(string url)
        {            
            BackgroundMediaPlayer.Shutdown();
            mediaplayer.Source = new Uri(url, UriKind.Absolute);
            mediaplayer.Play();
        }
        public static int GetQualityIdentifier(YouTubeQuality quality)
        {
            switch (quality)
            {
                case YouTubeQuality.Quality480P: return 18;
                case YouTubeQuality.Quality720P: return 22;
                case YouTubeQuality.Quality1080P: return 37;
            }
            throw new ArgumentException("maxQuality");
        }
        private void mediaplayer_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void mediaplayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AppSettings.ShowID != null && AppSettings.ShowID != "")
                {
                    ShowTitle = OnlineShow.GetShowDetail(Convert.ToInt32(AppSettings.ShowID)).Title.ToUpper();
                    titleval = ShowTitle + " " + AppSettings.LinkTitle;
                }
                else
                {                    
                    titleval = AppSettings.LinkTitle;
                }
                //stopwatch = System.Diagnostics.Stopwatch.StartNew();
                //var properties = new Dictionary<string, string> { { titleval, " Viewed" } };
                //var metrics = new Dictionary<string, double> { { "processing time", stopwatch.Elapsed.TotalMilliseconds } };
                //insights.Event("Youtube Page Time", properties, metrics);  
                var success = false;
                var startTime = DateTime.UtcNow;
                var timer = System.Diagnostics.Stopwatch.StartNew();
                insights.Dependency("Youtube", "Duration", startTime, timer.Elapsed, success);
                insights.Event(titleval + "View");
                //List<Parameter> Params = new List<Parameter> { new Parameter(AppSettings.LinkType, AppSettings.LinkTitle) };
                //FlurryWP8SDK.Api.LogEvent(AppSettings.Title, Params, true);
            }
            catch (Exception ex)
            {
             
            }
        }

        private void mediaplayer_MediaEnded(object sender, Microsoft.PlayerFramework.MediaPlayerActionEventArgs e)
        {
            stopwatch.Stop();
            if (AppSettings.startplayingforpro == true && (ResourceHelper.AppName == Apps.Indian_Cinema_Pro.ToString() || ResourceHelper.AppName != Apps.Kids_TV_Pro.ToString() || ResourceHelper.AppName != Apps.Story_Time_Pro.ToString()))
            {
                if (Constants.YoutubePlayList.Count > 0)
                {
                    AppSettings.LinkUrl = Constants.YoutubePlayList.FirstOrDefault().Key;
                    AppSettings.LinkTitle = Constants.YoutubePlayList.FirstOrDefault().Value;
                    Constants.YoutubePlayList.Remove(Constants.YoutubePlayList.FirstOrDefault().Key);
                    AppSettings.PlayVideoTitle = Constants.YoutubePlayList.FirstOrDefault().Value;

                    string conn = AppSettings.LinkUrl;

                    if (ResourceHelper.AppName == Apps.Video_Mix.ToString())
                    {
                        AppSettings.LinkUrl= AppSettings.LinkUrl.Replace("&amp;hl=en&amp;fs=1&amp;rel=0", "").Replace("http://www.youtube.com/v/", "").Replace("https://www.youtube.com/v/", "");
                        PlayYoutubeVideo(AppSettings.LinkUrl);
                    }
                    else
                        PlayYoutubeVideo(AppSettings.LinkUrl);
                }
                else if (Constants.YoutubePlayList.Count == 0)
                {
                    AppSettings.startplayingforpro = false;
                    Frame.GoBack();
                }
            }
            else if (AppSettings.startplaying == true && ResourceHelper.AppName != Apps.Indian_Cinema_Pro.ToString() && ResourceHelper.AppName != Apps.Kids_TV_Pro.ToString() && ResourceHelper.AppName != Apps.Story_Time_Pro.ToString())
            {
                if (Constants.YoutubePlayList != null)
                {
                    if (Constants.YoutubePlayList.Count > 0)
                    {                       
                        PageHelper.RootApplicationFrame.Navigate(typeof(Advertisement));
                    }
                    else if (Constants.YoutubePlayList.Count == 0)
                    {
                        AppSettings.startplaying = false;
                        while (Frame.BackStack.Count() > 1)
                        {
                            if (Frame.BackStack.FirstOrDefault().SourcePageType.Equals("Youtube") || Frame.BackStack.FirstOrDefault().SourcePageType.Equals("Advertisement"))
                            {
                                Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
                            }
                            else
                                break;
                        }
                        Frame.GoBack();
                    }
                }
                else
                {
                    Frame.GoBack();
                }
            }
            else
                Frame.GoBack();
        }
    }
}