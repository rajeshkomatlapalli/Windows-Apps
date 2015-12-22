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
    public sealed partial class PersonDetail : UserControl
    {
        // Customization objcustom = new Customization();
        // ShowCastManager objvideos = new ShowCastManager();
        public string castId = string.Empty;
        public string castName = string.Empty;
        public string movieid = string.Empty;
        CastProfile persondetail = null;
        public PersonDetail()
        {
            try
            {
            this.InitializeComponent();
            persondetail = new CastProfile();
            progressbar.IsActive = true;
            Loaded += PersonDetail_Loaded;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PersonDetail Method In PersonDetail.cs file", ex);
            }
        }

        void PersonDetail_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GetPageDataInBackground();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PersonDetail_Loaded Method In PersonDetail.cs file", ex);
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

                                                a.Result = ShowCastManager.GetPersonDetail(AppSettings.PersonID);
                                            },
                                            (object s, RunWorkerCompletedEventArgs a) =>
                                            {
                                                persondetail = (CastProfile)a.Result;

                                                //tblkcastdesc.Text = persondetail.Description;
                                                progressbar.IsActive = false;
                                                if (persondetail.Description != "")
                                                {
                                                    tblkCanvasBiodata.Visibility = Visibility.Collapsed;
                                                    tblkcastdesc.Text = persondetail.Description;
                                                }
                                                else
                                                {
                                                    progressbar.IsActive = false;
                                                    if (AppSettings.ProjectName != "Yoga Regimen")
                                                    {
                                                        tblkcastdesc.Text = "Biodata Not Available";
                                                    }
                                                    else
                                                    {
                                                        tblkcastdesc.Text = "Description Not Available";
                                                    }
                                                    tblkcastdesc.HorizontalAlignment = HorizontalAlignment.Center;
                                                    tblkcastdesc.VerticalAlignment = VerticalAlignment.Center;
                                                    tblkCanvasBiodata.Visibility = Visibility.Visible;
                                                }

                                            }
                                          );

                bwHelper.RunBackgroundWorkers();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPageDataInBackground Method In PersonDetail.cs file", ex);
            }
        }

    }
}
