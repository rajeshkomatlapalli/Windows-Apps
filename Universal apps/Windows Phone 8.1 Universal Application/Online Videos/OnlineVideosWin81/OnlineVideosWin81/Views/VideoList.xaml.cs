using AdRotator;
using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OnlineVideos.Views
{
    public class val : INotifyPropertyChanged
    {

        private bool _Status;
        public bool Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
                OnPropertyChanged("Status");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VideoList : Page
    {
        val buttonvisibility = new val();
        public VideoList()
        {
            this.InitializeComponent();
            Loaded += VideoList_Loaded;
            this.DataContext = buttonvisibility;
        }
        void VideoList_Loaded(object sender, RoutedEventArgs e)
        {
            AdRotatorWin8.Invalidate();
            Window.Current.CoreWindow.PointerWheelChanged += CoreWindow_PointerWheelChanged;
            title.Text = AppSettings.ViewAllTitle.Replace(">", "");

        }

        private void CoreWindow_PointerWheelChanged(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.PointerEventArgs args)
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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private void BottmAppBar_Opened(object sender, object e)
        {
            // Musicstak.Visibility = Visibility.Collapsed;

            //if (Constants.appbarvisible == false)
            //{
            //    BottomAppBar.IsOpen = false;
            //    Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
            //    AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
            //    adcontrol.IsAdRotatorEnabled = true;
            //    adcontrol.Visibility = Visibility.Visible;
            //}
            //else
            //{

            //Constants.appbarvisible = false;
            Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
            AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
            adcontrol.IsAdRotatorEnabled = false;
            adcontrol.Visibility = Visibility.Collapsed;

            //}
        }

        private void BottmAppBar_Closed(object sender, object e)
        {
            Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
            AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
            adcontrol.IsAdRotatorEnabled = true;
            adcontrol.Visibility = Visibility.Visible;
        }

        private void BackButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (App.rootFrame.CanGoBack)
            {
                if (ContentPanel.Children.Count < 2)
                    ResetPageCache();
                App.rootFrame.GoBack();
            }
        }
        public void DetailPage()
        {

            if (ContentPanel.Children.Count < 2)
                ResetPageCache();

            App.rootFrame.Navigate(typeof(Detail));
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }

        private void deleteshow_Click_1(object sender, RoutedEventArgs e)
        {
            string filename = "";

            int showid = Convert.ToInt32(AppSettings.ShowID);
            ShowList showlist = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result;
            filename = showlist.Title;
            DeleteLinks(showid);
            DeleteShowCast(showid);
            CategoriesByShowID CategoriesByShowIDlist = Task.Run(async () => await Constants.connection.Table<CategoriesByShowID>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result;
            Task.Run(async () => await Constants.connection.DeleteAsync(CategoriesByShowIDlist));
            Task.Run(async () => await Constants.connection.DeleteAsync(showlist));
            List<string> ImageDic = new List<string>();
            ImageDic.Add("scale-100");
            ImageDic.Add("scale-140");
            ImageDic.Add("scale-180");
            ImageDic.Add("ListImages");
            ImageDic.Add("TileImages\\30-30");
            ImageDic.Add("TileImages\\150-150");
            ImageDic.Add("TileImages\\310-150");
            foreach (string foldername in ImageDic)
            {
                if (showlist.TileImage != "Default.jpg")
                {
                    if (Task.Run(async () => await Storage.FileExists("Images\\" + foldername + "\\" + showlist.Title + ".jpg")).Result)
                    {
                        var isoStore = Task.Run(async () => await ApplicationData.Current.LocalFolder.GetFolderAsync("Images")).Result;
                        var isoStore1 = Task.Run(async () => await isoStore.GetFolderAsync(foldername)).Result;
                        //var isoStore2 = Task.Run(async () => await isoStore1.GetFolderAsync(showid.ToString())).Result;
                        var isoStore3 = Task.Run(async () => await isoStore1.GetFileAsync(showlist.Title + ".jpg")).Result;
                        Task.Run(async () => await isoStore3.DeleteAsync());
                    }
                }
            }
            ss.ViewAllList.current.GetPageDataInBackground();
        }

        private void DeleteShowCast(int filename)
        {

            foreach (ShowCast pid in Task.Run(async () => await Constants.connection.Table<ShowCast>().Where(i => i.ShowID == filename).ToListAsync()).Result)
            {
                int castcount = 0;
                var aa = Task.Run(async () => await Constants.connection.Table<ShowCast>().Where(id => id.PersonID == pid.PersonID).ToListAsync()).Result.GroupBy(id => id.PersonID).OrderByDescending(id => id.Count()).Select(g => new { Count = g.Count() });



                foreach (var itm in aa)
                {
                    castcount = itm.Count;

                    if (castcount == 1 && Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == filename && i.Status == "Custom").FirstOrDefaultAsync()).Result != null)
                    {
                        CastProfile ds1 = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID == pid.PersonID).FirstOrDefaultAsync()).Result;
                        Task.Run(async () => await Constants.connection.DeleteAsync(ds1));

                    }

                }

            }
            List<ShowCast> ds = Task.Run(async () => await Constants.connection.Table<ShowCast>().Where(i => i.ShowID == filename).ToListAsync()).Result;
            foreach (ShowCast cast in ds)
            {
                Task.Run(async () => await Constants.connection.DeleteAsync(cast));
            }
        }
        private void DeleteLinks(int filename)
        {
            string linkurl = "";

            List<ShowLinks> xquery = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == filename).ToListAsync()).Result;
            if (xquery.Count() > 0)
            {
                foreach (var itm in xquery)
                {
                    linkurl = itm.LinkUrl;
                    Task.Run(async () => await Constants.connection.DeleteAsync(itm));
                }

            }

        }
        public void ResetPageCache()
        {
            var cacheSize = Frame.CacheSize;
            Frame.CacheSize = 0;
            Frame.CacheSize = cacheSize;
        }
        public void btnvisbility()
        {
            int showid = Convert.ToInt32(AppSettings.ShowID);
            if (showid != 0)
            {
                if (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result.Status != "Custom")
                {
                    buttonvisibility.Status = false;
                }
                else
                    buttonvisibility.Status = true;
            }
            else
                buttonvisibility.Status = false;
        }
        public void apbar()
        {
            BottomAppBar.IsOpen = true;
        }

        private void AppnBarNew_Click(object sender, RoutedEventArgs e)
        {
            Viewbox vb = new Viewbox();
            vb.Name = "popupviewbox";
            vb.Margin = new Thickness(0, 20, 0, 20);
            UserUpload upload = new UserUpload();
            upload.Tag = viewalllist.Tag;
            PopupManager.CopyControl(this.LayoutRoot, this.BottomAppBar, "popupviewbox", this.ContentPanel);
            vb.Child = upload;
            ContentPanel.Children.Add(vb);
            PopupManager.DisableControls();
            Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
            AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
            adcontrol.IsAdRotatorEnabled = false;
            adcontrol.Visibility = Visibility.Collapsed;
        }

    }
}
