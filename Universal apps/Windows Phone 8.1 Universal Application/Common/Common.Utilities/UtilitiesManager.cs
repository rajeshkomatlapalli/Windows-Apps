using System;
using Common.Library;
using OnlineVideos.UI;
using Windows.UI.Xaml;
using Windows.ApplicationModel.Store;

namespace Common.Utilities
{
    public class UtilitiesManager
    {
        public async static void LoadBrowserTaskToSocialNetwork(string url)
        {
            try
            {              
                // The URI to launch
                string uriToLaunch = url;

                // Create a Uri object from a URI string 
                var uri = new Uri(uriToLaunch);
                
                // Launch the URI
                var success = await Windows.System.Launcher.LaunchUriAsync(uri);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadBrowserTsk Method In AboutUs.Xaml.cs file.", ex);
            }
        }

        public async static void ShowMarketplaceAppReviewTask()
        {
            try
            {
                //rate this app                
                await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowMarketplaceAppReviewTask Method In PhoneHelper.cs file.", ex);
            }
        }
        public async static void LoadBrowserTask(string url)
        {
            try
            {
                await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                      {
                          PageHelper.NavigateTo(NavigationHelper.youtubeBrowserPage(url));
                      });
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadBrowserTsk Method In AboutUs.Xaml.cs file.", ex);
            }
        }
        public async static void ShareWithFriendsComposeEmailTask()
        {
            try
            {
                //share this app                
                //EmailMessage mail = new EmailMessage();
                //string lnk = "I have recently found a very good entertainment app that I would like to share with you, Get the app at ";
                //lnk += UtilitiesResources.AppMarketplaceWebUrl + UtilitiesResources.ApplicationProductID;
                //mail.Body = lnk;
                //mail.Subject = UtilitiesResources.ProjectName + " App";              
                //await EmailManager.ShowComposeNewEmailAsync(mail);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShareWithFriendsComposeEmailTask Method In PhoneHelper.cs file.", ex);
            }
        }

        public async static void LaunchMarketplaceSearchTask()
        {
            try
            {
                //related apps
                await Windows.System.Launcher.LaunchUriAsync(new Uri(string.Format("ms-windows-store:search?{0}={1}", "LART SOFT", "LART SOFT")));
            }
            catch (Exception ex)
            {
               Exceptions.SaveOrSendExceptions("Exception in LaunchMarketplaceSearchTask Method In PhoneHelper.cs file.", ex);
            }
        }
    }
}