using Common.Library;
using System;
using System.Collections.Generic;
using System.IO;
//using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
using System.Xml.Linq;
using System.ComponentModel;
using OnlineVideos.Data;
using System.Threading.Tasks;
using OnlineVideos.Entities;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using OnlineVideos.Views;


namespace OnlineVideos.ViewModels
{
    public static class CreateCustomImage
    {
       static string fullpath = string.Empty;
    
       static Page Navigationpurpose = default(Page);
       public static List<Tuple<int, int, string>> tup = default(List<Tuple<int, int, string>>);

       public async static void CreateImage(string Foldername, string filename, object page = null)
       {
           try
           {

               string FolderName = Foldername;
                if (ResourceHelper.AppName == Apps.Story_Time.ToString() || ResourceHelper.AppName==Apps.Vedic_Library.ToString())
                {
                    Constants.newimage = AppSettings.ImageTitle + ".jpg";
                    tup = new List<Tuple<int, int, string>>() { new Tuple<int, int, string>(130, 190, "Images\\ListImages"), new Tuple<int, int, string>(350, 350, "Images\\scale-140"), new Tuple<int, int, string>(350, 350, "Images\\storyImages\\" + AppSettings.ShowID), new Tuple<int, int, string>(450, 450, "Images\\scale-180") };
                }                
                else
                {
                    tup = new List<Tuple<int, int, string>>() { new Tuple<int, int, string>(130, 190, "Images\\ListImages"), new Tuple<int, int, string>(350, 350, "Images\\scale-140"), new Tuple<int, int, string>(450, 450, "Images\\scale-180") };
                }
               //else if (FolderName == "PersonImages")
               //{
               //    tup = new List<Tuple<int, int, string>>() { new Tuple<int, int, string>(120, 110, "Images\\PersonImages") };
               //}
               //else
               //{
               //    tup = new List<Tuple<int, int, string>>() { new Tuple<int, int, string>(120, 110, "Images\\storyImages\\"+AppSettings.ShowUniqueID) };
               //}
               foreach (Tuple<int, int, string> tups in tup)
               {
                   int ImageHeight = tups.Item2;
                   int ImageWidth = tups.Item1;
                   string FolderNames = tups.Item3;
                   string path1 = string.Empty;
                   foreach (string path in FolderNames.Split('\\'))
                   {

                       if (!string.IsNullOrEmpty(path1))
                       {
                           path1 += "\\" + path;
                       }
                       else
                       {
                           path1 = path;
                       }
                       if (!Task.Run(async () => await Storage.FolderExists(path1)).Result)
                       {
                           StorageFolder store = ApplicationData.Current.LocalFolder;
                           var story = Task.Run(async () => await store.CreateFolderAsync(path1, CreationCollisionOption.OpenIfExists)).Result;
                       }
                   }
                   StorageFolder store8 = ApplicationData.Current.LocalFolder;

                   StorageFile file6 = Task.Run(async () => await store8.CreateFileAsync(FolderNames + "\\" + filename, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;

                   var storys = Task.Run(async () => await store8.GetFileAsync("Images\\" + FolderName + "\\" + filename)).Result;

                   BitmapDecoder decoder = await BitmapDecoder.CreateAsync(await storys.OpenReadAsync());


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

               Frame frame = Window.Current.Content as Frame;
               Page p = frame.Content as Page;
               p.Frame.GoBack();

           }
           catch (Exception ex)
           {

           }
       }
        public async static void createFrontImage(Page currentpage,string imagename,string path="",string types="")
        {
            if (types == "Person")
            {
                //OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
                Navigationpurpose = currentpage;
                if (path != "")
                {
                    fullpath = path;
                    BitmapImage bi = new BitmapImage(new Uri(App.webinfo.PivotImage, UriKind.RelativeOrAbsolute));
                    bi.CreateOptions = BitmapCreateOptions.None;
                    bi.ImageOpened += bi_ImageOpened;
                }
                else
                {
                    Grid grid = new Grid();
                    BitmapImage Bitmap = new BitmapImage();
                    Bitmap.CreateOptions = BitmapCreateOptions.None;



                    StorageFolder myIsolatedStorage = ApplicationData.Current.LocalFolder;
                    var fs = await ApplicationData.Current.LocalFolder.GetFileAsync("Movies.xml");
                    var local = await fs.OpenAsync(FileAccessMode.ReadWrite);


                    //using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    //{
                    //    using (IsolatedStorageFileStream local = myIsolatedStorage.OpenFile("personImage.jpg", FileMode.Open, FileAccess.Read))
                    //    {
                            Bitmap.SetSource(local);
                            //local.Close();
                            local.Dispose();
                            //myIsolatedStorage.Dispose();
                    //    }
                    //}
                    Image ProductImage = new Image();

                    ProductImage.Source = Bitmap;
                    grid.Children.Add(ProductImage);
                    //WriteableBitmap wbmp = new WriteableBitmap(grid, null);
                    //using (var myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())                  
                    //{
                        StorageFolder isoStore = ApplicationData.Current.LocalFolder;
                        //if (!myIsolatedStorage.DirectoryExists(ResourceHelper.ProjectName))
                        //    myIsolatedStorage.CreateDirectory(ResourceHelper.ProjectName);
                        if (myIsolatedStorage.GetFolderAsync("Images/" + "PersonImages")==null)
                            myIsolatedStorage.CreateFolderAsync("Images/" + "PersonImages");
                        int persnid = 0;
                        int minpersonid = AppSettings.MinPersonID;
                        if (Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID < minpersonid).OrderByDescending(i => i.PersonID).FirstOrDefaultAsync()).Result != null)
                            persnid = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID < minpersonid).OrderByDescending(i => i.PersonID).FirstOrDefaultAsync()).Result.PersonID + 1;
                        else
                            persnid = 2;
                        string personid = Convert.ToString(persnid);

                        var fs1 = await ApplicationData.Current.LocalFolder.GetFileAsync("Images" + "/" + "PersonImages" + "/" + personid + ".jpg");
                        using (var stream = await fs.OpenAsync(FileAccessMode.ReadWrite))
                        //using (IsolatedStorageFileStream local = myIsolatedStorage.OpenFile("Images" + "/" + "PersonImages" + "/" + personid + ".jpg", System.IO.FileMode.Create))
                        {
                            //wbmp.SaveJpeg(local, 120, 110, 0, 100);
                            //local.Close();
                            local.Dispose();
                            //myIsolatedStorage.Dispose();
                        }
                    //}
                    Navigationpurpose.Frame.GoBack();
                }
            }
            else if (types == "Story")
            {
                //OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
                Navigationpurpose = currentpage;
                if (path != "")
                {
                    fullpath = path;
                    BitmapImage bi = new BitmapImage(new Uri(App.webinfo.PivotImage, UriKind.RelativeOrAbsolute));
                    bi.CreateOptions = BitmapCreateOptions.None;
                    bi.ImageOpened += bi_ImageOpened;
                }
                else
                {

                    Grid grid = new Grid();
                    BitmapImage Bitmap = new BitmapImage();
                    Bitmap.CreateOptions = BitmapCreateOptions.None;

                    //using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    //{
                    var fs = await ApplicationData.Current.LocalFolder.GetFileAsync("Movies.xml");
                    using(var local = await fs.OpenAsync(FileAccessMode.ReadWrite))
                        //using (IsolatedStorageFileStream local = myIsolatedStorage.OpenFile("storyImage.jpg", FileMode.Open, FileAccess.Read))
                        {
                            Bitmap.SetSource(local);
                           // local.Close();
                            local.Dispose();
                          //  myIsolatedStorage.Dispose();
                        }
                    //}
                    Image ProductImage = new Image();

                    ProductImage.Source = Bitmap;
                    grid.Children.Add(ProductImage);
                    //WriteableBitmap wbmp = new WriteableBitmap(grid, null);
                    //using (var myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    //{
                        //if (!myIsolatedStorage.DirectoryExists(ResourceHelper.ProjectName))
                        //    myIsolatedStorage.CreateDirectory(ResourceHelper.ProjectName);
                    StorageFolder myIsolatedStorage = ApplicationData.Current.LocalFolder;

                        if (myIsolatedStorage.GetFolderAsync("Images/" + "storyImages/" + AppSettings.ShowUniqueID)==null)
                            myIsolatedStorage.CreateFolderAsync("Images/" + "storyImages/" + AppSettings.ShowUniqueID);



                        //using (IsolatedStorageFileStream local = myIsolatedStorage.OpenFile("Images" + "/" + "storyImages" + "/" + AppSettings.ShowUniqueID + "/" + imagename + ".jpg", System.IO.FileMode.Create))
                        //{
                        //    Constants.newimage = AppSettings.ImageTitle + ".jpg";
                        //    wbmp.SaveJpeg(local, 400, 280, 0, 100);
                        //    local.Close();
                        //    local.Dispose();
                        //    myIsolatedStorage.Dispose();
                        //}
                    //}

                    Navigationpurpose.Frame.GoBack();
                }
            }
            else if (types == "Quiz")
            {
                //OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
                Navigationpurpose = currentpage;
                if (path != "")
                {
                    fullpath = path;
                    BitmapImage bi = new BitmapImage(new Uri(App.webinfo.PivotImage, UriKind.RelativeOrAbsolute));
                    bi.CreateOptions = BitmapCreateOptions.None;
                    bi.ImageOpened += bi_ImageOpened;
                }
                else
                {

                    Grid grid = new Grid();
                    BitmapImage Bitmap = new BitmapImage();
                    Bitmap.CreateOptions = BitmapCreateOptions.None;

                    //using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    //{
                    var myIsolatedStorage = await ApplicationData.Current.LocalFolder.GetFileAsync("quizImage.jpg");                
                    using(var local = await myIsolatedStorage.OpenAsync(FileAccessMode.ReadWrite))
                        //using (IsolatedStorageFileStream local = myIsolatedStorage.OpenFile("quizImage.jpg", FileMode.Open, FileAccess.Read))
                        {
                            Bitmap.SetSource(local);
                           // local.Close();
                            local.Dispose();
                           // myIsolatedStorage.Dispose();
                        //}
                    }
                    Image ProductImage = new Image();

                    ProductImage.Source = Bitmap;
                    grid.Children.Add(ProductImage);
                    //WriteableBitmap wbmp = new WriteableBitmap(grid, null);
                    //using (var myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    //{
                    StorageFolder myIsolatedStorage1 = ApplicationData.Current.LocalFolder;
                        //if (!myIsolatedStorage.DirectoryExists(ResourceHelper.ProjectName))
                        //    myIsolatedStorage.CreateDirectory(ResourceHelper.ProjectName);
                        if (myIsolatedStorage1.GetFolderAsync("Images/" + "QuestionsImage")==null)
                            myIsolatedStorage1.CreateFolderAsync("Images/" + "QuestionsImage");
                        if (myIsolatedStorage1.GetFolderAsync("Images/" + "QuestionsImage/" + AppSettings.ShowUniqueID)==null)
                            myIsolatedStorage1.CreateFolderAsync("Images/" + "QuestionsImage/" + AppSettings.ShowUniqueID);

                        //using (IsolatedStorageFileStream local = myIsolatedStorage.OpenFile("Images" + "/" + "QuestionsImage" + "/" + AppSettings.ShowUniqueID + "/" + imagename + ".jpg", System.IO.FileMode.Create))
                        //{
                        //    wbmp.SaveJpeg(local, 200, 200, 0, 100);
                        //    local.Close();
                        //    local.Dispose();
                        //    myIsolatedStorage.Dispose();
                        //}
                    //}

                    Navigationpurpose.Frame.GoBack();
                }
            }
            else
            {
                Navigationpurpose = currentpage;
                if (path != "")
                {
                    fullpath = path;
                    BitmapImage bi = new BitmapImage(new Uri(App.webinfo.PivotImage, UriKind.RelativeOrAbsolute));
                    bi.CreateOptions = BitmapCreateOptions.None;
                    bi.ImageOpened += bi_ImageOpened;
                }
                else
                {

                    Grid grid = new Grid();
                    BitmapImage Bitmap = new BitmapImage();
                    Bitmap.CreateOptions = BitmapCreateOptions.None;

                    //using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    //{
                    var fs = await ApplicationData.Current.LocalFolder.GetFileAsync("myImage.jpg");
                    using (var local = await fs.OpenAsync(FileAccessMode.ReadWrite))
                        //using (IsolatedStorageFileStream local = myIsolatedStorage.OpenFile("myImage.jpg", FileMode.Open, FileAccess.Read))
                        {
                            Bitmap.SetSource(local);
                            //local.Close();
                            local.Dispose();
                            //myIsolatedStorage.Dispose();
                        }
                    //}
                    Image ProductImage = new Image();

                    ProductImage.Source = Bitmap;
                    grid.Children.Add(ProductImage);
                    //WriteableBitmap wbmp = new WriteableBitmap(grid, null);
                    //using (var myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    //{
                    StorageFolder myIsolatedStorage2 = ApplicationData.Current.LocalFolder;
                        //if (!myIsolatedStorage.DirectoryExists(ResourceHelper.ProjectName))
                        //    myIsolatedStorage.CreateDirectory(ResourceHelper.ProjectName);
                        if (myIsolatedStorage2.GetFolderAsync("Images")==null)
                            myIsolatedStorage2.CreateFolderAsync("Images");

                        //using (IsolatedStorageFileStream local = myIsolatedStorage.OpenFile("Images" + "/" + imagename + ".jpg", System.IO.FileMode.Create))
                        //{
                        //    wbmp.SaveJpeg(local, 130, 190, 0, 100);
                        //    local.Close();
                        //    local.Dispose();
                        //    myIsolatedStorage.Dispose();
                        //}
                    //}

                    Navigationpurpose.Frame.GoBack();
                }
            }
        }

        static async void bi_ImageOpened(object sender, RoutedEventArgs e)
        {

            try
            {

                if (Storage.DirectoryExists("/webslice/Images")==null)
                {
                    Storage.CreateDirectory("/webslice/Images");
                }

                Grid grid = new Grid();

                Canvas MainCanvas = new Canvas();
                MainCanvas.Width = 130;
                MainCanvas.Height = 190;

             
                TextBlock TitleText = new TextBlock();
                TitleText.Text = (Constants.WebSliceTitle.Length > 10) ? Constants.WebSliceTitle.Substring(0, 10) + ".." : Constants.WebSliceTitle; 
                TitleText.TextTrimming = TextTrimming.WordEllipsis;
                TitleText.FontFamily = new FontFamily("Segoe WP Semibold");
                TitleText.Foreground = new SolidColorBrush(Colors.White);
                TitleText.Margin = new Thickness(10, 0, 0, 0);
                TitleText.FontSize = 28;
                TitleText.Width = 130;

                BitmapImage ProductBitmap = new BitmapImage();
                ProductBitmap.CreateOptions = BitmapCreateOptions.None;
                //using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                //{
                StorageFolder isoStore = ApplicationData.Current.LocalFolder;
                var fs = await ApplicationData.Current.LocalFolder.GetFileAsync("myImage.jpg");                
                using(var local = await fs.OpenAsync(FileAccessMode.ReadWrite))
                    //using (IsolatedStorageFileStream local = myIsolatedStorage.OpenFile("myImage.jpg", FileMode.Open, FileAccess.Read))
                    {
                        ProductBitmap.SetSource(local);
                    }
                //}
                Image ProductImage = new Image();
                ProductImage.SetValue(Canvas.TopProperty, (double)32);
                ProductImage.Source = ProductBitmap;

                Image FavImage = new Image(); 
                FavImage.Height = 16;
                FavImage.Width =16;
                FavImage.Source = (sender as BitmapImage) as ImageSource;
                FavImage.SetValue(Canvas.LeftProperty, (double)105);
                FavImage.SetValue(Canvas.TopProperty, (double)40);

                TextBlock SelectedText = new TextBlock();
                SelectedText.Text = (App.webinfotable.SelectedText.Length > 10) ? App.webinfotable.SelectedText.Substring(0, 10) + ".." : App.webinfotable.SelectedText;
                SelectedText.TextTrimming = TextTrimming.WordEllipsis;
                SelectedText.Margin = new Thickness(10, 0, 0, 0);
                SelectedText.FontFamily = new FontFamily("Segoe WP Semibold");
                SelectedText.Foreground = new SolidColorBrush(Colors.White);
                SelectedText.FontSize = 22;
                SelectedText.SetValue(Canvas.TopProperty, (double)162);
                SelectedText.Width = 130;

                MainCanvas.Children.Add(TitleText);
                MainCanvas.Children.Add(ProductImage);
                MainCanvas.Children.Add(FavImage);
                MainCanvas.Children.Add(SelectedText);
                grid.Children.Add(MainCanvas);


                WriteableBitmap bitmap;
                byte[] srcPixels, dstPixels;
                Uri uri = new Uri(fullpath);
                var fs1 = await ApplicationData.Current.LocalFolder.GetFileAsync(fullpath);
                using (var fileStream = await fs1.OpenAsync(FileAccessMode.ReadWrite))
                {
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);
                    BitmapFrame frame = await decoder.GetFrameAsync(0);

                    // I know the parameterless version of GetPixelDataAsync works for this image
                    PixelDataProvider pixelProvider = await frame.GetPixelDataAsync();
                    srcPixels = pixelProvider.DetachPixelData();

                    // Create the WriteableBitmap
                    bitmap = new WriteableBitmap((int)frame.PixelWidth, (int)frame.PixelHeight);
                }
                //pixelStream = bitmap.PixelBuffer.AsStream();


                //WriteableBitmap wbmp = new WriteableBitmap(grid, null);
                //using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                //{
                //    using (var local = myIsolatedStorage.OpenFile(fullpath, System.IO.FileMode.OpenOrCreate))
                //    {
                //        wbmp.SaveJpeg(local, 130, 190, 0, 100);
                //        local.Close();
                //        local.Dispose();
                //        myIsolatedStorage.Dispose();
                //    }
                //}
                Constants.WebSliceImagePath = fullpath;
                Navigationpurpose.Frame.GoBack();

            }
            catch (Exception ex)
            {

            }
        }
    }
}
