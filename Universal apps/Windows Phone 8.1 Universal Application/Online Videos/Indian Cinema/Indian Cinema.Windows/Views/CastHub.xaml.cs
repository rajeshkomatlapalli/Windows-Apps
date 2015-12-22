//using AdRotator;
using Common.Library;
using Indian_Cinema.Views;
using OnlineVideos.Data;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ApplicationSettings;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CastHub : Page
    {
        AppInsights insights = new AppInsights();
        string PersonID = AppSettings.PersonID;
        public CastHub()
        {
            this.InitializeComponent();
            Loaded+=CastHub_Loaded;
        }

        void CastHub_Loaded(object sender, RoutedEventArgs e)
        {
            insights.Event("CastHub Loaded");
            Window.Current.CoreWindow.PointerWheelChanged += CoreWindow_PointerWheelChanged;
            //AdRotatorWin8.Invalidate();
            Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
            //AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
            //adcontrol.IsAdRotatorEnabled = true;
            //adcontrol.Visibility = Visibility.Visible;
        }

        private void CoreWindow_PointerWheelChanged(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.PointerEventArgs args)
        {
            if (args.CurrentPoint.Properties.MouseWheelDelta == (-120))
            {
                //MouseWheel Backward scroll
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset + Window.Current.CoreWindow.Bounds.Width / 10);
            }
            if (args.CurrentPoint.Properties.MouseWheelDelta == (120))
            {
                //MouseWheel Forward scroll
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - Window.Current.CoreWindow.Bounds.Width / 10);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                insights.pageview("CastHub page");
                tblkTitle.Text = ShowCastManager.GetPersonDetail(AppSettings.PersonID).Name.ToString();
                insights.Event(tblkTitle.Text + "Gallery view");
                LayoutRoot.Background = OnlineShow.LoadPanoramCastBackground(PersonID);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In CastHub page", ex);
                insights.Exception(ex);
            }
        }

        public void DetailPage()
        {
            App.rootFrame.Navigate(typeof(Detail));
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }
        public void GalleryPopup()
        {
            App.rootFrame.Navigate(typeof(GalleryPopup));
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }
        public GalleryPopup Popup()
        {
            GalleryPopup gp = new GalleryPopup();
            return gp;
        }
        public void Gallerypoup()
        {
            flpvwImageBind1.ItemsSource = ShowCastManager.Loadpopupimages(AppSettings.PersonID);
            flpvwImageBind1.SelectedIndex = Convert.ToInt32(AppSettings.GallCount1);
            LayoutRoot1.Visibility = Visibility.Visible;
        }

        private void imgclose1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            flpvwImageBind1.SelectedIndex = -1;
            LayoutRoot1.Visibility = Visibility.Collapsed;
        }
        private void CastHub_Loaded_1(object sender, RoutedEventArgs e)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += onCommandsRequested;
        }

        private void onCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            AddControlvisable1.Visibility = Visibility.Collapsed;
        }

        private void imgclose_Tapped(object sender, TappedRoutedEventArgs e)
        {
            flpvwImageBind1.SelectedIndex = -1;

            LayoutRoot1.Visibility = Visibility.Collapsed;
        }

        private void Backbutton_Click(object sender, RoutedEventArgs e)
        {
            persongallery.ClosePopUp();
            if (App.rootFrame.CanGoBack)
                App.rootFrame.GoBack();
        }
    }
}