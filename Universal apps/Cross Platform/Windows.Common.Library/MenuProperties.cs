using System;
using System.Net;
using System.Windows;
using Windows.UI.Xaml.Media;
#if WP8 && NOTANDROID
using System.Windows.Media;
#endif
#if WINDOWS_APP && NOTANDROID
using Windows.UI.Xaml.Media;
#endif
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;

namespace Common
{
    public class MenuProperties
    {
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _Url;
        public string Url
        {
            get { return _Url; }
            set { _Url = value; }
        }
		#if NOTANDROID
        private ImageSource _imgSource;
        public ImageSource ImgSource
        {
            get { return _imgSource; }
            set { _imgSource = value; }
        }
#endif
        private string _topics;
        public string Topics
        {
            get { return _topics; }
            set { _topics = value; }
        }
        private string _ImageUri;
        public string ImageUri
        {
            get { return _ImageUri; }
            set { _ImageUri = value; }
        }
    }
}
