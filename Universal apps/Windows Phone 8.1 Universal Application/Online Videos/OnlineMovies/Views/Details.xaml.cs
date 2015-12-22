using Common.Library;
using Common.Utilities;
using Indian_Cinema.Views;
using OnlineMovies.Views;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.UserControls;
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
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.ViewManagement;
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
    public sealed partial class Details : Page
    {
        #region Global
        AppInsights insights = new AppInsights();
        bool appbarvisible = true;
        string id = string.Empty;
        string title = string.Empty;
        string movietitle = string.Empty;
        PivotItem currentItem = default(PivotItem);
        Shows playerobj;
        private MediaPlayer _mediaPlayer;
        ComboBox pickr = default(ComboBox);
        Popup pop = new Popup();
        string[] parame = new string[3];
        public DispatcherTimer storyboardtimer;
        string pivotindex = string.Empty;
        #endregion

        public Details()
        {
            try
            {
                this.InitializeComponent();
                if (!ShowCastManager.ShowGamePivot(AppSettings.ShowID))
                {
                    pvtMainDetails.Items.Remove(gamepivot);
                }
                HideStatusBar();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Details Method In Details.cs file.", ex);
                insights.Exception(ex);
            }
        }

        public async void HideStatusBar()
        {
            var statusBar = StatusBar.GetForCurrentView();
            await statusBar.HideAsync();
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BackgroundMediaPlayer.Current.Pause();
            string[] parameters = (string[])e.Parameter;
            if (parameters[0] != null)
            {
                id = parameters[0];
                AppSettings.ShowID = id;
            }
            if(parameters[1] != null)
            {
                title = parameters[1];
            }
             HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            _mediaPlayer = BackgroundMediaPlayer.Current;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            int showid = AppSettings.ShowUniqueID;
            if (Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync().Result != null)
            {
                if (Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync().Result.Status == "Custom")
                {
                    Frame.Navigate(typeof(MainPage));
                }
            }
        }

        private void LoadAds()
        {            
            LoadAdds.LoadAdControl_New(LayoutRoot, adstaCast, 1);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if(State.BackStack=="cast")
                {
                    pvtMainDetails.SelectedIndex = 1;
                    State.BackStack = "";
                }
                else if(State.BackStack=="chapters")
                {
                    pvtMainDetails.SelectedIndex = 2;
                    State.BackStack = "";
                }
                else if(State.BackStack=="songs")
                {
                    pvtMainDetails.SelectedIndex = 3;
                    State.BackStack = "";
                }
                else if(State.BackStack=="audio")
                {
                    pvtMainDetails.SelectedIndex = 4;
                    State.BackStack = "";
                }
                else if(State.BackStack=="comedy")
                {
                    pvtMainDetails.SelectedIndex = 5;
                    State.BackStack="";
                }
                LoadAds();
                insights.Trace("Details page loaded");            
                tblkVideosTitle.Text = OnlineShow.GetShowDetail(Convert.ToInt32(AppSettings.ShowID)).Title.ToUpper();
                movietitle = tblkVideosTitle.Text;
            }
            catch (Exception ex)
            {

            }
            AppBarButton addButton = new AppBarButton();
            try
            {
                int showid = AppSettings.ShowUniqueID;
                if (Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync().Result != null)
                {
                    if (Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync().Result.Status != "Custom")
                    {                      
                        this.BottomAppBar.Visibility = Visibility.Collapsed;
                        appbarvisible = false;
                    }
                    else
                    {                      
                        this.BottomAppBar.Visibility = Visibility.Visible;
                    }
                }
                //string sharedstatus = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result.ShareStatus;
                string sharedstatus = "Not Shared To Blog";
                if (sharedstatus == "Shared To Blog")
                {
                    addButton.Label = "share this" + " " + AppResources.Type + "(" + "Shared)";
                }
                else
                {
                    //addButton.Label = "share this" + " " + AppResources.Type + "(" + "Not Shared)";
                    addButton.Label = "share this" + "  (" + "Not Shared)";
                }
                performanceProgressBar.IsIndeterminate = true;

                PageHelper.RemoveEntryFromBackStack("Details");
                if (Constants.topsongnavigation != "")
                {
                    pvtMainDetails.SelectedIndex = Convert.ToInt32(Constants.topsongnavigation);
                    Constants.topsongnavigation = "";
                }
                if (title == "")
                {
                    title = null;
                }
                if(title!=null)
                {                   
                    if (title == "movies")
                    {
                        title = "detail";
                        pvtMainDetails.SelectedIndex = 0;
                    }
                    if (title == "videos" || title=="songs")
                    {
                        title = "songs";
                        pvtMainDetails.SelectedIndex = 3;
                    }
                    if(title=="cast")
                    {
                        pvtMainDetails.SelectedIndex = 1;
                    }
                    if (title == "audio")
                    {
                        pvtMainDetails.SelectedIndex = 4;
                    }
                    if (title == "comedy")
                    {
                        pvtMainDetails.SelectedIndex = 5;
                    }
                    //PivotItem pi = pvtMainDetails.Items.Cast<PivotItem>().Single(i => i.Header.ToString() == title);                    
                    //pvtMainDetails.SelectedItem = pi;
                }
                performanceProgressBar.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In Details.cs file.", ex);
                insights.Exception(ex);
            }
        }

        public void selectpivot()
        {

        }

        private void pvtMainDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(State.BackStack=="cast")
            {
                pvtMainDetails.SelectedIndex = 2;
            }
            else if(State.BackStack=="chapters")
            {
                pvtMainDetails.SelectedIndex = 3;
            }
            else if(State.BackStack=="songs")
            {
                pvtMainDetails.SelectedIndex = 4;
            }
            else if(State.BackStack=="audio")
            {
                pvtMainDetails.SelectedIndex = 5;
            }
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
                                bottomCommandBar.SecondaryCommands.RemoveAt(1);
                                AppBarButton appBarMenuitem1= new AppBarButton();
                                appBarMenuitem1.Label = "Edit description";
                                appBarMenuitem1.Icon = new SymbolIcon(Symbol.Edit);
                                appBarMenuitem1.Click += btnadd_Click;
                                bottomCommandBar.SecondaryCommands.Add(appBarMenuitem1);
                            }
                            else
                            {
                                bottomCommandBar.SecondaryCommands.RemoveAt(1);
                                AppBarButton appBarMenuitem = new AppBarButton();
                                appBarMenuitem.Label = "Edit description";
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
                                appBarMenuitem.Label = "Add" + " " + currentItem.Header.ToString();
                                addButton.Click += btnadd_Click;
                                bottomCommandBar.SecondaryCommands.Add(appBarMenuitem);
                            }
                            else
                            {
                                bottomCommandBar.SecondaryCommands.RemoveAt(1);
                                AppBarButton appBarMenuitem11 = new AppBarButton();
                                appBarMenuitem11.Icon = new SymbolIcon(Symbol.Add);
                                appBarMenuitem11.Label = "Add" + " " + currentItem.Header.ToString();
                                appBarMenuitem11.Click += btnadd_Click;
                                bottomCommandBar.SecondaryCommands.Add(appBarMenuitem11);
                            }
                        }
                    }
                }
                if (pvtMainDetails.SelectedIndex == 2)
                {
                    BackgroundHelper bwhelper = new BackgroundHelper();
                    bwhelper.AddBackgroundTask(
                    (object s, DoWorkEventArgs a) =>
                    {
                         //addgame();
                    },
                    (object s, RunWorkerCompletedEventArgs a) =>
                    {
                    }
                    );
                    bwhelper.RunBackgroundWorkers();
                }
                else
                {                   
                    if (BackgroundMediaPlayer.Current.CurrentState == MediaPlayerState.Playing)
                    {                       
                        object name;
                        var TrackName = playerobj.AudioTrackName;
                        TrackName.TryGetValue("Play", out name);
                        string Tracksource = Convert.ToString(name);
                        if (BackgroundMediaPlayer.Current != null && Tracksource.Contains("Claps.mp3"))
                        {                            
                            BackgroundMediaPlayer.Current.Pause();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in pvtMainDetails_SelectionChanged Method In Details.cs file.", ex);
            }
        }

        public void addgame()
        {
            try
            {
                //Deployment.Current.Dispatcher.BeginInvoke(() =>
                //{
                    if (!ShowCastManager.ShowGameControl(AppSettings.ShowID))
                    {
                        if (GameGrid.Children.Count > 0)
                            GameGrid.Children.RemoveAt(0);
                        MemoryGame_1 showgame = new MemoryGame_1();
                        GameGrid.Children.Add(showgame);
                        storyboardtimer = new DispatcherTimer();
                        storyboardtimer.Interval = TimeSpan.FromSeconds(2);
                        storyboardtimer.Tick += storyboardtimer_Tick;
                        storyboardtimer.Start();
                    }
                //});
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in addgame Method In Details.cs file.", ex);
                //insights.Exception(ex);
            }
        }

        void storyboardtimer_Tick(object sender, object e)
        {
            try
            {
                if (pvtMainDetails.SelectedIndex == 2)
                {
                    if (OnlineVideosCardGame.GameInstance.gameopened == false)
                    {
                        foreach (OnlineVideosCardGame.Model.MemoryCard m in OnlineVideosCardGame.GameInstance.stopstoryboard)
                        {
                            m.storyboardfromx = "3";
                            m.storyboardtox = "-3";
                            m.storyboardfromy = "3";
                            m.storyboardtoy = "-3";
                            m.storyboardfromz = "3";
                            m.storyboardtoz = "-3";
                        }
                        storyboardtimer.Stop();
                    }
                }
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in storyboardtimer_Tick Method In Details.cs file.", ex);
                //insights.Exception(ex);

            }
        }

        private void imgTitle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));            
        }

        private void Share_Click(object sender, RoutedEventArgs e)
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
                Exceptions.SaveOrSendExceptions("Exception in btnshare_Click_1 Method In Details.cs file.", ex);
               // insights.Exception(ex);
            }
        }

        private void btnadd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((sender as AppBarButton).Label.Contains("cast"))
                {
                    Constants.DownloadTimer.Stop();
                    Frame.Navigate(typeof(AddCast_New));
                }
                else if ((sender as AppBarButton).Label.Contains("description"))
                {
                    Constants.DownloadTimer.Stop();
                    Constants.editdescription = true;
                    Frame.Navigate(typeof(Description));
                }
                else
                {
                    if ((sender as AppBarButton).Label.Contains("chapters"))
                    {
                        Constants.DownloadTimer.Stop();
                        Constants.Linkstype = "Movies";
                        parame[0] = id;
                        parame[1] = string.Empty;
                        parame[2] = "chapters";

                        Frame.Navigate(typeof(LinksFromOnline), parame);
                    }
                    else if ((sender as AppBarButton).Label.Contains("songs"))
                    {
                        Constants.DownloadTimer.Stop();
                        Constants.Linkstype = "Songs";

                        parame[0] = id;
                        parame[1] = string.Empty;
                        parame[2] = "songs";

                        Frame.Navigate(typeof(LinksFromOnline), parame);
                    }
                    else if ((sender as AppBarButton).Label.Contains("audio"))
                    {
                        Constants.DownloadTimer.Stop();
                        Constants.Linkstype = "Audio";
                        if (ResourceHelper.AppName == "Indian_Cinema.WindowsPhone" || ResourceHelper.AppName == Apps.Indian_Cinema_Pro.ToString())
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
                            saveimg.Source = new BitmapImage(new Uri("ms-appx:///Images/rating/appbar.check.rest.png", UriKind.RelativeOrAbsolute));
                            saveimg.PointerPressed += saveimg_PointerPressed;
                            Image cancelimg = new Image();
                            cancelimg.Height = 48;
                            cancelimg.Width = 48;
                            cancelimg.Margin = new Thickness(40, 0, 0, 0);
                            cancelimg.Source = new BitmapImage(new Uri("ms-appx:///Images/rating/appbar.cancel.rest.png", UriKind.RelativeOrAbsolute));
                            cancelimg.PointerPressed += cancelimg_PointerPressed;
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
                            parame[0] = id;
                            parame[1] = "Hindi";
                            parame[2] = "audio";
                            Frame.Navigate(typeof(LinksFromOnline), parame);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in btnadd_Click_1 Method In Details.cs file.", ex);
                //insights.Exception(ex);
            }
        }

        void pickr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string language = (sender as ComboBox).SelectedItem.ToString();
        }

        void cancelimg_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            pop.IsOpen = false;
        }

        void saveimg_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            pop.IsOpen = false;
            parame[0] = id;
            parame[1] = pickr.SelectedItem.ToString();
            parame[2] = "audio";
            Frame.Navigate(typeof(LinksFromOnline), parame);
        }

        private void chapters_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LinksFromOnline), null);
        }
    }
}