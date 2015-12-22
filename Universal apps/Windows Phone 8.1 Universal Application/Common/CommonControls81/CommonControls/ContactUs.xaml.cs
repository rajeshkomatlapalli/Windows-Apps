using Common.Library;
using Common.Utilities;
using Indian_Cinema;
using OnlineVideos;
using System;
using System.Collections.Generic;
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

namespace CommonControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContactUs : Page
    {
        public ContactUs()
        {
            this.InitializeComponent();
            Loaded += ContactUs_Loaded;
        }

        void ContactUs_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAdds();            
        }
       
        private void LoadAdds()
        {
            //LoadAdds.LoadAdControl_New(LayoutRoot, AdContainer, 1);
            if (!UtilitiesResources.ShowAdControl)
            {
                if (LayoutRoot.RowDefinitions.Count > 1)
                {
                    RowDefinition myrow = LayoutRoot.RowDefinitions[1];
                    LayoutRoot.RowDefinitions.Remove(myrow);
                }
            }
            else
            {
                if (UtilitiesResources.ShowAdRotator)
                {
                    AdContainer.AdUnitId = UtilitiesResources.AdControlAdUnitID;
                    AdContainer.ApplicationId = UtilitiesResources.AdControlApplicationID;
                }
                else
                {
                    AdContainer.AdUnitId = UtilitiesResources.AdControlAdUnitID;
                    AdContainer.ApplicationId = UtilitiesResources.AdControlApplicationID;
                }
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if(Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        private void imgTitle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void facebookImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            fb_Tapped(sender, e);
        }

        private void fb_Tapped(object sender, TappedRoutedEventArgs e)
        {
            UtilitiesManager.LoadBrowserTaskToSocialNetwork(Constants.CompanyFacebookPageUrl);
        }

        private void twitter_Tapped(object sender, TappedRoutedEventArgs e)
        {
            tw_Tapped(sender, e);
        }

        private void tw_Tapped(object sender, TappedRoutedEventArgs e)
        {
            UtilitiesManager.LoadBrowserTaskToSocialNetwork(Constants.CompanyTwitterPageUrl); 
        }

        private void blogger_Tapped(object sender, TappedRoutedEventArgs e)
        {
            bg_Tapped(sender, e);
        }

        private void bg_Tapped(object sender, TappedRoutedEventArgs e)
        {
            UtilitiesManager.LoadBrowserTaskToSocialNetwork(Constants.CompanyBlogPageUrl);
        }

        private void youtube_Tapped(object sender, TappedRoutedEventArgs e)
        {
            yt_Tapped(sender, e);
        }

        private void yt_Tapped(object sender, TappedRoutedEventArgs e)
        {
            UtilitiesManager.LoadBrowserTaskToSocialNetwork(Constants.CompanyYoutubePageUrl);
        }

        private void SendFeedback_Tapped(object sender, TappedRoutedEventArgs e)
        {
            sfb_Tapped(sender, e);
        }

        private void sfb_Tapped(object sender, TappedRoutedEventArgs e)
        {           
            //Frame frame = PageHelper.GetDependencyObjectFromVisualTree(this, typeof(Frame)) as Frame;
            string[] parametes = new string[5];
            parametes[0] = null;
            parametes[1] = null;
            parametes[2] = null;
            parametes[3] = null;
            parametes[4] = null;
            Frame.Navigate(typeof(Feedback), parametes);
        }
    }
}
