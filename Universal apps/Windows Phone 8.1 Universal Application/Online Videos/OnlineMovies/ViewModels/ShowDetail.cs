using System;
using System.Net;
using System.Windows;
using OnlineVideos.Views;
using Common.Library;
using System.Linq;
using OnlineVideos.UI;
using System.Collections.Generic;
using System.Threading;
using OnlineVideos.Library;
using OnlineVideos.Entities;
using Common.Utilities;
using System.Threading.Tasks;
using Windows.Common.Data;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Storage;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Core;
using OnlineVideos.Data;
using OnlineVideos.UserControls;


namespace OnlineVideos.View_Models
{
    public class ShowDetail
    {
        RatingPopup_New ratingPopup;        

        public ShowDetail()
        {
            //ratingPopup = new RatingPopup();
            ratingPopup = new RatingPopup_New();
        }

        public void RateThisLink(ListView listBox, MenuFlyoutItem selectedItem, string pageName)
        {
            ListViewItem selectedListBoxItem = listBox.ContainerFromItem(selectedItem.DataContext) as ListViewItem;

            ShowLinks LinkInfo = selectedListBoxItem.Content as ShowLinks;
            string tilename = LinkInfo.LinkType;
            if (tilename == "Songs")
                tilename = "songs";
            else
                tilename = "audio";
            ratingPopup.showPopup(LinkInfo.LinkOrder.ToString(), AppSettings.ShowID, Convert.ToDouble(LinkInfo.Rating), pageName, tilename, LinkInfo.LinkType);
        }

        //public void RateThisShow(RatingControl starRating)
        //{            
        //    ratingPopup.showPopup(AppSettings.ShowID, (int)starRating.RateValue, "Details");
        //}

        string message;
        ListView listBox;
        MenuFlyoutItem selectedItem;
        public void PostAppLinkToSocialNetworks(ListView listBox1, MenuFlyoutItem selectedItem1, string message1)
        {
            ListViewItem selectedListBoxItem = listBox1.ContainerFromItem(selectedItem1.DataContext) as ListViewItem;

            //ShareLinkTask shareLinkTask = new ShareLinkTask();
            DataTransferManager shareLinkTask = DataTransferManager.GetForCurrentView();
            message = message1;
            listBox = listBox1;
            selectedItem = selectedItem1;
            shareLinkTask.DataRequested += shareLinkTask_DataRequested;         
            //shareLinkTask.DataRequested += delegate(DataTransferManager sender, DataRequestedEventArgs args) { shareLinkTask_DataRequested1(sender, args, message, listBox, selectedItem); };
            //shareLinkTask.Title = ResourceHelper.ProjectName + " App";
            //string lnk = message;
            //lnk += "'" + (selectedListBoxItem.Content as ShowLinks).Title + "', Get the app at \n";
            //shareLinkTask.LinkUri = ResourceHelper.AppMarketplaceWebLink;
            //shareLinkTask.Message = lnk;
            //shareLinkTask.Show();
        }

        void shareLinkTask_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            try
            {
                ListViewItem selectedListBoxItem = listBox.ContainerFromItem(selectedItem.DataContext) as ListViewItem;
                string lnk = message;
                lnk += "'" + (selectedListBoxItem.Content as ShowLinks).Title + "', Get the app at \n";
                DataRequest request = args.Request;
                request.Data.Properties.Title = ResourceHelper.ProjectName + " App";
                request.Data.SetWebLink(ResourceHelper.AppMarketplaceWebLink);
                request.Data.Properties.Description = lnk;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in shareLinkTask_DataRequested Method In ShowDetails.cs file.", ex);
            }
        }
       
        void shareLinkTask_DataRequested1(DataTransferManager sender, DataRequestedEventArgs args, string message, ListView listBox, MenuFlyoutItem selectedItem)
        {
            ListViewItem selectedListBoxItem = listBox.ContainerFromItem(selectedItem.DataContext) as ListViewItem;
            string lnk = message;
            lnk += "'" + (selectedListBoxItem.Content as ShowLinks).Title + "', Get the app at \n";
            DataRequest request = args.Request;
            request.Data.Properties.Title = ResourceHelper.ProjectName + " App";
            request.Data.SetWebLink(ResourceHelper.AppMarketplaceWebLink);
            request.Data.Properties.Description = lnk;
        }

        public void ReportBrokenLink(ListView listBox, MenuFlyoutItem selectedItem)
        {
            ListViewItem selectedListBoxItem = listBox.ContainerFromItem(selectedItem.DataContext) as ListViewItem;
            if (selectedListBoxItem != null)
            {                
                PageHelper.NavigateTo(NavigationHelper.getFeedbackPage(selectedListBoxItem.Content as OnlineVideos.Entities.ShowLinks));
            }
        }

        public void AddToFavorites(ListView listBox, MenuFlyoutItem selectedItem, LinkType linkType)
        {
            ListViewItem selectedListBoxItem = listBox.ContainerFromItem(selectedItem.DataContext) as ListViewItem;
            if (selectedListBoxItem == null)
                return;

            ShowLinks LinkInfo = selectedListBoxItem.Content as ShowLinks;
            
            FavoritesManager.AddOrRemoveFavorite(LinkInfo.ShowID, LinkInfo.LinkID);

            SyncFavouritesManager.savexml(LinkInfo.LinkType, LinkInfo.LinkID.ToString());
            //Thread SongThread = new Thread(() =>
            Task.Run(async delegate
            {
                List<ShowLinks> showLinks = OnlineShow.GetShowLinksByTypeForWp8(LinkInfo.ShowID.ToString(), linkType/*, false*/);
                CoreDispatcher dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { listBox.ItemsSource = showLinks; });
            });
           // SongThread.Start();
        }

        public void PinOrUnpinShow(Image PinUnpinIcon)
        {
            bool IsMoviePinned;

            ShowList show = OnlineShow.GetShowDetail(Convert.ToInt32(AppSettings.ShowID));

            //IsMoviePinned = ShellTileHelper.IsPinned(AppSettings.ShowID);
            //if (IsMoviePinned)
            //{
            //    ShellTileHelper.UnPin(AppSettings.ShowID);
            //    Storage.DeleteSecondaryTileImage(show.TileImage);
            //    if(PinUnpinIcon != null)
            //        PinUnpinIcon.Source = ResourceHelper.PinImage;
            //}
            //else
            //{
            //    ShellTileHelper.PinShowToStartScreen(show);

            //    if (PinUnpinIcon != null)
            //        PinUnpinIcon.Source = ResourceHelper.UnpinImage;
            //}
        }

        public async static Task<Brush> LoadShowPivotBackground(long ShowID)
        {
            ShowList show = OnlineShow.GetShowDetail(ShowID);
            return await ImageHelper.LoadShowPivotBackground(show.PivotImage);
        }

        public void QuizRateThisLink(ListView listBox, MenuFlyoutItem selectedItem, string pageName)
        {
            ListViewItem selectedListBoxItem = listBox.ContainerFromItem(selectedItem.DataContext) as ListViewItem;
            QuizList LinkInfo = selectedListBoxItem.Content as QuizList;
            string rating = QuizManager.GetQuizrating(LinkInfo.QuizID.ToString(), Convert.ToInt32(LinkInfo.ShowID));
            if (rating != "")
                ratingPopup.showPopup(LinkInfo.QuizID.ToString(), LinkInfo.ShowID.ToString(), Convert.ToDouble(rating), pageName, "Quiz", "Quiz");
            else
                ratingPopup.showPopup(LinkInfo.QuizID.ToString(), LinkInfo.ShowID.ToString(), 0, pageName, "Quiz", "Quiz");

        }

        public void QuizAddToFavorites(ListView listBox, MenuFlyoutItem selectedItem, LinkType linkType)
        {
            ListViewItem selectedListBoxItem = listBox.ContainerFromItem(selectedItem.DataContext) as ListViewItem;
            if (selectedListBoxItem == null)
                return;

            QuizList LinkInfo = selectedListBoxItem.Content as QuizList;
            FavoritesManager.QuizAddOrRemoveFavorite(Convert.ToInt32(LinkInfo.ShowID), LinkInfo.QuizID);
            
            //Thread SongThread = new Thread(() =>
            
            //{
            //    List<QuizList> showsubjects = QuizManager.GetsubjectsForWP8(LinkInfo.ShowID.ToString()/*, false*/);
            //    Deployment.Current.Dispatcher.BeginInvoke(() => { listBox.ItemsSource = showsubjects; });
            //});
            //SongThread.Start();

            //Thread SongThread = new Thread(() =>
        Task.Run(async delegate
            {
                List<QuizList> showsubjects = QuizManager.GetsubjectsForWP8(LinkInfo.ShowID.ToString()/*, false*/);
                CoreDispatcher dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
               await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { listBox.ItemsSource = showsubjects; });
            });
            //SongThread.Start();
            
        }

        public void QuizPostAppLinkToSocialNetworks(ListView listBox, MenuFlyoutItem selectedItem, string message) 
        {
            //ListBoxItem selectedListBoxItem = listBox.ItemContainerGenerator.ContainerFromItem(selectedItem.DataContext) as ListBoxItem;

            //ShareLinkTask shareLinkTask = new ShareLinkTask();
            DataTransferManager shareLinkTask = DataTransferManager.GetForCurrentView();
            shareLinkTask.DataRequested += delegate(DataTransferManager sender, DataRequestedEventArgs args) { shareLinkTask_DataRequested(sender, args, message,listBox,selectedItem); };        
            //shareLinkTask.Title = ResourceHelper.ProjectName + " App";
            //string lnk = message;
            //lnk += "'" + (selectedListBoxItem.Content as QuizList).Name + "', Get the app at \n";
            //shareLinkTask.LinkUri = ResourceHelper.AppMarketplaceWebLink;
            //shareLinkTask.Message = lnk;
            //shareLinkTask.Show();
        }

        void shareLinkTask_DataRequested(DataTransferManager sender, DataRequestedEventArgs args, string message,ListView listBox, MenuFlyoutItem selectedItem)
        {            
            ListBoxItem selectedListBoxItem = listBox.ItemContainerGenerator.ContainerFromItem(selectedItem.DataContext) as ListBoxItem;
            string lnk = message;
            lnk += "'" + (selectedListBoxItem.Content as QuizList).Name + "', Get the app at \n";
            DataRequest request = args.Request;
            request.Data.Properties.Title = ResourceHelper.ProjectName + " App";
            request.Data.SetWebLink(ResourceHelper.AppMarketplaceWebLink);
            request.Data.Properties.Description = lnk;
        }

        public void DeleteDownloadedShow(ListView listBox, MenuFlyoutItem selectedItem)
        {
            string filename = "";
            ListBoxItem selectedListBoxItem = listBox.ItemContainerGenerator.ContainerFromItem(selectedItem.DataContext) as ListBoxItem;
            int title = (selectedListBoxItem.Content as ShowList).ShowID;          
            List<ShowList> xquery = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(v=>v.ShowID == title).ToListAsync()).Result;
            if (xquery.Count() > 0)
            {
                foreach (var itm in xquery)
                {
                    filename = itm.Title;
                    Deletefromisotore(title);
                    DeleteShowCast(title);
                    if (ResourceHelper.ProjectName == "Story Time")
                    {
                        List<Stories> xquerystory = Task.Run(async () => await Constants.connection.Table<Stories>().Where(v=>v.ShowID == title).ToListAsync()).Result;
                        if (xquerystory.Count() > 0)
                        {
                            foreach (Stories st in xquerystory)
                            {
                                Task.Run(async () => await Constants.connection.DeleteAsync(st));
                            }                         
                        }
                    }
                    if (ResourceHelper.ProjectName == "Online Education")
                    {

                        List<QuizList> xquery9 = Task.Run(async () => await Constants.connection.Table<QuizList>().Where(v => v.ShowID == title).ToListAsync()).Result;
                        if (xquery9.Count() > 0)
                        {
                            foreach (QuizList quizlist in xquery9)
                            {
                                Task.Run(async () => await Constants.connection.DeleteAsync(quizlist));
                            }
                        }
                        List<QuizQuestions> xquery10 = Task.Run(async () => await Constants.connection.Table<QuizQuestions>().Where(v => v.ShowID == title).ToListAsync()).Result;
                        if (xquery10.Count() > 0)
                        {
                            foreach (QuizQuestions quizquestion in xquery10)
                            {
                                if (Task.Run(async()=>await Storage.FileExists("Images/QuestionsImage/" + title + "/" + quizquestion.Image + ".jpg")).Result)
                                {
                                    Storage.DeleteFile("Images/QuestionsImage/" + title + "/" + quizquestion.Image + ".jpg");
                                }
                                Task.Run(async () => await Constants.connection.DeleteAsync(quizquestion));
                            }
                        }
                    }
                }
                if (ResourceHelper.ProjectName == "Web Tile")
                {
                    List<WebDailyTable> wt = Task.Run(async () => await Constants.connection.Table<WebDailyTable>().Where(v => v.ShowID == title).ToListAsync()).Result;
                    if (wt.Count() > 0)
                    {
                        foreach (WebDailyTable webdaily in wt)
                        {
                            Task.Run(async () => await Constants.connection.DeleteAsync(webdaily));
                        }                     
                    }
                    List<WebInformation> wi = Task.Run(async () => await Constants.connection.Table<WebInformation>().Where(v => v.ShowID == title).ToListAsync()).Result;
                    if (wi.Count() > 0)
                    {
                        foreach (WebInformation webinformation in wi)
                        {
                            Task.Run(async () => await Constants.connection.DeleteAsync(webinformation));
                        }
                    }
                }

                if (Task.Run(async()=>await Storage.FileExists("Images/" + xquery.FirstOrDefault().TileImage)).Result)
                {
                    Storage.DeleteFile("Images/" + xquery.FirstOrDefault().TileImage);
                }
                ShowList ds = xquery.FirstOrDefault();
                Task.Run(async () => await Constants.connection.DeleteAsync(ds));

            }
            listBox.SelectedIndex = -1;
        }
        private void DeleteShowCast(int filename)
        {

            if (Task.Run(async () => await Constants.connection.Table<OnlineVideos.Entities.ShowCast>().FirstOrDefaultAsync()).Result != null)
            {
                if (Task.Run(async () => await Constants.connection.Table<OnlineVideos.Entities.ShowCast>().Where(i => i.ShowID == filename).FirstOrDefaultAsync()).Result != null)
                {
                    List<OnlineVideos.Entities.ShowCast> xquery = Task.Run(async () => await Constants.connection.Table<OnlineVideos.Entities.ShowCast>().Where(i => i.ShowID == filename).ToListAsync()).Result;
                    if (xquery.Count() > 0)
                    {
                        foreach (OnlineVideos.Entities.ShowCast pid in xquery)
                        {
                            int castcount = 0;

                            var xqueryw = Task.Run(async () => await Constants.connection.Table<OnlineVideos.Entities.ShowCast>().Where(id => id.PersonID == pid.PersonID).ToListAsync()).Result.GroupBy(id => id.PersonID).OrderByDescending(id => id.Count()).Select(g => new { Count = g.Count() });
                            if (xqueryw.Count() > 0)
                            {

                                foreach (var itm in xqueryw)
                                {
                                    castcount = itm.Count;

                                    if (castcount == 1 && Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == filename && i.Status == "Custom").FirstOrDefaultAsync()).Result != null)
                                    {
                                        CastProfile ds1 = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID == pid.PersonID).FirstOrDefaultAsync()).Result;
                                        if (Task.Run(async()=>await Storage.FileExists("Images/PersonImages/" + ds1.PersonID + ".jpg")).Result)
                                        {
                                            Storage.DeleteFile("Images/PersonImages/" + ds1.PersonID + ".jpg");
                                        }
                                        Task.Run(async () => await Constants.connection.DeleteAsync(ds1));                                      
                                    }
                                }
                            }
                        }
                        List<OnlineVideos.Entities.ShowCast> ds = Task.Run(async () => await Constants.connection.Table<OnlineVideos.Entities.ShowCast>().Where(i => i.ShowID == filename).ToListAsync()).Result;
                        foreach (var cast in ds)
                        {
                            Task.Run(async () => await Constants.connection.DeleteAsync(cast));
                        }
                    }
                }
            }

        }
        private void Deletefromisotore(int filename)
        {
            string linkurl = "";        
            List<ShowLinks> xquery = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i=>i.ShowID == filename).ToListAsync()).Result;
            if (xquery.Count() > 0)
            {
                foreach (var itm in xquery)
                {
                    linkurl = itm.LinkUrl;                    
                    Task.Run(async () => await Constants.connection.DeleteAsync(itm));
                }              
            }
            //IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication()
            //if (isoStore.FileExists(linkurl))
            //{
            //    isoStore.DeleteFile(linkurl);
            //}

            StorageFile isoStore = Task.Run(async () => await ApplicationData.Current.LocalFolder.GetFileAsync(linkurl)).Result;
            Task.Run(async () => await isoStore.DeleteAsync());
            
            //var isoStore = await ApplicationData.Current.LocalFolder.GetFileAsync(linkurl);
            //if (isoStore != null)
            //{
            //    await isoStore.DeleteAsync(StorageDeleteOption.PermanentDelete);
            //}

        }
        private void DeletefromLinks(int ShowID)
        {
            string linkurl = "";
            //IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
           // StorageFolder isoStore = ApplicationData.Current.LocalFolder;
            var xquery = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == ShowID).ToListAsync()).Result;
            if (xquery.Count() > 0)
            {
                foreach (var itm in xquery)
                {
                    linkurl = itm.LinkUrl;
                    ShowLinks ds = xquery.FirstOrDefault();
                    Task.Run(async () => await Constants.connection.DeleteAsync(ds));
                    //if (isoStore.FileExists(linkurl))
                    //{
                    //    isoStore.DeleteFile(linkurl);
                    //}

                    StorageFile isoStore = Task.Run(async () => await ApplicationData.Current.LocalFolder.GetFileAsync(linkurl)).Result;
                    Task.Run(async () => await isoStore.DeleteAsync());

                    //var isoStore = await ApplicationData.Current.LocalFolder.GetFileAsync(linkurl);
                    //if (isoStore != null)
                    //{
                    //    await isoStore.DeleteAsync(StorageDeleteOption.PermanentDelete);
                    //}
                }               
            }            
        }

        private void DeletefromPlayList(int ShowID)
        {
            string linkurl = "";
            //IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
            StorageFolder isoStore = ApplicationData.Current.LocalFolder;
            var xquery = Task.Run(async () => await Constants.connection.Table<PlayListTable>().Where(i => i.ShowID == ShowID).ToListAsync()).Result;
            if (xquery.Count() > 0)
            {
                foreach (var itm in xquery)
                {
                    linkurl = itm.LinkUrl;
                    PlayListTable ds = xquery.FirstOrDefault();
                    Task.Run(async () => await Constants.connection.DeleteAsync(ds));
                }
            }
        }

        public void DeleteDownloadedShowlinks(ListBox listBox, MenuFlyoutItem selectedItem,LinkType type)
        {
            string filename = "";
            ListBoxItem selectedListBoxItem = listBox.ItemContainerGenerator.ContainerFromItem(selectedItem.DataContext) as ListBoxItem;
            int showid = (selectedListBoxItem.Content as ShowLinks).ShowID;
            int linkorder = (selectedListBoxItem.Content as ShowLinks).ShowID;
            string linktype=type.ToString();
            var xquery = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkOrder == linkorder && i.LinkType == linktype).ToListAsync()).Result;
            if (xquery.Count() > 0)
            {
                foreach (var itm in xquery)
                {
                    filename = itm.LinkUrl;
                }
                ShowLinks ds = xquery.FirstOrDefault();
                //await Constants.connection.DeleteAsync(ds);
                Task.Run(async () => await Constants.connection.DeleteAsync(ds));
            }
            //IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();

            //if (isoStore.FileExists(filename))
            //{
            //    isoStore.DeleteFile(filename);
            //}

            StorageFile isoStore = Task.Run(async () => await ApplicationData.Current.LocalFolder.GetFileAsync(filename)).Result;
            Task.Run(async () => await isoStore.DeleteAsync());

            //var isoStore = await ApplicationData.Current.LocalFolder.GetFileAsync(filename);
            //if (isoStore != null)
            //{
            //    await isoStore.DeleteAsync(StorageDeleteOption.PermanentDelete);
            //}
        }
    }
}