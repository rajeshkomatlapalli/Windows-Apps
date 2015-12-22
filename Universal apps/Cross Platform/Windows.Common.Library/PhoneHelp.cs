using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
#if WP8 && NOTANDROID
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
#endif
using System.Xml;

namespace Common.Library
{
    public static class PhoneHelp
    {
#if WINDOWS_PHONE_APP && NOTANDROID
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
                using (XmlReader reader = XmlReader.Create("Package.appxmanifest", settings))
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
               // Exceptions.SaveOrSendExceptions("Exception in GetAppAttribute Method In PhoneHelper.cs file", ex);
            }
            return attribute;
        }
#endif
    }
}
