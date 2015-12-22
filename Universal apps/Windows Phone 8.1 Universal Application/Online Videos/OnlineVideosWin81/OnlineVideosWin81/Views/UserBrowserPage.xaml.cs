using Common.Library;
using HtmlAgilityPack;
using OnlineVideos.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Common.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using System.Reflection;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserBrowserPage : Page
    {
        AddShow addshow = new AddShow();
        string absolutepath = string.Empty;
        string description = string.Empty;
        public List<Uri> allowedUris = new List<Uri>();
        string searchquery = string.Empty;
        public UserBrowserPage()
        {
            this.InitializeComponent();
            Loaded += UserBrowserPage_Loaded;
        }

        void UserBrowserPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (description == "Cast")
            {
                string searc = string.Empty;
                foreach(string ss in searchquery.Split(' '))
                {
                    string sss = string.Empty;
                    char[] a = ss.ToCharArray();
                    a[0] = char.ToUpper(a[0]);
                    sss = new string(a);
                    if (string.IsNullOrEmpty(searc))
                    {
                        searc = sss;
                    }
                    else
                    {
                        searc = searc + "_" + sss;
                    }
                }
                wv1.Navigate(new Uri("http://en.wikipedia.org/wiki/" + searc, UriKind.RelativeOrAbsolute));
                this.IsEnabled = false;
            }
            else if (description == "Lyrics")
            {
                string searc = string.Empty;
                foreach (string ss in searchquery.Split(' '))
                {
                    string sss = string.Empty;
                    char[] a = ss.ToCharArray();
                    a[0] = char.ToUpper(a[0]);
                    sss = new string(a);
                    if (string.IsNullOrEmpty(searc))
                    {
                        searc = sss;
                    }
                    else
                    {
                        searc = searc + "_" + sss;
                    }
                }
                wv1.Navigate(new Uri("http://www.lyrics4u.co/search/label/" + searc, UriKind.RelativeOrAbsolute));
                this.IsEnabled = false;
            }
            else
            {
                wv1.Navigate(new Uri("http://www.google.com/search?q=" + description, UriKind.RelativeOrAbsolute));
                //wv1.Navigate(new Uri("http://www.questionpapers.net.in/question_papers/physics_questions/1.html", UriKind.RelativeOrAbsolute));
                this.IsEnabled = false;
            }
            if (!string.IsNullOrEmpty(Constants.FinalUrl))
            {
                wv1.Navigate(new Uri(Constants.FinalUrl, UriKind.RelativeOrAbsolute));
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string[] query = new string[2];
            query = e.Parameter as string[];
            description = query[0];
            searchquery = query[1];
        }

        private void wv1_ScriptNotify_1(object sender, NotifyEventArgs e)
        {
            try
            {
                List<ShowLinks> showlinks = addshow.GetShowLinks();
                if (description == "Lyrics")
                {
                    Constants.Lyrics = e.Value.ToString();
                    string lyrics = WebUtility.HtmlDecode(Regex.Replace(Constants.Lyrics, "<[^>]+?>", ""));
                    if (showlinks.Where(i => i.ShowID == AppSettings.ShowUniqueID).FirstOrDefault() != null)
                    {
                        if (showlinks.Where(i => i.ShowID == AppSettings.ShowUniqueID && i.LinkType == "Audio" && i.Title == AppSettings.LiricsLink).FirstOrDefault() != null)
                        {
                            ShowLinks lyricsList = showlinks.Where(i => i.ShowID == AppSettings.ShowUniqueID && i.LinkType == "Audio" && i.Title == AppSettings.LiricsLink).FirstOrDefault();
                            lyricsList.Description = lyrics;
                            Constants.connection.UpdateAsync(lyricsList);
                        }
                    }
                    Constants.Lyrics = string.Empty;
                    //Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);
                    Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().FullName);
                    frame.GoBack();
                }
                else
                {
                    string des = WebUtility.HtmlDecode(Regex.Replace(e.Value.ToString(), "<[^>]+?>", ""));
                    Constants.Description.Append(" " + des.Trim()).Replace("\t", "").Replace("\n", "").Replace("\r", "");

                    HttpClient http = new HttpClient();
                    HttpResponseMessage response = Task.Run(async () => await http.GetAsync(Constants.FinalUrl)).Result;
                    if (response != null)
                    {
                        MemoryStream mm = new MemoryStream();
                        string pagehtml = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
                        Constants.OnlineImageUrls1 = new ObservableCollection<string>();
                        Constants.OnlineImageUrls1 = GetImages(pagehtml);
                        Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().FullName);
                        frame.GoBack();
                    }
                }
            }
            catch(Exception ex)
            {
                string exc = ex.Message;
            }
        }

        public ObservableCollection<string> GetImages(string input)
        {
            ObservableCollection<string> list = new ObservableCollection<string>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(input);

            foreach (HtmlNode node in doc.DocumentNode.Descendants().Where(n => n.Name == "img"))
            {
                if (node.OuterHtml.Contains("src") || node.OuterHtml.Contains("data-src") || node.OuterHtml.Contains("dfr-src") || node.OuterHtml.Contains("deferredsrc"))
                {
                    if (((node.Attributes["src"] == null) ? (node.Attributes["data-src"] == null) ? (node.Attributes["dfr-src"] == null) ? node.Attributes["deferredsrc"] : node.Attributes["dfr-src"] : node.Attributes["data-src"] : node.Attributes["src"]).Value.StartsWith("http"))
                    {
                        list.Add(((node.Attributes["src"] == null) ? (node.Attributes["data-src"] == null) ? (node.Attributes["dfr-src"] == null) ? node.Attributes["deferredsrc"] : node.Attributes["dfr-src"] : node.Attributes["data-src"] : node.Attributes["src"]).Value);
                    }
                    else
                    {
                        list.Add(absolutepath + ((node.Attributes["src"] == null) ? (node.Attributes["data-src"] == null) ? (node.Attributes["dfr-src"] == null) ? node.Attributes["deferredsrc"] : node.Attributes["dfr-src"] : node.Attributes["data-src"] : node.Attributes["src"]).Value);
                    }
                }
            }
            return list;
        }

        private async void wv1_LoadCompleted_1(object sender, NavigationEventArgs e)
        {
            try
            {
                Constants.FinalUrl = e.Uri.ToString();
                absolutepath = e.Uri.Host.ToString() + e.Uri.AbsolutePath.ToString().Substring(0, e.Uri.AbsolutePath.ToString().LastIndexOf('/') + 1);
                allowedUris.Add(new Uri(e.Uri.ToString()));
                //wv1.AllowedScriptNotifyUris = allowedUris;
               // string[] args = { "this.newfunc_eventHandler=function(e){e.cancelBubble = true;var  dataValue= document.selection.createRange().htmlText.toString();window.external.notify(dataValue);}" };
               //await wv1.InvokeScriptAsync("eval", args);
               // string[] arg = { "document.body.addEventListener('copy',newfunc_eventHandler,true);" };
               //await wv1.InvokeScriptAsync("eval", arg);

                string inject = "function clickhandler(e){e.cancelBubble = true;var  dataValue= document.selection.createRange().htmlText.toString();window.external.notify(dataValue);}";
                await wv1.InvokeScriptAsync("eval",new List<string>{inject});
                //wv1.InvokeScript("eval", "this.Add_eventHandler=function() {for (var i = 0; i < document.all.length; i++) {document.all[i].addEventListener(\"copy\", clickhandler);}}");
                //wv1.InvokeScript("Add_eventHandler");
                this.IsEnabled = true;
            }
            catch(Exception ex)
            {
                string excp = ex.Message;
            }
        }

        private void BackButton_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);
                frame.GoBack();
            }
            catch(Exception ex)
            {
                string exc = ex.Message;
            }
        }
    }
}