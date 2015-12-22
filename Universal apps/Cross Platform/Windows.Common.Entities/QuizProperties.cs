using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideos.Entities
{
    public class QuizProperties
    {
        private int _questionid;
        public int QuestionID
        {
            get { return _questionid; }
            set { _questionid = value; }
        }
        private string _Movieid;
        public string Movieid
        {
            get { return _Movieid; }
            set { _Movieid = value; }
        }
        private string _question;
        public string Question
        {
            get { return _question; }
            set { _question = value; }
        }
        private string _answer;
        public string Answer
        {
            get { return _answer; }
            set { _answer = value; }
        }
        private string _option1;
        public string Option1
        {
            get { return _option1; }
            set { _option1 = value; }
        }
        private string _option2;
        public string Option2
        {
            get { return _option2; }
            set { _option2 = value; }
        }
        private string _option3;
        public string Option3
        {
            get { return _option3; }
            set { _option3 = value; }
        }
        private string _option4;
        public string Option4
        {
            get { return _option4; }
            set { _option4 = value; }
        }
        private string _QuizName;
        public string QuizName
        {
            get { return _QuizName; }
            set { _QuizName = value; }
        }
        private int _QuizId;
        public int QuizId
        {
            get { return _QuizId; }
            set { _QuizId = value; }
        }
        private int _Timelimt;
        public int Timelimt
        {
            get { return _Timelimt; }
            set { _Timelimt = value; }
        }
        private string _QuestionType;
        public string QuestionType
        {
            get { return _QuestionType; }
            set { _QuestionType = value; }
        }
        private string _pin;
        public string pin
        {
            get { return _pin; }
            set { _pin = value; }
        }
        private double _rating;
        public double Rating
        {
            get { return _rating; }
            set { _rating = value; }
        }
        private string _ratetitle;
        public string Ratetitle
        {
            get { return _ratetitle; }
            set { _ratetitle = value; }
        }
        private string _ImgQuestion;
        public string ImgQuestion
        {
            get { return _ImgQuestion; }
            set { _ImgQuestion = value; }
        }

        private string _QuizResult;
        public string QuizResult
        {
            get { return _QuizResult; }
            set { _QuizResult = value; }
        }
        private string _UserAnswer;
        public string UserAnswer
        {
            get { return _UserAnswer; }
            set { _UserAnswer = value; }
        }
        private int _LinkId;
        public int LinkId
        {
            get { return _LinkId; }
            set { _LinkId = value; }
        }
        private string _RatingImage;
        public string RatingImage
        {
            get { return _RatingImage; }
            set { _RatingImage = value; }
        }
    }
}
