using Common.Library;
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
    public sealed partial class DownloadVideoHelpPopup : UserControl
    {
        public DownloadVideoHelpPopup()
        {
            this.InitializeComponent();
            Loaded += DownloadVideoHelpPopup_Loaded;
        }

        void DownloadVideoHelpPopup_Loaded(object sender, RoutedEventArgs e)
        {
            RootGrid.Visibility = Visibility.Visible;
            LoadHelpData("9");
        }

        private async void LoadHelpData(string id)
        {
            try
            {
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
                    if (!d.Value.EndsWith(".jpg"))
                    {
                        HelpItem.HelpText = d.Value;
                    }
                    else
                    {
                        HelpItem.DownLoadImage = ResourceHelper.getDownLoadImagesFromStorageOrInstalledFolder(d.Value);
                    }
                    HelpItemList.Add(HelpItem);
                }
                lbxhelp.ItemsSource = HelpItemList;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadHelpData Method In DownloadVideoHelpPopup.cs file", ex);
            }
        }

        private void imgclose_Tapped(object sender, TappedRoutedEventArgs e)
        {
            RootGrid.Visibility = Visibility.Collapsed;
        }
    }
}
