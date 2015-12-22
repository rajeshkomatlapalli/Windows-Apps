using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using OnlineVideos.Entities;
using Common.Library;
using System.Linq;
#if WINDOWS_APP
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
#endif
#if WP8 && NOTANDROID
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
#endif
using System.Threading.Tasks;
using System.Threading;


namespace OnlineVideos.Data
{
    public static class CricketCast
    {

        #if WINDOWS_APP
        public static ImageSource loadBitmapImageInBackground(string imagefile, UserControl thread, AutoResetEvent aa)
        {
            BitmapImage image = null;
            thread.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {

                image = new BitmapImage(new Uri(imagefile, UriKind.RelativeOrAbsolute));
                aa.Set();
            });
            aa.WaitOne();
            return image;
        }
#endif
        public static List<CastRole> GetCricketCast(string id, string TeamId)
        {
            List<CastRole> objCastList = new List<CastRole>();
            try
            {
                List<ShowCast> objCastDetails = new List<ShowCast>();
                int showid = Convert.ToInt32(id);
                int teamid = Convert.ToInt32(TeamId);
                objCastDetails = Task.Run(async () => await Constants.connection.Table<ShowCast>().Where(i => i.ShowID == showid && i.CategoryID == teamid).OrderBy(j => j.ShowID).ToListAsync()).Result; //(datamanager.GetList(i => i.ShowID.ToString() == id && i.CategoryID == Convert.ToInt32(TeamId), j => Convert.ToInt32(j.ShowID), "R")).ToList();
                foreach (ShowCast cast in objCastDetails)
                {
                    CastRole role = new CastRole();
                    CastProfile cp = new CastProfile();
                    CastRoles crl = new CastRoles();
                    role.PersonID = cast.PersonID;
                    cp = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(j => j.PersonID == cast.PersonID).FirstOrDefaultAsync()).Result;
                    role.PersonName = cp.Name;
                    if (cast.RoleID != null)
                        crl = Task.Run(async () => await Constants.connection.Table<CastRoles>().Where(k => k.RoleID == cast.RoleID).FirstOrDefaultAsync()).Result;
                    if (crl == null)
                    {
                        if (ResourceHelper.AppName == Apps.Cricket_Videos.ToString() && Task.Run(async () => await Constants.connection.Table<ShowList>().Where(k => k.ShowID == AppSettings.ShowUniqueID).FirstOrDefaultAsync()).Result.Status == "Custom")
                        {
                            role.PersonRole = "";
                        }
                    }
                    else
                        role.PersonRole = crl.Name;
                    role.TeamID = Convert.ToInt32(cast.CategoryID);
                    role.PersonImage = cp.FlickrPersonImageUrl;
                    // role.PersonImage = ResourceHelper.getPersonTileImage(cast.PersonID.ToString());
                    objCastList.Add(role);
                }
                return objCastList;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetCricketCast Method In CricketCast.cs file", ex);
            }
            return objCastList;
        }
       
    }
}
