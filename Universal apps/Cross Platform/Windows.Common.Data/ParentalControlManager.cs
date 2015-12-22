using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using Common.Library;
using OnlineVideos.Entities;
using System.Collections.Generic;
using System.Globalization;

namespace OnlineVideos.Data
{
    public static class ParentalControlManager
    {
        public static List<ShowLinks> GetLastUpdatedLinkForParentalControl()
        {

            DataManager<ShowLinks> LinksManager = new DataManager<ShowLinks>();
            int showid =Convert.ToInt32(SettingsHelper.getStringValue("LastUploadedVideoIdHid"));
            DateTime time = ValidationHelper.convertdatetime(SettingsHelper.getStringValue("LastUpdatedDateForHide"));
            var list = LinksManager.GetList(i => i.LinkID > showid && i.ClientPreferenceUpdated > time, j => j.ShowID, "asc").ToList().Take(10);
            return list.AsQueryable<ShowLinks>().ToList();
        }

        public static List<ShowList> GetLastUpdatedShowForParentalControl()
        {

            if (!string.IsNullOrEmpty(SettingsHelper.getStringValue("LastUploadedShowIdHid")))
            {
                DataManager<ShowList> ListManager = new DataManager<ShowList>();
                int showid=Convert.ToInt32(SettingsHelper.getStringValue("LastUploadedShowIdHid"));
                    DateTime time=ValidationHelper.convertdatetime(SettingsHelper.getStringValue("LastUpdatedDateForHide"));
                    var list = ListManager.GetList(i => i.ShowID > showid && i.ClientPreferenceUpdated > time, j => j.ShowID, "asc").ToList().Take(10);
                return list.AsQueryable<ShowList>().ToList();
            }
            else
                return null;


        }

        public static IQueryable<ShowLinks> GetLastUpdatedLinkCountForParentalControl()
        {
            DataManager<ShowLinks> LinksManager = new DataManager<ShowLinks>();
            int showid =Convert.ToInt32(SettingsHelper.getStringValue("LastUploadedVideoIdHid"));
            DateTime time = ValidationHelper.convertdatetime(SettingsHelper.getStringValue("LastUpdatedDateForHide"));
            var list = LinksManager.GetList(i => i.LinkID > showid && i.ClientPreferenceUpdated > time, j => j.ShowID, "asc");
            return list.AsQueryable<ShowLinks>();
        }

        public static IQueryable<ShowList> GetLastUpdatedVideoCountForParentalControl()
        {
            DataManager<ShowList> ListManager = new DataManager<ShowList>();
            int showid=Convert.ToInt32(SettingsHelper.getStringValue("LastUploadedMovieIdHid"));
            DateTime time=ValidationHelper.convertdatetime(SettingsHelper.getStringValue("LastUpdatedDateForHide"));
            var list = ListManager.GetList(i => i.ShowID >showid  && i.ClientPreferenceUpdated >time , j => j.ShowID, "asc");
            return list.AsQueryable<ShowList>();
        }

        public static void UpdateParentalControlForSongByShowID(List<ShowLinks> CheckMovieIDForParentalControlPreferencesForSong)
        {
            DataManager<ShowLinks> LinksManager = new DataManager<ShowLinks>();
            ShowLinks RowToUpdate = CheckMovieIDForParentalControlPreferencesForSong.FirstOrDefault();
            RowToUpdate.IsHidden = true;
            LinksManager.SaveToList(RowToUpdate, "ShowID", "LinkID");
        }

        public static void UpdateParentalControlPreferencesByShowID(List<ShowList> CheckMovieIDForParentalControlPreferences)
        {
            DataManager<ShowList> ListManager = new DataManager<ShowList>();
            ShowList RowToUpdate = CheckMovieIDForParentalControlPreferences.FirstOrDefault();
            RowToUpdate.IsHidden = true;
            ListManager.SaveToList(RowToUpdate, "ShowID", "");
        }

        public static List<ShowLinks> CheckShowIDForParentalControlPreferencesForSong(string id, string songno)
        {
            DataManager<ShowLinks> LinksManager = new DataManager<ShowLinks>();
            int showid=Convert.ToInt32(id);
            var list = LinksManager.GetList(i => i.ShowID == showid && i.LinkOrder.ToString() == songno, j => j.ShowID, "asc");
            return list.AsQueryable<ShowLinks>().ToList();

        }

        public static List<ShowList> CheckShowIDForParentalControlPreferences(string MovieID)
        {
            DataManager<ShowList> ListManager = new DataManager<ShowList>();
            int showid=Convert.ToInt32(MovieID);
            var list = ListManager.GetList(i => i.ShowID == showid, j => j.ShowID, "asc");
            return list.AsQueryable<ShowList>().ToList();
        }
    }
}
