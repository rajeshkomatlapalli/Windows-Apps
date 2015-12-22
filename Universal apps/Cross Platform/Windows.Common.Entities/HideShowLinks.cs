using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideos.Entities
{
  public  class HideShowLinks
    {
      [SQLite.PrimaryKey, SQLite.Column("ID"),SQLite.AutoIncrement]
      public int ID
      {
          get;
          set;
      }
      [SQLite.Column("ParentLink"),SQLite.Unique]
      public string ParentLink
      {
          get;
          set;
      }
      [SQLite.Column("ChildLink"),SQLite.Unique]
      public string ChildLink
      {
          get;
          set;
      }
    }
}
