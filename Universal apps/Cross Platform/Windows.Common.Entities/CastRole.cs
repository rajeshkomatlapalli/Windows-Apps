using System;
using System.Net;
using System.Windows;
using Windows.UI.Xaml.Media;
#if ANDROID
using Android.Graphics;
#endif

namespace OnlineVideos.Entities
{
    public class CastRole
    {
        long m_PersonID;
        public long PersonID
        {
            get { return m_PersonID; }
            set { m_PersonID = value; }
        }

        string m_PersonName;
        public string PersonName
        {
            get { return m_PersonName; }
            set { m_PersonName = value; }
        }

        string m_PersonRole;
        public string PersonRole
        {
            get { return m_PersonRole; }
            set { m_PersonRole = value; }
        }

        string m_PersonImage;
        public string PersonImage
        {
            get { return m_PersonImage; }
            set { m_PersonImage = value; }
        }

        long m_TeamID;
        public long TeamID
        {
            get { return m_TeamID; }
            set { m_TeamID = value; }
        }
		#if ANDROID
		Bitmap m_PersonBitmapImage;
		public Bitmap PersonBitmapImage
		{
			get{ return m_PersonBitmapImage; }
			set{ m_PersonBitmapImage = value; }
		}
		#endif

# if NOTANDROID
        ImageSource m_PersonImage1;
        public ImageSource PersonImage1
        {
            get { return m_PersonImage1; }
            set { m_PersonImage1 = value; }
        }
#endif
    }
}
