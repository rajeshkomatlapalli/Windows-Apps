using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Common;
using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Threading.Tasks;

namespace OnlineVideos.UI
{
    public static class QuizHelper
    {

        public static void SetQuizOptionCheckedState(string QuestionType, string UserAnswer, string CurrentOption, RadioButton rbOption, CheckBox cbOption)
        {
            bool OptionState = UserAnswer == CurrentOption;
            if (string.IsNullOrEmpty(QuestionType))
            {
                cbOption.Visibility = Visibility.Collapsed;
                rbOption.IsChecked = OptionState;
            }
            else
            {
                rbOption.Visibility = Visibility.Collapsed;
                cbOption.IsChecked = OptionState;
            }
        }

        public static void GetOptionChecked(int questionscount, RadioButton rbA, RadioButton rbB, RadioButton rbC, RadioButton rbD, RadioButton rbA1, RadioButton rbB1, RadioButton rbC1, RadioButton rbD1, RadioButton rbA2, RadioButton rbB2, RadioButton rbC2, RadioButton rbD2)
        {

            List<string> anslist = new List<string>();
            int questionid = Convert.ToInt32(AppSettings.ShowStartQuestionId);
            int qid = Convert.ToInt32(AppSettings.ShowStartQuestionId);
            List<string> qidlist = new List<string>();

            if (rbA.IsChecked == false && rbB.IsChecked == false && rbC.IsChecked == false && rbD.IsChecked == false)
            {
                AppSettings.QuizQuestNoList = questionid.ToString();
            }
            if (questionscount >= 2)
            {
                if (rbA1.IsChecked == false && rbB1.IsChecked == false && rbC1.IsChecked == false && rbD1.IsChecked == false)
                {
                    questionid = questionid + 1;
                    if (AppSettings.QuizQuestNoList == string.Empty)
                        AppSettings.QuizQuestNoList = questionid.ToString();
                    else
                        AppSettings.QuizQuestNoList += ", " + questionid.ToString();
                }
                if (questionscount >= 3)
                {
                    if (rbA2.IsChecked == false && rbB2.IsChecked == false && rbC2.IsChecked == false && rbD2.IsChecked == false)
                    {
                        if (questionid == Convert.ToInt32(AppSettings.ShowStartQuestionId) + 1)
                        {
                            questionid = questionid + 1;
                        }
                        else
                        {
                            questionid = questionid + 2;
                        }
                        if (AppSettings.QuizQuestNoList == string.Empty)
                            AppSettings.QuizQuestNoList = questionid.ToString();
                        else
                            AppSettings.QuizQuestNoList += ", " + questionid.ToString();
                    }
                }
            }
            if (rbA.IsChecked == true)
                anslist.Add("A");
            if (rbB.IsChecked == true)
                anslist.Add("B");
            if (rbC.IsChecked == true)
                anslist.Add("C");
            if (rbD.IsChecked == true)
                anslist.Add("D");


            if (rbA1.IsChecked == true)
                anslist.Add("A");
            if (rbB1.IsChecked == true)
                anslist.Add("B");
            if (rbC1.IsChecked == true)
                anslist.Add("C");
            if (rbD1.IsChecked == true)
                anslist.Add("D");



            if (rbA2.IsChecked == true)
                anslist.Add("A");
            if (rbB2.IsChecked == true)
                anslist.Add("B");
            if (rbC2.IsChecked == true)
                anslist.Add("C");
            if (rbD2.IsChecked == true)
                anslist.Add("D");

            for (int id = 0; id <= anslist.Count - 1; id++)
            {

                qidlist.Add(qid.ToString());
                qid++;
            }

            if (anslist.Count == questionscount)
            {
                List<QuizUserAnswers> Quiz = new List<QuizUserAnswers>();
                SettingsHelper.Save("messagehighlight", "True");
                if (!Constants.Quizanswerlist.Exists(x => x.QuestionID == Convert.ToInt32(AppSettings.ShowStartQuestionId)))
                {

                    for (int i = 0; i <= anslist.Count - 1; i++)
                    {
                        QuizUserAnswers ans = new QuizUserAnswers();
                        ans.ID = Convert.ToInt32(AppSettings.ShowID);
                        ans.QuestionID = Convert.ToInt32(qidlist[i]);
                        ans.UserAnswer = anslist[i];
                        Constants.Quizanswerlist.Add(ans);
                        Quiz.Add(ans);
                    }

                    QuizManager.SaveAnswers(Quiz);
                    Quiz.Clear();
                }
                else
                {

                    foreach (QuizUserAnswers links in Constants.Quizanswerlist)
                    {
                        for (int i = 0; i <= anslist.Count - 1; i++)
                        {
                            if (links.QuestionID == Convert.ToInt32(qidlist[i]))
                            {
                                links.UserAnswer = anslist[i];

                            } 
                        }

                    }
                    QuizManager.UpdateSaveAnswers(Constants.Quizanswerlist);
                }

                
                }
                //if (Constants.Quizanswerlist.Count == int.Parse(AppSettings.ShowMaxQuestionId))
                //{
                  
                //}
            
        }

        public static void SetQuizOptionColor(string UserAnswer, string CorrectAnswer, string Option, TextBlock txtoption)
        {

            if (UserAnswer != CorrectAnswer)
            {
                if (UserAnswer == Option)
                {
                    txtoption.Foreground = new SolidColorBrush(Colors.Red);
                    txtoption.FontWeight = FontWeights.Normal;
                }
                else if (CorrectAnswer == Option)
                {
                    txtoption.Foreground = new SolidColorBrush(Colors.Green);
                    txtoption.FontWeight = FontWeights.Bold;
                }
                else
                {
                    txtoption.Foreground = new SolidColorBrush(Colors.White);
                    txtoption.FontWeight = FontWeights.Normal;
                }
            }
            else
            {
                if (CorrectAnswer == Option)
                {
                    txtoption.Foreground = new SolidColorBrush(Colors.Green);
                    txtoption.FontWeight = FontWeights.Bold;
                }
                else
                {
                    txtoption.Foreground = new SolidColorBrush(Colors.White);
                    txtoption.FontWeight = FontWeights.Normal;
                }
            }

        }

        public static void SetQuizOptionCheckedState(string CurrentOption, RadioButton rbA, RadioButton rbA1, RadioButton rbA2)
        {
            if (Constants.UserAnswerlist.Count >= 1)
            {
                bool OptionState = Constants.UserAnswerlist[0] == CurrentOption;
                rbA.IsChecked = OptionState;
            }
            else
            {
                rbA.IsChecked = false;
            }
            if (Constants.UserAnswerlist.Count >= 2)
            {
                bool OptionState1 = Constants.UserAnswerlist[1] == CurrentOption;
                rbA1.IsChecked = OptionState1;
            }
            else
            {
                rbA1.IsChecked = false;
            }
            if (Constants.UserAnswerlist.Count >= 3)
            {
                bool OptionState2 = Constants.UserAnswerlist[2] == CurrentOption;
                rbA2.IsChecked = OptionState2;
            }
            else
            {
                rbA2.IsChecked = false;
            }

        }

        public static void SetQuizOptionColor(string Option, TextBlock txtoption4, TextBlock txtoption8, TextBlock txtoption12)
        {
            if (Constants.UserAnswerlist[0] != Constants.CorrectAnswerlist[0])
            {
                SetColorForWrongOption(Constants.UserAnswerlist[0], Option, txtoption4, Constants.CorrectAnswerlist[0]);
            }
            else
            {
                SetColorForCorrectOption(Constants.CorrectAnswerlist[0], Option, txtoption4);
            }
            if (Constants.UserAnswerlist.Count >= 2)
            {
                if (Constants.UserAnswerlist[1] != Constants.CorrectAnswerlist[1])
                {
                    SetColorForWrongOption(Constants.UserAnswerlist[1], Option, txtoption8, Constants.CorrectAnswerlist[1]);
                }
                else
                {
                    SetColorForCorrectOption(Constants.CorrectAnswerlist[1], Option, txtoption8);
                }
            }
            if (Constants.UserAnswerlist.Count >= 3)
            {
                if (Constants.UserAnswerlist[2] != Constants.CorrectAnswerlist[2])
                {
                    SetColorForWrongOption(Constants.UserAnswerlist[2], Option, txtoption12, Constants.CorrectAnswerlist[2]);
                }
                else
                {
                    SetColorForCorrectOption(Constants.CorrectAnswerlist[2], Option, txtoption12);
                }
            }

        }

        private static void SetColorForCorrectOption(string Answer, string Option, TextBlock txtoption)
        {
            if (Answer == Option)
            {
                txtoption.Foreground = new SolidColorBrush(Colors.Green);
                txtoption.FontWeight = FontWeights.Normal;
            }
            else
            {
                if(AppSettings.ProjectName != "DrivingTest.Windows")
                txtoption.Foreground = new SolidColorBrush(Colors.Black);
                else
                    txtoption.Foreground = new SolidColorBrush(Colors.White);
                txtoption.FontWeight = FontWeights.Normal;
            }
        }



        private static void SetColorForWrongOption(string Useranswer, string Option, TextBlock txtoption, string Answer)
        {
            if (Useranswer == Option)
            {
                txtoption.Foreground = new SolidColorBrush(Colors.Red);
                txtoption.FontWeight = FontWeights.Normal;
            }
            else if (Answer == Option)
            {
                txtoption.Foreground = new SolidColorBrush(Colors.Green);
                txtoption.FontWeight = FontWeights.Normal;
            }
            else
            {
                if (AppSettings.ProjectName != "DrivingTest.Windows")
                    txtoption.Foreground = new SolidColorBrush(Colors.Black);
                else
                    txtoption.Foreground = new SolidColorBrush(Colors.White);
                txtoption.FontWeight = FontWeights.Normal;
            }
        }

        public static void GetSnapViewOptionChecked(int questionscount, RadioButton rbA, RadioButton rbB, RadioButton rbC, RadioButton rbD)
        {

            int qid = Convert.ToInt32(AppSettings.ShowStartQuestionId);
            string answer = "";

            if (rbA.IsChecked == true)
                answer = "A";
            if (rbB.IsChecked == true)
                answer = "B";
            if (rbC.IsChecked == true)
                answer = "C";
            if (rbD.IsChecked == true)
                answer = "D";
            if (answer != "")
            {
                SettingsHelper.Save("messagehighlight", "True");

                if (!Constants.Quizanswerlist.Exists(x => x.QuestionID == Convert.ToInt32(AppSettings.ShowStartQuestionId)))
                {
                    QuizUserAnswers ans = new QuizUserAnswers();
                    ans.ID = Convert.ToInt32(AppSettings.ShowID);
                    ans.QuestionID = Convert.ToInt32(AppSettings.ShowQuestionId);
                    ans.UserAnswer = answer;
                    Constants.Quizanswerlist.Add(ans);
                }
                else
                {
                    foreach (QuizUserAnswers links in Constants.Quizanswerlist)
                    {
                        if (links.QuestionID == Convert.ToInt32(AppSettings.ShowQuestionId))
                        {
                            links.UserAnswer = answer;
                        }
                    }
                }
                if (Constants.Quizanswerlist.Count == int.Parse(AppSettings.ShowMaxQuestionId))
                {

                    QuizManager.SaveAnswers(Constants.Quizanswerlist);
                }
            }
        }

        public static void SetSnapQuizOptionCheckedState(string CurrentOption, RadioButton rbA)
        {

            if (Constants.UserAnswerlist.Count >= 1)
            {
                bool OptionState = Constants.UserAnswerlist[0] == CurrentOption;
                rbA.IsChecked = OptionState;
            }
            else
            {
                rbA.IsChecked = false;
            }

        }



    }
}
