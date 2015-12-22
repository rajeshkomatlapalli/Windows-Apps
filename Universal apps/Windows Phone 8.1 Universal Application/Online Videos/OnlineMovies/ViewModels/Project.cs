using System;
using System.Net;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;
using System.Windows.Input;

namespace OnlineVideos.ViewModels
{
    public class Project
    {
       
        private string _chapid;
        public string chapid
        {
            get { return _chapid; }
            set { _chapid = value; }
        }
        private string _background;
        public string background
        {
            get { return _background; }
            set { _background = value; }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private int _count;
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
       
        private string _path;
        public string path
        {
            get { return _path; }
            set { _path = value; }
        }
        private Uri _uri;
        public Uri uri
        {
            get { return _uri; }
            set { _uri = value; }
        }
        private string _image;
        public string image
        {
            get { return _image; }
            set { _image = value; }
        }
     
    }
}
