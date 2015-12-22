using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideos.Entities
{
  [SQLite.Table("FavoriteLinksTable")]
  public   class FavoriteLinksTable
    {
      [SQLite.Column("ID"),SQLite.PrimaryKey,SQLite.AutoIncrement]
      public int ID
      { get; set; }
      [SQLite.Column("Title")]
      public string Title
      { get; set; }
      [SQLite.Column("LinkUrl")]
      public string LinkUrl
      { get; set; }
      [SQLite.Column("ChildLinksCount")]
      public int ChildLinksCount
      { get; set; }

    }
}
