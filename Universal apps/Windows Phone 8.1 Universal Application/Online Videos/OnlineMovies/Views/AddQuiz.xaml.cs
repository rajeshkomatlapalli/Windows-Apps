using Common.Library;
using Common.Utilities;
using InsertIntoDataBase;
using OnlineVideos;
using OnlineVideos.Entities;
using OnlineVideos.UserControls;
using OnlineVideos.Views;
using PicasaMobileInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Common.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OnlineMovies.Views
{
    public class values1
    {
        public int QuizID
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }

    } 
    public sealed partial class AddQuiz : Page
    {
        AddShow addshow = new AddShow();
        public string ImageUrl = string.Empty;
        public static bool navigated = false;
        public static string imagetype = string.Empty;
        public bool TaskNavigation = false;
        public AutoResetEvent auto = new AutoResetEvent(false);
        //public static PhoneTextBox phonetbx = default(PhoneTextBox);
        public static TextBox phonetbx = default(TextBox);
        public List<values1> QuizList = new List<values1>();

        public AddQuiz()
        {
            this.InitializeComponent();
            Loaded += AddQuiz_Loaded;
        }

        void AddQuiz_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> anslist = new List<string>();
            anslist.Add("A");
            anslist.Add("B");
            anslist.Add("C");
            anslist.Add("D");
            lpickranswers.ItemsSource = anslist;
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back)
            {
                int showid = AppSettings.ShowUniqueID;
                //OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
                //QuizList = context.ShowSubjects.Where(i=>i.ShowID==showid).Select(i => new values1 { QuizID = i.QuizID, Name = i.Name }).ToList();
                //tblkshowname.ItemsSource = QuizList;
            }
            //if (phonetbx != default(PhoneTextBox))
            //{
            //    phonetbx.Hint = string.Empty;
            //    phonetbx.Text = Constants.Description.ToString();
            //    Constants.Description.Clear();
            //    phonetbx = default(PhoneTextBox);
            //}
            if (TaskNavigation == false)
            {

                if (Constants.UserImage1 != null)
                {
                    if (navigated == false && Constants.navigation == false)
                    {
                        navigated = true;
                        //NavigationService.Navigate(new Uri("/Views/PhotoChooser.Xaml?ImageName=" + AppSettings.ImageTitle + "&type=Quiz", UriKind.Relative));
                        string[] parame = new string[2];
                        parame[0] = AppSettings.ImageTitle;
                        parame[1] = "Quize";
                        Frame.Navigate(typeof(PhotoChooser_New), parame);

                    }
                    else
                    {

                        navigated = false;
                        Constants.UIThread = true;

                        IRandomAccessStream stream = addshow.GetImageFromStorage("QuestionsImage\\" + AppSettings.ShowUniqueID, AppSettings.ImageTitle);
                        BitmapImage ProductBitmap = new BitmapImage();
                        ProductBitmap.CreateOptions = BitmapCreateOptions.None;
                        //ProductBitmap.SetSource(stream.AsStream());

                        stream.Dispose();
                        if (imagetype == "LocalImage")
                        {
                            userlocalimg.Source = ProductBitmap;
                            useronlineimg.Source = new BitmapImage(new Uri("/Images/fromonline.png", UriKind.RelativeOrAbsolute));
                        }
                        if (imagetype == "OnlineImage")
                        {
                            useronlineimg.Source = ProductBitmap;
                            userlocalimg.Source = new BitmapImage(new Uri("/Images/fromlocal.png", UriKind.RelativeOrAbsolute));
                        }

                        Constants.UIThread = false;


                    }
                }
                else
                    Constants.navigation = false;
            }
            else
            {
                TaskNavigation = false;
            }
        }

        private void imgTitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void question_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Constants.navigation = true;
            
            phonetbx = (TextBox)this.FindName((sender as Image).Name + "tbx");
            string[] parameters=new string[3];
            
            if(phonetbx.PlaceholderText.Contains("Question"))
            {
                parameters[0] = AppSettings.Title.ToString() + "question and answers";//querytext or Description
                parameters[1] = string.Empty;//searchquery
                parameters[2] = string.Empty;//pagename
                Frame.Navigate(typeof(UserBrowserPage), parameters);
            }
            else
            {
                parameters[0] = questiontbx.Text.ToString();
                parameters[1] = string.Empty;
                parameters[2] = string.Empty;
                Frame.Navigate(typeof(UserBrowserPage), parameters);
            }
            //phonetbx = (PhoneTextBox)this.FindName((sender as Image).Name + "tbx");

            //if (phonetbx.Hint.Contains("Question"))
               //NavigationService.Navigate(new Uri("/Views/UserBrowserPage.xaml?querytext=" + AppSettings.Title + " Quiz Questions And Answers" + "&searchquery=" + string.Empty, UriKind.RelativeOrAbsolute));
            //else
            //    NavigationService.Navigate(new Uri("/Views/UserBrowserPage.xaml?querytext=" + questiontbx.Text, UriKind.RelativeOrAbsolute));
        }

        private void userlocalimg_PointerPressed(object sender, PointerRoutedEventArgs e)
        {          
            CoreApplicationView view = CoreApplication.GetCurrentView();
            try
            {
                AppSettings.ImageTitle = tblkVideosTitle.Text + 1;
                imagetype = "LocalImage";
                int showid = AppSettings.ShowUniqueID;
                List<QuizQuestions> quizquetions = addshow.GetQuizQuestions();
                //if (quizquetions.Where(i => i.ShowID == showid).OrderByDescending(i => i.Image).FirstOrDefault() != null)
                //    AppSettings.ImageTitle = (!string.IsNullOrEmpty(quizquetions.Where(i => i.ShowID == showid).OrderByDescending(i => i.Image).FirstOrDefault().Image) ? Convert.ToInt32(quizquetions.Where(i => i.ShowID == showid).OrderByDescending(i => i.Image).FirstOrDefault().Image) + 1 : 1).ToString();
                //else
                //    AppSettings.ImageTitle = "1";
                Constants.navigation = true;
                imagetype = "LocalImage";
                try
                {
                    FileOpenPicker openPicker = new FileOpenPicker();
                    openPicker.ViewMode = PickerViewMode.Thumbnail;
                    openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                    openPicker.FileTypeFilter.Add(".jpg");
                    openPicker.FileTypeFilter.Add(".jpeg");
                    openPicker.FileTypeFilter.Add(".bmp");
                    openPicker.FileTypeFilter.Add(".png");

                    openPicker.PickSingleFileAndContinue();
                    view.Activated += View_Activated;
                }
                catch (Exception ex)
                {

                }
                TaskNavigation = true;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in userlocalimg_MouseLeftButtonDown_1 event In UserUpload.cs file.", ex);
            }

        }

        private async void View_Activated(CoreApplicationView sender, Windows.ApplicationModel.Activation.IActivatedEventArgs args1)
        {
            FileOpenPickerContinuationEventArgs args = args1 as FileOpenPickerContinuationEventArgs;
            try
            {
                if (args.Files.Count > 0)
                {
                    var stream = await args.Files[0].OpenAsync(Windows.Storage.FileAccessMode.Read);
                    if (stream != null)
                    {
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
                        Constants.UserImage1 = new BitmapImage();
                        Constants.UserImage1.SetSource(stream);
                        string[] array = new string[2];
                        array[0] = "Tile";
                        array[1] = AppSettings.ImageTitle.ToString();
                        Frame.Navigate(typeof(PhotoChooser_New), array);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        private void useronlineimg_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Constants.navigation = false;
            imagetype = "OnlineImage";
            int showid = AppSettings.ShowUniqueID;
            List<QuizQuestions> quizquetions = addshow.GetQuizQuestions();
            if (quizquetions.Where(i => i.ShowID == showid).OrderByDescending(i => i.Image).FirstOrDefault() != null)
                AppSettings.ImageTitle = (!string.IsNullOrEmpty(quizquetions.Where(i => i.ShowID == showid).OrderByDescending(i => i.Image).FirstOrDefault().Image) ? Convert.ToInt32(quizquetions.Where(i => i.ShowID == showid).OrderByDescending(i => i.Image).FirstOrDefault().Image) + 1 : 1).ToString();
            else
                AppSettings.ImageTitle = "1";

            Constants.OnlineImageUrls = new ObservableCollection<string>();
            foreach (string ss in Constants.OnlineImageUrls1)
            {
                Constants.OnlineImageUrls.Add(ss);
            }
            //Constants.OnlineImageUrls1.Clear();

            Frame.Navigate(typeof(OnlineImages_New), "Tile");
        }

        //void task_Completed(object sender, PhotoResult e)
        //{
        //    if (e.TaskResult == TaskResult.OK)
        //    {
        //        Constants.UserImage1 = new BitmapImage();
        //        Constants.UserImage1.SetSource(e.ChosenPhoto);
        //        Deployment.Current.Dispatcher.BeginInvoke(() =>
        //        {
        //            NavigationService.Navigate(new Uri("/Views/PhotoChooser.Xaml?ImageName=" + AppSettings.ImageTitle + "&type=Quiz", UriKind.Relative));

        //        });
        //        // userimg.Source = bitmapimage;
        //    }
        //}

       async void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int quizno = Convert.ToInt32(AppSettings.ShowQuizId);
            int showid = AppSettings.ShowUniqueID;

            if (!string.IsNullOrEmpty(questiontbx.Text) && lpickranswers.SelectedIndex != 0)
            {
                valid1.Visibility = Visibility.Collapsed;
                List<QuizQuestions> quizquetions = addshow.GetQuizQuestions();
                InsertData<QuizQuestions> insert1 = new InsertData<QuizQuestions>();
                insert1.ParameterList = new QuizQuestions();
                insert1.ParameterList.ShowID = showid;
                insert1.ParameterList.QuestionID = (quizquetions.Where(i => i.ShowID == showid && i.QuizNo == quizno).FirstOrDefault() != null) ? quizquetions.Where(i => i.ShowID == showid && i.QuizNo == quizno).Max(i => i.QuestionID) + 1 : 1;
                insert1.ParameterList.QuizNo = quizno;
                insert1.ParameterList.Question = questiontbx.Text;
                insert1.ParameterList.Option1 = "A) " + option1tbx.Text;
                insert1.ParameterList.Option2 = "B) " + option2tbx.Text;
                insert1.ParameterList.Option3 = "C) " + option3tbx.Text;
                insert1.ParameterList.Option4 = "D) " + option4tbx.Text;
                insert1.ParameterList.Answer = lpickranswers.SelectedItem.ToString();

                var isoStore = ApplicationData.Current.LocalFolder;

                if(!Task.Run(async () => await Storage.FileExists(Constants.QuizImagePathForDownloads + AppSettings.ShowUniqueID + "/" + AppSettings.ImageTitle + ".jpg")).Result)
                {
                    insert1.ParameterList.Image = "";
                }
                else
                {
                    insert1.ParameterList.Image = AppSettings.ImageTitle;
                }
                //IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                //if (!isoStore.FileExists(Constants.QuizImagePathForDownloads + AppSettings.ShowUniqueID + "/" + AppSettings.ImageTitle + ".jpg"))

                //    insert1.ParameterList.Image = "";
                //else
                //{
                //    insert1.ParameterList.Image = AppSettings.ImageTitle;
                //}
                if (!string.IsNullOrEmpty(ImageUrl))
                    insert1.ParameterList.FlickrQuizImageUrl = ImageUrl;
                else
                    insert1.ParameterList.FlickrQuizImageUrl = string.Empty;
                insert1.Insert();
                Constants.Description.Clear();
                AppSettings.ImageTitle = string.Empty;
                Constants.FinalUrl = string.Empty;
                Constants.OnlineImageUrls.Clear();
                Constants.OnlineImageUrls1.Clear();
                Constants.UserImage = null;
                Constants.UserImage1 = null;
                Constants.DownloadTimer.Start();
                Frame.GoBack();
                ShowQuiz.currentshowquiz.GetPageDataInBackground();
                ShowQuiz.currentshowquiz.tblkquestionsnotavailable.Visibility = Visibility.Collapsed;
            }
            else
            {
                valid1.Visibility = Visibility.Visible;

            }
        }

        void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument == null)
            {

                if (!string.IsNullOrEmpty(AppSettings.ImageTitle))
                {
                    string ImageName = AppSettings.ImageTitle + ".jpg";
                    //IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                    if (Task.Run(async () => await Storage.FileExists("Images\\QuestionsImage\\" + AppSettings.ShowUniqueID + "\\" + ImageName)).Result)
                    {
                        ImageUrl = string.Empty;
                        List<BlogCategoryTable> blogcat = addshow.BlogCategoryTable();
                        BlogCategoryTable bt = blogcat.Where(i => i.BlogType == "shows").FirstOrDefault();
                        if (NetworkHelper.IsNetworkAvailableForDownloads())
                        {
                            PicasaInterface pi = new PicasaInterface(bt.BlogUserName, bt.BlogPassword);
                            uploadImage();
                        }
                    }
                    else
                    {
                        ImageUrl = string.Empty;
                    }
                }
            }
        }

        public void uploadImage()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://picasaweb.google.com/data/feed/api/user/" + CONST.USER);
            request.Method = "POST";
            request.ContentType = "image/png";
            request.Headers["Authorization"] = CONST.PIC_AUTH + CONST.AUTH_Token;
            request.BeginGetRequestStream(new AsyncCallback(GetPicasaStreamCallback), request);
            auto.WaitOne();
        }

        private void GetPicasaStreamCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;


                string ImageName = AppSettings.ImageTitle + ".jpg";

                //IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                //IsolatedStorageFileStream fileStream = isoStore.OpenFile("Images" + "/" + "QuestionsImage" + "/" + AppSettings.ShowUniqueID + "/" + ImageName, System.IO.FileMode.Open, FileAccess.Read, FileShare.Read);
                //Stream postStream = request.EndGetRequestStream(asynchronousResult);
                //byte[] buffer = new byte[fileStream.Length / 4];
                //int bytesRead = -1;
                //fileStream.Position = 0;
                //while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                //{
                //    postStream.Write(buffer, 0, bytesRead);
                //}
                //fileStream.Close();
                //postStream.Close();
                //isoStore.Dispose();
                //request.BeginGetResponse(new AsyncCallback(RequestPicasaCompleted), request);
            }
            catch (Exception ex)
            {
                auto.Set();
            }
        }

        private void RequestPicasaCompleted(IAsyncResult result)
        {
            try
            {
                var request = (HttpWebRequest)result.AsyncState;
                var response = (HttpWebResponse)request.EndGetResponse(result);
                StreamReader responseReader = new StreamReader(response.GetResponseStream());
                XElement MyXMLConfig = XElement.Load(responseReader);
                XNamespace atomNS = "http://www.w3.org/2005/Atom";
                ImageUrl = MyXMLConfig.Descendants(atomNS + "content").Attributes().Where(i => i.Name == "src").FirstOrDefault().Value;
                auto.Set();
            }
            catch (Exception ex)
            {
                auto.Set();
            }

        }
        private void btnsave_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += bg_DoWork;
            bg.RunWorkerCompleted += bg_RunWorkerCompleted;
            //bg.RunWorkerAsync(tblkpersonname.Text);
            bg.RunWorkerAsync();
        }
    }
}
