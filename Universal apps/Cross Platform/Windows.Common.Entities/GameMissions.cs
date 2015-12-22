using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.UI.Xaml.Media;

namespace OnlineVideos.Entities
{
    [SQLite.Table("GameMissions"), ConditionType("GameMissions", "DeleteCondition")]
    public class GameMissions
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int ID
        {
            get;
            set;
        }
        [SQLite.Column("MissionId"), ConditionType("MissionId", "SecondaryCondition")]
        public int MissionId
        {
            get;
            set;
        }

        [SQLite.Column("MissionName")]
        public string MissionName
        {
            get;
            set;
        }

       [ SQLite.Column("Missiondescription")]
        public string Missiondescription
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
        [SQLite.Column("Walkthrough")]
        public string Walkthrough
        {
            get;
            set;
        }
# if NOTANDROID
        [SQLite.Ignore, XmlIgnore()]
        public ImageSource Image
        {
            get;
            set;
        }
#endif
        [SQLite.Ignore,XmlIgnore()]
        public string Walkthroughtitle
        {
            get;
            set;
        }
    }
}
