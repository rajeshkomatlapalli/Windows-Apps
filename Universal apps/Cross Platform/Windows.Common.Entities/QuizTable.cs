using System;
using System.Net;
using System.Windows;


namespace OnlineVideos.Data
{
    
    public class QuizTable
    {
        public int Id
        {
            get;
            set;
        }
        public int MovieId
        {
            get;
            set;
        }
        public int QuizId
        {
            get;
            set;
        }
        public string QuizName
        {
            get;
            set;
        }
        public string Timelimt
        {
            get;
            set;
        }
        public string Rating
        {
            get;
            set;
        }
        public string Favorite
        {
            get;
            set;
        }
        public string QuizResult
        {
            get;
            set;
        }
        public int LinkId
        {
            get;
            set;
        }

        public string RatingImage
        {
            get;
            set;
        }
    }
}
