using System;
using System.Net;
using System.Windows;


namespace OnlineVideos.Data
{
    [SQLite.Table("CastRoles")]
    public class CastRoles
    {
        [SQLite.Column("RoleID")]
            public int RoleID
            {
                get;
                set;
            }
        [SQLite.Column("Name")]
            public string Name
            {
                get;
                set;
            }
        }
}
