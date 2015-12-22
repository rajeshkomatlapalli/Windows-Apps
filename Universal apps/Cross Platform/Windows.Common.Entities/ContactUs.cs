using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
#if WINDOWS_APP
using Windows.UI.Xaml.Media;
#endif
namespace OnlineVideos.Entities
{
   public class ContactUs
    {
       public int ID
       {
           get;
           set;
       }
        public string Name
        {
            get;
            set;
        }
        public string Image
        {
            get;
            set;
        }
        public string Link
        {
            get;
            set;
        }
    }
}
