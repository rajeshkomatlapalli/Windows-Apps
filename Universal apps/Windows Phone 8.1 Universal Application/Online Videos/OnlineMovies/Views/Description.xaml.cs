using Common.Library;
using OnlineMovies.Views;
using OnlineVideos;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Indian_Cinema.Views
{   
    public sealed partial class Description : Page
    {
        public Description()
        {
            this.InitializeComponent();
            Loaded += Description_Loaded;
        }

        void Description_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {               
                txtlimit.Visibility = Visibility.Collapsed;
                if (Constants.editdescription == true)
                {
                    int showid = AppSettings.ShowUniqueID;
                    List<ShowList> showlist = Task.Run(async () => await Constants.connection.Table<ShowList>().ToListAsync()).Result;
                    string des = showlist.Where(i => i.ShowID == showid).FirstOrDefault().Description;
                    txtdes.Text = des;
                }
                else
                {
                    txtdes.Text = Constants.Description.ToString();
                }
                txtlength.Text = Convert.ToString(txtdes.Text.Length) + "/" + Convert.ToString(4000);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Description_Loaded Method In Description.cs file.", ex);
            }
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {               
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In Description.cs file.", ex);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {              
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom Method In Description.cs file.", ex);
            }
        }

        private async void savebtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<ShowList> showlist = Task.Run(async () => await Constants.connection.Table<ShowList>().ToListAsync()).Result;
                if (txtdes.Text.Length > 4000)
                {
                    txtlimit.Visibility = Visibility.Visible;
                }
                else
                {
                    txtlimit.Visibility = Visibility.Collapsed;
                    if (Constants.editdescription == true)
                    {
                        int showid = AppSettings.ShowUniqueID;
                        Constants.editdescription = false;
                        ShowList s = showlist.Where(i => i.ShowID == showid).FirstOrDefault();
                        s.Description = txtdes.Text;
                        await Constants.connection.UpdateAsync(s);
                        Constants.DownloadTimer.Start();
                    }
                    else
                    {
                        Constants.Description.Clear();
                        Constants.Description.Append(txtdes.Text);
                    }
                    Frame.GoBack();
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in btnsave_Click_1 Method In Description.cs file.", ex);
            }
        }

        private void txtdes_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            txtlimit.Visibility = Visibility.Collapsed;
            txtlength.Text = Convert.ToString(txtdes.Text.Length) + "/" + Convert.ToString(4000);
        }

        private void imgTitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}