using System;
using OnlineVideos.Entities;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;
#if WINDOWS_APP
using Windows.Storage.Streams;
using Windows.Foundation;
#endif
#if WINDOWS_PHONE_APP
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
using System.Text;
#endif
#if WINDOWS_APP && NOTANDROID
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
#endif
#if NOTANDROID
using Windows.Foundation;
using Windows.Storage.Streams;
using System.Text;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml;
using Windows.Media.SpeechSynthesis;
#endif
#if WP8 && NOTANDROID
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using Windows.Phone.Speech.Synthesis;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
#endif

namespace Common.Library
{
    public enum CategoryId
    {
        hindi = 20,
        telugu = 18,
        tamil = 19
    };
    public enum CreateFolderUsingShowId
    {
        empty1,
        FlickrStoryImageUrl,
        FlickrQuizImageUrl
    };

    public enum ShowImages
    {
#if WINDOWS_APP && NOTANDROID
        empty,
		[ConditionTypeAttribute(".jpg", "Images\\PersonImages\\")]
		FlickrPersonImageUrl,
		[ConditionTypeAttribute(".jpg", "Images\\scale-100\\")]
		Scale100,
		[ConditionTypeAttribute(".jpg", "Images\\scale-140\\")]
		Scale140,
		[ConditionTypeAttribute(".jpg", "Images\\scale-180\\")]
		Scale180,
		[ConditionTypeAttribute(".jpg", "Images\\ListImages\\")]
		ListImages,
		[ConditionTypeAttribute(".jpg", "Images\\TileImages\\30-30\\")]
		Tile30_30,
		[ConditionTypeAttribute(".jpg", "Images\\TileImages\\150-150\\")]
		Tile150_150,
		[ConditionTypeAttribute(".jpg", "Images\\TileImages\\310-150\\")]
		Tile310_150,
		[ConditionTypeAttribute(".jpg", "Images\\storyImages\\")]
		FlickrStoryImageUrl,
		[ConditionTypeAttribute(".jpg", "Images\\QuestionsImage\\")]
		FlickrQuizImageUrl,
#endif
#if WINDOWS_PHONE_APP && NOTANDROID
        empty,
        [ConditionTypeAttribute(".jpg", "Images\\")]
        FlickrImageUrl,
        //[ConditionTypeAttribute(".jpg", "Images\\PersonImages\\")]
        //FlickrPersonImageUrl,
        //empty,
        //[ConditionTypeAttribute("", "SendImageBytesToAppCompleted")]
        //FlickrImageUrl,
        //[ConditionTypeAttribute("2", "SendPovitImageBytesToAppCompleted")]
        //FlickrPivotImageUrl,
        //[ConditionTypeAttribute("", "SendPeopleImageBytesToAppCompleted")]
        //FlickrPersonImageUrl,
        //[ConditionTypeAttribute("", "SaveStoryImageBytesToAppCompleted")]
        //FlickrStoryImageUrl,
        //[ConditionTypeAttribute("", "SaveQuizImageBytesToAppCompleted")]
        //FlickrQuizImageUrl,
#endif
#if ANDROID
		empty,
		[ConditionTypeAttribute("", "SendImageBytesToAppCompleted")]
		FlickrImageUrl,
		[ConditionTypeAttribute("2", "SendPovitImageBytesToAppCompleted")]
		FlickrPivotImageUrl,
		[ConditionTypeAttribute("", "SendPeopleImageBytesToAppCompleted")]
		FlickrPersonImageUrl,
		[ConditionTypeAttribute("", "SaveStoryImageBytesToAppCompleted")]
		FlickrStoryImageUrl,
		[ConditionTypeAttribute("", "SaveQuizImageBytesToAppCompleted")]
		FlickrQuizImageUrl,
#endif
    };
    public enum SyncAgentStatus
    {
        RestoreStory,
        DownloadFavourites,
        //DownloadParentalControlPreferences,
        DownloadPeople,
        UpdatePeople,
        DownloadShows,
        UpdateShows,
        //UpDateLiveTile,
        UploadStory,
        UploadFavourites,
        LiveTileUpdate,
        UpDateLiveTile,
        DownloadParentalControlPreferences,
        UploadParentalControlPreferences,
        //UploadParentalControlPreferences,
        UploadSearch,
        UploadRatingForShows,
        UploadRatingForVideos,
        DownloadRatingForShows,
        DownloadRatingForVideos,
        UpdateBlogUserNameandPassword,
        DeleteIsolatedFoldersAndFiles
    };

    public enum TeamType
    {
        TeamABatting,
        TeamABowling,
        TeamBBatting,
        TeamBBowling

    }
    public enum YouTubeQuality
    {

        Quality480P,
        Quality720P,
        Quality1080P,

    }
    public enum LinkType
    {
        Movies,
        Songs,
        Audio,
        Quiz,
        Comedy,
        Images
    }
    public enum Apps
    {
        Kids_TV_Shows,
        Kids_TV_Pro,
        Cricket_Videos,
        Bollywood_World,
        Bollywood_Movies,
        Video_Games,
        DownloadManger,
        Story_Time,
        Vedic_Library,
        DrivingTest,
        Driving_Exam,
        Recipe_Shows,
        Online_Education,
        Animation_Planet,
        Indian_Cinema,
        Bollywood_Music,
        Yoga_and_Health,
        Fitness_Programs,
        World_Dance,
        Web_Tile,
        Video_Mix,
        Kids_TV,
        Story_Time_Pro,
        Indian_Cinema_Pro,
        Social_Celebrities,
        Web_Media,
    }

    public enum FileSizeUnit
    {
        KB = 1024,
        MB = 1024 * 1024,
        GB = 1024 * 1024 * 1024,
        //TB = 1024 * 1024 * 1024 * 1024
    }

    public enum ApplicationStatus
    {
        Active,
        Deactive,
        Launching,
        Closing
    }

    public static class Constants
    {
#if NOTANDROID || WINDOWS_PHONE_APP         
        public static int downstate = 0;   
        public static int count = 0;
        public static bool editcelebrity = false;
        public static bool navigation = false;
        public static string backstack = string.Empty;
        public static BitmapImage UserImage1
        {
            get;
            set;
        }
        public static BitmapImage UserImage_New
        {
            get;
            set;
        }
        public static bool CommentPosted = false;
        public static bool popupopened = false;
        public static string navigationfrom = string.Empty;
        public static object[] SelectedSyndicationItem = new object[2];
        public static int navigationvalue = 1;
        public static int selectedindex = 0;
        public static string FinalUrl = string.Empty;
        public static string newimage = string.Empty;
        public static int ParaId = default(int);
        public static bool editstory = false;
        public static TextBlock errormsg = default(TextBlock);
        public static IDictionary<string, string> AudiosLinks = new Dictionary<string, string>();
        public static string personname = string.Empty;

        public static int personid = 0;
        public static string movietitle = string.Empty;
        public static string NavigatedUri = string.Empty;
        public static string Linkstype = string.Empty;
        public static string Lyrics = string.Empty;
        public static StringBuilder Description = new StringBuilder();
        public static UserControl usercontrol = default(UserControl);
        public static TextBlock networktblk = default(TextBlock);
        public static string searchtext = string.Empty;
                
#endif
#if WINDOWS_APP && NOTANDROID
        public static FlipView flip = default(FlipView);
#endif
        public static ObservableCollection<feedclass> Items = default(ObservableCollection<feedclass>);
        public static List<feedclass> feedclass = new List<feedclass>();
        private static string _CountryName = string.Empty;
        private static string _StateName = string.Empty;
        public static bool NavigationFromPhotoChooser = false;
        public static bool NavigationFromWebView = false;
        public static bool FavouritesMessageBox = false;
        public static bool NavigationFromOnlineLinks = false;
        public static bool LiveTileBackgroundAgentStatus = false;
        public static int StartPosition = default(int);
        public static int EndPosition = default(int);
        public static bool IsAudioPlaying = false;
        public static string mode = "";
        public static string _savehtml = "";
        public static string SelectedValue = string.Empty;
        public static List<Uri> _saveimages = new List<Uri>();
        public static List<Uri> _saveaudios = new List<Uri>();
        public static List<Uri> _savevideos = new List<Uri>();
        public static List<Uri> _savelinks = new List<Uri>();
        public static object ringtoneinstance = default(object);
        public static bool BackgroundDownloadStatus = false;
        private static ShowList _RenameLinkShowlistItem = null;
        public const string ParentTagForDownloadedXml = "NewDataSet";
        public const string NamespaceForData = "OnlineVideos.Entities";
#if NOTANDROID
        private static ImageSource _AppbarImage = null;
#endif
        private static string _AppbarTitle = null;
        private static TimeSpan mediaposition = new TimeSpan();
        private static Dictionary<string, string> PlayList = new Dictionary<string, string>();
        private static string _topsongnavigation = "";
        private static Dictionary<string, string> _YoutubePlayList = null;
        private static List<DownloadStatus> _DownloadList = new List<DownloadStatus>();
        private static ShowLinks _selecteditem = null;
        private static string _Link = null;
        private static int _VideofileSize = 0;
        public static Stream Downloadedstream = default(Stream);
#if NOTANDROID
        private static Page _Instance = null;        
#endif
        private static ObservableCollection<ShowLinks> _selectedMovielinklist = null;
        private static ObservableCollection<ShowLinks> _selecteditemshowlinklist = null;
        private static ObservableCollection<QuizList> _selecteditemquizlist = null;
        private static List<ShowLinks> _selecteditemshowlinkslist = null;
        private static List<ShowLinks> _selecteditemshowMovielinkslist = null;
        private static List<ShowLinks> _selecteditemshowAudiolinkslist = null;
        public static bool _VisiablehelpImage = false;
        private static List<ShowLinks> _TopSonList = null;
        private static List<ShowLinks> _selectedAllitemTelugushows = null;
        private static List<ShowLinks> _selectedAllitemTamilshows = null;
        private static DownloadPivot _RenameLinkItem = null;
        private static List<ShowList> _ViellList = new List<ShowList>();
        private static List<ShowLinks> _selectedAllitemHindishows = null;
        private static List<ShowLinks> _DownLadStatus = null;
        private static List<DownloadStatus> _DownLoadList = null;
        private static SQLite.SQLiteAsyncConnection _connection = default(SQLite.SQLiteAsyncConnection);
        public const string InstalledCustomizationXmlPath = "DefaultData/Customization.xml";
        public const string InstalledDatabasePath = "OnlineMoviesDb.sdf";
        public const string DatabaseConnectionString = "isostore:/OnlineMoviesDb.sdf";
        public const string CloudImagePath = "http://onlineimages.welcomeandhra.com/";
        public const string SecondaryTileImagePath = "Shared/ShellContent/secondary/";
        public static bool isEventRegistered = false;

#if NOTANDROID
        public static string DataBaseConnectionstringForSqlite = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "OnlineMoviesDb.sqlite").ToString();
#endif
#if ANDROID
		public static string DataBaseConnectionstringForSqlite = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),"OnlineMoviesDb.sqlite").ToString();
		private static int _SelectedSongPosition=0;
		public static int SelectedSongPosition
		{
			get{ return _SelectedSongPosition;}
			set{_SelectedSongPosition = value;}
		}
		private static int _SelectedSongStatus=0;
		public static int SelectedSongStatus
		{
			get{ return _SelectedSongStatus;}
			set{_SelectedSongStatus = value;}
		}
#endif
        public const string CompanyFacebookPageUrl = "http://www.facebook.com/lartsoftsolutions";
        public const string CompanyTwitterPageUrl = "http://twitter.com/lartsoft";
        public const string CompanyBlogPageUrl = "http://lartsofttechsolutions.blogspot.com/";
        public const string CompanyYoutubePageUrl = "http://www.youtube.com/view_play_list?p=4AF91C4A3D710D2C";
        public const string AppMarketplaceWebUrl = "http://www.windowsphone.com/en-US/apps/";
        public const string webtiletoprated = "top rated ";
        public const string webtilerecent = "recent ";
        public const string toprated = "top rated >";
        public const string getMoreLabel = "get more";
        public const string Hindi = "hindi >";
        public const string Telugu = "telugu >";
        public const string MovieCategoryEnglish = "English";
        public static List<DownloadStatus> _SaveDownloadList = new List<DownloadStatus>();
        public const string Tamil = "tamil >";
        public const string Recent = "recent >";
        public static string pintomovie = string.Empty;
        public static string AppbaritemVisbility = string.Empty;
        public const string DownloadList = "download list  >";
        public const string PlayImagePath = "ms-appx:///Images/ArrowImg.png";
        public const string SongPlayPath = "ms-appx:///Images/appbar.transport.pause.rest.png";
        public const string SongPausePath = "/Images/ArrowImg.png";
        public const string PlayerImagesPath = "/Images/PlayerImages/";
        public const string ImagePath = "/Images/";
        public const string PivotImagePath = "ms-appx:///Images/Pivot/";
        public const string PersonImagePath = "/Images/PersonImages/";
        public const string storyImagePath = "ms-appx:///Images/storyImages/";
        public const string storyImagePath_New = "/Images/storyImages/";
        public const string QuestionImagePath = "ms-appx:///Images/QuestionsImage/";
        public const string ListImagePath = "/Images/ListImages/";
        public const string DlVImagePath = "/Images/DownLoadVideoImages/";
        public const string PinToStartScreen = "pin to start page";
        public const string UnpinFromStartScreen = "unpin from start page";
        public const int PageSize = 10;
        public const string HelpMenuXmlPath = "DefaultData/HelpMenu.xml";
        public const string UpgradeXmlPath = "DefaultData/Upgrade.xml";
        public const string AboutUsMenuXmlPath = "About Us/Data.xml";
        public const string MainMenuXmlPath = "MainMenu/Data.xml";
        public const string FeedbackTopicsXmlPath = "About Us/FeedbackTopics.xml";
        public const string PlayerPauseImage = "/Images/PlayerImages/pause.png";
        public const string PlayerPlayImage = "/Images/PlayerImages/play.png";
        public const string SongSizeErrorMessage = "selected song should be less than 1 Mb";
        public const string NetworkErrorMessageForRingTone = "Network not available";
        public const string PreviewModeErrorMessageForRingTone = "cannot trim in preview mode";
        public const string SongDurationErrorMessageForRingTone = "song duration should not be more than 40 secs";
        public const string BeginTimeErrorMessageForRingTone = "select begin time first";
        public const string TimeErrorMessageForRingTone = "select begin and end time to preview";
        public const string TimeErrorMessageForSaveRingTone = "select begin and end time to save";
        public static string _TileImageTitle = string.Empty;
        public static string _TileImageUrl = string.Empty;
        public static bool _Gettileimage = false;

        public const string MovieCategoryHindi = "Hindi";
        public const string MovieCategoryTamil = "Tamil";
        public const string MovieCategoryTelugu = "Telugu";
        public static bool UIThread = false;
        public const string UTCFormat = "yyyy-MM-dd HH:mm:ss";
        public static int QuizId;
        public static int ShowID;
        public static bool _UpdatePlayListTitleAndDescrption = false;
        public const string WeaponImagePath = "/Images/WeaponImages/";
        public const string VehicleImagePath = "/Images/VehicleImages/";
        public static Uri baseUri;
        public static List<ShowLinks> objpropMovieChap = new List<ShowLinks>();
        public static List<QuizUserAnswers> Quizanswerlist = new List<QuizUserAnswers>();
        public static List<string> UserAnswerlist = new List<string>();
        public static List<string> CorrectAnswerlist = new List<string>();
        public static bool appbarvisible = false;
        public static bool Favoriteappbarvisible = false;
        private static bool _Downloadstatus = false;
        public static XDocument DownLoadHist = default(XDocument);
        private static ShowLinks _Favoritesselecteditem = null;
        private static LinkHistory _histroryselecteditem = null;
        private static QuizList _Quizselecteditem = null;
        public static bool editdescription = false;
        private static int _LinkID = 0;
#if NOTANDROID
        public static Popup CloseLyricspopup = new Popup();
        public static string SocialCelebrityId = string.Empty;
        private static ObservableCollection<string> _OnlineImageUrls = new ObservableCollection<string>();
        public static ObservableCollection<string> OnlineImageUrls
        {
            get
            {
                return _OnlineImageUrls;
            }
            set
            {
                _OnlineImageUrls = value;
            }
        }
        private static int _SelectedTextCat = 0;
        public static int SelectedTextCat
        {
            get
            {
                return _SelectedTextCat;
            }
            set
            {
                _SelectedTextCat = value;
            }
        }
        public static string frompagename
        {
            get{return frompagename;}
            set { frompagename = value; }
        }
        // public static GameWeapons _selecteditem1 = null;
        private static ObservableCollection<string> _OnlineImageUrls1 = new ObservableCollection<string>();
        public static ObservableCollection<string> OnlineImageUrls1
        {
            get
            {
                return _OnlineImageUrls1;
            }
            set
            {
                _OnlineImageUrls1 = value;
            }
        }
#endif

        // public static GameWeapons _selecteditem1 = null;
        public static string TopAudioItemsFilePath
        {
            get
            {
                return "DefaultData/ShowTopAudioLinks.xml";
            }
        }
        private static double _ScrollPosition = 0;
        public static double ScrollPosition
        {
            get
            {
                return _ScrollPosition;
            }
            set
            {
                _ScrollPosition = value;
            }
        }

        public static BitmapImage UserImage_socialcelebs
        {
            get;
            set;
        }
        public static string FacebookNID
        {
            get;
            set;
        }

        public static string TwitterScreenName
        {
            get;
            set;
        }
        public static string BlogAddress
        {
            get;
            set;
        }
        //public static TextBox TbCelebrity
        //{
        //    get;
        //    set;
        //}
        private static double _scrollwidth = 0;
        public static double scrollwidth
        {
            get
            {
                return _scrollwidth;
            }
            set
            {
                _scrollwidth = value;
            }
        }
        public static TimeSpan MediaElementPosition
        {
            get
            {
                return mediaposition;
            }
            set
            {
                mediaposition = value;
            }

        }
        private static double _GameGridColumnWidth = 0;
        public static double GameGridColumnWidth
        {
            get
            {
                return _GameGridColumnWidth;
            }
            set
            {
                _GameGridColumnWidth = value;
            }
        }
        private static bool _Mainpagerefresh = false;
        public static bool Mainpagerefresh
        {
            get
            {
                return _Mainpagerefresh;
            }
            set
            {
                _Mainpagerefresh = value;
            }
        }
        public static string StoryImagePathForDownloads
        {
            get
            {
                return "/Images/storyImages/";
            }
        }
        public static string QuizImagePathForDownloads
        {
            get
            {
                return "/Images/QuestionsImage/";
            }
        }
        private static bool _VideoMixHelpLine = false;
        public static bool VideoMixHelpLine
        {
            get
            {
                return _VideoMixHelpLine;
            }
            set
            {
                _VideoMixHelpLine = value;
            }
        }
        private static bool _VideoMixHelpLineVisibility = false;
        public static bool VideoMixHelpLineVisibility
        {
            get
            {
                return _VideoMixHelpLineVisibility;
            }
            set
            {
                _VideoMixHelpLineVisibility = value;
            }
        }
        public static Dictionary<string, string> _PlayList
        {
            get
            {
                return PlayList;
            }
            set
            {
                PlayList = value;
            }
        }

        public static string bingsearch(string projectname)
        {
            if (ResourceHelper.AppName == "Yoga_&_Health")
            {
                projectname = "Yoga_and_Health";
            }
            if (ResourceHelper.AppName == "Driving_Exam")
            {
                projectname = "DrivingTest";
            }
            if (ResourceHelper.AppName == "Indian_Cinema.WindowsPhone" || ResourceHelper.AppName=="Indian_Cinema.Windows")
            {
                return "bWLUHaFy/isiNPxzdwKesuTrCL/KP19RQaGMlw+wEMw=";
            }
            if (ResourceHelper.AppName == "Online_Education.WindowsPhone" || ResourceHelper.AppName == "Online_Education.Windows")
            {
                return "fcT6h878b+gXZjkp8Z8R818YYQ2DHv18l4BAZn82qys=";
            }
            if (ResourceHelper.AppName == "DrivingTest.WindowsPhone" || ResourceHelper.AppName == "DrivingTest.Windows")
            {
                return "LJiC7PkqTfAQQnVpRJgFqtvwIVlWio0xTpAajIdNevY=";
            }
            if (ResourceHelper.AppName == "Kids_TV.WindowsPhone" || ResourceHelper.AppName == "Kids_TV.Windows")
            {
                return "p4twcfHfRVHF3nhgnxtNE4lVSk8kpJjEW58LAQQwwFU=";
            }
            switch ((Apps)Enum.Parse(typeof(Apps), projectname, true))
            {
                case Apps.Online_Education:
                    return "fcT6h878b+gXZjkp8Z8R818YYQ2DHv18l4BAZn82qys=";
                case Apps.Animation_Planet:
                    return "EnT1VjXPePD60Zcjq4H36MwTmP5c1robJW4PwhjQCv8=";
                case Apps.Bollywood_Movies:
                    return "pz6dNMBMCTmrK+GUQCPuT/63+YAEyn6hFKr1wYq3WfE=";
                case Apps.Bollywood_World:
                    return "4o7s5JZoWyQykjpg+wHEUR+X3/TzskPKzAsHqjONiyU=";
                case Apps.Cricket_Videos:
                    return "c/rvceb1YYet98SZ/34OY08CyC19svZ/BbpBuW8OnWs=";
                case Apps.DrivingTest:
                    return "LJiC7PkqTfAQQnVpRJgFqtvwIVlWio0xTpAajIdNevY=";
                case Apps.Fitness_Programs:
                    return "80qL3wURrTVIzcMCwyMkce3M7LKN87RKG50l1lcbp1g=";
                case Apps.Indian_Cinema:
                    return "bWLUHaFy/isiNPxzdwKesuTrCL/KP19RQaGMlw+wEMw=";
                case Apps.Kids_TV_Shows:
                    return "p4twcfHfRVHF3nhgnxtNE4lVSk8kpJjEW58LAQQwwFU=";
                case Apps.Kids_TV:
                    return "p4twcfHfRVHF3nhgnxtNE4lVSk8kpJjEW58LAQQwwFU=";
                case Apps.Recipe_Shows:
                    return "OocfHyaByAk18Z1jr+u9QYJ3w0zQgsQie7GwVA7l5mk=";
                case Apps.Story_Time:
                    return "yIPLbfROAN2g11ICrds8y8ZJib8nBqVcBOr4MNtIO14=";
                case Apps.Vedic_Library:
                    return "LGjJReIuzf53Q0paYBLyWohbZXRBuSkU0WWk6cvNWdM=";
                case Apps.Video_Games:
                    return "bMl5yAX0z7cxyRd7x+uHHsFlUdxIhH1vaDP729oVJ6Q=";
                case Apps.World_Dance:
                    return "4tI2OQ8pWryzk4iYSc3VLxsQ7ikR6wYuGubNXw17WaE=";
                case Apps.Video_Mix:
                    return "4tI2OQ8pWryzk4iYSc3VLxsQ7ikR6wYuGubNXw17WaE=";
                case Apps.Yoga_and_Health:
                    return "cXmcl2XTuxggZpWdASEju6vyc4RXT7Gxy1vBzaMhB/c=";
                case Apps.Bollywood_Music:
                    return "bWLUHaFy/isiNPxzdwKesuTrCL/KP19RQaGMlw+wEMw=";
                case Apps.Indian_Cinema_Pro:
                    return "bWLUHaFy/isiNPxzdwKesuTrCL/KP19RQaGMlw+wEMw=";
                case Apps.Kids_TV_Pro:
                    return "p4twcfHfRVHF3nhgnxtNE4lVSk8kpJjEW58LAQQwwFU=";
                case Apps.Story_Time_Pro:
                    return "yIPLbfROAN2g11ICrds8y8ZJib8nBqVcBOr4MNtIO14=";
                default:
                    return string.Empty;
            }
        }
        public static bool DownloadStatus
        {
            get
            {
                return _Downloadstatus;
            }
            set
            {
                _Downloadstatus = value;
            }
        }
        public static List<DownloadStatus> SaveDownloadList
        {
            get
            {
                return _SaveDownloadList;
            }
            set
            {
                _SaveDownloadList = value;
            }
        }
        public static string ExceptionHistoryXmlPath
        {
            get
            {
                return ResourceHelper.ProjectName + "/DetailedException.xml";
            }
        }
#if NOTANDROID
        public static ImageSource AppbarImage
        {
            get
            {
                return _AppbarImage;
            }
            set
            {
                _AppbarImage = value;
            }
        }
#endif
        public static string AppbarTitle
        {
            get
            {
                return _AppbarTitle;
            }
            set
            {
                _AppbarTitle = value;
            }
        }
        public static bool UpdatePlayListTitleAndDescrption
        {
            get
            {
                return _UpdatePlayListTitleAndDescrption;
            }
            set
            {
                _UpdatePlayListTitleAndDescrption = value;
            }
        }
        public static string CountryName
        {
            get
            {
                return _CountryName;
            }
            set
            {
                _CountryName = value;
            }
        }
        public static string StateName
        {
            get
            {
                return _StateName;
            }
            set
            {
                _StateName = value;
            }
        }
        public static string topsongnavigation
        {
            get
            {
                return _topsongnavigation;
            }
            set
            {
                _topsongnavigation = value;
            }
        }


        public static Dictionary<string, string> YoutubePlayList
        {
            get
            {
                return _YoutubePlayList;
            }
            set
            {
                _YoutubePlayList = value;
            }
        }
        public static List<DownloadStatus> LoadDownloadList
        {
            get
            {
                return _DownloadList;
            }
            set
            {
                _DownloadList = value;
            }
        }
        public static int VideofileSize
        {
            get
            {
                return _VideofileSize;
            }
            set
            {
                _VideofileSize = value;
            }
        }
        public static ShowLinks selecteditem
        {
            get
            {
                return _selecteditem;
            }
            set
            {
                _selecteditem = value;
            }
        }
        public static string Link
        {
            get
            {
                return _Link;
            }
            set
            {
                _Link = value;
            }
        }
#if NOTANDROID
        public static Page Instance
        {
            get
            {
                return _Instance;
            }
            set
            {
                _Instance = value;
            }
        }       
#endif
        public static int LinkID
        {
            get
            {
                return _LinkID;
            }
            set
            {
                _LinkID = value;
            }
        }
        public static ObservableCollection<ShowLinks> selectedMovielinklist
        {
            get
            {
                return _selectedMovielinklist;
            }
            set
            {
                _selectedMovielinklist = value;
            }
        }
        public static bool VisiablehelpImage
        {
            get
            {
                return _VisiablehelpImage;
            }
            set
            {
                _VisiablehelpImage = value;
            }
        }
        public static ObservableCollection<ShowLinks> selecteditemshowlinklist
        {
            get
            {
                return _selecteditemshowlinklist;
            }
            set
            {
                _selecteditemshowlinklist = value;
            }
        }
        public static ObservableCollection<QuizList> selecteditemquizlist
        {
            get
            {
                return _selecteditemquizlist;
            }
            set
            {
                _selecteditemquizlist = value;
            }
        }
        public static List<ShowLinks> selecteditemshowlinkslist
        {
            get
            {
                return _selecteditemshowlinkslist;
            }
            set
            {
                _selecteditemshowlinkslist = value;
            }
        }
        public static List<ShowLinks> selecteditemshowAudiolinkslist
        {
            get
            {
                return _selecteditemshowAudiolinkslist;
            }
            set
            {
                _selecteditemshowAudiolinkslist = value;
            }
        }
        public static List<ShowLinks> selecteditemshowMovielinkslist
        {
            get
            {
                return _selecteditemshowMovielinkslist;
            }
            set
            {
                _selecteditemshowMovielinkslist = value;
            }
        }
        public static List<ShowLinks> TopSonList
        {
            get
            {
                return _TopSonList;
            }
            set
            {
                _TopSonList = value;
            }
        }

        public static List<DownloadStatus> DownLoadList
        {
            get
            {
                return _DownLoadList;
            }
            set
            {
                _DownLoadList = value;
            }
        }
        public static List<ShowLinks> selectedAllitemTamilshows
        {
            get
            {
                return _selectedAllitemTamilshows;
            }
            set
            {
                _selectedAllitemTamilshows = value;
            }
        }

        public static List<ShowList> ViellList
        {
            get
            {
                return _ViellList;
            }
            set
            {
                _ViellList = value;
            }
        }
        public static String Savehtml
        {
            get
            {
                return _savehtml;
            }
            set
            {
                _savehtml = value;
            }
        }

        public static List<Uri> SaveImages
        {
            get
            {
                return _saveimages;
            }
            set
            {
                _saveimages = value;
            }
        }
        public static List<Uri> SaveAudios
        {
            get
            {
                return _saveaudios;
            }
            set
            {
                _saveaudios = value;
            }
        }
        public static List<Uri> SaveVideos
        {
            get
            {
                return _savevideos;
            }
            set
            {
                _savevideos = value;
            }
        }
        public static List<Uri> SaveLinks
        {
            get
            {
                return _savelinks;
            }
            set
            {
                _savelinks = value;
            }
        }
        public static List<ShowLinks> selectedAllitemHindishows
        {
            get
            {
                return _selectedAllitemHindishows;
            }
            set
            {
                _selectedAllitemHindishows = value;
            }
        }
        public static List<ShowLinks> selectedAllitemTelugushows
        {
            get
            {
                return _selectedAllitemTelugushows;
            }
            set
            {
                _selectedAllitemTelugushows = value;
            }
        }
        public static List<ShowLinks> DownLadStatus
        {
            get
            {
                return _DownLadStatus;
            }
            set
            {
                _DownLadStatus = value;
            }
        }

        public static SQLite.SQLiteAsyncConnection connection
        {
            get
            {
                return _connection;

            }
            set
            {
                _connection = value;
            }
        }
        
        public static string SaveImageHistoryFile
        {
            get
            {
                return "SaveImageHistory.xml";
            }
        }
        public static string SongHistoryFile
        {
            get
            {
                return AppSettings.ProjectName + "SaveSongHistory.xml";
            }
        }
        public static string ComedyHistoryFile
        {
            get
            {
                return "SaveComedyHistory.xml";
            }
        }
        public static string AudioHistoryFile
        {
            get
            {
                //return AppSettings.ProjectName + "/SaveSongHistory.xml";
                return "SaveAudioHistory.xml";
            }
        }
        public static string VideoHistoryFile
        {
            get
            {
                return "SaveVideoHistory.xml";
            }
        }
        public static string MovieHistoryFile
        {
            get
            {
                return "SaveMovieHistory.xml";
            }
        }
        public static string HelpMenuItemsFilePath
        {
            get
            {
                return "DefaultData/Help.xml";
            }
        }

        public static string PersonImagePathForDownloads
        {
            get
            {
                return "/" + "/Images/PersonImages/";
            }
        }


        public static string FrontImageFilePathForPrimaryTile
        {
            get
            {
                return "Shared/ShellContent/" + ResourceHelper.ProjectName + "/Primary/";
            }
        }

        public static string PanoramaImagePathForDownloads
        {
            get
            {
                return "/" + ResourceHelper.ProjectName + "/Images/PersonImages/Panorama/";
            }
        }

        public static string ImagePathForDownloads
        {
            get
            {
                return "/" + ResourceHelper.ProjectName + "/Images/";
            }
        }
        public static string PivotImagePathForDownloads
        {
            get
            {
                return "/" + ResourceHelper.ProjectName + "/Images/Pivot/";
            }
        }


        public static string BackgroundImageForTile
        {
            get
            {
                return "/Shared/ShellContent/" + ResourceHelper.ProjectName + "/Primary/";
            }
        }

        public static string BackBackgroundImageForTile
        {
            get
            {
                return "/Shared/ShellContent/" + ResourceHelper.ProjectName + "/Secondary/";
            }
        }

        public static QuizList Quizselecteditem
        {
            get
            {
                return _Quizselecteditem;
            }
            set
            {
                _Quizselecteditem = value;
            }
        }

        public static ShowLinks Favoritesselecteditem
        {
            get
            {
                return _Favoritesselecteditem;
            }
            set
            {
                _Favoritesselecteditem = value;
            }
        }
#if WINDOWS_PHONE_APP  && NOTANDROID
        public static bool NavigatedToBrowser = false;
        private static DispatcherTimer _downloadtimer = null;
        public static DispatcherTimer DownloadTimer
        {
            get { return _downloadtimer; }
            set { _downloadtimer = value; }
        }
        private static SpeechSynthesizer _synthesizer = null;
        public static SpeechSynthesizer Synthesizer
        {
            get { return _synthesizer; }
            set { _synthesizer = value; }
        }         
#endif
        public static LinkHistory histroryselecteditem
        {
            get
            {
                return _histroryselecteditem;
            }
            set
            {
                _histroryselecteditem = value;
            }
        }
#if WINDOWS_APP  || NOTANDROID
        public static IRandomAccessStream Imagestream = default(InMemoryRandomAccessStream);
		public static Rect ImageRectangle = default(Rect);
#endif
        public static string BasicUri = string.Empty;
        public static double PreviousImageWidth = default(double);
        public static double PreviousImageHeight = default(double);
        public static double CurrentImageWidth = default(double);
        public static double CurrentImageHeight = default(double);
        public static Stream UserImage { get; set; }
        public static string WebSliceTitle { get; set; }
        public static string WebSliceImagePath { get; set; }
        private static ObservableCollection<string> _ImageUrls = new ObservableCollection<string>();
        public static ObservableCollection<string> ImageUrls
        {
            get
            {
                return _ImageUrls;
            }
            set
            {
                _ImageUrls = value;
            }
        }

        public static bool Gettileimage
        {
            get
            {
                return _Gettileimage;
            }
            set
            {
                _Gettileimage = value;
            }
        }
        public static string TileImageTitle
        {
            get
            {
                return _TileImageTitle;
            }
            set
            {
                _TileImageTitle = value;
            }
        }
        public static string TileImageUrl
        {
            get
            {
                return _TileImageUrl;
            }
            set
            {
                _TileImageUrl = value;
            }
        }

        public static ShowList RenameLinkShowlistItem
        {
            get { return _RenameLinkShowlistItem; }
            set { _RenameLinkShowlistItem = value; }
        }

        public static DownloadPivot RenameLinkItem
        {
            get { return _RenameLinkItem; }
            set { _RenameLinkItem = value; }
        }
        public static string _uploadshowrating;
        public static string Uploadshowrating
        {
            get { return _uploadshowrating; }
            set { _uploadshowrating = value; }
        }
        public static string _uploadlinksrating;
        public static string UploadLinksrating
        {
            get { return _uploadlinksrating; }
            set { _uploadlinksrating = value; }
        }
        public static string _uploadquizrating;
        public static string Uploadquizrating
        {
            get { return _uploadquizrating; }
            set { _uploadquizrating = value; }
        }
        public static string _username;
        public static string username
        {
            get { return _username; }
            set { _username = value; }
        }
        public static string _password;
        public static string password
        {
            get { return _password; }
            set { _password = value; }
        }
#if ANDROID
		private static Android.Media.MediaPlayer _mediaPlayer=null;
		public static Android.Media.MediaPlayer mediaPlayer
		{
			get{return _mediaPlayer;}
			set{_mediaPlayer = value; }
		}
		private static string _SpinnerSelectedItem="Select Category";
		public static string  SpinnerSelectedItem
		{
			get{return _SpinnerSelectedItem; }
			set{_SpinnerSelectedItem = value; }
		}
#endif
        public static string _mediaPlayerStatus;
        public static string mediaPlayerStatus
        {
            get { return _mediaPlayerStatus; }
            set { _mediaPlayerStatus = value; }

        }

#if ANDROID
		private static string _flurryApiKey=string.Empty;
		public static string FlurryApiKey
		{
			get{return _flurryApiKey; }
			set{_flurryApiKey = value; }
		}
		private static bool _timerStatus=false;
		public static bool TimerStatus
		{
			get{return _timerStatus; }
			set{_timerStatus = value; }
		}
		private static System.Timers.Timer _timer=default(System.Timers.Timer);
		public static System.Timers.Timer Timer
		{
			get{ return _timer;}
			set{_timer = value; }
		}
		private static Stream _databaseStream = null;

		public static Stream DataBaseStream {
			get {
				return _databaseStream;
			}

			set{_databaseStream = value; }
		}

		private static Stream _helpMenuStream=null;
		public static Stream HelpMenuStream
		{
			get{ return _helpMenuStream;}
			set{ _helpMenuStream = value;}
		}

		private static Stream _helpStream=null;
		public static Stream HelpStream
		{
			get{ return _helpStream;}
			set{ _helpStream = value;}
		}
		private static int _catId=0;
		public static int CatId
		{
			get{return _catId;}
			set{ _catId = value;}
		}
		private static bool _backButtonClick = false;
		public static bool BackButtonClick
		{
			get{return _backButtonClick;}
			set{_backButtonClick = value;}
		}

		private static List<string> _saveSelectedUrls = new List<string> ();
		public static List<string> SaveSelectedUrls
		{
			get{ return _saveSelectedUrls;}
			set{_saveSelectedUrls = value; }
		}
		private static List<DownloadPivot> _SaveVideoList=new List<DownloadPivot>();
		public static List<DownloadPivot> SaveVideolist
		{
			get{return _SaveVideoList;}
			set{_SaveVideoList = value;}
		}
		private static List<PlayListTable> _bookMarkList = new List<PlayListTable> ();
		public static List<PlayListTable> BookMarkList
		{
			get{return _bookMarkList; }
			set{_bookMarkList = value;}
		}
		private static List<PlayListTable> _previewBookMarkList = new List<PlayListTable> ();
		public static List<PlayListTable> PreviewBookMarkList
		{
			get{return _previewBookMarkList; }
			set{_previewBookMarkList = value;}
		}
		private static int _bookmarkPosition=0;
		public static int  BookMarkPosition
		{
			get{return _bookmarkPosition; }
			set{_bookmarkPosition = value;}
		}
		private static bool _isFullscreen = false;
		public static bool  IsFullScreen
		{
			get{return _isFullscreen; }
			set{_isFullscreen = value; }
		}
		private static int _SelectedMenuItem = 1;
		public static int SelectedMenuItem
		{
			get{return _SelectedMenuItem; }
			set{_SelectedMenuItem = value; }
		}
		private static System.Timers.Timer _videoMixItemDelteTimer=new System.Timers.Timer();
		public static System.Timers.Timer videoMixItemDelteTimer
		{
			get{ return _videoMixItemDelteTimer;}
			set{_videoMixItemDelteTimer = value; }
		}
		private static System.Timers.Timer _MainActivityRefreshTimer=new System.Timers.Timer();
		public static System.Timers.Timer MainActivityRefreshTimer
		{
			get{ return _MainActivityRefreshTimer;}
			set{_MainActivityRefreshTimer = value; }
		}
		private static int _LanguageID=0;
		public static int LanguageID
		{
			get{return _LanguageID;}
			set{_LanguageID = value;}
		}
		private static PlayListTable _bookMarkItem=new PlayListTable();
		public static PlayListTable BookMarkItem
		{
			get{return _bookMarkItem;}
			set{_bookMarkItem = value;}
		}
#endif

    }
}
