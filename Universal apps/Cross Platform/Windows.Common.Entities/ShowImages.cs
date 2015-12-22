using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideos.Entities
{
     [ConditionType("ShowImages", "DownloadCondition")]
    public class ShowImages
    {

        [ConditionType("TileImage", "SecondaryCondition")]
        public string TileImage
        {
            get;
            set;
        }
        [ConditionType("ShowID", "PrimaryCondition")]
        public string ShowID
        {
            get;
            set;
        }
#if WINDOWS_PHONE_APP && NOTANDROID
        public string PivotImage
        {
            get;
            set;
        }
        public string FlickrImageUrl
        {
            get;
            set;
        }
        public string FlickrPivotImageUrl
        {
            get;
            set;
        }
#endif
#if WINDOWS_APP
        [SQLite.Column("Scale100")]
        public string Scale100
        {
            get;
            set;
        }
        [SQLite.Column("Scale140")]
        public string Scale140
        {
            get;
            set;
        }
        [SQLite.Column("Scale180")]
        public string Scale180
        {
            get;
            set;
        }
        [SQLite.Column("ListImages")]
        public string ListImages
        {
            get;
            set;
        }
        [SQLite.Column("Tile30_30")]
        public string Tile30_30
        {
            get;
            set;
        }
        [SQLite.Column("Tile150_150")]
        public string Tile150_150
        {
            get;
            set;
        }
#endif
		#if ANDROID
		public string FlickrImageUrl
		{
			get;
			set;
		}
		#endif
    }
}
