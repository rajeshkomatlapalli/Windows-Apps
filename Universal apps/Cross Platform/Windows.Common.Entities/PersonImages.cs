using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideos.Entities
{
    [ConditionType("PersonImages", "DownloadCondition")]
    public class PersonImages
    {

        [ConditionType("PersonID", "PrimaryCondition")]
        public int PersonID
        {
            get;
            set;
        }

        public string FlickrPersonImageUrl
        {
            get;
            set;
        }
         
    }
}
