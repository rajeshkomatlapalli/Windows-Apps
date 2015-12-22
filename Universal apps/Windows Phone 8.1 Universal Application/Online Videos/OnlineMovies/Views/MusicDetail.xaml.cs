using Common.Library;
using Common.Utilities;
using Indian_Cinema.Views;
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
using Windows.Media.Playback;
using Windows.UI;
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
    public sealed partial class MusicDetail : Page
    {
        #region GlobalDeclaration
        bool appbarvisible = true;
        ComboBox pickr = default(ComboBox);
        Popup pop = new Popup();
        string pivotindex = string.Empty;
        string navigationvalue = string.Empty;
        PivotItem currentItem = default(PivotItem);
        string background = "0";
        string id = "";
        string Atitle = "";
        string chapter = string.Empty;
        string type = string.Empty;
        string secondarytileindex = string.Empty;
        public DispatcherTimer storyboardtimer;
        Shows playerobj;
        private MediaPlayer _mediaPlayer;
        #endregion

        public MusicDetail()
        {
            try
            {
                this.InitializeComponent();
                Loaded += MusicDetail_Loaded;
                if(AppSettings.ShowID!="0")
                {                    
                    tblkVideosTitle.Text = OnlineShow.GetShowDetail(AppSettings.ShowUniqueID).Title.ToUpper();
                    background = "1";
                }
                if(!ShowCastManager.ShowGamePivot(AppSettings.ShowID))
                {
                    pvtMainDetails.Items.Remove(gamepivot);
                }
            }
            catch(Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in MusicDetail Method In MusicDetail.cs file.", ex);
            }
        }        

        #region "Common Methods"
        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            try
            {
                //FlurryWP8SDK.Api.EndTimedEvent("MusicDetail Page");
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom Method In MusicDetail.cs file.", ex);
            }
        }
       
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _mediaPlayer = BackgroundMediaPlayer.Current;            
            try
            {
                //FlurryWP8SDK.Api.LogPageView();
                int showid = AppSettings.ShowUniqueID;

                if (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result.Status != "Custom")
                {                   
                    this.BottomAppBar.Visibility = Visibility.Collapsed;
                    appbarvisible = false;
                }
                else
                {                    
                    this.BottomAppBar.Visibility = Visibility.Visible;
                }
                //if (NavigatedValues.navigationvalue != null)
                //{

                //    if (NavigatedValues.navigationvalue == 1)
                //    {
                //        if (NavigatedValues.pivotindex != null)

                //            pvtMainDetails.SelectedIndex = Convert.ToInt32(NavigatedValues.pivotindex);
                //        Constants.navigationvalue++;
                //    }
                //    else
                //    {
                //        if (NavigatedValues.pivotindex != null)
                //            pvtMainDetails.SelectedIndex = Convert.ToInt32(NavigatedValues.pivotindex);
                //        Constants.navigationvalue--;
                //    }
                //    while (Frame.BackStack.Count() > 1)
                //    {
                //        if (!Frame.BackStack.FirstOrDefault().SourcePageType.Equals("MusicDetail"))
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
                performanceProgressBar.IsIndeterminate = true;                
                if (type == "search")
                {                    
                    Frame.BackStack.RemoveAt(-1);
                }
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In MusicDetail.cs file.", ex);
            }
        }
        public async void addgame()
        {
            try
            {
                await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                {
                    if (ShowCastManager.ShowGamePivot(AppSettings.ShowID))
                    {
                        storyboardtimer = new DispatcherTimer();
                        storyboardtimer.Interval = TimeSpan.FromSeconds(2);
                        storyboardtimer.Tick += new EventHandler<object>(storyboardtimer_Tick);
                        storyboardtimer.Start();
                    }
                });
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in addgame Method In MusicDetail.cs file.", ex);
            }
        }
        #endregion
        #region PageLoad
        void MusicDetail_Loaded(object sender, RoutedEventArgs e)            
        {
            AppBarButton addButton = new AppBarButton();
            try
            {
                LoadAds();
                //FlurryWP8SDK.Api.LogEvent("MusicDetail Page", true);
                int showid = AppSettings.ShowUniqueID;
                string sharedstatus = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result.ShareStatus;
                if (sharedstatus == "Shared To Blog")
              
                    addButton.Label = "share this" + " " + AppResources.Type + "(" + "Shared)";
                else
                    
                    addButton.Label = "share this" + " " + AppResources.Type + "(" + "Not Shared)";
                if (background == "0")
                {
                    tblkVideosTitle.Text = OnlineShow.GetShowDetail(AppSettings.ShowUniqueID).Title.ToUpper();
                }


                PageHelper.RemoveEntryFromBackStack(AppResources.DetailPageName);
                if (Constants.topsongnavigation != "")
                {
                    pvtMainDetails.SelectedIndex = Convert.ToInt32(Constants.topsongnavigation);
                    Constants.topsongnavigation = "";
                }
                performanceProgressBar.IsIndeterminate = false;
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in MusicDetail_Loaded Method In MusicDetail.cs file.", ex);
            }

        }
        #endregion

        private void imgTitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {           
            Frame.Navigate(typeof(MainPage));
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
                Exceptions.SaveOrSendExceptions("Exception in btnshare_Click_1 Method In MusicDetail.cs file.", ex);
            }
        }

        private void btnadd_Click(object sender, RoutedEventArgs e)
        {
            try
            {              
                if ((sender as CommandBar).Content.Equals("cast"))
                {
                    Constants.DownloadTimer.Stop();                   
                    Frame.Navigate(typeof(AddCast_New));
                }
              
                else if ((sender as CommandBar).Content.Equals("description"))
                {
                    Constants.DownloadTimer.Stop();
                    Constants.editdescription = true;                   
                    Frame.Navigate(typeof(Description));
                }
                else
                {                    
                    if ((sender as CommandBar).Content.Equals("chapters"))
                    {
                        Constants.DownloadTimer.Stop();
                        Constants.Linkstype = "Movies";
                        string language = string.Empty;                      
                        Frame.Navigate(typeof(LinksFromOnline), language);
                    }                   
                    else if ((sender as CommandBar).Content.Equals("songs"))
                    {
                        Constants.DownloadTimer.Stop();
                        Constants.Linkstype = "Songs";
                        string language = string.Empty;                    
                        Frame.Navigate(typeof(LinksFromOnline), language);
                    }                
                    else if ((sender as CommandBar).Content.Equals("audio"))
                    {
                        Constants.DownloadTimer.Stop();
                        Constants.Linkstype = "Audio";

                        if (ResourceHelper.ProjectName == "Indian Cinema")
                        {
                            StackPanel messagestk = new StackPanel();
                            messagestk.Background = new SolidColorBrush(Colors.Black);
                            messagestk.Opacity = 0.8;
                            messagestk.Margin = new Thickness(70, 130, 0, 0);
                            messagestk.HorizontalAlignment = HorizontalAlignment.Center;
                            messagestk.VerticalAlignment = VerticalAlignment.Center;
                            messagestk.Orientation = Windows.UI.Xaml.Controls.Orientation.Vertical;
                            messagestk.Height = 300;
                            messagestk.Width = 300;

                            TextBlock tblklinktype = new TextBlock();
                            tblklinktype.Height = 60;
                            tblklinktype.Width = 200;
                            tblklinktype.Foreground = new SolidColorBrush(Colors.White);
                            tblklinktype.Margin = new Thickness(-10, 60, 0, 0);
                            tblklinktype.FontSize = 28;
                            tblklinktype.Text = "Select Language";
                            pickr = new ComboBox();
                            pickr.Width = 170;
                            pickr.Margin = new Thickness(-20, -10, 0, 0);

                            pickr.Items.Add("Hindi");
                            pickr.Items.Add("Telugu");
                            pickr.Items.Add("Tamil");
                            StackPanel imgstck = new StackPanel();
                            imgstck.Orientation = Windows.UI.Xaml.Controls.Orientation.Horizontal;
                            imgstck.Margin = new Thickness(45, 30, 0, 0);
                            Image saveimg = new Image();
                            saveimg.Height = 48;
                            saveimg.Width = 48;
                            saveimg.Source = new BitmapImage(new Uri("Images/rating/appbar.check.rest.png", UriKind.Relative));
                            saveimg.PointerPressed+=saveimg_PointerPressed;
                            Image cancelimg = new Image();
                            cancelimg.Height = 48;
                            cancelimg.Width = 48;
                            cancelimg.Margin = new Thickness(40, 0, 0, 0);
                            cancelimg.Source = new BitmapImage(new Uri("Images/rating/appbar.cancel.rest.png", UriKind.Relative));
                            cancelimg.PointerPressed+=cancelimg_PointerPressed;
                            imgstck.Children.Add(saveimg);
                            imgstck.Children.Add(cancelimg);
                            messagestk.Children.Add(tblklinktype);
                            messagestk.Children.Add(pickr);
                            messagestk.Children.Add(imgstck);

                            pop.Child = messagestk;
                            pop.IsOpen = true;
                            pickr.SelectionChanged += pickr_SelectionChanged;
                        }
                        else
                        {                           
                            Frame.Navigate(typeof(LinksFromOnline),"?language=Hindi");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in btnadd_Click Method In MusicDetail.cs file.", ex);
            }
        }

        void cancelimg_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
             pop.IsOpen = false;
        }

        void saveimg_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            pop.IsOpen = false;       
            Frame.Navigate(typeof(LinksFromOnline),"?language="+pickr.SelectedItem.ToString());
        } 	
         
        void pickr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string language = (sender as ComboBox).SelectedItem.ToString();
        }
    
     #region Events
        private void LoadAds()
        {
            try
            {
                //PageHelper.LoadAdControl(LayoutRoot, adstackpl, 1);
                LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In SongDetails file", ex);
                string excepmess = "Exception in LoadAds Method In SongDetails file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }

        void storyboardtimer_Tick(object sender, object e)
        {
            try
            {
            if (pvtMainDetails.SelectedIndex == 2)
            {                
            }
            }
            catch (Exception ex)
            {
              Exceptions.SaveOrSendExceptions("Exception in storyboardtimer_Tick Method In MusicDetail.cs file.", ex);
            }
        }

        private void pvtMainDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AppBarButton addButton = new AppBarButton();
            CommandBar bottomCommandBar = this.BottomAppBar as CommandBar;
            try
            {
            currentItem = e.AddedItems[0] as PivotItem;
            if (appbarvisible == true)
            {
                if (currentItem != null)
                {
                    if (currentItem.Header.ToString() == "detail")
                    {
                        if (bottomCommandBar.SecondaryCommands.Count == 2)
                        {
                            addButton.Label = "Edit description";
                        }
                        else
                        {                           
                            AppBarButton appBarMenuitem = new AppBarButton();
                            appBarMenuitem.Icon = new SymbolIcon(Symbol.Edit);
                            appBarMenuitem.Click += btnadd_Click;
                            bottomCommandBar.SecondaryCommands.Add(appBarMenuitem);
                        }
                    }
                    else if (currentItem.Header.ToString() == "lyrics")
                    {                      
                        if (bottomCommandBar.SecondaryCommands.Count == 2)
                            bottomCommandBar.SecondaryCommands.RemoveAt(1);
                    }
                    else if (currentItem.Header.ToString() == "game")
                    {                     
                        if (bottomCommandBar.SecondaryCommands.Count == 2)
                            bottomCommandBar.SecondaryCommands.RemoveAt(1);
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
            if (pvtMainDetails.SelectedIndex == 2)
            {
                performanceProgressBar.IsIndeterminate = true;
                BackgroundHelper bwhelper = new BackgroundHelper();
                bwhelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                addgame();
                                            },
                                             (object s, RunWorkerCompletedEventArgs a) =>
                                             {
                                                 performanceProgressBar.IsIndeterminate = false;
                                             }
                                          );
                bwhelper.RunBackgroundWorkers();
                bwhelper.StartScheduledTasks();
            }
            else
            {               
                if (BackgroundMediaPlayer.Current.CurrentState==MediaPlayerState.Playing)
                {                    
                    object name;
                    var TrackName = playerobj.AudioTrackName;
                    TrackName.TryGetValue("Play", out name);
                    string Tracksource=Convert.ToString(name);
                    if (BackgroundMediaPlayer.Current != null && Tracksource.Contains("Claps.mp3"))
                    {                       
                        BackgroundMediaPlayer.Current.Pause();
                    }
                }
            }
            }
            catch (Exception ex)
            {
                  Exceptions.SaveOrSendExceptions("Exception in pvtMainDetails_SelectionChanged Method In MusicDetail.cs file.", ex);
            }
        }
     #endregion
  }
}