using Common.Library;
using Common.Utilities;
using OnlineVideos.UI;
using System;
using Windows.Devices.Geolocation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {

        #region GlobalDeclaration
        string myid = string.Empty;
        CustomizationSettings objCustom = new CustomizationSettings();
        private SolidColorBrush adcontrolborder = new SolidColorBrush(Colors.Transparent);
        private static PageHelper oldstate;
        int PopupSetting, PopupSettings;
        CoreDispatcher dispatcher;
        #endregion

        #region Constructor
        public Settings()
        {
            this.InitializeComponent();
            LoadSettings();
            PopupSetting = 0;
            PopupSettings = 0;
            if (ResourceHelper.AppName != Apps.Indian_Cinema_Pro.ToString() && ResourceHelper.AppName != Apps.Kids_TV_Pro.ToString() && ResourceHelper.AppName != Apps.Story_Time_Pro.ToString())
                LoadAds();
            //Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {            
            if (AppSettings.popupcount == true)
            {
                if (!SettingsHelper.Contains("Password"))
                {
                    toggleswitch.OnContent = "Off";                    
                    toggleswitch.IsOn = false;
                    AppSettings.ParentalControl = false;
                }
            }        
        }

        #endregion

        #region "Common Methods"

        private void LoadAds()
        {
            try
            {
                LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In Settings file", ex);
                string excepmess = "Exception in LoadAds Method In SongDetails file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }

        private void LoadDownLoadTheme()
        {
            if (AppSettings.AddNewShowIconVisibility)
            {
                toggleswitchRunUnderLoock.Visibility = Visibility.Visible;
            }
        }
       
        #endregion       
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
               // FlurryWP8SDK.Api.LogPageView();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In settings file", ex);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
               // FlurryWP8SDK.Api.EndTimedEvent("Settings Page");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom Method In settings.cs file", ex);
            }
        }

        private void imgTitle_KeyDown(object sender, KeyRoutedEventArgs e)
        {           
            Frame.Navigate(typeof(MainPage));
        }

        private async void GpsLocationToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if(GpsLocationToggleSwitch !=null)
            {
                if(GpsLocationToggleSwitch.IsOn==true)
                {
                    try
                    {
                        bool enabled = true;
                        Geolocator locator = new Geolocator();
                        //locator.PositionChanged += new TypedEventHandler<Geolocator, PositionChangedEventArgs>(OnPositionChanged);
                        locator.MovementThreshold = 100;
                        locator.PositionChanged += locator_PositionChanged;
                        if (enabled)
                        {
                            GpsLocationToggleSwitch.OnContent = "On";
                            SettingsHelper.Save("GeoLocationStatus", "true");
                        }
                        else
                        {                            
                            MessageDialog result = new MessageDialog("An error occurred.");
                            await result.ShowAsync();                            
                            GpsLocationToggleSwitch.IsOn = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        string mess = "Exception in GpsLocationToggleSwitch_Checked Method In settings file.\n\n" + ex.Message + "\n\n Stack Trace:- " + ex.StackTrace;
                        ex.Data.Add("Date", DateTime.Now);
                        Exceptions.SaveOrSendExceptions(mess, ex);
                    }
                }
                else
                {
                    try
                    {
                        GpsLocationToggleSwitch.OnContent = "Off";
                        SettingsHelper.Save("GeoLocationStatus", "false");
                        toggleswitchRunUnderLoock.OnContent = "Off";
                        SettingsHelper.Save("runUnderLock", "false");
                        toggleswitchRunUnderLoock.IsOn = false;
                    }
                    catch (Exception ex)
                    {
                        string mess = "Exception in GpsLocationToggleSwitch_Unchecked Method In settings file.\n\n" + ex.Message + "\n\n Stack Trace:- " + ex.StackTrace;
                        ex.Data.Add("Date", DateTime.Now);
                        Exceptions.SaveOrSendExceptions(mess, ex);
                    }
                }
            }
        }
        Geolocator geolocator = new Geolocator();
        async void locator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            geolocator.DesiredAccuracyInMeters = 50;
            Geoposition geoposition = await geolocator.GetGeopositionAsync(
            maximumAge: TimeSpan.FromMinutes(5),
            timeout: TimeSpan.FromSeconds(10)
            );

            string latitude = geoposition.Coordinate.Latitude.ToString("0.0000000000");
            string Longitude = geoposition.Coordinate.Longitude.ToString("0.0000000000");
            string Accuracy = geoposition.Coordinate.Accuracy.ToString("0.0000000000");
        }        
               
        private async void toggleswitchRunUnderLoock_Toggled(object sender, RoutedEventArgs e)
        {
            if(toggleswitchRunUnderLoock!=null)
            {
                if(toggleswitchRunUnderLoock.IsOn==true)
                {
                    bool IsLocationTrackingEnable = SettingsHelper.getBoolValue("GeoLocationStatus");

                    if (IsLocationTrackingEnable)
                    {
                        if (!ApplicationIdleModeHelper.Current.RunsUnderLock)
                        {                            
                            MessageDialog result = new MessageDialog("Running under lockscreen can consume battery even when screen is locked and you are not using the application. Click OK to enable", "");
                            result.Commands.Add(new UICommand("Ok", new UICommandInvokedHandler(this.TriggerThisFunctionForOK)));
                            result.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler(this.TriggerThisFunctionForCancel)));
                            await result.ShowAsync();                                      
                        }
                        else
                        {
                            toggleswitchRunUnderLoock.OnContent = "On";
                            SettingsHelper.Save("runUnderLock", "true");
                        }
                    }
                    else
                    {                        
                        MessageDialog location = new MessageDialog("Location tracking must be enabled");
                        await location.ShowAsync();
                        toggleswitchRunUnderLoock.IsOn = false;
                    }
                }
                else
                {
                    toggleswitchRunUnderLoock.OnContent = "Off";
                    SettingsHelper.Save("runUnderLock", "false");
                    ApplicationIdleModeHelper.Current.RunsUnderLock = false; 
                }
            }
        }

        private void TriggerThisFunctionForCancel(IUICommand command)
        {
            ApplicationIdleModeHelper.Current.RunsUnderLock = false;
            toggleswitchRunUnderLoock.IsOn = false;
            SettingsHelper.Save("runUnderLock", "false");
        }

        private void TriggerThisFunctionForOK(IUICommand command)
        {
            ApplicationIdleModeHelper.Current.HasUserAgreedToRunUnderLock = true;
            ApplicationIdleModeHelper.Current.RunsUnderLock = true;
            toggleswitchRunUnderLoock.OnContent = "On";
            SettingsHelper.Save("runUnderLock", "true");
        }
        private void toggleswitchUpdateMovies_Toggled(object sender, RoutedEventArgs e)
        {

            if (toggleswitch.IsOn==true)
            {
                try
                {
                    AppSettings.AutomaticallyDownloadShows = true;
                    toggleswitchUpdateMovies.OnContent = "Data Plan";
                    toggleswitchUpdateMovies.Foreground = new SolidColorBrush(Colors.White);
                }
                catch (Exception ex)
                {
                    Exceptions.SaveOrSendExceptions("Exception in toggleswitchUpdateMovies_Checked Method In Settings file", ex);
                }
            }
            else
            {
                try
                {
                    AppSettings.AutomaticallyDownloadShows = false;                 
                    toggleswitchUpdateMovies.OnContent = "Wi-Fi";
                    toggleswitchUpdateMovies.Foreground = new SolidColorBrush(Colors.White);
                }
                catch (Exception ex)
                {
                    Exceptions.SaveOrSendExceptions("Exception in toggleswitchUpdateMovies_Unchecked Method In Settings file.", ex);
                }
            }
        }

        private void toggleswitch_Toggled(object sender, RoutedEventArgs e)
        {
            if(toggleswitch != null)
            {
                if(toggleswitch.IsOn==true)
                {
                    if (PopupSettings != 1)
                    {
                        toggleswitch.OnContent = "On";                        
                        PageHelper.RemoveEntryFromBackStack("Settings");
                        PopupSettings = 0;
                    }
                    else
                    {
                        toggleswitch.OnContent = "On";
                        PopupSettings = 0;
                    }
                }
                else
                {
                    if (PopupSetting != 1)
                    {
                        if (SettingsHelper.Contains("Password"))
                        {                            
                            PageHelper.RemoveEntryFromBackStack("Settings");
                            toggleswitch.OnContent = "Off";
                            PopupSetting = 0;
                        }
                        else
                        {
                            toggleswitch.OnContent = "Off";
                            PopupSetting = 0;
                        }
                    }
                    else
                    {
                        toggleswitch.OnContent = "Off";
                        PopupSetting = 0;
                    }
                }
            }
        }

        private void btnparentalcontrol_Click(object sender, RoutedEventArgs e)
        {
            if (AppSettings.ParentalControl)
            {               
                PageHelper.RemoveEntryFromBackStack("Settings");
            }
            else            
            Frame.Navigate(typeof(NavigationHelper), NavigationHelper.ParentalControlShowListPage);            
        }

        private void btnabout_Click(object sender, RoutedEventArgs e)
        {            
            Frame.Navigate(typeof(AboutMemory));
        }

        #region PageLoad
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //FlurryWP8SDK.Api.LogEvent("Settings Page", true);
            LoadAds();
            if (ResourceHelper.ProjectName == "VideoMix")
            {
                btnparentalcontrol.Visibility = Visibility.Collapsed;
                toggleswitch.Visibility = Visibility.Collapsed;
            }            
            try
            {                
                var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (SettingsHelper.Contains("UpdateMovies"))
                   toggleswitchUpdateMovies.IsOn = AppSettings.AutomaticallyDownloadShows;                                    
                if (settings.Values.ContainsKey("GeoLocationStatus"))                
                    GpsLocationToggleSwitch.IsOn = bool.Parse(settings.Values["GeoLocationStatus"].ToString());                
                if (settings.Values.ContainsKey("runUnderLock"))                    
                    toggleswitchRunUnderLoock.IsOn = bool.Parse(settings.Values["runUnderLock"].ToString());
                if (SettingsHelper.Contains("Parental Control"))
                {
                    if (AppSettings.ParentalControl)
                        PopupSetting = 1;
                    else
                        PopupSetting = 0;
                }

                PageHelper.RemoveEntryFromBackStack("Settings");
                LoadDownLoadTheme();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In settings file", ex);
            }
        }

        private void LoadSettings()
        {
            //if (!AppResources.ShowParentalControl)
            //{
                btnparentalcontrol.Visibility = Visibility.Collapsed;
                toggleswitch.Visibility = Visibility.Collapsed;
            //}
        }
        #endregion
    }
}
