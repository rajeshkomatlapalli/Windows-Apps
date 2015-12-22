using Common.Library;
using InsertIntoDataBase;
using OnlineMovies.Views;
using OnlineVideos.Entities;
using OnlineVideos.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

namespace OnlineMovies.UserControls
{
    public sealed partial class AddQuizPopup : UserControl
    {
        AddShow addshow = new AddShow();
        public AddQuizPopup()
        {
            this.InitializeComponent();
        }

        private void save_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            int showid = AppSettings.ShowUniqueID;
            List<ShowLinks> showlinks = addshow.GetShowLinks();
            InsertData<ShowLinks> insert3 = new InsertData<ShowLinks>();
            insert3.ParameterList = new ShowLinks();
            insert3.ParameterList.ShowID = showid;
            insert3.ParameterList.LinkType = "Quiz";
            insert3.ParameterList.Title = "Quiz";
            insert3.ParameterList.Rating = (rate.Value) / 2;
            insert3.ParameterList.LinkOrder = (showlinks.Where(i => i.ShowID == showid).FirstOrDefault() != null) ? showlinks.Where(i => i.ShowID == showid).Max(i => i.LinkOrder) + 1 : 1;
            insert3.ParameterList.ClientShowUpdated = System.DateTime.Now;
            insert3.ParameterList.ClientPreferenceUpdated = System.DateTime.Now;
            insert3.Insert();
            InsertData<QuizList> insert = new InsertData<QuizList>();
            insert.ParameterList = new QuizList();
            insert.ParameterList.ShowID = showid;
            //quizno = insert.ParameterList.QuizID;
            insert.ParameterList.LinkID = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid).OrderByDescending(i => i.LinkID).FirstOrDefaultAsync()).Result.LinkID;
            insert.ParameterList.QuizID = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid).OrderByDescending(i => i.LinkOrder).FirstOrDefaultAsync()).Result.LinkOrder;
            insert.ParameterList.Rating = (rate.Value) / 2;
            insert.ParameterList.Timelimt = "00:20:00";
            insert.ParameterList.Name = tbxquizname.Text;

            insert.Insert();
            Constants.DownloadTimer.Start();
            ShowQuiz.currentshowquiz.GetPageDataInBackground();
            //SubjectDetail.current.LayoutRoot.Children.Remove(SubjectDetail.current.quizpop);
        }

        private void cancel_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            //SubjectDetail.current.LayoutRoot.Children.Remove(SubjectDetail.current.quizpop);
            Constants.DownloadTimer.Start();
        }
    }
}
