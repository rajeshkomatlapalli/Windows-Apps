using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Reflection;
using Common.Library;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class QuizHistory : UserControl
    {
        List<QuizList> objSubjectList;
        public QuizHistory()
        {
            try
            {
            this.InitializeComponent();
            progressbar.IsActive = true;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in QuizHistory Method In QuizHistory.cs file", ex);
            }
        }

        private void QuizHistory_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                GetPageDataInBackground();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in QuizHistory_Loaded_1 Method In QuizHistory.cs file", ex);
            }
        }
        private void GetPageDataInBackground()
        {
            //List<QuizList> objlist = new List<QuizList>();
            //objlist = QuizManager.GetsubjectsHistory();
            //if (objlist.Count != 0)
            //{
            //    lstvwQuiz.ItemsSource = objlist;
            //}
            //else
            //    tblksubjectstest.Visibility = Visibility.Visible;
            try
            {

                BackgroundHelper bwHelper = new BackgroundHelper();

                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {

                                                a.Result = QuizManager.GetsubjectsHistory();
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                objSubjectList = (List<QuizList>)a.Result;
                                                if (objSubjectList.Count > 0)
                                                {
                                                    progressbar.IsActive = false;
                                                    lstvwQuiz.Visibility = Visibility.Visible;
                                                    lstvwQuiz.ItemsSource = objSubjectList;
                                                    txtmsg.Visibility = Visibility.Collapsed;
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
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In QuizHistory.cs file", ex);
            }
        }

        private void lstvwQuiz_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                AppSettings.ShowID = (lstvwQuiz.SelectedItem as QuizList).ShowID.ToString();
                AppSettings.QuizFavTitle = (lstvwQuiz.SelectedItem as QuizList).Name;
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("DetailPage").Invoke(p, null);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstvwQuiz_SelectionChanged_1  Method In QuizHistory.cs file", ex);
            }
        }
    }
}
