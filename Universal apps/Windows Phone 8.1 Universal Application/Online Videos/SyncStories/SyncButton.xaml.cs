using Common.Library;
using Microsoft.Live;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
        public static LiveConnectSession session = default(LiveConnectSession);
        public SyncButton()
        {
            this.InitializeComponent();
        }

        public static async Task Login()
        {
            bool connected1 = false;
            try
            {
                var authclient1 = new LiveAuthClient();
                LiveLoginResult res = await authclient1.LoginAsync(new string[] { "wl.signin", "wl.skydrive", "wl.skydrive_update", "wl.photos" });

                if (res.Status == LiveConnectSessionStatus.Connected)
                {
                    connected1 = true;
                    var connectClient1 = new LiveConnectClient(res.Session);
                    string AccessTkn = connectClient1.Session.AccessToken;
                    session = connectClient1.Session;
                    AppSettings.MicrosoftAccessToken = AccessTkn;
                    AppSettings.SkyDriveLogin = true;
                    var meresult = await connectClient1.GetAsync("me");
                    dynamic meData = meresult.Result;
                }
            }
            catch (LiveAuthException ex) 
            {
                //MessageBox.Show(ex.Message);
                MessageDialog msgbox = new MessageDialog("An error occurred.");
                //await msgbox.ShowAsync();
            }
            catch (LiveConnectException ex) 
            { 
                //MessageBox.Show(ex.Message); 
                MessageDialog msgbox = new MessageDialog("An error occurred.");
                //await msgbox.ShowAsync();
            }

            //btnSignin.Visibility = connected1 ? Visibility.Collapsed : Visibility.Visible;            
        }
    }
}
