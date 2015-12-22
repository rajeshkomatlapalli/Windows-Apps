using Common.Library;
using Common.Utilities;
using Indian_Cinema.Views;
using InsertIntoDataBase;
using OnlineVideos;
using OnlineVideos.Entities;
using OnlineVideos.Views;
using PicasaMobileInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Common.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineMovies.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Story : Page
    {
        public static string oldimage = string.Empty;
        public string ImageUrl = string.Empty;
        public string PageName = string.Empty;
        public static string imagetype = string.Empty;
        public bool TaskNavigation = false;
        public AutoResetEvent auto = new AutoResetEvent(false);
        public static bool navigated = false;
        AddShow addshow = new AddShow();

        public Story()
        {
            this.InitializeComponent();
            Loaded += Story_Loaded;
        }
        private void LoadAds()
        {
            try
            {
                LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In Story file", ex);
                string excepmess = "Exception in LoadAds Method In Story file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }
        void Story_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAds();
            if (PageName != null)
            {

            }
            if (Constants.editstory == true)
            {
                if (ResourceHelper.AppName == Apps.Story_Time.ToString())
                    tblkVideosTitle.Text = "Edit Story Paragraph";
                else
                    tblkVideosTitle.Text = "Edit Vedic Paragraph";
            }
            else
            {
                if (ResourceHelper.AppName == Apps.Story_Time.ToString())
                    tblkVideosTitle.Text = "Add Story Paragraph";
                else
                    tblkVideosTitle.Text = "Add Vedic Paragraph";
            }
            int showid = AppSettings.ShowUniqueID;
            List<Stories> str = new List<Stories>();
            List<Stories> str1 = new List<Stories>();
            if (Constants.editstory == true)
            {
                int paraid = Constants.ParaId;
                str = Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == showid && i.paraId < paraid).OrderByDescending(i => i.paraId).ToListAsync()).Result.Take(2).ToList();
                str1 = Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == showid && i.paraId > paraid).OrderBy(i => i.paraId).ToListAsync()).Result.Take(2).ToList();
            }
            else
            {
                str = Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == showid).OrderByDescending(i => i.ID).ToListAsync()).Result.Take(2).ToList();
            }
            if ((Constants.editstory == true) ? (str.Where(i => i.Image != "").FirstOrDefault() != null || str1.Where(i => i.Image != "").FirstOrDefault() != null) : str.Where(i => i.Image != "").FirstOrDefault() != null)
            {
                //storyonlineimg.PointerPressed -= storyonlineimg_PointerPressed_1;
                storylocalimg.PointerPressed += storylocalimg_PointerPressed;
            }
            if (Constants.editstory == true)
            {
                int paraid = Constants.ParaId;

                tblkdes.Text = Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == showid && i.paraId == paraid).FirstOrDefaultAsync()).Result.Description;

                string imagename = Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == showid && i.paraId == paraid).FirstOrDefaultAsync()).Result.Image;
                //if (Task.Run(async () => await Storage.FileExists(Constants.storyImagePath + AppSettings.ShowUniqueID + "/" + imagename)).Result)
                //{
                //    Constants.UIThread = true;
                //    storylocalimg.Source = ResourceHelper.getStoryImageFromStorageFolder(imagename);
                //    Constants.UIThread = false;
                //}
                if (Task.Run(async () => await Storage.FileExists("Images\\storyImages\\" + AppSettings.ShowUniqueID + "\\" + imagename)).Result)
                {
                    storylocalimg.Source = new BitmapImage(new Uri("ms-appdata:///local" + ResourceHelper.getstoryImagePath1(imagename), UriKind.RelativeOrAbsolute));
                }
                else
                {
                    storylocalimg.Source = new BitmapImage(new Uri("ms-appx://" + ResourceHelper.getstoryImagePath1(imagename), UriKind.RelativeOrAbsolute));
                }
            }
            if (Constants.UserImage1 != null)
            {

                Constants.UIThread = true;
                if (imagetype == "LocalImage")
                {
                    if (string.IsNullOrEmpty(Constants.newimage))
                        storylocalimg.Source = new BitmapImage(new Uri("ms-appx:///Images/fromlocal.png", UriKind.RelativeOrAbsolute));
                    else
                        storylocalimg.Source = ResourceHelper.getStoryImageFromStorageFolder(AppSettings.ImageTitle + ".jpg");
                    storyonlineimg.Source = new BitmapImage(new Uri("ms-appx:///Images/fromonline.png", UriKind.RelativeOrAbsolute));
                }
                if (imagetype == "OnlineImage")
                {
                    if (string.IsNullOrEmpty(Constants.newimage))
                        storyonlineimg.Source = new BitmapImage(new Uri("ms-appx:///Images/fromonline.png", UriKind.RelativeOrAbsolute));
                    else
                        storyonlineimg.Source = ResourceHelper.getStoryImageFromStorageFolder(AppSettings.ImageTitle + ".jpg");
                    storylocalimg.Source = new BitmapImage(new Uri("ms-appx:///Images/fromlocal.png", UriKind.RelativeOrAbsolute));
                }
                Constants.UIThread = false;
                Constants.UserImage1 = null;
            }
            if (Constants.editstory != true)
            {
                if (Constants.Description != null)
                {
                    txtlimit.Visibility = Visibility.Collapsed;
                    tblkdes.Text = Constants.Description.ToString();
                    txtlength.Text = Convert.ToString(tblkdes.Text.Length) + "/" + Convert.ToString(4000);
                }
                if (Constants.UserImage != null)
                {
                    string title = AppSettings.ImageTitle;
                    IRandomAccessStream stream = addshow.GetImageFromStorage("scale-100", title);
                    BitmapImage ProductBitmap = new BitmapImage();
                    ProductBitmap.SetSource(stream);
                    stream.Dispose();
                    if (imagetype == "LocalImage")
                        storylocalimg.Source = ProductBitmap;
                    if (imagetype == "OnlineImage")
                        storyonlineimg.Source = ProductBitmap;
                    Constants.UserImage = null;

                }
            }
        }
       
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            PageName = (string)e.Parameter;
            if (TaskNavigation == false)
            {
                if (Constants.Description.Length > 0)
                {
                    txtlimit.Visibility = Visibility.Collapsed;
                    tblkdes.Text = Constants.Description.ToString();
                    txtlength.Text = Convert.ToString(tblkdes.Text.Length) + "/" + Convert.ToString(4000);

                }
                if (Constants.UserImage1 != null && Constants.navigation == false)
                {
                    if (navigated == false)
                    {
                        navigated = true;
                        //NavigationValues.ImageName = AppSettings.ImageTitle;
                        //NavigationValues.type = "Story";
                        //Frame.Navigate(typeof(PhotoChooser), NavigationValues);                       
                    }
                    else
                    {
                        navigated = false;                       
                    }
                }
                else
                    Constants.navigation = false;
            }
            else
            {
                TaskNavigation = false;
            }
        }

        private async void RequestBlogCompleted(IAsyncResult result)
        {
            try
            {
                var request = (HttpWebRequest)result.AsyncState;
                var response = (HttpWebResponse)request.EndGetResponse(result);
                StreamReader responseReader = new StreamReader(response.GetResponseStream());
                string responseStr = responseReader.ReadToEnd();
                Stream strm = new MemoryStream(Encoding.UTF8.GetBytes(responseStr));
                XElement MyXMLConfig = XElement.Load(strm);
                XNamespace atomNS = "http://www.w3.org/2005/Atom";
                XNamespace Img = "http://schemas.microsoft.com/ado/2007/08/dataservices";
                XNamespace met = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

                Constants.OnlineImageUrls = new ObservableCollection<string>(MyXMLConfig.Descendants(atomNS + "entry").Descendants(met + "properties").Elements().Where(i => i.Name == Img + "MediaUrl").Select(i => i.Value).ToList());

                await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                {
                    PageHelper.NavigateToOnlineImagesPage(AppResources.OnlineImagesPageName);
                });
            }
            catch (Exception ex)
            {
                Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                {
                    PageHelper.NavigateToOnlineImagesPage(AppResources.OnlineImagesPageName);
                });
            }
        }

        private void imgTitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {

        }

        private void onlinedescimg_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Constants.navigation = true;
            string[] para = new string[3];
            para[0] = AppSettings.Title + "Story";
            para[1] = string.Empty;
            para[2] = "Story";
            //NavigationValues.querytext = AppSettings.Title;
            //NavigationValues.searchquery = string.Empty;
            Frame.Navigate(typeof(UserBrowserPage), para);            
        }

        private void edit_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Constants.Description.Clear();
            Constants.Description.Append(tblkdes.Text);
            Constants.navigation = true;            
            Frame.Navigate(typeof(Description));
        }

        private void storylocalimg_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                PhotoChooser_New.CropStyle = false;
                CoreApplicationView view = CoreApplication.GetCurrentView();
                Constants.navigation = true;
                imagetype = "LocalImage";
                int paraid = Constants.ParaId;

                oldimage = (Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == AppSettings.ShowUniqueID && i.paraId == paraid).FirstOrDefaultAsync()).Result != null) ? Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == AppSettings.ShowUniqueID && i.paraId == paraid).FirstOrDefaultAsync()).Result.Image : string.Empty;
                if (Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == AppSettings.ShowUniqueID).OrderByDescending(i => i.Image).FirstOrDefaultAsync()).Result == null)
                {
                    AppSettings.ImageTitle = "1";
                }
                else
                {
                    string image = string.Empty;
                    if (Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == AppSettings.ShowUniqueID && i.Image != "").OrderByDescending(i => i.Image).FirstOrDefaultAsync()).Result != null)
                    {
                        image = Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == AppSettings.ShowUniqueID && i.Image != "").OrderByDescending(i => i.Image).FirstOrDefaultAsync()).Result.Image;
                    }
                    else
                    {
                        image = string.Empty;
                    }

                    if (!string.IsNullOrEmpty(image))
                        AppSettings.ImageTitle = Convert.ToString(Convert.ToInt32(image.Substring(0, image.IndexOf(".jpg"))) + 1);
                    else
                        AppSettings.ImageTitle = "1";
                }
                //FileOpenPicker openPicker = new FileOpenPicker();
                //openPicker.ViewMode = PickerViewMode.Thumbnail;
                //openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                //openPicker.FileTypeFilter.Add(".jpg");
                //openPicker.FileTypeFilter.Add(".jpeg");
                //openPicker.FileTypeFilter.Add(".png");
                //// Launch file open picker and caller app is suspended and may be terminated if required 
                //openPicker.PickSingleFileAndContinue();     
                Constants.Description.Clear();
                Constants.Description.Append(tblkdes.Text);
                AppSettings.ImageTitle = AppSettings.ImageTitle;

                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".jpeg");
                openPicker.FileTypeFilter.Add(".bmp");
                openPicker.FileTypeFilter.Add(".png");

                openPicker.PickSingleFileAndContinue();
                view.Activated += View_Activated;
                TaskNavigation = true;
            }
            catch (Exception ex)
            {
                
            }
        }

        private async void View_Activated(CoreApplicationView sender, IActivatedEventArgs args1)
        {
            FileOpenPickerContinuationEventArgs args = args1 as FileOpenPickerContinuationEventArgs;
            try
            {
                if (args.Files.Count > 0)
                {
                    var stream = await args.Files[0].OpenAsync(Windows.Storage.FileAccessMode.Read);
                    if (stream != null)
                    {
                        Constants.UserImage = stream.AsStream();
                        using (IRandomAccessStream fileStream = addshow.GetImageFromStorage(string.Empty, "SelectedImage.jpg"))
                        {
                            using (IOutputStream outputStream = fileStream.GetOutputStreamAt(0))
                            {
                                using (DataWriter dataWriter = new DataWriter(outputStream))
                                {
                                    MemoryStream ms = new MemoryStream();
                                    Constants.UserImage.Position = 0;
                                    Constants.UserImage.CopyTo(ms);
                                    dataWriter.WriteBytes(ms.ToArray());
                                    await dataWriter.StoreAsync();
                                    dataWriter.DetachStream();
                                }
                                await outputStream.FlushAsync();
                            }
                        }
                        Constants.UserImage1 = new BitmapImage();
                        Constants.UserImage1.SetSource(stream);
                        string[] array = new string[2];
                        array[0] = "Tile";
                        array[1] = AppSettings.ImageTitle.ToString();
                        Frame.Navigate(typeof(PhotoChooser_New), array);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        public async void ContinueFileOpenPicker(FileOpenPickerContinuationEventArgs args)
        {
            if (args.Files.Count > 0)
            {            
                var stream = await args.Files[0].OpenAsync(Windows.Storage.FileAccessMode.Read);
                var bitmapImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                await bitmapImage.SetSourceAsync(stream);
                Constants.UserImage1 = new BitmapImage();
                Constants.UserImage1.SetSource(stream);
                await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                {
                    //NavigationValues.ImageName = AppSettings.ImageTitle;
                    //NavigationValues.type = "Story";
                    //Frame.Navigate(typeof(PhotoChooser_New), NavigationValues);                   
                });              
            }
        }
        private void btnsave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tblkdes.Text.Length > 4000)
                {
                    txtlimit.Visibility = Visibility.Visible;
                }
                else
                {
                    txtlimit.Visibility = Visibility.Collapsed;
                    storylocalimg.Source = null;
                    storyonlineimg.Source = null;
                    BackgroundWorker bg = new BackgroundWorker();
                    bg.DoWork += bg_DoWork;
                    bg.RunWorkerCompleted += bg_RunWorkerCompleted;
                    //bg.RunWorkerAsync(tblkpersonname.Text);
                    bg.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in btnsave_Click event In Story.cs file.", ex);
            }
        }
        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int showid = AppSettings.ShowUniqueID;
            int paraid = Constants.ParaId;
            if (Constants.ParaId != default(int))
            {
                Stories s = Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == showid && i.paraId == paraid).FirstOrDefaultAsync()).Result;
                s.Description = tblkdes.Text;
                if (!string.IsNullOrEmpty(Constants.newimage))
                {
                    s.Image = AppSettings.ImageTitle + ".jpg";
                    s.FlickrStoryImageUrl = ImageUrl;
                    Constants.newimage = string.Empty;
                    if (!string.IsNullOrEmpty(oldimage))
                    {
                        if (s.Image != oldimage)
                        {
                            try
                            {
                                StorageFolder store = ApplicationData.Current.LocalFolder;
                                var file = Task.Run(async () => await store.GetFolderAsync("Images")).Result;
                                var file1 = Task.Run(async () => await file.GetFolderAsync("storyImages")).Result;
                                var file2 = Task.Run(async () => await file1.GetFolderAsync(showid.ToString())).Result;
                                var file3 = Task.Run(async () => await file2.GetFileAsync(oldimage)).Result;

                                var file4 = Task.Run(async () => await file3.DeleteAsync());
                                oldimage = string.Empty;
                            }
                            catch (Exception ex)
                            {
                                
                            }
                            //    string FolderName = "Images\\storyImages";
                            //    if (Task.Run(async () => await Storage.FileExists("Images/storyImages/" + showid + "/" + oldimage)).Result)
                            //    {
                            //        Storage.DeleteFile("Images/storyImages/" + showid + "/" + oldimage);
                            //    }
                            //    oldimage = string.Empty;
                        }
                    }
                }
                Constants.connection.UpdateAsync(s);
                Constants.editstory = false;
            }
            else
            {
                InsertData<Stories> insert = new InsertData<Stories>();
                insert.ParameterList = new Stories();
                insert.ParameterList.ShowID = showid;
                insert.ParameterList.Description = tblkdes.Text;
                insert.ParameterList.LanguageID = 1;
                insert.ParameterList.Title = AppSettings.Title;
                if (Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result == null)
                    insert.ParameterList.paraId = 1;
                else
                    insert.ParameterList.paraId = Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == showid).OrderByDescending(i => i.paraId).FirstOrDefaultAsync()).Result.paraId + 1;
                //IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();

                //try
                //{
                //    StorageFolder isoStore = ApplicationData.Current.LocalFolder;
                //    if (isoStore.GetFileAsync(Constants.storyImagePath + "/" + AppSettings.ShowUniqueID + "/" + AppSettings.ImageTitle + ".jpg") != null)
                //        insert.ParameterList.Image = "";
                //}
                //catch (Exception ex)
                //{
                    //if (!string.IsNullOrEmpty(Constants.newimage))
                    //{
                        insert.ParameterList.Image = AppSettings.ImageTitle + ".jpg";
                        Constants.newimage = string.Empty;
                    //}
                //}
                                                   
                insert.ParameterList.FlickrStoryImageUrl = ImageUrl;
                insert.Insert();
            }
            AppSettings.ImageTitle = string.Empty;
            Constants.UserImage1 = null;
            Constants.Description.Clear();
            if (Constants.ParaId != default(int))
            {
                Constants.ParaId = default(int);
            }
            if (string.IsNullOrEmpty(PageName))
                Frame.GoBack();
            else
            {
                Random ran = new Random();
            }
        }
        void bg_DoWork(object sender, DoWorkEventArgs e)
        {

            int showid = AppSettings.ShowUniqueID;
            int paraid = Constants.ParaId;
            if (Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == showid).OrderByDescending(i => i.Image).FirstOrDefaultAsync()).Result == null)
            {
                AppSettings.ImageTitle = "1";
            }
            else
            {
                string image = string.Empty;
                if (Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == showid && i.Image != "").OrderByDescending(i => i.Image).FirstOrDefaultAsync()).Result != null)
                {
                    image = Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == showid && i.Image != "").OrderByDescending(i => i.Image).FirstOrDefaultAsync()).Result.Image;
                }
                else
                {
                    image = string.Empty;
                }
                if (!string.IsNullOrEmpty(image))
                    AppSettings.ImageTitle = Convert.ToString(Convert.ToInt32(image.Substring(0, image.IndexOf(".jpg"))) + 1);
                else
                    AppSettings.ImageTitle = "1";
            }
            string ImageName = AppSettings.ImageTitle + ".jpg";            
            //StorageFolder isoStore = ApplicationData.Current.LocalFolder;      
            //if (isoStore.GetFileAsync(Constants.storyImagePath + "/" + AppSettings.ShowUniqueID + "/" + ImageName)!=null)
            //{
            //    //ImageUrl = string.Empty;
            //    //BlogCategoryTable bt = Task.Run(async () => await Constants.connection.Table<BlogCategoryTable>().Where(i => i.BlogType == "shows").FirstOrDefaultAsync()).Result;
            //    //if (NetworkHelper.IsNetworkAvailableForDownloads())
            //    //{
            //    //    //PicasaInterface pi = new PicasaInterface(bt.BlogUserName, bt.BlogPassword);
            //    //    //uploadImage(ImageName);
            //    //}
            //}
            //else
            //{
            //    ImageUrl = string.Empty;
            //}
        }

        public void uploadImage(string imagename)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://picasaweb.google.com/data/feed/api/user/" + CONST.USER);
            request.Method = "POST";
            request.ContentType = "image/png";
            request.Headers["Authorization"] = CONST.PIC_AUTH + CONST.AUTH_Token;
            object[] array = new object[2];
            array[0] = imagename;
            array[1] = request;
            request.BeginGetRequestStream(new AsyncCallback(GetPicasaStreamCallback), array);
            auto.WaitOne();
        }
        private async void GetPicasaStreamCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                object[] array = new object[2];
                array = (object[])asynchronousResult.AsyncState;
                HttpWebRequest request = (HttpWebRequest)array[1];
                string ImageName = array[0].ToString();              
                StorageFolder isoStore = ApplicationData.Current.LocalFolder;
                var file =await isoStore.GetFileAsync(Constants.storyImagePath + AppSettings.ShowUniqueID + "/" + ImageName);
                IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);
                Stream postStream = request.EndGetRequestStream(asynchronousResult);
                byte[] buffer = new byte[fileStream.Size / 4];
                int bytesRead = -1;
                WriteableBitmap writeableBitmap = new WriteableBitmap(300, 300);
                Stream pixelStream = writeableBitmap.PixelBuffer.AsStream();
                await pixelStream.ReadAsync(buffer, 0, buffer.Length);             
                fileStream.Position.Equals(0);
                while ((bytesRead =await pixelStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    postStream.Write(buffer, 0, bytesRead);
                }               
                request.BeginGetResponse(new AsyncCallback(RequestPicasaCompleted), request);
            }
            catch (Exception ex)
            {
                auto.Set();
            }
        }

        private void RequestPicasaCompleted(IAsyncResult result)
        {
            try
            {
                var request = (HttpWebRequest)result.AsyncState;
                var response = (HttpWebResponse)request.EndGetResponse(result);
                StreamReader responseReader = new StreamReader(response.GetResponseStream());
                XElement MyXMLConfig = XElement.Load(responseReader);
                XNamespace atomNS = "http://www.w3.org/2005/Atom";
                ImageUrl = MyXMLConfig.Descendants(atomNS + "content").Attributes().Where(i => i.Name == "src").FirstOrDefault().Value;
                auto.Set();
            }
            catch (Exception ex)
            {
                auto.Set();
            }
        }

        private void tblkdes_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            txtlimit.Visibility = Visibility.Collapsed;
            txtlength.Text = Convert.ToString(tblkdes.Text.Length) + "/" + Convert.ToString(4000);
        }

        private async void storyonlineimg_PointerPressed_1(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                PhotoChooser_New.CropStyle = false;
                imagetype = "OnlineImage";
                int paraid = Constants.ParaId;

                oldimage = (Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == AppSettings.ShowUniqueID && i.paraId == paraid).FirstOrDefaultAsync()).Result != null) ? Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == AppSettings.ShowUniqueID && i.paraId == paraid).FirstOrDefaultAsync()).Result.Image : string.Empty;
                if (Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == AppSettings.ShowUniqueID).OrderByDescending(i => i.Image).FirstOrDefaultAsync()).Result == null)
                {
                    AppSettings.ImageTitle = "1";
                }
                else
                {
                    string image = string.Empty;
                    if (Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == AppSettings.ShowUniqueID && i.Image != "").OrderByDescending(i => i.Image).FirstOrDefaultAsync()).Result != null)
                    {
                        image = Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == AppSettings.ShowUniqueID && i.Image != "").OrderByDescending(i => i.Image).FirstOrDefaultAsync()).Result.Image;
                    }
                    else
                    {
                        image = string.Empty;
                    }

                    if (!string.IsNullOrEmpty(image))
                        AppSettings.ImageTitle = Convert.ToString(Convert.ToInt32(image.Substring(0, image.IndexOf(".jpg"))) + 1);
                    else
                        AppSettings.ImageTitle = "1";
                }
                //var bingKey = Constants.bingsearch(ResourceHelper.AppName);
                //var authentication = string.Format("{0}:{1}", string.Empty, bingKey);
                //var encodedKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(authentication));
                ////HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.datamarket.azure.com/Bing/Search/v1/Composite?Sources=%27Image%27&Query=%27Yevadu%27&Adult=%27Off%27&ImageFilters=%27Size%3AMedium%27");
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.datamarket.azure.com/Bing/Search/v1/Image?Query='" + AppSettings.Title + " Movie" + "'&ImageFilters=%27Style%3aPhoto%2bSize%3aMedium%2bAspect%3aTall%27&$top=50&$format=Atom");
                //request.Method = "GET";
                //request.Headers["Authorization"] = "Basic " + encodedKey;
                //request.BeginGetResponse(new AsyncCallback(RequestBlogCompleted), request);

                Constants.Description.Clear();
                Constants.Description.Append(tblkdes.Text);
                Constants.movietitle = AppSettings.Title;
                AppSettings.ImageTitle = AppSettings.ImageTitle;
                imagetype = "OnlineImage";
                await addshow.GetOnlineImages("Movie", AppSettings.Title);
                Frame.Navigate(typeof(OnlineImages_New), "Tile");
                Window.Current.Content = Frame;
                Window.Current.Activate();
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
