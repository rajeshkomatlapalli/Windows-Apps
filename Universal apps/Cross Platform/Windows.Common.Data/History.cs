using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Common;
using Common.Library;
using OnlineVideos.Entities;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
#if WINDOWS_APP
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
using System.Threading;
#endif

namespace OnlineVideos.Data
{
	public class History
	{
        //LinkHistory devi = new LinkHistory();
#if WINDOWS_PHONE_APP
        public List<LinkHistory> LoadImageHistory(int FirstIndex, int LastIndex, int Count)
        {            
           return GetHistoryList(Constants.SaveImageHistoryFile, FirstIndex, LastIndex, Count, LinkType.Images);
        }
#endif
        //public static string path = Path.Combine (System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal), "History.xml");
        public void SaveImageHistory(string songID, string songName)
        {
            SaveHistoryItem(Constants.SaveImageHistoryFile, songID, songName);
        }
		private int GetMaxHistoryID(ref XDocument doc)
		{
			int ID = 1;
			try
			{
				var DescendingHistoryList = from i in doc.Descendants("h")
				                            orderby Convert.ToInt32(i.Attribute("id").Value) descending
				                            select i;
				if (DescendingHistoryList != null)
				{
					if (DescendingHistoryList.ElementAt(0) != null)
						ID = Convert.ToInt32(DescendingHistoryList.ElementAt(0).Attribute("id").Value);
					ID++;
				}
			}
			catch (Exception ex)
			{

			}
			return ID;
		}

		private void SaveHistoryRootItem(string file, string itemID, string itemName,string link,string rating)
		{  
			#if ANDROID
			string path = Path.Combine (System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal),file);
			#endif
			XDocument xdoc = new XDocument(new XElement("NewDataSet",
				new XElement("h",
					new XElement("id", 1),
					new XElement("mid", itemID),
					new XElement("cno", itemName),
					new XElement("link", link),
					new XElement("rating",rating))));

			#if NOTANDROID
			Storage.SaveFileFromDocument(file, xdoc);
			#endif
			#if ANDROID
			xdoc.Save (path);
			#endif
		}

		private async void SaveHistoryItem(string file, string itemID, string itemName)
		{
			#if ANDROID
			string path = Path.Combine (System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal),file);
			#endif
			XDocument xdoc = new XDocument ();
			#if WINDOWS_APP
			if (Task.Run(async()=> await Storage.FileExists(file)).Result)
			#endif
			#if WINDOWS_PHONE_APP && NOTANDROID
			if (Task.Run(async () => await Storage.FileExists(file.Replace(AppSettings.ProjectName,string.Empty).Replace("/",string.Empty))).Result)
			#endif
			#if ANDROID
			if (File.Exists (path))
				#endif
            {
                #if WINDOWS_PHONE_APP && NOTANDROID
                xdoc = await Storage.ReadFileAsDocument(file);
				#endif
				#if ANDROID
				xdoc = XDocument.Load (path);
				#endif
				if (xdoc.Document.Declaration != null)
				{
					int ID = GetMaxHistoryID(ref xdoc);

					var eles = from f in xdoc.Descendants("h") select f;

					int MaxHistoryNo = 30;

					if (eles.Count() >= MaxHistoryNo)
					{
						int HistoryIdToRemove = ID - MaxHistoryNo;
						(from r in xdoc.Descendants("h") where r.Attribute("id").Value == HistoryIdToRemove.ToString() select r).Remove();
					}
					XElement xele = new XElement("h",
						new XAttribute("id", ID),
						new XAttribute("mid", itemID),
						new XAttribute("cno", itemName));
					xdoc.Root.Add(xele);
					#if ANDROID
					xdoc.Save (path);
					#endif
					#if NOTANDROID
					Storage.SaveFileFromDocument(file, xdoc);
					#endif
				}
				else
					SaveHistoryRootItem(file, itemID, itemName);
			}
			else
				SaveHistoryRootItem(file, itemID, itemName);
		}

		private void SaveHistoryRootItem(string file, string itemID, string itemName)
		{
			#if ANDROID
			string path = Path.Combine (System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal), file);
			#endif
			XDocument xdoc = new XDocument(new XElement("mh",
				new XElement("h",
					new XAttribute("id", 1),
					new XAttribute("mid", itemID),
					new XAttribute("cno", itemName))));
			#if ANDROID
			xdoc.Save (path);
			#endif
			#if NOTANDROID
			Storage.SaveFileFromDocument(file, xdoc);
			#endif
		}
		private async void SaveHistoryItem(string file, string itemID, string itemName,string link,string rating)
		{
			#if ANDROID
			string path = Path.Combine (System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal), file);
			#endif
			XDocument xdoc = new XDocument ();
			try
			{
				#if NOTANDROID
				if (Task.Run(async () => await Storage.FileExists(file)).Result)
				#endif
				#if ANDROID
				if (File.Exists (path))
					#endif
                {
                    #if WINDOWS_PHONE_APP && NOTANDROID
                    xdoc = await Storage.ReadFileAsDocument(file);
					#endif
					#if ANDROID
					xdoc = XDocument.Load (path);
					#endif
#if WINDOWS_APP && NOTANDROID
                    xdoc = await Storage.ReadFileAsDocument(file);
#endif
                    if (xdoc.Document != null)
					{
                        #if WINDOWS_APP && NOTANDROID
                        if(AppSettings.ProjectName=="Video Mix")
                        {
                            List<XElement> ss = xdoc.Descendants("h").Where(i => i.Element("mid").Value == itemID.ToString()).ToList();
                            if(ss.Count!=0)
                           ss.Remove();
                        }
#endif
                        int ID = GetMaxHistoryID(ref xdoc);
						var eles = from f in xdoc.Descendants("h") select f;
						int MaxHistoryNo = 30;
						if (eles.Count() >= MaxHistoryNo)
						{
							int HistoryIdToRemove = ID - MaxHistoryNo;
							(from r in xdoc.Descendants("h") where r.Element("id").Value == HistoryIdToRemove.ToString() select r).Remove();
						}
						XElement xele = new XElement("h",
							new XElement("id", ID),
							new XElement("mid", itemID),
							new XElement("cno", itemName),
							new XElement("link", link),
							new XElement("rating",rating));
						xdoc.Root.Add(xele);
						#if ANDROID
						xdoc.Save (path);
						#endif
						#if NOTANDROID
						Storage.SaveFileFromDocument(file, xdoc);
						#endif
					}
					else
						SaveHistoryRootItem(file, itemID, itemName, link,rating);
				}
				else
					SaveHistoryRootItem(file, itemID, itemName, link,rating);
			}
			catch (Exception ex)
			{
				Exceptions.SaveOrSendExceptions("Exception in SaveHistoryItem Method In History.cs file", ex);
			}
		}

		public void SaveSongHistory(string songID, string songName)
		{
			SaveHistoryItem(Constants.SongHistoryFile, songID, songName);
		}

        public void SaveComedyHistory(string songID, string songName)
        {
            SaveHistoryItem(Constants.ComedyHistoryFile, songID, songName);
        }

		public void SaveAudioHistory(string songID, string songName,string link,string rating)
		{
			SaveHistoryItem(Constants.AudioHistoryFile, songID, songName,link,rating);
		}

		public void SaveVideoHistory(string songID, string songName, string link)
		{
			SaveHistoryItem(Constants.VideoHistoryFile, songID, songName, link,"");
		}

		public void SaveMovieHistory(string songID, string songName, string link)
		{
			SaveHistoryItem(Constants.MovieHistoryFile, songID, songName, link,"");
		}

		public void SaveMovieHistory(string movieID, string chapterNo)
		{
			SaveHistoryItem(Constants.MovieHistoryFile, movieID, chapterNo);
		}

		public int GetLastHisId(string file)
		{
			int Id = 0;
			try
			{
				XDocument xdoc = Task.Run(async()=>await Storage.ReadFileAsDocument(file)).Result;
				if (xdoc != null)
					Id = GetMaxHistoryID(ref xdoc) - 1;
			}
			catch (Exception ex)
			{
				ex.Data.Add("file", file);
				Exceptions.SaveOrSendExceptions("Exception in GetLastHisId Method In History.cs file", ex);
			}
			return Id;
		}
#if WINDOWS_PHONE_APP && NOTANDROID
		public List<LinkHistory> LoadSongHistory(int FirstIndex, int LastIndex, int Count)
		{
		return GetHistoryList(Constants.SongHistoryFile, FirstIndex, LastIndex, Count, LinkType.Songs);
		}
        public List<LinkHistory> LoadComedyHistory(int FirstIndex, int LastIndex, int Count)
        {
            return GetHistoryList(Constants.ComedyHistoryFile, FirstIndex, LastIndex, Count, LinkType.Comedy);
        }
		public List<LinkHistory> LoadMovieHistory(int FirstIndex, int LastIndex, int Count)
		{
		return GetHistoryList(Constants.MovieHistoryFile, FirstIndex, LastIndex, Count, LinkType.Movies);
		}
		public List<LinkHistory> LoadAudioHistory(int FirstIndex, int LastIndex, int Count)
		{
		return GetHistoryList(Constants.AudioHistoryFile, FirstIndex, LastIndex, Count, LinkType.Audio);
		}
#endif
        public void SaveAudioHistory(string songID, string songName)
		{
			SaveHistoryItem(Constants.AudioHistoryFile, songID, songName);
		}
		
        #if WINDOWS_PHONE_APP && NOTANDROID		        
        public List<LinkHistory> GetHistoryList(string file, int StartIndex, int LastIndex, int Count, LinkType linkType)
		{
		XDocument xdoc = new XDocument ();
		List<LinkHistory> LinkHistoryList = new List<LinkHistory>();
		try
		{
            if (Task.Run(async () => await Storage.FileExists(file.Replace(AppSettings.ProjectName, string.Empty).Replace("/", string.Empty))).Result)
            {
                xdoc = Task.Run(async () => await Storage.ReadFileAsDocument(file)).Result;

                var HistoryList = from f in xdoc.Descendants("h")
                                  where (Convert.ToInt32(f.Attribute("id").Value) >= StartIndex && Convert.ToInt32(f.Attribute("id").Value) <= LastIndex)
                                  select f;

                foreach (var history in HistoryList)
                {
                    long showID = Convert.ToInt32(history.Attribute("mid").Value);
                    string songname = history.Attribute("cno").Value;
                    ShowLinks LinkInfo = OnlineShow.GetLinkDetailByTitle(linkType, Convert.ToInt32(history.Attribute("mid").Value),
                    history.Attribute("cno").Value);
                    ShowList showInfo = OnlineShow.GetShowDetailForHistory(showID);
                    if (showInfo != null)
                    {
                        LinkHistory objHistory = new LinkHistory();
                        objHistory.ShowID = showID;
                        objHistory.ID = history.Attribute("id").Value;
                        if (LinkInfo != null)
                            objHistory.LinkUrl = LinkInfo.LinkUrl;
                        objHistory.SubTitle = showInfo.SubTitle;

                        if (ResourceHelper.AppName == "Web_Tile")
                        {
                            Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                        {
                            objHistory.ImgSource = Storage.ReadBitmapImageFromFileForWp8(Constants.ImagePath + showInfo.TileImage, BitmapCreateOptions.None);
                        });
                        }
                        else if (ResourceHelper.AppName == "Video_Mix")
                        {
                            Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                         {
                             objHistory.ImgSource = Storage.ReadBitmapImageFromFileForWp8(Constants.ImagePath + showInfo.TileImage, BitmapCreateOptions.None);
                             objHistory.SubTitle = null;
                         });
                        }
                        else if (ResourceHelper.AppName == Apps.Web_Media.ToString())
                        {
                            Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                           {
                               objHistory.ImgSource = Storage.ReadBitmapImageFromFileForWp8(AppSettings.ProjectName + Constants.ImagePath + showInfo.TileImage, BitmapCreateOptions.None);
                               objHistory.SubTitle = null;
                           });
                        }
                        else
                        {
                            //Task.Run(async () =>
                            //{
                            //   await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate()
                            //        {
                            try
                            {
                                BitmapImage bimage = new BitmapImage();
                                string url = "ms-appx://" + Constants.ImagePath + showInfo.TileImage;
                                bimage.UriSource = new Uri(url, UriKind.RelativeOrAbsolute);
                                objHistory.ImgSource = bimage;
                            }
                            catch (Exception ex)
                            {
                                string msg = ex.Message;
                                objHistory.ImgSource = null;
                            }
                            //        });
                            //});
                        }
                        //if (ResourceHelper.AppName == Apps.DownloadManger.ToString())
                        //    objHistory.Title = showInfo.Title.Substring(showInfo.Title.LastIndexOf('/') + 1).ToString();
                        //else
                        objHistory.Title = showInfo.Title;
                        objHistory.Genre = showInfo.Genre;
                        objHistory.ReleaseDate = showInfo.ReleaseDate;
                        objHistory.ImageRating = ImageHelper.LoadRatingImage(showInfo.Rating.ToString()).ToString();

                        if (ResourceHelper.AppName == Apps.Cricket_Videos.ToString())
                            objHistory.LinkTitle = history.Attribute("cno").Value;
                        else if (ResourceHelper.AppName == Apps.Web_Media.ToString())
                            objHistory.LinkTitle = history.Attribute("cno").Value;
                        else
                            objHistory.LinkTitle = showInfo.Title + " - " + history.Attribute("cno").Value;

                        if (AppSettings.SongID == objHistory.ID.ToString() && AppSettings.AudioImage == Constants.PlayImagePath)
                            objHistory.Songplay = Constants.SongPlayPath;
                        else
                            objHistory.Songplay = Constants.PlayImagePath;
                        LinkHistoryList.Add(objHistory);
                    }
                }
                if (LinkHistoryList.Count == Count && StartIndex != 1)
                    LinkHistoryList.Add(new LinkHistory() { LinkTitle = "get more" });
            }
		}
		catch (Exception ex)
		{
		Exceptions.SaveOrSendExceptions("Exception in GetHistoryList Method In History.cs file", ex);
		}
		return LinkHistoryList;
		}
		#endif
		#if WINDOWS_APP
		public static ImageSource loadBitmapImageInBackground(string imagefile, UserControl thread, AutoResetEvent aa)
		{
		BitmapImage image = null;
		thread.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
		{

		image = new BitmapImage(new Uri(imagefile, UriKind.RelativeOrAbsolute));
		aa.Set();
		});
		aa.WaitOne();
		return image;
		}
		#endif

		public List<LinkHistory> GetHistoryList(string FileName)
		{
			List<LinkHistory> objHistoryList = new List<LinkHistory>();
			try
			{
				#if NOTANDROID
				if (Task.Run(async () => await Storage.FileExists(FileName)).Result)
				#endif
				#if ANDROID
				string path = Path.Combine (System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal),FileName);
				if (File.Exists (path))
				#endif
				{
					#if NOTANDROID
					StorageFolder store = ApplicationData.Current.LocalFolder;
					StorageFile hfile = Task.Run(async () => await store.CreateFileAsync(FileName, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
					if (hfile != null)
					{
					var f = Task.Run(async () => await hfile.OpenAsync(FileAccessMode.Read)).Result;
					IInputStream inputStream = f.GetInputStreamAt(0);
					DataReader dataReader = new DataReader(inputStream);
					uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
					string timestamp = (dataReader.ReadString(numBytesLoaded)).ToString();
					System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(timestamp));
					var xdoc = XDocument.Load(ms);
					#endif
                        #if ANDROID
					var xdoc = XDocument.Load(path);
#endif
					var hisElements = from p in xdoc.Descendants("h")
					                  select p;
					foreach (var hisEle in hisElements)
					{
						LinkHistory objHistory = new LinkHistory();
						#if NOTANDROID
						objHistory.Thumbnail = new BitmapImage(new Uri("http://img.youtube.com/vi/" + hisEle.Element("link").Value + "/default.jpg", UriKind.Absolute));
						#endif
						#if ANDROID && NOTIOS
						objHistory.Thumbnail =  "http://img.youtube.com/vi/" +hisEle.Element("link").Value+ "/default.jpg";
						#endif
						objHistory.ShowID = Int32.Parse(hisEle.Element("mid").Value);
						objHistory.LinkUrl = hisEle.Element("link").Value;
						objHistory.ImageRating = hisEle.Element("rating").Value;
						#if NOTANDROID
						if (AppResources.ShowthumbnailTitle == "true")
						{
						string ShowTitle = OnlineShow.GetMovieTitle(hisEle.Element("mid").Value);
						objHistory.Title = ShowTitle + " - " + hisEle.Element("cno").Value;
						}
						else
						{
						#endif
						objHistory.Title = hisEle.Element("cno").Value;
						#if NOTANDROID
						}
						#endif
						objHistoryList.Add(objHistory);
					}
					#if NOTANDROID
					dataReader.DetachStream();
					inputStream.Dispose();
					dataReader.Dispose();
					f.Dispose();
					#endif
				}
				#if NOTANDROID
				}
				#endif
			}
			catch (Exception ex)
			{
				Exceptions.SaveOrSendExceptions("Exception in GetHistoryList Method In History.cs file", ex);
			}
            objHistoryList.Reverse(0, objHistoryList.Count);
			return objHistoryList;
		}

		public List<ShowList> GetHistoryByShows(string FileName)
		{
			#if ANDROID
			string path = Path.Combine (System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal),FileName);
			#endif
			List<LinkHistory> objHistoryList = new List<LinkHistory>();
			List<ShowList> objfilmography = new List<ShowList>();
			List<ShowCast> objHistoryshows = new List<ShowCast>();
			List<ShowList> objMovieList = new List<ShowList>();
			try
			{
				#if NOTANDROID
				if (Task.Run(async () => await Storage.FileExists(FileName)).Result)
				#endif
				#if ANDROID
				if (File.Exists (path))
					#endif
				{
					#if NOTANDROID
					StorageFolder store = ApplicationData.Current.LocalFolder;
					StorageFile hfile = Task.Run(async () => await store.CreateFileAsync(FileName, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
					if (hfile != null)
					{
					var f = Task.Run(async () => await hfile.OpenAsync(FileAccessMode.Read)).Result;
					IInputStream inputStream = f.GetInputStreamAt(0);
					DataReader dataReader = new DataReader(inputStream);
					uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
					string timestamp = (dataReader.ReadString(numBytesLoaded)).ToString();
					System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(timestamp));
					var xdoc = XDocument.Load(ms);
					#endif
#if ANDROID
					var xdoc = XDocument.Load(path);
#endif
					var hisElements = from p in xdoc.Descendants("h")

					                  select p;
					foreach (var hisEle in hisElements)
					{
						LinkHistory objHistory = new LinkHistory();
						#if ANDROID
						objHistory.ShowID = Int32.Parse(hisEle.Attribute("mid").Value);
						#endif
						#if NOTANDROID
						objHistory.ShowID = Int32.Parse(hisEle.Element("mid").Value);
						#endif
						DataManager<ShowList> datamanager1 = new DataManager<ShowList>();
						ShowList objdetail = new ShowList();
						objdetail = datamanager1.GetFromList(i => i.ShowID == objHistory.ShowID);
						//objdetail.Image = loadBitmapImageInBackground(ResourceHelper.getViewAllImageFromStorageOrInstalledFolder(objdetail.TileImage), thread, aa);
						#if NOTANDROID
						objdetail.LandingImage = ResourceHelper.getViewAllImageFromStorageOrInstalledFolder(objdetail.TileImage);
						#endif
						objdetail.ShowID = Convert.ToInt32(objdetail.ShowID);
						objdetail.Title = objdetail.Title;
						objdetail.TileImage = objdetail.TileImage;
						objdetail.RatingBitmapImage = ImageHelper.LoadRatingImage(objdetail.Rating.ToString());
						objdetail.ReleaseDate = objdetail.ReleaseDate;
						objdetail.SubTitle = "Subtitle: " + objdetail.SubTitle;
						//objdetail.Image = ResourceHelper.getShowTileImage(objdetail.TileImage) as ImageSource;
						objdetail.RatingBitmapImage = ImageHelper.LoadRatingImage(objdetail.Rating.ToString());
						objfilmography.Add(objdetail);
					}
					#if NOTANDROID
					}
					#endif
				}
			}
			catch (Exception ex)
			{
				Exceptions.SaveOrSendExceptions("Exception in GetHistoryByShows Method In History.cs file", ex);
			}
            objfilmography.Reverse(0, objfilmography.Count); 
            return objfilmography;
		}
	}
}