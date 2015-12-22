using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideos.Entities
{
    [SQLite.Table("GameControls"), ConditionType("GameControls", "DeleteCondition")]
    public class GameControls
    {
       [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int ID
        {
            get;
            set;
        }
        [SQLite.Column("ControlId"), ConditionType("VehicleID", "SecondaryCondition")]
        public int ControlId
        {
            get;
            set;
        }

       [SQLite.Column("ControlName")]
        public string ControlName
        {
            get;
            set;
        }

        [SQLite.Column("Controldescription")]
        public string Controldescription
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

    }
}
