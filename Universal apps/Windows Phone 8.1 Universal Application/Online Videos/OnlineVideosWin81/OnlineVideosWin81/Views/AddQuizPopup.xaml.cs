using AdRotator;
using Common.Library;
using InsertIntoDataBase;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideosWin81.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Common.Data;
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

namespace OnlineVideos.Views
{
    public sealed partial class AddQuizPopup : UserControl
    {
        AddShow addshow = new AddShow();
        public AddQuizPopup()
        {
            this.InitializeComponent();
            Loaded += AddQuizPopup_Loaded;
        }

        private void AddQuizPopup_Loaded(object sender, RoutedEventArgs e)
        {
            Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
            //AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
            //adcontrol.IsAdRotatorEnabled = false;
            //adcontrol.Visibility = Visibility.Collapsed;
            //adcontrol.Height = 0;
            //adcontrol.Width = 0;
        }

        private void close_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            PopupManager.EnableControl();
            //Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
            //AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
            //adcontrol.IsAdRotatorEnabled = true;
            //adcontrol.Visibility = Visibility.Visible;
            //adcontrol.Height = 90;
            //adcontrol.Width = 728;
        }

        private void save_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            List<ShowLinks> showlinks = addshow.GetShowLinks();
            List<ShowLinks> showlinks1 = new List<ShowLinks>();
            //int quizno = default(int);
            if (!string.IsNullOrEmpty(tbxquizname.Text))
            {
                req.Visibility = Visibility.Collapsed;
                int showid = AppSettings.ShowUniqueID;

                InsertData<ShowLinks> insert3 = new InsertData<ShowLinks>();
                insert3.ParameterList = new ShowLinks();
                insert3.ParameterList.ShowID = showid;
                insert3.ParameterList.LinkType = "Quiz";
                insert3.ParameterList.Title = "Quiz";
                insert3.ParameterList.Rating = 4.0;
                insert3.ParameterList.LinkOrder = (showlinks.Where(i => i.ShowID == showid).FirstOrDefault() != null) ? showlinks.Where(i => i.ShowID == showid).OrderByDescending(i => i.LinkOrder).FirstOrDefault().LinkOrder + 1 : 1;
                insert3.ParameterList.ClientShowUpdated = System.DateTime.Now;
                insert3.ParameterList.ClientPreferenceUpdated = System.DateTime.Now;
                insert3.Insert();
                InsertData<QuizList> insert = new InsertData<QuizList>();
                insert.ParameterList = new QuizList();
                insert.ParameterList.ShowID = showid;
                try
                {
                    //ShowLinks link = new ShowLinks();
                    //showlinks.Reverse();
                    //link = showlinks.Where(j => j.ShowID == showid).FirstOrDefault();
                    showlinks1 = addshow.GetShowLinks();
                    int id = showlinks1.Where(i => i.ShowID == showid).OrderByDescending(i => i.LinkID).FirstOrDefault().LinkID;
                    insert.ParameterList.LinkID = id;

                }
                catch (Exception ex)
                {
                }
                try
                {
                    insert.ParameterList.QuizID = showlinks1.Where(i => i.ShowID == showid).OrderByDescending(i => i.LinkOrder).FirstOrDefault().LinkOrder;
                }
                catch (Exception ex)
                {

                }
                //quizno = insert.ParameterList.QuizID;
                Constants.QuizId = (insert.ParameterList.QuizID);

                AppSettings.ShowQuizId =(insert.ParameterList.QuizID).ToString();
                insert.ParameterList.Rating = 4.0;
                insert.ParameterList.Timelimt = "00:20:00";
                insert.ParameterList.Name = tbxquizname.Text;

                insert.Insert();
                PopupManager.EnableControl();
                //Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                //AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
                //adcontrol.IsAdRotatorEnabled = true;
                //adcontrol.Visibility = Visibility.Visible;
                //adcontrol.Height = 90;
                //adcontrol.Width = 728;
                ShowQuiz.currentshowquiz.GetPageDataInBackground();
            }
            else
            {
                req.Visibility = Visibility.Visible;
            }
        }
    }
}
