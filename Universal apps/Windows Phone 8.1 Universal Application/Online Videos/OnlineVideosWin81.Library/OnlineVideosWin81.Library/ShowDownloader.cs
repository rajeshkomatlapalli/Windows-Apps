using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using Common.Library;
using Common;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using PicasaMobileInterface;

namespace OnlineVideos.Library
{
    public static class ShowDownloader
    {
        public static void StartBackgroundDownload(BackgroundHelper helper, Page dispatcher)
        {
            try
            {
                Task.Run(async () => await dispatcher.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                   {
                       helper.StopScheduledTasks(0);
                       try
                       {
                           string uploadshowrating = AppResources.uploadshowrating;
                           Constants.Uploadshowrating = AppResources.uploadshowrating;
                           string uploadlinkrating = AppResources.uploadlinkrating;
                           Constants.UploadLinksrating = AppResources.uploadlinkrating;
                           string username = AppResources.username;
                           Constants.username = AppResources.username;
                           string password = AppResources.password;
                           Constants.password = AppResources.password;
                           if (ResourceHelper.AppName == Apps.Online_Education.ToString() || ResourceHelper.AppName == Apps.Driving_Exam.ToString())
                           {
                               string uploadquizrating = AppResources.uploadquizrating;
                               Constants.Uploadquizrating = AppResources.uploadquizrating;
                           }
                       }
                       catch (Exception ex)
                       {
                           string excepp = ex.Message;
                       }
                   }));
                if (ValidationHelper.IsDownloadCompletedToday())
                {
                    PersonalizationDataSync personalization = new PersonalizationDataSync();

                    switch (AppSettings.BackgroundAgentStatus)
                    {
                        case SyncAgentStatus.DownloadParentalControlPreferences:
                            try
                            {
                                personalization.DownloadParentalPreferences();
                            }
                            catch
                            {
                            }
                            break;
                        case SyncAgentStatus.UploadParentalControlPreferences:
                            try
                            {
                                personalization.UploadParentalPreferences();                               
                            }
                            catch
                            {
                            }
                            break;
                        case SyncAgentStatus.DownloadPeople:
                            if (AppSettings.Downloadpeople)
                                MovieUpdates.checkForPeopleUpdates();
                            else
                                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadShows;
                            break;
                        case SyncAgentStatus.UpdatePeople:
                            if (Task.Run(async () => await Storage.FileExists("Peoples.xml")).Result)
                                MovieUpdates.GetPeopleCompleted("Peoples.xml");
                            else
                                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadShows;
                            break;
                        case SyncAgentStatus.DownloadShows:
                            if (AppSettings.Downloadshows)
                            {
                                MovieUpdates.checkForShowUpdates();
                            }
                            else
                                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UpDateLiveTile;
                            break;
                        case SyncAgentStatus.UpdateShows:
                            if (Task.Run(async () => await Storage.FileExists("Movies.xml")).Result)
                            {
                                MovieUpdates.DownloadNextShowWithTopVideosAndAppCountCompleted("Movies.xml");
                            }
                            else
                                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UpDateLiveTile;
                            break;
                        //case SyncAgentStatus.UploadFavourites:
                        //    personalization.UploadFavourites();
                        //    break;
                        case SyncAgentStatus.UpDateLiveTile:
                            //SendLocalImageTile Sendlocalimagetile = new SendLocalImageTile();
                            Constants.LiveTileBackgroundAgentStatus = true;
                            //Sendlocalimagetile.UpdateTileWithImage();
                            LiveTileUpDate.LiveTileForCycle();                            
                            break;
                        case SyncAgentStatus.UploadSearch:
                            personalization.UploadSearch();
                            break;
                        case SyncAgentStatus.UploadRatingForShows:
                            AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadRatingForShows;
                            //string username = Constants.username;
                            //string password = Constants.password;
                            //BloggerInterface BI = new BloggerInterface(username, password);
                            //personalization.UploadRatingForShows();
                            break;
                        case SyncAgentStatus.UploadRatingForVideos:
                            //personalization.UploadRatingForVideos();
                            break;
                        case SyncAgentStatus.DownloadRatingForShows:
                            personalization.DownloadRatingForShows();
                            break;
                        case SyncAgentStatus.DownloadRatingForVideos:
                            personalization.DownloadRatingForVideos();
                            break;
                        case SyncAgentStatus.UpdateBlogUserNameandPassword:
                            personalization.GetBlogDetails();
                            AppSettings.StopTimer = "True";
                            break;
                    }
                    if (Common.SyncAgentState.SyncAgent != null)
                        ((Common.IDowloadCompleteCallback)Common.SyncAgentState.SyncAgent).OnStartEvent();
                    Task.Run(async () => await dispatcher.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            helper.StartScheduledTasks();
                        }));
                }
                else
                {
                    Task.Run(async () => await dispatcher.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        helper.StopScheduledTasks(0);
                    }));
                    if (Common.SyncAgentState.SyncAgent != null)
                        ((Common.IDowloadCompleteCallback)Common.SyncAgentState.SyncAgent).OnStartEvent();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Date", DateTime.Now);
                Exceptions.SaveOrSendExceptions("Failed to download movie updates - {0}", ex);
            }
        }
    }
}