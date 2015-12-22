
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows;
using Windows.Networking;
using Windows.Networking.Connectivity;
#if ANDROID
using Android.Net;
using Android.App;
using Android.Content;
#endif

#if WP8 && NOTANDROID
using Microsoft.Phone.Net.NetworkInformation;
#endif
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;

//using Common.Common;



namespace Common.Library
{
    public static class NetworkHelper
    {
        
        public static bool IsNetworkAvailable()
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                return true;

            return false;
        }

        public static bool IsNetworkAvailableForDownloads()
        {

#if WINDOWS_PHONE_APP && NOTANDROID

            ConnectionProfile InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();

          //if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() && (AppSettings.AutomaticallyDownloadShows || NetworkInterface.NetworkInterfaceType== NetworkInterfaceType.Ethernet || (!AppSettings.AutomaticallyDownloadShows && NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)))
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() && (AppSettings.AutomaticallyDownloadShows || InternetConnectionProfile!=null))
                return true;

			return false;
#endif
            
#if ANDROID
			ConnectivityManager connMgr = (ConnectivityManager)
				Application.Context.GetSystemService (
					Context.ConnectivityService);
			if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() && (AppSettings.AutomaticallyDownloadShows ||connMgr.ActiveNetworkInfo.Type==ConnectivityType.Mobile || (!AppSettings.AutomaticallyDownloadShows && connMgr.ActiveNetworkInfo.Type ==ConnectivityType.Wifi)))
				return true;

			return false;
#endif
#if WINDOWS_APP
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                return true;

            return false;
#endif

        }
    }
}
