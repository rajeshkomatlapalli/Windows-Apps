using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideos.Entities
{
   [SQLite.Table("WebDailyTable")]
    public class WebDailyTable
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement, SQLite.Column("ID")]
        public int ID
        {
            get;
            set;
        }
         [SQLite.Column("ShowID")]
        public int ShowID
        {
            get;
            set;
        }

        [SQLite.Column("SelectedText")]
        public string SelectedText
        {
            get;
            set;
        }

        [SQLite.Column("Date")]
        public DateTime Date
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

    }
}
