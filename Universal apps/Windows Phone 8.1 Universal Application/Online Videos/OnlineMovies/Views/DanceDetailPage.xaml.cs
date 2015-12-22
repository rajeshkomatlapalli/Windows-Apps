using Common.Library;
using Common.Utilities;
using Indian_Cinema.Views;
using OnlineVideos;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.UI;
using OnlineVideos.UserControls;
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
    public sealed partial class DanceDetailPage : Page
    {
        string id = string.Empty;
        string background = "0";
        string chapter = string.Empty;
        public DispatcherTimer storyboardtimer;
        bool appbarvisible = true;
        string navigationvalue = string.Empty;
        PivotItem currentItem = default(PivotItem);
        string pivotindex = string.Empty;
        showvideolbx svlbx;

        private async void LoadPivotThemes(long ShowID)
        {
            pvtMainDetails.Background =await ShowDetail.LoadShowPivotBackground(ShowID);

            Loaded += new RoutedEventHandler(PhoneApplicationPage_Loaded);
        }
        public DanceDetailPage()
        {
            this.InitializeComponent();
             if (AppSettings.ShowID != "0")
            {
                LoadPivotThemes(AppSettings.ShowUniqueID);
                tblkVideosTitle.Text = OnlineShow.GetShowDetail(AppSettings.ShowUniqueID).Title;
                background = "1";
            }            
        }
         private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
         {
             AppBarButton addButton = new AppBarButton();
           // FlurryWP8SDK.Api.LogEvent("DanceDetailPage", true);
            int showid = AppSettings.ShowUniqueID;
            string sharedstatus = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result.ShareStatus;
            if (sharedstatus == "Shared To Blog")              
                addButton.Label = "share this" + " " + AppResources.Type + "(" + "Shared To Blog)";
            else               
                addButton.Label = "share this" + " " + AppResources.Type + "(" + "Not Shared)";
            performanceProgressBar.IsIndeterminate = true;            
                AppSettings.ShowID = id;

            if (background == "0")
            {
                LoadPivotThemes(AppSettings.ShowUniqueID);
                tblkVideosTitle.Text = OnlineShow.GetShowDetail(AppSettings.ShowUniqueID).Title;
            }
            performanceProgressBar.IsIndeterminate = false;
            PageHelper.RemoveEntryFromBackStack("DanceDetailPage");
        }
       
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //HardwareButtons.BackPressed += HardwareButtons_BackPressed;           
            id = (string)e.Parameter;
            //FlurryWP8SDK.Api.LogPageView();
            int showid = AppSettings.ShowUniqueID;
            if (Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync().Result.Status != "Custom")
            {              
              this.BottomAppBar.Visibility = Visibility.Collapsed;
              appbarvisible = false;
            }
            else
            {               
                this.BottomAppBar.Visibility = Visibility.Visible;
            }
            //if (NavigatedValues.navigationvalue!=null)
            //{            
            //    if (NavigatedValues.navigationvalue == 1)
            //    {
            //        if (NavigatedValues.pivotindex!=null)                    
            //        pvtMainDetails.SelectedIndex = Convert.ToInt32(NavigatedValues.pivotindex);
            //        Constants.navigationvalue++;
            //    }
            //    else
            //    {
            //        if (NavigatedValues.pivotindex!=null)                   
            //        pvtMainDetails.SelectedIndex = Convert.ToInt32(NavigatedValues.pivotindex);
            //        Constants.navigationvalue--;
            //    }
            //    while (Frame.BackStack.Count() > 1)
            //    {
            //        if (!Frame.BackStack.FirstOrDefault().SourcePageType.Equals("DanceDetailPage"))
            //        {                     
            //            Frame.BackStack.RemoveAt(-1);
            //        }
            //        else
            //        {                        
            //            Frame.BackStack.RemoveAt(-1);
            //            break;
            //        }
            //    }
            //}
            if (e.NavigationMode == Windows.UI.Xaml.Navigation.NavigationMode.Back)
            {
                AppState.searchtitle = "";
            }
            else
            {                
            }
            
            if (svlbx.SVListBox.IsEnabled == false)
                svlbx.SVListBox.IsEnabled = true;
            
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if(Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //FlurryWP8SDK.Api.EndTimedEvent("DanceDetailPage");
            AppState.searchtitle = "";
        }      
        private void imgTitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {            
            Frame.Navigate(typeof(MainPage));
        }
         private void pvtMainDetails_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            AppBarButton addButton = new AppBarButton();
            CommandBar bottomCommandBar = this.BottomAppBar as CommandBar;
            currentItem = e.AddedItems[0] as PivotItem;
            if (appbarvisible == true)
            {
                if (currentItem != null)
                {
                    if (currentItem.Header.ToString() == "detail")
                    {
                        if (bottomCommandBar.SecondaryCommands.Count == 2)                         
                            addButton.Label = "Edit description";
                        else
                        {
                            AppBarButton appBarMenuitem = new AppBarButton();
                            appBarMenuitem.Icon = new SymbolIcon(Symbol.Edit);
                            appBarMenuitem.Click += btnadd_Click;
                            bottomCommandBar.SecondaryCommands.Add(appBarMenuitem);                           
                        }
                    }
                    else
                    {                        
                        if (bottomCommandBar.SecondaryCommands.Count < 2)
                        {                            
                            AppBarButton appBarMenuitem = new AppBarButton();
                            appBarMenuitem.Icon = new SymbolIcon(Symbol.Add);
                            appBarMenuitem.Label = "Add";
                            addButton.Click += btnadd_Click;
                            bottomCommandBar.SecondaryCommands.Add(addButton);
                        }
                        else
                        {                          
                            addButton.Label = "Add" + " " + currentItem.Header.ToString();
                        }
                    }
                }
            }
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
            if ((sender as CommandBar).Content.Equals("videos"))
            {
                Constants.DownloadTimer.Stop();
                Constants.Linkstype = "Songs";
                string language = string.Empty;                
                Frame.Navigate(typeof(LinksFromOnline), language);
            }
           
            else if ((sender as CommandBar).Content.Equals("description"))
            {
                Constants.DownloadTimer.Stop();
                Constants.editdescription = true;                
                Frame.Navigate(typeof(Description));
            }
        }
    }
}
