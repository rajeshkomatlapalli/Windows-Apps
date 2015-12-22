using System;
using System.Net;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
#if NOTANDROID
using Windows.ApplicationModel;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Data.Xml;
using System.Linq;
#endif
namespace Common.Library
{
    public static class DeviceHelp
    {
#if NOTANDROID
        public async static Task<string> GetAppAttribute(string attributeName)
        {
            string attribute=string.Empty;
            try
            {
#if WINDOWS_APP
                StorageFile file =Task.Run(async()=>await  Package.Current.InstalledLocation.CreateFileAsync("AppxManifest.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                 var f = Task.Run(async()=>await file.OpenAsync(FileAccessMode.Read)).Result;
                 IInputStream inputStream = f.GetInputStreamAt(0);
                 DataReader dataReader = new DataReader(inputStream);
                 uint numBytesLoaded = Task.Run(async()=>await dataReader.LoadAsync((uint)f.Size)).Result;
                 string timestamp = (dataReader.ReadString(numBytesLoaded)).ToString();
                 System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(timestamp));
                 var xdoc = XDocument.Load(ms);
                 var xquery = (from i in xdoc.Descendants() select i).ToList();
                 foreach (var s in xquery)
                 {
                     if (s.Name.LocalName==attributeName)
                         attribute = s.Value.ToString();
                 }
#endif
#if WINDOWS_PHONE_APP

                var doc = XDocument.Load("AppxManifest.xml", LoadOptions.None);
                var xname = XNamespace.Get("http://schemas.microsoft.com/appx/2010/manifest");

                string Title = doc.Descendants(xname + "DisplayName").First().Value;
                string Author = doc.Descendants(xname + "PublisherDisplayName").First().Value;

                attribute = Title;
                AppSettings.ProjectName = attribute;
                //XmlReaderSettings settings2 = new XmlReaderSettings
                //{                   
                //   //XmlResolver = new XmlXapResolver()
                //};
                //XmlReaderSettings settings = settings2;
                //using (XmlReader reader = XmlReader.Create("WMAppManifest.xml", settings))
                //{
                //    reader.ReadToDescendant("App");
                //    if (!reader.IsStartElement())
                //    {
                //        throw new FormatException("WMAppManifest.xml is missing App");
                //    }
                //    attribute = reader.GetAttribute(attributeName);
                //    AppSettings.ProjectName = attribute;
                //}
#endif               
            }
            catch (Exception ex)
            {
                attribute = string.Empty;
                //Exceptions.SaveOrSendExceptions("Exception in GetAppAttribute Method In PhoneHelper.cs file", ex);
            }
            return attribute;
        }

        public async static Task<string> GetVersion(string attributeName)
        {
            string attribute = string.Empty;
            try
            {
                StorageFile file = Task.Run(async () => await Package.Current.InstalledLocation.CreateFileAsync("AppxManifest.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var f = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = f.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
                string timestamp = (dataReader.ReadString(numBytesLoaded)).ToString();
                System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(timestamp));
                var xdoc = XDocument.Load(ms);
                var xquery = (from i in xdoc.Descendants() select i).ToList();
                foreach (var s in xquery)
                {
                    if (s.Name.LocalName == "Identity")
                    {
                        attribute = s.Attribute("Version").Value;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                attribute = string.Empty;
                Exceptions.SaveOrSendExceptions("Exception in GetAppAttribute Method In PhoneHelper.cs file", ex);
            }
            return attribute;
        }
#endif
    }
}