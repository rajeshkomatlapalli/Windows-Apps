using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
#if ANDROID
using Android.Graphics;
#endif

namespace OnlineVideos.Entities
{
 public    class GalleryImageInfo
    {

        long m_ImageID;
        public long ImageID
        {
            get { return m_ImageID; }
            set { m_ImageID = value; }
        }

        string m_Title;
        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }
#if ANDROID
        string m_FullImage;
        public string FullImage
        {
            get { return m_FullImage; }
            set { m_FullImage = value; }
        }
#endif

        int PersonId;
        public int PersonID
        {
            get { return PersonId; }

            set { PersonId = value; }
        }
        string m_ThumbnailImage;
        public string ThumbnailImage
        {
            get { return m_ThumbnailImage; }
            set { m_ThumbnailImage = value; }
        }
		#if ANDROID
		Bitmap m_PersonBitmapImage;
		public Bitmap PersonBitmapImage
		{
			get{ return m_PersonBitmapImage; }
			set{ m_PersonBitmapImage = value; }
		}
		#endif
#if NOTANDROID
        ImageSource m_Thumbnail;
        public ImageSource Thumbnail
        {
            get { return m_Thumbnail; }
            set { m_Thumbnail = value; }
        }
        ImageSource m_FullImage;
        public ImageSource FullImage
        {
            get { return m_FullImage; }
            set { m_FullImage = value; }
        }
#endif
            int _gallcount;
        public int Gallcount
        {
            get { return _gallcount; }
            set { _gallcount = value; }
        }
        int imageno;
        public int ImageNo
        {
            get { return imageno; }

            set { imageno = value; }
        }
#if IOS || ANDROID
		string m_FullImage;
		public string FullImage
		{
		get { return m_FullImage; }
		set { m_FullImage = value; }
		}
#endif

    }

  
}
