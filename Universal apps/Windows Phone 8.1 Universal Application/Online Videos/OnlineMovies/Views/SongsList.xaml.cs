using Common.Library;
using Common.Utilities;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SongsList : Page
    {
        #region Global
        string id = "";
        List<ShowLinks> objsonglist = new List<ShowLinks>();
        #endregion

        #region Constructor
        public SongsList()
        {
            this.InitializeComponent();
            tblkVideosTitle.Text = OnlineShow.GetShowDetail(long.Parse(AppSettings.ShowID)).Title;
        }
        #endregion
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
              //  FlurryWP8SDK.Api.LogPageView();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo event In SongsList.cs file.", ex);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
                //FlurryWP8SDK.Api.EndTimedEvent("SongsList Page");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom event In SongsList.cs file.", ex);
            }
        }        

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ListBoxItem selectedListBoxItem = this.lbxRemoveList.ItemContainerGenerator.ContainerFromItem((sender as MenuFlyoutItem).DataContext) as ListBoxItem;
                if (selectedListBoxItem == null)
                    return;
                string ChapterNo = (selectedListBoxItem.Content as ShowLinks).LinkOrder.ToString();
                OnlineShow.SaveRemoveSongs(id, ChapterNo);

                lbxRemoveList.ItemsSource = OnlineShow.GetRemoveShowList(id);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in MenuItem_Click event In SongsList.cs file.", ex);
            }
        }

        #region PageLoad
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAds();
            try
            {
              //  FlurryWP8SDK.Api.LogEvent("SongsList Page", true);               
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded event In SongsList.cs file.", ex);
            }
        }
        #endregion

        private void LoadAds()
        {
            try
            {
               // LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 2);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In SongDetails file", ex);
                string excepmess = "Exception in LoadAds Method In SongDetails file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }

        private void imgTitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {            
            Frame.Navigate(typeof(MainPage));
        }
    }
}
