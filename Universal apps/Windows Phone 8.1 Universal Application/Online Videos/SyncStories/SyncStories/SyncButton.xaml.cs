using Common.Library;
using Microsoft.Live;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SyncStories
{
    public sealed partial class SyncButton : UserControl
    {
        public static LiveConnectClient client;
        public static LiveAuthClient authClient;
        public static LiveConnectSession session;
        public SyncButton()
        {

            this.InitializeComponent();
            SetNameField(false);
        }

        private async void SignInClick(object sender, RoutedEventArgs e)
        {
            await SetNameField(true);
        }

        private void AccountBackClicked(object sender, RoutedEventArgs e)
        {
            Popup parent = this.Parent as Popup;
            if (parent != null)
            {
                parent.IsOpen = false;
            }


            if (Windows.UI.ViewManagement.ApplicationView.Value != Windows.UI.ViewManagement.ApplicationViewState.Snapped)
            {
                SettingsPane.Show();
            }
        }
        public async Task SetNameField(Boolean login)
        {

            await updateUserName(this.userName, login);

            Boolean userCanSignOut = true;

            LiveAuthClient LCAuth = new LiveAuthClient();
            LiveLoginResult LCLoginResult = await LCAuth.InitializeAsync();

            if (LCLoginResult.Status == LiveConnectSessionStatus.Connected)
            {
                session = LCLoginResult.Session;
                AppSettings.SkyDriveLogin = true;
                userCanSignOut = LCAuth.CanLogout;
            }

            if (this.userName.Text.Equals("You're not signed in."))
            {

                signInBtn.Visibility = Windows.UI.Xaml.Visibility.Visible;
                msgtxt.Visibility = Visibility.Visible;
                signOutBtn.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else
            {

                signOutBtn.Visibility = (userCanSignOut ? Windows.UI.Xaml.Visibility.Visible : Windows.UI.Xaml.Visibility.Collapsed);
                signInBtn.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                msgtxt.Visibility = Visibility.Collapsed ;
            }
        }
        public async static void login()
        {
            string[] scopes = new string[2];

            authClient = new LiveAuthClient();
            LiveLoginResult LiveLog = await authClient.InitializeAsync();
            if (LiveLog.Status == LiveConnectSessionStatus.Connected)
            {
                session = LiveLog.Session;
                AppSettings.SkyDriveLogin = true;
            }
            else
            {
                AppSettings.SkyDriveLogin = false;
            }

        }


        public static async Task updateUserName(TextBlock userName, Boolean signIn)
        {
            try
            {
                LiveAuthClient LCAuth = new LiveAuthClient();
                LiveLoginResult LCLoginResult = Task.Run(async()=>await LCAuth.InitializeAsync(new string[] { "wl.basic", "wl.offline_access", "wl.signin" })).Result;
                try
                {
                    LiveLoginResult loginResult = null; 
                    if (signIn)
                    {

                        loginResult = Task.Run(async () => await LCAuth.LoginAsync(new string[] { "wl.basic wl.offline_access wl.skydrive_update" })).Result;
                    }
                    else
                    {

                        loginResult = LCLoginResult;
                    }
                    if (loginResult.Status == LiveConnectSessionStatus.Connected)
                    {

                        LiveConnectClient connect = new LiveConnectClient(LCAuth.Session);
                        session = LCAuth.Session;
                        AppSettings.SkyDriveLogin = true;
                        LiveOperationResult operationResult = Task.Run(async () => await connect.GetAsync("me")).Result;
                        dynamic result = operationResult.Result;
                        if (result != null)
                        {

                            userName.Text = string.Join(" ", "Hello", result.name, "!");
                        }
                        else
                        {

                        }
                    }
                    else
                    {

                        userName.Text = "You're not signed in.";
                    }
                }
                catch (LiveAuthException ex)
                {
                    Exceptions.SaveOrSendExceptions("Exception in updateUserName Method In SyncButton.xaml file", ex);
                }
            }
            catch (LiveAuthException ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in updateUserName Method In SyncButton.xaml file", ex);
            }
            catch (LiveConnectException ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in updateUserName Method In SyncButton.xaml file", ex);
            }
        }

        private async void SignOutClick(object sender, RoutedEventArgs e)
        {
            try
            {

                LiveAuthClient LCAuth = new LiveAuthClient();
                LiveLoginResult LCLoginResult = await LCAuth.InitializeAsync();

                if (LCLoginResult.Status == LiveConnectSessionStatus.Connected)
                {
                    AppSettings.SkyDriveLogin = false;
                    LCAuth.Logout();
                   
                }
                msgtxt.Visibility = Visibility.Visible;
                this.userName.Text = "You're not signed in.";
                signInBtn.Visibility = Windows.UI.Xaml.Visibility.Visible;
                signOutBtn.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            catch (LiveConnectException ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SignOutClick Method In SyncButton.xaml file", ex);
            }
        }
    }
}
