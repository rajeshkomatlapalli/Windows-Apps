using Common.Library;
using Common.Utilities;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideos.UserControls
{
    public sealed partial class PersonGallery : UserControl
    {
        #region GlobalDeclaration
        AppInsights insights = new AppInsights();
        Stopwatch stopwatch = new Stopwatch();
        string Title = string.Empty;
        private bool IsDataLoaded;
        int gallerycount, galleryeventcount;
        Size galleryImageSize;
        #endregion
        #region Constructor
        public PersonGallery()
        {
            this.InitializeComponent();
            IsDataLoaded = false;
            gallerycount = 0;
            galleryeventcount = 0;
        }
        #endregion

        private void lbxGallery_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxGallery.SelectedIndex != -1)
            {
                AppSettings.GalleryTitle = (lbxGallery.SelectedItem as GalleryImageInfo).Title;
                insights.Event(Title + "Viewed");
                Frame frame = Window.Current.Content as Frame;
                Page p = frame.Content as Page;
                p.Frame.Navigate(typeof(PersonGalleryPopup));                                
            }
            lbxGallery.SelectedIndex = -1;
        }

       void lbxGallery_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                insights.Event("Gallery Loaded");
                IEnumerable<DependencyObject> cboxes = PageHelper.GetChildsRecursive(lbxGallery);
                //TODO: verify if this code can be reused
                foreach (DependencyObject obj in cboxes.OfType<Image>())
                {
                    Type type = obj.GetType();
                    if (type.Name == "Image")
                    {
                        Image cb = obj as Image;
                        if (cb.Source != null)
                        {

                            if (cb.RenderSize != galleryImageSize)
                            {
                                cb.ImageFailed += cb_ImageFailed;

                                cb.SizeChanged += new SizeChangedEventHandler(cb_SizeChanged);
                                gallerycount++;
                            }
                            else
                            {
                                _performanceProgressBargallery.IsIndeterminate = false;
                            }
                        }
                        else
                        {
                            _performanceProgressBargallery.IsIndeterminate = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in lbxGallery_Loaded Method In PersonGallery.cs file.", ex);
            }
        }

       void cb_ImageFailed(object sender, ExceptionRoutedEventArgs e)
       {
           try
           {
               _performanceProgressBargallery.IsIndeterminate = false;
           }
           catch (Exception ex)
           {
               Exceptions.SaveOrSendExceptions("Exception in cb_ImageFailed Method In PersonGallery.cs file.", ex);
           }
       }

        void cb_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            galleryeventcount++;
            galleryImageSize = e.NewSize;

            if (galleryeventcount == gallerycount)
                _performanceProgressBargallery.IsIndeterminate = false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Title = OnlineShow.GetPersonDetail(AppSettings.PersonID).Name;
                stopwatch = System.Diagnostics.Stopwatch.StartNew();
                var properties = new Dictionary<string, string> { { Title, AppSettings.GalleryTitle } };
                var metrics = new Dictionary<string, double> { { "Processing Time", stopwatch.Elapsed.TotalMilliseconds } };
                insights.Event("Gallery page Time", properties, metrics);
                //LoadAds();
                if (IsDataLoaded == false)
                {
                    LoadGallery();
                    IsDataLoaded = true;
                }
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In PersonGallery.cs file.", ex);
            }
        }

        #region Comman Methods
        private void LoadAds()
        {
            try
            {
                //LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 2);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In SongDetails file", ex);
                string excepmess = "Exception in LoadAds Method In SongDetails file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }

        }
        private void LoadGallery()
        {
            if (NetworkHelper.IsNetworkAvailable())
            {
                _performanceProgressBargallery.IsIndeterminate = true;
                lbxGallery.ItemsSource = OnlineShow.GetPersonGallery(AppSettings.PersonUniqueID);
                AppSettings.gallcount = lbxGallery.Items.Count.ToString();
                if (lbxGallery.Items.Count > 0)
                {
                    lbxGallery.Loaded += new RoutedEventHandler(lbxGallery_Loaded);

                    tblkGallery.Visibility = Visibility.Collapsed;
                    _performanceProgressBargallery.IsIndeterminate = false;
                }
                else
                {
                    tblkGallery.Text = "Images are Not Available";
                    tblkGallery.Visibility = Visibility.Visible;
                    _performanceProgressBargallery.IsIndeterminate = false;
                }

            }
            else
            {
                tblkGallery.Text = "Network Not Available";
                tblkGallery.Visibility = Visibility.Visible;
            }
        }
        #endregion

        private void lbxGallery_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (lbxGallery.SelectedIndex != -1)
            {
                Title = OnlineShow.GetPersonDetail(AppSettings.PersonID).Name;
                AppSettings.GalleryTitle = (lbxGallery.SelectedItem as GalleryImageInfo).Title;
                insights.Event(AppSettings.GalleryTitle + "Viewed");
                Frame frame = Window.Current.Content as Frame;
                Page p = frame.Content as Page;
                p.Frame.Navigate(typeof(PersonGalleryPopup));
            }
            lbxGallery.SelectedIndex = -1;
        }

    }
}
