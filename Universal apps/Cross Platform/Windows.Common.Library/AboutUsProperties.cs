using System;
using System.Net;
using System.Windows;
using Windows.UI.Xaml;
#if WINDOWS_APP
using Windows.UI.Xaml;
#endif

namespace Common.Library
{
    public class FeedbackTopic
    {
        private string _topic;
        public string Topic
        {
            get { return _topic; }
            set { _topic = value; }
        }
    }

    public class AboutUsProperties
    {
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private int _count;
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
#if NOTANDROID
        private Visibility _countvisible;
        public Visibility CountVisible
        {
            get { return _countvisible; }
            set { _countvisible = value; }
        }
#endif
    }
}
