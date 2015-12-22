using Common.Library;
using Common.Utilities;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideos.UserControls
{
    public sealed partial class GameWeaponsDisplay : UserControl
    {
        #region GlobalDeclaration
        private bool IsDataLoaded;
        List<VideoGameProperties> ShowWeaponList = null;
        #endregion

        #region Constructor
        public GameWeaponsDisplay()
        {
            this.InitializeComponent();
            ShowWeaponList = new List<VideoGameProperties>();
            IsDataLoaded = false;
        }
        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsDataLoaded == false)
            {
                MyProgressBar1.IsIndeterminate = true;
                GetPageDataInBackground();
                IsDataLoaded = true;
            }
        }
        #region "Common Methods"
        private void GetPageDataInBackground()
        {
            BackgroundHelper bwHelper = new BackgroundHelper();

            bwHelper.AddBackgroundTask(
                                        (object s, DoWorkEventArgs a) =>
                                        {
                                            a.Result = OnlineShow.GetWeaponsList(AppSettings.ShowUniqueID);
                                        },
                                        (object s, RunWorkerCompletedEventArgs a) =>
                                        {
                                            ShowWeaponList = (List<VideoGameProperties>)a.Result;
                                            if (ShowWeaponList.Count > 0)
                                            {
                                                lbxWeapon.ItemsSource = ShowWeaponList;
                                                tblkWeapon.Visibility = Visibility.Collapsed;
                                            }
                                            else
                                            {
                                                tblkWeapon.Text = "No Weapons available";
                                                tblkWeapon.Visibility = Visibility.Visible;
                                            }
                                            MyProgressBar1.IsIndeterminate = false;
                                        }
                                      );
            bwHelper.RunBackgroundWorkers();
        }
        #endregion

        private void lbxWeapon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PageHelper.NavigateTo(NavigationHelper.getVideoGameDescriptionPage((lbxWeapon.SelectedItem as VideoGameProperties).Id.ToString(), AppSettings.ShowID, "Weapon"));
        }
    }
}
