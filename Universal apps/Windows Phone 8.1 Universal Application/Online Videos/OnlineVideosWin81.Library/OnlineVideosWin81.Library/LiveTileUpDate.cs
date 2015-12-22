using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TileNotificationsPack;
using Windows.Data.Xml.Dom;
using Windows.Graphics.Display;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace OnlineVideos.Library
{
   public static class LiveTileUpDate
    {
        public async static void LiveTileForCycle()
        {
            List<ShowList> downloadrecentlist = null;
            int downloadHistory;
            int downloadHistory1;
            Constants.UIThread = true;
            downloadrecentlist = OnlineShow.GetRecentlyAddedShows().Items.ToList();
            List<ShowList> downloadUpcominglist = null;
            downloadUpcominglist = OnlineShow.GetUpComingMovies().Items.ToList();
            Constants.UIThread = false;
            List<Uri> cycleimagelist = new List<Uri>();
            List<string> wideimages = new List<string>();
            List<string> cycleimagelist1 = new List<string>();
            List<string> cycleimagetitle = new List<string>();
            
            downloadHistory = downloadrecentlist.Count();
            downloadHistory1 = downloadUpcominglist.Count();
            AppSettings.LiveTileHistroryCount = downloadrecentlist.Count;
            AppSettings.LiveTileHistroryCount = downloadUpcominglist.Count;
            if (Task.Run(async () => await Storage.FileExists("Historys.xml")).Result)
            {
                XDocument xdoc = await Storage.ReadFileAsDocument("Historys.xml");

                var data = from c in xdoc.Descendants("Historys") select c;
                OnlineVideos.Common.SyncAgentState.HistoryCount = data.Distinct().Count();
                AppSettings.LiveTileHistroryCount = data.Distinct().Count();
                Storage.DeleteFile("Historys.xml");
            }
            else
            {
                OnlineVideos.Common.SyncAgentState.HistoryCount = downloadHistory;
                AppSettings.LiveTileHistroryCount = downloadHistory;
            }
            foreach (var item in downloadrecentlist)
            {
                // string ImageUrl = await ResourceHelper.GetLiveTileUrl(item.TileImage);
                cycleimagetitle.Add(item.Title);
                Uri img = new Uri(item.TileImage, UriKind.RelativeOrAbsolute);
                cycleimagelist.Add(img);
                var path = getImageFromStorageOrInstalledFolder(item.TileImage);
                cycleimagelist1.Add(path);
                var path1 = getImageFromWideimages(item.TileImage);
                wideimages.Add(path1);
            }
            foreach(var item in downloadUpcominglist)
            {
                cycleimagetitle.Add(item.Title);
                Uri img = new Uri(item.TileImage, UriKind.RelativeOrAbsolute);
                cycleimagelist.Add(img);
                var path = getImageFromStorageOrInstalledFolder(item.TileImage);
                cycleimagelist1.Add(path);
                var path1 = getImageFromWideimages(item.TileImage);
                wideimages.Add(path1);
            }

            try
            {
                var updater = TileUpdateManager.CreateTileUpdaterForApplication();
                updater.EnableNotificationQueue(true);
                updater.Clear();

                for (int i = 0; i < 5; ++i)
                {
                    var tiles = new TileCollection()
                    {
                        new TileSquare150x150Image() {
                            Binding = new TileBinding() { Branding = TileBranding.name },
                            Image = new TileImage() { Src = cycleimagelist1[i], Alt = cycleimagelist1[i] }
                        },
                        new TileWide310x150Image(){
                            Binding = new TileBinding() { Branding = TileBranding.name },
                            Image = new TileImage() { Src = cycleimagelist1[i], Alt = cycleimagelist1[i] }                            
                        },
                        new TileSquare310x310Image() {
                            Binding = new TileBinding() { Branding = TileBranding.name },
                            Image = new TileImage() { Src = cycleimagelist1[i], Alt = cycleimagelist1[i] }                            
                        }                        
                    };
                    updater.Update(tiles);
                }
                
                #region GivingBadgeNumberToTile
                var badgeXML = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);
                var badge = badgeXML.SelectSingleNode("/badge") as XmlElement;
                badge.SetAttribute("value", AppSettings.LiveTileHistroryCount.ToString());
                var badgeNotification = new BadgeNotification(badgeXML);
                BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(badgeNotification);
                #endregion
            }
            catch (Exception ex)
            {
                if (Constants.LiveTileBackgroundAgentStatus == true)
                {
                    AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadSearch;
                }
            }
            AppSettings.LiveTileBackgroundAgentStatus = true;
            if (Constants.LiveTileBackgroundAgentStatus == true)
            {
                Exceptions.UpdateAgentLog("UpDateLiveTile Completed in LiveTileUpdate.cs ");
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadSearch;
            }
        }

        private static string getImageFromStorageOrInstalledFolder(string filename)
        {
            string FolderName = string.Empty;
            string FolderName1 = string.Empty;
            if (DisplayProperties.ResolutionScale == ResolutionScale.Scale100Percent)
            {
                FolderName = "scale-100";
            }
            else if (DisplayProperties.ResolutionScale == ResolutionScale.Scale140Percent)
            {
                FolderName = "scale-140";
            }
            else if (DisplayProperties.ResolutionScale == ResolutionScale.Scale180Percent)
            {
                FolderName = "scale-180";
            }
            else if (DisplayProperties.ResolutionScale == ResolutionScale.Scale150Percent)
            {
                FolderName1 = "150-150";
            }
            if (Task.Run(async () => await Storage.FileExists("Images\\" + filename)).Result)
            {
                return "ms-appdata:///local/Images/" + filename;
            }

            if (Task.Run(async () => await Storage.FileExists("Images\\" + FolderName + "\\" + filename)).Result)
            {
                return "ms-appdata:///local/Images/" + FolderName + "/" + filename;
            }
            if (Task.Run(async () => await Storage.FileExists("Images\\TileImages\\" + FolderName1 + "\\" + filename)).Result)
            {
                return "ms-appdata:///local/Images/TileImages" + "/" + FolderName1 + "/" + filename;
            }
            else
            {
                return "ms-appx:///Images/scale-100/" + filename;
            }
        }

        private static string getImageFromWideimages(string filename)
        {
            string FolderName = "310-150";

            if (Task.Run(async () => await Storage.FileExists("Images\\" + filename)).Result)
            {
                return "ms-appdata:///local/Images/" + filename;
            }

            if (Task.Run(async () => await Storage.FileExists("Images\\TileImages\\" + FolderName + "\\" + filename)).Result)
            {
                return "ms-appdata:///local/Images/TileImages/" + FolderName + "/" + filename;
            }
            else
            {
                return "ms-appx:///Images/TileImages/310-150/" + filename;
            }
        }
    }
}
