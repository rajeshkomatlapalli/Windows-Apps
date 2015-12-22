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
    public class VideosTable
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
        public string Image
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
        public string RelaseDate
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string Gener
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public DateTime AddDate
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
        public string SubTitle
        {
            get;
            set;
        }
        [Column(CanBeNull = true)]
        public string RemoveMovie
        {
            get;
            set;

        }
        [Column(CanBeNull = true)]
        public string PviotImage
        {
            get;
            set;

        }
        [Column(CanBeNull = true)]
        public string Description
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
        [Column(CanBeNull = true)]
        public DateTime ClientUpdatedDate
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
