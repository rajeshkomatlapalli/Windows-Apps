using System;
using System.Net;
using System.Windows;
using System.Windows.Input;


namespace OnlineVideos.Entities
{
   [SQLite.Table("SearchHistory")]
    public class SearchHistory
    {
          [SQLite.Column("ID"), SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int ID
        {
            get;
            set;
        }

          [SQLite.Column("ShowName")]
        public string ShowName
        {
            get;
            set;
        }

         [SQLite.Column("SearchCount")]
        public int SearchCount
        {
            get;
            set;
        }
    }
}
