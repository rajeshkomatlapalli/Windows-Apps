using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideos.Entities
{
    [SQLite.Table("ReminderTable")]
    public class ReminderTable
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

        [SQLite.Column("GreaterThan")]
        public int GreaterThan
        {
            get;
            set;
        }
        [SQLite.Column("LessThan")]
        public int LessThan
        {
            get;
            set;
        }
        [SQLite.Column("Type")]
        public string Type
        {
            get;
            set;
        }

    }
}
