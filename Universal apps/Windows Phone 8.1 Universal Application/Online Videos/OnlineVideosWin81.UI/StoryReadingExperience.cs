using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Library;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Foundation;
using System.Collections.Generic;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Text;
using System.IO;

namespace OnlineVideos.UI
{
    public static class StoryReadingExperience
    {
        public static bool added = false;
        public static string TempText = string.Empty;
        public static Page Instance = default(Page);
        
        public static List<KeyValuePair<object, string>> MasterList = null;
        static int RowCount = 0;


        public static void DivideRowsasperViewableArea(string description, string image)
        {
            try
            {
                string[] currentrow = new string[2];
                double actualwidth = default(double);
                string Text = string.Empty;
                string des = string.Empty;
                if (added == true)
                {
                    actualwidth = 212 - (MeasureString(TempText));
                }
                else
                {
                    if (!string.IsNullOrEmpty(image))
                        actualwidth = 212;
                    else
                        actualwidth = 492;
                }
                if (actualwidth < MeasureString("A"))
                {
                    RowCount++;
                    if (RowCount <= StoryManager.MaxRows(AppSettings.ShowID, 1))
                    {
                        added = false;
                        TempText = string.Empty;
                        currentrow = StoryManager.ReadFromDatabase(AppSettings.ShowID, RowCount, 1);
                        currentrow[0] = currentrow[0].ToString().Trim();
                        currentrow[0] = currentrow[0].ToString().Trim(Environment.NewLine.ToCharArray());
                        DivideRowsasperViewableArea(description + Environment.NewLine + "                    " + currentrow[0].ToString(), currentrow[1].ToString());
                    }
                    else
                    {

                        string dess = string.Empty;
                        string dess1 = string.Empty;
                        if (TempText != "")
                        {
                            dess = description.Substring(TempText.Length);
                            dess1 = dess;
                        }
                        else
                        {
                            dess = description;
                            dess1 = dess;
                        }


                        if (492 < MeasureString(dess))
                        {
                        divide:
                            string RemainingText = string.Empty;
                            while (492 < MeasureString(dess))
                            {
                                RemainingText = dess.Substring(0, dess.LastIndexOfAny(new char[] { ' ', '.', ',', '"', '?' }));
                                RemainingText = dess.Substring(0, RemainingText.LastIndexOf(' '));
                                dess = RemainingText;
                            }
                            if (MeasureString(dess1.Substring(RemainingText.Length)) > 492)
                            {
                                Tuple<string, string> T = new Tuple<string, string>(RemainingText, "");
                                MasterList.Add(new KeyValuePair<object, string>(T, ""));
                                dess = dess1.Substring(RemainingText.Length);
                                dess1 = dess;
                                goto divide;

                            }
                            else
                            {
                                Tuple<string, string> T = new Tuple<string, string>(RemainingText, "");
                                MasterList.Add(new KeyValuePair<object, string>(T, ""));
                                if (MeasureString(dess1.Substring(RemainingText.Length)) < 412)
                                {
                                    Tuple<string, string> T1 = new Tuple<string, string>(dess1.Substring(RemainingText.Length), "End-marker.png");
                                    MasterList.Add(new KeyValuePair<object, string>(T1, ""));
                                    added = false;
                                }
                                else
                                {
                                    Tuple<string, string> T2 = new Tuple<string, string>(dess1.Substring(RemainingText.Length), "");
                                    MasterList.Add(new KeyValuePair<object, string>(T2, ""));
                                    Tuple<string, string> T1 = new Tuple<string, string>("", "End-marker.png");
                                    MasterList.Add(new KeyValuePair<object, string>(T1, ""));
                                    added = false;
                                }
                            }

                        }
                        else
                        {
                            if (MeasureString(dess) < 412)
                            {
                                Tuple<string, string> T = new Tuple<string, string>(dess, "End-marker.png");
                                MasterList.Add(new KeyValuePair<object, string>(T, ""));
                                added = false;
                            }
                            else
                            {
                                Tuple<string, string> T2 = new Tuple<string, string>(dess, "");
                                MasterList.Add(new KeyValuePair<object, string>(T2, ""));
                                Tuple<string, string> T1 = new Tuple<string, string>("", "End-marker.png");
                                MasterList.Add(new KeyValuePair<object, string>(T1, ""));
                                added = false;
                            }
                        }

                    }
                }
                else
                {
                    if (actualwidth < MeasureString(description))
                    {
                        des = description;
                        while (actualwidth < MeasureString(des))
                        {
                            Text = description.Substring(0, des.LastIndexOfAny(new char[] { ' ', '.', ',', '"', '?' }));
                            Text = description.Substring(0, Text.LastIndexOf(' '));
                            des = Text;
                        }
                        if (added == true)
                        {
                            TempText = string.Empty;
                            MasterList[MasterList.Count - 1] = new KeyValuePair<object, string>(MasterList[MasterList.Count - 1].Key, Text);
                            added = false;
                        }
                        else
                        {
                            Tuple<string, string> T = new Tuple<string, string>(Text, image);
                            MasterList.Add(new KeyValuePair<object, string>(T, ""));
                            TempText = Text;
                            added = false;
                        }
                        RowCount++;
                        if (RowCount <= StoryManager.MaxRows(AppSettings.ShowID, 1))
                        {
                            currentrow = StoryManager.ReadFromDatabase(AppSettings.ShowID, RowCount, 1);
                            currentrow[0] = currentrow[0].ToString().Trim();
                            currentrow[0] = currentrow[0].ToString().Trim(Environment.NewLine.ToCharArray());
                            DivideRowsasperViewableArea(description.Substring(Text.Length) + Environment.NewLine + "                    " + currentrow[0].ToString(), currentrow[1].ToString());
                        }
                        else
                        {
                            string dess = string.Empty;
                            string dess1 = string.Empty;
                            if (TempText != "")
                            {
                                dess = description.Substring(TempText.Length);
                                dess1 = dess;
                            }
                            else
                            {
                                dess = description;
                                dess1 = dess;
                            }


                            if (492 < MeasureString(dess))
                            {
                            divide:
                                string RemainingText = string.Empty;
                                while (492 < MeasureString(dess))
                                {
                                    RemainingText = dess.Substring(0, dess.LastIndexOfAny(new char[] { ' ', '.', ',', '"', '?' }));
                                    RemainingText = dess.Substring(0, RemainingText.LastIndexOf(' '));
                                    dess = RemainingText;
                                }

                                if (MeasureString(dess1.Substring(RemainingText.Length)) > 492)
                                {
                                    Tuple<string, string> T = new Tuple<string, string>(RemainingText, "");
                                    MasterList.Add(new KeyValuePair<object, string>(T, ""));
                                    dess = dess1.Substring(RemainingText.Length);
                                    dess1 = dess;
                                    goto divide;

                                }
                                else
                                {
                                    Tuple<string, string> T = new Tuple<string, string>(RemainingText, "");
                                    MasterList.Add(new KeyValuePair<object, string>(T, ""));
                                    if (MeasureString(dess1.Substring(RemainingText.Length)) < 412)
                                    {
                                        Tuple<string, string> T1 = new Tuple<string, string>(dess1.Substring(RemainingText.Length), "End-marker.png");
                                        MasterList.Add(new KeyValuePair<object, string>(T1, ""));
                                        added = false;
                                    }
                                    else
                                    {
                                        Tuple<string, string> T2 = new Tuple<string, string>(dess1.Substring(RemainingText.Length), "");
                                        MasterList.Add(new KeyValuePair<object, string>(T2, ""));
                                        Tuple<string, string> T1 = new Tuple<string, string>("", "End-marker.png");
                                        MasterList.Add(new KeyValuePair<object, string>(T1, ""));
                                        added = false;
                                    }
                                }

                            }
                            else
                            {
                                if (MeasureString(dess) < 412)
                                {
                                    Tuple<string, string> T = new Tuple<string, string>(dess, "End-marker.png");
                                    MasterList.Add(new KeyValuePair<object, string>(T, ""));
                                    added = false;
                                }
                                else
                                {
                                    Tuple<string, string> T2 = new Tuple<string, string>(dess, "");
                                    MasterList.Add(new KeyValuePair<object, string>(T2, ""));
                                    Tuple<string, string> T1 = new Tuple<string, string>("", "End-marker.png");
                                    MasterList.Add(new KeyValuePair<object, string>(T1, ""));
                                    added = false;
                                }
                            }

                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(image) || added == true)
                        {
                            if (added == true)
                            {
                                TempText = string.Empty;
                                MasterList[MasterList.Count - 1] = new KeyValuePair<object, string>(MasterList[MasterList.Count - 1].Key, description);
                                added = false;
                            }
                            else
                            {
                                Tuple<string, string> T = new Tuple<string, string>(description, image);
                                MasterList.Add(new KeyValuePair<object, string>(T, ""));
                                TempText = description;
                                added = true;
                            }
                            RowCount++;
                            if (RowCount <= StoryManager.MaxRows(AppSettings.ShowID, 1))
                            {
                                currentrow = StoryManager.ReadFromDatabase(AppSettings.ShowID, RowCount, 1);
                                currentrow[0] = currentrow[0].ToString().Trim();
                                currentrow[0] = currentrow[0].ToString().Trim(Environment.NewLine.ToCharArray());
                                DivideRowsasperViewableArea("                    " + currentrow[0].ToString(), currentrow[1].ToString());
                            }
                            else
                            {
                                string dess = string.Empty;
                                string dess1 = string.Empty;
                                if (TempText != "")
                                {
                                    dess = description.Substring(TempText.Length);
                                    dess1 = dess;
                                }
                                else
                                {
                                    dess = description;
                                    dess1 = dess;
                                }


                                if (492 < MeasureString(dess))
                                {
                                divide:
                                    string RemainingText = string.Empty;
                                    while (492 < MeasureString(dess))
                                    {
                                        RemainingText = dess.Substring(0, dess.LastIndexOfAny(new char[] { ' ', '.', ',', '"', '?' }));
                                        RemainingText = dess.Substring(0, RemainingText.LastIndexOf(' '));
                                        dess = RemainingText;
                                    }
                                    if (MeasureString(dess1.Substring(RemainingText.Length)) > 492)
                                    {
                                        Tuple<string, string> T = new Tuple<string, string>(RemainingText, "");
                                        MasterList.Add(new KeyValuePair<object, string>(T, ""));
                                        dess = dess1.Substring(RemainingText.Length);
                                        dess1 = dess;
                                        goto divide;

                                    }
                                    else
                                    {
                                        Tuple<string, string> T = new Tuple<string, string>(RemainingText, "");
                                        MasterList.Add(new KeyValuePair<object, string>(T, ""));
                                        if (MeasureString(dess1.Substring(RemainingText.Length)) < 412)
                                        {
                                            Tuple<string, string> T1 = new Tuple<string, string>(dess1.Substring(RemainingText.Length), "End-marker.png");
                                            MasterList.Add(new KeyValuePair<object, string>(T1, ""));
                                            added = false;
                                        }
                                        else
                                        {
                                            Tuple<string, string> T2 = new Tuple<string, string>(dess1.Substring(RemainingText.Length), "");
                                            MasterList.Add(new KeyValuePair<object, string>(T2, ""));
                                            Tuple<string, string> T1 = new Tuple<string, string>("", "End-marker.png");
                                            MasterList.Add(new KeyValuePair<object, string>(T1, ""));
                                            added = false;
                                        }
                                    }

                                }
                                else
                                {
                                    if (MeasureString(dess) < 412)
                                    {
                                        Tuple<string, string> T = new Tuple<string, string>(dess, "End-marker.png");
                                        MasterList.Add(new KeyValuePair<object, string>(T, ""));
                                        added = false;
                                    }
                                    else
                                    {
                                        Tuple<string, string> T2 = new Tuple<string, string>(dess, "");
                                        MasterList.Add(new KeyValuePair<object, string>(T2, ""));
                                        Tuple<string, string> T1 = new Tuple<string, string>("", "End-marker.png");
                                        MasterList.Add(new KeyValuePair<object, string>(T1, ""));
                                        added = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Text = description;
                            RowCount++;
                            if (RowCount <= StoryManager.MaxRows(AppSettings.ShowID, 1))
                            {
                                currentrow = StoryManager.ReadFromDatabase(AppSettings.ShowID, RowCount, 1);
                                currentrow[0] = currentrow[0].ToString().Trim();
                                currentrow[0] = currentrow[0].ToString().Trim(Environment.NewLine.ToCharArray());
                                DivideRowsasperViewableArea(Text + Environment.NewLine + "                    " + currentrow[0].ToString(), currentrow[1].ToString());
                            }
                            else
                            {
                                string dess = string.Empty;
                                string dess1 = string.Empty;

                                dess = description;
                                dess1 = dess;



                                if (492 < MeasureString(dess))
                                {
                                divide:
                                    string RemainingText = string.Empty;
                                    while (492 < MeasureString(dess))
                                    {
                                        RemainingText = dess.Substring(0, dess.LastIndexOfAny(new char[] { ' ', '.', ',', '"', '?' }));
                                        RemainingText = dess.Substring(0, RemainingText.LastIndexOf(' '));
                                        dess = RemainingText;
                                    }
                                    if (MeasureString(dess1.Substring(RemainingText.Length)) > 492)
                                    {
                                        Tuple<string, string> T = new Tuple<string, string>(RemainingText, "");
                                        MasterList.Add(new KeyValuePair<object, string>(T, ""));
                                        dess = dess1.Substring(RemainingText.Length);
                                        dess1 = dess;
                                        goto divide;

                                    }
                                    else
                                    {
                                        Tuple<string, string> T = new Tuple<string, string>(RemainingText, "");
                                        MasterList.Add(new KeyValuePair<object, string>(T, ""));
                                        if (MeasureString(dess1.Substring(RemainingText.Length)) < 412)
                                        {
                                            Tuple<string, string> T1 = new Tuple<string, string>(dess1.Substring(RemainingText.Length), "End-marker.png");
                                            MasterList.Add(new KeyValuePair<object, string>(T1, ""));
                                            added = false;
                                        }
                                        else
                                        {
                                            Tuple<string, string> T2 = new Tuple<string, string>(dess1.Substring(RemainingText.Length), "");
                                            MasterList.Add(new KeyValuePair<object, string>(T2, ""));
                                            Tuple<string, string> T1 = new Tuple<string, string>("", "End-marker.png");
                                            MasterList.Add(new KeyValuePair<object, string>(T1, ""));
                                            added = false;
                                        }
                                    }

                                }
                                else
                                {
                                    if (MeasureString(dess) < 412)
                                    {
                                        Tuple<string, string> T = new Tuple<string, string>(dess, "End-marker.png");
                                        MasterList.Add(new KeyValuePair<object, string>(T, ""));
                                        added = false;
                                    }
                                    else
                                    {
                                        Tuple<string, string> T2 = new Tuple<string, string>(dess, "");
                                        MasterList.Add(new KeyValuePair<object, string>(T2, ""));
                                        Tuple<string, string> T1 = new Tuple<string, string>("", "End-marker.png");
                                        MasterList.Add(new KeyValuePair<object, string>(T1, ""));
                                        added = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
                Exceptions.SaveOrSendExceptions("Exception in DivideRowsasperViewableArea Method In StoryReadingExperience.cs file", ex);
            }
        }

        public static void StartRetriving()
        {
            try
            {
                TempText = string.Empty;
                MasterList = new List<KeyValuePair<object, string>>();
                string[] firstRecord = new string[2];
                for (RowCount = 1; RowCount < StoryManager.MaxRows(AppSettings.ShowID, 1) + 1; RowCount++)
                {
                    firstRecord = StoryManager.ReadFromDatabase(AppSettings.ShowID, RowCount, 1);

                    firstRecord[0] = firstRecord[0].ToString().Trim();
                    firstRecord[0] = firstRecord[0].ToString().Trim(Environment.NewLine.ToCharArray());
                    DivideRowsasperViewableArea("                    " + firstRecord[0].ToString(), firstRecord[1].ToString());
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in StartRetriving Method In StoryReadingExperience.cs file", ex);
            }
        }

        public static ImageSource loadBitmapImageInBackground(string imagefile)
        {
            BitmapImage image = null;
            image = new BitmapImage(new Uri(imagefile, UriKind.RelativeOrAbsolute));
            return image;
        }

        public static double MeasureString(string text)
        {
            double height = 0;
            Instance.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    TextBlock sampletext = new TextBlock();
                    sampletext.TextWrapping = TextWrapping.Wrap;
                    sampletext.Text = text;
                    sampletext.Width = 442;
                    if (AppSettings.ProjectName == "Vedic Library")
                    {
                        //sampletext.FontWeight = FontWeights.Bold;
                        // sampletext.FontFamily = new FontFamily("/Balham.ttf#Balham");
                    }
                    else
                    {
                        sampletext.FontFamily = new FontFamily("/Organic_Elements.TTF#Organic Elements");
                    }
                    sampletext.FontSize = 20;
                    sampletext.TextTrimming = TextTrimming.WordEllipsis;
                    sampletext.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    height = sampletext.DesiredSize.Height;
                }).AsTask().Wait();
            return height;
        }
        public static bool AddMarkers(int pagenumber, MediaElement media)
        {
            bool exists = false;
            try
            {
                if (Storage.FileExistsInMusicLibrary("StoryRecordings-8.xml"))
                {
                    XDocument xdoc = new XDocument();
                    StorageFolder store = ApplicationData.Current.LocalFolder;
                    var story = Task.Run(async () => await store.CreateFolderAsync("StoryRecordings", CreationCollisionOption.OpenIfExists)).Result;
                    var story1 = Task.Run(async () => await story.CreateFolderAsync(AppSettings.ShowID, CreationCollisionOption.OpenIfExists)).Result;

                    StorageFile file = Task.Run(async () => await story1.CreateFileAsync("StoryRecordings-8.xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
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
                    var data = (from p in xdoc.Descendants("Stories").Elements() where p.Attribute("pageno").Value == (pagenumber).ToString() select p).ToList();
                    if (data.Count() > 0)
                    {
                        foreach (var d in data)
                        {
                            exists = true;
                            //Constants.StartPosition = Convert.ToInt32(d.Element("Begin").Value);
                            //Constants.EndPosition = Convert.ToInt32(d.Element("End").Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in AddMarkers Method In StoryReadingExperience.cs file", ex);
            }
            return exists;
        }
        public static bool AddMusicMarkers(int pagenumber, MediaElement media)
        {
            bool exists = false;
            try
            {
                if (AppSettings.ProjectName == "Vedic Library")
                {
                    XDocument xmlDoc =
                    XDocument.Load("DefaultData/StoryReading.xml");
                    var findEle = from i in xmlDoc.Descendants("show") where i.Attribute("id").Value.ToString() == AppSettings.ShowID select i;
                    var data = from p in findEle.Descendants("story")
                               where (Convert.ToInt32(p.Attribute("id").Value)) == pagenumber
                               select p;
                    if (data.Count() > 0)
                    {
                        foreach (var d in data)
                        {
                            exists = true;
                            Constants.StartPosition = Convert.ToInt32(d.Element("begintime").Value);
                            Constants.EndPosition = Convert.ToInt32(d.Element("endtime").Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in AddMusicMarkers Method In StoryReadingExperience.cs file", ex);
            }
            return exists;
        }
        public async static void ListenStory(int pagenumber, MediaElement media)
        {
            try
            {
                if (media != null && Constants.mode == "Listen")
                {
                    bool exists = false;
                    if (Storage.FileExistsInMusicLibrary(AppSettings.Title + pagenumber+ ".wav".Trim()))
                    {
                        exists = AddMarkers(pagenumber, media);
                        StorageFolder stores = ApplicationData.Current.LocalFolder;
                        var storyfolder = Task.Run(async () => await stores.CreateFolderAsync("StoryRecordings", CreationCollisionOption.OpenIfExists)).Result;
                        var storyfile = Task.Run(async () => await storyfolder.CreateFolderAsync(AppSettings.ShowID, CreationCollisionOption.OpenIfExists)).Result;
                        StorageFile file1 = Task.Run(async () => await storyfile.CreateFileAsync(AppSettings.Title + pagenumber + ".wav".Trim(), CreationCollisionOption.OpenIfExists)).Result;
                        var fquery = Task.Run(async () => await file1.OpenAsync(FileAccessMode.Read)).Result;
                        if (exists == true)
                        {
                            media.SetSource(fquery, file1.ContentType);
                        }
                        else
                        {
                            media.Source = null;
                        }

                    }
                    else
                    {
                        media.Source = null;
                    }
                }
                else
                {
                    bool exists = false;
                    string title = StoryManager.Gettitle(AppSettings.ShowID);


                    var sf = Task.Run(async () => await ApplicationData.Current.LocalFolder.GetFolderAsync(AppSettings.ProjectName)).Result;
                    StorageFile file = default(StorageFile);
                    IReadOnlyList<StorageFile> file1 = Task.Run(async () => await sf.GetFilesAsync()).Result;
                    foreach (StorageFile t in file1.ToList())
                    {
                        string s = t.Name;

                        if (s == title + ".mp3")
                        {
                            exists = AddMusicMarkers(pagenumber, media);
                            Uri source = new Uri(t.Path, UriKind.RelativeOrAbsolute);
                            IRandomAccessStream writeStream = await t.OpenAsync(FileAccessMode.Read);
                            Stream outStream = Task.Run(() => writeStream.AsStreamForRead()).Result;
                            if (writeStream.Size != 0)
                            {
                                if (exists == true)
                                {
                                    media.SetSource(writeStream, "");
                                }
                            }
                            else
                            {
                                media.Source = null;
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ListenStory Method In StoryReadingExperience.cs file", ex);
            }
        }
     
        public static DataGroups RetriveFromStorage()
        {
            

                DataGroups dg = new DataGroups();
                try
                {
                DataGroup d = null;
                int pageno = 1;
                int index = 0;

                for (int i = 0; i < MasterList.Count; i++)
                {

                    if (index == 0)
                    {

                        if (Convert.ToString(Convert.ToDouble(MasterList.Count) / 2).Contains(".5"))
                            d = new DataGroup((pageno).ToString() + " / " + ((MasterList.Count / 2) + 1).ToString());
                        else
                            d = new DataGroup((pageno).ToString() + " / " + (MasterList.Count / 2).ToString());
                        pageno++;
                    }



                    Stories s = new Stories();

                    if (((Tuple<string, string>)MasterList.ElementAt(i).Key).Item1.StartsWith(Environment.NewLine))
                    {
                        s.Description = ((Tuple<string, string>)MasterList.ElementAt(i).Key).Item1.Substring(Environment.NewLine.Length);
                        if (!string.IsNullOrEmpty(MasterList[i].Value))
                            s.RemainingDescription = MasterList[i].Value.Substring(Environment.NewLine.Length);
                    }
                    else
                    {
                        s.Description = ((Tuple<string, string>)MasterList.ElementAt(i).Key).Item1;
                        if (!string.IsNullOrEmpty(MasterList[i].Value))
                            s.RemainingDescription = MasterList[i].Value;
                    }

                    if (!string.IsNullOrEmpty(((Tuple<string, string>)MasterList.ElementAt(i).Key).Item2))
                    {
                        s.ImageVisibility = "Visible";
                        s.Imageheight = 280;
                        s.Imagewidth = 440;
                        if (((Tuple<string, string>)MasterList.ElementAt(i).Key).Item2.ToString() != "End-marker.png")
                            s.StoryImage = ResourceHelper.getStoryImageFromStorageOrInstalledFolder(((Tuple<string, string>)MasterList.ElementAt(i).Key).Item2);
                        else
                        {
                            s.Imageheight = 80;
                            s.Imagewidth = 400;

                            s.StoryImage = "ms-appx:///Images/storyImages/End-marker.png";
                        }
                    }
                    else
                    {
                        s.ImageVisibility = "Collapsed";
                    }

                    index++;

                    d.Items.Add(s);
                    if (i == MasterList.Count - 1)
                    {
                        dg.Items.Add(d);
                    }
                    else
                    {
                        if (index == 2 || i == MasterList.Count)
                        {
                            index = 0;

                            dg.Items.Add(d);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in RetriveFromStorage Method In StoryReadingExperience.cs file", ex);
            }
            return dg;
        }


    }
}
