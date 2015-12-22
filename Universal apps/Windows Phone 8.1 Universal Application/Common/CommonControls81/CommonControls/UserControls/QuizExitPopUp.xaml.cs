using OnlineVideos.Common;
using OnlineVideos.Data;
using System;
using System.Collections.Generic;
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
    public sealed partial class QuizExitPopUp : UserControl
    {
        public QuizExitPopUp()
        {
            try
            {
                this.InitializeComponent();
            }
            catch (Exception ex)
            {                                
            }
        }

        private void imgok_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            LayoutRoot.Visibility = Visibility.Collapsed;
            QuizManager.deletestoriongdata();
            //Popup p = (Popup)PageNavigationManager.GetParentOfType(this, typeof(Popup));
            //object pg = p.GetType().GetTypeInfo().BaseType.GetRuntimeProperty("Tag").GetValue(p);
            //((Page)pg).GetType().GetTypeInfo().GetDeclaredMethod("DetailPage").Invoke(pg, null);
            Page p = (Page)PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
            p.GetType().GetTypeInfo().GetDeclaredMethod("DetailPage").Invoke(p, null);
        }

        private void imgcancel_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            //Popup p = (Popup)PageNavigationManager.GetParentOfType(this, typeof(Popup));
            //object pg = p.GetType().GetTypeInfo().BaseType.GetRuntimeProperty("Tag").GetValue(p);
            //((Page)pg).GetType().GetTypeInfo().GetDeclaredMethod("ImageEnable").Invoke(pg, null);
            Page p = (Page)PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
            p.GetType().GetTypeInfo().GetDeclaredMethod("ImageEnable").Invoke(p, null);
        }
    }
}
