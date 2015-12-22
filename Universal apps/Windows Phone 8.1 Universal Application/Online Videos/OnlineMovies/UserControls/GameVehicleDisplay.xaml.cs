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
    public sealed partial class GameVehicleDisplay : UserControl
    {
        #region GlobalDeclaration
        private bool IsDataLoaded;
        List<VideoGameProperties> ShowVehicleList = null;
        #endregion

        public GameVehicleDisplay()
        {
            this.InitializeComponent();
            ShowVehicleList = new List<VideoGameProperties>();
            IsDataLoaded = false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsDataLoaded == false)
            {
                MyProgressBar1.IsIndeterminate = true;
                GetPageDataInBackground();
                IsDataLoaded = true;
            }
        }

        private void GetPageDataInBackground()
        {
            BackgroundHelper bwHelper = new BackgroundHelper();

            bwHelper.AddBackgroundTask(
                                        (object s, System.ComponentModel.DoWorkEventArgs a) =>
                                        {
                                            a.Result = OnlineShow.GetVehicleList(AppSettings.ShowUniqueID);
                                        },
                                        (object s, RunWorkerCompletedEventArgs a) =>
                                        {
                                            ShowVehicleList = (List<VideoGameProperties>)a.Result;
                                            if (ShowVehicleList.Count > 0)
                                            {
                                                lbxVehicle.ItemsSource = ShowVehicleList;
                                                tblkVehicle.Visibility = Visibility.Collapsed;
                                            }
                                            else
                                            {
                                                tblkVehicle.Text = "No Vehicles available";
                                                tblkVehicle.Visibility = Visibility.Visible;
                                            }
                                            MyProgressBar1.IsIndeterminate = false;
                                        }
                                      );
            bwHelper.RunBackgroundWorkers();
        }

        private void lbxVehicle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PageHelper.NavigateTo(NavigationHelper.getVideoGameDescriptionPage((lbxVehicle.SelectedItem as VideoGameProperties).Id.ToString(), AppSettings.ShowID, "Vehicle"));
        }
    }
}
