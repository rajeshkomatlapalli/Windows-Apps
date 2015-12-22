using Common.Library;
using Common.Utilities;
using OnlineMovies.UserControls;
using OnlineVideos;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.UserControls;
using OnlineVideos.ViewModels;
using PicasaMobileInterface;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
    public sealed partial class SubjectDetail : Page
    {
        #region GlobalDeclaration
        public static SubjectDetail current = default(SubjectDetail);
        string navigationvalue = string.Empty;
        string pivotindex = null;
        public PivotItem currentItem = default(PivotItem);
        AddQuizPopup_New addquize = new AddQuizPopup_New();
        bool appbarvisible = true;
        string id = null;        
        #endregion

        #region Constructor
        public SubjectDetail()
        {
            this.InitializeComponent();
            current = this;            
            if (AppSettings.ShowID != "0")
            {
                LoadPivotThemes(AppSettings.ShowUniqueID);
            }
        }
        #endregion
        #region CommonMethods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string[] parametrs = (string[])e.Parameter;
            if (parametrs[0] != null)
            {
                id = parametrs[0];
            }
            if (parametrs[1] != null)
            {
                pivotindex = parametrs[1];
            }

            if (id != null)
            {
                AppSettings.ShowID = id;
            }
            int showid = AppSettings.ShowUniqueID;
            if (Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync().Result.Status != "Custom")
            {
                //this.ApplicationBar.IsVisible = false;
                this.BottomAppBar.Visibility = Visibility.Collapsed;
                appbarvisible = false;
            }
            else
            {
                //this.ApplicationBar.IsVisible = true;
                this.BottomAppBar.Visibility = Visibility.Visible;
                while (Frame.BackStack.Count() > 1)
                {
                    if (!Frame.BackStack.FirstOrDefault().SourcePageType.Equals("SubjectDetail"))
                    {
                        //Frame.BackStack.RemoveAt(-1);
                        Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
                    }
                    else
                    {
                        //Frame.BackStack.RemoveAt(-1);
                        //Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
                        //break;
                    }
                }
            }           
        }
        #endregion

        #region PageLoad

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAds();
            if (pivotindex == "songs")
            {
                if (pivotindex != null)
                    pvtMainDetails.SelectedIndex = 0;
                Constants.navigationvalue++;
            }
            if (pivotindex == "Quiz")
            {
                if (pivotindex != null)
                    pvtMainDetails.SelectedIndex = 1;
                Constants.navigationvalue--;
            }
            AppBarButton addButton = new AppBarButton();
            int showid = AppSettings.ShowUniqueID;
            string sharedstatus = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result.ShareStatus;
            if (sharedstatus == "Shared To Blog")
            {
                addButton.Label = "share this" + " " + AppResources.Type + "(" + "Shared)";
            }
            else
            {
                addButton.Label = "share this" + " " + AppResources.Type + "(" + "Not Shared)";
            }
            PageHelper.RemoveEntryFromBackStack("SubjectDetail");
            
            //if (NavigationContext.QueryString.TryGetValue("QuizPivot", out QuizPivot))
            //{
            //    pvtMainDetails.SelectedIndex = 1;
            //    SettingsHelper.Save("QuizTitle", QuizPivot);
            //}
        }
        private void LoadAds()
        {
            try
            {
                LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In SubjectDetails file", ex);
                string excepmess = "Exception in LoadAds Method In SubjectDetails file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }
        #endregion

        private void imgTitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {            
            Frame.Navigate(typeof(MainPage));
        }

        private void btnshare_Click(object sender, RoutedEventArgs e)
        {
            if (NetworkHelper.IsNetworkAvailableForDownloads())
            {
                UploadToBlog upload = new UploadToBlog(AppSettings.ShowUniqueID, page1: this);
            }
        }        
        private void btnadd_Click(object sender, RoutedEventArgs e)
        {
            if (btnadd.Label.Contains("videos"))
            {
                Constants.DownloadTimer.Stop();
                Constants.Linkstype = "Songs";
                string language = string.Empty;
                //NavigationService.Navigate(new Uri("/Views/LinksFromOnline.xaml?language=" + string.Empty, UriKind.Relative));
                string[] parame = new string[3];
                parame[0] = id;
                parame[1] = string.Empty;
                parame[2] = "Songs";

                Frame.Navigate(typeof(LinksFromOnline), parame);
            }
            else
            {
                if (btnadd.Label.Contains("quiz") || btnadd.Label.Contains("practice tests"))
                {
                    Constants.DownloadTimer.Stop();
                    addquize.showPopup();
                }
                else
                {
                    Constants.DownloadTimer.Stop();
                    Frame.Navigate(typeof(AddQuiz));
                }
            }
        }          
        private void pvtMainDetails_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {            
            AppBarButton addButton = new AppBarButton();
            currentItem = e.AddedItems[0] as PivotItem;
            if (appbarvisible == true)
            {
                if (currentItem != null)
                {                                   
                    btnadd.Label = "Add" + " " + currentItem.Header.ToString();                    
                }               
                if (ShowQuiz.currentshowquiz.lbxSubjectsList.Visibility == Visibility.Visible && currentItem.Header.ToString() == "quiz")
                {                   
                    btnadd.Label = "Add" + " " + currentItem.Header.ToString();
                }
                else if (ShowQuiz.currentshowquiz.lbxSubjectsList.Visibility == Visibility.Collapsed && currentItem.Header.ToString() == "quiz")
                {                   
                    btnadd.Label = "Add" + " " + "questions";
                }
                else if (ShowQuiz.currentshowquiz.lbxSubjectsList.Visibility == Visibility.Visible && currentItem.Header.ToString() == "practice tests")
                {                  
                    btnadd.Label = "Add" + " " + currentItem.Header.ToString();
                }
                else if (ShowQuiz.currentshowquiz.lbxSubjectsList.Visibility == Visibility.Collapsed && currentItem.Header.ToString() == "practice tests")
                {                    
                    btnadd.Label = "Add" + " " + "questions";
                }                
            }
        }

        private async void LoadPivotThemes(long ShowID)
        {
            pvtitmTest.Header = AppResources.ShowQuizPivotTitle;
            pvtMainDetails.Background =await LoadShowPivotBackground(ShowID);
            tblkVideosTitle.Text = OnlineShow.GetShowDetail(AppSettings.ShowUniqueID).Title;
        }
        public async static Task<Brush> LoadShowPivotBackground(long ShowID)
        {
            ShowList show = OnlineShow.GetShowDetail(ShowID);
            return await LoadShowPivotBackground(show.PivotImage);
        }
        public static Color PhoneBackgroundColor
        {
            get { return (Color)Application.Current.Resources["PhoneBackgroundColor"]; }
        }
        public async static Task<ImageBrush> LoadShowPivotBackground(string PivotImage)
        {

            string filePath = string.Empty;
            ImageBrush pivotbackground = new ImageBrush();
            var backgroundColor = PhoneBackgroundColor.ToString();
            try
            {
                if (!string.IsNullOrEmpty(PivotImage))
                {
                    filePath = System.IO.Path.Combine(Constants.PivotImagePath, PivotImage);

                    if (Task.Run(async () => await Storage.FileExists(filePath)).Result)
                    {
                        pivotbackground.ImageSource = await Storage.ReadImageFromLocalStorage(filePath);
                        if (backgroundColor == "#FF000000")
                        {
                            pivotbackground.Opacity = 0.5;
                        }
                    }
                    else if (PivotImage != "")
                    {
                        pivotbackground.ImageSource = new BitmapImage(new Uri(Constants.PivotImagePath + PivotImage, UriKind.RelativeOrAbsolute));
                        //pivotbackground.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/Pivot/COSMOLOGY-AND-ASTRONOMY.jpg", UriKind.RelativeOrAbsolute));

                        if (backgroundColor == "#FF000000")
                        {
                            pivotbackground.Opacity = 0.5;
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
    }
}
