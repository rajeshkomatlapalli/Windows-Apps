using System;
using System.Net;
using System.Windows;
using System.Linq;
using System.Collections.Generic;
using OnlineVideos.Entities;
using Common.Library;
 #if NOTANDROID
using Windows.Storage;
using Windows.Storage.Streams;
#endif
using System.Threading.Tasks;
using System.Xml.Linq;
using Common;
#if WINDOWS_APP
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
#endif
using System.Threading;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
#if WP8 &&  NOTANDROID
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
#endif


namespace OnlineVideos.Data
{
    public static class SearchManager
    {
        public static int SearchId = 0;
      #if NOTANDROID
        public static ImageSource loadBitmapImageInBackground(string imagefile, UserControl thread, AutoResetEvent aa)
        {
            BitmapImage image = null;
#if WINDOWS_APP
            thread.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {

                image = new BitmapImage(new Uri(imagefile, UriKind.RelativeOrAbsolute));
                aa.Set();
            });
            aa.WaitOne();
#endif
            return image;
        }
#endif
        public static List<ShowList> GetShowsBySearch(string searchText)
        {
            List<ShowList> objMovieList = new List<ShowList>();
            List<ShowList> objMovieList1 = new List<ShowList>();
            try
            {
                objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.Title.Contains(searchText)).OrderBy(j => j.ShowID).ToListAsync()).Result;
                foreach (var item in objMovieList)
                {
                    ShowList objMovie = new ShowList();
                    #if NOTANDROID
                    objMovie.LandingImage = ResourceHelper.getViewAllImageFromStorageOrInstalledFolder(item.TileImage);
#endif
                    objMovie.ShowID = Convert.ToInt32(item.ShowID);
                    objMovie.Title = item.Title;
                    objMovie.TileImage = item.TileImage;
                    objMovie.RatingBitmapImage = ImageHelper.LoadRatingImage(item.Rating.ToString());
                    objMovie.ReleaseDate = item.ReleaseDate;
                    if (ResourceHelper.ProjectName == "Web Tile")
                    {
                        objMovie.SubTitle = null;
                    }
                    else
                    {
                        objMovie.SubTitle = item.SubTitle;
                        objMovie.SubTitle = "Subtitle: " + objMovie.SubTitle;
                    }
                    objMovieList1.Add(objMovie);


                }
            }

            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetShowsBySearch Method In SearchManger.cs file", ex);
            }
            return objMovieList1;
        }
        public static List<ShowLinks> GetShowsLinksBySearch(string searchText, LinkType linkType)
        {
            List<ShowLinks> objMovieLinks = new List<ShowLinks>();
            List<ShowLinks> objMovieLinks1 = new List<ShowLinks>();
            try
            {
                string type = linkType.ToString();
                objMovieLinks = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.Title.Contains(searchText) && i.LinkType == type && i.IsHidden == false).OrderBy(j => j.ShowID).ToListAsync()).Result;
                if (linkType != LinkType.Audio)
                {
                    foreach (ShowLinks sl in objMovieLinks)
                    {
#if NOTANDROID
                        sl.ThumbnailImage = "http://img.youtube.com/vi/" + sl.LinkUrl + "/default.jpg";
#endif
#if ANDROID && NOTIOS
					sl.Thumbnail ="http://img.youtube.com/vi/" + sl.LinkUrl + "/default.jpg";
#endif

                    }
                }
                foreach (ShowLinks links in objMovieLinks)
                {
                    ShowLinks showlink = new ShowLinks();
                    showlink.ShowID = links.ShowID;
                    showlink.LinkID = links.LinkID;

                    showlink.SongNO = links.SongNO;
                    showlink.LinkType = links.LinkType;
                    showlink.LinkUrl = links.LinkUrl;
                    showlink.LinkOrder = links.LinkOrder;
                    if (links.LinkType == "Audio")
                    {
                        if (AppSettings.SongID == links.LinkID.ToString() && AppSettings.AudioImage == Constants.PlayImagePath)
                            showlink.Songplay = Constants.SongPlayPath;
                        else
                            showlink.Songplay = Constants.PlayImagePath;
                    }
                    if (ResourceHelper.AppName == Apps.Web_Media.ToString())
                        showlink.Title = links.Title.Substring(links.Title.LastIndexOf('/') + 1).ToString();
                    else
                        showlink.Title = links.Title;
                    
                    #if NOTANDROID
                    showlink.ThumbnailImage = "http://img.youtube.com/vi/" + links.LinkUrl + "/default.jpg";
                    #endif
                    #if ANDROID && NOTIOS
					showlink.Thumbnail ="http://img.youtube.com/vi/" + links.LinkUrl + "/default.jpg";
                    #endif

                    objMovieLinks1.Add(showlink);

                }

            }

            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetShowsLinksBySearch Method In SearchManger.cs file", ex);
            }
            return objMovieLinks1;
        }

        public static List<CastProfile> GetPeopleBySearch(string searchText)
        {
            List<CastProfile> matchedPeople = new List<CastProfile>();
            try
            {
                List<CastProfile> objcastprofile = new List<CastProfile>();
                objcastprofile =Task.Run(async () => await Constants.connection.Table<CastProfile>().ToListAsync()).Result;
                foreach (var row in objcastprofile.Where(i => i.Name.ToLower().Trim().Contains(searchText.ToLower().Trim())).OrderBy(j => j.PersonID))
                {
                    CastProfile person = new CastProfile();
                    person.Name = row.Name;
                    person.PersonID = row.PersonID;
               #if ANDROID
				    person.FlickrPersonImageUrl=row.FlickrPersonImageUrl;
				#endif
                    matchedPeople.Add(person);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("searchText", searchText);
                Exceptions.SaveOrSendExceptions("Exception in GetPeopleBySearch Method In SearchManager.cs file", ex);
            }
            return matchedPeople;
        }

        public static void SaveWebSearchHistory(string name)
        {
            try
            {
                var matchedshow = Constants.connection.Table<SearchHistory>().Where(i => i.ShowName == name).FirstOrDefaultAsync().Result;
                if (matchedshow!=null)
                {
                   
                        matchedshow.SearchCount = matchedshow.SearchCount + 1;
                        Constants.connection.UpdateAsync(matchedshow);
                   
                }
                else
                {
                    SearchHistory searchhistory = new SearchHistory();
                    searchhistory.ShowName = name;
                    searchhistory.SearchCount = 1;
                    Constants.connection.InsertAsync(searchhistory);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SaveSearchHistory Method In SearchManger.cs file", ex);
            }

        }
        public async static void SaveSearchHistoryIntoDatabase(string name)
        {
            try
            {
                SearchHistory searchhistory = new SearchHistory();
                searchhistory =Constants.connection.Table<SearchHistory>().Where(row => row.ShowName == name).FirstOrDefaultAsync().Result;
                if (searchhistory!=null)
                {

                    searchhistory.SearchCount = searchhistory.SearchCount + 1;
                  await Constants.connection.UpdateAsync(searchhistory);
                   
                }
                else
                {
                    SearchHistory searchhistory1 = new SearchHistory();
                    searchhistory1.ShowName = name;
                    searchhistory1.SearchCount = 1;
                   await Constants.connection.InsertAsync(searchhistory1);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Show Name", name);
                Exceptions.SaveOrSendExceptions("Exception in SaveSearchHistoryIntoDatabase Method In SearchManager.cs file", ex);
            }
        }
        public async static void SaveSearchHistory(string name)
        {
            try
            {
                string file = @"XmlData\SearchHistory.xml";
                #if NOTANDROID
                if (Task.Run(async () => await Storage.FileExists(file)).Result)
#endif
                #if ANDROID
                if (Task.Run(async()=> await  Storage.FileExists(file)).Result)
#endif 
                {
                    XDocument xdoc = await Storage.ReadFileAsDocument(file);
                    SearchId = SearchId + 1;

                    XElement xele = new XElement("NewDataSet",
                    new XElement("SearchHistory",
                    new XElement("ID", SearchId),
                    new XElement("ShowName", name),
                    new XElement("SearchCount", 1)));
                    xdoc.Root.Add(xele);
                    Storage.SaveFileFromDocument(file, xdoc);
                }

                else
                    SaveSearchHistoryRootItem(name);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SaveSearchHistory Method In SearchManger.cs file", ex);
            }

        }

        private static void SaveSearchHistoryRootItem(string name)
        {
            string Mcount = "1";
            SearchId = 1;
            string file = @"XmlData\SearchHistory.xml";
            XDocument xdoc = new XDocument(new XElement("NewDataSet",
            new XElement("SearchHistory",
            new XElement("ID", Mcount),
            new XElement("ShowName", name),
            new XElement("SearchCount", Mcount))));
            Storage.SaveFileFromDocument(file, xdoc);

        }

        public static List<QuizList> GetQuizBySearch(string searchText)
        {
            List<QuizList> objMovieList = new List<QuizList>();
            List<QuizList> objMovieList1 = new List<QuizList>();
            try
            {
                objMovieList = Task.Run(async () => await Constants.connection.Table<QuizList>().Where(i => i.Name.ToLower().Contains(searchText)).OrderBy(j => j.ShowID).ToListAsync()).Result;
                foreach (var item in objMovieList)
                {
                    QuizList objMovie = new QuizList();
                    objMovie.ShowID = Convert.ToInt32(item.ShowID);
                    objMovie.Name = item.Name;
                    objMovie.QuizID = item.QuizID;
                    objMovieList1.Add(objMovie);
                }
            }

            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetQuizBySearch Method In SearchManger.cs file", ex);
            }
            return objMovieList1;
        }

        public static List<QuizList> GetSubjectsBySearch(string searchText)
        {
            List<QuizList> matchedsubjects = null;
            try
            {
                matchedsubjects = new List<QuizList>();
                var subjectsList = Task.Run(async () => await Constants.connection.Table<QuizList>().ToListAsync()).Result;
                foreach (var row in subjectsList.Where(i => i.Name.ToLower().Trim().Contains(searchText.ToLower().Trim())))
                {
                    QuizList subject = new QuizList();

                    subject.Name = row.Name;
                    subject.QuizID = row.QuizID;
                    subject.ShowID = row.ShowID;
                    matchedsubjects.Add(subject);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("searchText", searchText);
                Exceptions.SaveOrSendExceptions("Exception in GetPeopleBySearch Method In SearchManager.cs file", ex);
            }
            return matchedsubjects;
        }



    }
}
