using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WINDOWS_APP && NOTANDROID
using Windows.UI.Xaml;
#endif
namespace CommonWin8.Library
{
 public static class ScreenResolutionForPopup
    {
     #if WINDOWS_APP && NOTANDROID
     public static double GetCurrentScreenHeight()
     {
        
         double h;
         double w;
         double ScreenHeight=0.0;
         try
         {
             ScreenHeight = Window.Current.Bounds.Height;
             var bounds = Window.Current.Bounds;
             w = bounds.Width;
             h = bounds.Height;
             if (w == 1024.0 && h == 768.0)
             {
                 ScreenHeight = h - 20.0;
             }
             if (w == 1366.0 && h == 768.0)
             {
                 ScreenHeight = h - 30.0;
             }
             if (w == 1920.0 && h == 1080.0)
             {
               ScreenHeight = h - 30.0;
             }
             if (w == 2560.0 && h == 1440.0)
             {
                 ScreenHeight = h - 30.0;
             }
             if (w == 1280.0 && h == 800.0)
             {
                 ScreenHeight = h - 30.0;
             }
             if (w == 1024.0 && h == 768.0)
             {
                 ScreenHeight = h - 30.0;
             }
         }
         catch (Exception ex)
         {
             
         }
         return ScreenHeight;
     }

     public static double GetCurrentScreenWidth()
     {
         double h;
         double w;
         double ScreenWidth=0.0;
         try
         {
             ScreenWidth = Window.Current.Bounds.Width;
             var bounds = Window.Current.Bounds;
             w = bounds.Width;
             h = bounds.Height;

             if (w == 1024.0 && h == 768.0)
             {
                 ScreenWidth = w - 20.0;
             }
             if (w == 1366.0 && h == 768.0)
             {
                 ScreenWidth = w - 30.0;
             }
             if (w == 1920.0 && h == 1080.0)
             {
                 ScreenWidth = w - 30.0;
             }
             if (w == 2560.0 && h == 1440.0)
             {
                 ScreenWidth = w - 30.0;
             }
             if (w == 1280.0 && h == 800.0)
             {
                 ScreenWidth = w - 30.0;
             }
             if (w == 1024.0 && h == 768.0)
             {
                 ScreenWidth = w - 30.0;
             }
         }
         catch (Exception ex)
         {
           
         }
         return ScreenWidth;
     }
#endif
    }
}