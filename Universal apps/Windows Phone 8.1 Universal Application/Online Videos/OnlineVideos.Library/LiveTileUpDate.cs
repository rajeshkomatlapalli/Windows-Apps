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
            Constants.UIThread = true;
            downloadrecentlist = OnlineShow.GetRecentlyAddedShows().Items.ToList();
            Constants.UIThread = false;
            List<Uri> cycleimagelist = new List<Uri>();
            List<string> isolatelist = new List<string>();
            List<string> cycleimagelist1 = new List<string>();
            downloadHistory = downloadrecentlist.Count();
            if (Task.Run(async () => await Storage.FileExists("Historys.xml")).Result)
            {
                XDocument xdoc = await Storage.ReadFileAsDocument("Historys.xml");

                var data = from c in xdoc.Descendants("Historys") select c;
                OnlineVideos.Common.SyncAgentState.HistoryCount = data.Distinct().Count();
                AppSettings.LiveTileHistroryCount = data.Distinct().Count();
                downloadHistory = data.Distinct().Count();
            }
            else
            {
                OnlineVideos.Common.SyncAgentState.HistoryCount = downloadHistory;
                AppSettings.LiveTileHistroryCount = downloadHistory;
            }
            foreach (var item in downloadrecentlist)
            {
                if (ResourceHelper.AppName == Apps.Video_Mix.ToString())
                {
                    string ImageUrl = getpath(item.TileImage);
                    isolatelist.Add(ImageUrl);
                }
                else
                {
                    Uri img = new Uri(item.TileImage, UriKind.RelativeOrAbsolute);
                    cycleimagelist.Add(img);
                    cycleimagelist1.Add(item.TileImage);
                }
            }
            try
            {
                if (ResourceHelper.AppName == Apps.Video_Mix.ToString())
                {
                    if (isolatelist.Count >= 1)
                    {
                        var updater1 = TileUpdateManager.CreateTileUpdaterForApplication();
                        updater1.EnableNotificationQueue(true);
                        updater1.Clear();

                        for (int i = 0; i < isolatelist.Count; ++i)
                        {
                            var tiles = new TileCollection()
                    {
                        new TileSquare150x150Image() {
                            Binding = new TileBinding() { Branding = TileBranding.name },
                            Image = new TileImage() { Src = isolatelist[i], Alt = isolatelist[i] }
                        },
                        new TileWide310x150Image(){
                            Binding = new TileBinding() { Branding = TileBranding.name },
                            Image = new TileImage() { Src = isolatelist[i], Alt =  isolatelist[i] }
                        },
                    };
                            updater1.Update(tiles);
                        }

                        #region GivingBadgeNumberToTile
                        var badgeXML1 = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);
                        var badge1 = badgeXML1.SelectSingleNode("/badge") as XmlElement;
                        badge1.SetAttribute("value", downloadHistory.ToString());
                        var badgeNotification1 = new BadgeNotification(badgeXML1);
                        BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(badgeNotification1);
                        #endregion
                    }
                }
                else
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
                            Image = new TileImage() { Src ="ms-appx:///Images/" + cycleimagelist1[i], Alt ="ms-appx:///Images/" + cycleimagelist1[i] }
                        },
                        new TileWide310x150ImageAndText01(){
                            Image = new TileImage() { Src ="ms-appx:///Images/" + cycleimagelist1[i], Alt ="ms-appx:///Images/" + cycleimagelist1[i] },
                            Text = string.Format(AppSettings.AppName)
                        },
                    };
                        updater.Update(tiles);
                    }

                    #region GivingBadgeNumberToTile
                    var badgeXML = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);
                    var badge = badgeXML.SelectSingleNode("/badge") as XmlElement;
                    badge.SetAttribute("value", downloadHistory.ToString());
                    var badgeNotification = new BadgeNotification(badgeXML);
                    BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(badgeNotification);
                    #endregion
                }
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
        public static string getpath(string filename)
        {
            if (Task.Run(async () => await Storage.FileExists("Images\\" + filename)).Result)
            {
                //string file= filename.Replace("\\", "/");
                //return "ms-appdata:///local/Images/" + file;
                return "ms-appdata:///local/Images/" + filename;
            }
            else
            {
                return null;
            }
        }
    }
}