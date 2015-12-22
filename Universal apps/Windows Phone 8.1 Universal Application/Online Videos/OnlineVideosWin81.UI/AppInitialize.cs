using System;
using System.Net;
using System.Windows;
using System.Linq;
using Common.Library;
using System.IO;
using Common;
using Windows.Storage;
using System.Threading.Tasks;
using OnlineVideos.UI;
//using Common.Common;

namespace OnlineVideos
{
    public class AppInitialize
    {
        CustomizationSettings objcustom = null;
        public AppInitialize()
        {
            objcustom = new CustomizationSettings();
        }

        public void CheckElementSection()
        {
            var applicationData = Windows.Storage.ApplicationData.Current;
            var settings = applicationData.LocalSettings;
            string version = String.Empty;

            version = DeviceHelp.GetVersion("Version").Result;
            //version = "1.0.0.0";           

            if (version != SettingsHelper.getStringValue("Version"))
            {
                 AppSettings.Downloadpeople = AppResources.DownloadPeople;
                 AppSettings.Downloadshows = AppResources.DownloadShow;
                 AppSettings.Primarypeoplefromblog = AppResources.PrimaryPeopleDownloadUri;
                 AppSettings.Primarypeoplefromblogwin81 = AppResources.PrimaryPeopleDownloadUriwin81;
                 AppSettings.Primaryshowsfromblog = AppResources.PrimaryShowsDownloadUri;
                AppSettings.Primaryshowsfromblogwin81 = AppResources.PrimaryShowsDownloadUriwin81;
                if (AppResources.visablesecondaryblog == true)
                  {
                    AppSettings.Secondarypeoplefromblog = AppResources.SecondaryPeopleDownloadUri;
                    AppSettings.Secondarypeoplefromblogwin81 = AppResources.SecondaryPeopleDownloadUriwin81;
                    AppSettings.Secondaryshowsfromblog = AppResources.SecondaryShowsDownloadUri;
                    AppSettings.Secondaryshowsfromblogwin81 = AppResources.SecondaryShowsDownloadUriwin81;
                }
                    AppSettings.BlogStatus = 0;
                    //..............................................................

                    //Uploading Parental control Data..............................

                    AppSettings.LastUploadedShowIdHid = 0;
                    AppSettings.LastUploadedVideoIdHid = 0;
                    AppSettings.LastUpdatedDateForHide = "25/11/2015";
                    //..............................................................

                    //Downloading Parental control Data..............................
                    AppSettings.LastDownloadIdHid = 0;
                    //...................................................................
                    AppSettings.LastShowInsertId = 0;

                    //Downloading Favourite Movies or Songs...............................
                    AppSettings.LastDownloadIdFav = 0;
                    //...................................................................

                    //Uploading Favourite Movies or Songs...............................
                    AppSettings.LastUploadedVideoIdFav = 0;
                    AppSettings.LastUploadedShowIdFav = 0;
                    AppSettings.LastUpdatedDateforFavourites = new DateTime(2015,11, 26);
                    //...................................................................

                    //Downloading Movies Rating..........................................
                    AppSettings.lastDownloadedRatingId = 0;
                    AppSettings.lastDownloadedRatingDate = new DateTime(2015, 11, 26);
                    //....................................................................

                    //Uploading Movies Rating..........................................
                    AppSettings.LastUploadRatingId = 0;
                    //....................................................................

                    //Downloading Movies Videos..........................................
                    AppSettings.lastvideoDownloadedRatingId = 0;
                    AppSettings.lastVideoDownloadedRatingDate = new DateTime(2015, 11, 26);
                    //....................................................................
                    AppSettings.LastPublishedDate = new DateTimeOffset(new DateTime(2015, 11, 26));
                    //People LastUpdated Date..............................................
                    AppSettings.PeopleLastUpdatedDate = new DateTimeOffset(new DateTime(2015, 11, 26));
                    //.....................................................................
                    AppSettings.LastPeoplePublishedDate = new DateTimeOffset(new DateTime(2015, 11, 26));
                    //Movies LastUpdated Date..............................................
                    AppSettings.LastUpdatedDate = new DateTimeOffset(new DateTime(2015, 11, 26));
                    //......................................................................

                    //Uploading Search History.............................................
                    AppSettings.SearchLastUploadedId = 0;
                    //......................................................................
                    AppSettings.StopTimer = "False";
                    //State for Switch case to download favourite for first time...............
                    if (AppSettings.ProjectName == "Story Time" || AppSettings.ProjectName == "Vedic Library")
                        AppSettings.BackgroundAgentStatus = SyncAgentStatus.RestoreStory;
                    else
                        AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadFavourites;
                    //..........................................................................

                    //Overall downloaded date used as condition to download data once per day....
                    AppSettings.DownloadCompletedDate = new DateTime(2015, 11, 26);
                    //..........................................................................

                    //Uploading Videos Rating..................................................
                    AppSettings.LastUploadVideoRatingid = 0;
                    //........................................................................

                    //Condition to know that app is new or updated..............................
                    AppSettings.IsNewVersion = true;
                    AppSettings.IsNewVersionForTables = true;
                    AppSettings.DownloadFavCompleted = false;
                    AppSettings.DownloadStoryCompleted = false;

                    //........................................................................
                    AppSettings.BackGroundAgentRegistered = false;
                    AppSettings.IsRingtone = false;
                    StorageFolder store1 = ApplicationData.Current.LocalFolder;
                    StorageFolder Image = Task.Run(async () => await store1.CreateFolderAsync("Images", CreationCollisionOption.OpenIfExists)).Result;
                    Task.Run(async () => await Image.CreateFolderAsync("PersonImages", CreationCollisionOption.OpenIfExists));
                    Task.Run(async () => await Image.CreateFolderAsync("scale-100", CreationCollisionOption.OpenIfExists));
                    Task.Run(async () => await Image.CreateFolderAsync("scale-140", CreationCollisionOption.OpenIfExists));
                    Task.Run(async () => await Image.CreateFolderAsync("scale-180", CreationCollisionOption.OpenIfExists));
                    Task.Run(async () => await Image.CreateFolderAsync("ListImages", CreationCollisionOption.OpenIfExists));
                    StorageFolder f = Task.Run(async () => await Image.CreateFolderAsync("TileImages", CreationCollisionOption.OpenIfExists)).Result;
                    Task.Run(async () => await f.CreateFolderAsync("150-150", CreationCollisionOption.OpenIfExists));
                    Task.Run(async () => await f.CreateFolderAsync("30-30", CreationCollisionOption.OpenIfExists));
                    Task.Run(async () => await f.CreateFolderAsync("310-150", CreationCollisionOption.OpenIfExists));
                    if (AppSettings.ProjectName == "Story Time" || ResourceHelper.AppName == Apps.Vedic_Library.ToString())
                    {
                        Task.Run(async () => await Image.CreateFolderAsync("storyImages", CreationCollisionOption.OpenIfExists));
                    }
                    if (AppSettings.ProjectName == "DrivingTest.Windows" || ResourceHelper.AppName== "Online_Education.Windows")
                    {
                        Task.Run(async () => await Image.CreateFolderAsync("QuestionsImage", CreationCollisionOption.OpenIfExists));
                    }
                    //To save Version No...........................................................
                    SettingsHelper.AddOrUpdateKey(settings, "Version", version);

                    //........................................................................................................................
                    AppSettings.PeopleDownloadCompleted = "1";
                    AppSettings.DownloadCompleted = "1";
           }
        }
    }
}