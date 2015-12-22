namespace Mvvm.Services.Sound
{
    using System;
    using System.Threading.Tasks;
    using Windows.Media;
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;

    /// <summary>
    /// Delivers foreground and background sounds.
    /// </summary>
    internal class SoundPlayer : BindableBase
    {
        private static SoundPlayer instance = new SoundPlayer();
        private static MediaElement ForegroundPlayer { get; set; }
        private static MediaElement BackgroundPlayer { get; set; }

        public static SoundPlayer Instance
        {
            get
            {
                return instance;
            }
        }

        public void Initialize()
        {
            // Register media elements to the Sound Service.
            try
            {               
                DependencyObject rootGrid = VisualTreeHelper.GetChild(Window.Current.Content, 0);
                var foregroundPlayer = (MediaElement)VisualTreeHelper.GetChild(rootGrid, 0) as MediaElement;
                var backgroundPlayer = (MediaElement)VisualTreeHelper.GetChild(rootGrid, 1) as MediaElement;
            
                SoundPlayer.ForegroundPlayer = foregroundPlayer;

                // Keep the state.
                var isMuted = this.IsBackgroundMuted;                
                SoundPlayer.BackgroundPlayer = backgroundPlayer;
                this.IsBackgroundMuted = isMuted;
                
            }
            catch (Exception)
            {
                // Most probably you forgot to apply the custom root frame style.
                SoundPlayer.ForegroundPlayer = null;
                SoundPlayer.BackgroundPlayer = null;
            }
        }

        public bool IsBackgroundMuted
        {
            get
            {
                if (BackgroundPlayer == null)
                {
                    return false;
                }

                return BackgroundPlayer.IsMuted;
            }

            set
            {
                if (BackgroundPlayer != null)
                {
                    BackgroundPlayer.IsMuted = value;
                    this.OnPropertyChanged("IsBackgroundMuted");
                }
            }
        }

        public async Task Play(String sound, bool inBackground = false)
        {                        
            var mediaElement = inBackground ? BackgroundPlayer : ForegroundPlayer;

            if (mediaElement == null)
            {
                return;
            }

            string source = string.Format(sound.ToString());
            mediaElement.Source = new Uri(source);

            await mediaElement.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                mediaElement.Stop();
                mediaElement.Play();
            });
        }
    }
}
