using Common.Library;
using Common.Utilities;
using OnlineVideos.Entities;
using OnlineVideos.Views;
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

namespace OnlineVideos.UserControls
{
    public sealed partial class RatingPopup_New : UserControl
    {
        string mid = string.Empty;
        string navigationPage = string.Empty;        
        string sd = string.Empty;
        Dictionary<string, object> KvPair = default(Dictionary<string, object>);
        string Linktype = string.Empty;
        string st = string.Empty;
        string QuizTitle = null;

        public RatingPopup_New()
        {
            this.InitializeComponent();
        }
        public void showPopup(string sid, string movid, double ratingvalue, string pageName, string quiztitle, string linktype)
        {
            AppSettings.bcount = true;
            mypopMessage.IsOpen = true;
            Linktype = linktype;
            rate.Value = ratingvalue;
            mid = sid;
            st = movid;
            QuizTitle = quiztitle;
            navigationPage = pageName;
        }
        public void showPopup(string MovieId, double ratingvalue, string pageName)
        {
            AppSettings.bcount = true;
            
            mypopMessage.IsOpen = true;
            sd = "Description";
            if (ResourceHelper.AppName == Apps.Web_Tile.ToString())
                rate.Value = ratingvalue * 2;
            else
            rate.Value = ratingvalue;
            mid = MovieId;

            navigationPage = pageName;
        }

        private void save_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                if (navigationPage == "onlineshare")
                {
                    KvPair["Rating"] = (Convert.ToDouble(rate.Value)).ToString();
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

                                desToUpdate.Rating = (Convert.ToDouble(rate.Value));
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
                                desToUpdate.Rating = (Convert.ToDouble(rate.Value));
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

                                    desToUpdate.Rating = (Convert.ToDouble(rate.Value));
                                    desToUpdate.RatingFlag = 1;
                                    Task.Run(async () => await Constants.connection.UpdateAsync(desToUpdate));
                                }
                            }
                        }
                        else
                        {
                            ShowList desToUpdate = query1.FirstOrDefault();

                            desToUpdate.Rating = (Convert.ToDouble(rate.Value));
                            desToUpdate.RatingFlag = 1;
                            Task.Run(async () => await Constants.connection.UpdateAsync(desToUpdate));
                        }

                    }
                    string[] parame=new string[2];
                    if (mid!=null)
                    {
                        bool Nid = AppSettings.NavigationID;
                        //PageHelper.NavigateTo(AppResources.DetailPageName, "1");
                        Frame frame = Window.Current.Content as Frame;
                        Page p = frame.Content as Page;
                        parame[0] = null;
                        parame[1] = QuizTitle;
                        p.Frame.Navigate(typeof(Details), parame);                        
                    }                    
                    close();
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in save_MouseLeftButtonDown Method In RatingPopup file", ex);
            }
        }
        public void close()
        {
            mypopMessage.IsOpen = false;            
            AppSettings.bcount = false;
        }
        private void cancel_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            close();
        }
    }
}
