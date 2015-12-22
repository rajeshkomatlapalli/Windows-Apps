using Common.Library;
using Common.Utilities;
using OnlineMovies.UserControls;
using OnlineVideos;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.UI;
using OnlineVideos.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
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
    public sealed partial class QuestionsDisplay : Page
    {
        #region GlobalDeclaration
        TimerHelper objtime = new TimerHelper();
        private DispatcherTimer timer;
        string QuestionType = "";
        int popupdisplay = 0;
        QuizExitPopUp Quizpopup;
        #endregion

        public QuestionsDisplay()
        {
            try
            {
                this.InitializeComponent();
                if (AppSettings.ShowID != "0")
                {
                    LoadPivotThemes(AppSettings.ShowUniqueID);
                    Quizpopup = new QuizExitPopUp();
                    timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += timer_Tick;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in QuestionsDisplay method In QuestionsDisplay.cs file.", ex);
            }
        }
        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {            
            Quizpopup.Show();
            popupdisplay = 1;
            e.Handled = true;
        }

        void timer_Tick(object sender, object e)
        {
            try
            {
                tblktimer.Text = objtime.manipulatetime();
                if (tblktimer.Text == (new TimeSpan(0, 0, 0)).ToString())
                {
                    timer.Stop();
                    //NavigationService.Navigate(NavigationHelper.getQuizresultPage(tblktimer.Text));
                    Frame.Navigate(typeof(QuizResult), tblktimer.Text);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in timer_Tick event In QuestionsDisplay.cs file.", ex);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadAds();
                //HardwareButtons.BackPressed += HardwareButtons_BackPressed;
                string timelimit = "";
                timer.Start();
                GetPageDataInBackground();
                PageHelper.RemoveEntryFromBackStack("QuestionsDisplay");
                AppSettings.ShowQuestionId = "1";
                timelimit = QuizManager.GetTimerForTest();
                string s = timelimit;
                string[] readlines2 = s.Split(':');
                objtime.time(readlines2);                

            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded event In QuestionsDisplay.cs file.", ex);
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
                Exceptions.SaveOrSendExceptions("Excseption in LoadAds Method In QuestionsDisplay file", ex);
                string excepmess = "Exception in LoadAds Method In QuestionsDisplay file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }
        private void GetPageDataInBackground()
        {
            Constants.UIThread = true;

            BackgroundHelper bwHelper = new BackgroundHelper();

            bwHelper.AddBackgroundTask(
                                        (object s, DoWorkEventArgs a) =>
                                        {
                                            //Constants.UIThread = true;
                                            a.Result = QuizManager.Getquestion();
                                            
                                        },
                                        (object s, RunWorkerCompletedEventArgs a) =>
                                        {
                                            List<QuizQuestions> question = (List<QuizQuestions>)a.Result;
                                            spQuestions.DataContext = question[0];
                                            QuizHelper.SetQuizOptionCheckedState(question[0].QuestionType, question[0].UserAnswer, "A", rbA, cbA);
                                            QuizHelper.SetQuizOptionCheckedState(question[0].QuestionType, question[0].UserAnswer, "B", rbB, cbB);
                                            QuizHelper.SetQuizOptionCheckedState(question[0].QuestionType, question[0].UserAnswer, "C", rbC, cbC);
                                            QuizHelper.SetQuizOptionCheckedState(question[0].QuestionType, question[0].UserAnswer, "D", rbD, cbD);
                                            QuestionType = question[0].QuestionType;
                                        }
                                      );
            bwHelper.RunBackgroundWorkers();
            Constants.UIThread = false;
            SettingsHelper.Save("messagehighlight", "False");
        }     

        private void LoadPivotThemes(long ShowID)
        {
            tblkVideosTitle.Text = QuizManager.GetSubjectsTitle();
        }
        private void imgTitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void imgprevious_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(AppSettings.ShowQuestionId) - 1;
                int maxid = Convert.ToInt32(AppSettings.ShowMaxQuestionId) - 1;
                AppSettings.ShowQuestionId = id.ToString();
                txtmsg.Visibility = Visibility.Collapsed;
                if (AppSettings.ShowQuestionId == maxid.ToString())
                {
                    imgnext.Source = ImageHelper.GetImageForQuiz("Next");
                }
                SettingsHelper.Save("messagehighlight", "True");
                GetPageDataInBackground();
                if (AppSettings.ShowQuestionId == "1")
                {
                    imgprevious.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in imgprevious_MouseLeftButtonDown method In QuestionsDisplay.cs file.", ex);
            }
        }

        private void imgnext_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                if (AppSettings.ShowQuestionId != AppSettings.ShowMaxQuestionId)
                {
                    QuizHelper.GetOptionChecked(rbA, rbB, rbC, rbD, cbA, cbB, cbC, cbD, QuestionType);
                    if (SettingsHelper.getStringValue("messagehighlight") == "False")
                    {
                        txtmsg.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        tblktimer.Margin = new Thickness(27, 27, 0, 0);
                        imgnext.Margin = new Thickness(0, 0, 0, 0);
                        txtmsg.Visibility = Visibility.Collapsed;
                        imgprevious.Visibility = Visibility.Visible;
                        int id = Convert.ToInt32(AppSettings.ShowQuestionId) + 1;
                        AppSettings.ShowQuestionId = id.ToString();
                        GetPageDataInBackground();
                    }
                    if (AppSettings.ShowMaxQuestionId == AppSettings.ShowQuestionId)
                    {
                        imgnext.Source = ImageHelper.GetImageForQuiz("Finish");
                    }
                }
                else
                {
                    timer.Stop();
                    QuizHelper.GetOptionChecked(rbA, rbB, rbC, rbD, cbA, cbB, cbC, cbD, QuestionType);
                    if (SettingsHelper.getStringValue("messagehighlight") == "False")
                    {
                        txtmsg.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        string timeused=SettingsHelper.getStringValue("TimeUsed");
                        Frame.Navigate(typeof(QuizResult), timeused);
                    }
                }
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in imgnext_MouseLeftButtonDown method In QuestionsDisplay.cs file.", ex);
            }
        }
    }
}
