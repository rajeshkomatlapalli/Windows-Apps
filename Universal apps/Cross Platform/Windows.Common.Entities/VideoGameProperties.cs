using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
#if WP8
using System.Windows.Ink;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
#endif

#if WINDOWS_PHONE_APP
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Controls;
#endif


namespace OnlineVideos.Entities
{
    public class VideoGameProperties
    {
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
        public string Image
        {
            get;
            set;
        }
      
        //public int VehicleId
        //{
        //    get;
        //    set;
        //}

       
        //public string VehicleName
        //{
        //    get;
        //    set;
        //}

        public string Description
        {
            get;
            set;
        }
        //public ImageSource VehicleImage
        //{
        //    get;
        //    set;
        //}
    }
}
