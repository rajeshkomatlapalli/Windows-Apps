using Common.Library;
using OnlineVideos.Data;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class ShowGameVehicles : UserControl
    {
        public ShowGameVehicles()
        {
            try
            {
                //popgallimages = new Popup();
                this.InitializeComponent();
                progressbar.IsActive = true;
                Loaded += ShowGameVehicles_Loaded;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowGameVehicles Method In ShowGameVehicles.cs file", ex);
            }
        }

        void ShowGameVehicles_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<GameVehicles> objwaplist = new List<GameVehicles>();
                objwaplist = OnlineShow.GetGameVehicle(AppSettings.ShowID);
                if (objwaplist.Count != 0)
                {
                    progressbar.IsActive = false;
                    //if (objwaplist.FirstOrDefault().Description != null && objwaplist.FirstOrDefault().Description != "")
                    //    tblkname.Text = objwaplist.FirstOrDefault().Name;
                    //tblkdescription.Text = objwaplist.FirstOrDefault().Description;
                    lstvvehicle.ItemsSource = objwaplist;
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

        private void lstvvehicle_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstvvehicle.SelectedIndex == -1)
                    return;
                //if (popgallimages.IsOpen == false)
                //{
                //var selectedItem = (sender as Selector).SelectedItem as GalleryImageInfo;
                AppSettings.GameClassName = "Vehicle";
                AppSettings.GameGalCount = (sender as Selector).SelectedIndex.ToString();
                //Page p = (Page)PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                //popgallimages.Child = (UIElement)p.GetType().GetTypeInfo().GetDeclaredMethod("Popup").Invoke(p, null);
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("DescriptionPopup").Invoke(p, new object[] { false });
                //popgallimages.IsOpen = true;
                //popgallimages.Height = 1000;
                lstvvehicle.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstvvehicle_SelectionChanged_1 Method In ShowGameVehicles.cs file", ex);
            }
            return;
        }

    }
}
