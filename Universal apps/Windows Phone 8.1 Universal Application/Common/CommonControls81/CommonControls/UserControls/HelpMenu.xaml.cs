using Common;
using Common.Library;
using Microsoft.Advertising.WinRT.UI;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
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
    public sealed partial class HelpMenu : UserControl
    {
        public static string id = string.Empty;
        public HelpMenu()
        {
            this.InitializeComponent();
            Loaded += HelpMenu_Loaded;
        }

        void HelpMenu_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SettingsPane.GetForCurrentView().CommandsRequested += onCommandsRequested;
                if (AppResources.advisible == true)
                {
                    //AdControl adctrl = new AdControl();
                    //adctrl.ApplicationId = AppResources.adApplicationId;
                    //adctrl.AdUnitId = AppResources.adUnitId;
                    ////AdControl.ApplicationId = AppResources.adApplicationId;
                    ////AdControl.AdUnitId = AppResources.adUnitId;
                    //advisible.Visibility = Visibility.Visible;
                }
                lbxHelpMenu.ItemsSource = OnlineShow.LoadMenuList();
                tblkTitle.Visibility = Visibility.Visible;
                if (Constants.VideoMixHelpLine == true)
                {
                    LoadHelpData("5");
                    Constants.VideoMixHelpLine = false;
                }
                else
                {
                    LoadHelpData("1");
                    AppSettings.LinkUrl = string.Empty;
                    AppSettings.LinkUrl = AppResources.Helplinkurl;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in HelpMenu_Loaded Event In HelpMenu.cs file", ex);
            }
        }

        private void onCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            advisible.Visibility = Visibility.Collapsed;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
            p.GetType().GetTypeInfo().GetDeclaredMethod("Mainpage").Invoke(p, null);
        }

        private void lbxHelpMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AppSettings.LinkUrl = string.Empty;
            var selectedItem = (sender as Selector).SelectedItem as MenuProperties;
            id = selectedItem.Id;
            AppSettings.LinkUrl = selectedItem.Url;
            LoadHelpData(id);
        }

        private async void LoadHelpData(string id)
        {
            try
            {
                playergrid.Visibility = Visibility.Visible;
                string timestamp = await OnlineShow.Loadhelpstorage();
                System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(timestamp));
                var xdoc = XDocument.Load(ms);

                var findEle = from i in xdoc.Descendants("de") where i.Attribute("id").Value.ToString() == id select i;
                List<HelpItem> HelpItemList = new List<HelpItem>();


                foreach (var n in findEle.Descendants("c"))
                {
                    tblkVideosTitle.Text = n.Attribute("title").Value;

                    break;
                }
                foreach (var d in findEle.Descendants("c").Elements("des"))
                {
                    HelpItem HelpItem = new HelpItem();

                    HelpItem.HelpText = d.Value;
                    HelpItemList.Add(HelpItem);
                }


                lbxhelp.ItemsSource = HelpItemList;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadHelpData Method In HelpMenu.cs file", ex);
            }
        }

        private void platimage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AppSettings.YoutubeID = "1";
            AppSettings.Status = "";
            Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
            p.GetType().GetTypeInfo().GetDeclaredMethod("Youtubepage").Invoke(p, null);
        }
    }
}