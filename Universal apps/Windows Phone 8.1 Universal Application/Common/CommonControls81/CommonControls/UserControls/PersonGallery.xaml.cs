using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
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
    public sealed partial class PersonGallery : UserControl
    {
        public string castId = string.Empty;
        public string castName = string.Empty;
        public string movieid = string.Empty;
        Popup popgallimages = new Popup();
        public PersonGallery()
        {
            try
            {
            this.InitializeComponent();
            progressbar.IsActive = true;
            Loaded += PersonGallery_Loaded;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PersonGallery Method In PersonGallery.cs file", ex);
            }
        }

        void PersonGallery_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                grdvwGallery.ItemsSource = ShowCastManager.GetGalleryImageList(AppSettings.PersonID);
                progressbar.IsActive = false;

                if (grdvwGallery.Items.Count != 0)
                {
                    grdvwGallery.Visibility = Visibility.Visible;
                }
                else
                {
                    tblkgllmsg.Visibility = Visibility.Visible;
                    if (AppSettings.ProjectName != "Yoga Regimen")
                    {
                        tblkgllmsg.Text = "No gallery available";
                    }
                    else
                    {
                        tblkgllmsg.Text = "No poses available";
                    }
                    grdvwGallery.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PersonGallery_Loaded Method In PersonGallery.cs file", ex);
            }
        }

        private void grdvwGallery_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                var selectedItem = (sender as Selector).SelectedItem as GalleryImageInfo;
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                AppSettings.GallCount1 = (sender as Selector).SelectedIndex.ToString();

                popgallimages.Child = (UIElement)p.GetType().GetTypeInfo().GetDeclaredMethod("Popup").Invoke(p, null);
                popgallimages.IsOpen = true;
                popgallimages.Height = 1000;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in grdvwGallery_SelectionChanged Event In PersonGallery.cs file", ex);
            }
        }

        public void ClosePopUp()
        {
            try
            {
                if (popgallimages.IsOpen == true)
                {
                    popgallimages.IsOpen = false;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ClosePopUp Method In PersonGallery.cs file", ex);
            }
        }

    }
}
