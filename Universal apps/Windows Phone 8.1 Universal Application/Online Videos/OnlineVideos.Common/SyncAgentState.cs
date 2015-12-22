using System;
using System.Net;
using System.Windows;
#if WP8
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Scheduler; 
#endif
using System.Globalization;
using System.Threading;
using Common.Library;



namespace OnlineVideos.Common
{
    public class SyncAgentState
    {
        public static CultureInfo culture;
        public static object SyncAgent = null;
        public static DateTime NextUpdatedDate;
        public static string MovieId;
        public static DateTime PeopleNextUpdatedDate;
        public static string PeopleId;
       
        public static int HistoryCount;
        public static AutoResetEvent auto=new AutoResetEvent(false);
        public static string[] mids;
        public static SyncAgentStatus DownloadStatus;

        public static void ResetEvent()
        {
            if (SyncAgentState.SyncAgent != null)
            {
                SyncAgentState.auto.Set();
            }
        }

        public static void WaitEvent()
        {
            if (SyncAgentState.SyncAgent != null)
            {
                SyncAgentState.auto.WaitOne();
            }
        }
    }
}
