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
    public class BattingTable
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public int BatMatchId
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string BatsmanId
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string BatsManName
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string Out
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string Runs
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string Balls
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string BatType
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
