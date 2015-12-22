using Common.Library;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Reflection;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LinksFromOnline : Page
    {
        public List<Uri> allowedUris = new List<Uri>();
        public IDictionary<string, string[]> MusicDic = new Dictionary<string, string[]>();
        string absoluteuri = string.Empty;
        string language = string.Empty;
        public LinksFromOnline()
        {
            this.InitializeComponent();
            Loaded += LinksFromOnline_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            language = e.Parameter.ToString();

            if (e.NavigationMode != NavigationMode.Back)
            {
                MusicDic.Add("Hindi", new string[] { "http://www.songsaround.com/Hindisongs.aspx", "http://www.machomusiq.com/search/label/Hindi%20-MP3", "http://www.songslover.pk/bollywood/", "http://songspk3.in/bollywood.html", "http://webmusic.in/hindi.html", "http://www.pakheaven.com/music/indian-movies/" });
                MusicDic.Add("Telugu", new string[] { "http://www.songsaround.com/Telugusongslist.aspx", "http://www.machomusiq.com/search/label/Telugu%20-%20MP3", "http://songspk3.in/telugu.html" });
                MusicDic.Add("Tamil", new string[] { "http://www.machomusiq.com/search/label/Tamil%20-%20MP3", "http://songspk3.in/tamil.html", "http://tamiltunes.com/" });
                //tamil:"http://www.starmusiq.com/mp3database.asp";
            }
        }

        void LinksFromOnline_Loaded(object sender, RoutedEventArgs e)
        {
            if (Constants.NavigationFromOnlineLinks == true)
            {
                Constants.NavigationFromOnlineLinks = false;
                //Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);
                Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);
                frame.GoBack();
            }
            else
            {
                if (Constants.Linkstype == "Movies" || Constants.Linkstype == "Songs")
                {
                    listBox.Items.Add("http://www.youtube.com");
                    tb.Text = "http://www.youtube.com";
                    wv1.Navigate(new Uri("http://www.youtube.com", UriKind.RelativeOrAbsolute));
                    //wv1.LoadCompleted += wv1_LoadCompleted;
                }
                if (Constants.Linkstype == "Audio")
                {
                    listBox.ItemsSource = MusicDic.Where(i => i.Key == language).FirstOrDefault().Value;
                }
            }
            //AdRotatorWin8.Invalidate();         
        }

        void wv1_LoadCompleted(object sender, NavigationEventArgs e)
        {
            try
            {
                allowedUris.Add(new Uri(e.Uri.ToString()));
                //wv1.AllowedScriptNotifyUris = allowedUris;
                tb.Text = e.Uri.AbsoluteUri;
                progressbar.IsActive = false;
                txtbrowsefiles.IsEnabled = true;
                Constants.NavigatedUri = e.Uri.AbsoluteUri;
                //advisible.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", tb.Text);
                //  Exceptions.SaveOrSendExceptions("Exception in wv1_LoadCompleted Event In BrowserPage", ex);
            }
        }

        private void BackButton_Click_1(object sender, RoutedEventArgs e)
        {
            Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);
            if (frame.CanGoBack)
                frame.GoBack();
        }

        private void goB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                advisible.Visibility = Visibility.Visible;
                progressbar.IsActive = true;
                txtbrowsefiles.IsEnabled = false;
                Uri uri = new Uri("http://" + tb.Text);
                if (tb.Text.StartsWith("www"))
                {
                    uri = new Uri("http://" + tb.Text);
                }
                else
                {
                    uri = new Uri(tb.Text);
                }
                wv1.Navigate(uri);
                //wv1.LoadCompleted += wv1_LoadCompleted;


            }
            catch (Exception ex)
            {
                ex.Data.Add("NavigationUrl", tb.Text);
                //Exceptions.SaveOrSendExceptions("Exception in goB_Click Event In BrowserPage", ex);
                tb.Text = "invalid adress !";
            }
        }

        private async void txtbrowsefiles_Click_1(object sender, RoutedEventArgs e)
        {
            AppSettings.htmltext = await wv1.InvokeScriptAsync("eval", new String[] { "document.location.href;" });
            //string[] args = { "function newfunc_eventHandler(){window.external.notify(window.location.href);}" };
            //wv1.InvokeScript("eval", args);
            //wv1.InvokeScript("newfunc_eventHandler", null);

            //Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);
            Frame.Navigate(typeof(OnlineLinks));
            //frame.Navigate(typeof(OnlineLinks));
            //Window.Current.Content = frame;
            //Window.Current.Activate();
        }

        public async void audiolinks()
        {
            Constants.AudiosLinks.Clear();
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = await web.LoadFromWebAsync(Constants.NavigatedUri);
            if (Constants.NavigatedUri.Contains("songsaround.com"))
            {
                foreach (HtmlNode node in doc.DocumentNode.Descendants().Where(n => n.Name == "a" && n.Attributes.Contains("href")))
                {
                    if (node.Attributes["href"].Value.EndsWith(".mp3"))
                    {
                        Constants.AudiosLinks.Add(node.Attributes["href"].Value, node.Attributes["href"].Value.Substring(node.Attributes["href"].Value.LastIndexOf('/') + 1, (node.Attributes["href"].Value.IndexOf(".mp3")) - (node.Attributes["href"].Value.LastIndexOf('/'))));
                    }
                }
            }
            else
            {
                foreach (HtmlNode node in doc.DocumentNode.Descendants().Where(n => n.Name == "a" && n.Attributes.Contains("href")))
                {
                    if (node.Attributes["href"].Value.EndsWith(".mp3"))
                    {
                        Constants.AudiosLinks.Add(node.Attributes["href"].Value, node.InnerText);
                    }
                }
            }
            Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);
            //frame.Navigate(typeof(OnlineLinks));
            Window.Current.Content = frame;
            Window.Current.Activate();
        }

        private void wv1_ScriptNotify_1(object sender, NotifyEventArgs e)
        {
            allowedUris.Add(new Uri(e.Value.ToString()));
            wv1.AllowedScriptNotifyUris = allowedUris;
            tb.Text = e.Value.ToString();
            progressbar.IsActive = false;
            txtbrowsefiles.IsEnabled = true;
            Constants.NavigatedUri = e.Value.ToString();
            //advisible.Visibility = Visibility.Collapsed;

            if (Constants.Linkstype == "Audio")
                audiolinks();
            else
            {
                Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);
                //frame.Navigate(typeof(OnlineLinks));
                Window.Current.Content = frame;
                Window.Current.Activate();
            }
        }

        private void tb_GotFocus_1(object sender, RoutedEventArgs e)
        {
            listBox.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            listBox.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void listBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {
                var selecteditem = ((ListBox)sender).SelectedItem;
                tb.Text = selecteditem.ToString();
                listBox.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                listBox.SelectedIndex = -1;
            }
        }
    }
}