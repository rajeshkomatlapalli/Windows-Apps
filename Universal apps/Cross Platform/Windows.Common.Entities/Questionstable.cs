using System;
using System.Net;
using System.Windows;

namespace OnlineVideos.Data
{
    public class Questionstable
    {
        public int Id
        {
            get;
            set;
        }

        public int Movieid
        {
            get;
            set;
        }
        public int QuestionID
        {
            get;
            set;
        }

        public string Question
        {
            get;
            set;
        }

        public string Answer
        {
            get;
            set;
        }
        public string Option1
        {
            get;
            set;
        }
        public string Option2
        {
            get;
            set;
        }
        public string Option3
        {
            get;
            set;
        }
        public string Option4
        {
            get;
            set;
        }
        public string QuestionType
        {
            get;
            set;
        }
        public int QuizNo
        {
            get;
            set;
        }
        public string Image
        {
            get;
            set;
        }
    }
}
