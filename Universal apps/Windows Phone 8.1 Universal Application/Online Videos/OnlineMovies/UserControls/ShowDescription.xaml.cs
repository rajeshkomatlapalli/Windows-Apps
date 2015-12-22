using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Common.Library;
using Windows.Media.SpeechSynthesis;
using OnlineVideos.Entities;
using OnlineVideos.View_Models;
using System.ComponentModel;
using OnlineVideos.Data;
using OnlineVideos.UI;
using Windows.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel.DataTransfer;
using OnlineVideos.Views;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideos.UserControls
{
    public sealed partial class ShowDescription : UserControl
    {
        #region Global
        AppInsights insights = new AppInsights();
        SpeechSynthesizer _synthesizer;
        SpeechSynthesizer _synthesizer11;
        ShowList MovieDetails = null;
        ShowDetail showdetail;
        double rateindconverted;
        #endregion

        public ShowDescription()
        {
            this.InitializeComponent();
            showdetail = new ShowDetail();
            MovieDetails = new ShowList();
        }

        public void LoadDescriptionSection(long showID)
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();

                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {

                                                a.Result = OnlineShow.GetShowDetail(showID);
                                            },
                                            async (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                if (a.Result != null)
                                                {
                                                    MovieDetails = (ShowList)a.Result;
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
                                                    tblkTitle.Text = MovieDetails.Title;

                                                    rateindconverted = Convert.ToDouble(MovieDetails.Rating);
                                                    // ratingnew.Value = rateindconverted;

                                                    var imagesouce = ImageHelper.LoadRatingImage(rateindconverted.ToString()).ToString();

                                                    ratingimage.Source = new BitmapImage(new Uri("ms-appx://" + imagesouce));


                                                    if (!string.IsNullOrEmpty(MovieDetails.Description))
                                                    {
                                                        tblkDescription.Text = MovieDetails.Description;
                                                        imgShare.Margin = new Thickness(20, 0, 10, 0);
                                                        imgFav.Margin = new Thickness(20, 0, 0, 0);
                                                        imgSpeech.Visibility = Visibility.Visible;
                                                    }
                                                    else
                                                    {
                                                        tblkDescription.Visibility = Visibility.Collapsed;
                                                        tblkNoDescription.Visibility = Visibility.Visible;
                                                        tblkNoDescription.Text = "No Description Available";
                                                    }

                                                    bool img = ShellTileHelper_New.IsPinned(AppSettings.ShowID);
                                                    if (img == true)
                                                    {
                                                        imgpin.Source = new BitmapImage(new Uri("ms-appx:///Images/unpin.png", UriKind.RelativeOrAbsolute));
                                                    }
                                                    else
                                                    {
                                                        imgpin.Source = new BitmapImage(new Uri("ms-appx:///Images/pin.png", UriKind.RelativeOrAbsolute));
                                                    }

                                                    imgMovie.Source = await ImageHelper.LoadTileImage(MovieDetails.TileImage);
                                                    imgShare.Tag = MovieDetails.Title;
                                                    imgFav.Source = ResourceHelper.getShowFavoriteStatusImage(MovieDetails.IsFavourite);
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

        #region Events        
        

        private void imgShare_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            try
            {                
                DataTransferManager dtManager = DataTransferManager.GetForCurrentView();
                dtManager.DataRequested += dtManager_DataRequested;
                Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();                
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in imgShare_MouseLeftButtonDown method In ShowDescription.cs file.", ex);
            }
        }

        void dtManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            args.Request.Data.Properties.Title = ResourceHelper.ProjectName + "App";
            args.Request.Data.Properties.Description = " Watch bollywood movie '" + imgShare.Tag + "', Get the app at \n";
            args.Request.Data.SetWebLink(ResourceHelper.AppMarketplaceWebLink);            
        }

        private void imgpin_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            try
            {                
                 bool img = ShellTileHelper_New.IsPinned(AppSettings.ShowID);
                 if (img == true)
                 {
                     imgpin.Source = new BitmapImage(new Uri("ms-appx:///Images/pin.png", UriKind.RelativeOrAbsolute));
                     ShellTileHelper_New.UnPin(AppSettings.ShowID);
                 }
                 else
                 {
                     imgpin.Source = new BitmapImage(new Uri("ms-appx:///Images/unpin.png", UriKind.RelativeOrAbsolute));
                     ShellTileHelper_New.CreatePin(AppSettings.ShowID);
                 }                 
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in imgpin_MouseLeftButtonDown method In ShowDescription.cs file.", ex);
            }
        }

        private void imgFav_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                FavoritesManager.SaveFavorites(AppSettings.ShowUniqueID);
                if (FavoritesManager.IsFavoriteShow(AppSettings.ShowUniqueID))
                    imgFav.Source = ResourceHelper.RemoveFromFavoritesImage;
                else
                    imgFav.Source = ResourceHelper.AddToFavoritesImage;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in imgFav_MouseLeftButtonDown method In ShowDescription.cs file.", ex);
            }
        }

        public void PinOrUnpinShow(Image PinUnpinIcon)
        {
            try
            {
                bool IsMoviePinned;
                ShowList show = OnlineShow.GetShowDetail(Convert.ToInt32(AppSettings.ShowID));                
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PinOrUnpinShow method In ShowDescription.cs file.", ex);
            }
        }

        MediaElement audio = new MediaElement();
        private async void imgSpeech_PointerPressed(object sender, PointerRoutedEventArgs e)
        {            
            try
            {                
                if (Constants.Synthesizer == null)
                {                              
                    Constants.Synthesizer = new SpeechSynthesizer();                   
                    SpeechSynthesisStream ttsStream= await Constants.Synthesizer.SynthesizeTextToStreamAsync(tblkDescription.Text);
                    audio.SetSource(ttsStream, ttsStream.ContentType);
                    audio.Play();
                    imgSpeech.Source = ResourceHelper.StopSpeech;
                }
                else
                {
                    audio.Stop();
                    imgSpeech.Source = ResourceHelper.StartSpeech;
                    Constants.Synthesizer.Dispose();
                    Constants.Synthesizer = null;                    
                    
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in imgSpeech_MouseLeftButtonDown method In ShowDescription.cs file.", ex);
            }
        }
        #endregion

        #region Page Load
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadDescriptionSection(AppSettings.ShowUniqueID);
                Constants.Synthesizer = null;
                insights.Event("Description viewed");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in UserControl_Loaded method In ShowDescription.cs file.", ex);
            }
        }
        #endregion
        
        private void ComboBoxMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
            
        private void ratingimage_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            RatingPopup_New ratingpop = new RatingPopup_New();            
            ratingpop.showPopup(AppSettings.ShowID, rateindconverted, "Details");
        }        
    }
}
