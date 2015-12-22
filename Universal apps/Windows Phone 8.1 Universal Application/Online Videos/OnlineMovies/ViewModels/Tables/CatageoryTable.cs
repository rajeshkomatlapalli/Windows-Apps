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
    public class CatageoryTable
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int ID
        {
            get;
            set;
        }

        [Column(CanBeNull = true)]
        public int CatID
        {
            get;
            set;
        }

        [Column(CanBeNull = true)]
        public string CatName
        {
            get;
            set;
        }
        //[Column(CanBeNull = true)]
        //public string SubTitle
        //{
        //    get;
        //    set;
        //}
        //[Column(CanBeNull = true)]
        //public string Rating
        //{
        //    get;
        //    set;
        //}
        //[Column(CanBeNull = true)]
        //public string Image
        //{
        //    get;
        //    set;
        //}
        //[Column(CanBeNull = true)]
        //public string RelaseDate
        //{
        //    get;
        //    set;
        //}
        //[Column(CanBeNull = true)]
        //public string RemoveMovie
        //{
        //    get;
        //    set;
        //}
    }
}
