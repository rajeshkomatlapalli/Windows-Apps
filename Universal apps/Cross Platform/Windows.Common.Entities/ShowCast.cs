using System;
using System.Net;
using System.Windows;

namespace OnlineVideos.Entities
{
    [SQLite.Table("ShowCast"), ConditionType("ShowCast", "DeleteCondition")]
    public class ShowCast
    {
      [SQLite.Column("ShowID"),ConditionType("ShowID", "PrimaryCondition")]
        public int ShowID
        {
            get;
            set;
        }

       [SQLite.Column("PersonID")] 
        public int PersonID
        {
            get;
            set;
        }

       [SQLite.Column("RoleID")] 
        public int RoleID
        {
            get;
            set;
        }
      [SQLite.Column("CategoryID")] 
        public int CategoryID
        {
            get;
            set;
        }
     [SQLite.PrimaryKey,SQLite.AutoIncrement]
        public int CastID
        {
            get;
            set;
        }
    }
}
