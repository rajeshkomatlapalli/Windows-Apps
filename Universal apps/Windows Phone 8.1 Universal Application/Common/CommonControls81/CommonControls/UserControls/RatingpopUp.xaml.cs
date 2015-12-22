using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Reflection;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class RatingpopUp : UserControl
    {
        public static RatingpopUp current;
        bool moviestatus = false;
        public RatingpopUp()
        {
            try
            {
                current = this; 
                this.InitializeComponent();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in RatingpopUp Method In RatingpopUp.cs file", ex);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                if (AppSettings.MovieRateShowStatus == true)
                {
                    ratingimg.Source = OnlineShow.GetRatingImageForMovie();
                    AppSettings.MovieRateShowStatus = false;
                    moviestatus = true;
                }
                else
                {
                    ratingimg.Source = OnlineShow.GetRatingImage();
                }


            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in RatingpopUp_Loaded_1 Method In RatingpopUp.cs file", ex);
            }
        }

        private void ratingimg_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                double Linkrating = 0.0;
                if (AppSettings.linkRating != "5")
                {
                    Linkrating = Convert.ToDouble(AppSettings.linkRating) + 0.5;
                }
                else
                {
                    Linkrating = 0.5;
                }
                string rate = Linkrating.ToString();
                AppSettings.linkRating = rate;
                string rating = ImageHelper.LoadRatingImage((rate).ToString());
                BitmapImage objImg = new BitmapImage(new Uri(Package.Current.InstalledLocation.Path + rating, UriKind.Relative));
                ratingimg.Source = objImg;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ratingimg_Tapped_1 Method In RatingpopUp.cs file", ex);
            }
        }

        private void save_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                gridpopup.Visibility = Visibility.Collapsed;

                if (moviestatus == true)
                {
                    OnlineShow.SaveMovieRating();

                    //ShowList list = ShowDescription.collection.FirstOrDefault();
                    //list.RatingBitmapImage = ImageHelper.LoadRatingImage(AppSettings.linkRating);
                    //ShowDescription.collection.Clear();
                    //ShowDescription.collection.Add(list);
                    moviestatus = false;
                }
                else
                {
                    OnlineShow.SaveRating();
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in save_Tapped_1 Method In RatingpopUp.cs file", ex);
            }
        }

        private void cancel_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                close();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in cancel_Tapped_1 Method In RatingpopUp.cs file", ex);
            }
        }

        public void close()
        {
            try
            {
                AppSettings.LinkType = "";
                AppSettings.LinkID = "";
                gridpopup.Visibility = Visibility.Collapsed;
                if (moviestatus == true)
                {
                    moviestatus = false;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in close Method In RatingpopUp.cs file", ex);
            }
        }

    }
}
