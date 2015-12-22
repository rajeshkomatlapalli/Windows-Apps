namespace Mvvm
{
    using Services.Sound;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Base class for MVVM Views.
    /// </summary>
    public class ViewBase : Page
    {
        public ViewBase()
        {
            this.Loaded += this.OnLoaded;
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
            SoundPlayer.Instance.Initialize();
        }
    }
}
