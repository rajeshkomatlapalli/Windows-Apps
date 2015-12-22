using System;
using System.Net;
using System.Windows;
#if ANDROID
using Android.App;
using Common.Libary;
#endif
#if WINDOWS_PHONE_APP && NOTANDROID
//using System.Windows.Media;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Windows.UI;
#endif
#if (WINDOWS_APP || WINDOWS_PHONE_APP) && NOTANDROID
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI;
#endif
namespace Common.Library
{
    public static class AppResources
    {
# if (WINDOWS_APP || WINDOWS_PHONE_APP) && NOTANDROID
        public static bool ShowTopRatedLinks
        {
            get
            {
                return Application.Current.Resources["songs"] as string == "true";
            }
        }
        public static string ProjectName
        {
            get
            {
                return Application.Current.Resources["ProjectName"] as string;
            }
        }
        public static string UserUploadPageName
        {
            get
            {
                return Application.Current.Resources["UserUploadPageName"] as string;
            }
        }
        public static string OnlineImagesPageName
        {
            get
            {
                return Application.Current.Resources["OnlineImagesPageName"] as string;
            }
        }
#if WINDOWS_PHONE_APP
        public static string AdRotatorSettingsUrl
        {
            get
            {
                return Application.Current.Resources["SettingsUrl"] as string;
            }
        }
        public static string RatingUserName
        {
            get
            {
                return Application.Current.Resources["RatingUserName"] as string;
            }
        }
                public static string FlurryAppID
        {
            get
            {
                return Application.Current.Resources["FlurryAppID"] as string;
            }
        }
       

        public static string RatingPassword
        {
            get
            {
                return Application.Current.Resources["RatingPassword"] as string;
            }
        }
        public static string TopRatedMsg
        {
            get
            {
                return Application.Current.Resources["TopRatedMsg"] as string;
            }
        }
        public static string RecentdMsg
        {
            get
            {
                return Application.Current.Resources["RecentdMsg"] as string;
            }
        }
        public static string ShowsRatingBlogUrl
        {
            get
            {
                return Application.Current.Resources["ShowsRatingBlogUrl"] as string;
            }
        }
        public static string QuizRatingBlogUrl
        {
            get
            {
                return Application.Current.Resources["QuizRatingBlogUrl"] as string;
            }
        }
        public static string LinksRatingBlogUrl
        {
            get
            {
                return Application.Current.Resources["LinksRatingBlogUrl"] as string;
            }
        }
        public static string ShowsRatingBlogName
        {
            get
            {
                return Application.Current.Resources["ShowsRatingBlogName"] as string;
            }
        }
        public static string LinksRatingBlogName
        {
            get
            {
                return Application.Current.Resources["LinksRatingBlogName"] as string;
            }
        }
        public static string QuizLinksRatingBlogName
        {
            get
            {
                return Application.Current.Resources["QuizLinksRatingBlogName"] as string;
            }
        }
        public static bool ShowonlinewebTile
        {
            get
            {
                return Application.Current.Resources["ShowonlinewebTile"] as string == "true";
            }
        }

        public static Brush PhoneAccentBrush
        {
            get
            {
                return (Brush)Application.Current.Resources["PhoneAccentBrush"];
            }
        }
        public static Brush PhoneForegroundBrush
        {
            get
            {
                return (Brush)Application.Current.Resources["PhoneForegroundBrush"];
            }
        }

        public static string ShowCategoryPivotTitle
        {
            get
            {
                return Application.Current.Resources["CategoryPvt"] as string;
            }
        }

        public static SolidColorBrush PhoneBackgroundBrush
        {
            get
            {
                return Application.Current.Resources["PhoneBackgroundBrush"] as SolidColorBrush;
            }
        }

        public static string DownloadImagePageName
        {
            get
            {
                return Application.Current.Resources["DownloadImagePageName"] as string;
            }
        }
        public static string OnlinewebTilePageName
        {
            get
            {
                return Application.Current.Resources["OnlinewebTilePageName"] as string;
            }
        }
#endif
        public static bool ShowTopRatedAudioLinks
        {
            get
            {
                return Application.Current.Resources["audios"] as string == "true";
            }
        }
        public static bool RegisterDownloadAgent
        {
            get
            {
                return Application.Current.Resources["RegisterDownloadAgent"] as string == "true";
            }
        }

         public static string Helplinkurl
        {
            get
            {
                return Application.Current.Resources["Helplinkurl"] as string;
            }
        }




        public static string PersonGalleryPopupPage
        {
            get
            {
                return Application.Current.Resources["PersonGalleryPopupPage"] as string;
            }
        }
        public static bool DownloadPeople
        {
            get
            {
                return Application.Current.Resources["DownloadPeople"] as string == "true";
            }
        }
        public static bool visablesecondaryblog
        {
            get
            {
                return Application.Current.Resources["visablesecondaryblog"] as string == "true";
            }
        }
        public static bool DownloadShow
        {
            get
            {
                return Application.Current.Resources["DownloadShow"] as string == "true";
            }
        }
        public static bool ShowFavoritesPageMoviesPivot
        {
            get
            {
                return Application.Current.Resources["movies"] as string == "true";
            }
        }

        public static bool ShowFavouritesPageComedyPivot
        {
            get
            {
                return Application.Current.Resources["comedy"] as string == "true";
            }
        }
        public static string VideoMix
        {
            get
            {
                return Application.Current.Resources["VideoMix"] as string;
            }
        }
        public static bool ShowCricketDetailPage
        {
            get
            {
                return Application.Current.Resources["CricketDetail"] as string == "true";
            }
        }

        public static bool IsSimpleDetailPage
        {
            get
            {
                return Application.Current.Resources["DetailPage"] as string != "true";
            }
        }

        public static string MainMenuItemShowListName
        {
            get
            {
                return Application.Current.Resources["mln"].ToString();
            }
        }

        public static string FavoriteMoviesPivotTitle
        {
            get
            {
                return Application.Current.Resources["pvtFHStitlesOne"].ToString();
            }
        }

        public static string FavoriteSongsPivotTitle
        {
            get
            {
                return Application.Current.Resources["pvtFHStitlesTwo"].ToString();
            }
        }

        public static string ComedyHistoryPivotTitle
        {
            get
            {
                return Application.Current.Resources["pvtHCtitlesFive"].ToString();
            }
        }

        public static string FavoriteComedyPivotTitle
        {
            get
            {
                return Application.Current.Resources["pvtHCtitlesFive"].ToString();
            }
        }

        public static string FavoritePeoplePivotTitle
        {
            get
            {
                return Application.Current.Resources["pvtFHStitlesThree"].ToString();
            }
        }
        public static int ShowsInsertLimit
        {
            get
            {
                return Convert.ToInt32(Application.Current.Resources["ShowsInsertLimit"].ToString());
            }
        }
        public static int PeopleInsertLimit
        {
            get
            {
                return Convert.ToInt32(Application.Current.Resources["PeopleInsertLimit"].ToString());
            }
        }

        public static string TopRatedLinksTitle
        {
            get
            {
                return Application.Current.Resources["prmh"].ToString();
            }
        }

         public static string Type
        {
            get
            {
                return Application.Current.Resources["type"] as string;
            }
        }

        public static string ShowListReadMoreLabel
        {
            get
            {
                return Application.Current.Resources["readMore"].ToString();
            }
        }

        public static string ApplicationProductID
        {
            get
            {
                return Application.Current.Resources["productid"].ToString();
            }
        }

        public static string[] ParentalControlContextMenuItems
        {
            get
            {
                return Application.Current.Resources["ContextMenu"].ToString().Split(',');
            }
        }

        public static bool AllowRatingLinks
        {
            get
            {
                return Application.Current.Resources["ratingsongs"].ToString().Split(',')[0] == "true";
            }
        }

        public static string AllowRatingLinkContextMenuLabel
        {
            get
            {
                if (AllowRatingLinks)
                    return Application.Current.Resources["ratingsongs"].ToString().Split(',')[1];
                else
                    return Application.Current.Resources["ratingsongs"].ToString().Split(',')[2];
            }
        }

        public static bool ShowAdControl
        {
            get
            {
                return Application.Current.Resources["AdVisible"] as string != "False";
            }
        }

        public static bool ShowDetailPageTitle
        {
            get
            {
                return Application.Current.Resources["VideoTitleName"] as string == "true";
            }
        }

        public static bool ShowDetailPageDetailPivot
        {
            get
            {
                return Application.Current.Resources["CheckDetail"] as string == "true";
            }
        }

        public static bool ShowDetailPageSongsPivot
        {
            get
            {
                return Application.Current.Resources["dtailsongs"] as string == "true";
            }
        }

        public static bool ShowDetailPageChaptersPivot
        {
            get
            {
                return Application.Current.Resources["dtailchapter"] as string == "true";
            }
        }

        public static bool ShowDetailPageCastPivot
        {
            get
            {
                return Application.Current.Resources["dtailcast"] as string == "true";
            }
        }

        public static bool ShowDetailPageAudioPivot
        {
            get
            {
                return Application.Current.Resources["AudioApp"] as string == "true";
            }
        }

        public static bool ShowDetailpageComedyPivot
        {
            get
            {
                return Application.Current.Resources["Comedy"] as string == "true";
            }
        }
        public static bool ShowLoadAudioPivotinSearchPage
        {
            get
            {
                return Application.Current.Resources["LoadAudioApp"] as string == "true";
            }
        }

        public static bool ShowDetailPageCheatCodesPivot
        {
            get
            {
                return Application.Current.Resources["dtailcheat"] as string == "true";
            }
        }

        public static bool ShowDetailPageAudioVideoHeaders
        {
            get
            {
                return Application.Current.Resources["Message"] as string == "true";
            }
        }

        public static string AdControlApplicationID
        {
            get
            {
                return Application.Current.Resources["misappid"].ToString();
            }
        }

        public static string AdControlAdUnitID
        {
            get
            {
                return Application.Current.Resources["misuid"].ToString();
            }
        }

        public static Style ShowDetailPageTitleStyle
        {
            get
            {
                return Application.Current.Resources["PivotTitle"] as Style;
            }
        }

        public static string ShowDetailPageDetailPivotTitle
        {
            get
            {
                return Application.Current.Resources["pvtTitleOne"] as string;
            }
        }

        public static string ShowDetailPageCastPivotTitle
        {
            get
            {
                return Application.Current.Resources["pvtTitleTwo"] as string;
            }
        }
        
        public static string Downloadpivot
        {
            get
            {
                return Application.Current.Resources["Downloadpivot"] as string;
            }
        }

        public static string DetailPageName
        {
            get
            {
                return Application.Current.Resources["DetailPageName"] as string;
            }
        }
        public static string ScoreBoardPageName
        {
            get
            {
                return Application.Current.Resources["ScoreBoardPageName"] as string;
            }
        }

        public static string CastDetailPageName
        {
            get
            {
                return Application.Current.Resources["CastDetailPageName"] as string;
            }
        }

        public static string HistoryPageName
        {
            get
            {
                return Application.Current.Resources["HistoryPageName"] as string;
            }
        }
        public static string FavoritePageName
        {
            get
            {
                return Application.Current.Resources["FavoritePageName"] as string;
            }
        }

        public static string RingTonePageName
        {
            get
            {
                return Application.Current.Resources["RingTonePageName"] as string;
            }
        }

        public static string LiricsDetailPageName
        {
            get
            {
                return Application.Current.Resources["LiricsDetailPageName"] as string;
            }
        }
        public static string ShowDetailPageChaptersPivotTitle
        {
            get
            {
                return Application.Current.Resources["pvtTitleThree"] as string;
            }
        }

        public static string ShowDetailPageCheatCodesPivotTitle
        {
            get
            {
                return Application.Current.Resources["pvtTitleFour"] as string;
            }
        }

        public static string ShowDetailPageSongsPivotTitle
        {
            get
            {
                return Application.Current.Resources["pvtitmSongsTitle"] as string;
            }
        }

        public static string ShowDetailPageComedyPivotTitle
        {
            get
            {
                return Application.Current.Resources["pvtitmComedyTitle"] as string;
            }
        }

        public static string FavoriteAudioPivotTitle
        {
            get
            {
                return Application.Current.Resources["AudioPivotTitle"] as string;
            }
        }

        public static string ShowFavoriteAudioPivotTitle
        {
            get
            {
                return Application.Current.Resources["pvtFHStitlesFour"] as string;
            }
        }
        public static string ShowDetailPageLiricsPivotTitle
        {
            get
            {
                return Application.Current.Resources["LiricsPivotTitle"] as string;
            }
        }
        public static string PrimaryShowsDownloadUri
        {
            get
            {
                return Application.Current.Resources["Primaryshowsfromblog"] as string;
            }
        }
        public static string PrimaryShowsDownloadUriwin81
        {
            get
            {
                return Application.Current.Resources["Primaryshowsfromblogwin81"] as string;
            }
        }
        public static string PrimaryPeopleDownloadUri
        {
            get
            {
                return Application.Current.Resources["Primarypeoplefromblog"] as string;
            }
        }
        public static string PrimaryPeopleDownloadUriwin81
        {
            get
            {
                return Application.Current.Resources["Primarypeoplefromblogwin81"] as string;
            }
        }
        public static string SecondaryShowsDownloadUri
        {
            get
            {
                return Application.Current.Resources["Secondaryshowsfromblog"] as string;
            }
        }
        public static string SecondaryShowsDownloadUriwin81
        {
            get
            {
                return Application.Current.Resources["Secondaryshowsfromblogwin81"] as string;
            }
        }
        public static string SecondaryPeopleDownloadUri
        {
            get
            {
                return Application.Current.Resources["Secondarypeoplefromblog"] as string;
            }
        }
        public static string SecondaryPeopleDownloadUriwin81
        {
            get
            {
                return Application.Current.Resources["Secondarypeoplefromblogwin81"] as string;
            }
        }
        public static string PrivacyOnlineEducation
        {
            get
            {
                return Application.Current.Resources["PrivacyOnlineEducation"] as string;
            }
        }
        public static bool ShowCastPanoramaPage
        {
            get
            {
                return Application.Current.Resources["castpnm"] as string == "true";
            }
        }
        public static bool ShowCategoryPivot
        {
            get
            {
                string[] a = Application.Current.Resources["CategoryPvt"].ToString().Split(',');
                return a[0].ToString() == "true";
            }
        }
        public static Uri UpgradeAppLink
        {
            get
            {
                return new Uri(Application.Current.Resources["UpgradeLink"] as string);
            }
        }

        public static bool ShowUpgradePage
        {
            get
            {
                return Application.Current.Resources["ShowUpgradePage"] as string == "true";
            }
        }

        public static string UpgradePageMessage
        {
            get
            {
                return Application.Current.Resources["UpgradePageMessage"] as string;
            }
        }

        public static Uri getDefaultYoutubePlayerLink(string strLinkID)
        {
            return new Uri("vnd.youtube:" + strLinkID + "?vndapp=youtube_mobile&vndclient=mv-google&vndel=watch");
        }

        public static bool ShowQuiz
        {
            get
            {
                return Application.Current.Resources["Quizdisplay"] as string == "true";
            }
        }
        public static string AllowSubjectRatingLinkContextMenuLabel
        {
            get
            {
                if (AllowRatingLinks)
                    return Application.Current.Resources["ratingsongs"].ToString().Split(',')[1];
                else
                    return Application.Current.Resources["ratingsongs"].ToString().Split(',')[2];
            }
        }

        public static string ShowQuizResultTitle
        {
            get
            {
                return Application.Current.Resources["QuizResultTitle"] as string;
            }
        }

        public static bool ShowParentalControl
        {
            get
            {
                return Application.Current.Resources["ShowParentalControl"] as string == "true";
            }
        }
        public static bool ToStartDownload
        {
            get
            {
                return Application.Current.Resources["ToStartDownloads"] as string == "true";
            }
        }
        public static string FromMailID
        {
            get
            {
                return Application.Current.Resources["FromMailID"] as string;
            }
        }
        public static string PopUpVisibleInCastpage
        {
            get
            {
                return Application.Current.Resources["PopUpVisibleInCastpage"] as string;
            }
        }
        public static string CastPanoramaVisible
        {
            get
            {
                return Application.Current.Resources["CastPanoramaVisible"] as string;
            }
        }
        public static string ShowStartMessage
        {
            get
            {
                return Application.Current.Resources["Quizstartmessage"] as string;
            }
        }
        public static string ShowQuizPivotTitle
        {
            get
            {
                return Application.Current.Resources["SubjectsTitle"] as string;
            }
        }
        public static string ShowthumbnailTitle
        {
            get
            {
                return Application.Current.Resources["Thumbnailtitle"] as string;
            }
        }
        public static string showOnlineeducation
        {
            get
            {
                return Application.Current.Resources["Onlineeducation"] as string;
            }
        }
        public static bool advisible
        {
            get
            {
                return Application.Current.Resources["advisible"] as string == "true";
              
            }
        }
        public static string adUnitId
        {
            get
            {
                return Application.Current.Resources["adUnitId"] as string;
            }
        }
        public static string adApplicationId
        {
            get
            {
                return Application.Current.Resources["adApplicationId"] as string;
            }
        }
        public static string adUnitId1
        {
            get
            {
                return Application.Current.Resources["adUnitId1"] as string;
            }
        }
        public static string adApplicationId1
        {
            get
            {
                return Application.Current.Resources["adApplicationId1"] as string;
            }
        }
        public static SolidColorBrush WhiteBrush
        {
            get
            {
                return new SolidColorBrush(Colors.White);
            }
        }
        public static SolidColorBrush RedBrush
        {
            get
            {
                return new SolidColorBrush(Colors.Red);
            }
        }

        public static string upgradeappurl
        {
            get
            {
                return Application.Current.Resources["upgradeappurl"] as string;
            }
        }
        public static string uploadshowrating
        {
            get
            {
                return Application.Current.Resources["uploadshowrating"] as string;
            }
        }
        public static string uploadlinkrating
        {
            get
            {
                return Application.Current.Resources["uploadlinkrating"] as string;
            }
        }
        public static string uploadquizrating
        {
            get
            {
                return Application.Current.Resources["uploadquizrating"] as string;
            }
        }
        public static string username
        {
            get
            {
                return Application.Current.Resources["Username"] as string;
            }
        }
        public static string password
        {
            get
            {
                return Application.Current.Resources["Password"] as string;
            }
        }
#endif
# if ANDROID
		public static string PrimaryShowsDownloadUri
		{
			get
			{
				return Application.Context.GetString(Resource.String.Primaryshowsfromblog);
			}
		}
		public static string PrimaryPeopleDownloadUri
		{
			get
			{
				return Application.Context.GetString(Resource.String.Primarypeoplefromblog);
			}
		}
		public static string SecondaryShowsDownloadUri
		{
			get
			{
				return Application.Context.GetString(Resource.String.Secondaryshowsfromblog);
			}
		}
		public static string SecondaryPeopleDownloadUri
		{
			get
			{
				return Application.Context.GetString(Resource.String.Secondarypeoplefromblog);
			}
		}
		public static bool ShowTopRatedLinks
		{
			get
			{
				return Convert.ToBoolean(Application.Context.GetString(Resource.String.songs));
			}
		}
		public static bool DownloadPeople
		{
			get
			{
				return Convert.ToBoolean( (Application.Context.GetString(Resource.String.DownloadPeople)));
			}
		}
		public static bool ShowCricketDetailPage
		{
			get
			{
				return Convert.ToBoolean(Application.Context.GetString(Resource.String.CricketDetail));
			}
		}
		public static bool DownloadShow
		{
			get
			{
				return Convert.ToBoolean(Application.Context.GetString(Resource.String.DownloadShow));
			}
		}
		public static int ShowsInsertLimit
		{
			get
			{
				return Convert.ToInt32(Application.Context.GetString(Resource.String.ShowsInsertLimit));
			}
		}
		public static int PeopleInsertLimit
		{
			get
			{
				return Convert.ToInt32(Application.Context.GetString(Resource.String.PeopleInsertLimit));
			}
		}
		public static string RingTonePageName
		{
			get
			{
				try
				{
					int int_recordTable = Resource.String.songs;
					String recordTable = (Application.Context.GetString(int_recordTable));
					return recordTable;
				}
				catch (Exception)
				{
					return null;
				}
			}
		}
#endif


    }
}
