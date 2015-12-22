using Common.Library;
using Common.Utilities;
using OnlineMovies.UserControls;
using OnlineVideos;
using OnlineVideos.Data;
using OnlineVideos.Entities;
using OnlineVideos.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OnlineMovies.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QuizResult : Page
    {
        QuizExitPopUp QuizPopUp;
        int displaypopup = 0;
        int exitpopup = 0;
        string Timeused;

        public QuizResult()
        {
            try
            {
                this.InitializeComponent();
                QuizPopUp = new QuizExitPopUp();
                LoadPivotThemes(AppSettings.ShowUniqueID);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in QuizResult Method In QuizResult.cs file.", ex);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Timeused = e.Parameter.ToString();
        }

        private void LoadPivotThemes(long ShowID)
        {

            tblkVideosTitle.Text = QuizManager.GetSubjectsTitle();
        }

        private void imgTitle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
        private void DisplayTimeUsed(string Timeused)
        {
            //OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
            try
            {
                if (Timeused != "")
                {
                    var Answerdata = Task.Run(async () => await Constants.connection.Table<QuizUserAnswers>().ToListAsync()).Result;
                    int count = Answerdata.Count();
                    if (Answerdata.Count() != Convert.ToInt32(AppSettings.ShowMaxQuestionId))
                    {
                        exitpopup = 1;
                        imgexit.Visibility = Visibility.Visible;
                        imgreview.Visibility = Visibility.Collapsed;
                    }
                    string[] displaytime = Timeused.Split(':');
                    if (displaytime[1] != "0")
                    {
                        int min = Convert.ToInt32(displaytime[1]);
                        if (min >= 1)
                            Timeused = displaytime[1] + ":" + displaytime[2];
                        else
                        {
                            if (displaytime[2] != "00")
                                Timeused = displaytime[2] + " Seconds";
                        }
                    }
                    else if (displaytime[2] != "00")
                    {
                        Timeused = displaytime[2] + "Seconds";
                    }

                    else
                        Timeused = "0";
                }
                txttime.Text = Timeused;
                if (SettingsHelper.getStringValue("QuizMarks") == "")
                    txtmarks.Text = "0";
                else
                {
                    txtmarks.Text = SettingsHelper.getStringValue("QuizMarks");
                }
                txttotalmarks.Text = AppSettings.ShowMaxQuestionId.ToString();
                QuizManager.SaveQuizResult(txtmarks.Text + " / " + txttotalmarks.Text);
                txttotaltime.Text = SettingsHelper.getStringValue("TotalTime");
                txtpshow.Text = SettingsHelper.getStringValue("QuizPercentage");
                txtgradedisplay.Text = SettingsHelper.getStringValue("QuizGrade");
                SettingsHelper.Save("QuizMarks", "");
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in DisplayTimeUsed Method In QuizResult.cs file.", ex);
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadAds();      
                PageTitle.Text = AppResources.ShowQuizResultTitle;
                //if (NavigationContext.QueryString.TryGetValue("Timeused", out Timeused))
                if(Timeused!=null)
                {
                    QuizManager.Displayingresult();
                    DisplayTimeUsed(Timeused);
                }
                PageHelper.RemoveEntryFromBackStack("QuizResult");
                while (Frame.BackStack.Count() > 1)
                {
                    if (!Frame.BackStack.FirstOrDefault().SourcePageType.Equals("QuizResult"))
                    {
                        //Frame.BackStack.RemoveAt(-1);
                        Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
                    }
                    else
                    {
                        //Frame.BackStack.RemoveAt(-1);
                        Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
                        break;
                    }
                } 
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in PhoneApplicationPage_Loaded Method In QuizResult.cs file.", ex);
            }
        }
        private void LoadAds()
        {
            try
            {
                LoadAdds.LoadAdControl_New(LayoutRoot, adstackpl, 1);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadAds Method In QuizResult file", ex);
                string excepmess = "Exception in LoadAds Method In QuizResult file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
            }
        }
        private void imgreview_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            AppSettings.ShowQuestionId = "1";
            Frame.Navigate(typeof(QuizResultReview));
        }

        private void imgexit_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                AppSettings.ShowQuestionId = "1";
                PageHelper.RemoveEntryFromBackStack("MainPage");
                //NavigationService.Navigate(NavigationHelper.getQuizList(AppSettings.ShowID, tblkVideosTitle.Text));
                string[] parameters = new string[2];
                parameters[0] = AppSettings.ShowID.ToString();
                parameters[1] = null;
                Frame.Navigate(typeof(SubjectDetail), parameters);
            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in imgexit_MouseLeftButtonDown Method In QuizResult.cs file.", ex);
            }
        }
    }
}
