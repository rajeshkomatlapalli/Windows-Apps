using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Reflection;
using Windows.UI.Core;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class ShowQuiz : UserControl
    {
        public static TextBlock block = new TextBlock();
        public static ShowQuiz currentshowquiz = default(ShowQuiz);
        public string ShowID = string.Empty;
        QuizList selecteditem = null;
        bool check = false;
        int SubjectId = 0;
        List<QuizList> objSubjectList;
        public ShowQuiz()
        {
            try
            {
            this.InitializeComponent();
            currentshowquiz = this;
            this.Tag = this.lstvwQuiz;
            block = this.txtmsg;
            progressbar.IsActive = true;
            AppSettings.YoutubeID = "1";
            Window.Current.SizeChanged += Current_SizeChanged;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowQuiz Method In ShowQuiz.cs file", ex);
            }
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            try
            {
                ApplicationViewState currentState = Windows.UI.ViewManagement.ApplicationView.Value;
                if (currentState == ApplicationViewState.Snapped)
                {
                    VisualStateManager.GoToState(this, "Snapped", false);
                }
                else
                {
                    VisualStateManager.GoToState(this, "Filled", false);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Current_SizeChanged Method In ShowQuiz.cs file", ex);
            }
        }
        public string time(string[] p)
        {
            string r = string.Empty;
            try
            {

                TimeSpan t = new TimeSpan(Convert.ToInt32(p[0]), Convert.ToInt32(p[1]), Convert.ToInt32(p[2]));

                r = ((t.Hours.ToString() == "0") ? "" : ((Convert.ToInt32(t.Hours) > 1) ? t.Hours.ToString() + " hours" : t.Hours.ToString() + " hour")) + ((t.Minutes.ToString() == "0") ? "" : ((Convert.ToInt32(t.Minutes) > 1) ? t.Minutes.ToString() + " minutes" : t.Minutes.ToString() + " minute")) + ((t.Seconds.ToString() == "0") ? "" : ((Convert.ToInt32(t.Seconds) > 1) ? t.Seconds.ToString() + " seconds" : t.Seconds.ToString() + " second"));
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in time Method In ShowQuiz.cs file", ex);
            }


            return r;
        }

        private void txtquiz_RightTapped_1(object sender, RightTappedRoutedEventArgs e)
        {
            try
            {
                check = true;
                Constants.appbarvisible = true;
                AppSettings.LinkType = "Quiz";

                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("appbar").Invoke(p, new object[] { true });

                Page p1 = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p1.GetType().GetTypeInfo().GetDeclaredMethod("Reprtbrokenvisible").Invoke(p1, new object[] { true });
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in txtquiz_RightTapped_1 Method In ShowQuiz.cs file", ex);
            }
        }

        private void lstvwQuiz_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //Page p = (Page)PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                //p.GetType().GetTypeInfo().GetDeclaredMethod("HelpPopupClose").Invoke(p, null);
                int showid = AppSettings.ShowUniqueID;
                if (lstvwQuiz.SelectedIndex == -1)
                    return;
                selecteditem = (sender as Selector).SelectedItem as QuizList;
                if (check == true)
                {
                    check = false;
                    Constants.Quizselecteditem = selecteditem;
                    AppSettings.ShowQuizId = (selecteditem.QuizID).ToString();
                    Constants.LinkID = selecteditem.LinkID;
                    Page p6 = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                    p6.GetType().GetTypeInfo().GetDeclaredMethod("changequizfav").Invoke(p6, null);

                    //AppSettings.ShowQuizId = (selecteditem.QuizID).ToString();
                    lstvwQuiz.SelectedIndex = -1;
                    return;

                }
                Constants.QuizId = selecteditem.QuizID;
                SubjectId = (lstvwQuiz.SelectedItem as QuizList).QuizID;
                AppSettings.ShowQuizId = SubjectId.ToString();                
                if (Task.Run(async () => await Constants.connection.Table<QuizQuestions>().Where(i => i.ShowID == showid && i.QuizNo == SubjectId).FirstOrDefaultAsync()).Result != null)
                {
                    txtxstackpl.Visibility = Visibility.Visible;
                    tblkquestionsnotavailable.Visibility = Visibility.Collapsed;
                }
                else
                {
                    txtxstackpl.Visibility = Visibility.Collapsed;
                    tblkquestionsnotavailable.Text = "No Questions Available,add from appbar";
                    tblkquestionsnotavailable.Visibility = Visibility.Visible;
                }
                lstvwQuiz.Visibility = Visibility.Collapsed;
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("questioniconenable").Invoke(p, null);
                //((ApplicationBarMenuItem)SubjectDetail.current.ApplicationBar.MenuItems[1]).Text = "Add" + " " + "questions";
                QuizManager.LoadMaxQuestionid();
                txttotquestions.Text = AppSettings.ShowMaxQuestionId.ToString();
                string timelimit = QuizManager.GetTimerForTest();
                string s = timelimit;
                string[] readlines2 = s.Split(':');
                txttimelimit.Text = time(readlines2);
                lstvwQuiz.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstvwQuiz_SelectionChanged_1 Method In ShowQuiz.cs file", ex);
            }
        }

        private void ShowQuiz_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                tblkquestionsnotavailable.Visibility = Visibility.Collapsed;
                checkstate();
                GetPageDataInBackground();
                //List<QuizList> objquizlist = new List<QuizList>();
                //objquizlist = QuizManager.Getsubjects(AppSettings.ShowID);
                //if (objquizlist.Count != 0)
                //{
                //    lstvwQuiz.ItemsSource = objquizlist;
                //}
                //else
                //    txtmsg.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowQuiz_Loaded_1 Method In ShowQuiz.cs file", ex);
            }
        }
        private void checkstate()
        {
            try
            {
                ApplicationViewState currentState = Windows.UI.ViewManagement.ApplicationView.Value;
                if (currentState == ApplicationViewState.Snapped)
                {
                    tblktext.Width = 300;
                    VisualStateManager.GoToState(this, "Snapped", false);
                }
                else
                {
                    VisualStateManager.GoToState(this, "Filled", false);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in checkstate Method In ShowQuiz.cs file", ex);
            }
        }
        public void GetPageDataInBackground()
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();

                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {

                                                a.Result = QuizManager.Getsubjects(AppSettings.ShowID/*, false*/);
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                objSubjectList = (List<QuizList>)a.Result;
                                                if (Constants.selecteditemquizlist.Count > 0)
                                                {
                                                    objSubjectList = Constants.selecteditemquizlist.ToList();
                                                    progressbar.IsActive = false;
                                                    txtxstackpl.Visibility = Visibility.Collapsed;
                                                    lstvwQuiz.Visibility = Visibility.Visible;
                                                    lstvwQuiz.ItemsSource = Constants.selecteditemquizlist;
                                                    itemlistview.ItemsSource = Constants.selecteditemquizlist;
                                                    txtmsg.Visibility = Visibility.Collapsed;
                                                    tblktext.Text = AppResources.ShowStartMessage as string;

                                                }
                                                else
                                                {
                                                    progressbar.IsActive = false;
                                                    txtmsg.Text = "No " + AppResources.ShowQuizPivotTitle as string + " available";
                                                    txtmsg.Visibility = Visibility.Visible;
                                                    lstvwQuiz.Visibility = Visibility.Collapsed;
                                                }

                                            }
                                          );

                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In ShowQuiz.cs file", ex);
            }
        }

        private void snaplstvwaudiosongs_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                if (itemlistview.SelectedIndex == -1)
                    return;
                if (check == true)
                {
                    check = false;
                    selecteditem = (sender as Selector).SelectedItem as QuizList;
                    Constants.Quizselecteditem = selecteditem;
                    AppSettings.ShowQuizId = (selecteditem.QuizID).ToString();
                    itemlistview.SelectedIndex = -1;
                    return;

                }
                tblktext.Width = 300;
                SubjectId = (itemlistview.SelectedItem as QuizList).QuizID;
                AppSettings.ShowQuizId = SubjectId.ToString();
                txtxstackpl.Visibility = Visibility.Visible;
                itemlistview.Visibility = Visibility.Collapsed;
                QuizManager.LoadMaxQuestionid();
                txttotquestions.Text = AppSettings.ShowMaxQuestionId.ToString();
                string timelimit = QuizManager.GetTimerForTest();
                string s = timelimit;
                string[] readlines2 = s.Split(':');
                txttimelimit.Text = time(readlines2);

                itemlistview.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in snaplstvwaudiosongs_SelectionChanged_1 Method In ShowQuiz.cs file", ex);
            }
        }

        private void imgstart_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                QuizManager.deletestoriongdata();
                Constants.Quizanswerlist.Clear();
                AppSettings.ShowQuestionId = "1";
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("QuestionsAndOptions").Invoke(p, null);
                //var pag = new QuestionsAndOptions();
                //Window.Current.Content = pag;
                //Window.Current.Activate();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in imgstart_Tapped_1 Method In ShowQuiz.cs file", ex);
            }
        }

        private void imgcancel_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                txtxstackpl.Visibility = Visibility.Collapsed;
                lstvwQuiz.Visibility = Visibility.Visible;
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("questionicondisable").Invoke(p, null);
                //((ApplicationBarMenuItem)SubjectDetail.current.ApplicationBar.MenuItems[1]).Text = "Add" + " " + "quiz";
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in imgcancel_Tapped_1 Method In ShowQuiz.cs file", ex);
            }
        }

        public void tblkvisible()
        {
            txtmsg.Visibility = Visibility.Visible;
            txtmsg.Text = "No Quiz available";

        }

        private void itemlistview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
