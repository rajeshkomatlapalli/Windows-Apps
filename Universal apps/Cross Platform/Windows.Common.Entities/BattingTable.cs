using System;
using System.Net;
using System.Windows;

namespace OnlineVideos.Data
{
   
    public class BattingTable
    {
     
        public int Id
        {
            get;
            set;
        }
       
        public int BatMatchId
        {
            get;
            set;
        }
        
        public string BatsmanId
        {
            get;
            set;
        }
      
        public string BatsManName
        {
            get;
            set;
        }
       
        public string Out
        {
            get;
            set;
        }
       
        public int Runs
        {
            get;
            set;
        }
      
        public string Balls
        {
            get;
            set;
        }
      
        public string BatType
        {
            get;
            set;
        }
     
        public string TeamType
        {
            get;
            set;
        }
    }
}
