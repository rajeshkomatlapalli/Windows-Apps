using Common.Library;
using Common.Utilities;
using OnlineMovies.Views;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineMovies.UserControls
{
    public sealed partial class QuizHistory : UserControl
    {
        List<QuizList> objSubjectList;
        private bool IsDataLoaded;

        public QuizHistory()
        {
            this.InitializeComponent();
            IsDataLoaded = false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsDataLoaded == false)
            {
                GetPageDataInBackground();
                IsDataLoaded = true;
                PageHelper.RemoveEntryFromBackStack("QuizHistory");
            }
        }

        private void GetPageDataInBackground()
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
                                                lbxSubjectsList.Visibility = Visibility.Visible;
                                                lbxSubjectsList.ItemsSource = objSubjectList;
                                                tblksubjectstest.Visibility = Visibility.Collapsed;
                                            }
                                            else
                                            {
                                                tblksubjectstest.Text = "No " + AppResources.ShowQuizPivotTitle as string + " available";
                                                tblksubjectstest.Visibility = Visibility.Visible;
                                                lbxSubjectsList.Visibility = Visibility.Collapsed;
                                            }

                                        }
                                      );

            bwHelper.RunBackgroundWorkers();


        }

        private void lbxSubjectsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxSubjectsList.SelectedIndex == -1)
                    return;
                AppSettings.ShowID = (lbxSubjectsList.SelectedItem as QuizList).ShowID.ToString();
                AppState.searchtitle = (lbxSubjectsList.SelectedItem as QuizList).Name;

                Frame f = Window.Current.Content as Frame;
                Page p = f.Content as Page;
                string[] parame = new string[2];
                parame[0] = AppSettings.ShowID.ToString();
                parame[1] = "Quiz";
                p.Frame.Navigate(typeof(SubjectDetail), parame);
                //(Application.Current.RootVisual as PhoneApplicationFrame).Navigate(NavigationHelper.getSubjectDetailPage((lbxSubjectsList.SelectedItem as QuizList), "true"));

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxSubjectsList_SelectionChanged Method In Favorites file.", ex);
            }
        }
    }
}
