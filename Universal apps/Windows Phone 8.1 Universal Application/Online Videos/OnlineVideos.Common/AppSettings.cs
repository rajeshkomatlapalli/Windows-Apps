using System;
using System.Net;
using System.Windows;
using Common.Library;

namespace OnlineVideos.Common
{
    public static class AppSettings
    {
        public static string FeedbackEmailID
        {
            get
            {
                return SettingsHelper.getStringValue("FeedBackEmailId");
            }
            set
            {
                SettingsHelper.Save("FeedBackEmailId", value);
            }
        }

        public static string AppProductID
        {
            get
            {
                return SettingsHelper.getStringValue("productid");
            }
            set
            {
                SettingsHelper.Save("productid", value);
            }
        }


        public static string PivotBackground
        {
            get
            {
                return SettingsHelper.getStringValue("PivotBackground");
            }
            set
            {
                SettingsHelper.Save("PivotBackground", value);
            }
        }

        public static string PivotTitle
        {
            get
            {
                return SettingsHelper.getStringValue("PivotTitle");
            }
            set
            {
                SettingsHelper.Save("PivotTitle", value);
            }
        }

        public static bool ShowUpgradePage
        {
            get
            {
                return SettingsHelper.getBoolValue("AboutUsUpgrade");
            }
            set
            {
                SettingsHelper.Save("AboutUsUpgrade", value);
            }
        }
        public static string ShowQuizId
        {
            get
            {
                return SettingsHelper.getStringValue("QuizId");
            }
            set
            {
                SettingsHelper.Save("QuizId", value);
            }
        }

        public static string ShowQuestionId
        {
            get
            {
                return SettingsHelper.getStringValue("QuestionId");
            }
            set
            {
                SettingsHelper.Save("QuestionId", value);
            }
        }
    }
}
