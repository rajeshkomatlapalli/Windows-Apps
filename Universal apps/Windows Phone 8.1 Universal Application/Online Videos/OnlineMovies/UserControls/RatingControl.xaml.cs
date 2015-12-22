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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnlineVideos.Views.RatingControlTest
{
    public partial class RatingControl : UserControl
    {
        public RatingControl()
        {
            this.InitializeComponent();
        }

        DependencyProperty RateValueProperty = DependencyProperty.Register("RateValue", typeof(double), typeof(RatingControl), new PropertyMetadata(0.1, UpdateValue));

        private static void UpdateValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RatingControl control = d as RatingControl;
            control.Value = (double)e.NewValue;
        }


        private double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }


        private static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(RatingControl), new PropertyMetadata(0.1));


        public double RateValue
        {
            get
            {
                return (double)GetValue(RateValueProperty);
            }
            set
            {
                SetValue(RateValueProperty, value);
            }
        }

    }
}
