
using System;
using System.Net;
using System.Windows;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;
//using System.IO.IsolatedStorage;
//using System.Windows.Media.Imaging;
//using Microsoft.Phone.Scheduler;
using System.IO;
using Common.Library;
using System.Threading;
using OnlineVideos.Library;
using OnlineVideos.Common;
using OnlineVideos.Data;
using System.Collections.Generic;
using OnlineVideos.Entities;
using Windows.Storage;
using Windows.Storage.Streams;
//using Common.Common;

namespace OnlineVideos.Library
{
    public class PrimaryTileUpdate
    {
    
        string tid;
        string imgname = "";
        string Title = "";
        List<ShowList> shows = null;
        ShowList TableQuery;
        //IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
        StorageFolder isoStore = ApplicationData.Current.LocalFolder;
        IRandomAccessStream isoStream = null;
        //IsolatedStorageFileStream isoStream = null;
        XDocument xdoc = null;
        //private IsolatedStorageFileStream isostream;
        private IRandomAccessStream isostream;
        ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
        //IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
       OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);

        private static void HandleException(Exception ex, string message)
        {
            Exceptions.SaveOrSendExceptions(message + "\n\n" + ex.Message + " \n\n Stack Trace \n" + ex.StackTrace, ex);
            if (SyncAgentState.SyncAgent != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    ((IDowloadCompleteCallback)(SyncAgentState.SyncAgent)).OnStartEvent();
                });
            }
        }

        public void CreateTile(string[] id)
        {
            try
            {
                if (SettingsHelper.Contains("LastPhoneUpdatedDate"))
                {
                    if (AppSettings.LastPhoneUpdatedDate != AppSettings.LartTileDate)
                    {
                        if (Storage.FileExists(ResourceHelper.ProjectName + "/Tile.xml"))
                        {
                            isostream = new IsolatedStorageFileStream(ResourceHelper.ProjectName + "/Tile.xml", FileMode.Open, isoStore);
                            xdoc = XDocument.Load(isostream);
                            isostream.Close();
                            var delrec = (from i in xdoc.Descendants("Tile") select i).ToList();

                            foreach (var d in delrec)
                            {
                                string Imgname = d.Element("Image").Value.ToString().Substring(d.Element("Image").Value.ToString().LastIndexOf('/') + 1);
                                Storage.DeleteFile(Constants.SecondaryTileImagePath + Imgname);
                            }
                        }
                        if (Storage.FileExists(ResourceHelper.ProjectName + "/Tile.xml"))
                            Storage.DeleteFile(ResourceHelper.ProjectName + "/Tile.xml");

                        if (SettingsHelper.Contains(ResourceHelper.ProjectName + "CurrentMovieId"))
                        {
                            SettingsHelper.Remove(ResourceHelper.ProjectName + "CurrentMovieId");
                        }

                    }
                }
                foreach (string s in id)
                {
                    var xquery = (from p in context.ShowList where p.ShowID == Convert.ToInt32(s) select p);
                    foreach (var query in xquery)
                    {
                      
                        TableQuery = query;
                        imgname = query.TileImage;
                        Title = query.Title;

                          
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                Constants.UIThread = true;
                                BitmapImage im = new BitmapImage();
                                im.CreateOptions = BitmapCreateOptions.None;
                                im = ResourceHelper.getShowTileImage(query.TileImage) as BitmapImage;
                                ResizeImage(im);
                                Constants.UIThread = false;
                            });
                           
                            if (SyncAgentState.SyncAgent != null)
                            {
                                Deployment.Current.Dispatcher.BeginInvoke(() =>
                               {
                                   ((IDowloadCompleteCallback)(SyncAgentState.SyncAgent)).OnStartEvent();
                               });
                            
                            }
                            SyncAgentState.auto.WaitOne();
                        
                    }
                }
                Exceptions.UpdateAgentLog("CreateTile Completed In PrimaryTileUpdate");

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in CreateTile Method In Vidoes file", ex);
            }
        }

        private void AddToXml(ShowList query)
        {
            if (isoStore.FileExists(ResourceHelper.ProjectName + "/Tile.xml"))
            {
               
                isostream = new IsolatedStorageFileStream(ResourceHelper.ProjectName + "/Tile.xml", FileMode.Open, isoStore);
                xdoc = XDocument.Load(isostream);
                isostream.Close();
                var tiledata = from c in xdoc.Descendants("Tile") orderby Convert.ToInt32(c.Attribute("Id").Value.ToString()) descending select c;

                foreach (var tile in tiledata)
                {
                    tid = (Convert.ToInt32(tile.Attribute("Id").Value) + 1).ToString();
                    break;
                }
                if (Storage.FileExists(Constants.SecondaryTileImagePath + query.TileImage))
                {
                    isostream = new IsolatedStorageFileStream(ResourceHelper.ProjectName + "/Tile.xml", FileMode.Open, isoStore);
                    xdoc = XDocument.Load(isostream);
                    isostream.Close();
                    var CompareNode = from v in xdoc.Descendants("Tile") where (v.Element("Image").Value == "isostore:" + Constants.SecondaryTileImagePath + imgname) select v;

                    if (CompareNode.Count()==0)
                    {
                        xdoc.Root.Add(new XElement("Tile",
                                new XAttribute("Id", tid),

                                new XElement("Image", "isostore:" + Constants.SecondaryTileImagePath + imgname)));
                        isostream = new IsolatedStorageFileStream(ResourceHelper.ProjectName + "/Tile.xml", FileMode.Create, isoStore);
                        xdoc.Save(isostream);
                        isostream.Close();
                 }
                }
            }
            else
            {
                if (!isoStore.DirectoryExists(ResourceHelper.ProjectName))
                    isoStore.CreateDirectory(ResourceHelper.ProjectName);
                isostream = new IsolatedStorageFileStream(ResourceHelper.ProjectName + "/Tile.xml", FileMode.Create, isoStore);
                xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Tiles",
                    new XElement("Tile",
                    new XAttribute("Id", "1"),
                    new XElement("Image", "isostore:" + Constants.SecondaryTileImagePath + imgname))));
                xdoc.Save(isostream);
                isostream.Close();
            }

            if (Storage.FileExists(ResourceHelper.ProjectName + "/Tile.xml"))
            {
                isoStream = new IsolatedStorageFileStream(ResourceHelper.ProjectName + "/Tile.xml", FileMode.Open, isoStore);
                xdoc = XDocument.Load(isoStream);
                isoStream.Close();
                var data1 = from d in xdoc.Descendants("Tile") select d;
                if (data1.Count() > 0)
                {

                    if (data1.Count() ==6)
                    {
                        if (Storage.FileExists(ResourceHelper.ProjectName + "/History.xml"))
                            Storage.DeleteFile(ResourceHelper.ProjectName + "/History.xml");
                        AppSettings.LastPhoneUpdatedDate = AppSettings.LastPublishedDate;
                        if (!AppSettings.LiveTileBackgroundAgentStatus)
                        {
                              createFrontImage(Constants.FrontImageFilePathForPrimaryTile);
                              AppSettings.LiveTileBackgroundAgentStatus = true;
                        }
                        LiveTileHelper.UpdateFromXmlFile(ResourceHelper.ProjectName);
                        SyncAgentState.auto.Set();
                    }
                    else
                    {
                        SyncAgentState.auto.Set();
                    }
                }
            }
            if (!isoStore.FileExists(ResourceHelper.ProjectName + "/History.xml"))
            {
                if (SyncAgentState.SyncAgent != null)
                {
                     Deployment.Current.Dispatcher.BeginInvoke(() =>
                     {
                         ((IDowloadCompleteCallback)(SyncAgentState.SyncAgent)).OnStartEvent();
                     });
                }
            }
           
        }

        //TODO: move to a common class
        public void ResizeImage(ImageSource biInput)
        {
            try
            {
                string filePath = string.Empty;
                WriteableBitmap wbOutput;
                Image imgTemp = new Image();
                imgTemp.Source = biInput;

                wbOutput = new WriteableBitmap(imgTemp, null);
                using (MemoryStream stream = new MemoryStream())
                {
                    wbOutput.SaveJpeg(stream, 173, 173, 0, 100);
                   
                        filePath = imgname;
                        if (!Storage.DirectoryExists(Constants.SecondaryTileImagePath))
                            Storage.CreateDirectory(Constants.SecondaryTileImagePath.Substring(0,Constants.SecondaryTileImagePath.LastIndexOf('/')));
                        using (IsolatedStorageFileStream local = isoStore.CreateFile(Constants.SecondaryTileImagePath + filePath))
                        {
                            local.Write(stream.GetBuffer(), 0, stream.GetBuffer().Length);
                        }
                }
                CreateBackImage(Constants.SecondaryTileImagePath + imgname);
                Exceptions.UpdateAgentLog("ResizeImage Completed In PrimaryTileUpdate");

            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ResizeImage Method In Vidoes file", ex);
            }
        }

        public void CreateBackImage(string File)
        {
            try
            {
                Rectangle rect = new Rectangle();
                rect.Width = 173;
                rect.Height = 173;

                Grid grid = new Grid();

                TextBlock text = new TextBlock();
                //text.Text = Title;
                if (SettingsHelper.getStringValue("textcolor") == "true")
                    text.Foreground = Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
                else
                    text.Foreground = new SolidColorBrush(Colors.White);

                text.FontSize = 20;
                text.VerticalAlignment = VerticalAlignment.Bottom;
                text.HorizontalAlignment = HorizontalAlignment.Left;
                text.TextWrapping = TextWrapping.Wrap;
                text.MaxWidth = 170;
                text.MaxHeight = 60;
                text.Padding = new Thickness(5, 5, 5, 5);

                BitmapImage bi = new BitmapImage();
                bi.CreateOptions = BitmapCreateOptions.None;
                if (isoStore.FileExists(File))
                {
                    using (IsolatedStorageFileStream local = isoStore.OpenFile(File, FileMode.Open, FileAccess.Read))
                    {
                        bi.SetSource(local);
                    }
                }
                Image i = new Image();
                i.Source = bi;
                grid.Children.Add(rect);
                grid.Children.Add(i);
                grid.Children.Add(text);
                grid.Arrange(new Rect(0d, 0d, 173d, 173d));

                WriteableBitmap wbmp = new WriteableBitmap(grid, null);
                string fullPath = File;
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {

                    using (var stream = store.OpenFile(fullPath, System.IO.FileMode.OpenOrCreate))
                    {
                        wbmp.SaveJpeg(stream, 173, 173, 0, 100);
                        stream.Close();
                        stream.Dispose();
                        store.Dispose();
                      
                    }
                }

                AddToXml(TableQuery);
                Exceptions.UpdateAgentLog("CreateBackImage Completed In PrimaryTileUpdate");

            }

            catch (Exception ex)
            {
                Exception exNew = new Exception("Exception in CreateBackImage Method In Vidoes file\n\n" + ex.Message + " \n\n Stack Trace \n" + ex.StackTrace);
                Exceptions.SaveOrSendExceptions(exNew.ToString(), ex);
                if (SyncAgentState.SyncAgent != null)
                {
                     Deployment.Current.Dispatcher.BeginInvoke(() =>
                     {
                         ((IDowloadCompleteCallback)(SyncAgentState.SyncAgent)).OnStartEvent();
                     });
                }

            }

        }
        public void createFrontImage(string path)
        {
           
            string extension = string.Empty;
            try
            {

                if (Storage.FileExists(Constants.BackgroundImageForTile+ ResourceHelper.ProjectName + "_173_173.png"))
                {
                    Storage.DeleteFile(Constants.BackgroundImageForTile + ResourceHelper.ProjectName + "_173_173.png");
                }
                
                Rectangle rect = new Rectangle();
                rect.Width = 173;
                rect.Height = 173;

                Grid grid = new Grid();

                Canvas c = new Canvas();
                c.Height = 40;
                c.Width = 40;
                string canvasmargin = (SettingsHelper.getStringValue("canvasmargin"));
                string[] s1 = canvasmargin.Split(',');
                c.Margin = new Thickness(Convert.ToDouble(s1[0]), Convert.ToDouble(s1[1]), Convert.ToDouble(s1[2]), Convert.ToDouble(s1[3]));
                TextBlock text1 = new TextBlock();
                text1.FontFamily = new FontFamily("Segoe WP Semibold");
                if (isoStore.FileExists(ResourceHelper.ProjectName + "/History.xml"))
                {
                    isoStream = new IsolatedStorageFileStream(ResourceHelper.ProjectName + "/History.xml", FileMode.Open, isoStore);
                    xdoc = XDocument.Load(isoStream);
                    isoStream.Close();
                    var data1 = from d in xdoc.Descendants("History") select d;
                    if (data1.Count() > 0)
                    {
                        AppSettings.LiveTileHistroryCount = data1.Count();
                        text1.Text = AppSettings.LiveTileHistroryCount.ToString();
                    }
                }
                else
                {
                    if (isoStore.FileExists(ResourceHelper.ProjectName + "/Tile.xml"))
                    {
                        isoStream = new IsolatedStorageFileStream(ResourceHelper.ProjectName + "/Tile.xml", FileMode.Open, isoStore);
                        xdoc = XDocument.Load(isoStream);
                        isoStream.Close();
                        var data1 = from d in xdoc.Descendants("Tile") select d;
                        if (data1.Count() > 0)
                        {
                            text1.Text = AppSettings.LiveTileHistroryCount.ToString();
                        }
                    }
                }

                text1.Foreground = new SolidColorBrush(Colors.White);
                text1.FontSize = 60;
                string textmargin = (SettingsHelper.getStringValue("textblockmargin"));

                string[] s2 = textmargin.Split(',');
               
                if (text1.Text.Length > 1)
              
                text1.Margin = new Thickness(Convert.ToDouble(s2[0]), Convert.ToDouble(s2[1]), Convert.ToDouble(s2[2]), Convert.ToDouble(s2[3]));
                else
                       
                    text1.Margin = new Thickness(-100, -55, 0, 0);
               
                text1.Padding = new Thickness(5, 5, 5, 5);

                c.Children.Add(text1);
                BitmapImage bi = new BitmapImage();
                bi.CreateOptions = BitmapCreateOptions.None;

                bi.UriSource = new Uri("/ApplicationLiveTile.png", UriKind.Relative);

                Image i = new Image();
                i.Source = bi;
               
                string imagemargin = (SettingsHelper.getStringValue("imagemargin"));
                string[] s3 = imagemargin.Split(',');
                i.Margin = new Thickness(Convert.ToDouble(s3[0]), Convert.ToDouble(s3[1]), Convert.ToDouble(s3[2]), Convert.ToDouble(s3[3]));

                grid.Children.Add(rect);
                grid.Children.Add(i);

                grid.Children.Add(c);
                grid.Arrange(new Rect(0d, 0d, 173d, 173d));

                WriteableBitmap wbmp = new WriteableBitmap(grid, null);

                var imageData = new ImageTools.Image(wbmp.PixelWidth, wbmp.PixelHeight);
                for (int y = 0; y < wbmp.PixelHeight; ++y)
                {
                    for (int x = 0; x < wbmp.PixelWidth; ++x)
                    {
                        int pixel = wbmp.Pixels[wbmp.PixelWidth * y + x];
                        imageData.SetPixel(x, y,
                            (byte)((pixel >> 16) & 0xFF),
                            (byte)((pixel >> 8) & 0xFF),
                            (byte)(pixel & 0xFF),
                            (byte)((pixel >> 24) & 0xFF)
                        );
                    }
                }
                var imageStream = new MemoryStream();
                var pngEncoder = new ImageTools.IO.Png.PngEncoder();
                pngEncoder.Encode(imageData, imageStream);
                extension = "png";
                imageStream.Position = 0;
                byte[] binaryData = new byte[imageStream.Length];
                imageStream.Read(binaryData, 0, (int)imageStream.Length);
                string fullPath = path + "ApplicationLiveTile.png";
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!isoStore.DirectoryExists(Constants.BackgroundImageForTile.Substring(0,Constants.BackgroundImageForTile.LastIndexOf('/'))))
                        isoStore.CreateDirectory(Constants.BackgroundImageForTile.Substring(0, Constants.BackgroundImageForTile.LastIndexOf('/')));

                    using (var local = store.OpenFile(fullPath, System.IO.FileMode.OpenOrCreate))
                    {
                        local.Write(binaryData, 0, binaryData.Length);
                        local.Close();
                        store.Dispose();
                    }
                }
                SyncAgentState.ResetEvent();
                Exceptions.UpdateAgentLog("createFrontImage Completed In PrimaryTileUpdate");

            }
            catch (Exception ex)
            {
                Exception exNew = new Exception("Exception in createFrontImage Method In Vidoes file\n\n" + ex.Message + " \n\n Stack Trace \n" + ex.StackTrace);
                Exceptions.SaveOrSendExceptions(exNew.ToString(), ex);
                if (SyncAgentState.SyncAgent != null)
                {
                     Deployment.Current.Dispatcher.BeginInvoke(() =>
                     {
                         ((IDowloadCompleteCallback)(SyncAgentState.SyncAgent)).OnStartEvent();
                     });
                }//end if
            }// end catch
        }//end CreateFrontImage

        

        public void LiveTileUpdate(string appName)
        {
            int downloadedShowIndex = 0;
            int downloadHistory;
            List<ShowList> downloadrecentlist = null;
           
                downloadrecentlist = OnlineShow.GetRecentlyAddedShows();

                downloadHistory = downloadrecentlist.Count();

                if (Storage.FileExists(appName + "/History.xml"))
                {
                    XDocument xdoc = Storage.ReadFileAsDocument(appName + "/History.xml");

                    var data = from c in xdoc.Descendants("History") select c;

                    SyncAgentState.HistoryCount = data.Count();
                    AppSettings.LiveTileHistroryCount = data.Count();
                }
                else
                {
                    SyncAgentState.HistoryCount = downloadHistory;
                    AppSettings.LiveTileHistroryCount = downloadHistory;
                }
                        SyncAgentState.mids = new string[downloadHistory];
                        foreach (var id in downloadrecentlist)
                        {
                            SyncAgentState.mids[downloadedShowIndex] =id.ShowID.ToString();
                            downloadedShowIndex++;
                        }

                PrimaryTileUpdate primaryTile = new PrimaryTileUpdate();
                primaryTile.CreateTile(SyncAgentState.mids);
            //}
                if (Constants.LiveTileBackgroundAgentStatus == true)
                {
                    AppSettings.BackgroundAgentStatus = SyncAgentStatus.UploadStory;
                }
        }

    }
}
