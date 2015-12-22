using System;
using System.Net;
using System.Windows;
using System.Linq;
using Common.Library;
using OnlineVideos.Library;
using System.IO;
using System.Reflection;
using Windows.ApplicationModel;

namespace OnlineVideos
{
    public class AppInitialize
    {
        CustomizationSettings objcustom;

        public AppInitialize()
        {
            objcustom = new CustomizationSettings();
        }

        public void CheckElementSection()
        {
            try
            {
                PackageVersion pv = Package.Current.Id.Version;                
                Version version = new Version(Package.Current.Id.Version.Major,Package.Current.Id.Version.Minor,Package.Current.Id.Version.Revision,Package.Current.Id.Version.Build);
                var AppVer = version.ToString();
                var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
                var assembly = (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly").Invoke(null, new object[0]);

                if (AppVer != SettingsHelper.getStringValue("Version"))
                {
                DataHelper obj = new DataHelper();
                obj.MoveReferenceDatabase();
                obj.CopyDatabase();
                obj.CopyDefaultImage();
                obj.Deletexml();
                obj.IsLowMemDevice();

                AppSettings.ratingreviewpopcount = 20;
                AppSettings.AppLaunchCount = 0;
                SettingsHelper.AddOrUpdateKey(settings, "CheckDownloadTheme", objcustom.CheckDoenloadTheme());

                if (SettingsHelper.getStringValue("CheckDownloadTheme") != "true")
                {
                   // Settings used to check in BackgroundAgent.....................
                    AppSettings.Downloadpeople = AppResources.DownloadPeople;
                    AppSettings.Downloadshows = AppResources.DownloadShow;
                    AppSettings.Primarypeoplefromblog = AppResources.PrimaryPeopleDownloadUri;
                    AppSettings.Secondarypeoplefromblog = AppResources.SecondaryPeopleDownloadUri;
                    AppSettings.Primaryshowsfromblog = AppResources.PrimaryShowsDownloadUri;
                    AppSettings.Secondaryshowsfromblog = AppResources.SecondaryShowsDownloadUri;
                    AppSettings.ShowsInsertLimit = AppResources.ShowsInsertLimit;
                    AppSettings.PeopleInsertLimit = AppResources.PeopleInsertLimit;
                    AppSettings.BlogStatus = 0;

                    //..............................................................

                    //Uploading Parental control Data..............................

                    AppSettings.LastUploadedShowIdHid = 0;
                    AppSettings.LastUploadedVideoIdHid = 0;
                    AppSettings.LastUpdatedDateForHide = "26/11/2015";
                    //..............................................................

                    //Downloading Parental control Data..............................
                    AppSettings.LastDownloadIdHid = 0;
                    //...................................................................

                    //Downloading Favourite Movies or Songs...............................
                    AppSettings.LastDownloadIdFav = 0;
                    //...................................................................

                    //Uploading Favourite Movies or Songs...............................
                    AppSettings.LastUploadedVideoIdFav = 0;
                    AppSettings.LastUploadedShowIdFav = 0;
                    AppSettings.LastUpdatedDateforFavourites = new DateTime(2015, 11, 26);
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
                    AppSettings.lastVideoDownloadedRatingDate = new DateTimeOffset(new DateTime(2015, 11, 26));
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
                    if (ResourceHelper.ProjectName == "Story Time" || ResourceHelper.ProjectName == "Vedic Library")
                        AppSettings.BackgroundAgentStatus = SyncAgentStatus.RestoreStory;
                    else
                        AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadFavourites;

                    //..........................................................................

                    //Overall downloaded date used as condition to download data once per day....

                    AppSettings.DownloadCompletedDate = new DateTime(2015, 11, 26);
                    AppSettings.StoryUploadedDate = new DateTime(2015, 11, 26);

                    AppSettings.SkyDriveLogin = false;
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

                    if (Storage.DirectoryExists("Images") == null)
                        Storage.CreateDirectory("Images");
                    if (ResourceHelper.AppName == Apps.Online_Education.ToString() || ResourceHelper.ProjectName == Apps.DrivingTest.ToString())
                    {
                        if (Storage.DirectoryExists("Images\\QuestionsImage") == null)
                            Storage.CreateDirectory("Images\\QuestionsImage");
                    }
                    if (ResourceHelper.AppName == Apps.Story_Time.ToString() || ResourceHelper.AppName == Apps.Vedic_Library.ToString())
                    {
                        if (Storage.DirectoryExists("Images\\storyImages") == null)
                            Storage.CreateDirectory("Images\\storyImages");
                    }
                }

                SettingsHelper.AddOrUpdateKey(settings, "FromEmailid", objcustom.GetFromEmailId());
                SettingsHelper.AddOrUpdateKey(settings, "ToEmailid", objcustom.GetToEmailId());
                SettingsHelper.AddOrUpdateKey(settings, "canvasmargin", objcustom.Checkcanvasmargin());
                SettingsHelper.AddOrUpdateKey(settings, "textblockmargin", objcustom.Checktextblockmargin());

                SettingsHelper.AddOrUpdateKey(settings, "imagemargin", objcustom.Checkimagemargin());
                SettingsHelper.AddOrUpdateKey(settings, "textcolor", objcustom.Checktextcolor());
                SettingsHelper.AddOrUpdateKey(settings, "AdVisible", objcustom.AdVisible());
                SettingsHelper.AddOrUpdateKey(settings, "People", objcustom.PeopleExists());
                SettingsHelper.AddOrUpdateKey(settings, "AboutUsUpgrade", objcustom.Checkaboutupgrade());
                SettingsHelper.AddOrUpdateKey(settings, "Upgrade", objcustom.Upgrade());
                SettingsHelper.AddOrUpdateKey(settings, "upgrademessage", objcustom.Checkupgrademessage());
                SettingsHelper.AddOrUpdateKey(settings, "upgradelink", objcustom.Checkupgradelink());
                SettingsHelper.AddOrUpdateKey(settings, "Repeatads", "0");

                Storage.DeleteFile(Constants.ExceptionHistoryXmlPath);
                Storage.DeleteFile(Constants.InstalledCustomizationXmlPath);                    
                
                 //To save Version No...........................................................
                 SettingsHelper.AddOrUpdateKey(settings, "Version",AppVer);

                //........................................................................................................................
                 }
                AppSettings.ShowCricketDetailPage = AppResources.ShowCricketDetailPage;
                AppSettings.PeopleDownloadCompleted = "1";
                AppSettings.DownloadCompleted = "1";
                SettingsHelper.saveAllSettings(settings);
            }
            catch(Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Failed to download movie updates - {0}", ex);
            }
        }

        public void getImages()
        {
            //App.PanaromaBg = objcustom.LoadPanoramBackground();
            //App.PanaramaLogo = objcustom.LoadPanoramaLogo();
            //App.PanaramaTitle = objcustom.LoadPanoramaTitle();
            //App.Povitbg = objcustom.LoadPivotBackground();
            //App.povitTitle = objcustom.LoadPivotTitle();
        }
    }
}