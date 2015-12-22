using Common.Library;
using System;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
//using LART.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineVideos.Views
{

    public sealed partial class BackAgentError : Page
    {
        #region Constructor
        public BackAgentError()
        {
            this.InitializeComponent();
            Loaded += BackAgentError_Loaded;
        }
        #endregion

        #region PageLoad
        void BackAgentError_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAds();
            string backgroundAgentError = AppSettings.BackgroundAgenError;

            if (!string.IsNullOrEmpty(backgroundAgentError))
            {
                NoError.Visibility = Visibility.Visible;
                NoError.Text = backgroundAgentError;
            }
            else
            {
                NoError.Visibility = Visibility.Visible;
                NoError.Text = "No errors in background agent.";
            }
        }
                       
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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //HardwareButtons.BackPressed += HardwareButtons_BackPressed;
          //  FlurryWP8SDK.Api.LogPageView();
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
         
        }        

        private void btnmusic_Click(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(MusicSettings));
        }

        private void imgTitle_Tapped(object sender, TappedRoutedEventArgs e)
        {          
            Frame.Navigate(typeof(MainPage));
        }
    }
}
