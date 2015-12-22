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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideos.UserControls
{
    public sealed partial class PersonDetail : UserControl
    {
        #region GlobalDeclaration
        CastProfile persondetail = null;
        private bool IsDataLoaded;
        #endregion

        #region Constructor
        public PersonDetail()
        {
            this.InitializeComponent();
        }
        #endregion
        
        #region PageLoad
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsDataLoaded == false)
                {
                    BackgroundHelper bwHelper = new BackgroundHelper();

                    bwHelper.AddBackgroundTask(
                                                (object s, DoWorkEventArgs a) =>
                                                {
                                                    a.Result = OnlineShow.GetPersonDetail(AppSettings.PersonID);
                                                },
                                                (object s, RunWorkerCompletedEventArgs a) =>
                                                {
                                                    persondetail = (CastProfile)a.Result;
                                                    tblkBiographDescription.Text = persondetail.Description;
                                                    if (tblkBiographDescription.Text != string.Empty)
                                                    {
                                                        tblkCanvasBiodata.Visibility = Visibility.Collapsed;
                                                    }
                                                    else
                                                    {
                                                        tblkCanvasBiodata.Text = "Descriprtion not available";
                                                        tblkCanvasBiodata.Visibility = Visibility.Visible;
                                                    }
                                                }
                                              );

                    bwHelper.RunBackgroundWorkers();
                    IsDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in UserControl_Loaded Method In PersonDetail.cs file.", ex);
            }
            //CastProfile persondetail=  OnlineShow.GetPersonDetail(AppSettings.PersonID);
        }
        #endregion
    }
}
