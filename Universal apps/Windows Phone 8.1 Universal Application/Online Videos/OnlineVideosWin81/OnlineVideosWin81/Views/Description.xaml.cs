using Common.Library;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Common.Data;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Description : Page
    {
        string type = string.Empty;
        AddShow addshow = new AddShow();
        public Description()
        {
            this.InitializeComponent();
            Loaded += Description_Loaded;
        }

        void Description_Loaded(object sender, RoutedEventArgs e)
        {
            if (type == "Movie")
                tblkTitle.Text = AppResources.Type + " Description";
            else
                tblkTitle.Text = "Biography";
            txtlimit.Visibility = Visibility.Collapsed;
            if (Constants.editdescription == true)
            {
                int showid = AppSettings.ShowUniqueID;
                List<ShowList> showlist = addshow.GetShowList();
                string des = showlist.Where(i => i.ShowID == showid).FirstOrDefault().Description;
                txtdes.Text = des;

            }
            else
            {
                txtdes.Text = Constants.Description.ToString();
            }
            txtlength.Text = Convert.ToString(txtdes.Text.Length) + "/" + Convert.ToString(4000);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                type = e.Parameter.ToString();
            }
        }

        private void txtdes_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            txtlimit.Visibility = Visibility.Collapsed;
            txtlength.Text = Convert.ToString(txtdes.Text.Length) + "/" + Convert.ToString(4000);
        }

        private void btnsave_Click(object sender, RoutedEventArgs e)
        {
            List<ShowList> showlist = addshow.GetShowList();
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
                    Task.Run(async () => await Constants.connection.UpdateAsync(s));

                }
                else
                {
                    Constants.Description.Clear();
                    Constants.Description.Append(txtdes.Text);
                }
                Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);


                frame.GoBack();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Constants.editdescription = false;
            Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);
            frame.GoBack();
        }
    }
}
