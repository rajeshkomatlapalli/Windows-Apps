using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideosCardGame
{
    public class Constants
    {
        public const string AppLoadedMessage = "AppLoaded";
        public const string AppResumedMessage = "AppResumed";
        public const string StartMessage = "Start";
        public const string EndGameMessage = "EndGame";
        public const string PauseMessage = "PauseGame";
        public const int FlipTimeMs = 600;
        public const string DatabaseConnectionString = "isostore:/OnlineMoviesDb.sdf";
        public static string DataBaseConnectionstringForSqlite = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "OnlineMoviesDb.sqlite").ToString();
    }
}
