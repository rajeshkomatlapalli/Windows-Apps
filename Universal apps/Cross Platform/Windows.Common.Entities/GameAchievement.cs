using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.UI.Xaml.Media;

namespace OnlineVideos.Entities
{
    [SQLite.Table("GameAchievement"), ConditionType("GameAchievement", "DeleteCondition")]
    public class GameAchievement
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int ID
        {
            get;
            set;
        }
        [SQLite.Column("AchievementId"), ConditionType("AchievementId", "SecondaryCondition")]
        public int AchievementId
        {
            get;
            set;
        }

        [SQLite.Column("AchievementName")]
        public string AchievementName
        {
            get;
            set;
        }

        [SQLite.Column("AchievementDescription")]
        public string AchievementDescription
        {
            get;
            set;
        }
        [SQLite.Column("ShowID"), ConditionType("ShowID", "PrimaryCondition")]
        public int ShowID
        {
            get;
            set;
        }
         [SQLite.Column("Points")]
        public string Points
        {
            get;
            set;
        }
# if NOTANDROID
        [XmlIgnore(),SQLite.Ignore]
        public ImageSource Image
        {
            get;
            set;
        }
#endif
        [XmlIgnore(),SQLite.Ignore]
        public string Pointstitle
        {
            get;
            set;
        }
    }
}
