using System;
using System.Net;
using System.Windows;
using System.Windows.Input;


namespace OnlineVideos.Entities
{
     [SQLite.Table("MatchExtras"), ConditionType("MatchExtras", "DeleteCondition")]
    public class MatchExtras
    {
         [SQLite.PrimaryKey, SQLite.AutoIncrement, ConditionType("ID", "DoNotInsert")]
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
           [SQLite.Column("TeamAExtras")]
        public string TeamAExtras
        {
            get;
            set;
        }
         [SQLite.Column("TeamBExtras")]
        public string TeamBExtras
        {
            get;
            set;
        }
           [SQLite.Column("TeamATotal")]
        public string TeamATotal
        {
            get;
            set;
        }
           [SQLite.Column("TeamBTotal")]
        public string TeamBTotal                                                            
        {
            get;
            set;
        }
         [SQLite.Column("TeamAInnings")]
        public string TeamAInnings
        {
            get;
            set;
        }
         [SQLite.Column("TeamBInnings")]
        public string TeamBInnings
        {
            get;
            set;
        }
        [SQLite.Column("TeamResult")]
        public string TeamResult
        {
            get;
            set;
        }
       [SQLite.Column("MatchID"), ConditionType("MatchID", "PrimaryCondition")]
        public int MatchID
        {
            get;
            set;
        }

    }
}
