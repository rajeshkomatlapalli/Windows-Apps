using Common.Library;
using OnlineVideos.Views;
using OnlineVideosCardGame.Helper;
using OnlineVideosCardGame.Model;
//using OnlineVideos.Game;
//using OnlineVideos.Game.MemoryGame.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideosWin81.Controls
{
    public sealed partial class MemoryGame_1 : UserControl
    {
        AppInsights insights = new AppInsights();
       MediaElement RootMediaElement = new MediaElement();
       public MemoryGame_1()
        {
            try
            {
                this.InitializeComponent();
                Border border = (Border)VisualTreeHelper.GetChild(Window.Current.Content as Frame, 0);
                //RootMediaElement = (MediaElement)border.FindName("MediaPlayer");
                Unloaded += MemoryGame_1_Unloaded;
                OnlineVideosCardGame.Messenger.Default.Register<string>(this, (msg) =>
                {
                    if (OnlineVideosCardGame.Constants.EndGameMessage == msg)
                    {
                        VisualStateManager.GoToState(this, "EndState", true);
                        if (RootMediaElement.CurrentState.ToString() == "Closed" || RootMediaElement.CurrentState.ToString() == "Stopped" || RootMediaElement.CurrentState.ToString() == "Paused")
                        {                            
                            StorageFile fileToTest = Task.Run(async () => await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Claps.mp3"))).Result;
                            var stream =Task.Run(async()=> await fileToTest.OpenAsync(FileAccessMode.Read)).Result;
                            RootMediaElement.SetSource(stream, fileToTest.ContentType);
                            RootMediaElement.Volume = 10;
                            RootMediaElement.Play();
                        }
                    }
                });
                VisualStateGroup.CurrentStateChanged += VisualStateGroup_CurrentStateChanged;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                insights.Exception(ex);
            }
        }

        void VisualStateGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            currentState = e.NewState.Name;
            if (currentState == "GameState")
            {
                if (RootMediaElement.CurrentState.ToString() == "Playing")
                {
                    RootMediaElement.Stop();
                }
            }
        }
        string currentState = "";
        void MemoryGame_1_Unloaded(object sender, RoutedEventArgs e)
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
                RootMediaElement.Stop();                
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                insights.Event("MemoryGame Loaded");
                CallDatacontextMethodAction c = new CallDatacontextMethodAction(this, "Start");
                if (OnlineVideosCardGame.MemoryViewModelLocator.MainStatic.MemoryCardList.Count > 0)
                {
                    VisualStateManager.GoToState(this, "GameState", true);
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                insights.Exception(ex);
            }
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

        private void ToggleButton_Unchecked_1(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState((Control)sender, "Unchecked", true);
        }

        private void ToggleButton_Checked_1(object sender, RoutedEventArgs e)
        {
            if (AppSettings.ProjectName != "Bollywood Music")
            {
                //Page p = (Page)PageNavigationManager.GetParentOfTypePage(this, typeof(Page));
                //p.GetType().GetTypeInfo().GetDeclaredMethod("HelpPopupClose").Invoke(p, null);
            }
            object nn = sender;
            VisualStateManager.GoToState((Control)sender, "Checked", true);
            CallDatacontextMethodAction c = new CallDatacontextMethodAction(sender, "OnSelectionChanged");
        }

        
    }
}
