using System;
using System.ComponentModel;
using Common.Utilities;
using Windows.Storage;

namespace Common.Library
{
    public class ApplicationIdleModeHelper : INotifyPropertyChanged
    {

        #region Initialize

        static ApplicationIdleModeHelper current;

        public static ApplicationIdleModeHelper Current
        {
            get
            {
                if (null == current)
                {
                    current = new ApplicationIdleModeHelper();
                }
                System.Diagnostics.Debug.Assert(current != null);

                return current;
            }
        }


        //public void Initialize()
        //{            
        //    System.Diagnostics.Debug.WriteLine("initialized " + PhoneApplicationService.Current.StartupMode);


        //    bool fromStorage = false;
        //    if (IsolatedStorageSettings.ApplicationSettings.TryGetValue("RunsUnderLock", out fromStorage))
        //    {
        //        if (fromStorage != (PhoneApplicationService.Current.ApplicationIdleDetectionMode == IdleDetectionMode.Disabled))
        //        {
        //            RunsUnderLock = fromStorage;
        //            System.Diagnostics.Debug.WriteLine("Did not match");
        //        }
        //        else
        //        {
        //            System.Diagnostics.Debug.WriteLine("Matched");
        //            runsUnderLock = fromStorage;
        //        }
        //    }

        //    bool hasPrompted = false;
        //    if (IsolatedStorageSettings.ApplicationSettings.TryGetValue("HasUserAgreedToRunUnderLock", out hasPrompted))
        //    {
        //        HasUserAgreedToRunUnderLock = hasPrompted;
        //    }
        //}

        #endregion

        #region private members

        bool runsUnderLock;
        bool isRunningUnderLock;
        bool hasUserAgreedToRunUnderLock;
        bool isRestartRequired;

        #endregion

        #region public properties

        public bool RunsUnderLock
        {
            get
            {
                return runsUnderLock;
            }
            set
            {
                if (value != runsUnderLock)
                {
                    runsUnderLock = value;


                    if (runsUnderLock)
                    {
                        //PhoneApplicationService.Current.ApplicationIdleDetectionMode = IdleDetectionMode.Disabled;
                        Windows.System.Display.DisplayRequest KeepScreenOnRequest = new Windows.System.Display.DisplayRequest();
                        KeepScreenOnRequest.RequestActive();

                        if (PageHelper.RootApplicationFrame != null)
                        {
                            //PageHelper.RootApplicationFrame.Obscured += new EventHandler<ObscuredEventArgs>(rootframe_Obscured);
                            //PageHelper.RootApplicationFrame.Unobscured += new EventHandler(rootframe_Unobscured);
                        }

                    }
                    else
                    {
                        IsRestartRequired = true;
                        EventHandler eh = RestartRequired;
                        if (eh != null)
                            eh(this, new EventArgs());
                    }

                    SaveSetting("RunsUnderLock", runsUnderLock);
                    OnPropertyChanged("RunsUnderLock");
                }
            }
        }

        public bool IsRestartRequired
        {
            get
            {
                return isRestartRequired;
            }
            private set
            {
                if (value != isRestartRequired)
                {
                    isRestartRequired = value;
                    OnPropertyChanged("IsRestartRequired");
                }
            }
        }

        public bool HasUserAgreedToRunUnderLock
        {
            get
            {
                return hasUserAgreedToRunUnderLock;
            }
            set
            {
                if (value != hasUserAgreedToRunUnderLock)
                {
                    hasUserAgreedToRunUnderLock = value;
                    SaveSetting("HasUserAgreedToRunUnderLock", hasUserAgreedToRunUnderLock);
                    OnPropertyChanged("HasUserAgreedToRunUnderLock");
                }
            }
        }

        public void SaveSetting(string key, object value)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                ApplicationData.Current.LocalSettings.Values[key] = value;
            }
            //if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
            //{
            //    IsolatedStorageSettings.ApplicationSettings[key] = value;
            //}
            else
                ApplicationData.Current.LocalSettings.Values.Add(key, value);
                //IsolatedStorageSettings.ApplicationSettings.Add(key, value);

            //ApplicationData.Current.LocalSettings.Values.Save();
            //IsolatedStorageSettings.ApplicationSettings.Save();
        }

        void rootframe_Unobscured(object sender, EventArgs e)
        {
            IsRunningUnderLock = false;

        }

        //void rootframe_Obscured(object sender, ObscuredEventArgs e)
        //{
        //    if (e.IsLocked)
        //    {
        //        IsRunningUnderLock = e.IsLocked;
        //    }
        //}

        public bool IsRunningUnderLock
        {
            get
            {
                return isRunningUnderLock;
            }
            private set
            {
                if (value != isRunningUnderLock)
                {
                    isRunningUnderLock = value;
                    OnPropertyChanged("IsRunningUnderLock");

                    if (isRunningUnderLock)
                    {
                        EventHandler eh = Locked;
                        if (eh != null)
                            eh(this, new EventArgs());
                    }
                    else
                    {
                        EventHandler eh = UnLocked;
                        if (eh != null)
                            eh(this, new EventArgs());
                    }

                }

            }
        }

        #endregion

        #region events

        public event EventHandler Locked;
        public event EventHandler UnLocked;
        public event EventHandler RestartRequired;

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler ph = PropertyChanged;
            if (ph != null)
                ph(this, new PropertyChangedEventArgs(propertyName));

        }

        #endregion

    }
}
