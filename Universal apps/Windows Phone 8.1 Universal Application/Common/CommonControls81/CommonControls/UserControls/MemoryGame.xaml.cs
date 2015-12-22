using Common.Library;
using OnlineVideosCardGame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Reflection;
using Microsoft.PlayerFramework;
using OnlineVideosCardGame.Model;
using OnlineVideosCardGame.Helper;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class MemoryGame : UserControl
    {
        private static ToggleButton togglebtn = default(ToggleButton);

        MediaElement RootMediaElement = default(MediaElement);
        //MediaPlayer RootMediaElement = new MediaPlayer();

        public MemoryGame()
        {
            this.InitializeComponent();
            Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
            RootMediaElement = (MediaElement)border.FindName("MediaPlayer");            
            Unloaded += MemoryGame_Unloaded;
            OnlineVideosCardGame.Messenger.Default.Register<string>(this, (msg) =>
            {
                if (OnlineVideosCardGame.Constants.EndGameMessage == msg)
                {
                    VisualStateManager.GoToState(this, "EndState", true);
                    if (RootMediaElement.CurrentState.ToString() == "Closed" || RootMediaElement.CurrentState.ToString() == "Stopped" || (RootMediaElement.Source.ToString().Contains("Claps.mp3") && RootMediaElement.CurrentState.ToString() == "Paused"))
                    {
                        RootMediaElement.Source = new Uri("ms-appx:///Claps.mp3", UriKind.RelativeOrAbsolute);
                        RootMediaElement.Play();
                    }
                }
            });
            VisualStateGroup.CurrentStateChanged += VisualStateGroup_CurrentStateChanged;
        }
        #region Events

        void VisualStateGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            currentState = e.NewState.Name;
            if (currentState == "GameState")
                if (RootMediaElement.CurrentState.ToString() == "Playing")
                {
                    if (RootMediaElement.Source.ToString().Contains("Claps.mp3"))
                    {
                        RootMediaElement.Stop();
                    }
                }
        }
        string currentState = "";
        #endregion

        void MemoryGame_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                IEnumerable<MemoryCard> mf = OnlineVideosCardGame.MemoryViewModelLocator.MainStatic.MemoryCardList.Where(m => m.Upside == true);
                if (mf != null)
                {
                    foreach (MemoryCard mcar in mf)
                    {
                        mcar.Upside = false;
                    }
                }
                if (RootMediaElement.CurrentState.ToString() == "Playing")
                {
                    if (RootMediaElement.Source.ToString().Contains("Claps.mp3"))
                    {
                        RootMediaElement.Stop();
                    }
                }
            }
            catch(Exception ex)
            {
                string Exception = ex.Message;
            }
        }

        private void ToggleButton_Unchecked_1(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState((Control)sender, "Unchecked", true);
        }

        private void ToggleButton_Checked_1(object sender, RoutedEventArgs e)
        {
            if (AppSettings.ProjectName != "Bollywood Music")
            {
                Page p = (Page)OnlineVideos.Common.PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                p.GetType().GetTypeInfo().GetDeclaredMethod("HelpPopupClose").Invoke(p, null);
            }
            VisualStateManager.GoToState((Control)sender, "Checked", true);
            CallDatacontextMethodAction c = new CallDatacontextMethodAction(sender, "OnSelectionChanged");
        }

        private void btnNewGame_Click_1(object sender, RoutedEventArgs e)
        {
            CallDatacontextMethodAction c = new CallDatacontextMethodAction(sender, "Start");
            VisualStateManager.GoToState(this, "GameState", true);
        }

        private void btnResume_Click_1(object sender, RoutedEventArgs e)
        {
            CallDatacontextMethodAction c = new CallDatacontextMethodAction(sender, "OnResume");
            VisualStateManager.GoToState(this, "GameState", true);
        }

        private void btnPNew_Click_1(object sender, RoutedEventArgs e)
        {
            CallDatacontextMethodAction c = new CallDatacontextMethodAction(sender, "Start");
            VisualStateManager.GoToState(this, "GameState", true);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CallDatacontextMethodAction c = new CallDatacontextMethodAction(this, "Start");
            if (OnlineVideosCardGame.MemoryViewModelLocator.MainStatic.MemoryCardList.Count > 0)
            {
                VisualStateManager.GoToState(this, "GameState", true);
            }
        }
    }
}