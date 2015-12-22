using Common.Library;
using System;
using System.Linq;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using System.Reflection;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class LoginPopup : UserControl
    {
        public LoginPopup()
        {
            this.InitializeComponent();
            this.Tag = this;
            Loaded += LoginPopup_Loaded;
            pop.Visibility = Visibility.Visible;
        }

        #region "Common Methods"

        //TODO: fix the too many margin settings hard coded here 
        //and also remove the requirement of navigation id.
        public void show(string status)
        {
            pop.Visibility = Visibility.Visible;
            btn.Visibility = Visibility.Visible;
            //oldstate = PageHelper.InactivatePage(true);
            tblk.Visibility = Visibility.Collapsed;
            tblkreenter.Visibility = Visibility.Collapsed;
            AppSettings.popupcount = true;
            if (status == "true")
            {
                // mypopMessage.IsOpen = true;
                if (gridpopup.RowDefinitions.Count() > 4)
                {
                    RowDefinition r = gridpopup.RowDefinitions[3];
                    gridpopup.RowDefinitions.Remove(r);
                    gridpopup.Children.Remove(stkpnlretype);
                    RowDefinition r1 = gridpopup.RowDefinitions[4];
                    gridpopup.RowDefinitions.Remove(r1);
                    gridpopup.Children.Remove(tbxretype);
                }
                //btn.Content = "Delete";
                tbktitle.Text = "Delete Password";
                tbktitle.Margin = new Thickness(-40, 20, 0, 0);
                btnstk.Margin = new Thickness(60, 40, 0, 0);
                tblkuserpwd.Margin = new Thickness(75, 80, 0, 0);
                btn.Source = new BitmapImage(new Uri("ms-appx:///Images/Rating/submit.png", UriKind.RelativeOrAbsolute));
            }
            else
            {
                // mypopMessage.IsOpen = true;
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
                    btn.Source = new BitmapImage(new Uri("ms-appx:///Images/Rating/login.png", UriKind.RelativeOrAbsolute));
                    tbktitle.Text = "Sign On";
                    tbktitle.Margin = new Thickness(-20, 20, 0, 0);
                    btnstk.Margin = new Thickness(60, 30, 0, 0);
                    tblkuserpwd.Margin = new Thickness(75, 50, 0, 0);
                }
                else
                {
                    btn.Source = new BitmapImage(new Uri("ms-appx:///Images/Rating/submit.png", UriKind.RelativeOrAbsolute));
                    tbktitle.Text = "Create Password";
                    tbktitle.Margin = new Thickness(-10, 20, 0, 0);
                    btnstk.Margin = new Thickness(60, 5, 0, 0);
                    tblkuserpwd.Margin = new Thickness(75, 45, 0, 0);
                }
            }
        }

        public void close()
        {
            try
            {
                pop.Visibility = Visibility.Collapsed;
                //Page p = (Page)PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                //p.GetType().GetTypeInfo().GetDeclaredMethod("MainPage1").Invoke(p, null);
            }
            catch (Exception ex)
            {
            }
            // mypopMessage.IsOpen = false;
            AppSettings.popupcount = false;

        }

        #endregion

        void LoginPopup_Loaded(object sender, RoutedEventArgs e)
        {
            //pop.Visibility = Visibility.Visible;
            //  show("false");
        }

        private void tbxuserpwd_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbktitle.Text == "Create Password")
                {
                    if (tbxuserpwd.Password.Length > 4 && tbxuserpwd.Password.Length < 12)
                    {
                        tblk.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        tblk.Margin = new Thickness(-180, 20, 0, 0);
                        //tblk.HorizontalAlignment =HorizontalAlignment.Left;
                        tblk.Text = "Password Length Should Be In Between 4 And 12 ";
                        tblk.Visibility = Visibility.Visible;
                    }
                }

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in tbxuserpwd_LostFocus Method In LoginPopup.cs file", ex);
            }
        }

        private void tbxuserpwd_GotFocus(object sender, RoutedEventArgs e)
        {
            tblk.Visibility = Visibility.Collapsed;
        }

        private void tblkreenter_GotFocus(object sender, RoutedEventArgs e)
        {
            tblk.Visibility = Visibility.Collapsed;
            tblkreenter.Visibility = Visibility.Collapsed;
        }

        private void btn_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            if (AppSettings.Parentalappbarclick != true)
            {

                if (tbktitle.Text == "Sign On")
                {
                    if (tbxuserpwd.Password == AppSettings.ParentalControlPassword)
                    {


                        close();
                        if (AppSettings.PasswordToggle == false)
                        {
                            AppSettings.PasswordToggle = true;
                        }
                        else
                        {
                            AppSettings.PasswordToggle = false;
                        }
                        AppSettings.ParentalControl = true;
                        tbxuserpwd.Password = string.Empty;
                        Popup parent = this.Parent as Popup;
                        if (parent != null)
                        {
                            parent.IsOpen = false;
                        }
                        if (Windows.UI.ViewManagement.ApplicationView.Value != Windows.UI.ViewManagement.ApplicationViewState.Snapped)
                        {
                            SettingsPane.Show();
                        }
                        Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                        p.GetType().GetTypeInfo().GetDeclaredMethod("MainPage1").Invoke(p, null);

                    }
                    else
                    {
                        AppSettings.ParentalControl = false;
                        tblk.Visibility = Visibility.Visible;
                        tblk.Text = "Invalid Password";
                        tblk.Margin = new Thickness(-180, 20, 0, 0);
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
                                btn.Visibility = Visibility.Collapsed;
                                SettingsHelper.Save("Parental Control", "true");
                                SettingsHelper.Save("Password", tbxuserpwd.Password);
                                tblkreenter.Visibility = Visibility.Collapsed;
                                if (AppSettings.PasswordToggle == false)
                                {
                                    AppSettings.PasswordToggle = true;
                                }
                                else
                                {
                                    AppSettings.PasswordToggle = false;
                                }
                                close();
                                tbxuserpwd.Password = string.Empty;
                                tbxretype.Password = string.Empty;
                                Popup parent = this.Parent as Popup;
                                if (parent != null)
                                {
                                    parent.IsOpen = false;
                                }
                                if (Windows.UI.ViewManagement.ApplicationView.Value != Windows.UI.ViewManagement.ApplicationViewState.Snapped)
                                {
                                    SettingsPane.Show();
                                }
                                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                                p.GetType().GetTypeInfo().GetDeclaredMethod("MainPage1").Invoke(p, null);
                            }
                            else
                            {
                                SettingsHelper.Save("Parental Control", "false");
                                tblkreenter.Visibility = Visibility.Visible;
                                tblkreenter.Margin = new Thickness(-190, 5, 0, 0);
                                tblkreenter.Text = "Password And Confirm Password Should Be Same";
                                btn.Visibility = Visibility.Visible;
                                tbxretype.Password = string.Empty;
                            }
                        }
                        else
                        {
                            SettingsHelper.Save("Parental Control", "false");
                            tblk.Text = "Password Length Should Be In Between 4 And 12 ";
                            btn.Visibility = Visibility.Visible;
                            tblk.Margin = new Thickness(-180, 20, 0, 0);
                            tblk.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        SettingsHelper.Save("Parental Control", "false");
                        tblk.Visibility = Visibility.Visible;
                        tblk.Text = "Password Cannot Be Null";
                        btn.Visibility = Visibility.Visible;
                        tblk.Margin = new Thickness(-180, 20, 0, 0);
                    }
                }
            }

            else
            {
                passowrdcreation();
            }
        }

        private void cancelbtn_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            if (AppSettings.PasswordToggle == false || AppSettings.PasswordToggle == true)
            {
               // Settings settings = new Settings();
                if (AppSettings.PasswordToggle == false)
                {
                    AppSettings.PasswordToggle = false;
                }
                else
                {
                    AppSettings.PasswordToggle = true;
                }
                if (AppSettings.Parentalappbarclick == false)
                {
                    Popup parent = this.Parent as Popup;
                    if (parent != null)
                    {
                        parent.IsOpen = false;
                    }
                    if (Windows.UI.ViewManagement.ApplicationView.Value != Windows.UI.ViewManagement.ApplicationViewState.Snapped)
                    {
                        SettingsPane.Show();
                    }
                }
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("MainPage1").Invoke(p, null);
            }
        }

        public void passowrdcreation()
        {
            if (tbktitle.Text == "Sign On")
            {
                if (tbxuserpwd.Password == AppSettings.ParentalControlPassword)
                {
                    close();
                    AppSettings.ParentalControl = true;
                    tbxuserpwd.Password = string.Empty;
                    Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                    p.GetType().GetTypeInfo().GetDeclaredMethod("ParentalControl").Invoke(p, null);

                }
                else
                {
                    AppSettings.ParentalControl = false;
                    tblk.Visibility = Visibility.Visible;
                    tblk.Text = "Invalid Password";
                    tblk.Margin = new Thickness(-180, 20, 0, 0);
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
                            btn.Visibility = Visibility.Collapsed;
                            SettingsHelper.Save("Parental Control", "true");
                            SettingsHelper.Save("Password", tbxuserpwd.Password);
                            tblkreenter.Visibility = Visibility.Collapsed;
                            close();
                            tbxuserpwd.Password = string.Empty;
                            tbxretype.Password = string.Empty;
                        }
                        else
                        {
                            SettingsHelper.Save("Parental Control", "false");
                            tblkreenter.Visibility = Visibility.Visible;
                            tblkreenter.Margin = new Thickness(-190, 5, 0, 0);
                            tblkreenter.Text = "Password And Confirm Password Should Be Same";
                            btn.Visibility = Visibility.Visible;
                            tbxretype.Password = string.Empty;
                        }
                    }
                    else
                    {
                        SettingsHelper.Save("Parental Control", "false");
                        tblk.Text = "Password Length Should Be In Between 4 And 12 ";
                        btn.Visibility = Visibility.Visible;
                        tblk.Margin = new Thickness(-180, 20, 0, 0);
                        tblk.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    SettingsHelper.Save("Parental Control", "false");
                    tblk.Visibility = Visibility.Visible;
                    tblk.Text = "Password Cannot Be Null";
                    btn.Visibility = Visibility.Visible;
                    tblk.Margin = new Thickness(-180, 20, 0, 0);
                }
            }

        }

    }
}
