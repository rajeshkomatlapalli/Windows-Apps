using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Common;
using OnlineVideos.Data;
using Common.Library;
using Common.Utilities;
using OnlineVideos.Entities;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideos.Controls
{
    public sealed partial class ShowCast : UserControl
    {

        string ShowId = AppSettings.ShowID;
        public ShowCast()
        {
            this.InitializeComponent();
            Loaded += ShowCast_Loaded;
        }

       async void ShowCast_Loaded(object sender, RoutedEventArgs e)
        {

            lbxCast.ItemsSource = await OnlineShow.GetCastSection(ShowId);
        }

       private void lbxCast_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (lbxCast.SelectedIndex == -1)
                return;
            AppSettings.PersonID = (lbxCast.SelectedItem as CastRole).PersonID.ToString();
           
            //PageHelper.NavigateToCastDetailPage(AppResources.CastDetailPageName,null);
            lbxCast.SelectedIndex = -1;
            //var rootFrame = new Frame();
            // rootFrame.Navigate(typeof(MainPage));
            // Window.Current.Content = rootFrame;
            // Window.Current.Activate();
            

        }
    }
}
