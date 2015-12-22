using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using Windows.Storage;
using OnlineVideos.Entities;
using Common.Library;
using OnlineVideos.Data;
using Windows.Phone.UI.Input;
using Indian_Cinema.Views;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PersonGalleryPopup : Page
    {
        #region GlobalDeclaration
        //OnlineVideoDataContext context;
        public static int CurrentImageIndex;
        public static int filecount;
        public static int fileid = 1;
        public static int ind1;
        public static string CurrentImageCount;
        public static List<string> filenames;
        public static List<GalleryImageInfo> Listofgalleryimages;
        double x;
        public string galleryImageCount;
        #endregion

        #region Initialization
        public PersonGalleryPopup()
        {
            this.InitializeComponent();
            Loaded += PersonGalleryPopup_Loaded;
            filenames = new List<string>();
            galleryImageCount = string.Empty;
            Listofgalleryimages = new List<GalleryImageInfo>();
        }
        #endregion

        #region PageLoad
        void PersonGalleryPopup_Loaded(object sender, RoutedEventArgs e)
        {
            
            try
            {               
                Show(AppSettings.GalleryTitle);
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in PersonGalleryPopup_Loaded Method In PersonGalleryPopup.cs file.", ex);
            }
        }
        #endregion

        #region "Common Methods"
        public void Show(string img)
        {
            string path = string.Empty;
            string name = string.Empty;

            if (AppSettings.AddNewShowIconVisibility)
            {
                var gallery1 = Task.Run(async () => await Constants.connection.Table<ShowLinks>().ToListAsync()).Result;
                galleryImageCount = gallery1.Count().ToString();
                int linkorder = Convert.ToInt32(img);
                var gallery = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkOrder == linkorder).ToListAsync()).Result;
                foreach (var gal in gallery)
                {
                    path = gal.LinkUrl;
                    name = System.IO.Path.GetFileName(path);
                }
                AppSettings.bcount = true;
                SetImageSource(path);
            }
            else
            {
                galleryImageCount = AppSettings.gallcount;
                AppSettings.bcount = true;
                ind1 = img.IndexOf('.');
                CurrentImageCount = img.Substring(0, ind1);
                Listofgalleryimages = OnlineShow.GetPersonGallery(AppSettings.PersonUniqueID);
                SetImageSource(CurrentImageCount);
            }
            
            this.Visibility = Visibility.Visible;
        }
        public void SetImageSource(string imagePath)
        {

            if (AppSettings.AddNewShowIconVisibility)
            {
                BitmapImage bmImage = Storage.ReadBitmapImageFromFile(imagePath, BitmapCreateOptions.None);               
                galleryimage.Height = Window.Current.Bounds.Height;                
                galleryimage.Width = Window.Current.Bounds.Width;
                galleryimage.Stretch = Stretch.UniformToFill;              
                _performanceProgressBargallery.IsIndeterminate = false;
                _performanceProgressBargallery.Visibility = Visibility.Collapsed;
                this.Visibility = Visibility.Visible;
            }
            else
            {
                AppSettings.imageno = imagePath;
                GalleryImageInfo galleryimageinfo = new GalleryImageInfo();

                galleryimageinfo = Listofgalleryimages.Where(i => i.Title == imagePath + ".jpg").FirstOrDefault();
                try
                {
                    galleryimage.Source = galleryimageinfo.FullImage;
                }
                catch (Exception ex)
                {


                }
                _performanceProgressBargallery.IsIndeterminate = false;
                _performanceProgressBargallery.Visibility = Visibility.Collapsed;
                if (!AppResources.ShowAdControl)
                {                   
                    galleryimage.Height = Window.Current.Bounds.Height;                    
                    galleryimage.Width = Window.Current.Bounds.Width;
                }
            }
        }

        #endregion
      
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
               // FlurryWP8SDK.Api.LogPageView();
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In PersonGalleryPopup.cs file.", ex);
            }
            //HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom Method In PersonGalleryPopup.cs file.", ex);
            }
        }

        #region Events
        private void galleryimage_ManipulationCompleted_1(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            try
            {
                _performanceProgressBargallery.Visibility = Visibility.Visible;
                _performanceProgressBargallery.IsIndeterminate = true;
                //gallery images for download manger
                if (AppSettings.AddNewShowIconVisibility)
                {
                    var gallery = Task.Run(async () => await Constants.connection.Table<ShowLinks>().ToListAsync()).Result;
                    foreach (var gal in gallery)
                    {
                        filenames.Add(gal.LinkUrl);
                    }
                    if (x < 0)
                    {
                        if (filecount == int.Parse(galleryImageCount) - 1)
                        {
                            fileid = 0;
                            SetImageSource(filenames[fileid].ToString());
                            filecount = (fileid + 1);
                            fileid++;
                        }
                        else
                        {
                            filecount = Convert.ToInt32(fileid) + 1;
                            SetImageSource(filenames[fileid].ToString());
                            filecount = fileid;
                            fileid++;
                        }
                    }
                    if (x > 0)
                    {
                        if (filecount.ToString() == "0")
                        {
                            fileid = int.Parse(galleryImageCount);
                            SetImageSource(filenames[fileid].ToString());
                            filecount = (fileid - 1);
                            fileid--;
                        }
                        else
                        {
                            filecount = Convert.ToInt32(fileid) - 1;
                            SetImageSource(filenames[fileid].ToString());
                            filecount = (fileid);
                            fileid--;
                        }
                    }
                }
                //gallery images for net and previous
                else
                {
                    if (x < 0)
                    {
                        if (CurrentImageCount == galleryImageCount)
                        {
                            CurrentImageIndex = 1;
                            SetImageSource(CurrentImageIndex.ToString());
                            CurrentImageCount = (CurrentImageIndex + 1).ToString();
                            CurrentImageIndex++;

                        }
                        else
                        {
                            CurrentImageIndex = Convert.ToInt32(CurrentImageCount) + 1;
                            SetImageSource(CurrentImageIndex.ToString());
                            CurrentImageCount = (CurrentImageIndex).ToString();
                            CurrentImageIndex++;
                        }
                    }

                    if (x > 0)
                    {
                        if (CurrentImageCount == "1")
                        {
                            CurrentImageIndex = int.Parse(galleryImageCount);
                            SetImageSource(CurrentImageIndex.ToString());
                            CurrentImageCount = (CurrentImageIndex - 1).ToString();
                            CurrentImageIndex--;
                        }
                        else
                        {
                            CurrentImageIndex = Convert.ToInt32(CurrentImageCount) - 1;
                            SetImageSource(CurrentImageIndex.ToString());
                            CurrentImageCount = (CurrentImageIndex).ToString();
                            CurrentImageIndex--;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in galleryimage_ManipulationCompleted_1 Method In PersonGalleryPopup.cs file.", ex);
            }
        }

        private void galleryimage_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            try
            {                
                x = e.Delta.Translation.X;
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in galleryimage_ManipulationDelta Method In PersonGalleryPopup.cs file.", ex);
            }
        }

        private void galleryimage_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            try
            {
                _performanceProgressBargallery.Visibility = Visibility.Visible;
                _performanceProgressBargallery.IsIndeterminate = true;
                //gallery images for download manger
                if (AppSettings.AddNewShowIconVisibility)
                {
                    var gallery = Task.Run(async () => await Constants.connection.Table<ShowLinks>().ToListAsync()).Result;
                    foreach (var gal in gallery)
                    {
                        filenames.Add(gal.LinkUrl);
                    }
                    if (x < 0)
                    {
                        if (filecount == int.Parse(galleryImageCount) - 1)
                        {
                            fileid = 0;
                            SetImageSource(filenames[fileid].ToString());
                            filecount = (fileid + 1);
                            fileid++;
                        }
                        else
                        {
                            filecount = Convert.ToInt32(fileid) + 1;
                            SetImageSource(filenames[fileid].ToString());
                            filecount = fileid;
                            fileid++;
                        }
                    }
                    if (x > 0)
                    {
                        if (filecount.ToString() == "0")
                        {
                            fileid = int.Parse(galleryImageCount);
                            SetImageSource(filenames[fileid].ToString());
                            filecount = (fileid - 1);
                            fileid--;
                        }
                        else
                        {
                            filecount = Convert.ToInt32(fileid) - 1;
                            SetImageSource(filenames[fileid].ToString());
                            filecount = (fileid);
                            fileid--;
                        }
                    }
                }
                //gallery images for net and previous
                else
                {
                    if (x < 0)
                    {
                        if (CurrentImageCount == galleryImageCount)
                        {
                            CurrentImageIndex = 1;
                            SetImageSource(CurrentImageIndex.ToString());
                            CurrentImageCount = (CurrentImageIndex + 1).ToString();
                            CurrentImageIndex++;

                        }
                        else
                        {
                            CurrentImageIndex = Convert.ToInt32(CurrentImageCount) + 1;
                            SetImageSource(CurrentImageIndex.ToString());
                            CurrentImageCount = (CurrentImageIndex).ToString();
                            CurrentImageIndex++;
                        }
                    }

                    if (x > 0)
                    {
                        if (CurrentImageCount == "1")
                        {
                            CurrentImageIndex = int.Parse(galleryImageCount);
                            SetImageSource(CurrentImageIndex.ToString());
                            CurrentImageCount = (CurrentImageIndex - 1).ToString();
                            CurrentImageIndex--;
                        }
                        else
                        {
                            CurrentImageIndex = Convert.ToInt32(CurrentImageCount) - 1;
                            SetImageSource(CurrentImageIndex.ToString());
                            CurrentImageCount = (CurrentImageIndex).ToString();
                            CurrentImageIndex--;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in galleryimage_ManipulationCompleted_1 Method In PersonGalleryPopup.cs file.", ex);
            }
        }

        private void galleryimage_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            try
            {
                _performanceProgressBargallery.Visibility = Visibility.Visible;
                _performanceProgressBargallery.IsIndeterminate = true;
                //gallery images for download manger
                if (AppSettings.AddNewShowIconVisibility)
                {
                    var gallery = Task.Run(async () => await Constants.connection.Table<ShowLinks>().ToListAsync()).Result;
                    foreach (var gal in gallery)
                    {
                        filenames.Add(gal.LinkUrl);
                    }
                    if (x < 0)
                    {
                        if (filecount == int.Parse(galleryImageCount) - 1)
                        {
                            fileid = 0;
                            SetImageSource(filenames[fileid].ToString());
                            filecount = (fileid + 1);
                            fileid++;
                        }
                        else
                        {
                            filecount = Convert.ToInt32(fileid) + 1;
                            SetImageSource(filenames[fileid].ToString());
                            filecount = fileid;
                            fileid++;
                        }
                    }
                    if (x > 0)
                    {
                        if (filecount.ToString() == "0")
                        {
                            fileid = int.Parse(galleryImageCount);
                            SetImageSource(filenames[fileid].ToString());
                            filecount = (fileid - 1);
                            fileid--;
                        }
                        else
                        {
                            filecount = Convert.ToInt32(fileid) - 1;
                            SetImageSource(filenames[fileid].ToString());
                            filecount = (fileid);
                            fileid--;
                        }
                    }
                }
                //gallery images for net and previous
                else
                {
                    if (x < 0)
                    {
                        if (CurrentImageCount == galleryImageCount)
                        {
                            CurrentImageIndex = 1;
                            SetImageSource(CurrentImageIndex.ToString());
                            CurrentImageCount = (CurrentImageIndex + 1).ToString();
                            CurrentImageIndex++;

                        }
                        else
                        {
                            CurrentImageIndex = Convert.ToInt32(CurrentImageCount) + 1;
                            SetImageSource(CurrentImageIndex.ToString());
                            CurrentImageCount = (CurrentImageIndex).ToString();
                            CurrentImageIndex++;
                        }
                    }

                    if (x > 0)
                    {
                        if (CurrentImageCount == "1")
                        {
                            CurrentImageIndex = int.Parse(galleryImageCount);
                            SetImageSource(CurrentImageIndex.ToString());
                            CurrentImageCount = (CurrentImageIndex - 1).ToString();
                            CurrentImageIndex--;
                        }
                        else
                        {
                            CurrentImageIndex = Convert.ToInt32(CurrentImageCount) - 1;
                            SetImageSource(CurrentImageIndex.ToString());
                            CurrentImageCount = (CurrentImageIndex).ToString();
                            CurrentImageIndex--;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in galleryimage_ManipulationCompleted_1 Method In PersonGalleryPopup.cs file.", ex);
            }
        }

        #endregion
    }
}
