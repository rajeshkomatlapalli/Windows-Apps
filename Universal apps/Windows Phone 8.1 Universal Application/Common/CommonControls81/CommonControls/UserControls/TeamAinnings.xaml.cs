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
    public sealed partial class TeamAinnings : UserControl
    {
        MatchExtras objExtras = null;
        List<MatchBowlingScore> objbowlinginnings = null;
        List<MatchBattingScore> objbattinginnings = null;
        MatchExtras objextras = null;
        public string Teaminnings = string.Empty;
        public TeamAinnings()
        {
            try
            {
            this.InitializeComponent();
            objbattinginnings = new List<MatchBattingScore>();
            objbowlinginnings = new List<MatchBowlingScore>();
            objextras = new MatchExtras();
            Loaded += TeamAinnings_Loaded;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in TeamAinnings Method In TeamAinnings.cs file", ex);
            }
        }

        private void TeamAinnings_Loaded(object sender, RoutedEventArgs e)
        {


            try
            {
                GetPageDataInBackground();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in TeamAinnings_Loaded Method In TeamAinnings.cs file", ex);
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

                                                a.Result = CricketMatch.LoadBattingInnings(AppSettings.ShowID, TeamType.TeamABatting);
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {

                                                objbattinginnings = (List<MatchBattingScore>)a.Result;
                                                if (objbattinginnings.Count > 0)
                                                {
                                                    lbxteamAbatting.ItemsSource = objbattinginnings;
                                                    //tblkFavNoMovies.Visibility = Visibility.Collapsed;

                                                    tblktitlebowler.Visibility = Visibility.Visible;
                                                    tblkbowlertitle.Visibility = Visibility.Visible;

                                                }
                                                else
                                                {
                                                    //tblkFavNoMovies.Text = "No " + AppResources.ShowDetailPageChaptersPivotTitle + " available";
                                                    //tblkFavNoMovies.Visibility = Visibility.Visible;
                                                }

                                            }
                                          );

                bwHelper.AddBackgroundTask(
                                          (object s, DoWorkEventArgs a) =>
                                          {

                                              a.Result = CricketMatch.LoadbowlingInnings(AppSettings.ShowID, TeamType.TeamBBowling);
                                          },
                                          (object s, RunWorkerCompletedEventArgs a) =>
                                          {

                                              objbowlinginnings = (List<MatchBowlingScore>)a.Result;
                                              if (objbowlinginnings.Count > 0)
                                              {
                                                  lbxteamBBbowling.ItemsSource = objbowlinginnings;
                                                  //tblkFavNoMovies.Visibility = Visibility.Collapsed;

                                              }
                                              else
                                              {
                                                  //tblkFavNoMovies.Text = "No " + AppResources.ShowDetailPageChaptersPivotTitle + " available";
                                                  //tblkFavNoMovies.Visibility = Visibility.Visible;
                                              }

                                          }
                                        );

                bwHelper.AddBackgroundTask(
                                       (object s, DoWorkEventArgs a) =>
                                       {

                                           a.Result = CricketMatch.GetExtas(AppSettings.ShowID);
                                       },
                                       (object s, RunWorkerCompletedEventArgs a) =>
                                       {
                                           objextras = (MatchExtras)a.Result;
                                           if (objextras != null)
                                           {
                                               tblkTeamAExtra.Text = "Extras : " + "" + objextras.TeamAExtras;
                                               tblkTeamATotal.Text = "Total   :  " + "" + objextras.TeamATotal;




                                           }
                                           else
                                           {
                                               //tblkFavNoMovies.Text = "No " + AppResources.ShowDetailPageChaptersPivotTitle + " available";
                                               //tblkFavNoMovies.Visibility = Visibility.Visible;
                                           }
                                       }
                                     );




                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In TeamAinnings.cs file", ex);
            }

        }
        private void btnMatchResult_Click(object sender, RoutedEventArgs e)
        {


        }

    }
}
