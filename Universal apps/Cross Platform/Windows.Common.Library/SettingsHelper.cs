#if WINDOWS_PHONE
#define WINDOWS_PHONE_APP
#endif
#if WINRT
#define WINDOWS_APP
#endif
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using Common.Library;
using OnlineVideos.Entities;
using Windows.Storage;
#if NOTANDROID || WINDOWS_PHONE_APP
//using Windows.Storage;
using Windows.ApplicationModel;
#endif
#if WINDOWS_APP && NOTANDROID
using Windows.UI.Xaml;
#endif
#if ANDROID
using Android.App;
using Android.Content;
#endif

namespace Common.Library
{
    public static class SettingsHelper
    {
#if WINDOWS_APP && NOTANDROID
        public static void AddOrUpdateKey(ApplicationDataContainer settings, string keyName, object keyValue)
        {
            if (settings.Values.ContainsKey(keyName))
               
                settings.Values[keyName] = Convert.ToString(keyValue);
            else
                settings.Values.Add(keyName,Convert.ToString(keyValue));
         }

        public static void AddOrUpdateListKey(ApplicationDataContainer settings, string keyName, object keyValue)
        {
            if (settings.Values.ContainsKey(keyName))

                settings.Values[keyName] = keyValue;
            else
                settings.Values.Add(keyName, keyValue);
        }

        //public static void saveAllSettings(Windows.Storage.ApplicationData settings)
        //{
        //    settings.Save();
        //}

        public static void Save(string keyName, object keyValue)
        {
            try
            {
                var applicationData = Windows.Storage.ApplicationData.Current;
                var settings = applicationData.LocalSettings;             
                AddOrUpdateKey(settings, keyName, keyValue);                
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in getBoolValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                ex.Data.Add("setkeyname_save", keyName);
                ex.Data.Add("setkeyvalue_save", keyValue);
                //Exceptions.SaveExceptionInLocalStorage(excepmess);
            }
        }

        public static void SaveDictionaryValue(string keyName, object keyValue)
        {
            try
            {
                var applicationData = Windows.Storage.ApplicationData.Current;
                var settings = applicationData.LocalSettings;
                AddOrUpdateListKey(settings, keyName, keyValue);
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in getBoolValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                ex.Data.Add("getkeyname_getStringValue", keyName);
            }
        }

        public static Dictionary<string,string> GetDictionaryValue(string KeyName)
        {
            try
            {
                Dictionary<string, string> keyvalue = null;
                var applicationData = Windows.Storage.ApplicationData.Current;
                var settings = applicationData.LocalSettings;
                if (settings.Values.ContainsKey(KeyName))
                    keyvalue = settings.Values[KeyName] as Dictionary<string, string>;
                return keyvalue;
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in getBoolValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                ex.Data.Add("getkeyname_getStringValue", KeyName);
                //Exceptions.SaveExceptionInLocalStorage(excepmess);
                return null;
            }
        }

        public static void SaveListValue(string keyName, object keyValue)
        {
            try
            {
                var applicationData = Windows.Storage.ApplicationData.Current;
                var settings = applicationData.LocalSettings;
                AddOrUpdateListKey(settings, keyName, keyValue);
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in getBoolValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                ex.Data.Add("setkeyname_save", keyName);
                ex.Data.Add("setkeyvalue_save", keyValue);
                //Exceptions.SaveExceptionInLocalStorage(excepmess);
            }
        }

        public static List<string> GetListValue(string keyname)
        {
            try
            {
                List<string> keyvalue = null;
                var applicationData = Windows.Storage.ApplicationData.Current;
                var settings = applicationData.LocalSettings;
                if (settings.Values.ContainsKey(keyname))
                    keyvalue = settings.Values[keyname] as List<string>;
                return keyvalue;
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in getBoolValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                ex.Data.Add("getkeyname_getStringValue", keyname);
                //Exceptions.SaveExceptionInLocalStorage(excepmess);
                return null;               
            }
        }

        public static  string getStringValue(string keyname)
        {
            try
            {
                string keyvalue = string.Empty;
                var applicationData = Windows.Storage.ApplicationData.Current;
                var settings = applicationData.LocalSettings;                
                if (settings.Values.ContainsKey(keyname))
                    keyvalue = settings.Values[keyname].ToString();
                return keyvalue;
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in getBoolValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                ex.Data.Add("getkeyname_getStringValue", keyname);
                //Exceptions.SaveExceptionInLocalStorage(excepmess);
                return null;               
            }
        }

        public static int? getIntValue(string keyName)
        {
            try
            {
                int keyValue = 0;
                var applicationData = Windows.Storage.ApplicationData.Current;
                var settings = applicationData.LocalSettings;              
                var value = settings.Values[keyName];
                if (value == null)
                    keyValue = 0;
                else
                    keyValue = Convert.ToInt32(value.ToString());
                return keyValue;
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in getBoolValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                ex.Data.Add("getkeyname_getBoolValue", keyName);
                /// Exceptions.SaveExceptionInLocalStorage(excepmess);
                return null;
            }
        }
      
        public static bool Contains(string keyname)
        {
            var applicationData = Windows.Storage.ApplicationData.Current;
            var settings = applicationData.LocalSettings;
            return settings.Values.ContainsKey(keyname);
        }

        public static bool Remove(string keyname)
        {
            var applicationData = Windows.Storage.ApplicationData.Current;
            var settings = applicationData.LocalSettings;
            return settings.Values.Remove(keyname);
        }

        public static bool getBoolValue(string keyname)
        {
            try
            {
                bool keyvalue = false;
                var applicationData = Windows.Storage.ApplicationData.Current;
                var settings = applicationData.LocalSettings;
                if (settings.Values.ContainsKey(keyname))
                    keyvalue = Convert.ToBoolean(settings.Values[keyname]);
                return keyvalue;
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in getBoolValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                ex.Data.Add("getkeyname_getBoolValue", keyname);
                return false;
            }
        }

        public static Double getDouableValue(string keyname)
        {
            try
            {
               Double keyvalue =0;
                var applicationData = Windows.Storage.ApplicationData.Current;
                var settings = applicationData.LocalSettings;
                if (settings.Values.ContainsKey(keyname))
                    keyvalue = Convert.ToDouble(settings.Values[keyname]);
                return keyvalue;
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in getBoolValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                ex.Data.Add("getkeyname_getBoolValue", keyname);
                return 0;
            }
        }
#endif
#if NOTANDROID && WINDOWS_PHONE_APP
         public static void AddOrUpdateKey(ApplicationDataContainer settings, string keyName, object keyValue)
         {
           
            if (settings.Values.ContainsKey(keyName))
                settings.Values[keyName] =Convert.ToString(keyValue);
            else
                settings.Values.Add(keyName, Convert.ToString(keyValue));
        }

        public static List<string> GetListValue(string KeyName)
        {
            try
            {
                List<string> keyvalue = null;
                var settings = ApplicationData.Current.LocalSettings;
                if (settings.Values.ContainsKey(KeyName))
                    keyvalue = (List<string>)settings.Values[KeyName];
                return keyvalue;
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in getBoolValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                ex.Data.Add("getkeyname_getStringValue", KeyName);
                Exceptions.SaveExceptionInLocalStorage(excepmess);
                return null;
            }
        }
        //public static List<object> GetListValue1(string KeyName)
        //{
        //    try
        //    {
        //        List<object> keyvalue = null;
        //        var settings = IsolatedStorageSettings.ApplicationSettings;
        //        if (settings.Contains(KeyName))
        //            keyvalue = (List<object>)settings[KeyName];
        //        return keyvalue;
        //    }
        //    catch (Exception ex)
        //    {
        //        string excepmess = "Exception in getBoolValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
        //        ex.Data.Add("getkeyname_getStringValue", KeyName);
        //        Exceptions.SaveExceptionInLocalStorage(excepmess);
        //        return null;
        //    }
        //}
        
        public static Dictionary<string, string> GetDictionaryValue(string KeyName)
        {
            try
            {
                Dictionary<string, string> keyvalue = null;
                var settings = ApplicationData.Current.LocalSettings;
                if (settings.Values.ContainsKey(KeyName))
                    keyvalue = (Dictionary<string, string>)settings.Values[KeyName];
                return keyvalue;
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in getBoolValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                ex.Data.Add("getkeyname_getStringValue", KeyName);
                Exceptions.SaveExceptionInLocalStorage(excepmess);
                return null;
            }
        }

        public static void SaveListValue(string keyName, object keyValue)
        {
            try
            {
                var settings = ApplicationData.Current.LocalSettings;
                if (settings.Values.ContainsKey(keyName))
                {
                    settings.Values[keyName] = (List<string>)keyValue;
                }
                else
                {
                    settings.Values.Add(keyName, (List<string>)keyValue);
                }
                
                //settings.Values.Save();
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in getBoolValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                ex.Data.Add("getkeyname_getStringValue", keyName);
                Exceptions.SaveExceptionInLocalStorage(excepmess);
            }
        }

        //public static void SaveListValue1(string keyName, object keyValue)
        //{
        //    try
        //    {
        //        var settings = IsolatedStorageSettings.ApplicationSettings;
        //        if (settings.Contains(keyName))
        //        {
        //            settings[keyName] = (List<object>)keyValue;
        //        }
        //        else
        //        {
        //            settings.Add(keyName, (List<object>)keyValue);
        //        }
        //        settings.Save();
        //    }
        //    catch (Exception ex)
        //    {
        //        string excepmess = "Exception in getBoolValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
        //        ex.Data.Add("getkeyname_getStringValue", keyName);
        //        Exceptions.SaveExceptionInLocalStorage(excepmess);
        //    }
        //}

        public static void SaveDictionaryValue(string keyName, object keyValue)
        {
            try
            {
                var settings = ApplicationData.Current.LocalSettings;
                if (settings.Values.ContainsKey(keyName))
                {
                    settings.Values[keyName] = (Dictionary<string, string>)keyValue;
                }
                else
                {
                    settings.Values.Add(keyName, (Dictionary<string, string>)keyValue);
                }
                //settings.Save();
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in getBoolValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                ex.Data.Add("getkeyname_getStringValue", keyName);
                Exceptions.SaveExceptionInLocalStorage(excepmess);
            }
        }

        public static void saveAllSettings(ApplicationDataContainer settings)
        {
            //settings.LocalSettings.Values.Save();
        }

        public static void Save(string keyName, object keyValue)
        {
            try
            {
                var settings = ApplicationData.Current.LocalSettings;
                AddOrUpdateKey(settings, keyName, keyValue);                
                //settings.Save();               
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in getBoolValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                ex.Data.Add("setkeyname_save", keyName);
                ex.Data.Add("setkeyvalue_save", keyValue);
                Exceptions.SaveExceptionInLocalStorage(excepmess);
            }
        }
    
        public static string getStringValue(string keyname)
        {
            try
            {
                string keyvalue = string.Empty;
                ApplicationDataContainer container = ApplicationData.Current.LocalSettings;

                if (container.Values.ContainsKey(keyname))
                {
                    keyvalue = container.Values[keyname].ToString();
                }
                else
                {
                    //container.Values.Add(keyname, "Indian Cinema.WindowsPhone");
                   // keyvalue = container.Values[keyname].ToString();
                }
                //else if (container.Name.Contains(keyname))
                //    keyvalue = container.Containers[keyname].ToString();
                    //keyvalue = settings.Containers[keyname].Values.ContainsKey(keyname).ToString();
                return keyvalue;
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in getBoolValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                ex.Data.Add("getkeyname_getStringValue", keyname);
                Exceptions.SaveExceptionInLocalStorage(excepmess);
                return null; 
            }
        }

        public static bool Contains(string keyname)
        {
            var settings = ApplicationData.Current.LocalSettings;
            return settings.Values.ContainsKey(keyname);
        }

        public static bool Remove(string keyname)
        {
            var settings = ApplicationData.Current.LocalSettings;
            return settings.Values.Remove(keyname);
        }

        public static bool getBoolValue(string keyname)
        {
            try
            {
                bool keyvalue = false;
                var settings = ApplicationData.Current.LocalSettings;
                if (settings.Values.ContainsKey(keyname))
                    keyvalue = Convert.ToBoolean(settings.Values[keyname]);
                return keyvalue;
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in getBoolValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                ex.Data.Add("getkeyname_getBoolValue", keyname);
                Exceptions.SaveExceptionInLocalStorage(excepmess);
                return false;
            }
        }

        public static int? getIntValue(string keyName)
        {
            try
            {
                int keyValue = 0;
                var settings = ApplicationData.Current.LocalSettings;
                if (settings.Values.ContainsKey(keyName))
                {
                    if (!Int32.TryParse(settings.Values[keyName].ToString(), out keyValue))
                        return null;
                }
                return keyValue;
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in getBoolValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                ex.Data.Add("getkeyname_getBoolValue", keyName);
                Exceptions.SaveExceptionInLocalStorage(excepmess);
                return null;
            }
        }

        public static double getDouableValue(string keyname)
        {
            try
            {
                double keyvalue = 0.0;
                var settings = ApplicationData.Current.LocalSettings;
                if (settings.Values.ContainsKey(keyname))
                    keyvalue = double.Parse( settings.Values[keyname].ToString());
                return keyvalue;
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in getDouableValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                ex.Data.Add("getkeyname_getStringValue", keyname);
                Exceptions.SaveExceptionInLocalStorage(excepmess);
                return 0.0;
            }
        }
#endif
# if ANDROID

		public static string getStringValue(string keyName)
		{
			string keyvalue = string.Empty;
			try
			{
				var settings = Application.Context.GetSharedPreferences("ApplicationSettings", FileCreationMode.Private);
				if (settings.Contains(keyName))
					keyvalue = settings.GetString(keyName, "");
			}
			catch (Exception ex)
			{
				string excepmess = "Exception in getStringValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
				ex.Data.Add("getkeyname_getStringValue", keyName);
				// Exceptions.SaveExceptionInLocalStorage(excepmess);
				return null; 
			}
			return keyvalue;
		}

		public static bool getBoolValue(string keyName)
		{
			bool keyvalue = false;
			try
			{
				var settings = Application.Context.GetSharedPreferences("ApplicationSettings", FileCreationMode.Private);
				if (settings.Contains(keyName))
					keyvalue = Convert.ToBoolean(settings.GetString(keyName, ""));
			}
			catch (Exception ex)
			{
				string excepmess = "Exception in getBoolValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
				ex.Data.Add("getkeyname_getBoolValue", keyName);
				// Exceptions.SaveExceptionInLocalStorage(excepmess);
				return false;
			}
			return keyvalue;
		}

		public static int? getIntValue(string keyName)
		{
			int keyvalue = 0;
			try
			{
				var settings = Application.Context.GetSharedPreferences("ApplicationSettings", FileCreationMode.Private);
				if (settings.Contains(keyName))
					if (!Int32.TryParse(settings.GetString(keyName,""), out keyvalue))
						return null;
			}
			catch (Exception ex)
			{
				string excepmess = "Exception in getIntValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
				ex.Data.Add("getkeyname_getIntValue", keyName);
				// Exceptions.SaveExceptionInLocalStorage(excepmess);
				return null;
			}
			return keyvalue;
		}

		public static double getDouableValue(string keyName)
		{
			double keyvalue =0;
			try
			{
				var settings = Application.Context.GetSharedPreferences("ApplicationSettings", FileCreationMode.Private);
				if (settings.Contains(keyName))
					keyvalue =Convert.ToDouble(settings.GetString(keyName, ""));
			}
			catch (Exception ex)
			{
				string excepmess = "Exception in getDouableValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
				ex.Data.Add("getkeyname_getDouableValue", keyName);
				// Exceptions.SaveExceptionInLocalStorage(excepmess);
				return 0;
			}
			return keyvalue;
		}

		public static bool Contains(string keyName)
		{
			var settings = Application.Context.GetSharedPreferences("ApplicationSettings", FileCreationMode.Private);
			return settings.Contains(keyName);
		}



		public static List<string> ConvertStringsToStringList(string items)
		{
			List<string> list = new List<string>();
			string[] listItmes = new[] {items}; 
			foreach (string item in listItmes)
			{
				list.Add(item);
			}
			return list;
		}

		public static void Save(string keyName, object keyValue)
		{
			try
			{
				var prefs = Application.Context.GetSharedPreferences("ApplicationSettings", FileCreationMode.Private);
				var editor = prefs.Edit();
				editor.PutString(keyName, keyValue.ToString());
				editor.Commit();
			}
			catch (Exception ex)
			{
				string excepmess = "Exception in Save Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
				ex.Data.Add("setkeyname_save", keyName);
				ex.Data.Add("setkeyvalue_save", keyValue);
				// Exceptions.SaveExceptionInLocalStorage(excepmess);
			}
		}

		public static void SaveListValue(string keyName, object keyValue)
		{
			try
			{

				List<string> keyvalue = null;
				keyvalue = (List<string>)keyValue;
				IsolatedStorageFile isoStore1 = IsolatedStorageFile.GetUserStoreForApplication();
				if (isoStore1.FileExists(keyName+".xml"))
				{
					isoStore1.DeleteFile(keyName + ".xml");
				}
				System.Xml.Linq.XDocument xdoc = new System.Xml.Linq.XDocument();
				foreach (var item in keyvalue)
				{
					if(xdoc.Declaration==null)
					{
						xdoc = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "utf-8", "yes"),
						                                     new System.Xml.Linq.XElement("NewDataSet",
						                             new System.Xml.Linq.XElement(keyName,
						                             new System.Xml.Linq.XElement("Value",item))));
					}
					else
					{

						xdoc.Root.Add(
							new System.Xml.Linq.XElement(keyName,
						                             new System.Xml.Linq.XElement("Value", item)));


					}

					// var settings = Application.Context.GetSharedPreferences("ApplicationSettings", FileCreationMode.Private);
					//if (settings.Contains(keyName))
					//{
					// var editor = settings.Edit();
					// editor.PutString(keyName, keyValue.ToString());
					// editor.Commit();
					//}
				}
				System.IO.StreamWriter writer = null;

				writer = new System.IO.StreamWriter(new IsolatedStorageFileStream(keyName + ".xml", System.IO.FileMode.Create, isoStore1));
				writer.Write(xdoc);
				writer.Close();
				writer.Dispose();
				isoStore1.Dispose();
			}
			catch (Exception ex)
			{
				string excepmess = "Exception in getBoolValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
				ex.Data.Add("getkeyname_getStringValue", keyName);
				// Exceptions.SaveExceptionInLocalStorage(excepmess);


			}
		}

		public static void SaveListValue1(string keyName, object keyValue)
		{
			try
			{
				List<string> keyvalue = null;
				keyvalue = (List<string>)keyValue;
				IsolatedStorageFile isoStore1 = IsolatedStorageFile.GetUserStoreForApplication();
				if (isoStore1.FileExists(keyName+".xml"))
				{
					isoStore1.DeleteFile(keyName + ".xml");
				}
				System.Xml.Linq.XDocument xdoc = new System.Xml.Linq.XDocument();
				foreach (var item in keyvalue)
				{
					if(xdoc.Declaration==null)
					{
						xdoc = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "utf-8", "yes"),
						                                     new System.Xml.Linq.XElement("NewDataSet",
						                             new System.Xml.Linq.XElement(keyName,
						                             new System.Xml.Linq.XElement("Value",item))));
					}
					else
					{

						xdoc.Root.Add(
							new System.Xml.Linq.XElement(keyName,
						                             new System.Xml.Linq.XElement("Value", item)));


					}

					// var settings = Application.Context.GetSharedPreferences("ApplicationSettings", FileCreationMode.Private);
					//if (settings.Contains(keyName))
					//{
					// var editor = settings.Edit();
					// editor.PutString(keyName, keyValue.ToString());
					// editor.Commit();
					//}
				}
				System.IO.StreamWriter writer = null;

				writer = new System.IO.StreamWriter(new IsolatedStorageFileStream(keyName + ".xml", System.IO.FileMode.Create, isoStore1));
				writer.Write(xdoc);
				writer.Close();
				writer.Dispose();
				isoStore1.Dispose();
			}
			catch (Exception ex)
			{
				string excepmess = "Exception in getBoolValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
				ex.Data.Add("getkeyname_getStringValue", keyName);
				// Exceptions.SaveExceptionInLocalStorage(excepmess);


			}
		}
		public static List<string> GetListValue(string KeyName)
		{
			List<string> keyvalue = new List<string>();
			try
			{

				IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
				IsolatedStorageFileStream fs = isoStore.OpenFile(KeyName+".xml", System.IO.FileMode.Open, System.IO.FileAccess.Read);
				System.Xml.Linq.XDocument doc = System.Xml.Linq.XDocument.Load(fs);
				fs.Close();
				fs.Dispose();
				isoStore.Dispose();
				foreach (System.Xml.Linq.XElement element in doc.Descendants(KeyName))
				{
					keyvalue.Add(element.Element("Value").Value);
				}

				return keyvalue;
			}
			catch (Exception ex)
			{
				string excepmess = "Exception in GetListValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
				ex.Data.Add("getkeyname_GetListValue", KeyName);
				// Exceptions.SaveExceptionInLocalStorage(excepmess);
				return keyvalue;
			}
			return keyvalue;
		}

		public static List<string> GetListValue1(string KeyName)
		{
			List<string> keyvalue = new List<string>();
			try
			{

				IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
				IsolatedStorageFileStream fs = isoStore.OpenFile(KeyName+".xml", System.IO.FileMode.Open, System.IO.FileAccess.Read);
				System.Xml.Linq.XDocument doc = System.Xml.Linq.XDocument.Load(fs);
				fs.Close();
				fs.Dispose();
				isoStore.Dispose();
				foreach (System.Xml.Linq.XElement element in doc.Descendants(KeyName))
				{
					keyvalue.Add(element.Element("Value").Value);
				}

				return keyvalue;
			}
			catch (Exception ex)
			{
				string excepmess = "Exception in GetListValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
				ex.Data.Add("getkeyname_GetListValue", KeyName);
				// Exceptions.SaveExceptionInLocalStorage(excepmess);
				return null;
			}

		}

		public static Dictionary<string, string> GetDictionaryValue(string KeyName)
		{
			try
			{
				Dictionary<string, string> keyvalue = new Dictionary<string,string>();
				IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
				IsolatedStorageFileStream fs = isoStore.OpenFile(KeyName+".xml", System.IO.FileMode.Open, System.IO.FileAccess.Read);
				System.Xml.Linq.XDocument doc = System.Xml.Linq.XDocument.Load(fs);
				fs.Close();
				fs.Dispose();
				isoStore.Dispose();
				foreach (System.Xml.Linq.XElement element in doc.Descendants(KeyName))
				{
					keyvalue.Add(element.Element("Key").Value, element.Element("Value").Value);
				}

				return keyvalue;
			}
			catch (Exception ex)
			{
				string excepmess = "Exception in GetDictionaryValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
				ex.Data.Add("getkeyname_GetDictionaryValue", KeyName);
				// Exceptions.SaveExceptionInLocalStorage(excepmess);
				return null;

			}
		}


		public static void SaveDictionaryValue(string keyName, object keyValue)
		{

			try
			{
				Dictionary<string, string> keyvalue = null;
				keyvalue = (Dictionary<string, string>)keyValue;
				IsolatedStorageFile isoStore1 = IsolatedStorageFile.GetUserStoreForApplication();
				if (isoStore1.FileExists(keyName+".xml"))
				{
					isoStore1.DeleteFile(keyName + ".xml");
				}
				System.Xml.Linq.XDocument xdoc = new System.Xml.Linq.XDocument();
				foreach (var item in keyvalue)
				{
					if(xdoc.Declaration==null)
					{
						xdoc = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "utf-8", "yes"),
						                                     new System.Xml.Linq.XElement("NewDataSet",
						                             new System.Xml.Linq.XElement(keyName,
						                             new System.Xml.Linq.XElement("Key",item.Key ),
						                             new System.Xml.Linq.XElement("Value", item.Value))));
					}
					else
					{

						xdoc.Root.Add(
							new System.Xml.Linq.XElement(keyName,
						                             new System.Xml.Linq.XElement("Key", item.Key),
						                             new System.Xml.Linq.XElement("Value", item.Value)));

					}


				}
				System.IO.StreamWriter writer = null;

				writer = new System.IO.StreamWriter(new IsolatedStorageFileStream(keyName + ".xml", System.IO.FileMode.Create, isoStore1));
				writer.Write(xdoc);
				writer.Close();
				writer.Dispose();
				isoStore1.Dispose();
			}
			catch (Exception ex)
			{
				string excepmess = "Exception in SaveDictionaryValue Method In IsolatedSettings.cs file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
				ex.Data.Add("getkeyname_SaveDictionaryValue", keyName);
				// Exceptions.SaveExceptionInLocalStorage(excepmess);


			}
		}

#endif
    }
}