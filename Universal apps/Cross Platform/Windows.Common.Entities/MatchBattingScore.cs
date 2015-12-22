using SQLite;
using System;
using System.Net;
using System.Windows;
using System.Windows.Input;


namespace OnlineVideos.Entities
{
    [SQLite.Table("MatchBattingScore"), ConditionType("MatchBattingScore", "DeleteCondition")]
    public class MatchBattingScore
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement,ConditionType("ID", "DoNotInsert")]
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

        [SQLite.Column("MatchId"), ConditionType("MatchId", "PrimaryCondition")]
        public int MatchId
        {
            get;
            set;
        }
          [SQLite.Column("PlayerID")]
        public string PlayerID
        {
            get;
            set;
        }
        [SQLite.Column("BatsmanId")]
        public string BatsmanId
        {
            get;
            set;
        }
           [SQLite.Column("PlayerName")]
        public string PlayerName
        {
            get;
            set;
        }
           [SQLite.Column("Out")]
        public string Out
        {
            get;
            set;
        }
            [SQLite.Column("Runs")]
        public int Runs
        {
            get;
            set;
        }
           [SQLite.Column("Balls")]
        public string Balls
        {
            get;
            set;
        }
          [SQLite.Column("BatType")]
        public string BatType
        {
            get;
            set;
        }
         [SQLite.Column("TeamType")]
        public string TeamType
        {
            get;
            set;
        }
       
    }
}
