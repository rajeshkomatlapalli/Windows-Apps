using System;
using System.Net;
using System.Windows;
//using Common.Services.FileStoreService;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
//using Microsoft.Phone.Net.NetworkInformation;
using System.Diagnostics;
//using Common.Library.FileStoreService;
//using System.ServiceModel;

using Common.Library;
using System.Threading.Tasks;

#if WINDOWS_PHONE_APP && NOTANDROID
//using OnlineVideos.Services.FileStoreService;
//using Microsoft.Phone.Tasks;
#endif
//using Common.Common;
//using OnlineVideos.Services;

//using OnlineVideos.Services.FileStoreService;
//using Common.Services;

namespace Common.Library
{
    public class Exceptions
    {

        //Saving Detailed Exceptions
#if WINDOWS_APP
        public static void SaveOrSendExceptions(string message, Exception ex)
        {
            try
            {
                string fromAddress = "OnlineVideos@lartsoft.com";
                string Toaddress = "win8errors@lartsoft.com";
                //Common.Services.FileStoreService.Service1Client service = new Common.Services.FileStoreService.Service1Client();
                string format = message + "\n\n" + ex.Message + "\n\n Stack Trace:- " + ex.StackTrace + "\n Extra Data \n";
                string Subject = AppSettings.ProjectName + "  Errors";
                message = message + Environment.NewLine;
                foreach (DictionaryEntry d in ex.Data)
                    message =  message + d.Key + " -> " + d.Value + "\n";
                     message = message +"Exception Name:"+" "+ ex.Message;
                //await service.SendMailAsync(AppSettings.FeedbackEmailID, "win8errors@lartsoft.com", Subject, message);
                //await service.SendMailAsync(fromAddress, Toaddress, Subject, message);
            }
            catch (Exception e)
            {
            }
           // SaveOrSendErrorMessage(message);
        }
        public async static void SaveOrSendHttpclientExceptions(string Message, string ExceptionName, string UrlName)
        {
            try
            {
                string fromAddress = "OnlineVideos@lartsoft.com";
                string Toaddress = "win8errors@lartsoft.com";
                //Common.Services.FileStoreService.Service1Client service = new Common.Services.FileStoreService.Service1Client();
                //Common.Services.FileStoreService.Service1Client service = new Common.Services.FileStoreService.Service1Client();
                string Subject = AppSettings.ProjectName + "  Errors";
                Message = string.Format("{0} in {1} ", Message, "Download Manage Errors");
                Message = string.Format("{0} {1} UrlName: {2} {3} Exception Name: {4}", Message, Environment.NewLine, UrlName, Environment.NewLine, ExceptionName);
                //await service.SendMailAsync(fromAddress, Toaddress, Subject, Message);
            }
            catch (Exception ex)
            {
                
                
            }
        }
        public static void SaveOrSendErrorMessage(string message)
        {
            try
            {
               
            }
            catch (Exception e)
            {
                string excepmess = "Exception in SaveData Method In Exceptions file.\n\n" + e.Message + "\n\n StackTrace :- \n" + e.StackTrace;
                //TODO: show this in the error log page.
               
            }
        }
#endif

        public static void UpdateAgentLog(string message)
        {
           AppSettings.BackgroundAgenError +=Environment.NewLine+message + Environment.NewLine + Environment.NewLine;
           
        }

        //Saving Detailed Exceptions

#if WINDOWS_PHONE_APP
		public static void SaveOrSendExceptions(string message, Exception ex)
		{
           // FlurryWP8SDK.Api.LogError(message, ex);
			string format = message + Environment.NewLine + Environment.NewLine + ex.Message + Environment.NewLine + Environment.NewLine + "Stack Trace:- " + ex.StackTrace + Environment.NewLine + "Extra Data" + Environment.NewLine;

			foreach (DictionaryEntry d in ex.Data)
				message = message + d.Key + " -> " + d.Value + Environment.NewLine;
			message = message + ResourceHelper.GetMailMessageInfo();
		
            string UserAgentErrorMessage= ex.Message +Environment.NewLine+ message;
            UpdateAgentLog(UserAgentErrorMessage);
			SaveOrSendErrorMessage(message);
		}
#endif

//#if WP8
//        public static void SendErrortoMail(string Error)
//        {
//            EmailComposeTask task = new EmailComposeTask();
//            task.Subject = ResourceHelper.ProjectName + " " + "Errors";
//            task.Body = Error;
//            task.To = "larttweets@gmail.com";
//            //task.Cc = "socialcelebrities@gmail.com";
//            task.Show();
//        }
//#endif

#if WINDOWS_PHONE_APP

        public static void SaveOrSendErrorMessage(string message)
        {
            try
            {
                SaveExceptionInLocalStorage(message);

                if (AppSettings.AutomaticallyEmailErrors && NetworkHelper.IsNetworkAvailable())
                {
                    string Subject = ResourceHelper.ProjectName + "  Errors";
                    // ServiceManager.SendMailToAppAsync(AppSettings.FeedbackEmailID, "wp7errors@lartsoft.com", Subject, message, service_SendMailCompleted);
                    //Service1Client service = new Service1Client();
                    //service.SendMailAsync(AppSettings.FeedbackEmailID, "wp7errors@lartsoft.com", Subject, message);
                    //service.SendMailCompleted += new EventHandler<SendMailCompletedEventArgs>(service_SendMailCompleted);
                }
            }
            catch (Exception e)
            {
                string excepmess = "Exception in SaveData Method In Exceptions file.\n\n" + e.Message + "\n\n StackTrace :- \n" + e.StackTrace;
                //TODO: show this in the error log page.
                SaveExceptionInLocalStorage(excepmess);
            }
        }
#endif

        //static void service_SendMailCompleted(object sender, SendMailCompletedEventArgs e)
        //{
        //    if (e.Error != null)
        //    {
        //        FaultException fault = e.Error as FaultException;
        //        SaveExceptionInLocalStorage(e.Error.Message);
        //    }
        //}


      public static async void SaveExceptionInLocalStorage(string Message)
        {
            try
            {
                XDocument doc;
                if (!Task.Run(async()=> await  Storage.FileExists(Constants.ExceptionHistoryXmlPath)).Result)
                {
                    doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                                new XElement("exceptions",
                                        new XElement("exceptionmessage", Message)));
                }
                else
                {
                    doc = await Storage.ReadFileAsDocument(Constants.ExceptionHistoryXmlPath);
                    doc.Root.Add(new XElement("exceptionmessage", Message));
                }
                Storage.SaveFileFromDocument(Constants.ExceptionHistoryXmlPath, doc);
            }
            catch(Exception ex)
            {
                AppSettings.BackgroundAgenError += "\n";
                AppSettings.BackgroundAgenError += ex.Message;
            }
        }



#if ANDROID
        public async static void SaveOrSendExceptions(string message, Exception ex)
        {
            try
            {
                string fromAddress = "OnlineVideos@lartsoft.com";
                string Toaddress = "win8errors@lartsoft.com";

                string format = message + "\n\n" + ex.Message + "\n\n Stack Trace:- " + ex.StackTrace + "\n Extra Data \n";
                string Subject = "  Errors";
                message = message + Environment.NewLine;
                foreach (DictionaryEntry d in ex.Data)
                    message = message + d.Key + " -> " + d.Value + "\n";
                message = message + "Exception Name:" + " " + ex.Message;
                //await service.SendMailAsync(AppSettings.FeedbackEmailID, "win8errors@lartsoft.com", Subject, message);

            }
            catch (Exception e)
            {
            }
            // SaveOrSendErrorMessage(message);
        }
        public async static void SaveOrSendHttpclientExceptions(string Message, string ExceptionName, string UrlName)
        {
            try
            {
                string fromAddress = "OnlineVideos@lartsoft.com";
                string Toaddress = "win8errors@lartsoft.com";

                string Subject = "  Errors";
                Message = string.Format("{0} in {1} ", Message, "Download Manage Errors");
                Message = string.Format("{0} {1} UrlName: {2} {3} Exception Name: {4}", Message, Environment.NewLine, UrlName, Environment.NewLine, ExceptionName);

            }
            catch (Exception ex)
            {


            }
        }
#endif
    }
}
