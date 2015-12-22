using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using Common.Library;
using OnlineVideos.Entities;
using Common;
using System.Collections.Generic;
#if WINDOWS_APP
//using Common.Services.OnlineVideoService;
#endif
#if WP8 &&  NOTANDROID
using OnlineVideos.Services.OnlineVideoService;
#endif
namespace OnlineVideos.Data
{
    public static class RatingManager
    {
#if NOTANDROID
        public static void UpdateRatingForShows(string showid,double rating)
        {
            try
            {
                int ShowID = Convert.ToInt32(showid);
                DataManager<ShowList> ShowListManager = new DataManager<ShowList>();
                ShowList list = ShowListManager.GetFromList(i => i.ShowID == ShowID);
                if (list != default(ShowList))
                {
                    list.Rating = rating;
                    list.ShowID = ShowID;
                    ShowListManager.SaveToList(list, "ShowID", "");
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in UpdateRatingForShows Method In RatingManger.cs file", ex);
            }
           
        }

        //public static ShowList GetShowIdForNextUpdatedDateForRating(Rating objrat)
        //{
        //    DataManager<ShowList> ShowListManager = new DataManager<ShowList>();
        //    ShowList list =await ShowListManager.GetFromList(i => i.ShowID == objrat._movieId);
        //    return list;
        //}

        public static void UpdateRatingForVideos(int showid,int linkid)
        {
            DataManager<ShowLinks> ShowListManager = new DataManager<ShowLinks>();
            ShowLinks list = ShowListManager.GetFromList(i => i.ShowID == showid && i.LinkID == linkid);
            //list.Rating = Convert.ToDouble(d.Rating);
            list.ShowID = showid;
            list.LinkID = linkid;
            list.RatingFlag = 0;
            ShowListManager.SaveToList(list, "ShowID", "LinkID");
          
        }

        public static void UpdateRatingForVideos(string title,double rating)
        {
            //int ShowID =Convert.ToInt32(title.Substring(0, title.IndexOf('-')));
            //int LinkID = Convert.ToInt32(title.Substring(title.IndexOf('-'),title.Length));
            int ShowID = Convert.ToInt32(title.Split('-')[0]);
            int LinkID = Convert.ToInt32(title.Split('-')[1]);
            DataManager<ShowLinks> ShowListManager = new DataManager<ShowLinks>();
            ShowLinks list = ShowListManager.GetFromList(i => i.ShowID == ShowID && i.LinkID== LinkID);
            if (list != default(ShowLinks))
            {
                list.Rating = rating;
                list.ShowID = ShowID;
                list.LinkID = LinkID;
                ShowListManager.SaveToList(list, "ShowID", "LinkID");
            }
        }
        public static void updateRating(int showid)
        {
           
            DataManager<ShowList> ShowListManager = new DataManager<ShowList>();
            ShowList list = ShowListManager.GetFromList(i => i.ShowID == showid);
            //list.Rating = Convert.ToDouble(d.Rating);
            list.ShowID = showid;
            list.RatingFlag = 0;
            ShowListManager.SaveToList(list, "ShowID", "");
        }
#endif
        public static List<ShowLinks> GetUpdatedRatingForShow()
        {
            DataManager<ShowLinks> ShowListManager = new DataManager<ShowLinks>();
            int videorateid=AppSettings.LastUploadVideoRatingid;
            List<ShowLinks> list = ShowListManager.GetList(i => i.RatingFlag == 1 && i.LinkID > videorateid, j => j.LinkOrder, "asc").Take(10).ToList();
            return list.AsQueryable<ShowLinks>().ToList();
        }

        public static List<ShowList> GetRatingvalues()
        {
            DataManager<ShowList> ShowListManager = new DataManager<ShowList>();
            int rateid = AppSettings.LastUploadRatingId;
            List<ShowList> list = ShowListManager.GetList(i => i.RatingFlag == 1 && i.ShowID > rateid, j => j.ShowID, "asc").Take(10).ToList();
            return list.AsQueryable<ShowList>().ToList();
        }

        public static List<ShowList> GetShowsForRating()
        {
            DataManager<ShowList> ShowListManager = new DataManager<ShowList>();
            int rateid=AppSettings.LastUploadRatingId;
            List<ShowList> list = ShowListManager.GetList(i => i.RatingFlag == 1 && i.ShowID > rateid, j => j.ShowID, "asc");
            return list.AsQueryable<ShowList>().ToList();
        }

        public static List<ShowLinks> GetShowForVideoRatingUpload()
        {
            DataManager<ShowLinks> ShowListManager = new DataManager<ShowLinks>();
            int videorateid = AppSettings.LastUploadVideoRatingid;
            List<ShowLinks> list = ShowListManager.GetList(i => i.RatingFlag == 1 && i.LinkID > videorateid, j => j.LinkOrder, "asc");
            return list.AsQueryable<ShowLinks>().ToList();
        }
    }
}
