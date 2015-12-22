using System;
using System.Net;
using System.Windows;
using System.Xml.Linq;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading;
using System.Globalization;
using Common.Library;
//using OnlineVideos.Common;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace OnlineVideos.Library
{
    public class PersonalizationDataSync
    {
        //OnlineVideoServiceClient client = new OnlineVideoServiceClient();

        private async static void HandleException(Exception ex, string message)
        {
            Exceptions.SaveOrSendExceptions(message + "\n\n" + ex.Message + " \n\n Stack Trace \n" + ex.StackTrace, ex);

            if (OnlineVideos.Common.SyncAgentState.SyncAgent != null)
            {
                await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                {
                    ((OnlineVideos.Common.IDowloadCompleteCallback)(OnlineVideos.Common.SyncAgentState.SyncAgent)).OnStartEvent();
                });
            }
        }

        //public void DownloadFavorites()
        //{
        //    if (AppSettings.IsNewVersion)
        //    {
        //        RestoreFavourites();
        //    }
        //}
       
        public void DeleteIsolatedFoldersAndFiles()
        {
            if (AppSettings.IsNewVersion)
            {
               Storage.DeleteIsolatedFoldersAndFilesFile();
               //await Storage.DeleteIsolatedFoldersAndFilesFile();
            }
        }

        public void DownloadParentalPreferences()
        {
            if (AppSettings.IsNewVersion)
            {
               // RestoreParentalControlPreferences();

                //Added to Continue next process delete it when RestoreParentalControlPreferences() method is workinh
                AppSettings.IsNewVersion = false;
                AppSettings.LastDownloadIdHid = 0;
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadParentalControlPreferences;
            }
            else
            {
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadParentalControlPreferences;
            }
        }

        //public void UploadFavourites()
        //{
        //    try
        //    {

        //        if (FavoritesManager.GetCountForLastUpdatedFavouritesForVideos().Count() > 0 || FavoritesManager.GetCountForLastUpdatedFavouritesForLinks().Count() > 0)
        //        {
        //            BackupFavourites();
        //        }
        //        else
        //        {
        //            AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadParentalControlPreferences;
        //        }
        //        Exceptions.UpdateAgentLog("UploadFavourites Completed In PersonalizationDataSync");
        //    }
        //    catch (Exception ex)
        //    {
        //        Exceptions.SaveOrSendExceptions("Exception in UploadFavourites Method In Vidoes file", ex);
        //    }
        //}

        public async void UploadSearch()
        {
            try
            {
                List<SearchHistory> GetShowIdForSearchHistory = SearchHistoryManager.GetShowIDForSearchHistory();

                if (GetShowIdForSearchHistory.Count() > 0 && OnlineVideos.Common.SyncAgentState.SyncAgent != null)
                {
                    await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                    {
                        //getsavesearch();
                    });
                    OnlineVideos.Common.SyncAgentState.auto.WaitOne();
                }
                else if (GetShowIdForSearchHistory.Count() > 0 && OnlineVideos.Common.SyncAgentState.SyncAgent == null)
                {
                    //getsavesearch();
                }
                else
                {
                    AppSettings.SearchLastUploadedId = 0;
                    OnlineVideos.Common.SyncAgentState.ResetEvent();
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

        public async void UploadParentalPreferences()
        {
            try
            {
                //if ((ParentalControlManager.GetLastUpdatedVideoCountForParentalControl().Count() > 0 || ParentalControlManager.GetLastUpdatedLinkCountForParentalControl().Count() > 0) && OnlineVideos.Common.SyncAgentState.SyncAgent != null)
                //{
                //    await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                //       {
                //           //BackupHide();
                //       });
                //    OnlineVideos.Common.SyncAgentState.auto.WaitOne();
                //}

                //else if ((ParentalControlManager.GetLastUpdatedVideoCountForParentalControl().Count() > 0 || ParentalControlManager.GetLastUpdatedLinkCountForParentalControl().Count() > 0) && OnlineVideos.Common.SyncAgentState.SyncAgent == null)
                //{
                //    //BackupHide();
                //}

                //else
                //{
                //    AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadPeople;
                //}
                //Exceptions.UpdateAgentLog("UploadParentalPreferences Completed In PersonalizationDataSync");



                //This is also added for continue the next process you can delete it by rectify errors form above code
                DateTime dt = DateTime.Now;
                AppSettings.LastUploadedShowIdHid = 0;
                AppSettings.LastUploadedVideoIdHid = 0;
                AppSettings.LastUpdatedDateForHide = dt.ToString("d");
                OnlineVideos.Common.SyncAgentState.ResetEvent();
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadPeople;


            }

            catch (Exception ex)
            {
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadPeople;
                Exceptions.SaveOrSendExceptions("Exception in UploadParentalPreferences Method In Vidoes file", ex);
            }
        }

        public void DownloadRatingForShows()
        {

            try
            {
                if (OnlineVideos.Common.SyncAgentState.SyncAgent != null)
                {
                    //Deployment.Current.Dispatcher.BeginInvoke(() =>
                    //   {
                    Restorerating();
                    //});

                    OnlineVideos.Common.SyncAgentState.auto.WaitOne();
                }
                if (OnlineVideos.Common.SyncAgentState.SyncAgent == null)
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
        public async void GetBlogDetails()
        {
            try
            {
                if (OnlineVideos.Common.SyncAgentState.SyncAgent != null)
                {
                    await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                      {
                          //ServiceManager.GetBlogDetails("wp7", ResourceHelper.ProjectName, client_GetUsernameandPasswordCompleted);

                      });
                    OnlineVideos.Common.SyncAgentState.auto.WaitOne();
                }
                else 
                {
                    //ServiceManager.GetBlogDetails("wp7", ResourceHelper.ProjectName, client_GetUsernameandPasswordCompleted);
                }
               
            }
            catch (Exception ex)
            {

                if (AppSettings.ProjectName == "Story Time" || AppSettings.ProjectName == "Vedic Library")
                    AppSettings.BackgroundAgentStatus = SyncAgentStatus.RestoreStory;
                else
                    AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadFavourites;

            }
        }
        //void client_GetUsernameandPasswordCompleted(object sender, GetUsernameandPasswordCompletedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Result != null)
        //        {
        //            ObservableCollection<BlogInfo> info = e.Result as ObservableCollection<BlogInfo>;
        //            foreach (BlogInfo binfo in info)
        //            {
        //                string blogtype = binfo.BlogType.ToLower();
        //                BlogCategoryTable BlogCategoryTable = Constants.connection.Table<BlogCategoryTable>().Where(i => i.BlogType == blogtype).FirstOrDefaultAsync().Result;
        //                BlogCategoryTable.BlogUserName = binfo.UserName;
        //                BlogCategoryTable.BlogPassword = binfo.Password;
        //                int ss = Task.Run(async () => await Constants.connection.UpdateAsync(BlogCategoryTable)).Result;
        //            }
                  
        //            AppSettings.DownloadCompletedDate = DateTime.Parse(DateTime.Now.ToString("d"));
        //            if (AppSettings.ProjectName == "Story Time" || AppSettings.ProjectName == "Vedic Library")
        //                AppSettings.BackgroundAgentStatus = SyncAgentStatus.RestoreStory;
        //            else
        //                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadFavourites;
        //        }
        //        OnlineVideos.Common.SyncAgentState.ResetEvent();
        //    }
        //    catch (Exception ex)
        //    {
                
        //          if (AppSettings.ProjectName == "Story Time" || AppSettings.ProjectName == "Vedic Library")
        //            AppSettings.BackgroundAgentStatus = SyncAgentStatus.RestoreStory;
        //        else
        //            AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadFavourites;
        //    }
        //}
        public void DownloadRatingForVideos()
        {
            try
            {
                if (OnlineVideos.Common.SyncAgentState.SyncAgent != null)
                {
                    //Deployment.Current.Dispatcher.BeginInvoke(() =>
                    //{
                    Restoreratingvideos();
                    //});
                    OnlineVideos.Common.SyncAgentState.auto.WaitOne();
                }

                if (OnlineVideos.Common.SyncAgentState.SyncAgent == null)
                {
                    Restoreratingvideos();
                }
                Exceptions.UpdateAgentLog("DownloadRatingForVideos Completed In PersonalizationDataSync");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in DownloadRatingForVideos Method In Vidoes file", ex);
                //if (ResourceHelper.ProjectName == "Story Time" || ResourceHelper.ProjectName == "Vedic Library")
                //    AppSettings.BackgroundAgentStatus = SyncAgentStatus.RestoreStory;
                //else
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UpdateBlogUserNameandPassword;
            }
        }

        public void UploadRatingForShows()
        {
            try
            {
                List<ShowList> GetShowsForRating = RatingManager.GetShowsForRating();
                if (OnlineVideos.Common.SyncAgentState.SyncAgent != null && GetShowsForRating.Count() > 0)
                {
                    //Deployment.Current.Dispatcher.BeginInvoke(() =>
                    //{
                    getratingvalues();
                    //});
                    OnlineVideos.Common.SyncAgentState.auto.WaitOne();
                }

                else if (OnlineVideos.Common.SyncAgentState.SyncAgent == null && GetShowsForRating.Count() > 0)
                {
                    getratingvalues();
                }
                else
                {
                    AppSettings.LastUploadRatingId = 0;

                    OnlineVideos.Common.SyncAgentState.ResetEvent();
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



        public void UploadRatingForVideos()
        {
            try
            {
                List<ShowLinks> GetShowsForVideoRatingUpload = RatingManager.GetShowForVideoRatingUpload();
                if (OnlineVideos.Common.SyncAgentState.SyncAgent != null && GetShowsForVideoRatingUpload.Count() > 0)
                {
                    //Deployment.Current.Dispatcher.BeginInvoke(() =>
                    //{
                    getratingvaluesforvideos();
                    //});
                    OnlineVideos.Common.SyncAgentState.auto.WaitOne();

                }
                else if (OnlineVideos.Common.SyncAgentState.SyncAgent == null && GetShowsForVideoRatingUpload.Count() > 0)
                {
                    getratingvaluesforvideos();
                }
                else
                {
                    AppSettings.LastUploadVideoRatingid = 0;

                    OnlineVideos.Common.SyncAgentState.ResetEvent();
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
                        if ((ResourceHelper.AppName == Apps.Online_Education.ToString() || ResourceHelper.AppName == Apps.DrivingTest.ToString()) && ss.LinkType == "Quiz")
                            blograt.getshow(ss.ShowID.ToString() + "-" + ss.LinkID.ToString(), ss.Rating, "Quiz");
                        else
                            blograt.getshow(ss.ShowID.ToString() + "-" + ss.LinkID.ToString(), ss.Rating, "links");
                        RatingManager.UpdateRatingForVideos(ss.ShowID, ss.LinkID);
                    }
                    OnlineVideos.Common.SyncAgentState.ResetEvent();
                    //ServiceManager.GetRatingvideos(fav, client_GetRatingvideosCompleted);
                }
                else
                {
                    AppSettings.LastUploadVideoRatingid = 0;

                    OnlineVideos.Common.SyncAgentState.ResetEvent();
                }
                Exceptions.UpdateAgentLog("getratingvaluesforvideos Completed In PersonalizationDataSync");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in getratingvaluesforvideos Method In Vidoes file", ex);
            }
        }

        //void client_GetRatingvideosCompleted(object sender, GetRatingvideosCompletedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Result != null)
        //        {
        //            ObservableCollection<OnlineVideos.Services.OnlineVideoService.Favorites> fav = e.Result;
        //            if (fav.Count > 0)
        //            {
        //                foreach (OnlineVideos.Services.OnlineVideoService.Favorites f in fav)
        //                {
        //                    RatingManager.UpdateRatingForVideos(f);
        //                }
        //            }
        //        }
        //        SyncAgentState.ResetEvent();
        //        Exceptions.UpdateAgentLog("client_GetRatingvideosCompleted Completed In PersonalizationDataSync");

        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex, "Exception in client_GetRatingvideosCompleted Method In Vidoes file");
        //    }
        //}

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
                    OnlineVideos.Common.SyncAgentState.ResetEvent();
                    //ServiceManager.GetRating(fav, client_GetRatingCompleted);
                }
                else
                {
                    AppSettings.LastUploadRatingId = 0;
                    OnlineVideos.Common.SyncAgentState.ResetEvent();
                }
                Exceptions.UpdateAgentLog("getratingvalues Completed In PersonalizationDataSync");
            }
            catch (Exception ex)
            {
                //RatingManager.updateRating(AppSettings.LastUploadRatingId);
                //AppSettings.LastUploadRatingId = 0;
                Exceptions.SaveOrSendExceptions("Exception in getratingvalues Method In Vidoes file", ex);
            }
        }

        //void client_getRatingnextupdateddateCompleted(object sender, getRatingnextupdateddateCompletedEventArgs e)
        //{
        //    string lastDownloadedRatingDate = string.Empty;
        //    int lastDownloadedRatingId = 0;
        //    try
        //    {
        //        if (e.Result != null)
        //        {
        //            ObservableCollection<Rating> objRatLists = e.Result as ObservableCollection<Rating>;
        //            if (objRatLists.Count > 0)
        //            {
        //                foreach (Rating objrat in objRatLists)
        //                {
        //                    ShowList getShow = RatingManager.GetShowIdForNextUpdatedDateForRating(objrat);
        //                    if (getShow!=null)
        //                    {
        //                        lastDownloadedRatingId = RatingManager.UpdateRatingForShows(lastDownloadedRatingId, objrat, getShow);
        //                    }
        //                }
        //                AppSettings.lastDownloadedRatingId = lastDownloadedRatingId;
        //            }
        //            else
        //            {
        //                AppSettings.lastDownloadedRatingId = 0;
        //                DateTime dt = System.DateTime.Now;
        //                AppSettings.lastDownloadedRatingDate = dt.ToString("d");
        //                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadRatingForVideos;
        //            }
        //        }
        //        SyncAgentState.ResetEvent();
        //        Exceptions.UpdateAgentLog("client_getRatingnextupdateddateCompleted Completed In PersonalizationDataSync");
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex, "Exception in client_getRatingnextupdateddateCompleted Method In Vidoes file");
        //        AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadRatingForVideos;
        //    }
        //}

        //void client_GetRatingCompleted(object sender, GetRatingCompletedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Result != null)
        //        {
        //            ObservableCollection<Favorites> f = e.Result as ObservableCollection<Favorites>;
        //            foreach (Favorites fav in f)
        //            {
        //                if (fav._status >= 1)
        //                {
        //                    RatingManager.updateRating(fav);
        //                }
        //            }
        //        }
        //        SyncAgentState.ResetEvent();
        //        Exceptions.UpdateAgentLog("client_GetRatingCompleted Completed In PersonalizationDataSync");
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex, "Exception in client_GetRatingCompleted Method In Vidoes file");
        //    }
        //}

        //private void BackupHide()
        //{
        //    int LastUploadedShowIdHid = 0;
        //    int LastUploadedVideoIdHid = 0;
        //    string LastUpdatedDateForHide = string.Empty;
        //    try
        //    {
        //        List<ShowList> GetVideo = null;
        //        List<ShowLinks> getLink = null;
        //        string deviceid = BitConverter.ToString(DeviceHelper.GetDeviceUniqueID());
        //        ObservableCollection<Favorites> fav = new ObservableCollection<Favorites>();
        //        GetVideo = ParentalControlManager.GetLastUpdatedShowForParentalControl();
        //        EntityHelper.CreateShowListForBackupHide(LastUploadedShowIdHid, GetVideo, deviceid, fav);
        //        getLink = ParentalControlManager.GetLastUpdatedLinkForParentalControl();
        //        EntityHelper.CreateSongListForBackupHide(LastUploadedVideoIdHid, getLink, deviceid, fav);
        //        if (fav.Count > 0)
        //        {
        //            ServiceManager.InsertParentalControlData(fav);
        //        }
        //        else
        //        {
        //            DateTime dt = DateTime.Now;
        //            AppSettings.LastUploadedShowIdHid = 0;
        //            AppSettings.LastUploadedVideoIdHid = 0;
        //            AppSettings.LastUpdatedDateForHide = dt.ToString("d");
        //            SyncAgentState.ResetEvent();
        //            AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadPeople;
        //        }
        //        Exceptions.UpdateAgentLog("BackupHide Completed In PersonalizationDataSync");
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex, "Exception in BackupHide Method In PersonalizationDataSync file");
        //        AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadPeople;
        //    }
        //}

        //private void BackupFavourites()
        //{
        //    int LastUploadedShowIdFav = 0;
        //    int LastUploadedVideoIdFav = 0;
        //    string LastUpdatedDateforFavourites = string.Empty;
        //    try
        //    {
        //        List<ShowList> selectShow = null;
        //        List<ShowLinks> selectLink = null;
        //        string deviceid = BitConverter.ToString(DeviceHelper.GetDeviceUniqueID());
        //        ObservableCollection<Favorites> fav = new ObservableCollection<Favorites>();
        //        selectShow = FavoritesManager.GetLastUpdatedFavouriteShows();
        //        EntityHelper.CreateShowListForBackupFavourites(LastUploadedShowIdFav, selectShow, deviceid, fav);
        //        selectLink = FavoritesManager.GetLastUpdatedFavouriteLinks();
        //        EntityHelper.CreateSongListForBackupFavourites(LastUploadedVideoIdFav, selectLink, deviceid, fav);
        //        if (fav.Count > 0)
        //        {
        //            ServiceManager.InsertUserData(fav);
        //        }
        //        else
        //        {
        //            DateTime dt = DateTime.Now;
        //            AppSettings.LastUploadedShowIdFav = 0;
        //            AppSettings.LastUploadedVideoIdFav = 0;
        //            AppSettings.LastUpdatedDateforFavourites = dt;
        //            SyncAgentState.ResetEvent();
        //        }
        //        Exceptions.UpdateAgentLog("BackupFavourites Completed In PersonalizationDataSync");
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex, "Exception in BackupFavourites Method In Vidoes file");
        //    }
        //}

        //public void getsavesearch()
        //{
        //    string SearchLastUploadedId = string.Empty;
        //    try
        //    {
        //        ObservableCollection<Favorites> searchhis = new ObservableCollection<Favorites>();
        //        searchhis = SearchHistoryManager.GetSearchHistory(SearchLastUploadedId, searchhis);
        //        if (searchhis.Count > 0)
        //        {
        //            ServiceManager.GetSearchHistory(searchhis, client_GetSearchHistoryCompleted);
        //        }
        //        else
        //        {
        //            AppSettings.SearchLastUploadedId = 0;
        //            SyncAgentState.ResetEvent();
        //        }
        //        Exceptions.UpdateAgentLog("getsavesearch Completed In PersonalizationDataSync");
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex, "Exception in getsavesearch Method In Vidoes file");
        //    }
        //}

        //void client_GetSearchHistoryCompleted(object sender, GetSearchHistoryCompletedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Result != null)
        //        {
        //            ObservableCollection<Favorites> saved = e.Result as ObservableCollection<Favorites>;
        //            foreach (Favorites f in saved)
        //            {
        //                if (f._Hstatus >= 1)
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

        //public void RestoreFavourites()
        //{
        //    ServiceManager.RetrieveuserDataAsync(DeviceHelper.GetDeviceUniqueID(), ResourceHelper.ProjectName, AppSettings.LastDownloadIdFav.ToString(), client_RetrieveuserDataCompleted);
        //    SyncAgentState.auto.WaitOne();
        //}

        //void client_RetrieveuserDataCompleted(object sender, RetrieveuserDataCompletedEventArgs e)
        //{
        //    int LastDownloadIdFav = 0;
        //    try
        //    {
        //        string id = "";
        //        string linktype = "";
        //        if (e.Result != null)
        //        {
        //            ObservableCollection<Favorites> objfavLists = e.Result;
        //            if (objfavLists.Count > 0)
        //            {
        //                foreach (Favorites objfavs in objfavLists)
        //                {
        //                    LastDownloadIdFav = objfavs._Id;
        //                    id = (objfavs._movieId).ToString();
        //                    linktype = (objfavs._LinkType);
        //                    if (linktype == "Movies")
        //                    {
        //                        List<ShowList> CheckShowIDForFavouritesDownload = FavoritesManager.CheckShowIDForFavouritesDownload(id);
        //                        if (CheckShowIDForFavouritesDownload.Count() > 0)
        //                        {
        //                            FavoritesManager.UpdateFavouritesByShowID(CheckShowIDForFavouritesDownload);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        List<ShowLinks> CheckShowIDForFavouriteSongDownload = FavoritesManager.CheckShowIDForFavouriteSongDownload(id, objfavs._songno);
        //                        if (CheckShowIDForFavouriteSongDownload.Count() > 0)
        //                        {
        //                            FavoritesManager.UpdateFavouritesForSongByShowID(CheckShowIDForFavouriteSongDownload);
        //                        }
        //                    }
        //                }
        //                AppSettings.LastDownloadIdFav = LastDownloadIdFav;
        //            }
        //            else
        //            {
        //                AppSettings.LastDownloadIdFav = 0;
        //                AppSettings.BackgroundAgentStatus= SyncAgentStatus.DownloadParentalControlPreferences;
        //            }
        //        }
        //        SyncAgentState.ResetEvent();
        //        Exceptions.UpdateAgentLog("client_RetrieveuserDataCompleted Completed in PersonalizationDataSync");
        //    }
        //    catch (Exception ex)
        //    {
        //        AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadParentalControlPreferences;
        //        HandleException(ex, "Exception in client_RetrieveuserDataCompleted Method In Vidoes file");
        //        SyncAgentState.ResetEvent();
        //    }
        //}

        public void RestoreParentalControlPreferences()
        {
            //try
            //{
            //    ServiceManager.RestoreParentalControlPreferences(DeviceHelper.GetDeviceUniqueID(), ResourceHelper.ProjectName, AppSettings.LastDownloadIdHid.ToString(), client_RetrieveuserData1Completed);
            //    SyncAgentState.auto.WaitOne();
            //}
            //catch (Exception ex)
            //{
            //    AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadParentalControlPreferences;
            //}
        }

        //void client_RetrieveuserData1Completed(object sender, RetrieveParentalControlDataCompletedEventArgs e)
        //{
        //    int LastDownloadIdHid = 0;
        //    try
        //    {
        //        string id = "";
        //        string linktype = "";
        //        if (e.Result != null)
        //        {
        //            ObservableCollection<Favorites> objfavLists = e.Result as ObservableCollection<Favorites>;
        //            if (objfavLists.Count > 0)
        //            {
        //                foreach (Favorites objfavs in objfavLists)
        //                {
        //                    id = (objfavs._movieId).ToString();
        //                    linktype = (objfavs._LinkType);
        //                    LastDownloadIdHid = objfavs._Id;
        //                    if (linktype == "Movies")
        //                    {
        //                        List<ShowList> CheckShowIDForParentalControlPreferences = ParentalControlManager.CheckShowIDForParentalControlPreferences(id);
        //                        if (CheckShowIDForParentalControlPreferences.Count() > 0)
        //                        {
        //                            ParentalControlManager.UpdateParentalControlPreferencesByShowID(CheckShowIDForParentalControlPreferences);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        List<ShowLinks> CheckShowIDForParentalControlPreferencesForSong = ParentalControlManager.CheckShowIDForParentalControlPreferencesForSong(id, objfavs._songno);
        //                        if (CheckShowIDForParentalControlPreferencesForSong.Count() > 0)
        //                        {
        //                            ParentalControlManager.UpdateParentalControlForSongByShowID(CheckShowIDForParentalControlPreferencesForSong);
        //                        }
        //                    }
        //                }
        //                AppSettings.LastDownloadIdHid = LastDownloadIdHid;
        //            }
        //            else
        //            {
        //                AppSettings.IsNewVersion = false;
        //                AppSettings.LastDownloadIdHid = 0;
        //                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadParentalControlPreferences;
        //            }
        //        }
        //        OnlineVideos.Common.SyncAgentState.auto.Set();
        //        Exceptions.UpdateAgentLog("client_RetrieveuserDataCompleted Completed in PersonalizationDataSync");
        //    }
        //    catch (Exception ex)
        //    {
        //        AppSettings.LastDownloadIdHid = LastDownloadIdHid;
        //        HandleException(ex, "Exception in client_RetrieveuserData1Completed Method In Vidoes file");
        //    }
        //}

        public void Restorerating()
        {
            try
            {
                UpdateRatingInBlog blograt = new UpdateRatingInBlog();
                blograt.downloadrating(AppSettings.lastDownloadedRatingDate, "show");
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadRatingForVideos;
                OnlineVideos.Common.SyncAgentState.ResetEvent();
            }
            catch (Exception ex)
            {

                HandleException(ex, "Exception in Restorerating Method In PersonalizationDataSync file");
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadRatingForVideos;
            }
            //ServiceManager.Restorerating(AppSettings.lastDownloadedRatingDate, ResourceHelper.ProjectName, AppSettings.lastDownloadedRatingId.ToString(), client_getRatingnextupdateddateCompleted);
        }

        public void Restoreratingvideos()
        {
            //ServiceManager.Restoreratingvideos(AppSettings.lastVideoDownloadedRatingDate, ResourceHelper.ProjectName, AppSettings.lastvideoDownloadedRatingId.ToString(), client_getRatingVideosnextupdateddateCompleted);
            try
            {
                UpdateRatingInBlog blograt = new UpdateRatingInBlog();
                if (ResourceHelper.AppName == Apps.Online_Education.ToString() || ResourceHelper.AppName == Apps.DrivingTest.ToString())
                {
                    blograt.downloadrating(AppSettings.lastVideoDownloadedRatingDate, "links");
                    blograt.downloadrating(AppSettings.lastVideoDownloadedRatingDate, "Quiz");
                }
                else
                    blograt.downloadrating(AppSettings.lastVideoDownloadedRatingDate, "links");
                //AppSettings.DownloadCompletedDate = DateTime.Parse(DateTime.Now.ToString("d"));
                //if (ResourceHelper.ProjectName == "Story Time" || ResourceHelper.ProjectName == "Vedic Library")
                //    AppSettings.BackgroundAgentStatus = SyncAgentStatus.RestoreStory;
                //else
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UpdateBlogUserNameandPassword;

                OnlineVideos.Common.SyncAgentState.ResetEvent();
            }
            catch (Exception ex)
            {

                HandleException(ex, "Exception in Restoreratingvideos Method In PersonalizationDataSync file");
                //if (ResourceHelper.ProjectName == "Story Time" || ResourceHelper.ProjectName == "Vedic Library")
                //    AppSettings.BackgroundAgentStatus = SyncAgentStatus.RestoreStory;
                //else
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UpdateBlogUserNameandPassword;
            }
        }

        //void client_getRatingVideosnextupdateddateCompleted(object sender, getRatingVideosnextupdateddateCompletedEventArgs e)
        //{
        //    string LastDownloadedRatingDate = string.Empty;
        //    int LastDownloadedRatingId = 0;

        //    try
        //    {
        //        if (e.Result != null)
        //        {
        //            ObservableCollection<Rating> objRatingList = e.Result as ObservableCollection<Rating>;
        //            if (objRatingList.Count > 0)
        //            {
        //                foreach (Rating objrate in objRatingList)
        //                {
        //                    LastDownloadedRatingId = objrate._Id;
        //                    RatingManager.UpdateRatingForVideos(objrate);
        //                }
        //                AppSettings.lastvideoDownloadedRatingId = LastDownloadedRatingId;
        //            }
        //            else
        //            {
        //                AppSettings.lastvideoDownloadedRatingId = 0;
        //                AppSettings.lastVideoDownloadedRatingDate = DateTime.Now.ToString("d");
        //                AppSettings.DownloadCompletedDate= DateTime.Parse(DateTime.Now.ToString("d"));
        //                if (ResourceHelper.ProjectName == "Story Time" || ResourceHelper.ProjectName == "Vedic Library")
        //                    AppSettings.BackgroundAgentStatus = SyncAgentStatus.RestoreStory;
        //                else
        //                    AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadFavourites;
        //            }
        //        }

        //        SyncAgentState.ResetEvent();
        //        Exceptions.UpdateAgentLog("client_getRatingVideosnextupdateddateCompleted Completed In PersonalizationDataSync");

        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex, "Exception in client_getRatingVideosnextupdateddateCompleted Method In Vidoes file");
        //        if (ResourceHelper.ProjectName == "Story Time" || ResourceHelper.ProjectName == "Vedic Library")
        //            AppSettings.BackgroundAgentStatus = SyncAgentStatus.RestoreStory;
        //        else
        //            AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadFavourites;
        //    }
        //}
    }
}

