using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Xml.Serialization;
using System.Linq;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
#if WP8
using System.Windows.Media;
using System.Windows.Media.Imaging;
#endif
#if WINDOWS_APP
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;
#endif

namespace OnlineVideos.Entities
{
   [SQLite.Table("ShowList"), ConditionType("ShowList","PrimaryCondition")]
    public class ShowList
    {
       public ShowList()
       {
       }
       public ShowList(string LandingImage, int ShowID, string Title, string TileImage)
       {
           this.ShowID = ShowID;
           this.Title = Title;
           this.TileImage = TileImage;
           this.LandingImage = LandingImage;
       }
      [SQLite.PrimaryKey,ConditionType("ShowID", "PrimaryCondition")]
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

       [SQLite.Column("TileImage")]
        public string TileImage
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
[SQLite.Column("ReleaseDate")]
        public string ReleaseDate
        {
            get;
            set;
        }
[SQLite.Ignore]
public string Height
{
    get;
    set;
}
[SQLite.Ignore]
public string Width
{
    get;
    set;
}
       [SQLite.Column("Genre")]
        public string Genre
        {
            get;
            set;
        }
       [SQLite.Column("CreatedDate")]
       public DateTime CreatedDate
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
       [SQLite.Column("SubTitle")]
        public string SubTitle
        {
            get;
            set;
        }
       [SQLite.Column("IsHidden")]
       public bool IsHidden
        {
            get;
            set;
        }
       [SQLite.Column("PivotImage")]
        public string PivotImage 
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

       [SQLite.Column("ClientShowUpdated")]
       public DateTime ClientShowUpdated
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
      [SQLite.Column("ShareStatus")]
      public string ShareStatus
      {
          get;
          set;
      }
      [SQLite.Ignore]
      public int ReleaseDateyear
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
       [SQLite.Column("RatingImage")]
        public string RatingImage
        {
            get;
            set;
        }
       [SQLite.Ignore]
        public string RemoveShow
        {
            get;
            set;
        }
       
        private string _filesize;
        [SQLite.Ignore]
        public string FileSize
        {
            get { return _filesize; }
            set { _filesize = value; }

        }
        [XmlIgnore(), SQLite.Ignore]
        public string LandingImage
        {
            get;
            set;
        }
        //[XmlIgnore(), SQLite.Ignore]
        //public BitmapImage LandingImage1
        //{
        //    get;
        //    set;
        //}
        [XmlIgnore(), SQLite.Ignore]
        public string RatingBitmapImage
        {
            get;
            set;
        }
      
         [SQLite.Column("Status")]
        public string Status
        {
            get;
            set;
        }
        [SQLite.Ignore]
        public string TitleForDownLoad
        {
            get;
            set;
        }
#if NOTANDROID
        [XmlIgnore(), SQLite.Ignore]
        public ImageSource Image
        {
            get;
            set;
        }
#endif
    }
   public class ListGroup
   {
#if NOTANDROID
       public ListGroup(string menu,Thickness margin)
       {
           this.Menu = menu;
           this.margin = margin;
       }
       public string Menu
       {
           get;
           set;
       }
       public Thickness margin
       {
           get;
           set;
       }
#endif
       private ObservableCollection<ShowList> _items = new ObservableCollection<ShowList>();
       public ObservableCollection<ShowList> Items
       {
           get { return this._items; }
       }
       public IEnumerable<ShowList> TopItems
       {
           get { return this._items; }
       }
   }
   public class ListGroups
   {
       private ObservableCollection<ListGroup> _items = new ObservableCollection<ListGroup>();
       public ObservableCollection<ListGroup> Items
       {
           get { return this._items; }
       }
   }
}
