
using OnlineVideos.Data;
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
using ss = OnlineVideos.Entities;
using OnlineVideos.Entities;
using OnlineVideos.Views;
using Common.Library;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class ShowCast : UserControl
    {
        Popup popgallimages;
        public static ShowCast current = null;
        string ShowId = AppSettings.ShowID;
        public string TeamId = string.Empty;
        ss.ShowCast selecteditem = null;
        AppInsights insights = new AppInsights();
        bool check = false;
        public ShowCast()
        {
            try
            {            
            this.InitializeComponent();
            this.Tag = this;
            current = this;
            progressbar.IsActive = true;
            Loaded += ShowCast_Loaded;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowCast Method In ShowCast.cs file", ex);
                insights.Exception(ex);
            }
        }

        void ShowCast_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                insights.Event("ShowCast_Loaded");
                GetPageDataInBackground();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowCast_Loaded Method In ShowCast.cs file", ex);
                insights.Exception(ex);
            }
        }
        public void GetPageDataInBackground()
        {
            try
            {
                List<CastRole> objlist = new List<CastRole>();
                BackgroundHelper bwHelper = new BackgroundHelper();
                if (AppSettings.ProjectName == "Cricket Videos")
                {

                    bwHelper.AddBackgroundTask(
                                                (object s, DoWorkEventArgs a) =>
                                                {
                                                    //progressbar.IsActive = false;
                                                    a.Result = CricketCast.GetCricketCast(AppSettings.ShowID, TeamId);
                                                },
                                                (object s, RunWorkerCompletedEventArgs a) =>
                                                {
                                                    objlist = (List<CastRole>)a.Result;
                                                    if (objlist.Count() != 0)
                                                    {
                                                        progressbar.IsActive = false;
                                                        lbxCast.ItemsSource = (List<CastRole>)a.Result;
                                                    }
                                                    else
                                                    {
                                                        progressbar.IsActive = false;
                                                        txtmsg.Visibility = Visibility.Visible;
                                                    }
                                                }
                                              );
                }
                else
                {
                    if (AppResources.PopUpVisibleInCastpage == "true")
                    {
                        //Layoutroot.Height = 400;
                        Layoutroot.Margin = new Thickness(0, 30, 0, 0);
                        bwHelper.AddBackgroundTask(
                                               (object s, DoWorkEventArgs a) =>
                                               {
                                                   a.Result = ShowCastManager.GetGameCastSection(AppSettings.ShowID);
                                               },
                                               (object s, RunWorkerCompletedEventArgs a) =>
                                               {
                                                   objlist = (List<CastRole>)a.Result;
                                                   if (objlist.Count() != 0)
                                                   {
                                                       progressbar.IsActive = false;
                                                       lbxCast.ItemsSource = (List<CastRole>)a.Result;
                                                   }
                                                   else
                                                   {
                                                       progressbar.IsActive = false;
                                                       txtmsg.Visibility = Visibility.Visible;
                                                   }
                                               }
                                             );
                    }
                    else
                    {
                        Layoutroot.Margin = new Thickness(100, 35, 20, 0);
                        bwHelper.AddBackgroundTask(
                                             (object s, DoWorkEventArgs a) =>
                                             {
                                                 a.Result = ShowCastManager.GetCastSection(AppSettings.ShowID);
                                             },
                                             (object s, RunWorkerCompletedEventArgs a) =>
                                             {
                                                 objlist = (List<CastRole>)a.Result;
                                                 if (objlist.Count() != 0)
                                                 {
                                                     progressbar.IsActive = false;

                                                     lbxCast.ItemsSource = (List<CastRole>)a.Result;
                                                 }
                                                 else
                                                 {
                                                     progressbar.IsActive = false;
                                                     if (ResourceHelper.AppName == "Yoga_&_Health")
                                                         txtmsg.Text = "No asanas available";
                                                     else
                                                         txtmsg.Text = "No cast available";
                                                     txtmsg.Visibility = Visibility.Visible;
                                                 }
                                             }
                                           );
                    }
                }

                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In ShowCast.cs file", ex);
                insights.Exception(ex);
            }
        }

        private void lbxCast_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxCast.SelectedIndex == -1)
                    return;
                AppSettings.PersonID = (lbxCast.SelectedItem as CastRole).PersonID.ToString();
                if (check == true)
                {
                    check = false;
                    //selecteditem = (sender as Selector).SelectedItem as ss.ShowCast;
                    //AppSettings.PersonID = selecteditem.PersonID.ToString();
                    AppSettings.LinkType = "Cast";
                    insights.Event(AppSettings.LinkType + "View");
                    lbxCast.SelectedIndex = -1;
                    Page p1 = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                    p1.GetType().GetTypeInfo().GetDeclaredMethod("appbar").Invoke(p1, new object[] { true });
                    return;
                }
                //PageHelper.NavigateToCastDetailPage(AppResources.CastDetailPageName,null);

                if (AppResources.PopUpVisibleInCastpage == "true")
                {
                    if (popgallimages.IsOpen == false)
                    {
                        ////var selectedItem = (sender as Selector).SelectedItem as GalleryImageInfo;
                        //AppSettings.GallCount = (sender as Selector).SelectedIndex;
                        //Page p = (Page)PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                        //popgallimages.Child = (UIElement)p.GetType().GetTypeInfo().GetDeclaredMethod("Popup").Invoke(p, null);

                        //popgallimages.IsOpen = true;
                        //popgallimages.Height = 1000;
                        //lbxCast.SelectedIndex = -1;
                        //return;

                        if (lbxCast.SelectedIndex == -1)
                            return;

                        AppSettings.GameClassName = "Cast";
                        AppSettings.GameGalCount = (sender as Selector).SelectedIndex.ToString();
                        Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                        p.GetType().GetTypeInfo().GetDeclaredMethod("DescriptionPopup").Invoke(p, new object[] { false });
                        lbxCast.SelectedIndex = -1;
                        return;
                    }
                    //else
                    //{
                    //    popgallimages.IsOpen = false;
                    //    Page p = (Page)PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                    //    popgallimages.Child = (UIElement)p.GetType().GetTypeInfo().GetDeclaredMethod("ClosePop").Invoke(p, null);
                    //    lbxCast.SelectedIndex = -1;
                    //    return;
                    //}
                }
                if (AppResources.CastPanoramaVisible == "true")
                {
                    lbxCast.SelectedIndex = -1;
                    Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                    p.GetType().GetTypeInfo().GetDeclaredMethod("CastPanorama").Invoke(p, null);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxCast_SelectionChanged_1 Method In ShowCast.cs file", ex);
                insights.Exception(ex);
            }
        }

        private void StackPanel_RightTapped_1(object sender, RightTappedRoutedEventArgs e)
        {
            check = true;
            Constants.appbarvisible = true;
            Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
            p.GetType().GetTypeInfo().GetDeclaredMethod("apbars").Invoke(p, null);
        }
    }
}