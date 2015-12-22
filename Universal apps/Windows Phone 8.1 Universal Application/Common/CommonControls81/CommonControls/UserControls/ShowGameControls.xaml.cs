using Common.Library;
using OnlineVideos.Entities;
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
using OnlineVideos.Data;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class ShowGameControls : UserControl
    {
        public ShowGameControls()
        {
            this.InitializeComponent();
            progressbar.IsActive = true;
            Loaded += ShowGameControls_Loaded;
        }

        void ShowGameControls_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<GameControls> objwaplist = new List<GameControls>();
                objwaplist = OnlineShow.GetGameControls(AppSettings.ShowID);
                if (objwaplist.Count != 0)
                {
                    progressbar.IsActive = false;
                    lstvcontrols.ItemsSource = objwaplist;
                }
                else
                {
                    progressbar.IsActive = false;
                    txtmsg.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowGameVehicles_Loaded Method In ShowGameVehicles.cs file", ex);
            }
        }

        private void lstvcontrols_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstvcontrols.SelectedIndex == -1)
                    return;
                AppSettings.GameClassName = "Controle";
                AppSettings.GameGalCount = (sender as Selector).SelectedIndex.ToString();
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("ControlPopUp").Invoke(p, new object[] { false });
                lstvcontrols.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstvvehicle_SelectionChanged_1 Method In ShowGameVehicles.cs file", ex);
            }
            return;
        }
    }
}
