using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideos.Entities
{
  public   class DownLoadHistory
    {
      [SQLite.PrimaryKey,SQLite.AutoIncrement]
      public int ID
      {
          get;
          set;
      }
      public int ShowID
      {
          get;
          set;
      }
      public string LinkUrl
      {
          get;
          set;
      }
      public string Title
      {
          get;
          set;
      }
      public string UrlType
      {
          get;
          set;
      }
      [SQLite.Ignore]
      public string LandingImage
      {
          get;
          set;
      }
    }
}
