using System;
using System.Net;
using System.Windows;
#if WP8
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes; 
#endif

namespace OnlineVideos.Common
{
    public interface INavigationContext
    {
        int ShowID
        {
            get;
            set;
        }

        string Title
        {
            get;
            set;
        }
    }
}
