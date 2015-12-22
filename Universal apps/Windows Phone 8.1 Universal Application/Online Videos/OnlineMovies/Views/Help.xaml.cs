using Common.Library;
using Common.Utilities;
using OnlineMovies.Views;
using OnlineVideos.Data;
using System;
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
    public sealed partial class Help : Page
    {
        #region GlobalDeclaration
        string id, url;
        #endregion

        #region Constructor
        public Help()
        {
            this.InitializeComponent();
            id = string.Empty;
            url = string.Empty;        
            Loaded += Help_Loaded;
        }        
        #endregion

        #region "Common Methods"
        private void LoadAds()
        {
            try
            {
                LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 2);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In SongDetails file", ex);
                string excepmess = "Exception in LoadAds Method In SongDetails file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }
        #endregion

        #region PageLoad
        void Help_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadAds();
                if (id != null)
                {
                    if (url != null)
                    {
                        tblkVideosTitle.Text = OnlineShow.GetHelpItemTitle(id);
                    }
                }
                lbxhelp.ItemsSource = OnlineShow.GetHelpItem(id);
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in Help_Loaded Method In Help.cs file.", ex);
            }
        }
        #endregion
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //HardwareButtons.BackPressed+=HardwareButtons_BackPressed;
            string[] parameters = (string[])e.Parameter;
            id = parameters[0];
            url = parameters[1];

            try
            {
               
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In Help.cs file.", ex);
            }            
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            SettingsHelper.Save("Repeatads", "1");
            if(Frame.CanGoBack)
            {
                //e.Handled = true;
                //Frame.GoBack();
                Frame.Navigate(typeof(HelpMenu));
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
                //FlurryWP8SDK.Api.EndTimedEvent("Help Page");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom Method In Help.cs file.", ex);
            }
        }

        private void imgTitle_Tapped(object sender, TappedRoutedEventArgs e)
        {            
            Frame.Navigate(typeof(MainPage));
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AppSettings.LinkTitle = OnlineShow.GetHelpItemTitle(id);
                Frame.Navigate(typeof(Youtube), url);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ApplicationBarIconButton_youtube_Click Method In Help.cs file.", ex);
                UtilitiesManager.LoadBrowserTask(url);
            }
        }
    }
}
