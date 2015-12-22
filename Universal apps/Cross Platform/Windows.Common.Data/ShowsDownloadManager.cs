using System;
using System.Net;
using System.Windows;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.IO;

namespace OnlineVideos.Data
{
    public static class ShowsDownloadManager
    {
    //  static  IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
    //    public static void DeleteShowLinkList(string movieId)
    //    {
    //        OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
    //        IQueryable<ShowLinkInfo> showLinks = from c in context.ShowLinks where c.MovieID == Convert.ToInt32(movieId) select c;
    //        context.ShowLinks.DeleteAllOnSubmit(showLinks);
    //        context.SubmitChanges();
    //    }

    //    public static void AddShowLinkList(List<OnlineVideos.BlogEntities.DownloadTopSongs> ShowVideoLinks)
    //    {
    //        OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
    //        List<ShowLinkInfo> objMoveLinks = new List<ShowLinkInfo>();
    //        DateTime LastClientUpdatedDateForParentalControl = (from c in context.ShowLinks select c.ClientUpdatedDateForRemove).Max();
    //        DateTime LastClientUpdatedDateForShows = (from c in context.ShowLinks select c.ClientUpdatedDate).Max();
    //        foreach (OnlineVideos.BlogEntities.DownloadTopSongs objLink in ShowVideoLinks)
    //        {
    //            ShowLinkInfo movelinks = new ShowLinkInfo();
    //            DateTime dt = DateTime.Now;

    //            if (objLink.LinkType == "Songs")
    //            {
    //                movelinks.MovieID = Convert.ToInt32(objLink.MovieId);
    //                movelinks.Title = objLink.UrlName;
    //                movelinks.Rating = objLink.Rating;
    //                movelinks.Favorite = objLink.Song_Favorite;
    //                movelinks.Link = objLink.Url;
    //                movelinks.Cno = objLink.No;
    //                movelinks.LinkType = "Songs";
    //                movelinks.RemoveShow = "0";
    //                movelinks.ClientUpdatedDate = LastClientUpdatedDateForShows;
    //                movelinks.ClientUpdatedDateForRemove = LastClientUpdatedDateForParentalControl;
    //                objMoveLinks.Add(movelinks);
    //            }
    //            else if (objLink.LinkType == "Audio")
    //            {
    //                string Filename = "Lyrics.txt";
    //                string Lyric = "";
    //                List<string> lines = new List<string>();
    //                movelinks.MovieID = Convert.ToInt32(objLink.MovieId);
    //                movelinks.Title = objLink.UrlName;
    //                movelinks.Rating = objLink.Rating;
    //                movelinks.Favorite = "0";
    //                movelinks.Link = objLink.Url;
    //                movelinks.Cno = objLink.No;
    //                movelinks.LinkType = "Audio";
    //                movelinks.RemoveShow = "0";
    //                string ly = objLink.Description;
    //                if (ly != "")
    //                {
    //                    if (Storage.FileExists(Filename))
    //                    {

    //                        IsolatedStorageFileStream dataFile = Storage.CreateFile(Filename);
    //                        dataFile.Close();
    //                    }
    //                    StreamWriter sw = new StreamWriter(new IsolatedStorageFileStream(Filename, FileMode.Append, isoStore));
    //                    sw.WriteLine(ly);
    //                    sw.Close();
    //                    StreamReader reader = new StreamReader(new IsolatedStorageFileStream(Filename, FileMode.Open, isoStore));

    //                    long count = 1;
    //                    int start = 0;
    //                    while ((start = ly.IndexOf('\n', start)) != -1)
    //                    {
    //                        count++;
    //                        start++;
    //                    }
    //                    for (int i = 0; i < count; i++)
    //                    {
    //                        Lyric = reader.ReadLine();
    //                        string l1 = Lyric.Trim();
    //                        if (l1 != "")
    //                            lines.Add(l1);
    //                        if (Lyric == null) break;
    //                    }

    //                    reader.Close();
    //                    movelinks.Description = string.Join("\n", lines.ToArray());
    //                    using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
    //                    {
    //                        if (myIsolatedStorage.FileExists(Filename))
    //                        {
    //                            myIsolatedStorage.DeleteFile(Filename);
    //                        }
    //                    }
    //                }
    //                movelinks.ClientUpdatedDate = LastClientUpdatedDateForShows;
    //                movelinks.ClientUpdatedDateForRemove = LastClientUpdatedDateForParentalControl;
    //                objMoveLinks.Add(movelinks);
    //            }
    //            else
    //            {
    //                movelinks.MovieID = Convert.ToInt32(objLink.MovieId);
    //                movelinks.Title = objLink.UrlName;
    //                movelinks.Link = objLink.Url;
    //                movelinks.LinkType = "Movies";
    //                movelinks.ClientUpdatedDate = LastClientUpdatedDateForShows;
    //                movelinks.ClientUpdatedDateForRemove = LastClientUpdatedDateForParentalControl;
    //                objMoveLinks.Add(movelinks);
    //            }
    //        }
    //        context.ShowLinks.InsertAllOnSubmit(objMoveLinks);
    //        context.SubmitChanges();
    //    }

    //    public static void DeleteTopVideos()
    //    {
    //        OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
    //        IQueryable<TopSongs> topVideos = from c in context.TopSongs select c;
    //        context.TopSongs.DeleteAllOnSubmit(topVideos);
    //        context.SubmitChanges();
    //    }

    //    public static void AddTopVideos(List<OnlineVideos.BlogEntities.TopVideos> TopVideoLinks)
    //    {
    //        OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
    //        List<TopSongs> objTopVideos = new List<TopSongs>();
    //        foreach (OnlineVideos.BlogEntities.TopVideos objLink in TopVideoLinks)
    //        {
    //            TopSongs TopVideo = new TopSongs();
    //            TopVideo.MovieID = Convert.ToInt32(objLink.ShowId);
    //            TopVideo.Title = objLink.Title;
    //            TopVideo.Sno = objLink.Index;
    //            objTopVideos.Add(TopVideo);
    //        }

    //        context.TopSongs.InsertAllOnSubmit(objTopVideos);
    //        context.SubmitChanges();
    //    }

    //    public static void DeleteBattingDetails(string matchID)
    //    {
    //        OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
    //        IQueryable<BattingTable> Batdetails = from c in context.BattingInfo where c.BatMatchId == Convert.ToInt32(matchID) select c;
    //        context.BattingInfo.DeleteAllOnSubmit(Batdetails);
    //        context.SubmitChanges();
    //    }

    //    public static void AddBattingDetails(List<OnlineVideos.BlogEntities.Cricket> objCricketLists)
    //    {
    //        List<BattingTable> objbatsmans = new List<BattingTable>();
    //        OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
    //        foreach (OnlineVideos.BlogEntities.Cricket objCricketList in objCricketLists)
    //        {
    //            if (objCricketList.TypeBt == "TeamABatting")
    //            {
    //                BattingTable objbatting = new BattingTable();
    //                objbatting.BatMatchId = Convert.ToInt32(objCricketList.MatchId);
    //                objbatting.BatsManName = objCricketList.BtName;
    //                objbatting.Out = objCricketList.Out;
    //                objbatting.Runs =Convert.ToInt32(objCricketList.BtRuns);
    //                objbatting.Balls = objCricketList.Balls;
    //                objbatting.TeamType = "TeamABatting";
    //                objbatsmans.Add(objbatting);
    //            }
    //            else
    //            {
    //                BattingTable objbatting = new BattingTable();
    //                objbatting.BatMatchId = Convert.ToInt32(objCricketList.MatchId);
    //                objbatting.BatsManName = objCricketList.BtName;
    //                objbatting.Out = objCricketList.Out;
    //                objbatting.Runs =Convert.ToInt32(objCricketList.BtRuns);
    //                objbatting.Balls = objCricketList.Balls;
    //                objbatting.TeamType = "TeamBBatting";
    //                objbatsmans.Add(objbatting);
    //            }
    //        }

    //        context.BattingInfo.InsertAllOnSubmit(objbatsmans);
    //        context.SubmitChanges();
    //    }

    //    public static void DeleteBowlingDetails(string matchId)
    //    {
    //        OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
    //        IQueryable<BowlingTable> Bowldetails = from c in context.BowlingInfo where c.BMatchId == Convert.ToInt32(matchId) select c;
    //        context.BowlingInfo.DeleteAllOnSubmit(Bowldetails);
    //        context.SubmitChanges();
    //    }

    //    public static void AddBowlingDetails(List<Cricket> objCricketListBowl)
    //    {
    //        List<BowlingTable> objbowl = new List<BowlingTable>();
    //        OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
    //        foreach (Cricket objCricketList in objCricketListBowl)
    //        {
    //            if (objCricketList.TypeBl == "TeamABowling")
    //            {
    //                BowlingTable objbowlinga = new BowlingTable();
    //                objbowlinga.BMatchId = Convert.ToInt32(objCricketList.MatchId);
    //                objbowlinga.BowlerName = objCricketList.BowlerName;
    //                objbowlinga.Overs = objCricketList.Overs;
    //                objbowlinga.Maidens = objCricketList.Maidens;
    //                objbowlinga.BowlRuns =Convert.ToInt32(objCricketList.BlRuns);
    //                objbowlinga.Wkts =Convert.ToInt32(objCricketList.Wickets);
    //                objbowlinga.TeamType = "TeamABowling";
    //                objbowl.Add(objbowlinga);
    //            }
    //            else
    //            {
    //                BowlingTable objbowling = new BowlingTable();
    //                objbowling.BMatchId = Convert.ToInt32(objCricketList.MatchId);
    //                objbowling.BowlerName = objCricketList.BowlerName;
    //                objbowling.Overs = objCricketList.Overs;
    //                objbowling.Maidens = objCricketList.Maidens;
    //                objbowling.BowlRuns =Convert.ToInt32(objCricketList.BlRuns);
    //                objbowling.Wkts =Convert.ToInt32(objCricketList.Wickets);
    //                objbowling.TeamType = "TeamBBowling";
    //                objbowl.Add(objbowling);
    //            }
    //        }
    //        context.BowlingInfo.InsertAllOnSubmit(objbowl);
    //        context.SubmitChanges();
    //    }

    //    public static void DeleteScoreDetails(string matchId)
    //    {
    //        OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
    //        IQueryable<ExtrasTable> scoredetails = from c in context.ExtraTble where c.EMatchId == Convert.ToInt32(matchId) select c;
    //        context.ExtraTble.DeleteAllOnSubmit(scoredetails);
    //        context.SubmitChanges();
    //    }

    //    public static void AddScoreDetails(Cricket objCricketDetails)
    //    {
    //        OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
    //        ExtrasTable objextras = new ExtrasTable();
    //        objextras.EMatchId = Convert.ToInt32(objCricketDetails.MatchId);
    //        objextras.TeamAExtras = objCricketDetails.TeamAExtras;
    //        objextras.TeamBExtras = objCricketDetails.TeamBExtras;
    //        objextras.TeamATotal = objCricketDetails.TeamATotal;
    //        objextras.TeamBTotal = objCricketDetails.TeamBTotal;
    //        objextras.TeamAInn = objCricketDetails.CountryA;
    //        objextras.TeamBInn = objCricketDetails.CountryB;
    //        objextras.TeamResult = objCricketDetails.MatchResult;

    //        context.ExtraTble.InsertOnSubmit(objextras);
    //        context.SubmitChanges();
    //    }

    //    public static void UpdateShowDetails(OnlineVideos.BlogEntities.Details ShowDetail)
    //    {
    //        OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
    //        DateTime LastClientUpdatedDateForParentalControl = (from c in context.ShowList select c.ClientUpdatedDateForRemove).Max();
    //        DateTime LastClientUpdatedDateForShows = (from c in context.ShowList select c.ClientUpdatedDate).Max();

    //        IQueryable<Show> showDetails = from c in context.ShowList where c.MovieID == Convert.ToInt32(ShowDetail.MovieId) select c;
    //        Show showToUpdate = showDetails.FirstOrDefault();


    //        showToUpdate.Title = ShowDetail.MovieTitle;
    //        showToUpdate.Image = ShowDetail.ImagePath;
    //        showToUpdate.Rating = ShowDetail.Rating;
    //        showToUpdate.Gener = ShowDetail.Genre;
    //        showToUpdate.RelaseDate = ShowDetail.ReleaseDate;
    //        if (ShowDetail.imagePovitPath != "" && ShowDetail.imagePovitPath != null)
    //            showToUpdate.PviotImage = ShowDetail.imagePovitPath;
    //        showToUpdate.AddDate = DateTime.Parse(ShowDetail.AddedDate);
    //        showToUpdate.Favorite = ShowDetail.Favorite;
    //        showToUpdate.SubTitle = ShowDetail.SubTitle;
    //        showToUpdate.RemoveMovie = ShowDetail.Removemovie;
    //        showToUpdate.Description = ShowDetail.Description;
    //        showToUpdate.ClientUpdatedDate = LastClientUpdatedDateForShows;
    //        showToUpdate.ClientUpdatedDateForRemove = LastClientUpdatedDateForParentalControl;


    //        context.SubmitChanges();
    //    }

    //    public static bool CheckShow(string movieId)
    //    {
    //        OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
    //        IQueryable<Show> checkMovie = from i in context.ShowList where i.MovieID == Convert.ToInt32(movieId) select i;

    //        if (checkMovie.Count() > 0)
    //            return true;
    //        else
    //            return false;
    //    }

    //    public static void AddShowDetails(OnlineVideos.BlogEntities.Details ShowDetail)
    //    {

    //        OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
    //        DateTime LastClientUpdatedDateForParentalControl = (from c in context.ShowList select c.ClientUpdatedDateForRemove).Max();
    //        DateTime LastClientUpdatedDateForShows = (from c in context.ShowList select c.ClientUpdatedDate).Max();
    //        Show objShow = new Show();

    //        objShow.MovieID = Convert.ToInt32(ShowDetail.MovieId);
    //        objShow.Title = ShowDetail.MovieTitle;
    //        objShow.Image = ShowDetail.ImagePath;
    //        objShow.Rating = ShowDetail.Rating;
    //        objShow.Gener = InsertGenre(ShowDetail);
    //        objShow.RelaseDate = ShowDetail.ReleaseDate;
    //        if (ShowDetail.imagePovitPath != "" && ShowDetail.imagePovitPath != null)
    //            objShow.PviotImage = ShowDetail.imagePovitPath;
    //        //TODO:Return DateTime field from service
    //        objShow.AddDate = DateTime.Parse(ShowDetail.AddedDate);
    //        objShow.Favorite = ShowDetail.Favorite;
    //        objShow.SubTitle = ShowDetail.SubTitle;
    //        objShow.RemoveMovie = ShowDetail.Removemovie;
    //        objShow.Description = ShowDetail.Description;
    //        objShow.ClientUpdatedDate = LastClientUpdatedDateForShows;
    //        objShow.ClientUpdatedDateForRemove = LastClientUpdatedDateForParentalControl;
    //        context.ShowList.InsertOnSubmit(objShow);
    //        context.SubmitChanges();
    //    }

    //    public static void InsertCast(OnlineVideos.BlogEntities.PersonInfo personInformation)
    //    {
    //        OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
    //        CastProfile person = new CastProfile();
    //        person.PersonID = Convert.ToInt32(personInformation.PersonId);
    //        person.Name = personInformation.PersonName;
    //        if (personInformation.GalleryCount != 0)
    //        {
    //            person.Count = (personInformation.GalleryCount).ToString();
    //        }
    //        person.Des = personInformation.Description;
    //        person.Cheatdata = personInformation.CheatData;
    //        context.PersonProfile.InsertOnSubmit(person);
    //        context.SubmitChanges();
    //    }

    //    public static void DeleteCast(string personID)
    //    {
    //        OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
    //        IQueryable<CastProfile> castToDelete = from c in context.PersonProfile where c.PersonID == Convert.ToInt32(personID) select c;
    //        context.PersonProfile.DeleteAllOnSubmit(castToDelete);
    //        context.SubmitChanges();
    //    }

    //    public static bool CheckPeople(string personID)
    //    {
    //        OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
    //        IQueryable<CastProfile> checkPerson = from i in context.PersonProfile where i.PersonID == Convert.ToInt32(personID) select i;
    //        if (checkPerson.Count() > 0)
    //            return true;
    //        else
    //            return false;
    //    }

    //    private static string InsertGenre(OnlineVideos.BlogEntities.Details ShowDetail)
    //    {
    //        string genre = string.Empty;
    //        OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
    //        if (ShowDetail.Genre != null)
    //        {
    //            if (ShowDetail.Genre.Contains("Hindi") || ShowDetail.Genre.Contains("Telugu") || ShowDetail.Genre.Contains("Tamil"))
    //            {
    //                foreach (string s in ShowDetail.Genre.Substring(0, ShowDetail.Genre.LastIndexOf(',')).Split(','))
    //                {
    //                    var categoeryquery = from i in context.CatageoryInfo where i.CatName == s select i;
    //                    if (categoeryquery.Count() > 0)
    //                    {
    //                        genre = ShowDetail.Genre;
    //                    }
    //                    else
    //                    {
    //                        var categoerycount = from i in context.CatageoryInfo select i;
    //                        CatageoryTable catt = new CatageoryTable();
    //                        catt.CatID = categoerycount.Count() + 1;
    //                        catt.CatName = s;
    //                        genre = ShowDetail.Genre;
    //                        context.CatageoryInfo.InsertOnSubmit(catt);
    //                        context.SubmitChanges();
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                foreach (string s in ShowDetail.Genre.Split(','))
    //                {
    //                    var categoeryquery = from i in context.CatageoryInfo where i.CatName == s select i;
    //                    if (categoeryquery.Count() > 0)
    //                    {
    //                        genre = ShowDetail.Genre;
    //                    }
    //                    else
    //                    {
    //                        var categoerycount = from i in context.CatageoryInfo select i;
    //                        CatageoryTable catt = new CatageoryTable();
    //                        catt.CatID = categoerycount.Count() + 1;
    //                        catt.CatName = s;
    //                        genre = ShowDetail.Genre;
    //                        context.CatageoryInfo.InsertOnSubmit(catt);
    //                        context.SubmitChanges();
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            genre = ShowDetail.Genre;
    //        }
    //        return genre;
    //    }

    }
}
