using Common.Library;
using Common.Utilities;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using OnlineVideos.Views;
using System.Diagnostics;
using Windows.Phone.UI.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Indian_Cinema.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CastHub : Page
    {
        #region GlobalDeclaration
        AppInsights insights = new AppInsights();
        Stopwatch stopwatch = new Stopwatch();
        string title = string.Empty;
        private SolidColorBrush adcontrolborder = new SolidColorBrush(Colors.Transparent);
        private bool IsDataLoaded;
        string type = "";
        int ToggleBackgroundFlag = 0;
        string ReadText = string.Empty;
        #endregion

        #region Constructor
        public CastHub()
        {
            this.InitializeComponent();
            PersonHub.Background = LoadCastHubBackground(AppSettings.PersonID);
            IsDataLoaded = false;
        }

        private Brush LoadCastHubBackground(string p)
        {
            ImageBrush Hubbackground = new ImageBrush();
            string path = "";
            int personid = Convert.ToInt32(p);
            CastProfile Cast = new CastProfile();
            var topRatedList = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID == personid).ToListAsync()).Result;
            foreach (CastProfile itm in topRatedList)
            {
                path = itm.FlickrPanoramaImageUrl;
            }
            if (path != null && path != "")
            {
                BitmapImage PersonHubImage = new BitmapImage();
                PersonHubImage.UriSource = new Uri(path, UriKind.Absolute);
                Hubbackground.ImageSource = PersonHubImage;
            }
            return Hubbackground;
        }
        #endregion
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                insights.pageview("CastHub Page");
                //HardwareButtons.BackPressed += HardwareButtons_BackPressed;
                if (ResourceHelper.AppName == Apps.Cricket_Videos.ToString())
                {
                    HubSectionMovies.Header = "match";
                }
                else
                {

                }
            _performanceProgressBar.IsIndeterminate = true;
            string pid = "";
            string Mid = "";

            string[] parameters = (string[])e.Parameter;
            pid = parameters[0].ToString();
            if (parameters[1] !=null)
            type = parameters[1].ToString();
                if(pid!=null)
                {
                    if(type!=null && type!="")
                    {
                        if (type == "search")
                        {
                                                  
                        }
                    }
                }
                if(type=="search")
                {
                 
                }            
        }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In CastHub.cs file.", ex);
                insights.Exception(ex);
            }        
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if(Frame.CanGoBack)
            {
                Frame.GoBack();
                e.Handled = true;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
                //stopwatch = System.Diagnostics.Stopwatch.StartNew();
                //var properties = new Dictionary<string, string> { { "CastHub", "Viewed" } };
                //var metrics = new Dictionary<string, double> { { "Processing Time", stopwatch.Elapsed.TotalMilliseconds } };
                //insights.Event("CastHub page Time", properties, metrics);
                var success = false;
                var startTime = DateTime.UtcNow;
                var timer = System.Diagnostics.Stopwatch.StartNew();
                insights.Dependency("Cast View", "Duration", startTime, timer.Elapsed, success);
                insights.Event("CastHub View");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom Method In CastHub.cs file.", ex);
                insights.Exception(ex);
            }
        }

        private void ToggleBackground_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if (ToggleBackgroundFlag == 1)
                {
                    PersonHub.Background = LoadCastHubBackground(AppSettings.PersonID);
                    ToggleBackgroundFlag = 0;
                }
                else
                {
                    PersonHub.Background = new SolidColorBrush(Colors.Black);
                    ToggleBackgroundFlag = 1;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ToggleBackground_Click Method In CastHub.cs file.", ex);
                insights.Exception(ex);
            }
        }

        MediaElement media = new MediaElement();
        private async void imageSpeech_Tapped(object sender, TappedRoutedEventArgs e)
        {            
            try
            {
                if (Constants.Synthesizer == null)
                {
                    Constants.Synthesizer = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();                    
                    var stream= await Constants.Synthesizer.SynthesizeTextToStreamAsync(ReadText);
                    media.SetSource(stream, stream.ContentType);
                    media.Play();
                    imageSpeech.Source = ResourceHelper.StopSpeech;
                }
                else
                {
                    media.Pause();
                    imageSpeech.Source = ResourceHelper.StartSpeech;
                    Constants.Synthesizer.Dispose();
                    Constants.Synthesizer = null;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in imageSpeech_Click Method In CastHub.cs file.", ex);
                insights.Exception(ex);
            }
        }

        #region "Common Methods"
        public ImageBrush BlankBackground()
        {
            ImageBrush HubBrush = new ImageBrush();
            HubBrush = null;
            return HubBrush;
        }
        #endregion
      
        private void LoadAds()
        {
            //PageHelper.LoadAdControl_New(LayoutRoot, adstackPsnProfile, 1);
            LoadAdds.LoadAdControl_New(LayoutRoot, adstackPsnProfile, 1);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            insights.Trace("CastHub Loaded");
            LoadAds();
            Constants.Synthesizer = null;
            if (IsDataLoaded == false)
            {
                BackgroundHelper bwHelper = new BackgroundHelper();
                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                a.Result = OnlineShow.GetPersonDetail(AppSettings.PersonID).Description;
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                ReadText = a.Result.ToString();
                                                if (ReadText != null)
                                                {
                                                    imageSpeech.Visibility = Visibility.Visible;
                                                }
                                            });
                bwHelper.RunBackgroundWorkers();

                CastHubTitle.Text = OnlineShow.GetPersonDetail(AppSettings.PersonID).Name;
                insights.Event(CastHubTitle.Text + " Viewed");
                IsDataLoaded = true;
            }
            _performanceProgressBar.IsIndeterminate = false;
        }
        private void hub_gallery_Loaded(object sender, RoutedEventArgs e)
        {
            title = OnlineShow.GetPersonDetail(AppSettings.PersonID).Name;
            string gallery = hub_gallery.Text;
            string Person_title = title + " " + gallery;
            if (gallery == "gallery")
            {
                //stopwatch = System.Diagnostics.Stopwatch.StartNew();
                //var properties = new Dictionary<string, string> { { Person_title, "viewed" } };
                //var metrics = new Dictionary<string, double> { { "Processing Time", stopwatch.Elapsed.TotalMilliseconds } };
                //insights.Event("Gallery Time", properties, metrics);
                var success = false;
                var startTime = DateTime.UtcNow;
                var timer = System.Diagnostics.Stopwatch.StartNew();
                insights.Dependency(title+"view", "Duration", startTime, timer.Elapsed, success);
                insights.Event(Person_title + "View");
            }
            else
            {
                stopwatch.Stop();
            }
        }

        private void hub_movies_Loaded(object sender, RoutedEventArgs e)
        {
            string movies = hub_movies.Text;
            if (movies == "movies")
            {
                //stopwatch = System.Diagnostics.Stopwatch.StartNew();
                //var properties = new Dictionary<string, string> { { hub_movies.Text, "viewed" } };
                //var metrics = new Dictionary<string, double> { { "Processing Time", stopwatch.Elapsed.TotalMilliseconds } };
                //insights.Event("movies Time", properties, metrics);
                var success = false;
                var startTime = DateTime.UtcNow;
                var timer = System.Diagnostics.Stopwatch.StartNew();
                insights.Dependency("Movies", "Duration", startTime, timer.Elapsed, success);
                insights.Event(movies + "View");
            }
            else
            {
                stopwatch.Stop();
            }
        }
    }
}
