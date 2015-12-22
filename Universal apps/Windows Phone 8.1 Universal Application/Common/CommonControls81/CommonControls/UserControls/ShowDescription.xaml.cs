using Common.Library;
using comm = OnlineVideos.Common;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using OnlineVideos.Views;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class ShowDescription : UserControl
    {
        public static ObservableCollection<ShowList> collection = new ObservableCollection<ShowList>();
        public string ShowID = string.Empty;
        bool check = false;
        public static ShowDescription current = null;
        AppInsights insights = new AppInsights();
        public ShowDescription()
        {
            try
            {
            this.InitializeComponent();
            Loaded += ShowDescription_Loaded_1;
            current = this;
            progressbar.IsActive = true;

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowDescription Method In ShowDescription.cs file", ex);
                insights.Exception(ex);
            }
        }

        public static Image ItemImage(GridView GridviewName)
        {
            Image ch = default(Image);
            try
            {
                IEnumerable<DependencyObject> cboxes = null;
                cboxes = GetChildsRecursive(GridviewName);
                foreach (DependencyObject obj in cboxes.OfType<Image>())
                {
                    Type type = obj.GetType();
                    if (type.Name == "Image" && (obj as Image).Name == "mixplayimg")
                    {
                        ch = obj as Image;

                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in CheckedItemList Method In ShowDescription.cs file", ex);
                

            }
            return ch;
        }

        public static Border Itemchild(GridView GridviewName)
        {
            Border ch = default(Border);
            try
            {
                IEnumerable<DependencyObject> cboxes = null;
                cboxes = GetChildsRecursive(GridviewName);
                foreach (DependencyObject obj in cboxes.OfType<Border>())
                {
                    Type type = obj.GetType();
                    if (type.Name == "Border" && (AppSettings.ProjectName == "Video Mix" ? (obj as Border).Name == "videomixborder" : (obj as Border).Name == "normalborder"))
                    {
                        ch = obj as Border;

                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in CheckedItemList Method In ShowDescription.cs file", ex);               
            }
            return ch;
        }
        public static StackPanel ItemStack(GridView GridviewName)
        {
            StackPanel ch = default(StackPanel);
            try
            {
                IEnumerable<DependencyObject> cboxes = null;
                cboxes = GetChildsRecursive(GridviewName);
                foreach (DependencyObject obj in cboxes.OfType<StackPanel>())
                {
                    Type type = obj.GetType();
                    if (type.Name == "StackPanel" && (obj as StackPanel).Name == "detailstk")
                    {
                        ch = obj as StackPanel;

                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in CheckedItemList Method In ShowDescription.cs file", ex);

            }
            return ch;
        }
        public static IEnumerable<DependencyObject> GetChildsRecursive(DependencyObject root)
        {
            List<DependencyObject> elts = new List<DependencyObject>();
            try
            {
                elts.Add(root);

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
                {
                    elts.AddRange(GetChildsRecursive(VisualTreeHelper.GetChild(root, i)));
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetChildsRecursive Method In DownloadManagerHelper.cs file", ex);
           
            }
            return elts;
        }

        private void ShowDescription_Loaded_1(object sender, RoutedEventArgs e)
        {

            try
            {
                insights.Event("Description View");
                int showid = AppSettings.ShowUniqueID;

                if (AppSettings.ProjectName == "Video Mix")
                {
                    tlkdesc.Width = 950;
                    grdvwDetails.IsHitTestVisible = true;
                }
                else
                {
                    tlkdesc.Width = 1050;
                    grdvwDetails.IsHitTestVisible = false;
                }

                GetPageDataInBackground();

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowDescription_Loaded_1 Method In ShowDescription.cs file", ex);
            }
        }
        public void GetPageDataInBackground()
        {
            try
            {
                collection.Clear();
                List<ShowList> objlist = new List<ShowList>();
                BackgroundHelper bwHelper = new BackgroundHelper();
                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                a.Result = OnlineShow.GetVideoDetails(AppSettings.ShowID);
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                objlist = (List<ShowList>)a.Result;
                                                foreach (ShowList ss in objlist)
                                                {
                                                    collection.Add(ss);
                                                }
                                                grdvwDetails.ItemsSource = collection;
                                                foreach (ShowList des in collection)
                                                {
                                                    if (des.Description != null)
                                                        tlkdesc.Text = des.Description;
                                                    break;
                                                }
                                                progressbar.IsActive = false;
                                                LayoutUpdated += ShowDescription_LayoutUpdated;
                                                //tlkdesc.Text = objlist.Description; ;
                                                // lstvwsongs.ItemsSource = (List<ShowLinks>)a.Result;
                                            }
                                          );

                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In ShowVideos.cs file", ex);
                insights.Exception(ex);
            }
        }

        void ShowDescription_LayoutUpdated(object sender, object e)
        {
            //Border border = Itemchild(grdvwDetails);
            //border.Visibility = Visibility.Visible;
            if (AppSettings.ProjectName == "Video Mix")
            {
                StackPanel stackpanel = ItemStack(grdvwDetails);
                if (stackpanel != null)
                    stackpanel.Margin = new Thickness(100, 0, 0, 0);
                int showid = AppSettings.ShowUniqueID;
                Image playimage = ItemImage(grdvwDetails);
                if (playimage != null)
                {
                    string imagename = Task.Run(async () => await Constants.connection.Table<OnlineVideos.Entities.ShowList>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result.TileImage;
                    if (imagename == "Vlogo.jpg")
                        playimage.Source = new BitmapImage(new Uri("ms-appx:///Images/PlayerImages/play1.png", UriKind.RelativeOrAbsolute));
                    else
                        playimage.Source = new BitmapImage(new Uri("ms-appx:///Images/play.png", UriKind.RelativeOrAbsolute));
                }
            }
        }

        private void mixplayimg_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            Page p = (Page)comm.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
            p.GetType().GetTypeInfo().GetDeclaredMethod("playmix").Invoke(p, null);
        }

        private void LayoutRoot_RightTapped_2(object sender, RightTappedRoutedEventArgs e)
        {
            try
            {
                check = true;
                Constants.appbarvisible = true;
                Constants.pintomovie = "ok";
                Constants.AppbaritemVisbility = "visbil";
                Page p = (Page)comm.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));

                p.GetType().GetTypeInfo().GetDeclaredMethod("appbar").Invoke(p, new object[] { false });
                p.GetType().GetTypeInfo().GetDeclaredMethod("changetext").Invoke(p, new object[] { "Movies" });

                Constants.selecteditem = null;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LayoutRoot_RightTapped_2 Method In ShowDescription.cs file", ex);
                insights.Exception(ex);
            }
        }
    }
}