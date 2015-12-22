using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using System.Threading;
using OnlineVideos.Views;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using System.Threading.Tasks;
using OnlineVideos;
using Windows.Storage.Pickers;
using Common.Library;
using OnlineVideos.Entities;
using InsertIntoDataBase;
using OnlineVideos.View_Models;
using Windows.Common.Data;
using Indian_Cinema.Views;
using Windows.ApplicationModel.Activation;
using System.Collections.ObjectModel;
using HtmlAgilityPack;
using Windows.ApplicationModel.Core;
using Windows.Storage.Streams;
//using Callisto;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineMovies.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserUpload : Page
    {
        AddShow addshow = new AddShow();
        public static string imagetype = string.Empty;
        public bool TaskNavigation = false;
        public AutoResetEvent auto = new AutoResetEvent(false);
        public static bool navigated = false;
        ShowDetail showdetail;
        double ratingvalue = 0.0;
        IDictionary<string, int> Dic = new Dictionary<string, int>();
        
        public UserUpload()
        {
            this.InitializeComponent();
            showdetail = new ShowDetail();
            Loaded += UserUpload_Loaded;            
            
        }

        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            Constants.Description.Clear();
            Constants.OnlineImageUrls.Clear();
            Constants.UserImage1 = null;
            Constants.DownloadTimer.Start();
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

      void UserUpload_Loaded(object sender, RoutedEventArgs e)
        {         
            try
            {
                //FlurryWP8SDK.Api.LogEvent("UserUpload Page", true);
                LoadAds();
                tblkVideosTitle.Text = "CREATE" + " " + AppResources.Type.ToUpper();
                if (ResourceHelper.AppName == "Indian_Cinema.WindowsPhone" || ResourceHelper.AppName == Apps.Indian_Cinema_Pro.ToString())
                {
                    genr.Text = "Genre";
                    if (Dic.Keys.Count == 0)
                    {
                        Dictionary<string, int> cat = addshow.GetCat();
                        foreach (var ss in cat)
                        {
                            Dic.Add(ss.Key, ss.Value);
                        }
                        lpiccategory.ItemsSource = Dic.Keys;
                        lpiccategory.SelectedIndex = 0;
                    }

                }
                else if (ResourceHelper.AppName == Apps.Recipe_Shows.ToString() || ResourceHelper.AppName == Apps.Online_Education.ToString())
                {
                    if (Dic.Keys.Count == 0)
                    {
                        Dic.Add("--Choose Category--", 0);
                        List<CatageoryTable> cat = addshow.GetCatageories();
                        foreach (CatageoryTable list in cat)
                        {
                            if (!Dic.ContainsKey(list.CategoryName))
                                Dic.Add(list.CategoryName, list.CategoryID);
                        }
                        lpiccategory.ItemsSource = Dic.Keys;
                        lpiccategory.SelectedIndex = 0;
                    }
                }
                else if (ResourceHelper.AppName == "Kids_TV.WindowsPhone" || ResourceHelper.AppName == Apps.Kids_TV_Pro.ToString() || ResourceHelper.AppName == Apps.Kids_TV.ToString())
                {
                    genr.Text = "Age";
                    if (Dic.Keys.Count == 0)
                    {
                        Dic.Add("--Choose Category--", 0);                     
                        List<ShowList> showlist = addshow.GetShowList();
                        foreach (ShowList list in showlist)
                        {
                            if (list.ReleaseDate != null)
                            {
                                if (!Dic.ContainsKey(list.ReleaseDate))
                                     Dic.Add(list.ReleaseDate, 0);
                            }
                        }
                        lpiccategory.ItemsSource = Dic.Keys;
                        lpiccategory.SelectedIndex = 0;
                    }
                }
                else
                {
                    genr.Visibility = Visibility.Collapsed;
                    lpiccategory.Visibility = Visibility.Collapsed;
                }
                type.Text = AppResources.Type + " Name";
                if (!string.IsNullOrEmpty(Constants.movietitle))
                    tblkshowname.Text = Constants.movietitle;
                if (Constants.Description != null)
                {
                    txtlimit.Visibility = Visibility.Collapsed;
                    tblkdes.Text = Constants.Description.ToString();
                    txtlength.Text = Convert.ToString(tblkdes.Text.Length) + "/" + Convert.ToString(4000);
                }
                if (Constants.UserImage != null)
                {
                    string title = tblkshowname.Text;
                    IRandomAccessStream stream = addshow.GetImageFromStorage("ListImages", title);                   
                    BitmapImage ProductBitmap = new BitmapImage();                 
                    ProductBitmap.SetSource(stream);                   
                    stream.Dispose();
                    if (imagetype == "LocalImage")
                        userlocalimg.Source = ProductBitmap;
                    if (imagetype == "OnlineImage")
                        useronlineimg.Source = ProductBitmap;
                    Constants.UserImage = null;

                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in UserUpload_Loaded event In UserUpload.cs file.", ex);
            }
        }

        private void LoadAds()
        {
            try
            {
                LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 2);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In UserUpload file", ex);
                string excepmess = "Exception in LoadAds Method In UserUpload file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }
      
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            try
            {                
                if (TaskNavigation == false)
                {
                    if (Constants.Description.Length > 0)
                    {
                        txtlimit.Visibility = Visibility.Collapsed;
                        tblkdes.Text = Constants.Description.ToString();
                        txtlength.Text = Convert.ToString(tblkdes.Text.Length) + "/" + Convert.ToString(4000);
                    }
                    if (Constants.UserImage1 != null)
                    {
                        if (navigated == false && Constants.navigation == false)
                        {
                            navigated = true;                          
                            //NavigationValues.ImageName = tblkshowname.Text;
                            //Frame.Navigate(typeof(PhotoChooser_New), NavigationValues);
                        }
                        else
                        {
                            navigated = false;
                            Constants.UIThread = true;
                            if (imagetype == "LocalImage")
                            {
                                userlocalimg.Source =ResourceHelper.getShowTileImage(tblkshowname.Text + ".jpg");
                                useronlineimg.Source = new BitmapImage(new Uri("ms-appx:///Images/fromonline.png", UriKind.RelativeOrAbsolute));
                            }
                            if (imagetype == "OnlineImage")
                            {
                                useronlineimg.Source =ResourceHelper.getShowTileImage(tblkshowname.Text + ".jpg");
                                userlocalimg.Source = new BitmapImage(new Uri("ms-appx:///Images/fromlocal.png", UriKind.RelativeOrAbsolute));
                            }
                            Constants.UIThread = false;
                            Constants.UserImage1 = null;
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
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo event In UserUpload.cs file.", ex);
            }
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            if (tb.Text.Contains("Choose Category"))
            {
                tb.Visibility = Visibility.Collapsed;
                tb.Loaded -= TextBlock_Loaded;
            }
        }
        private void imgTitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {           
            Frame.Navigate(typeof(MainPage));
        }
        
        private void userlocalimg_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            CoreApplicationView view = CoreApplication.GetCurrentView();
            try
            {
                Constants.Description.Clear();
                Constants.Description.Append(tblkdes.Text);
                Constants.movietitle = tblkshowname.Text;
                AppSettings.ImageTitle = tblkshowname.Text;                
                Constants.navigation = true;
                imagetype = "LocalImage";               
                try
                {
                    FileOpenPicker openPicker = new FileOpenPicker();
                    openPicker.ViewMode = PickerViewMode.Thumbnail;
                    openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                    openPicker.FileTypeFilter.Add(".jpg");
                    openPicker.FileTypeFilter.Add(".jpeg");
                    openPicker.FileTypeFilter.Add(".bmp");
                    openPicker.FileTypeFilter.Add(".png");
                    
                    openPicker.PickSingleFileAndContinue();
                    view.Activated += view_Activated;
                }
                catch (Exception ex)
                {

                }                    
                TaskNavigation = true;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in userlocalimg_MouseLeftButtonDown_1 event In UserUpload.cs file.", ex);
            }
        }

       async void view_Activated(CoreApplicationView sender, IActivatedEventArgs args1)
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

        private async void useronlineimg_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                Constants.Description.Clear();
                Constants.Description.Append(tblkdes.Text);
                Constants.movietitle = tblkshowname.Text;
                AppSettings.ImageTitle = tblkshowname.Text;
                imagetype = "OnlineImage";
                await addshow.GetOnlineImages("Movie", tblkshowname.Text);                
                Frame.Navigate(typeof(OnlineImages_New), "Tile");
                Window.Current.Content = Frame;
                Window.Current.Activate();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in useronlineimg_MouseLeftButtonDown_1 event In UserUpload.cs file.", ex);
            }
        }

        public ObservableCollection<string> GetImages(string input)
        {
            ObservableCollection<string> list = new ObservableCollection<string>();
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(input);
                foreach (HtmlNode node in doc.DocumentNode.Descendants("img"))
                {
                    if (node.OuterHtml.Contains("src") || node.OuterHtml.Contains("data-src") || node.OuterHtml.Contains("dfr-src") || node.OuterHtml.Contains("deferredsrc") || node.OuterHtml.Contains("src2"))
                    {
                        if (((node.Attributes["src"] == null) ? (node.Attributes["data-src"] == null) ? (node.Attributes["src2"] == null) ? (node.Attributes["dfr-src"] == null) ? node.Attributes["deferredsrc"] : node.Attributes["dfr-src"] : node.Attributes["src2"] : node.Attributes["data-src"] : node.Attributes["src"]).Value.StartsWith("http"))
                        {
                            list.Add(((node.Attributes["src"] == null) ? (node.Attributes["data-src"] == null) ? (node.Attributes["src2"] == null) ? (node.Attributes["dfr-src"] == null) ? node.Attributes["deferredsrc"] : node.Attributes["dfr-src"] : node.Attributes["src2"] : node.Attributes["data-src"] : node.Attributes["src"]).Value);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in GetImages event In UserUpload.cs file.", ex);
            }
            return list;
        }

        private void btnsave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tblkshowname.Text) && lpiccategory.SelectedIndex != 0)
                {
                    valid.Visibility = Visibility.Collapsed;
                    if (tblkdes.Text.Length > 4000)
                    {
                        txtlimit.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        txtlimit.Visibility = Visibility.Collapsed;
                        IStorageFolder iso = ApplicationData.Current.LocalFolder;
                        InsertData<ShowList> insert = new InsertData<ShowList>();
                        insert.ParameterList = new ShowList();

                        insert.ParameterList.ShowID = (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.Status == "Custom").OrderByDescending(i => i.ShowID).FirstOrDefaultAsync()).Result != null) ? Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.Status == "Custom").OrderByDescending(i => i.ShowID).FirstOrDefaultAsync()).Result.ShowID + 1 : 1;
                        insert.ParameterList.Title = tblkshowname.Text;
                        insert.ParameterList.Description = tblkdes.Text;
                        insert.ParameterList.Rating = ratingvalue;
                        insert.ParameterList.Status = "Custom";
                        insert.ParameterList.CreatedDate = System.DateTime.Now;

                        if (ResourceHelper.AppName == Apps.Kids_TV_Shows.ToString() || ResourceHelper.AppName == Apps.Kids_TV_Pro.ToString() || ResourceHelper.AppName == Apps.Kids_TV.ToString())
                        {
                            insert.ParameterList.ReleaseDate = lpiccategory.SelectedItem.ToString();
                            insert.ParameterList.SubTitle = "English";
                        }
                        else if (ResourceHelper.AppName == Apps.Story_Time.ToString() || ResourceHelper.AppName == Apps.Animation_Planet.ToString() || ResourceHelper.AppName == Apps.Vedic_Library.ToString())
                            insert.ParameterList.SubTitle = "English";
                        else
                        {
                            insert.ParameterList.ReleaseDate = System.DateTime.Now.Subtract(TimeSpan.FromDays(1)).ToString("dd MMMM yyyy");
                            insert.ParameterList.SubTitle = "None";
                        }

                        string title = tblkshowname.Text;
                        if (!Task.Run(async () => await Storage.FileExists("Images\\ListImages\\" + title + ".jpg")).Result)
                            insert.ParameterList.TileImage = "Default.jpg";
                        else
                            insert.ParameterList.TileImage = "ListImages\\" + tblkshowname.Text + ".jpg";

                        insert.Insert();
                        if (ResourceHelper.AppName == "Indian_Cinema.WindowsPhone" || ResourceHelper.AppName == Apps.Indian_Cinema_Pro.ToString())
                        {
                            InsertData<CategoriesByShowID> insert1 = new InsertData<CategoriesByShowID>();
                            insert1.ParameterList = new CategoriesByShowID();
                            insert1.ParameterList.ShowID = insert.ParameterList.ShowID;

                            insert1.ParameterList.CatageoryID = Dic.Where(i => i.Key == lpiccategory.SelectedItem.ToString()).FirstOrDefault().Value;
                            insert1.Insert();
                        }
                        else if (ResourceHelper.AppName == Apps.Bollywood_Movies.ToString() || ResourceHelper.AppName == Apps.Bollywood_Music.ToString())
                        {
                            InsertData<CategoriesByShowID> insert1 = new InsertData<CategoriesByShowID>();
                            insert1.ParameterList = new CategoriesByShowID();
                            insert1.ParameterList.ShowID = insert.ParameterList.ShowID;

                            insert1.ParameterList.CatageoryID = 7;
                            insert1.Insert();
                        }
                        else if (ResourceHelper.AppName == Apps.Recipe_Shows.ToString() || ResourceHelper.AppName == Apps.Online_Education.ToString())
                        {
                            InsertData<CategoriesByShowID> insert1 = new InsertData<CategoriesByShowID>();
                            insert1.ParameterList = new CategoriesByShowID();
                            insert1.ParameterList.ShowID = insert.ParameterList.ShowID;

                            insert1.ParameterList.CatageoryID = Dic.Where(i => i.Key == lpiccategory.SelectedItem.ToString()).FirstOrDefault().Value;
                            insert1.Insert();
                        }
                        if (ResourceHelper.AppName != Apps.Story_Time.ToString() && ResourceHelper.AppName != Apps.Vedic_Library.ToString() && ResourceHelper.AppName != Apps.Online_Education.ToString() && ResourceHelper.AppName != Apps.Fitness_Programs.ToString() && ResourceHelper.AppName != Apps.DrivingTest.ToString() && ResourceHelper.AppName != Apps.World_Dance.ToString())
                        {
                            if (insert.ParameterList.ShowID == 1)
                            {
                                AppSettings.MinPersonID = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID > 1).OrderBy(i => i.PersonID).FirstOrDefaultAsync()).Result.PersonID;
                            }
                        }
                        Constants.Description.Clear();
                        Constants.OnlineImageUrls.Clear();
                        Constants.DownloadTimer.Start();
                        Frame.GoBack();
                    }
                }
                else
                {
                    valid.Visibility = Visibility.Visible;
                }                    
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in btnsave_Click_1 event In UserUpload.cs file.", ex);
            }
        }

        private void tblkdes_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtlimit.Visibility = Visibility.Collapsed;
            txtlength.Text = Convert.ToString(tblkdes.Text.Length) + "/" + Convert.ToString(4000);
        }

        private void onlinedescimg_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Constants.Description.Clear();
            Constants.Description.Append(tblkdes.Text);
            Constants.movietitle = tblkshowname.Text;
            AppSettings.ImageTitle = tblkshowname.Text;
            Constants.navigation = true;                      
            string[] parameters = new string[3];
            parameters[0] = tblkshowname.Text;
            parameters[1] = string.Empty;
            parameters[2] = "UserUploadPage";
            Frame.Navigate(typeof(UserBrowserPage), parameters);
        }


        private void edit_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Constants.navigation = true;
            Constants.Description.Clear();
            Constants.Description.Append(tblkdes.Text);          
            Frame.Navigate(typeof(Description));
        }

        private void tblkshowname_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void tblkshowname_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void rate_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ratingvalue = rate.Value;
        }

    }
}
