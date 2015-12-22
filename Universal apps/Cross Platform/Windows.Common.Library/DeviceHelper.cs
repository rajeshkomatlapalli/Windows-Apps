
using System;
using System.Net;
using System.Windows;
# if WP8 && NOTANDROID
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Info;
#endif
#if WINDOWS_PHONE_APP
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;
using Windows.Security.ExchangeActiveSyncProvisioning;
#endif
namespace Common.Library
{
    public static class DeviceHelper
    {
		#if WP8 && NOTANDROID || WINDOWS_PHONE_APP
        private static readonly int ANIDLength = 32;
        private static readonly int ANIDOffset = 2;
        public static string GetManufacturer()
        {
            string result = string.Empty;
            //object manufacturer;
            EasClientDeviceInformation device = new EasClientDeviceInformation();
            result = device.SystemManufacturer;
            //if (DeviceExtendedProperties.TryGetValue("DeviceManufacturer", out manufacturer))
            //    result = manufacturer.ToString();
            return result;
        }
        public static string GetDeviceModel()
        {
            string model = null;
            object theModel = null;
            EasClientDeviceInformation device = new EasClientDeviceInformation();
            model= device.SystemProductName;
            //if (Microsoft.Phone.Info.DeviceExtendedProperties.TryGetValue("DeviceName", out theModel))
            //    model = theModel as string;           
            theModel = device.SystemProductName;
            return model;
        }  
        //Note: to get a result requires ID_CAP_IDENTITY_DEVICE  
        // to be added to the capabilities of the WMAppManifest  
        // this will then warn users in marketplace  
        public static byte[] GetDeviceUniqueID()
        {
            byte[] bytes=null;
            try
            {
                var token = Windows.System.Profile.HardwareIdentification.GetPackageSpecificToken(null);
                var hardwareId = token.Id;
                var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(hardwareId);

                bytes = new byte[hardwareId.Length];
                dataReader.ReadBytes(bytes);

                string nn = BitConverter.ToString(bytes).Replace("-", "");
                return bytes;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return bytes;
            }
            //byte[] result = null;
            //object uniqueId;
            //if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
            //    result = (byte[])uniqueId;

            //return result;



            //byte[] result = null;
            ////object uniqueId;
            //EasClientDeviceInformation device = new EasClientDeviceInformation();
            ////Guid gd = new Guid(result);        
            //result = device.Id.ToByteArray();
            ////if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
            ////    result = (byte[])uniqueId;

            //return result;
        }

        // NOTE: to get a result requires ID_CAP_IDENTITY_USER  
        //  to be added to the capabilities of the WMAppManifest  
        // this will then warn users in marketplace  
        
        //public static string GetWindowsLiveAnonymousID()
        //{
        //    string result = string.Empty;
        //    object anid;
        //    if (UserExtendedProperties.TryGetValue("ANID", out anid))
        //    {
        //        if (anid != null && anid.ToString().Length >= (ANIDLength + ANIDOffset))
        //        {
        //            result = anid.ToString().Substring(ANIDOffset, ANIDLength);
        //        }
        //    }

        //    return result;
        //}
#endif
    }
}
