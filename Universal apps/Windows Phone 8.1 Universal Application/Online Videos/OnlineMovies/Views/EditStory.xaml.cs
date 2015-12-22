using Common.Library;
using Common.Utilities;
using OnlineVideos;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace OnlineMovies.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditStory : Page
    {
        ObservableCollection<Stories> story = new ObservableCollection<Stories>();
        public EditStory()
        {
            this.InitializeComponent();
            Loaded += EditStory_Loaded;
        }

        void EditStory_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAds();
            PageHelper.RemoveEntryFromBackStack("EditStory");
            loadstory();
        }
        private void LoadAds()
        {
            try
            {
                LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In Story file", ex);
                string excepmess = "Exception in LoadAds Method In Story file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }
        public void loadstory()
        {
            try
            {
                int showid = AppSettings.ShowUniqueID;
                story = new ObservableCollection<Stories>(Constants.connection.Table<Stories>().Where(i => i.ShowID == showid).OrderBy(j => j.paraId).ToListAsync().Result);
                if (story.Count() == 0)
                    grd2.Visibility = Visibility.Visible;
                else
                {
                    grd2.Visibility = Visibility.Collapsed;                    
                    lbxeditstory.ItemsSource = story;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in loadstory Method In EditStory file", ex);
            }
        }
       
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void imgTitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void edit_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Constants.editstory = true;
            Constants.ParaId = (int)(sender as Image).Tag;
            //NavigationService.Navigate(new Uri("/Views/Story.xaml?PageName=EditStory.xaml", UriKind.Relative));
            Frame.Navigate(typeof(Story), string.Empty);
        }

        private void delete_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            int showid = AppSettings.ShowUniqueID;
            int paraid = (int)(sender as Image).Tag;

            Stories s = Constants.connection.Table<Stories>().Where(i => i.ShowID == showid && i.paraId == paraid).FirstOrDefaultAsync().Result;
            string image = s.Image;
            Constants.connection.DeleteAsync(s);

            story.Remove(story.Where(i => i.ShowID == showid && i.paraId == paraid).FirstOrDefault());
            if (!string.IsNullOrEmpty(image))
            {
                if (Task.Run(async () => await Storage.FileExists("Images/storyImages/" + showid + "/" + image)).Result)
                {
                    Storage.DeleteFile("Images/storyImages/" + showid + "/" + image);
                }
            }
        }
    }
}
