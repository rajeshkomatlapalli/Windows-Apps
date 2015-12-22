
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideos.Entities
{
    [SQLite.Table("ShareTable"), ConditionType("ShareTable", "PrimaryCondition")]
    public class ShareTable
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int ID
        {
            get;
            set;
        }
     [SQLite.Column("ShowID")]
        public int ShowID
        {
            get;
            set;
        }
        [SQLite.Column("Description")]
        public string Description
        {
            get;
            set;
        }
        [SQLite.Column("ImageUrl")]
        public string ImageUrl
        {
            get;
            set;
        }
        [SQLite.Column("NextPostTime")]
        public DateTime NextPostTime
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

    }
}
