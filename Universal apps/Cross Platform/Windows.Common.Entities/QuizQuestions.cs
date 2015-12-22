using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using Windows.UI.Xaml.Media;

namespace OnlineVideos.Entities
{
    [SQLite.Table("QuizQuestions"), ConditionType("QuizQuestions", "DeleteCondition")]
    public class QuizQuestions
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
        [SQLite.Column("QuestionID")]
        public int QuestionID
        {
            get;
            set;
        }

        [SQLite.Column("Question")]
        public string Question
        {
            get;
            set;
        }

        [SQLite.Column("Answer")]
        public string Answer
        {
            get;
            set;
        }
        [SQLite.Column("Option1")]
        public string Option1
        {
            get;
            set;
        }
        [SQLite.Column("Option2")]
        public string Option2
        {
            get;
            set;
        }
        [SQLite.Column("Option3")]
        public string Option3
        {
            get;
            set;
        }
        [SQLite.Column("Option4")]
        public string Option4
        {
            get;
            set;
        }
        [XmlIgnore(), SQLite.Ignore]
        public string QuestionType
        {
            get;
            set;
        }
        [XmlIgnore(), SQLite.Ignore]
        public string UserAnswer
        {
            get;
            set;
        }
        [SQLite.Column("QuizNo"), ConditionType("QuizNo", "SecondaryCondition")]
        public int QuizNo
        {
            get;
            set;
        }
        [SQLite.Column("Image")]
        public string Image
        {
            get;
            set;
        }
        [SQLite.Column("FlickrQuizImageUrl")]
        public string FlickrQuizImageUrl
        {
            get;
            set;
        }
		#if NOTANDROID
        [SQLite.Ignore]
        public ImageSource ImgQuestion
        {
            get;
            set;
        }
		#endif
    }
}
