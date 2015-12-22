using Common.Library;
using OnlineVideos.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Indian_Cinema.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GalleryPopup : Page
    {
        public static int ImageNo = 0;
        public GalleryPopup()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                flpvwImageBind.ItemsSource = ShowCastManager.Loadpopupimages(AppSettings.PersonID);
                if (AppSettings.GallCount1 != "0")
                    flpvwImageBind.SelectedIndex = Convert.ToInt32(AppSettings.GallCount1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GalleryPopup_Loaded_1  event In GalleryPopup", ex);
            }
        }

        private void imgclose_Tapped(object sender, TappedRoutedEventArgs e)
        {
              flpvwImageBind.Visibility = Visibility.Collapsed;
              imgclose.Visibility = Visibility.Collapsed;
              LayoutRoot.Visibility = Visibility.Collapsed;          
        }
    }
}
