using Common.Library;
using Common.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineVideos.Views
{
    public class settingclass
    {
        public string SettingName
        {
            get;
            set;
        }
        public string SettingValue
        {
            get;
            set;
        }
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MusicSettings : Page
    {
        List<settingclass> settingclasslist = new List<settingclass>();
        public MusicSettings()
        {
            this.InitializeComponent();
            Loaded += MusicSettings_Loaded;
        }

        void MusicSettings_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAds();
            try
            {                
                Assembly DataAssembly = Assembly.Load(new AssemblyName("Common.Library"));
                Type ClassType = DataAssembly.GetType("Common.Library" + "." + "AppResources");
                //PropertyInfo[] ConditionProperty = ClassType.GetProperties().ToArray();
                PropertyInfo[] ConditionProperty = ClassType.GetRuntimeProperties().ToArray();
                foreach (PropertyInfo property in ConditionProperty)
                {
                    settingclass settingclass = new settingclass();
                    if (property.PropertyType != typeof(Uri))
                    {
                        settingclass.SettingName = property.Name.ToString() + " : ";                        
                            settingclass.SettingValue = (property.GetValue(property) != null ? property.GetValue(property).ToString() : null);
                            if (settingclass.SettingValue != null)
                                settingclasslist.Add(settingclass);                        
                    }
                }
                lbxsettings.ItemsSource = settingclasslist;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in MusicSettings_Loaded Method In MusicSettings.cs file.", ex);
            }
        }

        private void LoadAds()
        {
            try
            {
                //PageHelper.LoadAdControl_New(LayoutRoot, adstackpl, 2);
                LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 2);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In SongDetails file", ex);
                string excepmess = "Exception in LoadAds Method In SongDetails file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In MusicSettings.cs file.", ex);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom Method In MusicSettings.cs file.", ex);
            }
        }
    }
}
