using Common.Library;
using Common.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Reflection;
using System.Text.RegularExpressions;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class Feedback : UserControl
    {
        public static bool MailSent = false;
        public Feedback()
        {
            try
            {
            this.InitializeComponent();
            Loaded += Feedback_Loaded;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Feedback Method In Feedback.cs file", ex);
            }
        }

        private void LoadFeedbackTopics()
        {
            try
            {
                List<FeedbackTopic> objfeedback = new List<FeedbackTopic>();

                XDocument xdoc = new XDocument();
                xdoc = XDocument.Load(Constants.FeedbackTopicsXmlPath);

                var xquery = from i in xdoc.Descendants("feedbacktopics").Elements() select i;
                foreach (var item in xquery)
                {
                    FeedbackTopic objmenu = new FeedbackTopic();
                    objmenu.Topic = item.Value;
                    objfeedback.Add(objmenu);
                }

                lstcombobox.ItemsSource = objfeedback;
                lstcombobox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadFeedbackTopics Method In Feedback.cs file", ex);
            }

        }

        void Feedback_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadFeedbackTopics();
                if (!string.IsNullOrEmpty(Storage.getSettingsStringValue("FeedBackEmailId")))
                    tbxMailId.Text = Storage.getSettingsStringValue("FeedBackEmailId");
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Feedback_Loaded Method In Feedback.cs file", ex);
            }
        }

        private async void imgsend_PointerPressed(object sender, RoutedEventArgs e)
        {
            try
            {

                if (lstcombobox.SelectedIndex != -1)
                {
                    string Toaddress = "feedback.win8@lartsoft.com";
                    if (txtboxFeedback.Text != "")
                    {
                        tblkMessageValidation.Visibility = Visibility.Collapsed;
                        string body = txtboxFeedback.Text;
                        OnlineVideosWin81.Services.OnlineVideoService.OnlineVideoServiceClient service = new OnlineVideosWin81.Services.OnlineVideoService.OnlineVideoServiceClient();

                        string fromAddress = tbxMailId.Text;

                        if (fromAddress != "")
                        {
                            Storage.saveSettings("FeedBackEmailId", fromAddress);
                            if (Regex.IsMatch(fromAddress, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
                            {
                                tblkMailIdValidation.Text = "(optional)";
                                tblkMailIdValidation.Foreground = new SolidColorBrush(Colors.Red);
                                MailSent = ServiceManager.SendMailToAppAsync(fromAddress, Toaddress, (lstcombobox.SelectedItem as FeedbackTopic).Topic, body);
                                if (MailSent == true)
                                {
                                    SentMessage.Visibility = Visibility.Visible;
                                    SentMessage.Text = "Message Sent Successfully";
                                    txtboxFeedback.Text = "";
                                }
                                else
                                {
                                    SentMessage.Visibility = Visibility.Visible;
                                    SentMessage.Text = "Failed to send feedback, try again later";

                                }

                            }
                            else
                            {
                                tblkMailIdValidation.Text = "Please Enter Correct Mail Id";
                                tblkMailIdValidation.Foreground = new SolidColorBrush(Colors.Red);
                                tbxMailId.Text = "";
                            }
                        }
                        else
                        {
                            fromAddress = "OnlineVideos@lartsoft.com";
                            MailSent = await service.SendMailAsync(fromAddress, Toaddress, (lstcombobox.SelectedItem as FeedbackTopic).Topic, body);
                            if (MailSent == true)
                            {
                                SentMessage.Visibility = Visibility.Visible;
                                SentMessage.Text = "Message Sent Successfully";

                            }
                            else
                            {
                                SentMessage.Visibility = Visibility.Visible;
                                SentMessage.Text = "Failed to send feedback, try again later";
                            }
                        }
                    }
                    else
                        tblkMessageValidation.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                string excepmess = "Exception in imgsend_PointerPressed Method In Feedback file.\n\n" + ex.Message + "\n\n StackTrace :- \n" + ex.StackTrace;
                //Exception.SaveExceptionInLocalStorage(excepmess);
            }
        }

        private void TopicListPicker_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                LoadFeedbackTopics();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in TopicListPicker_SelectionChanged_1 Method In Feedback.cs file", ex);
            }
        }

    }
}
