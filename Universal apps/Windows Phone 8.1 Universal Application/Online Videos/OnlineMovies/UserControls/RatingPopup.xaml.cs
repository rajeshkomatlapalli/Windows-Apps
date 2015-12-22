using Common.Library;
using Common.Utilities;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class RatingPopup : UserControl
    {
        #region GlobalDeclaration
        string mid = string.Empty;
        string st = string.Empty;
        string sd = string.Empty;
        string Linktype = string.Empty;
        string QuizTitle = string.Empty;
        private static PageHelper oldstate;
        string navigationPage = string.Empty;
        Dictionary<string, object> KvPair = default(Dictionary<string, object>);
        #endregion

        public RatingPopup()
        {
            this.InitializeComponent();
        }
        #region "Common Methods"
        public void showPopup(string sid, string movid, double ratingvalue, string pageName, string quiztitle, string linktype)
        {
            AppSettings.bcount = true;
            oldstate = PageHelper.InactivatePage(true);

            mypopMessage.IsOpen = true;
            Linktype = linktype;
            rate.Value = Convert.ToInt32(ratingvalue * 2);
            mid = sid;
            st = movid;
            QuizTitle = quiztitle;
            navigationPage = pageName;

        }
        public void showPopup(int ratingvalue, string pageName, Dictionary<string, object> k)
        {
            KvPair = k;
            oldstate = PageHelper.InactivatePage(true);
            mypopMessage.IsOpen = true;
            rate.Value = Convert.ToInt32(ratingvalue * 2);
            navigationPage = pageName;
        }
        public void showPopup(string MovieId, int ratingvalue, string pageName)
        {
            AppSettings.bcount = true;
            oldstate = PageHelper.InactivatePage(true);
            mypopMessage.IsOpen = true;
            sd = "Description";
            if (ResourceHelper.AppName == Apps.Web_Tile.ToString())
                rate.Value = Convert.ToInt32(ratingvalue * 2);
            else
                rate.Value = ratingvalue;
            mid = MovieId;

            navigationPage = pageName;
        }
        public void close()
        {
            mypopMessage.IsOpen = false;
            oldstate = PageHelper.InactivatePage(false);
            AppSettings.bcount = false;
        }
        #endregion

        private void save_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                if (navigationPage == "onlineshare")
                {
                    KvPair["Rating"] = (Convert.ToDouble(rate.Value) / 2).ToString();
                    close();
                }
                else
                {
                    int id = int.Parse(mid);
                    int showid = default(int);
                    if (!string.IsNullOrEmpty(st))
                    {
                        showid = int.Parse(st);
                    }
                    var query = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkOrder == id & i.ShowID == showid && i.LinkType == Linktype).ToListAsync()).Result;
                    var query1 = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(k => k.ShowID == id).ToListAsync()).Result;
                    if (sd == "Description")
                    {
                        if (query1.Count() > 0)
                        {
                            foreach (var s in query1)
                            {
                                ShowList desToUpdate = query1.FirstOrDefault();

                                desToUpdate.Rating = (Convert.ToDouble(rate.Value) / 2);
                                desToUpdate.RatingFlag = 1;
                                Task.Run(async () => await Constants.connection.UpdateAsync(desToUpdate));
                            }
                        }
                    }
                    if (QuizTitle == "Quiz")
                    {
                        var query2 = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.LinkOrder == id & i.ShowID == showid & i.LinkType == "Quiz").ToListAsync()).Result;
                        if (query2.Count() > 0)
                        {
                            foreach (var s in query2)
                            {
                                ShowLinks desToUpdate = query2.FirstOrDefault();
                                desToUpdate.Rating = (Convert.ToDouble(rate.Value) / 2);
                                desToUpdate.RatingFlag = 1;
                                Task.Run(async () => await Constants.connection.UpdateAsync(desToUpdate));
                            }
                        }
                    }
                    else
                    {
                        if (AppResources.AllowRatingLinks)
                        {
                            if (query.Count() > 0)
                            {
                                foreach (var s in query)
                                {
                                    ShowLinks desToUpdate = query.FirstOrDefault();

                                    desToUpdate.Rating = (Convert.ToDouble(rate.Value) / 2);
                                    desToUpdate.RatingFlag = 1;
                                    Task.Run(async () => await Constants.connection.UpdateAsync(desToUpdate));
                                }
                            }
                        }
                        else
                        {
                            ShowList desToUpdate = query1.FirstOrDefault();

                            desToUpdate.Rating = (Convert.ToDouble(rate.Value) / 2);
                            desToUpdate.RatingFlag = 1;
                            Task.Run(async () => await Constants.connection.UpdateAsync(desToUpdate));
                        }

                    }

                    if (!AppSettings.NavigationID)
                        PageHelper.NavigateTo(AppResources.DetailPageName, "1");
                    else
                        PageHelper.NavigateTo(AppResources.DetailPageName, "0");
                    close();
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in save_MouseLeftButtonDown Method In RatingPopup file", ex);
            }
        }

        private void cancel_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            close();
        }
    }
}
