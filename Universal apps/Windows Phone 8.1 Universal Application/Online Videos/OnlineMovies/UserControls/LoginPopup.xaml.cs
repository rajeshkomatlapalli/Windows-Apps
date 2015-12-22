using Common.Library;
using Common.Utilities;
using OnlineVideos.UI;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideos.UserControls
{
    public sealed partial class LoginPopup : UserControl
    {
        #region GlobalDeclaration

        private static PageHelper oldstate;

        #endregion

        #region Constructor
        public LoginPopup()
        {
            this.InitializeComponent();
        }
        #endregion

        #region "Common Methods"

        //TODO: fix the too many margin settings hard coded here 
        //and also remove the requirement of navigation id.
        public void show(string status)
        {
            try
            {
                btn.IsEnabled = true;
                oldstate = PageHelper.InactivatePage(true);
                tblk.Visibility = Visibility.Collapsed;
                tblkreenter.Visibility = Visibility.Collapsed;
                AppSettings.popupcount = true;
                if (status == "true")
                {
                    mypopMessage.IsOpen = true;
                    if (gridpopup.RowDefinitions.Count() > 4)
                    {
                        RowDefinition r = gridpopup.RowDefinitions[3];
                        gridpopup.RowDefinitions.Remove(r);
                        gridpopup.Children.Remove(stkpnlretype);
                        RowDefinition r1 = gridpopup.RowDefinitions[4];
                        gridpopup.RowDefinitions.Remove(r1);
                        gridpopup.Children.Remove(tbxretype);
                    }
                    btn.Content = "Delete";
                    tbktitle.Text = "Delete Password";
                    tbktitle.Margin = new Thickness(-40, 20, 0, 0);
                    btnstk.Margin = new Thickness(20, 40, 0, 0);
                    tblkuserpwd.Margin = new Thickness(15, 80, 0, 0);
                }
                else
                {
                    mypopMessage.IsOpen = true;
                    if (SettingsHelper.Contains("Password"))
                    {
                        if (gridpopup.RowDefinitions.Count() > 4)
                        {
                            RowDefinition r = gridpopup.RowDefinitions[3];
                            gridpopup.RowDefinitions.Remove(r);
                            gridpopup.Children.Remove(stkpnlretype);
                            RowDefinition r1 = gridpopup.RowDefinitions[4];
                            gridpopup.RowDefinitions.Remove(r1);
                            gridpopup.Children.Remove(tbxretype);
                        }
                        btn.Content = "Login";
                        tbktitle.Text = "Sign On";
                        tbktitle.Margin = new Thickness(-240, 20, 0, 0);
                        btnstk.Margin = new Thickness(20, 60, 0, 0);
                        tblkuserpwd.Margin = new Thickness(15, 80, 0, 0);
                    }
                    else
                    {
                        btn.Content = "Submit";
                        tbktitle.Text = "Create Password";
                        tbktitle.Margin = new Thickness(-60, 2, 0, 0);
                        btnstk.Margin = new Thickness(20, 5, 0, 0);
                        tblkuserpwd.Margin = new Thickness(15, 35, 0, 0);
                    }
                }
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in show Method In LoginPopup.cs file.", ex);
            }
        }

        public void close()
        {
            try
            {
                mypopMessage.IsOpen = false;
                AppSettings.popupcount = false;
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in close Method In LoginPopup.cs file.", ex);
            }
        }

        #endregion

        #region Events
        private void tbxuserpwd_LostFocus(object sender, RoutedEventArgs e)
        {
            if (btn.Content.ToString() == "Submit")
            {
                if (tbxuserpwd.Password.Length > 4 && tbxuserpwd.Password.Length < 12)
                {
                    tblk.Visibility = Visibility.Collapsed;
                }
                else
                {
                    tblk.Margin = new Thickness(-160, 5, 0, 0);                    
                    tblk.Text = "Password Length Should Be In Between 4 And 12 ";
                    tblk.Visibility = Visibility.Visible;
                }
            }
        }

        private void tbxuserpwd_GotFocus(object sender, RoutedEventArgs e)
        {
            tblk.Visibility = Visibility.Collapsed;
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _performanceProgressBar.IsIndeterminate = true;
                if (btn.Content.ToString() == "Login")
                {
                    if (tbxuserpwd.Password == AppSettings.ParentalControlPassword)
                    {
                        PageHelper.NavigateTo(NavigationHelper.ParentalControlShowListPage);
                        close();
                        AppSettings.ParentalControl = true;
                        tbxuserpwd.Password = string.Empty;
                    }
                    else
                    {
                        AppSettings.ParentalControl = false;
                        tblk.Visibility = Visibility.Visible;
                        tblk.Text = "Invalid Password";
                        tblk.Margin = new Thickness(-150, 25, 0, 0);
                        tbxuserpwd.Password = string.Empty;
                    }
                }
                else if (btn.Content.ToString() == "Delete")
                {
                    if (tbxuserpwd.Password == AppSettings.ParentalControlPassword)
                    {
                        btn.IsEnabled = false;
                        AppSettings.ParentalControl = false;
                        if (!AppSettings.NavigationID)
                            PageHelper.NavigateTo(NavigationHelper.getSettingsPage("1"));
                        else
                            PageHelper.NavigateTo(NavigationHelper.getSettingsPage("0"));
                        close();
                        tbxuserpwd.Password = string.Empty;
                        SettingsHelper.Remove("Password");
                    }
                    else
                    {
                        AppSettings.ParentalControl = true;
                        tblk.Visibility = Visibility.Visible;
                        tblk.Text = "Invalid Password";
                        btn.IsEnabled = true;
                        tblk.Margin = new Thickness(-150, 25, 0, 0);
                        tbxuserpwd.Password = string.Empty;
                    }
                }
                else
                {
                    if (tbxuserpwd.Password != "" && tbxretype.Password != "")
                    {
                        if (tbxuserpwd.Password.Length > 4 && tbxuserpwd.Password.Length < 12)
                        {
                            if (tbxuserpwd.Password == tbxretype.Password)
                            {
                                btn.IsEnabled = false;
                                SettingsHelper.Save("Parental Control", "true");
                                SettingsHelper.Save("Password", tbxuserpwd.Password);
                                tblkreenter.Visibility = Visibility.Collapsed;
                                PageHelper.NavigateTo(NavigationHelper.ParentalControlShowListPage);                                
                                close();
                                tbxuserpwd.Password = string.Empty;
                                tbxretype.Password = string.Empty;
                            }
                            else
                            {
                                SettingsHelper.Save("Parental Control", "false");
                                tblkreenter.Visibility = Visibility.Visible;
                                tblkreenter.Margin = new Thickness(-250, 5, 0, 0);
                                tblkreenter.Text = "Password And Confirm Password Should Be Same";
                                btn.IsEnabled = true;
                                tbxretype.Password = string.Empty;
                            }
                        }
                        else
                        {
                            SettingsHelper.Save("Parental Control", "false");
                            tblk.Text = "Password Length Should Be In Between 4 And 12 ";
                            btn.IsEnabled = true;
                            tblk.Margin = new Thickness(-160, 5, 0, 0);
                            tblk.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        SettingsHelper.Save("Parental Control", "false");
                        tblk.Visibility = Visibility.Visible;
                        tblk.Text = "Password Cannot Be Null";
                        btn.IsEnabled = true;
                        tblk.Margin = new Thickness(-160, 5, 0, 0);
                    }
                }
                _performanceProgressBar.IsIndeterminate = false;
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in btn_Click Method In LoginPopup.cs file.", ex);
            }
        }

        private void cancelbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btn.Content.ToString() == "Delete")
                {
                    SettingsHelper.Save("Parental Control", "true");
                    close();
                    tblk.Visibility = Visibility.Collapsed;
                    tblkreenter.Visibility = Visibility.Collapsed;
                    if (!AppSettings.NavigationID)
                        PageHelper.NavigateTo(NavigationHelper.getSettingsPage("1"));
                    else
                        PageHelper.NavigateTo(NavigationHelper.getSettingsPage("0"));
                }
                else if (btn.Content.ToString() == "Login")
                {
                    SettingsHelper.Save("Parental Control", "true");
                    close();
                    tblk.Visibility = Visibility.Collapsed;
                    tblkreenter.Visibility = Visibility.Collapsed;
                    oldstate = PageHelper.InactivatePage(false);
                }
                else
                {
                    close();
                    SettingsHelper.Save("Parental Control", "false");

                    if (!AppSettings.NavigationID)
                        PageHelper.NavigateTo(NavigationHelper.getSettingsPage("1"));
                    else
                        PageHelper.NavigateTo(NavigationHelper.getSettingsPage("0"));
                }
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in cancelbtn_Click Method In LoginPopup.cs file.", ex);
            }
        }

        private void tblkreenter_GotFocus(object sender, RoutedEventArgs e)
        {
            tblk.Visibility = Visibility.Collapsed;
            tblkreenter.Visibility = Visibility.Collapsed;
        }
        #endregion

    }
}
