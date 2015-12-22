using Common.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using Windows.Web.Syndication;
using System.Reflection;
using OnlineVideos.Entities;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class Settings : UserControl
    {
        const int ContentAnimationOffset = 100;
        public Settings()
        {
            this.InitializeComponent();
            Loaded += Settings_Loaded;
            FlyoutContent.Transitions = new TransitionCollection();
            FlyoutContent.Transitions.Add(new EntranceThemeTransition()
            {
                FromHorizontalOffset = (SettingsPane.Edge == SettingsEdgeLocation.Right) ? ContentAnimationOffset : (ContentAnimationOffset * -1)
            });            
        }

        void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            if (AppSettings.ProjectName == "Web Tile" || AppSettings.ProjectName == "Video Mix" || AppSettings.ProjectName == "Web Media")
                btnagentlog.Visibility = Visibility.Collapsed;
            else
                btnagentlog.Visibility = Visibility.Visible;
            if (AppSettings.ProjectName == "Hindu Almanac")
            {
                almanaccombo.Visibility = Visibility.Visible;
                country.Text = Constants.CountryName;
                if (!string.IsNullOrEmpty(Constants.StateName))
                    state.Text = Constants.StateName;
                else
                    state.Text = "states not available";
                cityCombo.ItemsSource = Constants.feedclass;
                if (Constants.feedclass.Count > 1)
                {
                    if (!string.IsNullOrEmpty(AppSettings.Country))
                    {
                        cityCombo.SelectedIndex = Constants.feedclass.IndexOf(Constants.feedclass.Where(i => i.name == AppSettings.Country).FirstOrDefault());
                    }
                    else
                    {
                        cityCombo.SelectedIndex = 0;
                    }
                }

            }
            ObservableCollection<string> list = new ObservableCollection<string>();
            list.Add("480p");
            list.Add("720p");
            list.Add("1080p");
            AppSettings.Parentalappbarclick = false;
            this.ComboyoutubeLinkUrl.ItemsSource = list;
            ComboyoutubeLinkUrl.SelectedIndex = AppSettings.ComboYoutube;
            //string s = AppResources.PrivacyOnlineEducation;
            PrivacyLine.NavigateUri = new Uri(AppResources.PrivacyOnlineEducation);
            if (AppSettings.ProjectName == "Story Time" || AppSettings.ProjectName == "Story Time Pro" || AppSettings.ProjectName == "Kids TV Pro" || AppSettings.ProjectName == "Kids TV Shows")
            {
                passwordtoggle.Visibility = Visibility.Visible;
            }
            if (AppSettings.PasswordToggle == true)
            {
                passwordtoggle.IsOn = true;
                //login.Visibility = Visibility.Collapsed;
            }
            else
            {
                passwordtoggle.IsOn = false;
                //login.Visibility = Visibility.Collapsed;
            }

        }

        private void btnagentlog_Click(object sender, RoutedEventArgs e)
        {
            Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
            p.GetType().GetTypeInfo().GetDeclaredMethod("BackAgentError").Invoke(p, null);
        }

        public void ReadRss(Uri rssUri)
        {
            List<feedclass> dataclass = new List<feedclass>();
            XDocument xdoc = new XDocument();
            string ttitle = string.Empty;
            SyndicationClient client = new SyndicationClient();
            Uri feedUri = rssUri;
            SyndicationFeed feed = Task.Run(async () => await client.RetrieveFeedAsync(feedUri)).Result;
            if (feed == null)
                return;
            foreach (SyndicationItem item in feed.Items)
            {
                feedclass data = new feedclass();

                data.title = Html2Xaml.Html2XamlConverter.gettitle(item.Summary.Text);
                data.Text = item.Summary.Text;
                Constants.Items.Add(data);
            }
        }

        private void cityCombo_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (cityCombo.SelectedIndex != -1 && cityCombo.ItemsSource != null && cityCombo.SelectedIndex != 0)
            {
                Constants.networktblk.Visibility = Visibility.Collapsed;
                Constants.Items.Clear();
                AppSettings.CityUrl = ((feedclass)cityCombo.SelectedItem).url.Trim();
                AppSettings.Country = ((feedclass)cityCombo.SelectedItem).name;
                for (int i = -1; i < 2; i++)
                {
                    DateTime currentdate = DateTime.Now.Add(TimeSpan.FromDays(i));
                    string NavigationUri = "http://mypanchang.com/rssfeeddata.php?cityname=" + AppSettings.CityUrl.Trim() + "&yr=" + currentdate.Year + "&mn=" + currentdate.Month + "&dt=" + currentdate.Date.Day;
                    if (!string.IsNullOrEmpty(NavigationUri))
                        ReadRss(new Uri(NavigationUri));
                }
                Constants.flip.SelectedIndex = 1;
            }
        }

        private void ComboyoutubeLinkUrl_DropDownClosed_1(object sender, object e)
        {
            var selecteditem1 = (sender as Selector).SelectedIndex;
            AppSettings.YoutubeQuality = (YouTubeQuality)selecteditem1;
            if (Constants.MediaElementPosition != new TimeSpan())
            {
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("Youtube").Invoke(p, null);
                AppSettings.ComboYoutube = selecteditem1;
                ComboyoutubeLinkUrl.SelectedIndex = AppSettings.ComboYoutube;
            }
            else
            {
                AppSettings.ComboYoutube = selecteditem1;
                ComboyoutubeLinkUrl.SelectedIndex = AppSettings.ComboYoutube;

            }
        }

        private void MySettingsBackClicked(object sender, RoutedEventArgs e)
        {
            Popup parent = this.Parent as Popup;
            if (parent != null)
            {
                parent.IsOpen = false;
            }

            // If the app is not snapped, then the back button shows the Settings pane again.
            if (Windows.UI.ViewManagement.ApplicationView.Value != Windows.UI.ViewManagement.ApplicationViewState.Snapped)
            {
                SettingsPane.Show();
            }
        }

        private void ToggleSwitch_Toggled_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((ToggleSwitch)sender).IsOn == true)
                    AppSettings.AutomaticallyEmailErrors = true;
                else
                    AppSettings.AutomaticallyEmailErrors = false;
            }
            catch (Exception ex)
            {
                ex.Data.Add("Date", DateTime.Now);
                Exceptions.SaveOrSendExceptions("Exception in EmailErrorsToggleSwitch_Checked Method In settings file", ex);
            }
        }

        private void ToggleSwitch_Toggled_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((ToggleSwitch)sender).IsOn == true)
                    AppSettings.AutomaticallyDownloadShows = true;
                else
                    AppSettings.AutomaticallyDownloadShows = false;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in toggleswitchUpdateMovies_Checked Method In Settings file", ex);
            }
        }

        private void ToggleSwitch_Toggled_3(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((ToggleSwitch)sender).IsOn == true)
                    AppSettings.AllowvideoLibabry = true;
                else
                    AppSettings.AllowvideoLibabry = false;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in toggleswitchUpdateMovies_Checked Method In Settings file", ex);
            }
        }

        private void ToggleSwitch_Toggled_4(object sender, RoutedEventArgs e)
        {
            try
            {
                //((LoginPopup)login.Tag).show("false");
                //login.Visibility = Visibility.Visible;

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in toggleswitchUpdateMovies_Checked Method In Settings file", ex);
            }
        }
    }
}
