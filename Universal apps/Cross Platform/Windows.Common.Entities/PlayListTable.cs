using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideos.Entities
{
    [SQLite.Table("PlayListTable"), ConditionType("PlayListTable", "PrimaryCondition")]
    public class PlayListTable
    {
        [SQLite.Column("ID"), SQLite.PrimaryKey,SQLite.AutoIncrement]
        public int ID
        { get; set; }
        [SQLite.Column("StartTime")]
        public string StartTime
        { get; set; }
        [SQLite.Column("EndTime")]
        public string EndTime
        { get; set; }
        [SQLite.Column("LinkUrl")]
        public string LinkUrl
        { get; set; }
      [ConditionType("ShowID", "PrimaryCondition")]
        public int ShowID
        { get; set; }
        [SQLite.Ignore()]
        public string BookMarkNo
        { get; set; }
        
    }
}
