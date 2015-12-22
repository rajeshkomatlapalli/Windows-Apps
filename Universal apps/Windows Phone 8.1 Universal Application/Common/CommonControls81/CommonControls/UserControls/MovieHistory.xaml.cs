using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Common.Library;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class MovieHistory : UserControl
    {
        BackgroundHelper bwHelper = new BackgroundHelper();
        public MovieHistory()
        {
            try
            {
            this.InitializeComponent();
            progressbar.IsActive = true;
            Loaded += MovieHistory_Loaded;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in MovieHistory Method In MovieHistory.cs file", ex);
            }
        }

        void MovieHistory_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GetPageDataInBackground();
            }
            catch (Exception ex)
            {
            }
        }

        private void GetPageDataInBackground()
        {
            try
            {
                History history = new History();

                bwHelper.AddBackgroundTask(
                                      (object s, DoWorkEventArgs a) =>
                                      {
                                          a.Result = history.GetHistoryByShows(Constants.MovieHistoryFile);
                                      },
                                      (object s, RunWorkerCompletedEventArgs a) =>
                                      {
                                          if (((List<ShowList>)a.Result).Count != 0)
                                          {
                                              progressbar.IsActive = false;
                                              lstvwmoviehistory.ItemsSource = (List<ShowList>)a.Result;
                                          }
                                          else
                                          {
                                              progressbar.IsActive = false;

                                              if (AppSettings.ProjectName == "Video Mix")
                                                  txtmsg.Text = "No video mixes  available";
                                              else if (AppSettings.ProjectName == "Web Tile")
                                                  txtmsg.Text = "No tiles available";
                                              else
                                                  txtmsg.Text = "No movies  available";
                                              txtmsg.Visibility = Visibility.Visible;
                                          }
                                      }
                                    );
                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In MovieHistory.cs file", ex);
            }

        }

        private void lstvwmoviehistory_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selecteditem1 = (sender as Selector).SelectedItem as ShowList;

                AppSettings.ShowID = selecteditem1.ShowID.ToString();
                AppSettings.TileImage = selecteditem1.TileImage;
                AppSettings.ChannelImage = selecteditem1.Title;
                ObservableCollection<ShowLinks> objmovie = OnlineShow.GetShowLinksByType(AppSettings.ShowID, LinkType.Movies);
                foreach (ShowLinks objmovie1 in objmovie)
                {
                    AppSettings.LinkUrl = objmovie1.LinkUrl;
                }


                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("DetailPage").Invoke(p, null);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstvwmoviehistory_SelectionChanged_1 Method In MovieHistory.cs file", ex);
            }
        }
    }
}
