using System;
using System.Net;
using System.Windows;


namespace OnlineVideos.Entities
{
    [ConditionType("QuizImages", "DownloadCondition")]
    public class QuizImages
    {
        [ConditionType("ImageNo", "SecondaryCondition"), SQLite.Column("ImageNo")]
        public string ImageNo
        {
            get;
            set;
        }
        [ConditionType("ShowID", "PrimaryCondition"), SQLite.Column("ShowID")]
        public string ShowID
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
    }
}
