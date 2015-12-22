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
    public class DownloadTable
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int id
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string Title
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string Url
        {
            get;
            set;

        }
        [Column(CanBeNull = true)]
        public string Status
        {
            get;
            set;

        }
       
       
       
    }
}
