using System;
using System.Net;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;
//using System.Data.Linq.Mapping;
using Windows.UI.Xaml.Controls;

namespace OnlineMovies
{
    [Table]
    public class MovieLinks
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
        public string Title
        {
            get;
            set;
        }

        [Column(CanBeNull = true)]
        public string Link
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string Rating
        {
            get;
            set;
        }

        [Column(CanBeNull = true)]
        public string Favorite
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string Cno
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string LinkType
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string UrlType
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public DateTime ClientUpdatedDate
        {
            get;
            set;

        }
        [Column(CanBeNull = true)]
        public string RemoveShow
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public DateTime ClientUpdatedDateForRemove
        {
            get;
            set;

        }
        [Column(CanBeNull = true)]
        public string ratingflag
        {
            get;
            set;

        }
       
    }
}
