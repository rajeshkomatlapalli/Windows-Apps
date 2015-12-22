using System;
using System.Net;
using System.Windows;


namespace OnlineVideos
{
    public class CheatTable
    {
        public int ID
        {
            get;
            set;
        }
        public int CheatId
        {
            get;
            set;
        }

        public string CheatName
        {
            get;
            set;
        }

        public string CheatDescription
        {
            get;
            set;
        }
        public int MovieID
        {
            get;
            set;
        }
        public byte[] CheatData
        {
            get;
            set;
        }
        public string Firstcolumn
        {
            get;
            set;
        }
        public string Secondcolumn
        {
            get;
            set;
        }
    }
}
