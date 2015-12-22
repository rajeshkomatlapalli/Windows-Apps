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
    public sealed partial class ShowGameWeapons : UserControl
    {
        public static ShowGameWeapons current;
        GameWeapons selecteditem1 = null;
        //Popup popgallimages;
        public ShowGameWeapons()
        {
            try
            {
                current = this;
                this.InitializeComponent();
                progressbar.IsActive = true;
                //popgallimages = new Popup();

                Loaded += GameWeapons_Loaded;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowGameWeapons Method In ShowGameWeapons.cs file", ex);
            }
        }

        void GameWeapons_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadWeapons();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GameWeapons_Loaded Method In ShowGameWeapons.cs file", ex);
            }
        }

        private void LoadWeapons()
        {
            try
            {
                List<GameWeapons> objwaplist = new List<GameWeapons>();
                objwaplist = OnlineShow.GetGameWeapons(AppSettings.ShowID);
                if (objwaplist.Count != 0)
                {
                    if (objwaplist.FirstOrDefault().Description != null && objwaplist.FirstOrDefault().Description != "")
                    // tblkname.Text = objwaplist.FirstOrDefault().Name;
                    // tblkdescription.Text = objwaplist.FirstOrDefault().Description;
                    {
                        progressbar.IsActive = false;
                        lstvwwapons.ItemsSource = objwaplist;
                    }
                }
                else
                {
                    progressbar.IsActive = false;
                    txtmsg.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadWeapons Method In ShowGameWeapons.cs file", ex);
            }
        }

        private void lstvwwapons_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstvwwapons.SelectedIndex == -1)
                    return;

                AppSettings.GameClassName = "Weapon";
                AppSettings.GameGalCount = (sender as Selector).SelectedIndex.ToString();
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("DescriptionPopup").Invoke(p, new object[] { false });
                lstvwwapons.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstvwwapons_SelectionChanged_1 Method In ShowGameWeapons.cs file", ex);
            }
            return;
            // tblkdescription.Text = (lstvwwapons.SelectedItem as GameWeapons).Description;
        }
    }
}
