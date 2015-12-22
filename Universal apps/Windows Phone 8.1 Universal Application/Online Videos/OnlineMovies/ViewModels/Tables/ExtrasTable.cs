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
    public class ExtrasTable
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public int EMatchId
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string TeamAExtras
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string TeamBExtras
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string TeamATotal
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string TeamBTotal
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string TeamAInn
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string TeamBInn
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string TeamResult
        {
            get;
            set;
        }
    }
}
