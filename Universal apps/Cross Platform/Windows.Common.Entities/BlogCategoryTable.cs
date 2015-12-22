using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideos.Entities
{
      [SQLite.Table("BlogCategoryTable")]
   public class BlogCategoryTable
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int ID
        {
            get;
            set;
        }
        
        [SQLite.Column("BlogName")]
        public string BlogName
        {
            get;
            set;
        }
        [SQLite.Column("BlogUserName")]
        public string BlogUserName
        {
            get;
            set;
        }
        [SQLite.Column("BlogPassword")]
        public string BlogPassword
        {
            get;
            set;
        }
        [SQLite.Column("BlogCategory")]
        public string BlogCategory
        {
            get;
            set;
        }
        [SQLite.Column("BlogUrl")]
        public string BlogUrl
        {
            get;
            set;
        }
        [SQLite.Column("BlogType")]
        public string BlogType
        {
            get;
            set;
        }
       
    }
}
