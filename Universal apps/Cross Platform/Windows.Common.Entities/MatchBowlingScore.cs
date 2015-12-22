using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;


namespace OnlineVideos.Entities
{
    [SQLite.Table("MatchBowlingScore"), ConditionType("MatchBowlingScore", "DeleteCondition")]
    public class MatchBowlingScore
    {
         [SQLite.PrimaryKey, SQLite.AutoIncrement,ConditionType("ID", "DoNotInsert")] 
        public int ID
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
         [SQLite.Column("PlayerID")]
         public string PlayerID
         {
             get;
             set;
         }
          [SQLite.Column("Overs")]
        public double Overs
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
        [SQLite.Column("Maidens")]
        public int Maidens
        {
            get;
            set;
        }
         [SQLite.Column("Wickets")]
        public int Wickets
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
        [SQLite.Column("MatchId"), ConditionType("MatchId", "PrimaryCondition")]
         public int MatchId
        {
            get;
            set;
        }
       [XmlIgnore(), SQLite.Ignore]
        public string BowlerID
        {
            get;
            set;
        }

        [XmlIgnore(), SQLite.Ignore]
        public string BowlBalls
        {
            get;
            set;
        }
        [XmlIgnore(), SQLite.Ignore]
        public string BowlType
        {
            get;
            set;
        }

       

        
    }
}
