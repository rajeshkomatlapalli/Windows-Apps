using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading;
using System.Globalization;
using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using Common.Services;
using Common;
//using Common.Services.OnlineVideoService;
using OnlineVideosWin81.Services.OnlineVideoService;
using Windows.UI.Core;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace OnlineVideos.Library
{
    public class PersonalizationDataSync
    {

        OnlineVideoServiceClient client = new OnlineVideoServiceClient();
        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        private static async void HandleException(Exception ex, string message)
        {
            Exceptions.SaveOrSendExceptions(message + "\n\n" + ex.Message + " \n\n Stack Trace \n" + ex.StackTrace, ex);

            if (Common.SyncAgentState.SyncAgent != null)
            {
                ((Common.IDowloadCompleteCallback)(Common.SyncAgentState.SyncAgent)).OnStartEvent();
            }
        }

        public void DownloadFavorites()
        {
            if (AppSettings.IsNewVersion)
            {
                RestoreFavourites();
            }
        }
        public void DeleteIsolatedFoldersAndFiles()
        {
            if (AppSettings.IsNewVersion)
            {
                //Storage.DeleteIsolatedFoldersAndFilesFile();
            }
        }

        public void DownloadParentalPreferences()
        {
            if (AppSettings.IsNewVersion)
            {
                RestoreParentalControlPreferences();
            }
            else
            {
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadParentalControlPreferences;
            }
        }

        public void UploadFavourites()
        {
            try
            {

                if (FavoritesManager.GetCountForLastUpdatedFavouritesForVideos().Count() > 0 || FavoritesManager.GetCountForLastUpdatedFavouritesForLinks().Count() > 0)
                {
                    BackupFavourites();
                }
                else
                {
                    AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadSearch;
                }

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in UploadFavourites Method In PersonalizationDataSync.cs file", ex);
            }
        }
        public async void UploadWebSearch()
        {
            try
            {
                List<SearchHistory> GetShowIdForSearchHistory = SearchHistoryManager.GetShowIDForSearchHistory();
                if (GetShowIdForSearchHistory.Count() > 0)
                {
                    getsavesearch();
                }
                else
                {
                    AppSettings.SearchLastUploadedId = 0;
                    Common.SyncAgentState.ResetEvent();
                    //AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadRatingForShows;
                }
                Exceptions.UpdateAgentLog("UploadWebSearch Completed In PersonalizationDataSync");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in UploadWebSearch Method In Vidoes file", ex);
                //////AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadRatingForShows;
            }
        }
        public async void UploadSearch()
        {
            try
            {
                List<SearchHistory> GetShowIdForSearchHistory = SearchHistoryManager.GetShowIDForSearchHistory();

                if (GetShowIdForSearchHistory.Count() > 0 && Common.SyncAgentState.SyncAgent != null)
                {
                    var dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
                    await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                     {
                         getsavesearch();
                     });
                    Common.SyncAgentState.auto.WaitOne();
                }
                else if (GetShowIdForSearchHistory.Count() > 0 && Common.SyncAgentState.SyncAgent == null)
                {
                    getsavesearch();
                }
                else
                {
                    AppSettings.SearchLastUploadedId = 0;
                    Common.SyncAgentState.ResetEvent();
                    AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadRatingForShows;
                }
                Exceptions.UpdateAgentLog("UploadSearch Completed In PersonalizationDataSync");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in UploadSearch Method In Vidoes file", ex);
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadRatingForShows;
            }
        }

        public void UploadParentalPreferences()
        {
            try
            {
                if ((ParentalControlManager.GetLastUpdatedVideoCountForParentalControl().Count() > 0 || ParentalControlManager.GetLastUpdatedLinkCountForParentalControl().Count() > 0) && Common.SyncAgentState.SyncAgent != null)
                {
                    BackupHide();
                }
                else if ((ParentalControlManager.GetLastUpdatedVideoCountForParentalControl().Count() > 0 || ParentalControlManager.GetLastUpdatedLinkCountForParentalControl().Count() > 0) && Common.SyncAgentState.SyncAgent == null)
                {
                    BackupHide();
                }
                else
                {
                    AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadPeople;
                }
            }
            catch (Exception ex)
            {                
                Exceptions.SaveOrSendExceptions("Exception in UploadParentalPreferences Method In PersonalizationDataSync.cs file", ex);
            }
        }

        public async void DownloadRatingForShows()
        {
            //try
            //{
            //    if (SyncAgentState.SyncAgent != null)
            //    {
            //        Restorerating();
            //        SyncAgentState.auto.WaitOne();
            //    }
            //    if (SyncAgentState.SyncAgent == null)
            //    {
            //        Restorerating();
            //    }

            //}
            //catch (Exception ex)
            //{
            //    Exceptions.SaveOrSendExceptions("Exception in DownloadRatingForShows Method In PersonalizationDataSync.cs file", ex);
            //}
            try
            {

                if (Common.SyncAgentState.SyncAgent != null)
                {
                    var dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
                    await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        Restorerating();
                    });

                    Common.SyncAgentState.auto.WaitOne();
                }
                if (Common.SyncAgentState.SyncAgent == null)
                {
                    Restorerating();
                }
                Exceptions.UpdateAgentLog("DownloadRatingForShows Completed In PersonalizationDataSync");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in DownloadRatingForShows Method In Vidoes file", ex);
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadRatingForVideos;
            }
        }
        public  void GetBlogDetails()
        {
            try
            {
                ObservableCollection<BlogInfo> info = ServiceManager.GetBlogDetails("win8", ResourceHelper.ProjectName);

                foreach (BlogInfo binfo in info)
                {
                    string blogtype=binfo.BlogType.ToLower();
                    BlogCategoryTable BlogCategoryTable = Constants.connection.Table<BlogCategoryTable>().Where(i => i.BlogType == blogtype).FirstOrDefaultAsync().Result;
                    BlogCategoryTable.BlogUserName = binfo.UserName;
                    BlogCategoryTable.BlogPassword = binfo.Password;
                    int ss=Task.Run(async () => await Constants.connection.UpdateAsync(BlogCategoryTable)).Result;
                }
                AppSettings.DownloadCompletedDate = DateTime.Parse(DateTime.Now.ToString("d"));
                    if (AppSettings.ProjectName == "Story Time" || AppSettings.ProjectName == "Vedic Library")
                        AppSettings.BackgroundAgentStatus = SyncAgentStatus.RestoreStory;
                    else
                        AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadFavourites;
            }
            catch (Exception ex)
            {
                    if (AppSettings.ProjectName == "Story Time" || AppSettings.ProjectName == "Vedic Library")
                        AppSettings.BackgroundAgentStatus = SyncAgentStatus.RestoreStory;
                    else
                        AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadFavourites;
            }
        }
        public async void DownloadRatingForVideos()
        {
            //try
            //{
            //    if (SyncAgentState.SyncAgent != null)
            //    {
            //        Restoreratingvideos();
            //        SyncAgentState.auto.WaitOne();
            //    }
            //    if (SyncAgentState.SyncAgent == null)
            //    {
            //        Restoreratingvideos();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Exceptions.SaveOrSendExceptions("Exception in DownloadRatingForVideos Method In PersonalizationDataSync.cs file", ex);
            //}
            try
            {
                if (Common.SyncAgentState.SyncAgent != null)
                {
                    var dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
                    await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        Restoreratingvideos();
                    });
                    Common.SyncAgentState.auto.WaitOne();
                }

                if (Common.SyncAgentState.SyncAgent == null)
                {
                    Restoreratingvideos();
                }
                Exceptions.UpdateAgentLog("DownloadRatingForVideos Completed In PersonalizationDataSync");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in DownloadRatingForVideos Method In Vidoes file", ex);
            }
        }

        public async void UploadRatingForShows()
        {
            //try
            //{
            //    List<ShowList> GetShowsForRating = RatingManager.GetShowsForRating();
            //    if (SyncAgentState.SyncAgent != null && GetShowsForRating.Count() > 0)
            //    {
            //        getratingvalues();
            //        SyncAgentState.auto.WaitOne();
            //    }
            //    else if (SyncAgentState.SyncAgent == null && GetShowsForRating.Count() > 0)
            //    {
            //        getratingvalues();
            //    }
            //    else
            //    {
            //        AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadRatingForVideos;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Exceptions.SaveOrSendExceptions("Exception in UploadRatingForShows Method In PersonalizationDataSync.cs file", ex);
            //}
            try
            {
                List<ShowList> GetShowsForRating = RatingManager.GetShowsForRating();
                if (Common.SyncAgentState.SyncAgent != null && GetShowsForRating.Count() > 0)
                {
                    var dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
                    await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        getratingvalues();
                    });
                    Common.SyncAgentState.auto.WaitOne();
                }

                else if (Common.SyncAgentState.SyncAgent == null && GetShowsForRating.Count() > 0)
                {
                    getratingvalues();
                }
                else
                {
                    AppSettings.LastUploadRatingId = 0;
                    Common.SyncAgentState.ResetEvent();
                    AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadRatingForVideos;
                }
                Exceptions.UpdateAgentLog("UploadRatingForShows Completed In PersonalizationDataSync");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in UploadRatingForShows Method In Vidoes file", ex);
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadRatingForVideos;
            }
        }

        public async void UploadRatingForVideos()
        {
            //try
            //{
            //    List<ShowLinks> GetShowsForVideoRatingUpload = RatingManager.GetShowForVideoRatingUpload();
            //    if (SyncAgentState.SyncAgent != null && GetShowsForVideoRatingUpload.Count() > 0)
            //    {
            //        getratingvaluesforvideos();
            //        SyncAgentState.auto.WaitOne();
            //    }
            //    else if (SyncAgentState.SyncAgent == null && GetShowsForVideoRatingUpload.Count() > 0)
            //    {
            //        getratingvaluesforvideos();
            //    }
            //    else
            //    {
            //        AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadRatingForShows;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Exceptions.SaveOrSendExceptions("Exception in UploadRatingForVideos Method In PersonalizationDataSync.cs file", ex);
            //}
            try
            {
                List<ShowLinks> GetShowsForVideoRatingUpload = RatingManager.GetShowForVideoRatingUpload();
                if (Common.SyncAgentState.SyncAgent != null && GetShowsForVideoRatingUpload.Count() > 0)
                {
                    var dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
                    await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        getratingvaluesforvideos();
                    });
                    Common.SyncAgentState.auto.WaitOne();
                }
                else if (Common.SyncAgentState.SyncAgent == null && GetShowsForVideoRatingUpload.Count() > 0)
                {
                    getratingvaluesforvideos();
                }
                else
                {
                    AppSettings.LastUploadVideoRatingid = 0;

                    Common.SyncAgentState.ResetEvent();
                    AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadRatingForShows;
                }
                Exceptions.UpdateAgentLog("UploadRatingForVideos Completed In PersonalizationDataSync");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in UploadRatingForVideos Method In Vidoes file", ex);
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadRatingForShows;
            }
        }

        public void getratingvaluesforvideos()
        {
            int LastUploadVideoRatingid = 0;
            try
            {
                ObservableCollection<ShowLinks> fav = new ObservableCollection<ShowLinks>();
                List<ShowLinks> GetUpdatedRating = RatingManager.GetUpdatedRatingForShow();
                EntityHelper.CreateListForVideosRating(LastUploadVideoRatingid, fav, GetUpdatedRating);

                if (fav.Count > 0)
                {
                    UpdateRatingInBlog blograt = new UpdateRatingInBlog();
                    foreach (var ss in fav)
                    {
                        if (ResourceHelper.AppName == Apps.Online_Education.ToString() && ss.LinkType == "Quiz")
                        {
                            blograt.getshow(ss.ShowID.ToString() + "-" + ss.LinkID.ToString(), ss.Rating, "Quiz");
                        }
                        else
                        {
                            blograt.getshow(ss.ShowID.ToString() + "-" + ss.LinkID.ToString(), ss.Rating, "links");
                        }
                        RatingManager.UpdateRatingForVideos(ss.ShowID, ss.LinkID);
                    }
                    Common.SyncAgentState.ResetEvent();
                    //ServiceManager.GetRatingvideos(fav, client_GetRatingvideosCompleted);
                }
                else
                {
                    AppSettings.LastUploadVideoRatingid = 0;
                    Common.SyncAgentState.ResetEvent();
                }
                Exceptions.UpdateAgentLog("getratingvaluesforvideos Completed In PersonalizationDataSync");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in getratingvaluesforvideos Method In Vidoes file", ex);
            }
        }
        public void getratingvalues()
        {
            try
            {
                int LastUploadRatingId = 0;
                ObservableCollection<ShowList> fav = new ObservableCollection<ShowList>();
                List<ShowList> getShow = RatingManager.GetRatingvalues();
                EntityHelper.CreateRatingList(LastUploadRatingId, fav, getShow);
                if (fav.Count > 0)
                {
                    UpdateRatingInBlog blograt = new UpdateRatingInBlog();
                    foreach (var ss in fav)
                    {
                        blograt.getshow(ss.ShowID.ToString(), ss.Rating, "show");
                        RatingManager.updateRating(ss.ShowID);
                    }
                    Common.SyncAgentState.ResetEvent();
                    //ServiceManager.GetRating(fav, client_GetRatingCompleted);
                }
                else
                {
                    AppSettings.LastUploadRatingId = 0;
                    Common.SyncAgentState.ResetEvent();
                }
                Exceptions.UpdateAgentLog("getratingvalues Completed In PersonalizationDataSync");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in getratingvalues Method In Vidoes file", ex);
            }
        }

        private void BackupHide()
        {
            int LastUploadedShowIdHid = 0;
            int LastUploadedVideoIdHid = 0;
            string LastUpdatedDateForHide = string.Empty;
            try
            {
                List<ShowList> GetVideo = null;
                List<ShowLinks> getLink = null;
                string deviceid = SettingsHelper.getStringValue("uniqueDeviceId");
                ObservableCollection<Favorites> fav = new ObservableCollection<Favorites>();
                GetVideo = ParentalControlManager.GetLastUpdatedShowForParentalControl();
                EntityHelper.CreateShowListForBackupHide(LastUploadedShowIdHid, GetVideo, deviceid, fav);
                getLink = ParentalControlManager.GetLastUpdatedLinkForParentalControl();
                EntityHelper.CreateSongListForBackupHide(LastUploadedVideoIdHid, getLink, deviceid, fav);
                if (fav.Count > 0)
                {
                    ServiceManager.InsertParentalControlData(fav);
                }
                else
                {
                    DateTime dt = DateTime.Now;
                    AppSettings.LastUploadedShowIdHid = 0;
                    AppSettings.LastUploadedVideoIdHid = 0;
                    AppSettings.LastUpdatedDateForHide = dt.ToString("d");
                    Common.SyncAgentState.ResetEvent();
                    AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadPeople;
                }
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in BackupHide Method In PersonalizationDataSync.cs file", ex);
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadPeople;
            }
        }

        private void BackupFavourites()
        {
            int LastUploadedShowIdFav = 0;
            int LastUploadedVideoIdFav = 0;
            string LastUpdatedDateforFavourites = string.Empty;
            try
            {
                List<ShowList> selectShow = null;
                List<ShowLinks> selectLink = null;
                string deviceid = SettingsHelper.getStringValue("uniqueDeviceId");
                ObservableCollection<Favorites> fav = new ObservableCollection<Favorites>();
                selectShow = FavoritesManager.GetLastUpdatedFavouriteShows();
                EntityHelper.CreateShowListForBackupFavourites(LastUploadedShowIdFav, selectShow, deviceid, fav);
                selectLink = FavoritesManager.GetLastUpdatedFavouriteLinks();
                EntityHelper.CreateSongListForBackupFavourites(LastUploadedVideoIdFav, selectLink, deviceid, fav);
                if (fav.Count > 0)
                {
                    ServiceManager.InsertUserData(fav);
                }
                else
                {
                    DateTime dt = DateTime.Now;
                    AppSettings.LastUploadedShowIdFav = 0;
                    AppSettings.LastUploadedVideoIdFav = 0;
                    //AppSettings.LastUpdatedDateforFavourites = dt.ToString("d");
                    Common.SyncAgentState.ResetEvent();
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in BackupFavourites Method In PersonalizationDataSync.cs file", ex);
            }
        }

        public void getsavesearch()
        {

            string SearchLastUploadedId = string.Empty;
            try
            {
                ObservableCollection<Favorites> searchhis = new ObservableCollection<Favorites>();
                searchhis = SearchHistoryManager.GetSearchHistory(SearchLastUploadedId, searchhis);

                if (searchhis.Count > 0)
                {
                    ObservableCollection<Favorites> saved = ServiceManager.GetSearchHistory(searchhis);
                    foreach (Favorites f in saved)
                    {
                        if (f._Hstatus == 1)
                        {
                            SearchHistoryManager.DeleteSearchHistory(f);
                        }
                    }
                }
                else
                {
                    AppSettings.SearchLastUploadedId = 0;
                    Common.SyncAgentState.ResetEvent();
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in getsavesearch Method In PersonalizationDataSync.cs file", ex);
            }
        }



        //void client_GetSearchHistoryCompleted(object sender, GetSearchHistoryCompletedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Result != null)
        //        {
        //            ObservableCollection<Favorites> saved = e.Result as ObservableCollection<Favorites>;
        //            foreach (Favorites f in saved)
        //            {
        //                if (f._Hstatus == 1)
        //                {
        //                    SearchHistoryManager.DeleteSearchHistory(f);
        //                }
        //            }
        //        }
        //        SyncAgentState.ResetEvent();
        //        Exceptions.UpdateAgentLog("client_GetSearchHistoryCompleted Completed In PersonalizationDataSync");

        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex, "Exception in client_GetSearchHistoryCompleted Method In Vidoes file");
        //    }
        //}

        public void RestoreFavourites()
        {
            try
            {
                int LastDownloadIdFav = 0;
                string id = "";
                string linktype = "";
                ObservableCollection<Favorites> objfavLists = ServiceManager.RetrieveuserDataAsync(encoding.GetBytes(SettingsHelper.getStringValue("uniqueDeviceId")), AppSettings.ProjectName, AppSettings.LastDownloadIdFav.ToString());
                if (objfavLists.Count > 0)
                {
                    foreach (Favorites objfavs in objfavLists)
                    {
                        LastDownloadIdFav = objfavs._Id;
                        id = (objfavs._movieId).ToString();
                        linktype = (objfavs._LinkType);

                        if (linktype == "Movies")
                        {

                            List<ShowList> CheckShowIDForFavouritesDownload = FavoritesManager.CheckShowIDForFavouritesDownload(id);
                            if (CheckShowIDForFavouritesDownload.Count() > 0)
                            {
                                FavoritesManager.UpdateFavouritesByShowID(CheckShowIDForFavouritesDownload);
                            }
                        }
                        else
                        {
                            List<ShowLinks> CheckShowIDForFavouriteSongDownload = FavoritesManager.CheckShowIDForFavouriteSongDownload(id, objfavs._songno);

                            if (CheckShowIDForFavouriteSongDownload.Count() > 0)
                            {

                                FavoritesManager.UpdateFavouritesForSongByShowID(CheckShowIDForFavouriteSongDownload);

                            }
                        }
                    }
                    AppSettings.LastDownloadIdFav = LastDownloadIdFav;

                }
                else
                {
                    AppSettings.LastDownloadIdFav = 0;
                    AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadPeople;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in RestoreFavourites Method In PersonalizationDataSync.cs file", ex);
            }
        }


        public void RestoreParentalControlPreferences()
        {
            int LastDownloadIdHid = 0;
            string id = "";
            string linktype = "";
            try
            {
                ObservableCollection<Favorites> objfavLists = ServiceManager.RestoreParentalControlPreferences(encoding.GetBytes(SettingsHelper.getStringValue("uniqueDeviceId")), AppSettings.ProjectName, AppSettings.LastDownloadIdHid.ToString());
                if (objfavLists.Count > 0)
                {
                    foreach (Favorites objfavs in objfavLists)
                    {
                        id = (objfavs._movieId).ToString();
                        linktype = (objfavs._LinkType);

                        LastDownloadIdHid = objfavs._Id;
                        if (linktype == "Movies")
                        {
                            List<ShowList> CheckShowIDForParentalControlPreferences = ParentalControlManager.CheckShowIDForParentalControlPreferences(id);

                            if (CheckShowIDForParentalControlPreferences.Count() > 0)
                            {
                                ParentalControlManager.UpdateParentalControlPreferencesByShowID(CheckShowIDForParentalControlPreferences);
                            }
                        }
                        else
                        {
                            List<ShowLinks> CheckShowIDForParentalControlPreferencesForSong = ParentalControlManager.CheckShowIDForParentalControlPreferencesForSong(id, objfavs._songno);

                            if (CheckShowIDForParentalControlPreferencesForSong.Count() > 0)
                            {
                                ParentalControlManager.UpdateParentalControlForSongByShowID(CheckShowIDForParentalControlPreferencesForSong);
                            }
                        }
                    }
                    AppSettings.LastDownloadIdHid = LastDownloadIdHid;
                }
                else
                {
                    AppSettings.IsNewVersion = false;
                    AppSettings.LastDownloadIdHid = 0;
                    AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadParentalControlPreferences;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in RestoreParentalControlPreferences Method In PersonalizationDataSync.cs file", ex);
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadParentalControlPreferences;
            }
        }

        public void Restorerating()
        {
            //string lastDownloadedRatingDate = string.Empty;
            //int lastDownloadedRatingId = 0;
            //try
            //{
            //    ObservableCollection<Rating> objRatLists = ServiceManager.Restorerating(AppSettings.lastDownloadedRatingDate, AppSettings.ProjectName, AppSettings.lastDownloadedRatingId.ToString());
            //    if (objRatLists.Count > 0)
            //    {
            //        foreach (Rating objrat in objRatLists)
            //        {
            //           ShowList getShow = RatingManager.GetShowIdForNextUpdatedDateForRating(objrat);
            //            if (getShow!=null)
            //            {
            //                lastDownloadedRatingId = RatingManager.UpdateRatingForShows(lastDownloadedRatingId, objrat, getShow);
            //            }
            //        }
            //        AppSettings.lastDownloadedRatingId = lastDownloadedRatingId;
            //    }
            //    else
            //    {
            //        AppSettings.lastDownloadedRatingId = 0;
            //        DateTime dt = System.DateTime.Now;
            //        AppSettings.lastDownloadedRatingDate = dt.ToString("d");
            //        AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadRatingForVideos;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Exceptions.SaveOrSendExceptions("Exception in Restorerating Method In PersonalizationDataSync.cs file", ex);
            //}


            try
            {
                UpdateRatingInBlog blograt = new UpdateRatingInBlog();
                blograt.downloadrating(AppSettings.lastDownloadedRatingDate, "show");


                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadRatingForVideos;
                Common.SyncAgentState.ResetEvent();
            }
            catch (Exception ex)
            {

                HandleException(ex, "Exception in Restorerating Method In PersonalizationDataSync file");
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadRatingForVideos;
            }
        }

        public void Restoreratingvideos()
        {
            //string LastDownloadedRatingDate = string.Empty;
            //int LastDownloadedRatingId = 0;
            //try
            //{
            //    ObservableCollection<Rating> objRatingList = ServiceManager.Restoreratingvideos(AppSettings.lastVideoDownloadedRatingDate, ResourceHelper.ProjectName, AppSettings.lastvideoDownloadedRatingId.ToString());
            //    if (objRatingList.Count > 0)
            //    {
            //        foreach (Rating objrate in objRatingList)
            //        {
            //            LastDownloadedRatingId = objrate._Id;
            //            RatingManager.UpdateRatingForVideos(objrate);
            //        }
            //        AppSettings.lastvideoDownloadedRatingId = LastDownloadedRatingId;
            //    }
            //    else
            //    {
            //        AppSettings.lastvideoDownloadedRatingId = 0;
            //        AppSettings.lastVideoDownloadedRatingDate = DateTime.Now.ToString("d");
            //        AppSettings.DownloadCompletedDate = DateTime.Parse(DateTime.Now.ToString("d"));
            //        AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadPeople;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Exceptions.SaveOrSendExceptions("Exception in Restoreratingvideos Method In PersonalizationDataSync.cs file", ex);
            //}
            try
            {
                //UpdateRatingInBlog blograt = new UpdateRatingInBlog();
                //blograt.downloadrating(AppSettings.lastVideoDownloadedRatingDate, "links");
                //AppSettings.DownloadCompletedDate = DateTime.Parse(DateTime.Now.ToString("d"));
                //if (AppSettings.ProjectName == "Story Time" || AppSettings.ProjectName == "Vedic Library")
                //    AppSettings.BackgroundAgentStatus = SyncAgentStatus.RestoreStory;
                //else
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UpdateBlogUserNameandPassword;

                Common.SyncAgentState.ResetEvent();
            }
            catch (Exception ex)
            {

                HandleException(ex, "Exception in Restoreratingvideos Method In PersonalizationDataSync file");
                //if (AppSettings.ProjectName == "Story Time" || AppSettings.ProjectName == "Vedic Library")
                //    AppSettings.BackgroundAgentStatus = SyncAgentStatus.RestoreStory;
                //else
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UpdateBlogUserNameandPassword;
            }
        }


    }
}

