using Common.Library;
using Common.Utilities;
using Indian_Cinema.Views;
using InsertIntoDataBase;
using OnlineMovies.Views;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using PicasaMobileInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.Activation;
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

namespace OnlineVideos.Views
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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddCast_New : Page
    {
        public string selecteditem = string.Empty;
        public int PersonId = 0;
        AddShow addshow = new AddShow();
        public static string imagetype = string.Empty;
        public AutoResetEvent auto = new AutoResetEvent(false);
        public IDictionary<int, string> castdic = new Dictionary<int, string>();
        public List<values> CastList = new List<values>();
        public string ImageUrl = string.Empty;

        public AddCast_New()
        {
            this.InitializeComponent();
            Loaded += AddCast_New_Loaded;
        }

        void AddCast_New_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadAds();
                if (ResourceHelper.AppName == "Indian_Cinema.WindowsPhone" || ResourceHelper.AppName == Apps.Bollywood_Movies.ToString() || ResourceHelper.AppName == Apps.Bollywood_Music.ToString() || ResourceHelper.AppName == Apps.Recipe_Shows.ToString() || ResourceHelper.AppName == Apps.Indian_Cinema_Pro.ToString())
                {
                    List<string> roles = new List<string>();
                    if (ResourceHelper.AppName != Apps.Recipe_Shows.ToString())
                    {
                        roles.Add("-- Choose Role --");
                    }
                    else
                    {
                        roles.Add("-- Choose Quantity --");
                    }
                    List<CastRoles> castroles = addshow.CastRoles();
                    foreach (CastRoles role in castroles)
                    {
                        roles.Add(role.Name);
                    }
                    lpicrole.ItemsSource = roles;
                    lpicrole.SelectedIndex = 0;
                    if (ResourceHelper.AppName == Apps.Recipe_Shows.ToString())
                    {
                        desstk.Visibility = Visibility.Collapsed;
                    }
                }

                else
                {
                    if (ResourceHelper.AppName != "Yoga_&_Health")
                    {
                        desstk.Visibility = Visibility.Collapsed;
                    }
                    rolestk.Visibility = Visibility.Collapsed;
                }
                if (!string.IsNullOrEmpty(Constants.personname))
                    tblkpersonname.Text = Constants.personname;

                CastList = Constants.connection.Table<CastProfile>().ToListAsync().Result.Select(i => new values { PersonID = i.PersonID, Name = i.Name, FlickrPersonImageUrl = i.FlickrPersonImageUrl, Description = i.Description }).ToList();
                listBox.ItemsSource = CastList;

                if (Constants.Description != null)
                {
                    txtlimit.Visibility = Visibility.Collapsed;
                    tblkdes.Text = Constants.Description.ToString();
                    txtlength.Text = Convert.ToString(tblkdes.Text.Length) + "/" + Convert.ToString(4000);

                }

                if (Constants.UserImage != null)
                {
                    int personid = 0;

                    int minpersonid = AppSettings.MinPersonID;
                    if (Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID < minpersonid).OrderByDescending(i => i.PersonID).FirstOrDefaultAsync()).Result != null)
                        personid = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID < minpersonid).OrderByDescending(i => i.PersonID).FirstOrDefaultAsync()).Result.PersonID + 1;
                    else
                        personid = 2;
                    string Personid = Convert.ToString(personid);
                    string title = tblkpersonname.Text;
                    string FolderName = "PersonImages";
                    IRandomAccessStream stream = addshow.GetImageFromStorage("PersonImages", Personid);
                    BitmapImage ProductBitmap = new BitmapImage();
                    ProductBitmap.CreateOptions = BitmapCreateOptions.None;
                    ProductBitmap.SetSource(stream);

                    stream.Dispose();
                    if (imagetype == "LocalImage")
                        personlocalimg.Source = ProductBitmap;
                    if (imagetype == "OnlineImage")
                        persononlineimg.Source = ProductBitmap;
                    Constants.UserImage = null;

                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PageLoad Method In AddCast_New file", ex);
            }
        }        
        private void LoadAds()
        {
            try
            {
                LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 4);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In AddCast_New file", ex);
                string excepmess = "Exception in LoadAds Method In SongDetails file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }
        private void close_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            Constants.UserImage = null;
            Constants.Description.Clear();
            Constants.personname = string.Empty;
            PopupManager.EnableControl();
            
        }

        private void tblkpersonname_GotFocus_1(object sender, RoutedEventArgs e)
        {

        }
        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<CastProfile> castprofiles = addshow.CastProfile();

            if (!string.IsNullOrEmpty(selecteditem))
            {
                InsertData<ShowCast> insert = new InsertData<ShowCast>();
                insert.ParameterList = new ShowCast();
                insert.ParameterList.ShowID = AppSettings.ShowUniqueID;
                insert.ParameterList.PersonID = PersonId;

                if (ResourceHelper.AppName == "Kids_TV" || ResourceHelper.AppName == Apps.Animation_Planet.ToString() || ResourceHelper.AppName == "Yoga_&_Health" || ResourceHelper.AppName == Apps.Kids_TV_Pro.ToString())
                    insert.ParameterList.RoleID = 0;
                else if (ResourceHelper.AppName == Apps.Video_Games.ToString())
                    insert.ParameterList.RoleID = 1;
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
                if (castprofiles.Where(i => i.PersonID < minpersonid).OrderByDescending(i => i.PersonID).FirstOrDefault() != null)
                    personid = castprofiles.Where(i => i.PersonID < minpersonid).OrderByDescending(i => i.PersonID).FirstOrDefault().PersonID + 1;
                else
                    personid = 2;

                insert.ParameterList.PersonID = personid;

                if (ResourceHelper.AppName == "Kids_TV" || ResourceHelper.AppName == Apps.Animation_Planet.ToString() || ResourceHelper.AppName == "Yoga_&_Health" || ResourceHelper.AppName == Apps.Kids_TV_Pro.ToString())
                    insert.ParameterList.RoleID = 0;
                else if (ResourceHelper.AppName == Apps.Video_Games.ToString())
                    insert.ParameterList.RoleID = 1;
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
            Constants.UserImage = null;
            Constants.personname = string.Empty;
            tblkpersonname.Text = string.Empty;
            PopupManager.EnableControl();
            


        }
        void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            List<CastProfile> castprofiles = addshow.CastProfile();
            if (e.Argument == null)
            {
                int personid = 0;
                int minpersonid = AppSettings.MinPersonID;
                if (castprofiles.Where(i => i.PersonID < minpersonid).OrderByDescending(i => i.PersonID).FirstOrDefault() != null)
                    personid = castprofiles.Where(i => i.PersonID < minpersonid).OrderByDescending(i => i.PersonID).FirstOrDefault().PersonID + 1;
                else
                    personid = 2;
                string ImageName = Convert.ToString(personid) + ".jpg";

                if (Task.Run(async () => await Storage.FileExists("Images\\PersonImages\\" + ImageName)).Result)
                {
                    ImageUrl = string.Empty;
                    List<BlogCategoryTable> blogcategorytable = addshow.BlogCategoryTable();
                    BlogCategoryTable bt = blogcategorytable.Where(i => i.BlogType == "people").FirstOrDefault();
                    PicasaInterface pi = new PicasaInterface(bt.BlogUserName, bt.BlogPassword);
                    uploadImage(ImageName);
                }
                else
                {
                    ImageUrl = "http://t1.gstatic.com/images?q=tbn:ANd9GcQyJXo-3YWevA9tkGmycIy5hs1KTgonIT03l-iwKDj09_qLYbivzRgslxlK";
                }
            }
        }
        private void tblkpersonname_LostFocus_1(object sender, RoutedEventArgs e)
        {
            listBox.Visibility = Visibility.Collapsed;
        }

        private void listBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedIndex == -1)
            {
                listBox.Visibility = Visibility.Collapsed;

                return;
            }
            tblkpersonname.Text = ((values)listBox.SelectedItem).Name;
            selecteditem = ((values)listBox.SelectedItem).Name;
            PersonId = ((values)listBox.SelectedItem).PersonID;

            if (tblkpersonname.Text != null)
            {
                tblkdes.Text = ((values)listBox.SelectedItem).Description;
                if (!string.IsNullOrEmpty(((values)listBox.SelectedItem).FlickrPersonImageUrl))
                    persononlineimg.Source = new BitmapImage(new Uri(((values)listBox.SelectedItem).FlickrPersonImageUrl, UriKind.RelativeOrAbsolute));
            }
            listBox.Visibility = Visibility.Collapsed;
            listBox.SelectedIndex = -1;
        }

        private void tblkpersonname_KeyDown_1(object sender, KeyRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tblkpersonname.Text) && e.Key.ToString().ToLower() == "back")
            {
                listBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                string CheckString = string.Empty;
                if (!string.IsNullOrEmpty(tblkpersonname.Text))
                {
                    if (e.Key.ToString().ToLower() != "back")
                        CheckString = tblkpersonname.Text + e.Key.ToString();
                    else
                        CheckString = tblkpersonname.Text;
                }
                else
                {
                    CheckString = e.Key.ToString();
                }
                if (CastList.Where(i => i.Name.ToLower().StartsWith(CheckString.ToLower()) || i.Name.ToLower().Contains(CheckString.ToLower()) || i.Name.ToLower().EndsWith(CheckString.ToLower())).ToList().Count > 0)
                {
                    listBox.Visibility = Visibility.Visible;
                    listBox.ItemsSource = CastList.Where(i => i.Name.ToLower().StartsWith(CheckString.ToLower()) || i.Name.ToLower().Contains(CheckString.ToLower()) || i.Name.ToLower().EndsWith(CheckString.ToLower())).ToList();
                }
            }
        }

        private void persondes_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            Constants.Description.Clear();
            Constants.Description.Append(tblkdes.Text);
            Constants.personname = tblkpersonname.Text;
            string[] array = new string[3];
            array[0] = "Cast";
            array[1] = tblkpersonname.Text;
            array[2] = "AddCast";
            

            Frame.Navigate(typeof(UserBrowserPage), array);
            Window.Current.Content = Frame;
            Window.Current.Activate();           
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

        private void GetPicasaStreamCallback(IAsyncResult asynchronousResult)
        {
            string FolderName = "PersonImages";
            object[] array = new object[2];
            array = (object[])asynchronousResult.AsyncState;
            HttpWebRequest request = (HttpWebRequest)array[1];

            string ImageName = array[0].ToString();

            IRandomAccessStream stream = addshow.GetImageFromStorage(string.Empty, "Images\\" + FolderName + "\\" + ImageName);

            Stream postStream = request.EndGetRequestStream(asynchronousResult);
            byte[] buffer = new byte[stream.AsStream().Length / 4];
            int bytesRead = -1;
            stream.AsStream().Position = 0;
            while ((bytesRead = stream.AsStream().Read(buffer, 0, buffer.Length)) > 0)
            {
                postStream.Write(buffer, 0, bytesRead);
            }
            stream.AsStream().Dispose();
            postStream.Dispose();

            request.BeginGetResponse(new AsyncCallback(RequestPicasaCompleted), request);
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

                Exceptions.SaveOrSendExceptions("Exception in RequestPicasaCompleted event In Add Cast.xaml.cs file", ex);
            }

        }
        private void edit_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            Constants.Description.Clear();
            Constants.Description.Append(tblkdes.Text);
            Constants.personname = tblkpersonname.Text;
            

            Frame.Navigate(typeof(Description), "People");
            Window.Current.Content = Frame;
            Window.Current.Activate();

        }

        private void tblkdes_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            txtlimit.Visibility = Visibility.Collapsed;
            txtlength.Text = Convert.ToString(tblkdes.Text.Length) + "/" + Convert.ToString(4000);
        }

        private async void personlocalimg_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            Constants.Description.Clear();
            Constants.Description.Append(tblkdes.Text);
            Constants.personname = tblkpersonname.Text;
            List<CastProfile> castprofiles = addshow.CastProfile();
            int personid = 0;
            int minpersonid = AppSettings.MinPersonID;
            if (castprofiles.Where(i => i.PersonID < minpersonid).OrderByDescending(i => i.PersonID).FirstOrDefault() != null)
                personid = castprofiles.Where(i => i.PersonID < minpersonid).OrderByDescending(i => i.PersonID).FirstOrDefault().PersonID + 1;
            else
                personid = 2;
            AppSettings.ImageTitle = Convert.ToString(personid);
            imagetype = "LocalImage";
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".bmp");
            openPicker.FileTypeFilter.Add(".png");

            openPicker.PickSingleFileAndContinue();
           
        }

        public async void ContinueFileOpenPicker(FileOpenPickerContinuationEventArgs args)
        {
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
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        private async void persononlineimg_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            Constants.Description.Clear();
            Constants.Description.Append(tblkdes.Text);
            Constants.personname = tblkpersonname.Text;
            List<CastProfile> castprofiles = addshow.CastProfile();
            int personid = 0;
            int minpersonid = AppSettings.MinPersonID;
            if (castprofiles.Where(i => i.PersonID < minpersonid).OrderByDescending(i => i.PersonID).FirstOrDefault() != null)
                personid = castprofiles.Where(i => i.PersonID < minpersonid).OrderByDescending(i => i.PersonID).FirstOrDefault().PersonID + 1;
            else
                personid = 2;
            AppSettings.ImageTitle = Convert.ToString(personid);
            imagetype = "OnlineImage";
            await addshow.GetOnlineImages(string.Empty, tblkpersonname.Text);

            Frame.Navigate(typeof(OnlineImages_New), "Person");
            Window.Current.Content = Frame;
            Window.Current.Activate();
        }

        private void save_Click_1(object sender, RoutedEventArgs e)
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
                    bg.RunWorkerAsync(listBox.SelectedItem);
                    Frame.GoBack();
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
       
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void imgtitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {

        }
    }
}
