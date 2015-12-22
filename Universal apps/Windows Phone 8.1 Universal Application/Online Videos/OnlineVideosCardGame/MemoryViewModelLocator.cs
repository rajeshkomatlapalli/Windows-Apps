using OnlineVideosCardGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideosCardGame
{
    public class MemoryViewModelLocator
    {
        private static MainViewModel _main;

        /// <summary>
        /// Initializes a new instance of the MemoryViewModelLocator class.
        /// </summary>
        public MemoryViewModelLocator()
        {
            //if (ViewModelBase.IsInDesignModeStatic)
            //{
            //    // Create design time view models
            //}
            //else
            //{
            //    // Create run time view models
            //}

            // create in App.xaml.cs application start event
            //CreateMain();
        }

        public static void Cleanup()
        {
            ClearMain();
        }

        #region Main

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        public static MainViewModel MainStatic
        {
            get
            {
                if (_main == null)
                {
                    CreateMain();
                }

                return _main;
            }
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return MainStatic;
            }
        }

        /// <summary>
        /// Provides a deterministic way to delete the Main property.
        /// </summary>
        public static void ClearMain()
        {
            if (_main != null)
            {
                _main.Cleanup();
                _main = null;
            }
        }

        /// <summary>
        /// Provides a deterministic way to create the Main property.
        /// </summary>
        public static void CreateMain()
        {
            if (_main == null)
            {
                if (AppState != null)
                {
                    _main = new MainViewModel(AppState);
                    AppState = null;
                }
                else
                {
                    _main = new MainViewModel();
                }
            }
        }

        public static AppState AppState = null;

        /// <summary>
        /// Provides a deterministic way to create the Main property.
        /// </summary>
        public static void SetAppState(AppState appState)
        {
            AppState = appState;
        }

        #endregion
    }
}
