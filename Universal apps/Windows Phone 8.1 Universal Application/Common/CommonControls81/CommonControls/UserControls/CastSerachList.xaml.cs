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
    public sealed partial class CastSerachList : UserControl
    {
        public string TeamId = string.Empty;
        List<CastRole> objcastlist = null;

        public CastSerachList()
        {
            try
            {
            this.InitializeComponent();
            objcastlist = new List<CastRole>();
            progressbar.IsActive = true;
            Loaded += CastSerachList_Loaded;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in CastSerachList Method In CastSerachList.cs file", ex);
            }
        }

        void CastSerachList_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GetPageDataInBackground();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in CastSerachList_Loaded Method In CastSerachList.cs file", ex);
            }
        }

        private void GetPageDataInBackground()
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();
                if (AppSettings.ProjectName == "Cricket Videos")
                {

                    bwHelper.AddBackgroundTask(
                                                (object s, DoWorkEventArgs a) =>
                                                {

                                                    a.Result = ShowCastManager.GetCastSearchSection(AppSettings.SearchText);
                                                },
                                             (object s, RunWorkerCompletedEventArgs a) =>
                                             {
                                                 objcastlist = (List<CastRole>)a.Result;
                                                 if (objcastlist.Count > 0)
                                                 {
                                                     progressbar.IsActive = false;
                                                     lbxCast.ItemsSource = objcastlist;
                                                 }
                                                 else
                                                 {
                                                     progressbar.IsActive = false;
                                                     txtmsg.Visibility = Visibility.Visible;
                                                     txtmsg.Text = "No cast available";
                                                 }
                                             }
                                           );
                }
                else
                {
                    if (AppResources.PopUpVisibleInCastpage == "true")
                    {
                        Layoutroot.Height = 400;
                        Layoutroot.Margin = new Thickness(0, 30, 0, 0);
                        bwHelper.AddBackgroundTask(
                                               (object s, DoWorkEventArgs a) =>
                                               {
                                                   a.Result = ShowCastManager.GetCastSearchSection(AppSettings.SearchText);
                                               },
                                               (object s, RunWorkerCompletedEventArgs a) =>
                                               {
                                                   progressbar.IsActive = false;
                                                   lbxCast.ItemsSource = (List<CastRole>)a.Result;
                                               }
                                             );
                    }
                    else
                    {
                        Layoutroot.Margin = new Thickness(100, 35, 20, 0);
                        bwHelper.AddBackgroundTask(
                                             (object s, DoWorkEventArgs a) =>
                                             {
                                                 a.Result = ShowCastManager.GetCastSearchSection(AppSettings.SearchText);
                                             },
                                             (object s, RunWorkerCompletedEventArgs a) =>
                                             {
                                                 objcastlist = (List<CastRole>)a.Result;
                                                 if (objcastlist.Count > 0)
                                                 {
                                                     progressbar.IsActive = false;
                                                     lbxCast.ItemsSource = objcastlist;
                                                 }
                                                 else
                                                 {
                                                     progressbar.IsActive = false;
                                                     txtmsg.Visibility = Visibility.Visible;
                                                     txtmsg.Text = "No cast available";
                                                 }
                                             }
                                           );
                    }
                }
                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In CastSerachList.cs file", ex);
            }
        }

        private void lbxCast_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (AppResources.CastPanoramaVisible == "true")
                {
                    if (lbxCast.SelectedIndex == -1)
                        return;
                    AppSettings.PersonID = (lbxCast.SelectedItem as CastRole).PersonID.ToString();
                    lbxCast.SelectedIndex = -1;
                    Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                    p.GetType().GetTypeInfo().GetDeclaredMethod("CastPanorama").Invoke(p, null);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxCast_SelectionChanged_1 Method In CastSerachList.cs file", ex);
            }
        }
    }
}