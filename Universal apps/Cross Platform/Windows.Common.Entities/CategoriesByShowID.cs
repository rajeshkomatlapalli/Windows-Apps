using System;
using System.Net;
using System.Windows;
using System.Windows.Input;


namespace OnlineVideos.Entities
{
    [SQLite.Table("CategoriesByShowID"), ConditionType("CategoriesByShowID", "DeleteCondition")]
    public class CategoriesByShowID
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int ID
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
             [SQLite.Column("CatageoryID"), ConditionType("CatageoryID", "SecondaryCondition")]
           public int CatageoryID
        {
            get;
            set;
        }

       
     }
}
