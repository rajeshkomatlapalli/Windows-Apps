using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Common.Library;

namespace OnlineVideos.ViewModels
{
    public  class TimerHelper
    {
        TimeSpan t;
        TimeSpan ot = new TimeSpan();
        public  string manipulatetime()
        {
          
            t = t.Subtract(TimeSpan.FromSeconds(1));
            string TimeUsed = (ot - t).ToString();
            SettingsHelper.Save("TimeUsed", TimeUsed);
            return t.ToString();
        }
        public  void time(string[] p)
        {
          
           
            string r = string.Empty;
            t = new TimeSpan(Convert.ToInt32(p[0]), Convert.ToInt32(p[1]), Convert.ToInt32(p[2]));
            ot = t;
            string TotalTime = ((ot.Hours.ToString() == "0") ? "" : ((Convert.ToInt32(ot.Hours) > 1) ? ot.Hours.ToString() + " hours" : ot.Hours.ToString() + " hour")) + ((ot.Minutes.ToString() == "0") ? "" : ((Convert.ToInt32(ot.Minutes) > 1) ? ot.Minutes.ToString() + " minutes" : ot.Minutes.ToString() + " minute")) + ((ot.Seconds.ToString() == "0") ? "" : ((Convert.ToInt32(ot.Seconds) > 1) ? ot.Seconds.ToString() + " seconds" : ot.Seconds.ToString() + " second"));
            SettingsHelper.Save("TotalTime", TotalTime);
        }
    }
}
