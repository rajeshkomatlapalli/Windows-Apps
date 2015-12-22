using Common.Library;
//using Microsoft.Phone.Scheduler;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
#if WP8
using System.IO.IsolatedStorage;
using System.Windows.Threading;
#endif
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Xaml;
using Windows.Storage;
using Windows.ApplicationModel.Background;

namespace OnlineVideos.ViewModels
{
    public class AddReminder
    {
        public DispatcherTimer timer = new DispatcherTimer();

        //IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
       // Windows.Storage.ApplicationDataContainer settings = Windows.Storage.ApplicationData.Current.LocalSettings;
        Windows.Storage.ApplicationDataContainer settings = Windows.Storage.ApplicationData.Current.LocalSettings;
        public async void Add()
        {            
            try
            {
                //Deployment.Current.Dispatcher.BeginInvoke(() =>
                await Windows.UI.Xaml.Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                {
                    timer.Stop();
                });
                int CheckShowID = 1;
                int CheckID = 1;
                int LessValue = 0;
                int GreatValue = 0;
               
                if (Task.Run(async()=>await Constants.connection.Table<ReminderTable>().FirstOrDefaultAsync()).Result != null)
                {
                    if (settings.Values.ContainsKey("LastCheckedReminderShowID"))
                    {
                        CheckID = (int)settings.Values["LastCheckedReminderShowID"];
                        if (CheckID > Task.Run(async()=>await Constants.connection.Table<ReminderTable>().ToListAsync()).Result.Max(i => i.ID))
                        {
                            CheckID = Task.Run(async()=>await Constants.connection.Table<ReminderTable>().ToListAsync()).Result.Min(i => i.ID);
                        }
                    }
                    if (Task.Run(async()=>await Constants.connection.Table<ReminderTable>().Where(i => i.ID == CheckID).FirstOrDefaultAsync()).Result != null)
                    {
                        CheckShowID = Task.Run(async()=>await Constants.connection.Table<ReminderTable>().Where(i => i.ID == CheckID).FirstOrDefaultAsync()).Result.ShowID;
                        LessValue = Task.Run(async()=>await Constants.connection.Table<ReminderTable>().Where(i => i.ID == CheckID).FirstOrDefaultAsync()).Result.LessThan;
                        GreatValue = Task.Run(async()=>await Constants.connection.Table<ReminderTable>().Where(i => i.ID == CheckID).FirstOrDefaultAsync()).Result.GreaterThan;
                        int[] Values = new int[4];
                        Values[0] = CheckShowID;
                        Values[1] = LessValue;
                        Values[2] = GreatValue;
                        Values[3] = CheckID;
                        Check(Values);
                    }
                    else
                    {
                        if (settings.Values.ContainsKey("LastCheckedReminderShowID"))
                            settings.Values["LastCheckedReminderShowID"] = CheckID + 1;
                        else
                            settings.Values.Add("LastCheckedReminderShowID", CheckID + 1);
                        //settings.Save();
                        //settings.Values("LastCheckedReminderShowID");
                    }
                    //Deployment.Current.Dispatcher.BeginInvoke(() =>
                    await Windows.UI.Xaml.Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                    {
                        timer.Start();
                    });
                }
                else
                {
                    //Deployment.Current.Dispatcher.BeginInvoke(() =>
                    await Windows.UI.Xaml.Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                    {
                        timer.Stop();
                    });
                }
            }
            catch (Exception ex)
            {
            }
        }

        void Check(int[] val)
        {
            try
            {
                int[] values = new int[4];
                values = val;

               
                if (Task.Run(async()=>await Constants.connection.Table<WebDailyTable>().FirstOrDefaultAsync()).Result != null)
                {
                    if (val[2].ToString() != "0")
                    {

                        int showid = Convert.ToInt32(values[0].ToString());
                        double selectedvalue = Convert.ToDouble(values[2].ToString());
                        List<WebDailyTable> list = Constants.connection.Table<WebDailyTable>().Where(i => i.ShowID == showid).ToListAsync().Result;
                        foreach (WebDailyTable dt in list)
                        {
                            if (Convert.ToDouble(dt.SelectedText) > selectedvalue)
                            {
                                //AddRemOrAlaram(Convert.ToInt32(values[3].ToString()), "WebTile- " + Constants.connection.Table<ShowList>().Where(j => j.ShowID == showid).FirstOrDefaultAsync().Result.Title, "Data in this web tile has passed high limit", "Great");

                                if (SettingsHelper.Contains("UserEmail"))
                                {
                                    if (!string.IsNullOrEmpty(SettingsHelper.getStringValue("UserEmail").ToString()))
                                    {
                                        //CloudService.SendMailWebTile("Web Tile- " + Task.Run(async () => await Constants.connection.Table<ShowList>().Where(j => j.ShowID == showid).FirstOrDefaultAsync()).Result.Title, "Data in this web tile has passed high limit", settings["UserEmail"].ToString());
                                    }
                                }
                                break;
                            }
                        }
                    }
                    //if (val[2].ToString() != "0")
                    //{
                    //    int showid = Convert.ToInt32(values[0].ToString());
                    //    if (Task.Run(async () => await Constants.connection.Table<WebDailyTable>().Where(i => i.ShowID == showid && Convert.ToDouble(i.SelectedText) > Convert.ToDouble(values[2].ToString())).FirstOrDefaultAsync()).Result != null)
                    //    {
                            
                    //        AddRemOrAlaram(Convert.ToInt32(values[3].ToString()), "Web Tile- " + Task.Run(async () => await Constants.connection.Table<ShowList>().Where(j => j.ShowID == showid).FirstOrDefaultAsync()).Result.Title, "Data in this web tile has passed high limit", "Great");

                    //        if (settings.Contains("UserEmail"))
                    //        {
                    //            if (!string.IsNullOrEmpty(settings["UserEmail"].ToString()))
                    //                CloudService.SendMailWebTile("Web Tile- " + Task.Run(async () => await Constants.connection.Table<ShowList>().Where(j => j.ShowID == showid).FirstOrDefaultAsync()).Result.Title, "Data in this web tile has passed high limit", settings["UserEmail"].ToString());
                    //        }
                    //    }
                    //}
                    if (val[1].ToString() != "0")
                    {
                        if (Constants.connection.Table<WebDailyTable>().FirstOrDefaultAsync().Result != null)
                        {
                            int showid = Convert.ToInt32(values[0].ToString());
                            double selectedvalue = Convert.ToDouble(values[1].ToString());
                            string sd = Constants.connection.Table<WebDailyTable>().Where(i => i.ShowID == showid).FirstOrDefaultAsync().Result.SelectedText;
                            int pp = 0;
                            List<WebDailyTable> list = Constants.connection.Table<WebDailyTable>().Where(i => i.ShowID == showid).ToListAsync().Result;
                            foreach (WebDailyTable dt in list)
                            {
                                if (Convert.ToDouble(dt.SelectedText) < selectedvalue)
                                {
                                    //AddRemOrAlaram(Convert.ToInt32(values[3].ToString()), "WebTile- " + Constants.connection.Table<ShowList>().Where(j => j.ShowID == showid).FirstOrDefaultAsync().Result.Title, "Data in this web tile has passed low limit", "Less");

                                    if (SettingsHelper.Contains("UserEmail"))
                                    {
                                        if (!string.IsNullOrEmpty(SettingsHelper.getStringValue("UserEmail").ToString()))
                                        {
                                            //CloudService.SendMailWebTile("Web Tile- " + Task.Run(async () => await Constants.connection.Table<ShowList>().Where(j => j.ShowID == showid).FirstOrDefaultAsync()).Result.Title, "Data in this web tile has passed low limit", settings["UserEmail"].ToString());
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    //if (val[1].ToString() != "0")
                    //{
                    //    int showid = Convert.ToInt32(values[0].ToString());
                    //    if (Task.Run(async () => await Constants.connection.Table<WebDailyTable>().Where(i => i.ShowID == showid && Convert.ToDouble(i.SelectedText) < Convert.ToDouble(values[1].ToString())).FirstOrDefaultAsync()).Result != null)
                    //    {
                    //        AddRemOrAlaram(Convert.ToInt32(values[3].ToString()), "Web Tile- " + Task.Run(async () => await Constants.connection.Table<ShowList>().Where(j => j.ShowID == showid).FirstOrDefaultAsync()).Result.Title, "Data in this web tile has passed low limit", "Less");

                    //        if (settings.Contains("UserEmail"))
                    //        {
                    //            if (!string.IsNullOrEmpty(settings["UserEmail"].ToString()))
                    //                CloudService.SendMailWebTile("Web Tile- " + Task.Run(async () => await Constants.connection.Table<ShowList>().Where(j => j.ShowID == showid).FirstOrDefaultAsync()).Result.Title, "Data in this web tile has passed low limit", settings["UserEmail"].ToString());
                    //        }
                    //    }
                    //}

                    if (settings.Values.ContainsKey("LastCheckedReminderShowID"))
                        settings.Values["LastCheckedReminderShowID"] = Convert.ToInt32(values[0].ToString()) + 1;
                    else
                        settings.Values.Add("LastCheckedReminderShowID", Convert.ToInt32(values[0].ToString()) + 1);
                    //settings.Save();
                }
            }
            catch (Exception ex)
            {
            }

        }

        //public void AddRemOrAlaram(int Id, string Title, string Desc, string type)
        //{
        //    try
        //    {
               
        //        ReminderTable Rt = Task.Run(async () => await Constants.connection.Table<ReminderTable>().Where(i => i.ID == Id).FirstOrDefaultAsync()).Result;
        //        if (Rt.Type.Contains("-"))
        //        {
        //            string name = Id.ToString();
        //            Reminder reminder = new Reminder(type + "Reminder" + name);
        //            reminder.Title = Title;
        //            reminder.Content = Desc;
        //            reminder.BeginTime = System.DateTime.Now.AddMinutes(1);
        //            reminder.ExpirationTime = System.DateTime.Now.AddMinutes(2);
        //            if (ScheduledActionService.Find(type + "Reminder" + name) != null)
        //            {
        //                ScheduledActionService.Remove(type + "Reminder" + name);
        //                ScheduledActionService.Add(reminder);
        //            }
        //            else
        //            {
        //                ScheduledActionService.Add(reminder);
        //            }
        //            Alarm alarm = new Alarm(type + "alarm" + name);
        //            alarm.Content = Desc;
        //            alarm.Sound = new Uri("/RingTones/smokealarm.wav", UriKind.Relative);
        //            alarm.BeginTime = System.DateTime.Now.AddMinutes(2);
        //            alarm.ExpirationTime = System.DateTime.Now.AddMinutes(2);
        //            if (ScheduledActionService.Find(type + "alarm" + name) != null)
        //            {
        //                ScheduledActionService.Remove(type + "alarm" + name);
        //                ScheduledActionService.Add(alarm);
        //            }
        //            else
        //            {
        //                ScheduledActionService.Add(alarm);
        //            }
        //        }
        //        else
        //        {
        //            if (Rt.Type == "Reminder")
        //            {
        //                string name = Id.ToString();
        //                Reminder reminder = new Reminder(type + "Reminder" + name);
        //                reminder.Title = Title;
        //                reminder.Content = Desc;
        //                reminder.BeginTime = System.DateTime.Now.AddMinutes(1);
        //                reminder.ExpirationTime = System.DateTime.Now.AddMinutes(2);
        //                if (ScheduledActionService.Find(type + "Reminder" + name) != null)
        //                {
        //                    ScheduledActionService.Remove(type + "Reminder" + name);
        //                    ScheduledActionService.Add(reminder);
        //                }
        //                else
        //                {
        //                    ScheduledActionService.Add(reminder);
        //                }
        //            }
        //            else
        //            {
        //                string name = Id.ToString();
        //                Alarm alarm = new Alarm(type + "alarm" + name);
        //                alarm.Content = Desc;
        //                alarm.Sound = new Uri("/RingTones/smokealarm.wav", UriKind.Relative);
        //                alarm.BeginTime = System.DateTime.Now.AddMinutes(1);
        //                alarm.ExpirationTime = System.DateTime.Now.AddMinutes(2);
        //                if (ScheduledActionService.Find(type + "alarm" + name) != null)
        //                {
        //                    ScheduledActionService.Remove(type + "alarm" + name);
        //                    ScheduledActionService.Add(alarm);
        //                }
        //                else
        //                {
        //                    ScheduledActionService.Add(alarm);
        //                }
        //            }

        //        }
        //        Constants.connection.DeleteAsync(Rt);
              

        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //}
    }
}
