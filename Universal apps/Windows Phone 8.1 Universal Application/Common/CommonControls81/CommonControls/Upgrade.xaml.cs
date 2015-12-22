using Common.Library;
using OnlineVideos;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace CommonControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Upgrade : Page
    {
        public Upgrade()
        {
            this.InitializeComponent();
            Loaded += new RoutedEventHandler(Upgrade_Loaded);
            imgtitle.Source = ResourceHelper.getPivotTitle();
        }

        void Upgrade_Loaded(object sender, RoutedEventArgs e)
        {
            LayoutRoot.Background = ImageHelper.LoadPivotBackground();
            lbxupgrade.ItemsSource = OnlineShow.GetUpgrade();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void imgtitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void getapp_Click(object sender, RoutedEventArgs e)
        {
            if(AppResources.ShowUpgradePage)
            {
                WebView w = new WebView();
                //WebBrowserTask w = new WebBrowserTask();
                //w.Uri = AppResources.UpgradeAppLink;
                //w.Show();
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}