using System;
using System.Net;
using System.Windows;
#if WP8
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using OnlineVideos.Common; 
#endif
using OnlineVideos.Data;
using OnlineVideos.Entities;
//using OnlineVideos.Common;
using Common.Library;
//using Common.Common;

namespace OnlineVideos.UI
{
    public static class NavigationHelper
    {
        public static Uri MainPanoramaPage
        {
            get
            {
                return new Uri("/MainPage.xaml", UriKind.Relative);
            }
        }
    
        public static Uri FeedbackPage
        {
            get
            {
                return new Uri("/About Us/Feedback.xaml", UriKind.Relative);
            }
        }

        public static Uri ContactUsPage
        {
            get
            {
                return new Uri("/About Us/ContactUs.xaml", UriKind.Relative);
            }
        }
        public static Uri StoryReadingPage
        {
            get
            {
                return new Uri("/Views/StoryReading.xaml", UriKind.Relative);
            }
        }
        public static Uri UpgradePage
        {
            get
            {
                return new Uri("/About Us/Upgrade.xaml", UriKind.Relative);
            }
        }

        public static Uri BrowserPage
        {
            get
            {
                if (ResourceHelper.ProjectName == "Web Tile")
                return new Uri("/Views/PreviewPage.xaml", UriKind.Relative);
                else
                    return new Uri("/Views/BrowserPage.xaml", UriKind.Relative);
            }
        }

        public static Uri HistoryPage
        {
            get
            {
                return new Uri("/Views/HistoryPivot.xaml", UriKind.Relative);
            }
        }

        public static Uri FavoritesPage
        {
            get
            {
                return new Uri("/Views/ShowFavorites.xaml", UriKind.Relative);
            }
        }

        public static Uri HelpMenuPage
        {
            get
            {
                return new Uri("/Views/HelpMenu.xaml", UriKind.Relative);
            }
        }
        public static Uri CreatePlayListPage
        {
            get
            {
                return new Uri("/Views/CreatePlayListPage.xaml", UriKind.Relative);
            }
        }
        public static Uri MixVideosPopupPage
        {
            get
            {
                return new Uri("/Views/MixVideosPopupPage.xaml", UriKind.Relative);
            }
        }
        public static Uri SettingsPage
        {
            get
            {
                return new Uri("/Views/Settings.xaml", UriKind.Relative);
            }
        }

        public static Uri getSettingsPage(string NavigationID)
        {
           return new Uri("/Views/Settings.xaml?myid=" + NavigationID, UriKind.Relative);
        }

        public static Uri BackAgentErrorPage
        {
            get
            {
               return new Uri("/Views/BackAgentError.xaml", UriKind.Relative);
            }
        }

        public static Uri AboutMemoryPage
        {
            get
            {
                return new Uri("/Views/AboutMemory.xaml", UriKind.Relative);
            }
        }

        public static Uri getShowListPage(string name)
        {
            return new Uri("/Views/ShowList.xaml?name=" + name, UriKind.Relative);
        }

        public static Uri getSearchPage(string searchText)
        {
            return new Uri("/Views/Search.xaml?searchtext=" + searchText, UriKind.Relative);
        }

        public static Uri getMovieDetailPage(string ShowID)
        {
            return new Uri("/Views/Details.xaml?id=" + ShowID, UriKind.Relative);
        }
        public static Uri getMovieDetailPage(ShowList context, string pivottitle)
        {
            return new Uri("/Views/Details.xaml?title="+pivottitle+"&id=" + context.ShowID, UriKind.Relative);
        }
        public static Uri getMovieDetailPageForMix(LinkHistory context)
        {
            if(ResourceHelper.AppName==Apps.Video_Mix.ToString())
            return new Uri("/Views/DetailPage.xaml?id=" + context.ShowID, UriKind.Relative);
            else
                return new Uri("/Views/Details.xaml?id=" + context.ShowID, UriKind.Relative);
        }
        public static Uri getMovieDetailPage(OnlineVideos.Common.INavigationContext context)
        {
            return new Uri("/Views/Details.xaml?title=songs" + "&id=" + context.ShowID + "&Stitle=" + context.Title, UriKind.Relative);
        }
        public static Uri getMovieDetailPageAudio(ShowLinks context)
        {
            return new Uri("/Views/MusicDetail.xaml?title=songs" + "&id=" + context.ShowID + "&type=search" + "&Atitle=" + context.Title, UriKind.Relative);
        }
        public static Uri getsearchdetailspage(string page,OnlineVideos.Common.INavigationContext context)
        {
            return new Uri("/Views/" + page + ".xaml?title=songs" + "&id=" + context.ShowID + "&type=search" + "&Atitle=" + context.Title, UriKind.Relative);
        }
        public static Uri getsearchdetailspage(string page, ShowList context,string pivottitle)
        {
            return new Uri("/Views/" + page + ".xaml?title="+pivottitle+ "&id=" + context.ShowID + "&type=search" + "&Atitle=" + context.Title, UriKind.Relative);
        }
        public static Uri getMovieDetailPageFromSearch(ShowLinks context)
        {
            return new Uri("/Views/Details.xaml?title=songs" + "&id=" + context.ShowID + "&type=search" + "&Stitle=" + context.Title, UriKind.Relative);
        }
        public static Uri getMovieDetailPageFromSearch(ShowLinks context, string pivottitle)
        {
            return new Uri("/Views/Details.xaml?title=" + pivottitle + "&id=" + context.ShowID + "&type=search" + "&Stitle=" + context.Title, UriKind.Relative);
        }
        public static Uri getDanceDetailPageFromSearch(ShowLinks context, string pivottitle)
        {
            return new Uri("/Views/DanceDetailPage.xaml?title=" + pivottitle + "&id=" + context.ShowID + "&type=search" + "&Stitle=" + context.Title, UriKind.Relative);
        }
        public static Uri getMovieDetailPageFromSearchMovies(ShowLinks context)
        {
            return new Uri("/Views/Details.xaml?title=songs" + "&id=" + context.ShowID, UriKind.Relative);
        }
        public static Uri getMovieDetailPageFromSearchMovies(ShowLinks context, string pivottitle)
        {
            return new Uri("/Views/Details.xaml?title=" + pivottitle + "&id=" + context.ShowID, UriKind.Relative);
        }
        public static Uri getSongDetailPage(string ShowID)
        {
            return new Uri("/Views/ShowDetails.xaml?id=" + ShowID, UriKind.Relative);
        }
        public static Uri getStoryDetailPage(ShowList show)
        {
            return new Uri("/Views/StoryDetails.xaml?id=" + show.ShowID, UriKind.Relative);
        }
        public static Uri getSongDetailPage(ShowList show)
        {
            return new Uri("/Views/ShowDetails.xaml?id=" + show.ShowID, UriKind.Relative);
        }

        public static Uri getSongDetailPage(string ShowID, string ShowTitle, string SongTitle)
        {
            return new Uri("/Views/ShowDetails.xaml?id=" + ShowID + "&chapter=" + ShowTitle + "&id+cno=" + SongTitle, UriKind.Relative);
        }

        public static Uri getSongListPage(string ShowID)
        {
            return new Uri("/Views/SongsList.xaml?mid=" + ShowID, UriKind.Relative);
        }



        public static Uri getCricketDetailPage(OnlineVideos.Common.INavigationContext context)
        {
            return new Uri("/Views/CricketDetail.xaml?id=" + context.ShowID, UriKind.Relative);
        }

        public static Uri getCheatCodePage(string PersonID, string ShowID)
        {
            return new Uri("/Views/CheatCodePage.xaml?pid=" + PersonID + "&id=" + ShowID, UriKind.Relative);
        }

        public static Uri getFeedbackPage(ShowLinks showLinkInfo)
        {
            return new Uri("/About Us/Feedback.xaml?id=" + showLinkInfo.ShowID + "&chno=" + showLinkInfo.LinkOrder + "&title=" + showLinkInfo.Title + "&LinkType=" + showLinkInfo.LinkType + "&uri=" + showLinkInfo.LinkUrl, UriKind.Relative);
        }

        public static Uri getCastPanoramaPage()
        {
            return new Uri("/Views/CastPanorama.xaml?pid=" + AppSettings.PersonID, UriKind.Relative);
        }

        public static Uri getCastPanoramaPageFromSearch()
        {
            return new Uri("/Views/CastPanorama.xaml?pid=" + AppSettings.PersonID + "&type=search", UriKind.Relative);
        }

        public static Uri getCharacterDetailPage()
        {
            return new Uri("/Views/CharacterDetail.xaml?pid=" + AppSettings.PersonID, UriKind.Relative);
        }
        
        public static Uri getHelpDetailPage(string ID, string Url)
        {
            return new Uri("/Views/Help.xaml?id=" + ID + "&url=" + Url, UriKind.Relative);
        }

        public static Uri ParentalControlShowListPage
        {
            get
            {
                return new Uri("/Views/MoviesList.xaml", UriKind.Relative);
            }
        }
        public static Uri getCategoryDetailPageForWeb(ShowCategories ShowCategories)
        {
            return new Uri("/Views/CategoryDetails.xaml?&catname=" + ShowCategories.CategoryName + "&catid=" + ShowCategories.CategoryID, UriKind.Relative);
        }
        public static Uri getCategoryDetailPage(CatageoryTable CatageoryTable)
        {
            return new Uri("/Views/CategoryDetails.xaml?&catname=" +CatageoryTable.CategoryName + "&catid=" +CatageoryTable.CategoryID, UriKind.Relative);
        }
        public static Uri getSubjectDetailPage(QuizList context,string QuizPivot)
        {
            return new Uri("/Views/SubjectDetail.xaml?id=" + context.ShowID + "&QuizPivot=" + QuizPivot , UriKind.Relative);
        }
        public static Uri getMusicDetailPage(ShowList context, string pivottitle)
        {
            return new Uri("/Views/MusicDetail.xaml?title="+pivottitle+"&id=" + context.ShowID, UriKind.Relative);
        }
        public static Uri getDanceDetailPage(ShowList context, string pivottitle)
        {
            return new Uri("/Views/DanceDetailPage.xaml?title=" + pivottitle + "&id=" + context.ShowID, UriKind.Relative);
        }
        public static Uri getRingTonePage(ShowLinks showLinkInfo)
        {
            return new Uri("/Views/RingTone.xaml?link=" + showLinkInfo.LinkUrl + "&showid=" + showLinkInfo.ShowID+ "&title=" + showLinkInfo.Title, UriKind.Relative);
        }
        public static Uri getQuizresultPage(string TimeUsed)
        {
            return new Uri("/Views/QuizResult.xaml?Timeused=" + TimeUsed, UriKind.Relative);
        }
        public static Uri getQuestionDisplay()
        {
            return new Uri("/Views/QuestionsDisplay.xaml", UriKind.Relative);
        }
        public static Uri getQuizResultReview()
        {
            return new Uri("/Views/QuizResultReview.xaml", UriKind.Relative);
        }
        public static Uri getQuizList(string MovieId , string QuizTitle)
        {
            return new Uri("/Views/SubjectDetail.xaml?id=" + MovieId + "&QuizPivot=" + QuizTitle, UriKind.Relative);
        }

        public static Uri getVideoGameDescriptionPage(string ID, string ShowID,string type)
        {
            return new Uri("/Views/VideoGameDescription.xaml?wid=" + ID + "&id=" + ShowID + "&type=" + type, UriKind.Relative);
        }
        public static Uri youtubeBrowserPage(string Linkurl)
        {
            return new Uri("/Views/Browser.xaml?url=" + Linkurl, UriKind.Relative);
        }
    }
}
