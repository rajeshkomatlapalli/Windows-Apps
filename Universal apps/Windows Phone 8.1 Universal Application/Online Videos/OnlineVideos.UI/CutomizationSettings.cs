using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using Common.Library;
using Windows.UI.Xaml;
using Windows.Storage;
using System.Threading.Tasks;

namespace OnlineVideos
{
    public class CustomizationSettings
    {
        public static DependencyObject searched;
        public static int cnt;
        XDocument xdoc;

       public CustomizationSettings()
        {
            searched = null;
            xdoc = null;

         //   var file=  Storage.FileExists(Constants.InstalledCustomizationXmlPath);
           
            StorageFolder isoStore = ApplicationData.Current.LocalFolder;
            //if (file==null)
            //{
                xdoc = XDocument.Load(Constants.InstalledCustomizationXmlPath);
                if (isoStore.GetFolderAsync("DefaultData") == null)
                {
                    xdoc = XDocument.Load(Constants.InstalledCustomizationXmlPath);
                    if (isoStore.GetFolderAsync("DefaultData") == null)
                    
                     Task.Run(async()=>{  
                         await isoStore.CreateFolderAsync("DefaultData");
                    });
                    var fs =ApplicationData.Current.LocalFolder.CreateFileAsync("Movies.xml");
                    //var isoStream =fs.OpenAsync(FileAccessMode.ReadWrite);
                    xdoc.Save(fs as Stream);

                    //await isoStore.CreateFolderAsync("DefaultData");
                    ////IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(Constants.InstalledCustomizationXmlPath, FileMode.Create, isoStore);
                    //var fs = await ApplicationData.Current.LocalFolder.GetFileAsync("Movies.xml");
                    //var isoStream = await fs.OpenAsync(FileAccessMode.ReadWrite);
                    //xdoc.Save(isoStream.AsStream());
                    //isoStream.Close();
                //}
            }                     
        }
  
        public string CheckingCastElement()
        {
            string value = "";
            try
            {
                var xquery = from c in xdoc.Descendants("dtailcast")
                                select c;
                foreach (XElement item in xquery)
                {
                    value = item.Attribute("visible").Value;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in CheckingCastElement Method In Custom.cs file", ex);
            }
            return value;
        }

        public string CheckingDescriptionElement()
        {
            string value = "";
            try
            {
                var xquery = from c in xdoc.Descendants("CheckDetail")
                             orderby (bool)c.Attribute("visible") descending
                             select c;

                foreach (XElement item in xquery)
                {
                    value = item.Attribute("visible").Value;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in CheckingDescriptionElement Method In Custom.cs file", ex);
            }
            return value;
        }
  
        public string GetPivotMessage()
        {
            string message = "";
            try
            {
                var xquery = from c in xdoc.Descendants("pivotmessage")
                                select c;
                foreach (XElement item in xquery)
                {
                    message = (item.Attribute("message").Value);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPivotMessage Method In Custom.cs file", ex);
            }
            return message;
        }

        //Custom App Methods
        public string AdVisible()
        {
            string name = "";
            try
            {
                var xquery = from i in xdoc.Descendants("AdVisible") select i;
                foreach (var item in xquery)
                    name = item.Attribute("visible").Value;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in AdVisible Method In Custom.cs file", ex);
            }
            return name;
        }

        public string Checkaboutupgrade()
        {
            string value = "";
            try
            {
                var xquery = from c in xdoc.Descendants("AboutUsUpgrade")
                                orderby (bool)c.Attribute("visible") descending
                                select c;

                foreach (XElement item in xquery)
                {
                    value = item.Attribute("visible").Value;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Checkaboutupgrade Method In Custom.cs file", ex);
            }
            return value;
        }
        public string Checkupgrademessage()
        {
            string name = "";
            try
            {
                var xquery = from i in xdoc.Descendants("Upgrade")
                                select i;
                foreach (var item in xquery)
                    name = item.Attribute("upgrademessage").Value;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Checkupgrademessage Method In Custom.cs file", ex);
            }
            return name;
        }

        public string Checkupgradelink()
        {
            string link = "";
            try
            {
                var xquery = from i in xdoc.Descendants("Upgrade")
                                select i;
                foreach (var item in xquery)
                    link = item.Attribute("upgradelink").Value;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Checkupgradelink Method In Custom.cs file", ex);
            }
            return link;
        }
  
        public string GetMicrosoftAddsValues(out string MicrosoftUnitAd)
        {
            MicrosoftUnitAd = "";
            string MicrosoftAppAd = "";
            try
            {
                var xquery = from i in xdoc.Descendants("ads")
                                select i;
                foreach (var item in xquery)
                {
                    MicrosoftUnitAd = item.Attribute("misuid").Value;
                    MicrosoftAppAd = item.Attribute("misappid").Value;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetMicrosoftAddsValues Method In Custom.cs file", ex);
            }
            return MicrosoftAppAd;
        }

        public string GetFromEmailId()
        {
            string Id = "";
            try
            {
                var xquery = from i in xdoc.Descendants("mailids")
                                select i;
                foreach (var item in xquery)
                    Id = item.Attribute("from").Value;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetFromEmailId Method In Custom.cs file", ex);
            }
            return Id;
        }

        public string GetToEmailId()
        {
            string Id = "";
            try
            {
                var xquery = from i in xdoc.Descendants("mailids")
                                select i;
                foreach (var item in xquery)
                    Id = item.Attribute("to").Value;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetToEmailId Method In Custom.cs file", ex);
            }
            return Id;
        }
   
        public string CheckDoenloadTheme()
        {
            string value = "";
            try
            {
                var xquery = from c in xdoc.Descendants("downloadtheme")
                                orderby (bool)c.Attribute("visible") descending
                                select c;

                foreach (XElement item in xquery)
                {

                    value = item.Attribute("visible").Value;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in CheckDoenloadTheme Method In Custom.cs file", ex);
            }
            return value;
        }

        public string Checkcanvasmargin()
        {
            string value = "";
            try
            {
                var xquery = from c in xdoc.Descendants("canvasmargin")
                                select c;

                foreach (XElement item in xquery)
                {

                    value = item.Attribute("value").Value;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Checkcanvasmargin Method In Custom.cs file", ex);
            }
            return value;
        }
        
        public string Checktextblockmargin1()
        {
            string value = "";
            try
            {
                var xquery = from c in xdoc.Descendants("textblockmargin1")
                                select c;

                foreach (XElement item in xquery)
                {

                    value = item.Attribute("value").Value;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Checktextblockmargin1 Method In Custom.cs file", ex);
            }
            return value;
        }

        public string Checktextblockmargin()
        {
            string value = "";
            try
            {
                var xquery = from c in xdoc.Descendants("textblockmargin")
                                select c;

                foreach (XElement item in xquery)
                {

                    value = item.Attribute("value").Value;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Checktextblockmargin Method In Custom.cs file", ex);
            }
            return value;
        }

        public string Checkimagemargin()
        {
            string value = "";
            try
            {
                var xquery = from c in xdoc.Descendants("imagemargin")
                                select c;

                foreach (XElement item in xquery)
                {

                    value = item.Attribute("value").Value;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Checkimagemargin Method In Custom.cs file", ex);
            }
            return value;
        }

        public string Checktextcolor()
        {
            string value = "";
            try
            {
                var xquery = from c in xdoc.Descendants("textcolor")
                                select c;

                foreach (XElement item in xquery)
                {

                    value = item.Attribute("value").Value;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Checktextcolor Method In Custom.cs file", ex);
            }
            return value;
        }

        public string PeopleExists()
        {
            string name = "";
            try
            {
                var xquery = from i in xdoc.Descendants("People") select i;
                foreach (var item in xquery)
                    name = item.Attribute("Exists").Value;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PeopleExists Method In Custom.cs file", ex);
            }
            return name;
        }

        public string Upgrade()
        {
            string name = "";
            try
            {
                var xquery = from i in xdoc.Descendants("Upgrade") select i;
                foreach (var item in xquery)
                    name = item.Attribute("Exists").Value;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Upgrade Method In Custom.cs file", ex);
            }
            return name;
        }
    }
}
