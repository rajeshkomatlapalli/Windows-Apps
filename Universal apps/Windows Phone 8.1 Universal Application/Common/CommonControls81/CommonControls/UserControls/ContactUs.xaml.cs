using Common.Library;
using OnlineVideos.Data;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using System.Reflection;
using OnlineVideos.Common;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class ContactUs : UserControl
    {
        public ContactUs()
        {
            this.InitializeComponent();
            Loaded += ContactUs_Loaded;
        }

        void ContactUs_Loaded(object sender, RoutedEventArgs e)
        {
            ContactList.ItemsSource = OnlineShow.GetContactList();
            wb.Visibility = Visibility.Collapsed;
            FeedBackControl.Visibility = Visibility.Visible;
        }

        private async void ContactList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                if (ContactList.SelectedIndex == -1)
                    return;
                if (ContactList.SelectedIndex > 0)
                {
                    OnlineVideos.Entities.ContactUs contact = (sender as Selector).SelectedItem as OnlineVideos.Entities.ContactUs;
                    if (ContactList.SelectedIndex == 5 || contact.ID == 6)
                    {
                        await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:Search?query=LART SOFT"));
                    }
                    if (contact.ID == 7)
                    {
                        var link = AppResources.upgradeappurl;
                        await Windows.System.Launcher.LaunchUriAsync(new Uri(link.ToString()));
                    }

                    else
                    {
                        FeedBackControl.Visibility = Visibility.Collapsed;
                        wb.Visibility = Visibility.Visible;

                        Uri Source = new Uri(contact.Link.ToString());
                        wb.Source = Source;
                    }
                }
                else
                {

                    wb.Visibility = Visibility.Collapsed;
                    FeedBackControl.Visibility = Visibility.Visible;

                }
                ContactList.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ContactList_SelectionChanged Method In ContactUs.cs file", ex);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Page p = (Page)PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
            p.GetType().GetTypeInfo().GetDeclaredMethod("MainPage").Invoke(p, null);
        }
    }
}
