using Common.Library;
using Common.Utilities;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.Views;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideos.UserControls
{
    public sealed partial class PersonProfileShows : UserControl
    {
        #region GlobalDeclaration
        private bool IsDataLoaded;
        public static dynamic PersonRelatedShows = default(dynamic);
        #endregion

        #region Constructor
        public PersonProfileShows()
        {
            this.InitializeComponent();
            IsDataLoaded = false;
        }
        #endregion

        #region PageLoad
        private void lbxFilmography_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxFilmography.SelectedIndex == -1)
                return;          
            Frame frame = Window.Current.Content as Frame;
            Page p = frame.Content as Page;
            string[] parameters = new string[2];
            parameters[0] = ((sender as Selector).SelectedItem as ShowList).ShowID.ToString();
            parameters[1] = null;            
            p.Frame.Navigate(typeof(Details), parameters);

            AppSettings.ShowID = ((sender as Selector).SelectedItem as ShowList).ShowID.ToString();

            lbxFilmography.SelectedIndex = -1;
        }
        #endregion

        #region PageLoad
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Constants.UIThread = true;
            PersonRelatedShows = OnlineShow.GetPersonRelatedShows(AppSettings.PersonID);
            Constants.UIThread = false;
            try
            {
                if (IsDataLoaded == false)
                {

                    BackgroundHelper bwHelper = new BackgroundHelper();

                    bwHelper.AddBackgroundTask(
                                                (object s, DoWorkEventArgs a) =>
                                                {

                                                    a.Result = PersonRelatedShows;
                                                },
                                                (object s, RunWorkerCompletedEventArgs a) =>
                                                {
                                                    lbxFilmography.ItemsSource = (List<ShowList>)a.Result;

                                                }
                                              );

                    bwHelper.RunBackgroundWorkers();

                    IsDataLoaded = true;

                }
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in UserControl_Loaded Method In PersonProfileShows.cs file.", ex);
            }
        }
        #endregion

    }
}
