using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.ComponentModel;
using OnlineVideos.Entities;
using OnlineVideos.View_Models;
using Common.Library;
using OnlineVideos.Data;
using OnlineVideos;
using Windows.UI.Xaml.Media.Imaging;
using OnlineVideos.Views;
using OnlineVideos.Views.RatingControlTest;
using Windows.Phone.UI.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineMovies.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Advertisement : Page
    {
        int elapsedtime = 0;
        int time = 0;
        ShowList MovieDetails = null;
        ShowDetail showdetail;
        DispatcherTimer dt;
        public Advertisement()
        {
            this.InitializeComponent();
            showdetail = new ShowDetail();
            MovieDetails = new ShowList();
        }
      
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {           
            elapsedtime = 15;
            time = 0;
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += dt_Tick;
            dt.Start();
            LoadDescriptionSection(AppSettings.ShowUniqueID);
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if(Frame.CanGoBack)
            {
                Frame.GoBack();
                var lpt = Frame.BackStack.Last().ToString();
                e.Handled = true;
            }
        }
        void dt_Tick(object sender, object e)
        {
            if (time < 15)
            {
                elapsedtimetblk.Text = "Video: " + AppSettings.PlayVideoTitle + "- Next video will resume in " + elapsedtime-- + " seconds";
                time++;
            }
            else
            {
                (sender as DispatcherTimer).Stop();
                if (Constants.YoutubePlayList != null)
                {
                    if (NetworkHelper.IsNetworkAvailableForDownloads())
                    {
                        if (Constants.YoutubePlayList.Count > 0)
                        {
                            AppSettings.LinkUrl = Constants.YoutubePlayList.FirstOrDefault().Key;
                            AppSettings.LinkTitle = Constants.YoutubePlayList.FirstOrDefault().Value;
                            Constants.YoutubePlayList.Remove(Constants.YoutubePlayList.FirstOrDefault().Key);
                            AppSettings.PlayVideoTitle = Constants.YoutubePlayList.FirstOrDefault().Value;
                            if (ResourceHelper.AppName == Apps.Video_Mix.ToString())
                            {
                                AppSettings.LinkUrl = AppSettings.LinkUrl.Replace("&amp;hl=en&amp;fs=1&amp;rel=0", "").Replace("http://www.youtube.com/v/", "").Replace("https://www.youtube.com/v/", "");
                                Frame.Navigate(typeof(Youtube), AppSettings.LinkUrl);
                            }
                            else
                                Frame.Navigate(typeof(Youtube), AppSettings.LinkUrl);
                        }
                        else
                        {
                            Frame.GoBack();
                        }
                    }
                    else
                    {
                        if (Constants.YoutubePlayList.Count > 0)
                        {
                            {
                                AppSettings.LinkUrl = Constants.YoutubePlayList.FirstOrDefault().Key;
                                AppSettings.LinkTitle = Constants.YoutubePlayList.FirstOrDefault().Value;
                                Constants.YoutubePlayList.Remove(Constants.YoutubePlayList.FirstOrDefault().Key);
                                AppSettings.PlayVideoTitle = Constants.YoutubePlayList.FirstOrDefault().Value;
                                if (ResourceHelper.AppName == Apps.Video_Mix.ToString())
                                {
                                    Frame.Navigate(typeof(OfflineVideoPlayer), AppSettings.LinkUrl);
                                }
                                else
                                    Frame.Navigate(typeof(OfflineVideoPlayer), AppSettings.LinkUrl);
                            }
                        }
                        else
                        {
                            Frame.GoBack();
                        }
                    }
                }
                else if (Constants.YoutubePlayList.Count == 0)
                {
                    Frame.GoBack();
                }
            }
        }
        
        private void LoadDescriptionSection(long showID)
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();

                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                a.Result = OnlineShow.GetShowDetail(showID);
                                            },
                                            async(object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                MovieDetails = (ShowList)a.Result;
                                                if (MovieDetails != null)
                                                {
                                                    tblkTitle.Text = MovieDetails.Title;
                                                    if (MovieDetails.ReleaseDate != null)
                                                    {
                                                        if (MovieDetails.ReleaseDate.Contains(','))
                                                        {
                                                            tblRelease.Text = MovieDetails.ReleaseDate.Substring(MovieDetails.ReleaseDate.LastIndexOf(',') + 1);
                                                        }
                                                        else if (ResourceHelper.ProjectName == "Indian Cinema.WindowsPhone" || ResourceHelper.ProjectName == "Indian Cinema Pro" || ResourceHelper.ProjectName == "Bollywood Movies" || ResourceHelper.ProjectName == "Bollywood Music")
                                                        {
                                                            tblRelease.Text = MovieDetails.ReleaseDate.Substring(MovieDetails.ReleaseDate.Length - 4);
                                                        }
                                                        else
                                                        {
                                                            tblRelease.Text = MovieDetails.ReleaseDate.Replace("12:00:00 AM", "");
                                                        }
                                                    }
                                                    tblkTitle1.Text = MovieDetails.Title;
                                                    double rateindconverted = Convert.ToDouble(MovieDetails.Rating);
                                                    var imagesouce = ImageHelper.LoadRatingImage(rateindconverted.ToString()).ToString();
                                                    ratingimage.Source = new BitmapImage(new Uri("ms-appx://" + imagesouce));
                                                    //rate.Value = Convert.ToInt32(rateindconverted);
                                                    imgMovie.Source =await ImageHelper.LoadTileImage(MovieDetails.TileImage);
                                                }
                                            }
                                          );
                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadDescriptionSection method In ShowDescription.cs file.", ex);
            }
        }

        private void RatingControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {            
            //RatingControl starratingcontrol = new RatingControl();
            //starratingcontrol.RateValue = rate.RateValue;
            //showdetail.RateThisShow(starratingcontrol);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Border frameBorder = (Border)VisualTreeHelper.GetChild(Window.Current.Content, 0);
        }
        
        void Advertisement_Loaded(object sender, RoutedEventArgs e)
        {
            loadads();
        }
        public void loadads()
        {
            LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 2);
        }
    }
}