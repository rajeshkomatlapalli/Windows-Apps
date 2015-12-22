using Common.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineVideos
{   
    public sealed partial class Browser : Page
    {
        string backentryid = string.Empty;
        bool web = false;        
        public Browser()
        {
            this.InitializeComponent();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {               
                if(backentryid!=null)
                {
                    web = true;
                    webview1.Navigate(new Uri("http://www.youtube.com/embed/" + backentryid + "?autoplay=1&origin=http://example.com", UriKind.RelativeOrAbsolute));
                }
            }
            catch (Exception ex)
            {
                string mess = "Exception in PhoneApplicationPage_Loaded Method In WebBrowserPage file.\n\n" + ex.Message + "\n\n Stack Trace:- " + ex.StackTrace;
                ex.Data.Add("Date", DateTime.Now);
                Exceptions.SaveOrSendExceptions("Exception in MainPage_Loaded Method In Browser.cs file.", ex);
            }
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {           
            backentryid = (string)e.Parameter;
            if (web == true)
            {
                web = false;               
                Frame.GoBack();
            }
            //HardwareButtons.BackPressed+=HardwareButtons_BackPressed;
        }
        

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (ResourceHelper.AppName != Apps.Indian_Cinema_Pro.ToString() && ResourceHelper.AppName != Apps.Kids_TV_Pro.ToString() && ResourceHelper.AppName != Apps.Story_Time_Pro.ToString())
                AppSettings.startplaying = false;
            else
                AppSettings.startplayingforpro = false;
            
            while (Frame.BackStack.Count() > 1)
            {
                if (Frame.BackStack.FirstOrDefault().SourcePageType.Equals("Youtube") || Frame.BackStack.FirstOrDefault().SourcePageType.Equals("Advertisement"))
                {
                    Frame.BackStack.RemoveAt(-1);
                }
                else
                    break;
            }      

            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            
        }
    }
}
