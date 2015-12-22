using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineVideos.Entities;
#if WINDOWS_APP && NOTANDROID
using Windows.UI.Xaml.Controls;
#endif
namespace Common.Library
{
    public static class AppSettings
    {

        public static bool startplaying
        {
            get
            {
                return SettingsHelper.getBoolValue("startplaying");
            }
            set
            {
                SettingsHelper.Save("startplaying", value);
            }
        }
        public static bool startplayingforpro
        {
            get
            {
                return SettingsHelper.getBoolValue("startplayingforpro");
            }
            set
            {
                SettingsHelper.Save("startplayingforpro", value);
            }
        }
        public static string blogtoken
        {
            get
            {
                return SettingsHelper.getStringValue("blogtoken");
            }
            set
            {
                SettingsHelper.Save("blogtoken", value);
            }
        }
        public static string BloggerAccessTokenForRating
        {
            get
            {
                return SettingsHelper.getStringValue("BloggerAccessTokenForRating");
            }
            set
            {
                SettingsHelper.Save("BloggerAccessTokenForRating", value);
            }
        }
       
        public static string BlogUserName
        {
            get
            {
                return SettingsHelper.getStringValue("BlogUserName");
            }
            set
            {
                SettingsHelper.Save("BlogUserName", value);
            }
        }
        public static string OAuthToken
        {
            get
            {
                return SettingsHelper.getStringValue("OAuthToken");
            }
            set
            {
                SettingsHelper.Save("OAuthToken", value);
            }
        }
        public static string OAuthTokenSecret
        {
            get
            {
                return SettingsHelper.getStringValue("OAuthTokenSecret");
            }
            set
            {
                SettingsHelper.Save("OAuthTokenSecret", value);
            }
        }
        public static string FaceBookAccessToken
        {
            get
            {
                return SettingsHelper.getStringValue("FaceBookAccessToken");
            }
            set
            {
                SettingsHelper.Save("FaceBookAccessToken", value);
            }
        }
        public static string BloggerAccessToken
        {
            get
            {
                return SettingsHelper.getStringValue("BloggerAccessToken");
            }
            set
            {
                SettingsHelper.Save("BloggerAccessToken", value);
            }
        }

        public static string MicrosoftAccessToken
        {
            get
            {
                return SettingsHelper.getStringValue("MicrosoftAccessToken");
            }
            set
            {
                SettingsHelper.Save("MicrosoftAccessToken", value);
            }
        }
        public static string musicvisibility
        {
            get
            {
                return SettingsHelper.getStringValue("musicvisibility");
            }
            set
            {
                SettingsHelper.Save("musicvisibility", value);
            }
        }
        public static string DownLoadPersonName
        {
            get
            {
                return SettingsHelper.getStringValue("DownLoadPersonName");
            }
            set
            {
                SettingsHelper.Save("DownLoadPersonName", value);
            }
        }

        public static string gallcount
        {
            get
            {
                return SettingsHelper.getStringValue("gallcount");
            }
            set
            {
                SettingsHelper.Save("gallcount", value);
            }
        }
    #if NOTANDROID
        public static List<ShowLinks> TopAudio
        {
            get
            {
                StorageHelper<List<ShowLinks>> help = new StorageHelper<List<ShowLinks>>(StorageType.Local);
                return Task.Run(async () => await help.LoadASync("TopAudio")).Result;

            }
            set
            {
                StorageHelper<List<ShowLinks>> help = new StorageHelper<List<ShowLinks>>(StorageType.Local);
               
                    bool result = Task.Run(async () => await help.SaveASync(value, "TopAudio")).Result;
               

            }
        }
        public static List<ShowLinks> TopSong
        {
            get
            {
                StorageHelper<List<ShowLinks>> help = new StorageHelper<List<ShowLinks>>(StorageType.Local);
                return Task.Run(async () => await help.LoadASync("TopSong")).Result;

            }
            set
            {
                StorageHelper<List<ShowLinks>> help = new StorageHelper<List<ShowLinks>>(StorageType.Local);
               
                    bool result = Task.Run(async () => await help.SaveASync(value, "TopSong")).Result;
               

            }
        }
      #endif
        public static int LiveTileHistroryCount
        {
            get
            {
                return (int)SettingsHelper.getIntValue("LiveTileHistroryCount");
            }
            set
            {
                SettingsHelper.Save("LiveTileHistroryCount", value);
            }
        }

        public static bool FromYoutubePage
        {
            get
            {
                return SettingsHelper.getBoolValue("FromYoutubePage");
            }
            set
            {
                SettingsHelper.Save("FromYoutubePage", value);
            }
        }
        public static bool IsNewVersionForTables
        {
            get
            {
                return SettingsHelper.getBoolValue("IsNewVersionForTables");
            }
            set
            {
                SettingsHelper.Save("IsNewVersionForTables", value);
            }
        }
        public static string GalleryTitle
        {
            get
            {
                return SettingsHelper.getStringValue("GalleryTitle");
            }
            set
            {
                SettingsHelper.Save("GalleryTitle", value);
            }
        }
        public static bool MovieRateShowStatus
        {
            get
            {
                return SettingsHelper.getBoolValue("MovieRateShowStatus");
            }
            set
            {
                SettingsHelper.Save("MovieRateShowStatus", value);
            }
        }
        public static bool LiveTileBackgroundAgentStatus
        {
            get
            {
                return SettingsHelper.getBoolValue("LiveTileBackgroundAgentStatus");
            }
            set
            {
                SettingsHelper.Save("LiveTileBackgroundAgentStatus", value);
            }
        }
        public static string htmltextfordownloadmanger
        {
            get
            {
                return SettingsHelper.getStringValue("HtmlText");
            }
            set
            {
                SettingsHelper.Save("HtmlText", value);
            }
        }
        public static string DownloadedFolderName
        {
            get
            {
                return SettingsHelper.getStringValue("DownloadedFolderName");
            }
            set
            {
                SettingsHelper.Save("DownloadedFolderName", value);
            }
        }
        public static int MainPageSongCategory
        {
            get
            {
                return (int)SettingsHelper.getIntValue("MainPageSongCategory");
            }
            set
            {
                SettingsHelper.Save("MainPageSongCategory", value);
            }
        }
        public static string LinkOrder
        {
            get
            {
                return SettingsHelper.getStringValue("LinkOrder");
            }
            set
            {
                SettingsHelper.Save("LinkOrder", value);
            }
        }
        public static int MainPageAudioCategory
        {
            get
            {
                return (int)SettingsHelper.getIntValue("MainPageAudioCategory");
            }
            set
            {
                SettingsHelper.Save("MainPageAudioCategory", value);
            }
        }
        public static string NavigationUrl
        {
            get
            {
                return SettingsHelper.getStringValue("NavigationUrl");
            }
            set
            {
                SettingsHelper.Save("NavigationUrl", value);
            }
        }
        public static bool IsRingtone
        {
            get
            {
                return SettingsHelper.getBoolValue("IsRingtone");
            }
            set
            {
                SettingsHelper.Save("IsRingtone", value);
            }
        }
        public static bool SkyDriveLogin
        {
            get
            {
                return SettingsHelper.getBoolValue("SkyDriveLogin");
            }
            set
            {
                SettingsHelper.Save("SkyDriveLogin", value);
            }
        }
        public static bool Downloadpeople
        {
            get
            {
                return SettingsHelper.getBoolValue("Downloadpeople");
            }
            set
            {
                SettingsHelper.Save("Downloadpeople", value);
            }
        }
        public static bool Downloadshows
        {
            get
            {
                return SettingsHelper.getBoolValue("Downloadshows");
            }
            set
            {
                SettingsHelper.Save("Downloadshows", value);
            }
        }
        public static string Secondaryshowsfromblog
        {
            get
            {
                return SettingsHelper.getStringValue("Secondaryshowsfromblog");
            }
            set
            {
                SettingsHelper.Save("Secondaryshowsfromblog", value);
            }
        }
        public static string Secondaryshowsfromblogwin81
        {
            get
            {
                return SettingsHelper.getStringValue("Secondaryshowsfromblogwin81");
            }
            set
            {
                SettingsHelper.Save("Secondaryshowsfromblogwin81", value);
            }
        }
        public static string imageno
        {
            get
            {
                return SettingsHelper.getStringValue("imageno");
            }
            set
            {
                SettingsHelper.Save("imageno", value);
            }
        }
        public static string Snaplyrics
        {
            get
            {
                return SettingsHelper.getStringValue("Snaplyrics");
            }
            set
            {
                SettingsHelper.Save("Snaplyrics", value);
            }
        }
        public static bool ShowCricketDetailPage
        {
            get
            {
                return SettingsHelper.getBoolValue("ShowCricketDetailPage");
            }
            set
            {
                SettingsHelper.Save("ShowCricketDetailPage", value);
            }
        }
        public static string LinkType
        {
            get
            {
                return SettingsHelper.getStringValue("LinkType");
            }
            set
            {
                SettingsHelper.Save("LinkType", value);
            }
        }

        public static string CityUrl
        {
            get
            {
                return SettingsHelper.getStringValue("CityUrl");
            }
            set
            {
                SettingsHelper.Save("CityUrl", value);
            }
        }
        public static string Country
        {
            get
            {
                return SettingsHelper.getStringValue("Country");
            }
            set
            {
                SettingsHelper.Save("Country", value);
            }
        }

        public static string ProjectName
        {
            get
            {
                return SettingsHelper.getStringValue("ProjectName");
            }
            set
            {
                SettingsHelper.Save("ProjectName", value);
            }
        }
        public static string ProName
        {
            get
            {
                return SettingsHelper.getStringValue("ProName");
            }
            set
            {
                SettingsHelper.Save("ProName", value);
            }
        }

        public static string PackageName
		{
			get
			{
				return SettingsHelper.getStringValue("AndroidPackageName");
			}
			set
			{
				SettingsHelper.Save("AndroidPackageName", value);
			}
		}
        public static string SaveTopSongId
        {
            get
            {
                return SettingsHelper.getStringValue("SaveTopSongId");
            }
            set
            {
                SettingsHelper.Save("SaveTopSongId", value);
            }
        }
        public static string DownloadFile
        {
            get {
                return SettingsHelper.getStringValue("DownloadFile");
            }
            set
            {
                SettingsHelper.Save("DownloadFile", value);
            }
        }
        public static string SaveTopMusicId
        {
            get
            {
                return SettingsHelper.getStringValue("SaveTopMusicId");
            }
            set
            {
                SettingsHelper.Save("SaveTopMusicId", value);
            }
        }
        public static string Primaryshowsfromblog
        {
            get
            {
                return SettingsHelper.getStringValue("Primaryshowsfromblog");
            }
            set
            {
                SettingsHelper.Save("Primaryshowsfromblog", value);
            }
        }
        public static string Primaryshowsfromblogwin81
        {
            get
            {
                return SettingsHelper.getStringValue("Primaryshowsfromblogwin81");
            }
            set
            {
                SettingsHelper.Save("Primaryshowsfromblogwin81", value);
            }
        }
        public static int ShowsInsertLimit
        {
            get
            {
                return (int)SettingsHelper.getIntValue("ShowsInsertLimit");
            }
            set
            {
                SettingsHelper.Save("ShowsInsertLimit", value);
            }
        }
        public static int DownloadStatues
        {
            get
            {
                return (int)SettingsHelper.getIntValue("DownloadStatues");
            }
            set
            {
                SettingsHelper.Save("DownloadStatues", value);
            }
        }
        public static string ShowStartQuestionId
        {
            get
            {
                return SettingsHelper.getStringValue("ShowStartQuestionId");
            }
            set
            {
                SettingsHelper.Save("ShowStartQuestionId", value);
            }
        }
        public static int BlogStatus
        {
            get
            {
                return (int)SettingsHelper.getIntValue("BlogStatus");
            }
            set
            {
                SettingsHelper.Save("BlogStatus", value);
            }
        }
        public static string YoutubeID
        {
            get
            {
                return SettingsHelper.getStringValue("YoutubeID");
            }
            set
            {
                SettingsHelper.Save("YoutubeID", value);
            }
        }
        public static int GallCount
        {
            get
            {
                return (int)SettingsHelper.getIntValue("GallCount");
            }
            set
            {
                SettingsHelper.Save("GallCount", value);
            }
        }
        public static string GallCount1
        {
            get
            {
                return SettingsHelper.getStringValue("GallCount1");
            }
            set
            {
                SettingsHelper.Save("GallCount1", value);
            }
        }

        public static string DownloadTitle
        {
            get
            {
                return SettingsHelper.getStringValue("DownloadTitle");
            }
            set
            {
                SettingsHelper.Save("DownloadTitle", value);
            }
        }
        public static string FavTitle
        {
            get
            {
                return SettingsHelper.getStringValue("FavTitle");
            }
            set
            {
                SettingsHelper.Save("FavTitle", value);
            }
        }
        public static string TileImage
        {
            get
            {
                return SettingsHelper.getStringValue("TileImage");
            }
            set
            {
                SettingsHelper.Save("TileImage", value);
            }
        }

        public static string ChannelImage
        {
            get
            {
                return SettingsHelper.getStringValue("ChannelImage");
            }
            set
            {
                SettingsHelper.Save("ChannelImage", value);
            }
        }
        public static string ChannelTitle
        {
            get
            {
                return SettingsHelper.getStringValue("ChannelTitle");
            }
            set
            {
                SettingsHelper.Save("ChannelTitle", value);
            }
        }
        public static string QuizFavTitle
        {
            get
            {
                return SettingsHelper.getStringValue("QuizFavTitle");
            }
            set
            {
                SettingsHelper.Save("QuizFavTitle", value);
            }
        }
        public static string Youtubelink
        {
            get
            {
                return SettingsHelper.getStringValue("Youtubelink");
            }
            set
            {
                SettingsHelper.Save("Youtubelink", value);
            }
        }
        public static string YoutubeUri
        {
            get
            {
                return SettingsHelper.getStringValue("YoutubeUri");
            }
            set
            {
                SettingsHelper.Save("YoutubeUri", value);
            }
        }
        public static string YoutubeError
        {
            get
            {
                return SettingsHelper.getStringValue("YoutubeError");
            }
            set
            {
                SettingsHelper.Save("YoutubeError", value);
            }
        }
        public static string FavoritesSelectedIndex
        {
            get
            {
                return SettingsHelper.getStringValue("FavoritesSelectedIndex");
            }
            set
            {
                SettingsHelper.Save("FavoritesSelectedIndex", value);
            }
        }

        public static int LastShowInsertId
        {
            get
            {
                return (int)SettingsHelper.getIntValue("LastShowInsertId");
            }
            set
            {
                SettingsHelper.Save("LastShowInsertId", value);
            }
        }
        public static int ComboYoutube
        {
            get
            {
                return (int)SettingsHelper.getIntValue("ComboYoutube");
            }
            set
            {
                SettingsHelper.Save("ComboYoutube", value);
            }
        }
        public static int LastPeopleInsertId
        {
            get
            {
                return (int)SettingsHelper.getIntValue("LastPeopleInsertId");
            }
            set
            {
                SettingsHelper.Save("LastPeopleInsertId", value);
            }
        }
        public static int PeopleInsertLimit
        {
            get
            {
                return (int)SettingsHelper.getIntValue("PeopleInsertLimit");
            }
            set
            {
                SettingsHelper.Save("PeopleInsertLimit", value);
            }
        }
        public static string starturidownloadmanger
        {
            get
            {
                return SettingsHelper.getStringValue("htmltext");
            }
            set
            {
                SettingsHelper.Save("htmltext", value);
            }
        }
        public static string LinkUrl
        {
            get
            {
                return SettingsHelper.getStringValue("LinkUrl");
            }
            set
            {
                SettingsHelper.Save("LinkUrl", value);
            }
        }
        public static string Status
        {
            get
            {
                return SettingsHelper.getStringValue("Status");
            }
            set
            {
                SettingsHelper.Save("Status", value);
            }
        }
        public static string MovieTitle
        {
            get
            {
                return SettingsHelper.getStringValue("MovieTitle");
            }
            set
            {
                SettingsHelper.Save("MovieTitle", value);
            }
        }

        public static string SearchText
        {
            get
            {
                return SettingsHelper.getStringValue("SearchText");
            }
            set
            {
                SettingsHelper.Save("SearchText", value);
            }
        }

        public static string Secondarypeoplefromblog
        {
            get
            {
                return SettingsHelper.getStringValue("Secondarypeoplefromblog");
            }
            set
            {
                SettingsHelper.Save("Secondarypeoplefromblog", value);
            }
        }

        public static string Secondarypeoplefromblogwin81
        {
            get
            {
                return SettingsHelper.getStringValue("Secondarypeoplefromblogwin81");
            }
            set
            {
                SettingsHelper.Save("Secondarypeoplefromblogwin81", value);
            }
        }

        public static string AudioFavTitle
        {
            get
            {
                return SettingsHelper.getStringValue("AudioFavTitle");
            }
            set
            {
                SettingsHelper.Save("AudioFavTitle", value);
            }
        }
        public static string CategoryVisible
        {
            get
            {
                return SettingsHelper.getStringValue("CategoryVisible");
            }
            set
            {
                SettingsHelper.Save("CategoryVisible", value);
            }
        }
        public static string AppName
        {
            get
            {
                return SettingsHelper.getStringValue("AppName");
            }
            set
            {
                SettingsHelper.Save("AppName", value);
            }
        }
         public static string ImageTitle
        {
            get
            {
                return SettingsHelper.getStringValue("ImageTitle");
            }
            set
            {
                SettingsHelper.Save("ImageTitle", value);
            }
        }
        public static string Title
        {
            get
            {
                return SettingsHelper.getStringValue("Title");
            }
            set
            {
                SettingsHelper.Save("Title", value);
            }
        }
        public static string PlayVideoTitle
        {
            get
            {
                return SettingsHelper.getStringValue("PlayVideoTitle");
            }
            set
            {
                SettingsHelper.Save("PlayVideoTitle", value);
            }
        }
        public static string ViewAllTitle
        {
            get
            {
                return SettingsHelper.getStringValue("ViewAllTitle");
            }
            set
            {
                SettingsHelper.Save("ViewAllTitle", value);
            }
        }
        public static string Primarypeoplefromblog
        {
            get
            {
                return SettingsHelper.getStringValue("Primarypeoplefromblog");
            }
            set
            {
                SettingsHelper.Save("Primarypeoplefromblog", value);
            }
        }
        public static string Primarypeoplefromblogwin81
        {
            get
            {
                return SettingsHelper.getStringValue("Primarypeoplefromblogwin81");
            }
            set
            {
                SettingsHelper.Save("Primarypeoplefromblogwin81", value);
            }
        }
        public static string lastDownloadedPersonId
        {
            get
            {
                return SettingsHelper.getStringValue("lastDownloadedPersonId");
            }
            set
            {
                SettingsHelper.Save("lastDownloadedPersonId", value);
            }
        }
      
       
        public static string ApplictionNameForImageDownload
        {
            get
            {
                return SettingsHelper.getStringValue("ApplictionNameForImageDownload");
            }
            set
            {
                SettingsHelper.Save("ApplictionNameForImageDownload", value);
            }
        }

        public static string ImagePathForImageDownload
        {
            get
            {
                return SettingsHelper.getStringValue("ImagePathForImageDownload");
            }
            set
            {
                SettingsHelper.Save("ImagePathForImageDownload", value);
            }
        }

        public static string ShowIDForImageDownload
        {
            get
            {
                return SettingsHelper.getStringValue("ShowIDForImageDownload");
            }
            set
            {
                SettingsHelper.Save("ShowIDForImageDownload", value);
            }
        }

        public static string FeedbackEmailID
        {
            get
            {
                return SettingsHelper.getStringValue("FeedBackEmailId");
            }
            set
            {
                SettingsHelper.Save("FeedBackEmailId", value);
            }
        }
        public static string AudioImage
        {
            get
            {
                return SettingsHelper.getStringValue("AudioImage");
            }
            set
            {
                SettingsHelper.Save("AudioImage", value);
            }
        }
        public static string SongID
        {
            get
            {
                return SettingsHelper.getStringValue("SongID");
            }
            set
            {
                SettingsHelper.Save("SongID", value);
            }
        }

        public static string AppProductID
        {
            get
            {
                return SettingsHelper.getStringValue("productid");
            }
            set
            {
                SettingsHelper.Save("productid", value);
            }
        }

        public static string PivotBackground
        {
            get
            {
                return SettingsHelper.getStringValue("PivotBackground");
            }
            set
            {
                SettingsHelper.Save("PivotBackground", value);
            }
        }

        public static string PivotTitle
        {
            get
            {
                return SettingsHelper.getStringValue("PivotTitle");
            }
            set
            {
                SettingsHelper.Save("PivotTitle", value);
            }
        }

        public static bool VideoMixFlag
        {
            get
            {
                return SettingsHelper.getBoolValue("VideoMixFlag");

            }
            set
            {
                SettingsHelper.Save("VideoMixFlag", value);
            }
        }

        public static bool AutomaticallyEmailErrors
        {
            get
            {
                return SettingsHelper.getBoolValue("SendErrors");
            }
            set
            {
                SettingsHelper.Save("SendErrors", value);
            }
        }


        public static ApplicationStatus AppStatus
        {
            get
            {
                return (ApplicationStatus)SettingsHelper.getIntValue("AppStatus");
            }
            set
            {
                SettingsHelper.Save("AppStatus", (int)value);
            }
        }

        public static bool IsNewVersion
        {
            get
            {
                return SettingsHelper.getBoolValue("NewVersion");
            }
            set
            {
                SettingsHelper.Save("NewVersion", value);
            }
        }
        public static bool DownloadFavCompleted
        {
            get
            {
                return SettingsHelper.getBoolValue("DownloadFavCompleted");
            }
            set
            {
                SettingsHelper.Save("DownloadFavCompleted", value);
            }
        }
        public static bool DownloadStoryCompleted
        {
            get
            {
                return SettingsHelper.getBoolValue("DownloadStoryCompleted");
            }
            set
            {
                SettingsHelper.Save("DownloadStoryCompleted", value);
            }
        }
        public static string CategoryID
        {
            get
            {
                return SettingsHelper.getStringValue("CategoryID");
            }
            set
            {
                SettingsHelper.Save("CategoryID", value);
            }
        }
        public static string AllSubjects
        {
            get
            {
                return SettingsHelper.getStringValue("AllSubjects");
            }
            set
            {
                SettingsHelper.Save("AllSubjects", value);
            }
        }
        public static string AllRecipes
        {
            get
            {
                return SettingsHelper.getStringValue("AllRecipes");
            }
            set
            {
                SettingsHelper.Save("AllRecipes", value);
            }
        }
        public static string CategoryIDForCompare
        {
            get
            {
                return SettingsHelper.getStringValue("CategoryIDForCompare");
            }
            set
            {
                SettingsHelper.Save("CategoryIDForCompare", value);
            }
        }
        public static string CricketTeam1Title
        {
            get
            {
                return SettingsHelper.getStringValue("CricketTeam1Title");
            }
            set
            {
                SettingsHelper.Save("CricketTeam1Title", value);
            }
        }

        public static string CricketTeam2Title
        {
            get
            {
                return SettingsHelper.getStringValue("CricketTeam2Title");
            }
            set
            {
                SettingsHelper.Save("CricketTeam2Title", value);
            }
        }

        public static SyncAgentStatus BackgroundAgentStatus
        {
            get
            {
                return (SyncAgentStatus)SettingsHelper.getIntValue("AgentState");
            }
            set
            {
                SettingsHelper.Save("AgentState", (int)value);
            }
        }

        public static YouTubeQuality YoutubeQuality
        {
            get
            {
                return (YouTubeQuality)SettingsHelper.getIntValue("Youtube");
            }
            set
            {
                SettingsHelper.Save("Youtube", (int)value);
            }
        }
        public static YouTubeQuality VideoPosition
        {
            get
            {
                return (YouTubeQuality)SettingsHelper.getIntValue("VideoPosition");
            }
            set
            {
                SettingsHelper.Save("VideoPosition", (int)value);
            }
        }
        public static string AudioShowID
        {
            get
            {
                return SettingsHelper.getStringValue("AudioMovieId");
            }
            set
            {
                SettingsHelper.Save("AudioMovieId", value);
            }
        }

        public static string SongPlayImage
        {
            get
            {
                return SettingsHelper.getStringValue("SongPlayImage");
            }
            set
            {
                SettingsHelper.Save("SongPlayImage", value);
            }
        }

        public static int ShowUniqueID
        {
            get
            {
                int? UniqueID = SettingsHelper.getIntValue("id");
                return UniqueID.HasValue ? UniqueID.Value : 0;
            }
        }
        public static string IDForImagePath
        {
            get
            {
                return SettingsHelper.getStringValue("ImagePath");
            }
            set
            {
                SettingsHelper.Save("ImagePath", value);
            }
        }
        public static string ShowID
        {
            get
            {
                return SettingsHelper.getStringValue("id");
            }
            set
            {
                SettingsHelper.Save("id", value);
            }
        }
        public static string ShowsIsHidden
        {
            get
            {
                return SettingsHelper.getStringValue("IsHidden");
            }
            set
            {
                SettingsHelper.Save("IsHidden", value);
            }
        }
        public static string ShowsVideoIsHidden
        {
            get
            {
                return SettingsHelper.getStringValue("VideoIsHidden");
            }
            set
            {
                SettingsHelper.Save("VideoIsHidden", value);
            }
        }
        public static string TeamA
        {
            get
            {
                return SettingsHelper.getStringValue("TeamA");
            }
            set
            {
                SettingsHelper.Save("TeamA", value);
            }
        }
        public static string TeamB
        {
            get
            {
                return SettingsHelper.getStringValue("TeamB");
            }
            set
            {
                SettingsHelper.Save("TeamB", value);
            }
        }
        public static string TeamATitle
        {
            get
            {
                return SettingsHelper.getStringValue("TeamATitle");
            }
            set
            {
                SettingsHelper.Save("TeamATitle", value);
            }
        }
        public static string TeamBTitle
        {
            get
            {
                return SettingsHelper.getStringValue("TeamBTitle");
            }
            set
            {
                SettingsHelper.Save("TeamBTitle", value);
            }
        }
        public static string Chapterno
        {
            get
            {
                return SettingsHelper.getStringValue("cno");
            }
            set
            {
                SettingsHelper.Save("cno", value);
            }
        }

        public static string DownloadCompleted
        {
            get
            {
                return SettingsHelper.getStringValue("DownloadCompleted");
            }
            set
            {
                SettingsHelper.Save("DownloadCompleted", value);
            }
        }

        public static double ShowRating
        {
            get
            {
                int? relatedAppCount = SettingsHelper.getIntValue("MovieRating");
                return relatedAppCount.HasValue ? relatedAppCount.Value : 0;
            }
            set
            {
                SettingsHelper.Save("MovieRating", value);
            }
        }

        public static bool AddNewShowIconVisibility
        {
            get
            {
                return SettingsHelper.getStringValue("CheckDownloadTheme") == "true" ? true : false;
            }
            set
            {
                SettingsHelper.Save("CheckDownloadTheme", value);
            }
        }

        public static bool ShowAdControl
        {
            get
            {
                return SettingsHelper.getBoolValue("advisibility");
            }
            set
            {
                SettingsHelper.Save("advisibility", value);
            }
        }

        public static bool AppLevelState
        {
            get
            {
                return SettingsHelper.getBoolValue("AppLevelState");
            }
            set
            {
                SettingsHelper.Save("AppLevelState", value);
            }
        }

        public static bool RunUnderLock
        {
            get
            {
                return SettingsHelper.getBoolValue("runUnderLock");
            }
            set
            {
                SettingsHelper.Save("runUnderLock", value);
            }
        }

        public static bool AutomaticallyDownloadShows
        {
            get
            {
                return SettingsHelper.getBoolValue("UpdateMovies");
            }
            set
            {
                SettingsHelper.Save("UpdateMovies", value);
            }
        }
        public static bool AllowvideoLibabry
        {
            get
            {
                return SettingsHelper.getBoolValue("videoLibabry");
            }
            set
            {
                SettingsHelper.Save("videoLibabry", value);
            }
        }

        public static int RelatedAppCount
        {
            get
            {
                int? relatedAppCount = SettingsHelper.getIntValue("RelatedAppCount");
                return relatedAppCount.HasValue ? relatedAppCount.Value : 0;
            }
            set
            {
                SettingsHelper.Save("RelatedAppCount", value);
            }
        }
        public static int AppLaunchCount
        {
            get
            {
                return (int)SettingsHelper.getIntValue("AppLaunchCount");
            }
            set
            {
                SettingsHelper.Save("AppLaunchCount", value);
            }
        }
        public static int ratingreviewpopcount
        {
            get
            {
                return (int)SettingsHelper.getIntValue("ratingreviewpopcount");
            }
            set
            {
                SettingsHelper.Save("ratingreviewpopcount", value);
            }
        }
        public static int lastDownloadedRatingId
        {
            get
            {
                return (int)SettingsHelper.getIntValue("lastDownloadedRatingId");
            }
            set
            {
                SettingsHelper.Save("lastDownloadedRatingId", value);
            }
        }

        public static int LastUploadRatingId
        {
            get
            {
                return (int)SettingsHelper.getIntValue("LastUploadRatingId");
            }
            set
            {
                SettingsHelper.Save("LastUploadRatingId", value);
            }
        }

        public static int SearchLastUploadedId
        {
            get
            {
                return (int)SettingsHelper.getIntValue("SearchLastUploadedId");
            }
            set
            {
                SettingsHelper.Save("SearchLastUploadedId", value);
            }
        }

        public static int LastUploadedVideoIdHid
        {
            get
            {
                return (int)SettingsHelper.getIntValue("LastUploadedVideoIdHid");
            }
            set
            {
                SettingsHelper.Save("LastUploadedVideoIdHid", value);
            }
        }
        public static int LastUploadedShowIdHid
        {
            get
            {
                return (int)SettingsHelper.getIntValue("LastUploadedMovieIdHid");
            }
            set
            {
                SettingsHelper.Save("LastUploadedMovieIdHid", value);
            }
        }

        public static int lastvideoDownloadedRatingId
        {
            get
            {
                return (int)SettingsHelper.getIntValue("lastvideoDownloadedRatingId");
            }
            set
            {
                SettingsHelper.Save("lastvideoDownloadedRatingId", value);
            }
        }

        public static int LastUploadedShowIdFav
        {
            get
            {
                return (int)SettingsHelper.getIntValue("LastUploadedMovieIdFav");
            }
            set
            {
                SettingsHelper.Save("LastUploadedMovieIdFav", value);
            }
        }
        public static int mediaposition
        {
            get
            {
                return (int)SettingsHelper.getIntValue("mediaposition");
            }
            set
            {
                SettingsHelper.Save("mediaposition", value);
            }
        }
        public static int LastUploadedVideoIdFav
        {
            get
            {
                return (int)SettingsHelper.getIntValue("LastUploadedVideoIdFav");
            }
            set
            {
                SettingsHelper.Save("LastUploadedVideoIdFav", value);
            }
        }
        public static int LastUploadVideoRatingid
        {
            get
            {
                return (int)SettingsHelper.getIntValue("LastUploadVideoRatingid");
            }
            set
            {
                SettingsHelper.Save("LastUploadVideoRatingid", value);
            }
        }

        public static string StopTimer
        {
            get
            {
                return SettingsHelper.getStringValue("StopTimer");
            }
            set
            {
                SettingsHelper.Save("StopTimer", value);
            }
        }

        public static DateTimeOffset lastVideoDownloadedRatingDate
        {
            get
            {
                string szCompletedDate = SettingsHelper.getStringValue("lastVideoDownloadedRatingDate");
                DateTimeOffset dtCompletedDate;
                DateTimeOffset.TryParse(szCompletedDate, out dtCompletedDate);
                return dtCompletedDate;
              
            }
            set
            {
                SettingsHelper.Save("lastVideoDownloadedRatingDate", value);
            }
        }

      
             public static DateTime LastUpdatedDateforFavourites
        {
            get
            {
                string szCompletedDate = SettingsHelper.getStringValue("LastUpdatedDateforFavourites");
                DateTime dtCompletedDate;
                DateTime.TryParse(szCompletedDate, out dtCompletedDate);
                return dtCompletedDate;
            }
            set
            {
                SettingsHelper.Save("LastUpdatedDateforFavourites", value);
            }
      
        }

        public static string LastUpdatedDateForHide
        {
            get
            {
                return SettingsHelper.getStringValue("LastUpdatedDateForHide");
            }
            set
            {
                SettingsHelper.Save("LastUpdatedDateForHide", value);
            }
        }
        public static string UserEmail
        {
            get
            {
                return SettingsHelper.getStringValue("UserEmail");
            }
            set
            {
                SettingsHelper.Save("UserEmail", value);
            }
        }
        public static string PeopleDownloadCompleted
        {
            get
            {
                return SettingsHelper.getStringValue("PeopleDownloadCompleted");
            }
            set
            {
                SettingsHelper.Save("PeopleDownloadCompleted", value);
            }
        }

        public static DateTime lastDownloadedRatingDate
        {
              get
            {
                string szCompletedDate = SettingsHelper.getStringValue("lastDownloadedRatingDate");
                DateTime dtCompletedDate;
                DateTime.TryParse(szCompletedDate, out dtCompletedDate);
                return dtCompletedDate;
              
            }
            set
            {
                SettingsHelper.Save("lastDownloadedRatingDate", value);
            }
           
        }
        public static object BackGroundAgentwebview
        {
            get
            {
                return SettingsHelper.getBoolValue("BackGroundAgentwebview");
            }
            set
            {
                SettingsHelper.Save("BackGroundAgentRegistered", value);
            }
        }
        public static bool BackGroundAgentRegistered
        {
            get
            {
                return SettingsHelper.getBoolValue("BackGroundAgentRegistered");
            }
            set
            {
                SettingsHelper.Save("BackGroundAgentRegistered", value);
            }
        }

        public static int MinPersonID
        {
            get
            {
                return (int)SettingsHelper.getIntValue("MinPersonID");
            }
            set
            {
                SettingsHelper.Save("MinPersonID", value);
            }
        }

        public static string PersonID
        {
            get
            {
                return SettingsHelper.getStringValue("CastProfileID");
            }
            set
            {
                SettingsHelper.Save("CastProfileID", value);
            }
        }
        public static string LiricsLink
        {
            get
            {
                return SettingsHelper.getStringValue("LiricsLinkDescription");
            }
            set
            {
                SettingsHelper.Save("LiricsLinkDescription", value);
            }
        }
        public static string LiricsType
        {
            get
            {
                return SettingsHelper.getStringValue("LiricsType");
            }
            set
            {
                SettingsHelper.Save("LiricsType", value);
            }
        }
        public static long PersonUniqueID
        {
            get
            {
                long PersonID = 0;
                string personID = SettingsHelper.getStringValue("CastProfileID");
                if (!string.IsNullOrEmpty(personID))
                    PersonID = Convert.ToInt32(personID);

                return PersonID;
            }
        }

        public static DateTime DownloadCompletedDate
        {
            get
            {
                string szCompletedDate = SettingsHelper.getStringValue("DownloadCompletedDate");
                DateTime dtCompletedDate;
                DateTime.TryParse(szCompletedDate, out dtCompletedDate);
                return dtCompletedDate;
            }
            set
            {
                SettingsHelper.Save("DownloadCompletedDate", value);
            }
        }

        public static DateTime StoryUploadedDate
        {
            get
            {
                string szCompletedDate = SettingsHelper.getStringValue("StoryUploadedDate");
                DateTime dtCompletedDate;
                DateTime.TryParse(szCompletedDate, out dtCompletedDate);
                return dtCompletedDate;
            }
            set
            {
                SettingsHelper.Save("StoryUploadedDate", value);
            }
        }
        public static DateTimeOffset LastUpdatedDate
        {
            get
            {
                string szCompletedDate = SettingsHelper.getStringValue("LastUpdatedDate");
                DateTimeOffset dtCompletedDate;
                DateTimeOffset.TryParse(szCompletedDate, out dtCompletedDate);
                return dtCompletedDate;
            }
            set
            {
                SettingsHelper.Save("LastUpdatedDate", value);
            }
        }
        public static DateTimeOffset LastPeoplePublishedDate
        {
            get
            {
                string szCompletedDate = SettingsHelper.getStringValue("LastPeoplePublishedDate");
                DateTimeOffset dtCompletedDate;
                DateTimeOffset.TryParse(szCompletedDate, out dtCompletedDate);
                return dtCompletedDate;
            }
            set
            {
                SettingsHelper.Save("LastPeoplePublishedDate", value);
            }
        }
        public static DateTimeOffset LastPublishedDate
        {
            get
            {
                string szCompletedDate = SettingsHelper.getStringValue("LastPublishedDate");
                DateTimeOffset dtCompletedDate;
                DateTimeOffset.TryParse(szCompletedDate, out dtCompletedDate);
                return dtCompletedDate;
            }
            set
            {
                SettingsHelper.Save("LastPublishedDate", value);
            }
        }

        public static DateTimeOffset LartTileDate
        {
            get
            {
                string szCompletedDate = SettingsHelper.getStringValue(ResourceHelper.ProjectName + "LartTileDate");
                DateTimeOffset dtCompletedDate;
                DateTimeOffset.TryParse(szCompletedDate, out dtCompletedDate);
                return dtCompletedDate;
            }
            set
            {
                SettingsHelper.Save(ResourceHelper.ProjectName + "LartTileDate", value);
            }
        }
        public static DateTimeOffset LastPhoneUpdatedDate
        {
            get
            {
                string szCompletedDate = SettingsHelper.getStringValue("LastPhoneUpdatedDate");
                DateTimeOffset dtCompletedDate;
                DateTimeOffset.TryParse(szCompletedDate, out dtCompletedDate);
                return dtCompletedDate;
            }
            set
            {
                SettingsHelper.Save("LastPhoneUpdatedDate", value);
            }
        }

        public static DateTimeOffset PeopleLastUpdatedDate
        {
            get
            {
                string szCompletedDate = SettingsHelper.getStringValue("PeopleLastUpdatedDate");
                DateTimeOffset dtCompletedDate;
                DateTimeOffset.TryParse(szCompletedDate, out dtCompletedDate);
                return dtCompletedDate;
            }
            set
            {
                SettingsHelper.Save("PeopleLastUpdatedDate", value);
            }
        }

        public static int DownloadFolderID
        {
            get
            {
                int? folderID = SettingsHelper.getIntValue("DowloadFolderID");
                return folderID.HasValue ? folderID.Value : 0;
            }
            set
            {
                SettingsHelper.Save("DowloadFolderID", value);
            }
        }

        public static int LastDownloadIdHid
        {
            get
            {
                return (int)SettingsHelper.getIntValue("LastDownloadIdHid");
            }
            set
            {
                SettingsHelper.Save("LastDownloadIdHid", value);
            }
        }

        public static string LastDownloadedShowID
        {
            get
            {
                return SettingsHelper.getStringValue("lastDownloadedMovieId");
            }
            set
            {
                SettingsHelper.Save("lastDownloadedMovieId", value);
            }
        }
        public static int LastDownloadIdFav
        {
            get
            {
                return (int)SettingsHelper.getIntValue("LastDownloadIdFav");
            }
            set
            {
                SettingsHelper.Save("LastDownloadIdFav", value);
            }
        }
        public static string DownloadFolderName
        {
            get
            {
                return SettingsHelper.getStringValue("DownloadFolderName");
            }
            set
            {
                SettingsHelper.Save("DownloadFolderName", value);
            }
        }

        public static bool IsDownloadFolder
        {
            get
            {
                return SettingsHelper.getBoolValue("DownloadFolder");
            }
            set
            {
                SettingsHelper.Save("DownloadFolder", value);
            }
        }

        public static string BackgroundAgenError
        {
            get
            {
                return SettingsHelper.getStringValue("BackgroundAgenError");
            }
            set
            {
                SettingsHelper.Save("BackgroundAgenError", value);
            }
        }

        public static string People
        {
            get
            {
                return SettingsHelper.getStringValue("People");
            }
            set
            {
                SettingsHelper.Save("People", value);
            }
        }

        public static bool NavigationID
        {
            get
            {
                return SettingsHelper.getBoolValue("NavigationID");
            }
            set
            {
                SettingsHelper.Save("NavigationID", value);
            }
        }

        public static bool ParentalControl
        {
            get
            {
                string szParentalControl = SettingsHelper.getStringValue("Parental Control");
                return szParentalControl == "true" ? true : false;

            }
            set
            {
                SettingsHelper.Save("Parental Control", value);
            }
        }

        public static bool bcount
        {
            get
            {
                return SettingsHelper.getBoolValue("IsPopupOpen");

            }
            set
            {
                SettingsHelper.Save("IsPopupOpen", value);
            }
        }

        public static int detailtocast
        {
            get
            {
                int? nav = SettingsHelper.getIntValue("NavigationFromDetailToCast");
                return nav.HasValue ? nav.Value : 0;
            }
            set
            {
                SettingsHelper.Save("NavigationFromDetailToCast", value);
            }
        }

        public static bool popupcount
        {
            get
            {
                return SettingsHelper.getBoolValue("IsLoginPopupOpen");
            }
            set
            {
                SettingsHelper.Save("IsLoginPopupOpen", value);
            }
        }

        public static string ParentalControlPassword
        {
            get
            {
                return SettingsHelper.getStringValue("Password");
            }
            set
            {
                SettingsHelper.Save("Password", value);
            }
        }
        public static string ShowLinkTitle
        {
            get
            {
                return SettingsHelper.getStringValue("ShowLinkTitle");
            }
            set
            {
                SettingsHelper.Save("ShowLinkTitle", value);
            }
        }
        public static string AudioTitle
        {
            get
            {
                return SettingsHelper.getStringValue("AudioTitle");
            }
            set
            {
                SettingsHelper.Save("AudioTitle", value);
            }
        }
        public static int SubjectId
        {
            get
            {
                int? SubId = SettingsHelper.getIntValue("SubjectId");
                return SubId.HasValue ? SubId.Value : 0;
            }
            set
            {
                SettingsHelper.Save("SubjectId", value);
            }
        }
        public static string ShowQuizId
        {
            get
            {
                return SettingsHelper.getStringValue("QuizID");

            }
            set
            {
                SettingsHelper.Save("QuizID", value);
            }
        }
        public static string ShowQuestionId
        {
            get
            {
                return SettingsHelper.getStringValue("QuestionId");


            }
            set
            {
                SettingsHelper.Save("QuestionId", value);
            }
        }
        public static string ShowMaxQuestionId
        {
            get
            {
                return SettingsHelper.getStringValue("MaxQuestionId");


            }
            set
            {
                SettingsHelper.Save("MaxQuestionId", value);
            }
        }
        public static string GameClassName
        {
            get
            {
                return SettingsHelper.getStringValue("GameClassName");
            }
            set
            {
                SettingsHelper.Save("GameClassName", value);
            }
        }
        public static string TitleVisibleForVideos
        {
            get
            {
                return SettingsHelper.getStringValue("TitleVisibleForVideos");
            }
            set
            {
                SettingsHelper.Save("TitleVisibleForVideos", value);
            }
        }

        public static string GameGalCount
        {
            get
            {
                return SettingsHelper.getStringValue("GameGalCount");
            }
            set
            {
                SettingsHelper.Save("GameGalCount", value);
            }
        }
        public static string GamePopUpVisible
        {
            get
            {
                return SettingsHelper.getStringValue("GamePopUpVisible");
            }
            set
            {
                SettingsHelper.Save("GamePopUpVisible", value);
            }
        }
        public static string linkRating
        {
            get
            {
                return SettingsHelper.getStringValue("linkRating");
            }
            set
            {
                SettingsHelper.Save("linkRating", value);
            }
        }

        public static string PopupGridTap
        {
            get
            {
                return SettingsHelper.getStringValue("PopupGridTap");
            }
            set
            {
                SettingsHelper.Save("PopupGridTap", value);
            }
        }
        public static string LinkID
        {
            get
            {
                return SettingsHelper.getStringValue("LinkID");
            }
            set
            {
                SettingsHelper.Save("LinkID", value);
            }
        }
        public static string LinkTitle
        {
            get
            {
                return SettingsHelper.getStringValue("LinkTitle");
            }
            set
            {
                SettingsHelper.Save("LinkTitle", value);
            }
        }
        public static string SnapQuestionID1
        {
            get
            {
                return SettingsHelper.getStringValue("SnapQuestionID1");
            }
            set
            {
                SettingsHelper.Save("SnapQuestionID1", value);
            }
        }
        public static string SnapQuestionID2
        {
            get
            {
                return SettingsHelper.getStringValue("SnapQuestionID2");
            }
            set
            {
                SettingsHelper.Save("SnapQuestionID2", value);
            }
        }
        public static string SnapQuestionID3
        {
            get
            {
                return SettingsHelper.getStringValue("SnapQuestionID3");
            }
            set
            {
                SettingsHelper.Save("SnapQuestionID3", value);
            }
        }
        public static string QuizQuestNoList
        {
            get
            {
                return SettingsHelper.getStringValue("QuizQuestNoList");
            }
            set
            {
                SettingsHelper.Save("QuizQuestNoList", value);
            }
        }



        public static string RegstrationDate
        {
            get
            {
                return SettingsHelper.getStringValue("RegstrationDate");
            }
            set
            {
                SettingsHelper.Save("RegstrationDate", value);
            }
        }
        public static double TotalSumForSalesPrice
        {
            get
            {
                return SettingsHelper.getDouableValue("TotalSumForSalesPrice");
            }
            set
            {
                SettingsHelper.Save("TotalSumForSalesPrice", value);
            }
        }
        public static double CompareMonth
        {
            get
            {
                return SettingsHelper.getDouableValue("ComparePrincipale");
            }
            set
            {
                SettingsHelper.Save("ComparePrincipale", value);
            }
        }
        public static double LoanAmount
        {
            get
            {
                return SettingsHelper.getDouableValue("LoanAmount");
            }
            set
            {
                SettingsHelper.Save("LoanAmount", value);
            }
        }
        public static double IntrestRate
        {
            get
            {
                return SettingsHelper.getDouableValue("IntrestRate");
            }
            set
            {
                SettingsHelper.Save("IntrestRate", value);
            }
        }

        public static double TermOfLoan
        {
            get
            {
                return SettingsHelper.getDouableValue("TermOfLoan");
            }
            set
            {
                SettingsHelper.Save("TermOfLoan", value);
            }
        }
        public static double Price
        {
            get
            {
                return SettingsHelper.getDouableValue("Price");
            }
            set
            {
                SettingsHelper.Save("Price", value);
            }
        }
        public static double AgentCommission
        {
            get
            {
                return SettingsHelper.getDouableValue("AgentCommission");
            }
            set
            {
                SettingsHelper.Save("AgentCommission", value);
            }
        }
        public static double AgentCommissionHouse
        {
            get
            {
                return SettingsHelper.getDouableValue("AgentCommissionHouse");
            }
            set
            {
                SettingsHelper.Save("AgentCommissionHouse", value);
            }
        }
     
        public static double LoanTream
        {
            get
            {
                return SettingsHelper.getDouableValue("LoanTream");
            }
            set
            {
                SettingsHelper.Save("LoanTream", value);
            }
        }
        public static double HomeCost
        {
            get
            {
                return SettingsHelper.getDouableValue("HomeCost");
            }
            set
            {
                SettingsHelper.Save("HomeCost", value);
            }
        }
        public static int DiffMonths
        {
            get
            {
                return (int)SettingsHelper.getIntValue("DiffMonths");
            }
            set
            {
                SettingsHelper.Save("DiffMonths", value);
            }
        }

        public static bool PasswordToggle
        {
            get
            {
                return SettingsHelper.getBoolValue("PasswordToggle");
            }
            set
            {
                SettingsHelper.Save("PasswordToggle", value);
            }
        }
        public static bool Parentalappbarclick
        {
            get
            {
                return SettingsHelper.getBoolValue("Parentalappbarclick");
            }
            set
            {
                SettingsHelper.Save("Parentalappbarclick", value);
            }
        }
        #region HomeBuyerTools
        public static string FromDate
        {
            get
            {
                return SettingsHelper.getStringValue("FromDate");
            }
            set
            {
                SettingsHelper.Save("FromDate", value);
            }
        }
        public static string EFromDate
        {
            get
            {
                return SettingsHelper.getStringValue("EFromDate");
            }
            set
            {
                SettingsHelper.Save("EFromDate", value);
            }
        }
        public static double TotalAgentCommission
        {
            get
            {
                return SettingsHelper.getDouableValue("TotalAgentCommission");
            }
            set
            {
                SettingsHelper.Save("TotalAgentCommission", value);
            }
        }
        public static double intialcoast
        {
            get
            {
                return SettingsHelper.getDouableValue("intialcoast");
            }
            set
            {
                SettingsHelper.Save("intialcoast", value);
            }
        }
       
        public static double InterestRates
        {
            get
            {
                return SettingsHelper.getDouableValue("InterestRates");
            }
            set
            {
                SettingsHelper.Save("InterestRates", value);
            }
        }
        public static double TermofLoan
        {
            get
            {
                return SettingsHelper.getDouableValue("TermofLoan");
            }
            set
            {
                SettingsHelper.Save("TermofLoan", value);
            }
        }
        public static string ToDate
        {
            get
            {
                return SettingsHelper.getStringValue("ToDate");
            }
            set
            {
                SettingsHelper.Save("ToDate", value);
            }
        }
        public static string EToDate
        {
            get
            {
                return SettingsHelper.getStringValue("EToDate");
            }
            set
            {
                SettingsHelper.Save("EToDate", value);
            }
        }
#if WINDOWS_APP
        public static string htmltext
        {
            get
            {
                return SettingsHelper.getStringValue("htmltext");
            }
            set
            {
                SettingsHelper.Save("htmltext", value);
            }
        } 
#endif
        #endregion

#if WINDOWS_PHONE_APP
        public static string RatingUserName
        {
            get
            {
                return SettingsHelper.getStringValue("RatingUserName");
            }
            set
            {
                SettingsHelper.Save("RatingUserName", value);
            }
        }
        public static string RatingPassword
        {
            get
            {
                return SettingsHelper.getStringValue("RatingPassword");
            }
            set
            {
                SettingsHelper.Save("RatingPassword", value);
            }
        }
        public static string ShowsRatingBlogUrl
        {
            get
            {
                return SettingsHelper.getStringValue("ShowsRatingBlogUrl");
            }
            set
            {
                SettingsHelper.Save("ShowsRatingBlogUrl", value);
            }
        }
        public static string QuizRatingBlogUrl
        {
            get
            {
                return SettingsHelper.getStringValue("QuizRatingBlogUrl");
            }
            set
            {
                SettingsHelper.Save("QuizRatingBlogUrl", value);
            }
        }
        public static string LinksRatingBlogUrl
        {
            get
            {
                return SettingsHelper.getStringValue("LinksRatingBlogUrl");
            }
            set
            {
                SettingsHelper.Save("LinksRatingBlogUrl", value);
            }
        }
        public static string ShowsRatingBlogName
        {
            get
            {
                return SettingsHelper.getStringValue("ShowsRatingBlogName");
            }
            set
            {
                SettingsHelper.Save("ShowsRatingBlogName", value);
            }
        }
        public static string QuizLinksRatingBlogName
        {
            get
            {
                return SettingsHelper.getStringValue("QuizLinksRatingBlogName");
            }
            set
            {
                SettingsHelper.Save("QuizLinksRatingBlogName", value);
            }
        }
        public static string LinksRatingBlogName
        {
            get
            {
                return SettingsHelper.getStringValue("LinksRatingBlogName");
            }
            set
            {
                SettingsHelper.Save("LinksRatingBlogName", value);
            }
        }
        public static string htmltext
        {
            get
            {
                return SettingsHelper.getStringValue("htmltext");
            }
            set
            {
                SettingsHelper.Save("htmltext", value);
            }
        }
        public static Dictionary<string, string> ShowTableCondition
        {
            get
            {
                return SettingsHelper.GetDictionaryValue("ShowTableCondition");
            }
            set
            {
                SettingsHelper.SaveDictionaryValue("ShowTableCondition", value);
            }
        }

        public static Dictionary<string, string> ShowTableCondition_New
        {
            get;

            set;
        }
        
        public static Dictionary<string, string> PeopleTableCondition
        {
            get
            {
                return SettingsHelper.GetDictionaryValue("PeopleTableCondition");
            }
            set
            {
                SettingsHelper.SaveDictionaryValue("PeopleTableCondition", value);
            }
        }
        public static Dictionary<string, string> PeopleTableCondition_New
        {
            get;
            set;
        }
        public static Dictionary<string, string> PeopleTableConditions
        {
            get
            {
                return SettingsHelper.GetDictionaryValue("PeopleTableConditions");
            }
            set
            {
                SettingsHelper.SaveDictionaryValue("PeopleTableConditions", value);
            }
        }
        public static Dictionary<string, string> PeopleTableConditions_New
        {
            get;
            set;
        }
        public static List<string> DeletedTableNames
        {
            get
            {
                return SettingsHelper.GetListValue("DeletedTableNames");
            }
            set
            {
                SettingsHelper.SaveListValue("DeletedTableNames", value);
            }
        }

        public static List<string> DeletedTableNames_New
        {
            get;
            set;
        }

        public static List<string> CheckedTableNames
        {
            get
            {
                return SettingsHelper.GetListValue("CheckedTableNames");
            }
            set
            {
                SettingsHelper.SaveListValue("CheckedTableNames", value);
            }
        }
        public static List<string> PeopleTableNames
        {
            get
            {
                return SettingsHelper.GetListValue("PeopleTableNames");
            }
            set
            {
                SettingsHelper.SaveListValue("PeopleTableNames", value);
            }
        }
        
        public static Dictionary<string, string> ShowTableConditions
        {
            get
            {
                return SettingsHelper.GetDictionaryValue("ShowTableConditions");
            }
            set
            {
                SettingsHelper.SaveDictionaryValue("ShowTableConditions", value);
            }
        }
        public static Dictionary<string, string> ShowTableConditions_New
        {
            get;
            set;
        }
        public static List<string> ShowTableNames
        {
            get
            {
                return SettingsHelper.GetListValue("ShowTableNames");
            }
            set
            {
                SettingsHelper.SaveListValue("ShowTableNames", value);
            }
        }

        public static List<string> ShowTableNames_New
        {
            get;
            set;
        }
        
        public static List<string> PeopleTableNames_New
        {
            get;
            set;
        }
        public static string LinkId
        {
            get
            {
                return SettingsHelper.getStringValue("LinkId");
            }

            set
            {
                SettingsHelper.Save("LinkId", value);
            }
        }

#if ANDROID
		public static bool DataBaseFlag
		{
			get{return SettingsHelper.getBoolValue ("DataBaseFlag"); }
			set{SettingsHelper.Save ("DataBaseFlag", value); }
		}
		private static String _Timelimt;
		public static String Timelimt
		{
			get{return _Timelimt;}
			set{ _Timelimt = value;}
		}

#endif
#endif

#if WINDOWS_APP && NOTANDROID
        public static Dictionary<string, string> ShowTableCondition
        {
            get
            {
                StorageHelper<Dictionary<string, string>> help = new StorageHelper<Dictionary<string, string>>(StorageType.Local);
                return Task.Run(async () => await help.LoadDictionaryASync("ShowTableCondition")).Result;

            }
            set
            {
                StorageHelper<Dictionary<string, string>> help = new StorageHelper<Dictionary<string, string>>(StorageType.Local);
                bool result = Task.Run(async () => await help.SaveDictionaryASync(value, "ShowTableCondition")).Result;

            }
        }
        public static Dictionary<string, string> PeopleTableCondition
        {
            get
            {
                StorageHelper<Dictionary<string, string>> help = new StorageHelper<Dictionary<string, string>>(StorageType.Local);
                return Task.Run(async () => await help.LoadDictionaryASync("PeopleTableCondition")).Result;
            }
            set
            {
                StorageHelper<Dictionary<string, string>> help = new StorageHelper<Dictionary<string, string>>(StorageType.Local);
                bool result = Task.Run(async () => await help.SaveDictionaryASync(value, "PeopleTableCondition")).Result;
            }
        }
        public static Dictionary<string, string> PeopleTableConditions
        {
            get
            {
                StorageHelper<Dictionary<string, string>> help = new StorageHelper<Dictionary<string, string>>(StorageType.Local);
                return Task.Run(async () => await help.LoadDictionaryASync("PeopleTableConditions")).Result;

            }
            set
            {
                StorageHelper<Dictionary<string, string>> help = new StorageHelper<Dictionary<string, string>>(StorageType.Local);
                bool result = Task.Run(async () => await help.SaveDictionaryASync(value, "PeopleTableConditions")).Result;
            }
        }
        public static List<string> DeletedTableNames
        {

            get
            {
                StorageHelper<List<string>> help = new StorageHelper<List<string>>(StorageType.Local);
                return Task.Run(async () => await help.LoadASync("DeletedTableNames")).Result;

            }
            set
            {
                StorageHelper<List<string>> help = new StorageHelper<List<string>>(StorageType.Local);
                if (Task.Run(async () => await Storage.FileExists("DeletedTableNames")).Result)
                {
                    List<string> PreviousList = Task.Run(async () => await help.LoadASync("DeletedTableNames")).Result;
                    foreach (string s in PreviousList)
                    {
                        value.Add(s);
                    }
                    bool result = Task.Run(async () => await help.SaveASync(value, "DeletedTableNames")).Result;
                }
                else
                {
                    bool result = Task.Run(async () => await help.SaveASync(value, "DeletedTableNames")).Result;
                }
            }
        }
        public static List<string> CheckedTableNames
        {
            get
            {
                StorageHelper<List<string>> help = new StorageHelper<List<string>>(StorageType.Local);
                return Task.Run(async () => await help.LoadASync("CheckedTableNames")).Result;

            }
            set
            {
                StorageHelper<List<string>> help = new StorageHelper<List<string>>(StorageType.Local);
                if (Task.Run(async () => await Storage.FileExists("CheckedTableNames")).Result)
                {
                    List<string> PreviousList = Task.Run(async () => await help.LoadASync("CheckedTableNames")).Result;
                    foreach (string s in PreviousList)
                    {
                        value.Add(s);
                    }
                    bool result = Task.Run(async () => await help.SaveASync(value, "CheckedTableNames")).Result;
                }
                else
                {
                    bool result = Task.Run(async () => await help.SaveASync(value, "CheckedTableNames")).Result;
                }

            }
        }
        public static List<string> PeopleTableNames
        {
            get
            {
                StorageHelper<List<string>> help = new StorageHelper<List<string>>(StorageType.Local);
                return Task.Run(async () => await help.LoadASync("PeopleTableNames")).Result;

            }
            set
            {
                StorageHelper<List<string>> help = new StorageHelper<List<string>>(StorageType.Local);
                if (Task.Run(async () => await Storage.FileExists("PeopleTableNames")).Result)
                {
                    List<string> PreviousList = Task.Run(async () => await help.LoadASync("PeopleTableNames")).Result;
                    foreach (string s in PreviousList)
                    {
                        value.Add(s);
                    }
                    bool result = Task.Run(async () => await help.SaveASync(value, "PeopleTableNames")).Result;
                }
                else
                {
                    bool result = Task.Run(async () => await help.SaveASync(value, "PeopleTableNames")).Result;
                }

            }
        }
        public static Dictionary<string, string> ShowTableConditions
        {
            get
            {
                StorageHelper<Dictionary<string, string>> help = new StorageHelper<Dictionary<string, string>>(StorageType.Local);
                return Task.Run(async () => await help.LoadDictionaryASync("ShowTableConditions")).Result;

            }
            set
            {
                StorageHelper<Dictionary<string, string>> help = new StorageHelper<Dictionary<string, string>>(StorageType.Local);
                bool result = Task.Run(async () => await help.SaveDictionaryASync(value, "ShowTableConditions")).Result;
            }
        }
        public static List<string> ShowTableNames
        {
            get
            {
                StorageHelper<List<string>> help = new StorageHelper<List<string>>(StorageType.Local);
                return Task.Run(async () => await help.LoadASync("ShowTableNames")).Result;

            }
            set
            {
                StorageHelper<List<string>> help = new StorageHelper<List<string>>(StorageType.Local);
                if (Task.Run(async () => await Storage.FileExists("ShowTableNames")).Result)
                {
                    List<string> PreviousList = Task.Run(async () => await help.LoadASync("ShowTableNames")).Result;
                    foreach (string s in PreviousList)
                    {
                        value.Add(s);
                    }
                    bool result = Task.Run(async () => await help.SaveASync(value, "ShowTableNames")).Result;
                }
                else
                {
                    bool result = Task.Run(async () => await help.SaveASync(value, "ShowTableNames")).Result;
                }

            }
        }
#endif
    }
}
