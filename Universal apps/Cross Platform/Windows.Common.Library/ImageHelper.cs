using System;
using System.Net;
using System.Windows;
using System.IO;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Storage.Pickers;
using System.Collections.Generic;
using Windows.Graphics.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;

#if WP8
#if NOTANDROID
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
#endif
using System.Threading.Tasks;
using System.IO.IsolatedStorage;

#endif
#if WINDOWS_APP && NOTANDROID
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using System.Threading.Tasks;
#endif
namespace Common.Library
{
    public static class ImageHelper
    {
#if NOTANDROID
        public static BitmapImage LoadPersonImage(string filePath)
        {
            BitmapImage personImage = null;
            try
            {
                personImage = new BitmapImage(new Uri(filePath, UriKind.Relative));
            }
            catch (Exception ex)
            {
                ex.Data.Add("Person Image Path", personImage);
            }
            return personImage;
        }
#endif
        public static string LoadRatingImage(string rating)
        {
            string RatingImagePath = "";
            if(rating=="0.2")
            {
                RatingImagePath = "/Images/Rating/img3.png";
            }
            else if(rating=="0.4")
            {
                RatingImagePath = "/Images/Rating/img5.png";
            }
            else if(rating=="0.6")
            {
                RatingImagePath = "/Images/Rating/img7.png";
            }
            else if(rating=="0.8")
            {
                RatingImagePath = "/Images/Rating/img9.png";
            }
            else if (rating == "0.1")
            {
                RatingImagePath = "/Images/Rating/img11.png";
            }
            else
            {
                string rate = GetRatingValue(Convert.ToDouble(rating)).ToString();
                if (rate.Length == 1)
                    rate = rate + ".0";
                switch (rate)
                {
                    case "0.0":
                        RatingImagePath = "/Images/Rating/img1.png";
                        break;
                    case "0.5":
                        RatingImagePath = "/Images/Rating/img2.png";
                        break;
                    case "1.0":
                        RatingImagePath = "/Images/Rating/img3.png";
                        break;
                    case "1.5":
                        RatingImagePath = "/Images/Rating/img4.png";
                        break;
                    case "2.0":
                        RatingImagePath = "/Images/Rating/img5.png";
                        break;
                    case "2.5":
                        RatingImagePath = "/Images/Rating/img6.png";
                        break;
                    case "3.0":
                        RatingImagePath = "/Images/Rating/img7.png";
                        break;
                    case "3.5":
                        RatingImagePath = "/Images/Rating/img8.png";
                        break;
                    case "4.0":
                        RatingImagePath = "/Images/Rating/img9.png";
                        break;
                    case "4.5":
                        RatingImagePath = "/Images/Rating/img10.png";
                        break;
                    case "5.0":
                        RatingImagePath = "/Images/Rating/img11.png";
                        break;
                    default:
                        RatingImagePath = "/Images/Rating/img1.png";
                        break;
                }
            }
            return RatingImagePath;
        }
#if WINDOWS_PHONE_APP && NOTANDROID
        public static ImageSource GetImageForQuiz(string imgname)
        {
            //ImageSource objImgSource = (ImageSource)new ImageSourceConverter().ConvertFromString("/Images/" + imgname + ".png");
            ImageSource objImgSource = new BitmapImage(new Uri("ms-appx:///Images/" + imgname + ".png"));
            return objImgSource;
        }
        public static ImageBrush LoadPivotBackground()
        {
            ImageBrush PanoramBrush = new ImageBrush();
            BitmapImage PivotBackground = ResourceHelper.getPivotBackground();

            if (PivotBackground != null)
            {
                PanoramBrush.ImageSource = PivotBackground;
                PanoramBrush.Opacity = .6;
            }
            return PanoramBrush;
        }
        public async static void ResizeImage(ImageSource biInput, string ImgName)
        {
            try
            {
                //IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                StorageFolder isoStore = ApplicationData.Current.LocalFolder;
                string filePath = string.Empty;
                if (isoStore.GetFolderAsync("Shared/ShellContent/secondary")==null)
                   await isoStore.CreateFolderAsync("Shared/ShellContent/secondary");
                filePath = ImgName;
                if (isoStore.GetFileAsync("Shared/ShellContent/secondary/" + filePath)!=null)
                {

                    //WriteableBitmap wbOutput;
                    //Image imgTemp = new Image();
                    //imgTemp.Source = biInput;
                    //imgTemp.Opacity = 0.6;

                    //wbOutput = new WriteableBitmap(imgTemp, null);
                    //using (MemoryStream stream = new MemoryStream())
                    //{

                    //    wbOutput.SaveJpeg(stream, 173, 173, 0, 100);

                    WriteableBitmap writeableBitmap = new WriteableBitmap(300, 300);
                    StorageFile file = await StorageFile.GetFileFromPathAsync(biInput.ToString());
                    using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                    {
                        await writeableBitmap.SetSourceAsync(fileStream);
                    }
                    FileSavePicker picker = new FileSavePicker();
                    picker.FileTypeChoices.Add("JPG File", new List<string>() { ".jpg" });
                    StorageFile savefile = await picker.PickSaveFileAsync();
                    if (savefile == null)
                        return;
                    IRandomAccessStream stream1 = await savefile.OpenAsync(FileAccessMode.ReadWrite);
                    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream1);
                    // Get pixels of the WriteableBitmap object 
                    Stream pixelStream = writeableBitmap.PixelBuffer.AsStream();
                    byte[] pixels = new byte[pixelStream.Length];
                    await pixelStream.ReadAsync(pixels, 0, pixels.Length);
                    // Save the image file with jpg extension 
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)writeableBitmap.PixelWidth, (uint)writeableBitmap.PixelHeight, 96.0, 96.0, pixels);
                    await encoder.FlushAsync();

                        //using (StorageFolder myIsolatedStorage = ApplicationData.Current.LocalFolder)
                        //{
                        //    using (IsolatedStorageFileStream local = myIsolatedStorage.CreateFile("Shared/ShellContent/secondary/" + filePath))
                        //    {
                        //        local.Write(stream.GetBuffer(), 0, stream.GetBuffer().Length);
                        //        local.Close();
                        //    }
                        //}
                    }
                //}
            }
            catch (Exception ex)
            {                
                 Exceptions.SaveOrSendExceptions("Exception in ResizeImage Method In ImageHelper.cs file", ex);
            }

        }
        public async static Task<ImageBrush> LoadShowPivotBackground(string PivotImage)
        {

            string filePath = string.Empty;
            ImageBrush pivotbackground = new ImageBrush();
            var backgroundColor = Resources.PhoneBackgroundColor.ToString();
            try
            {
                if (!string.IsNullOrEmpty(PivotImage))
                {
                    filePath = System.IO.Path.Combine(Constants.PivotImagePath, PivotImage);

                    if ( Task.Run(async()=> await Storage.FileExists(filePath)).Result)
                    {
                        pivotbackground.ImageSource =await Storage.ReadImageFromLocalStorage(filePath);
                        if (backgroundColor == "#FF000000")
                        {
                            pivotbackground.Opacity = 0.5;
                        }
                    }
                    else if (PivotImage != "")
                    {
                        pivotbackground.ImageSource = new BitmapImage(new Uri(Constants.PivotImagePath + PivotImage, UriKind.RelativeOrAbsolute));

                        if (backgroundColor == "#FF000000")
                        {
                            pivotbackground.Opacity = 5;
                        }
                        else
                            pivotbackground.Opacity = 0.9;
                    }
                    else
                    {
                        pivotbackground.ImageSource = ResourceHelper.GetDefaultBackground();
                    }
                }
                else
                {
                    pivotbackground.ImageSource = ResourceHelper.GetDefaultBackground();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("id", PivotImage);
                Exceptions.SaveOrSendExceptions("Exception in LoadPtBackground Method In ImageHelper.cs file", ex);
            }
            return pivotbackground;
        }

        public async static Task<BitmapImage> LoadTileImage(string TileImage)
        {
            BitmapImage objImgSource = null;
            string filePath = "";
            try
            {
                filePath = System.IO.Path.Combine(@"Images\", TileImage);
                if (Storage.FileExistsForWp8(filePath))
                {
                    objImgSource =await Storage.ReadImageFromLocalStorage(filePath.Replace(ResourceHelper.ProjectName,string.Empty));
                }
                else
                {
                    objImgSource = new BitmapImage(new Uri("ms-appx:///Images/" + TileImage, UriKind.RelativeOrAbsolute));
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Show Tile Image", TileImage);
                Exceptions.SaveOrSendExceptions("Exception in LoadTileImage Method In ImageHelper.cs file", ex);
            }
            return objImgSource;
        }
#endif
        private static double GetRatingValue(double rating)
        {
            double rate;
            if (rating.ToString().Contains(".5"))
            {
                rate = Math.Ceiling(rating * 20) / 20;
            }
            else if (rating.ToString().Contains(".6"))
            {
                rate = (rating) - (0.1);
            }
            else
            {
                double floorValue = Math.Floor(rating);
                if ((rating - floorValue) > .5)
                {
                    return (floorValue + 1);
                }
                else
                {
                    return (floorValue);
                }
            }

            return rate;
        }
    }

}
