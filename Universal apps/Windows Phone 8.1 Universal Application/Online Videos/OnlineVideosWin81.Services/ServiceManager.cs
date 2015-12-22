using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
//using Common.Services.ImageService;
using System.Threading.Tasks;
//using Common.Services.OnlineVideoService;
//using Common.Services.FileStoreService;
using System.Collections.Generic;
using OnlineVideosWin81.Services.OnlineVideoService;
using OnlineVideosWin81.Services.ImageService;
using OnlineVideosWin81.Services.FileStoreService;

namespace Common.Services
{
       
    public static class ServiceManager
    {
        public static ObservableCollection<Favorites> RetrieveuserDataAsync(byte[] AnonymousId, string ApplicationName, string ID)
        {
            OnlineVideoServiceClient client = new OnlineVideoServiceClient();
            ObservableCollection<Favorites> list = Task.Run(async () => await client.RetrieveuserDataAsync(ApplicationName,BitConverter.ToString(AnonymousId), Convert.ToInt32(ID))).Result;
            return list;
        }
       
        public static ObservableCollection<Favorites> RestoreParentalControlPreferences(byte[] AnonymousID, string ApplicationName, string ID)
        {
            OnlineVideoServiceClient client = new OnlineVideoServiceClient();
            ObservableCollection<Favorites> GetFavourites=Task.Run(async()=> await client.RetrieveParentalControlDataAsync(BitConverter.ToString(AnonymousID), ApplicationName, Convert.ToInt32(ID))).Result;
            return GetFavourites;
        }

        public static ObservableCollection<Rating> Restoreratingvideos(string rdate,string ApplicationName,string ID)
        {
            OnlineVideoServiceClient client = new OnlineVideoServiceClient();
           ObservableCollection<Rating> GetNextRatingUpdatedDate=Task.Run(async()=>await client.getRatingVideosnextupdateddateAsync(rdate, ApplicationName, Convert.ToInt32(ID))).Result;
           return GetNextRatingUpdatedDate;
        }
        public static ObservableCollection<Rating> Restorerating(string rdate, string ApplicationName, string ID)
        {
            OnlineVideoServiceClient client = new OnlineVideoServiceClient();
            ObservableCollection<Rating> RestoreRating=Task.Run(async()=>await client.getRatingnextupdateddateAsync(rdate, ApplicationName, Convert.ToInt32(ID))).Result;
           return RestoreRating;
        }

        public static void InsertUserData(ObservableCollection<Favorites> fav)
        {
            OnlineVideoServiceClient client = new OnlineVideoServiceClient();
            client.InsertUserDataAsync(fav);
        }
        public static void InsertParentalControlData(ObservableCollection<Favorites> fav)
        {
            OnlineVideoServiceClient client = new OnlineVideoServiceClient();
            client.InsertParentalControlDataAsync(fav);
        }

        public static  ObservableCollection<Favorites> GetRatingvideos(ObservableCollection<Favorites> fav)
        {
            OnlineVideoServiceClient client = new OnlineVideoServiceClient();
           ObservableCollection<Favorites> GetVideoRating=Task.Run(async()=> await client.GetRatingvideosAsync(fav)).Result;
         return GetVideoRating;
        }

        public static  ObservableCollection<Favorites> GetRating(ObservableCollection<Favorites> fav)
        {
            OnlineVideoServiceClient client = new OnlineVideoServiceClient();
            ObservableCollection<Favorites> GetRating=Task.Run(async()=>await client.GetRatingAsync(fav)).Result;
            return GetRating;
        
        }
        public static ObservableCollection<BlogInfo> GetBlogDetails(string Category, string ApplicationName)
        {
            OnlineVideoServiceClient client = new OnlineVideoServiceClient();
            ObservableCollection<BlogInfo> binfo = Task.Run(async () => await client.GetUsernameandPasswordAsync(Category, ApplicationName)).Result;
            return binfo;

        }
        public static ObservableCollection<Favorites> GetSearchHistory(ObservableCollection<Favorites> searchhis)
        {
            OnlineVideoServiceClient client = new OnlineVideoServiceClient();
            ObservableCollection<Favorites> GetSearch = Task.Run(async () => await client.GetSearchHistoryAsync(searchhis)).Result;
            return GetSearch;

        }
        //public static void GetSearchHistory(ObservableCollection<Favorites> searchhis, EventHandler<GetSearchHistoryCompletedEventArgs> callback)
        //{
        //    OnlineVideoServiceClient client = new OnlineVideoServiceClient();
        //    client.GetSearchHistoryAsync(searchhis);
        //    client.GetSearchHistoryCompleted += new EventHandler<GetSearchHistoryCompletedEventArgs>(callback);
        //}
        public static bool SendMailToAppAsync(string fromAddress, string Toaddress, string subject, string body)
        {
            try
            {
                OnlineVideoServiceClient FileStoreclient = new OnlineVideoServiceClient();
                bool MailStatus=Task.Run(async()=>await FileStoreclient.SendMailAsync(fromAddress, Toaddress, subject, body)).Result;
                return true;
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
                return false;
            }
        }

        public static bool SendWebTileMailToAppAsync(string subject,string body, string Toaddress )
        {
            try
            {
                OnlineVideoServiceClient FileStoreclient = new OnlineVideoServiceClient();
                bool MailStatus = Task.Run(async () => await FileStoreclient.SendMailAsync("support@lartsoft.com", Toaddress, subject, body)).Result;
                return true;
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
                return false;
            }
        }
    }
}
