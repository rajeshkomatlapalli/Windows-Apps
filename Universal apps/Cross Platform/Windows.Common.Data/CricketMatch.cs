using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using OnlineVideos.Entities;
using Common.Library;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineVideos.Data
{
    public static class CricketMatch
    {
        public static void GetCricketTeam1Title(string movieId)
        {
           
               // List<ShowCategories> sc = new List<ShowCategories>();
               // int showid = Convert.ToInt32(movieId);
               // var cs = Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(i => i.ShowID == showid).ToListAsync()).Result;
               // foreach (var dd in cs)
               // {
               //      sc = Task.Run(async () => await Constants.connection.Table<ShowCategories>().Where(i => i.CategoryID == dd.CatageoryID).ToListAsync()).Result;
               // }
               //// var xquery = (from a in context.CatageoryInfo join i in context.MovieCategories on a.CatageoryID equals i.CategoryID where (a.ShowID == Convert.ToInt32(movieId)) select i.Abbreviation).ToList();

               // AppSettings.TeamATitle = sc[0].ToString();
               // AppSettings.TeamBTitle = sc[1].ToString();
                try
                {
                    int showid = Convert.ToInt32(movieId);
                    MatchExtras q1 = Task.Run(async () => await Constants.connection.Table<MatchExtras>().Where(i => i.MatchID == showid).FirstOrDefaultAsync()).Result;
                    if (q1 != null)
                    {
                        ShowCategories dda = Task.Run(async () => await Constants.connection.Table<ShowCategories>().Where(i => i.CategoryName == q1.TeamAInnings).FirstOrDefaultAsync()).Result;
                        ShowCategories ddb = Task.Run(async () => await Constants.connection.Table<ShowCategories>().Where(i => i.CategoryName == q1.TeamBInnings).FirstOrDefaultAsync()).Result;
                        if(dda!=null)
                        AppSettings.TeamATitle = dda.Abbreviation;
                        if(ddb!=null)
                        AppSettings.TeamBTitle = ddb.Abbreviation;

                    }
                }
                catch (Exception ex)
                {
                     GetTeamTitle(AppSettings.ShowID);
                string mess = "Exception in GetCricketTeam1Title Method In Vidoes.cs file\n\n" + ex.Message + " \n\n Stack Trace \n" + ex.StackTrace;
                ex.Data.Add("movieId", movieId);
                Exceptions.SaveOrSendExceptions(mess, ex);
                }
           
        }
        public static void GetTeamTitle(string movieId)
        {
            try
            {
            int showid=Convert.ToInt32(movieId);
            var q1 = Task.Run(async () => await Constants.connection.Table<MatchExtras>().Where(i => i.MatchID == showid).ToListAsync()).Result;

                foreach (var a in q1)
                {
                    AppSettings.TeamATitle = a.TeamAInnings;
                    AppSettings.TeamBTitle = a.TeamBInnings;

                }
            }
            catch (Exception ex)
            {

            }
        }
        public static string GetMovieTitle(string movieId)
        {
            string movieTitle = "";
            //OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
            try
            {
                if (movieId != string.Empty)
                {
                    int id = Convert.ToInt32(movieId);
                    var xquery = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == id).ToListAsync()).Result;
                    foreach (var item in xquery)
                    {
                        movieTitle = item.Title;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                string mess = "Exception in GetMovieTitle Method In Vidoes.cs file\n\n" + ex.Message + " \n\n Stack Trace \n" + ex.StackTrace;
                ex.Data.Add("movieId", movieId);
                Exceptions.SaveOrSendExceptions(mess, ex);
            }
            return movieTitle;

        }
        public static MatchExtras GetExtas(string pid)
        {
            MatchExtras objLinkList = new MatchExtras();
            try
            {
                int matchid=Convert.ToInt32(pid);
                objLinkList = Task.Run(async () => await Constants.connection.Table<MatchExtras>().Where(i => i.MatchID == matchid).FirstOrDefaultAsync()).Result;  
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetExtas Method In CricketMatch.cs file", ex);
            }
            return objLinkList;
        }
        public static List<MatchBattingScore> LoadBattingSummary(string id, TeamType TeamType)
        {
            List<MatchBattingScore> BattingSummary = null;
            try
            {
              
                int showid=Convert.ToInt32(id);
                string teamtype = TeamType.ToString();
                List<MatchBattingScore> BatingScore = Task.Run(async () => await Constants.connection.Table<MatchBattingScore>().Where(i => i.MatchId == showid && i.Runs != 0 && i.TeamType == teamtype).OrderByDescending(i => i.Runs).ToListAsync()).Result;
                BattingSummary = new List<MatchBattingScore>();

                int j = 0;
                foreach (var item in BatingScore)
                {


                    BattingSummary.Add(item);
                    j++;
                    if (j == 3)
                        break;
                }
            }
            catch (Exception ex)
            {
                string mess = "Exception in LoadBattingSummary  Method In Vidoes.cs file\n\n" + ex.Message + " \n\n Stack Trace \n" + ex.StackTrace;
                ex.Data.Add("movieId", id);
                Exceptions.SaveOrSendExceptions("Exception in LoadBattingSummary  Method In Vidoes.cs file", ex);
            }
            return BattingSummary;

        }

        public static List<MatchBowlingScore> LoadbowlingSummary(string id, TeamType TeamType)
        {
            List<MatchBowlingScore> BowlingSummary = null;
            try
            {

                int showid = Convert.ToInt32(id);
                string teamtype = TeamType.ToString();
                List<MatchBowlingScore> BollingScore = Task.Run(async () => await Constants.connection.Table<MatchBowlingScore>().Where(p => p.MatchId == showid && p.Runs != 0 && p.Wickets != 0 && p.TeamType == teamtype).OrderBy(i => i.Runs).OrderByDescending(i => i.Wickets).ToListAsync()).Result;
                BowlingSummary = new List<MatchBowlingScore>();
                int j = 0;
                foreach (MatchBowlingScore item in BollingScore)
                {
                    BowlingSummary.Add(item);
                    j++;
                    if (j == 3)
                        break;
                }
            }
            catch (Exception ex)
            {
                string mess = "Exception in LoadTeamAbowlSummary  Method In Vidoes.cs file\n\n" + ex.Message + " \n\n Stack Trace \n" + ex.StackTrace;
                ex.Data.Add("movieId", id);
                Exceptions.SaveOrSendExceptions("Exception in GetExtraSection  Method In Vidoes.cs file", ex);
            }
            return BowlingSummary;

        }
        public static List<MatchBattingScore> LoadBattingInnings(string id, TeamType TeamType)
        {
            List<MatchBattingScore> BattingSummary = new List<MatchBattingScore>();
            try
            {
                int teamid=Convert.ToInt32(id);
                string teamtype=TeamType.ToString();
                List<MatchBattingScore> objBattingSummaryDetails = new List<MatchBattingScore>();
                objBattingSummaryDetails = Task.Run(async () => await Constants.connection.Table<MatchBattingScore>().Where(i => i.MatchId == teamid && i.Runs != 0 && i.TeamType == teamtype).OrderByDescending(j => j.Runs).ToListAsync()).Result; 
                foreach (MatchBattingScore item in objBattingSummaryDetails)
                {
                    MatchBattingScore battinglist = new MatchBattingScore();
                    battinglist.PlayerName = item.PlayerName;
                    battinglist.Out = item.Out;
                    battinglist.Runs = item.Runs;
                    battinglist.Balls = item.Balls;
                    BattingSummary.Add(battinglist);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadBattingInnings Method In CricketMatch.cs file", ex);
            }
            return BattingSummary;
        }
        public static MatchExtras GetExtraSection(string id)
        {
          int showid=Convert.ToInt32(id);

            try
            {
                if (Task.Run(async () => await Constants.connection.Table<MatchExtras>().Where(i => i.MatchID == showid).ToListAsync()).Result.Count != 0)
                {
                    var Extras = Task.Run(async () => await Constants.connection.Table<MatchExtras>().Where(i => i.MatchID == showid).ToListAsync()).Result;
                    return Extras.FirstOrDefault();
                }
                else
                return null;
            }
            catch (Exception ex)
            {
                string mess = "Exception in GetExtraSection  Method In Vidoes.cs file\n\n" + ex.Message + " \n\n Stack Trace \n" + ex.StackTrace;
                ex.Data.Add("movieId", id);
                Exceptions.SaveOrSendExceptions("Exception in GetExtraSection  Method In Vidoes.cs file", ex);
                return null;
            }
        }
        public static List<MatchBowlingScore> LoadbowlingInnings(string id, TeamType TeamType)
        {
            List<MatchBowlingScore> bowlingSummary = new List<MatchBowlingScore>();
            try
            {
                List<MatchBowlingScore> objbowlingSummaryDetails = new List<MatchBowlingScore>();
                 int teamid=Convert.ToInt32(id);
                string teamtype=TeamType.ToString();
                objbowlingSummaryDetails = Task.Run(async () => await Constants.connection.Table<MatchBowlingScore>().Where(i => i.MatchId == teamid && i.Runs != 0 && i.TeamType == teamtype).OrderByDescending(j => j.Wickets).ToListAsync()).Result;
                foreach (MatchBowlingScore item in objbowlingSummaryDetails)
                {
                    MatchBowlingScore bowlinglist = new MatchBowlingScore();
                    bowlinglist.PlayerName = item.PlayerName;
                    bowlinglist.Wickets = item.Wickets;
                    bowlinglist.Runs = item.Runs;
                    bowlinglist.Overs = item.Overs;
                    bowlinglist.Maidens = item.Maidens;
                    bowlingSummary.Add(bowlinglist);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadbowlingInnings Method In CricketMatch.cs file", ex);
            }
            return bowlingSummary;
        }
    }
}
