using System;
using System.Xml;
using Windows.UI.Xaml;

namespace Common.Utilities
{
    public class UtilitiesResources
    {
      
        public static Uri MainPageUri
        {
            get
            {
                return new Uri("/MainPage.xaml", UriKind.Relative);
            }
        }
        public static Uri FeedbackPage
        {
            get
            {
                return new Uri("/About Us/Feedback.xaml", UriKind.Relative);
            }
        }

        public static Uri ContactUsPage
        {
            get
            {
                return new Uri("/About Us/ContactUs.xaml", UriKind.Relative);
            }
        }

        public static Uri UpgradePage
        {
            get
            {
                return new Uri("/About Us/Upgrade.xaml", UriKind.Relative);
            }
        }

        public static string AdControlApplicationID
        {
            get
            {
                return Application.Current.Resources["misappid"].ToString();
            }
        }

        public static string AdControlAdUnitID
        {
            get
            {
                return Application.Current.Resources["misuid"].ToString();
            }
        }
        public static string ApplicationProductID
        {
            get
            {
                return Application.Current.Resources["productid"].ToString();
            }
        }
        public static string StoreUrl
        {
            get
            {
                return Application.Current.Resources["StoreUrl"].ToString();
            }
        }
        public static bool ShowAdControl
        {
            get
            {
                return Application.Current.Resources["AdVisible"] as string != "False";
            }
        }
        public static bool ShowAdRotator
        {
            get
            {
                return Application.Current.Resources["AdRotator"] as string != "False";
            }
        }
        public static string GetAppAttribute(string attributeName)
        {
            string attribute;
            try
            {
                XmlReaderSettings settings2 = new XmlReaderSettings
                {
                    //XmlResolver = new XmlXapResolver()
                };
                XmlReaderSettings settings = settings2;
                using (XmlReader reader = XmlReader.Create("WMAppManifest.xml", settings))
                {
                    reader.ReadToDescendant("App");
                    if (!reader.IsStartElement())
                    {
                        throw new FormatException("WMAppManifest.xml is missing App");
                    }
                    attribute = reader.GetAttribute(attributeName);
                }
            }
            catch (Exception ex)
            {
                attribute = string.Empty;
              
            }
            return attribute;
        }
        public static string ProjectName
        {
            get
            {
                return GetAppAttribute("Title");

            }
        }
        public const string AppMarketplaceWebUrl = "http://www.windowsphone.com/s?appid=";

    }
}
