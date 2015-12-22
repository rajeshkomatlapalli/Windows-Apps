//using AdRotator;
using Common.Library;
using OnlineVideos.ViewModels;
using OnlineVideos.ViewModels.Manipulations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Reflection;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PhotoChooser : Page
    {
        StorageFile sourceImageFile = null;
        private ManipulationManager _manipulationManager;
        DispatcherTimer appbartimer = default(DispatcherTimer);
        string[] array = default(string[]);
        Dictionary<Tuple<int, int, string>, string> dictionary = new Dictionary<Tuple<int, int, string>, string>();
        byte[] pixels = default(byte[]);
        public WriteableBitmap cropBmp = default(WriteableBitmap);
        BitmapDecoder decoder = default(BitmapDecoder);
        public PhotoChooser()
        {
            this.InitializeComponent();
            Loaded += PhotoChooser_Loaded;
            Unloaded += PhotoChooser_Unloaded;
        }

        void PhotoChooser_Loaded(object sender, RoutedEventArgs e)
        {
            Tuple<int, int, string> tiletuple = new Tuple<int, int, string>(250, 250, "scale-100");
            Tuple<int, int, string> persontuple = new Tuple<int, int, string>(120, 110, "PersonImages");
            Tuple<int, int, string> storytuple = new Tuple<int, int, string>(400, 280, "storyImages" + "\\" + AppSettings.ShowUniqueID);
            Tuple<int, int, string> quiztuple = new Tuple<int, int, string>(200, 200, "QuestionsImage" + "\\" + AppSettings.ShowUniqueID);
            dictionary.Add(tiletuple, "Tile");
            dictionary.Add(persontuple, "Person");
            dictionary.Add(storytuple, "Story");
            dictionary.Add(quiztuple, "Quiz");
            StorageFolder store = ApplicationData.Current.LocalFolder;
            StorageFile file = Task.Run(async () => await store.CreateFileAsync("SelectedImage.jpg", CreationCollisionOption.OpenIfExists)).Result;
            this.sourceImageFile = file;
            IRandomAccessStream stream = Task.Run(async () => await file.OpenAsync(Windows.Storage.FileAccessMode.Read)).Result;
            BitmapImage bi = new BitmapImage();
            bi.CreateOptions = BitmapCreateOptions.None;
            bi.ImageOpened += bitmap_ImageOpened;
            bi.SetSource(stream);
            stream.Dispose();
            img1.Source = bi;
            Image img = new Image();
            img.Source = bi;
        }

        void bitmap_ImageOpened(object sender, RoutedEventArgs e)
        {            
            Constants.PreviousImageWidth = 250;
            Constants.PreviousImageHeight = 250;
            this._manipulationManager = new ManipulationManager(img1, grd);
            this._manipulationManager.OnFilterManipulation = ManipulationFilter.Clamp;
            this._manipulationManager.Configure(true, false, true, true);
        }

        private void BottomAppBar1_Opened(object sender, object e)
        {
            if (ResourceHelper.AppName != AppSettings.AppName)
            {
                //Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                //AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
                //adcontrol.Visibility = Visibility.Collapsed;
            }
            if (appbartimer != null)
                appbartimer.Stop();
            appbartimer = new DispatcherTimer();
            appbartimer.Interval = TimeSpan.FromSeconds(6);
            appbartimer.Tick += appbartimer_Tick;
            appbartimer.Start();
        }

        private void BottomAppBar1_Closed(object sender, object e)
        {
            if (ResourceHelper.AppName != AppSettings.AppName)
            {
                //Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                //AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
                //adcontrol.Visibility = Visibility.Visible;
            }
        }

        void appbartimer_Tick(object sender, object e)
        {
            BottomAppBar1.IsOpen = false;
            appbartimer.Stop();
        }

        private async void crop_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (crop.Style == (Style)Application.Current.Resources["SaveAppbarStyle"])
                {
                    Constants.newimage = AppSettings.ImageTitle + ".jpg";
                    int ImageHeight = default(int);
                    int ImageWidth = default(int);
                    string FolderName = string.Empty;
                    Constants.NavigationFromPhotoChooser = true;
                    StorageFolder store1 = ApplicationData.Current.LocalFolder;
                    StorageFile file = Task.Run(async () => await store1.CreateFileAsync("test.jpg", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
                    using (IRandomAccessStream newImgFileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        Guid encoderID = Guid.Empty;
                        switch (file.FileType.ToLower())
                        {
                            case ".png":
                                encoderID = BitmapEncoder.PngEncoderId;
                                break;
                            case ".bmp":
                                encoderID = BitmapEncoder.BmpEncoderId;
                                break;
                            default:
                                encoderID = BitmapEncoder.JpegEncoderId;
                                break;
                        }
                        BitmapEncoder bmpEncoder = await BitmapEncoder.CreateAsync(
                            encoderID,
                            newImgFileStream);
                        bmpEncoder.SetPixelData(
                            BitmapPixelFormat.Bgra8,
                            BitmapAlphaMode.Straight,
                             (uint)Math.Sqrt((pixels.Length) / 4),
                            (uint)Math.Sqrt((pixels.Length) / 4),
                            decoder.DpiX,
                            decoder.DpiY,
                            pixels);
                        await bmpEncoder.FlushAsync();
                        newImgFileStream.Dispose();
                    }
                    foreach (KeyValuePair<Tuple<int, int, string>, string> dic in dictionary)
                    {
                        if (dic.Value.ToString() == array[0].ToString())
                        {
                            ImageHeight = ((Tuple<int, int, string>)dic.Key).Item2;
                            ImageWidth = ((Tuple<int, int, string>)dic.Key).Item1;
                            FolderName = ((Tuple<int, int, string>)dic.Key).Item3;

                            StorageFolder store = ApplicationData.Current.LocalFolder;
                            if (!Task.Run(async () => await Storage.FolderExists("Images")).Result)
                            {
                                var story = Task.Run(async () => await store.CreateFolderAsync("Images", CreationCollisionOption.OpenIfExists)).Result;
                            }
                            if (!Task.Run(async () => await Storage.FolderExists("Images\\" + FolderName)).Result)
                            {
                                var story = Task.Run(async () => await store.CreateFolderAsync("Images\\" + FolderName, CreationCollisionOption.OpenIfExists)).Result;
                            }
                            StorageFile file6;
                            //if (!Task.Run(async () => await Storage.FileExists("Images\\" + FolderName + "\\" + AppSettings.ImageTitle + ".jpg")).Result)
                            //{
                                file6 = Task.Run(async () => await store.CreateFileAsync("Images\\" + FolderName + "\\" + AppSettings.ImageTitle + ".jpg", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
                            //}

                            //file6 = Task.Run(async () => await store.GetFileAsync("Images\\" + FolderName + "\\" + AppSettings.ImageTitle + ".jpg")).Result;
                            var storys = Task.Run(async () => await store.GetFileAsync("test.jpg")).Result;
                            decoder = await BitmapDecoder.CreateAsync(await storys.OpenReadAsync());
                            BitmapTransform transform = new BitmapTransform() { ScaledHeight = (uint)ImageHeight, ScaledWidth = (uint)ImageWidth };
                            PixelDataProvider pixelData = await decoder.GetPixelDataAsync(
                                BitmapPixelFormat.Rgba8,
                                BitmapAlphaMode.Straight,
                                transform,
                                ExifOrientationMode.RespectExifOrientation,
                                ColorManagementMode.DoNotColorManage);
                            using (var destinationStream = await file6.OpenAsync(FileAccessMode.ReadWrite))
                            {
                                Guid encoderID = Guid.Empty;
                                switch (file6.FileType.ToLower())
                                {
                                    case ".png":
                                        encoderID = BitmapEncoder.PngEncoderId;
                                        break;
                                    case ".bmp":
                                        encoderID = BitmapEncoder.BmpEncoderId;
                                        break;
                                    default:
                                        encoderID = BitmapEncoder.JpegEncoderId;
                                        break;
                                }
                                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(encoderID, destinationStream);
                                encoder.SetPixelData(BitmapPixelFormat.Rgba8, BitmapAlphaMode.Premultiplied, (uint)ImageWidth, (uint)ImageHeight, decoder.DpiX, decoder.DpiY, pixelData.DetachPixelData());
                                await encoder.FlushAsync();
                                destinationStream.Dispose();
                            }
                        }
                    }
                    if (FolderName == "scale-100")
                    {
                        CreateCustomImage.CreateImage(FolderName, AppSettings.ImageTitle + ".jpg", this);

                        Frame frame = InsertIntoDataBase.retrieveframe.getframe(((PhotoChooser)this).GetType().GetTypeInfo().Assembly.FullName);
                        frame.GoBack();
                    }
                    else
                    {
                        Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);
                        frame.GoBack();
                        //Frame.GoBack();
                    }
                    //CreateCustomImage.createFrontImage(ResourceHelper.getShowTileImage1((Task.Run(async () => await Constants.connection.Table<ShowList>().FirstOrDefaultAsync()).Result != null ? Task.Run(async () => await Constants.connection.Table<ShowList>().OrderByDescending(i => i.ShowID).FirstOrDefaultAsync()).Result.ShowID + 1 : 1).ToString() + ".jpg"), this);
                }
                else
                {
                    grd.Visibility = Visibility.Collapsed;
                    grdview.Visibility = Visibility.Collapsed;
                    previewview.Visibility = Visibility.Visible;
                    crop.Style = (Style)Application.Current.Resources["SaveAppbarStyle"];
                    this.previewImage.Source = await GetCroppedBitmapAsync(this.sourceImageFile);
                }
            }
            catch (Exception ex)
            {
                string mss = ex.Message;
                Frame.GoBack();
            }
        }

        async public Task<ImageSource> GetCroppedBitmapAsync(StorageFile originalImageFile)
        {
            try
            {
                uint startPointX = (uint)Math.Floor((Constants.ImageRectangle.Left < 0) ? -Constants.ImageRectangle.Left : Constants.ImageRectangle.Left);
                uint startPointY = (uint)Math.Floor((Constants.ImageRectangle.Top < 0) ? -Constants.ImageRectangle.Top : Constants.ImageRectangle.Top);
                uint height = (uint)250;
                uint width = (uint)250;

                using (IRandomAccessStream stream = await originalImageFile.OpenReadAsync())
                {
                    decoder = await BitmapDecoder.CreateAsync(stream);
                    uint scaledWidth = (uint)Math.Floor(Constants.CurrentImageWidth);
                    uint scaledHeight = (uint)Math.Floor(Constants.CurrentImageHeight);
                    if (startPointX == 0)
                        startPointX = 5;
                    if (startPointY == 0)
                        startPointY = 5;
                    if (scaledWidth == 0)
                        scaledWidth = 260;
                    if (scaledHeight == 0)
                        scaledHeight = 260;
                    if (scaledWidth - startPointX < width)
                    {
                        width = scaledWidth - startPointX;
                    }
                    if (scaledHeight - startPointY < height)
                    {
                        height = scaledHeight - startPointY;
                    }

                    pixels = await GetPixelData(decoder, startPointX, startPointY, width, height,
                        scaledWidth, scaledHeight);
                    cropBmp = new WriteableBitmap((int)width, (int)height);
                    Stream pixStream = cropBmp.PixelBuffer.AsStream();
                    pixStream.Write(pixels, 0, (int)(width * height * 4));
                    stream.Dispose();
                    return cropBmp;
                }
            }
            catch (Exception ex)
            {
                string mss = ex.Message;
                return null;
            }
        }
        async private Task<byte[]> GetPixelData(BitmapDecoder decoder, uint startPointX, uint startPointY, uint width, uint height, uint scaledWidth, uint scaledHeight)
        {
            try
            {
                BitmapTransform transform = new BitmapTransform();
                BitmapBounds bounds = new BitmapBounds();
                bounds.X = startPointX;
                bounds.Y = startPointY;
                bounds.Height = height;
                bounds.Width = width;
                transform.Bounds = bounds;
                transform.ScaledWidth = scaledWidth;
                transform.ScaledHeight = scaledHeight;

                // Get the cropped pixels within the bounds of transform.
                PixelDataProvider pix = await decoder.GetPixelDataAsync(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Straight,
                    transform,
                    ExifOrientationMode.IgnoreExifOrientation,
                    ColorManagementMode.ColorManageToSRgb);
                byte[] pixels = pix.DetachPixelData();
                return pixels;
            }
            catch (Exception ex)
            {
                string mss = ex.Message;
                return null;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            array = new string[2];
            if (e.Parameter != null)
            {
                array = e.Parameter as string[];
            }
        }

        void PhotoChooser_Unloaded(object sender, RoutedEventArgs e)
        {
            if (ResourceHelper.AppName != AppSettings.AppName)
            {
                Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                //AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
                //adcontrol.Visibility = Visibility.Visible;
            }
        }

        private void BackButton_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}