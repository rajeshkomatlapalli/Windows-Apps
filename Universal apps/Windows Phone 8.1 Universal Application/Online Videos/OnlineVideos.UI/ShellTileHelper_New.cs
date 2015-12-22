using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.UI.Xaml.Media.Imaging;

namespace OnlineVideos.UI
{
   public class ShellTileHelper_New
    {
       public static SecondaryTile _tile;
       //Check Wether tile is created or not for particular ID
        public static bool IsPinned(string uniqueId)
        {
            if(SecondaryTile.Exists(uniqueId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }       

        public async static void CreatePin(string ShowID)
        {
            ShowList ojsShow = new ShowList();
            ojsShow = OnlineShow.GetShowDetail(Convert.ToInt32(AppSettings.ShowID));
            SecondaryTile initialData = new SecondaryTile();          
                initialData = new SecondaryTile(
                    ShowID,
                    ojsShow.Title,
                    "NoArguments",
                    new Uri("ms-appx:///Images/" + ojsShow.TileImage),
                    TileSize.Square150x150);
                initialData.VisualElements.ShowNameOnSquare150x150Logo = true;

                await initialData.RequestCreateAsync();
        }

       //Pin Tile to start menu with id
        public async static void Pin(SecondaryTile initialData)
        {
            if(!SecondaryTile.Exists(initialData.TileId))
            {
               await initialData.RequestCreateAsync();              
            }
        }

       //Delete the tile added to start menu by ID
       public async static void UnPin(string ShowID)
        {
           if(SecondaryTile.Exists(ShowID))
           {
               SecondaryTile UnpinTile = new SecondaryTile(ShowID);
               bool isUnpinned= await UnpinTile.RequestDeleteAsync();
           }
        }

        public static void PinMovieToStartScreen(string ShowID)
        {
            try
            {
                ShowList ojsShow = new ShowList();
                ojsShow = OnlineShow.GetShowDetail(Convert.ToInt32(AppSettings.ShowID));
                SecondaryTile initialData = new SecondaryTile();

                string uri = "ms-appx:///Images/" + ojsShow.TileImage;

                if (ShellTileHelper_New.IsPinned(ojsShow.Title) == true)
                {
                    ShellTileHelper_New.UnPin(ojsShow.Title);
                }
                else
                {                    
                    initialData = new SecondaryTile(
                        ShowID,
                        ojsShow.Title,
                        "NoArguments",
                        new Uri("ms-appx:///Images/" + ojsShow.TileImage),
                        TileSize.Square150x150);
                    initialData.VisualElements.ShowNameOnSquare150x150Logo = true;

                    ShellTileHelper_New.Pin(initialData);
                }
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in PinMovieToStartScreen Method In ShellTileHelper.cs file", ex);
            }
        }
        
        public static void PinVideoLinkToStartScreen(string ShowID, ShowLinks showLink)
        {
            try
            {
                ShowList ojsShow = new ShowList();
                ojsShow = OnlineShow.GetShowDetail(Convert.ToInt32(AppSettings.ShowID));
                string name = ShowID + showLink.Title;
                string ID = Regex.Replace(name, @"\s+", "");
                SecondaryTile initialData = new SecondaryTile();

                if (ShellTileHelper_New.IsPinned(ID) == true)
                {
                    ShellTileHelper_New.UnPin(ID);
                }
                else
                {
                    
                    //initialData = new SecondaryTile(
                    //    ID,
                    //    ojsShow.Title+"-"+showLink.Title,
                    //    "NoA",
                    //    new Uri("ms-appx:///Images/" + ojsShow.TileImage),
                    //    TileSize.Square150x150);
                    //initialData.VisualElements.ShowNameOnSquare150x150Logo = true;

                    Uri squareTileUri = new Uri("ms-appx:///Images/" + ojsShow.TileImage);
                    Uri wideTileUri = new Uri("ms-appx:///Images/" + ojsShow.TileImage);

                    _tile = new SecondaryTile(ID,
                                                    ojsShow.Title,
                                                    ojsShow.Title,
                                                    ojsShow.Title,
                                                    TileOptions.ShowNameOnWideLogo | TileOptions.ShowNameOnLogo,
                                                    squareTileUri,
                                                    wideTileUri);

                    ShellTileHelper_New.Pin(_tile);
                    
                    TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
                    var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150PeekImageAndText01);

                    var tileImage = tileXml.GetElementsByTagName("image")[0] as XmlElement;
                    tileImage.SetAttribute("src", "ms-appx:///Images/" + ojsShow.TileImage);
                    var tileText = tileXml.GetElementsByTagName("text");
                    (tileText[0] as XmlElement).InnerText = ojsShow.Title + "-" + showLink.Title;
                    var tileNotification11 = new TileNotification(tileXml);
                    TileUpdateManager.CreateTileUpdaterForSecondaryTile(_tile.TileId).Update(tileNotification11);
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in PinMovieToStartScreen Method In ShellTileHelper.cs file", ex);
            }
        }

    }
}
