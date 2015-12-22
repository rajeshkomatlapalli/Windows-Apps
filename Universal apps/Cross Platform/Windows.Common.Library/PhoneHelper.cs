using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
#if WINDOWS_PHONE_APP
#endif
using System.Xml;
using Common.Library;

namespace Common.Library
{
    public class PhoneHelper
    {
        private const string AppManifestName = "WMAppManifest.xml";
        private const string AppNodeName = "App";

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
                Exceptions.SaveOrSendExceptions("Exception in GetAppAttribute Method In PhoneHelper.cs file", ex);
            }
            return attribute;
        }

      

    }
}
