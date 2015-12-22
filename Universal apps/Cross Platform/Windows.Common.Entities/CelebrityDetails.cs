using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WP8
using System.Windows.Media;
using System.Windows.Media.Imaging;
#endif
#if WINDOWS_PHONE_APP
using Windows.UI.Xaml.Media;
#endif
namespace OnlineVideos.Entities
{
    public class CelebrityDetails
    {
        private int _personId;
        public int PersonID
        {
            get { return _personId; }
            set { _personId = value; }
        }
        private string _PersonName;
        public string PersonName
        {
            get { return _PersonName; }
            set { _PersonName = value; }
        }
        private string _image;
        public string Image
        {
            get { return _image; }
            set { _image = value; }
        }
        private string _alphabet;
        public string Alphabet
        {
            get { return _alphabet; }
            set { _alphabet = value; }
        }
        private ImageSource _PersonImage;
        public ImageSource PersonImage
        {
            get
            { return _PersonImage; }
            set
            { _PersonImage = value;}
        }
      
    }
}
