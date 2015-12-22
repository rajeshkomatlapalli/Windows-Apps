using Common.Library;
using Common.Utilities;
using OnlineMovies.Views;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineMovies.UserControls
{
    public sealed partial class QuizExitPopUp : UserControl
    {
        public QuizExitPopUp()
        {
            this.InitializeComponent();
            PageHelper.RemoveEntryFromBackStack("SubjectDetail");
        }

        private void btnok_Click(object sender, RoutedEventArgs e)
        {
            Close();
            PageHelper.RemoveEntryFromBackStack("QuizExitPopUp");           
            string[] parameters = new string[2];
            parameters[0] = AppSettings.ShowID;
            parameters[1] = null;
            Frame frame = Window.Current.Content as Frame;
            Page p = frame.Content as Page;
            p.Frame.Navigate(typeof(SubjectDetail), parameters);
            QuizManager.deletestoriongdata();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            SettingsHelper.Save("popclose", "true");
            Close();
        }
        public void Show()
        {
            popMessage.IsOpen = true;
        }

        public void Close()
        {
            popMessage.IsOpen = false;
        }
    }
}
