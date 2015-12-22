using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideosCardGame.Model
{
    public class AppState
    {
        public List<MemoryCard> MemoryCardList { get; set; }

        public int TimeCounter { get; set; }

        public int MoveCounter { get; set; }

        public int PairCounter { get; set; }

        public int CardSize { get; set; }

        //public Difficulties Difficulty { get; set; }

        public AppState()
        {
        }
    }
}
