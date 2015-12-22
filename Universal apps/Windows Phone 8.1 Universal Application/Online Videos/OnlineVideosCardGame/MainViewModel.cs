using Common.Library;
using OnlineVideos.Entities;
using OnlineVideosCardGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace OnlineVideosCardGame
{
   public class MainViewModel : ViewModelBase
    {
        public DispatcherTimer storyboardtimer = new DispatcherTimer();

        #region MemoryCardList

        /// <summary>
        /// The <see cref="MemoryCardList" /> property's name.
        /// </summary>
        public const string MemoryCardListPropertyName = "MemoryCardList";
        public const string RowPropertyName = "numberofrows";
        private List<MemoryCard> _memoryCardList = new List<MemoryCard>();


        /// <summary>
        /// Gets the MemoryCardList property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        private int _numberofrows;
        public int numberofrows
        {
            get { return _numberofrows; }
            set
            {
                if (_numberofrows == value)
                {
                    return;
                }
                _numberofrows = value;
                RaisePropertyChanged(RowPropertyName);
            }
        }
        public List<MemoryCard> MemoryCardList
        {
            get
            {
                return _memoryCardList;
            }

            set
            {
                if (_memoryCardList == value)
                {
                    return;
                }

                _memoryCardList = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(MemoryCardListPropertyName);
            }
        }

        #endregion

        #region TimeCounter

        /// <summary>
        /// The <see cref="TimeCounter" /> property's name.
        /// </summary>
        public const string TimeCounterPropertyName = "TimeCounter";

        private int _timeCounter = 0;

        /// <summary>
        /// Gets the TimeCounter property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int TimeCounter
        {
            get
            {
                return _timeCounter;
            }

            set
            {
                if (_timeCounter == value)
                {
                    return;
                }

                _timeCounter = value;


                // Update bindings, no broadcast
                RaisePropertyChanged(TimeCounterPropertyName);

            }
        }

        #endregion

        #region MoveCounter

        /// <summary>
        /// The <see cref="MoveCounter" /> property's name.
        /// </summary>
        public const string MoveCounterPropertyName = "MoveCounter";

        private int _moveCounter = 0;

        /// <summary>
        /// Gets the MoveCounter property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public int MoveCounter
        {
            get
            {
                return _moveCounter;
            }

            set
            {
                if (_moveCounter == value)
                {
                    return;
                }

                _moveCounter = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(MoveCounterPropertyName);

            }
        }

        #endregion

        #region PairCounter

        /// <summary>
        /// The <see cref="PairCounter" /> property's name.
        /// </summary>
        public const string PairCounterPropertyName = "PairCounter";

        private int _pairCounter = 0;

        /// <summary>
        /// Gets the PairCounter property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public int PairCounter
        {
            get
            {
                return _pairCounter;
            }

            set
            {
                if (_pairCounter == value)
                {
                    return;
                }

                _pairCounter = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(PairCounterPropertyName);

            }
        }

        #endregion

        #region CardSize

        /// <summary>
        /// The <see cref="CardSize" /> property's name.
        /// </summary>
        public const string CardSizePropertyName = "CardSize";

        private int _cardSize = 100;

        /// <summary>
        /// Gets the CardSize property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int CardSize
        {
            get
            {
                return _cardSize;
            }

            set
            {
                if (_cardSize == value)
                {
                    return;
                }

                _cardSize = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(CardSizePropertyName);
            }
        }

        #endregion

        #region Difficulty

        /// <summary>
        /// The <see cref="Difficulty" /> property's name.
        /// </summary>
        public const string DifficultyPropertyName = "Difficulty";

        //private Difficulties _difficulty = Difficulties.Easy;

        /// <summary>
        /// Gets the Difficulty property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        //public Difficulties Difficulty
        //{
        //    get
        //    {
        //        return _difficulty;
        //    }

        //    set
        //    {
        //        if (_difficulty == value)
        //        {
        //            return;
        //        }

        //        _difficulty = value;

        //        // Update bindings, no broadcast
        //        RaisePropertyChanged(DifficultyPropertyName);
        //    }
        //}

        #endregion

        #region IsBusy

        /// <summary>
        /// The <see cref="IsBusy" /> property's name.
        /// </summary>
        public const string IsBusyPropertyName = "IsBusy";

        private bool _isBusy = false;

        /// <summary>
        /// Gets the IsBusy property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                if (_isBusy == value)
                {
                    return;
                }

                _isBusy = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(IsBusyPropertyName);
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            initialize();
        }

        public MainViewModel(AppState state)
        {
            initialize();

            CardSize = state.CardSize;
            //Difficulty = state.Difficulty;
            MemoryCardList = state.MemoryCardList;
            MoveCounter = state.MoveCounter;
            PairCounter = state.PairCounter;
            TimeCounter = state.TimeCounter;
            foreach (var card in MemoryCardList)
            {
                card.Image = _imageCache[card.ID];
            }
        }

        private void loadimages()
        {
            SQLite.SQLiteAsyncConnection conn = new SQLite.SQLiteAsyncConnection(Constants.DataBaseConnectionstringForSqlite);
            // DataManager<ShowCast> castmanager = new DataManager<ShowCast>();
            //var q1= castmanager.GetDistinctFromList(i => i.ShowID == Convert.ToInt32(AppSettings.ShowID), j => j.PersonID).ToList();
            int showid = Convert.ToInt32(AppSettings.ShowID);
            List<CastProfile> q = new List<CastProfile>();
            q = Task.Run(async () => await conn.Table<CastProfile>().OrderBy(j => j.PersonID).ToListAsync()).Result.Join(Task.Run(async () => await conn.Table<ShowCast>().Where(i => i.ShowID == showid).OrderBy(j => j.PersonID).ToListAsync()).Result, p => p.PersonID, o => o.PersonID, (p, o) => p).ToList();
            _imageCache = new List<BitmapImage>();
            foreach (CastProfile img in q.Distinct())
            {

                try
                {
                    var bi = new BitmapImage();
                    bi.CreateOptions = BitmapCreateOptions.None;
                    bi.UriSource = new Uri(img.FlickrPersonImageUrl, UriKind.RelativeOrAbsolute);
                    _imageCache.Add(bi);
                }
                catch (Exception)
                {


                }

                //string filePath = "";
                //StorageFolder store1 = ApplicationData.Current.LocalFolder;

                //if (Task.Run(async () => await Storage.FileExists("Images\\PersonImages\\" + q[i] + ".jpg")).Result)
                //{

                //    StorageFile file = Task.Run(async () => await store1.GetFileAsync("Images\\PersonImages\\" + q[i].PersonID + ".jpg")).Result;
                //    IRandomAccessStream fileStream = new InMemoryRandomAccessStream();
                //    using (fileStream = Task.Run(async () => await file.OpenAsync(Windows.Storage.FileAccessMode.Read)).Result)
                //    {
                //        var bi = new BitmapImage();
                //        bi.CreateOptions = BitmapCreateOptions.None;
                //        bi.SetSource(fileStream);
                //        Task.Run(async () => await fileStream.FlushAsync());
                //        fileStream.Dispose();

                //        _imageCache.Add(bi);
                //    }

                //}
                //else
                //{
                //    StorageFolder store = Package.Current.InstalledLocation as StorageFolder;
                //    string filePath1 = store.Path + filePath;
                //    var bi = new BitmapImage();
                //    bi.CreateOptions = BitmapCreateOptions.None;
                //    bi.UriSource = new Uri(filePath1 + "/Images/PersonImages/" + q[i].PersonID + ".jpg", UriKind.Relative);
                //    _imageCache.Add(bi);
                //}

            }
        }

        private void initialize()
        {
            Messenger.Default.Register<string>(this, Constants.PauseMessage, OnPause);

            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 1, 500);
            _timer.Tick += _timer_Tick;

            _elapsedTimer = new DispatcherTimer();
            _elapsedTimer.Interval = new TimeSpan(0, 0, 1);
            _elapsedTimer.Tick += _elapsedTimer_Tick;

            loadimages();
        }

        void _timer_Tick(object sender, object e)
        {
            _timer.Stop();
            hideCards(default(MemoryCard));
        }

        void _elapsedTimer_Tick(object sender, object e)
        {
            TimeCounter++;
        }

        [DoNotSerialize]
        List<BitmapImage> _imageCache;

        public void Start()
        {
            loadimages();
            _gameEnded = false;
            IsBusy = true;
            LoadCards();
            IsBusy = false;
            StartGame();
        }

        private void StartGame()
        {
            MoveCounter = 0;
            TimeCounter = 0;
            PairCounter = 0;
            //_elapsedTimer.Start();
        }



        /// <summary>
        /// ************************************
        /// loads the cards
        /// ************************************
        /// </summary>        
        public void LoadCards()
        {
            try
            {
                SQLite.SQLiteAsyncConnection conn = new SQLite.SQLiteAsyncConnection(Constants.DataBaseConnectionstringForSqlite);
                int showid = Convert.ToInt32(AppSettings.ShowID);

                string filePath = System.IO.Path.Combine(Common.Library.Constants.PersonImagePathForDownloads.Substring(1), "back.png");
                StorageFolder store = Package.Current.InstalledLocation as StorageFolder;
                string filePath1 = store.Path + filePath;
                BitmapImage b = new BitmapImage();
                b.CreateOptions = BitmapCreateOptions.None;
                b.UriSource = new Uri("ms-appx://" + filePath, UriKind.RelativeOrAbsolute);
                double height = 0;
                double width = 0;
                int cardCount = 0;

                Thickness t = new Thickness(0, 0, 0, 0);

                cardCount = _imageCache.Count();
                if (AppSettings.ProName.Contains("WindowsPhone"))
                {
                    if (cardCount >= 6 && cardCount < 10)
                    {

                        t = new Thickness(30, 10, 0, 0);
                        height = 80;
                        width = 85;
                        cardCount = 6;
                        CardSize = 85;


                    }
                    else if (cardCount >= 10 && cardCount < 15)
                    {

                        t = new Thickness(9, 0, 0, 0);
                        height = 80;
                        width = 85;
                        cardCount = 10;
                        CardSize = 85;

                    }
                    else if (cardCount >= 15)
                    {

                        t = new Thickness(15, 5, 0, 0);
                        height = 75;
                        width = 75;
                        cardCount = 15;
                        CardSize = 68;

                    }
                }
                else
                {
                    if (cardCount >= 6 && cardCount < 10)
                    {

                        t = new Thickness(30, 10, 0, 0);
                        height = 95;
                        width = 95;
                        cardCount = 6;
                        CardSize = 105;


                    }
                    else if (cardCount >= 10 && cardCount < 15)
                    {

                        t = new Thickness(9, 0, 0, 0);
                        height = 75;
                        width = 75;
                        cardCount = 10;
                        CardSize = 95;

                    }
                    else if (cardCount >= 15)
                    {

                        t = new Thickness(15, 5, 0, 0);
                        height = 75;
                        width = 75;
                        cardCount = 15;
                        CardSize = 68;

                    }
                }

                // clear
                //MemoryCardList.Clear();
                var tempList = new List<MemoryCard>();
                // add cards
                for (int i = 0; i < cardCount; i++)
                {
                    //var bi = new BitmapImage(new Uri(string.Format("Images/mm{0}.png", i), UriKind.Relative));
                    tempList.Add(new MemoryCard(i, t, height, width)
                    {

                        storyboardfromx = "0",
                        storyboardtox = "0",
                        storyboardfromy = "0",
                        storyboardtoy = "0",
                        storyboardfromz = "0",
                        storyboardtoz = "0",
                        imgfored = Visibility.Collapsed,
                        Upside = false,
                        Image = _imageCache[i],//bi

                        backimage = b

                    });

                    tempList.Add(new MemoryCard(i, t, height, width)
                    {
                        storyboardfromx = "0",
                        storyboardtox = "0",
                        storyboardfromy = "0",
                        storyboardtoy = "0",
                        storyboardfromz = "0",
                        storyboardtoz = "0",
                        Upside = false,
                        imgfored = Visibility.Collapsed,
                        Image = _imageCache[i],//bi
                        backimage = b
                    });
                }

                // shuffle them
                var rand = new Random();

                for (int i = tempList.Count - 1; i > 0; i--)
                {
                    int n = rand.Next(i + 1);
                    var temp = tempList[i];
                    tempList[i] = tempList[n];
                    tempList[n] = temp;
                }

                // set list
                MemoryCardList = tempList;

                if (MemoryCardList.Count == 12)
                {
                    numberofrows = 3;
                    Common.Library.Constants.GameGridColumnWidth = 400;

                }
                else if (MemoryCardList.Count == 20)
                {
                    numberofrows = 4;
                    Common.Library.Constants.GameGridColumnWidth = 425;
                }
                else if (MemoryCardList.Count == 30)
                {
                    numberofrows = 5;
                    Common.Library.Constants.GameGridColumnWidth = 376;
                }

            }
            catch (Exception ex)
            {

                Exceptions.SaveOrSendExceptions("Exception in LoadCards Method In MainViewModel.cs file", ex);
            }
        }

        // timer for turning back upside cards
        DispatcherTimer _timer;
        // track elapsed time in game
        DispatcherTimer _elapsedTimer;

        DispatcherTimer _endTimer;

        private void hideCards(MemoryCard m)
        {

            foreach (var item in MemoryCardList.Where(mh => mh != m))
            {
                item.Upside = false;
            }

        }
        public void OnSelectionChanged(MemoryCard lastCard)
        {

            foreach (MemoryCard m in GameInstance.stopstoryboard)
            {
                m.storyboardfromx = "0";
                m.storyboardtox = "0";
                m.storyboardfromy = "0";
                m.storyboardtoy = "0";
                m.storyboardfromz = "0";
                m.storyboardtoz = "0";
            }
            if (storyboardtimer.IsEnabled == true)
            {
                storyboardtimer.Stop();
            }
            _elapsedTimer.Start();
            GameInstance.gameopened = true;
            lastCard.imgfored = Visibility.Visible;
            int upsideNo = MemoryCardList.Count((m) => m.Upside);

            if (upsideNo == 2)
            {
                MemoryCard upsideCard = null;
                // one is upside + lastCard is 2, check IDs
                MoveCounter++;
                var upsideCards = MemoryCardList.Where(m => m.ID != lastCard.ID && m.Upside == true);
                if (upsideCards.Count() > 0)
                {
                    upsideCard = upsideCards.First();
                }
                else
                {
                    foreach (MemoryCard mc in MemoryCardList.Where(m => m.Upside == true))
                    {
                        if (mc != lastCard)
                        {
                            upsideCard = mc;
                            upsideCard.imgfored = Visibility.Visible;
                        }
                    }

                }

                if (upsideCard.ID == lastCard.ID)
                {
                    if (MemoryCardList.Count((m) => m.Solved == Visibility.Visible) == 2)
                    {

                        _elapsedTimer.Stop();
                        EndGame();
                    }
                    upsideCard.SetSolved();
                    lastCard.SetSolved();
                    PairCounter++;
                    storyboardtimer.Interval = TimeSpan.FromSeconds(10);
                    storyboardtimer.Tick += storyboardtimer_Tick;
                    storyboardtimer.Start();
                    return;
                }
                //if (App.gameinstance.gametimer.IsEnabled == true)
                //    App.gameinstance.gametimeend();
                //else
                //    App.gameinstance.gametimestart();

                if (!_timer.IsEnabled)
                {
                    _timer.Start();
                }
            }
            else if (upsideNo == 3)
            {
                // two is already upside, hide them, lastCard will be the only upside
                _timer.Stop();
                hideCards(lastCard);
            }

            storyboardtimer.Interval = TimeSpan.FromSeconds(10);
            storyboardtimer.Tick += storyboardtimer_Tick;
            storyboardtimer.Start();



        }

        void storyboardtimer_Tick(object sender, object e)
        {
            if (GameInstance.gameopened == true)
            {
                foreach (MemoryCard m in GameInstance.stopstoryboard)
                {
                    m.storyboardfromx = "3";
                    m.storyboardtox = "-3";
                    m.storyboardfromy = "3";
                    m.storyboardtoy = "-3";
                    m.storyboardfromz = "3";
                    m.storyboardtoz = "-3";
                }
                GameInstance.gameopened = false;
                storyboardtimer.Stop();
            }
        }


        private bool _gameEnded = false;

        public void EndGame()
        {


            _gameEnded = true;
            if (_endTimer == null)
            {
                _endTimer = new DispatcherTimer();
                _endTimer.Interval = TimeSpan.FromMilliseconds(Constants.FlipTimeMs);
                _endTimer.Tick += (s, e) =>
                {
                    _endTimer.Stop();
                    Messenger.Default.Send(Constants.EndGameMessage);
                };
            }
            _endTimer.Start();
        }


        private void OnPause(string s)
        {
            _elapsedTimer.Stop();
            if (_timer.IsEnabled)
            {
                _timer.Stop();
            }
        }

        public void OnResume()
        {
            _elapsedTimer.Start();
        }

        public AppState GetAppState()
        {
            // check if game started
            if (MemoryCardList.Count == 0) return null;
            // check if game ended
            if (_gameEnded) return null;
            return new AppState()
            {
                CardSize = this.CardSize,
                //Difficulty = this.Difficulty,
                MemoryCardList = this.MemoryCardList,
                MoveCounter = this.MoveCounter,
                PairCounter = this.PairCounter,
                TimeCounter = this.TimeCounter
            };
        }
    }
}
