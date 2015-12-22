using SQLite;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;

namespace OnlineVideos.Entities
{
 [SQLite.Table("Phone_SocialFeed")]
   public class Phone_SocialFeed
    {
           [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int id
        {
            get;
            set;
        }
         [SQLite.Column("PersonId")]
        public int PersonId
        {
            get;
            set;
        }
        [SQLite.Column("FeedLink")]
        public string FeedLink
        {
            get;
            set;
        }
           [SQLite.Column("FeedType")]
        public string FeedType
        {
            get;
            set;
        }
          [SQLite.Column("LastAccessed")]
        public DateTime LastAccessed
        {
            get;
            set;
        }
        [SQLite.Column("UpdatesAvailable")]
        public Boolean UpdatesAvailable
        {
            get;
            set;
        }
    }
}