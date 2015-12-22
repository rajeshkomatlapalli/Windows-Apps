using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideos.Entities
{
   [SQLite.Table("WebInformation")]
    public class WebInformation
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
          [SQLite.Column("BeforeText")]
        public string BeforeText
        {
            get;
            set;
        }
        [SQLite.Column("AfterText")]
        public string AfterText
        {
            get;
            set;
        }
       [SQLite.Column("NavigationUri")]
        public string NavigationUri
        {
            get;
            set;
        }



    }
}
