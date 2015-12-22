using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.UI.Xaml.Media;

namespace OnlineVideos.Entities
{
   public class LinkHistory
    {
        long m_ShowID;
        public long ShowID
        {
            get { return m_ShowID; }
            set { m_ShowID = value; }
        }

#if NOTANDROID
        private ImageSource _imageSource;
        public ImageSource ImgSource
        {
            get { return _imageSource; }
            set { _imageSource = value; }

        }
        public ImageSource Thumbnail
        {
            get;
            set;
        }
#endif
        string m_LinkTitle;
        public string LinkTitle
        {
            get { return m_LinkTitle; }
            set { m_LinkTitle = value; }
        }

        string m_LinkUrl;
        public string LinkUrl
        {
            get { return m_LinkUrl; }
            set { m_LinkUrl = value; }
        }
        private string _songPlay;
        public string Songplay
        {
            get { return _songPlay; }
            set { _songPlay = value; }

        }
        private string _id;
        public string ID
        {
            get { return _id; }
            set { _id = value; }

        }
       
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }

        }
        private string _genre;
        public string Genre
        {
            get { return _genre; }
            set { _genre = value; }

        }
        private string _imageRating;
        public string ImageRating
        {
            get { return _imageRating; }
            set { _imageRating = value; }

        }
        private string _subTitle;
        public string SubTitle
        {
            get { return _subTitle; }
            set { _subTitle = value; }

        }

        private string _releaseDate;
        public string ReleaseDate
        {
            get { return _releaseDate; }
            set { _releaseDate = value; }

        }

		#if ANDROID
		private string _thumbnail;
		public string Thumbnail
		{
			get{ return _thumbnail; }
			set{ _thumbnail = value; }
		}
		private double _rating;
		public double Rating
		{
			get{return _rating; }
			set{ _rating=value; }
		}
		public string Status
		{
			get;
			set;
		}

		#endif
    }
}
