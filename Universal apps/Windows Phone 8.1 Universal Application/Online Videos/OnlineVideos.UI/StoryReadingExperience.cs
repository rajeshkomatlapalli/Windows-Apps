using System;
using System.Net;
using Common.Library;
using System.Windows.Input;
using System.Collections;
using OnlineVideos.Data;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
//using OnlineVideos.Common;
using OnlineVideos.Entities;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using System.Text;
using Windows.Storage.Streams;

//using Common.Common;

namespace OnlineVideos.UI
{
    public static class StoryReadingExperience
    {
        static DispatcherTimer buttonTimer = new DispatcherTimer();
        public static StackPanel messagestack = null;
        public static Image imgRight = null;
        public static Image ResumeButton = null;
        static Canvas can = new Canvas();
        static long begin = 0;
        static long end = 0;
        public static IDictionary<string, string> storyDictionary = null;
        public static string Image1 = string.Empty;
        static int RowCount = 0;

        public static int storystackmanipulationCompleted(Image imgstory, TextBlock txtpara, TextBlock txtpageno, int pagenum, Grid dummygrid, MediaElement mymedia, bool buttontapped)
        {
            RetriveFromStorage(imgstory, txtpara, txtpageno, pagenum, dummygrid, mymedia, buttontapped);
            return 0;
        }

        public static int storystackmanipulationdelta(Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e, int pagenumber, int slidestatus, StackPanel stk, Storyboard myStoryboard)
        {

            if (e == null)
            {

                if (pagenumber != storyDictionary.Count)
                {
                    pagenumber = ++pagenumber;
                    //animatepage("Left", stk);
                    myStoryboard.Begin();

                }

            }
            else
            {

                if (e.Delta.Translation.X < 0)
                {
                    if (pagenumber != storyDictionary.Count)
                    {
                        pagenumber = ++pagenumber;
                        //animatepage("Left", stk);
                        myStoryboard.Begin();
                    }

                }

                if (e.Delta.Translation.X > 0)
                {
                    if (pagenumber != 1)
                    {
                        pagenumber = --pagenumber;
                        //animatepage("Right", stk);
                        myStoryboard.Begin();
                    }
                }
            }
            return pagenumber;
        }
        public static void animatepage(string direction, StackPanel mainstk)
        {
            try
            {

                Storyboard flip = new Storyboard();
                DoubleAnimation animation = new DoubleAnimation()
                {
                    Duration = new TimeSpan(0, 0, 0, 0, 500)
                };
                flip.Children.Add(animation);
                if (direction == "Left")
                {
                    if (mainstk.Projection == null)
                    {

                        mainstk.Projection = new PlaneProjection()
                        {
                            CenterOfRotationX = -0.01
                        };
                    }
                    PlaneProjection projection = mainstk.Projection as PlaneProjection;

                    if (projection.GlobalOffsetX == 0)
                    {
                        animation.From = 460;
                        animation.To = 0;

                    }
                    else
                    {
                        animation.From = 460;
                        animation.To = 0;
                    }
                    Storyboard.SetTarget(animation, projection);
                }
                else if (direction == "Right")
                {
                    if (mainstk.Projection == null)
                    {
                        mainstk.Projection = new PlaneProjection()
                        {
                            CenterOfRotationX = -0.01
                        };
                    }

                    PlaneProjection projection = mainstk.Projection as PlaneProjection;
                    if (projection.GlobalOffsetX == 0)
                    {
                        animation.From = -460;
                        animation.To = 0;
                    }
                    else
                    {
                        animation.From = -460;
                        animation.To = 0;
                    }
                    Storyboard.SetTarget(animation, projection);
                }

                Storyboard.SetTargetProperty(animation, PlaneProjection.GlobalOffsetXProperty.ToString());

                flip.Begin();
            }
            catch (Exception ex)
            {

                string str = ex.Message;
            }
        }

        public static void storystackmouseenter(StackPanel Resume)
        {
            Resume.Visibility = Visibility.Visible;

            messagestack = Resume;
            buttonTimer.Interval = TimeSpan.FromSeconds(5);
            buttonTimer.Tick += buttonTimer_Tick;
            buttonTimer.Start();
        }

        static void buttonTimer_Tick(object sender, object e)
        {
            messagestack.Visibility = Visibility.Collapsed;
            buttonTimer.Stop();
        }
        public static void DivideRowsasperViewableArea(string description, string image, double font, Grid dummygrid)
        {
            try
            {
                string[] currentrow = new string[2];
                string Text = string.Empty;
                double controlheight = 0;
                TextBlock proto = new TextBlock();
                proto.FontSize = font;
                if (ResourceHelper.AppName == Apps.Story_Time.ToString())
                {
                    proto.FontFamily = new FontFamily("/COM4NRG_.TTF#COM4t Nuvu Regular");
                }
                proto.Width = 350;
                proto.Height = 500;
                //proto.Margin = new Thickness(400, 400, 0, 0);
                proto.TextWrapping = TextWrapping.Wrap;
                proto.Text = description;
                dummygrid.Children.Add(proto);
                dummygrid.UpdateLayout();
                Image1 = image;
                if (image == "")
                {
                    ShowList objlist = new ShowList();
                    objlist = OnlineShow.GetShowDetail(AppSettings.ShowUniqueID);
                    if (objlist.SubTitle == Constants.MovieCategoryTelugu || objlist.SubTitle == Constants.MovieCategoryHindi)
                    {
                        controlheight = 500;
                    }
                    else
                    {
                        controlheight = 500;
                    }

                }
                else
                {
                    if (proto.RenderSize.Height > 500)
                    {
                        ShowList objlist = OnlineShow.GetShowDetail(AppSettings.ShowUniqueID);
                        if (objlist.SubTitle == Constants.MovieCategoryTelugu || objlist.SubTitle == Constants.MovieCategoryHindi)
                        {
                            controlheight = 500;
                            image = "";
                        }
                        else
                        {
                            controlheight = 500;
                            image = "";
                        }
                    }
                    else
                    {
                        controlheight = 500;
                        image = Image1;
                    }
                }
                if (controlheight < proto.RenderSize.Height)
                {
                    while (controlheight < proto.RenderSize.Height)
                    {
                        Text = proto.Text.Substring(0, proto.Text.LastIndexOf(' '));
                        proto.Text = Text;
                        dummygrid.UpdateLayout();
                    }

                    if (image == Image1)
                    {
                        storyDictionary.Add(Text, image);
                        Image1 = "";

                    }
                    else
                    {
                        storyDictionary.Add(Text, image);
                    }
                    if (description.Substring(Text.Length).Length > 0)
                    {
                        ShowList objlist = OnlineShow.GetShowDetail(AppSettings.ShowUniqueID);
                        if (ResourceHelper.AppName == Apps.Vedic_Library.ToString() && objlist.SubTitle == Constants.MovieCategoryTelugu)
                        {
                            DivideRowsasperViewableArea(description.Substring(Text.Length), Image1, 23, dummygrid);

                        }
                        if (objlist.SubTitle == Constants.MovieCategoryEnglish)
                        {
                            if (ResourceHelper.AppName == Apps.Story_Time.ToString())
                            {
                                DivideRowsasperViewableArea(description.Substring(Text.Length), Image1, 30, dummygrid);
                            }
                            else
                            {
                                DivideRowsasperViewableArea(description.Substring(Text.Length), Image1, 23, dummygrid);
                            }

                        }
                        if (ResourceHelper.AppName == Apps.Vedic_Library.ToString() && objlist.SubTitle == Constants.MovieCategoryHindi)
                        {
                            DivideRowsasperViewableArea(description.Substring(Text.Length), Image1, 23, dummygrid);
                        }

                    }
                }
                else
                {
                    RowCount++;

                    if (RowCount < StoryManager.MaxRows(AppSettings.ShowUniqueID.ToString(), 1) + 1)
                    {

                        currentrow = StoryManager.ReadFromDatabase(AppSettings.ShowUniqueID.ToString(), RowCount, 1);
                        if (Image1 != "")
                        {
                            storyDictionary.Add(proto.Text, Image1);
                            proto.Text = "";

                        }
                        ShowList objlist = OnlineShow.GetShowDetail(AppSettings.ShowUniqueID);
                        if (ResourceHelper.AppName == Apps.Vedic_Library.ToString() && objlist.SubTitle == Constants.MovieCategoryTelugu)
                        {
                            DivideRowsasperViewableArea(proto.Text + Environment.NewLine + currentrow[0].ToString(), currentrow[1].ToString(), 23, dummygrid);

                        }
                        if (objlist.SubTitle == Constants.MovieCategoryEnglish)
                        {
                            if (ResourceHelper.AppName == Apps.Story_Time.ToString())
                            {
                                DivideRowsasperViewableArea(proto.Text + Environment.NewLine + "                    " + currentrow[0].ToString(), currentrow[1].ToString(), 30, dummygrid);
                            }
                            else
                            {
                                DivideRowsasperViewableArea(proto.Text + Environment.NewLine + "                    " + currentrow[0].ToString(), currentrow[1].ToString(), 23, dummygrid);
                            }

                        }
                        if (ResourceHelper.AppName == Apps.Vedic_Library.ToString() && objlist.SubTitle == Constants.MovieCategoryHindi)
                        {
                            DivideRowsasperViewableArea(proto.Text + Environment.NewLine + currentrow[0].ToString(), currentrow[1].ToString(), 23, dummygrid);
                        }
                    }
                    else
                    {
                        if (proto.RenderSize.Height > 500)
                        {
                            storyDictionary.Add(proto.Text, "");
                            storyDictionary.Add("", "End-marker.png");
                        }
                        else
                        {
                            storyDictionary.Add(proto.Text, "End-marker.png");
                        }
                    }
                }

                dummygrid.Children.Remove(proto);
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in DivideRowsasperViewableArea Method In StoryReadingExperience.cs file", ex);
            }

        }
        public static void StartRetriving(Grid dummygrid)
        {
            try
            {
                storyDictionary = new Dictionary<string, string>();
                string[] firstRecord = new string[2];

                for (RowCount = 1; RowCount < StoryManager.MaxRows(AppSettings.ShowUniqueID.ToString(), 1) + 1; RowCount++)
                {
                    firstRecord = StoryManager.ReadFromDatabase(AppSettings.ShowUniqueID.ToString(), RowCount, 1);

                    ShowList objlist = OnlineShow.GetShowDetail(AppSettings.ShowUniqueID);

                    if (ResourceHelper.AppName == Apps.Vedic_Library.ToString() && objlist.SubTitle == Constants.MovieCategoryTelugu)
                    {
                        DivideRowsasperViewableArea(firstRecord[0].ToString(), firstRecord[1].ToString(), 23, dummygrid);

                    }
                    if (objlist.SubTitle == Constants.MovieCategoryEnglish)
                    {
                        if (ResourceHelper.AppName == Apps.Story_Time.ToString())
                        {
                            DivideRowsasperViewableArea(firstRecord[0].ToString(), firstRecord[1].ToString(), 30, dummygrid);
                        }
                        else
                        {
                            DivideRowsasperViewableArea(firstRecord[0].ToString(), firstRecord[1].ToString(), 23, dummygrid);
                        }

                    }
                    if (ResourceHelper.AppName == Apps.Vedic_Library.ToString() && objlist.SubTitle == Constants.MovieCategoryHindi)
                    {
                        DivideRowsasperViewableArea(firstRecord[0].ToString(), firstRecord[1].ToString(), 23, dummygrid);
                    }


                }
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in StartRetriving Method In StoryReadingExperience.cs file", ex);
            }
        }
        //public static IsolatedStorageFileStream Trimstoryvoice(int start, int end)
        //{
        //    try
        //    {
        //        IsolatedStorageFileStream output = default(IsolatedStorageFileStream);
        //        MemoryStream sfd = new MemoryStream();
        //        var userStore = IsolatedStorageFile.GetUserStoreForApplication();

        //        if (userStore.FileExists("/StoryRecordings/" + AppSettings.ShowID + "/" + AppSettings.Title + ".wav"))
        //        {
        //            using (IsolatedStorageFileStream fs = userStore.OpenFile("/StoryRecordings/" + AppSettings.ShowID + "/" + AppSettings.Title + ".wav", FileMode.Open, FileAccess.ReadWrite))
        //            {
        //                fs.CopyTo(sfd);
        //                output = userStore.OpenFile("story.wav", FileMode.Create);
        //                byte[] readBuffer = new byte[4096];
        //                int bytesRead = -1;
        //                sfd.Position = start;
        //                sfd.SetLength(end);
        //                WriteWavHeader(output, 16000);
        //                while ((bytesRead = sfd.Read(readBuffer, 0, readBuffer.Length)) > 0)
        //                {
        //                    output.Write(readBuffer, 0, bytesRead);
        //                }
        //                fs.Dispose();
        //                fs.Close();
        //                UpdateWavHeader(output);
        //                output.Position = 0;
        //            }
        //        }

        //        return output;
        //    }
        //    catch (Exception ex)
        //    {

        //        Exceptions.SaveOrSendExceptions("Exception in Trimstoryvoice Method In StoryReadingExperience.cs file", ex);
        //         return null;
        //    }
        //}




        public async static Task<Stream> Trimstoryvoice(int start, int end)
        {
            try
            {
                Stream output = default(Stream);
                MemoryStream sfd = new MemoryStream();
                var userStore = ApplicationData.Current.LocalFolder;
                await userStore.GetFileAsync("/StoryRecordings/" + AppSettings.ShowID + "/" + AppSettings.Title + ".wav");
                var SourceFile = await ApplicationData.Current.LocalFolder.GetFileAsync("/StoryRecordings/" + AppSettings.ShowID + "/" + AppSettings.Title + ".wav");
                IRandomAccessStream stream = await SourceFile.OpenAsync(FileAccessMode.Read);
                //using (Stream fs =await fs11.OpenStreamForWriteAsync())
                //{
                //    fs.CopyTo(sfd);
                //    var fs111 = await ApplicationData.Current.LocalFolder.CreateFileAsync("Movies.xml");
                //    output =(Stream)await fs111.OpenAsync(FileAccessMode.ReadWrite);
                //    //output = userStore.OpenFile("story.wav", FileMode.Create);
                //    byte[] readBuffer = new byte[4096];
                //    int bytesRead = -1;
                //    sfd.Position = start;
                //    sfd.SetLength(end);
                //    WriteWavHeader(output, 16000);
                //    while ((bytesRead = sfd.Read(readBuffer, 0, readBuffer.Length)) > 0)
                //    {
                //        output.Write(readBuffer, 0, bytesRead);
                //    }
                //    fs.Dispose();
                //    //fs.Close();
                //    UpdateWavHeader(output);
                //    output.Position = 0;
                //}
                //}

                return output;
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in Trimstoryvoice Method In StoryReadingExperience.cs file", ex);
                return null;
            }
        }


        public static void UpdateWavHeader(Stream stream)
        {
            try
            {
                var oldPos = stream.Position;
                stream.Seek(4, SeekOrigin.Begin);
                stream.Write(BitConverter.GetBytes((int)stream.Length - 8), 0, 4);
                stream.Seek(40, SeekOrigin.Begin);
                stream.Write(BitConverter.GetBytes((int)stream.Length - 44), 0, 4);
                stream.Seek(oldPos, SeekOrigin.Begin);

            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in UpdateWavHeader Method In StoryReadingExperience.cs file", ex);
            }
        }
        public static void WriteWavHeader(Stream stream, int sampleRate)
        {
            try
            {
                const int bitsPerSample = 16;
                const int bytesPerSample = bitsPerSample / 8;
                var encoding = System.Text.Encoding.UTF8;
                stream.Write(encoding.GetBytes("RIFF"), 0, 4);
                stream.Write(BitConverter.GetBytes(0), 0, 4);
                stream.Write(encoding.GetBytes("WAVE"), 0, 4);
                stream.Write(encoding.GetBytes("fmt "), 0, 4);
                stream.Write(BitConverter.GetBytes(16), 0, 4);
                stream.Write(BitConverter.GetBytes((short)1), 0, 2);
                stream.Write(BitConverter.GetBytes((short)1), 0, 2);
                stream.Write(BitConverter.GetBytes(sampleRate), 0, 4);
                stream.Write(BitConverter.GetBytes(sampleRate * bytesPerSample), 0, 4);
                stream.Write(BitConverter.GetBytes((short)(bytesPerSample)), 0, 2);
                stream.Write(BitConverter.GetBytes((short)(bitsPerSample)), 0, 2);
                stream.Write(encoding.GetBytes("data"), 0, 4);
                stream.Write(BitConverter.GetBytes(0), 0, 4);
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in WriteWavHeader Method In StoryReadingExperience.cs file", ex);
            }
        }
        public async static void RetriveFromStorage(Image imgstory, TextBlock txtPara, TextBlock txtpageno, int pagenum, Grid dummygrid, MediaElement mymedia, bool buttontapped)
        {
            try
            {
                string text = string.Empty;
                txtpageno.Text = (pagenum).ToString() + "/" + (storyDictionary.Count).ToString();

                if (mymedia != null && Constants.mode == "Listen")
                {
                    XDocument xdoc = new XDocument();

                    StorageFolder store = ApplicationData.Current.LocalFolder;
                    var story = Task.Run(async () => await store.CreateFolderAsync("StoryRecordings", CreationCollisionOption.OpenIfExists)).Result;
                    var story1 = Task.Run(async () => await story.CreateFolderAsync(AppSettings.ShowID, CreationCollisionOption.OpenIfExists)).Result;
                    StorageFile file = Task.Run(async () => await story1.GetFileAsync("StoryRecordings.xml")).Result;
                    var f1 = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                    IInputStream inputStream = f1.GetInputStreamAt(0);
                    DataReader dataReader = new DataReader(inputStream);
                    uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f1.Size)).Result;
                    string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlData));
                    xdoc = XDocument.Load(ms);
                    dataReader.DetachStream();
                    inputStream.Dispose();
                    f1.Dispose();
                    ms.Dispose();
                    //    if (Task.Run(async () => await Storage.FileExists("/StoryRecordings/" + AppSettings.ShowID + "/StoryRecordings.xml")).Result)
                    //{
                    //XDocument xdoc =await Storage.ReadFileAsDocument("/StoryRecordings/" + AppSettings.ShowID + "/StoryRecordings.xml");

                    var data = from f in xdoc.Descendants("Stories").Elements("Story")
                               where (Convert.ToInt32(f.Attribute("Id").Value)) == pagenum
                               select f;
                    if (data.Count() > 0)
                    {
                        foreach (var d in data)
                        {
                            StorageFolder store1 = ApplicationData.Current.LocalFolder;
                            var story111 = Task.Run(async () => await store1.CreateFolderAsync("StoryRecordings", CreationCollisionOption.OpenIfExists)).Result;
                            var story11 = Task.Run(async () => await story111.CreateFolderAsync(AppSettings.ShowID, CreationCollisionOption.OpenIfExists)).Result;
                            StorageFile file1 = Task.Run(async () => await story11.GetFileAsync(AppSettings.Title + pagenum + ".wav".Trim())).Result;
                            var stream = Task.Run(async () => await file1.OpenAsync(FileAccessMode.Read)).Result;

                            mymedia.SetSource(stream, file1.ContentType);
                            if (buttontapped == false)
                            {
                                mymedia.AutoPlay = true;
                                mymedia.Play();
                            }
                            else
                                mymedia.AutoPlay = false;
                        }

                    }
                    else
                    {
                        mymedia.Source = null;
                    }
                }

                else
                {
                    try
                    {
                        if (Constants.mode == "Listenmp3")
                        {

                            XDocument xmlDoc =
                        XDocument.Load("Story Reading Xml/StoryReading.xml");
                            var findEle = from i in xmlDoc.Descendants("show") where i.Attribute("id").Value.ToString() == AppSettings.ShowID select i;
                            var data = from f in findEle.Descendants("story")
                                       where (Convert.ToInt32(f.Attribute("id").Value)) == pagenum
                                       select f;
                            if (data.Count() > 0)
                            {
                                foreach (var d in data)
                                {
                                    mymedia.Markers.Clear();
                                    Constants.StartPosition = Convert.ToInt32(d.Element("begintime").Value);
                                    StorageFolder isoStore = ApplicationData.Current.LocalFolder;
                                    string title = StoryManager.GetTitle(AppSettings.ShowUniqueID);
                                    var sf = Task.Run(async () => await ApplicationData.Current.LocalFolder.GetFolderAsync(AppSettings.ProjectName)).Result;
                                    StorageFile file = default(StorageFile);
                                    IReadOnlyList<StorageFile> file1 = Task.Run(async () => await sf.GetFilesAsync()).Result;
                                    foreach (StorageFile t in file1.ToList())
                                    {
                                        string s = t.Name;

                                        if (s == title + ".mp3")
                                        {
                                            IRandomAccessStream writeStream = await t.OpenAsync(FileAccessMode.Read);
                                            if (writeStream.Size != 0)
                                            {
                                                mymedia.SetSource(writeStream, "");
                                                Constants.EndPosition = Convert.ToInt32(d.Element("endtime").Value);
                                            }
                                            else
                                            {
                                                mymedia.Source = null;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                mymedia.Source = null;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Exceptions.SaveOrSendExceptions("Exception in RetriveFromStorage Method In StoryReadingExperience.cs file", ex);
                    }

                }
                if (pagenum == 1)
                {
                    txtPara.Text = "";
                    Run runLabel = new Run();
                    runLabel.FontSize = 30;
                    runLabel.Text = "                    " + storyDictionary.ElementAt(pagenum - 1).Key.Substring(0, 1);
                    Run runLabel1 = new Run();

                    ShowList objlist = OnlineShow.GetShowDetail(AppSettings.ShowUniqueID);
                    if (ResourceHelper.AppName == Apps.Vedic_Library.ToString() && objlist.SubTitle == Constants.MovieCategoryTelugu)
                    {
                        runLabel1.FontSize = 23;
                    }
                    if (objlist.SubTitle == Constants.MovieCategoryEnglish)
                    {
                        if (ResourceHelper.AppName == Apps.Story_Time.ToString())
                        {
                            runLabel1.FontSize = 30;
                        }
                        else
                        {
                            runLabel1.FontSize = 23;
                        }
                    }
                    if (ResourceHelper.AppName == Apps.Vedic_Library.ToString() && objlist.SubTitle == Constants.MovieCategoryHindi)
                    {
                        runLabel1.FontSize = 23;
                    }
                    runLabel1.Text = storyDictionary.ElementAt(pagenum - 1).Key.Substring(1);
                    txtPara.Inlines.Add(runLabel);
                    txtPara.Inlines.Add(runLabel1);
                }
                else
                {
                    txtPara.Text = storyDictionary.ElementAt(pagenum - 1).Key;
                }


                if (storyDictionary.ElementAt(pagenum - 1).Value != "")
                {
                    TextBlock proto = new TextBlock();
                    ShowList objlist = OnlineShow.GetShowDetail(AppSettings.ShowUniqueID);
                    if (ResourceHelper.AppName == Apps.Vedic_Library.ToString() && objlist.SubTitle == Constants.MovieCategoryTelugu)
                    {
                        proto.FontSize = 23;
                    }
                    if (objlist.SubTitle == Constants.MovieCategoryEnglish)
                    {
                        if (ResourceHelper.AppName == Apps.Story_Time.ToString())
                        {
                            proto.FontSize = 30;
                        }
                        else
                        {
                            proto.FontSize = 23;
                        }
                    }
                    if (ResourceHelper.AppName == Apps.Vedic_Library.ToString() && objlist.SubTitle == Constants.MovieCategoryHindi)
                    {
                        proto.FontSize = 23;
                    }
                    if (ResourceHelper.AppName == Apps.Story_Time.ToString())
                    {
                        proto.FontFamily = new FontFamily("/COM4NRG_.TTF#COM4t Nuvu Regular");
                    }
                    proto.Width = 350;
                    proto.Margin = new Thickness(400, 400, 0, 0);
                    proto.TextWrapping = TextWrapping.Wrap;
                    proto.Text = txtPara.Text;
                    dummygrid.Children.Add(proto);
                    dummygrid.UpdateLayout();
                    if (proto.RenderSize.Height < 60)

                        imgstory.Margin = new Thickness(0, 10, 0, 0);
                    else
                        imgstory.Margin = new Thickness(0, 18, 0, 0);

                    imgstory.Visibility = Visibility.Visible;
                    if (storyDictionary.ElementAt(pagenum - 1).Value.ToString() != "End-marker.png")
                    {
                        //Constants.UIThread = true;
                        //imgstory.Source = ResourceHelper.getStoryImageFromStorageOrInstalledFolderForWp8(storyDictionary.ElementAt(pagenum - 1).Value);
                        //Constants.UIThread = false;

                        if (Task.Run(async () => await Storage.FileExists("Images\\storyImages\\" + AppSettings.ShowID + "\\" + storyDictionary.ElementAt(pagenum - 1).Value)).Result)
                        {
                            imgstory.Source = new BitmapImage(new Uri("ms-appdata:///local" + ResourceHelper.getstoryImagePath1(storyDictionary.ElementAt(pagenum - 1).Value), UriKind.RelativeOrAbsolute));
                        }
                        else
                        {
                            imgstory.Source = new BitmapImage(new Uri("ms-appx://" + ResourceHelper.getstoryImagePath1(storyDictionary.ElementAt(pagenum - 1).Value), UriKind.RelativeOrAbsolute));
                        }
                    }
                    else
                        imgstory.Source = new BitmapImage(new Uri("ms-appx:///Images/storyImages/" + storyDictionary.ElementAt(pagenum - 1).Value, UriKind.RelativeOrAbsolute));

                }
                else
                {
                    //txtPara.Height = 590;

                    imgstory.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                Exceptions.SaveOrSendExceptions("Exception in RetriveFromStorage Method In StoryReadingExperience.cs file", ex);
            }

        }
    }
}
