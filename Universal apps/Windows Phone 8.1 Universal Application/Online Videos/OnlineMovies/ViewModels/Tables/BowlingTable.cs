using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq.Mapping;

namespace OnlineMovies
{
    [Table]
    public class BowlingTable
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get;
            set;
        }

        [Column(CanBeNull = true)]
        public int BMatchId
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string BowlerID
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string BowlerName
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string Overs
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string BowlRuns
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string Maidens
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string Wkts
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string BowlBalls
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string BowlType
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string TeamType
        {
            get;
            set;
        }
    }
}
