using Common.Library;
using Common.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Email;
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

namespace CommonControls
{
    public sealed partial class AboutUs : UserControl
    {
        public AboutUs()
        {
            this.InitializeComponent();
            List<AboutUsProperties> menuItems = MenuHelper.GetAboutUsMenuItems();
            if (!AppResources.ShowUpgradePage)
            {
                menuItems.RemoveAt(1);
            }
            lbxAboutUs.ItemsSource = menuItems;
        }

        private void lbxAboutUs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxAboutUs.SelectedIndex == -1)
                return;

            string SelIndex = (lbxAboutUs.SelectedItem as AboutUsProperties).Id;
            switch (SelIndex)
            {
                case "1":
                    //rate this app
                    UtilitiesManager.ShowMarketplaceAppReviewTask();
                    break;
                case "2":
                    //upgrade page
                    PageHelper.NavigateTo(UtilitiesResources.UpgradePage);
                    break;
                case "3":
                    //related apps
                    UtilitiesManager.LaunchMarketplaceSearchTask();
                    break;
                case "4":
                    //share with friends
                    //UtilitiesManager.ShareWithFriendsComposeEmailTask();
                    send();
                    break;
                case "5":
                    //Feedback
                    //PageHelper.NavigateTo(UtilitiesResources.ContactUsPage);
                    PageHelper.RootApplicationFrame.Navigate(typeof(ContactUs));
                    break;
            }
            lbxAboutUs.SelectedIndex = -1;
        }

        public async void send()
        {
            //share this app
            EmailMessage mail = new EmailMessage();
            string lnk = "I have recently found a very good entertainment app that I would like to share with you, Get the app at ";
            lnk += UtilitiesResources.AppMarketplaceWebUrl + UtilitiesResources.ApplicationProductID;
            mail.Body = lnk;
            mail.Subject = UtilitiesResources.ProjectName + " App";
            await EmailManager.ShowComposeNewEmailAsync(mail);
        }
    }
}
