using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideos.Entities
{
    [SQLite.Table("PersonGallery"),  ConditionType("PersonGallery", "DeleteCondition")]
 public  class PersonGallery
    {
 [SQLite.PrimaryKey, ConditionType("PersonID", "PrimaryCondition")]
     public int PersonID
     {
         get;
         set;
     }
        [SQLite.Column("ImageNo"), ConditionType("ImageNo", "SecondaryCondition")]
     public int ImageNo
     {
         get;
         set;
     }
 [SQLite.Column("FlickrThumbNailImage")]
     public string FlickrThumbNailImage
     {
         get;
         set;
     }
        [SQLite.Column("FlickrGalleryImage")]
     public string FlickrGalleryImage
     {
         get;
         set;
     }
    }
}
