using Common.Library;
using OnlineVideos.Data;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineVideos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CharacterDetail : Page
    {
        #region Constructer
        public CharacterDetail()
        {
            this.InitializeComponent();
           try
            {
                LayoutRoot.Background = LoadCastHubBackground(AppSettings.PersonID);
            }
            catch(Exception ex)
           {
               Exceptions.SaveOrSendExceptions("Exception In CharecterDetail.cs file.", ex);
           }
        }
        private Brush LoadCastHubBackground(string p)
        {
            ImageBrush HubBackground = new ImageBrush();
            string path = "";
            int personid = Convert.ToInt32(p);
            CastProfile Cast = new CastProfile();
            var topRatedList = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID == personid).ToListAsync()).Result;
            foreach(CastProfile itm in topRatedList)
            {
                path = itm.FlickrPanoramaImageUrl;
            }
            if(path != "")
            {
                BitmapImage PersonHubImage = new BitmapImage();
                PersonHubImage.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
                HubBackground.ImageSource = PersonHubImage;
            }
            return HubBackground;
        }
        #endregion

        #region Pageload
        private void Page_loaded(object sender,RoutedEventArgs e)
        {
            try
            {
                LoadAds();         
                tblkVideosTitle.Text = OnlineShow.GetPersonDetail(AppSettings.PersonID).Name;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In CharacterDetail.cs file.", ex);
            }
        }
        private void LoadAds()
        {
            try
            {
                LoadAdds.LoadAdControl_New(LayoutRoot, adstaSongs, 1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In CharacterDetail file", ex);
                string excepmess = "Exception in LoadAds Method In CharacterDetail file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }

        }
        #endregion

        #region "Common Methods"
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedFrom Method In CharacterDetail.cs file.", ex);
            }
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in OnNavigatedTo Method In CharacterDetail.cs file.", ex);
            }
        }
        
        #endregion
        private void imgTitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
