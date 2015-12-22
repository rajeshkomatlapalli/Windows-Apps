using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if NOTANDROID
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif
namespace OnlineVideos.Data
{
    public static class PopupManager
    {
#if NOTANDROID
        public static Grid PopGrid = default(Grid);
        public static Grid MainGrid = default(Grid);
        public static AppBar PageAppBar = default(AppBar);
        public static string ViewboxName = string.Empty;
        public static string ControlName = string.Empty;
        public static ScrollViewer MainScroll = default(ScrollViewer);


        public static void EnableControl()
        {

            if (MainGrid != default(Grid))
                MainGrid.IsHitTestVisible = true;
            if (PageAppBar != default(AppBar))
                PageAppBar.Visibility = Visibility.Visible;
            if (PopGrid != default(Grid))
            {
                PopGrid.Children.Remove((UIElement)PopGrid.FindName(ControlName));
                PopGrid.UpdateLayout();
            }
            //if (MainScroll != default(ScrollViewer))
            //{
            //    MainScroll.HorizontalScrollMode = ScrollMode.Auto;
            //}
            DisposeControls();
        }


        public static void CopyControl(Grid maingrid, AppBar pageappbar, string ControlNames, Grid popgrid)
        {
            ControlName = ControlNames;
            MainGrid = maingrid;
            PageAppBar = pageappbar;
            PopGrid = popgrid;

        }
        public static void CopyControls(Grid popgrid, Grid maingrid, AppBar pageappbar, string viewboxname)
        {
            PopGrid = popgrid;
            MainGrid = maingrid;
            PageAppBar = pageappbar;
            ViewboxName = viewboxname;

        }
        public static void CopyControls(Grid popgrid, Grid maingrid, AppBar pageappbar, ScrollViewer mainscroll, string viewboxname)
        {
            PopGrid = popgrid;
            MainGrid = maingrid;
            PageAppBar = pageappbar;
            MainScroll = mainscroll;
            ViewboxName = viewboxname;
        }
        public static void DisposeControls()
        {

            PopGrid = default(Grid);
            MainGrid = default(Grid);
            PageAppBar = default(AppBar);
            MainScroll = default(ScrollViewer);
            ViewboxName = string.Empty;
            ControlName = string.Empty;
        }
        public static void DisableControls()
        {
            if (MainGrid != default(Grid))
                MainGrid.IsHitTestVisible = false;
            if (PageAppBar != default(AppBar))
                PageAppBar.Visibility = Visibility.Collapsed;
            if (MainScroll != default(ScrollViewer))
            {
                MainScroll.ScrollToHorizontalOffset(0);
                MainScroll.HorizontalScrollMode = ScrollMode.Disabled;
            }

        }
        public static void EnableControls()
        {

            if (MainGrid != default(Grid))
                MainGrid.IsHitTestVisible = true;
            if (PageAppBar != default(AppBar))
                PageAppBar.Visibility = Visibility.Visible;
            if (PopGrid != default(Grid))
            {
                PopGrid.Children.Remove((UIElement)PopGrid.FindName(ViewboxName));
                PopGrid.UpdateLayout();
            }
            if (MainScroll != default(ScrollViewer))
            {
                MainScroll.HorizontalScrollMode = ScrollMode.Auto;
            }
            DisposeControls();
        }
#endif

    }
}
