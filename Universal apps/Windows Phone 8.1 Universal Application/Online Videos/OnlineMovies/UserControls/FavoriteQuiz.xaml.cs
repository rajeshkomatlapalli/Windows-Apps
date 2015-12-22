using Common.Library;
using Common.Utilities;
using OnlineMovies.Views;
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
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Input;
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
    public sealed partial class FavoriteQuiz : UserControl
    {
        #region GlobalDeclaration
        List<QuizList> showFavSub = null;

        string link = string.Empty;
        private bool IsDataLoaded;
        #endregion

        public FavoriteQuiz()
        {
            this.InitializeComponent();
            IsDataLoaded = false;
            showFavSub = new List<QuizList>();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsDataLoaded == false)
            {
                GetPageDataInBackground();
                IsDataLoaded = true;
                PageHelper.RemoveEntryFromBackStack("FavoriteQuiz");
            }
        }

        private void Quizremovefromfavorites_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lbxFavoritesubjects == null)
                    return;
                ListViewItem selectedListBoxItem = this.lbxFavoritesubjects.ContainerFromItem((sender as MenuFlyoutItem).DataContext) as ListViewItem;

                FavoritesManager.QuizAddOrRemoveFavorite((selectedListBoxItem.Content as QuizList).ShowID, (selectedListBoxItem.Content as QuizList).QuizID);

                Frame f = Window.Current.Content as Frame;
                Page p = f.Content as Page;
                p.Frame.Navigate(typeof(MainPage));

            }
            catch (Exception ex)
            {
                string mess = "Exception in examremovefromfavorites_Click Method In Favorites file.\n\n" + ex.Message + "\n\n Stack Trace:- " + ex.StackTrace;
                Exceptions.SaveOrSendExceptions(mess, ex);
            }
        }
        private void GetPageDataInBackground()
        {
            BackgroundHelper bwHelper = new BackgroundHelper();

            bwHelper.AddBackgroundTask(
                                           (object s, DoWorkEventArgs a) =>
                                           {
                                               a.Result = FavoritesManager.GetFavoritequiz();
                                           },
                                           (object s, RunWorkerCompletedEventArgs a) =>
                                           {


                                               showFavSub = (List<QuizList>)a.Result;
                                               if (showFavSub.Count > 0)
                                               {
                                                   lbxFavoritesubjects.ItemsSource = showFavSub;
                                                   tblkFavNosubjects.Visibility = Visibility.Collapsed;
                                               }
                                               else
                                               {
                                                   tblkFavNosubjects.Text = "No " + AppResources.ShowQuizPivotTitle + " available";
                                                   tblkFavNosubjects.Visibility = Visibility.Visible;
                                               }
                                           }
                                         );


            bwHelper.RunBackgroundWorkers();
        }

        private void lbxFavoritesubjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbxFavoritesubjects.SelectedIndex == -1)
                    return;
                AppState.searchtitle = (lbxFavoritesubjects.SelectedItem as QuizList).Name;
                //PageHelper.NavigateTo(NavigationHelper.getSubjectDetailPage((lbxFavoritesubjects.SelectedItem as QuizList), "true"));
                Frame f = Window.Current.Content as Frame;
                Page p = f.Content as Page;
                string[] parame = new string[2];
                //Status.listitem = lbxFavoritesubjects.SelectedItem.ToString();
                parame[0] = (lbxFavoritesubjects.SelectedItem as QuizList).ShowID.ToString();
                parame[1] = "Quiz";
                p.Frame.Navigate(typeof(SubjectDetail), parame);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lbxMovies_SelectionChanged Method In Favorites file.", ex);
            }
        }

        private void StackPanel_Holding(object sender, HoldingRoutedEventArgs e)
        {
            if (e.HoldingState != HoldingState.Started) return;

            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;

            FlyoutBase.ShowAttachedFlyout(element);
        }
    }
}
