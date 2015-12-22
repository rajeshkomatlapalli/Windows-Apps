using System;
using System.Net;
using System.Windows;
using System.Xml.Serialization;
#if WINDOWS_APP
using Windows.UI.Xaml.Media;
#endif

namespace OnlineVideos.Entities
{
   [SQLite.Table("CastProfile"), ConditionType("CastProfile", "PrimaryCondition")]
    public class CastProfile
    {


       [SQLite.PrimaryKey, ConditionType("PersonID", "PrimaryCondition")]
        public int PersonID
        {
            get;
            set;
        }

    [SQLite.Column("Name")]
        public string Name
        {
            get;
            set;
        }
    [SQLite.Column("Rating")]
    public float Rating
    {
        get;
        set;
    }
         [SQLite.Ignore]
        public string Image
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
     [SQLite.Column("GalleryImageCount")]
        public int GalleryImageCount
        {
            get;
            set;
        }
     [SQLite.Column("FlickrPanoramaImageUrl")]
     public string FlickrPanoramaImageUrl
     {
         get;
         set;
     }
     [SQLite.Column("FlickrPersonImageUrl")]
     public string FlickrPersonImageUrl
     {
         get;
         set;
     }
        [XmlIgnore(),SQLite.Ignore]
        public string Descriptiontitle
        {
            get;
            set;
        }
        [XmlIgnore(), SQLite.Ignore]
        public bool IsCelebrityPublished
        {
            get;
            set;
        }
        [XmlIgnore(), SQLite.Ignore]
        public string RatingBitmapImage
        {
            get;
            set;
        }
       
    }
}
