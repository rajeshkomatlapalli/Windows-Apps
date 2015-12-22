using Common.Library;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using Windows.UI.Xaml;
//using System.Windows.Data;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Threading;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace OnlineVideos.View_Models
{
    public class ImageConverter : IValueConverter
    {

        public object Convert(object value, System.Type type, object parameter, string language)
        {
            ImageSource returnimage = default(ImageSource);
            if (value == null || value=="") return null;
            Constants.UIThread = true;
            returnimage= new BitmapImage(new Uri( ResourceHelper.getStoryImageFromStorageOrInstalledFolder(value.ToString()), UriKind.RelativeOrAbsolute));
           Constants.UIThread = false;
           return returnimage;
        }


        public object ConvertBack(object value, System.Type type, object parameter, string language)
        {
            return null;
        }
    }
    public class ImageVisibility : IValueConverter
    {

        public object Convert(object value, System.Type type, object parameter, string language)
        {
            if (string.IsNullOrEmpty(value.ToString()))
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
           
        }
        public object ConvertBack(object value, System.Type type, object parameter, string language)
        {
            return null;
        }
    }
}