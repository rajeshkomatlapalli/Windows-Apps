using Common.Library;
using OnlineVideos.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineVideos.Views
{

    public sealed partial class PhotoChooser_New : Page
    {
        //private ManipulationManager _manipulationManager;
        StorageFile sourceImageFile = null;
        private string types = string.Empty;
        byte[] pixels = default(byte[]);
        BitmapDecoder decoder = default(BitmapDecoder);
        string[] array = default(string[]);
        public WriteableBitmap cropBmp = default(WriteableBitmap);
        Dictionary<Tuple<int, int, string>, string> dictionary = new Dictionary<Tuple<int, int, string>, string>();
        public static bool CropStyle { get; set; }

        public PhotoChooser_New()
        {
            this.InitializeComponent();
            Loaded += PhotoChooser_New_Loaded;
        }

        private void LoadAds()
        {
            LoadAdds.LoadAdControl_New(LayoutRoot, adstaCast, 1);
        }

        void PhotoChooser_New_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadAds();

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
                bi.ImageOpened += bi_ImageOpened;
                bi.SetSource(stream);
                stream.Dispose();
                ImgZoom.Source = bi;
            }
            catch (Exception ex)
            {
                
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

        void bi_ImageOpened(object sender, RoutedEventArgs e)
        {
            Constants.PreviousImageWidth = 250;
            Constants.PreviousImageHeight = 250;
            //this._manipulationManager = new ManipulationManager(ImgZoom, ContentPanel);
            //this._manipulationManager.OnFilterManipulation = ManipulationFilter.Clamp;
            //this._manipulationManager.Configure(true, false, true, true);
        }

        private void imgTitle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CropStyle==true)
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
                            
                            StorageFile file6 = Task.Run(async () => await store.CreateFileAsync("Images\\" + FolderName + "\\" + AppSettings.ImageTitle + ".jpg", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;

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
                    }
                    else
                    {                        
                        Frame.GoBack();
                    }
                }

                else
                {
                    ContentPanel.Visibility = Visibility.Collapsed;
                    grdprev.Visibility = Visibility.Visible;
                    CropStyle = true;
                    this.previewImage.Source = await GetCroppedBitmapAsync(this.sourceImageFile);
                    crop.Label = "save";
                    crop.Icon= new SymbolIcon(Symbol.Save);
                }
            }
            catch (Exception ex)
            {                
                Exceptions.SaveOrSendExceptions("Exception in Accept_Click Method In PhotoChooser.cs file.", ex);
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

        async private Task<byte[]> GetPixelData(BitmapDecoder decoder, uint startPointX, uint startPointY,
        uint width, uint height, uint scaledWidth, uint scaledHeight)
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
           
    }
}
