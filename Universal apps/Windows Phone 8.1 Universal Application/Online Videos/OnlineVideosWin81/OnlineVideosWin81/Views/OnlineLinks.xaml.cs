using Common.Library;
using HtmlAgilityPack;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

    public class Prop
    {
        public string VideoID
        {
            get;
            set;
        }

        public string VideoTitle
        {
            get;
            set;
        }

        public string ImageUrl
        {
            get;
            set;
        }

        public string Duration
        {
            get;
            set;
        }

        public string Views
        {
            get;
            set;
        }
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OnlineLinks : Page
    {
        //AddShow addshow = new AddShow();
        ObservableCollection<Prop> listprop = new ObservableCollection<Prop>();
        public List<string> chkboxes = new List<string>();

        public OnlineLinks()
        {
            this.InitializeComponent();
            Loaded += OnlineLinks_Loaded;
        }
        void OnlineLinks_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {


                listprop.Clear();
                if (Constants.Linkstype == "Audio")
                {
                    ApplicationTitle.Text = "Online Audio";
                    progressbaraudio.IsActive = true;
                    audiogrid.Visibility = Visibility.Visible;
                    videogrid.Visibility = Visibility.Collapsed;
                    lbxAudio.ItemsSource = listprop;
                    foreach (KeyValuePair<string, string> values in Constants.AudiosLinks)
                    {
                        Prop p = new Prop();
                        p.VideoID = values.Key;
                        p.VideoTitle = WebUtility.HtmlDecode(values.Value);
                        listprop.Add(p);
                    }
                    progressbaraudio.IsActive = false;
                }
                else
                {
                    ApplicationTitle.Text = "Online Videos";
                    progressbarvideo.IsActive = true;
                    audiogrid.Visibility = Visibility.Collapsed;
                    videogrid.Visibility = Visibility.Visible;
                    lbxVideo.ItemsSource = listprop;
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; Trident/5.0)");
                    string hh = Task.Run(async () => await client.GetStringAsync(AppSettings.htmltext)).Result;
                    //HtmlWeb web = new HtmlWeb();
                    //HtmlDocument doc = Task.Run(async () => await web.LoadFromWebAsync(Constants.NavigatedUri)).Result;
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(hh);
                    if (AppSettings.htmltext.Contains("v="))
                    {
                        foreach (HtmlNode node in doc.DocumentNode.DescendantNodes().Where(n => n.Name == "li"))
                        {
                            try
                            {
                                //http://www.youtube.com/watch?v=uIDx3eUZ-vw
                                if (node.Attributes.Where(i => i.Name == "class").FirstOrDefault() != null)
                                {
                                    if (node.Attributes["class"].Value.ToString().Contains("video-list-item"))
                                    {
                                        Prop p = new Prop();
                                        string hrefvalue = node.Descendants("a").FirstOrDefault().Attributes.Where(i => i.Name == "href").FirstOrDefault().Value.ToString();
                                        p.VideoID = hrefvalue.Substring(hrefvalue.LastIndexOf('=') + 1);
                                        p.VideoTitle = WebUtility.HtmlDecode(node.Descendants("a").Where(i => i.Attributes["class"] != null && i.Attributes["class"].Value != null).FirstOrDefault().Attributes["title"].Value);
                                        p.Duration = node.Descendants("span").Where(i => i.Attributes["class"] != null && i.Attributes["class"].Value == "video-time").FirstOrDefault().InnerText;
                                        p.Views = node.Descendants("span").Where(i => i.Attributes["class"] != null && i.Attributes["class"].Value == "stat view-count").FirstOrDefault().InnerText.Trim();
                                        p.ImageUrl = "http://i1.ytimg.com/vi/" + p.VideoID + "/default.jpg";
                                        listprop.Add(p);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
#if WINDOWS_APP
                                Exceptions.SaveOrSendExceptions("Exception in OnlineLinks_Loaded event In OnlineLinks.xaml.cs file", ex);
#endif
                            }
                        }
                    }
                    else if (AppSettings.htmltext.Contains("search_query="))
                    {
                        foreach (HtmlNode node in doc.DocumentNode.DescendantNodes().Where(n => n.Name == "li"))
                        {
                            //http://www.youtube.com/results?search_query=yevadu
                            if (node.Attributes.Where(i => i.Name == "class").FirstOrDefault() != null)
                            {
                                try
                                {
                                    if (node.Attributes["class"].Value.ToString().Contains("yt-lockup clearfix yt-uix-tile result-item-padding yt-lockup-video yt-lockup-tile"))
                                    {
                                        Prop p = new Prop();
                                        string hrefvalue = node.Descendants("a").FirstOrDefault().Attributes.Where(i => i.Name == "href").FirstOrDefault().Value.ToString();
                                        p.VideoID = hrefvalue.Substring(hrefvalue.LastIndexOf('=') + 1);
                                        string title = node.Descendants("a").Where(i => i.Attributes["class"] != null && i.Attributes["class"].Value == "yt-uix-sessionlink yt-uix-tile-link  yt-ui-ellipsis yt-ui-ellipsis-2").FirstOrDefault().InnerText;
                                        p.VideoTitle = WebUtility.HtmlDecode(title).Trim();
                                        p.Duration = node.Descendants("span").Where(i => i.Attributes["class"] != null && i.Attributes["class"].Value == "video-time").FirstOrDefault().InnerText;
                                        p.Views = node.Descendants("div").Where(i => i.Attributes["class"].Value == "yt-lockup-meta").FirstOrDefault().Descendants("ul").FirstOrDefault().Elements("li").Skip(2).FirstOrDefault().InnerHtml;
                                        p.ImageUrl = "http://i1.ytimg.com/vi/" + p.VideoID + "/default.jpg";
                                        // p.ImageUrl = "http:" + node.Descendants("img").FirstOrDefault().Attributes.Where(i => i.Name == "src").FirstOrDefault().Value.ToString().Trim();
                                        listprop.Add(p);
                                    }
                                }
                                catch (Exception ex)
                                {

                                    Exceptions.SaveOrSendExceptions("Exception in OnlineLinks_Loaded event In OnlineLinks.xaml.cs file", ex);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (HtmlNode node in doc.DocumentNode.DescendantNodes().Where(n => n.Name == "div"))
                        {

                            try
                            {
                                if (node.Attributes.Where(i => i.Name == "class").FirstOrDefault() != null)
                                {

                                    if (node.Attributes["class"].Value.ToString() == "yt-lockup clearfix  yt-lockup-video yt-lockup-grid vve-check")
                                    {
                                        Prop p = new Prop();
                                        string hrefvalue = node.Descendants("a").FirstOrDefault().Attributes.Where(i => i.Name == "href").FirstOrDefault().Value.ToString();
                                        p.VideoID = hrefvalue.Substring(hrefvalue.LastIndexOf('=') + 1);
                                        string title = node.Descendants("a").Where(i => i.Attributes["class"] != null && i.Attributes["class"].Value == "yt-uix-sessionlink yt-uix-tile-link  yt-ui-ellipsis yt-ui-ellipsis-2").FirstOrDefault().InnerText;
                                        p.VideoTitle = WebUtility.HtmlDecode(title).Trim();
                                        p.Duration = node.Descendants("span").Where(i => i.Attributes["class"] != null && i.Attributes["class"].Value == "video-time").FirstOrDefault().InnerText;
                                        p.Views = node.Descendants("div").Where(i => i.Attributes["class"].Value == "yt-lockup-meta").FirstOrDefault().Descendants("ul").FirstOrDefault().Elements("li").Skip(1).FirstOrDefault().InnerHtml;
                                        p.ImageUrl = "http://i1.ytimg.com/vi/" + p.VideoID + "/default.jpg";
                                        // p.ImageUrl = "http:" + node.Descendants("img").FirstOrDefault().Attributes.Where(i => i.Name == "src").FirstOrDefault().Value.ToString().Trim();
                                        listprop.Add(p);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Exceptions.SaveOrSendExceptions("Exception in OnlineLinks_Loaded event In OnlineLinks.xaml.cs file", ex);
                            }
                        }
                    }
                    progressbarvideo.IsActive = false;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }
        public string trimtitle(string title)
        {
            string itemtitle = title;
            if (title.Contains(","))
            {
                string tit = itemtitle;
                if (tit.Contains("||"))
                {
                    tit = tit.Replace("||", "");
                }
                if (tit.Contains("'"))
                {
                    tit = tit.Replace("'", "");
                }
                if (tit.Contains("-"))
                {
                    tit = tit.Replace("-", "");
                }
                if (tit.Contains("|"))
                {
                    tit = tit.Replace("|", "");
                }
                tit = tit.Replace(",", "");
                title = tit.Split(' ')[0];
                title = title + " " + tit.Split(' ')[1];
                title = title + " " + tit.Split(' ')[2];
                title = title + " " + tit.Split(' ')[3];
                title = title.Trim();
            }
            else
            {
                string tite = itemtitle;
                if (tite.Contains("-"))
                {
                    tite = tite.Replace("-", "");
                }
                if (tite.Contains("'"))
                {
                    tite = tite.Replace("'", "");
                }
                if (tite.Contains("||"))
                {
                    tite = tite.Replace("||", "");
                }
                if (tite.Contains("|"))
                {
                    tite = tite.Replace("|", "");
                }
                title = tite.Split(' ')[0];
                title = title + " " + tite.Split(' ')[1];
                title = title + " " + tite.Split(' ')[2];
                title = title + " " + tite.Split(' ')[3];
                title = title.Trim();
            }
            return title;
        }
        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Constants.NavigationFromOnlineLinks = true;
                //List<ShowLinks> showlinks = addshow.GetShowLinks();
                if (Constants.Linkstype == "Movies" || Constants.Linkstype == "Songs")
                {
                    foreach (string data in chkboxes)
                    {
                        Prop insertdata = listprop.Where(i => i.VideoID == data).FirstOrDefault();
                        int showid = AppSettings.ShowUniqueID;
                        string linktype = Constants.Linkstype;
                        ShowLinks links = new ShowLinks();

                        links.ShowID = AppSettings.ShowUniqueID;

                        if (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result != null)
                        {
                            if (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == linktype).FirstOrDefaultAsync()).Result != null ? Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == linktype).FirstOrDefaultAsync()).Result.LinkOrder != null : false)
                            {
                                links.LinkOrder = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == linktype).OrderByDescending(i => i.LinkOrder).FirstOrDefaultAsync()).Result.LinkOrder + 1;
                            }
                            else
                            {
                                links.LinkOrder = 1;
                            }
                        }
                        else
                        {
                            links.LinkOrder = 1;
                        }
                        links.LinkType = Constants.Linkstype;
                        links.LinkUrl = insertdata.VideoID;
                        links.Rating = 4.0;
                        links.RatingFlag = 0;
                        string videotitle = trimtitle(insertdata.VideoTitle);
                        links.Title = videotitle;
                        links.ClientPreferenceUpdated = DateTime.Now;
                        links.ClientShowUpdated = DateTime.Now;
                        int result = Task.Run(async () => await Constants.connection.InsertAsync(links)).Result;
                    }
                    Constants.NavigatedUri = string.Empty;
                    Constants.AudiosLinks.Clear();
                    Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);
                    frame.GoBack();

                    //if (Constants.Linkstype == "Movies")
                    //    NavigationService.Navigate(new Uri("/Views/Details.xaml?navigationvalue=" + Constants.navigationvalue + "&pivotindex=" + 2, UriKind.Relative));
                    //else
                    //    NavigationService.Navigate(new Uri("/Views/Details.xaml?navigationvalue=" + Constants.navigationvalue + "&pivotindex=" + 3, UriKind.Relative));
                }
                else if (Constants.Linkstype == "Audio")
                {
                    foreach (string data in chkboxes)
                    {
                        Prop insertdata = listprop.Where(i => i.VideoID == data).FirstOrDefault();
                        int showid = AppSettings.ShowUniqueID;
                        string linktype = Constants.Linkstype;
                        ShowLinks links = new ShowLinks();
                        links.ShowID = AppSettings.ShowUniqueID;
                        if (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result != null)
                        {
                            if (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == linktype).FirstOrDefaultAsync()).Result != null ? Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == linktype).FirstOrDefaultAsync()).Result.LinkOrder != null : false)
                            {
                                links.LinkOrder = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == linktype).OrderByDescending(i => i.LinkOrder).FirstOrDefaultAsync()).Result.LinkOrder + 1;
                            }
                            else
                            {
                                links.LinkOrder = 1;
                            }
                        }
                        else
                        {
                            links.LinkOrder = 1;
                        }
                        links.LinkType = Constants.Linkstype;
                        links.LinkUrl = insertdata.VideoID;
                        links.Rating = 4.0;
                        links.RatingFlag = 0;
                        links.Title = insertdata.VideoTitle;
                        links.ClientPreferenceUpdated = DateTime.Now;
                        links.ClientShowUpdated = DateTime.Now;
                        int result = Task.Run(async () => await Constants.connection.InsertAsync(links)).Result;
                    }
                    Constants.NavigatedUri = string.Empty;
                    Constants.AudiosLinks.Clear();
                    Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);
                    frame.GoBack();
                    //NavigationService.Navigate(new Uri("/Views/Details.xaml?navigationvalue=" + Constants.navigationvalue + "&pivotindex=" + 4, UriKind.Relative));
                }
                //NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                string gg = ex.Message;
            }
        }

        private void BackButton_Click_1(object sender, RoutedEventArgs e)
        {
            Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);
            frame.GoBack();
        }

        private void lbxVideo_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void chkVideo_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (!chkboxes.Contains(cb.Tag.ToString()))
                chkboxes.Add(cb.Tag.ToString());
        }

        private void chkVideo_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (chkboxes.Contains(cb.Tag.ToString()))
                chkboxes.Remove(cb.Tag.ToString());
        }

        private void chkAudio_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (!chkboxes.Contains(cb.Tag.ToString()))
                chkboxes.Add(cb.Tag.ToString());
        }

        private void chkAudio_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (chkboxes.Contains(cb.Tag.ToString()))
                chkboxes.Remove(cb.Tag.ToString());
        }
    }
}
