using Common.Library;
using Common.Utilities;
using OnlineMovies.UserControls;
using OnlineVideos;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OnlineMovies.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QuizResultReview : Page
    {
        #region GlobalDeclaration
        List<QuizQuestions> question = null;
        QuizExitPopUp Quizpopup;
        int popupdisplay = 0;
        #endregion

        public QuizResultReview()
        {
            try
            {
                this.InitializeComponent();
                Quizpopup = new QuizExitPopUp();
                if (AppSettings.ShowID != "0")
                {
                    LoadPivotThemes(AppSettings.ShowUniqueID);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in QuizResultReview Method In QuizResultReview.cs file.", ex);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadAds();            
                AppSettings.ShowQuestionId = "1";
                GetPageDataInBackground();
                PageHelper.RemoveEntryFromBackStack("QuizResultReview");
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In QuizResultReview.cs file.", ex);
            }
        }
        private void LoadAds()
        {
            try
            {
                LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In SongDetails file", ex);
                string excepmess = "Exception in LoadAds Method In QuizResultReview file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }
        private void GetPageDataInBackground()
        {
            BackgroundHelper bwHelper = new BackgroundHelper();

            bwHelper.AddBackgroundTask(
                                        (object s, DoWorkEventArgs a) =>
                                        {
                                            a.Result = QuizManager.GetresultReviewdatawp8();
                                        },
                                        (object s, RunWorkerCompletedEventArgs a) =>
                                        {
                                            question = (List<QuizQuestions>)a.Result;
                                            int showid = AppSettings.ShowUniqueID;
                                            int quizno = Convert.ToInt32(AppSettings.ShowQuizId);
                                            if (Task.Run(async () => await Constants.connection.Table<QuizQuestions>().Where(i => i.ShowID == showid && i.QuizNo == quizno).ToListAsync()).Result.Count == 1)
                                            {
                                                imgprevious.Visibility = Visibility.Collapsed;
                                                imgnext.Visibility = Visibility.Collapsed;
                                            }

                                            spQuizresult.DataContext = question[0];
                                            QuizHelper.SetQuizOptionColor(question[0].QuestionType, question[0].UserAnswer, question[0].Answer, "A", txtoption1, txtuserans, txtcorrectans, useransgrid, correctansgrid, stkcorrectans);
                                            QuizHelper.SetQuizOptionColor(question[0].QuestionType, question[0].UserAnswer, question[0].Answer, "B", txtoption2, txtuserans, txtcorrectans, useransgrid, correctansgrid, stkcorrectans);
                                            QuizHelper.SetQuizOptionColor(question[0].QuestionType, question[0].UserAnswer, question[0].Answer, "C", txtoption3, txtuserans, txtcorrectans, useransgrid, correctansgrid, stkcorrectans);
                                            QuizHelper.SetQuizOptionColor(question[0].QuestionType, question[0].UserAnswer, question[0].Answer, "D", txtoption4, txtuserans, txtcorrectans, useransgrid, correctansgrid, stkcorrectans);
                                        }
                                      );
            bwHelper.RunBackgroundWorkers();
        }
        private void LoadPivotThemes(long ShowID)
        {
            try
            {
                tblkVideosTitle.Text = QuizManager.GetSubjectsTitle();
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in LoadPivotThemes Method In QuizResultReview.cs file.", ex);
            }
        }

        private void imgTitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void imgprevious_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                int id = (Convert.ToInt32(AppSettings.ShowQuestionId) - 1);
                int maxid = (Convert.ToInt32(AppSettings.ShowMaxQuestionId) - 1);
                AppSettings.ShowQuestionId = id.ToString();
                if (AppSettings.ShowQuestionId == "1")
                {
                    imgprevious.Visibility = Visibility.Collapsed;
                }
                if (AppSettings.ShowQuestionId == maxid.ToString())
                {
                    imgnext.Visibility = Visibility.Visible;
                    //imgnext.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("/Images/Next.png");
                }
                GetPageDataInBackground();
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in Imgprvs_MouseLeftButtonDown Method In QuizResultReview.cs file.", ex);
            }
        }

        private void imgexit_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                PageHelper.RemoveEntryFromBackStack("MainPage");
                //NavigationService.Navigate(NavigationHelper.getQuizList(AppSettings.ShowID, tblkVideosTitle.Text));\
                string[] parame = new string[2];
                parame[0] = AppSettings.ShowID.ToString();
                parame[1] = null;
                Frame.Navigate(typeof(SubjectDetail), parame);
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in imgexit_MouseLeftButtonDown Method In QuizResultReview.cs file.", ex);
            }
        }

        private void imgnext_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                //if(AppSettings.ShowMaxQuestionId=="1")
                //    imgnext.Visibility = Visibility.Collapsed;
                int maxid = (Convert.ToInt32(AppSettings.ShowMaxQuestionId) - 1);
                if (AppSettings.ShowQuestionId == maxid.ToString())
                {
                    imgnext.Visibility = Visibility.Collapsed;
                }
                AppSettings.ShowQuestionId = (Convert.ToInt32(AppSettings.ShowQuestionId) + 1).ToString();
                imgprevious.Visibility = Visibility.Visible;
                imgnext.Margin = new Thickness(5, 0, 0, 0);
                imgexit.Margin = new Thickness(5, 0, 0, 0);
                GetPageDataInBackground();
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in Imgnxt_MouseLeftButtonDown Method In QuizResultReview.cs file.", ex);
            }
        }
    }
}
