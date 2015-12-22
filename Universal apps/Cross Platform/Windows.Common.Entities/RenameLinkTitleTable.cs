using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideos.Entities
{
    [SQLite.Table("RenameLinkTitleTable")]
   public  class RenameLinkTitleTable
    {
      [SQLite.Column("ID"),SQLite.PrimaryKey,SQLite.AutoIncrement]
       public int ID
       { get; set; }
        [SQLite.Column("LinkTitle")]
        public string LinkTitle
        { get; set; }
        [SQLite.Column("RenameLinkTitle")]
        public string RenameLinkTitle
        { get; set; }
        [SQLite.Column("LinkUrl")]
        public string LinkUrl
        { get; set; }


    }
}
