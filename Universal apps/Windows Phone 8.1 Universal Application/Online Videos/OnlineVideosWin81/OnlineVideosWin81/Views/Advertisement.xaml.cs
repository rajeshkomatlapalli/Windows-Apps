using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
using System.Reflection;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Advertisement : Page
    {
        int elapsedtime = 15;
        int time = 0;
        DispatcherTimer dt = default(DispatcherTimer);
        public Advertisement()
        {
            this.InitializeComponent();
            Loaded += Advertisement_Loaded;
        }

        private void GetPageDataInBackground()
        {
            try
            {
                List<ShowList> objlist = new List<ShowList>();
                BackgroundHelper bwHelper = new BackgroundHelper();
                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                a.Result = OnlineShow.GetVideoDetails(AppSettings.ShowID);
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                objlist = (List<ShowList>)a.Result;
                                                tblkTitle.Text = objlist.FirstOrDefault().Title;
                                                grdvwDetails.ItemsSource = objlist;
                                            }
                                          );
                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In ShowVideos.cs file", ex);
            }
        }

        private void Advertisement_Loaded(object sender, RoutedEventArgs e)
        {
            //AdRotator12duplex.DefaultSettingsFileUri = "AdvertisementPagedefaultAdSettings.xml";
            //AdRotator12duplex.SettingsUrl = AppResources.AdRotatorSettingsUrl;
            //AdRotator12duplex.Invalidate();
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += dt_Tick;
            dt.Start();
            try
            {
                GetPageDataInBackground();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowDescription_Loaded_1 Method In ShowDescription.cs file", ex);
            }
        }

        void dt_Tick(object sender, object e)
        {
            if (time < 15)
            {
                elapsedtimetblk.Text = "Video: " + AppSettings.PlayVideoTitle + "- Next video will resume in " + elapsedtime-- + " seconds";
                time++;
            }
            else
            {
                (sender as DispatcherTimer).Stop();
                Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);
                if (frame.CanGoBack)
                    frame.GoBack();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            dt.Stop();
            Constants.NavigationFromWebView = true;
            Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);
            if (frame.CanGoBack)
                frame.GoBack();
        }
    }
}
