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
using System.Reflection;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class GalleryPopup : UserControl
    {
        public GalleryPopup()
        {
            try
            {
            this.InitializeComponent();
            progressbar.IsActive = true;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GalleryPopup Method In GalleryPopup.cs file", ex);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                // Customization objcustom = new Customization();

                flpvwImageBind.ItemsSource = ShowCastManager.Loadpopupimages(AppSettings.PersonID);
                progressbar.IsActive = false;
                flpvwImageBind.SelectedIndex = Convert.ToInt32(AppSettings.GallCount1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GalleryPopup_Loaded_1 Method In GalleryPopup.cs file", ex);
            }
        }

        private void imgclose_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                flpvwImageBind.Visibility = Visibility.Collapsed;
                imgclose.Visibility = Visibility.Collapsed;
                LayoutRoot.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in imgclose_Tapped Method In GalleryPopup.cs file", ex);
            }
        }

        private void descpopup_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            AppSettings.PopupGridTap = "false";

        }
    }
}
