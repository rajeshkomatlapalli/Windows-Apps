using System;
using System.Net;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;
//using Microsoft.Phone.Shell;
using System.Windows.Input;
//using System.Windows.Media.Imaging;
//using System.IO.IsolatedStorage;
using System.Linq;
using Common.Library;
using System.IO;
using OnlineVideos.Data;
//using OnlineVideos.Common;
using OnlineVideos.Entities;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.UI.Xaml.Media.Imaging;
//using Common.Common;

namespace OnlineVideos.UI
{
    public static class ShellTileHelper
    {
        //public static void TilePin(Uri uri, ShellTileData initialData)
        public async static void TilePin(Uri uri, TileTemplateType initialData)
        {
            SecondaryTile secondaytitle=new SecondaryTile();
            //var item = ShellTile.ActiveTiles.FirstOrDefault
            //var item = SecondaryTile.ActiveTiles.FirstOrDefault
            //            (x => x.NavigationUri == uri); //TODO: Changed chapterno to ShellTileData.Title

            var item = (await SecondaryTile.FindAllAsync()).FirstOrDefault((x)=>x.TileId=="");

            if (item == null)
                // Create the tile and pin to start. This will cause the app to be 
                // deactivated
                ShellTile.Create(uri, initialData);
        }
        public async static void Pin(Uri uri, TileTemplateType initialData)
        {
            //var item = ShellTile.ActiveTiles.FirstOrDefault
            //            (x => x.NavigationUri.ToString().Contains(initialData.Title)); //TODO: Changed chapterno to ShellTileData.Title
            var item = (await SecondaryTile.FindAllAsync()).FirstOrDefault((x) => x.TileId == initialData.Title);
            if (item == null)
                // Create the tile and pin to start. This will cause the app to be 
                // deactivated
                ShellTile.Create(uri, initialData);
        }
        public async static void UnPin(string id)
        {
            //var item = ShellTile.ActiveTiles.FirstOrDefault
            //            (x => x.NavigationUri.ToString().Contains(id));
            var item = (await SecondaryTile.FindAllAsync()).FirstOrDefault((x) => x.TileId == id);

            if (item != null)
               await item.RequestDeleteAsync();
        }

        public async static Task<bool> IsPinned(string uniqueId)
        {
            //var item = ShellTile.ActiveTiles.FirstOrDefault
            //    (x => x.NavigationUri.ToString().Contains(uniqueId));
            var item = (await SecondaryTile.FindAllAsync()).FirstOrDefault((x) => x.TileId == uniqueId);

            if (item == null)
                return false;
            else
                return true;
        }

        public async static void PinShowToStartScreen(ShowList Tile)
        {
            try
            {
                BitmapImage TileImage = null;
                if (Task.Run(async () => await Storage.FileExists(Constants.ImagePath + Tile.Image)).Result)
                {

                    TileImage =Storage.ReadBitmapImageFromFile(Constants.ImagePath + Tile.Image, BitmapCreateOptions.None);
                }
                else
                {
                    TileImage =await ResourceHelper.getShowTileImageFromInstallFolder(Constants.ImagePath + Tile.Image);
                }

                Storage.ResizeImageForLiveTile(TileImage, Tile.TileImage);
                StandardTileData TileInfo = new StandardTileData();
                TileInfo = new StandardTileData
                {
                    BackgroundImage = ResourceHelper.SecondaryTileImage(Tile.TileImage),
                    Title = Tile.Title,
                };

                ShellTileHelper.Pin(NavigationHelper.getMovieDetailPage(AppSettings.ShowID), TileInfo);
            }
            catch (Exception ex)
            {
                
                 Exceptions.SaveOrSendExceptions("Exception in PinShowToStartScreen Method In ShellTileHelper.cs file", ex);
            }
        }
        

        public static void PinVideoLinkToStartScreen(string ShowID, ShowLinks showLink)
        {
            try
            {
                bool exists = false;
                ShowList ojsShow = new ShowList();
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                TileTemplateType initialData = new TileTemplateType();
                ojsShow = OnlineShow.GetShowDetail(Convert.ToInt32(AppSettings.ShowID));
                if (ShellTileHelper.IsPinned(AppSettings.ShowLinkTitle) == true)
                {
                    ShellTileHelper.UnPin(AppSettings.ShowLinkTitle);
                    bool img = ShellTileHelper.IsPinned(AppSettings.ShowID + showLink.LinkOrder.ToString());
                    if (ShellTileHelper.IsPinned(AppSettings.ShowID + showLink.LinkOrder.ToString()) == true)
                    {
                        exists = true;
                    }
                    if (exists == false)
                    {
                        if (isoStore.FileExists(Constants.SecondaryTileImagePath + ojsShow.TileImage))
                        {
                            isoStore.DeleteFile(Constants.SecondaryTileImagePath + ojsShow.TileImage);
                        }
                    }
                }
                else
                {
                    if (isoStore.FileExists(Constants.ImagePath + ojsShow.TileImage))
                    {

                        BitmapImage im = new BitmapImage();
                        im.CreateOptions = BitmapCreateOptions.None;
                        if (ResourceHelper.AppName == Apps.Video_Mix.ToString())
                        {
                            using (IsolatedStorageFileStream local = isoStore.OpenFile("Images\\" + ojsShow.TileImage, FileMode.Open, FileAccess.Read))
                            {
                                im.SetSource(local);
                            }
                        }
                        else
                        {
                            using (IsolatedStorageFileStream local = isoStore.OpenFile(Constants.ImagePath + ojsShow.TileImage, FileMode.Open, FileAccess.Read))
                            {
                                im.SetSource(local);
                            }
                        }
                        ImageHelper.ResizeImage(im, ojsShow.TileImage);

                    }
                    else
                    {
                        BitmapImage im1 = new BitmapImage();
                        im1.CreateOptions = BitmapCreateOptions.None;
                        Stream s = Application.GetResourceStream(new Uri(Constants.ImagePath.Substring(1) + ojsShow.TileImage, UriKind.Relative)).Stream;
                        im1.SetSource(s);
                        ImageHelper.ResizeImage(im1, ojsShow.TileImage);
                    }
                    initialData = new TileTemplateType
                    {
                        BackgroundImage = new Uri("isostore:/Shared/ShellContent/secondary/" + ojsShow.TileImage, UriKind.RelativeOrAbsolute),
                        Title = ojsShow.Title + "-" + showLink.Title,
                        BackContent = showLink.Title,
                    };

                    Uri uri = new Uri("/Views/" + AppResources.DetailPageName + ".xaml?id=" + AppSettings.ShowID + "&chapter=" + showLink.Title + "&id+cno=" + AppSettings.ShowID + showLink.Title + "&secondarytileindex=" + Constants.topsongnavigation.ToString(), UriKind.Relative);
                    Constants.topsongnavigation = "";
                    ShellTileHelper.Pin(uri, initialData);
                }
            }
            catch (Exception ex)
            {
                
          Exceptions.SaveOrSendExceptions("Exception in PinVideoLinkToStartScreen Method In ShellTileHelper.cs file", ex);
            }

        }
        public static void PinMovieToStartScreen(string ShowID)
        {
            try
            {
                bool exists = false;
                ShowList ojsShow = new ShowList();
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                TileTemplateType initialData = new TileTemplateType();
                ojsShow = OnlineShow.GetShowDetail(Convert.ToInt32(AppSettings.ShowID));
                if (ShellTileHelper.IsPinned(ShowID) == true)
                {
                    ShellTileHelper.UnPin(ShowID);
                    bool img = ShellTileHelper.IsPinned(AppSettings.ShowID);
                    if (ShellTileHelper.IsPinned(AppSettings.ShowID) == true)
                    {
                        exists = true;
                    }
                    if (exists == false)
                    {
                        if (isoStore.FileExists(Constants.SecondaryTileImagePath + ojsShow.TileImage))
                        {
                            isoStore.DeleteFile(Constants.SecondaryTileImagePath + ojsShow.TileImage);
                        }
                    }
                }
                else
                {
                    if (isoStore.FileExists(Constants.ImagePath + ojsShow.TileImage))
                    {

                        BitmapImage im = new BitmapImage();
                        im.CreateOptions = BitmapCreateOptions.None;
                        using (IsolatedStorageFileStream local = isoStore.OpenFile(Constants.ImagePath + ojsShow.TileImage, FileMode.Open, FileAccess.Read))
                        {
                            im.SetSource(local);
                        }
                        ImageHelper.ResizeImage(im, ojsShow.TileImage);

                    }
                    else
                    {
                        BitmapImage im1 = new BitmapImage();
                        im1.CreateOptions = BitmapCreateOptions.None;
                        Stream s = Application.GetResourceStream(new Uri(Constants.ImagePath.Substring(1) + ojsShow.TileImage, UriKind.Relative)).Stream;
                        im1.SetSource(s);
                        ImageHelper.ResizeImage(im1, ojsShow.TileImage);
                    }
                    initialData = new TileTemplateType
                    {
                        BackgroundImage = new Uri("isostore:/Shared/ShellContent/secondary/" + ojsShow.TileImage, UriKind.RelativeOrAbsolute),
                        Title = ojsShow.Title,
                        //BackContent = showLink.Title,
                    };

                    Uri uri = new Uri("/Views/" + AppResources.DetailPageName + ".xaml?id=" + AppSettings.ShowID, UriKind.Relative);
                    ShellTileHelper.Pin(uri, initialData);
                }
            }
            catch (Exception ex)
            {
                
               Exceptions.SaveOrSendExceptions("Exception in PinMovieToStartScreen Method In ShellTileHelper.cs file", ex);
            }

        }
        public static void PinSubjectLinkToStartScreen(string ShowID, QuizList showLink)
        {
            try
            {
                bool exists = false;

                ShowList ojsShow = new ShowList();
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                TileTemplateType initialData = new TileTemplateType();
                ojsShow = OnlineShow.GetShowDetail(Convert.ToInt32(AppSettings.ShowID));

                if (ShellTileHelper.IsPinned(AppSettings.ShowLinkTitle) == true)
                {
                    ShellTileHelper.UnPin(AppSettings.ShowLinkTitle);


                    bool img = ShellTileHelper.IsPinned(AppSettings.ShowID + showLink.QuizID.ToString());
                    if (ShellTileHelper.IsPinned(AppSettings.ShowID + showLink.QuizID.ToString()) == true)
                    {
                        exists = true;
                    }

                    if (exists == false)
                    {
                        if (isoStore.FileExists(Constants.SecondaryTileImagePath + ojsShow.TileImage))
                        {
                            isoStore.DeleteFile(Constants.SecondaryTileImagePath + ojsShow.TileImage);
                        }

                    }
                }
                else
                {
                    if (isoStore.FileExists(Constants.ImagePath + ojsShow.TileImage))
                    {

                        BitmapImage im = new BitmapImage();
                        im.CreateOptions = BitmapCreateOptions.None;

                        using (IsolatedStorageFileStream local = isoStore.OpenFile(Constants.ImagePath + ojsShow.TileImage, FileMode.Open, FileAccess.Read))
                        {

                            im.SetSource(local);
                        }

                        ImageHelper.ResizeImage(im, ojsShow.TileImage);

                    }
                    else
                    {
                        BitmapImage im1 = new BitmapImage();

                        im1.CreateOptions = BitmapCreateOptions.None;

                        Stream s = Application.GetResourceStream(new Uri(Constants.ImagePath.Substring(1) + ojsShow.TileImage, UriKind.Relative)).Stream;

                        im1.SetSource(s);
                        ImageHelper.ResizeImage(im1, ojsShow.TileImage);
                    }

                    initialData = new TileTemplateType
                    {

                        BackgroundImage = new Uri("isostore:/Shared/ShellContent/secondary/" + ojsShow.TileImage, UriKind.RelativeOrAbsolute),
                        Title = ojsShow.Title + "-" + showLink.Name,
                        BackContent = showLink.Name,

                    };
                    Uri uri = new Uri("/Views/" + AppResources.DetailPageName + ".xaml?id=" + AppSettings.ShowID + "&chapter=" + showLink.Name + "&id+Sno=" + AppSettings.ShowID + showLink.Name, UriKind.Relative);

                    ShellTileHelper.Pin(uri, initialData);
                }
            }
            catch (Exception ex)
            {
                
              Exceptions.SaveOrSendExceptions("Exception in PinSubjectLinkToStartScreen Method In ShellTileHelper.cs file", ex);
            }
        }
        public static void PinTileToStartScreen(string ShowID)
        {
            try
            {
                bool exists = false;
                ShowList ojsShow = new ShowList();
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                TileTemplateType initialData = new TileTemplateType();
                ojsShow = OnlineShow.GetShowDetail(Convert.ToInt32(Task.Run(async () => await Constants.connection.Table<ShowList>().ToListAsync()).Result.Max(i => i.ShowID).ToString()));
                if (ShellTileHelper.IsPinned(ShowID) == true)
                {
                    ShellTileHelper.UnPin(ShowID);
                    bool img = ShellTileHelper.IsPinned(AppSettings.ShowID);
                    if (ShellTileHelper.IsPinned(Task.Run(async () => await Constants.connection.Table<ShowList>().ToListAsync()).Result.Max(i => i.ShowID).ToString()) == true)
                    {
                        exists = true;
                    }
                    if (exists == false)
                    {
                        if (isoStore.FileExists(Constants.SecondaryTileImagePath + ojsShow.TileImage))
                        {
                            isoStore.DeleteFile(Constants.SecondaryTileImagePath + ojsShow.TileImage);
                        }
                    }
                }
                else
                {
                    if (isoStore.FileExists(Constants.ImagePath + ojsShow.TileImage))
                    {

                        BitmapImage im = new BitmapImage();
                        im.CreateOptions = BitmapCreateOptions.None;
                        using (IsolatedStorageFileStream local = isoStore.OpenFile(Constants.ImagePath + ojsShow.TileImage, FileMode.Open, FileAccess.Read))
                        {
                            im.SetSource(local);
                        }
                        ImageHelper.ResizeImage(im, ojsShow.TileImage);

                    }
                    else
                    {
                        BitmapImage im1 = new BitmapImage();
                        im1.CreateOptions = BitmapCreateOptions.None;
                        Stream s = Application.GetResourceStream(new Uri(Constants.ImagePath.Substring(1) + ojsShow.TileImage, UriKind.Relative)).Stream;
                        im1.SetSource(s);
                        ImageHelper.ResizeImage(im1, ojsShow.TileImage);
                    }

                    initialData = new TileTemplateType
                    {
                        BackgroundImage = new Uri("isostore:/Shared/ShellContent/secondary/" + ojsShow.TileImage, UriKind.RelativeOrAbsolute),
                        //Title = ojsShow.Title,
                        //BackContent = showLink.Title,
                    };

                    Uri uri = new Uri("/Views/" + AppResources.DetailPageName + ".xaml?id=" + Task.Run(async () => await Constants.connection.Table<ShowList>().ToListAsync()).Result.Max(i => i.ShowID).ToString(), UriKind.Relative);
                    ShellTileHelper.TilePin(uri, initialData);

                }
            }
            catch (Exception ex)
            {
                
                 Exceptions.SaveOrSendExceptions("Exception in PinTileToStartScreen Method In ShellTileHelper.cs file", ex);
            }

        }
      

        
    }
}

