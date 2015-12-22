using Common.Library;
using Common.Utilities;
using Indian_Cinema.Views;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.UI;
using OnlineVideos.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Input;
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
    public sealed partial class ShowCast : UserControl
    {
        #region GlobalDeclaration
        AppInsights insights = new AppInsights();
        private static bool IsDataLoaded;
        List<CastRole> ShowCastList = null;
        #endregion

        #region Constructor
        public ShowCast()
        {
            this.InitializeComponent();
            ShowCastList = new List<CastRole>();
            IsDataLoaded = false;
        }
        #endregion

        #region "Common Methods"
        public void GetPageDataInBackground()
        {
            //if (AppResources.ShowCricketDetailPage)
            //{
            //    IDictionary<string, int> index = new Dictionary<string, int>();                
            //    int showid = int.Parse(AppSettings.ShowID);
            //    var getcast = Task.Run(async () => await Constants.connection.Table<OnlineVideos.Entities.ShowCast>().Where(i => i.ShowID == showid).ToListAsync()).Result;
            //    var getcastteamid = getcast.Select(i => i.CategoryID).Distinct().ToList();              
            //    Frame frame = Windows.UI.Xaml.Window.Current.Content as Frame;
            //    Page PageInstance = frame.Content as Page;
            //    Pivot MainPivot = (Pivot)(PageInstance.FindName("pvtMainDetails"));
            //    PivotItem TeamAPivot = (PivotItem)(MainPivot.FindName("pvtitmcast1"));
            //    PivotItem TeamBPivot = (PivotItem)(MainPivot.FindName("pvtitmcast2"));
            //    ShowCast[] ShowCastArray = new ShowCast[] { (ShowCast)(TeamAPivot.FindName("Cast")), (ShowCast)(TeamBPivot.FindName("Cast1")) };
            //    TeamAPivot.Header = AppSettings.TeamATitle;

            //    TeamBPivot.Header = AppSettings.TeamBTitle;
            //    BackgroundHelper bwHelper = new BackgroundHelper();

            //    for (int i = 0; i < getcastteamid.Count; i++)
            //    {
            //        bwHelper.AddBackgroundTask(
            //                                    (object s, DoWorkEventArgs a) =>
            //                                    {
            //                                        a.Result = CricketCast.GetCricketCast(AppSettings.ShowID, getcastteamid[(Convert.ToInt32(a.Argument.ToString()))].ToString());
            //                                        index.Add(getcastteamid[(Convert.ToInt32(a.Argument.ToString()))].ToString(), Convert.ToInt32(a.Argument.ToString()));
            //                                    },
            //                                    (object s, RunWorkerCompletedEventArgs a) =>
            //                                    {
            //                                        int argumentIndex = 0;
            //                                        ShowCastList = (List<CastRole>)a.Result;
            //                                        index.TryGetValue(ShowCastList.FirstOrDefault().TeamID.ToString(), out argumentIndex);
            //                                        if (ShowCastList.Count > 0)
            //                                        {
            //                                            ((ShowCast)(ShowCastArray[argumentIndex])).lbxCast.ItemsSource = ShowCastList;
            //                                            tblkcast.Visibility = Visibility.Collapsed;
            //                                        }
            //                                        else
            //                                        {
            //                                            tblkcast.Text = "No " + AppResources.ShowDetailPageCastPivotTitle as string + " available";
            //                                            tblkcast.Visibility = Visibility.Visible;
            //                                        }
            //                                        MyProgressBar1.IsIndeterminate = false;
            //                                    },
            //                                    (i)
            //                                  );

            //    }

            //    bwHelper.RunBackgroundWorkers();
            //}
            //else
            //{
                BackgroundHelper bwHelper = new BackgroundHelper();

                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                a.Result = ShowCastManager.GetCastSection(AppSettings.ShowUniqueID.ToString());
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                ShowCastList = (List<CastRole>)a.Result;
                                                if (ShowCastList.Count > 0)
                                                {
                                                    lbxCast.ItemsSource = ShowCastList;
                                                    tblkcast.Visibility = Visibility.Collapsed;
                                                }
                                                else
                                                {
                                                    lbxCast.ItemsSource = null;
                                                    tblkcast.Text = "No " + AppResources.FavoritePeoplePivotTitle as string + " available";
                                                    tblkcast.Visibility = Visibility.Visible;
                                                }
                                                MyProgressBar1.IsIndeterminate = false;
                                            }
                                          );

                bwHelper.RunBackgroundWorkers();
            //}
        }
        #endregion

        #region Events
        private void lbxCast_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxCast.SelectedIndex == -1)
                    return;

                if (ResourceHelper.AppName == Apps.Video_Games.ToString())
                {
                    if (!AppResources.ShowCastPanoramaPage)
                    {
                        if (!AppResources.ShowCastPanoramaPage)
                        {
                            if (lbxCast.SelectedIndex == -1)
                                return;
                            PageHelper.NavigateTo(NavigationHelper.getVideoGameDescriptionPage((lbxCast.SelectedItem as CastRole).PersonID.ToString(), AppSettings.ShowID, "Cast"));
                            lbxCast.SelectedIndex = -1;

                        }
                    }
                }
                else
                {
                    if (!AppResources.ShowCastPanoramaPage)
                        return;
                }


                if (lbxCast.SelectedIndex == -1)
                    return;
                Frame frame = Windows.UI.Xaml.Window.Current.Content as Frame;
                Page p = frame.Content as Page;
                AppSettings.PersonID = (lbxCast.SelectedItem as CastRole).PersonID.ToString();
                State.BackStack = "cast";
                string[] parameters = new string[2];
                parameters[0] = AppSettings.PersonID;
                parameters[1] = null;
                p.Frame.Navigate(typeof(CastHub), parameters);
                AppSettings.detailtocast++;               
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxCast_SelectionChanged method In ShowCast.cs file.", ex);
            }
            lbxCast.SelectedIndex = -1;
        }

        private void MenuFlyout_Opened(object sender, object e)
        {
            try
            {                
                //MenuFlyoutItem menu = sender as MenuFlyoutItem;
                MenuFlyout mainmenu = sender as MenuFlyout;
                var item = mainmenu.Items.OfType<MenuFlyoutItem>().First(m => (string)m.Name == "deletecast");
                if (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == AppSettings.ShowUniqueID && i.Status == "Custom").FirstOrDefaultAsync()).Result != null)
                {
                    ListViewItem selectedListBoxItem = lbxCast.ItemContainerGenerator.ContainerFromItem(item.DataContext) as ListViewItem;                    
                    int personid = Convert.ToInt32((selectedListBoxItem.Content as CastRole).PersonID);

                    OnlineVideos.Entities.ShowCast pid = Task.Run(async () => await Constants.connection.Table<OnlineVideos.Entities.ShowCast>().Where(i => i.ShowID == AppSettings.ShowUniqueID && i.PersonID == personid).FirstOrDefaultAsync()).Result;

                    int castcount = 0;
                    var xqueryw = Task.Run(async () => await Constants.connection.Table<OnlineVideos.Entities.ShowCast>().Where(id => id.PersonID == pid.PersonID).ToListAsync()).Result.GroupBy(id => id.PersonID).OrderByDescending(id => id.Count()).Select(g => new { Count = g.Count() });
                    if (xqueryw.Count() > 0)
                    {

                        foreach (var itm in xqueryw)
                        {
                            castcount = itm.Count;

                            if (castcount == 1)
                            {
                                item.IsEnabled = true;
                            }
                            else
                            {
                                item.IsEnabled = true;
                            }
                        }
                    }
                }
                else
                {
                    item.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ContextMenu_Opened_1 method In ShowCast.cs file.", ex);
            }
        }

        private void deletecast_Click(object sender, RoutedEventArgs e)
        {
            try
            {               
                MenuFlyoutItem menu = sender as MenuFlyoutItem;
                int showid = AppSettings.ShowUniqueID;
                //ListBoxItem selectedListBoxItem = lbxCast.ItemContainerGenerator.ContainerFromItem(menu.DataContext) as ListBoxItem;
                ListViewItem selectedListBoxItem = selectedListBoxItem1;
                int personid = Convert.ToInt32((selectedListBoxItem.Content as CastRole).PersonID);               
                OnlineVideos.Entities.ShowCast pid = Task.Run(async () => await Constants.connection.Table<OnlineVideos.Entities.ShowCast>().Where(i => i.ShowID == AppSettings.ShowUniqueID && i.PersonID == personid).FirstOrDefaultAsync()).Result;
                int castcount = 0;
                var xqueryw = Task.Run(async () => await Constants.connection.Table<OnlineVideos.Entities.ShowCast>().Where(id => id.PersonID == pid.PersonID).ToListAsync()).Result.GroupBy(id => id.PersonID).OrderByDescending(id => id.Count()).Select(g => new { Count = g.Count() });
                if (xqueryw.Count() > 0)
                {
                    foreach (var itm in xqueryw)
                    {
                        castcount = itm.Count;

                        if (castcount == 1 && Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid && i.Status == "Custom").FirstOrDefaultAsync()).Result != null)
                        {
                            CastProfile ds1 = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID == personid).FirstOrDefaultAsync()).Result;
                            Task.Run(async () => await Constants.connection.DeleteAsync(ds1));
                        }
                    }
                }

                Task.Run(async () => await Constants.connection.DeleteAsync(pid));
                if (Task.Run(async () => await Storage.FileExists("/" + "Images/PersonImages/" + personid + ".jpg")).Result)
                {
                    Storage.DeleteFile("/" + "Images/PersonImages/" + personid + ".jpg");
                }
                GetPageDataInBackground();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in deletecast_Click_1 method In ShowCast.cs file.", ex);
            }
        }
        #endregion

        #region Page Load
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsDataLoaded == false)
                {
                    MyProgressBar1.IsIndeterminate = true;
                    GetPageDataInBackground();
                    IsDataLoaded = true;
                    MyProgressBar1.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in UserControl_Loaded method In ShowCast.cs file.", ex);
            }
        }
        #endregion

        ListViewItem selectedListBoxItem1;
        private void StackPanel_Holding(object sender, HoldingRoutedEventArgs e)
        {
            
            // this event is fired multiple times. We do not want to show the menu twice
            if (e.HoldingState != HoldingState.Started) return;

            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            // If the menu was attached properly, we just need to call this handy method
            FlyoutBase.ShowAttachedFlyout(element);

            FrameworkElement element1 = (FrameworkElement)e.OriginalSource;
            var datacontext = element1.DataContext;
            selectedListBoxItem1 = this.lbxCast.ContainerFromItem(datacontext) as ListViewItem;
        }
    }
}
