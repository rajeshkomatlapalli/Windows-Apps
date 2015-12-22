//using AdRotator;
using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Reflection;
using Windows.Storage.Streams;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Common.Data;
using Windows.UI.Xaml.Media.Imaging;
using OnlineVideos.Entities;
using OnlineVideosWin81.Controls;
using System.Threading.Tasks;
using InsertIntoDataBase;
using OnlineVideos;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Views
{
    public sealed partial class UserUpload : UserControl
    {
        IDictionary<string, int> Dic = new Dictionary<string, int>();
        AddShow addshow = new AddShow();
        public static string imagetype = string.Empty;
        public UserUpload()
        {
            this.InitializeComponent();
            Loaded += UserUpload_Loaded;
        }

        void UserUpload_Loaded(object sender, RoutedEventArgs e)
        {

            if (ResourceHelper.AppName != AppSettings.AppName)
            {                
            }
            if (ResourceHelper.AppName == Apps.Indian_Cinema_Pro.ToString() || ResourceHelper.AppName=="Indian_Cinema.Windows" || ResourceHelper.AppName=="Indian Cinema.Windows")
            {
                tblkTitle.Text = "Create Show";
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
                tblkTitle.Text = "Create Quiz";
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
            else if (ResourceHelper.AppName == "Kids_TV.Windows" || ResourceHelper.AppName == Apps.Kids_TV_Pro.ToString())
            {
                tblkTitle.Text = "Create Show";
                genr.Text = "Age";
                if (Dic.Keys.Count == 0)
                {
                    Dic.Add("--Choose Category--", 0);

                    List<ShowList> showlist = addshow.GetShowList();
                    foreach (ShowList list in showlist)
                    {
                        try
                        {
                            if (!Dic.ContainsKey(list.ReleaseDate))
                                Dic.Add(list.ReleaseDate, 0);
                        }
                        catch (Exception ex)
                        {
                            Exceptions.SaveOrSendExceptions("Exception in UserUpload_Loaded Method In UserUploa.xaml.cs file", ex);
                        }
                    }
                    lpiccategory.ItemsSource = Dic.Keys;
                    lpiccategory.SelectedIndex = 0;
                }
            }
            else
            {
                tblkTitle.Text = "Create Show";
                catstck.Visibility = Visibility.Collapsed;
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
                IRandomAccessStream stream = addshow.GetImageFromStorage("scale-100", title);
                BitmapImage ProductBitmap = new BitmapImage();
                ProductBitmap.CreateOptions = BitmapCreateOptions.None;
                ProductBitmap.SetSource(stream);
                stream.Dispose();
                if (imagetype == "LocalImage")
                    userlocalimg.Source = ProductBitmap;
                if (imagetype == "OnlineImage")
                    useronlineimg.Source = ProductBitmap;
                Constants.UserImage = null;
            }
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Constants.Description.Clear();
            Constants.OnlineImageUrls.Clear();
            Constants.UserImage = null;
            Constants.movietitle = string.Empty;
            PopupManager.EnableControl();
            if (ResourceHelper.AppName != AppSettings.AppName)
            {
                
            }
            Frame f = Window.Current.Content as Frame;
            Page p = f.Content as Page;
            p.Frame.Navigate(typeof(MainPage));
        }

        private void onlinedescimg_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            Constants.Description.Clear();
            Constants.Description.Append(tblkdes.Text);
            Constants.movietitle = tblkshowname.Text;
            string[] array = new string[2];
            array[0] = tblkshowname.Text;
            array[1] = string.Empty;
            Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);
            frame.Navigate(typeof(UserBrowserPage), array);
            Window.Current.Content = frame;
            Window.Current.Activate();
        }

        private void edit_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            Constants.Description.Clear();
            Constants.Description.Append(tblkdes.Text);
            Constants.movietitle = tblkshowname.Text;
            Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);
            frame.Navigate(typeof(Description), "Movie");
            Window.Current.Content = frame;
            Window.Current.Activate();
        }

        private async void userlocalimg_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                Constants.Description.Clear();
                Constants.Description.Append(tblkdes.Text);
                Constants.movietitle = tblkshowname.Text;
                AppSettings.ImageTitle = tblkshowname.Text;
                imagetype = "LocalImage";
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".jpeg");
                openPicker.FileTypeFilter.Add(".bmp");
                openPicker.FileTypeFilter.Add(".png");
                StorageFile imgFile = await openPicker.PickSingleFileAsync();
                if (imgFile != null)
                {
                    IRandomAccessStream stream = await imgFile.OpenAsync(FileAccessMode.Read);
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
                    string[] array = new string[2];
                    array[0] = "Tile";
                    array[1] = AppSettings.ImageTitle.ToString();
                    Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);

                    frame.Navigate(typeof(PhotoChooser), array);
                    Window.Current.Content = frame;
                    Window.Current.Activate();
                }
            }
            catch(Exception ex)
            {
                string excep = ex.Message;
            }
        }

        private async void useronlineimg_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tblkshowname.Text))
                {
                    valid.Visibility = Visibility.Visible;
                }
                else
                {                    
                    Constants.Description.Clear();
                    Constants.Description.Append(tblkdes.Text);
                    Constants.movietitle = tblkshowname.Text;
                    AppSettings.ImageTitle = tblkshowname.Text;
                    imagetype = "OnlineImage";
                    await addshow.GetOnlineImages("Movie", tblkshowname.Text);                    
                    Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);
                    frame.Navigate(typeof(OnlineImages), "Tile");
                    Window.Current.Content = frame;
                    Window.Current.Activate();
                }
            }
            catch(Exception ex)
            {
                string excp = ex.Message;
                string[] array1 = new string[2];
                array1[0] = ex.StackTrace.ToString();
                array1[1] = ex.Source.ToString();
            }
        }

        private void tblkdes_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            txtlimit.Visibility = Visibility.Collapsed;
            txtlength.Text = Convert.ToString(tblkdes.Text.Length) + "/" + Convert.ToString(4000);
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceHelper.AppName == Apps.Story_Time.ToString() || ResourceHelper.AppName == Apps.Vedic_Library.ToString())
            {
                if (!string.IsNullOrEmpty(tblkshowname.Text))
                {
                    valid.Visibility = Visibility.Collapsed;
                    if (tblkdes.Text.Length > 4000)
                    {
                        txtlimit.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        string title = tblkshowname.Text;
                        txtlimit.Visibility = Visibility.Collapsed;
                        InsertData<ShowList> insert = new InsertData<ShowList>();
                        insert.ParameterList = new ShowList();
                        insert.ParameterList.ShowID = (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.Status == "Custom").OrderByDescending(i => i.ShowID).FirstOrDefaultAsync()).Result != null) ? Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.Status == "Custom").OrderByDescending(i => i.ShowID).FirstOrDefaultAsync()).Result.ShowID + 1 : 1;
                        insert.ParameterList.Description = tblkdes.Text;
                        insert.ParameterList.Title = tblkshowname.Text;
                        insert.ParameterList.Status = "Custom";
                        insert.ParameterList.ReleaseDate = System.DateTime.Now.Subtract(TimeSpan.FromDays(1)).ToString("dd MMMM yyyy");
                        insert.ParameterList.CreatedDate = System.DateTime.Now.Date;
                        //insert.ParameterList.Genre = "Telugu";
                        insert.ParameterList.Rating = 5.0;
                        if (!Task.Run(async () => await Storage.FileExists("Images\\scale-100\\" + title + ".jpg")).Result)
                            insert.ParameterList.TileImage = "Default.jpg";
                        else
                            insert.ParameterList.TileImage = tblkshowname.Text + ".jpg";
                        insert.Insert();
                        Constants.Description.Clear();
                        Constants.movietitle = string.Empty;
                        Constants.OnlineImageUrls.Clear();
                        PopupManager.EnableControl();

                        Frame f = Window.Current.Content as Frame;
                        Page p = f.Content as Page;
                        p.Frame.Navigate(typeof(MainPage));
                        //Shows s = (Shows)(this.Tag);
                        //s.Groups.Items.Clear();
                        //s.GroupsForTamilAndUpcomming.Items.Clear();
                        //s.GetTopRatedinBackground();
                    }
                }
                else
                {
                    valid.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (lpiccategory.SelectedIndex != 0 && !string.IsNullOrEmpty(tblkshowname.Text))
                {
                    validrole.Visibility = Visibility.Collapsed;
                    valid.Visibility = Visibility.Collapsed;
                    if (tblkdes.Text.Length > 4000)
                    {
                        txtlimit.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        string title = tblkshowname.Text;
                        txtlimit.Visibility = Visibility.Collapsed;
                        InsertData<ShowList> insert = new InsertData<ShowList>();
                        insert.ParameterList = new ShowList();
                        insert.ParameterList.ShowID = (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.Status == "Custom").OrderByDescending(i => i.ShowID).FirstOrDefaultAsync()).Result != null) ? Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.Status == "Custom").OrderByDescending(i => i.ShowID).FirstOrDefaultAsync()).Result.ShowID + 1 : 1;
                        insert.ParameterList.Description = tblkdes.Text;
                        insert.ParameterList.Title = tblkshowname.Text;
                        insert.ParameterList.Status = "Custom";
                        if (ResourceHelper.AppName == "Kids_TV.Windows" || ResourceHelper.AppName == Apps.Kids_TV_Pro.ToString())
                        {
                            insert.ParameterList.ReleaseDate = lpiccategory.SelectedItem.ToString();
                            insert.ParameterList.SubTitle = "English";
                        }
                        else if (ResourceHelper.AppName == Apps.Story_Time.ToString() || ResourceHelper.AppName == Apps.Vedic_Library.ToString())
                            insert.ParameterList.SubTitle = "English";
                        else
                        {
                            insert.ParameterList.ReleaseDate = System.DateTime.Now.Subtract(TimeSpan.FromDays(1)).ToString("dd MMMM yyyy");
                            insert.ParameterList.SubTitle = "None";
                        }
                        insert.ParameterList.CreatedDate = System.DateTime.Now.Date;
                        //insert.ParameterList.Genre = "Telugu";
                        insert.ParameterList.Rating = 5.0;
                        
                        if (!Task.Run(async () => await Storage.FileExists("Images\\scale-100\\" + title + ".jpg")).Result)
                            insert.ParameterList.TileImage = "Default.jpg";
                        else
                            insert.ParameterList.TileImage = tblkshowname.Text + ".jpg";
                        insert.Insert();
                        InsertData<CategoriesByShowID> insert1 = new InsertData<CategoriesByShowID>();
                        insert1.ParameterList = new CategoriesByShowID();
                        if (ResourceHelper.AppName == Apps.Indian_Cinema_Pro.ToString() || ResourceHelper.AppName == Apps.Recipe_Shows.ToString() || ResourceHelper.AppName == Apps.Online_Education.ToString() || ResourceHelper.AppName == "Indian_Cinema.Windows" || ResourceHelper.AppName == "Indian Cinema.Windows")
                        {
                            insert1.ParameterList.ShowID = insert.ParameterList.ShowID;
                            insert1.ParameterList.CatageoryID = Dic.Where(i => i.Key == lpiccategory.SelectedItem.ToString()).FirstOrDefault().Value;
                            insert1.Insert();
                        }
                        else if (ResourceHelper.AppName == Apps.Bollywood_Movies.ToString() || ResourceHelper.AppName == Apps.Bollywood_Music.ToString())
                        {
                            insert1.ParameterList.ShowID = insert.ParameterList.ShowID;
                            insert1.ParameterList.CatageoryID = 1;
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
                        Constants.movietitle = string.Empty;
                        Constants.OnlineImageUrls.Clear();
                        PopupManager.EnableControl();

                        Frame f = Window.Current.Content as Frame;
                        Page p = f.Content as Page;
                        p.Frame.Navigate(typeof(MainPage));
                        //Shows s = (Shows)(this.Tag);
                        //s.Groups.Items.Clear();
                        //s.GroupsForTamilAndUpcomming.Items.Clear();
                        //s.GetTopRatedinBackground();
                        //s.GetTamilShows();
                    }
                }
                else
                {
                    if (lpiccategory.SelectedIndex == 0 && !string.IsNullOrEmpty(tblkshowname.Text))
                    {
                        validrole.Visibility = Visibility.Visible;
                        valid.Visibility = Visibility.Collapsed;
                    }
                    else if (string.IsNullOrEmpty(tblkshowname.Text) && lpiccategory.SelectedIndex != 0)
                    {
                        valid.Visibility = Visibility.Visible;
                        validrole.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        validrole.Visibility = Visibility.Visible;
                        valid.Visibility = Visibility.Visible;
                    }
                }
            }
            if (ResourceHelper.AppName != AppSettings.AppName)
            {
                //Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                //AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
                //adcontrol.IsAdRotatorEnabled = false;
                //adcontrol.Visibility = Visibility.Collapsed;
                //adcontrol.Height = 90;
                //adcontrol.Width = 728;
            }
        }
    }
}