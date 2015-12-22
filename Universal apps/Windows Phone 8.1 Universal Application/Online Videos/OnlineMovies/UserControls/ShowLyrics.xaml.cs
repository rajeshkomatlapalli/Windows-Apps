using Common.Library;
using Common.Utilities;
using OnlineVideos.Data;
using OnlineVideos.Entities;
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
    public sealed partial class ShowLyrics : UserControl
    {
        List<ShowLinks> objLiricsList;
        public ShowLyrics()
        {
            this.InitializeComponent();
            objLiricsList = new List<ShowLinks>();
        }

        private void lbxLyricsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxLyricsList.SelectedIndex == -1)
                return;
            AppSettings.LiricsLink = (lbxLyricsList.SelectedItem as ShowLinks).Title;
            AppSettings.LiricsType = (lbxLyricsList.SelectedItem as ShowLinks).LinkType;
            PageHelper.NavigateToLiricsShowPage(AppResources.LiricsDetailPageName, AppSettings.LiricsLink);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GetPageDataInBackground();
        }

        private void GetPageDataInBackground()
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();

                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                a.Result = OnlineShow.GetShowLinksByType(AppSettings.ShowID, LinkType.Audio/*, false*/);
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                objLiricsList = (List<ShowLinks>)a.Result;
                                                if (objLiricsList.Count > 0)
                                                {
                                                    lbxLyricsList.ItemsSource = objLiricsList;
                                                    tblkFavNoLyrics.Visibility = Visibility.Collapsed;
                                                }
                                                else
                                                {
                                                    tblkFavNoLyrics.Text = "No " + AppResources.ShowDetailPageLiricsPivotTitle + " available";
                                                    tblkFavNoLyrics.Visibility = Visibility.Visible;
                                                }
                                            }
                                          );

                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In ShowLyrics file.", ex);
            }
        }
    }
}
