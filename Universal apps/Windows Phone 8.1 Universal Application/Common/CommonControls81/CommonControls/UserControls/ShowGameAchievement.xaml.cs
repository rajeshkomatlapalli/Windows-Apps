﻿using Common.Library;
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
    public sealed partial class ShowGameAchievement : UserControl
    {
        public ShowGameAchievement()
        {
            try
            {
                //popgallimages = new Popup();
                this.InitializeComponent();
                progressbar.IsActive = true;
                Loaded += ShowGameAchievement_Loaded;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowGameAchievement Method In ShowGameAchievement.cs file", ex);
            }        
        }

        void ShowGameAchievement_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<GameAchievement> objwaplist = new List<GameAchievement>();
                objwaplist = OnlineShow.GetGameAchievement();
                if (objwaplist.Count != 0)
                {
                    progressbar.IsActive = false;
                    //if (objwaplist.FirstOrDefault().Description != null && objwaplist.FirstOrDefault().Description != "")
                    //    tblkname.Text = objwaplist.FirstOrDefault().Name;
                    //tblkdescription.Text = objwaplist.FirstOrDefault().Description;
                    lstvachivement.ItemsSource = objwaplist;
                }
                else
                {
                    progressbar.IsActive = false;
                    txtmsg.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ShowGameAchievement_Loaded Method In ShowGameAchievement.cs file", ex);
            }
        }

        private void lstvachivement_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstvachivement.SelectedIndex == -1)
                    return;
                //if (popgallimages.IsOpen == false)
                //{
                //var selectedItem = (sender as Selector).SelectedItem as GalleryImageInfo;
                AppSettings.GameClassName = "Achievement";
                AppSettings.GameGalCount = (sender as Selector).SelectedIndex.ToString();
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("AchievementPopUp").Invoke(p, new object[] { false });
                lstvachivement.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in lstvachivement_SelectionChanged_1 Method In ShowGameAchievement.cs file", ex);
            }
        }
    }
}
