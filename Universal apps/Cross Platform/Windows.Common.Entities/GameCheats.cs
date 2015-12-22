using System;
using System.Net;
using System.Windows;
using System.Windows.Input;


namespace OnlineVideos.Entities
{
      [SQLite.Table("GameCheats"), ConditionType("GameCheats", "DeleteCondition")]
    public class GameCheats
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int ID
        {
            get;
            set;
        }
        [SQLite.Column("CheatID"), ConditionType("CheatID", "SecondaryCondition")]
        public int CheatID
        {
            get;
            set;
        }

           [SQLite.Column("CheatName")]
        public string CheatName
        {
            get;
            set;
        }

          [SQLite.Column("CheatDescription")]
        public string CheatDescription
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
          [SQLite.Ignore]
         public byte[] CheatData
         {
             get;
             set;
         }
          [SQLite.Column("FirstColumn")]
         public string FirstColumn
        {
            get;
            set;
        }
          [SQLite.Column("SecondColumn")]
         public string SecondColumn
        {
            get;
            set;
        }
    }
}
