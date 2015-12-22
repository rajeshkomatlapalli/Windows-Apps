using Common.Library;
using OnlineMovies.Views;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.UI;
using OnlineVideos.View_Models;
using OnlineVideos.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideos.UserControls
{
    public sealed partial class ShowQuiz : UserControl
    {
        #region GlobalDeclaration
        public static ShowQuiz currentshowquiz = default(ShowQuiz);
        private bool IsDataLoaded;        
        ShowDetail detailModel;
        ObservableCollection<QuizList> objSubjectList;        
        int SubjectId = 0;       
        #endregion

        public ShowQuiz()
        {
            this.InitializeComponent();            
            Loaded += ShowQuiz_Loaded;
            currentshowquiz = this;
            IsDataLoaded = false;            
            detailModel = new ShowDetail();
            objSubjectList = new ObservableCollection<QuizList>();
        }

        private void imgstart_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            QuizManager.deletestoriongdata();            
            Frame frame = Window.Current.Content as Frame;
            Page p = frame.Content as Page;
            p.Frame.Navigate(typeof(QuestionsDisplay));
        }

        private void imgcancel_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            txtxstackpl.Visibility = Visibility.Collapsed;
            lbxSubjectsList.Visibility = Visibility.Visible;            
        }        
        private void lbxSubjectsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int showid = AppSettings.ShowUniqueID;
            if (lbxSubjectsList.SelectedIndex == -1)
                return;
            SubjectId = (lbxSubjectsList.SelectedItem as QuizList).QuizID;
            AppSettings.ShowQuizId = (SubjectId.ToString());
            if (Task.Run(async () => await Constants.connection.Table<QuizQuestions>().Where(i => i.ShowID == showid && i.QuizNo == SubjectId).FirstOrDefaultAsync()).Result != null)
            {
                txtxstackpl.Visibility = Visibility.Visible;
                tblkquestionsnotavailable.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtxstackpl.Visibility = Visibility.Collapsed;
                tblkquestionsnotavailable.HorizontalAlignment = HorizontalAlignment.Center;
                tblkquestionsnotavailable.VerticalAlignment = VerticalAlignment.Center;
                tblkquestionsnotavailable.Text = "No Questions Available";
                tblkquestionsnotavailable.Visibility = Visibility.Visible;
            }            
            lbxSubjectsList.Visibility = Visibility.Collapsed;           
            SubjectDetail.current.btnadd.Label = "Add" + " " + "questions";
            QuizManager.LoadMaxQuestionid();
            txttotquestions.Text = AppSettings.ShowMaxQuestionId.ToString();
            string timelimit = QuizManager.GetTimerForTest();
            string s = timelimit;
            string[] readlines2 = s.Split(':');
            txttimelimit.Text = time(readlines2);

            lbxSubjectsList.SelectedIndex = -1;
        }

        private void ShowQuiz_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsDataLoaded == false)
            {                
                GetPageDataInBackground();
                IsDataLoaded = true;
            }
        }
        public string time(string[] p)
        {
            string r = string.Empty;
            TimeSpan t = new TimeSpan(Convert.ToInt32(p[0]), Convert.ToInt32(p[1]), Convert.ToInt32(p[2]));

            r = ((t.Hours.ToString() == "0") ? "" : ((Convert.ToInt32(t.Hours) > 1) ? t.Hours.ToString() + " hours" : t.Hours.ToString() + " hour")) + ((t.Minutes.ToString() == "0") ? "" : ((Convert.ToInt32(t.Minutes) > 1) ? t.Minutes.ToString() + " minutes" : t.Minutes.ToString() + " minute")) + ((t.Seconds.ToString() == "0") ? "" : ((Convert.ToInt32(t.Seconds) > 1) ? t.Seconds.ToString() + " seconds" : t.Seconds.ToString() + " second"));

            return r;
        }

        public void GetPageDataInBackground()
        {
            BackgroundHelper bwHelper = new BackgroundHelper();

            bwHelper.AddBackgroundTask(
                                        (object s, DoWorkEventArgs a) =>
                                        {

                                            a.Result = QuizManager.Getsubjects(AppSettings.ShowID/*, false*/);
                                        },
                                        (object s, RunWorkerCompletedEventArgs a) =>
                                        {

                                            objSubjectList = (ObservableCollection<QuizList>)a.Result;
                                            if (objSubjectList.Count > 0)
                                            {
                                                lbxSubjectsList.Visibility = Visibility.Visible;
                                                lbxSubjectsList.Loaded += LbxSubjectsList_Loaded;
                                                txtxstackpl.Visibility = Visibility.Collapsed;
                                                lbxSubjectsList.ItemsSource = objSubjectList;
                                                tblksubjectstest.Visibility = Visibility.Collapsed;
                                                tblktext.Text = AppResources.ShowStartMessage as string;                                                                                                                                                                                                                                        
                                            }
                                            else
                                            {
                                                tblksubjectstest.Text = "No " + AppResources.ShowQuizPivotTitle as string + " available";
                                                tblksubjectstest.Visibility = Visibility.Visible;                                                
                                            }

                                        }
                                      );

            bwHelper.RunBackgroundWorkers();
        }

        private void LbxSubjectsList_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Pin_Click(object sender, RoutedEventArgs e)
        {
            string showLinkTitle;
            ListViewItem selectedListBoxItem = this.lbxSubjectsList.ContainerFromItem((sender as MenuFlyoutItem).DataContext) as ListViewItem;
            if (selectedListBoxItem == null)
                return;
            showLinkTitle = (selectedListBoxItem.Content as QuizList).Name;
            AppSettings.ShowLinkTitle = AppSettings.ShowID + (selectedListBoxItem.Content as QuizList).Name;
            ShellTileHelper_New.PinVideoLinkToStartScreen(AppSettings.ShowID, selectedListBoxItem.Content as ShowLinks);
            //ShellTileHelper.PinSubjectLinkToStartScreen(AppSettings.ShowID, selectedListBoxItem.Content as QuizList);
        }

        private void Quizaddtofavorites_Click(object sender, RoutedEventArgs e)
        {
            detailModel.QuizAddToFavorites(lbxSubjectsList, sender as MenuFlyoutItem, LinkType.Songs);
        }

        private void Quizsharelink_Click(object sender, RoutedEventArgs e)
        {
            //detailModel.QuizPostAppLinkToSocialNetworks(lbxSubjectsList, sender as MenuFlyoutItem, " Write the test ");
            detailModel.PostAppLinkToSocialNetworks(lbxSubjectsList, sender as MenuFlyoutItem, " Write the test ");
        }

        private void Rating_Click(object sender, RoutedEventArgs e)
        {
            detailModel.QuizRateThisLink(lbxSubjectsList, sender as MenuFlyoutItem, "SubjectDetail");
        }

        private void del_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem menu = sender as MenuFlyoutItem;
            ListViewItem selectedListBoxItem = lbxSubjectsList.ContainerFromItem(menu.DataContext) as ListViewItem;

            int showid = Convert.ToInt32((selectedListBoxItem.Content as QuizList).ShowID);
            int quizno = (selectedListBoxItem.Content as QuizList).QuizID;

            ShowLinks xquery = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(id => id.ShowID == showid && id.LinkOrder == quizno && id.LinkType == "Quiz").FirstOrDefaultAsync()).Result;
            if (xquery != null)
            {
                if (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid && i.Status == "Custom").FirstOrDefaultAsync()).Result != null)
                {
                    Task.Run(async () => await Constants.connection.DeleteAsync(xquery));

                }
            }
            QuizList xquery1 = Task.Run(async () => await Constants.connection.Table<QuizList>().Where(id => id.ShowID == showid && id.QuizID == quizno).FirstOrDefaultAsync()).Result;
            if (xquery1 != null)
            {
                if (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid && i.Status == "Custom").FirstOrDefaultAsync()).Result != null)
                {
                    Task.Run(async () => await Constants.connection.DeleteAsync(xquery1));
                }
            }
            List<QuizQuestions> xquery2 = Task.Run(async () => await Constants.connection.Table<QuizQuestions>().Where(id => id.ShowID == showid && id.QuizNo == quizno).ToListAsync()).Result;
            if (xquery2.Count() > 0)
            {
                if (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid && i.Status == "Custom").FirstOrDefaultAsync()).Result != null)
                {
                    foreach (var ss in xquery2)
                    {
                        if (Task.Run(async () => await Storage.FileExists("Images/QuestionsImage/" + showid + "/" + ss.Image + ".jpg")).Result)
                        {
                            Storage.DeleteFile("Images/QuestionsImage/" + showid + "/" + ss.Image + ".jpg");
                        }
                        Task.Run(async () => await Constants.connection.DeleteAsync(ss));
                    }

                }
            }
            GetPageDataInBackground();
        }
        ListViewItem selectedListBoxItem;
        private void StackPanel_Holding(object sender, HoldingRoutedEventArgs e)
        {
            // this event is fired multiple times. We do not want to show the menu twice
            if (e.HoldingState != HoldingState.Started) return;

            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            // If the menu was attached properly, we just need to call this handy method
            FlyoutBase.ShowAttachedFlyout(element);

            FrameworkElement element1 = (FrameworkElement)e.OriginalSource;
            var datacontext = element1.DataContext;
            selectedListBoxItem = this.lbxSubjectsList.ContainerFromItem(datacontext) as ListViewItem;
        }

        private void MenuFlyout_Opened(object sender, object e)
        {
            try
            {
                MenuFlyout mainmenu = sender as MenuFlyout;
                foreach (MenuFlyoutItem contextMenuItem in mainmenu.Items)
                {
                    if (selectedListBoxItem == null)
                        return;

                    if (contextMenuItem.Name == "Pin")
                    {
                        string name = AppSettings.ShowID + (selectedListBoxItem.Content as QuizList).Name;
                        string ID = Regex.Replace(name, @"\s", "");
                        if (ShellTileHelper_New.IsPinned(ID) == true)
                        {
                            contextMenuItem.Text = Constants.UnpinFromStartScreen;
                        }
                        else
                        {
                            contextMenuItem.Text = Constants.PinToStartScreen;
                        }
                    }
                    if (contextMenuItem.Name == "Rating")
                    {
                        if (ResourceHelper.AppName == Apps.DrivingTest.ToString())
                            contextMenuItem.Text = "rate this test";
                        else
                            contextMenuItem.Text = "rate this quiz";
                    }
                    if (contextMenuItem.Name.Contains("favorites"))
                    {
                        int showid1 = Convert.ToInt32(AppSettings.ShowID);

                        int linkid = (selectedListBoxItem.Content as QuizList).LinkID;                        

                        if (Task.Run(async () => await Constants.connection.Table<QuizList>().Where(i => i.ShowID == showid1 && i.LinkID == linkid).FirstOrDefaultAsync()).Result.IsFavourite == false)
                        {
                            contextMenuItem.Text = "add to favorites";
                        }
                        else
                            contextMenuItem.Text = "remove from favorites";
                    }
                }
                int showid = AppSettings.ShowUniqueID;
                if (Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid && i.Status == "Custom").FirstOrDefaultAsync()).Result != null || ResourceHelper.AppName == Apps.Video_Mix.ToString())
                {
                    var item = mainmenu.Items.OfType<MenuFlyoutItem>().First(m => (string)m.Name == "del");
                    item.IsEnabled = true;
                }
                else
                {
                    var item = mainmenu.Items.OfType<MenuFlyoutItem>().First(m => (string)m.Name == "del");
                    item.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ContextMenu_Opened Method In ShowVideos.cs file.", ex);
            }
        }

    }
}
