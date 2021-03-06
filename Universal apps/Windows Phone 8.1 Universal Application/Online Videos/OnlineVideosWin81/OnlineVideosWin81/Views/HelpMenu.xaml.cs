﻿using AdRotator;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HelpMenu : Page
    {
        public HelpMenu()
        {
            this.InitializeComponent();
            Loaded += HelpMenu_Loaded;
        }

        private void HelpMenu_Loaded(object sender, RoutedEventArgs e)
        {
            App.Adcollapased = true;
            Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
            AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
            adcontrol.IsAdRotatorEnabled = false;
            adcontrol.Visibility = Visibility.Collapsed;
        }

        public void Mainpage()
        {
            App.rootFrame.Navigate(typeof(MainPage));
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }

        public void Youtubepage()
        {
            App.rootFrame.Navigate(typeof(Youtube));
            Window.Current.Content = App.rootFrame;
            Window.Current.Activate();
        }

    }
}
