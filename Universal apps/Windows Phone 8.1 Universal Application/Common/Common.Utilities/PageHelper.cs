using System;
using System.Linq;
using System.Collections.Generic;
using Common.Library;
using Windows.UI.Xaml;
using Windows.UI;
using Windows.Devices.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media;

namespace Common.Utilities
{
    public class PageHelper
    {
        public UIElement OldControl { get; private set; }
        private double oldControlOpacity;
        private Color oldTrayBg;
        private Color oldTrayFg;        
        private Page page;
        private bool disableInteractions;

        public static void NavigateToMainPage(object sender, MouseEventArgs e)
        {
            NavigateTo(UtilitiesResources.MainPageUri);
        }

        public static PageHelper InactivatePage()
        {
            return InactivatePage(false);
        }

        public static PageHelper InactivatePage(bool makePageInactive)
        {
            var s = new PageHelper();
            //s.SetPageStatus(makePageInactive);
            return s;
        }
        public static void NavigateToOnlineImagesPage(string page)
        {
            NavigateTo(new Uri("/Views/" + page + ".xaml", UriKind.Relative));
        }
        public static void NavigateToUserUploadPage(string page)
        {
            NavigateTo(new Uri("/Views/" + page + ".xaml", UriKind.Relative));
        }

        public static Frame RootApplicationFrame
        {
            get { return (Frame)Window.Current.Content; }
        }

        public static Page RootApplicationPage
        {
            get { return (Page)((Frame)Window.Current.Content).Content; }
        }
        public static void AdControlForPro(Grid ContainerGrid, StackPanel AdContainer)
        {
            AdControlForPro(ContainerGrid, AdContainer, 1);
        }
        //public static void AdControlForPro_New(Grid ContainerGrid, AdControl AdContainer)
        //{
        //    AdControlForPro_New(ContainerGrid, AdContainer, 1);
        //}
        public static void AdControlForPro(Grid ContainerGrid, StackPanel AdContainer, int RowPosition)
        {
            if (!UtilitiesResources.ShowAdControl)
            {
                if (ContainerGrid.RowDefinitions.Count > RowPosition)
                {
                    RowDefinition myrow = ContainerGrid.RowDefinitions[RowPosition];
                    ContainerGrid.RowDefinitions.Remove(myrow);
                }
            }

        }
        //public static void AdControlForPro_New(Grid ContainerGrid, AdControl AdContainer, int RowPosition)
        //{
        //    if (!UtilitiesResources.ShowAdControl)
        //    {
        //        if (ContainerGrid.RowDefinitions.Count > RowPosition)
        //        {
        //            RowDefinition myrow = ContainerGrid.RowDefinitions[RowPosition];
        //            ContainerGrid.RowDefinitions.Remove(myrow);
        //        }
        //    }

        //}
        public static void LoadAdControl(Grid ContainerGrid, StackPanel AdContainer)
        {
            // LoadAdControl(ContainerGrid, AdContainer, 1);
        }

        //public static void LoadAdControl(Grid ContainerGrid, StackPanel AdContainer, int RowPosition)
        //{
        //    if (!UtilitiesResources.ShowAdControl)
        //    {
        //        if (ContainerGrid.RowDefinitions.Count > RowPosition)
        //        {
        //            RowDefinition myrow = ContainerGrid.RowDefinitions[RowPosition];
        //            ContainerGrid.RowDefinitions.Remove(myrow);
        //        }
        //    }
        //    else
        //    {
        //        if (UtilitiesResources.ShowAdRotator)
        //        {
        //            AdRotatorControl adControl = new AdRotatorControl();
        //            adControl.Width = 480;
        //            adControl.Height = 80;
        //            adControl.Visibility = (Visibility)0;
        //            adControl.BorderBrush = Resources.TransparentBrush;
        //            adControl.Foreground = Resources.PhoneForegroundBrush;
        //            adControl.Background = Resources.PhoneBackgroundBrush;
        //            if(ResourceHelper.AppName==Apps.Web_Tile.ToString())
        //                   adControl.DefaultHouseAdURI = new Uri("/webslice;component/DefaultData/defaultAdSettings.xml", UriKind.RelativeOrAbsolute).ToString();
        //                    else
        //            adControl.DefaultHouseAdURI = new Uri("/" + ResourceHelper.ProjectName.Replace(" ", "") + ";component/DefaultData/defaultAdSettings.xml", UriKind.RelativeOrAbsolute).ToString();
        //            adControl.LocalSettingsLocation = AppResources.AdRotatorSettingsUrl;
        //            AdContainer.Children.Add(adControl);
        //            //((AdRotator.AdRotatorControl)AdContainer.Children[0]).Invalidate();
        //        }
        //        else
        //        {
        //            //AdControl adControl = new AdControl();
        //            //adControl.AdUnitId = UtilitiesResources.AdControlAdUnitID;
        //            //adControl.ApplicationId = UtilitiesResources.AdControlApplicationID;
        //            //adControl.Width = 480;
        //            //adControl.Height = 80;
        //            //adControl.Visibility = (Visibility)0;
        //            //adControl.BorderBrush = Resources.TransparentBrush;
        //            //adControl.Foreground = Resources.PhoneForegroundBrush;
        //            //adControl.Background = Resources.PhoneBackgroundBrush;
        //            //adControl.IsAutoRefreshEnabled = true;

        //            //AdContainer.Children.Add(adControl);
        //        }
        //    }
        //}

        //public static void LoadAdControl_New(Grid ContainerGrid, AdControl AdContainer, int RowPosition)
        //{
        //    if (!UtilitiesResources.ShowAdControl)
        //    {
        //        if (ContainerGrid.RowDefinitions.Count > RowPosition)
        //        {
        //            RowDefinition myrow = ContainerGrid.RowDefinitions[RowPosition];
        //            ContainerGrid.RowDefinitions.Remove(myrow);
        //        }
        //    }
        //    else
        //    {
        //        if (UtilitiesResources.ShowAdRotator)
        //        {
                    //AdContainer.AdUnitId = UtilitiesResources.AdControlAdUnitID;
        //            AdContainer.ApplicationId = UtilitiesResources.AdControlApplicationID;
        //        }
        //        else
        //        {
        //            AdContainer.AdUnitId = UtilitiesResources.AdControlAdUnitID;
        //            AdContainer.ApplicationId = UtilitiesResources.AdControlApplicationID;
        //        }
        //    }
        //}

        //StatusBar sb;
        //internal void SetPageStatus(bool disablePageInteractions)
        //{
        //    disableInteractions = disablePageInteractions;

        //    page = RootApplicationPage;
        //    OldControl = RootApplicationPage;
        //    oldControlOpacity = OldControl.Opacity;
        //    oldTrayBg = (Color)sb.BackgroundColor;
        //    oldTrayFg = (Color)sb.ForegroundColor;

        //    if (disableInteractions)
        //        OldControl.Opacity = 0.325;
        //    else
        //        OldControl.Opacity = 1;

        //    sb.BackgroundColor = Resources.PhoneBackgroundColor;
        //    sb.ForegroundColor = Resources.PhoneForegroundColor;

        //    if (disableInteractions)
        //        page.IsEnabled = false;
        //    else
        //        page.IsEnabled = true;
        //}

        //public static DependencyObject GetDependencyObjectFromVisualTree(DependencyObject startObject, Type type)
        //{
        //    DependencyObject parent = startObject;
        //    int i = 0;
        //    while (parent != null)
        //    {
        //        if (type.IsInstanceOfType(parent))
        //            break;
        //        else
        //            parent = VisualTreeHelper.GetParent(parent);
        //        i++;
        //    }

        //    return parent;
        //}

        public static IEnumerable<DependencyObject> GetChildsRecursive(DependencyObject root)
        {
            List<DependencyObject> elts = new List<DependencyObject>();
            try
            {
                elts.Add(root);

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
                {
                    elts.AddRange(GetChildsRecursive(VisualTreeHelper.GetChild(root, i)));
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetChildsRecursive Method In Details file", ex);
            }
            return elts;
        }

        public static void RemoveEntryFromBackStack(string pageName)
        {
            try
            {
                int NavigationID = 0;

                //if (PageHelper.RootApplicationFrame.Source.ToString().Contains("myid"))
                //{
                //    int indexOfLastEqualtoOperator = PageHelper.RootApplicationFrame.Source.ToString().LastIndexOf('=') + 1;
                //    string myID = PageHelper.RootApplicationFrame.Source.ToString().Substring(indexOfLastEqualtoOperator, 1);

                //    int myIntID = 0;

                //    if(Int32.TryParse(myID, out myIntID))
                //        NavigationID = myIntID;

                //    AppSettings.NavigationID = Convert.ToBoolean(NavigationID);
                //}

                //var backStackList = PageHelper.RootApplicationFrame.BackStack.ToList();

                //foreach (var page in backStackList)
                //{
                //    if (!pageName.Contains("MainPage"))
                //    {
                //        if (page.Source.ToString().Contains(pageName))
                //        {
                //            PageHelper.RootApplicationFrame.BackStack.RemoveAt();
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
            }
        }

        public static void NavigateTo(Uri uri)
        {
            string url = uri.ToString();
            Type type = Type.GetType(url);
            PageHelper.RootApplicationFrame.Navigate(type);
            //PageHelper.RootApplicationFrame.Navigate(uri);
        }

        public static void NavigateTo(string page, string navigationID)
        {
            NavigateTo(new Uri("/Views/" + page + ".xaml?myid=" + navigationID, UriKind.Relative));
        }

        public static void NavigateToDetailPage(string page, string ShowId)
        {
            //NavigateTo(new Uri("/Views/" + page + ".xaml?id=" + ShowId, UriKind.Relative));
            //PageHelper.RootApplicationFrame.Navigate(uri);
        }

        public static void NavigateToHistoryPage(string page)
        {
            NavigateTo(new Uri("/Views/" + page + ".xaml", UriKind.Relative));
        }
        public static void NavigateToPersonGalleryPopupPage(string page)
        {
            NavigateTo(new Uri("/Views/" + page + ".xaml", UriKind.Relative));
        }
        public static void NavigateToOnlineWebTile(string page)
        {
            NavigateTo(new Uri("/Views/" + page + ".xaml", UriKind.Relative));
        }
        public static void NavigateToFavoritePage(string page)
        {
            NavigateTo(new Uri("/Views/" + page + ".xaml", UriKind.Relative));
        }
        public static void NavigateToRingTonePage(string page, string link, string showid, string chapterno)
        {
            NavigateTo(new Uri("/Views/" + page + ".xaml?link=" + link + "&showid=" + showid + "&chapterno=" + chapterno, UriKind.Relative));
        }

        public static void NavigateToLiricsShowPage(string page, string Title)
        {
            NavigateTo(new Uri("/Views/" + page + ".xaml?id=" + Title, UriKind.Relative));
        }
        public static void NavigateToCastDetailPage(string page, string ShowId)
        {
            NavigateTo(new Uri("/Views/" + page + ".xaml?id=" + ShowId, UriKind.Relative));
        }
        public static void NavigateToDownloadedImagePage(string page)
        {
            NavigateTo(new Uri("/Views/" + page + ".xaml", UriKind.Relative));
        }
        public static void NavigateToPlayDownloadedVideos(string page, string ShowId)
        {
            NavigateTo(new Uri("/Views/" + page + ".xaml?ShowId=" + ShowId, UriKind.Relative));
        }

        //public static string GetFeedbackMailMessage(NavigationContext context)
        //{
        //    string VideoTitle = string.Empty;
        //    string LinkType = string.Empty;
        //    string VideoUrl = string.Empty;
        //    string ShowTitle = string.Empty;
        //    string FeedbackMessage = string.Empty;

        //    context.QueryString.TryGetValue("chno", out VideoTitle);
        //    context.QueryString.TryGetValue("LinkType", out LinkType);
        //    context.QueryString.TryGetValue("uri", out VideoUrl);
        //    context.QueryString.TryGetValue("title", out ShowTitle);
        //    if (context.QueryString.TryGetValue("chno", out VideoTitle) && context.QueryString.TryGetValue("LinkType", out LinkType) && context.QueryString.TryGetValue("uri", out VideoUrl) && context.QueryString.TryGetValue("title", out ShowTitle))
        //    {

        //        FeedbackMessage = "Movie Name : " + ShowTitle + "\n" + VideoTitle + " \n " + "http://m.youtube.com/watch?v=" + VideoUrl + "\n";
        //    }
        //    return FeedbackMessage;
        //}
        //public static string GetFeedbackMailMessageForAudio(NavigationContext context)
        //{
        //    string VideoTitle = string.Empty;
        //    string LinkType = string.Empty;
        //    string VideoUrl = string.Empty;
        //    string ShowTitle = string.Empty;
        //    string FeedbackMessage = string.Empty;
        //    context.QueryString.TryGetValue("chno", out VideoTitle);

        //    context.QueryString.TryGetValue("LinkType", out LinkType);
        //    context.QueryString.TryGetValue("uri", out VideoUrl);
        //    context.QueryString.TryGetValue("title", out ShowTitle);
        //    if (context.QueryString.TryGetValue("chno", out VideoTitle) && context.QueryString.TryGetValue("LinkType", out LinkType) && context.QueryString.TryGetValue("uri", out VideoUrl) && context.QueryString.TryGetValue("title", out ShowTitle))
        //    {

        //        FeedbackMessage = "Movie Name : " + ShowTitle + "\n" + VideoTitle + " \n " +  VideoUrl + "\n";
        //    }
        //    return FeedbackMessage;
        //}
        public static void SetSelectedItemForegroundColor(ListView listBox, string itemTitle)
        {
            try
            {
                IEnumerable<DependencyObject> cboxes = PageHelper.GetChildsRecursive(listBox);

                foreach (DependencyObject obj in cboxes.OfType<TextBlock>())
                {
                    Type type = obj.GetType();
                    if (type.Name == "TextBlock")
                    {
                        TextBlock cb = obj as TextBlock;
                        if (cb.Text == itemTitle)
                            cb.Foreground = new SolidColorBrush(Color.FromArgb(100, 24, 255, 250));
                    }
                }
            }
            catch(Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SetSelectedItemForegroundColor() method in PageHelper.cs under Common.Utilities Class Library", ex);
            }
        }
    }
}