using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Linq;
using OnlineVideos;
using Common.Library;
using System.Collections.Generic;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using Windows.UI.Xaml.Media.Imaging;

namespace OnlineVideos.Library
{
    public static class GalleryHelper
    {
       
        public static GalleryImageInfo GetGalleryImageList(long personID)
        {
            GalleryImageInfo imageInfo = new GalleryImageInfo();
            try
            {
                List<GalleryImageInfo> GalleryImageList = new List<GalleryImageInfo>();
                  PersonGallery profile = OnlineShow.GetPersonGallerypoup(personID, AppSettings.imageno);
                    imageInfo.FullImage = new BitmapImage(new Uri(profile.FlickrGalleryImage, UriKind.RelativeOrAbsolute));
                    imageInfo.Thumbnail = new BitmapImage(new Uri(profile.FlickrThumbNailImage, UriKind.RelativeOrAbsolute));
              
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetGalleryImageList Method In GalleryHelper.cs file", ex);
            }
            return imageInfo;
           }
                                                                    
            //}
      
        //}

        public static BitmapImage LoadGallaryimage(long personID, int galleryImageID)
        {
            return ResourceHelper.getGalleryImage(personID, galleryImageID);
        }
    }
}
