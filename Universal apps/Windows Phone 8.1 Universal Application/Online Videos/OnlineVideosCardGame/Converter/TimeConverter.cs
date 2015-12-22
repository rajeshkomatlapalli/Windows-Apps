using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace OnlineVideosCardGame.Converter
{
   public class TimeConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, string culture)
        {

            if (value == null) return "00:00";
            int time = (int)value;
            int minutes = time / 60;
            int secs = time % 60;
            return String.Format("{0}:{1}", minutes.ToString("D2"), secs.ToString("D2"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return default(object);
        }

        #endregion
    }
}
