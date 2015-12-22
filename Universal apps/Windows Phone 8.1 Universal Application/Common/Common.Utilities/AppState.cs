using Windows.UI.Xaml;

namespace Common.Library
{
    public static class AppState
    {
        public static string ApplicationName
        {
            get
            {
                return Application.Current.Resources["ApplicationName"].ToString();
            }
            set
            {
                Application.Current.Resources["ApplicationName"] = value;                
            }
        }

        public static string ShowID
        {
            get
            {
                return Application.Current.Resources["ShowID"].ToString();
            }
            set
            {
                Application.Current.Resources["ShowID"] = value;
            }
        }
        
        public static string ImagePath
        {
            get
            {
                return Application.Current.Resources["ImagePath"].ToString();
            }
            set
            {
                Application.Current.Resources["ImagePath"] = value;
            }
        }
        public static string RingtoneStatus
        {
            get
            {
                return Application.Current.Resources["RingtoneStatus"].ToString();
            }
            set
            {
                Application.Current.Resources["RingtoneStatus"] = value;
            }
        }
      
        public static string searchtitle
        {
            get
            {
                return Application.Current.Resources["searchtitle"].ToString();
            }
            set
            {
                Application.Current.Resources["searchtitle"] = value;
            }
        }
    }
}
