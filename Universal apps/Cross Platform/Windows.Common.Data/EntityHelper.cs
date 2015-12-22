using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using Common.Library;
using OnlineVideos.Data;
using System.Collections.ObjectModel;
using OnlineVideos.Entities;
#if WINDOWS_APP
using System.Collections.Generic;
using OnlineVideosWin81.Services.OnlineVideoService;
#endif
using Common;
#if WINDOWS_PHONE_APP
using System.Collections.Generic;
#endif

namespace OnlineVideos.Data
{
    public static class EntityHelper
    {
#if WINDOWS_APP || WINDOWS_PHONE_APP
        public static void CreateRatingList(int LastUploadRatingId, ObservableCollection<ShowList> fav, List<ShowList> getShow)
        {
            if (getShow != null)
            {
                if (getShow.Count() > 0)
                {
                    foreach (var rat in getShow)
                    {
                        ShowList v = new ShowList();
                        LastUploadRatingId = rat.ShowID;
                        v.ShowID = rat.ShowID;
                        v.Rating = rat.Rating;
                        fav.Add(v);
                    }
                }
                AppSettings.LastUploadRatingId = LastUploadRatingId;
            }
        }
        public static void CreateListForVideosRating(int LastUploadVideoRatingid, ObservableCollection<ShowLinks> fav, List<ShowLinks> GetUpdatedRating)
        {
            if (GetUpdatedRating != null)
            {
                if (GetUpdatedRating.Count() > 0)
                {
                    foreach (var rat in GetUpdatedRating.Where(x => x.LinkType == LinkType.Songs.ToString() || x.LinkType == LinkType.Audio.ToString() || x.LinkType == LinkType.Quiz.ToString()))
                    {
                        LastUploadVideoRatingid = rat.LinkID;
                        ShowLinks v = new ShowLinks();
                        v.ShowID = rat.ShowID;
                        v.LinkID = rat.LinkID;
                        v.Rating = rat.Rating;
                        v.LinkType = rat.LinkType;
                        fav.Add(v);
                    }
                }
                AppSettings.LastUploadVideoRatingid = LastUploadVideoRatingid;
            }

        }
#endif

#if NOTANDROID && WINDOWS_APP
        public static void CreateShowListForBackupFavourites(int LastUploadedShowIdFav, List<ShowList> selectShow, string deviceid, ObservableCollection<Favorites> fav)
        {
            if (selectShow != null)
            {
                if (selectShow.Count() > 0)
                {
                    foreach (var item in selectShow)
                    {
                        Favorites f = new Favorites();
                        LastUploadedShowIdFav = item.ShowID;
                        f._movieId = item.ShowID;
                        f._AnonymousId = deviceid;
                        f._LinkType = "Movies";
                        f._songno = "";
                        DateTime dt = DateTime.Now;
                        f._LastUpdated = dt.ToString("d");
                        f._ApplicationName = ResourceHelper.ProjectName;
                        f._favourite = Convert.ToInt32(item.IsFavourite);

                        fav.Add(f);
                    }
                }
                AppSettings.LastUploadedShowIdFav = LastUploadedShowIdFav;
            }
        }
             
        public static void CreateShowListForBackupHide(int LastUploadedShowIdHid, List<ShowList> GetVideo, string deviceid, ObservableCollection<Favorites> fav)
        {
            try
            {
                if (GetVideo != null)
                {
                    if (GetVideo.Count() > 0)
                    {
                        foreach (var item in GetVideo)
                        {
                            Favorites f = new Favorites();
                            LastUploadedShowIdHid = item.ShowID;
                            f._movieId = item.ShowID;
                            f._AnonymousId = deviceid;
                            f._LinkType = "Movies";
                            f._songno = "";
                            DateTime dt = DateTime.Now;
                            f._LastUpdated = dt.ToString("d");
                            f._ApplicationName = ResourceHelper.ProjectName;
                            f._remove = Convert.ToInt32(item.IsHidden).ToString();

                            fav.Add(f);
                        }
                    }
                    AppSettings.LastUploadedShowIdHid = LastUploadedShowIdHid;
                }
            }
            catch (Exception ex)
            {
                AppSettings.LastUploadedShowIdHid = LastUploadedShowIdHid;
            }
        }

        public static void CreateSongListForBackupHide(int LastUploadedVideoIdHid, List<ShowLinks> getLink, string deviceid, ObservableCollection<Favorites> fav)
        {
            try
            {
                if (getLink != null)
                {
                    if (getLink.Count() > 0)
                    {
                        foreach (var item in getLink)
                        {

                            Favorites f = new Favorites();
                            LastUploadedVideoIdHid = item.LinkID;
                            f._movieId = item.ShowID;
                            f._songno = item.LinkOrder.ToString();
                            f._AnonymousId = deviceid;
                            f._LinkType = "Songs";
                            DateTime dt = DateTime.Now;
                            f._LastUpdated = dt.ToString("d");
                            f._ApplicationName = ResourceHelper.ProjectName;
                            f._remove = Convert.ToInt32(item.IsHidden).ToString();

                            fav.Add(f);
                        }
                    }
                    AppSettings.LastUploadedVideoIdHid = LastUploadedVideoIdHid;
                }
            }
            catch (Exception ex)
            {
                AppSettings.LastUploadedVideoIdHid = LastUploadedVideoIdHid;
            }
        }

        public static void CreateSongListForBackupFavourites(int LastUploadedVideoIdFav, List<ShowLinks> selectLink, string deviceid, ObservableCollection<Favorites> fav)
        {
            if (selectLink != null)
            {
                if (selectLink.Count() > 0)
                {
                    foreach (var item in selectLink)
                    {
                        Favorites f = new Favorites();
                        LastUploadedVideoIdFav = item.LinkID;
                        f._movieId = item.ShowID;
                        f._AnonymousId = deviceid;
                        f._LinkType = "Songs";
                        f._songno = item.LinkOrder.ToString();
                        DateTime dt = DateTime.Now;
                        f._LastUpdated = dt.ToString("d");
                        f._ApplicationName = ResourceHelper.ProjectName;
                        f._favourite = Convert.ToInt32(item.IsFavourite);

                        fav.Add(f);
                    }
                }
                AppSettings.LastUploadedVideoIdFav = LastUploadedVideoIdFav;
            }
        }
#endif
    }
}
