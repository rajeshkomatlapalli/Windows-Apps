using System;

namespace Common.Library
{
    public static class CloudService
    {
        public static string SendMailWebTile(string subject, string body, string ToAddress)
        {
            try
            {
                //ServiceManager.SendMailToAppAsync("support@lartsoft.com", ToAddress, subject, body, service_SendMailCompleted);
                return ToAddress;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SendMailWebTile Method In CloudService.cs file.", ex);
                return null;
            }
        }
        public async static void SendMail(string subject, string body, string fromAddress)
        {
            try
            {
                string Toaddress = SettingsHelper.getStringValue("ToEmailid");
                if (Toaddress == "")
                {
                    Toaddress = "support@lartsoft.com";
                }
                if (string.IsNullOrEmpty(fromAddress))
                {
                    fromAddress = SettingsHelper.getStringValue("FromEmailid");
                }
                //ServiceManager.SendMailToAppAsync(fromAddress, Toaddress, subject, body, service_SendMailCompleted);

                //EmailRecipient sendTo = new EmailRecipient()
                //{
                //    Address = Toaddress
                //};
                //EmailMessage mail = new EmailMessage();
                //mail.Subject = subject;
                //mail.Body = body;

                //mail.To.Add(sendTo);
                //await EmailManager.ShowComposeNewEmailAsync(mail);               
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SendMail Method In CloudService.cs file.", ex);                
            }
        }

        //TODO: finish this in the consumer classes
        //static void service_SendMailCompleted(object sender, SendMailCompletedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Error == null)
        //        {                  
        //            MessageBox.Show("Feedback sent successfully");
        //            ((PhoneApplicationFrame)Application.Current.RootVisual).GoBack();
        //        }
        //        else
        //        {
        //            MessageBox.Show("Failed to send feedback, try again later.");
        //            ((PhoneApplicationFrame)Application.Current.RootVisual).GoBack();
        //        }
        //    }
        //    catch (Exception ex)
        //    {                
        //      Exceptions.SaveOrSendExceptions("Exception in service_SendMailCompleted Method In CloudService.cs file.", ex);
        //    }                        
        //}
    }
}
