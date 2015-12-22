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
    public class CountryTable
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int ID
        {
            get;
            set;
        }

        [Column(CanBeNull = true)]
        public int Tid
        {
            get;
            set;
        }

        [Column(CanBeNull = true)]
        public string CountryName
        {
            get;
            set;
        }

    }
}
