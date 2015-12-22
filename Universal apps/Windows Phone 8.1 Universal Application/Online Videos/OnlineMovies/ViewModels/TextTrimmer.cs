using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using Windows.UI.Xaml.Data;
//using System.Windows.Data;
//using System.Windows.Media.Imaging;
//using System.Windows.Threading;

namespace OnlineVideos.View_Models
{
    public class TextTrimmer : IValueConverter
    {

        public object Convert(object value, System.Type type, object parameter, string language)
        {
            if (value == null) return null;
            int maxLength = 50;
            int strLength = 0;
            string fixedString = value.ToString();
           
            strLength = fixedString.ToString().Length;
            if (strLength == 0)
            {
                return null;
            }
            else if (strLength >= maxLength)
            {
                fixedString = fixedString.Substring(0, maxLength);
                //fixedString = fixedString.Substring(0, fixedString.LastIndexOf(" "));
                fixedString += "...";
            }
          
            return fixedString;
        }


        public object ConvertBack(object value, System.Type type, object parameter, string language)
        {
            return null;
        }
    }
   
}
