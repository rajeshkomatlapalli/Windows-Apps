using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using OnlineVideos.Data;
using Common.Library;
using OnlineVideos.Entities;
using OnlineVideos.View_Models;
using Common.Utilities;
using OnlineVideos.UI;
using PicasaMobileInterface;
using OnlineVideos.Views;
using OnlineVideos.UserControls;
using OnlineVideos;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineMovies.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StoryDetails : Page
    {
        #region GlobalDeclaration
        string navigationvalue = null;
        string pivotindex = string.Empty;
        PivotItem currentItem = default(PivotItem);
        bool appbarvisible = true;
        bool IsBackgroundLoaded = false;
        #endregion

        #region Constructor
        public StoryDetails()
        {
            this.InitializeComponent();
            Loaded += new RoutedEventHandler(StoryDetails_Loaded);
            LoadStoryIntro();
        }
        #endregion

        #region GeneralMethods
        public void LoadStoryIntro()
        {
            try
            {
                string storyIntro = StoryManager.GetStoryIntro(AppSettings.ShowUniqueID.ToString(), 1);
                ShowList showDetail = OnlineShow.GetShowDetail(AppSettings.ShowUniqueID);
                if (ResourceHelper.AppName == Apps.Vedic_Library.ToString() && showDetail.SubTitle == Constants.MovieCategoryTelugu)
                {
                    tbstoryintro.FontFamily = new FontFamily("/Pothana2000.ttf#Pothana2000");
                    tbstoryintro.FontSize = 27;
                }
                if (showDetail.SubTitle == Constants.MovieCategoryEnglish)
                {
                    tbstoryintro.FontFamily = new FontFamily("/COM4NRG_.TTF#COM4t Nuvu Regular");
                    tbstoryintro.FontSize = 27;
                }
                if (ResourceHelper.AppName == Apps.Vedic_Library.ToString() && showDetail.SubTitle == Constants.MovieCategoryHindi)
                {
                    tbstoryintro.FontFamily = new FontFamily("/CDACOTYGB.TTF#CDAC-GISTYogesh");
                    tbstoryintro.FontSize = 27;
                }
                if (string.IsNullOrEmpty(storyIntro))
                {
                    txterror.Text = "story not available";
                    btnreadmore.Visibility = Visibility.Collapsed;
                    Record.Visibility = Visibility.Collapsed;
                    txterror.Visibility = Visibility.Visible;
                    btnreadmore.Visibility = Visibility.Collapsed;
                }
                else
                {
                    txterror.Visibility = Visibility.Collapsed;
                    btnreadmore.Visibility = Visibility.Visible;
                    Record.Visibility = Visibility.Visible;

                    if (!DirectoryExists("/StoryRecordings/" + AppSettings.ShowID + "/StoryRecordings.xml"))
                    {
                        Record.Content = "Record Narration";
                    }
                    else
                    {
                        Record.Content = "Resume Narration";
                    }
                    tbstoryintro.Text = storyIntro;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadStoryIntro event In StoryDetail.cs file.", ex);
            }
        }

        private async void LoadPivotThemes(long ShowID)
        {
            MainGrid.Background =await ShowDetail.LoadShowPivotBackground(ShowID);
        }
        #endregion
        private void LoadAds()
        {
            try
            {
                LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In StoryDetails file", ex);
                string excepmess = "Exception in LoadAds Method In StoryDetails file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }
        #region PageLoad
        async void StoryDetails_Loaded(object sender, RoutedEventArgs e)
        {
            AppBarButton Sharebtn=new AppBarButton();
            try
            {
                LoadAds();
                int showid = AppSettings.ShowUniqueID;
                string sharedstatus = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result.ShareStatus;
                if (sharedstatus == "Shared To Blog")
                    //((ApplicationBarMenuItem)ApplicationBar.MenuItems[0]).Text = "share this" + " " + AppResources.Type + "(" + "Shared)";
                    btnshare.Label = "share this" + " " + AppResources.Type + "(" + "Shared)";
                else
                    //((ApplicationBarMenuItem)ApplicationBar.MenuItems[0]).Text = "share this" + " " + AppResources.Type + "(" + "Not Shared)";
                    btnshare.Label = "share this" + " " + AppResources.Type + "(" + "Not Shared)";
                if (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result.Status == "Custom")
                {
                    btnreadmore.Height = 50;
                    btnlisten.Height = 50;
                    Record.Height = 50;
                    Record.Margin = new Thickness(0, 230, 0, 15);
                    btnlisten.Margin = new Thickness(0, 320, 0, 0);
                }
                else
                {
                    btnreadmore.Height = 50;
                    btnlisten.Height = 50;
                    Record.Height = 50;
                    Record.Margin = new Thickness(0, 230, 0, 15);
                    btnlisten.Margin = new Thickness(0, 320, 0, 0);
                }
                btnreadmore.Content = "Read Story";
                btnlisten.Content = "Listen";
                if (DirectoryExists("/StoryRecordings/" + AppSettings.ShowID))
                {
                    this.btnlisten.Visibility = Visibility.Visible;
                }
                else
                {
                    this.btnlisten.Visibility = Visibility.Collapsed;
                }
                string id = "";
                //if (NavigationContext.QueryString.TryGetValue("id", out id))
                //    AppSettings.ShowID = id;
                if (IsBackgroundLoaded == false)
                {
                    LoadPivotThemes(AppSettings.ShowUniqueID);
                    tblkVideosTitle.Text = OnlineShow.GetShowDetail(AppSettings.ShowUniqueID).Title;
                }
                PageHelper.RemoveEntryFromBackStack("StoryDetails");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in StoryDetails_Loaded event In StoryDetail.cs file.", ex);
            }
        }
        #endregion

        public bool DirectoryExists(string path)
        {
            bool exist = false;
            try
            {
                StorageFolder store = ApplicationData.Current.LocalFolder;
                var story = Task.Run(async () => await store.GetFolderAsync("StoryRecordings")).Result;
                var story1 = Task.Run(async () => await story.GetFolderAsync(AppSettings.ShowID)).Result;
                var file = Task.Run(async () => await story1.GetFileAsync("StoryRecordings.xml")).Result;
                exist = true;
            }
            catch (Exception ex)
            {
                
            }
            return exist;
        }
        #region DefaultMethods       
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                string[] parame = (string[])(e.Parameter);
                int showid = Convert.ToInt32(parame[0]);
                navigationvalue = parame[1];
                if (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result.Status != "Custom")
                {                    
                    //this.ApplicationBar.IsVisible = false;
                    this.BottomAppBar.Visibility = Visibility.Collapsed;
                    appbarvisible = false;
                }
                else
                {
                    while (Frame.BackStack.Count() > 1)
                    {
                        if (!Frame.BackStack.FirstOrDefault().SourcePageType.Equals("StoryDetails"))
                        {
                            Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
                        }
                        else
                        {
                            //Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
                        }
                    }
                    //this.ApplicationBar.IsVisible = true;
                    this.BottomAppBar.Visibility = Visibility.Visible;
                }
                
                if (navigationvalue == "videos")
                {
                    pivotindex = "1";
                    if (pivotindex != null)
                        pvtMainDetails.SelectedIndex = Convert.ToInt32(pivotindex);
                    Constants.navigationvalue++;
                }
                else
                {
                    pivotindex = "0";
                    if (pivotindex != null)
                        pvtMainDetails.SelectedIndex = Convert.ToInt32(pivotindex);
                    Constants.navigationvalue--;
                }                                
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo event In StoryDetail.cs file.", ex);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom event In StoryDetail.cs file.", ex);
            }
        } 
        #endregion

        #region Events
        private void pvtMainDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CommandBar bottomCommandBar = this.BottomAppBar as CommandBar;
            AppBarButton addbtn = new AppBarButton();
            try
            {
                currentItem = e.AddedItems[0] as PivotItem;
                if (appbarvisible == true)
                {
                    if (currentItem != null)
                    {
                        if (currentItem.Header.ToString() == "videos")
                        {
                            if (bottomCommandBar.SecondaryCommands.Count == 3)
                                bottomCommandBar.SecondaryCommands.RemoveAt(2);
                            btnadd.Label= "Add" + " " + currentItem.Header.ToString();                           
                        }
                        else
                        {
                            if(bottomCommandBar.SecondaryCommands.Count<3)
                            {
                                btnadd.Label = "Edit" + " " + "story";                                                               
                                bottomCommandBar.SecondaryCommands.Add(addbtn);
                                addbtn.Click += btnadd_Click;
                                addbtn.Label = "Add" + " " + "story";
                            }                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in pvtMainDetails_SelectionChanged event In StoryDetail.cs file.", ex);
            }
        }

        private void btnreadmore_Click(object sender, RoutedEventArgs e)
        {
            Constants.mode = "Read";        
            Frame.Navigate(typeof(StoryReading));
        }

        private void btnlisten_Click(object sender, RoutedEventArgs e)
        {
            Constants.mode = "Listen";           
            Frame.Navigate(typeof(StoryReading));
        }

        private void btnrecard_Click(object sender, RoutedEventArgs e)
        {
            if (Storage.DirectoryExists("StoryRecordings")==null)
                Storage.CreateDirectory("StoryRecordings");
            Constants.mode = "Rec";
            Frame.Navigate(typeof(StoryReading));          
        }

        private void btnshare_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (NetworkHelper.IsNetworkAvailableForDownloads())
                {
                    UploadToBlog upload = new UploadToBlog(AppSettings.ShowUniqueID, page1: this);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in btnshare_Click event In StoryDetail.cs file.", ex);
            }
        }

        private void btnadd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((sender as AppBarButton).Label.Contains("videos"))
                {
                    Constants.DownloadTimer.Stop();
                    Constants.Linkstype = "Songs";
                    string[] para = new string[3];
                    para[0] = AppSettings.ShowID.ToString();
                    para[1] = string.Empty;
                    para[2] = string.Empty;                    

                    Frame.Navigate(typeof(LinksFromOnline), para);
                }

                else if ((sender as AppBarButton).Label.Contains("Add"))
                {
                    if (ResourceHelper.AppName == Apps.Story_Time.ToString())
                    {                     
                        string PageName = "StoryDetails.xaml";
                        Frame.Navigate(typeof(Story), string.Empty);
                    }
                    else if (ResourceHelper.AppName == Apps.Vedic_Library.ToString())
                    {
                        string PageName = "VedicDetails.xaml";
                        Frame.Navigate(typeof(Story), string.Empty);
                    }
                }
                if ((sender as AppBarButton).Label.Contains("Edit"))
                {
                    Frame.Navigate(typeof(EditStory));
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in btnadd_Click event In StoryDetail.cs file.", ex);
            }
        }

        private void btnedit_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/Views/EditStory.xaml", UriKind.Relative));
            //Frame.Navigate(typeof(EditStory));
        }
        #endregion
    }
}
