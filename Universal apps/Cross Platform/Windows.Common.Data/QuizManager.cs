using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Collections.Generic;
using OnlineVideos.Entities;
using System.ComponentModel;
using Common;
using Common.Library;
#if NOTANDROID
using Windows.Storage;
#endif
using System.Text;
using System.IO;
using System.Xml.Linq;
#if WINDOWS_APP
using Windows.UI.Xaml.Media;
//using Windows.Common.Entities;
#endif
using System.Threading.Tasks;
using System.Collections.ObjectModel;
#if WP8

#endif

namespace OnlineVideos.Data
{

    public static class QuizManager
    {
        public static string Userans = "";
        public static string GetSubjectsTitle()
        {
            List<QuizList> objquizList = new List<QuizList>();

            string SubjectTitle = "";
            try
            {
                if (AppSettings.ShowID != string.Empty)
                {
                    int showid = int.Parse(AppSettings.ShowID);
                    // int quizid = int.Parse(AppSettings.ShowQuestionId);
#if WINDOWS_APP
                    int quizid = Constants.QuizId;
#endif
#if WINDOWS_PHONE_APP
                    int quizid =Convert.ToInt32(AppSettings.ShowQuizId);
#endif
                    objquizList = Task.Run(async () => await Constants.connection.Table<QuizList>().Where(i => i.ShowID == showid && i.QuizID == quizid).OrderBy(j => j.QuizID).ToListAsync()).Result;
                    foreach (var item in objquizList)
                    {
                        SubjectTitle = item.Name;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetSubjectsTitle Method In QuizManager.cs file", ex);
                ex.Data.Add("movieId", AppSettings.ShowID);
                // Exceptions.SaveOrSendExceptions(mess, ex);
            }
            return SubjectTitle;

        }
        public static string GetQuizrating(string LinkId, int Movieid)
        {
            int linkid = Convert.ToInt32(LinkId);
            string rating = "";
#if WINDOWS_PHONE_APP
            var query2 = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkOrder == linkid & i.ShowID == Movieid & i.LinkType == "Quiz").ToListAsync()).Result;
#endif
#if WINDOWS_APP
            var query2 = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkOrder == linkid & i.ShowID == Movieid & i.LinkType == "Quiz").ToListAsync()).Result;
#endif
            foreach (var itm in query2)
            {
                rating = (itm.Rating).ToString();
            }
            return rating;
        }
        public static ObservableCollection<QuizList> Getsubjects(string id)
        {
            List<QuizList> objMovieList = new List<QuizList>();
            List<QuizList> objMovieList1 = new List<QuizList>();
            try
            {
                int showid = int.Parse(id);
                Constants.selecteditemquizlist =new ObservableCollection<QuizList>(Task.Run(async () => await Constants.connection.Table<QuizList>().Where(i => i.ShowID == showid).OrderBy(j => j.QuizID).ToListAsync()).Result);

                foreach (QuizList item in Constants.selecteditemquizlist)
                {
                    QuizList objMovie = new QuizList();

                    item.ShowID = Convert.ToInt32(item.ShowID);
                    item.QuizID = item.QuizID;
                    item.LinkID = item.LinkID;
                    item.Name = item.Name;
                    item.Result = item.Result;
                    item.RatingBitmapImage = GetRatingimgForQuiz(item.QuizID, item.ShowID);
                   // item.Add(objMovie);

                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Getsubjects Method In QuizManager.cs file", ex);
            }
            return Constants.selecteditemquizlist;
        }

        private static string GetRatingimgForQuiz(int linkorder, int movieid)
        {
            List<ShowLinks> objRatingList = new List<ShowLinks>();
            string rating = null;
            try
            {
                int showid = int.Parse(AppSettings.ShowID);
                objRatingList = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkOrder == linkorder && i.LinkType == "Quiz").OrderBy(j => j.ShowID).ToListAsync()).Result;
                foreach (var itm in objRatingList)
                {
                    rating = ImageHelper.LoadRatingImage((itm.Rating).ToString());
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetRatingimgForQuiz Method In QuizManager.cs file", ex);
            }
            return rating;
        }

        public static List<QuizQuestions> Getquestion()
        {

            List<QuizUserAnswers> AnswerList = new List<QuizUserAnswers>();
            List<QuizQuestions> objMovieList = new List<QuizQuestions>();
            List<QuizQuestions> objMovieList1 = new List<QuizQuestions>();
			objMovieList1.Clear ();
            try
            {
				AppSettings.ShowStartQuestionId = (AppSettings.ShowQuestionId).ToString();
                int showid = int.Parse(AppSettings.ShowID);
                int showQuizeID = int.Parse(AppSettings.ShowQuizId);
                objMovieList = Task.Run(async () => await Constants.connection.Table<QuizQuestions>().Where(i => i.ShowID == showid && i.QuizNo == showQuizeID).OrderBy(j => j.QuestionID).ToListAsync()).Result;
                int qid = 0;
                foreach (var item in objMovieList)
                {
                    if (item.QuestionID == Convert.ToInt32(AppSettings.ShowQuestionId))
                    {
                        QuizQuestions objQuizProp = new QuizQuestions();
					
						int QuestionID=Convert.ToInt32(AppSettings.ShowQuestionId);
						AnswerList = Task.Run(async () => await Constants.connection.Table<QuizUserAnswers>().Where(i => i.ID == showid && i.QuestionID == QuestionID).OrderBy(j => j.QuestionID).ToListAsync()).Result; 
                        if (AnswerList.Count() != 0)
                        {
                            foreach (var itm in AnswerList)
                            {
								 objQuizProp.UserAnswer = itm.UserAnswer;
                            }
                        }

                        objQuizProp.ShowID = item.ShowID;
                        objQuizProp.Question = item.Question;
                        objQuizProp.QuestionType = item.QuestionType;
                        objQuizProp.QuestionID = item.QuestionID;
                        objQuizProp.Option1 = item.Option1;
                        objQuizProp.Option2 = item.Option2;
                        if (item.Option3 != "")
                            objQuizProp.Option3 = item.Option3;
                        if (item.Option4 != "")
                            objQuizProp.Option4 = item.Option4;
                        if (item.Image != "")
                        {
							#if WINDOWS_PHONE_APP && NOTANDROID
                            objQuizProp.ImgQuestion = ResourceHelper.getQuestionImagefromstorage(item.Image);
#endif
                        }
                        objMovieList1.Add(objQuizProp);
                       
						#if WINDOWS_APP
                        if (qid == 2)
                        {

                            break;
                        }
						#endif
						#	if ANDROID 

							break;
					
						#endif
                        qid++;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Getquestion Method In QuizManager.cs file", ex);
            }
            return objMovieList1;
        }



        public static string GetTimerForTest()
        {
            List<QuizList> objMovieList = new List<QuizList>();

            string timelimt = "";
            try
            {
                int showid = int.Parse(AppSettings.ShowID);

                objMovieList = Task.Run(async () => await Constants.connection.Table<QuizList>().Where(i => i.ShowID == showid).OrderByDescending(j => j.ShowID).ToListAsync()).Result;
                int q = 1;
                foreach (var item in objMovieList)
                {
                    if (item.QuizID == Convert.ToInt32(AppSettings.ShowQuizId))
                    {

                        timelimt = item.Timelimt;
#if ANDROID
						AppSettings.Timelimt=timelimt;
#endif
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetTimerForTest Method In QuizManager.cs file", ex);
            }

            return timelimt;
        }

        public static void LoadMaxQuestionid()
        {
            List<QuizQuestions> objMovieList = new List<QuizQuestions>();
            List<QuizQuestions> objMovieList1 = new List<QuizQuestions>();
            try
            {
                int showid = int.Parse(AppSettings.ShowID);

                objMovieList = Task.Run(async () => await Constants.connection.Table<QuizQuestions>().Where(i => i.ShowID == showid).OrderByDescending(j => j.QuestionID).ToListAsync()).Result; 
                int q = 1;
                foreach (var item in objMovieList)
                {
                    if (item.QuizNo == Convert.ToInt32(AppSettings.ShowQuizId))
                    {
                        string MaxQuestionID = (item.QuestionID).ToString();
                        AppSettings.ShowMaxQuestionId = MaxQuestionID;
                        if (q == 1)
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadMaxQuestionid Method In QuizManager.cs file", ex);
            }


        }

        public static void Displayingresult()
        {
            try
            {
                int k = 0;
                List<QuizQuestions> objMovieList = new List<QuizQuestions>();
                List<QuizUserAnswers> Answerdata = new List<QuizUserAnswers>();
                DataManager<QuizUserAnswers> datamanager1 = new DataManager<QuizUserAnswers>();
                int showid = int.Parse(AppSettings.ShowID);
                int ShowQuizId = int.Parse(AppSettings.ShowQuizId);
                Answerdata = Task.Run(async () => await Constants.connection.Table<QuizUserAnswers>().Where(i => i.ID == showid).OrderBy(j => j.QuestionID).ToListAsync()).Result;
                objMovieList = Task.Run(async () => await Constants.connection.Table<QuizQuestions>().Where(i => i.ShowID == showid && i.QuizNo == ShowQuizId).OrderBy(j => j.QuestionID).ToListAsync()).Result;
                int MaxQuestionID = Convert.ToInt32(AppSettings.ShowMaxQuestionId);
                foreach (var question in objMovieList)
                {
                    int id = question.QuestionID;
                    string ans = question.Answer;
                    foreach (var Answer in Answerdata)
                    {
                        if (Answer.QuestionID == id)
                        {
                            if (Answer.UserAnswer == ans)
                            {
                                k++;
                            }
                            SettingsHelper.Save("QuizMarks", k.ToString());
                        }
                    }
                }
                Decimal de;
                de = Decimal.Divide(k, MaxQuestionID) * 100;
                SettingsHelper.Save("QuizPercentage", Math.Round(de, 0).ToString());
                if (de >= 70)
                {
                    SettingsHelper.Save("QuizGrade", "Distinction");
                }
                else if (de >= 60)
                {
                    SettingsHelper.Save("QuizGrade", "First Class");
                }
                else if (de >= 50)
                {
                    SettingsHelper.Save("QuizGrade", "Second Class");
                }
                else
                {
                    SettingsHelper.Save("QuizGrade", "Fail");
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Displayingresult Method In QuizManager.cs file", ex);
            }

        }
        public static List<QuizQuestions> GetresultReviewdatawp8()
        {
            List<QuizUserAnswers> AnswerList = new List<QuizUserAnswers>();
            List<QuizQuestions> objMovieList = new List<QuizQuestions>();
            List<QuizQuestions> objMovieList1 = new List<QuizQuestions>();
            try
            {
                AppSettings.ShowStartQuestionId = AppSettings.ShowQuestionId;
                int showid = int.Parse(AppSettings.ShowID);
                int showQuizeID = int.Parse(AppSettings.ShowQuizId);
                objMovieList = Task.Run(async () => await Constants.connection.Table<QuizQuestions>().Where(i => i.ShowID == showid && i.QuizNo == showQuizeID).OrderBy(j => j.QuestionID).ToListAsync()).Result;
                int qid = 0;
                foreach (var item in objMovieList)
                {
                    if (item.QuestionID == Convert.ToInt32(AppSettings.ShowQuestionId))
                    {
                        QuizQuestions objQuizProp = new QuizQuestions();
                        DataManager<QuizUserAnswers> datamanager1 = new DataManager<QuizUserAnswers>();
                        int ShowQuestionId = int.Parse(AppSettings.ShowQuestionId);
                        AnswerList = Task.Run(async () => await Constants.connection.Table<QuizUserAnswers>().Where(i => i.ID == showid && i.QuestionID == ShowQuestionId).OrderBy(j => j.QuestionID).ToListAsync()).Result;
                        if (AnswerList.Count() != 0)
                        {
                            foreach (var itm in AnswerList)
                            {
                                Constants.UserAnswerlist.Add(itm.UserAnswer);
                            }
                        }

                        objQuizProp.ShowID = item.ShowID;
                        Constants.CorrectAnswerlist.Add(item.Answer);
                        objQuizProp.UserAnswer = Constants.UserAnswerlist.LastOrDefault();
                        objQuizProp.Answer = item.Answer;
                        objQuizProp.Question = item.Question;

                        objQuizProp.QuestionType = item.QuestionType;
                        objQuizProp.QuestionID = item.QuestionID;
                        objQuizProp.Option1 = item.Option1;
                        objQuizProp.Option2 = item.Option2;
                        if (item.Option3 != "")
                            objQuizProp.Option3 = item.Option3;
                        if (item.Option4 != "")
                            objQuizProp.Option4 = item.Option4;
                        if (item.Image != "" && item.Image != null)
                        {
#if NOTANDROID && WP8
                            objQuizProp.Image = ResourceHelper.getQuizImageFromStorageOrInstalledFolder(item.Image);
#endif
                        }
                        objMovieList1.Add(objQuizProp);

                        //if (AppSettings.ShowQuestionId != AppSettings.ShowMaxQuestionId)
                        //{
                        //    int showquestionid = Convert.ToInt32(AppSettings.ShowQuestionId) + 1;
                        //    AppSettings.ShowQuestionId = showquestionid.ToString();
                        //}

#if WINDOWS_APP
						if (qid == 2)
						{

							break;
						}
#endif
#	if ANDROID 

						break;

#endif
                        qid++;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetresultReviewdata Method In QuizManager.cs file", ex);
            }

            return objMovieList1;
        }
        public static List<QuizQuestions> GetresultReviewdata()
        {
            List<QuizUserAnswers> AnswerList = new List<QuizUserAnswers>();
            List<QuizQuestions> objMovieList = new List<QuizQuestions>();
            List<QuizQuestions> objMovieList1 = new List<QuizQuestions>();
            try
            {
                AppSettings.ShowStartQuestionId = AppSettings.ShowQuestionId;
                int showid = int.Parse(AppSettings.ShowID);
                int showQuizeID = int.Parse(AppSettings.ShowQuizId);
                objMovieList = Task.Run(async () => await Constants.connection.Table<QuizQuestions>().Where(i => i.ShowID == showid && i.QuizNo == showQuizeID).OrderBy(j => j.QuestionID).ToListAsync()).Result;
                int qid = 0;
                foreach (var item in objMovieList)
                {
                    if (item.QuestionID == Convert.ToInt32(AppSettings.ShowQuestionId))
                    {
                        QuizQuestions objQuizProp = new QuizQuestions();
                        DataManager<QuizUserAnswers> datamanager1 = new DataManager<QuizUserAnswers>();
                        int ShowQuestionId = int.Parse(AppSettings.ShowQuestionId);
                        AnswerList = Task.Run(async () => await Constants.connection.Table<QuizUserAnswers>().Where(i => i.ID == showid && i.QuestionID == ShowQuestionId).OrderBy(j => j.QuestionID).ToListAsync()).Result;
                        if (AnswerList.Count() != 0)
                        {
                            foreach (var itm in AnswerList)
                            {
                                Constants.UserAnswerlist.Add(itm.UserAnswer);
                            }
                        }

                        objQuizProp.ShowID = item.ShowID;
                        Constants.CorrectAnswerlist.Add(item.Answer);
						objQuizProp.UserAnswer=item.UserAnswer;
						objQuizProp.Answer=item.Answer;
                        objQuizProp.Question = item.Question;

                        objQuizProp.QuestionType = item.QuestionType;
                        objQuizProp.QuestionID = item.QuestionID;
                        objQuizProp.Option1 = item.Option1;
                        objQuizProp.Option2 = item.Option2;
                        if (item.Option3 != "")
                            objQuizProp.Option3 = item.Option3;
                        if (item.Option4 != "")
                            objQuizProp.Option4 = item.Option4;
                        if (item.Image != "" && item.Image!=null)
                        {
                            objQuizProp.Image = ResourceHelper.getQuizImageFromStorageOrInstalledFolder(item.Image);
                            #if NOTANDROID && WINDOWS_PHONE_APP
                            objQuizProp.Image = ResourceHelper.getQuizImageFromStorageOrInstalledFolder(item.Image);
                              #endif
                        }
                        objMovieList1.Add(objQuizProp);

                        if (AppSettings.ShowQuestionId != AppSettings.ShowMaxQuestionId)
                        {
                            int showquestionid = Convert.ToInt32(AppSettings.ShowQuestionId) + 1;
                            AppSettings.ShowQuestionId = showquestionid.ToString();
                        }

						#if WINDOWS_APP
						if (qid == 2)
						{

							break;
						}
						#endif
						#	if ANDROID 

						break;

						#endif
                        qid++;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetresultReviewdata Method In QuizManager.cs file", ex);
            }

            return objMovieList1;
        }
        public static void DeleteAllUserAnswers()
        {
            string command = "Delete from QuizUserAnswers";
            Task.Run(async () => await Constants.connection.QueryAsync<QuizUserAnswers>(command));
        }
        public static void deletestoriongdata()
        {
            List<QuizUserAnswers> answerlist = new List<QuizUserAnswers>();
            //DataManager<QuizUserAnswers> linksmanager = new DataManager<QuizUserAnswers>();
            //linksmanager.SaveQuizData(answerlist);
            List<QuizUserAnswers> useranswerslist = Task.Run(async () => await Constants.connection.Table<QuizUserAnswers>().ToListAsync()).Result;

            foreach (QuizUserAnswers qua in useranswerslist)
            {
                Task.Run(async () => await Constants.connection.DeleteAsync(qua));
            }
        }

        public   static void SaveAnswers(List<QuizUserAnswers> answerlist)
        {
            //Task.Run(async () => await Constants.connection.InsertAllAsync(answerlist));
            foreach (QuizUserAnswers links in answerlist)
            {
                    string command = "insert into QuizUserAnswers(ID,QuestionID,UserAnswer) values(" + links.ID + "," + links.QuestionID + ",'" + links.UserAnswer + "') ";
                    Task.Run(async () => await Constants.connection.QueryAsync<QuizUserAnswers>(command));
               
            }
                    
        }
        #if NOTANDROID
        
        private async static void updatexml(string filename, XDocument xdoc)
        {
            try
            {


                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = await store1.CreateFileAsync(filename, Windows.Storage.CreationCollisionOption.OpenIfExists);
                var fquery1 = await file1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
                var outputStream = fquery1.GetOutputStreamAt(0);
                var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                StringBuilder sb = new StringBuilder();
                TextWriter tx = new StringWriter(sb);
                xdoc.Save(tx);
                string text = tx.ToString();
                text = text.Replace("utf-16", "utf-8");
                writer.WriteString(text);
                var fi = await writer.StoreAsync();
                writer.DetachStream();
                var oi = await outputStream.FlushAsync();
                outputStream.Dispose();
                fquery1.Dispose();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in updatexml Method In QuizManager.cs file", ex);
            }
        }
#endif
        public static void SaveQuizResult(string Result)
        {
            List<QuizList> objQuizList = new List<QuizList>();
            string command = string.Empty;
            int showid = int.Parse(AppSettings.ShowID);
            int ShowQuizId = int.Parse(AppSettings.ShowQuizId);
            objQuizList = Task.Run(async()=> await Constants.connection.Table<QuizList>().Where(i=>i.ShowID==showid && i.QuizID==ShowQuizId).OrderBy(j=>j.QuizID).ToListAsync()).Result; 
            foreach (QuizList list in objQuizList)
            {
                list.Result = Result;
                command = "UPDATE QuizList SET Result='" + Result + "' " + "WHERE  ShowID=" + list.ShowID + " And   QuizID=" + list.QuizID;
                Task.Run(async () => await Constants.connection.QueryAsync<QuizList>(command));
            }
        }

        public static List<QuizList> GetsubjectsHistory()
        {
            List<QuizList> objMovieLinks = new List<QuizList>();
            List<QuizList> objMovieList1 = new List<QuizList>();
            try
            {
                objMovieLinks = Task.Run(async () => await Constants.connection.Table<QuizList>().Where(i => i.Result != null).OrderBy(j => j.QuizID).ToListAsync()).Result; 
                foreach (QuizList Link in objMovieLinks)
                {
                    if (Link.Result != "")
                    {
                        objMovieList1.Add(Link);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("id", AppSettings.ShowID);
                Exceptions.SaveOrSendExceptions("Exception in GetsubjectsHistory Method In Quizmanager.cs file", ex);
            }
            return objMovieLinks;
        }

    


        public static int GetUserAnswersCount()
        {
            int count = 0;
            List<QuizUserAnswers> Answerdata = new List<QuizUserAnswers>();
            DataManager<QuizUserAnswers> datamanager1 = new DataManager<QuizUserAnswers>();
            int showid=int.Parse(AppSettings.ShowID);
            Answerdata = Task.Run(async () => await Constants.connection.Table<QuizUserAnswers>().Where(i => i.ID == showid).OrderBy(j => j.QuestionID).ToListAsync()).Result;

            if (Answerdata.Count() != 0)
            {
                count = Answerdata.Count();
            }
            return count;
        }

        public static List<QuizQuestions> GetSnapViewQuestions()
        {
            List<QuizUserAnswers> AnswerList = new List<QuizUserAnswers>();
            List<QuizQuestions> objMovieList = new List<QuizQuestions>();
            List<QuizQuestions> objMovieList1 = new List<QuizQuestions>();
            try
            {
                AppSettings.ShowStartQuestionId = AppSettings.ShowQuestionId;
                DataManager<QuizQuestions> datamanager = new DataManager<QuizQuestions>();
                int showid = int.Parse(AppSettings.ShowID);
                int ShowQuizId = int.Parse(AppSettings.ShowQuizId);
                int ShowQuestionId = int.Parse(AppSettings.ShowQuestionId);
                objMovieList = Task.Run(async () => await Constants.connection.Table<QuizQuestions>().Where(i => i.ShowID == showid && i.QuizNo == ShowQuizId && i.QuestionID == ShowQuestionId).OrderBy(j => j.QuestionID).ToListAsync()).Result;

                foreach (var item in objMovieList)
                {

                    QuizQuestions objQuizProp = new QuizQuestions();
                    DataManager<QuizUserAnswers> datamanager1 = new DataManager<QuizUserAnswers>();
                    AnswerList = Task.Run(async () => await Constants.connection.Table<QuizUserAnswers>().Where(i => i.ID == showid && i.QuestionID == ShowQuestionId).OrderBy(j => j.QuestionID).ToListAsync()).Result;
                    if (AnswerList.Count() != 0)
                    {
                        foreach (var itm in AnswerList)
                        {
                            Constants.UserAnswerlist.Add(itm.UserAnswer);
                        }
                    }

                    objQuizProp.ShowID = item.ShowID;
                    Constants.CorrectAnswerlist.Add(item.Answer);
                    objQuizProp.Question = item.Question;
                    objQuizProp.QuestionType = item.QuestionType;
                    objQuizProp.QuestionID = item.QuestionID;
                    objQuizProp.Option1 = item.Option1;
                    objQuizProp.Option2 = item.Option2;
                    if (item.Option3 != "")
                        objQuizProp.Option3 = item.Option3;
                    if (item.Option4 != "")
                        objQuizProp.Option4 = item.Option4;
                    if (item.Image != "")
                    {
                        #if NOTANDROID
                        objQuizProp.Image = ResourceHelper.GetQuestionImage(item.Image);
#endif
                    }
                    objMovieList1.Add(objQuizProp);

                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetSnapViewQuestions Method In QuizManager.cs file", ex);
            }
            return objMovieList1;
        }

        public static void UpdateSaveAnswers(List<QuizUserAnswers> answerlist)
        {

            foreach (QuizUserAnswers links in answerlist)
            {
               string command = "UPDATE QuizUserAnswers SET UserAnswer='" + links.UserAnswer + "' " + "WHERE  ID=" + links.ID + " And   QuestionID=" + links.QuestionID;
                Task.Run(async () => await Constants.connection.QueryAsync<QuizUserAnswers>(command));
            }

        }

        public static List<QuizList> GetsubjectsForWP8(string id)
        {
            List<QuizList> objsubjects = null;
            try
            {
                int showid = Convert.ToInt32(id);
                var Showsubjects = Task.Run(async () => await Constants.connection.Table<QuizList>().Where(i => i.ShowID == showid).ToListAsync()).Result;
                objsubjects = new List<QuizList>();
                foreach (var Link in Showsubjects)
                {
                    QuizList quiz = new QuizList();
                    quiz.LinkID= Link.LinkID;
                    quiz.ShowID = Link.ShowID;
                    quiz.QuizID = Link.QuizID;
                    quiz.Name = Link.Name;
                    quiz.Result = Link.Result;
                    quiz.RatingBitmapImage = GetRatingimgForQuiz(Link.QuizID, Link.ShowID);
                    objsubjects.Add(quiz);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("id", id);
                Exceptions.SaveOrSendExceptions("Exception in GetVideoLinksOfAMovie Method In OnlineShow.cs file", ex);
            }
            return objsubjects;
        }

        //Code for Wp8
        public static void SaveAnswersForWp8(string Answer)
        {
            int questionid = Convert.ToInt32(AppSettings.ShowQuestionId);
            var xquery = Task.Run(async () => await Constants.connection.Table<QuizUserAnswers>().Where(k => k.QuestionID == questionid).ToListAsync()).Result;
            if (xquery.Count() == 0)
            {
                QuizUserAnswers answerinsert = new QuizUserAnswers();
				answerinsert.ID=Convert.ToInt32(AppSettings.ShowID);
                answerinsert.QuestionID = Convert.ToInt32(AppSettings.ShowQuestionId);
                if (Answer != "")
                {
                    answerinsert.UserAnswer = Answer;
                }
                Task.Run(async () => await Constants.connection.InsertAsync(answerinsert));
            }
            else
            {
                QuizUserAnswers answerinsert = new QuizUserAnswers();


                var storingdata = Task.Run(async () => await Constants.connection.Table<QuizUserAnswers>().Where(k => k.QuestionID == questionid).ToListAsync()).Result;
                QuizUserAnswers answerupdate = storingdata.FirstOrDefault();

                Task.Run(async () => await Constants.connection.DeleteAsync(answerupdate));
                answerinsert.QuestionID = Convert.ToInt32(AppSettings.ShowQuestionId);
                if (Answer != "")
                {
                    answerinsert.UserAnswer = Answer;
                }
                Task.Run(async () => await Constants.connection.InsertAsync(answerinsert));

            }
        }
		#if ANDROID && NOTIOS
		public static string GetUserAnser(int questionId)
		{
			string _anser = string.Empty;
			try {
				var _userAnser = Task.Run (async () => await Constants.connection.Table<QuizUserAnswers> ().Where (i => i.QuestionID == questionId).FirstOrDefaultAsync ()).Result;
				_anser=_userAnser.UserAnswer;
			} catch (Exception ex) {

			}
			return _anser;
		}
		public static Android.Graphics.Color SetQuestionOptionColor(string UserAnswer, string CorrectAnswer, string Option)
		{
			Android.Graphics.Color optioncolor = Android.Graphics.Color.Black;
			try {
				if (UserAnswer != CorrectAnswer) {
					if (UserAnswer == Option) {
						optioncolor = Android.Graphics.Color.Red;

					} else if (CorrectAnswer == Option) {
						optioncolor = Android.Graphics.Color.Green;
					} else {
						optioncolor = Android.Graphics.Color.Black;
					}
				} else {
					if (CorrectAnswer == Option) {
						optioncolor = Android.Graphics.Color.Green;
					} else {
						optioncolor = Android.Graphics.Color.Black;
					}
				}
			} catch (Exception ex) {

			}
			return optioncolor;
		}
		#endif

    }
}
