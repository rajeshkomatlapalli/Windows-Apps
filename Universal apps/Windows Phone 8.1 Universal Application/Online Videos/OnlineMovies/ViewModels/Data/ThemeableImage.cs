using System;
#if WP8
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes; 
#endif
using System.Windows;
using Common.Library;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace OnlineVideos
{
    public class ThemeableImage : Grid
    {
        Rectangle _rectangle;

        public ThemeableImage()
        {
            _rectangle = new Rectangle();

            var bgBrush = AppResources.PhoneBackgroundBrush;
            if (bgBrush.Color == Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF))
                _rectangle.Fill = new SolidColorBrush(Colors.Black);
            else
                _rectangle.Fill = new SolidColorBrush(Colors.White);

            Children.Add(_rectangle);
        }

        public ImageSource Source
        {
            get
            {
                if (_rectangle.Opacity != null)
                {
                    //return (_rectangle.OpacityMask as ImageBrush).ImageSource;
                    return null;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                //_rectangle.OpacityMask = new ImageBrush { ImageSource = value };
            }
        }
    }
}
