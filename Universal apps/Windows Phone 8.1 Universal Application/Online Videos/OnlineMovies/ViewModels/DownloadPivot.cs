using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace OnlineVideos
{
    public class DownloadPivot : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler ph = PropertyChanged;
            if (ph != null)
                ph(this, new PropertyChangedEventArgs(propertyName));

        }

        #endregion
        private Uri _downloaduri;
        public Uri Downloaduri
        {
            get { return this._downloaduri; }

            set
            {
                if (value != this._downloaduri)
                {
                    this._downloaduri = value;
                    OnPropertyChanged("Downloaduri");
                }
            }
        }

        private ImageSource _downloadimage;
        public ImageSource Downloadimage
        {
            get { return this._downloadimage; }

            set
            {
                if (value != this._downloadimage)
                {
                    this._downloadimage = value;
                    OnPropertyChanged("Downloadimage");
                }
            }
        }
        //private ImageSource _downloadimage;
        //public ImageSource Downloadimage
        //{
        //    get { return _downloadimage; }
        //    set { _downloadimage = value; }
        //}
        //private Uri _downloaduri;
        //public Uri Downloaduri
        //{
        //    get { return _downloaduri; }
        //    set { _downloaduri = value; }
        //}
        private string _title;
        public string title
        {
            get { return this._title; }

            set
            {
                if (value != this._title)
                {
                    this._title = value;
                    OnPropertyChanged("title");
                }
            }
        }
        private string _DownLoadVideoImage;
        public string DownLoadVideoImage
        {
            get { return _DownLoadVideoImage; }
            set { _DownLoadVideoImage = value; }
        }
        private string _size;
        public string size
        {
            get { return _size; }
            set { _size = value; }
        }
    }
}
