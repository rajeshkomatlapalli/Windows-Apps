using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using Common.Library;
using System.Collections.ObjectModel;
using OnlineVideos.Entities;
using Common;
using System.Threading.Tasks;
using System.Collections.Generic;
#if WINDOWS_APP
using OnlineVideosWin81.Services.OnlineVideoService;
#endif
#if WP8 &&  NOTANDROID
using OnlineVideos.Services.OnlineVideoService;
using System.Collections.Generic;
#endif

namespace OnlineVideos.Data
{
    public static class SearchHistoryManager
    {
#if WINDOWS_APP
        public static void DeleteSearchHistory(Favorites f)
        {
            DataManager<SearchHistory> searchManager = new DataManager<SearchHistory>();
            try
            {
                SearchHistory searchhis = Task.Run(async () => await Constants.connection.Table<SearchHistory>().Where(i => i.ID == f._historyid).FirstOrDefaultAsync()).Result;
                Task.Run(async () => await Constants.connection.DeleteAsync(searchhis));
                //searchManager.DeleteFromList(i => i.ID == f._historyid);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in DeleteSearchHistory Method In SearchHistoryManager.cs file", ex);
            }
        }

#if WINDOWS_APP
        public static ObservableCollection<Favorites> GetSearchHistory(string SearchLastUploadedId, ObservableCollection<Favorites> searchhis)
        {
            DataManager<SearchHistory> searchManager = new DataManager<SearchHistory>();
            int id = Convert.ToInt32(SettingsHelper.getStringValue("SearchLastUploadedId"));
            var GetSearch = searchManager.GetList(i => i.ID > id, j => j.ID, "asc").ToList().Take(10);
            try
            {
                if (GetSearch.Count() > 0)
                {
                    foreach (var search in GetSearch)
                    {
                        Favorites v = new Favorites();
                        SearchLastUploadedId = search.ID.ToString();
                        v._historyid = search.ID;
                        v._historyMoviename = search.ShowName;
                        v._historycount = search.SearchCount;
                        v._ApplicationName = AppSettings.ProjectName;
                        searchhis.Add(v);
                    }
                    SettingsHelper.Save("SearchLastUploadedId", SearchLastUploadedId);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ObservableCollection<Favorites> Method In SearchHistoryManager.cs file", ex);
            }
            return searchhis;
        }
#endif
#endif
        public static List<SearchHistory> GetShowIDForSearchHistory()
        {
            DataManager<SearchHistory> searchManager = new DataManager<SearchHistory>();
            int id = AppSettings.SearchLastUploadedId;
            return Constants.connection.Table<SearchHistory>().Where(i => i.ID > id).OrderBy(i => i.ID).ToListAsync().Result;
        }

        public static IQueryable<SearchHistory> GetShowIDForWebSearchHistory()
        {
            int id = AppSettings.SearchLastUploadedId;
            return Constants.connection.Table<SearchHistory>().Where(i => i.ID > id).OrderBy(i => i.ID).ToListAsync().Result.AsQueryable();
        }
    }
}