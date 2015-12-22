using Common.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using System.Reflection;
//using WinRTXamlToolkit.Composition;
using Windows.Graphics.Imaging;
//using Indian_Cinema;
using OnlineVideos.Views;

namespace OnlineVideos.ViewModels
{
    public static class CreateCustomImage
    {
      public static Image Img = default(Image);
      public static Image FavImg = default(Image);
      public  static Grid grid = default(Grid);
      public static List<Tuple<int, int, string>> tup = default(List<Tuple<int, int, string>>);
     
        public async static void CreateImage(string Foldername,string filename,object page=null)
        {
            try
            {
              string FolderName = Foldername;
              tup = new List<Tuple<int, int, string>>() { new Tuple<int, int, string>(130, 160, "Images\\ListImages"), new Tuple<int, int, string>(150, 150, "Images\\TileImages\\150-150"), new Tuple<int, int, string>(30, 30, "Images\\TileImages\\30-30"), new Tuple<int, int, string>(310, 150, "Images\\TileImages\\310-150"), new Tuple<int, int, string>(350, 350, "Images\\scale-140"), new Tuple<int, int, string>(450, 450, "Images\\scale-180") };
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
                     if(!string.IsNullOrEmpty(path1))
                     {
                         path1+="\\"+path;
                     }
                     else
                     {
                         path1=path;
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
                //Frame frame = InsertIntoDataBase.retrieveframe.getframe(((PhotoChooser)page).GetType().GetTypeInfo().Assembly.FullName);
                //frame.GoBack();
            }
            catch (Exception ex)
            {
              
            }
        }
    }
}