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
    public sealed partial class ShowVideoList : UserControl
    {
        ShowLinks selecteditem = null;
        bool check = false;
        public ShowVideoList()
        {
            this.InitializeComponent();
            Loaded += ShowVideoList_Loaded;
        }
        void ShowVideoList_Loaded(object sender, RoutedEventArgs e)
        {
            GetPageDataInBackground();
        }

        private void ImageTextCollectionGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (ImageTextCollectionGrid.SelectedIndex == -1)
                return;
            selecteditem = (sender as Selector).SelectedItem as ShowLinks;
            Constants.selecteditem = selecteditem;
            AppSettings.LinkType = selecteditem.LinkType;
            AppSettings.ShowsVideoIsHidden = selecteditem.IsHidden.ToString();
            Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
            p.GetType().GetTypeInfo().GetDeclaredMethod("changeimage").Invoke(p, null);
            ImageTextCollectionGrid.SelectedIndex = -1;
            return;
        }

        private void GetPageDataInBackground()
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();
                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                a.Result = OnlineShow.GetShowVideoLinks(AppSettings.ShowID, LinkType.Songs);
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                ImageTextCollectionGrid.ItemsSource = (List<ShowLinks>)a.Result;
                                            }
                                          );

                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In ShowVideoList.cs file", ex);
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
                Exceptions.SaveOrSendExceptions("Exception in TextBlock_RightTapped_1 Method In ShowVideoList.cs file", ex);
            }
        }
    }
}
