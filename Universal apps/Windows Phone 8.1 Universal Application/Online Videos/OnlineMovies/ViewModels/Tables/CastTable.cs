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
{[Table]
    public class CastTable
    {
         [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int ID
        {
            get;
            set;
        }

        [Column(CanBeNull = true)]
        public int MovieID
        {
            get;
            set;
        }

        [Column(CanBeNull = true)]
        public string Pid
        {
            get;
            set;
        }

        [Column(CanBeNull = true)]
        public string Rid
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string TeamId
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string TeamA
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string TeamB
        {
            get;
            set;
        }
    
    }
}
