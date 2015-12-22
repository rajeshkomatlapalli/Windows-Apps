using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
#if WINDOWS_APP
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using System.Linq;
#endif
using System.Xml.Serialization;

namespace OnlineVideos.Entities
{
    [SQLite.Table("Stories"), ConditionType("Stories", "DeleteCondition")]
    public class Stories
    {

        [XmlIgnore(), SQLite.Ignore]
        public double Imagewidth
        {
            get;
            set;
        }
        [XmlIgnore(), SQLite.Ignore]
        public double Imageheight
        {
            get;
            set;
        }
        [SQLite.Column("paraId")]
        public int paraId
        {
            get;
            set;
        }
        [SQLite.PrimaryKey, SQLite.AutoIncrement, SQLite.Column("ID")]
        public int ID
        {
            get;
            set;
        }
        [XmlIgnore(), SQLite.Ignore]
        public string StoryImage
        {
            get;
            set;
        }
        [XmlIgnore(), SQLite.Ignore]
        public string ImageVisibility
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
        [XmlIgnore(), SQLite.Ignore]
        public int val
        {
            get;
            set;
        }
        [XmlIgnore(), SQLite.Ignore]
        public string RemainingDescription
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

        [SQLite.Column("Image")]
        public string Image
        {
            get;
            set;
        }
        [SQLite.Column("LanguageID")]
        public int LanguageID
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
        [SQLite.Column("FlickrStoryImageUrl")]
        public string FlickrStoryImageUrl
        {
            get;
            set;
        }
          [SQLite.Ignore]
        public bool RecordCompleted
        {
            get;
            set;
        }
		#if ANDROID
		[XmlIgnore(), SQLite.Ignore]
		public Android.Graphics.Bitmap StoryImageForAndroid
		{
			get;
			set;
		}
		#endif
    }
    public class DataGroup
    {
        public DataGroup(string menu)
        {
            this.Menu = menu;
        }
        public string Menu
        {
            get;
            set;
        }
        private ObservableCollection<Stories> _items = new ObservableCollection<Stories>();
        public ObservableCollection<Stories> Items
        {
            get { return this._items; }
        }


        public IEnumerable<Stories> TopItems
        {
            get { return this._items.Take(2); }
        }

    }
    public class DataGroups
    {
        private ObservableCollection<DataGroup> _items = new ObservableCollection<DataGroup>();
        public ObservableCollection<DataGroup> Items
        {
            get { return this._items; }
        }


    }
}
