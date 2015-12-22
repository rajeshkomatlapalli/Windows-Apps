using System;
using System.Net;
using System.Windows;
using System.Windows.Input;


namespace OnlineVideos.Entities
{
     [SQLite.Table("ShowCategories")]
    public class ShowCategories
    {
         [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int CategoryID
        {
            get;
            set;
        }

          [SQLite.Column("CategoryName")]
        public string CategoryName
        {
            get;
            set;
        }
          [SQLite.Column("Abbreviation")]
        public string Abbreviation
        {
            get;
            set;
        }
    }
}
