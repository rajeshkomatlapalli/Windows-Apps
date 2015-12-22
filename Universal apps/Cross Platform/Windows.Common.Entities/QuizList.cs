using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
#if WINDOWS_APP
using Windows.UI.Xaml.Media;
#endif

namespace OnlineVideos.Entities
{
    [SQLite.Table("QuizList"), ConditionType("QuizList", "DeleteCondition")]
    public class QuizList
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement, SQLite.Column("ID")]
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
        [SQLite.Column("QuizID"), ConditionType("QuizID", "SecondaryCondition")]
        public int QuizID
        {
            get;
            set;
        }
        [SQLite.Column("Name")]
        public string Name
        {
            get;
            set;
        }
        [SQLite.Column("Timelimt")]
        public string Timelimt
        {
            get;
            set;
        }
        [SQLite.Column("Rating")]
        public double Rating
        {
            get;
            set;
        }
        [SQLite.Column("IsFavourite")]
        public bool IsFavourite
        {
            get;
            set;
        }
        [SQLite.Column("Result")]
        public string Result
        {
            get;
            set;
        }
        [SQLite.Column("LinkID")]
        public int LinkID
        {
            get;
            set;
        }

        [SQLite.Ignore]
        public string RatingImage
        {
            get;
            set;
        }
        [XmlIgnore(), SQLite.Ignore]
        public string RatingBitmapImage
        {
            get;
            set;
        }
    }
}
