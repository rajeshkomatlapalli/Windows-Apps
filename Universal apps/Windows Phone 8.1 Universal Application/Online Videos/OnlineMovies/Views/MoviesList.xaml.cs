using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MoviesList : Page
    {
        string[] a4;

        public MoviesList()
        {
            this.InitializeComponent();
            if (ResourceHelper.AppName != Apps.Indian_Cinema_Pro.ToString() && ResourceHelper.AppName != Apps.Kids_TV_Pro.ToString() && ResourceHelper.AppName != Apps.Story_Time_Pro.ToString())
                LoadAds();
            a4 = Application.Current.Resources["ContextMenu"].ToString().Split(',');
            tblkVideosTitle.Text = a4[2].ToString();
        }

        #region "Common Methods"
        private void LoadAds()
        {
            try
            {
                //LoadAdds.LoadAdControl(LayoutRoot, adstackpl, 2);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In SongDetails file", ex);
                string excepmess = "Exception in LoadAds Method In SongDetails file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }
        #endregion
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In MoviesList.cs file.", ex);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom Method In MoviesList.cs file.", ex);
            }
        }

        private void imgTitle_Tapped(object sender, TappedRoutedEventArgs e)
        {           
            Frame.Navigate(typeof(MainPage));
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ListBoxItem selectedListBoxItem = this.lbxRemoveList.ItemContainerGenerator.ContainerFromItem((sender as MenuFlyoutItem).DataContext) as ListBoxItem;
                if (selectedListBoxItem == null)
                    return;
                OnlineShow.SaveRemoveVideos((selectedListBoxItem.Content as ShowList).ShowID);
                lbxRemoveList.ItemsSource = OnlineShow.GetperentList();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in MenuItem_Click Method In MoviesList.cs file.", ex);
            }
        }

        private void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                ListBoxItem selectedListBoxItem = this.lbxRemoveList.ItemContainerGenerator.ContainerFromItem((sender as MenuFlyoutItem).DataContext) as ListBoxItem;
                if (selectedListBoxItem == null)
                    return;
                AppSettings.ShowID = (selectedListBoxItem.Content as ShowList).ShowID.ToString();
               // Frame.Navigate(typeof(SongsList), "mid=" + AppSettings.ShowID);              
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in MenuItem1_Click Method In MoviesList.cs file.", ex);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {                
                _performanceProgressBar.IsIndeterminate = true;
                BackgroundHelper bwHelper = new BackgroundHelper();
                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                a.Result = OnlineShow.GetperentList();
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                lbxRemoveList.ItemsSource = (List<ShowList>)a.Result;
                                                _performanceProgressBar.IsIndeterminate = false;
                                            }
                                          );
                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In MoviesList.cs file.", ex);
            }
        }
    }
}
