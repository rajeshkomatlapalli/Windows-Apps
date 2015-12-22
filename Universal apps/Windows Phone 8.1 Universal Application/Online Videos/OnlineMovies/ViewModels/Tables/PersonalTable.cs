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
    public class PersonalTable
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int ID
        {
            get;
            set;
        }

        [Column(CanBeNull = true)]
        public int PersonID
        {
            get;
            set;
        }

        [Column(CanBeNull = true)]
        public string Name
        {
            get;
            set;
        }

        [Column(CanBeNull = true)]
        public string Des
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string Count
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public byte[] Cheatdata
        {
            get;
            set;
        }
    }
}
