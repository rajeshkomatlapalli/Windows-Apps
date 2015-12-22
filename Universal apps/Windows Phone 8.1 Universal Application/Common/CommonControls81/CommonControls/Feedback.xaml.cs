using Common.Library;
using Common.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace CommonControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Feedback : Page
    {        
        string id = string.Empty;
        string chno = string.Empty;
        string title = string.Empty;
        string LinkType = string.Empty;
        string uri = string.Empty;

        public Feedback()
        {
            this.InitializeComponent();
            LoadFeedbackTopics();
            LayoutRoot.Background = ImageHelper.LoadPivotBackground();
            Loaded += new RoutedEventHandler(Feedback_Loaded);
        }

        private void LoadFeedbackTopics()
        {
            List<FeedbackTopic> objfeedback = new List<FeedbackTopic>();

            XDocument xdoc = new XDocument();
            xdoc = XDocument.Load(Constants.FeedbackTopicsXmlPath);

            var xquery = from i in xdoc.Descendants("topic") select i;
            foreach (var item in xquery)
            {
                FeedbackTopic objmenu = new FeedbackTopic();
                objmenu.Topic = item.Value;
                objfeedback.Add(objmenu);
            }

            TopicListPicker.ItemsSource = objfeedback;
        }

        void Feedback_Loaded(object sender, RoutedEventArgs e)
        {
            //if (NavigationContext.QueryString.TryGetValue("LinkType", out LinkType))
            //    LayoutRoot.Background = ImageHelper.LoadPivotBackground();
            if (LinkType == "Songs" || LinkType == "Movies")
            {
                txtboxFeedback.Text = "Title : " + title + "\n" + chno + " \n " + "http://m.youtube.com/watch?v=" + uri + "\n";
            }
            else
            {
                txtboxFeedback.Text = "Title : " + title + "\n" + chno + " \n " + uri + "\n";
            }

            tbxMailId.Text = AppSettings.FeedbackEmailID;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string[] paramet = (string[])e.Parameter;
            if (paramet[0] != null && paramet[1] != null && paramet[2] != null && paramet[3] != null && paramet[4] != null)
            {
                id = paramet[0];
                chno = paramet[1];
                title = paramet[2];
                LinkType = paramet[3];
                uri = paramet[4];
            }           
        }        
        private void SendMail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(TopicListPicker.SelectedIndex != -1)
                {
                    if(txtboxFeedback.Text!="")
                    {
                        tblkMessageValidation.Visibility = Visibility.Collapsed;
                        string body = txtboxFeedback.Text + ResourceHelper.GetMailMessageInfo();
                        string fromAddress = tbxMailId.Text;
                        if (fromAddress != "")
                        {
                            if (ValidationHelper.IsEmailAdressValid(fromAddress))
                            {
                                tblkMailIdValidation.Text = "(optional)";
                                tblkMailIdValidation.Foreground = AppResources.WhiteBrush;
                                //CloudService.SendMail((TopicListPicker.SelectedItem as FeedbackTopic).Topic, body, fromAddress);
                                Mailsending.SendingMail((TopicListPicker.SelectedItem as FeedbackTopic).Topic, body, fromAddress);
                                Frame.GoBack();
                            }
                            else
                            {
                                tblkMailIdValidation.Text = "Please Enter Correct Mail ID";
                                tblkMailIdValidation.Foreground = AppResources.RedBrush;
                            }
                        }
                        else
                        {
                            fromAddress = "";
                            //CloudService.SendMail((TopicListPicker.SelectedItem as FeedbackTopic).Topic, body, fromAddress);
                            Mailsending.SendingMail((TopicListPicker.SelectedItem as FeedbackTopic).Topic, body, fromAddress);
                            Frame.GoBack();
                        }
                    }
                    else
                        tblkMessageValidation.Visibility = Visibility.Visible;
                }
                
            }
            catch(Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ApplicationBarIconButton_Click Method In Feedback file.", ex);
            }
        }
    }
}
