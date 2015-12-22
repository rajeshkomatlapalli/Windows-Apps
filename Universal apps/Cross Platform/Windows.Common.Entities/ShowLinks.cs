using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Xml.Serialization;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
#if ANDROID
 using Android.Graphics;
#endif

namespace OnlineVideos.Entities
{
    [SQLite.Table("ShowLinks"), ConditionType("ShowLinks", "DeleteCondition")]
    public class ShowLinks : INotifyPropertyChanged

    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void raisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion                
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int LinkID
        {
            get;
            set;
        }

        [SQLite.Column("ShowID"), ConditionType("ShowID", "PrimaryCondition")]
        public int ShowID
        {
            get;
            set;
        }

        [SQLite.Column("Title")]
        public string Title
        {
            get;
            set;
        }
        
        [SQLite.Ignore, XmlIgnore()]
        public bool StatusCode
        {
            get;
            set;
        }

        [SQLite.Column("LinkUrl")]
        public string LinkUrl
        {
            get;
            set;
        }
        [SQLite.Column("Rating")]
        public double Rating
        {
            get;
            set;
        }

        [SQLite.Column("IsFavourite")]
        public bool IsFavourite
        {
            get;
            set;
        }
        [SQLite.Column("LinkOrder")]
        public int LinkOrder
        {
            get;
            set;
        }
        [SQLite.Column("LinkType"), ConditionType("LinkType", "SecondaryCondition")]
        public string LinkType
        {
            get;
            set;
        }
        [SQLite.Ignore]
        public int BrokenLinkCount
        {
            get;
            set;
        }
        [SQLite.Column("UrlType")]
        public string UrlType
        {
            get;
            set;
        }
        [SQLite.Column("ClientShowUpdated")]
        public DateTime ClientShowUpdated
        {
            get;
            set;

        }
        [SQLite.Column("IsHidden")]
        public Boolean IsHidden
        {
            get;
            set;
        }
        [SQLite.Column("ClientPreferenceUpdated")]
        public DateTime ClientPreferenceUpdated
        {
            get;
            set;

        }
        [SQLite.Column("RatingFlag")]
        public int RatingFlag
        {
            get;
            set;
        }

        [SQLite.Column("Description")]
        public string Description
        {
            get;
            set;

        }
        [SQLite.Column("DownloadStatus")]
        public string DownloadStatus
        {
            get;
            set;

        }
        [SQLite.Column("UniqueDownloadID")]
        public string UniqueDownloadID
        {
            get;
            set;

        }


        [SQLite.Ignore, XmlIgnore()]
        public string Height
        {
            get;
            set;
        }
        [SQLite.Ignore, XmlIgnore()]
        public string Width
        {
            get;
            set;
        }
        [SQLite.Ignore, XmlIgnore()]
        public string Status
        {
            get;
            set;
        }
        [XmlIgnore(), SQLite.Ignore]
        public string ThumbnailImage
        {
            get;
            set;
        }
        [XmlIgnore()]
        private string _DownStatus;
        [SQLite.Ignore]
        public string DownStatus
        {
            get
            {
                return _DownStatus;
            }

            set
            {
                if (_DownStatus == value)
                {
                    return;
                }

                _DownStatus = value;

                // Update bindings, no broadcast
                raisePropertyChanged(DownStatus);
            }
        }
        [XmlIgnore()]
        private string _RatingBitmapImage;
        [SQLite.Ignore]
        public string RatingBitmapImage
        {
            get { return _RatingBitmapImage; }
            set { _RatingBitmapImage = value; }
        }
        [XmlIgnore()]
        private string _songNo;
        [SQLite.Ignore]
        public string SongNO
        {
            get { return _songNo; }
            set { _songNo = value; }

        }
        [XmlIgnore()]
        private string _songPlay;
        [SQLite.Ignore]
        public string Songplay
        {
            get { return _songPlay; }
            set { _songPlay = value; }

        }

        [XmlIgnore()]
        private string _ringtonedetails;
        [SQLite.Ignore]
        public string RingToneDetails
        {
            get { return _ringtonedetails; }
            set { _ringtonedetails = value; }

        }
        [XmlIgnore()]
        private string _ContextRemoveShow;
        [SQLite.Ignore]
        public string ContextRemoveShow
        {
            get { return _ContextRemoveShow; }
            set { _ContextRemoveShow = value; }

        }

        [XmlIgnore()]
        private string _contextvalue;
        [SQLite.Ignore]
        public string Contextvalue
        {
            get { return _contextvalue; }
            set { _contextvalue = value; }
        }
        [XmlIgnore()]
        private string _movietitle;
        [SQLite.Ignore]
        public string movietitle
        {
            get { return _movietitle; }
            set { _movietitle = value; }
        }
#if ANDROID
		[XmlIgnore()]
		private string _thumbnail;
		[SQLite.Ignore]
		public string Thumbnail
		{
			get { return _thumbnail; }
			set { _thumbnail = value; }
		}

#if ANDROID
		Bitmap m_VideoBitmapImage;
		[SQLite.Ignore,XmlIgnore()]
		public Bitmap VideoBitmapImage
		{
			get{ return m_VideoBitmapImage; }
			set{ m_VideoBitmapImage = value; }
		}
#endif



#endif
#if NOTANDROID
        [XmlIgnore(), SQLite.Ignore]
        public ImageSource Thumbnail
        {
            get;
            set;
        }
        private ImageSource _downloadedimage;
        [XmlIgnore(), SQLite.Ignore]
        public ImageSource DownloadedImage
        {
            get { return _downloadedimage; }
            set { _downloadedimage = value; }

        }
        private Visibility _DownloadIconVisible;
        [XmlIgnore(), SQLite.Ignore]
        public Visibility DownloadIconVisible
        {
            get;
            set;
        }
        private Visibility _txtboxVisibility;
        [XmlIgnore(), SQLite.Ignore]
        public Visibility txtboxVisibility
        {
            get;
            set;
        }
        [XmlIgnore(), SQLite.Ignore]
        public Color Foreground
        {
            get;
            set;
        }
#endif
    }
}
