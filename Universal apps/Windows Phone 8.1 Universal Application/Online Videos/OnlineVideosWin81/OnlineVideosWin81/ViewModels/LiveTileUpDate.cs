using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace OnlineVideos
{
    public static class LiveTileUpDate
    {
        public async static void LiveTileForCycle()
        {
            List<ShowList> downloadrecentlist = null;
            int downloadHistory;
            //Constants.UIThread = true;
            downloadrecentlist = OnlineShow.GetRecentlyAddedShows().Items.ToList();
            //Constants.UIThread = false;
            List<Uri> cycleimagelist = new List<Uri>();
            List<string> cycleimagelist1 = new List<string>();
            downloadHistory = downloadrecentlist.Count();
            if (Task.Run(async () => await Storage.FileExists("Historys.xml")).Result)
            {
                XDocument xdoc = await Storage.ReadFileAsDocument("Historys.xml");

                var data = from c in xdoc.Descendants("Historys") select c;
                //OnlineVideos.Common.SyncAgentState.HistoryCount = data.Distinct().Count();
                AppSettings.LiveTileHistroryCount = data.Distinct().Count();
            }
            else
            {
                //OnlineVideos.Common.SyncAgentState.HistoryCount = downloadHistory;
                AppSettings.LiveTileHistroryCount = downloadHistory;
            }
            foreach (var item in downloadrecentlist)
            {
                string ImageUrl = await ResourceHelper.GetLiveTileUrl("scale-100/"+item.TileImage);
                Uri img = new Uri(ImageUrl, UriKind.RelativeOrAbsolute);
                cycleimagelist.Add(img);
                cycleimagelist1.Add(ImageUrl);
            }
            try
            {
                TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                BadgeUpdateManager.CreateBadgeUpdaterForApplication().Clear();
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);

                #region TileSquare150x150Image
                var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Image);
                var tileImage = tileXml.GetElementsByTagName("image")[0] as XmlElement;

                tileImage.SetAttribute("src", "ms-appx://" + cycleimagelist1[0].ToString());
                var tileNotification = new TileNotification(tileXml);
                tileNotification.Tag = "1";
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);

                tileImage.SetAttribute("src", "ms-appx://" + cycleimagelist1[1].ToString());
                var tileNotification1 = new TileNotification(tileXml);
                tileNotification1.Tag = "2";
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification1);

                tileImage.SetAttribute("src", "ms-appx://" + cycleimagelist1[2].ToString());
                var tileNotification2 = new TileNotification(tileXml);
                tileNotification2.Tag = "3";
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification2);

                //tileImage.SetAttribute("src", "ms-appx://" + cycleimagelist1[3].ToString());
                //var tileNotification3 = new TileNotification(tileXml);
                //tileNotification3.Tag = "4";
                //TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification3);
                #endregion


                #region TileWide310x150Image
                var tileXml1 = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Image);
                var tileImage1 = tileXml1.GetElementsByTagName("image")[0] as XmlElement;
                tileImage1.SetAttribute("src", "ms-appx://" + cycleimagelist1[3].ToString());
                var tileNotification5 = new TileNotification(tileXml1);
                tileNotification5.Tag = "5";
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification5);

                tileImage1.SetAttribute("src", "ms-appx://" + cycleimagelist1[4].ToString());
                var tileNotification6 = new TileNotification(tileXml1);
                tileNotification6.Tag = "6";
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification6);

                //tileImage1.SetAttribute("src", "ms-appx://" + cycleimagelist1[2].ToString());
                //var tileNotification7 = new TileNotification(tileXml1);
                //tileNotification7.Tag = "7";
                //TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification7);

                //tileImage1.SetAttribute("src", "ms-appx://" + cycleimagelist1[3].ToString());
                //var tileNotification8 = new TileNotification(tileXml1);
                //tileNotification8.Tag = "8";
                //TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification8);

                //tileImage1.SetAttribute("src", "ms-appx://" + cycleimagelist1[4].ToString());
                //var tileNotification9 = new TileNotification(tileXml1);
                //tileNotification9.Tag = "9";
                //TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification9);

                //tileImage1.SetAttribute("src", "ms-appx://" + cycleimagelist1[5].ToString());
                //var tileNotification10 = new TileNotification(tileXml1);
                //tileNotification10.Tag = "10";
                //TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification10);
                #endregion               
            }
            catch (Exception ex)
            {
                if (Constants.LiveTileBackgroundAgentStatus == true)
                {
                    AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadSearch;
                }
            }
            if (downloadrecentlist.Count != 0)
            {
                AppSettings.LiveTileBackgroundAgentStatus = true;
                if (Constants.LiveTileBackgroundAgentStatus == true)
                {
                    Exceptions.UpdateAgentLog("UpDateLiveTile Completed in LiveTileUpdate.cs ");
                    AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadSearch;
                }
            }
        }
    }
}
