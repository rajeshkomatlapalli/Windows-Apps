using Common;
using Common.Library;
using Common.Utilities;
using OnlineVideos.Data;
using OnlineVideos.UI;
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

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HelpMenu : Page
    {
        AppInsights insights = new AppInsights();
        #region Constructor
        public HelpMenu()
        {
            try
            {
            this.InitializeComponent();
            if (UtilitiesResources.ShowAdRotator != false && AppResources.ShowAdControl != true)
            {
                LoadAdds.AdControlForPro_New(LayoutRoot, adstackpl);
            }
            else
            {
                    LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 2);
            }
            Loaded += new RoutedEventHandler(Help_Loaded);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in HelpMenu Method In HelpMenu.cs file.", ex);
            }
        }
        #endregion

        #region PageLoad
        void Help_Loaded(object sender, RoutedEventArgs e)
        {
            insights.Event("Help view");   
            lbxHelpMenu.ItemsSource = OnlineShow.LoadMenuList();
            LoadAds();
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
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //HardwareButtons.BackPressed+=HardwareButtons_BackPressed;
            try
            {
             
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In HelpMenu.cs file.", ex);
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

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
              
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom Method In HelpMenu.cs file.", ex);
            }
        }

        private void lbxHelpMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxHelpMenu.SelectedIndex != -1)
                {
                    MenuProperties selectedMenuItem = (lbxHelpMenu.SelectedItem as MenuProperties);                    
                    string[] parameters = new string[2];
                    parameters[0] = selectedMenuItem.Id;
                    parameters[1] = selectedMenuItem.Url;
                   
                    Frame.Navigate(typeof(Help), parameters);
                }
                insights.Event("Help View");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxHelpMenu_SelectionChanged Method In HelpMenu.cs file.", ex);
            }
        }

        private void imgTitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {          
            Frame.Navigate(typeof(MainPage));
        }
    }
}
