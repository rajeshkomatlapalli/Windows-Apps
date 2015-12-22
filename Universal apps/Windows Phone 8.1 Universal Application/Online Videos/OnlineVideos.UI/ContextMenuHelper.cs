using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Common.Library;
//using OnlineVideos.Common;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.StartScreen;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
//using Common.Common;

namespace OnlineVideos.UI
{
    public static class ContextMenuHelper
    {
        public static void PinSongAsLiveTile(string ShowID, string ShowTitle, string ShowImage, string SongTitle)
        {
            //StandardTileData TileInfo = new StandardTileData
            //{
            //    BackgroundImage = new Uri("isostore:/Shared/ShellContent/secondary/" + ShowImage, UriKind.RelativeOrAbsolute),
            //    Title = SongTitle,
            //    BackContent = ShowTitle,

            //};
            XmlDocument TileInfo = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareBlock);
            XmlNodeList tileTextAttributes = TileInfo.GetElementsByTagName("text");
            tileTextAttributes[0].InnerText = "SongTitle";
            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareImage);
            XmlNodeList tileImageAttributes = tileXml.GetElementsByTagName("image");
            ((XmlElement)tileImageAttributes[0]).SetAttribute("isostore:/Shared/ShellContent/secondary/", ShowImage);

            Uri uri = new Uri("/Views/SongDetails.xaml?id=" + ShowID + "&chapter=" + ShowTitle + "&id+cno=" + SongTitle, UriKind.Relative);
            //ShellTileHelper.Pin(uri, TileInfo);
        }
        public static void ContextMenu_Opened(object sender)
        {           
            //ContextMenu mainmenu = sender as ContextMenu;
            MenuFlyout mainmenu = sender as MenuFlyout;
            foreach (MenuFlyoutItem contextMenuItem in mainmenu.Items)
            {
                if (contextMenuItem.Name == "Pin")
                {
                    //if (ShellTileHelper.IsPinned(AppSettings.ShowLinkTitle))
                    //{
                    //    contextMenuItem.Text = Constants.PinToStartScreen;
                    //}
                    //else
                    //{
                    //    contextMenuItem.Text = Constants.UnpinFromStartScreen;
                    //}
                }
                if (contextMenuItem.Name == "Rating")
                {
                    contextMenuItem.Text = AppResources.AllowRatingLinkContextMenuLabel;
                }

            }
        }
    }
}
