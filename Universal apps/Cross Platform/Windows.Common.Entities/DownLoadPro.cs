using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideos.Entities
{
   public class DownLoadPro : INotifyPropertyChanged
    {

       private string _Status;
       public string Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
                OnPropertyChanged("Status");
            }
        }
       public event PropertyChangedEventHandler PropertyChanged;
       private void OnPropertyChanged(String name)
       {
           if (PropertyChanged != null)
           {
               PropertyChanged(this, new PropertyChangedEventArgs(name));
           }
       }
    }
}
