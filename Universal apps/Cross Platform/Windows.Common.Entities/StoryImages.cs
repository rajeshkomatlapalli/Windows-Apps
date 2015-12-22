using OnlineVideos.Entities;
using System;
using System.Net;
using System.Windows;

using System.Windows.Input;


namespace OnlineVideos.Entities
{
    [ConditionType("StoryImages", "DownloadCondition")]
    public class StoryImages
    {
        [ConditionType("ImageNo", "SecondaryCondition")]
        public string ImageNo
        {
            get;
            set;
        }
        [ConditionType("ShowID", "PrimaryCondition")]
        public string ShowID
        {
            get;
            set;
        }
        public string FlickrStoryImageUrl
        {
            get;
            set;
        }
    }
}
