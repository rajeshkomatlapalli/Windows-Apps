using Common.Library;
using Common.Utilities;
using OnlineVideos;
using OnlineVideos.Data;
using OnlineVideos.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineMovies.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MoviesHistory : Page
    {
        string pivotimage = string.Empty;
        public static dynamic LoadMovieHistory = default(dynamic);
        int movieLastIndex = 1;
        int movieFirstIndex = 1;
        History objHistory = new History();
        int moviesHisCount = 0;
        //AppInsights insights = new AppInsights();

        public MoviesHistory()
        {
            this.InitializeComponent();
            //movieLastIndex = objHistory.GetLastHisId(Constants.MovieHistoryFile);
            //movieFirstIndex = movieFirstIndex - 14;
            //if (movieFirstIndex < 0)
            //    movieFirstIndex = 1;
            //moviesHisCount = 15;
            //Constants.UIThread = true;
            //LoadMovieHistory = objHistory.LoadMovieHistory(movieFirstIndex, movieLastIndex, moviesHisCount);
            //Constants.UIThread = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            try
            {
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In MoviesHistory.cs file.", ex);
            }
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom Method In MoviesHistory.cs file.", ex);
            }
        }

        private void imgTitle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (State.BackStack == "songs")
                {
                    pvtMainHistory.SelectedIndex = 1;
                    State.BackStack = "";
                }
                else if (State.BackStack == "audio")
                {
                    pvtMainHistory.SelectedIndex = 2;
                    State.BackStack = "";
                }
                else if (State.BackStack == "comedy")
                {
                    pvtMainHistory.SelectedIndex = 3;
                    State.BackStack = "";
                }
                LoadAds();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In MoviesHistory file.", ex);
            }
        }

        private void LoadAds()
        {
            try
            {
                //PageHelper.LoadAdControl_New(HistoryGrid, adstackplHistory, 1);
                LoadAdds.LoadAdControl_New(HistoryGrid, adstackplHistory, 1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In SongDetails file", ex);
                string excepmess = "Exception in LoadAds Method In SongDetails file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }

        private void pvtMainHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (State.BackStack == "songs")
                {
                    pvtMainHistory.SelectedIndex = 2;
                }
                else if (State.BackStack == "audio")
                {
                    pvtMainHistory.SelectedIndex = 3;
                }
                else if (State.BackStack == "songs")
                {
                    pvtMainHistory.SelectedIndex = 3;
                }
                string pvtindex = pvtMainHistory.SelectedIndex.ToString();
                string pvtitem = string.Empty;
                switch (pvtindex)
                {
                    case "0": pvtitem = "movies"; break;
                    case "1": pvtitem = "songs"; break;
                    case "2": pvtitem = "audio"; break;
                }
                string name = "History" + pvtitem + "View";
                //insights.Event(name);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in pvtMainHistory_SelectionChanged event in MoviesHistory.xaml.cs Page", ex);
            }
        }
    }
}