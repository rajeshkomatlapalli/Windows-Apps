using Common.Library;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.System;
using Windows.Networking.Connectivity;
using Windows.UI;
using OnlineVideos.Views;
using Windows.Phone.UI.Input;
using Windows.ApplicationModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineVideos
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutMemory : Page
    {
        #region GlobalDeclaration
        private SolidColorBrush adcontrolborder = new SolidColorBrush(Colors.Transparent);
        #endregion

        #region Constructor
        public AboutMemory()
        {
            this.InitializeComponent();
            Loaded += AboutMemory_Loaded;
        }

        #endregion
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            try
            {
              
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In AboutMemory.cs file.", ex);
            }
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if(Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }
        private void LoadAds()
        {
            try
            {
                LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In AboutMemory file", ex);
                string excepmess = "Exception in LoadAds Method In SongDetails file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {               
            }
            catch(Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom Method in AboutMemory.xaml.cs.file", ex);
            }
        }


        void AboutMemory_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionProfileInfo = string.Empty;
                LoadAds();
                PackageVersion pv = Package.Current.Id.Version;
                Version version = new Version(Package.Current.Id.Version.Major,
                    Package.Current.Id.Version.Minor,
                    Package.Current.Id.Version.Revision,
                    Package.Current.Id.Version.Build);

                AppVersion.Text =version.ToString();
                txtdevicetotalmemory.Text = (MemoryManager.AppMemoryUsage).ToString();
                txtmemoryUsage.Text = (MemoryManager.AppMemoryUsage / 1048576).ToString();
                ConnectionProfile InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (InternetConnectionProfile == null)
                {
                    txtNetworktype.Text = "Not connected to Internet";
                }
                else
                {
                    connectionProfileInfo = GetConnectionProfile(InternetConnectionProfile);
                    txtNetworktype.Text = connectionProfileInfo;
                }

                string currentconnection = MemoryManager.AppMemoryUsageLevel.ToString();
                var profiles = NetworkInformation.GetConnectionProfiles();
                var hostNamesList = Windows.Networking.Connectivity.NetworkInformation.GetHostNames();
                foreach (var entry in hostNamesList)
                {
                    if (entry.Type == Windows.Networking.HostNameType.DomainName)
                    {
                        txtpeakmemoryUsage.Text = entry.CanonicalName;
                    }
                }
                                      
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in AboutMemory_Loaded Method In AboutMemory.cs file.", ex);
            }
        }

        string GetConnectionProfile(ConnectionProfile connectionProfile)
        {
            string connectionProfileInfo = string.Empty;
            if (connectionProfile != null)
            {               
                switch (connectionProfile.GetNetworkConnectivityLevel())
                {
                    case NetworkConnectivityLevel.None:
                        connectionProfileInfo += "  None";
                        break;
                    case NetworkConnectivityLevel.LocalAccess:
                        connectionProfileInfo += "  Local Access";
                        break;
                    case NetworkConnectivityLevel.ConstrainedInternetAccess:
                        connectionProfileInfo += "  Constrained Internet Access";
                        break;
                    case NetworkConnectivityLevel.InternetAccess:
                        connectionProfileInfo += "  Internet Access";
                        break;
                }
            }
            return connectionProfileInfo;
        }        
        
        #region Events
        private void btnagentlog_Click(object sender, RoutedEventArgs e)
        {            
            this.Frame.Navigate(typeof(BackAgentError));
        }

        private void imgTitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        #endregion        
    }
}
