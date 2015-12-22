using Common.Library;
using Common.Utilities;
using Indian_Cinema.Views;
using InsertIntoDataBase;
using OnlineVideos;
using OnlineVideos.Data;
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
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public class values
    {
        public int PersonID
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string FlickrPersonImageUrl
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
    }
    
    public sealed partial class AddCast : Page
    {
        AddShow addshow = new AddShow();        
        public static string imagetype = string.Empty;
        public bool TaskNavigation = false;
        public AutoResetEvent auto = new AutoResetEvent(false);
        public static bool navigated = false;
        public IDictionary<int, string> castdic = new Dictionary<int, string>();
        public List<values> CastList = new List<values>();
        public string ImageUrl = string.Empty;
        public AddCast()
        {
            this.InitializeComponent();
            Loaded += AddCast_Loaded;           
        }

        void AddCast_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAds();
        }

        private void LoadAds()
        {
            try
            {
                LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 4);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In SongDetails file", ex);
                string excepmess = "Exception in LoadAds Method In SongDetails file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }
        

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {           
            if (e.NavigationMode != NavigationMode.Back)
            {
                Constants.Description.Clear();
                Constants.Description.Append(tblkdes.Text);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {              
                if(ResourceHelper.AppName==Apps.Indian_Cinema.ToString() || ResourceHelper.AppName == Apps.Indian_Cinema_Pro.ToString() || ResourceHelper.AppName == Apps.Bollywood_Movies.ToString() || ResourceHelper.AppName == Apps.Bollywood_Music.ToString() || ResourceHelper.AppName == Apps.Recipe_Shows.ToString())
                {
                    if (e.NavigationMode != NavigationMode.Back)
                    {
                        List<string> roles = new List<string>();
                        roles.Add("-- Choose Role --");
                        List<CastRoles> castroles = addshow.CastRoles();
                        foreach (CastRoles role in castroles)
                        {
                            roles.Add(role.Name);
                        }
                        lpicrole.ItemsSource = roles;
                        lpicrole.SelectedIndex = 0;
                    }
                }
                else
                {
                    desstk.Visibility = Visibility.Collapsed;
                    rolestk.Visibility = Visibility.Collapsed;
                }

                if (e.NavigationMode != NavigationMode.Back)
                {
                    CastList = Constants.connection.Table<CastProfile>().ToListAsync().Result.Select(i => new values { PersonID = i.PersonID, Name = i.Name, FlickrPersonImageUrl = i.FlickrPersonImageUrl, Description = i.Description }).ToList();
                    tblkpersonname.ItemsSource = CastList;
                }
                if (TaskNavigation == false)
                {
                    if (Constants.Description.Length > 0)
                    {
                        txtlimit.Visibility = Visibility.Collapsed;
                        tblkdes.Text = Constants.Description.ToString();
                        txtlength.Text = Convert.ToString(tblkdes.Text.Length) + "/" + Convert.ToString(4000);
                    }
                    
                    if (Constants.UserImage1 != null && navigated == false)
                    {
                        navigated = true;                     
                        Frame.Navigate(typeof(PhotoChooser), tblkpersonname.Text);
                    }
                    else
                    {
                        if (Constants.UserImage1 != null && navigated == true)
                        {
                            navigated = false;
                            Constants.UIThread = true;
                            int personid = 0;
                            int minpersonid = AppSettings.MinPersonID;
                            if (Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID < minpersonid).OrderByDescending(i => i.PersonID).FirstOrDefaultAsync()).Result != null)
                                personid = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID < minpersonid).OrderByDescending(i => i.PersonID).FirstOrDefaultAsync()).Result.PersonID + 1;
                            else
                                personid = 2;
                            string Personid = Convert.ToString(personid);
                            if (imagetype == "LocalImage")
                            {                                                          
                                personlocalimg.Source = ResourceHelper.getpersonTileImage1(Personid + ".jpg");
                                persononlineimg.Source = new BitmapImage(new Uri("ms-appx:///Images/peoplefromonline.png", UriKind.RelativeOrAbsolute));
                            }
                            if (imagetype == "OnlineImage")
                            {
                                persononlineimg.Source = ResourceHelper.getpersonTileImage1(Personid + ".jpg");
                                personlocalimg.Source = new BitmapImage(new Uri("ms-appx:///Images/peoplefromlocal.png", UriKind.RelativeOrAbsolute));
                            }
                            Constants.UIThread = false;
                            Constants.UserImage1 = null;
                        }
                    }
                }
                else
                {
                    TaskNavigation = false;
                }
            }
            catch(Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In AddCast.cs file.", ex);
            }
        }

        public void uploadImage()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://picasaweb.google.com/data/feed/api/user/" + CONST.USER);
                request.Method = "POST";
                request.ContentType = "image/png";
                request.Headers["Authorization"] = CONST.PIC_AUTH + CONST.AUTH_Token;
                request.BeginGetRequestStream(new AsyncCallback(GetPicasaStreamCallback), request);
                auto.WaitOne();
            }
            catch(Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in uploadImage Method In AddCast.cs file.", ex);
            }
        }

        private async void GetPicasaStreamCallback(IAsyncResult ar)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)ar.AsyncState;                
                int personid = 0;
                int minpersonid = AppSettings.MinPersonID;
                if (Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID < minpersonid).OrderByDescending(i => i.PersonID).FirstOrDefaultAsync()).Result != null)
                    personid = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID < minpersonid).OrderByDescending(i => i.PersonID).FirstOrDefaultAsync()).Result.PersonID + 1;
                else
                    personid = 2;
                string ImageName = Convert.ToString(personid) + ".jpg";
                
                IStorageFolder isoStore = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await isoStore.CreateFileAsync("Images" + "/" + "PersonImages" + "/" + ImageName, CreationCollisionOption.OpenIfExists)).Result;
                IRandomAccessStream fileStream = (IRandomAccessStream)file1.OpenAsync(FileAccessMode.Read);                
                Stream postStream = request.EndGetRequestStream(ar);               
                byte[] buffer = new byte[fileStream.Size / 4];
                IBuffer v;
                int bytesRead = -1;                
                fileStream.GetOutputStreamAt(0);                
                while ((bytesRead = await postStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    postStream.Write(buffer, 0, bytesRead);
                }
                fileStream.CloneStream();                                
                request.BeginGetResponse(new AsyncCallback(RequestPicasaCompleted), request);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPicasaStreamCallback Method In AddCast.cs file.", ex);
            }
        }

        private void RequestPicasaCompleted(IAsyncResult ar)
        {
            try
            {
                var request = (HttpWebRequest)ar.AsyncState;
                var response = (HttpWebResponse)request.EndGetResponse(ar);
                StreamReader responseReader = new StreamReader(response.GetResponseStream());
                XElement MyXMLConfig = XElement.Load(responseReader);
                XNamespace atomNS = "http://www.w3.org/2005/Atom";
                ImageUrl = MyXMLConfig.Descendants(atomNS + "content").Attributes().Where(i => i.Name == "src").FirstOrDefault().Value;
                auto.Set();
            }
            catch (Exception ex)
            {
                auto.Set();
                ImageUrl = "http://t1.gstatic.com/images?q=tbn:ANd9GcQyJXo-3YWevA9tkGmycIy5hs1KTgonIT03l-iwKDj09_qLYbivzRgslxlK";
                Exceptions.SaveOrSendExceptions("Exception in RequestPicasaCompleted Method In AddCast.cs file.", ex);
            }
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void tblkpersonname_KeyUp(object sender, KeyRoutedEventArgs e)
        {
           
        }

        private void persondes_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(UserBrowserPage), "querytext=Cast&searchquery=" + tblkpersonname.Text);            
        }

        private void edit_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Constants.Description.Clear();
            Constants.Description.Append(tblkdes.Text);           
            Frame.Navigate(typeof(Description));
        }

        private async void persononlineimg_Tapped(object sender, TappedRoutedEventArgs e)
        {
            imagetype = "OnlineImage";
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync(new Uri("http://www.bing.com/images/search?q=" + tblkpersonname.Text));
        }

        private void tblkdes_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lpicrole.SelectedIndex != 0 && !string.IsNullOrEmpty(tblkpersonname.Text))
                {
                    validrole.Visibility = Visibility.Collapsed;
                    valid.Visibility = Visibility.Collapsed;
                    if (tblkdes.Text.Length > 4000)
                    {
                        txtlimit.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        txtlimit.Visibility = Visibility.Collapsed;
                        personlocalimg.Source = null;
                        persononlineimg.Source = null;

                        BackgroundWorker bg = new BackgroundWorker();
                        bg.DoWork += bg_DoWork;
                        bg.RunWorkerCompleted += bg_RunWorkerCompleted;                        
                        bg.RunWorkerAsync(selectedItem);
                    }
                }
                else
                {
                    if (lpicrole.SelectedIndex == 0 && !string.IsNullOrEmpty(tblkpersonname.Text))
                    {
                        validrole.Visibility = Visibility.Visible;
                        valid.Visibility = Visibility.Collapsed;
                    }
                    else if (string.IsNullOrEmpty(tblkpersonname.Text) && lpicrole.SelectedIndex != 0)
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
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in btnsave_Click_1 Method In AddCast.cs file.", ex);
            }
        }

        private void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {             
                if (((values)selectedItem) != null)                
                {
                    InsertData<ShowCast> insert = new InsertData<ShowCast>();
                    insert.ParameterList = new ShowCast();
                    insert.ParameterList.ShowID = AppSettings.ShowUniqueID;                    
                    insert.ParameterList.PersonID = ((values)selectedItem).PersonID;
                    if (ResourceHelper.AppName == Apps.Kids_TV_Shows.ToString() || ResourceHelper.AppName == Apps.Kids_TV_Pro.ToString() || ResourceHelper.AppName == Apps.Animation_Planet.ToString() || ResourceHelper.AppName == Apps.Kids_TV.ToString() || ResourceHelper.AppName == Apps.Yoga_and_Health.ToString().Replace("and", "&"))
                        insert.ParameterList.RoleID = 0;
                    else
                    {
                        string name = lpicrole.SelectedItem.ToString();
                        List<CastRoles> rolesforcast = addshow.CastRoles();
                        insert.ParameterList.RoleID = rolesforcast.Where(i => i.Name == name).FirstOrDefault().RoleID;
                    }
                    insert.Insert();
                }
                else
                {
                    InsertData<ShowCast> insert = new InsertData<ShowCast>();
                    insert.ParameterList = new ShowCast();
                    insert.ParameterList.ShowID = AppSettings.ShowUniqueID;
                    int personid = 0;
                    int minpersonid = AppSettings.MinPersonID;
                    if (Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID < minpersonid).OrderByDescending(i => i.PersonID).FirstOrDefaultAsync()).Result != null)
                        personid = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID < minpersonid).OrderByDescending(i => i.PersonID).FirstOrDefaultAsync()).Result.PersonID + 1;
                    else
                        personid = 2;

                    insert.ParameterList.PersonID = personid;
                    if (ResourceHelper.AppName == Apps.Kids_TV_Shows.ToString() || ResourceHelper.AppName == Apps.Kids_TV_Pro.ToString() || ResourceHelper.AppName == Apps.Animation_Planet.ToString() || ResourceHelper.AppName == Apps.Kids_TV.ToString() || ResourceHelper.AppName == Apps.Yoga_and_Health.ToString().Replace("and", "&"))
                        insert.ParameterList.RoleID = 0;
                    else
                    {
                        string name = lpicrole.SelectedItem.ToString();
                        List<CastRoles> rolesforcast = addshow.CastRoles();
                        insert.ParameterList.RoleID = rolesforcast.Where(i => i.Name == name).FirstOrDefault().RoleID;
                    }
                    insert.Insert();
                    InsertData<CastProfile> insert1 = new InsertData<CastProfile>();
                    insert1.ParameterList = new CastProfile();
                    insert1.ParameterList.Name = tblkpersonname.Text;
                    insert1.ParameterList.Description = tblkdes.Text;
                    insert1.ParameterList.PersonID = insert.ParameterList.PersonID;

                    insert1.ParameterList.FlickrPersonImageUrl = ImageUrl;
                    insert1.ParameterList.FlickrPanoramaImageUrl = "";
                    insert1.Insert();
                }
                Constants.Description.Clear();
                tblkdes.Text = string.Empty;
                Constants.UserImage1 = null;
                Constants.DownloadTimer.Start();
                if (ResourceHelper.AppName == Apps.Kids_TV_Shows.ToString() || ResourceHelper.AppName == Apps.Kids_TV_Pro.ToString() || ResourceHelper.AppName == Apps.Recipe_Shows.ToString() || ResourceHelper.AppName == Apps.Animation_Planet.ToString() || ResourceHelper.AppName == Apps.Kids_TV.ToString() || ResourceHelper.AppName == Apps.Yoga_and_Health.ToString().Replace("and", "&"))
                    
                    Frame.Navigate(typeof(ShowDetails), "navigationvalue=" + Constants.navigationvalue + "&pivotindex=" + 1);
                else if (ResourceHelper.AppName == Apps.Bollywood_Music.ToString())
                    
                    Frame.Navigate(typeof(MusicDetail), "navigationvalue=" + Constants.navigationvalue + "&pivotindex=" + 3);
                else
                    
                    Frame.Navigate(typeof(Details), "navigationvalue=" + Constants.navigationvalue + "&pivotindex=" + 1);
                
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in bg_RunWorkerCompleted Method In AddCast.cs file.", ex);
            }
        }

        private async void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (e.Argument == null)
                {
                    //OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
                    int personid = 0;
                    int minpersonid = AppSettings.MinPersonID;
                    if (Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID < minpersonid).OrderByDescending(i => i.PersonID).FirstOrDefaultAsync()).Result != null)
                        personid = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID < minpersonid).OrderByDescending(i => i.PersonID).FirstOrDefaultAsync()).Result.PersonID + 1;
                    else
                        personid = 2;
                    string ImageName = Convert.ToString(personid) + ".jpg";
                    //IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                    StorageFolder isoStore = ApplicationData.Current.LocalFolder;
  
                      if(await isoStore.GetFileAsync(Constants.PersonImagePath + ImageName)!=null)
                      {
                          ImageUrl = string.Empty;
                          List<BlogCategoryTable> blogcategorytable = addshow.BlogCategoryTable();
                          BlogCategoryTable bt = blogcategorytable.Where(i => i.BlogType == "people").FirstOrDefault();
                          if (NetworkHelper.IsNetworkAvailableForDownloads())
                          {
                              PicasaInterface pi = new PicasaInterface(bt.BlogUserName, bt.BlogPassword);
                              uploadImage();
                          }
                          else
                              ImageUrl = "http://t1.gstatic.com/images?q=tbn:ANd9GcQyJXo-3YWevA9tkGmycIy5hs1KTgonIT03l-iwKDj09_qLYbivzRgslxlK";
                      }
            
                    //if (isoStore.FileExists(Constants.PersonImagePath + ImageName))
                    //{
                    //    ImageUrl = string.Empty;
                    //    List<BlogCategoryTable> blogcategorytable = addshow.BlogCategoryTable();
                    //    BlogCategoryTable bt = blogcategorytable.Where(i => i.BlogType == "people").FirstOrDefault();
                    //    if (NetworkHelper.IsNetworkAvailableForDownloads())
                    //    {
                    //        PicasaInterface pi = new PicasaInterface(bt.BlogUserName, bt.BlogPassword);
                    //        uploadImage();
                    //    }
                    //    else
                    //        ImageUrl = "http://t1.gstatic.com/images?q=tbn:ANd9GcQyJXo-3YWevA9tkGmycIy5hs1KTgonIT03l-iwKDj09_qLYbivzRgslxlK";
                    //}
                    else
                    {
                        ImageUrl = "http://t1.gstatic.com/images?q=tbn:ANd9GcQyJXo-3YWevA9tkGmycIy5hs1KTgonIT03l-iwKDj09_qLYbivzRgslxlK";
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in bg_DoWork Method In AddCast.cs file.", ex);
            }
        }

        values selectedItem = null;
        private void tblkpersonname_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {            
            selectedItem = args.SelectedItem as values;
            try
            {
                //if (tblkpersonname.SelectedItem != null)
                if (args.SelectedItem != null)
                {
                    //tblkdes.Text = ((values)tblkpersonname.SelectedItem).Description;
                    tblkdes.Text = ((values)args.SelectedItem).Description;
                    persononlineimg.Source = new BitmapImage(new Uri(((values)args.SelectedItem).FlickrPersonImageUrl, UriKind.RelativeOrAbsolute));
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in tblkpersonname_SelectionChanged_1 Method In AddCast.cs file.", ex);
            }
        }

        private void imgtitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void personlocalimg_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            CoreApplicationView view = CoreApplication.GetCurrentView();
            try
            {
                Constants.Description.Clear();
                Constants.Description.Append(tblkdes.Text);
                //Constants.movietitle = tblkshowname.Text;
               // AppSettings.ImageTitle = tblkshowname.Text;
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

       async void view_Activated(CoreApplicationView sender, Windows.ApplicationModel.Activation.IActivatedEventArgs args1)
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
                        array[0] = "Person";
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
        //public AddCast Hero { get; set; }
        //public AddCast Director { get; set; }
    }
}
