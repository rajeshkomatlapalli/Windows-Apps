using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
//using OnlineVideos.Common;
using Common.Library;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using PicasaMobileInterface;
using Windows.UI.Xaml.Controls;

namespace OnlineVideos.Library
{
    public static class ShowDownloader
    {
        static DispatcherTimer downloadtimer = default(DispatcherTimer);
        public async static void StartBackgroundDownload(DispatcherTimer timer)
        {
            try
            {
                if (downloadtimer != default(DispatcherTimer))
                    downloadtimer = timer;
                //Wait for ten seconds before to start downloading shows.

                if (NetworkHelper.IsNetworkAvailableForDownloads())
                {
                    try
                    {
                        if (ValidationHelper.IsDownloadCompletedToday())
                        {
                            try
                            {
                                if (timer != default(DispatcherTimer))
                                    timer.Stop();

                                PersonalizationDataSync personalization = new PersonalizationDataSync();
                                switch (AppSettings.BackgroundAgentStatus)
                                {

                                    case SyncAgentStatus.DownloadParentalControlPreferences:
                                        try
                                        {//not Working
                                            personalization.DownloadParentalPreferences();
                                        }
                                        catch
                                        {
                                        }
                                        break;
                                    case SyncAgentStatus.UploadParentalControlPreferences:
                                        try
                                        {//not Working
                                            personalization.UploadParentalPreferences();
                                        }
                                        catch
                                        {
                                        }
                                        break;
                                    case SyncAgentStatus.DownloadPeople:
                                        try
                                        {//Working

                                            if (AppSettings.Downloadpeople)
                                            {
                                                MovieUpdates.checkForPeopleUpdates();
                                            }
                                            else
                                                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadShows;
                                        }
                                        catch
                                        {

                                        }
                                        break;
                                    case SyncAgentStatus.UpdatePeople:
                                        try
                                        {//Working
                                            if (Task.Run(async () => await Storage.FileExists("Peoples.xml")).Result)
                                            {
                                                MovieUpdates.GetPeopleCompleted("Peoples.xml");
                                                //AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadRatingForShows;
                                            }
                                            else
                                                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadShows;
                                        }
                                        catch
                                        {

                                        }
                                        break;
                                    case SyncAgentStatus.DownloadShows:
                                        try
                                        {//Working                                            
                                            if (AppSettings.Downloadshows)
                                            {
                                                MovieUpdates.checkForShowUpdates();
                                            }
                                            else
                                                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UpDateLiveTile;
                                        }
                                        catch
                                        {

                                        }
                                        break;
                                    case SyncAgentStatus.UpdateShows:
                                        try
                                        {//Working
                                            if (Task.Run(async () => await Storage.FileExists("Movies.xml")).Result)
                                            {
                                                MovieUpdates.DownloadNextShowWithTopVideosAndAppCountCompleted("Movies.xml");
                                            }
                                            else
                                                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UpDateLiveTile;
                                        }
                                        catch
                                        {

                                        }
                                        break;
                                    case SyncAgentStatus.UpDateLiveTile:
                                        try
                                        {//Working                                          
                                            Constants.LiveTileBackgroundAgentStatus = true;
                                            LiveTileUpDate.LiveTileForCycle();
                                        }
                                        catch (Exception ex)
                                        {
                                            string mg = ex.Message;
                                        }
                                        break;
                                    case SyncAgentStatus.UploadSearch:
                                        try
                                        {//not working
                                            //personalization.UploadSearch();
                                            AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadRatingForShows;
                                        }
                                        catch
                                        {

                                        }
                                        break;
                                    case SyncAgentStatus.UploadRatingForShows:
                                        try
                                        {//not Working
                                            BloggerInterface_New BI = new BloggerInterface_New("uploadshowratings@gmail.com", "Lartshowratings", "win8showratings");
                                            personalization.UploadRatingForShows();
                                        }
                                        catch
                                        {

                                        }
                                        break;
                                    case SyncAgentStatus.UploadRatingForVideos:
                                        try
                                        {//not Working
                                            // BloggerInterface B1I = new BloggerInterface(AppSettings.RatingUserName, AppSettings.RatingPassword);
                                            personalization.UploadRatingForVideos();
                                        }
                                        catch
                                        {

                                        }
                                        break;
                                    case SyncAgentStatus.DownloadRatingForShows:
                                        try
                                        {//Working
                                            personalization.DownloadRatingForShows();
                                        }
                                        catch
                                        {

                                        }
                                        break;
                                    case SyncAgentStatus.DownloadRatingForVideos:
                                        try
                                        {//Working
                                            personalization.DownloadRatingForVideos();

                                        }
                                        catch
                                        {

                                        }
                                        break;
                                    case SyncAgentStatus.UpdateBlogUserNameandPassword:
                                        try
                                        {// not Working
                                            personalization.GetBlogDetails();
                                            AppSettings.StopTimer = "True";
                                        }
                                        catch(Exception ex)
                                        {
                                            Exceptions.SaveOrSendExceptions("Failed to download movie updates - {0}", ex);
                                        }
                                        break;
                                }
                                //await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                                //{
                                if (AppSettings.StopTimer != "True")
                                {
                                    if (timer != default(DispatcherTimer))
                                        timer.Start();
                                }
                                else
                                {
                                    timer.Stop();                                   
                                }

                                //});
                            }
                            catch (Exception ex)
                            {
                                ex.Data.Add("Date", DateTime.Now);
                                Exceptions.SaveOrSendExceptions("Failed to download movie updates - {0}", ex);
                            }

                        }
                        else
                        {
                            AppSettings.StopTimer = "True";
                            if (OnlineVideos.Common.SyncAgentState.SyncAgent != null)
                                ((OnlineVideos.Common.IDowloadCompleteCallback)OnlineVideos.Common.SyncAgentState.SyncAgent).OnStartEvent();
                        }

                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("Date", DateTime.Now);
                        Exceptions.SaveOrSendExceptions("Failed to download movie updates - {0}", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Date", DateTime.Now);
                Exceptions.SaveOrSendExceptions("Failed to download movie updates - {0}", ex);
            }
            finally
            {
                if (OnlineVideos.Common.SyncAgentState.SyncAgent != null && AppSettings.AppStatus != ApplicationStatus.Launching && AppSettings.AppStatus != ApplicationStatus.Active)
                {

                    ((OnlineVideos.Common.IDowloadCompleteCallback)OnlineVideos.Common.SyncAgentState.SyncAgent).OnStartEvent();
                }
            }
        }
    }
}

