using System;
using System.Net;
using System.Windows;
using System.Windows.Input;


namespace OnlineVideos.Entities
{
    
    public class DownloadTable
    {
       [SQLite.PrimaryKey,SQLite.AutoIncrement]
        public int ID
        {
            get;
            set;
        }
        
        public string Title
        {
            get;
            set;
        }
        
        public string Url
        {
            get;
            set;

        }
        
        public string Status
        {
            get;
            set;

        }
        public string ThumbNail
        {
            get;
            set;

        }
        public string DownLoadID
        {
            get;
            set;
        }
       
    }
}
