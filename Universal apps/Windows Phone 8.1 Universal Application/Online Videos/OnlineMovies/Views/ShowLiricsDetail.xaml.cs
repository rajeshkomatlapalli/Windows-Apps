using Common.Library;
using Common.Utilities;
using OnlineVideos;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class ShowLiricsDetail : Page
    {
        #region GlobalDeclaration
        List<ShowLinks> objLiricsListdes;
        #endregion

        #region Constructor
        public ShowLiricsDetail()
        {
            this.InitializeComponent();
            objLiricsListdes = new List<ShowLinks>();
        }
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                // FlurryWP8SDK.Api.LogPageView();
                int showid = AppSettings.ShowUniqueID;
                if (Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync().Result.Status != "Custom")
                {
                    this.BottomAppBar.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.BottomAppBar.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In ShowLiricsDetail file.", ex);
            }
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            if(frame==null)
            {
                return;
            }
            if(Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
                //FlurryWP8SDK.Api.EndTimedEvent("ShowLiricsDetail Page");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom Method In ShowLiricsDetail file.", ex);
            }
        }

        private void LoadAds()
        {
            try
            {
                //PageHelper.LoadAdControl_New(LayoutRoot, adstackpl, 1);
                LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In SongDetails file", ex);
                string excepmess = "Exception in LoadAds Method In SongDetails file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }

        private void GetPageDataInBackground()
        {
            BackgroundHelper bwHelper = new BackgroundHelper();

            bwHelper.AddBackgroundTask(
                                        (object s, DoWorkEventArgs a) =>
                                        {
                                            a.Result = OnlineShow.GetLyrics(AppSettings.ShowID, AppSettings.LiricsLink/*, false*/);
                                        },
                                        (object s, RunWorkerCompletedEventArgs a) =>
                                        {
                                            objLiricsListdes = (List<ShowLinks>)a.Result;
                                            if (objLiricsListdes.FirstOrDefault().Description != null)
                                            {
                                                lbxLyricsList.ItemsSource = objLiricsListdes;
                                                tblkFavNoLyricsDescription.Visibility = Visibility.Collapsed;
                                            }
                                            else
                                            {
                                                tblkFavNoLyricsDescription.Visibility = Visibility.Visible;
                                            }
                                        }
                                      );

            bwHelper.RunBackgroundWorkers();
        }

        #region Events
        private void imgTitle_KeyDown(object sender, KeyRoutedEventArgs e)
        {           
            Frame.Navigate(typeof(MainPage));
        }

        private void btnadd_Click(object sender, RoutedEventArgs e)
        {
            Constants.DownloadTimer.Stop();            
            Frame.Navigate(typeof(UserBrowserPage), "querytext=Lyrics&searchquery=" + AppSettings.Title);
        }
        #endregion

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
               // FlurryWP8SDK.Api.LogEvent("ShowLiricsDetail Page", true);
                if (ResourceHelper.AppName != Apps.Indian_Cinema_Pro.ToString() && ResourceHelper.AppName != Apps.Kids_TV_Pro.ToString() && ResourceHelper.AppName != Apps.Story_Time_Pro.ToString())
                    LoadAds();
                GetPageDataInBackground();
                tblkLyricTitle.Text = AppSettings.LiricsLink.ToUpper();
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In ShowLiricsDetail file.", ex);
            }
        }
    }
}
