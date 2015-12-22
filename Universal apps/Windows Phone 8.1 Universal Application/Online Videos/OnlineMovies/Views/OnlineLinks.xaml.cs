using Common.Library;
using Common.Utilities;
using HtmlAgilityPack;
using OnlineVideos;
using OnlineVideos.Entities;
using OnlineVideos.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineMovies.Views
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

    public sealed partial class OnlineLinks : Page
    {

        ObservableCollection<Prop> listprop = new ObservableCollection<Prop>();
        public List<string> chkboxes = new List<string>();
        string showid = string.Empty;
        string tile = string.Empty;
        string[] parameters = new string[2];

        public OnlineLinks()
        {
            this.InitializeComponent();
            Loaded += OnlineLinks_Loaded;
        }

        void OnlineLinks_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadAds();
                if (ResourceHelper.AppName != Apps.Indian_Cinema_Pro.ToString() && ResourceHelper.AppName != Apps.Kids_TV_Pro.ToString() && ResourceHelper.AppName != Apps.Story_Time_Pro.ToString())
                {

                }

                if (Constants.Linkstype == "Audio")
                {
                    audiogrid.Visibility = Visibility.Visible;
                    videogrid.Visibility = Visibility.Collapsed;
                    chkAudio.ItemsSource = listprop;

                    foreach (KeyValuePair<string, string> values in Constants.AudiosLinks)
                    {
                        Prop p = new Prop();
                        p.VideoID = values.Key;
                        p.VideoTitle = WebUtility.HtmlDecode(values.Value);
                        listprop.Add(p);
                    }
                    _PerformanceProgressBar.IsIndeterminate = false;
                    if (listprop.Count() == 0)
                    {
                        tblkAudio.Visibility = Visibility.Visible;
                    }
                    else
                        tblkAudio.Visibility = Visibility.Collapsed;
                }
                else
                {
                    audiogrid.Visibility = Visibility.Collapsed;
                    videogrid.Visibility = Visibility.Visible;
                    lbxVideo.ItemsSource = listprop;
                                        
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; Trident/5.0)");
                    string hh = Task.Run(async () => await client.GetStringAsync(AppSettings.htmltext)).Result;
                    HtmlDocument doc = new HtmlDocument();
                    
                    doc.LoadHtml(hh);
                    if (AppSettings.htmltext.Contains("v="))
                    {
                        foreach (HtmlNode node in doc.DocumentNode.DescendantNodes().Where(n => n.Name == "li"))
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
                    }
                    else if (AppSettings.htmltext.Contains("search_query="))
                    {
                        foreach (HtmlNode node in doc.DocumentNode.DescendantNodes().Where(n => n.Name == "div"))
                        {
                            //http://www.youtube.com/results?search_query=yevadu
                            if (node.Attributes.Where(i => i.Name == "class").FirstOrDefault() != null)
                            {
                                if (node.Attributes["class"].Value.ToString().Contains("yt-lockup yt-lockup-tile yt-lockup-video clearfix yt-uix-tile"))
                                {
                                    Prop p = new Prop();
                                    string hrefvalue = node.Descendants("a").FirstOrDefault().Attributes.Where(i => i.Name == "href").FirstOrDefault().Value.ToString();
                                    p.VideoID = hrefvalue.Substring(hrefvalue.LastIndexOf('=') + 1);
                                    string title = node.Descendants("a").Where(i => i.Attributes["class"] != null && i.Attributes["class"].Value == "yt-uix-tile-link yt-ui-ellipsis yt-ui-ellipsis-2 yt-uix-sessionlink     spf-link ").FirstOrDefault().InnerText;
                                    p.VideoTitle = WebUtility.HtmlDecode(title).Trim();
                                    p.Duration = node.Descendants("span").Where(i => i.Attributes["class"] != null && i.Attributes["class"].Value == "video-time").FirstOrDefault().InnerText;
                                    p.Views = node.Descendants("div").Where(i => i.Attributes["class"].Value == "yt-lockup-meta").FirstOrDefault().Descendants("ul").FirstOrDefault().Elements("li").Skip(2).FirstOrDefault().InnerHtml;
                                    p.ImageUrl = "http://i1.ytimg.com/vi/" + p.VideoID + "/default.jpg";
                                    // p.ImageUrl = "http:" + node.Descendants("img").FirstOrDefault().Attributes.Where(i => i.Name == "src").FirstOrDefault().Value.ToString().Trim();
                                    listprop.Add(p);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (HtmlNode node in doc.DocumentNode.DescendantNodes().Where(n => n.Name == "div"))
                        {
                            if (node.Attributes.Where(i => i.Name == "class").FirstOrDefault() != null)
                            {
                                if (node.Attributes["class"].Value.ToString() == "yt-lockup clearfix  yt-lockup-video yt-lockup-grid vve-check")
                                {
                                    Prop p = new Prop();
                                    string hrefvalue = node.Descendants("a").FirstOrDefault().Attributes.Where(i => i.Name == "href").FirstOrDefault().Value.ToString();
                                    p.VideoID = hrefvalue.Substring(hrefvalue.LastIndexOf('=') + 1);
                                    string title = node.Descendants("a").Where(i => i.Attributes["class"] != null && i.Attributes["class"].Value == "yt-uix-sessionlink yt-uix-tile-link  spf-link  yt-ui-ellipsis yt-ui-ellipsis-2").FirstOrDefault().InnerText;
                                    p.VideoTitle = WebUtility.HtmlDecode(title).Trim();
                                    p.Duration = node.Descendants("span").Where(i => i.Attributes["class"] != null && i.Attributes["class"].Value == "video-time").FirstOrDefault().InnerText;
                                    p.Views = node.Descendants("div").Where(i => i.Attributes["class"].Value == "yt-lockup-meta").FirstOrDefault().Descendants("ul").FirstOrDefault().Elements("li").Skip(1).FirstOrDefault().InnerHtml;
                                    p.ImageUrl = "http://i1.ytimg.com/vi/" + p.VideoID + "/default.jpg";
                                    // p.ImageUrl = "http:" + node.Descendants("img").FirstOrDefault().Attributes.Where(i => i.Name == "src").FirstOrDefault().Value.ToString().Trim();
                                    listprop.Add(p);
                                }
                            }
                        }
                    }
                   // _PerformanceProgressBar.IsIndeterminate = false;
                    if (listprop.Count() == 0)
                    {
                        tblkFavNoMovies.Visibility = Visibility.Visible;
                    }
                    else
                        tblkFavNoMovies.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
               // Exceptions.SaveOrSendExceptions("Exception in OnlineLinks_Loaded Method In OnlineLinks.cs file.", ex);
            }
        }

        #region "Common Methods"

        private void LoadAds()
        {
            try
            {
                LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In SongDetails file", ex);
                string excepmess = "Exception in LoadAds Method In SongDetails file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }

        }

        #endregion
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string[] parameters = (string[])e.Parameter;
            showid = parameters[0].ToString();
            tile = parameters[1].ToString();
            try
            {
                if (e.NavigationMode != NavigationMode.Back)
                    listprop.Clear();               
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In OnlineLinks.cs file.", ex);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {               
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom Method In OnlineLinks.cs file.", ex);
            }
        }

        private void lbxVideo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            string videoid = ((Prop)lbxVideo.SelectedItem).VideoID;            
            Frame.Navigate(typeof(Youtube),videoid);
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
        
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {            
            try
            {
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
                        links.Title = insertdata.VideoTitle;
                        links.ClientPreferenceUpdated = DateTime.Now;
                        links.ClientShowUpdated = DateTime.Now;
                        int result = Task.Run(async () => await Constants.connection.InsertAsync(links)).Result;
                    }
                    Constants.NavigatedUri = string.Empty;
                    Constants.AudiosLinks.Clear();
                    Constants.DownloadTimer.Start();
                    
                    if (Constants.Linkstype == "Movies")
                    {
                        //NavigationValues.navigationvalue = Constants.navigationvalue;
                        //NavigationValues.pivotindex = 2;                        
                        parameters[0]=showid;
                        parameters[1] = null;
                        //Frame.Navigate(typeof(Details), parameters);
                    }
                    else
                    {                       
                        if (ResourceHelper.AppName == "Kids_TV.WindowsPhone" || ResourceHelper.AppName == Apps.Kids_TV_Pro.ToString() || ResourceHelper.AppName == Apps.Fitness_Programs.ToString() || ResourceHelper.AppName == Apps.Recipe_Shows.ToString() || ResourceHelper.AppName == Apps.Animation_Planet.ToString() || ResourceHelper.AppName == Apps.Kids_TV.ToString() || ResourceHelper.AppName == Apps.Yoga_and_Health.ToString().Replace("and", "&"))
                        {//NavigationService.Navigate(new Uri("/Views/ShowDetails.xaml?navigationvalue=" + Constants.navigationvalue + "&pivotindex=" + 0, UriKind.Relative));
                            //NavigationValues.navigationvalue = Constants.navigationvalue;
                            //NavigationValues.pivotindex = 0;
                          //  Frame.Navigate(typeof(ShowDetails));
                        }
                        else if (ResourceHelper.AppName == Apps.Story_Time.ToString())
                        {//NavigationService.Navigate(new Uri("/Views/StoryDetails.xaml?navigationvalue=" + Constants.navigationvalue + "&pivotindex=" + 0, UriKind.Relative));
                            //NavigationValues.navigationvalue = Constants.navigationvalue;
                            //NavigationValues.pivotindex = 0;
                           // Frame.Navigate(typeof(StoryDetails));
                        }
                        else if (ResourceHelper.AppName == Apps.Vedic_Library.ToString())
                        {//NavigationService.Navigate(new Uri("/Views/VedicDetails.xaml?navigationvalue=" + Constants.navigationvalue + "&pivotindex=" + 0, UriKind.Relative));
                            //NavigationValues.navigationvalue = Constants.navigationvalue;
                            //NavigationValues.pivotindex = 0;
                           // Frame.Navigate(typeof(VedicDetails));
                        }
                        else if (ResourceHelper.AppName == Apps.World_Dance.ToString())
                        {   //NavigationService.Navigate(new Uri("/Views/DanceDetailPage.xaml?navigationvalue=" + Constants.navigationvalue + "&pivotindex=" + 1, UriKind.Relative));
                            //NavigationValues.navigationvalue = Constants.navigationvalue;
                            //NavigationValues.pivotindex = 1;
                            //Frame.Navigate(typeof(DanceDetailPage));
                        }
                        else if (ResourceHelper.AppName == Apps.Online_Education.ToString() || ResourceHelper.AppName == Apps.DrivingTest.ToString())
                        {   //NavigationService.Navigate(new Uri("/Views/SubjectDetail.xaml?navigationvalue=" + Constants.navigationvalue + "&pivotindex=" + 0, UriKind.Relative));
                            //NavigationValues.navigationvalue = Constants.navigationvalue;
                            //NavigationValues.pivotindex = 0;
                            //Frame.Navigate(typeof(SubjectDetail));
                        }
                        else if (ResourceHelper.AppName == Apps.Video_Games.ToString())
                        {   //NavigationService.Navigate(new Uri("/Views/VideoGamesDetail.xaml?navigationvalue=" + Constants.navigationvalue + "&pivotindex=" + 2, UriKind.Relative));
                            //NavigationValues.navigationvalue = Constants.navigationvalue;
                            //NavigationValues.pivotindex = 2;
                            //Frame.Navigate(typeof(VideoGamesDetail));
                        }
                        else if (ResourceHelper.AppName == Apps.Cricket_Videos.ToString())
                        {   //NavigationService.Navigate(new Uri("/Views/CricketDetail.xaml?navigationvalue=" + Constants.navigationvalue + "&pivotindex=" + 1, UriKind.Relative));
                            //NavigationValues.navigationvalue = Constants.navigationvalue;
                            //NavigationValues.pivotindex = 1;
                            //Frame.Navigate(typeof(CricketDetail));
                        }
                        else
                        {
                            parameters[0] = showid;
                            parameters[1] = null;
                            //Frame.Navigate(typeof(Details), parameters);
                        }
                    }
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
                    Constants.DownloadTimer.Start();
                    if (ResourceHelper.AppName == Apps.Bollywood_Movies.ToString() || ResourceHelper.AppName == "Indian_Cinema.WindowsPhone" || ResourceHelper.AppName == Apps.Indian_Cinema_Pro.ToString())
                    {
                        parameters[0] = showid;
                        parameters[1] = null;
                        //Frame.Navigate(typeof(Details), parameters);
                    }
                    else
                    {   
                        //Frame.Navigate(typeof(MusicDetail));
                    }
                }                
            }
            catch (Exception ex)
            {
                string gg = ex.Message;
            }
        }
    }
}
