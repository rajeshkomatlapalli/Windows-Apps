using Common.Library;
using HtmlAgilityPack;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Common.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net;
using OnlineVideos.Views;
using Windows.Data.Json;
using System.Threading.Tasks;
using OnlineVideos;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineMovies.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserBrowserPage : Page
    {
        AddShow addshow = new AddShow();
        string absolutepath = string.Empty;
        string description = string.Empty;//It is also querytext 
        string searchquery = string.Empty;
        string navigationuri = string.Empty;
        string pagename = string.Empty;
        public List<string> NavigationHistory = new List<string>();

        public UserBrowserPage()
        {
            this.InitializeComponent();
            Loaded += UserBrowserPage_Loaded;
            //HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

       async void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            await webBrowser1.InvokeScriptAsync("eval", new string[] { "history.go(-1)" });
            NavigationHistory.RemoveAt(NavigationHistory.Count - 1);

            Constants.DownloadTimer.Start();
            if (Frame.CanGoBack)
            {
               if(pagename=="UserUploadPage")
               {
                   Frame.Navigate(typeof(UserUpload));
               }
                if(pagename=="AddCast")
                {
                   // Frame.Navigate(typeof(AddCast_New));
                }
            }
        }

        void UserBrowserPage_Loaded(object sender, RoutedEventArgs e)
        {            
            try
            {
               // FlurryWP8SDK.Api.LogEvent("UserBrowserPage", true);
                if (description == "Movies" || description == "Songs")
                {                    
                    webBrowser1.Navigate(new Uri("https://www.youtube.com/results?search_query=" + searchquery, UriKind.RelativeOrAbsolute));
                    this.IsEnabled = false;
                }
                else if (description == "Audio")
                {                    
                    webBrowser1.Navigate(new Uri("http://www.songslover.pk/", UriKind.RelativeOrAbsolute));
                    this.IsEnabled = false;
                }
                else if (description == "Cast")
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
                    webBrowser1.Navigate(new Uri("http://en.wikipedia.org/wiki/" + searc, UriKind.RelativeOrAbsolute));
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
                    int showid = AppSettings.ShowUniqueID;
                    if (Constants.connection.Table<CategoriesByShowID>().Where(i => i.ShowID == showid && i.CatageoryID == 18).FirstOrDefaultAsync().Result != null)
                    {                        
                        webBrowser1.Navigate(new Uri("http://www.atozsongslyrics.com/", UriKind.RelativeOrAbsolute));
                    }
                    else if (Constants.connection.Table<CategoriesByShowID>().Where(i => i.ShowID == showid && i.CatageoryID == 19).FirstOrDefaultAsync().Result != null)
                    {                        
                        webBrowser1.Navigate(new Uri("http://www.magicalsongs.net/", UriKind.RelativeOrAbsolute));
                    }
                    else                        
                        webBrowser1.Navigate(new Uri("http://www.lyricsmint.com/", UriKind.RelativeOrAbsolute));                    
                    this.IsEnabled = false;
                }
                else
                {                    
                    webBrowser1.Navigate(new Uri("http://www.google.com/search?q=" + description, UriKind.RelativeOrAbsolute));                    
                    this.IsEnabled = false;
                }                
                webBrowser1.NavigationCompleted += new TypedEventHandler<WebView,WebViewNavigationCompletedEventArgs>(webBrowser1_NavigationCompleted);                
                if (!string.IsNullOrEmpty(Constants.FinalUrl) && (ResourceHelper.AppName == Apps.Online_Education.ToString() || ResourceHelper.AppName == Apps.DrivingTest.ToString()))
                {                    
                    webBrowser1.Navigate(new Uri(Constants.FinalUrl));
                }
            }
            catch (Exception ex)
            {
             Exceptions.SaveOrSendExceptions("Exception in UserBrowserPage_Loaded event In UserBrowserPage.cs file.", ex);
            }
        }

        void webBrowser1_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            try
            {
                Constants.FinalUrl = args.Uri.ToString();
                if (!NavigationHistory.Contains(args.Uri.ToString()))
                    NavigationHistory.Add(args.Uri.ToString());
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in webBrowser1_Navigated event In UserBrowserPage.cs file.", ex);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //HardwareButtons.BackPressed+=HardwareButtons_BackPressed;
            string[] parameters = (string[])e.Parameter;
            try
            {
                //FlurryWP8SDK.Api.LogPageView();
                description = parameters[0];
                searchquery = parameters[1];
                pagename = parameters[2];
           }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo event In UserBrowserPage.cs file.", ex);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
               // FlurryWP8SDK.Api.EndTimedEvent("UserBrowserPage");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom event In UserBrowserPage.cs file.", ex);
            }
        }

        private async void NavigateBack()
        {
            try
            {
                await webBrowser1.InvokeScriptAsync("eval",new string[] {"history.go(-1)"});
                NavigationHistory.RemoveAt(NavigationHistory.Count - 1);
            }
            catch
            {
               // NavigationService.GoBack();
                Frame.GoBack();
            }
        }

        private async void webBrowser1_LoadCompleted(object sender, NavigationEventArgs e)
        {
            absolutepath = e.Uri.Host.ToString() + e.Uri.AbsolutePath.ToString().Substring(0, e.Uri.AbsolutePath.ToString().LastIndexOf('/') + 1);
            if (description == "Audio")
            {                
                this.IsEnabled = true;
            }
            else
            {              
                try
                {
                   //await webBrowser1.InvokeScriptAsync("eval",new string[] {"function clickhandler(e){e.cancelBubble = true;var  dataValue= document.selection.createRange().htmlText.toString();window.external.notify(dataValue);}"});
                   //await webBrowser1.InvokeScriptAsync("eval",new string[] {"this.Add_eventHandler=function() {for (var i = 0; i < document.all.length; i++) {document.all[i].addEventListener(\"copy\", clickhandler);}}"});
                   //await webBrowser1.InvokeScriptAsync("Add_eventHandler",new string[] {""});
                    this.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    Exceptions.SaveOrSendExceptions("Exception in webBrowser1_LoadCompleted event In UserBrowserPage.cs file.", ex);
                }
            }
        }

        private void webBrowser1_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {          
            navigationuri = args.Uri.ToString();
        }

        private async void webBrowser1_ScriptNotify(object sender, NotifyEventArgs e)
        {
            try
            {
                List<ShowLinks> showlinks = addshow.GetShowLinks();
                if (description == "Lyrics")
                {
                    Constants.Lyrics = e.Value.ToString();                    
                    string lyrics = WebUtility.HtmlDecode(Regex.Replace(Constants.Lyrics, "<[^>]+?>", ""));                    
                    ShowLinks links = new ShowLinks();

                    if (showlinks.Where(i => i.ShowID == AppSettings.ShowUniqueID).FirstOrDefault() != null)
                    {
                        if (showlinks.Where(i => i.ShowID == AppSettings.ShowUniqueID && i.LinkType == "Audio" && i.Title == AppSettings.LiricsLink).FirstOrDefault() != null)
                        {
                            ShowLinks lyricsList = showlinks.Where(i => i.ShowID == AppSettings.ShowUniqueID && i.LinkType == "Audio" && i.Title == AppSettings.LiricsLink).FirstOrDefault();
                            lyricsList.Description = lyrics;
                            await Constants.connection.UpdateAsync(lyricsList);
                        }
                    }
                    Constants.Lyrics = string.Empty;
                    Constants.DownloadTimer.Start();
                    Frame.GoBack();
                }
                else
                {
                    string des = WebUtility.HtmlDecode(Regex.Replace(e.Value.ToString(), "<[^>]+?>", ""));
                    Constants.Description.Append(" " + des.Trim()).Replace("\t", "").Replace("\n", "").Replace("\r", "");
                    HttpClient wb = new HttpClient();                   
                    var response= await wb.GetAsync(new Uri(Constants.FinalUrl, UriKind.Absolute));
                    var responseString = await response.Content.ReadAsStringAsync();                 
                    try
                    {
                        string pagehtml = responseString;
                        Constants.OnlineImageUrls1 = new ObservableCollection<string>();
                        Constants.OnlineImageUrls1 = GetImages(pagehtml);
                        Constants.DownloadTimer.Start();
                        Frame.GoBack();
                    }
                    catch (Exception ex)
                    {
                        Constants.DownloadTimer.Start();
                        Exceptions.SaveOrSendExceptions("Exception in wb_DownloadStringCompleted event In UserBrowserPage.cs file.", ex);
                        Frame.GoBack();
                    }                                 
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in webBrowser1_ScriptNotify event In UserBrowserPage.cs file.", ex);
            }
        }
                   
        public ObservableCollection<string> GetImages(string input)
        {
            ObservableCollection<string> list = new ObservableCollection<string>();
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(input);
                foreach (HtmlNode node in doc.DocumentNode.Descendants(" //img"))
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
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetImages event In UserBrowserPage.cs file.", ex);
            }
            return list;
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
