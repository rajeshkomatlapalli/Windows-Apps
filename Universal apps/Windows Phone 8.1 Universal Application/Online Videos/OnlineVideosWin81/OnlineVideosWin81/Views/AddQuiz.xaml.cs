using AdRotator;
using Common.Library;
using OnlineVideos.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Common.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Reflection;
using System.Net;
using Windows.Storage;
using System.Threading.Tasks;
using System.Xml.Linq;
using OnlineVideos.Entities;
using Windows.Storage.Pickers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using InsertIntoDataBase;
using OnlineVideos.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideos.Views
{
    public sealed partial class AddQuiz : UserControl
    {
        AddShow addshow = new AddShow();        
        public static string imagetype = string.Empty;
        public string ImageUrl = string.Empty;
        public AutoResetEvent auto = new AutoResetEvent(false);
        public static TextBox phonetbx = default(TextBox);
        public AddQuiz()
        {
            this.InitializeComponent();
            Loaded += AddQuiz_Loaded;
        }

        private void AddQuiz_Loaded(object sender, RoutedEventArgs e)
        {
            if (Status.text1 != null && Status.text1 != string.Empty)
            {
                questiontbx.Text = Status.text1;
            }
            if (Status.text2 != null && Status.text2 != string.Empty)
            {
                option1tbx.Text = Status.text2;
            }
           if (Status.text3 != null && Status.text3 != string.Empty)
            {
                option2tbx.Text = Status.text3;
            }
           if (Status.text4 != null && Status.text4 != string.Empty)
            {
                option3tbx.Text = Status.text4;
            }
            if (Status.text5 != null && Status.text5 != string.Empty)
            {
                option4tbx.Text = Status.text5;
            }            
            //Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
            //AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
            //adcontrol.IsAdRotatorEnabled = false;
            //adcontrol.Visibility = Visibility.Collapsed;
            //adcontrol.Height = 0;
            //adcontrol.Width = 0;
            List<string> anslist = new List<string>();
            anslist.Add("--Choose Correct Answer--");
            anslist.Add("A");
            anslist.Add("B");
            anslist.Add("C");
            anslist.Add("D");
            lpickranswers.ItemsSource = anslist;
            if (Constants.SelectedTextCat != 0)
            {
                lpickranswers.SelectedIndex = Constants.SelectedTextCat;
                Constants.SelectedTextCat = 0;
            }
            else
            {
                lpickranswers.SelectedIndex = 0;
            }
            if (phonetbx != default(TextBox))
            {
                phonetbx.PlaceholderText = string.Empty;
                phonetbx.Text = Constants.Description.ToString();
                Constants.Description.Clear();
                phonetbx = default(TextBox);
            }
            if (Constants.UserImage != null)
            {
                IRandomAccessStream stream = addshow.GetImageFromStorage("QuestionsImage" + "\\" + AppSettings.ShowUniqueID, AppSettings.ImageTitle);
                BitmapImage ProductBitmap = new BitmapImage();
                ProductBitmap.CreateOptions = BitmapCreateOptions.None;
                ProductBitmap.SetSource(stream);

                stream.Dispose();
                if (imagetype == "LocalImage")
                    quizlocalimg.Source = ProductBitmap;
                if (imagetype == "OnlineImage")
                    quizonlineimg.Source = ProductBitmap;
                Constants.UserImage = null;
            }
        }

        private void close_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame f = Window.Current.Content as Frame;
            Page p = f.Content as Page;
            p.Frame.GoBack();
            //PopupManager.EnableControl();
            ////Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
            ////AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
            ////adcontrol.IsAdRotatorEnabled = true;
            ////adcontrol.Visibility = Visibility.Visible;
            ////adcontrol.Height = 90;
            ////adcontrol.Width = 728;
            //Constants.Description.Clear();
            //Constants.FinalUrl = string.Empty;
            //Constants.OnlineImageUrls.Clear();
            //Constants.OnlineImageUrls1.Clear();
            //Constants.UserImage = null;
            //Detail.current.questionicondisable();
            //((ListView)ShowQuiz.currentshowquiz.FindName("lstvwQuiz")).Visibility = Visibility.Visible;
            //((StackPanel)ShowQuiz.currentshowquiz.FindName("txtxstackpl")).Visibility = Visibility.Collapsed;
            //((TextBlock)ShowQuiz.currentshowquiz.FindName("tblkquestionsnotavailable")).Visibility = Visibility.Collapsed;
        }

        private void option1_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            Status.text1 = questiontbx.Text;
            Status.text2 = option1tbx.Text;
            Status.text3 = option2tbx.Text;
            Status.text4 = option3tbx.Text;
            Status.text5 = option4tbx.Text;
            Constants.navigation = true;
            phonetbx = (TextBox)this.FindName((sender as Image).Name + "tbx");
            if (phonetbx.PlaceholderText.Contains("Question"))
            {
                string[] array = new string[2];
                array[0] = AppSettings.Title + " Quiz Questions And Answers";
                array[1] = string.Empty;
                Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);

                frame.Navigate(typeof(UserBrowserPage), array);
                Window.Current.Content = frame;
                Window.Current.Activate();

            }
            else
            {
                string[] array = new string[2];
                array[0] = string.Empty;
                array[1] = string.Empty;
                Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);

                frame.Navigate(typeof(UserBrowserPage), array);
                Window.Current.Content = frame;
                Window.Current.Activate();
            }
        }
        public void uploadImage(string imagename)
        {
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://picasaweb.google.com/data/feed/api/user/" + CONST.USER);
            //request.Method = "POST";
            //request.ContentType = "image/png";
            //request.Headers["Authorization"] = CONST.PIC_AUTH + CONST.AUTH_Token;
            //object[] array = new object[2];
            //array[0] = imagename;
            //array[1] = request;
            //request.BeginGetRequestStream(new AsyncCallback(GetPicasaStreamCallback), array);
            //auto.WaitOne();
        }

        private void GetPicasaStreamCallback(IAsyncResult asynchronousResult)
        {
            string FolderName = "QuestionsImage";
            object[] array = new object[2];
            array = (object[])asynchronousResult.AsyncState;
            HttpWebRequest request = (HttpWebRequest)array[1];

            string ImageName = array[0].ToString();


            StorageFolder store = ApplicationData.Current.LocalFolder;
            StorageFile file = default(StorageFile);
            file = Task.Run(async () => await store.CreateFileAsync("Images\\" + FolderName + "\\" + AppSettings.ShowUniqueID + "\\" + ImageName, CreationCollisionOption.OpenIfExists)).Result;
            IRandomAccessStream stream = Task.Run(async () => await file.OpenAsync(Windows.Storage.FileAccessMode.Read)).Result;
            Stream postStream = request.EndGetRequestStream(asynchronousResult);
            byte[] buffer = new byte[stream.AsStream().Length / 4];
            int bytesRead = -1;
            stream.AsStream().Position = 0;
            while ((bytesRead = stream.AsStream().Read(buffer, 0, buffer.Length)) > 0)
            {
                postStream.Write(buffer, 0, bytesRead);
            }
            stream.AsStream().Dispose();
            postStream.Dispose();

            request.BeginGetResponse(new AsyncCallback(RequestPicasaCompleted), request);
        }

        private void RequestPicasaCompleted(IAsyncResult result)
        {
            var request = (HttpWebRequest)result.AsyncState;
            var response = (HttpWebResponse)request.EndGetResponse(result);
            StreamReader responseReader = new StreamReader(response.GetResponseStream());
            XElement MyXMLConfig = XElement.Load(responseReader);
            XNamespace atomNS = "http://www.w3.org/2005/Atom";
            ImageUrl = MyXMLConfig.Descendants(atomNS + "content").Attributes().Where(i => i.Name == "src").FirstOrDefault().Value;
            auto.Set();

        }
        private async void userlocalimg_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            Constants.SelectedTextCat = lpickranswers.SelectedIndex;
            imagetype = "LocalImage";
            int showid = AppSettings.ShowUniqueID;
            List<QuizQuestions> quizquetions = addshow.GetQuizQuestions();
            if (quizquetions.Where(i => i.ShowID == showid && (i.Image != null || i.Image != "")).FirstOrDefault() != null)
                AppSettings.ImageTitle = (!string.IsNullOrEmpty(quizquetions.Where(i => i.ShowID == showid).OrderByDescending(i => i.Image).FirstOrDefault().Image) ? Convert.ToInt32(quizquetions.Where(i => i.ShowID == showid).OrderByDescending(i => i.Image).FirstOrDefault().Image) + 1 : 1).ToString();
            //AppSettings.ImageTitle = (Convert.ToInt32(quizquetions.Where(i => i.ShowID == showid && (i.Image != null || i.Image != "")).OrderByDescending(i => i.Image).FirstOrDefault().Image) + 1).ToString();
            else
                AppSettings.ImageTitle = "1";
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".bmp");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile imgFile = await openPicker.PickSingleFileAsync();
            if (imgFile != null)
            {
                IRandomAccessStream stream = await imgFile.OpenAsync(FileAccessMode.Read);
                Constants.UserImage = stream.AsStream();

                using (IRandomAccessStream fileStream = addshow.GetImageFromStorage(string.Empty, "SelectedImage.jpg"))
                {

                    using (IOutputStream outputStream = fileStream.GetOutputStreamAt(0))
                    {
                        using (DataWriter dataWriter = new DataWriter(outputStream))
                        {
                            MemoryStream ms = new MemoryStream();
                            Constants.UserImage.Position = 0;
                            Constants.UserImage.CopyTo(ms);
                            dataWriter.WriteBytes(ms.ToArray());
                            await dataWriter.StoreAsync();
                            dataWriter.DetachStream();
                        }
                        await outputStream.FlushAsync();
                    }
                }
                string[] array = new string[2];
                array[0] = "Quiz";
                array[1] = AppSettings.ImageTitle.ToString();
                Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);

                frame.Navigate(typeof(PhotoChooser), array);
                Window.Current.Content = frame;
                Window.Current.Activate();

            }
        }

        private async void useronlineimg_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            Constants.SelectedTextCat = lpickranswers.SelectedIndex;
            imagetype = "OnlineImage";
            int showid = AppSettings.ShowUniqueID;
            List<QuizQuestions> quizquetions = addshow.GetQuizQuestions();
            if (quizquetions.Where(i => i.ShowID == showid && (i.Image != null || i.Image != "")).FirstOrDefault() != null)
                AppSettings.ImageTitle = (!string.IsNullOrEmpty(quizquetions.Where(i => i.ShowID == showid).OrderByDescending(i => i.Image).FirstOrDefault().Image) ? Convert.ToInt32(quizquetions.Where(i => i.ShowID == showid).OrderByDescending(i => i.Image).FirstOrDefault().Image) + 1 : 1).ToString();
            //AppSettings.ImageTitle = (Convert.ToInt32(quizquetions.Where(i => i.ShowID == showid && (i.Image != null || i.Image != "")).OrderByDescending(i => i.Image).FirstOrDefault().Image) + 1).ToString();
            else
                AppSettings.ImageTitle = "1";

            await addshow.GetOnlineImages("Quiz", questiontbx.Text);

            //Constants.OnlineImageUrls = new ObservableCollection<string>();
            //foreach (string ss in Constants.OnlineImageUrls1)
            //{
            //    Constants.OnlineImageUrls.Add(ss);
            //}
            //Constants.OnlineImageUrls1.Clear();
            Frame frame = InsertIntoDataBase.retrieveframe.getframe(this.GetType().GetTypeInfo().Assembly.FullName);

            frame.Navigate(typeof(OnlineImages), "Quiz");
            Window.Current.Content = frame;
            Window.Current.Activate();
        }

        private void save_Click_1(object sender, RoutedEventArgs e)
        {
            Status.text1 = null;
            Status.text2 = null;
            Status.text3 = null;
            Status.text4 = null;
            Status.text5 = null;
            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += bg_DoWork;
            bg.RunWorkerCompleted += bg_RunWorkerCompleted;
            //bg.RunWorkerAsync(tblkpersonname.Text);
            bg.RunWorkerAsync();
        }
        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //int quizno = Convert.ToInt32(AppSettings.ShowQuizId);
            int quizno = Constants.QuizId;
            int showid = AppSettings.ShowUniqueID;
            List<QuizQuestions> quizquetions = addshow.GetQuizQuestions();
            //if (!string.IsNullOrEmpty(questiontbx.Text) && !string.IsNullOrEmpty(tblkshowname.Text))
            if (!string.IsNullOrEmpty(questiontbx.Text) && lpickranswers.SelectedIndex != 0)
            {

                valid1.Visibility = Visibility.Collapsed;
                validrole.Visibility = Visibility.Collapsed;
                InsertData<QuizQuestions> insert1 = new InsertData<QuizQuestions>();
                insert1.ParameterList = new QuizQuestions();
                insert1.ParameterList.ShowID = showid;
                insert1.ParameterList.QuestionID = (quizquetions.Where(i => i.ShowID == showid && i.QuizNo == quizno).FirstOrDefault() != null) ? quizquetions.Where(i => i.ShowID == showid && i.QuizNo == quizno).OrderByDescending(i => i.QuestionID).FirstOrDefault().QuestionID + 1 : 1;
                insert1.ParameterList.QuizNo = quizno;
                insert1.ParameterList.Question = questiontbx.Text;
                insert1.ParameterList.Option1 = "A) " + option1tbx.Text;
                insert1.ParameterList.Option2 = "B) " + option2tbx.Text;
                insert1.ParameterList.Option3 = "C) " + option3tbx.Text;
                insert1.ParameterList.Option4 = "D) " + option4tbx.Text;
                insert1.ParameterList.Answer = lpickranswers.SelectedItem.ToString();

                if (!Task.Run(async () => await Storage.FileExists("Images\\QuestionsImage\\" + AppSettings.ShowUniqueID + "\\" + AppSettings.ImageTitle + ".jpg")).Result)

                    insert1.ParameterList.Image = "";
                else
                {
                    insert1.ParameterList.Image = AppSettings.ImageTitle;
                }
                insert1.ParameterList.FlickrQuizImageUrl = ImageUrl;
                insert1.Insert();
                Constants.Description.Clear();
                AppSettings.ImageTitle = string.Empty;
                Constants.FinalUrl = string.Empty;
                Constants.OnlineImageUrls.Clear();
                Constants.OnlineImageUrls1.Clear();
                Constants.UserImage = null;
                PopupManager.EnableControl();                
                //Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                //AdRotatorControl adcontrol = (AdRotatorControl)border.FindName("AdRotatorWin8");
                //adcontrol.IsAdRotatorEnabled = true;
                //adcontrol.Visibility = Visibility.Visible;
                //adcontrol.Height = 90;
                //adcontrol.Width = 728;
                //ShowQuiz.currentshowquiz.GetPageDataInBackground();
                //((TextBlock)ShowQuiz.currentshowquiz.FindName("tblkquestionsnotavailable")).Visibility = Visibility.Collapsed;
                Detail.current.questionicondisable();
                Frame f = Window.Current.Content as Frame;
                Page p = f.Content as Page;
                p.Frame.GoBack();

            }
            else
            {

                if (lpickranswers.SelectedIndex == 0 && !string.IsNullOrEmpty(questiontbx.Text))
                {
                    validrole.Visibility = Visibility.Visible;
                    valid1.Visibility = Visibility.Collapsed;
                }
                else if (string.IsNullOrEmpty(questiontbx.Text) && lpickranswers.SelectedIndex != 0)
                {
                    valid1.Visibility = Visibility.Visible;
                    validrole.Visibility = Visibility.Collapsed;
                }
                else
                {
                    validrole.Visibility = Visibility.Visible;
                    valid1.Visibility = Visibility.Visible;
                }

            }

        }

        void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument == null)
            {


                string ImageName = AppSettings.ImageTitle + ".jpg";

                if (Task.Run(async () => await Storage.FileExists("Images\\QuestionsImage\\" + AppSettings.ShowUniqueID + "\\" + ImageName)).Result)
                {
                    ImageUrl = string.Empty;
                    List<BlogCategoryTable> blogcat = addshow.BlogCategoryTable();
                    BlogCategoryTable bt = blogcat.Where(i => i.BlogType == "shows").FirstOrDefault();
                    //PicasaInterface pi = new PicasaInterface(bt.BlogUserName, bt.BlogPassword);
                    //uploadImage(ImageName);
                }
                else
                {
                    ImageUrl = string.Empty;
                }
            }
        }
    }
}
