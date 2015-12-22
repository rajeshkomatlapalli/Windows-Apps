using System;
using System.Net;
using System.Windows;
using System.Windows.Input;


namespace OnlineVideos.Entities
{
    
    public class QuizUserAnswers
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int PrimaryID
        {
            get;
            set;
        }
        [SQLite.Column("ID")]
        public int ID
        {
            get;
            set;
        }
         [SQLite.Column("QuestionID")]
        public int QuestionID
        {
            get;
            set;
        }

        [SQLite.Column("UserAnswer")]
        public string UserAnswer
        {
            get;
            set;
        }
    }
}
