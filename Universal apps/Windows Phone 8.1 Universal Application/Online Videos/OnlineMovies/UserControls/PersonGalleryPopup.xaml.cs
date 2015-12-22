using Common.Library;
using OnlineVideos.Entities;
using OnlineVideos.Library;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indian_Cinema.UserControls
{
    public sealed partial class PersonGalleryPopup : UserControl
    {
        #region GlobalDeclaration

        public static int pat;
        public static int filecount;
        public static int fileid = 1;
        public static int ind1;
        public static string path1;
        public static List<string> filenames;
        double x;
        public string galleryImageCount;
        #endregion

        #region Constructor
        public PersonGalleryPopup()
        {
            this.InitializeComponent();          
                Window.Current.SizeChanged += (s,e)=>
                {               
                LayoutRoot.Width = Window.Current.Bounds.Width;                
                LayoutRoot.Height = Window.Current.Bounds.Height;
                };

            filenames = new List<string>();
            galleryImageCount = string.Empty;
        }
        
        #endregion

        #region "Common Methods"
        public void Close()
        {
            popMessage.IsOpen = false;
            AppSettings.bcount = false;
        }
        public void Show(string img)
        {
            try
            {
                string path = string.Empty;
                string name = string.Empty;

                if (AppSettings.AddNewShowIconVisibility)
                {
                    var gallery1 = Task.Run(async () => await Constants.connection.Table<ShowLinks>().ToListAsync()).Result;
                    galleryImageCount = gallery1.Count().ToString();
                    int linkorder = Convert.ToInt32(img);
                    var gallery = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkOrder == linkorder).ToListAsync()).Result; //from i in context.ShowLinks where i.LinkOrder ==Convert.ToInt32(img) select i;
                    foreach (var gal in gallery)
                    {
                        path = gal.LinkUrl;
                        name = System.IO.Path.GetFileName(path);
                    }
                    AppSettings.bcount = true;
                    popMessage.IsOpen = true;
                    SetImageSource(path);
                }
                else
                {
                    galleryImageCount = AppSettings.gallcount;
                    AppSettings.bcount = true;
                    popMessage.IsOpen = true;

                    ind1 = img.IndexOf('.');
                    path1 = img.Substring(0, ind1);
                    SetImageSource(path1);
                }
            }
            catch (Exception ex)
            {
                
                Exceptions.SaveOrSendExceptions("Exception in Show Method In PersonGalleryPopup.cs file.", ex);
            }
        }
        public void SetImageSource(string imagePath)
        {
            if (AppSettings.AddNewShowIconVisibility)
            {               
                image1.Height = Window.Current.Bounds.Height;                
                image1.Width = Window.Current.Bounds.Width;
                image1.Stretch = Stretch.UniformToFill;                
                this.Visibility = Visibility.Visible;
            }
            else
            {
                AppSettings.imageno = imagePath;
                style();
                BitmapImage GalleryImage = GalleryHelper.GetGalleryImageList(AppSettings.PersonUniqueID).FullImage as BitmapImage;
                if (GalleryImage != null)
                  image1.Source = GalleryImage;
                if (!AppResources.ShowAdControl)
                {
                    image1.Height = Window.Current.Bounds.Height;
                    image1.Width = Window.Current.Bounds.Width;
                }
                this.Visibility = Visibility.Visible;
            }
        }
        void style()
        {
            image1.RenderTransform = new ScaleTransform();
            Duration duration = new Duration(TimeSpan.FromSeconds(0));
            DoubleAnimation myDoubleAnimation1 = new DoubleAnimation();
            myDoubleAnimation1.Duration = duration;
            Storyboard sb = new Storyboard();
            sb.Duration = duration;
            sb.Children.Add(myDoubleAnimation1);
            Storyboard.SetTarget(myDoubleAnimation1, image1);            
            Storyboard.SetTargetProperty(myDoubleAnimation1, "Opacity");
            myDoubleAnimation1.To = 10.00;
            myDoubleAnimation1.From = 0.00;
            LayoutRoot.Resources.Add("unique_id", sb);
            sb.Begin();
            LayoutRoot.Resources.Remove("unique_id");
        }
        #endregion

        #region Events
        private void image1_ManipulationCompleted_1(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            try
            {
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
                else
                {
                    if (x < 0)
                    {
                        if (path1 == galleryImageCount)
                        {
                            pat = 1;
                            SetImageSource(pat.ToString());
                            path1 = (pat + 1).ToString();
                            pat++;

                        }
                        else
                        {
                            pat = Convert.ToInt32(path1) + 1;
                            SetImageSource(pat.ToString());
                            path1 = (pat).ToString();
                            pat++;
                        }
                    }

                    if (x > 0)
                    {
                        if (path1 == "1")
                        {
                            pat = int.Parse(galleryImageCount);
                            SetImageSource(pat.ToString());
                            path1 = (pat - 1).ToString();
                            pat--;
                        }
                        else
                        {
                            pat = Convert.ToInt32(path1) - 1;
                            SetImageSource(pat.ToString());
                            path1 = (pat).ToString();
                            pat--;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in image1_ManipulationCompleted_1 Method In PersonGalleryPopup.cs file.", ex);
            }
        }

        private void image1_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            try
            {               
                x = e.Delta.Translation.X;
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in image1_ManipulationDelta Method In PersonGalleryPopup.cs file.", ex);
            }
        }
        #endregion
    }
}
