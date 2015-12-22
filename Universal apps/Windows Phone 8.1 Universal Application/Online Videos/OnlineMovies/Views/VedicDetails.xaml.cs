using Common.Library;
using Common.Utilities;
using OnlineVideos;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.UI;
using OnlineVideos.View_Models;
using OnlineVideos.Views;
using PicasaMobileInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineMovies.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VedicDetails : Page
    {
        string navigationvalue = string.Empty;
        string pivotindex = string.Empty;
        PivotItem currentItem = default(PivotItem);
        bool appbarvisible = true;
        bool IsBackgroundLoaded = false;
        public VedicDetails()
        {
            this.InitializeComponent();
            Loaded += new RoutedEventHandler(VedicDetails_Loaded);
            LoadStoryIntro();
        }
        public void LoadStoryIntro()
        {
            try
            {
                string storyIntro = StoryManager.GetStoryIntro(AppSettings.ShowUniqueID.ToString(), 1);
                ShowList showDetail = OnlineShow.GetShowDetail(AppSettings.ShowUniqueID);
                if (ResourceHelper.AppName == Apps.Vedic_Library.ToString() && showDetail.SubTitle == Constants.MovieCategoryTelugu)
                {
                    tbstoryintro.FontFamily = new FontFamily("/Pothana2000.ttf#Pothana2000");
                    tbstoryintro.FontSize = 20;
                }
                if (showDetail.SubTitle == Constants.MovieCategoryEnglish)
                {
                    tbstoryintro.FontSize = 20;
                }
                if (ResourceHelper.AppName == Apps.Vedic_Library.ToString() && showDetail.SubTitle == Constants.MovieCategoryHindi)
                {
                    tbstoryintro.FontFamily = new FontFamily("/CDACOTYGB.TTF#CDAC-GISTYogesh");
                    tbstoryintro.FontSize = 20;
                }

                if (string.IsNullOrEmpty(storyIntro))
                {
                    txterror.Text = "vedic text not available";
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
                Exceptions.SaveOrSendExceptions("Exception in LoadStoryIntro Method In VedicDetails.cs file.", ex);
            }
        }
        private void LoadAds()
        {
            try
            {
                LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In VedicDetails file", ex);
                string excepmess = "Exception in LoadAds Method In VedicDetails file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }
        private async void LoadPivotThemes(long ShowID)
        {
            try
            {
                MainGrid.Background =await ShowDetail.LoadShowPivotBackground(ShowID);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadPivotThemes Method In VedicDetails.cs file.", ex);
            }
        }

       async void VedicDetails_Loaded(object sender, RoutedEventArgs e)
        {
            //if (Constants.backstack == "Details")
            //{
            //    pvtMainDetails.SelectedIndex = 1;
            //    Constants.backstack = "";
            //}
            LoadAds();
            AppBarButton addButton = new AppBarButton();
            try
            {
                //FlurryWP8SDK.Api.LogEvent("VedicDetails Page", true);
                int showid = AppSettings.ShowUniqueID;
                string sharedstatus = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result.ShareStatus;
                if (sharedstatus == "Shared To Blog")               
                    addButton.Label = "share this" + " " + AppResources.Type + "(" + "Shared)";
                else                   
                    addButton.Label = "share this" + " " + AppResources.Type + "(" + "Not Shared)";
                if (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result.Status == "Custom")
                {
                    btnlistenmp3.Height = 55;
                    btnreadmore.Height = 55;
                    btnlisten.Height = 55;
                    Record.Height = 55;
                    Record.Margin = new Thickness(0, 320, 0, 15);
                    btnlisten.Margin = new Thickness(0, 400, 0, 0);
                }
                else
                {
                    btnlistenmp3.Height = 60;
                    btnreadmore.Height = 60;
                    btnlisten.Height = 60;
                    Record.Height = 60;
                    Record.Margin = new Thickness(0, 320, 0, 15);
                    btnlisten.Margin = new Thickness(0, 400, 0, 0);
                }
                if (ResourceHelper.AppName == Apps.Vedic_Library.ToString())
                {
                    btnreadmore.Content = "Read Text";
                    btnlisten.Content = "Narration";
                    var ShowLinksByType = OnlineShow.GetShowLinksByType(AppSettings.ShowID, LinkType.Audio);
                    foreach (ShowLinks item in ShowLinksByType)
                    {
                        if (!string.IsNullOrEmpty(item.LinkUrl))
                        {
                            btnlistenmp3.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            btnlistenmp3.Visibility = Visibility.Collapsed;
                        }
                    }
                }
                else
                {
                    btnreadmore.Content = "Read Story";
                    btnlisten.Content = "Listen";
                }

                if (DirectoryExists("/StoryRecordings/" + AppSettings.ShowID + "/StoryRecordings.xml"))
                {
                    this.btnlisten.Visibility = Visibility.Visible;
                }
                else
                {
                    this.btnlisten.Visibility = Visibility.Collapsed;
                }

                string id = "";               
                if (IsBackgroundLoaded == false)
                {
                    LoadPivotThemes(AppSettings.ShowUniqueID);
                    tblkVideosTitle.Text = OnlineShow.GetShowDetail(AppSettings.ShowUniqueID).Title;
                }

                PageHelper.RemoveEntryFromBackStack("VedicDetails");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in VedicDetails_Loaded Method In VedicDetails.cs file.", ex);
            }
        }
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
        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            try
            {
               // FlurryWP8SDK.Api.EndTimedEvent("VedicDetails Page");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom Method In VedicDetails.cs file.", ex);
            }
        }
       
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Constants.backstack == "Details")
            {
                pvtMainDetails.SelectedIndex = 1;
                Constants.backstack = "";
            }
            //HardwareButtons.BackPressed += HardwareButtons_BackPressed;            
             try
            {
               // FlurryWP8SDK.Api.LogPageView();
                int showid = AppSettings.ShowUniqueID;
                if (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result.Status != "Custom")
                {                   
                    BottomAppBar.Visibility = Visibility.Collapsed;
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
                    BottomAppBar.Visibility = Visibility.Visible;                    
                }
                //navigationvalue = Convert.ToString(NavigatedValues.navigationvalue);
                //pivotindex = Convert.ToString(NavigatedValues.pivotindex);
                //if (NavigatedValues.navigationvalue!=null)
                //{
                    //if (navigationvalue == "1")
                    //{
                    //    if (pivotindex!=null)
                    //        pvtMainDetails.SelectedIndex = Convert.ToInt32(pivotindex);
                    //    Constants.navigationvalue++;
                    //}
                    //else
                    //{
                    //    if (pivotindex!=null)
                    //        pvtMainDetails.SelectedIndex = Convert.ToInt32(pivotindex);
                    //    Constants.navigationvalue--;
                    //}
                    
                }              
            //}
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In VedicDetails.cs file.", ex);
            }            
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if(Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        private void btnreadmore_Click(object sender, RoutedEventArgs e)
        {
            Constants.mode = "Read";
            Frame.Navigate(typeof(StoryReading));
        }
        private void btnbtnlisten_Click(object sender, RoutedEventArgs e)
        {
            Constants.mode = "Listen";
            Frame.Navigate(typeof(StoryReading));
        }
        private void btnbtnlistenmp3_Click(object sender, RoutedEventArgs e)
        {
            Constants.mode = "Listenmp3";
            Frame.Navigate(typeof(StoryReading));
        }
        private void btnrecard_Click(object sender, RoutedEventArgs e)
        {            
            if (Storage.DirectoryExists("StoryRecordings")==null)
                Storage.CreateDirectory("StoryRecordings");
            Constants.mode = "Rec";
            Frame.Navigate(typeof(StoryReading));
        }

        private void btnshare_Click_1(object sender, EventArgs e)
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
               Exceptions.SaveOrSendExceptions("Exception in btnshare_Click_1 Method In VedicDetails.cs file.", ex);
            }
        }        

        private void pvtMainDetails_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //if (Constants.backstack == "Details")
            //{
            //    pvtMainDetails.SelectedIndex = 2;
            //}
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
                            btnadd.Label = "Add" + " " + currentItem.Header.ToString();
                        }
                        else
                        {
                            if (bottomCommandBar.SecondaryCommands.Count < 3)
                            {
                                btnadd.Label = "Edit" + " " + "vedic text";
                                bottomCommandBar.SecondaryCommands.Add(addbtn);
                                addbtn.Click += btnadd_Click;
                                addbtn.Label = "Add" + " " + "vedic text";
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
    }
}
