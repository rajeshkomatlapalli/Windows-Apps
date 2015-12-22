using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
#if WINDOWS_APP
using Windows.UI.Xaml.Media;
#endif

namespace OnlineVideos.Entities
{
    [SQLite.Table("GameWeapons"), ConditionType("GameWeapons", "DeleteCondition")]
    public class GameWeapons
    {
        [SQLite.AutoIncrement,SQLite.PrimaryKey]
        public int ID
        {
            get;
            set;
        }
        [SQLite.Column("WeaponID"), ConditionType("WeaponID", "SecondaryCondition")]
        public int WeaponID
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

        [SQLite.Column("Description")]
        public string Description
        {
            get;
            set;
        }
        [SQLite.Column("ShowID"),ConditionType("ShowID", "PrimaryCondition")]
        public int ShowID
        {
            get;
            set;
        }
       [ SQLite.Column("Image")]
        public string Image
        {
            get;
            set;
        }
          [SQLite.Ignore, XmlIgnore()]
          public string Descriptiontitle
          {
              get;
              set;
          }
    }
}
