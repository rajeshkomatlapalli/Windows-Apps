using Common.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;

namespace Common.Library
{
    public static class Mailsending
    {
        public async static void SendingMail(string subject, string body, string fromAddress)
        {
            try
            {
                string Toaddress = SettingsHelper.getStringValue("ToEmailid");
                if (Toaddress == "")
                {
                    Toaddress = "feedback.at@lartsoft.com";
                }
                if (string.IsNullOrEmpty(fromAddress))
                {
                    fromAddress = SettingsHelper.getStringValue("FromEmailid");
                }
                //ServiceManager.SendMailToAppAsync(fromAddress, Toaddress, subject, body, service_SendMailCompleted);

                EmailRecipient sendTo = new EmailRecipient()
                {
                    Address = Toaddress
                };
                EmailMessage mail = new EmailMessage();
                mail.Subject = subject;
                mail.Body = body;

                mail.To.Add(sendTo);
                await EmailManager.ShowComposeNewEmailAsync(mail);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SendMail Method In CloudService.cs file.", ex);
            }
        }
    }
}
