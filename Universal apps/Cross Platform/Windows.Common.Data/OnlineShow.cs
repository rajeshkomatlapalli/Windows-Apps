using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using OnlineVideos.Entities;
using System.Threading.Tasks;
using Common.Library;
# if NOTANDROID
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.ApplicationModel;
#endif
using System.IO;
using Common;
using System.Threading;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI;

#if WINDOWS_APP
using Windows.Data.Xml.Dom;
using System.Net.Http;
using Windows.Graphics.Display;
#endif
#if WINDOWS_PHONE_APP && NOTANDROID
//using System.Windows.Media.Imaging;
//using System.Windows.Media;
//using System.Windows.Controls;
#endif
namespace OnlineVideos.Data
{
    public static class OnlineShow
    {

        public static int cnt;
        public static string genere;
        public static DateTime ReleaseDateYear;
        public static List<ChartProp> RetriveChartDataFromWebInfo(int ShowID)
        {
            List<ChartProp> ChartList = new List<ChartProp>();
            try
            {
                string DataType = string.Empty;
                try
                {
                    double pp = Convert.ToDouble(Constants.connection.Table<WebInformation>().Where(i => i.ShowID == ShowID).FirstOrDefaultAsync().Result.SelectedText.ToString());
                    DataType = "int";
                }
                catch (Exception ex)
                {
                    DataType = "string";
                }
                if (DataType == "int")
                {
                    ShowList sl = Constants.connection.Table<ShowList>().Where(i => i.ShowID == ShowID).FirstOrDefaultAsync().Result;
                    List<WebInformation> wdt = Constants.connection.Table<WebInformation>().Where(i => i.ShowID == ShowID).ToListAsync().Result;
                    foreach (WebInformation tb in wdt)
                    {
                        ChartList.Add(new ChartProp() { value = Math.Round(Convert.ToDouble(tb.SelectedText.ToString())), date = sl.CreatedDate.ToString("d"), type = DataType });
                    }
                }
                else
                {
                    ShowList sl = Constants.connection.Table<ShowList>().Where(i => i.ShowID == ShowID).FirstOrDefaultAsync().Result;
                    List<WebInformation> wdt = Constants.connection.Table<WebInformation>().Where(i => i.ShowID == ShowID).ToListAsync().Result;
                    foreach (WebInformation tb in wdt)
                    {
                        ChartList.Add(new ChartProp() { value = tb.SelectedText, date = sl.CreatedDate.ToString("d"), type = DataType, FavUri = sl.PivotImage, Title = sl.Title });
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in RetriveChartDataForWebInfo Method In OnlineShow.cs file", ex);
            }
            return ChartList;
        }

        public static List<ChartProp> RetriveChartDataForWebInfo(int ShowID)
        {
            List<ChartProp> ChartList = new List<ChartProp>();
            try
            {
                string DataType = string.Empty;
                try
                {
                    double pp = Convert.ToDouble(Constants.connection.Table<WebInformation>().Where(i => i.ShowID == ShowID).FirstOrDefaultAsync().Result.SelectedText.ToString());
                    DataType = "int";
                }
                catch (Exception ex)
                {
                    DataType = "string";
                }
                if (DataType == "int")
                {
                    ShowList sl = Constants.connection.Table<ShowList>().Where(i => i.ShowID == ShowID).FirstOrDefaultAsync().Result;
                    List<WebDailyTable> wdt = Constants.connection.Table<WebDailyTable>().Where(i => i.ShowID == ShowID).ToListAsync().Result;
                    foreach (WebDailyTable tb in wdt)
                    {
                        ChartList.Add(new ChartProp() { value = Math.Round(Convert.ToDouble(tb.SelectedText.ToString())), date = tb.Date.ToString("d"), type = DataType });
                    }
                }
                else
                {
                    ShowList sl = Constants.connection.Table<ShowList>().Where(i => i.ShowID == ShowID).FirstOrDefaultAsync().Result;
                    List<WebDailyTable> wdt = Constants.connection.Table<WebDailyTable>().Where(i => i.ShowID == ShowID).ToListAsync().Result;
                    foreach (WebDailyTable tb in wdt)
                    {
                        ChartList.Add(new ChartProp() { value = tb.SelectedText, date = tb.Date.ToString("d"), type = DataType, FavUri = sl.PivotImage, Title = sl.Title });
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in RetriveChartDataForWebInfo Method In OnlineShow.cs file", ex);
            }
            return ChartList;
        }

        public static string GetMovieTitle(string movieId)
        {
            string movietitle = string.Empty;
            try
            {
                ShowList selecteditem = new ShowList();
                int showid = Convert.ToInt32(movieId);
                selecteditem = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result;
                movietitle = selecteditem.Title;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetMovieTitle Method In OnlineShow.cs file", ex);
            }
            return movietitle;
        }
        public static List<HelpItem> GetUpgrade()
        {
            List<HelpItem> helpMenuItems = new List<HelpItem>();

            try
            {
                XDocument xdoc = XDocument.Load(Constants.UpgradeXmlPath);

                var findEle = from i in xdoc.Descendants("description") select i;

                foreach (var d in findEle.Descendants("c").Elements("des"))
                {
                    HelpItem upgrademes = new HelpItem();
                    upgrademes.HelpText = d.Value;
                    helpMenuItems.Add(upgrademes);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadUpgrade  Method In Vidoes.cs file", ex);
            }
            return helpMenuItems;

        }
        public static int GetMovieID(string Title)
        {
            int movieID = 0;
            try
            {
                ShowLinks selecteditem = new ShowLinks();

                selecteditem = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.Title == Title && i.LinkType == "Audio").FirstOrDefaultAsync()).Result;
                movieID = selecteditem.ShowID;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetMovieTitle Method In OnlineShow.cs file", ex);
            }
            return movieID;
        }

        public static void UpdateReportBrokenCount()
        {
            try
            {
                int showid = Convert.ToInt32(AppSettings.ShowID);
                string linktype = AppSettings.LinkType;
                Constants.selecteditemshowlinkslist = new List<ShowLinks>();
                Constants.selecteditemshowlinkslist = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == linktype).OrderBy(j => j.LinkOrder).ToListAsync()).Result;
                foreach (ShowLinks links in Constants.selecteditemshowlinkslist)
                {
                    if (links.Title == Constants.selecteditem.Title)
                    {
                        links.BrokenLinkCount = links.BrokenLinkCount + 1;
                        DataManager<ShowLinks> linksmanager = new DataManager<ShowLinks>();
                        linksmanager.SaveToList(links, "ShowID", "LinkUrl");
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in UpdateReportBrokenCount Method In OnlineShow.cs file", ex);
            }
        }
        public static List<ShowLinks> GetTopRatedLinks()
        {

            List<ShowLinks> objMovieList = new List<ShowLinks>();
            List<ShowLinks> ReturnList = new List<ShowLinks>();
            List<ShowList> objMovieList1 = new List<ShowList>();
            try
            {
                string type1 = LinkType.Songs.ToString();
                var query = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == type1 && i.IsHidden == false).OrderByDescending(j => j.Rating).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Take(15);

                foreach (var item in query)
                {
                    if (!ReturnList.Contains(item))
                    {
                        ReturnList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetTopRatedShows Method In OnlineShow.cs file", ex);
            }
            return ReturnList;
        }
        public static string GetsongTitle(string movieId, string songid)
        {
            string Songtitle = string.Empty;
            try
            {
                ShowLinks selecteditem = new ShowLinks();
                int showid = Convert.ToInt32(movieId);
                int linkorder = Convert.ToInt32(songid);
                selecteditem = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkOrder == linkorder).FirstOrDefaultAsync()).Result;
                Songtitle = selecteditem.Title;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetsongTitle Method In OnlineShow.cs file", ex);
            }
            return Songtitle;
        }
        public static object GetDownloadedImages(string id, LinkType linkType)
        {
            List<ShowLinks> objMovieLinks = null;
            try
            {
                int showid = int.Parse(id);
                string LinkType = linkType.ToString();
                var ShowLinksByType = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == LinkType && i.IsHidden == false).ToListAsync()).Result;
                objMovieLinks = new List<ShowLinks>();
                foreach (ShowLinks Link in ShowLinksByType)
                {
#if NOTANDROID
                    Link.DownloadedImage =ResourceHelper.getShowTileImage(Link.LinkUrl);
#endif
                    objMovieLinks.Add(Link);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("id", id);
                Exceptions.SaveOrSendExceptions("Exception in GetVideoLinksOfAMovie Method In OnlineShow.cs file", ex);
            }
            return objMovieLinks;
        }
        public static List<ShowLinks> GetLyrics(string id, string LinkTitle)
        {
            //List<ShowLinks> objMovieLinks = null;
            //try
            //{
            //    //OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
            //    int showid = Convert.ToInt32(id);
            //    string linktype = "Audio";
            //    ShowLinks ShowLinksByType = Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.Title == LinkTitle && i.LinkType == linktype).FirstOrDefaultAsync().Result;

            //    //IQueryable<ShowLinks> ShowLinksByType = from k in context.ShowLinks
            //    //                                        where k.ShowID == Convert.ToInt32(id) && k.Title == LinkTitle
            //    //                                        select k;
            //    objMovieLinks = new List<ShowLinks>();

            //        if (ShowLinksByType.Description != null && ShowLinksByType.Description != "")
            //            objMovieLinks.Add(ShowLinksByType);

            //}
            //catch (Exception ex)
            //{
            //    ex.Data.Add("id", id);
            //    Exceptions.SaveOrSendExceptions("Exception in GetVideoLinksOfAMovie Method In OnlineShow.cs file", ex);
            //}
            //return objMovieLinks;
            List<ShowLinks> objLinkList = new List<ShowLinks>();
            try
            {
                int showid = Convert.ToInt32(id);
                string type1 = LinkType.Audio.ToString();
                objLinkList = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.Title == LinkTitle && i.LinkType == type1).OrderBy(j => j.LinkOrder).ToListAsync()).Result;
                foreach (ShowLinks sl in objLinkList)
                {
                    List<string> lines = new List<string>();
                    List<string> lines1 = new List<string>();
                    string ly = sl.Description;
                    if (ly != "")
                    {
                        string s2 = string.Empty;
                        if (!ly.StartsWith("\n"))
                        {
                            //  ly = ly.Replace("\\n" , "\n");
                            int lenth = ly.Length;
                            int idex = 0;
                            while (lenth > 0)
                            {
                                List<string> li = new List<string>();
                                ly = ly.Insert(idex, Environment.NewLine);
                                li.Add(ly);
                                lenth = lenth - 35;
                                idex = idex + 37;
                            }
                        }
                        s2 = ly;//.Replace(" ", "\n");
                        string[] Lyric;
                        long count = 1;
                        int start = 0;
                        while ((start = s2.IndexOf('\n', start)) != -1)
                        {
                            count++;
                            start++;
                        }
                        for (int i = 0; i < count; i++)
                        {
                            Lyric = s2.Split('\n');
                            string l2 = string.Empty;
                            string l1 = Lyric[i].Trim();
                            int length = l1.Length;
                            int maxlength = 35;
                            int stringLength = l1.Length;
                            for (int j = 0; j < stringLength; j += maxlength)
                            {
                                if (j + maxlength > stringLength) maxlength = stringLength - j;
                                string truncatedLyrics = l1.Substring(j, maxlength);
                                //l2 = truncatedLyrics.Substring(0, truncatedLyrics.LastIndexOfAny(new char[] { ' ', '.', ',', '"', '?' }));
                                lines.Add(truncatedLyrics);
                            }
                        }
                        sl.Description = string.Join("\n", lines.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetLyrics Method In OnlineShow.cs file", ex);
            }
            return objLinkList;

        }
        //public static int GetMovieID(string Title)
        //{
        //    int movieID = 0;
        //    try
        //    {
        //        ShowLinks selecteditem = new ShowLinks();

        //        selecteditem = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.Title == Title && i.LinkType == "Audio").FirstOrDefaultAsync()).Result;
        //        movieID = selecteditem.ShowID;
        //    }
        //    catch (Exception ex)
        //    {
        //        Exceptions.SaveOrSendExceptions("Exception in GetMovieTitle Method In OnlineShow.cs file", ex);
        //    }
        //    return movieID;
        //}

        public static List<ShowLinks> Getlyricsofsong(string ShowID)
        {
            try
            {
                DataManager<ShowLinks> datamanager = new DataManager<ShowLinks>();
                Constants.selecteditemshowlinkslist = new List<ShowLinks>();
                int showid = int.Parse(ShowID);
                string linktype = LinkType.Audio.ToString();
                Constants.selecteditemshowlinkslist = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == linktype && i.Description != "").OrderBy(j => j.LinkOrder).ToListAsync()).Result;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Getlyricsofsong Method In OnlineShow.cs file", ex);
            }
            return Constants.selecteditemshowlinkslist;
        }

        public static ObservableCollection<ShowLinks> GetShowLinksByType(string ShowID, LinkType type)
        {
            try
            {
                int showid = Convert.ToInt32(ShowID);
                string type1 = type.ToString();
				List<ShowLinks>RemoveList=new List<ShowLinks>();
                Constants.selecteditemshowlinklist = new ObservableCollection<ShowLinks>(Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == type1 && i.IsHidden == false).OrderBy(j => j.LinkOrder).ToListAsync()).Result);
				foreach (ShowLinks sl in Constants.selecteditemshowlinklist)
                {
                    if (Constants.searchtext == sl.Title)
                    {                        
                        sl.Foreground = Color.FromArgb(100, 24, 255, 250);
                        Constants.searchtext = string.Empty;
                    }
                    else
                    {
                        sl.Foreground = Colors.White;
                    }
                    if (sl.DownloadStatus==null)
                    {
                        sl.DownloadStatus = "Download";
                    }
                    //else
                    //{
                    //    sl.DownloadStatus = "Download";
                    //}
                    if (sl.LinkUrl.StartsWith("http://"))
                    {
                        sl.ThumbnailImage = sl.LinkUrl.Replace(".wmv", "") + "_512.jpg";
                    }
                    else
                    {
                        sl.ThumbnailImage = "http://img.youtube.com/vi/" + sl.LinkUrl + "/default.jpg";
                    }
#if ANDROID && NotIOS
					try {
						if(type1!="Audio")
						{
							if(AppSettings.ProjectName=="Video Mix")
								sl.ThumbnailImage=sl.UrlType;
							HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sl.ThumbnailImage);
							request.Method ="Get";
							HttpWebResponse response1 = (HttpWebResponse)Task.Run (async() => await request.GetResponseAsync ()).Result;
							System.IO.Stream str = response1.GetResponseStream ();
					#if ANDROID && NOTIOS
							sl.VideoBitmapImage = Android.Graphics.BitmapFactory.DecodeStream (str);
					#endif
							if(response1.StatusCode!=HttpStatusCode.OK)
								RemoveList.Add(sl);
						}
					} 
					catch (Exception ex) 
					{
						var errorcode=ex.GetBaseException();
						if(errorcode.ToString().Contains("404"))
							RemoveList.Add(sl);
					}
#endif
                }
				#if ANDROID
				try {
					foreach(ShowLinks ite in RemoveList)
					{
						Constants.selecteditemshowlinklist.Remove(ite);
					}
				} catch (Exception ex) {
					
				}
				#endif
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetShowLinksByType Method In OnlineShow.cs file", ex);
            }
            return Constants.selecteditemshowlinklist;
        }
        public static List<ShowLinks> GetShowAudioByType(string ShowID)
        {
            try
            {
                int showid = Convert.ToInt32(ShowID);
                string type = LinkType.Audio.ToString();
                Constants.selecteditemshowAudiolinkslist = new List<ShowLinks>();
                Constants.selecteditemshowAudiolinkslist = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == type && i.IsHidden == false).OrderBy(j => j.LinkOrder).ToListAsync()).Result;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetShowLinksByType Method In OnlineShow.cs file", ex);
            }
            return Constants.selecteditemshowAudiolinkslist;
        }
        public static ObservableCollection<ShowLinks> GetShowLinksByMovies(string ShowID)
        {
            try
            {
                int showid = Convert.ToInt32(ShowID);
                string type = LinkType.Movies.ToString();
                Constants.selectedMovielinklist = new ObservableCollection<ShowLinks>(Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == type && i.IsHidden == false).OrderBy(j => j.LinkOrder).ToListAsync()).Result);
                foreach (ShowLinks sl in Constants.selectedMovielinklist)
                {
                    sl.ThumbnailImage = "http://img.youtube.com/vi/" + sl.LinkUrl + "/default.jpg";
#if ANDROID
					try {
						HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sl.ThumbnailImage);
						request.Method ="Get";
						HttpWebResponse response1 = (HttpWebResponse)Task.Run (async() => await request.GetResponseAsync ()).Result;
						System.IO.Stream str = response1.GetResponseStream ();
					#if ANDROID && NOTIOS
						sl.VideoBitmapImage = Android.Graphics.BitmapFactory.DecodeStream (str);

					#endif
					} catch (Exception ex) {
					}
#endif
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetShowLinksByType Method In OnlineShow.cs file", ex);
            }
            return Constants.selectedMovielinklist;
        }

        public static List<ShowLinks> GetShowVideoLinks(string ShowID, LinkType type)
        {
            try
            {
                Constants.selecteditemshowlinkslist = new List<ShowLinks>();
                int showid = int.Parse(ShowID);
                string linktype = type.ToString();
                Constants.selecteditemshowlinkslist = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == linktype).OrderBy(j => j.LinkOrder).ToListAsync()).Result;
                foreach (ShowLinks sl in Constants.selecteditemshowlinkslist)
                {
#if NOTANDROID
                    sl.Thumbnail = new BitmapImage(new Uri("http://img.youtube.com/vi/" + sl.LinkUrl + "/default.jpg", UriKind.Absolute));
#endif
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetShowLinksByType Method In OnlineShow.cs file", ex);
            }
            return Constants.selecteditemshowlinkslist;
        }
#if NOTANDROID
        public static ImageBrush LoadPanoramCastBackground(string pid)
        {
            ImageBrush panoramabackground = new ImageBrush();
            try
            {
                int personid = Convert.ToInt32(pid);
                CastProfile Cast = new CastProfile();
                Cast = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID == personid).FirstOrDefaultAsync()).Result;
                BitmapImage objImg = new BitmapImage(new Uri(Cast.FlickrPanoramaImageUrl));
                panoramabackground.ImageSource = objImg;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadPanoramCastBackground Method In OnlineShow.cs file", ex);
            }
            return panoramabackground;
        }
#endif
        public static ListGroup GetTopRatedShows()
        {

            List<ShowList> objMovieList = new List<ShowList>();
#if NOTANDROID
            ListGroup g = new ListGroup("top rated >", new Thickness(750, 13, 0, 0));
#endif
#if ANDROID
            ListGroup g = new ListGroup();
#endif
            try
            {
#if WINDOWS_APP
                if (AppSettings.ProjectName == "Indian Cinema" || AppSettings.ProjectName == "Indian Cinema Pro" || AppSettings.ProjectName=="Indian_Cinema")
                {
                    g.Menu = "hindi >";
                    string datt = DateTime.Today.Date.ToString();
                    objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == 20).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Where(k => Convert.ToDateTime(k.ReleaseDate).CompareTo(DateTime.Today) <= 0).Take(8).ToList();

                    foreach (ShowList item in objMovieList)
                    {
                        try
                        {
                            g.Items.Add(new ShowList(ResourceHelper.getShowTileImage1(item.TileImage), Convert.ToInt32(item.ShowID), item.Title, item.TileImage));

                        }
                        catch (Exception ex)
                        {
                        }

                    }
                }
                else if (AppSettings.ProjectName == "Bollywood Music")
                {
                    var query = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(l => l.IsHidden == false).OrderByDescending(j => j.Rating).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Where(x => x.ShowID == ((Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result != null) ? (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result).ShowID : 0)).Take(8).ToList();
                    foreach (var item in query)
                    {
                        g.Items.Add(new ShowList(ResourceHelper.getShowTileImage1(item.TileImage), Convert.ToInt32(item.ShowID), item.Title, item.TileImage));
                    }
                }
                else
                {
                    var query = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(l => l.IsHidden == false).OrderByDescending(j => j.Rating).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Take(8);
                    foreach (var item in query)
                    {
                        g.Items.Add(new ShowList(ResourceHelper.getShowTileImage1(item.TileImage), Convert.ToInt32(item.ShowID), item.Title, item.TileImage));
                    }
                }
#endif
#if WINDOWS_PHONE_APP
                if (AppSettings.ProjectName == "Indian Cinema" || AppSettings.ProjectName == "Indian Cinema Pro" || AppSettings.ProjectName=="Indian_Cinema")
                {
					#if NOTANDROID
                    g.Menu = "hindi >";
					#endif 
                    objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == 20).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Where(k => Convert.ToDateTime(k.ReleaseDate).CompareTo(DateTime.Today) < 0).Take(6).ToList();

                    foreach (ShowList item in objMovieList)
                    {
                        try
                        {
                             #if NOTANDROID
                            item.Image =ResourceHelper.getShowTileImage(item.TileImage);
                            #endif
                            g.Items.Add(item);
                        }
                        catch (Exception ex)
                        {
                            string mess = ex.Message;
                        }
                    }
                }
               else if (AppSettings.ProjectName == "Bollywood Music")
                {
                    var query = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(l => l.IsHidden == false).OrderByDescending(j => j.Rating).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Where(x => x.ShowID == ((Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result != null) ? (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result).ShowID : 0)).Take(6).ToList();
                    foreach (var item in query)
                    {
                         #if NOTANDROID
                        item.Image =ResourceHelper.getShowTileImage(item.TileImage);
                        #endif
                        g.Items.Add(item);
                    }
                }
                else
                {
                    var query = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(l => l.IsHidden == false).OrderByDescending(j => j.Rating).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Take(6);
                    foreach (var item in query)
                    {
                         #if NOTANDROID
                        AppSettings.IDForImagePath = item.ShowID.ToString();
                            item.Image =ResourceHelper.getShowTileImage(item.TileImage);
#endif
                        g.Items.Add(item);
                    }
                }
#endif
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetTopRatedShows Method In OnlineShow.cs file", ex);
            }
            return g;
        }

        public static ListGroup GetDownLoadList()
        {
#if NOTANDROID
            ListGroup g = new ListGroup("download list >", new Thickness(650, 0, 0, 0));
#endif
#if ANDROID
            ListGroup g = new ListGroup();
#endif
            try
            {
                var query = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(l => l.IsHidden == false).OrderByDescending(j => j.CreatedDate).ToListAsync()).Result;
                foreach (var item in query.Take(25))
                {
                    ShowList g1 = new ShowList();
#if NOTANDROID
                    g1.LandingImage = ResourceHelper.getShowTileImage1(item.TileImage);
#endif
                    g1.ShowID = Convert.ToInt32(item.ShowID);
                    g1.Title = item.Title;
                    g1.TileImage = item.TileImage;
                    g1.TitleForDownLoad = item.Title.Substring(item.Title.LastIndexOf("/") + 1);
                    g.Items.Add(g1);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetDownLoadList Method In OnlineShow.cs file", ex);
            }
            return g;
        }

        public static bool StatusCodeforImage(string ImageUrl)
        {
            bool g = default(bool);
#if WINDOWS_APP
            try
            {
                var httpClient = new HttpClient();
                var accessToken = "1234";
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, ImageUrl);
                httpRequestMessage.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Bearer {0}", accessToken));
                httpRequestMessage.Headers.Add("User-Agent", "My user-Agent");
                var response1 = Task.Run(async () => await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead)).Result;
                g = response1.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in StatusCodeforImage Method In OnlineShow.cs file", ex);
            }
#endif
            return g;
        }

        public static List<ShowLinks> GetTopRatedSongs(int cat)
        {
            List<ShowLinks> objMovieList = new List<ShowLinks>();
            List<ShowLinks> ReturnList = new List<ShowLinks>();
            List<ShowList> objMovieList1 = new List<ShowList>();
            try
            {
                DataManager<ShowLinks> datamanager = new DataManager<ShowLinks>();
                Constants.selecteditemshowlinkslist = new List<ShowLinks>();
                string type1 = LinkType.Songs.ToString();
                var query = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == type1 && i.IsHidden == false).OrderByDescending(j => j.Rating).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(k => k.CatageoryID == cat).OrderByDescending(k => k.ShowID).ToListAsync()).Result, o => o.ShowID, p => p.ShowID, (o, p) => o).Take(20);
                //				#if ANDROID
                //				var query=Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i=>i.LinkType==type1 && i.IsHidden==false).OrderByDescending(j=>j.Rating).ToListAsync()).Result.Take(10);
                //				#endif
                foreach (var item in query)
                {
#if NOTANDROID
                    if (NetworkHelper.IsNetworkAvailable())
                    {
                        bool h = StatusCodeforImage("http://img.youtube.com/vi/" + item.LinkUrl + "/default.jpg");
                        item.StatusCode = h;
                    }

                    if (!ReturnList.Contains(item))
                    {
                        ReturnList.Add(item);
                    }
#endif
#if  ANDROID
					item.ThumbnailImage = "http://img.youtube.com/vi/" + item.LinkUrl + "/default.jpg";
					try {
						HttpWebRequest request = (HttpWebRequest)WebRequest.Create(item.ThumbnailImage);
						request.Method ="Get";
						HttpWebResponse response1 = (HttpWebResponse)Task.Run (async() => await request.GetResponseAsync ()).Result;
						System.IO.Stream str = response1.GetResponseStream ();
					#if ANDROID && NOTIOS
						item.VideoBitmapImage = Android.Graphics.BitmapFactory.DecodeStream (str);
					#endif
					} catch (Exception ex) {

					}
					ReturnList.Add(item);
#endif
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetTopRatedShows Method In OnlineShow.cs file", ex);
            }
            return ReturnList;
        }
        public static List<ShowLinks> GetTopRatedAudio(int Catageoryid)
        {
            List<ShowLinks> objMovieList = new List<ShowLinks>();
            List<ShowLinks> ReturnList = new List<ShowLinks>();
            List<ShowList> objMovieList1 = new List<ShowList>();
            try
            {
                Constants.selecteditemshowlinkslist = new List<ShowLinks>();
                string type1 = LinkType.Audio.ToString();
                // var topAudio = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == type1 && i.IsHidden == false).OrderByDescending(j => j.Rating).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(k => k.CatageoryID == cat).OrderByDescending(k => k.ShowID).ToListAsync()).Result, o => o.ShowID, p => p.ShowID, (o, p) => o).Take(15);
                var topAudio = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == type1 && i.IsHidden == false).OrderByDescending(j => j.Rating).OrderByDescending(k => k.ShowID).Take(500).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == Catageoryid).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Take(10).ToList();
                //				#if ANDROID
                //				var topAudio=Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i=> i.LinkType==type1 && i.IsHidden==false).OrderByDescending(j=>j.Rating).ToListAsync()).Result.Take(10);
                //				#endif
                foreach (var item in topAudio)
                {
                    if (!ReturnList.Contains(item))
                    {
                        ReturnList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetTopRatedShows Method In OnlineShow.cs file", ex);
            }
            return ReturnList;
        }

        public static List<ShowList> GetTopRatedListViewShows()
        {
            List<ShowList> objMovieList = new List<ShowList>();
            List<ShowList> g = new List<ShowList>();
            try
            {
                if (AppSettings.ProjectName == "Bollywood Music")
                {
                    objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(k => k.ReleaseDate).OrderByDescending(j => j.Rating).ToListAsync()).Result.Where(x => x.ShowID == ((Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result != null) ? (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result).ShowID : 0)).ToList(); //datamanager.GetList(i => i.IsHidden == false, k => k.ReleaseDate, j => j.Rating, "TopRated");
                }
                else
                {
                    objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(k => k.ReleaseDate).OrderByDescending(j => j.Rating).ToListAsync()).Result; //datamanager.GetList(i => i.IsHidden == false, k => k.ReleaseDate, j => j.Rating, "TopRated");
                }
                foreach (var item in objMovieList)
                {
                    ShowList objMovie = new ShowList();
#if NOTANDROID
                    objMovie.LandingImage = ResourceHelper.getViewAllImageFromStorageOrInstalledFolder(item.TileImage);
#endif
                    objMovie.ShowID = Convert.ToInt32(item.ShowID);
                    objMovie.Title = item.Title;
                    objMovie.RatingBitmapImage = ImageHelper.LoadRatingImage(item.Rating.ToString()).ToString();
                    //objMovie.ReleaseDate = item.ReleaseDate;
                    string releasedate = item.ReleaseDate;
                    string Year = releasedate.Substring(releasedate.LastIndexOf(' ') + 1);
                    objMovie.ReleaseDate = Year;
                    if (ResourceHelper.AppName == Apps.World_Dance.ToString())
                    {
                        objMovie.SubTitle = "Origin: " + item.SubTitle;
                    }
                    else
                    {
                        objMovie.SubTitle = "Subtitle: " + item.SubTitle;
                    }
                    g.Add(objMovie);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetTopRatedShows Method In OnlineShow.cs file", ex);
            }
            return g;
        }
#if NOTANDROID
        public static ImageSource loadBitmapImageInBackground(string imagefile, UserControl thread, AutoResetEvent aa)
        {
            BitmapImage image = null;
#if WINDOWS_APP
            thread.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                image = new BitmapImage(new Uri(imagefile, UriKind.RelativeOrAbsolute));
                image.CreateOptions = BitmapCreateOptions.None;
                aa.Set();
            });
            aa.WaitOne();
#endif
            return image;
        }
#endif
        public static ListGroup GetRecentlyAddedShows()
        {
            List<ShowList> objMovieList = new List<ShowList>();
            List<CategoriesByShowID> objcategbyshowlist = new List<CategoriesByShowID>();
            List<ShowList> objMovieList1 = new List<ShowList>();

#if NOTANDROID
            ListGroup g = new ListGroup("recent >", new Thickness(750, 13, 0, 0));
#endif
#if ANDROID
            ListGroup g = new ListGroup();
#endif
            try
            {
#if WINDOWS_APP
                if (AppSettings.ProjectName == "Indian Cinema" || AppSettings.ProjectName == "Indian Cinema Pro" || AppSettings.ProjectName == "Indian_Cinema")
                {
                    g.Menu = "telugu >";
                    string datt = DateTime.Today.Date.ToString();
                    objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == 18).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Where(k => Convert.ToDateTime(k.ReleaseDate).CompareTo(DateTime.Today) <= 0).Take(8).ToList();
                    foreach (ShowList item in objMovieList)
                    {
                        try
                        {
                            g.Items.Add(new ShowList(ResourceHelper.getShowTileImage1(item.TileImage), Convert.ToInt32(item.ShowID), item.Title, item.TileImage));
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                else if (AppSettings.ProjectName == "Bollywood Music")
                {
                    var query = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(l => l.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.Rating).ToListAsync()).Result.Where(x => x.ShowID == ((Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result != null) ? (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result).ShowID : 0)).Take(8).ToList();
                    foreach (var item in query)
                    {
                        g.Items.Add(new ShowList(ResourceHelper.getShowTileImage1(item.TileImage), Convert.ToInt32(item.ShowID), item.Title, item.TileImage));
                    }
                }
                else
                {
                    var query = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(l => l.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.Rating).ToListAsync()).Result.Take(8);
                    foreach (var item in query)
                    {
                        g.Items.Add(new ShowList(ResourceHelper.getShowTileImage1(item.TileImage), Convert.ToInt32(item.ShowID), item.Title, item.TileImage));
                    }
                }
#endif
#if WINDOWS_PHONE_APP
                if (AppSettings.ProjectName == "Indian Cinema" || AppSettings.ProjectName == "Indian Cinema Pro" || AppSettings.ProjectName=="Indian_Cinema")
                {
#if NOTANDROID
                    g.Menu = "telugu >";
#endif
                    string datt = DateTime.Today.Date.ToString();
                    objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == 18).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Where(k => Convert.ToDateTime(k.ReleaseDate).CompareTo(DateTime.Today) < 0).Take(6).ToList();
                    foreach (ShowList item in objMovieList)
                    {
                        try
                        {
#if NOTANDROID
                            item.Image =ResourceHelper.getShowTileImage(item.TileImage);
#endif
                            g.Items.Add(item);

                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                else if (AppSettings.ProjectName == "Bollywood Music")
                {
                    var query = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(l => l.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.Rating).ToListAsync()).Result.Where(x => x.ShowID == ((Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result != null) ? (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result).ShowID : 0)).Take(6).ToList();
                    foreach (var item in query)
                    {
#if NOTANDROID
                        item.Image =ResourceHelper.getShowTileImage(item.TileImage);
#endif
                        g.Items.Add(item);
                    }
                }
                else
                {
                    var query = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(l => l.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.Rating).ToListAsync()).Result.Take(6);
                    foreach (var item in query)
                    {
#if NOTANDROID
                        item.Image =ResourceHelper.getShowTileImage(item.TileImage);
#endif
                        g.Items.Add(item);
                    }
                }
#endif
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetRecentlyAddedShows Method In OnlineShow.cs file", ex);
            }
            return g;
        }
        public static ListGroup GetTamilAddedShows()
        {

            List<ShowList> objMovieList = new List<ShowList>();
            List<CategoriesByShowID> objcategbyshowlist = new List<CategoriesByShowID>();
            List<ShowList> objMovieList1 = new List<ShowList>();
#if NOTANDROID
            ListGroup g = new ListGroup("tamil >", new Thickness(750, 13, 0, 0));
#endif
#if ANDROID
            ListGroup g = new ListGroup();
#endif
            try
            {
                if (AppSettings.ProjectName == "Indian Cinema" || AppSettings.ProjectName == "Indian Cinema Pro" || AppSettings.ProjectName=="Indian_Cinema")
                {
                    string datt = DateTime.Today.Date.ToString();
#if WINDOWS_APP
                    objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == 19).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Where(k => Convert.ToDateTime(k.ReleaseDate).CompareTo(DateTime.Today) <= 0).Take(8).ToList();
#endif
#if WINDOWS_PHONE_APP
#if NOTANDROID
                    g.Menu = "tamil >";
				#endif 
                    objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == 19).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Where(k => Convert.ToDateTime(k.ReleaseDate).CompareTo(DateTime.Today) < 0).Take(6).ToList();
#endif
#if ANDROID
				objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == 19).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Where(k => Convert.ToDateTime(k.ReleaseDate).CompareTo(DateTime.Today) < 0).ToList();
#endif
                    foreach (ShowList item in objMovieList)
                    {
                        try
                        {
#if WINDOWS_APP
                            g.Items.Add(new ShowList(ResourceHelper.getShowTileImage1(item.TileImage), Convert.ToInt32(item.ShowID), item.Title, item.TileImage));
#endif
#if WINDOWS_PHONE_APP
#if NOTANDROID
                            item.Image =ResourceHelper.getShowTileImage(item.TileImage);
#endif
                            g.Items.Add(item);
#endif
#if ANDROID
					string releasedate = item.ReleaseDate;
						string Year = releasedate.Substring(releasedate.LastIndexOf(' ') + 1);
						item.ReleaseDate = Year;
                            g.Items.Add(item);
#endif
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                else
                {
                    var query = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(l => l.IsHidden == false).OrderByDescending(j => j.CreatedDate).ToListAsync()).Result;
                   #if WINDOWS_APP
                    foreach (var item in query.Take(8))
                    {
#if NOTANDROID
                        g.Items.Add(new ShowList(ResourceHelper.getShowTileImage1(item.TileImage), Convert.ToInt32(item.ShowID), item.Title, item.TileImage));
#endif
                    }
#endif
#if WINDOWS_PHONE_APP
                    foreach (var item in query.Take(6))
                    {
#if NOTANDROID
                        item.Image =ResourceHelper.getShowTileImage(item.TileImage);
#endif
                        g.Items.Add(item);
                    }
#endif
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetRecentlyAddedShows Method In OnlineShow.cs file", ex);
            }
            return g;
        }

        public static ListGroup GetUpComingMovies()
        {
#if NOTANDROID
            ListGroup g = new ListGroup("upcoming movies >", new Thickness(550, 13, 0, 0));
#endif
#if ANDROID
            ListGroup g = new ListGroup();
#endif
            try
            {
                List<ShowList> objMovieList = new List<ShowList>();
                string datt = DateTime.Today.Date.ToString();
                   #if WINDOWS_APP
                objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Where(k => Convert.ToDateTime(k.ReleaseDate).CompareTo(DateTime.Today) > 0).Take(8).ToList();
#endif
#if WINDOWS_PHONE_APP
#if NOTANDROID
                g.Menu = "upcoming movies >";
		#endif 
                objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Where(k => Convert.ToDateTime(k.ReleaseDate).CompareTo(DateTime.Today) > 0).Take(6).ToList();
                foreach (var item in objMovieList)
                {
                    try
                    {
#if NOTANDROID
                        item.Image =ResourceHelper.getShowTileImage(item.TileImage);
#endif
                        g.Items.Add(item);
                    }
                    catch (Exception ex)
                    {
                    }
                }
#endif
#if !WINDOWS_PHONE_APP
                foreach (var item in objMovieList)
                {
                    try
                    {
#if NOTANDROID
                        item.LandingImage = ResourceHelper.getShowTileImage1(item.TileImage);
#endif
                        item.ShowID = Convert.ToInt32(item.ShowID);
                        item.Title = item.Title;
                        item.TileImage = item.TileImage;
                        g.Items.Add(item);
                    }
                    catch (Exception ex)
                    {
                    }
                }
#endif
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetUpComingMovies Method In OnlineShow.cs file", ex);
            }
            return g;
        }
        public static List<ShowCategories> LoadCategoryListForWebTile()
        {
            List<ShowCategories> ShowCategoryList = new List<ShowCategories>();
            try
            {
                List<ShowCategories> ShowCategorys = new List<ShowCategories>();
                ShowCategorys = Task.Run(async () => await Constants.connection.Table<ShowCategories>().Where(i => i.CategoryID != null).OrderBy(j => j.CategoryID).ToListAsync()).Result;
                ShowCategories catageory1 = new ShowCategories();
                foreach (ShowCategories item in ShowCategorys)
                {
                    if (item.CategoryID != 18 && item.CategoryID != 19)
                    {
                        ShowCategories catageory = new ShowCategories();
                        catageory.CategoryID = Convert.ToInt32(item.CategoryID);
                        catageory.CategoryName = item.CategoryName.ToString();
                        ShowCategoryList.Add(catageory);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadCategoryListForWebTile Method In OnlineShow.cs file", ex);
            }
            return ShowCategoryList;
        }

        public static List<ShowCategories> LoadCategoryListForCricketVideos()
        {
            List<ShowCategories> ShowCategoryList = new List<ShowCategories>();
            try
            {
                List<ShowCategories> ShowCategorys = new List<ShowCategories>();
                ShowCategorys = Task.Run(async () => await Constants.connection.Table<ShowCategories>().Where(i => i.CategoryID != null).OrderBy(j => j.CategoryID).ToListAsync()).Result;
                ShowCategories catageory1 = new ShowCategories();
                foreach (ShowCategories item in ShowCategorys)
                {
                        ShowCategories catageory = new ShowCategories();
                        catageory.CategoryID = Convert.ToInt32(item.CategoryID);
                        catageory.CategoryName = item.CategoryName.ToString();
                        ShowCategoryList.Add(catageory);                    
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadCategoryListForWebTile Method In OnlineShow.cs file", ex);
            }
            return ShowCategoryList;
        }
        public static List<CatageoryTable> LoadCategoryList()
        {
            List<CatageoryTable> ShowCategoryList = new List<CatageoryTable>();
            try
            {
                List<CatageoryTable> ShowCategorys = new List<CatageoryTable>();
                ShowCategorys = Task.Run(async () => await Constants.connection.Table<CatageoryTable>().Where(i => i.CategoryID != null).OrderBy(j => j.CategoryID).ToListAsync()).Result;
                CatageoryTable catageory1 = new CatageoryTable();
                foreach (CatageoryTable item in ShowCategorys)
                {
                    if (item.CategoryID != 18 && item.CategoryID != 19)
                    {
                        CatageoryTable catageory = new CatageoryTable();
                        catageory.CategoryID = Convert.ToInt32(item.CategoryID);
                        catageory.CategoryName = item.CategoryName.ToString();
                        ShowCategoryList.Add(catageory);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadCategoryList Method In OnlineShow.cs file", ex);
            }
            return ShowCategoryList;
        }
#if NOTANDROID
        public async static Task<string> Loadhelpstorage()
        {
            string timestamp = string.Empty;
            try
            {
                StorageFolder store = ApplicationData.Current.LocalFolder;
                StorageFile file = await store.CreateFileAsync(@"DefaultData\Help.xml", Windows.Storage.CreationCollisionOption.OpenIfExists);
                var f = await file.OpenAsync(FileAccessMode.Read);
                IInputStream inputStream = f.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = await dataReader.LoadAsync((uint)f.Size);
                timestamp = (dataReader.ReadString(numBytesLoaded)).ToString();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Loadhelpstorage Method In OnlineShow.cs file", ex);
            }
            return timestamp;
        }
#endif

#if NOTANDROID
        public async static Task<string> LoadHelpPath()
        {
            string timestamp = string.Empty;
            try
            {
                StorageFolder store = ApplicationData.Current.LocalFolder;
                StorageFile file = await store.CreateFileAsync(@"XmlData\HelpMenu.xml", Windows.Storage.CreationCollisionOption.OpenIfExists);
                var f = await file.OpenAsync(FileAccessMode.Read);
                IInputStream inputStream = f.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = await dataReader.LoadAsync((uint)f.Size);
                timestamp = (dataReader.ReadString(numBytesLoaded)).ToString();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadHelpPath Method In OnlineShow.cs file", ex);
            }
            return timestamp;
        }
#endif
        public static List<ShowList> GetRecentLiveTileImages()
        {
            List<ShowList> LiveTileImageList = new List<ShowList>();
            try
            {               
                var query = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(l => l.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Take(8);
                foreach (var item in query)
                {
                    LiveTileImageList.Add(item);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetRecentLiveTileImages Method In OnlineShow.cs file", ex);
            }
            return LiveTileImageList;
        }

        public static List<ShowList> GetVideoDetails(string id)
        {
            List<ShowList> objfilmography = new List<ShowList>();
            try
            {
                int showid = Convert.ToInt32(id);
                ShowList objdetail = new ShowList();
                objdetail = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstAsync()).Result;
#if NOTANDROID
                //if (AppSettings.ProjectName == "Video Mix")
                //{
                //    string FolderName = string.Empty;
                //    if (DisplayProperties.ResolutionScale == ResolutionScale.Scale100Percent)
                //    {
                //        FolderName = "scale-100";
                //    }
                //    else if (DisplayProperties.ResolutionScale == ResolutionScale.Scale140Percent)
                //    {
                //        FolderName = "scale-140";
                //    }
                //    else if (DisplayProperties.ResolutionScale == ResolutionScale.Scale180Percent)
                //    {
                //        FolderName = "scale-180";
                //    }
                //    StorageFolder store = ApplicationData.Current.LocalFolder;
                //    StorageFolder story = store;
                //    if (!string.IsNullOrEmpty(FolderName))
                //        story = Task.Run(async () => await store.CreateFolderAsync("Images\\" + FolderName, CreationCollisionOption.OpenIfExists)).Result;
                //    var ss = Task.Run(async () => await story.CreateFileAsync(objdetail.TileImage, CreationCollisionOption.OpenIfExists)).Result;
                //    IRandomAccessStream stream = Task.Run(async () => await ss.OpenAsync(FileAccessMode.ReadWrite)).Result;
                //    control.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                //    {
                //        BitmapImage b = new BitmapImage();
                //        b.SetSource(stream);
                //        objdetail.LandingImage1 = b;
                //        stream.Dispose();
                //    });                   
                //}
                //else
                //{
                    objdetail.LandingImage = ResourceHelper.getViewAllImageFromStorageOrInstalledFolder(objdetail.TileImage);
                //}
#endif
                objdetail.RatingBitmapImage = ImageHelper.LoadRatingImage(objdetail.Rating.ToString());
                objdetail.Title = objdetail.Title;
                if (AppSettings.ProjectName == "Video Mix")
                {
                    string sharedstatus = Task.Run(async () => await Constants.connection.Table<OnlineVideos.Entities.ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result.ShareStatus;
                    if (sharedstatus == "Shared To Blog")
                        objdetail.SubTitle = "Status: Online";
                    else
                        objdetail.SubTitle = "Status: Local";
                }
                else
                {
                    objdetail.SubTitle = objdetail.SubTitle;
                    if (ResourceHelper.AppName == Apps.World_Dance.ToString())
                    {
                        objdetail.SubTitle = "Origin: " + objdetail.SubTitle;
                    }
                    else
                    {
                        objdetail.SubTitle = "Subtitle: " + objdetail.SubTitle;
                    }
                }
                try
                {
                    if (AppSettings.ProjectName == "Video Mix")
                    {
                        objdetail.ReleaseDate = objdetail.ReleaseDate.Replace("12:00:00 ", "").Replace("AM", "");
                    }
                    else
                    {
                        string releasedate = objdetail.ReleaseDate;
                        string Year = releasedate.Substring(releasedate.LastIndexOf(' ') + 1);
                        objdetail.ReleaseDate = Year;
                    }
                }
                catch
                {
                }
                objdetail.Description = objdetail.Description;
                objfilmography.Add(objdetail);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetVideoDetails Method In OnlineShow.cs file", ex);
            }
            return objfilmography;
        }

        public static ShowList GetVideoDetail(string id)
        {
            ShowList objMovie = new ShowList();
            try
            {
                int showid = Convert.ToInt32(id);
                objMovie = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result;
#if NOTANDROID
                objMovie.Image =ResourceHelper.getShowTileImage(objMovie.TileImage) as ImageSource;
#endif
                objMovie.RatingBitmapImage = ImageHelper.LoadRatingImage(objMovie.Rating.ToString());
                if (ResourceHelper.AppName == Apps.World_Dance.ToString())
                {
                    objMovie.SubTitle = "Origin: " + objMovie.SubTitle;
                }
                else
                {
                    objMovie.SubTitle = "Subtitle: " + objMovie.SubTitle;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetVideoDetail Method In OnlineShow.cs file", ex);
            }
            return objMovie;
        }

        public static List<ContactUs> GetContactList()
        {
            List<ContactUs> contactList = new List<ContactUs>();
            try
            {
                XElement xmlDoc =
                XElement.Load("DefaultData/ContactUs.xml");
                var elements = xmlDoc.Elements().OrderBy(j => j.Element("ID").Value).ToList();
                foreach (XElement item in elements)
                {
                    ContactUs cu = new ContactUs();
                    cu.ID = int.Parse(item.Element("ID").Value);
                    cu.Name = item.Element("Name").Value;
                    cu.Image = item.Element("Image").Value;
                    cu.Link = item.Element("Link").Value;
                    contactList.Add(cu);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetContactList Method In OnlineShow.cs file", ex);
            }
            return contactList;
        }

        public static List<ShowList> GetTopratedListShows()
        {
            List<ShowList> objMovieList = new List<ShowList>();
            List<ShowList> objMovieList1 = new List<ShowList>();
            try
            {
                int count = 1;
                if (AppSettings.ProjectName == "Indian Cinema" || AppSettings.ProjectName == "Indian Cinema Pro" || AppSettings.ProjectName=="Indian_Cinema")
                {
#if NOTANDROID
                    objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == 20).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Where(k => Convert.ToDateTime(k.ReleaseDate).CompareTo(DateTime.Today) <= 0).ToList();
#endif
#if ANDROID
				objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == 20).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).ToList();
#endif
                    foreach (ShowList objMovie in objMovieList)
                    {
                        try
                        {
#if NOTANDROID
                            objMovie.LandingImage = ResourceHelper.getViewAllImageFromStorageOrInstalledFolder(objMovie.TileImage);
#endif
                            objMovie.RatingBitmapImage = ImageHelper.LoadRatingImage(objMovie.Rating.ToString()).ToString();
                            if (ResourceHelper.AppName == Apps.World_Dance.ToString())
                            {
                                objMovie.SubTitle = "Origin: " + objMovie.SubTitle;
                            }
                            else
                            {
                                objMovie.SubTitle = "Subtitle: " + objMovie.SubTitle;
                            }
                            objMovie.Rating = objMovie.Rating;
                            try
                            {
                                string releasedate = objMovie.ReleaseDate;
                                string Year = releasedate.Substring(releasedate.LastIndexOf(' ') + 1);
                                objMovie.ReleaseDate = Year;
                            }
                            catch
                            {
                            }
                            objMovieList1.Add(objMovie);
                        }
                        catch (Exception ex)
                        {
                            Exceptions.SaveOrSendExceptions("Exception in GetTopratedListShows Method In OnlineShow.cs file", ex);
                        }
                    }
                }
                else if (AppSettings.ProjectName == "Bollywood Music")
                {
                    objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.Rating).ToListAsync()).Result.Where(x => x.ShowID == ((Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result != null) ? (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result).ShowID : 0)).ToList();
                    foreach (var item in objMovieList)
                    {
                        ShowList objMovie = new ShowList();
                        objMovie.Genre = genere;
#if NOTANDROID
                        objMovie.LandingImage = ResourceHelper.getViewAllImageFromStorageOrInstalledFolder(item.TileImage);
#endif
                        objMovie.ShowID = Convert.ToInt32(item.ShowID);
                        objMovie.Title = item.Title;
                        objMovie.TileImage = item.TileImage;
					#if NOTANDROID
                        objMovie.RatingBitmapImage = ImageHelper.LoadRatingImage(item.Rating.ToString()).ToString();
					#endif
					#if ANDROID
					objMovie.Rating=item.Rating;
					#endif
                        // objMovie.ReleaseDate = item.ReleaseDate;
                        try
                        {
                            string releasedate = item.ReleaseDate;
                            string Year = releasedate.Substring(releasedate.LastIndexOf(' ') + 1);
                            objMovie.ReleaseDate = Year;
                        }
                        catch
                        {
                        }
                        objMovie.SubTitle = item.SubTitle;
                        objMovie.SubTitle = "Subtitle: " + objMovie.SubTitle;
                        objMovieList1.Add(objMovie);
                        count++;
                    }
                }
                else
                {
			objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.Rating).OrderByDescending(k => k.ShowID).ToListAsync()).Result;
                    foreach (var item in objMovieList)
                    {
                        ShowList objMovie = new ShowList();
                        objMovie.Genre = genere;
#if NOTANDROID
                        objMovie.LandingImage = ResourceHelper.getViewAllImageFromStorageOrInstalledFolder(item.TileImage);
#endif
                        objMovie.ShowID = Convert.ToInt32(item.ShowID);
                        objMovie.Title = item.Title;
                        objMovie.TileImage = item.TileImage;
                        objMovie.RatingBitmapImage = ImageHelper.LoadRatingImage(item.Rating.ToString()).ToString();
                        //objMovie.ReleaseDate = item.ReleaseDate;
                        try
                        {
                            //if (ResourceHelper.AppName != Apps.Kids_TV.ToString() && ResourceHelper.AppName != Apps.Kids_TV_Pro.ToString() && ResourceHelper.AppName != Apps.Animation_Planet.ToString())
                            //{
                            //    string releasedate = item.ReleaseDate;
                            //    string Year = releasedate.Substring(releasedate.LastIndexOf(' ') + 1);
                            //    objMovie.ReleaseDate = Year;
                            //}
                            //else
                            //{
                            objMovie.ReleaseDate = item.ReleaseDate;
                            //}
                        }
                        catch
                        {
                        }
#if ANDROID
					objMovie.Rating=item.Rating;
#endif
                        objMovie.SubTitle = item.SubTitle;
                        if (ResourceHelper.AppName == Apps.World_Dance.ToString())
                        {
                            objMovie.SubTitle = "Origin: " + objMovie.SubTitle;
                        }
                        else
                        {
                            objMovie.SubTitle = "Subtitle: " + objMovie.SubTitle;
                        }
                        objMovieList1.Add(objMovie);
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetTopratedListShows Method In OnlineShow.cs file", ex);
            }
            return objMovieList1;
        }

        public static List<ShowList> GetperentList()
        {
            List<ShowList> objMovieList = new List<ShowList>();
            List<ShowList> objMovieList1 = new List<ShowList>();
            try
            {

                objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().ToListAsync()).Result;
                int count = 1;
                foreach (var item in objMovieList)
                {
                    ShowList objMovie = new ShowList();
                    objMovie.Genre = genere;
                    objMovie.ShowID = Convert.ToInt32(item.ShowID);
                    objMovie.Title = item.Title;
                    objMovie.TileImage = item.TileImage;
                    objMovie.IsHidden = item.IsHidden;
                    objMovieList1.Add(objMovie);
                    count++;

                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetperentList Method In OnlineShow.cs file", ex);
            }
            return objMovieList1;
        }

        private static void LoadCategoryByShowId(int Showid)
        {
            List<ShowList> objMovieList = new List<ShowList>();
            try
            {
                {
                    DataManager<CategoriesByShowID> datamanager = new DataManager<CategoriesByShowID>();
                    List<CategoriesByShowID> objcastList = new List<CategoriesByShowID>();
                    objcastList = (datamanager.GetList(i => i.ShowID == Showid, j => Convert.ToInt32(j.ShowID), "D"));
                    foreach (CategoriesByShowID objcast in objcastList)
                    {
                        DataManager<CatageoryTable> datamanager1 = new DataManager<CatageoryTable>();
                        CatageoryTable objdetail = new CatageoryTable();
                        objdetail = datamanager1.GetFromList(i => i.CategoryID == objcast.CatageoryID);
                        genere = objdetail.CategoryName;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadCategoryByShowId Method In OnlineShow.cs file", ex);
            }
        }
        public static List<GameCheats> GetGameCheatCodes(long gameID, int cheatCount)
        {
            List<GameCheats> GameCheatCodes = null;
            try
            {
               // OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
                GameCheatCodes = new List<GameCheats>();
                var cheat = Task.Run(async()=>await Constants.connection.Table<GameCheats>().Where(i=>i.ShowID == gameID).ToListAsync()).Result.Take(cheatCount);
                foreach (var itm in cheat)
                {
                    GameCheats castInfo = new GameCheats();
                    castInfo.CheatID = itm.CheatID;
                    castInfo.CheatName = itm.CheatName;
                    GameCheatCodes.Add(castInfo);
                }
                int totalCheatCount = Task.Run(async()=>await Constants.connection.Table<ShowCast>().ToListAsync()).Result.Count();
                if (GameCheatCodes.Count == cheatCount && cheatCount != totalCheatCount)
                {
                    GameCheats castInfo = new GameCheats();
                    castInfo.CheatName = "more cheats";
                    GameCheatCodes.Add(castInfo);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Game ID", gameID);
                Exceptions.SaveOrSendExceptions("Exception in GetCheatsection Method In OnlineShow.cs file", ex);
            }
            return GameCheatCodes;
        }
#if WINDOWS_PHONE_APP
        public static List<VideoGameProperties> GetWeaponsList(long showID)
        {
            List<VideoGameProperties> WeaponsList = new List<VideoGameProperties>();
            try
            {
                Constants.UIThread = false;
                int id = Convert.ToInt32(showID);
                //OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
                var WeaponList =Task.Run(async()=>await Constants.connection.Table<GameWeapons>().Where(i=>i.ShowID == id).ToListAsync()).Result;

                foreach (var Weapon in WeaponList)
                {
                    VideoGameProperties WeaponInfo = new VideoGameProperties();
                    WeaponInfo.Id = Weapon.WeaponID;
                    WeaponInfo.Name = Weapon.Name;
                    WeaponInfo.Image = Weapon.Image;
                    WeaponsList.Add(WeaponInfo);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Show ID", showID);
                Exceptions.SaveOrSendExceptions("Exception in GetMovieCast Method In OnlineShow.cs file", ex);
            }
            return WeaponsList;
        }

        public static List<VideoGameProperties> GetVehicleList(long showID)
        {
            List<VideoGameProperties> VehiclesList = new List<VideoGameProperties>();
            try
            {
                Constants.UIThread = false;
                int id=Convert.ToInt32(showID);
                //OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
                var VehList =Task.Run(async()=>await Constants.connection.Table<GameVehicles>().Where(i=>i.ShowID == id).ToListAsync()).Result;

                foreach (var Weapon in VehList)
                {
                    VideoGameProperties VehicleInfo = new VideoGameProperties();
                    VehicleInfo.Id = Weapon.VehicleID;
                    VehicleInfo.Name = Weapon.Name;
                    VehicleInfo.Image = Weapon.Image;
                    VehiclesList.Add(VehicleInfo);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Show ID", showID);
                Exceptions.SaveOrSendExceptions("Exception in GetMovieCast Method In OnlineShow.cs file", ex);
            }
            return VehiclesList;
        }

        public static object GetachievementList(int showID)
        {
            List<VideoGameProperties> achevementsList = new List<VideoGameProperties>();
            try
            {
                Constants.UIThread = false;
              
               // OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
                var achList =Task.Run(async()=>await Constants.connection.Table<GameAchievement>().Where(i=>i.ShowID == showID).ToListAsync()).Result;

                foreach (var list in achList)
                {
                    VideoGameProperties achInfo = new VideoGameProperties();
                    achInfo.Id = list.AchievementId;
                    achInfo.Name = list.AchievementName;
                    achevementsList.Add(achInfo);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Show ID", showID);
                Exceptions.SaveOrSendExceptions("Exception in GetMovieCast Method In OnlineShow.cs file", ex);
            }
            return achevementsList;
        }

        public static object GetMisionList(int showID)
        {
            List<VideoGameProperties> missionList = new List<VideoGameProperties>();
            try
            {
                Constants.UIThread = false;
                //OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
                var misList = Task.Run(async () => await Constants.connection.Table<GameMissions>().Where(i => i.ShowID == showID).ToListAsync()).Result;

                foreach (var list in misList)
                {
                    VideoGameProperties missionInfo = new VideoGameProperties();
                    missionInfo.Id = list.MissionId;
                    missionInfo.Name = list.MissionName;
                    missionList.Add(missionInfo);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Show ID", showID);
                Exceptions.SaveOrSendExceptions("Exception in GetMovieCast Method In OnlineShow.cs file", ex);
            }
            return missionList;
        }

        public static object GetControlsList(int showID)
        {
            List<VideoGameProperties> controlsList = new List<VideoGameProperties>();
            try
            {
                Constants.UIThread = false;
               // OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
                var controlList = Task.Run(async () => await Constants.connection.Table<GameControls>().Where(i => i.ShowID == showID).ToListAsync()).Result;

                foreach (var list in controlList)
                {
                    VideoGameProperties controlInfo = new VideoGameProperties();
                    controlInfo.Id = list.ControlId;
                    controlInfo.Name = list.ControlName;
                    // missionInfo.Description =(list.Controldescription.ToString());
                    controlsList.Add(controlInfo);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Show ID", showID);
                Exceptions.SaveOrSendExceptions("Exception in GetMovieCast Method In OnlineShow.cs file", ex);
            }
            return controlsList;
        }
#endif
        public static List<ShowList> GetSortByShows(int SortID, int catid)
        {
            List<ShowList> objfilmography = new List<ShowList>();
            List<ShowList> SortList = new List<ShowList>();

            try
            {
                {
                    List<ShowList> objcastList = new List<ShowList>();

                    if (catid != 0)
                    {

                        if (SortID == 1)
                        {
                            SortList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == catid).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Where(k => Convert.ToDateTime(k.ReleaseDate).CompareTo(DateTime.Today) < 0).ToList();

                        }
                        else if (SortID == 2)
                        {
                            SortList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.Rating).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == catid).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Where(k => Convert.ToDateTime(k.ReleaseDate).CompareTo(DateTime.Today) < 0).ToList();

                        }
                        else if (SortID == 3)
                        {
                            SortList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == catid).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Where(k => Convert.ToDateTime(k.ReleaseDate).CompareTo(DateTime.Today) < 0).OrderByDescending(j => DateTime.Parse(j.ReleaseDate)).ToList();

                        }
                        else
                        {
                            SortList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderBy(j => j.Title).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == catid).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Where(k => Convert.ToDateTime(k.ReleaseDate).CompareTo(DateTime.Today) < 0).ToList();

                        }
                    }
                    else if (AppSettings.ProjectName == "Bollywood Music")
                    {
                        if (SortID == 1)
                        {
                            SortList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Where(x => x.ShowID == ((Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result != null) ? (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result).ShowID : 0)).ToList();

                        }
                        else if (SortID == 2)
                        {
                            SortList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.Rating).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Where(x => x.ShowID == ((Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result != null) ? (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result).ShowID : 0)).ToList();

                        }
                        else if (SortID == 3)
                        {
                            SortList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Where(x => x.ShowID == ((Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result != null) ? (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result).ShowID : 0)).OrderByDescending(j => DateTime.Parse(j.ReleaseDate)).ToList();

                        }
                        else
                        {
                            SortList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderBy(j => j.Title).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Where(x => x.ShowID == ((Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result != null) ? (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result).ShowID : 0)).ToList();

                        }
                    }
                    else
                    {
                        if (SortID == 1)
                        {
                            SortList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.ToList();

                        }
                        else if (SortID == 2)
                        {
                            SortList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.Rating).OrderByDescending(k => k.ShowID).ToListAsync()).Result.ToList();

                        }
                        else if (SortID == 3)
                        {
                            SortList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.ReleaseDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.ToList();

                        }
                        else
                        {
                            SortList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderBy(j => j.Title).OrderByDescending(k => k.ShowID).ToListAsync()).Result.ToList();

                        }
                    }
                    foreach (ShowList objcateg in SortList)
                    {
                        try
                        {
#if NOTANDROID
                            objcateg.LandingImage = ResourceHelper.getViewAllImageFromStorageOrInstalledFolder(objcateg.TileImage);
#endif
                            objcateg.RatingBitmapImage = ImageHelper.LoadRatingImage(objcateg.Rating.ToString());
                            objcateg.ShowID = Convert.ToInt32(objcateg.ShowID);
                            objcateg.Title = objcateg.Title;
                            if (AppSettings.ProjectName == "Bollywood Music" || AppSettings.ProjectName == "Indian Cinema.WindowsPhone" || AppSettings.ProjectName == "Indian_Cinema.WindowsPhone" || AppSettings.ProjectName == "Indian Cinema Pro" || AppSettings.ProjectName == "Indian Cinema" || AppSettings.ProjectName == "Indian_Cinema")
                                objcateg.ReleaseDate = objcateg.ReleaseDate.ToString();
                            try
                            {
                                string Year = (objcateg.ReleaseDate).Substring((objcateg.ReleaseDate).LastIndexOf(' ') + 1);
                                objcateg.ReleaseDate = Year;
                            }
                            catch
                            {
                            }
                            if (ResourceHelper.AppName == Apps.World_Dance.ToString())
                            {
                                objcateg.SubTitle = "Origin: " + objcateg.SubTitle;
                            }
                            else
                            {
                                objcateg.SubTitle = "Subtitle: " + objcateg.SubTitle;
                            }
                            objfilmography.Add(objcateg);

                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetCategoryIdByShows Method In OnlineShow.cs file", ex);
            }
            return objfilmography;
        }
        public static List<ShowList> GetCategoryIdByShows(string cid, int catid)
        {
            List<ShowList> objfilmography = new List<ShowList>();
            List<ShowCast> objpersonshows = new List<ShowCast>();
            List<ShowList> objMovieList = new List<ShowList>();

            try
            {
                {
                    List<ShowList> objcastList = new List<ShowList>();
                    int cid1 = Convert.ToInt32(cid);
                    if (catid != 0)
                    {
                        objcastList = Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(i => i.CatageoryID == cid1).OrderByDescending(j => j.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == catid).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Where(k => Convert.ToDateTime(k.ReleaseDate).CompareTo(DateTime.Today) < 0).ToList(), p => p.ShowID, o => o.ShowID, (p, o) => o).ToList();
                    }
                    else if (AppSettings.ProjectName == "Bollywood Music")
                    {
                        objcastList = Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(i => i.CatageoryID == cid1).OrderByDescending(j => j.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.ToList(), p => p.ShowID, o => o.ShowID, (p, o) => o).Where(x => x.ShowID == ((Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result != null) ? (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result).ShowID : 0)).ToList();
                    }
                    else
                        objcastList = Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(i => i.CatageoryID == cid1).OrderByDescending(j => j.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.ToList(), p => p.ShowID, o => o.ShowID, (p, o) => o).ToList();

                    foreach (ShowList objcateg in objcastList)
                    {
                        try
                        {
#if NOTANDROID
                            objcateg.LandingImage = ResourceHelper.getViewAllImageFromStorageOrInstalledFolder(objcateg.TileImage);
#endif
                            objcateg.RatingBitmapImage = ImageHelper.LoadRatingImage(objcateg.Rating.ToString());
                            objcateg.ShowID = Convert.ToInt32(objcateg.ShowID);
                            objcateg.Title = objcateg.Title;

                            try
                            {
                                string Year = (objcateg.ReleaseDate).Substring((objcateg.ReleaseDate).LastIndexOf(' ') + 1);
                                objcateg.ReleaseDate = Year;
                            }
                            catch
                            {
                            }
                            objcateg.SubTitle = "Subtitle: " + objcateg.SubTitle;
                            objfilmography.Add(objcateg);

                        }
                        catch (Exception ex)
                        {
                            Exceptions.SaveOrSendExceptions("Exception in GetCategoryIdByShows Method In OnlineShow.cs file", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetCategoryIdByShows Method In OnlineShow.cs file", ex);
            }
            return objfilmography;
        }

        public static List<ShowList> GetRecentListShows()
        {
            List<ShowList> objMovieList = new List<ShowList>();
            List<ShowList> objMovieList1 = new List<ShowList>();

            try
            {
                int count = 1;
                if (AppSettings.ProjectName == "Indian_Cinema" || AppSettings.ProjectName == "Indian Cinema Pro" || AppSettings.ProjectName == "Indian Cinema")
                {

                    objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == 18).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Where(k => Convert.ToDateTime(k.ReleaseDate).CompareTo(DateTime.Today) < 0).ToList();
                    foreach (ShowList item in objMovieList)
                    {

                        try
                        {
#if NOTANDROID
                            item.LandingImage = ResourceHelper.getViewAllImageFromStorageOrInstalledFolder(item.TileImage);
#endif
                            item.RatingBitmapImage = ImageHelper.LoadRatingImage(item.Rating.ToString()).ToString();
                            if (ResourceHelper.AppName == Apps.World_Dance.ToString())
                            {
                                item.SubTitle = "Origin: " + item.SubTitle;
                            }
                            else
                            {
                                item.SubTitle = "Subtitle: " + item.SubTitle;
                            }
                            try
                            {
                                string releasedate = item.ReleaseDate;
                                string Year = releasedate.Substring(releasedate.LastIndexOf(' ') + 1);
                                item.ReleaseDate = Year;
                            }
                            catch
                            {
                            }
                            objMovieList1.Add(item);
                        }

                        catch (Exception ex)
                        {
                            Exceptions.SaveOrSendExceptions("Exception in GetRecentListShows Method In OnlineShow.cs file", ex);
                        }
                    }

                }
                else if (AppSettings.ProjectName == "Bollywood Music")
                {
                    objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.Rating).ToListAsync()).Result.Where(x => x.ShowID == ((Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result != null) ? (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result).ShowID : 0)).ToList();
                    foreach (var item in objMovieList)
                    {
                        ShowList objMovie = new ShowList();
                        objMovie.Genre = genere;
#if NOTANDROID
                        objMovie.LandingImage = ResourceHelper.getViewAllImageFromStorageOrInstalledFolder(item.TileImage);
#endif
                        objMovie.ShowID = Convert.ToInt32(item.ShowID);
                        objMovie.Title = item.Title;
                        objMovie.TileImage = item.TileImage;
					#if NOTANDROID
					objMovie.RatingBitmapImage = ImageHelper.LoadRatingImage(item.Rating.ToString()).ToString();
					#endif
					#if ANDROID
					objMovie.Rating=item.Rating;
					#endif
                        try
                        {
                            string releasedate = item.ReleaseDate;
                            string Year = releasedate.Substring(releasedate.LastIndexOf(' ') + 1);
                            objMovie.ReleaseDate = Year;
                        }
                        catch
                        {
                        }
                        objMovie.SubTitle = item.SubTitle;
                        objMovie.SubTitle = "Subtitle: " + objMovie.SubTitle;
                        objMovieList1.Add(objMovie);
                        count++;
                    }
                }
                else
                {
                    objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.Rating).ToListAsync()).Result;
                    foreach (var item in objMovieList)
                    {
                        ShowList objMovie = new ShowList();
                        objMovie.Genre = genere;
#if NOTANDROID
                        objMovie.LandingImage = ResourceHelper.getViewAllImageFromStorageOrInstalledFolder(item.TileImage);
#endif
                        objMovie.ShowID = Convert.ToInt32(item.ShowID);
                        objMovie.Title = item.Title;
                        objMovie.TileImage = item.TileImage;
                        objMovie.RatingBitmapImage = ImageHelper.LoadRatingImage(item.Rating.ToString()).ToString();
                        //objMovie.ReleaseDate = item.ReleaseDate;
                        try
                        {
                            //string releasedate = item.ReleaseDate;
                            //string Year = releasedate.Substring(releasedate.LastIndexOf(' ') + 1);
                            //objMovie.ReleaseDate = Year;
                            objMovie.ReleaseDate = item.ReleaseDate;
                        }
                        catch
                        {
                        }
#if ANDROID
					objMovie.Rating=item.Rating;
#endif
                        objMovie.SubTitle = item.SubTitle;
                        objMovie.SubTitle = "Subtitle: " + objMovie.SubTitle;
                        objMovieList1.Add(objMovie);
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetRecentListShows Method In OnlineShow.cs file", ex);
            }
            return objMovieList1;
        }




        public static List<ShowList> GetTamilListShows()
        {
            List<ShowList> objMovieList = new List<ShowList>();
            List<ShowList> objMovieList1 = new List<ShowList>();

            try
            {

                objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == 19).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Where(k => Convert.ToDateTime(k.ReleaseDate).CompareTo(DateTime.Today) <= 0).ToList();
                int count = 1;
                foreach (ShowList item in objMovieList)
                {
                    try
                    {
#if NOTANDROID
                        item.LandingImage = ResourceHelper.getViewAllImageFromStorageOrInstalledFolder(item.TileImage);
#endif
                        item.RatingBitmapImage = ImageHelper.LoadRatingImage(item.Rating.ToString()).ToString();
                        item.SubTitle = "Subtitle: " + item.SubTitle;
                        try
                        {
                            string releasedate = item.ReleaseDate;
                            string Year = releasedate.Substring(releasedate.LastIndexOf(' ') + 1);
                            item.ReleaseDate = Year;
                        }
                        catch
                        {
                        }
                        objMovieList1.Add(item);
                    }

                    catch (Exception ex)
                    {
                    }
                }

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetRecentListShows Method In OnlineShow.cs file", ex);
            }
            return objMovieList1;
        }



        public static List<ShowList> GetUpcomingMoviesListShows()
        {
            List<ShowList> objMovieList = new List<ShowList>();
            List<ShowList> objMovieList1 = new List<ShowList>();

            try
            {

                objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.Rating).ToListAsync()).Result.Where(k => Convert.ToDateTime(k.ReleaseDate).CompareTo(DateTime.Today) > 0).ToList();
                int count = 1;
                foreach (ShowList item in objMovieList)
                {
                    try
                    {
#if NOTANDROID
                        item.LandingImage = ResourceHelper.getViewAllImageFromStorageOrInstalledFolder(item.TileImage);
#endif
                        item.RatingBitmapImage = ImageHelper.LoadRatingImage(item.Rating.ToString()).ToString();
                        item.SubTitle = "Subtitle: " + item.SubTitle;
                        try
                        {
                            string releasedate = item.ReleaseDate;
                            string Year = releasedate.Substring(releasedate.LastIndexOf(' ') + 1);
                            item.ReleaseDate = Year;
                        }
                        catch
                        {
                        }
                        objMovieList1.Add(item);

                    }
                    catch (Exception ex)
                    {
                        Exceptions.SaveOrSendExceptions("Exception in GetUpcomingMoviesListShows Method In OnlineShow.cs file", ex);
                    }


                }

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetUpcomingMoviesListShows Method In OnlineShow.cs file", ex);
            }
            return objMovieList1;
        }




        public static List<GameWeapons> GetGameWeapons(string id)
        {
            List<GameWeapons> objMovieList = new List<GameWeapons>();
            List<GameWeapons> objMovieList1 = new List<GameWeapons>();
            try
            {
                int showid = int.Parse(id);
                objMovieList = Task.Run(async () => await Constants.connection.Table<GameWeapons>().Where(i => i.ShowID == showid).OrderBy(j => j.ShowID).ToListAsync()).Result;
                foreach (var item in objMovieList)
                {
                    GameWeapons objMovie = new GameWeapons();

                    objMovie.ShowID = Convert.ToInt32(item.ShowID);
                    objMovie.WeaponID = item.WeaponID;
                    objMovie.Name = item.Name;
                    if (item.Description != null)
                    {
                        objMovie.Descriptiontitle = "Description : ";
                        objMovie.Description = item.Description;
                    }

                    objMovie.Image = item.Image;
                    objMovieList1.Add(objMovie);

                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetGameWeapons Method In OnlineShow.cs file", ex);
            }
            return objMovieList1;
        }

        public static List<GameVehicles> GetGameVehicle(string id)
        {
            List<GameVehicles> objMovieList = new List<GameVehicles>();
            List<GameVehicles> objMovieList1 = new List<GameVehicles>();
            try
            {

                int showid = int.Parse(id);
                objMovieList = Task.Run(async () => await Constants.connection.Table<GameVehicles>().Where(i => i.ShowID == showid).OrderBy(j => j.ShowID).ToListAsync()).Result;

                foreach (var item in objMovieList)
                {
                    GameVehicles objMovie = new GameVehicles();

                    objMovie.ShowID = Convert.ToInt32(item.ShowID);
                    objMovie.VehicleID = item.VehicleID;
                    objMovie.Name = item.Name;
                    if (item.Description != null)
                    {
                        objMovie.Descriptiontitle = "Description : ";
                        objMovie.Description = item.Description;
                    }
                    objMovie.Image = item.Image;
                    objMovieList1.Add(objMovie);

                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetGameVehicle Method In OnlineShow.cs file", ex);
            }
            return objMovieList1;
        }
        public static List<GameMissions> GetGameMission()
        {
            List<GameMissions> objMovieList = new List<GameMissions>();
            List<GameMissions> objMovieList1 = new List<GameMissions>();
            try
            {
                int showid = int.Parse(AppSettings.ShowID);
                objMovieList = Task.Run(async () => await Constants.connection.Table<GameMissions>().Where(i => i.ShowID == showid).OrderBy(j => j.ShowID).ToListAsync()).Result;
                foreach (var item in objMovieList)
                {
                    GameMissions objMovie = new GameMissions();
                    objMovie.ShowID = Convert.ToInt32(item.ShowID);
                    objMovie.MissionId = item.MissionId;
                    objMovie.MissionName = item.MissionName;
                    objMovie.Missiondescription = item.Missiondescription;
                    if (item.Walkthrough != null)
                    {
                        objMovie.Walkthroughtitle = "WalkThrough  ";
                        objMovie.Walkthrough = item.Walkthrough;
                    }
                    objMovieList1.Add(objMovie);

                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetGameMission Method In OnlineShow.cs file", ex);
            }
            return objMovieList1;
        }

        public static List<GameAchievement> GetGameAchievement()
        {
            List<GameAchievement> objMovieList = new List<GameAchievement>();
            List<GameAchievement> objMovieList1 = new List<GameAchievement>();
            try
            {
                int showid = int.Parse(AppSettings.ShowID);
                objMovieList = Task.Run(async () => await Constants.connection.Table<GameAchievement>().Where(i => i.ShowID == showid).OrderBy(j => j.ShowID).ToListAsync()).Result;
                foreach (var item in objMovieList)
                {
                    GameAchievement objMovie = new GameAchievement();
                    objMovie.ShowID = Convert.ToInt32(item.ShowID);
                    objMovie.AchievementId = item.AchievementId;
                    objMovie.AchievementName = item.AchievementName;
                    objMovie.AchievementDescription = item.AchievementDescription;
                    if (item.Points != null)
                    {
                        objMovie.Pointstitle = "Points ";
                        objMovie.Points = item.Points;
                    }

                    objMovieList1.Add(objMovie);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetGameAchievement Method In OnlineShow.cs file", ex);
            }
            return objMovieList1;
        }

        public static List<GameCheats> GetGameCheats()
        {
            List<GameCheats> objMovieList = new List<GameCheats>();
            List<GameCheats> objMovieList1 = new List<GameCheats>();
            try
            {
                int showid = int.Parse(AppSettings.ShowID);
                objMovieList = Task.Run(async () => await Constants.connection.Table<GameCheats>().Where(i => i.ShowID == showid).OrderBy(j => j.ShowID).ToListAsync()).Result;
                foreach (var item in objMovieList)
                {
                    GameCheats objMovie = new GameCheats();
                    objMovie.ShowID = Convert.ToInt32(item.ShowID);
                    objMovie.CheatID = item.CheatID;
                    objMovie.CheatName = item.CheatName;
                    objMovieList1.Add(objMovie);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetGameCheats Method In OnlineShow.cs file", ex);
            }
            return objMovieList1;
        }


        public static List<CastProfile> GetGameCast(string id)
        {
            List<CastProfile> objCastList = new List<CastProfile>();
            try
            {
                int showid = int.Parse(id);
                DataManager<CastProfile> profilemanager = new DataManager<CastProfile>();
                List<ShowCast> objCastDetails = new List<ShowCast>();
                objCastDetails = Task.Run(async () => await Constants.connection.Table<ShowCast>().Where(i => i.ShowID == showid).OrderBy(j => j.ShowID).ToListAsync()).Result;
                foreach (ShowCast cast in objCastDetails)
                {
                    CastProfile role = new CastProfile();
                    CastProfile role1 = new CastProfile();
                    role.PersonID = cast.PersonID;
                    role1 = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID == cast.PersonID).FirstOrDefaultAsync()).Result;
                    role.Description = role1.Description;
                    role.Name = role1.Name;
                    if (role.Description != null)
                    {
                        role.Descriptiontitle = "Description  ";

                    }
                    role.Image = role1.FlickrPersonImageUrl;
                    //ResourceHelper.getPersonTileImage(cast.PersonID.ToString());
                    objCastList.Add(role);
                }

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetGameCast Method In OnlineShow.cs file", ex);
            }
            return objCastList;
        }

        public static List<ShowLinks> GetLyricsForFavoriteAndHistory(string Link, string LinkTitle, LinkType type)
        {
            List<ShowLinks> objLinkList = new List<ShowLinks>();
            try
            {
                string linktype = type.ToString();
                objLinkList = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkUrl == Link && i.Title == LinkTitle && i.LinkType == linktype).ToListAsync()).Result;
                foreach (ShowLinks sl in objLinkList)
                {
                    List<string> lines = new List<string>();
                    string ly = sl.Description;
                    if (ly != "")
                    {
                        string s2 = string.Empty;
                        if (!ly.StartsWith("\n"))
                        {
                            //ly = ly.Replace("\\n"  , "\n");
                            int lenth = ly.Length;
                            int idex = 0;
                            while (lenth > 0)
                            {
                                List<string> li = new List<string>();
                                ly = ly.Insert(idex, Environment.NewLine);
                                li.Add(ly);
                                lenth = lenth - 35;
                                idex = idex + 37;
                            }
                        }

                        s2 = ly.Replace("  ", "\n");
                        string[] Lyric;
                        long count = 1;
                        int start = 0;
                        while ((start = s2.IndexOf('\n', start)) != -1)
                        {
                            count++;
                            start++;
                        }
                        for (int i = 0; i < count; i++)
                        {
                            Lyric = s2.Split('\n');
                            string l1 = Lyric[i].Trim();
                            if (l1 != "")
                                lines.Add(l1);
                            if (Lyric == null) break;
                        }

                        sl.Description = string.Join("\n", lines.ToArray());

                    }


                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetLyricsForFavoriteAndHistory Method In OnlineShow.cs file", ex);
            }
            return objLinkList;
        }


        public static void SaveMovieRating()
        {
            try
            {
                List<ShowList> objlist = new List<ShowList>();
                string command = string.Empty;
                objlist = new List<ShowList>();
                int showid = int.Parse(AppSettings.ShowID);
                objlist = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).OrderBy(j => j.ShowID).ToListAsync()).Result;
                foreach (ShowList links in objlist)
                {

                    links.Rating = Convert.ToDouble(AppSettings.linkRating);
                    //DataManager<ShowLinks> linksmanager = new DataManager<ShowLinks>();
                    //linksmanager.SaveToList(links, "ShowID", "LinkID");
                    links.RatingFlag = 1;
                    command = "UPDATE ShowList SET Rating=" + links.Rating + "," + "RatingFlag=" + links.RatingFlag + " " + "WHERE  ShowID=" + links.ShowID;
                    Task.Run(async () => await Constants.connection.QueryAsync<ShowLinks>(command));

                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SaveMovieRating Method In OnlineShow.cs file", ex);
            }
        }
#if NOTANDROID
        public static ImageSource GetRatingImageForMovie()
        {
            BitmapImage objImg = null;
            try
            {
                List<ShowList> objRatingList = new List<ShowList>();

                int showid = int.Parse(AppSettings.ShowID);
                objRatingList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).OrderBy(j => j.ShowID).ToListAsync()).Result;

                foreach (ShowList itm in objRatingList)
                {
                    string rating = ImageHelper.LoadRatingImage((itm.Rating).ToString());
                    objImg = new BitmapImage(new Uri(Package.Current.InstalledLocation.Path + rating, UriKind.Relative));

                    AppSettings.linkRating = itm.Rating.ToString();
                }
                Constants.selecteditem = null;
                Constants.Quizselecteditem = null;
                AppSettings.MovieRateShowStatus = false;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetRatingImageForMovie Method In OnlineShow.cs file", ex);
            }
            return objImg;
        }

        public static ImageSource GetRatingImage()
        {
            BitmapImage objImg = null;
            try
            {
                List<ShowLinks> objRatingList = new List<ShowLinks>();

                int showid = int.Parse(AppSettings.ShowID);
                string linktype = AppSettings.LinkType;
                int linkid = int.Parse(AppSettings.LinkID);
                if (linktype != "Quiz")
                {
                    objRatingList = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == linktype && i.LinkID == linkid).OrderBy(j => j.ShowID).ToListAsync()).Result;
                }
                else
                {
                    objRatingList = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == linktype && i.LinkOrder == linkid).OrderBy(j => j.ShowID).ToListAsync()).Result;
                }

                foreach (ShowLinks itm in objRatingList)
                {
                    string rating = ImageHelper.LoadRatingImage((itm.Rating).ToString());
                    objImg = new BitmapImage(new Uri(Package.Current.InstalledLocation.Path + rating, UriKind.Relative));

                    AppSettings.linkRating = itm.Rating.ToString();
                }
                Constants.selecteditem = null;
                Constants.Quizselecteditem = null;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetRatingImage Method In OnlineShow.cs file", ex);
            }
            return objImg;
        }

#endif

        public static void SaveRating()
        {
            try
            {
                List<ShowLinks> objlist = new List<ShowLinks>();
                string command = string.Empty;
                objlist = new List<ShowLinks>();
                int showid = int.Parse(AppSettings.ShowID);
                string linktype = AppSettings.LinkType;
                int linkid = int.Parse(AppSettings.LinkID);
                if (linktype != "Quiz")
                {
                    objlist = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == linktype && i.LinkID == linkid).OrderBy(j => j.LinkOrder).ToListAsync()).Result;
                }
                else
                {
                    objlist = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == linktype && i.LinkOrder == linkid).OrderBy(j => j.LinkOrder).ToListAsync()).Result;
                }
                foreach (ShowLinks links in objlist)
                {
                    if (links.LinkID == Convert.ToInt32(AppSettings.LinkID) || links.LinkOrder == Convert.ToInt32(AppSettings.LinkID))
                    {
                        links.Rating = Convert.ToDouble(AppSettings.linkRating);
                        //DataManager<ShowLinks> linksmanager = new DataManager<ShowLinks>();
                        //linksmanager.SaveToList(links, "ShowID", "LinkID");
                        links.RatingFlag = 1;
                        if (linktype != "Quiz")
                        {
                            command = "UPDATE Showlinks SET Rating=" + links.Rating + "," + "RatingFlag=" + links.RatingFlag + " " + "WHERE  ShowID=" + links.ShowID + " And   LinkID=" + links.LinkID;
                        }
                        else
                        {
                            command = "UPDATE Showlinks SET Rating=" + links.Rating + "," + "RatingFlag=" + links.RatingFlag + " " + "WHERE  ShowID=" + links.ShowID + " And   LinkOrder=" + links.LinkOrder;
                        }
                        Task.Run(async () => await Constants.connection.QueryAsync<ShowLinks>(command));
                    }

                }

                AppSettings.LinkType = "";
                AppSettings.LinkID = "";
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SaveRating Method In OnlineShow.cs file", ex);
            }
        }

        public static List<GameControls> GetGameControls(string p)
        {
            List<GameControls> objMovieList = new List<GameControls>();
            List<GameControls> objMovieList1 = new List<GameControls>();
            try
            {
                int showid = int.Parse(AppSettings.ShowID);
                objMovieList = Task.Run(async () => await Constants.connection.Table<GameControls>().Where(i => i.ShowID == showid).OrderBy(j => j.ShowID).ToListAsync()).Result;
                foreach (var item in objMovieList)
                {
                    GameControls objMovie = new GameControls();
                    objMovie.ShowID = Convert.ToInt32(item.ShowID);
                    objMovie.ControlId = item.ControlId;
                    objMovie.ControlName = item.ControlName;
                    objMovie.Controldescription = item.Controldescription;
                    objMovieList1.Add(objMovie);

                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetGameControls Method In OnlineShow.cs file", ex);
            }
            return objMovieList1;
        }


        public static ShowLinks GetAudioLinks(string id, LinkType linkType)
        {
            ShowLinks objlinks = new ShowLinks();
            string songurl = string.Empty;
            try
            {
                int showid = int.Parse(id);
                string linktype = linkType.ToString();
                objlinks = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == linktype).FirstOrDefaultAsync()).Result;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetAudioLinks Method In OnlineShow.cs file", ex);
            }
            return objlinks;


        }
        public static ObservableCollection<ShowLinks> GetMixVideos()
        {
            try
            {
                int showid = int.Parse(AppSettings.ShowID);
                Constants.selecteditemshowlinklist = new ObservableCollection<ShowLinks>(Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid).OrderBy(j => j.LinkOrder).ToListAsync()).Result);
                foreach (ShowLinks sl in Constants.selecteditemshowlinklist)
                {
                    sl.ThumbnailImage = sl.UrlType;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetShowLinksByType Method In OnlineShow.cs file", ex);
            }
            return Constants.selecteditemshowlinklist;
        }
        public static List<PlayListTable> GetBookMarkDetailsForShow(int showid)
        {
            List<PlayListTable> playlist = new List<PlayListTable>();

            try
            {
                int bookmarkid = 1;
                List<PlayListTable> playlistforshows = Task.Run(async () => await Constants.connection.Table<PlayListTable>().Where(i => i.ShowID == showid).OrderBy(j => j.ID).ToListAsync()).Result;
                foreach (PlayListTable item in playlistforshows)
                {
                    PlayListTable table = new PlayListTable();
                    string starttime = item.StartTime.Split(':')[2];
                    string endtime = item.EndTime.Split(':')[2];
                    table.LinkUrl = item.LinkUrl;
                    table.ShowID = item.ShowID;
                    table.StartTime = item.StartTime.Replace(starttime, starttime.Split('.')[0]);
                    table.EndTime = item.EndTime.Replace(endtime, endtime.Split('.')[0]);
                    table.BookMarkNo = bookmarkid.ToString();
                    bookmarkid++;
                    playlist.Add(table);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadSortShowsForSortName Method In OnlineShow.cs file", ex);
            }
            return playlist;
        }

        #region LoadSortShowsForSortName
        public static List<SortByShowsTable> LoadSortShowsForSortName()
        {
            List<SortByShowsTable> sbst = new List<SortByShowsTable>();
            try
            {
                XElement xdoc = XElement.Load("DefaultData/SortByShowsTable.xml");
                foreach (XElement values in xdoc.Elements())
                {
                    SortByShowsTable item = new SortByShowsTable();
                    item.SortID = int.Parse(values.Element("SortID").Value);
                    item.SortName = values.Element("SortName").Value;
                    sbst.Add(item);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadSortShowsForSortName Method In OnlineShow.cs file", ex);
            }
            return sbst;
        }
        #endregion

        //***  code for Wp8.
        public static List<ShowLinks> GetTopRatedAudioLinks()
        {
            List<ShowLinks> objMovieList = new List<ShowLinks>();
            List<ShowLinks> ReturnList = new List<ShowLinks>();
            List<ShowList> objMovieList1 = new List<ShowList>();
            try
            {
                string type1 = LinkType.Audio.ToString();
                var query = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == type1 && i.IsHidden == false).OrderByDescending(j => j.Rating).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Take(15);

                foreach (var item in query)
                {
                    if (!ReturnList.Contains(item))
                    {
                        ReturnList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetTopRatedShows Method In OnlineShow.cs file", ex);
                Exceptions.UpdateAgentLog("TOP MUSIC:" + Environment.NewLine + "Message:" + Environment.NewLine + ex.Message + Environment.NewLine + "InnerException:" + Environment.NewLine + ex.InnerException + Environment.NewLine + "StackTrace:" + Environment.NewLine + ex.StackTrace);
            }
            return ReturnList;
        }

        public static List<ShowLinks> GetTopRatedAudioLinksforXml()
        {
            List<ShowLinks> objMovieList = new List<ShowLinks>();
            List<ShowLinks> ReturnList = new List<ShowLinks>();
            List<ShowList> objMovieList1 = new List<ShowList>();
	#if NOTANDROID
            try
            {
                XDocument xdoc = XDocument.Load(Constants.TopAudioItemsFilePath);
                var findEle = from i in xdoc.Descendants("NewDataSet") select i;

                foreach (var item in findEle.Descendants("ShowLinks"))
                {
                    ShowLinks objAudio = new ShowLinks();
                    objAudio.LinkID = Convert.ToInt32(item.Element("LinkID").Value);
                    objAudio.ShowID = Convert.ToInt32(item.Element("ShowID").Value);
                    objAudio.LinkUrl = item.Element("LinkUrl").Value;
                    objAudio.Title = item.Element("Title").Value;
                    ReturnList.Add(objAudio);
                }

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetTopRatedShows Method In OnlineShow.cs file", ex);
                Exceptions.UpdateAgentLog("TOP MUSIC:" + Environment.NewLine + "Message:" + Environment.NewLine + ex.Message + Environment.NewLine + "InnerException:" + Environment.NewLine + ex.InnerException + Environment.NewLine + "StackTrace:" + Environment.NewLine + ex.StackTrace);
            }
	#endif
            return ReturnList;
        }

        public static PersonGallery GetPersonGallerypoup(long personID, string imagepath)
        {
            try
            {
                int pid1 = Convert.ToInt32(personID);
                int imageno = Convert.ToInt32(imagepath);
                var PeopleList = Task.Run(async () => await Constants.connection.Table<PersonGallery>().Where(i => i.PersonID == pid1 && i.ImageNo == imageno).OrderByDescending(j => j.PersonID).ToListAsync()).Result;
                return PeopleList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                ex.Data.Add("PersonGallery", personID);
                Exceptions.SaveOrSendExceptions("Exception in GetpersonDetails Method In Vidoes.cs file", ex);
            }
            return null;
        }

        public static string GetHelpItemTitle(string Id)
        {

            string HelpItemTitle = string.Empty;
            try
            {
#if ANDROID
				string path=Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Help.xml");
				XDocument xdoc = XDocument.Load(path);
#endif

#if NOTANDROID
                XDocument xdoc = XDocument.Load(Constants.HelpMenuItemsFilePath);
#endif

                var findEle = from i in xdoc.Descendants("de") where i.Attribute("id").Value.ToString() == Id select i;

                foreach (var n in findEle.Descendants("c"))
                {
                    HelpItemTitle = n.Attribute("title").Value;
                    break;
                }


            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadMenuList  Method In Vidoes.cs file", ex);
            }
            return HelpItemTitle;

        }

        public static ShowList GetShowDetail(long ShowID)
        {
            try
            {
                var show = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == ShowID).FirstOrDefaultAsync()).Result;
                return show;

            }
            catch (Exception ex)
            {
                ex.Data.Add("id", ShowID);
                Exceptions.SaveOrSendExceptions("Exception in GetShowDetail Method In OnlineShow.cs file", ex);
                return null;
            }
        }

        public static List<ShowLinks> GetRemoveShowList(string id)
        {
            List<ShowLinks> objVideoLinks = new List<ShowLinks>();
            try
            {
                int s = 1;
                int showid = int.Parse(id);
                var VideoLinks = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == "Songs").ToListAsync()).Result;
                foreach (ShowLinks itm in VideoLinks)
                {
                    if (itm != null)
                    {
                        //if there is no link don't add this song to the list.
                        if (!string.IsNullOrEmpty(itm.LinkUrl))
                        {
                            if (itm.IsHidden == false)
                            {
                                itm.ContextRemoveShow = "Hide Video";
                            }
                            else
                            {
                                itm.ContextRemoveShow = "Show Video";
                            }
                            objVideoLinks.Add(itm);
                            s++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("id", id);
                Exceptions.SaveOrSendExceptions("Exception in GetRemoveSongsList  Method In Vidoes.cs file", ex);
            }
            return objVideoLinks;
        }

        public static void SaveRemoveSongs(string id, string ChapterNo)
        {
            List<ShowLinks> shows = null;
            try
            {
                int showid = int.Parse(id);
                int linkorder = int.Parse(ChapterNo);
                var ShowLinks = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkOrder == linkorder).ToListAsync()).Result;
                shows = new List<ShowLinks>();
                foreach (ShowLinks item in ShowLinks)
                {
                    if (item != null)
                        if (item.IsHidden == false)
                        {
                            item.IsHidden = true;
                            item.ClientPreferenceUpdated = DateTime.Now;
                        }
                        else
                        {
                            item.IsHidden = false;
                            item.ClientPreferenceUpdated = DateTime.Now;
                        }
                    ShowLinks desToUpdate = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkOrder == linkorder).FirstOrDefaultAsync()).Result;
                    desToUpdate.IsHidden = item.IsHidden;
                    desToUpdate.ClientPreferenceUpdated = item.ClientPreferenceUpdated;
                    Task.Run(async () => await Constants.connection.UpdateAsync(desToUpdate));

                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("id", id);
                Exceptions.SaveOrSendExceptions("Exception in SaveRemoveSongs  Method In Vidoes.cs file", ex);
            }
        }

        public static ShowLinks GetLinkDetailByTitle(LinkType linkType, long showID, string linkTitle)
        {
            try
            {

                string linktype = linkType.ToString();
                var showlinks = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showID && i.LinkType == linktype && i.Title == linkTitle).FirstOrDefaultAsync()).Result;
                return showlinks;
            }
            catch (Exception ex)
            {
                ex.Data.Add("Link Type", linkType.ToString());
                ex.Data.Add("Show ID", showID);
                ex.Data.Add("Link Title", linkTitle);
                Exceptions.SaveOrSendExceptions("Exception in GetLinkByTitle Method In OnlineShow.cs file", ex);
            }
            return null;
        }

        public static ShowList GetShowDetailForHistory(long ShowID)
        {
            try
            {
                var showlist = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == ShowID).FirstOrDefaultAsync()).Result;
                return showlist;
            }
            catch (Exception ex)
            {
                ex.Data.Add("id", ShowID);
                Exceptions.SaveOrSendExceptions("Exception in GetShowDetail Method In OnlineShow.cs file", ex);
                return null;
            }
        }

        public static List<ShowLinks> GetShowLinksByTypeForWp8(string id, LinkType linkType)
        {
            List<ShowLinks> objMovieLinks = null;
            try
            {

                int showid = Convert.ToInt32(id);
                string linktype = linkType.ToString();
                var ShowLinksByType = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == linktype && i.IsHidden == false).ToListAsync()).Result;
                objMovieLinks = new List<ShowLinks>();
                foreach (ShowLinks Link in ShowLinksByType)
                {


                    if (AppSettings.SongID == Link.LinkID.ToString() && AppSettings.AudioImage == Constants.PlayImagePath)
                    {
                        Link.Songplay = Constants.SongPlayPath;

                    }
                    else
                    {
                        Link.Songplay = Constants.PlayImagePath;
                        Link.RingToneDetails = Link.Title + "," + Link.LinkUrl;
                    }


                    objMovieLinks.Add(Link);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("id", id);
                Exceptions.SaveOrSendExceptions("Exception in GetVideoLinksOfAMovie Method In OnlineShow.cs file", ex);
            }
            return objMovieLinks;
        }

        public static CastProfile GetPersonDetail(string personID)
        {
            try
            {
                int personid = Convert.ToInt32(personID);
                var PeopleList = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID == personid).FirstOrDefaultAsync()).Result;
                return PeopleList;
            }
            catch (Exception ex)
            {
                ex.Data.Add("Cast Profile", personID);
                Exceptions.SaveOrSendExceptions("Exception in GetpersonDetails Method In Vidoes.cs file", ex);
            }
            return null;
        }

        public static List<GalleryImageInfo> GetPersonGallery(long personID)
        {
            List<GalleryImageInfo> GalleryImageList = new List<GalleryImageInfo>();
            try
            {
                int pid1 = Convert.ToInt32(personID);
                var PeopleList = Task.Run(async () => await Constants.connection.Table<PersonGallery>().Where(i => i.PersonID == pid1).OrderByDescending(j => j.PersonID).ToListAsync()).Result;
                foreach (PersonGallery gall in PeopleList)
                {
                    GalleryImageInfo objgalprop = new GalleryImageInfo();
#if NOTANDROID
                    objgalprop.Thumbnail = new BitmapImage(new Uri(gall.FlickrThumbNailImage, UriKind.RelativeOrAbsolute));
                    objgalprop.FullImage = new BitmapImage(new Uri(gall.FlickrGalleryImage, UriKind.RelativeOrAbsolute));
#endif
			#if IOS
			objgalprop.ThumbnailImage=gall.FlickrThumbNailImage;
			objgalprop.FullImage=gall.FlickrGalleryImage;
			#endif
                    objgalprop.Title = gall.ImageNo + ".jpg";
                    GalleryImageList.Add(objgalprop);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("PersonGallery", personID);
                Exceptions.SaveOrSendExceptions("Exception in GetpersonDetails Method In Vidoes.cs file", ex);
            }
            return GalleryImageList;
        }

        public static List<ShowList> GetPersonRelatedShows(string pid)
        {
            List<ShowList> shows = null;
            try
            {
                int pid1 = Convert.ToInt32(pid);
                List<ShowCast> shc = new List<ShowCast>();
                shc = Task.Run(async () => await Constants.connection.Table<ShowCast>().Where(i => i.PersonID == pid1).ToListAsync()).Result;
                var ShowList = shc.Select(k => k.ShowID).Distinct().ToList();

                shows = new List<ShowList>();

                foreach (int objcast in ShowList.OrderByDescending(i => i))
                {
                    ShowList show = new ShowList();
                    var item = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == objcast).FirstOrDefaultAsync()).Result;
                    show.ShowID = Convert.ToInt32(item.ShowID.ToString());
#if NOTANDROID
                    show.Image = ResourceHelper.getShowTileImage(item.TileImage);
#endif
                    show.Title = item.Title;
                    show.Rating = item.Rating;
                    show.RatingImage = ImageHelper.LoadRatingImage(item.Rating.ToString()).ToString();
                    if (item.ReleaseDate != null)
                    {
#if WINDOWS_PHONE_APP
                        if (item.ReleaseDate.Contains(','))
                        {
                            show.ReleaseDate = item.ReleaseDate.Substring(item.ReleaseDate.LastIndexOf(',') + 1);
                        }
                        else
                        {
                            show.ReleaseDate = item.ReleaseDate;
                        }
#endif
                    }

                    show.Genre = ProcessShowGenre(item);

                    if (item.SubTitle != null)
                    {
                        show.SubTitle = item.SubTitle;
                    }
                    shows.Add(show);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("pid", pid);
                Exceptions.SaveOrSendExceptions("Exception in GetPersonMovies Method In Vidoes.cs file", ex);
            }
            return shows;
        }

        private static string ProcessShowGenre(ShowList item)
        {
            string showGenre = string.Empty;
            try
            {
                int showid = Convert.ToInt32(item.ShowID.ToString());
                var cat = Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(i => i.ShowID == showid).ToListAsync()).Result;
                foreach (var gen in cat)
                {
                    int cateid = gen.CatageoryID;
#if WINDOWS_PHONE_APP
                    CatageoryTable catt = new CatageoryTable();
                    try
                    {
                        catt = Task.Run(async () => await Constants.connection.Table<CatageoryTable>().Where(i => i.CategoryID == cateid).FirstOrDefaultAsync()).Result;
                    }
                    catch (Exception ex)
                    {


                    }
#else
                    ShowCategories catt = new ShowCategories();
                    try
                    {
                        catt = Task.Run(async () => await Constants.connection.Table<ShowCategories>().Where(i => i.CategoryID == cateid).FirstOrDefaultAsync()).Result;
                    }
                    catch (Exception ex)
                    {

                    }
#endif

                    if (catt != null)
                    {
                        if (catt.CategoryName.Contains(Constants.MovieCategoryHindi) || catt.CategoryName.Contains(Constants.MovieCategoryTelugu) || catt.CategoryName.Contains(Constants.MovieCategoryTamil))
                        {

                            if (catt != null)
                            {
                                if (string.IsNullOrEmpty(showGenre))
                                {
                                    showGenre = catt.CategoryName;
                                }
                                else
                                {
                                    showGenre = showGenre + "," + " " + catt.CategoryName;
                                }
                            }

                            //disprop.GenreIsvisible = Visibility.Visible;
                        }
                        else
                        {

                            if (string.IsNullOrEmpty(showGenre))
                            {
                                showGenre = catt.CategoryName;
                            }
                            else
                            {
                                showGenre = showGenre + "," + " " + catt.CategoryName;
                            }

                            //disprop.GenreIsvisible = Visibility.Visible;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ProcessShowGenre  Method In Vidoes.cs file", ex);
            }
            return showGenre;
        }
     
        public static List<HelpItem> GetHelpItem(string Id)
        {
            List<HelpItem> helpMenuItems = new List<HelpItem>();

            try
            {
#if ANDROID
				string path=Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Help.xml");
				XDocument xdoc = XDocument.Load(path);
#endif

#if NOTANDROID
                XDocument xdoc = XDocument.Load(Constants.HelpMenuItemsFilePath);
#endif

                var findEle = from i in xdoc.Descendants("de") where i.Attribute("id").Value.ToString() == Id select i;

                foreach (var d in findEle.Descendants("c").Elements("des"))
                {
                    HelpItem h = new HelpItem();
                    h.HelpText = d.Value;
                    helpMenuItems.Add(h);
                }

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadMenuList  Method In Vidoes.cs file", ex);
            }
            return helpMenuItems;
        }

        public static List<MenuProperties> LoadMenuList()
        {
            List<MenuProperties> objMenulist = new List<MenuProperties>();
            try
            {
#if ANDROID
				string path=Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "HelpMenu.xml");
				XDocument xdoc = XDocument.Load(path);
#endif
#if NOTANDROID
                XDocument xdoc = XDocument.Load(Constants.HelpMenuXmlPath);
#endif
                var xquery = from v in xdoc.Descendants("menu") select v;
                foreach (var item in xquery)
                {

                    MenuProperties disprop = new MenuProperties();
                    disprop.Id = item.Attribute("id").Value;
                    //disprop.Image = "/" + App.ImagePath + it.Element("i").Value;
                    disprop.Name = item.Element("name").Value;
                    disprop.Url = item.Element("url").Value;
                    objMenulist.Add(disprop);
                }

            }

            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadMenuList  Method In Vidoes.cs file", ex);
            }
            return objMenulist;
        }

        public static void SaveRemoveVideos(int ShowID)
        {
            try
            {
                var selectedLink = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == ShowID).FirstOrDefaultAsync()).Result;
                if (selectedLink != null)
                {
                    if (selectedLink.IsHidden == false)
                    {
                        selectedLink.IsHidden = true;
                        selectedLink.ClientPreferenceUpdated = DateTime.Now;
                    }
                    else
                    {
                        selectedLink.IsHidden = false;
                        selectedLink.ClientPreferenceUpdated = DateTime.Now;

                    }
                    ShowList desToUpdate = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == ShowID).FirstOrDefaultAsync()).Result;
                    desToUpdate.IsHidden = selectedLink.IsHidden;
                    desToUpdate.ClientPreferenceUpdated = selectedLink.ClientPreferenceUpdated;
                    Task.Run(async () => await Constants.connection.UpdateAsync(desToUpdate));
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("id", ShowID);
                Exceptions.SaveOrSendExceptions("Exception in SaveRemoveVideos Method In Vidoes.cs file", ex);
            }
        }

        public static List<ShowList> LoadNextPageShowList(List<ShowList> showList, int rowCount,string language)
        {
            List<ShowList> shows = null;
            shows = new List<ShowList>();
            foreach (ShowList item in showList)
            {
                ShowList show = new ShowList();

                show.ShowID = Convert.ToInt32(item.ShowID.ToString());
#if NOTANDROID
                AppSettings.IDForImagePath = item.ShowID.ToString();
                show.Image =ResourceHelper.getShowTileImage(item.TileImage);
#endif
                //if (ResourceHelper.AppName == Apps.DownloadManger.ToString())
                //{
                //    show.Title = item.Title.Substring(item.Title.LastIndexOf('/')+1).ToString();
                //}
                //else
                //{
                    show.Title = item.Title;
                //}
                if (string.IsNullOrEmpty(item.Rating.ToString()))
                {
                    show.Rating = 3.0;
                    show.RatingImage = ImageHelper.LoadRatingImage(show.Rating.ToString()).ToString();
                }
                else
                {
                    show.Rating = item.Rating;
                    show.RatingImage = ImageHelper.LoadRatingImage(item.Rating.ToString()).ToString();
                }
                if (item.ReleaseDate != null)
                {
#if WINDOWS_PHONE_APP
                    if (item.ReleaseDate.Contains(','))
                    {
                        show.ReleaseDate = item.ReleaseDate.Substring(item.ReleaseDate.LastIndexOf(',') + 1);
                    }
                    else
                    {
                        show.ReleaseDate = item.ReleaseDate;
                        if (ResourceHelper.ProjectName == "Web Tile")
                        {
                            show.FileSize = null;
                        }
                        else
                        {
                            if (AppSettings.AddNewShowIconVisibility == true)
                            {
                                show.FileSize = "Size : " + item.ReleaseDate;
                            }
                            else
                                show.FileSize = string.Empty;
                        }
                    }
#endif
                }
                try
                {
                    if (ResourceHelper.ProjectName == "DrivingTest" || ResourceHelper.ProjectName == "Video Mix" || ResourceHelper.ProjectName == "Story Time" || ResourceHelper.ProjectName == "Vedic Library")
                    {
                        show.Genre = "";
                    }
                    else
                    {
                        show.Genre = ProcessShowGenre(item);
                    }
                }
                catch (Exception ex)
                {

                }
                if (ResourceHelper.ProjectName == "Web Tile")
                {
                    show.SubTitle = null;
                }
                else
                {
                    if (item.SubTitle != null)
                    {
                        show.SubTitle = item.SubTitle;
                    }
                }
              
                shows.Add(show);
            }
            if (ResourceHelper.ProjectName == "DrivingTest" || ResourceHelper.ProjectName == "Video Mix" || ResourceHelper.ProjectName == "Story Time" || ResourceHelper.ProjectName == "Vedic Library")
            {
                int totalMoviesCount = Task.Run(async () => await Constants.connection.Table<ShowList>().ToListAsync()).Result.Count;

                if (shows.Count() == rowCount && rowCount != totalMoviesCount)
                {
                    ShowList addgetmore = new ShowList();
                    addgetmore.Title = Constants.getMoreLabel;
                    shows.Add(addgetmore);
                }
            }
            else
            {
                int totalMoviesCount = 0;
                if (language == "hindi")
                    totalMoviesCount = Task.Run(async () => await Constants.connection.Table<ShowList>().ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == 20).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Count();
                if (language == "telugu")
                    totalMoviesCount = Task.Run(async () => await Constants.connection.Table<ShowList>().ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == 18).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Count();
                if (language == "tamil")
                    totalMoviesCount = Task.Run(async () => await Constants.connection.Table<ShowList>().ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == 19).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Count();
                if (language == "upcoming")
                    totalMoviesCount = Task.Run(async () => await Constants.connection.Table<ShowList>().ToListAsync()).Result.Count;
                if (shows.Count() == rowCount && rowCount != totalMoviesCount)
                {
                    ShowList addgetmore = new ShowList();
                    addgetmore.Title = Constants.getMoreLabel;
                    shows.Add(addgetmore);
                }
            }
            //Constants.UIThread = false;
            return shows;
        }

        public static List<ShowList> LoadCategoryTopRated(int topratedCount, string catid)
        {
            List<ShowList> shows = null;
            try
            {

                int cateid = int.Parse(catid);
#if WINDOWS_PHONE_APP
                List<ShowList> ShowList = new List<ShowList>();
                if (AppSettings.AllSubjects == "all subjects")
                {
                    ShowList = (List<ShowList>)Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result;
                    AppSettings.AllSubjects = string.Empty;
                }
                else if (AppSettings.AllRecipes == "all recipes")
                {
                    ShowList = (List<ShowList>)Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result;
                    AppSettings.AllRecipes = string.Empty;
                }
                else
                    ShowList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == cateid).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).ToList().Take(topratedCount).ToList();
#endif
#if WINDOWS_APP
                var ShowList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == cateid).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).ToList().Take(topratedCount);
#endif
                shows = new List<ShowList>();
                try
                {
                    foreach (ShowList item in ShowList)
                    {
                        ShowList disprop = new ShowList();

                        disprop.ShowID = Convert.ToInt32(item.ShowID.ToString());
#if NOTANDROID
                        disprop.Image = ResourceHelper.getShowTileImage(item.TileImage);
#endif
                        disprop.Title = item.Title;
                        disprop.RatingImage = ImageHelper.LoadRatingImage(item.Rating.ToString()).ToString();
                        if (item.ReleaseDate != null)
                        {
#if WINDOWS_PHONE_APP
                            if (item.ReleaseDate.Contains(','))
                            {
                                disprop.ReleaseDate = item.ReleaseDate.Substring(item.ReleaseDate.LastIndexOf(',') + 1);
                            }
                            else
                            {
                                disprop.ReleaseDate = item.ReleaseDate;
                            }
#endif
                        }

                        int showid = Convert.ToInt32(item.ShowID.ToString());
                        var cat = Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.ShowID == showid).ToListAsync()).Result;
                        foreach (var gen in cat)
                        {
                            var catt = Task.Run(async () => await Constants.connection.Table<CatageoryTable>().Where(k => k.CategoryID == gen.CatageoryID).FirstOrDefaultAsync()).Result;
                            if (catt != null)
                            {
                                if (catt.CategoryName.Contains(Constants.MovieCategoryHindi) || catt.CategoryName.Contains(Constants.MovieCategoryTelugu) || catt.CategoryName.Contains(Constants.MovieCategoryTamil))
                                {

                                    if (catt != null)
                                    {
                                        if (string.IsNullOrEmpty(disprop.Genre))
                                        {
                                            disprop.Genre = catt.CategoryName;
                                        }
                                        else
                                        {
                                            disprop.Genre = disprop.Genre + "," + " " + catt.CategoryName;
                                        }
                                    }

                                }
                                else
                                {

                                    if (string.IsNullOrEmpty(disprop.Genre))
                                    {
                                        disprop.Genre = catt.CategoryName;
                                    }
                                    else
                                    {
                                        disprop.Genre = disprop.Genre + "," + " " + catt.CategoryName;
                                    }

                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(item.SubTitle))
                        {
                            disprop.SubTitle = item.SubTitle;
                        }
                        if (AppSettings.AddNewShowIconVisibility)
                        {
                        }
                        shows.Add(disprop);
                    }
                }
                catch (Exception ex)
                {

                }


                int totalMoviesCount = Task.Run(async () => await Constants.connection.Table<ShowList>().ToListAsync()).Result.Count;
                if (shows.Count() == topratedCount && topratedCount != totalMoviesCount)
                {
                    ShowList addgetmore = new ShowList();
                    addgetmore.Title = "get more";
                    shows.Add(addgetmore);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("topratedCount", topratedCount);
                Exceptions.SaveOrSendExceptions("Exception in LoadCategoryTopRated  Method In Vidoes.cs file", ex);
            }
            Constants.UIThread = false;
            return shows;
        }
        public static List<ShowList> GetLanguages(int topratedCount, string catid,string language)
        {
            List<ShowList> shows = null;
            try
            {
                int cateid = int.Parse(catid);
                List<ShowList> ShowList = new List<ShowList>();
                List<CategoriesByShowID> CategoriesByShowID = new List<CategoriesByShowID>();
                List<ShowList> ShowList1 = new List<ShowList>();
                //while (ShowList.Count < 10)
                //{
                    ShowList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == cateid).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).ToList();
                //}
              if(language=="Hindi")
                CategoriesByShowID = Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(i => i.CatageoryID == 20).ToListAsync()).Result;
              if (language == "telugu")
                  CategoriesByShowID = Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(i => i.CatageoryID == 18).ToListAsync()).Result;
              if (language == "tamil")
                  CategoriesByShowID = Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(i => i.CatageoryID == 19).ToListAsync()).Result;
                ShowList1 = ShowList.Join(CategoriesByShowID, p => p.ShowID, o => o.ShowID, (p, o) => p).ToList().Take(topratedCount).ToList();
                shows = new List<ShowList>();
                foreach (ShowList item in ShowList1)
                {
                    ShowList disprop = new ShowList();

                    disprop.ShowID = Convert.ToInt32(item.ShowID.ToString());
#if NOTANDROID
                    disprop.Image =ResourceHelper.getShowTileImage(item.TileImage);
#endif
                    disprop.Title = item.Title;
                    disprop.RatingImage = ImageHelper.LoadRatingImage(item.Rating.ToString()).ToString();
                    if (item.ReleaseDate != null)
                    {
#if WINDOWS_PHONE_APP
                        if (item.ReleaseDate.Contains(','))
                        {
                            disprop.ReleaseDate = item.ReleaseDate.Substring(item.ReleaseDate.LastIndexOf(',') + 1);
                        }
                        else
                        {
                            disprop.ReleaseDate = item.ReleaseDate;
                        }
#endif
                    }

                    int showid = Convert.ToInt32(item.ShowID.ToString());
                    var cat = Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.ShowID == showid).ToListAsync()).Result;
                    foreach (var gen in cat)
                    {
                        var catt = Task.Run(async () => await Constants.connection.Table<CatageoryTable>().Where(k => k.CategoryID == gen.CatageoryID).FirstOrDefaultAsync()).Result;
                        if (catt != null)
                        {
                            if (catt.CategoryName.Contains(Constants.MovieCategoryHindi) || catt.CategoryName.Contains(Constants.MovieCategoryTelugu) || catt.CategoryName.Contains(Constants.MovieCategoryTamil))
                            {

                                if (catt != null)
                                {
                                    if (string.IsNullOrEmpty(disprop.Genre))
                                    {
                                        disprop.Genre = catt.CategoryName;
                                    }
                                    else
                                    {
                                        disprop.Genre = disprop.Genre + "," + " " + catt.CategoryName;
                                    }
                                }

                            }
                            else
                            {

                                if (string.IsNullOrEmpty(disprop.Genre))
                                {
                                    disprop.Genre = catt.CategoryName;
                                }
                                else
                                {
                                    disprop.Genre = disprop.Genre + "," + " " + catt.CategoryName;
                                }

                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(item.SubTitle))
                    {
                        disprop.SubTitle = item.SubTitle;
                    }

                    if (AppSettings.AddNewShowIconVisibility)
                    {
                    }
                    shows.Add(disprop);
                }
                int totalMoviesCount = 0;
                //if (language == "hindi")
                totalMoviesCount = ShowList.Join(CategoriesByShowID, p => p.ShowID, o => o.ShowID, (p, o) => p).ToList().Count;
                //if (language == "telugu")
                //    totalMoviesCount = Task.Run(async () => await Constants.connection.Table<ShowList>().ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == 18).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Count();
                //if (language == "tamil")
                //    totalMoviesCount = Task.Run(async () => await Constants.connection.Table<ShowList>().ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == 19).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Count();
                if (shows.Count() == topratedCount && topratedCount != totalMoviesCount)
                {
                    ShowList addgetmore = new ShowList();
                    addgetmore.Title = "get more";
                    shows.Add(addgetmore);
                }
            }
            catch (Exception ex)
            {

            }
            Constants.UIThread = false;
            return shows;
        }
        public static List<ShowList> GetTopRatedShows(int topratedCount)
        {
            try
            {
                List<ShowList> ShowList1 = new List<ShowList>();
                if (AppSettings.ProjectName == "Indian_Cinema" || AppSettings.ProjectName == "Indian Cinema Pro" || AppSettings.ProjectName == "Indian Cinema")
                {
                    ShowList1 = Task.Run(async () => await Constants.connection.Table<OnlineVideos.Entities.ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == 20).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Where(k => Convert.ToDateTime(k.ReleaseDate).CompareTo(DateTime.Today) <= 0).Take(topratedCount).ToList();
                    //var ShowList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.Rating).OrderByDescending(k => k.ReleaseDate).ToListAsync()).Result.Take(topratedCount).ToList();
                }
                else if (AppSettings.ProjectName == "Bollywood Music")
                {
                    ShowList1 = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.Rating).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Where(x => x.ShowID == ((Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result != null) ? (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result).ShowID : 0)).Take(topratedCount).ToList();
                }
                else if (AppSettings.ProjectName == "Kids TV")
                {
                    ShowList1 = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.Rating).OrderByDescending(k => k.ReleaseDate).ToListAsync()).Result.Take(topratedCount).ToList();
                }
                else
                {
                    ShowList1 = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.Rating).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Take(topratedCount).ToList();                
                }
                return LoadNextPageShowList(ShowList1, topratedCount,"hindi");
            }

            catch (Exception ex)
            {
                ex.Data.Add("topratedCount", topratedCount);
                Exceptions.SaveOrSendExceptions("Exception in GetTopRatedShows Method In OnlineShow.cs file", ex);
                return null;
            }
        }

        public static List<ShowList> GetRecentlyAddedShows(int recentCount)
        {
            try
            {
                List<ShowList> ShowList1 = new List<ShowList>();
                if (AppSettings.ProjectName == "Indian_Cinema" || AppSettings.ProjectName == "Indian Cinema Pro" || AppSettings.ProjectName == "Indian Cinema")
                {
                    ShowList1 = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == 18).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Where(k => Convert.ToDateTime(k.ReleaseDate).CompareTo(DateTime.Today) < 0).Take(recentCount).ToList();
                    //var ShowList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.Rating).OrderByDescending(k => k.ReleaseDate).ToListAsync()).Result.Take(topratedCount).ToList();
                }
                else if (AppSettings.ProjectName == "Bollywood Music")
                {

                    ShowList1 = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.Rating).ToListAsync()).Result.Where(x => x.ShowID == ((Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result != null) ? (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkType == "Audio" && i.ShowID == x.ShowID).FirstOrDefaultAsync()).Result).ShowID : 0)).Take(recentCount).ToList();
                }
                else if (AppSettings.ProjectName == "Kids TV")
                {
                    ShowList1 = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.Rating).ToListAsync()).Result.Take(recentCount).ToList();
                }
                else
                {
                    ShowList1 = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.Rating).ToListAsync()).Result.Take(recentCount).ToList();

                }                
                return LoadNextPageShowList(ShowList1, recentCount,"telugu");
            }
            catch (Exception ex)
            {
                ex.Data.Add("recentCount", recentCount);
                Exceptions.SaveOrSendExceptions("Exception in GetRecentlyAddedShows Method In OnlineShow.cs file", ex);
                return null;
            }
        }

        public static List<ShowList> GetTamilAddedShows(int tamilCount)
        {
            try
            {               
               var ShowList1 = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Join(Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(p => p.CatageoryID == 19).ToListAsync()).Result, p => p.ShowID, o => o.ShowID, (p, o) => p).Where(k => Convert.ToDateTime(k.ReleaseDate).CompareTo(DateTime.Today) <= 0).Take(tamilCount).ToList();
                //var ShowList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Take(recentCount).ToList();
               return LoadNextPageShowList(ShowList1, tamilCount, "tamil");
            }
            catch (Exception ex)
            {
                ex.Data.Add("tamilCount", tamilCount);
                Exceptions.SaveOrSendExceptions("Exception in GetTamilAddedShows Method In OnlineShow.cs file", ex);
                return null;
            }
        }
        public static List<ShowList> GetUpComingAddedShows(int upcomingCount)
        {
            try
            {
                var ShowList1 = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.Rating).ToListAsync()).Result.Where(k => Convert.ToDateTime(k.ReleaseDate).CompareTo(DateTime.Today) > 0).Take(upcomingCount).ToList();
                //var ShowList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.ShowID).ToListAsync()).Result.Take(recentCount).ToList();
                return LoadNextPageShowList(ShowList1, upcomingCount, "upcoming");
            }
            catch (Exception ex)
            {
                ex.Data.Add("upcomingCount", upcomingCount);
                Exceptions.SaveOrSendExceptions("Exception in GetTamilAddedShows Method In OnlineShow.cs file", ex);
                return null;
            }
        }
    }
}
