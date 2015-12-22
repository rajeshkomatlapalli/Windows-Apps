using System;
using System.Net;
using System.Windows;


namespace OnlineVideos.Data
{
    
    public class BowlingTable
    {
       
        public int Id
        {
            get;
            set;
        }

   
        public int BMatchId
        {
            get;
            set;
        }
       
        public string BowlerID
        {
            get;
            set;
        }
      
        public string BowlerName
        {
            get;
            set;
        }
     
        public string Overs
        {
            get;
            set;
        }
   
        public int BowlRuns
        {
            get;
            set;
        }
      
        public string Maidens
        {
            get;
            set;
        }
    
        public int Wkts
        {
            get;
            set;
        }
      
        public string BowlBalls
        {
            get;
            set;
        }
     
        public string BowlType
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
