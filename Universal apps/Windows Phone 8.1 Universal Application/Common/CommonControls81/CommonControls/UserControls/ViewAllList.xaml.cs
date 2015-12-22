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
using Windows.UI.ApplicationSettings;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Reflection;
using Windows.UI;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class ViewAllList : UserControl
    {
        int catid = 0;
        bool check = false;
        public static ViewAllList current = null;
        List<CatageoryTable> ShowCategoryList = null;
        List<ShowList> list = new List<ShowList>();
        List<ShowList> SortList = new List<ShowList>();
        int id = 1;
        public ViewAllList()
        {
            try
            {
            this.InitializeComponent();
            this.Tag = this;
            current = this;
            ShowCategoryList = new List<CatageoryTable>();
            progressbar.IsActive = true;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ViewAllList Method In ViewAllList.cs file", ex);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();
                if (lstcombobox.SelectedIndex > 0)
                {
                    progressbar.IsActive = true;
                    ImageTextCollectionGrid.ItemsSource = null;
                    var selectedItem = (sender as Selector).SelectedItem as CatageoryTable;
                    LoadcatgeoryList(bwHelper, selectedItem.CategoryID.ToString());
                    bwHelper.RunBackgroundWorkers();
                }
                else
                {
                    progressbar.IsActive = true;
                    ImageTextCollectionGrid.ItemsSource = null;
                    LoadData(bwHelper);
                    bwHelper.RunBackgroundWorkers();
                }

                if (lstcombobox.SelectedIndex == -1)
                    return;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ComboBox_SelectionChanged Method In ViewAllList.cs file", ex);
            }
        }

        private void LoadcatgeoryList(BackgroundHelper bwHelper, string cid)
        {
            try
            {
                advisible.Visibility = Visibility.Collapsed;
                if (catid == 0)
                {
                    foreach (CategoryId x in Enum.GetValues(typeof(CategoryId)))
                    {
                        if (AppSettings.ViewAllTitle.Contains(x.ToString()))
                        {
                            catid = (int)x;
                            break;
                        }
                    }
                }
                bwHelper.AddBackgroundTask(
                                              (object s, DoWorkEventArgs a) =>
                                              {
                                                  a.Result = OnlineShow.GetCategoryIdByShows(cid, catid);
                                              },
                                              (object s, RunWorkerCompletedEventArgs a) =>
                                              {
                                                  list = (List<ShowList>)a.Result;
                                                  if (list.Count() == 1)
                                                  {
                                                      vewbox.MaxWidth = 550;
                                                      vewbox.MaxHeight = 300;
                                                      // ImageTextCollectionGrid.ItemTemplate = AppResources.ImageTextCollectionTemplateSingleItem;
                                                  }
                                                  else
                                                  {
                                                      vewbox.MaxWidth = Double.MaxValue;
                                                      vewbox.MaxHeight = Double.MaxValue;
                                                      vewbox.Width = Double.NaN;
                                                      vewbox.Height = Double.NaN;
                                                  }
                                                  ImageTextCollectionGrid.ItemsSource = (List<ShowList>)a.Result;
                                                  progressbar.IsActive = false;
                                                  if (AppResources.advisible == true)
                                                  {
                                                      advisible.Visibility = Visibility.Visible;
                                                  }

                                              }
                                            );
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadcatgeoryList Method In ViewAllList.cs file", ex);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Window.Current.CoreWindow.PointerWheelChanged += CoreWindow_PointerWheelChanged;
                SettingsPane.GetForCurrentView().CommandsRequested += onCommandsRequested;
                if (AppSettings.ProjectName == "DownloadManger")
                {
                    lstcombobox.Visibility = Visibility.Collapsed;
                }
                if (AppSettings.ProjectName == "Recipe Shows" || AppSettings.ProjectName == "Indian Cinema" || AppSettings.ProjectName=="Indian_Cinema" || AppSettings.ProjectName == "Video Games" || AppSettings.ProjectName == "Bollywood Music" || AppSettings.ProjectName == "Kids TV Shows" || AppSettings.ProjectName == "Kids TV Pro" || AppSettings.ProjectName == "Indian Cinema Pro" || AppSettings.ProjectName == "Online Education.Windows")
                {
                    fstviewbox.Visibility = Visibility.Visible;
                }
                else
                {
                    LoadSortShows();
                    SortMenu.Visibility = Visibility.Visible;
                    lstcombobox.Visibility = Visibility.Visible;
                    fstviewbox.Visibility = Visibility.Collapsed;
                }
                GetPageDataInBackground();
                lstcombobox.DropDownOpened += lstcombobox_DropDownOpened;
                lstcombobox.DropDownClosed += lstcombobox_DropDownClosed;
                SortMenu.Visibility = Visibility.Visible;
              
                if (AppResources.advisible == true)
                {
                //    AdControl.ApplicationId = AppResources.adApplicationId;
                //    AdControl.AdUnitId = AppResources.adUnitId;
                  
                }
            }

            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ViewAllList_Loaded_1 Method In ViewAllList.cs file", ex);
            }
        }

         private void onCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            advisible.Visibility = Visibility.Collapsed;
        }

        void lstcombobox_DropDownClosed(object sender, object e)
        {
            try
            {
                lstcombobox.Foreground = new SolidColorBrush(Colors.White);
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
                lstcombobox.Foreground = new SolidColorBrush(Colors.Black);
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
                advisible.Visibility = Visibility.Collapsed;
                if (AppSettings.ViewAllTitle == Constants.Hindi || AppSettings.ViewAllTitle == Constants.toprated || AppSettings.ViewAllTitle==Constants.DownloadList)
                {
                    bwHelper.AddBackgroundTask(
                                         (object s, DoWorkEventArgs a) =>
                                         {
                                             a.Result = OnlineShow.GetTopratedListShows();
                                         },
                                         (object s, RunWorkerCompletedEventArgs a) =>
                                         {
                                             vewbox.MaxWidth = Double.MaxValue;
                                             vewbox.MaxHeight = Double.MaxValue;
                                             vewbox.Width = Double.NaN;
                                             vewbox.Height = Double.NaN;
                                             ImageTextCollectionGrid.ItemsSource = (List<ShowList>)a.Result;
                                             progressbar.IsActive = false;
                                             AppSettings.CategoryIDForCompare = "";
                                             if (AppResources.advisible == true)
                                             {
                                                 advisible.Visibility = Visibility.Visible;
                                             }
                                         }
                                       );
                }
                else if (AppSettings.ViewAllTitle == Constants.Telugu || AppSettings.ViewAllTitle == Constants.Recent || AppSettings.ViewAllTitle == Constants.DownloadList)
                {
                    advisible.Visibility = Visibility.Collapsed;
                    bwHelper.AddBackgroundTask(
                                        (object s, DoWorkEventArgs a) =>
                                        {
                                            a.Result = OnlineShow.GetRecentListShows();
                                        },
                                        (object s, RunWorkerCompletedEventArgs a) =>
                                        {
                                            vewbox.MaxWidth = Double.MaxValue;
                                            vewbox.MaxHeight = Double.MaxValue;
                                            vewbox.Width = Double.NaN;
                                            vewbox.Height = Double.NaN;
                                            ImageTextCollectionGrid.ItemsSource = (List<ShowList>)a.Result;
                                            progressbar.IsActive = false;
                                            //AppSettings.CategoryIDForCompare = "17";
                                            if (AppResources.advisible == true)
                                            {
                                                advisible.Visibility = Visibility.Visible;
                                            }
                                        }
                                      );
                }
                else if (AppSettings.ViewAllTitle == Constants.Tamil)
                {
                    advisible.Visibility = Visibility.Collapsed;
                    bwHelper.AddBackgroundTask(
                                      (object s, DoWorkEventArgs a) =>
                                      {
                                          a.Result = OnlineShow.GetTamilListShows();
                                      },
                                      (object s, RunWorkerCompletedEventArgs a) =>
                                      {
                                          vewbox.MaxWidth = Double.MaxValue;
                                          vewbox.MaxHeight = Double.MaxValue;
                                          vewbox.Width = Double.NaN;
                                          vewbox.Height = Double.NaN;
                                          ImageTextCollectionGrid.ItemsSource = (List<ShowList>)a.Result;
                                          progressbar.IsActive = false;
                                          //AppSettings.CategoryIDForCompare = "18";
                                          if (AppResources.advisible == true)
                                          {
                                              advisible.Visibility = Visibility.Visible;
                                          }
                                      }
                                    );
                }
                else
                {
                    advisible.Visibility = Visibility.Collapsed;
                    bwHelper.AddBackgroundTask(
                                     (object s, DoWorkEventArgs a) =>
                                     {
                                         a.Result = OnlineShow.GetUpcomingMoviesListShows();
                                     },
                                     (object s, RunWorkerCompletedEventArgs a) =>
                                     {
                                         vewbox.MaxWidth = Double.MaxValue;
                                         vewbox.MaxHeight = Double.MaxValue;
                                         vewbox.Width = Double.NaN;
                                         vewbox.Height = Double.NaN;
                                         ImageTextCollectionGrid.ItemsSource = (List<ShowList>)a.Result;
                                         progressbar.IsActive = false;
                                         AppSettings.CategoryIDForCompare = "18";
                                         if (AppResources.advisible == true)
                                         {
                                             advisible.Visibility = Visibility.Visible;
                                         }
                                     }
                                   );
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstcombobox_DropDownOpened Method In ViewAllList.cs file", ex);
            }
        }
        public void GetPageDataInBackground()
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();
                if (AppSettings.CategoryVisible != "")
                {
                    bwHelper.AddBackgroundTask(
                                                (object s, DoWorkEventArgs a) =>
                                                {
                                                    a.Result = OnlineShow.LoadCategoryList();
                                                },
                                                (object s, RunWorkerCompletedEventArgs a) =>
                                                {
                                                    ShowCategoryList = (List<CatageoryTable>)a.Result;
                                                    if (ShowCategoryList.Count > 0)
                                                    {
                                                        vewbox.MaxWidth = Double.MaxValue;
                                                        vewbox.MaxHeight = Double.MaxValue;
                                                        vewbox.Width = Double.NaN;
                                                        vewbox.Height = Double.NaN;
                                                        lstcombobox.ItemsSource = ShowCategoryList;
                                                        lstcombobox.SelectedIndex = 0;
                                                      
                                                        lstcombobox.Foreground = new SolidColorBrush(Colors.White);
                                                        lstcombobox.SelectionChanged += ComboBox_SelectionChanged;
                                                    }
                                                    LoadSortShows();
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

        public void LoadSortShows()
        {
            SortShowscombobox.ItemsSource = OnlineShow.LoadSortShowsForSortName();
           
            SortTitle.Visibility = Visibility.Visible;
            SortMenu.Visibility = Visibility.Visible;
            if (AppSettings.ViewAllTitle == "top rated >")
                SortShowscombobox.SelectedIndex = 1;
            else
                SortShowscombobox.SelectedIndex = 0;
           
            SortShowscombobox.SelectionChanged += SortShowscombobox_SelectionChanged_1;
            SortShowscombobox.Foreground = new SolidColorBrush(Colors.White);
            SortShowscombobox.DropDownOpened += SortShowscombobox_DropDownOpened;
            SortShowscombobox.DropDownClosed += SortShowscombobox_DropDownClosed;
        }

        private void SortShowscombobox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (catid == 0)
            {
                foreach (CategoryId x in Enum.GetValues(typeof(CategoryId)))
                {
                    if (AppSettings.ViewAllTitle.Contains(x.ToString()))
                    {
                        catid = (int)x;
                        break;
                    }
                }
            }
            try
            {
                var SelectedItem = (sender as Selector).SelectedItem as SortByShowsTable;
                if (SelectedItem != null)
                {
                    SortList = OnlineShow.GetSortByShows(SelectedItem.SortID, catid);
                    ImageTextCollectionGrid.ItemsSource = SortList;
                    vewbox.MaxWidth = Double.MaxValue;
                    vewbox.MaxHeight = Double.MaxValue;
                    vewbox.Width = Double.NaN;
                    vewbox.Height = Double.NaN;
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        void SortShowscombobox_DropDownClosed(object sender, object e)
        {
            SortShowscombobox.Foreground = new SolidColorBrush(Colors.White);
        }

        void SortShowscombobox_DropDownOpened(object sender, object e)
        {
            SortShowscombobox.Foreground = new SolidColorBrush(Colors.Black);
        } 

        private void CoreWindow_PointerWheelChanged(CoreWindow sender, PointerEventArgs args)
        {
            if (args.CurrentPoint.Properties.MouseWheelDelta == (-120))
            {
                //MouseWheel Backward scroll
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset + Window.Current.CoreWindow.Bounds.Width / 10);
            }
            if (args.CurrentPoint.Properties.MouseWheelDelta == (120))
            {
                //MouseWheel Forward scroll
                scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset - Window.Current.CoreWindow.Bounds.Width / 10);

            }
        }

        private void StackPanel_RightTapped_1(object sender, RightTappedRoutedEventArgs e)
        {
             check = true;
            //Constants.appbarvisible = true;
            Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
            p.GetType().GetTypeInfo().GetDeclaredMethod("apbar").Invoke(p, null);
        }

        private void view_PointerEntered_1(object sender, PointerRoutedEventArgs e)
        {
            //try
            //{

            //    Style FontStyle = Application.Current.Resources["PointerEnteredColor"] as Style;
            //    ((TextBlock)sender).Foreground = FontStyle.Setters.Cast<Setter>().Where(item => item.Property == TextBlock.ForegroundProperty).FirstOrDefault().Value as SolidColorBrush;
            //}
            //catch (Exception ex)
            //{
            //    Exceptions.SaveOrSendExceptions("Exception in view_PointerEntered_1 Method In Shows.cs file", ex);
            //}
        }

        private void view_PointerExited_1(object sender, PointerRoutedEventArgs e)
        {
            //try
            //{

            //    Style FontStyle = Application.Current.Resources["PointerExitedColor"] as Style;
            //    ((TextBlock)sender).Foreground = FontStyle.Setters.Cast<Setter>().Where(item => item.Property == TextBlock.ForegroundProperty).FirstOrDefault().Value as SolidColorBrush;
            //}
            //catch (Exception ex)
            //{
            //    Exceptions.SaveOrSendExceptions("Exception in view_PointerExited_1 Method In Shows.cs file", ex);
            //}
        }

        private void com_PointerEntered_1(object sender, PointerRoutedEventArgs e)
        {
            //Style FontStyle = Application.Current.Resources["PointerEnteredColor"] as Style;
          
            // ((TextBlock)sender).Foreground =FontStyle.Setters.Cast<Setter>().Where(item => item.Property == TextBlock.ForegroundProperty).FirstOrDefault().Value as SolidColorBrush;
        }

        private void com_PointerExited_1(object sender, PointerRoutedEventArgs e)
        {
            //Style FontStyle = Application.Current.Resources["PointerExitedColor"] as Style;
            //((TextBlock)sender).Foreground = FontStyle.Setters.Cast<Setter>().Where(item => item.Property == TextBlock.ForegroundProperty).FirstOrDefault().Value as SolidColorBrush;
        }

        private void ImageTextCollectionGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (AppSettings.ProjectName == "DownloadManger")
                {
                    try
                    {
                        GridView g = (GridView)sender;

                        if (g.SelectedIndex != -1)
                        {
                            AppSettings.ShowID = (g.SelectedItem as ShowList).ShowID.ToString();
                            AppSettings.ShowRating = Convert.ToDouble((g.SelectedItem as ShowList).Rating);
                            string title = (g.SelectedItem as ShowList).Title;

                            if (title != title)
                            {
                                Constants.UIThread = true;
                            }
                            else
                            {
                                string ext = System.IO.Path.GetExtension((g.SelectedItem as ShowList).Title);
                                if (ext == ".3gp" || ext == ".3g2" || ext == ".mp4" || ext == ".m4v" || ext == ".wmv" || ext == ".mkv" || ext == ".flv")
                                {
                                    // PageHelper.NavigateToPlayDownloadedVideos(AppResources.PlayDownloadedVideos, AppSettings.ShowID);
                                }
                                else if (ext == ".mp3" || ext == ".wav" || ext == ".aac" || ext == ".amr" || ext == ".wma")
                                {
                                    Page p1 = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                                    p1.GetType().GetTypeInfo().GetDeclaredMethod("Audio").Invoke(p1, new object[] { (g.SelectedItem as ShowList).Title });
                                }

                            }
                            g.SelectedIndex = -1;
                        }
                    }
                    catch (Exception ex)
                    {
                        Exceptions.SaveOrSendExceptions("Exception in g_SelectionChanged event In MainPage file.", ex);
                    }
                }
                GridView g1 = (GridView)sender;
                if (g1.SelectedIndex == -1)
                    return;
                var selectedItem = (sender as Selector).SelectedItem as ShowList;
                AppSettings.TileImage = selectedItem.TileImage;
                AppSettings.ShowID = selectedItem.ShowID.ToString();
                AppSettings.Title = selectedItem.Title;
                if (check == true)
                {
                    check = false;
                    Page ps = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                    ps.GetType().GetTypeInfo().GetDeclaredMethod("btnvisbility").Invoke(ps, null);
                    g1.SelectedIndex = -1;
                    return;
                }
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("DetailPage").Invoke(p, null);
                g1.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ImageTextCollectionGrid_SelectionChanged_1 Method In ViewAllList.cs file", ex);
            }
        }
    }
}
