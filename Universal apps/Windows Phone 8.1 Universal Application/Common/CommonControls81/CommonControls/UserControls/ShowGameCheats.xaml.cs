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
    public sealed partial class ShowGameCheats : UserControl
    {
        public ShowGameCheats()
        {
            try
            {
            this.InitializeComponent();
            progressbar.IsActive = true;
            Loaded += ShowGameCheats_Loaded;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowGameCheats Method In ShowGameCheats.cs file", ex);
            }
        }

        void ShowGameCheats_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<GameCheats> objwaplist = new List<GameCheats>();
                objwaplist = OnlineShow.GetGameCheats();
                if (objwaplist.Count != 0)
                {
                    progressbar.IsActive = false;
                    //if (objwaplist.FirstOrDefault().Description != null && objwaplist.FirstOrDefault().Description != "")
                    //    tblkname.Text = objwaplist.FirstOrDefault().Name;
                    //tblkdescription.Text = objwaplist.FirstOrDefault().Description;
                    lstvwcheats.ItemsSource = objwaplist;
                }
                else
                {
                    progressbar.IsActive = false;
                    txtmsg.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowGameCheats_Loaded Method In ShowGameCheats.cs file", ex);
            }
        }

        private void lstvwcheats_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstvwcheats.SelectedIndex == -1)
                    return;
                //if (popgallimages.IsOpen == false)
                //{
                //var selectedItem = (sender as Selector).SelectedItem as GalleryImageInfo;
                AppSettings.GameClassName = "Cheats";
                AppSettings.GameGalCount = (sender as Selector).SelectedIndex.ToString();
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("CheatcodePoUp").Invoke(p, new object[] { false });
                lstvwcheats.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstvwcheats_SelectionChanged_1 Method In ShowGameCheats.cs file", ex);
            }
        }

        private void tblkChapter_RightTapped_1(object sender, RightTappedRoutedEventArgs e)
        {
            try
            {
                AppSettings.GamePopUpVisible = "true";
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in tblkChapter_RightTapped_1 Method In ShowGameCheats.cs file", ex);
            }
        }

    }
}
