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
    public sealed partial class QuizFavorite : UserControl
    {
        List<QuizList> showFavSub = null;
        bool check = false;
        QuizList selecteditem = null;
        public QuizFavorite()
        {
            try
            {
                this.InitializeComponent();
                progressbar.IsActive = true;
                Loaded += QuizFavorite_Loaded;
            }
            catch (Exception ex)
            {                
               Exceptions.SaveOrSendExceptions("Exception in QuizFavorite Method In QuizFavorite.cs file", ex);
            }
        }

        void QuizFavorite_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GetPageDataInBackground();
                //List<QuizList> objlist = new List<QuizList>();
                //objlist = FavoritesManager.GetFavoritequiz();
                //if (objlist.Count != 0)
                //{
                //    lstvwquizfav.ItemsSource = objlist;
                //}
                //else
                //    txtmsg.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in QuizFavorite_Loaded Method In QuizFavorite.cs file", ex);
            }
        }

        public void GetPageDataInBackground()
        {
            try
            {
                if (AppSettings.FavoritesSelectedIndex != "")
                {
                    lstvwquizfav.ItemsSource = null;
                    lstvwquizfav.Items.Clear();
                    AppSettings.FavoritesSelectedIndex = "";
                }
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
                                                       progressbar.IsActive = false;
                                                       lstvwquizfav.ItemsSource = showFavSub;
                                                       txtmsg.Visibility = Visibility.Collapsed;
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
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In QuizFavorite.cs file", ex);
            }
        }

        private void tblkquiz_RightTapped_1(object sender, RightTappedRoutedEventArgs e)
        {
            try
            {
                Constants.Favoritesselecteditem = null;
                check = true;
                Constants.Favoriteappbarvisible = true;
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("appbar").Invoke(p, new object[] { true });
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in tblkquiz_RightTapped_1 Method In QuizFavorite.cs file", ex);
            }
        }

        private void lstvwquizfav_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstvwquizfav.SelectedIndex == -1)
                    return;
                if (check == true)
                {
                    check = false;
                    selecteditem = (sender as Selector).SelectedItem as QuizList;
                    Constants.Quizselecteditem = selecteditem;
                    AppSettings.ShowQuizId = ((lstvwquizfav.SelectedItem as QuizList).QuizID).ToString();
                    AppSettings.ShowID = (lstvwquizfav.SelectedItem as QuizList).ShowID.ToString();
                    AppSettings.FavoritesSelectedIndex = lstvwquizfav.SelectedIndex.ToString();
                    lstvwquizfav.SelectedIndex = -1;
                    return;

                }
                AppSettings.ShowID = (lstvwquizfav.SelectedItem as QuizList).ShowID.ToString();
                AppSettings.QuizFavTitle = (lstvwquizfav.SelectedItem as QuizList).Name;
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("DetailPage").Invoke(p, null);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstvwquizfav_SelectionChanged_1 Method In QuizFavorite.cs file", ex);
            }
        }
    }
}
