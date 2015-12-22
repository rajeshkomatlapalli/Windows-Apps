using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
//using System.Windows.Data;
//using System.Windows.Media.Imaging;
//using System.Windows.Threading;
using Windows.UI.Xaml.Data;

namespace OnlineVideos.View_Models
{
 public class ParaConverter : IValueConverter
    {

     public object Convert(object value, System.Type type, object parameter, string language)
        {
            if (value == null) return null;
            return "Paragraph" + " " + value.ToString();
        }


     public object ConvertBack(object value, System.Type type, object parameter, string language)
        {
            return null;
        }
    }
}

