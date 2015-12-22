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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class PersonProfileShows : UserControl
    {
        public string castId = string.Empty;
        public string castName = string.Empty;
        public string movieid = string.Empty;
        public PersonProfileShows()
        {
            try
            {
            this.InitializeComponent();
            progressbar.IsActive = true;
            Loaded += PersonProfileShows_Loaded;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PersonProfileShows Method In PersonProfileShows.cs file", ex);
            }
        }

        void PersonProfileShows_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GetPageDataInBackground();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PersonProfileShows_Loaded Method In PersonProfileShows.cs file", ex);
            }
        }
        private void GetPageDataInBackground()
        {
            try
            {
                BackgroundHelper bwHelper = new BackgroundHelper();
                bwHelper.AddBackgroundTask(
                                            (object s, DoWorkEventArgs a) =>
                                            {
                                                a.Result = ShowCastManager.GetPersonRelatedShows(AppSettings.PersonID);
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                grdvwRelatedMovies.ItemsSource = (List<ShowList>)a.Result;
                                                progressbar.IsActive = false;
                                            }
                                          );

                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In PersonProfileShows.cs file", ex);
            }
        }

        private void grdvwRelatedMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                var selectedItem = (sender as Selector).SelectedItem as ShowList;

                AppSettings.ShowID = selectedItem.ShowID.ToString();
                //Detail.title = selectedItem.Title;
                AppSettings.TileImage = selectedItem.TileImage;
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("DetailPage").Invoke(p, null);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in grdvwRelatedMovies_SelectionChanged Method In PersonProfileShows.cs file", ex);
            }
        }
    }
}
