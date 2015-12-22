using Common.Library;
using OnlineVideos.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
#if WP8 && NOTANDROID
using System.Windows.Media.Imaging;
#endif
using System.Xml.Linq;
  #if NOTANDROID
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;
#endif
#if WINDOWS_APP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
#endif
namespace OnlineVideos.Data
{
  public static  class DownloadManager
    {
      private static object regexImgSrc;
      public  static void Savetofolder(string title, string url)
      {

          try
          {
              DownloadTable city = new DownloadTable();
              city.Title = title;
              city.Url = url;
              city.Status = "Folder";
              Task.Run(async () => await Constants.connection.InsertAsync(city));

             
          }
          catch (Exception ex)
          {
              ex.Data.Add("title", title);
              ex.Data.Add("url", url);
              Exceptions.SaveOrSendExceptions("Exception in Savetofolder  Method In downloadmanger.cs file", ex);
          }
      }

      public static void SaveDownloadingInfo(string title, string url,string thumbnail="")
      {

          try
          {
              string thumbnailimage = thumbnail;
              DownloadTable city = new DownloadTable();
              city.Title = title;
              city.Url = url;
              city.ThumbNail = thumbnailimage;
              city.Status = "Downloading..";
              Task.Run(async () => await Constants.connection.InsertAsync(city));
             
          }
          catch (Exception ex)
          {
              ex.Data.Add("title", title);
              ex.Data.Add("url", url);
              Exceptions.SaveOrSendExceptions("Exception in SaveDownloadingInfo  Method In downloadmanger.cs file", ex);
          }
      }

      public static List<DownloadInfo> LoadDownloadUrls()
      {
          List<DownloadInfo> objVideoList = null;
          try
          {
         List<DownloadTable>  xquery = new List<DownloadTable>();
          xquery=Task.Run(async()=>await Constants.connection.Table<DownloadTable>().ToListAsync()).Result;
               objVideoList = new List<DownloadInfo>();
               foreach (var itn in xquery)
               {
                   DownloadInfo download = new DownloadInfo();
                   download.LinkUrl = itn.Url;
                   download.FolderID = itn.ID;
                   download.DownloadStatus = itn.Status;
                   download.title = itn.Title;
                   download.id = itn.ID.ToString();
                   download.DownLoadID = itn.DownLoadID;
                   download.ThumbNail = itn.ThumbNail;
                   objVideoList.Add(download);
               }

          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in LoadDownloadUrls  Method In downloadmanger.cs file", ex);
          }
          return objVideoList;
      }

      public static ListGroup GetDownLoadList()
      {
#if NOTANDROID
          ListGroup g = new ListGroup("Media Files >", new Thickness(230, 6, 0, 0));
#endif
#if ANDROID
          ListGroup g = new ListGroup();
#endif
          try
          {
              var query = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(l => l.IsHidden == false).OrderByDescending(j => j.CreatedDate).ToListAsync()).Result;
              foreach (var item in query.Take(25))
              {
                  ShowList g1 = new ShowList();
                  #if NOTANDROID
                  g1.LandingImage = ResourceHelper.getShowTileImage1(item.TileImage);
#endif
                  g1.ShowID = Convert.ToInt32(item.ShowID);
                  g1.Genre = item.Genre;
                  g1.Title = item.Title;
                  g1.TileImage = item.TileImage;
                  g1.TitleForDownLoad = item.Title.Substring(item.Title.LastIndexOf("/") + 1);
                  g.Items.Add(g1);
              }
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in GetDownLoadList Method In OnlineShow.cs file", ex);
          }
          return g;
      }
      public static void DeleteHideandShowlinks(int id)
      {
          try
          {
              
              List<HideShowLinks> xquery = Task.Run(async()=>await Constants.connection.Table<HideShowLinks>().Where(i=>i.ID==id).ToListAsync()).Result;
              if (xquery.Count() > 0)
              {
                  HideShowLinks ds = xquery.FirstOrDefault();
                 Constants.connection.DeleteAsync(ds);
                  
              }
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in DeleteDownloadUrls  Method In downloadmanger.cs file", ex);
          }
      }
      public static void DeleteDownloadUrls(int id)
      {
          try
          {

              var xquery = Task.Run(async () => await Constants.connection.Table<DownloadTable>().Where(v => v.ID == id).ToListAsync()).Result;
              if (xquery.Count() > 0)
              {
                  DownloadTable ds = xquery.FirstOrDefault();
                  Task.Run(async () => await Constants.connection.DeleteAsync(ds));
              }
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in DeleteDownloadUrls  Method In downloadmanger.cs file", ex);
          }
      }

      public static void DeleteDownloadStatus(int id)
      {
          try
          {
              
              var xquery = Task.Run(async () => await Constants.connection.Table<DownloadStatus>().Where(v => v.Id == id).ToListAsync()).Result;
              if (xquery.Count() > 0)
              {
                  DownloadStatus ds = xquery.FirstOrDefault();
                  string command = "delete from DownloadStatus where Id =" + ds.Id;
                  Task.Run(async () => await Constants.connection.QueryAsync<DownloadStatus>(command));
              }
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in DeleteDownloadUrls  Method In downloadmanger.cs file", ex);
          }
      }

      public static void InsertPlaylistTable(TimeSpan starttime,TimeSpan endtime,string linkurl,int showid)
      {
          try
          {
              string command = string.Empty;
              command = "Insert into PlayListTable(StartTime,EndTime,LinkUrl,Showid)Values('" + starttime + "','" + endtime + "','"+linkurl+"','" + showid + "')";
              Task.Run(async () => await Constants.connection.QueryAsync<PlayListTable>(command));
          }
          catch (Exception ex)
          {
               ex.Data.Add("Filename", linkurl);
              Exceptions.SaveOrSendExceptions("Exception in InsertPlaylistTable  Method In downloadmanger.cs file", ex);
            
          }
      }

      public static void DownloadingStatus(string Filename, string FolderStatus, long size)
      {
          try
          {
              string ext = System.IO.Path.GetExtension(Filename);
              string name = System.IO.Path.GetFileNameWithoutExtension(Filename);
              if (FolderStatus == "Folder")
              {
                  SaveDownloadedFolder(Filename, size);
                  SaveDownloadedFolderLink(Filename);
              }
              else
              {
                  SaveDownloaded(Filename, size);
                  SaveDownloadedLink(Filename);
              }
          }
          catch (Exception ex)
          {
              ex.Data.Add("Filename", Filename);
              Exceptions.SaveOrSendExceptions("Exception in DownloadingStatus  Method In downloadmanger.cs file", ex);
          }
      }

      public static void SaveDownloadedFolderLink(string url)
      {
          try
          {

              int NextId = 0;
              string ext = System.IO.Path.GetExtension(url);
              ShowLinks vt = new ShowLinks();
              vt.ShowID = AppSettings.DownloadFolderID;
              var xquery = Task.Run(async () => await Constants.connection.Table<ShowLinks>().OrderByDescending(j => j.LinkOrder).ToListAsync()).Result;
              if (xquery.Count() > 0)
              {
                  foreach (var itm in xquery)
                  {
                      int id = Convert.ToInt32(itm.LinkOrder);
                      NextId = id + 1;
                      break;
                  }
              }
              else
              {
                  NextId = 1;
              }
              vt.LinkOrder = NextId;
              vt.Title = url.Substring(url.LastIndexOf('/') + 1);
              vt.Rating = 0;

              vt.IsFavourite = false;
              if (ext == ".doc")
              {
                  vt.LinkType = "Word Document";
              }
              else if (ext == ".jpg" || ext == ".png")
              {
                  vt.LinkUrl = url.Substring(url.LastIndexOf('/') + 1);
                  vt.LinkType = "Images";
              }
              else if (ext == ".3gp" || ext == ".3g2" || ext == ".mp4" || ext == ".m4v" || ext == ".wmv")
              {
                  vt.LinkUrl = url;
                  vt.LinkType = "Songs";
              }
              else if (ext == ".mp3" || ext == ".wav" || ext == ".aac" || ext == ".amr" || ext == ".wma")
              {
                  vt.LinkUrl = url;
                  vt.LinkType = "Audio";
              }
              vt.UrlType = ext;
              vt.ClientPreferenceUpdated = DateTime.Now;
              vt.ClientShowUpdated = DateTime.Now;
              string command = "insert into Showlinks(ShowID,Title,Rating,IsFavourite,LinkOrder,LinkType,LinkUrl,IsHidden,ClientPreferenceUpdated,ClientShowUpdated,UrlType) values(" + vt.ShowID + ",'" + vt.Title + "'," + vt.Rating + ",'" + 0 + "'," + vt.LinkOrder + ",'" + vt.LinkType + "','" + vt.LinkUrl + "','" + 0 + "','" + vt.ClientPreferenceUpdated + "','" + vt.ClientShowUpdated + "','" + vt.UrlType + "') ";
              Task.Run(async () => await Constants.connection.QueryAsync<ShowList>(command));

          }
          catch (Exception ex)
          {

              ex.Data.Add("url", url);
              Exceptions.SaveOrSendExceptions("Exception in SaveDownloadedFolderLink  Method In downloadmanger.cs file", ex);
          }
      }

      public static void SaveDownloadedFolder(string url, long size)
      {
          try
          {

             string ext = System.IO.Path.GetExtension(url);
                string lenght = Convert.ToString(url.Length);
                ShowList vt = new ShowList();
                int NextId = 0;
                if (SettingsHelper.getStringValue("DownloadFolder") == "0")
                {
                    var xquery = Task.Run(async () => await Constants.connection.Table<ShowList>().OrderByDescending(j => j.ShowID).ToListAsync()).Result;
                    if (xquery.Count() > 0)
                    {
                        foreach (var itm in xquery)
                        {
                            int id = itm.ShowID;
                            NextId = id + 1;
                            break;
                        }
                    }
                    else
                    {
                        NextId = 1;
                    }
                    AppSettings.DownloadFolderID = NextId;
                    vt.ShowID = NextId;
                    vt.Title = AppSettings.DownloadedFolderName;
                    vt.Rating = 0;

                    vt.ReleaseDate = GetFileSize(Convert.ToInt32(size)); //Math.Round(((size / 1024f)/1024f), 2).ToString() + "  MB";
                    vt.TileImage = "folder.png";
                    vt.Genre = "Folder";
                    vt.CreatedDate = DateTime.Now;
                    vt.IsFavourite = false;
                    vt.SubTitle = "English";
                    vt.IsHidden = false;
                    vt.ClientPreferenceUpdated = DateTime.Now;
                    vt.ClientShowUpdated = DateTime.Now;
                    vt.Status = "Custom";
                    //vt.Status = "Downloading..";
                    vt.ClientPreferenceUpdated = DateTime.Now;
                    vt.ClientShowUpdated = DateTime.Now;
                    Task.Run(async () => await Constants.connection.InsertAsync(vt));
                    AppSettings.IsDownloadFolder = true;


                    
                }
          }
          catch (Exception ex)
          {

              ex.Data.Add("url", url);
              Exceptions.SaveOrSendExceptions("Exception in SaveDownloadedFolder  Method In downloadmanger.cs file", ex);
          }
      }

      public static void SaveDownloaded(string url, long size)
      {
          try
          {

               string ext = System.IO.Path.GetExtension(url);
                string lenght = Convert.ToString(url.Length);
                ShowList vt = new ShowList();
                int NextId = 0;
                //if (SettingsHelper.getStringValue("DownloadFolder") == "0")
                //{
                    var xquery = Task.Run(async () => await Constants.connection.Table<ShowList>().OrderByDescending(j => j.ShowID).ToListAsync()).Result;
                    if (xquery.Count() > 0)
                    {
                        foreach (var itm in xquery)
                        {
                            int id = itm.ShowID;

                            NextId = id + 1;
                            break;
                        }
                    }
                    else
                    {
                        NextId = 1;
                    }
                    vt.ShowID = NextId;
                    vt.Title = url;
                    //vt.Title = url.Replace(".jpg","").Replace(".png","").Replace(".mp4","").Replace(".mp3","").Replace(".wmv","").Replace(".mkv","").Replace(".avi","").Replace(".m4v","");
                    vt.Rating = 4.5;
                    vt.ReleaseDate = GetFileSize(Convert.ToInt32(size));//Math.Round(((size / 1024f)/1024f), 2).ToString() + "  MB";
                    if (ext == ".doc")
                    {
                        vt.TileImage = "doc.jpg";
                        vt.Genre = "Word Document";
                    }
                    else if (ext == ".jpg" || ext == ".png")
                    {
                        vt.TileImage = url;
                        vt.Genre = "Images";
                    }
                    if (ext == ".3gp" || ext == ".3g2" || ext == ".mp4" || ext == ".m4v" || ext == ".wmv")
                    {

                        string name = url.Replace(".mp4", "").Replace(".mp3", "").Replace(".wmv", "").Replace(".mkv", "").Replace(".avi", "").Replace(".m4v", "");
                        vt.TileImage = name + ".jpg";
                        vt.Genre = "Videos";
                    }
                    else if (ext == ".mp3" || ext == ".wav" || ext == ".aac" || ext == ".amr" || ext == ".wma")
                    {
                        vt.TileImage = "Audio.jpg";
                        vt.Genre = "Audio";
                    }

                    vt.CreatedDate = DateTime.Now;
                    vt.IsFavourite = false;
                    vt.SubTitle = "English";
                    vt.IsHidden = false;
                    vt.Status = "Custom";
                    //vt.Status = "Downloading..";
                    vt.ClientPreferenceUpdated = DateTime.Now;
                    vt.ClientShowUpdated = DateTime.Now;
                    Task.Run(async () => await Constants.connection.InsertAsync(vt));
                  
                    
          }
          catch (Exception ex)
          {
              ex.Data.Add("url", url);
              Exceptions.SaveOrSendExceptions("Exception in SaveDownloaded  Method In downloadmanger.cs file", ex);
          }
      }

      public static string GetFileSize(int Size)
      {
          if (Size < 2048)
              return Size + "B";
          if (Size < 10240)
              return (Size / 1024) + "KB";
          return (Size / 1024 / 1024) + "MB";
      }

      public static void SaveDownloadedLink(string url)
      {
          try
          {
             
              string ext = System.IO.Path.GetExtension(url);
              ShowLinks vt = new ShowLinks();
              int NextId = 0;
              var xquery = Task.Run(async () => await Constants.connection.Table<ShowLinks>().OrderByDescending(j => j.ShowID).ToListAsync()).Result;
              if (xquery.Count() > 0)
              {
                  foreach (var itm in xquery)
                  {
                      int id = itm.ShowID;
                      NextId = id + 1;
                      break;
                  }
              }
              else
              {
                  NextId = 1;
              }
              vt.ShowID = NextId;
              vt.Title = url.Substring(url.LastIndexOf('=') + 1);
              vt.Rating = 0;

              vt.IsFavourite = false;
              vt.LinkOrder = 0;
              if (ext == ".doc")
              {
                  vt.LinkType = "Word Document";
              }
              else if (ext == ".jpg" || ext == ".png")
              {
                  vt.LinkUrl = url.Substring(url.LastIndexOf('/') + 1);
                  vt.LinkType = "Images";
              }
              else if (ext == ".3gp" || ext == ".3g2" || ext == ".mp4" || ext == ".m4v" || ext == ".wmv")
              {
                  vt.LinkUrl = url;
                  vt.LinkType = "Songs";
              }
              else if (ext == ".mp3" || ext == ".wav" || ext == ".aac" || ext == ".amr" || ext == ".wma")
              {
                  vt.LinkUrl = url;
                  vt.LinkType = "Audio";
              }
              vt.LinkID = NextId;
              vt.UrlType = ext;
              vt.IsHidden = false;
              vt.RatingFlag = 4;
              vt.Description = url.Substring(url.LastIndexOf('=') + 1);
              vt.ClientPreferenceUpdated = DateTime.Now;
              vt.ClientShowUpdated = DateTime.Now;
              string command = "insert into Showlinks(ShowID,Title,Rating,IsFavourite,LinkOrder,LinkType,LinkUrl,IsHidden,ClientPreferenceUpdated,ClientShowUpdated,UrlType) values(" + vt.ShowID + ",'" + vt.Title + "'," + vt.Rating + ",'" + 0 + "'," + vt.LinkOrder + ",'" + vt.LinkType + "','" + vt.LinkUrl + "','" + 0+ "','" + vt.ClientPreferenceUpdated + "','" + vt.ClientShowUpdated + "','" + vt.UrlType + "') ";
              Task.Run(async () => await Constants.connection.QueryAsync<ShowList>(command));
           
          }
          catch (Exception ex)
          {
              ex.Data.Add("url", url);
              Exceptions.SaveOrSendExceptions("Exception in SaveDownloadedLink  Method In downloadmanger.cs file", ex);
          }
      }
   #if NOTANDROID
      public static List<ShowList> GetDownLoadImagepopup(int showid)
      {
          List<ShowList> Retunlist = new List<ShowList>();
          try
          {
              ShowList g = new ShowList();
              g = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(l => l.ShowID == showid).FirstOrDefaultAsync()).Result;
              g.TileImage = ResourceHelper.getShowTileImage1(g.TileImage);
              StorageFile sf=Task.Run(async ()=> await StorageFile.GetFileFromApplicationUriAsync(new Uri(g.TileImage))).Result;
              IRandomAccessStream fileStream = Task.Run(async () => await sf.OpenAsync(Windows.Storage.FileAccessMode.Read)).Result;
              BitmapImage image = new BitmapImage();
#if WINDOWS_APP
              image.SetSource(fileStream);
#endif
#if WINDOWS_PHONE_APP
              //image.SetSource(fileStream);
#endif
              int x = image.PixelHeight;
              int y = image.PixelWidth;
              g.Height = Convert.ToString(x);
              g.Width = Convert.ToString(y);

              Retunlist.Add(g);
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in GetDownLoadImagepopup  Method In downloadmanger.cs file", ex);
          }
          return Retunlist;
      }
#endif
      public static void SaveIsFavouriteSongs()
      {
          try
          {
              string command = string.Empty;
              string command1 = string.Empty;
              int showid = int.Parse(AppSettings.ShowID);
              string linktype = AppSettings.LinkType;
              Constants.selecteditemshowlinkslist = new List<ShowLinks>();
              Constants.selecteditemshowlinkslist = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid).ToListAsync()).Result;
              foreach (ShowLinks links in Constants.selecteditemshowlinkslist)
              {
                    if (links.IsFavourite == false)
                      {

                          command = "UPDATE ShowLinks SET IsFavourite =" + 1 + " " + "WHERE  ShowID=" + links.ShowID ;
                          command1 = "UPDATE ShowList SET IsFavourite =" + 1 + " " + "WHERE  ShowID=" + links.ShowID;
                      }
                      else
                      {

                          command = "UPDATE Showlinks SET IsFavourite =" + 0 + " " + "WHERE  ShowID=" + links.ShowID ;
                          command1 = "UPDATE ShowList SET IsFavourite =" + 0 + " " + "WHERE  ShowID=" + links.ShowID;
                      }

                      Task.Run(async () => await Constants.connection.QueryAsync<ShowLinks>(command));
                      Task.Run(async () => await Constants.connection.QueryAsync<ShowList>(command1));
              }
              Constants.selecteditem = null;
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in SaveIsFavouriteSongs  Method In downloadmanger.cs file", ex);
              Exception exNew = new Exception("Exception in SaveFavoriteSongs Method In Vidoes file\n\n" + ex.Message + " \n\n Stack Trace \n" + ex.StackTrace);
          }
      }


      public static void UpdateTileImage(string Tileimage)
      {
          try
          {
              string command = string.Empty;
              command = "UPDATE ShowList SET TileImage ='" + Tileimage + "' " + "WHERE  ShowID=" + AppSettings.ShowID;
              Task.Run(async () => await Constants.connection.QueryAsync<ShowLinks>(command));
          }
          catch (Exception ex)
          {
              
          }
      }

      public static void SaveIsFavouriteMovies()
      {
          try
          {
              string command = string.Empty;
              string command1 = string.Empty;
              int showid = int.Parse(AppSettings.ShowID);
              List<ShowList> selecteditemshowlinkslist = new List<ShowList>();
               selecteditemshowlinkslist = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).ToListAsync()).Result;
               foreach (ShowList links in selecteditemshowlinkslist)
              {
                  if (links.IsFavourite == false)
                  {

                      command = "UPDATE ShowList SET IsFavourite =" + 1 + " " + "WHERE  ShowID=" + links.ShowID;
                  }
                  else
                  {
                   
                      command = "UPDATE ShowList SET IsFavourite =" + 0 + " " + "WHERE  ShowID=" + links.ShowID;
                  }

                  Task.Run(async () => await Constants.connection.QueryAsync<ShowLinks>(command));
                
              }
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in SaveIsFavouriteSongs  Method In downloadmanger.cs file", ex);
              Exception exNew = new Exception("Exception in SaveFavoriteSongs Method In Vidoes file\n\n" + ex.Message + " \n\n Stack Trace \n" + ex.StackTrace);
          }
      } 

      public static void InsertGuidID(string GuidId,int Id)
      {
          try
          {
              string command = string.Empty;
              command = "UPDATE DownloadTable SET DownLoadID ='" + GuidId + "' " + "WHERE  ID =" + Id;
              Task.Run(async () => await Constants.connection.QueryAsync<ShowLinks>(command));
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in InsertGuidID  Method In downloadmanger.cs file", ex);
          }
      }

      public static void DeleteAll()
      {
          try
          {
              string command = string.Empty;
              command = "Delete From  DownloadStatus";
              Task.Run(async () => await Constants.connection.QueryAsync<ShowLinks>(command));
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in DeleteAll  Method In downloadmanger.cs file", ex);
          }
      }

      public static void DeleteAllShowlinks()
      {
          try
          {
              string command = string.Empty;
              command = "Delete From  Showlinks";
              Task.Run(async () => await Constants.connection.QueryAsync<ShowLinks>(command));
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in DeleteAllShowlinks  Method In downloadmanger.cs file", ex);
          }
      }
      #if NOTANDROID
      public static XDocument ReadFileAsDocument(string filename)
      {
          XDocument xdoc = new XDocument();
          try
          {
              StorageFolder store =  ApplicationData.Current.LocalFolder;
              StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"XmlData\"+filename, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
              var f = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
              IInputStream inputStream = f.GetInputStreamAt(0);
              DataReader dataReader = new DataReader(inputStream);
              uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
              string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
              System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlData));
              xdoc = XDocument.Load(ms);
              dataReader.DetachStream();
              inputStream.Dispose();
              f.Dispose();
              ms.Dispose();
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in ReadFileAsDocument  Method In downloadmanger.cs file", ex);
          }
          return xdoc;
      }

      public static  void SaveFileFromDocument(string filename, XDocument xdoc)
      {
          try
          {

              StorageFolder store1 = Task.Run(async()=> await ApplicationData.Current.LocalFolder.CreateFolderAsync("XmlData",CreationCollisionOption.OpenIfExists)).Result;
              StorageFile file1 =Task.Run(async()=> await store1.CreateFileAsync(filename, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
              var fquery1 =Task.Run(async()=> await file1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
              var outputStream = fquery1.GetOutputStreamAt(0);
              var writer = new Windows.Storage.Streams.DataWriter(outputStream);
              StringBuilder sb = new StringBuilder();
              TextWriter tx = new StringWriter(sb);
              xdoc.Save(tx);
              string text = tx.ToString();
              text = text.Replace("utf-16", "utf-8");
              writer.WriteString(text);
              var fi =writer.StoreAsync();
              var oi =outputStream.FlushAsync();
              writer.DetachStream();
              outputStream.Dispose();
              fquery1.Dispose();

          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in SaveFileFromDocument  Method In downloadmanger.cs file", ex);
          }
      }
#endif
      public static List<ShowList> GetDownViewAllLoadList()
      {
          List<ShowList> objMovieList1 = new List<ShowList>();
          List<ShowList> objMovieList = new List<ShowList>();
          try
          {
              objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.Rating).ToListAsync()).Result;
              foreach (var item in objMovieList)
              {
                  ShowList objMovie = new ShowList();
                  objMovie.Genre = item.Genre;
                  #if NOTANDROID
                  if (AppSettings.ProjectName == "Video Mix")
                  {
                      objMovie.LandingImage = ResourceHelper.getShowTileImage1(item.TileImage);
                  }
                  else
                  {
                      objMovie.LandingImage = ResourceHelper.getViewAllImageFromStorageOrInstalledFolder(item.TileImage);
                  }
#endif
                  objMovie.ShowID = Convert.ToInt32(item.ShowID);
                  objMovie.Title = item.Title.Substring(item.Title.LastIndexOf("/") + 1);
                  objMovie.TileImage = item.TileImage;
                  objMovie.RatingBitmapImage = ImageHelper.LoadRatingImage(item.Rating.ToString()).ToString();
                  if (AppSettings.ProjectName != "VideoMix")
                  objMovie.ReleaseDate = "size :" + "" + item.ReleaseDate.Replace("12:00:00 ", "").Replace("AM", "");
                  else
                  objMovie.ReleaseDate = item.ReleaseDate.Replace("12:00:00 ", "").Replace("AM", "");
                  objMovieList1.Add(objMovie);
              }

              Constants.ViellList = objMovieList1;
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in GetDownViewAllLoadList Method In DownloadManager.cs file", ex);

          }
          return objMovieList1;
      }

      public static List<ShowList> GetImageFav()
      {

          List<ShowList> ListForimages = new List<ShowList>();
          try
          {
             
              var GetImageList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsFavourite == true && i.Genre == "Images").ToListAsync()).Result;
                  foreach (ShowList item in GetImageList)
                  {
                      ShowList g1 = new ShowList();
                      #if NOTANDROID
                      g1.LandingImage = ResourceHelper.getShowTileImage1(item.TileImage);
#endif
                      g1.ShowID = Convert.ToInt32(item.ShowID);
                      g1.TileImage = item.TileImage;
                      g1.Title = item.Title.Substring(item.Title.LastIndexOf("/") + 1);
                      ListForimages.Add(g1);
                  }
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in GetImageFav Method In DownloadManager.cs file", ex);
          }

          return ListForimages;
      }


      public static string GetFolderName(string title)
      {
          string FolderName = string.Empty;
          try
          {
              string ext = System.IO.Path.GetExtension(title);

              if (ext == ".mp3" || ext == ".wav" || ext == ".aac" || ext == ".amr" || ext == ".wma")
              {
                  FolderName = "Audio";
              }
              else if (ext == ".3gp" || ext == ".3g2" || ext == ".mp4" || ext == ".m4v" || ext == ".wmv" || ext == ".mkv"||ext==".flv")
              {

                  FolderName = "Video";
              }
              else if (ext == ".doc")
              {

                  FolderName = "Documents";
              }
              else
              {

                  FolderName = "Images";
              }
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in GetFolderName Method In DownloadManager.cs file", ex);
          }
          return FolderName;
      }
      public static string trimtitle(string title)
      {
          string itemtitle = title;
          if (title.Contains(","))
          {
              string tit = itemtitle;
              if (tit.Contains("||"))
              {
                  tit = tit.Replace("||", "");
              }
              if (tit.Contains("'"))
              {
                  tit = tit.Replace("'", "");
              }
              if (tit.Contains("-"))
              {
                  tit = tit.Replace("-", "");
              }
              if (tit.Contains("|"))
              {
                  tit = tit.Replace("|", "");
              }
              tit = tit.Replace(",", "");
              title = tit.Split(' ')[0];
              title = title + " " + tit.Split(' ')[1];
              title = title + " " + tit.Split(' ')[2];
              title = title + " " + tit.Split(' ')[3];
              title = title.Trim();
          }
          else
          {
              string tite = itemtitle;
              if (tite.Contains("-"))
              {
                  tite = tite.Replace("-", "");
              }
              if (tite.Contains("'"))
              {
                  tite = tite.Replace("'", "");
              }
              if (tite.Contains("||"))
              {
                  tite = tite.Replace("||", "");
              }
              if (tite.Contains("|"))
              {
                  tite = tite.Replace("|", "");
              }
              title = tite.Split(' ')[0];
              title = title + " " + tite.Split(' ')[1];
              title = title + " " + tite.Split(' ')[2];
              title = title + " " + tite.Split(' ')[3];
              title = title.Trim();
          }
          return title;
      }
      public static void SaveAddVideoLinks(string url, string title, string Imageurl, int showid)
      {
          try
          {
              // string ext = System.IO.Path.GetExtension(url);
              ShowLinks vt = new ShowLinks();
              vt.ShowID = showid;
              string videotitle = trimtitle(title);
              vt.Title = videotitle;
              vt.Rating = 0;
              vt.IsFavourite = false;
              if (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result != null)
              {
                  if (Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == "Songs").FirstOrDefaultAsync()).Result != null ? Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == "Songs").FirstOrDefaultAsync()).Result.LinkOrder != null : false)
                  {
                      vt.LinkOrder = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(i => i.ShowID == showid && i.LinkType == "Songs").OrderByDescending(i => i.LinkOrder).FirstOrDefaultAsync()).Result.LinkOrder + 1;
                  }
                  else
                  {
                      vt.LinkOrder = 1;
                  }

              }
              else
              {
                  vt.LinkOrder = 1;
              }
              //vt.LinkOrder = 0;
              vt.LinkUrl = url;
              vt.LinkType = "Songs";
              vt.LinkID = showid;
              vt.UrlType = Imageurl;
              vt.IsHidden = false;
              vt.RatingFlag = 4;
              vt.Description = "";
              vt.ClientPreferenceUpdated = DateTime.Now;
              vt.ClientShowUpdated = DateTime.Now;
              string command = "insert into Showlinks(ShowID,Title,Rating,IsFavourite,LinkOrder,LinkType,LinkUrl,IsHidden,ClientPreferenceUpdated,ClientShowUpdated,UrlType) values(" + vt.ShowID + ",'" + vt.Title + "'," + vt.Rating + ",'" + 0 + "'," + vt.LinkOrder + ",'" + vt.LinkType + "','" + vt.LinkUrl + "','" + 0 + "','" + vt.ClientPreferenceUpdated + "','" + vt.ClientShowUpdated + "','" + vt.UrlType + "') ";
              Task.Run(async () => await Constants.connection.QueryAsync<ShowList>(command));

          }
          catch (Exception ex)
          {
              ex.Data.Add("url", url);
              Exceptions.SaveOrSendExceptions("Exception in SaveDownloadedLink  Method In downloadmanger.cs file", ex);
          }
      }

      public static List<ShowList> GetAudioFav()
      {
          List<ShowList> ListForimages = new List<ShowList>();
          try
          {
              var GetImageList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsFavourite == true && i.Genre == "Audio").ToListAsync()).Result;
                  
                  foreach (ShowList item in GetImageList)
                  {
                      ShowList g1 = new ShowList();
                        #if NOTANDROID
                      g1.LandingImage = ResourceHelper.getShowTileImage1(item.TileImage);
#endif
                      g1.ShowID = Convert.ToInt32(item.ShowID);
                      g1.TileImage = item.TileImage;
                      g1.Title = item.Title.Substring(item.Title.LastIndexOf("/") + 1);
                      ListForimages.Add(g1);
                  }
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in GetAudioFav Method In DownloadManager.cs file", ex);
          }

          return ListForimages;

      }

      public static List<ShowList> GetVideosFav()
      {
          List<ShowList> ListForimages = new List<ShowList>();
          try
          {
              var GetImageList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsFavourite == true && i.Genre == "Videos").ToListAsync()).Result;
              foreach (ShowList item in GetImageList)
              {
                  ShowList g1 = new ShowList();
                    #if NOTANDROID
                  g1.LandingImage = ResourceHelper.getShowTileImage1(item.TileImage);
#endif
                  g1.ShowID = Convert.ToInt32(item.ShowID);
                  g1.Title = item.Title.Substring(item.Title.LastIndexOf("/") + 1);
                  g1.TileImage = item.TileImage;
                  ListForimages.Add(g1);
              }
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in GetVideosFav Method In DownloadManager.cs file", ex);
          }
          return ListForimages;

      }

      public static List<FavoriteLinksTable> GetFavoriteLinks()
      {
          List<FavoriteLinksTable> favlist = new List<FavoriteLinksTable>();
          try
          {
              int LinkCount = 1;
              var favlinklist = Task.Run(async () => await Constants.connection.Table<FavoriteLinksTable>().ToListAsync()).Result;
              foreach (var item in favlinklist)
              {
                  FavoriteLinksTable table = new FavoriteLinksTable();
                  table.ChildLinksCount = item.ChildLinksCount;
                  table.Title = item.Title;
                  table.LinkUrl = item.LinkUrl;
                  table.ID = item.ID;
                  favlist.Add(table);
                  LinkCount++;
              }
          }
          catch (Exception ex)
          {
           
          }
          return favlist;
      }
      public static List<ShowList> SearchForImages(string searchText)
      {
          List<ShowList> ListForSearchForImages = new List<ShowList>();
          try
          {
              var GetImageList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.Title.Contains(searchText) && i.Genre == "Images").OrderBy(j => j.ShowID).ToListAsync()).Result;
              foreach (ShowList item in GetImageList)
              {
                  ShowList g1 = new ShowList();
                    #if NOTANDROID
                  g1.LandingImage = ResourceHelper.getShowTileImage1(item.TileImage);
#endif
                  g1.ShowID = Convert.ToInt32(item.ShowID);
                  g1.TileImage = item.TileImage;
                  g1.Title = item.Title.Substring(item.Title.LastIndexOf("/") + 1);
                  ListForSearchForImages.Add(g1);
              }
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in SearchForImages Method In DownloadManager.cs file", ex);
          }
          return ListForSearchForImages;
      }

      public static List<ShowList> SearchForAudios(string searchText)
      {
          List<ShowList> ListForSearchForAudios = new List<ShowList>();
          try
          {
              var GetImageList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.Title.Contains(searchText) && i.Genre == "Audio").OrderBy(j => j.ShowID).ToListAsync()).Result;
              foreach (ShowList item in GetImageList)
              {
                  ShowList g1 = new ShowList();
                    #if NOTANDROID
                  g1.LandingImage = ResourceHelper.getShowTileImage1(item.TileImage);
#endif
                  g1.ShowID = Convert.ToInt32(item.ShowID);
                  g1.TileImage = item.TileImage;
                  g1.Title = item.Title.Substring(item.Title.LastIndexOf("/") + 1);
                  ListForSearchForAudios.Add(g1);
              }
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in SearchForAudios Method In DownloadManager.cs file", ex);
          }
          return ListForSearchForAudios;
      }

      public static List<ShowList> SearchForVideos(string searchText)
      {
          List<ShowList> ListForSearchForSearchForVideos = new List<ShowList>();
          try
          {
              var GetImageList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.Title.Contains(searchText) && i.Genre == "Videos").OrderBy(j => j.ShowID).ToListAsync()).Result;
              foreach (ShowList item in GetImageList)
              {
                  ShowList g1 = new ShowList();
                    #if NOTANDROID
                  g1.LandingImage = ResourceHelper.getShowTileImage1(item.TileImage);
#endif
                  g1.ShowID = Convert.ToInt32(item.ShowID);
                  g1.TileImage = item.TileImage;
                  g1.Title = item.Title.Substring(item.Title.LastIndexOf("/") + 1);
                  ListForSearchForSearchForVideos.Add(g1);
              }


          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in SearchForVideos Method In DownloadManager.cs file", ex);
          }
          return ListForSearchForSearchForVideos;
      }

      public static List<ShowList> GetMoviefav()
      {
          List<ShowList> objMovieList1 = new List<ShowList>();
          List<ShowList> objMovieList = new List<ShowList>();
          try
          {
              objMovieList = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.IsFavourite == true).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.Rating).ToListAsync()).Result;
              foreach (var item in objMovieList)
              {
                  ShowList objMovie = new ShowList();
                  objMovie.Genre = item.Genre;
                    #if NOTANDROID
                  objMovie.LandingImage = ResourceHelper.getShowTileImage1(item.TileImage);
#endif
                  objMovie.ShowID = Convert.ToInt32(item.ShowID);
                  objMovie.Title = item.Title.Substring(item.Title.LastIndexOf("/") + 1);
                  objMovie.TileImage = item.TileImage;
                  objMovie.RatingBitmapImage = ImageHelper.LoadRatingImage(item.Rating.ToString()).ToString();
                  objMovie.ReleaseDate = "size :" + "" + item.ReleaseDate;
                  objMovieList1.Add(objMovie);
              }

              Constants.ViellList = objMovieList1;
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in GetDownViewAllLoadList Method In DownloadManager.cs file", ex);

          }
          return objMovieList1;
      }
        #if NOTANDROID
      public static void RemovieShows()
      {
          string command = string.Empty;
          string command1 = string.Empty;
          try
          {
             
              int showid = int.Parse(AppSettings.ShowID);
              var list = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).ToListAsync()).Result;
              foreach (ShowList items in list)
              {
                  command = "Delete From ShowLinks WHERE  ShowID=" + items.ShowID;
                  command1 = "Delete From ShowList WHERE  ShowID=" + items.ShowID;
                  string getfolder=items.Title.Substring(items.Title.LastIndexOf("/") + 1);
                  string folder = items.Title.Replace("/" + getfolder, "");
                  var storage = ApplicationData.Current.LocalFolder.GetFolderAsync(folder).GetResults();
                  StorageFile sf = Task.Run(async () => await storage.GetFileAsync(getfolder)).Result;
                  Task.Run(async()=>await sf.DeleteAsync());
              }
                Task.Run(async () => await Constants.connection.QueryAsync<ShowLinks>(command));
                Task.Run(async () => await Constants.connection.QueryAsync<ShowList>(command1));
          }
          catch (Exception ex)
          {
              Task.Run(async () => await Constants.connection.QueryAsync<ShowLinks>(command));
              Task.Run(async () => await Constants.connection.QueryAsync<ShowList>(command1));
              Exceptions.SaveOrSendExceptions("Exception in RemovieShows Method In DownloadManager.cs file", ex);
          }

      }
#endif
      public static void Savehistrory(int Showid,string Title,string Linkurl,string urltype)
      {
          //Constants.connection.CreateTableAsync<DownLoadHistory>();
          try
          {
              DownLoadHistory DlH = new DownLoadHistory();
              DlH.ShowID = Showid;
              DlH.Title = Title;
              DlH.LinkUrl = Linkurl;
              DlH.UrlType = urltype;
              Task.Run(async () => await Constants.connection.InsertAsync(DlH));
          }
          catch (Exception ex)
          {
              
              Exceptions.SaveOrSendExceptions("Exception in Savehistrory Method In DownloadManager.cs file", ex);
          }
      }

      public static void RemovieShowsForvideomix()
      {
          string command = string.Empty;
          string command1 = string.Empty;
          string commandShowlist = string.Empty;
          try
          {

              int showid = int.Parse(AppSettings.ShowID);
		var list = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).ToListAsync()).Result;
              foreach (ShowList items in list)
              {
                  command = "Delete From PlayListTable WHERE  ShowID=" + items.ShowID;
                  command1 = "Delete From ShowLinks WHERE  ShowID=" + items.ShowID;
                  commandShowlist = "Delete From ShowList WHERE  ShowID=" + items.ShowID;
                
              }
              Task.Run(async () => await Constants.connection.QueryAsync<ShowList>(command));
              Task.Run(async () => await Constants.connection.QueryAsync<ShowList>(command1));
              Task.Run(async () => await Constants.connection.QueryAsync<ShowList>(commandShowlist));
          }
          catch (Exception ex)
          {
              
              Exceptions.SaveOrSendExceptions("Exception in RemovieShows Method In DownloadManager.cs file", ex);
          }

      }

      public static List<DownLoadHistory> GetHistoryForImages()
      {
          List<DownLoadHistory> DlHList = new List<DownLoadHistory>();
          try
          {
              var listfordlh = Task.Run(async () => await Constants.connection.Table<DownLoadHistory>().Where(i => i.UrlType == "Images").OrderByDescending(j => j.ShowID).ToListAsync()).Result.Take(20);
              foreach (DownLoadHistory item in listfordlh)
              {
                  DownLoadHistory g1 = new DownLoadHistory();
                  g1.Title = item.Title.Substring(item.Title.LastIndexOf("/") + 1);
                  ShowLinks list = Task.Run(async () => await Constants.connection.Table<ShowLinks>().Where(k => k.ShowID == item.ShowID).FirstOrDefaultAsync()).Result;
                    #if NOTANDROID
                  g1.LandingImage = ResourceHelper.getShowTileImage1(list.Title.Substring(list.Title.LastIndexOf("/") + 1));
#endif
                  g1.ShowID = Convert.ToInt32(item.ShowID);
                  DlHList.Add(g1);
              }
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in GetHistoryForImages Method In DownloadManager.cs file", ex);
          }
          return DlHList;
      }

      public static List<DownLoadHistory> GetHistoryForAudios()
      {
          List<DownLoadHistory> DlHList = new List<DownLoadHistory>();
          try
          {
              var listfordlh = Task.Run(async () => await Constants.connection.Table<DownLoadHistory>().Where(i => i.UrlType == "Audio").OrderByDescending(j => j.ShowID).ToListAsync()).Result.Take(20);
              foreach (DownLoadHistory item in listfordlh)
              {
                  DownLoadHistory g1 = new DownLoadHistory();
                  g1.Title = item.Title.Substring(item.Title.LastIndexOf("/") + 1);
                    #if NOTANDROID
                  g1.LandingImage = ResourceHelper.getShowTileImage1("Audio.jpg");
#endif
                  g1.ShowID = Convert.ToInt32(item.ShowID);
                  DlHList.Add(g1);
              }
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in GetHistoryForAudios Method In DownloadManager.cs file", ex);
          }

          return DlHList;
      }

      public static void InsertDownloadItem(DownloadStatus Item)
      {
          try
          {
              Task.Run(async () => await Constants.connection.InsertAsync(Item));
          }
          catch (Exception ex)
          {
              
             Exceptions.SaveOrSendExceptions("Exception in InsertDownloadItem Method In DownloadManager.cs file", ex);
          }
      }

      public static List<DownloadStatus> GetDownloadList()
      {
          List<DownloadStatus> downloadstatuslist = new List<DownloadStatus>();
          try
          {
              downloadstatuslist = Task.Run(async () => await Constants.connection.Table<DownloadStatus>().ToListAsync()).Result;
          }
          catch (Exception ex)
          {
              
              Exceptions.SaveOrSendExceptions("Exception in GetDownloadList Method In DownloadManager.cs file", ex);
          }
          return downloadstatuslist;
      }

      public static List<DownLoadHistory> GetHistoryForVideos()
      {
          List<DownLoadHistory> DlHList = new List<DownLoadHistory>();
          try
          {
              var listfordlh = Task.Run(async () => await Constants.connection.Table<DownLoadHistory>().Where(i => i.UrlType == "Videos").OrderByDescending(j => j.ShowID).ToListAsync()).Result.Take(20);
              foreach (DownLoadHistory item in listfordlh)
              {
                  DownLoadHistory g1 = new DownLoadHistory();
                  g1.Title = item.Title.Substring(item.Title.LastIndexOf("/") + 1);
                    #if NOTANDROID
                  g1.LandingImage = ResourceHelper.getShowTileImage1("videos.jpg");
#endif
                  g1.ShowID = Convert.ToInt32(item.ShowID);
                  DlHList.Add(g1);
              }
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in GetHistoryForVideos Method In DownloadManager.cs file", ex);
          }
          return DlHList;
      }

      public static void SaveShowlistItems(string Title, string desc)
      {
          try
          {

              string ext = System.IO.Path.GetExtension(Title);
              string lenght = Convert.ToString(Title.Length);
              ShowList vt = new ShowList();
              int NextId = 0;
              //if (SettingsHelper.getStringValue("DownloadFolder") == "0")
              //{
              var xquery = Task.Run(async () => await Constants.connection.Table<ShowList>().OrderByDescending(j => j.ShowID).ToListAsync()).Result;
              if (xquery.Count() > 0)
              {
                  foreach (var itm in xquery)
                  {
                      int id = itm.ShowID;

                      NextId = id + 1;
                      break;
			} 
              }
              else
              {
                  NextId = 1;
              }
              vt.ShowID = NextId;
              AppSettings.ShowID = NextId.ToString();
              vt.Title = Title;
              vt.Rating = 0;
              vt.ReleaseDate = DateTime.Now.Date.ToString();
              vt.Status = "Custom";
              if (!Task.Run(async () => await Storage.FileExists("Images\\scale-100\\" + Title + ".jpg")).Result)
              vt.TileImage = "Vlogo.jpg";
              else
                  vt.TileImage = Title + ".jpg";
              vt.Genre = "Videos";
               vt.Description = desc;
              vt.CreatedDate = DateTime.Now;
              vt.IsFavourite = false;
              vt.SubTitle = "None";
		vt.IsHidden = false;
              //vt.Status = "Downloading..";
              vt.ClientPreferenceUpdated = DateTime.Now;
              vt.ClientShowUpdated = DateTime.Now;
		string command = "insert into ShowList(ShowID,Title,Rating,ReleaseDate,Status,TileImage,Genre,Description,CreatedDate,IsFavourite,SubTitle,IsHidden,ClientPreferenceUpdated,ClientShowUpdated)values("+vt.ShowID+ ",'" + vt.Title + "'," + 0+ ",'" +vt.ReleaseDate + "','" + "Custom" + "','" +vt.TileImage+ "','" +vt.Genre+ "','" + vt.Description + "','" + vt.CreatedDate+ "','" + vt.IsFavourite + "','" + vt.SubTitle + "','"+0+"','"+vt.ClientPreferenceUpdated+"','"+vt.ClientShowUpdated+"') ";		
		      Task.Run(async () => await Constants.connection.QueryAsync<ShowList>(command));
		//Task.Run(async () => await Constants.connection.InsertAsync(vt));

          }
          catch (Exception ex)
          {
              ex.Data.Add("url", Title);
              Exceptions.SaveOrSendExceptions("Exception in SaveShowlistItems  Method In downloadmanger.cs file", ex);
          }
      }

     

      public static ListGroup GetTopratedShows()
      {
            #if NOTANDROID
          ListGroup g = new ListGroup("downloads >", new Thickness(230, 6, 0, 0));
#endif
#if ANDROID
          ListGroup g = new ListGroup();
#endif
          try
          {
              var query = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(l => l.IsHidden == false).OrderByDescending(j => j.Rating).OrderByDescending(k => k.ShowID).ToListAsync()).Result;
              foreach (var item in query.Take(20))
              {
                  ShowList g1 = new ShowList();
                    #if NOTANDROID
                  g1.LandingImage = ResourceHelper.getShowTileImage1(item.TileImage);
#endif
                  g1.ShowID = Convert.ToInt32(item.ShowID);
                  g1.Genre = item.Genre;
                  g1.Title = item.Title;
                  g1.TileImage = item.TileImage;
                  g1.TitleForDownLoad = item.Title.Substring(item.Title.LastIndexOf("/") + 1);
                  g.Items.Add(g1);
              }
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in GetTopratedAndRecent Method In OnlineShow.cs file", ex);
          }
          return g;
      }


      public static ListGroup GetRecentShows()
      {
            #if NOTANDROID
          ListGroup g = new ListGroup("downloads >", new Thickness(230, 6, 0, 0));
#endif
#if ANDROID
          ListGroup g = new ListGroup();
#endif
          try
          {
              var query = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(l => l.IsHidden == false).OrderByDescending(j => j.CreatedDate).OrderByDescending(k => k.Rating).ToListAsync()).Result;
              foreach (var item in query.Take(20))
              {
                  ShowList g1 = new ShowList();
                    #if NOTANDROID
                  g1.LandingImage = ResourceHelper.getShowTileImage1(item.TileImage);
#endif
                  g1.ShowID = Convert.ToInt32(item.ShowID);
                  g1.Genre = item.Genre;
                  g1.Title = item.Title;
                  g1.TileImage = item.TileImage;
                  g1.TitleForDownLoad = item.Title.Substring(item.Title.LastIndexOf("/") + 1);
                  g.Items.Add(g1);
              }
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in GetTopratedAndRecent Method In OnlineShow.cs file", ex);
          }
          return g;
      }


      public static void InsertFavoriteLinks(DownloadPivot item,int count)
      {
          try
          {
              FavoriteLinksTable links = new FavoriteLinksTable();
              links.Title = item.title;
              links.LinkUrl = item.Downloaduri.ToString();
              links.ChildLinksCount=count;
              Task.Run(async () => await Constants.connection.InsertAsync(links));
          }
          catch (Exception ex)
          {
             
          }

      }

      public static void RemoveFavLinks(FavoriteLinksTable deleteselectedfav)
      {
          try
          {
              string command = "Delete from FavoriteLinksTable where ID=" + deleteselectedfav.ID;
              //Task.Run(async () => await Constants.connection.DeleteAsync(deleteselectedfav));
              Task.Run(async () => await Constants.connection.QueryAsync<FavoriteLinksTable>(command));
          }
          catch (Exception ex)
          {
             
          }
      }

      public static void UpDateChildLinkCount(int childcount,int id)
      {
          try
          {
              string command = "Update FavoriteLinksTable set ChildLinksCount="+ childcount + " " + "where ID=" + " " + id;
              Task.Run(async () => await Constants.connection.QueryAsync<FavoriteLinksTable>(command));
          }
          catch (Exception ex)
          {
              
             
          }
      }

      //*** Code for Wp8

      public static bool CompareCreateplayListTitle(string Title)
      {
          bool ExistOrNotExit = false;
          try
          {
              List<ShowList> showlist = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.Title == Title).ToListAsync()).Result;
              if (showlist.Count() != 0)
              {
                  ExistOrNotExit = true;
              }
              else
              {
                  ExistOrNotExit = false;
              }
          }
          catch (Exception ex)
          {


          }
          return ExistOrNotExit;
      }
      public static void UpdatePlayListTitleAndDescrption(string Title, string Descrption)
      {
          try
          {
              int showid = int.Parse(AppSettings.ShowID);
              List<ShowList> showlist = Task.Run(async () => await Constants.connection.Table<ShowList>().Where(i => i.ShowID == showid).ToListAsync()).Result;
              var item = showlist.FirstOrDefault();
              item.Title = Title;
              item.Description = Descrption;
              item.Rating = item.Rating;
              if (!Task.Run(async () => await Storage.FileExists("Images\\scale-100\\" + Title + ".jpg")).Result)
                  item.TileImage = "Vlogo.jpg";
              else
                  item.TileImage = Title + ".jpg";
              Constants.connection.UpdateAsync(item);
          }
          catch (Exception ex)
          {


          }
      }
      public static void DeleteYoutubeDownloadUrls(int id)
      {
          try
          {
              var xquery = Task.Run(async () => await Constants.connection.Table<DownloadTable>().Where(i => i.ID == id).ToListAsync()).Result;
              if (xquery.Count() > 0)
              {
                  DownloadTable ds = xquery.FirstOrDefault();
                  Task.Run(async () => await Constants.connection.DeleteAsync(ds));
              }
          }
          catch (Exception ex)
          {
              Exceptions.SaveOrSendExceptions("Exception in DeleteDownloadUrls  Method In downloadmanger.cs file", ex);
          }
      }

    }

}
