using Common.Library;
using HtmlAgilityPack;
using OnlineVideos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Devices.Sensors;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Phone.UI.Input;
using Windows.Graphics.Display;
using OnlineVideos.View_Models;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using OnlineVideos.Views;
using System.Threading.Tasks;
using System.Net.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineMovies.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LinksFromOnline : Page
    {
        string uri = string.Empty;
        public IDictionary<string, string[]> MusicDic = new Dictionary<string, string[]>();
        Stack<Uri> history = new Stack<Uri>();
        string absoluteuri = string.Empty;
        bool NavigatingBack = false;
        string language = string.Empty;
        string tile = string.Empty;
        string showid = string.Empty;
        string absolutepath = string.Empty;
        List<AutoComplete> v = new List<AutoComplete>();
        private SimpleOrientationSensor _orientation;

        public string navigatingURL;
        public string loadedURL;
        string[] parameters = new string[2];

        public LinksFromOnline()
        {
            try
            {
                this.InitializeComponent();
                
                this.Loaded += LinksFromOnline_Loaded;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }           
        }

       async void _orientation_OrientationChanged(SimpleOrientationSensor sender, SimpleOrientationSensorOrientationChangedEventArgs args)
        {
            try
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
               {
                SimpleOrientation orientation = args.Orientation;
                switch (orientation)
                {
                    case SimpleOrientation.NotRotated:
                        WBrowser.Height = 740;
                        WBrowser.Width = 480;
                    txtlink.Width = 410;
                    txtlink.Margin = new Thickness(0, -5, 0, 0);
                    imgfldr.Width = 90;
                    imgfldr.Margin = new Thickness(0, -4, 0, 0);
                    break;

                    case SimpleOrientation.Rotated90DegreesCounterclockwise:
                    //LandscapeLeft
                    WBrowser.Height = 420;
                    WBrowser.Width = 800;
                    txtlink.Margin = new Thickness(0, -5, 0, 0);
                    txtlink.Width = 730;
                    imgfldr.Margin = new Thickness(-20, -5, 0, 0);
                    imgfldr.Width = 100;
                    break;

                    case SimpleOrientation.Rotated270DegreesCounterclockwise:
                    WBrowser.Height = 420;
                    WBrowser.Width = 800;
                    txtlink.Margin = new Thickness(0, -5, 0, 0);
                    txtlink.Width = 730;
                    imgfldr.Margin = new Thickness(-20, -5, 0, 0);
                    imgfldr.Width = 100;
                    break;

                    default:
                    WBrowser.Height = 740;
                    WBrowser.Width = 480;
                    txtlink.Width = 410;
                    txtlink.Margin = new Thickness(0, -5, 0, 0);
                    imgfldr.Width = 90;
                    imgfldr.Margin = new Thickness(0, -4, 0, 0);
                    break;
                }
               });
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in BrowserPage_OrientationChanged Method In browserpage file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                Exceptions.SaveExceptionInLocalStorage(excepmess);
                Exceptions.SaveOrSendExceptions("Exception in BrowserPage_OrientationChanged Method In LinksFromOnline.cs file.", ex);
            }
        }

        void LinksFromOnline_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }

        void LinksFromOnline_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {               
                //if (Constants.Linkstype == "Movies" || Constants.Linkstype == "Songs")
                //{
                //    Uri Source = new Uri("http://www.youtube.com");
                //   // WBrowser.Navigate(new Uri("http://www.youtube.com", UriKind.RelativeOrAbsolute));
                //    WBrowser.Source = Source;
                //}
                //else if (Constants.Linkstype == "Audio")
                //{
                //    Uri Source = new Uri("http://songspk3.in/telugu.html");
                //    //WBrowser.Navigate(new Uri("http://songspk3.in/telugu.html", UriKind.RelativeOrAbsolute));
                //    WBrowser.Source = Source;
                //}
                
                //if (ResourceHelper.AppName != Apps.Indian_Cinema_Pro.ToString() && ResourceHelper.AppName != Apps.Kids_TV_Pro.ToString() && ResourceHelper.AppName != Apps.Story_Time_Pro.ToString())
                //{
                   
                //}
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LinksFromOnline_Loaded Method In LinksFromOnline.cs file.", ex);
            }
        }

        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {                           
            string[] paramers = (string[])e.Parameter;

            showid = paramers[0].ToString();
            language = paramers[1].ToString();
            tile = paramers[2].ToString();
            //HardwareButtons.BackPressed += HardwareButtons_BackPressed;
                           
                if (language != null)
                {
                    language = language.ToString();
                }
                if (e.NavigationMode != NavigationMode.Back)
                {
                    MusicDic.Add("Hindi", new string[] { "http://www.songsaround.com/Hindisongs.aspx", "http://www.machomusiq.com/search/label/Hindi%20-MP3", "http://www.songslover.pk/bollywood/", "http://songspk3.in/bollywood.html", "http://webmusic.in/hindi.html", "http://www.pakheaven.com/music/indian-movies/" });
                    MusicDic.Add("Telugu", new string[] { "http://www.songsaround.com/Telugusongslist.aspx", "http://www.machomusiq.com/search/label/Telugu%20-%20MP3", "http://songspk3.in/telugu.html" });
                    MusicDic.Add("Tamil", new string[] { "http://www.machomusiq.com/search/label/Tamil%20-%20MP3", "http://songspk3.in/tamil.html", "http://tamiltunes.com/" });
                  
                }
                if (e.NavigationMode == NavigationMode.Back)
                {
                    
                    Frame.GoBack();
                }
                if (Constants.Linkstype == "Movies" || Constants.Linkstype == "Songs")
                {
                    Uri Source = new Uri("http://www.youtube.com", UriKind.RelativeOrAbsolute);
                    // WBrowser.Navigate(new Uri("http://www.youtube.com", UriKind.RelativeOrAbsolute));
                    WBrowser.Navigate(Source);
                }
                else if (Constants.Linkstype == "Audio")
                {
                    txtlink.ItemsSource = MusicDic.Where(i => i.Key == language).FirstOrDefault().Value;
                    //Uri Source = new Uri("http://songspk3.in/telugu.html", UriKind.RelativeOrAbsolute);
                    //WBrowser.Navigate(new Uri("http://songspk3.in/telugu.html", UriKind.RelativeOrAbsolute));
                    //WBrowser.Navigate(Source);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In LinksFromOnline.cs file.", ex);
            }
        }

        async void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            try
            {
                await WBrowser.InvokeScriptAsync("eval", new string[] { "history.go(-1)" });
                Constants.DownloadTimer.Start();                
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnBackKeyPress Method In LinksFromOnline.cs file.", ex);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom Method In LinksFromOnline.cs file.", ex);
            }
        }        

        private void webBrowser1_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs e)
        {
            try
            {
               
                if (!string.IsNullOrEmpty(e.Uri.DnsSafeHost) && !e.Uri.ToString().Contains("adserver") && !e.Uri.ToString().Contains("out.popads"))
                {
                    if (!e.Uri.ToString().Contains("http"))
                    {
                        e.Cancel = true;

                        MessageDialog result = new MessageDialog("cannot add links from this site");
                        Frame.GoBack();

                    }
                    else
                    {
                        if (NavigatingBack == false)
                        {
                            e.Cancel = true;
                            WBrowser.Navigate(new Uri(e.Uri.ToString(), UriKind.RelativeOrAbsolute));
                            imgfldr.Visibility = Visibility.Visible;
                            navigatingURL = e.Uri.ToString();
                            txtlink.Text = navigatingURL;
                            AppSettings.NavigationUrl = navigatingURL;
                            NavigatingBack = true;
                        }
                        else
                        {
                            NavigatingBack = false;
                        }
                    }
                    Constants.NavigatedUri = e.Uri.AbsoluteUri;

                }
                else
                    e.Cancel = true;

            }

            catch (Exception ex)
            {
                string excepmess = "Exception in webBrowser1_Navigating Method In browserpage file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                Exceptions.SaveExceptionInLocalStorage(excepmess);
                Exceptions.SaveOrSendExceptions("Exception in webBrowser1_Navigating Method In LinksFromOnline.cs file.", ex);
            }
        }
        
        private void webBrowser1_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            try
            {
                txtmessage.Text = args.Uri.ToString();
                string error = args.WebErrorStatus.ToString();
                if(!args.IsSuccess)
                {
                    Debug.WriteLine("Navigation to this page failed, check your internet connection.");
                }
                //Constants.NavigatedUri = args.Uri.AbsoluteUri;
                
                //absolutepath = args.Uri.Host.ToString() + args.Uri.AbsolutePath.ToString().Substring(0, args.Uri.AbsolutePath.ToString().LastIndexOf('/') + 1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in webBrowser1_Navigated Method In LinksFromOnline.cs file.", ex);
            }
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {

        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {

        }
       
        public async void audiolinks()
        {
            try
            {
                Constants.AudiosLinks.Clear();             
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; Trident/5.0)");
                string hh = Task.Run(async () => await client.GetStringAsync(AppSettings.htmltext)).Result;
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(hh);
                
                if (AppSettings.htmltext.Contains("songsaround.com"))
                {
                    foreach (HtmlNode node in doc.DocumentNode.DescendantNodes().Where(n => n.Name == "a" && n.Attributes.Contains("href")))
                    {
                        if (node.Attributes["href"].Value.EndsWith(".mp3"))
                        {
                            Constants.AudiosLinks.Add(node.Attributes["href"].Value, node.Attributes["href"].Value.Substring(node.Attributes["href"].Value.LastIndexOf('/') + 1, (node.Attributes["href"].Value.IndexOf(".mp3")) - (node.Attributes["href"].Value.LastIndexOf('/'))));
                        }
                    }
                }

                else
                {
                    foreach (HtmlNode node in doc.DocumentNode.DescendantNodes().Where(n => n.Name == "a" && n.Attributes.Contains("href")))
                    {
                        if (node.Attributes["href"].Value.EndsWith(".mp3"))
                        {
                            Constants.AudiosLinks.Add(node.Attributes["href"].Value, node.InnerText);
                        }
                    }
                }

                parameters[0] = showid;
                parameters[1] = tile;
                Frame.Navigate(typeof(OnlineLinks), parameters);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in audiolinks Method In LinksFromOnline.cs file.", ex);               
                MessageDialog result = new MessageDialog("cannot add links from this site"); 
            }
        }

        private void webBrowser1_LoadCompleted(object sender, NavigationEventArgs e)
        {
            try
            {
                                
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in webBrowser1_LoadCompleted Method In browserpage file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                Exceptions.SaveExceptionInLocalStorage(excepmess);
                Exceptions.SaveOrSendExceptions("Exception in webBrowser1_LoadCompleted Method In LinksFromOnline.cs file.", ex);
            }
        }

        private void txtlink_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            try
            {
                if (txtlink.Text != "")
                {
                    txtmessage.Visibility = Visibility.Collapsed;                   
                    if (e.Key == VirtualKey.Enter)
                    {
                        if (!txtlink.Text.ToLower().StartsWith("http://"))
                        {
                            if (txtlink.Text.ToLower().Contains("www."))
                                txtlink.Text = "http://" + txtlink.Text;
                            else
                                txtlink.Text = "http://www." + txtlink.Text;
                        }
                        WBrowser.Navigate(new Uri(txtlink.Text, UriKind.RelativeOrAbsolute));
                    }
                }
                else
                {
                    txtmessage.Visibility = Visibility.Visible;
                }
            }

            catch (Exception ex)
            {
                string excepmess = "Exception in txtlink_KeyUp Method In browserpage file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                Exceptions.SaveExceptionInLocalStorage(excepmess);
                Exceptions.SaveOrSendExceptions("Exception in txtlink_KeyUp Method In LinksFromOnline.cs file.", ex);
            }
        }

        private async void imgfldr_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            try
            {                
                AppSettings.htmltext = await WBrowser.InvokeScriptAsync("eval", new String[] { "document.location.href;" });

                await WBrowser.InvokeScriptAsync("eval", new string[] { "history.go(-1)" });                

                Windows.Web.Http.Filters.HttpBaseProtocolFilter myFilter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
                var cookieManager = myFilter.CookieManager;
                Windows.Web.Http.HttpCookieCollection myCookieJar = cookieManager.GetCookies(new Uri("http://www.youtube.com"));
                foreach (Windows.Web.Http.HttpCookie cookie in myCookieJar)
                {
                    cookieManager.DeleteCookie(cookie);
                }

                if (Constants.Linkstype == "Audio")
                {
                    audiolinks();
                }
                else
                {
                    parameters[0] = showid;
                    parameters[1] = tile;
                    Frame.Navigate(typeof(OnlineLinks), parameters);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in imgfldr_Tapped Method In LinksFromOnline.cs file.", ex);
            }
        }

        private void WBrowser_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                //System.Diagnostics.Debugger.Break();
            }
        }

        private void WBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            //if (Constants.Linkstype == "Movies" || Constants.Linkstype == "Songs")
            //{
            //    Uri Source = new Uri("http://www.youtube.com");
            //    // WBrowser.Navigate(new Uri("http://www.youtube.com", UriKind.RelativeOrAbsolute));
            //    WBrowser.Source = Source;
            //}
            //else if (Constants.Linkstype == "Audio")
            //{
            //    Uri Source = new Uri("http://songspk3.in/telugu.html");
            //    //WBrowser.Navigate(new Uri("http://songspk3.in/telugu.html", UriKind.RelativeOrAbsolute));
            //    WBrowser.Source = Source;
            //}
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void go_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                if (txtlink.Text != "")
                {
                    txtmessage.Visibility = Visibility.Collapsed;
                    
                        if (!txtlink.Text.ToLower().StartsWith("http://"))
                        {
                            if (txtlink.Text.ToLower().Contains("www."))
                                txtlink.Text = "http://" + txtlink.Text;
                            else
                                txtlink.Text = "http://www." + txtlink.Text;
                        }
                        WBrowser.Navigate(new Uri(txtlink.Text, UriKind.RelativeOrAbsolute));
                    
                }
                else
                {
                    txtmessage.Visibility = Visibility.Visible;
                }
            }

            catch (Exception ex)
            {
                string excepmess = "Exception in txtlink_KeyUp Method In browserpage file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                Exceptions.SaveExceptionInLocalStorage(excepmess);
                Exceptions.SaveOrSendExceptions("Exception in txtlink_KeyUp Method In LinksFromOnline.cs file.", ex);
            }
        }

        
    }
}
