using System;
using System.Net;
using System.Windows;
using System.IO;
using System.Globalization;
using Common.Library;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
#if WINDOWS_PHONE_APP
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;
#endif
using Windows.Storage;
using OnlineVideos.Entities;
#if NOTANDROID
//using Windows.ApplicationModel;
using Windows.Graphics.Display;
using Windows.Storage.Streams;
using Windows.Foundation;
using System.Reflection;
using Windows.UI.Xaml.Controls;
#endif
#if WP8 && NOTANDROID
using System.Windows.Media.Imaging;
using System.Windows.Media;
//using Windows.Storage;
using System.IO.IsolatedStorage;
//using OnlineVideos.Entities;
#endif
#if WINDOWS_APP  && NOTANDROID
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
#endif
namespace Common.Library
{
    public static class ResourceHelper
    {
        static string title = string.Empty;
        public  static string ProjectName
        {
            get
            {
#if WINDOWS_APP  && NOTANDROID
                if (title == "")
                    title = DeviceHelp.GetAppAttribute("DisplayName").Result;
                return title;
#endif
#if WP8 || WINDOWS_PHONE_APP  && NOTANDROID
                if (title == "")
                //title = DeviceHelp.GetAppAttribute("Title").Result;
                    title = DeviceHelp.GetAppAttribute("DisplayName").Result;
                return title;
#endif
#if ANDROID
				return  "Indian Cinema"	;	
#endif
            }
        }
		public static string AppName
		{
			get
			{
				return ProjectName.Replace(" ", "_");
			}
		}
#if NOTANDROID || WINDOWS_PHONE_APP
        public static string getShowTileInstalFolderImagePath(string filename)
        {
            return Constants.ImagePath + filename;
        }
        public static string getPersonTileImagePath(string personId)
        {
            return Constants.PersonImagePath + personId + ".jpg";
        }
        public static string getPersonTileInstalFolderImagePath(string personId)
        {
            return Package.Current.InstalledLocation.Path +Constants.PersonImagePath+ personId + ".jpg";
        }
        public static ImageSource loadBitmapImageInBackground(string imagefile, BitmapCreateOptions options)
        {
            BitmapImage image = null;
            image = new BitmapImage(new Uri(imagefile, UriKind.RelativeOrAbsolute));
            return image;
        }
#if WINDOWS_PHONE_APP
        public static ImageSource getQuestionImagefromstorage(string imagename)
        {
            return getQuestionFromStorageOrInstalledFolder(imagename);
        }
        private static ImageSource getQuestionFromStorageOrInstalledFolder(string imagename)
        {
            if (Task.Run(async()=>await Storage.FileExists(getQuestionTileImagePath(imagename))).Result)
            {
                return Storage.ReadBitmapImageFromFileInBackground(getQuestionTileImagePath(imagename), BitmapCreateOptions.None);
            }
            else
            {
                return loadBitmapImageInBackground(getQuestionTileInstalFolderImagePath(imagename), BitmapCreateOptions.None);
            }
        }
        public static string getQuestionTileImagePath(string imagename)
        {
            return Constants.QuizImagePathForDownloads + AppSettings.ShowUniqueID + "/" + imagename + ".jpg";
        }
        public static string getQuestionTileInstalFolderImagePath(string imagename)
        {
            return Constants.QuestionImagePath + AppSettings.ShowUniqueID + "/" + imagename + ".png";
        }
        public static ImageSource loadBitmapImageInBackgroundForWp8(string imagefile, BitmapCreateOptions options)
        {
            BitmapImage image = null;
            try
            {                               
                AutoResetEvent wait = new AutoResetEvent(false);
                if (Constants.UIThread == false)
                {
                    Task.Run(async () =>
                    {
                        await Windows.UI.Xaml.Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                            {
                                string uri = "ms-appx://" + imagefile;
                                image = new BitmapImage(new Uri(uri, UriKind.RelativeOrAbsolute));
                                wait.Set();
                            });
                        });
                    wait.WaitOne();
                }
                else
                {
                //    //Uri myUri = new Uri(imagefile);
                //    //BitmapImage bmi = new BitmapImage();
                //    //bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                //    //bmi.UriSource = myUri;

                //    //image = bmi;
                //    //Image img = new Image();
                //    //StackPanel aa = new StackPanel();
                //    //aa.Children.Add(img);

                    string uri = "ms-appx://" + imagefile;
                    image = new BitmapImage(new Uri(uri, UriKind.RelativeOrAbsolute));

                }
                return image;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return image;
            }
        }
        public static ImageSource getStoryImageFromStorageFolder(string ImageName)
        {
            if (Task.Run(async()=>await Storage.FileExists(getstoryimgPath(ImageName))).Result)
            {
                return Storage.ReadBitmapImageFromFileInBackground(getstoryimgPath(ImageName), BitmapCreateOptions.None);
            }
            else
            {
                return loadBitmapImageInBackground(getstoryImagePath1(ImageName), BitmapCreateOptions.None);
            }
           
        }
        public static string getstoryimgPath(string ImageName)
        {
            return Constants.storyImagePath + AppSettings.ShowUniqueID + "/" + ImageName;
        }
        public static Task<string> GetLiveTileUrl(string filename)
		{
			return getImageFromStorageOrInstalledFolderForLiveTile(filename);
		}
#endif
#if WINDOWS_APP
        public static Task<string> GetLiveTileUrl(string filename)
        {
            return getImageFromStorageOrInstalledFolderForLiveTile(filename);
        }
#if WINDOWS_APP
        private static async Task<string> getImageFromStorageOrInstalledFolderForLiveTile(string filename)
        {
            if (Storage.FileExistsForWp8(getShowTileImagePath(filename)))
            {
                string[] FoldersNames = getShowTileImagePath(filename).TrimStart('/').Split('/');
                StorageFolder GetFolder1 = Task.Run(async () => await ApplicationData.Current.LocalFolder.GetFolderAsync(FoldersNames[0])).Result;
                StorageFile GetFile = Task.Run(async () => await GetFolder1.GetFileAsync(FoldersNames[1])).Result;
                // IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                StorageFolder isoStore = Windows.Storage.ApplicationData.Current.LocalFolder;
                Stream stre = Task.Run(async () => await GetFile.OpenStreamForReadAsync()).Result;
                StorageFolder store = ApplicationData.Current.LocalFolder;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(filename, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                IRandomAccessStream accessStream = await file.OpenReadAsync();
                Stream stream = accessStream.AsStreamForRead((int)accessStream.Size);
                //IAsyncOperation<StorageFile>
                var isoStoreFile = default(IAsyncOperation<StorageFile>);
                if (!Storage.FileExistsForWp8("shared/shellcontent/" + filename))
                {
                    isoStoreFile = isoStore.CreateFileAsync("shared/shellcontent/" + filename);
                    var dataBuffer = new byte[stre.Length];
                    try
                    {
                        while (stre.Read(dataBuffer, 0, dataBuffer.Length) > 0)
                        {
                            DataWriter dataWriter = new DataWriter();
                            dataWriter.WriteBuffer(stre as IBuffer);
                            //isoStoreFile.Write(dataBuffer, 0, dataBuffer.Length);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                // stre.Close();
                return "isostore:/shared/shellcontent/" + filename;
            }
            else
            {
                //return getShowTileInstalFolderImagePath(filename);
                return "/Images/scale-100/" + filename;
            }
        }
#endif
#endif
        public static string getShowTileImage1(string filename)
        {
            return getImageFromStorageOrInstalledFolder(filename);
        }

        public  static string getPersonTileImage(string personId)
        {
            return  getPersonImageFromStorageOrInstalledFolder(personId);
        }

#if WINDOWS_PHONE_APP
        public static BitmapImage getPivotBackground()
        {
            try
            {
                string image = DirectoryName + "/Images/Pivot/Background.jpg";
                BitmapImage pivotBackground = new BitmapImage(new Uri(image, UriKind.Relative));

                return pivotBackground;
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in getPivotBackground Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                Exceptions.SaveExceptionInLocalStorage(excepmess);
                return null;
            }
        }
        public static BitmapImage getPivotTitle()
        {
            try
            {
                string image = DirectoryName + "/Images/Pivot/Title.jpg";
                BitmapImage pivotTitle = new BitmapImage(new Uri(image, UriKind.Relative));
                return pivotTitle;
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in getPivotTitle Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                Exceptions.SaveExceptionInLocalStorage(excepmess);
                return null;
            }
        }
        public static ImageSource getStoryImageFromStorageOrInstalledFolderForWp8(string ImageName)
        {
            if (Task.Run(async () => await Storage.FileExists(getstoryImagePath1(ImageName))).Result)
            {
                return Storage.ReadBitmapImageFromFileInBackground(getstoryImagePath1(ImageName), BitmapCreateOptions.None);
            }
            else
            {
                return loadBitmapImageInBackground(getstoryImagePath1(ImageName), BitmapCreateOptions.None);
            }
        }
        public static Uri SecondaryTileImage(string filename)
        {
            return new Uri("isostore:/Shared/ShellContent/secondary/" + filename, UriKind.RelativeOrAbsolute);
        }
        public static async Task<BitmapImage> getShowTileImageFromInstallFolder(string filename)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.CreateOptions = BitmapCreateOptions.None;

            StorageFolder isoStore = Windows.Storage.ApplicationData.Current.LocalFolder;
            var GetFile=await isoStore.GetFileAsync(filename);
            Stream stre = Task.Run(async () => await GetFile.OpenStreamForReadAsync()).Result;            
            //Stream stream = Application.GetResourceStream(new Uri(filename, UriKind.Relative)).Stream;
            bitmap.SetSource(stre.AsRandomAccessStream());
            return bitmap;
        }

        public static BitmapImage getGalleryImage(long personID, int galleryImageID)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.UriSource = new Uri(@Constants.CloudImagePath + "/Gallery/" + personID.ToString() + "/" + galleryImageID.ToString() + ".jpg", UriKind.Absolute);
            bitmap.CreateOptions = BitmapCreateOptions.None;
            return bitmap;
        }
        private static async Task<string> getImageFromStorageOrInstalledFolderForLiveTile(string filename)
        {
            if (Storage.FileExistsForWp8(getShowTileImagePath(filename)))
            {
                string[] FoldersNames = getShowTileImagePath(filename).TrimStart('/').Split('/');
                StorageFolder GetFolder1 = Task.Run(async () => await ApplicationData.Current.LocalFolder.GetFolderAsync(FoldersNames[0])).Result;
                StorageFile GetFile = Task.Run(async () => await GetFolder1.GetFileAsync(FoldersNames[1])).Result;
               // IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                StorageFolder isoStore = Windows.Storage.ApplicationData.Current.LocalFolder;
                Stream stre = Task.Run(async () => await GetFile.OpenStreamForReadAsync()).Result;
                StorageFolder store = ApplicationData.Current.LocalFolder;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(filename, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                IRandomAccessStream accessStream = await file.OpenReadAsync();
                Stream stream = accessStream.AsStreamForRead((int)accessStream.Size);
                //IAsyncOperation<StorageFile>
                var isoStoreFile = default(IAsyncOperation<StorageFile>);
                if (! Storage.FileExistsForWp8("shared/shellcontent/" + filename))
                {
                    isoStoreFile = isoStore.CreateFileAsync("shared/shellcontent/" + filename);
                    var dataBuffer = new byte[stre.Length];
                    try
                    {
                        while (stre.Read(dataBuffer, 0, dataBuffer.Length) > 0)
                        {
                            DataWriter dataWriter = new DataWriter();
                            dataWriter.WriteBuffer(stre as IBuffer);
                            //isoStoreFile.Write(dataBuffer, 0, dataBuffer.Length);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
               // stre.Close();
                return "isostore:/shared/shellcontent/" + filename;
            }
            else
            {
                return getShowTileInstalFolderImagePath(filename);
            }
        }

        public static BitmapImage GetCastPanoramaImage(string PersonID)
        {
            BitmapImage PersonPanoramaImage = new BitmapImage();
            PersonPanoramaImage.UriSource = new Uri(@Constants.CloudImagePath + "Panorama/" + PersonID + ".jpg", UriKind.Absolute);
            return PersonPanoramaImage;
        }
        public static ImageBrush LoadCastPanoramaBackground(string pid)
        {
            ImageBrush panoramabackground = new ImageBrush();
            if (NetworkHelper.IsNetworkAvailable())
            {
                panoramabackground.ImageSource = ResourceHelper.GetCastPanoramaImage(pid);
            }
            else
            {
                panoramabackground.ImageSource = ResourceHelper.GetDefaultBackground();
            }
            return panoramabackground;
        }
        public static BitmapImage GetDefaultBackground()
        {
            return new BitmapImage(new Uri("ms-appx:///Images/Pivot/Background.jpg", UriKind.RelativeOrAbsolute));
        }
       
        public static BitmapImage StartSpeech
        {
            get { return new BitmapImage(new Uri("ms-appx:///Images/StartSpeech.png", UriKind.RelativeOrAbsolute)); }
        }
        public static BitmapImage StopSpeech
        {
            get { return new BitmapImage(new Uri("ms-appx:///Images/stopspeech.png", UriKind.RelativeOrAbsolute)); }
        }

        public static BitmapImage UnpinImage
        {
            get
            {
                return new BitmapImage(new Uri("ms-appx:///Images/unpin.png", UriKind.RelativeOrAbsolute));
            }
        }
        public static string IsolatedDirectoryName
        {
            get
            {
                return "AppData/";
            }
        }
        public static BitmapImage PinImage
        {
            get
            {
                return new BitmapImage(new Uri("ms-appx:///Images/pin.png", UriKind.RelativeOrAbsolute));
            }
        }
        public static string GetMailMessageInfo()
        {
            string DeviceId = "";
            if (DeviceHelper.GetDeviceUniqueID() != null)
            {
                byte[] id = DeviceHelper.GetDeviceUniqueID();
                DeviceId = BitConverter.ToString(id).Replace("-", "");
            }
            
            var package = Package.Current;
            var version = package.Id.Version;
            var pname = package.Id.Name;
            string body = "\n\n\nDateTime : " + System.DateTime.Now +
                "\nCulture : " + CultureInfo.CurrentCulture.Name +
                "\nDevice Name : " + DeviceHelper.GetDeviceModel() +
                "\nStandard Time : " + TimeZoneInfo.Local.StandardName +
                "\nManufacturer : " + DeviceHelper.GetManufacturer()+
                "\nApp Name : " + pname.ToLowerInvariant() +
            "\n Version:" + version.ToString();
            //"\nApp Name : " + PhoneHelp.GetAppAttribute("Title") +
            //"\n Version:" + PhoneHelp.GetAppAttribute("Version");
            string name = pname.ToLowerInvariant();
            return body;
        }
#endif
        private static string getImageFromStorageOrInstalledFolder(string filename)
        {
            string FolderName = string.Empty;            
            if (DisplayProperties.ResolutionScale == ResolutionScale.Scale100Percent)
            {
                FolderName = "scale-100";
            }
            else if (DisplayProperties.ResolutionScale == ResolutionScale.Scale140Percent)
            {
                FolderName = "scale-140";
            }
            else if (DisplayProperties.ResolutionScale == ResolutionScale.Scale180Percent)
            {
                FolderName = "scale-180";
            }
            if (Task.Run(async () => await Storage.FileExists("Images\\" + filename)).Result)
            {
                return "ms-appdata:///local/Images/"+ filename;
            }

            if (Task.Run(async () => await Storage.FileExists("Images\\" + FolderName + "\\" + filename)).Result)
            {
                return "ms-appdata:///local/Images/" + FolderName + "/" + filename;
            }
            else
            {
                return "ms-appx://" + getShowTileInstalFolderImagePath(filename);
            }
        }

        public static BitmapImage RemoveFromFavoritesImage
        {
            get
            {
                return new BitmapImage(new Uri("ms-appx:///Images/NotFav.png", UriKind.RelativeOrAbsolute));
            }
        }
        public static BitmapImage AddToFavoritesImage
        {
            get
            {
                return new BitmapImage(new Uri("ms-appx:///Images/Fav.png", UriKind.RelativeOrAbsolute));
            }
        }
        public static BitmapImage getShowFavoriteStatusImage(bool IsFavorite)
        {
            if (IsFavorite == true)
                return RemoveFromFavoritesImage;
            else
                return AddToFavoritesImage;
        }

        public static Uri AppMarketplaceWebLink
        {
            get
            {
                return new Uri(Constants.AppMarketplaceWebUrl + AppResources.ApplicationProductID, UriKind.Absolute);
            }
        }

        private static string getPersonImageFromStorageOrInstalledFolder(string personId)
        {
            if (Task.Run(async () => await Storage.FileExists("Images\\PersonImages\\" + personId + ".jpg")).Result)
            {
                return "ms-appdata:///local" + getPersonTileImagePath(personId);
            }
            else
            {
                return "ms-appx://" + getPersonTileImagePath(personId);
            }
        }

        public static BitmapImage GetStoryImage(string ImageName)
        {
            string imgPath = "/Images/storyImages/" + AppSettings.ShowID + "/" + ImageName;
            BitmapImage objImg = new BitmapImage(new Uri(Package.Current.InstalledLocation.Path + imgPath, UriKind.Relative));
            return objImg;
        }

        public static string getstoryImagePath1(string ImageName)
        {

            return Constants.storyImagePath_New + AppSettings.ShowID + "/" + ImageName;
        }

        public static string getstoryImagePath(string ImageName)
        {
            return "/" + ResourceHelper.ProjectName + Constants.storyImagePath + "/" + AppSettings.ShowID + "/" + ImageName;
        }

        public static string getStoryImageFromStorageOrInstalledFolder(string ImageName)
        {
            if (Task.Run(async () => await Storage.FileExists("Images\\storyImages\\" + AppSettings.ShowID + "\\" + ImageName)).Result)
            {
                return "ms-appdata:///local" + getstoryImagePath1(ImageName);
            }
            else
            {
                return "ms-appx://" + getstoryImagePath1(ImageName);
            }
        }
       static string file1;
        public static string getQuizImageFromStorageOrInstalledFolder(string ImageName)
        {
            if (Task.Run(async () => await Storage.FileExists("Images\\QuestionsImage\\" + AppSettings.ShowID + "\\" + ImageName+ ".jpg" )).Result)
            {
                //return "ms-appdata:///local" + GetQuestionImage(ImageName);
                return "ms-appdata:///local" + "/Images/QuestionsImage/" + AppSettings.ShowID + "/" + ImageName + ".jpg";
            }
            else
            {
                return "ms-appx://" + GetQuestionImage(ImageName);
            }
        }

        public static string GetQuestionImage(string imgname)
        {
            string imgpath = "";
            imgpath = "/Images/QuestionsImage/" + AppSettings.ShowID + "/" + imgname + ".png";
            return imgpath;
        }

        public static ImageSource getGameTileImage(string Id,string filename)
        {
            return getGameImageFromStorageOrInstalledFolder(Id, filename);
        }

        private static ImageSource getGameImageFromStorageOrInstalledFolder(string Id, string filename)
        {
            return loadBitmapImageInBackground(getGameTileInstalFolderImagePath(Id, filename), BitmapCreateOptions.None);
        }

        public static string getWeaponTileImagePath(string WeaponId)
        {
            return "/" + ResourceHelper.ProjectName + Constants.WeaponImagePath + WeaponId + ".jpg";
        }

        public static string getGameTileInstalFolderImagePath(string Id, string filename)
        {
            return Package.Current.InstalledLocation.Path + Constants.ImagePath + filename + "/" + Id + ".jpg";
        }

#if WP8 || WINDOWS_PHONE_APP
        public static ImageSource getpersonTileImage1(string filename)
        {
            return getpersonImageFromStorageOrInstalledFolderForWp81(filename);
        }

        public static ImageSource getpersonTileImage(string filename)
        {
            return getpersonImageFromStorageOrInstalledFolderForWp8(filename);
        }
#endif
        public static ImageSource getShowTileImage(string filename)
        {
#if WINDOWS_APP
            string imgPath = "/Images/ListImages/" + filename;
           BitmapImage objImg = new BitmapImage(new Uri(Package.Current.InstalledLocation.Path + imgPath,UriKind.Relative));
            return objImg;
#endif
#if WP8 || WINDOWS_PHONE_APP
            return getImageFromStorageOrInstalledFolderForWp8(filename);
#endif
        }
#if WP8 || WINDOWS_PHONE_APP
        private static ImageSource getImageFromStorageOrInstalledFolderForWp8(string filename)
        {
            if (Storage.FileExistsForWp8(getShowTileImagePath(filename)))
            {
                return Storage.ReadBitmapImageFromFileInBackground(getShowTileImagePath(filename), BitmapCreateOptions.None);
            }
            else
            {
                return loadBitmapImageInBackgroundForWp8(getShowTileInstalFolderImagePath(filename), BitmapCreateOptions.None);
            }            
        }
        private static ImageSource getpersonImageFromStorageOrInstalledFolderForWp8(string filename)
        {
            if (Storage.FileExistsForWp8(getpersonTileImagePath(filename)))
            {
                return Storage.ReadBitmapImageFromFileInBackground(getpersonTileImagePath(filename), BitmapCreateOptions.None);
            }
            else
                return null;
        }

        private static ImageSource getpersonImageFromStorageOrInstalledFolderForWp81(string filename)
        {
            if (Storage.FileExistsForWp8(getpersonTileImagePath1(filename)))
            {
                return Storage.ReadBitmapImageFromFileInBackground(getpersonTileImagePath1(filename), BitmapCreateOptions.None);
            }
            else
                return null;
        }
#endif
        public static string getViewAllImagePath(string filename)
        {
            return Constants.ListImagePath + filename;
        }

        public static string getDownLoadmagePath(string filename)
        {
            return Constants.DlVImagePath+ filename;
        }

        public static string getViewAllImageFromStorageOrInstalledFolder(string filename)
        {
            string FolderName = "scale-100";
            if (Task.Run(async () => await Storage.FileExists("Images\\" + filename)).Result)
            {
                return "ms-appdata:///local/Images/" + filename;
            }
            if (Task.Run(async () => await Storage.FileExists("Images\\" + FolderName + "\\" + filename)).Result)
            {
                return "ms-appdata:///local/Images/" + FolderName + "/" + filename;
            }
            if (Task.Run(async () => await Storage.FileExists("Images\\ListImages\\" + filename)).Result)
            {
                return "ms-appdata:///local/Images/ListImages/" + filename;
            }
            else
            {
                return "ms-appx://" + getViewAllImagePath(filename);
            }
        }

        public static string getDownLoadImagesFromStorageOrInstalledFolder(string filename)
        {
            if (Task.Run(async () => await Storage.FileExists("Images\\DownLoadVideoImages\\" + filename)).Result)
            {
                return "ms-appdata:///local/Images/DownLoadVideoImages/" + filename;
            }
            else
            {
                return "ms-appx://" + getDownLoadmagePath(filename);
            }
        }
        
        public static string getShowTileImagePath(string filename)
        {
            string FolderName = string.Empty;
            if (DisplayProperties.ResolutionScale == ResolutionScale.Scale100Percent)
            {
                FolderName = "scale-100";
            }
            else if (DisplayProperties.ResolutionScale == ResolutionScale.Scale140Percent)
            {
                FolderName = "scale-140";
            }
            else if (DisplayProperties.ResolutionScale == ResolutionScale.Scale180Percent)
            {
                FolderName = "scale-180";
            }
            if (Task.Run(async () => await Storage.FileExists("Images\\" + filename)).Result)
            {
                //string file= filename.Replace("\\", "/");
                //return "ms-appdata:///local/Images/" + file;
                return "Images\\" + filename;
            }

            if (Task.Run(async () => await Storage.FileExists("Images\\" + FolderName + "\\" + filename)).Result)
            {
                return "ms-appdata:///local/Images/" + FolderName + "/" + filename;
            }
            else
            {
                string imagepath = string.Empty;
                if (ResourceHelper.AppName == Apps.Web_Media.ToString())
                {
                    int showid = Convert.ToInt32(AppSettings.IDForImagePath);
                    string genre = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result.Genre;
                    if (genre == "Videos")
                        imagepath = "/" + Constants.ImagePath + filename;
                    else
                        imagepath = "/" + ResourceHelper.ProjectName + Constants.ImagePath + filename;
                }
                else
                    imagepath = "/" + Constants.ImagePath + filename;
                return imagepath;
            }
        }

        public static string getpersonTileImagePath(string filename)
        {
            return "/" + ResourceHelper.ProjectName + Constants.PersonImagePath + filename;
        }

        public static string getpersonTileImagePath1(string filename)
        {
            return Constants.PersonImagePath + filename;
        }

        public static string getListShowTileImage(string filename)
        {
			return  getViewAllImageFromStorageOrInstalledFolder(filename);
        }
#endif
#if ANDROID
		public static string IsolatedDirectoryName
		{
			get
			{
				return "AppData/";
			}
		}
#endif
        public static string DirectoryName
        {
            get
            {
                //return "AppData/" + ProjectName;
                return "";
            }
        }

        public static string RingToneFileName(string filename)
        {
            return filename + ".mp3";
        }
    }
}