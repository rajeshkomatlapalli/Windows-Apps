using Common.Library;
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
using System.Reflection;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class ParentalControl : UserControl
    {
        bool check = false;
        List<CatageoryTable> ShowCategoryList = null;
        public ParentalControl()
        {
            try
            {
            this.InitializeComponent();
            ShowCategoryList = new List<CatageoryTable>();
            progressbar.IsActive = true;
            progressbar.IsActive = true;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ViewAllList Method In ViewAllList.cs file", ex);
            }
        }

        private void ImageTextCollectionGrid_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            Page q = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
            q.GetType().GetTypeInfo().GetDeclaredMethod("ShowVideoList").Invoke(q, null);
        }

        private void ImageTextCollectionGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                GridView g = (GridView)sender;
                if (g.SelectedIndex == -1)
                    return;
                var selectedItem = (sender as Selector).SelectedItem as ShowList;
                AppSettings.ShowID = selectedItem.ShowID.ToString();
                AppSettings.ShowsIsHidden = selectedItem.IsHidden.ToString();
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("changeimage").Invoke(p, null);

                g.SelectedIndex = -1;

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ImageTextCollectionGrid_SelectionChanged_1 Method In ViewAllList.cs file", ex);
            }
        }

        private void TextBlock_RightTapped_1(object sender, RightTappedRoutedEventArgs e)
        {
            try
            {
                check = true;
                Constants.appbarvisible = true;
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("appbar").Invoke(p, new object[] { true });
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in tblkChapter_RightTapped_1 Method In ShowVideos.cs file", ex);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GetPageDataInBackground();

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ViewAllList_Loaded_1 Method In ViewAllList.cs file", ex);
            }
        }

        void lstcombobox_DropDownClosed(object sender, object e)
        {
            try
            {
                //lstcombobox.Foreground = new SolidColorBrush(Colors.White);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstcombobox_DropDownClosed Method In ViewAllList.cs file", ex);
            }
        }

        void lstcombobox_DropDownOpened(object sender, object e)
        {
            try
            {
                //lstcombobox.Foreground = new SolidColorBrush(Colors.Black);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstcombobox_DropDownOpened Method In ViewAllList.cs file", ex);
            }
        }


        private void LoadData(BackgroundHelper bwHelper)
        {
            try
            {

                bwHelper.AddBackgroundTask(
                                     (object s, DoWorkEventArgs a) =>
                                     {
                                         a.Result = OnlineShow.GetperentList();
                                     },
                                     (object s, RunWorkerCompletedEventArgs a) =>
                                     {

                                         ImageTextCollectionGrid.ItemsSource = (List<ShowList>)a.Result;
                                         progressbar.IsActive = false;
                                     }
                                   );
            }

            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstcombobox_DropDownOpened Method In ViewAllList.cs file", ex);
            }


        }
        private void GetPageDataInBackground()
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();
                if (AppSettings.CategoryVisible != "false")
                {

                    bwHelper.AddBackgroundTask(
                                                (object s, DoWorkEventArgs a) =>
                                                {

                                                    a.Result = OnlineShow.LoadCategoryList();
                                                },
                                                (object s, RunWorkerCompletedEventArgs a) =>
                                                {
                                                    ShowCategoryList = (List<CatageoryTable>)a.Result;

                                                }
                                              );
                }
                LoadData(bwHelper);
                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In ViewAllList.cs file", ex);
            }
        }

    }
}
