using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;


namespace OnlineVideos.Entities
{
    [SQLite.Table("GameVehicles"), ConditionType("GameVehicles", "DeleteCondition")]
    public class GameVehicles
    {

        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int ID
        {
            get;
            set;
        }
        [SQLite.Column("VehicleID"), ConditionType("VehicleID", "SecondaryCondition")]
        public int VehicleID
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
       [SQLite.Column("Image")]
        public string Image
        {
            get;
            set;
        }
        //[XmlIgnore()]
        //public ImageSource Image
        //{
        //    get;
        //    set;
        //}

        [SQLite.Ignore, XmlIgnore()]
        public string Descriptiontitle
        {
            get;
            set;
        }
       
    }
}
