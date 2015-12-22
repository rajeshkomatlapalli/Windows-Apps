using Common.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace OnlineVideosWin81.Controls
{
    public class VideoMixBorder : IValueConverter
    {
        public object Convert(object value, System.Type type, object parameter, string language)
        {
            if (AppSettings.ProjectName == "Video Mix") return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, System.Type type, object parameter, string language)
        {
            return null;
        }
    }

    public class AllAppsBorder : IValueConverter
    {
        public object Convert(object value, System.Type type, object parameter, string language)
        {
            if (AppSettings.ProjectName != "Video Mix") return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, System.Type type, object parameter, string language)
        {
            return null;
        }
    }
}