using Common.Library;
using OnlineMovies.Views;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CategoryDetails : Page
    {
        #region GlobalDeclaration
        int recentCount;
        int teluguCount;
        int tamilCount;
        string CategoryName = string.Empty;
        string CategoryID = string.Empty;

        public static dynamic GetLanguagesHindi = default(dynamic);
        public static dynamic GetLanguagesTelugu = default(dynamic);
        public static dynamic GetLanguagesTamil = default(dynamic);
        public static dynamic GetSubjects = default(dynamic);
        #endregion

        #region Constructor
        public CategoryDetails()
        {
            this.InitializeComponent();
            LoadPivotThemes();
            teluguCount = 10;
            tamilCount = 10;
            recentCount = 10;
        }
        #endregion
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            string[] parameter = (string[])e.Parameter;
            CategoryName = parameter[0];
            CategoryID = parameter[1];
         
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if(Frame.CanGoBack)
            {
                Frame.GoBack();
                e.Handled = true;
            }

        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
           
        }
        private void LoadAds()
        {
            try
            {
                LoadAdds.LoadAdControl_New(LayoutRoot, adstackPsnProfile, 1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In SongDetails file", ex);
                string excepmess = "Exception in LoadAds Method In SongDetails file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }

        } 
        #region "Common Methods"
        private void LoadPivotThemes()
        {
            if (AppSettings.ProjectName == "Indian Cinema.WindowsPhone" || AppSettings.ProjectName == "Indian Cinema Pro")
            {
                pvtcat.Header = "hindi";
                pvttelugu.Header = "telugu";
                pvttamil.Header = "tamil";
                pvttelugu.Visibility = Visibility.Visible;
                pvttamil.Visibility = Visibility.Visible;               
            }
            else
            {
                pvtcat.Header = "";
                pvttelugu.Visibility = Visibility.Collapsed;
                pvttamil.Visibility = Visibility.Collapsed;
                pvtVideos.Items.Remove(pvttamil);              
                pvtVideos.Items.Remove(pvttelugu);
            }
        }
        #endregion

        private void imgTitle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void lbxCatory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxCatory.SelectedIndex != -1)
                {
                    string title = (lbxCatory.SelectedItem as ShowList).Title;
                    AppSettings.ShowID = (lbxCatory.SelectedItem as ShowList).ShowID.ToString();
                    AppState.searchtitle = string.Empty;
                    if (title == "get more")
                    {
                        Constants.UIThread = true;
                        recentCount = recentCount + 10;
                        if (AppSettings.ProjectName == "Indian Cinema.WindowsPhone" || AppSettings.ProjectName == "Indian Cinema Pro")
                        {
                            lbxCatory.ItemsSource = OnlineShow.GetLanguages(recentCount, AppSettings.CategoryID, "Hindi");
                        }
                        else
                            lbxCatory.ItemsSource = OnlineShow.LoadCategoryTopRated(recentCount, AppSettings.CategoryID);
                        lbxCatory.ScrollIntoView(lbxCatory.Items[recentCount - 10]);
                    }
                    else
                    {
                        string[] parameters = new string[2];
                        parameters[0] = AppSettings.ShowID;
                        parameters[1] = null;
                        //Frame.Navigate(typeof(SubjectDetail), parameters);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxCatory_SelectionChanged Method In CategoryDetails file.", ex);
            }
            lbxCatory.SelectedIndex = -1;
        }

        private void lbxTopRated_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxTopRated.SelectedIndex != -1)
                {
                    string title = (lbxTopRated.SelectedItem as ShowList).Title;
                    AppSettings.ShowID = (lbxTopRated.SelectedItem as ShowList).ShowID.ToString();
                    AppState.searchtitle = string.Empty;
                    if (title == "get more")
                    {
                        Constants.UIThread = true;
                        teluguCount = teluguCount + 10;
                        lbxTopRated.ItemsSource = OnlineShow.GetLanguages(teluguCount, AppSettings.CategoryID, "telugu");
                        lbxTopRated.ScrollIntoView(lbxTopRated.Items[teluguCount - 10]);
                    }
                    else
                    {
                        string[] parameters = new string[2];
                        parameters[0] = AppSettings.ShowID;
                        parameters[1] = null;
                        //Frame.Navigate(typeof(Details), parameters);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxTopRated_SelectionChanged Method In CategoryDetails file.", ex);
            }
            lbxTopRated.SelectedIndex = -1;
        }

        private void lbxRecentlyAdded_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxRecentlyAdded.SelectedIndex != -1)
                {
                    string title = (lbxRecentlyAdded.SelectedItem as ShowList).Title;
                    AppSettings.ShowID = (lbxRecentlyAdded.SelectedItem as ShowList).ShowID.ToString();
                    AppState.searchtitle = string.Empty;
                    if (title == "get more")
                    {
                        Constants.UIThread = true;
                        tamilCount = tamilCount + 10;
                        lbxRecentlyAdded.ItemsSource = OnlineShow.GetLanguages(tamilCount, AppSettings.CategoryID, "tamil");
                        lbxRecentlyAdded.ScrollIntoView(lbxRecentlyAdded.Items[tamilCount - 10]);
                    }
                    else
                    {
                        string[] parameters = new string[2];
                        parameters[0] = AppSettings.ShowID;
                        parameters[1] = null;
                        //Frame.Navigate(typeof(Details), parameters);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxRecentlyAdded_SelectionChanged Method In CategoryDetails file.", ex);
            }
            lbxRecentlyAdded.SelectedIndex = -1;
        }

        #region PageLoad
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (AppSettings.ProjectName == "Indian Cinema.WindowsPhone" || AppSettings.ProjectName == "Indian Cinema Pro")
            {
                Constants.UIThread = true;
                GetLanguagesHindi = OnlineShow.GetLanguages(recentCount, CategoryID, "Hindi");
                Constants.UIThread = true;
                GetLanguagesTelugu = OnlineShow.GetLanguages(teluguCount, CategoryID, "telugu");
                Constants.UIThread = true;
                GetLanguagesTamil = OnlineShow.GetLanguages(tamilCount, CategoryID, "tamil");
                Constants.UIThread = false;
            }
            else
            {
                Constants.UIThread = true;
                GetSubjects= OnlineShow.LoadCategoryTopRated(recentCount, CategoryID);
                Constants.UIThread = false;
            }
            try
            {
                LoadAds();
                if (CategoryID != null && CategoryName!=null)
                { 
                    if (CategoryName == "all subjects")
                        AppSettings.AllSubjects = CategoryName;
                    if (CategoryName == "all recipes")
                        AppSettings.AllRecipes = CategoryName;
                    AppSettings.CategoryID = CategoryID;
                    tblkVideosTitle.Text = CategoryName.ToUpper();
                    BackgroundHelper bwHelper = new BackgroundHelper();
                    if (AppSettings.ProjectName == "Indian Cinema.WindowsPhone" || AppSettings.ProjectName == "Indian Cinema Pro")
                    {
                        bwHelper.AddBackgroundTask(
                                                  (object s, DoWorkEventArgs a) =>
                                                  {                                                      
                                                      a.Result = GetLanguagesHindi;
                                                  },
                                                  (object s, RunWorkerCompletedEventArgs a) =>
                                                  {
                                                      lbxCatory.ItemsSource = (List<ShowList>)a.Result;
                                                  }
                                                );
                        bwHelper.AddBackgroundTask(
                                                 (object s, DoWorkEventArgs a) =>
                                                 {                                                   
                                                     a.Result = GetLanguagesTelugu;
                                                 },
                                                 (object s, RunWorkerCompletedEventArgs a) =>
                                                 {
                                                     lbxTopRated.ItemsSource = (List<ShowList>)a.Result;
                                                 }
                                               );
                        bwHelper.AddBackgroundTask(
                                                 (object s, DoWorkEventArgs a) =>
                                                 {                                                   
                                                     a.Result = GetLanguagesTamil;
                                                 },
                                                 (object s, RunWorkerCompletedEventArgs a) =>
                                                 {
                                                     lbxRecentlyAdded.ItemsSource = (List<ShowList>)a.Result;
                                                 }
                                               );
                    }
                    else
                    {
                        bwHelper.AddBackgroundTask(
                                                    (object s, DoWorkEventArgs a) =>
                                                    {                                                       
                                                        a.Result = GetSubjects;                                                        
                                                    },
                                                    (object s, RunWorkerCompletedEventArgs a) =>
                                                    {
                                                        lbxCatory.ItemsSource = (List<ShowList>)a.Result;
                                                    }
                                                  );
                    }
                    bwHelper.RunBackgroundWorkers();
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In CategoryDetails file.", ex);
            }
        }
        #endregion
    }
}
