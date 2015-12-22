using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace OnlineVideosCardGame.Model
{
    public class MemoryCard : INotifyPropertyChanged
    {
        DispatcherTimer _solvedTimer;

        private void initTimer()
        {
            _solvedTimer = new DispatcherTimer();
            _solvedTimer.Interval = TimeSpan.FromMilliseconds(Constants.FlipTimeMs);
            _solvedTimer.Tick += _solvedTimer_Tick;

        }

        void _solvedTimer_Tick(object sender, object e)
        {
            _solvedTimer.Stop();
            Solved = Visibility.Collapsed;
        }
        [DoNotSerialize]
        public ImageSource backimage { get; set; }
        #region ID

        private int _ID;

        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        #endregion

        #region Image
        [DoNotSerialize]
        public ImageSource Image { get; set; }

        #endregion

        #region Upside

        /// <summary>
        /// The <see cref="Upside" /> property's name.
        /// </summary>
        public const string UpsidePropertyName = "Upside";
        public const string storyboardfromxPropertyName = "storyboardfromx";
        public const string storyboardtoxPropertyName = "storyboardtox";
        public const string storyboardfromyPropertyName = "storyboardfromy";
        public const string storyboardtoyPropertyName = "storyboardtoy";
        public const string storyboardfromzPropertyName = "storyboardfromz";
        public const string storyboardtozPropertyName = "storyboardtoz";
        private bool _upside = false;
        private string _storyboardfromx = "";
        private string _storyboardtox = "";
        private string _storyboardfromy = "";
        private string _storyboardtoy = "";
        private string _storyboardfromz = "";
        private string _storyboardtoz = "";

        /// <summary>
        /// Gets the Upside property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool Upside
        {
            get
            {
                return _upside;
            }

            set
            {
                if (_upside == value)
                {
                    return;
                }

                _upside = value;

                // Update bindings, no broadcast
                raisePropertyChanged(UpsidePropertyName);
            }
        }

        #endregion
        public string storyboardfromx
        {
            get
            {
                return _storyboardfromx;
            }

            set
            {
                if (_storyboardfromx == value)
                {
                    return;
                }

                _storyboardfromx = value;

                // Update bindings, no broadcast
                raisePropertyChanged(storyboardfromxPropertyName);
            }
        }
        public string storyboardtox
        {
            get
            {
                return _storyboardtox;
            }

            set
            {
                if (_storyboardtox == value)
                {
                    return;
                }

                _storyboardtox = value;

                // Update bindings, no broadcast
                raisePropertyChanged(storyboardtoxPropertyName);
            }
        }
        public string storyboardfromy
        {
            get
            {
                return _storyboardfromy;
            }

            set
            {
                if (_storyboardfromy == value)
                {
                    return;
                }

                _storyboardfromy = value;

                // Update bindings, no broadcast
                raisePropertyChanged(storyboardfromyPropertyName);
            }
        }
        public string storyboardtoy
        {
            get
            {
                return _storyboardtoy;
            }

            set
            {
                if (_storyboardtoy == value)
                {
                    return;
                }

                _storyboardtoy = value;

                // Update bindings, no broadcast
                raisePropertyChanged(storyboardtoyPropertyName);
            }
        }
        public string storyboardfromz
        {
            get
            {
                return _storyboardfromz;
            }

            set
            {
                if (_storyboardfromz == value)
                {
                    return;
                }

                _storyboardfromz = value;

                // Update bindings, no broadcast
                raisePropertyChanged(storyboardfromzPropertyName);
            }
        }
        public string storyboardtoz
        {
            get
            {
                return _storyboardtoz;
            }

            set
            {
                if (_storyboardtoz == value)
                {
                    return;
                }

                _storyboardtoz = value;

                // Update bindings, no broadcast
                raisePropertyChanged(storyboardtozPropertyName);
            }
        }
        #region Solved

        /// <summary>
        /// The <see cref="Solved" /> property's name.
        /// </summary>
        public const string SolvedPropertyName = "Solved";
        public const string imgforedPropertyName = "imgfored";
        public const string MemoryCardsCountPropertyName = "MemoryCardsCount";
        private Visibility _solved = Visibility.Visible;

        /// <summary>
        /// Gets the Solved property.        
        /// Changes to that property's value raise the PropertyChanged event.         
        /// </summary>        
        public Visibility Solved
        {
            get
            {
                return _solved;
            }

            set
            {
                _solved = value;

                // Update bindings, no broadcast
                raisePropertyChanged(SolvedPropertyName);
            }
        }
        private Visibility _imgfored = Visibility.Collapsed;
        public Visibility imgfored
        {
            get
            {
                return _imgfored;
            }

            set
            {
                _imgfored = value;

                // Update bindings, no broadcast
                raisePropertyChanged(imgforedPropertyName);
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void raisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        public void SetSolved()
        {
            _solvedTimer.Start();
        }
        private Thickness _gamethick;
        public Thickness gamethick
        {
            get { return _gamethick; }
            set { _gamethick = value; }
        }

        private double _gameheight;
        public double gameheight
        {
            get { return _gameheight; }
            set { _gameheight = value; }
        }
        private double _gamewidth;
        public double gamewidth
        {
            get { return _gamewidth; }
            set { _gamewidth = value; }
        }
        public MemoryCard()
        {
            _ID = 0;
            initTimer();
        }

        public MemoryCard(int id, Thickness t, double height, double width)
        {
            GameInstance.stopstoryboard.Add(this);

            _ID = id;
            _gamethick = t;
            _gameheight = height;
            _gamewidth = width;

            initTimer();
        }
    }
}
