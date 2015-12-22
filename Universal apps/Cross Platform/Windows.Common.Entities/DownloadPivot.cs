using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace OnlineVideos.Entities
{
  public  class DownloadPivot
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
#if NOTANDROID
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
#endif
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
		#if ANDROID
		private string _DownLoadImage;
		public string DownLoadImage
		{
			get { return _DownLoadImage; }
			set { _DownLoadImage = value; }
		}
		#endif
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
        private string _time;
        public string time
        {
            get { return this._time; }

            set
            {
                if (value != this._time)
                {
                    this._time = value;
                    OnPropertyChanged("time");
                }
            }
        }
        private string _views;
        public string views
        {
            get { return this._views; }

            set
            {
                if (value != this._views)
                {
                    this._views = value;
                    OnPropertyChanged("views");
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
        private string _height; 
        [SQLite.Ignore]
        public string height
        {
            get { return _height; }
            set { _height = value; }
        }
        private bool _IsChecked; 
       [SQLite.Ignore]
        public bool IsChecked
        {
            get { return _IsChecked; }
            set { _IsChecked = value; }
        }

       private string _Html;
       public string YoutubeUrl
       {
           get { return _Html; }
           set { _Html = value; }
       }
    }
}
