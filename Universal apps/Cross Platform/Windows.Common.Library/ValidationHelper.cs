using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;


namespace Common.Library
{
    public static class ValidationHelper
    {
        public static bool IsEmailAdressValid(string EmailID)
        {
            return Regex.IsMatch(EmailID, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
        }

        //If the downloads completed today, no need to check again until the next day.
        public static bool IsDownloadCompletedToday()
        {
            return AppSettings.DownloadCompletedDate.Date < DateTime.Now.Date;
        }
        public static DateTime convertdatetime(string parameter)
        {
            try
            {
                DateTime dtCompletedDate;
                DateTime.TryParse(parameter, out dtCompletedDate);
                return dtCompletedDate;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in convertdatetime Method In ValidationHelper", ex);
                return default(DateTime);
            }
        }
    }
}
