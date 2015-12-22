
using Common.Library;
using OnlineVideos;
using OnlineVideos.Data;
using OnlineVideos.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
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
    public sealed partial class CricketDetail : Page
    {
        Shows playerobj;
        public DispatcherTimer storyboardtimer;
        
        public CricketDetail()
        {
            try
            {
                this.InitializeComponent();
                if (!ShowCastManager.ShowGamePivot(AppSettings.ShowID))
                {
                    pvtMainDetails.Items.Remove(gamepivot);
                }
            }
            catch(Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in CricketDetail Method In CricketDetail.cs file.", ex);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //FlurryWP8SDK.Api.LogEvent("CricketDetail Page", true);
                if (pvtMainDetails.SelectedIndex == 0)
                {
                    Border frameBorder = (Border)VisualTreeHelper.GetChild(Window.Current.Content, 0);
                    
                }
                tblkVideosTitle.Text = CricketMatch.GetMovieTitle(AppSettings.ShowID);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In CricketDetail.cs file.", ex);
            }
        }

        public async void addgame()
        {
            try
            {
                await Windows.UI.Xaml.Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                {
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
                });
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in addgame Method In CricketDetail.cs file.", ex);
            }
        }

        private void storyboardtimer_Tick(object sender, object e)
        {
            try
            {
                if (pvtMainDetails.SelectedIndex == 2)
                {
                    
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in storyboardtimer_Tick Method In CricketDetail.cs file.", ex);
            }
        }
        
        showvideolbx svlbox;
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {            
            try
            {               
                if (svlbox.SVListBox.IsEnabled == false)
                    svlbox.SVListBox.IsEnabled = true;                                                 
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In CricketDetail.cs file.", ex);
            }
        }
        ShowVideos dd = new ShowVideos();        
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {            
            try
            {
                AppState.searchtitle = "";                
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom Method In CricketDetail.cs file.", ex);
            }
        }

        private void pvtMainDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {                    
            try
            {
                if (pvtMainDetails.SelectedIndex == 0)
                {
                    Border frameBorder = (Border)VisualTreeHelper.GetChild(Window.Current.Content, 0);
                    
                }
                else
                {
                    Border frameBorder = (Border)VisualTreeHelper.GetChild(Window.Current.Content, 0);
                   
                }

                if (pvtMainDetails.SelectedIndex == 2)
                {
                    BackgroundHelper bwhelper = new BackgroundHelper();
                    bwhelper.AddBackgroundTask(
                    (object s, DoWorkEventArgs a) =>
                    {
                        addgame();
                    },
                    (object s, RunWorkerCompletedEventArgs a) =>
                    {
                    }
                    );
                    bwhelper.RunBackgroundWorkers();
                }
                else
                {
                    if (BackgroundMediaPlayer.Current.CurrentState==MediaPlayerState.Playing)                   
                    {
                        object name;
                        var TrackName = playerobj.AudioTrackName;
                        TrackName.TryGetValue("Play", out name);
                        string Tracksource=Convert.ToString(name);                       
                        if(BackgroundMediaPlayer.Current!=null && Tracksource.Contains("Claps.mp3"))                    
                        {                           
                            BackgroundMediaPlayer.Current.Pause();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in pvtMainDetails_SelectionChanged Method In CricketDetail.cs file.", ex);
            }
        }

        private void imgTitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
