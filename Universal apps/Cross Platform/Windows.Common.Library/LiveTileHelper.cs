using System;
using System.Net;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;
//using Microsoft.Phone.Shell;
//using System.IO;
//using System.IO.IsolatedStorage;
using System.Linq;
using System.Xml.Linq;

namespace Common.Library
{
    public static class LiveTileHelper
    {
        //public static void UpdateFromXmlFile(string AppName)
        //{
        //    IsolatedStorageFile isostore = IsolatedStorageFile.GetUserStoreForApplication();
        //    IsolatedStorageFileStream isostream = null;
        //    XDocument xdoc = new XDocument();

        //    if (!isostore.FileExists(AppName + "/History.xml"))
        //    {
        //        if (isostore.FileExists(AppName + "/Tile.xml"))
        //        {
        //            isostream = new IsolatedStorageFileStream(AppName + "/Tile.xml", FileMode.Open, isostore);
        //            xdoc = XDocument.Load(isostream);
        //            isostream.Close();
        //            var TileNodes = from TileInfo in xdoc.Descendants("Tile") select TileInfo;
        //            if (TileNodes.Count() > 0)
        //            {
        //                if (SettingsHelper.Contains(AppName + "CurrentMovieId"))
        //                {
        //                    if (!string.IsNullOrEmpty(SettingsHelper.getStringValue(AppName + "CurrentMovieId")))
        //                    {
        //                        if (TileNodes.Count() == SettingsHelper.getIntValue(AppName + "CurrentMovieId"))
        //                        {
        //                            SettingsHelper.Save(AppName + "CurrentMovieId", 1);
        //                        }
        //                        else
        //                        {
        //                            SettingsHelper.Save(AppName + "CurrentMovieId", SettingsHelper.getIntValue(AppName + "CurrentMovieId") + 1);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    SettingsHelper.Save(AppName + "CurrentMovieId", 1);
        //                }

        //                long? nCurrentShowID = SettingsHelper.getIntValue(AppName + "CurrentMovieId");

        //                if (nCurrentShowID.HasValue)
        //                {
        //                    var data = from c in xdoc.Descendants("Tile") where c.Attribute("Id").Value == nCurrentShowID.Value.ToString() select c;

        //                    ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault();
        //                    if (tile != null)
        //                    {
        //                        StandardTileData initialData = new StandardTileData();

        //                        foreach (var tiledata in data)
        //                        {
        //                            initialData = new StandardTileData
        //                            {
        //                                BackgroundImage = new Uri("isostore:/Shared/ShellContent/" + AppName + "/Primary/ApplicationLiveTile.png", UriKind.RelativeOrAbsolute),
        //                                BackBackgroundImage = new Uri(tiledata.Element("Image").Value, UriKind.RelativeOrAbsolute),
        //                            };
        //                            tile.Update(initialData);
        //                            break;
        //                        } //end foreach tiledata
        //                    }//end if(tile != null)
        //                }//if (nCurrentMovieID.HasValue)
        //            }
        //        }
        //    }
        //}
    }
}
