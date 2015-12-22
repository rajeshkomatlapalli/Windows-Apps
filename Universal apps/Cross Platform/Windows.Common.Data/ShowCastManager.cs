using System;
using System.Net;
using System.Windows;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using OnlineVideos.Entities;
using Common.Library;
#if WINDOWS_APP
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
#endif
using System.Threading;
using System.Threading.Tasks;
using SQLite;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
#if WP8 &&   NOTANDROID
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
#endif
namespace OnlineVideos.Data
{
    public static class ShowCastManager
    {
        public static int cnt;
         #if NOTANDROID
        public static ImageSource loadBitmapImageInBackground(string imagefile, UserControl thread, AutoResetEvent aa)
        {
            BitmapImage image = null;
#if WINDOWS_APP
            thread.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {

                image = new BitmapImage(new Uri(imagefile, UriKind.RelativeOrAbsolute));
                aa.Set();
            });
            aa.WaitOne();
#endif
            return image;
        }
#endif
        public static List<CastRole> GetCastSection(string id)
        {
            List<CastRole> objCastList = new List<CastRole>();
            try
            {
                int showid=Convert.ToInt32(id);
                List<ShowCast> objCastDetails = new List<ShowCast>();
                objCastDetails = Task.Run(async () => await Constants.connection.Table<ShowCast>().Where(i => i.ShowID == showid).ToListAsync()).Result;
                foreach (ShowCast cast in objCastDetails)
                {
                    CastRole role = new CastRole();
                    CastProfile cp=new CastProfile();
                    CastRoles crl = new CastRoles();
                    role.PersonID = cast.PersonID;
                    cp = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(j => j.PersonID == cast.PersonID).FirstOrDefaultAsync()).Result;
                    if (cp != null)
                    {
                        role.PersonName = cp.Name;
                        if (cast.RoleID != null)
                            crl = Task.Run(async () => await Constants.connection.Table<CastRoles>().Where(k => k.RoleID == cast.RoleID).FirstOrDefaultAsync()).Result;
                        if (crl != null)
                        role.PersonRole = crl.Name;
                        role.PersonImage = cp.FlickrPersonImageUrl;
#if ANDROID && NOTIOS
				try {
					HttpWebRequest request = (HttpWebRequest)WebRequest.Create(cp.FlickrPersonImageUrl);
					request.Method ="Get";
					HttpWebResponse response1 = (HttpWebResponse)Task.Run (async() => await request.GetResponseAsync ()).Result;
					System.IO.Stream str = response1.GetResponseStream ();
					#if ANDROID && NOTIOS
					role.PersonBitmapImage = Android.Graphics.BitmapFactory.DecodeStream (str);
					#endif

				} catch (Exception ex) {

				}
#endif
                        // role.PersonImage = ResourceHelper.getPersonTileImage(cast.PersonID.ToString());
                        objCastList.Add(role);
                    }
                }

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetCastSection Method In ShowCastManger.cs file", ex);
            }
            return objCastList;
        }

        public static List<ShowList> GetPersonRelatedShows(string pid)
        {
            List<ShowList> objfilmography = new List<ShowList>();
            List<ShowCast> objpersonshows = new List<ShowCast>();
            List<ShowList> objMovieList = new List<ShowList>();
            try
            {
                {
                    List<int> objcastList = new List<int>();
                    int pid1=Convert.ToInt32(pid);
                  List<ShowCast>shc = new List<ShowCast>();
                    shc = Task.Run(async () => await Constants.connection.Table<ShowCast>().Where(i => i.PersonID == pid1).ToListAsync()).Result;
                    objcastList = shc.Select(k => k.ShowID).Distinct().ToList();
                    foreach (int objcast in objcastList.OrderByDescending(i => i))
                    {
                        DataManager<ShowList> datamanager1 = new DataManager<ShowList>();
                        ShowList objdetail = new ShowList();
                        objdetail = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == objcast).FirstOrDefaultAsync()).Result;
                        ShowList objlist = new ShowList();
                         #if NOTANDROID
                        objdetail.LandingImage = ResourceHelper.getViewAllImageFromStorageOrInstalledFolder(objdetail.TileImage);
#endif
                        objdetail.RatingBitmapImage = ImageHelper.LoadRatingImage(objdetail.Rating.ToString()).ToString();
                        objdetail.SubTitle = "Subtitle: " + objdetail.SubTitle;

					  string releasedate = objdetail.ReleaseDate;
					  string Year = releasedate.Substring(releasedate.LastIndexOf(' ') + 1);
					  objdetail.ReleaseDate = Year;

                      objfilmography.Add(objdetail);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPersonRelatedShows Method In ShowCastManger.cs file", ex);
            }
            return objfilmography;
        }

        public static CastProfile GetPersonDetail(string pid)
        {
            CastProfile objLinkList = new CastProfile();
            try
            {
                int pid1=Convert.ToInt32(pid);
                objLinkList = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID == pid1).FirstOrDefaultAsync()).Result;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetPersonDetail Method In ShowCastManger.cs file", ex);
            }
            return objLinkList;
        }

        public static bool ShowGameControl(string ShowId)
        {
          int showid=int.Parse(ShowId);
            List<int> objLinkList = new List<int>();
            List<ShowCast>castlist=new List<ShowCast>();
            castlist = Task.Run(async () => await Constants.connection.Table<ShowCast>().Where(i => i.ShowID == showid).ToListAsync()).Result;
            objLinkList = castlist.Select(j => j.PersonID).Distinct().ToList();
            if (objLinkList.Count >= 6)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool ShowGameControlForCricket(string ShowId)
        {
            int showid = int.Parse(ShowId);
            List<int> objLinkList = new List<int>();
            List<string> flickrimgs = new List<string>();
            List<ShowCast> castlist = new List<ShowCast>();
            castlist = Task.Run(async () => await Constants.connection.Table<ShowCast>().Where(i => i.ShowID == showid).ToListAsync()).Result;
            objLinkList = castlist.Select(j => j.PersonID).Distinct().ToList();
            foreach (int cc in objLinkList)
            {
                string flickrimage = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID == cc).FirstOrDefaultAsync()).Result.FlickrPersonImageUrl;
                if (!string.IsNullOrEmpty(flickrimage))
                    flickrimgs.Add(flickrimage);
            }
            if (objLinkList.Count >= 6 && flickrimgs.Count >= 6)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static List<GalleryImageInfo> GetGalleryImageList(string pid)
        {
            List<GalleryImageInfo> objgallerys = new List<GalleryImageInfo>();
            try
            {
                List<PersonGallery> objgallList = new List<PersonGallery>();
                int pid1=Convert.ToInt32(pid);
			    List<int>tglist=new List<int>();
                objgallList = Task.Run(async () => await Constants.connection.Table<PersonGallery>().Where(i => i.PersonID == pid1).OrderByDescending(j => j.PersonID).ToListAsync()).Result;
                foreach (PersonGallery imcnt in objgallList)
                {
                    GalleryImageInfo objgalprop = new GalleryImageInfo();
                    objgalprop.ThumbnailImage = imcnt.FlickrThumbNailImage;
                    objgalprop.Gallcount = imcnt.ImageNo;
                    objgalprop.Title = imcnt.ImageNo + ".jpg";
					objgalprop.PersonID=imcnt.PersonID;
				#if ANDROID
				try {
					HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imcnt.FlickrThumbNailImage);
					request.Method ="Get";
					HttpWebResponse response1 = (HttpWebResponse)Task.Run (async() => await request.GetResponseAsync ()).Result;
					System.IO.Stream str = response1.GetResponseStream ();
				#if ANDROID && NOTIOS
					objgalprop.PersonBitmapImage = Android.Graphics.BitmapFactory.DecodeStream (str);

				#endif
				} catch (Exception ex) {

				}
				#endif
				#if NOTANDROID
				objgallerys.Add(objgalprop);
				#endif

				#if ANDROID
				if(!tglist.Contains(imcnt.ImageNo))
				{
					tglist.Add(imcnt.ImageNo);
					objgallerys.Add(objgalprop);
				}
				#endif
                    
                }
            
            }

            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetGalleryImageList Method In ShowCastManger.cs file", ex);
            }

            return objgallerys;
        }

        public static List<GalleryImageInfo> Loadpopupimages(string pid)
        {
            List<GalleryImageInfo> objgallerys = new List<GalleryImageInfo>();
            try
            {
                List<PersonGallery> objgallList = new List<PersonGallery>();
                int pid1 = Convert.ToInt32(pid);
                objgallList = Task.Run(async () => await Constants.connection.Table<PersonGallery>().Where(i => i.PersonID == pid1).OrderByDescending(j => j.PersonID).ToListAsync()).Result;
                foreach (PersonGallery imcnt in objgallList)
                {
                    GalleryImageInfo objgalprop = new GalleryImageInfo();
                     #if NOTANDROID
                    objgalprop.Thumbnail = new BitmapImage(new Uri(imcnt.FlickrGalleryImage, UriKind.RelativeOrAbsolute));
#endif
				#if ANDROID && NOTIOS
                    objgalprop.FullImage=imcnt.FlickrGalleryImage;
#endif
                    objgallerys.Add(objgalprop);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Loadpopupimages Method In ShowCastManger.cs file", ex);
            }
            return objgallerys;
        }

        public static List<CastRole> GetGameCastSection(string id)
        {
            List<CastRole> objCastList = new List<CastRole>();
            try
            {
                int showid = Convert.ToInt32(id);
                List<ShowCast> objCastDetails = new List<ShowCast>();
                
                objCastDetails = Task.Run(async () => await Constants.connection.Table<ShowCast>().Where(i => i.ShowID == showid).OrderBy(j => j.ShowID).ToListAsync()).Result;
                foreach (ShowCast cast in objCastDetails)
                {
                    CastRole role = new CastRole();
                    CastProfile cp = new CastProfile();
                    role.PersonID = cast.PersonID;
                    cp = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID == cast.PersonID).FirstOrDefaultAsync()).Result;
                    role.PersonRole = cp.Name;
                    role.PersonImage =cp.FlickrPersonImageUrl;
                    objCastList.Add(role);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetGameCastSection Method In ShowCastManger.cs file", ex);
            }
            return objCastList;
        }

        public static List<CastRole> GetCastSearchSection(string SearchText)
        {
            List<CastRole> objCastList = new List<CastRole>();
            try
            {
                List<CastProfile> objCastDetails = new List<CastProfile>();
                
                objCastDetails = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.Name.Contains(SearchText)).OrderBy(j => j.PersonID).ToListAsync()).Result;
                foreach (CastProfile cast in objCastDetails)
                {
                   
                    CastRole role = new CastRole();
                    CastProfile cp = new CastProfile();
                    CastRoles crl = new CastRoles();
                    role.PersonID = cast.PersonID;
                    cp = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(j => j.PersonID == cast.PersonID).FirstOrDefaultAsync()).Result;
                    role.PersonName = cp.Name;
                    role.PersonImage = cp.FlickrPersonImageUrl;
                    objCastList.Add(role);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetCastSearchSection Method In ShowCastManger.cs file", ex);
            }
            return objCastList;
        }

        //Code for Wp8

        public static bool ShowGamePivot(string ShowId)
        {

            int showid = Convert.ToInt32(ShowId);
           // var CastList = Task.Run(async () => await Constants.connection.Table<ShowCast>().Where(i => i.ShowID == showid).ToListAsync()).Result;
            List<CastRole> CastList = null;

            CastList = ShowCastManager.GetCastSection(AppSettings.ShowUniqueID.ToString());
            if (CastList != null)
            {
                if (CastList.Count() < 6)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        public static bool ShowGamePivotForCricket(string ShowId)
        {
            List<string> flickrimgs = new List<string>();
            int showid = Convert.ToInt32(ShowId);
            var CastList = Task.Run(async () => await Constants.connection.Table<ShowCast>().Where(i => i.ShowID == showid).ToListAsync()).Result;
            foreach (ShowCast cc in CastList)
            {
                string flickrimage = Task.Run(async () => await Constants.connection.Table<CastProfile>().Where(i => i.PersonID == cc.PersonID).FirstOrDefaultAsync()).Result.FlickrPersonImageUrl;
                if (!string.IsNullOrEmpty(flickrimage))
                    flickrimgs.Add(flickrimage);
            }
            if (CastList.Count() < 6 || flickrimgs.Count() < 6)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
