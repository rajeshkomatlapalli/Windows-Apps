using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Phone;
using System.Collections;
using OnlineVideos.ViewModels;
using Microsoft.Phone.BackgroundTransfer;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using OnlineVideos.Library;
using Common.Library;
using OnlineVideos.UI;
using OnlineVideos.Common;
using OnlineVideos.Data;
using OnlineVideos.Entities;


namespace OnlineVideos
{
    public class Vidoes
    {
      
        IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
        XDocument xdoc = null;
        OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);

     

     
     

       

      

     

      

        public void GetCricketTeam1Title(string movieId)
        {
            try
            {
                var xquery = from a in context.MovieCastList where a.MovieID == Convert.ToInt32(movieId) select a;

                string categoryTeam1, categoryTeam2;
                categoryTeam1 = categoryTeam2 = string.Empty;

                foreach (var cat in xquery)
                    if (cat.TeamA != null)
                    {
                        categoryTeam1 = cat.TeamA;
                        categoryTeam2 = cat.TeamB;
                    }
                var xquery1 = from a in context.CountryInfo where a.Tid == Convert.ToInt32(categoryTeam1) select a;
                var xquery2 = from a in context.CountryInfo where a.Tid == Convert.ToInt32(categoryTeam2) select a;
                foreach (var item in xquery1)
                {
                    if (item.CountryName != null)
                        AppSettings.CricketTeam1Title = item.CountryName;
                    break;
                }
                foreach (var item1 in xquery2)
                {
                    if (item1.CountryName != null)
                        AppSettings.CricketTeam2Title = item1.CountryName;
                    break;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("movieId", movieId);
                Exceptions.SaveOrSendExceptions("Exception in GetCricketTeam1Title Method In Vidoes.cs file", ex);
            }
        }

        public string GetCricketT1Title(string movieId)
        {
            string TeamATitle = string.Empty;
            string categoryID = string.Empty;
            try
            {
                var xquery = from a in context.MovieCastList where a.MovieID == Convert.ToInt32(movieId) select a;
                foreach (var item in xquery)
                {

                    if (item.TeamA != null)
                    {
                        categoryID = item.TeamA;
                        break;
                    }
                }

                var xquery1 = from a in context.CountryInfo where a.ID == Convert.ToInt32(categoryID) select a;
                foreach (var item in xquery1)
                {
                    if (item.CountryName != null)
                        TeamATitle = item.CountryName;
                    break;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("movieId", movieId);
                Exceptions.SaveOrSendExceptions("Exception in GetCricketT1Title  Method In Vidoes.cs file", ex);
            }
            return TeamATitle;
        }

        //TODO: Use the teams table and removed TeamA, TeamB columns.
        public string GetCricketTeam2Title(string movieId)
        {
            string criketTitle = "";
            string criketid = "";
            try
            {
                var xquery = from a in context.MovieCastList where a.MovieID == Convert.ToInt32(movieId) select a;
                foreach (var item in xquery)
                {
                    if (item.TeamB != null)
                    {
                        criketid = item.TeamB;
                        break;
                    }
                }
                var xquery1 = from a in context.CountryInfo where a.ID == Convert.ToInt32(criketid) select a;
                foreach (var item in xquery1)
                {
                    if (item.CountryName != null)
                        criketTitle = item.CountryName;
                    break;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("movieId", movieId);
                Exceptions.SaveOrSendExceptions("Exception in GetCricketTeam2Title  Method In Vidoes.cs file", ex);
            }
            return criketTitle;
        }

        public void LoadInnings(string id, out List<BattingScore> Team1Batting, out List<BowlingScore> Team1Bowling,
                                out List<BattingScore> Team2Batting, out List<BowlingScore> Team2Bowling)
        {
            Team1Batting = null;
            Team1Bowling = null;
            Team2Batting = null;
            Team2Bowling = null;

            try
            {
                var batsmanInfo1 = from p in context.BattingInfo where p.BatMatchId == Convert.ToInt32(id) && p.TeamType == "TeamABatting" select p;
                Team1Batting = new List<BattingScore>();
                foreach (var batsman1 in batsmanInfo1)
                {
                    BattingScore team1Batting = new BattingScore();
                    team1Batting.BatsmanName = batsman1.BatsManName;
                    team1Batting.OutStatus = batsman1.Out;
                    team1Batting.Runs = Convert.ToInt32(batsman1.Runs);
                    team1Batting.Balls = Convert.ToInt32(batsman1.Balls);
                    Team1Batting.Add(team1Batting);
                }

                var bowlerInfo1 = from p in context.BowlingInfo where p.BMatchId == Convert.ToInt32(id) && p.TeamType == "TeamABowling" select p;
                Team1Bowling = new List<BowlingScore>();
                foreach (var bowler1 in bowlerInfo1)
                {
                    BowlingScore team1Bowling = new BowlingScore();
                    team1Bowling.BowlerName = bowler1.BowlerName;
                    team1Bowling.Overs = Convert.ToInt32(bowler1.Overs);
                    team1Bowling.Runs = Convert.ToInt32(bowler1.BowlRuns);
                    team1Bowling.Balls = Convert.ToInt32(bowler1.BowlBalls);
                    team1Bowling.Wickets = Convert.ToInt32(bowler1.Wkts);
                    team1Bowling.Maidens = Convert.ToInt32(bowler1.Maidens);
                    Team1Bowling.Add(team1Bowling);
                }

                var battingInfo2 = from p in context.BattingInfo where p.BatMatchId == Convert.ToInt32(id) && p.TeamType == "TeamBBatting" select p;
                Team2Batting = new List<BattingScore>();
                foreach (var batsman2 in battingInfo2)
                {
                    BattingScore team2Batting = new BattingScore();
                    team2Batting.BatsmanName = batsman2.BatsManName;
                    team2Batting.OutStatus = batsman2.Out;
                    team2Batting.Runs = Convert.ToInt32(batsman2.Runs);
                    team2Batting.Balls = Convert.ToInt32(batsman2.Balls);
                    Team2Batting.Add(team2Batting);
                }

                var BowlingInfo2 = from a in context.BowlingInfo where a.BMatchId == Convert.ToInt32(id) && a.TeamType == "TeamBBowling" select a;
                Team2Bowling = new List<BowlingScore>();
                foreach (var bowler2 in BowlingInfo2)
                {
                    BowlingScore team2Bowling = new BowlingScore();
                    team2Bowling.BowlerName = bowler2.BowlerName;
                    team2Bowling.Overs = Convert.ToInt32(bowler2.Overs);
                    team2Bowling.Runs = Convert.ToInt32(bowler2.BowlRuns);
                    team2Bowling.Balls = Convert.ToInt32(bowler2.BowlBalls);
                    team2Bowling.Wickets = Convert.ToInt32(bowler2.Wkts);
                    team2Bowling.Wickets = Convert.ToInt32(bowler2.Maidens);
                    Team2Bowling.Add(team2Bowling);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Match ID", id);
                Exceptions.SaveOrSendExceptions("Exception in LoadInnings  Method In Vidoes.cs file", ex);
            }
        }

        public List<BattingScore> LoadFirsthighestInnings(string id)
        {
            List<BattingScore> team1BattingSummary = null;
            try
            {
                var xquery = from i in context.BattingInfo where i.BatMatchId == Convert.ToInt32(id) && i.Runs != "" && i.TeamType == "TeamABatting" orderby Convert.ToInt32(i.Runs) descending select i;
                team1BattingSummary = new List<BattingScore>();
                int batsmanCount = 0;
                foreach (var item in xquery)
                {
                    BattingScore team1Batting = new BattingScore();
                    team1Batting.BatsmanName = item.BatsManName;
                    team1Batting.Runs = Convert.ToInt32(item.Runs);

                    team1BattingSummary.Add(team1Batting);
                    batsmanCount++;
                    if (batsmanCount == 3)
                        break;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("movieId", id);
                Exceptions.SaveOrSendExceptions("Exception in LoadFirsthighestInnings  Method In Vidoes.cs file", ex);
            }
            return team1BattingSummary;
        }

        public List<BattingScore> LoadSeconghighestInnings(string id)
        {
            List<BattingScore> team2BattingSummary = null;
            try
            {
                var xquery = from k in context.BattingInfo where k.BatMatchId == Convert.ToInt32(id) && k.Runs != "" && k.TeamType == "TeamBBatting" orderby Convert.ToInt32(k.Runs) descending select k;

                int i = 0;

                foreach (var item in xquery)
                {
                    BattingScore objscoreprop = new BattingScore();
                    objscoreprop.BatsmanName = item.BatsManName;
                    objscoreprop.Runs = Convert.ToInt32(item.Runs);

                    team2BattingSummary.Add(objscoreprop);
                    i++;
                    if (i == 3)
                        break;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("movieId", id);
                Exceptions.SaveOrSendExceptions("Exception in LoadSeconghighestInnings  Method In Vidoes.cs file", ex);
            }
            return team2BattingSummary;
        }

        public List<BowlingScore> LoadTeamAbowlSummary(string id)
        {
            List<BowlingScore> team1BowlingSummary = null;
            try
            {
                var xquery = from a in context.BowlingInfo where a.BMatchId == Convert.ToInt32(id) && a.BowlRuns != "" && a.Wkts != "" && a.TeamType == "TeamBBowling" orderby Convert.ToInt32(a.BowlRuns) ascending, Convert.ToInt32(a.Wkts) descending select a;
                int i = 0;

                foreach (var item in xquery)
                {
                    BowlingScore team1Bowling = new BowlingScore();
                    team1Bowling.BowlerName = item.BowlerName;
                    team1Bowling.Wickets = Convert.ToInt32(item.Wkts);
                    team1Bowling.Runs = Convert.ToInt32(item.BowlRuns);

                    team1BowlingSummary.Add(team1Bowling);

                    i++;
                    if (i == 3)
                        break;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("movieId", id);
                Exceptions.SaveOrSendExceptions("Exception in LoadTeamAbowlSummary  Method In Vidoes.cs file", ex);
            }
            return team1BowlingSummary;
        }

        public List<BowlingScore> LoadTeamBbowlSummary(string id)
        {
            List<BowlingScore> team2BowlingSummary = null;
            try
            {
                var xquery = from a in context.BowlingInfo where a.BMatchId == Convert.ToInt32(id) && a.BowlRuns != "" && a.Wkts != "" && a.TeamType == "TeamABowling" orderby Convert.ToInt32(a.BowlRuns) ascending, Convert.ToInt32(a.Wkts) descending select a;
                int i = 0;

                foreach (var item in xquery)
                {
                    BowlingScore team2Bowling = new BowlingScore();
                    team2Bowling.BowlerName = item.BowlerName;
                    team2Bowling.Wickets = Convert.ToInt32(item.Wkts);
                    team2Bowling.Runs = Convert.ToInt32(item.BowlRuns);

                    team2BowlingSummary.Add(team2Bowling);

                    i++;
                    if (i == 3)
                        break;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("movieId", id);
                Exceptions.SaveOrSendExceptions("Exception in LoadTeamBbowlSummary  Method In Vidoes.cs file", ex);
            }
            return team2BowlingSummary;
        }

        //public VideoProperties GetExtraSection(string id)
        //{
        //    VideoProperties objvideos = new VideoProperties();
        //    try
        //    {
        //        var xquery = from p in context.ExtraTble where p.EMatchId == Convert.ToInt32(id) select p;
        //        foreach (var itm in xquery)
        //        {
        //            objvideos.taExtras = itm.TeamAExtras;
        //            objvideos.taTotal = itm.TeamATotal;
        //            objvideos.Firstinngs = itm.TeamAInn;
        //            objvideos.tbExtras = itm.TeamBExtras;
        //            objvideos.tbTotal = itm.TeamBTotal;
        //            objvideos.Secondinngs = itm.TeamBInn;
        //            objvideos.MatchResult = itm.TeamResult;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Data.Add("movieId", id);
        //        Exceptions.SaveOrSendExceptions("Exception in GetExtraSection  Method In Vidoes.cs file", ex);
        //    }
        //    return objvideos;
        //}

      

       

      

       
    }
}