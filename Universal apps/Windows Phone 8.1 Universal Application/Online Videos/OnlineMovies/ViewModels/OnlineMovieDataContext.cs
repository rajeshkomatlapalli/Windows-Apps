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
using System.Data.Linq;
using OnlineMovies.Library;


namespace OnlineMovies
{
    public class OnlineMovieDataContext:DataContext
    {
        public OnlineMovieDataContext(string connectionString)
            : base(connectionString)
        {
        }

        public Table<VideosTable> VidList
        {
            get
            {
                return this.GetTable<VideosTable>();
            }
        }
        public Table<MovieLinks> MovLinks
        {
            get
            {
                return this.GetTable<MovieLinks>();
            }
        }
        public Table<TopSongs> TpSongs
        {
            get
            {
                return this.GetTable<TopSongs>();
            }
        }
        public Table<PersonalTable> personalProf
        {
            get
            {
                return this.GetTable<PersonalTable>();
            }
        }
        public Table<CastTable> CastInfo
        {
            get
            {
                return this.GetTable<CastTable>();
            }
        }
        public Table<RolesTable> RolesInfo
        {
            get
            {
                return this.GetTable<RolesTable>();
            }
        }
        public Table<CountryTable> CountryInfo
        {
            get
            {
                return this.GetTable<CountryTable>();
            }
        }
        public Table<BattingTable> BatTble
        {
            get
            {
                return this.GetTable<BattingTable>();
            }
        }
        public Table<BowlingTable> BowlTble
        {
            get
            {
                return this.GetTable<BowlingTable>();
            }
        }
        public Table<ExtrasTable> ExtraTble
        {
            get
            {
                return this.GetTable<ExtrasTable>();
            }
        }
        public Table<CatageoryTable > CatageoryInfo
        {
            get
            {
                return this.GetTable<CatageoryTable>();
            }
        }
        public Table<DownloadTable> DownloadInfo
        {
            get
            {
                return this.GetTable<DownloadTable>();
            }
        }
        public Table<BrowserHistory> history
        {
            get
            {
                return this.GetTable<BrowserHistory>();
            }
        }
        public Table<CatageorybyMovieID> CatbymID
        {
            get
            {
                return this.GetTable<CatageorybyMovieID>();
            }
        }
        public Table<SearchHistory> SaveSearch
        {
            get
            {
                return this.GetTable<SearchHistory>();
            }
        }
    }
}
