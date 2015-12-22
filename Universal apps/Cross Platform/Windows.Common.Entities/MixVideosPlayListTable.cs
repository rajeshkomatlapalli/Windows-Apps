using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideos.Entities
{
    [SQLite.Table("MixVideosPlayListTable")]
  public   class MixVideosPlayListTable
    {
        [SQLite.Column("ID"), SQLite.PrimaryKey]
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
        [SQLite.Column("PlayListID ")]
        public int PlayListID 
        { get; set; }
    }
}
