using System;
using System.Net;
using System.Windows;

namespace OnlineVideos.Data
{   
    public class story 
    {
        public int Id
        {
            get;
            set;
        }      
        public int MovieId
        {
            get;
            set;
        }
        
        public string StoryDescription
        {
            get;
            set;
        }
   
        public string Image
        {
            get;
            set;
        }
       
        public int LanguageId
        {
            get;
            set;
        }
     
        public string Title
        {
            get;
            set;
        }
    }
}