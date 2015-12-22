using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using Windows.UI.Notifications;
using OnlineVideos.TileContent;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using System.Threading.Tasks;
using Common.Library;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using Windows.Data.Xml.Dom;
using OnlineVideos.ToastContent;
using NotificationsExtensions.BadgeContent;


namespace OnlineVideos
{
    public sealed partial class SendLocalImageTile
    {
        public string ChecksmallLogo(string FileName)
        {

            if (Task.Run(async () => await Storage.FileExists("Images\\TileImages\\30-30\\" + FileName + ".jpg")).Result)
            {
                return "ms-appdata:///local/Images/TileImages/30-30/" + FileName + ".jpg";
            }
            else
            {
                return "ms-appx:///Images/TileImages/30-30/" + FileName + ".jpg";
            }
        }
        public string ChecksquareLogo(string FileName)
        {

            if (Task.Run(async () => await Storage.FileExists("Images\\TileImages\\150-150\\" + FileName + ".jpg")).Result)
            {
                return "ms-appdata:///local/Images/TileImages/150-150/" + FileName + ".jpg";
            }
            else
            {
                return "ms-appx:///Images/TileImages/150-150/" + FileName + ".jpg";
            }
        }

        public string CheckWideLogo(string FileName)
        {

            if (Task.Run(async () => await Storage.FileExists("Images\\TileImages\\310-150\\" + FileName + ".jpg")).Result)
            {
                return "ms-appdata:///local/Images/TileImages/310-150/" + FileName + ".jpg";
            }
            else
            {
                return "ms-appx:///Images/TileImages/310-150/" + FileName + ".jpg";
            }
        }
        public void CreateTileWithImage(string title,string filename)
        {
            try
            {
               
                ITileWideImageAndText01 tileContent = TileContentFactory.CreateTileWideImageAndText01();

                ITileSquareImage squareContent = TileContentFactory.CreateTileSquareImage();
            
                        tileContent.TextCaptionWrap.Text = title;

                    tileContent.Image.Src = CheckWideLogo(filename);
                    squareContent.Image.Src = ChecksquareLogo(filename);
                    tileContent.SquareContent = squareContent;
                    TileNotification tileNotification = tileContent.CreateNotification();
                   
                    TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
              
              
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in CreateTileWithImage Method In SendLocalImageTile.cs file", ex);
            }

           
        }
        public void UpdateTileWithImage()
        {
            try
            {
                int count = 0;
                int BadgeCount = 0;
                if (Task.Run(async () => await Storage.FileExists("Historys.xml")).Result)
                {
                   XElement xdoc;
                  xdoc = Storage.ReadFileElements("Historys.xml");
                    foreach (XElement element in xdoc.Elements())
                    {
                        count += 1;
                    }
                }
                 ITileWideImageAndText01 tileContent = TileContentFactory.CreateTileWideImageAndText01();
               
                ITileSquareImage squareContent = TileContentFactory.CreateTileSquareImage();
                List<ShowList> objShowList = new List<ShowList>();
                objShowList = OnlineShow.GetRecentLiveTileImages();
                
                foreach (ShowList objMovieList in objShowList)
                {
                    if (Task.Run(async () => await Storage.FileExists("Historys.xml")).Result)
                    {
                            tileContent.TextCaptionWrap.Text = objMovieList.Title;
                            BadgeCount = count;
                    }
                        
                    else
                     {
                            tileContent.TextCaptionWrap.Text = objMovieList.Title;
                            BadgeCount = objShowList.Count;
                     }
                       
                    
                    tileContent.Image.Src = CheckWideLogo(objMovieList.TileImage.Substring(0, objMovieList.TileImage.IndexOf('.')));
                    squareContent.Image.Src = ChecksquareLogo(objMovieList.TileImage.Substring(0, objMovieList.TileImage.IndexOf('.')));
                    tileContent.SquareContent = squareContent;
                    TileNotification tileNotification = tileContent.CreateNotification();
                    string tag = objMovieList.Title;
                    TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
                }
                BadgeNumericNotificationContent badgeContent = new BadgeNumericNotificationContent((uint)BadgeCount);
                
                // send the notification to the app's application tile with badge number
                BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(badgeContent.CreateNotification());
                if (Task.Run(async () => await Storage.FileExists("Historys.xml")).Result)
                    Storage.DeleteFile("Historys.xml");

               
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in UpdateTileWithImage Method In SendLocalImageTile.cs file", ex);
                if (Constants.LiveTileBackgroundAgentStatus == true)
                {
                  
                    AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadSearch;
                }
            }

            if (Constants.LiveTileBackgroundAgentStatus == true)
            {
                Exceptions.UpdateAgentLog(" UpDateLiveTile  Completed In SendLocalImageTile.cs ");
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadSearch;
            }
        }

        private void ClearTile()
        {
            try
            {
                TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ClearTile Method In SendLocalImageTile.cs file", ex);
            }
        }
    }
}
