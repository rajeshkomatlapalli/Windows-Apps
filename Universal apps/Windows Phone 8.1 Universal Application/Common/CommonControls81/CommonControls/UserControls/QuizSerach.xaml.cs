using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Entities;
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
using System.Reflection;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class QuizSerach : UserControl
    {
        List<QuizList> matchedSubjects = null;
        public QuizSerach()
        {
            try
            {
            this.InitializeComponent();
            progressbar.IsActive = true;
            Loaded += QuizSerach_Loaded;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in QuizSerach Method In QuizSerach.cs file", ex);
            }
        }

        void QuizSerach_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GetPageDataInBackground();
                //List<QuizList> objquizlist = new List<QuizList>();
                //objquizlist = SearchManager.GetQuizBySearch(AppSettings.SearchText);
                //if (objquizlist.Count != 0)
                //{
                //    lstvwquizsearch.ItemsSource = objquizlist;
                //}
                //else
                //    txtmsg.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in QuizSerach_Loaded Method In QuizSerach.cs file", ex);
            }

        }
        private void GetPageDataInBackground()
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();

                bwHelper.AddBackgroundTask(
                      (object s, DoWorkEventArgs a) =>
                      {
                          a.Result = SearchManager.GetQuizBySearch(AppSettings.SearchText);
                      },
                      (object s, RunWorkerCompletedEventArgs a) =>
                      {
                          matchedSubjects = (List<QuizList>)a.Result;
                          if (matchedSubjects.Count > 0)
                          {
                              progressbar.IsActive = false;
                              lstvwquizsearch.ItemsSource = matchedSubjects;
                          }
                          else
                          {
                              progressbar.IsActive = false;
                              txtmsg.Text = "No " + AppResources.ShowQuizPivotTitle + " available";
                              txtmsg.Visibility = Visibility.Visible;
                          }

                      }
                      );

                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In QuizSerach.cs file", ex);
            }
        }

        private void lstvwquizsearch_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                AppSettings.ShowID = (lstvwquizsearch.SelectedItem as QuizList).ShowID.ToString();
                AppSettings.QuizFavTitle = (lstvwquizsearch.SelectedItem as QuizList).Name;
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("DetailPage").Invoke(p, null);
                //var rootFrame = new Frame();
                //rootFrame.Navigate(typeof(Detail));
                //Window.Current.Content = rootFrame;
                //Window.Current.Activate();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstvwquizsearch_SelectionChanged_1 Method In QuizSerach.cs file", ex);
            }
        }
    }
}
