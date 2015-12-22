
using Common.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace OnlineVideos.ViewModels.Manipulations
{
    #region FilterManipulation

    public delegate void FilterManipulation(object sender, FilterManipulationEventArgs args);

    public class FilterManipulationEventArgs
    {
        internal FilterManipulationEventArgs(Windows.UI.Input.ManipulationUpdatedEventArgs args)
        {
            Delta = args.Delta;
            Pivot = args.Position;
        }

        public Windows.UI.Input.ManipulationDelta Delta
        {
            get;
            set;
        }

        public Windows.Foundation.Point Pivot
        {
            get;
            set;
        }
    }

    #endregion

    public sealed class ManipulationManager : InputProcessor
    {
        private bool _handlersRegistered;

        private Windows.UI.Xaml.Media.MatrixTransform _initialTransform;
        private Windows.UI.Xaml.Media.MatrixTransform _previousTransform;
        private Windows.UI.Xaml.Media.CompositeTransform _deltaTransform;
        private Windows.UI.Xaml.Media.TransformGroup _transform;

      
        public FilterManipulation OnFilterManipulation
        {
            get;
            set;
        }

        public ManipulationManager(Windows.UI.Xaml.FrameworkElement element, Grid parent)
            : base(element, parent)
        {
            this._handlersRegistered = false;
            this.InitialTransform = this._target.RenderTransform;
            this.ResetManipulation();
        }

        public Windows.UI.Xaml.Media.Transform InitialTransform
        {
            get { return this._initialTransform; }
            set
            {
               
                this._initialTransform = new Windows.UI.Xaml.Media.MatrixTransform()
                {
                    Matrix = new Windows.UI.Xaml.Media.TransformGroup()
                    {
                        Children = { value }
                    }.Value
                };
            }
        }

    
        public void Configure(bool scale, bool rotate, bool translate, bool inertia)
        {
            var settings = new Windows.UI.Input.GestureSettings();

            if (scale)
            {
                settings |= Windows.UI.Input.GestureSettings.ManipulationScale;
                if (inertia)
                {
                    settings |= Windows.UI.Input.GestureSettings.ManipulationScaleInertia;
                }
            }
            if (rotate)
            {
                settings |= Windows.UI.Input.GestureSettings.ManipulationRotate;
                if (inertia)
                {
                    settings |= Windows.UI.Input.GestureSettings.ManipulationRotateInertia;
                }
            }
            if (translate)
            {
                settings |= Windows.UI.Input.GestureSettings.ManipulationTranslateX |
                    Windows.UI.Input.GestureSettings.ManipulationTranslateY;
                if (inertia)
                {
                    settings |= Windows.UI.Input.GestureSettings.ManipulationTranslateInertia;
                }
            }
            this._gestureRecognizer.GestureSettings = settings;

            this.ConfigureHandlers(scale || rotate || translate);
        }

        public void ResetManipulation()
        {
            
            this._previousTransform = new Windows.UI.Xaml.Media.MatrixTransform()
            {
                Matrix = this._initialTransform.Matrix
            };

          
            this._deltaTransform = new Windows.UI.Xaml.Media.CompositeTransform();

          
            this._transform = new Windows.UI.Xaml.Media.TransformGroup()
            {
                Children = { this._previousTransform, this._deltaTransform }
            };

       
            this._target.RenderTransform = this._transform;
        }
        public double[] center(double offsetx, double offsety)
        {
            var pp = System.Math.Truncate(offsetx);
            double[] centter = new double[2];
            if (System.Math.Truncate(offsetx) == 0)
            {
                centter[0] = 0;
            }
            if (System.Math.Truncate(offsety) == 0)
            {
                centter[1] = 0;
            }
            if (System.Math.Truncate(offsetx) < 0)
            {
                centter[0] = -offsetx + 125;
            }
            if (System.Math.Truncate(offsety) < 0)
            {
                centter[1] = -offsety + 100;
            }
            if (System.Math.Truncate(offsetx) > 0)
            {
                centter[0] = 0;
            }
            if (System.Math.Truncate(offsety) > 0)
            {
                centter[1] = 0;
            }
            return centter;
        }
        private void OnManipulationUpdated(Windows.UI.Input.GestureRecognizer sender, Windows.UI.Input.ManipulationUpdatedEventArgs args)
        {
          

            var filteredArgs = new FilterManipulationEventArgs(args);
            if (OnFilterManipulation != null)
            {
                OnFilterManipulation(this, filteredArgs);
            }

            this._previousTransform.Matrix = _transform.Value;

            if (Constants.PreviousImageHeight < Constants.CurrentImageHeight && Constants.CurrentImageWidth > Constants.PreviousImageWidth)
            {
                this._deltaTransform.CenterX = _transform.Value.OffsetX + 125;
                this._deltaTransform.CenterY = _transform.Value.OffsetY + 100;
            }
            else
            {
                double[] qrr = center(_transform.Value.OffsetX, _transform.Value.OffsetY);
                this._deltaTransform.CenterX = qrr[0];
                this._deltaTransform.CenterY = qrr[1];
            }
           Constants.PreviousImageWidth= Constants.CurrentImageWidth;
            Constants.PreviousImageHeight =Constants.CurrentImageHeight;
            this._deltaTransform.Rotation = filteredArgs.Delta.Rotation;
            this._deltaTransform.ScaleX = filteredArgs.Delta.Scale;
            this._deltaTransform.ScaleY = filteredArgs.Delta.Scale;
            this._deltaTransform.TranslateX = filteredArgs.Delta.Translation.X;
            this._deltaTransform.TranslateY = filteredArgs.Delta.Translation.Y;


        }
        private void ConfigureHandlers(bool register)
        {
            if (register && !_handlersRegistered)
            {
                this._gestureRecognizer.ManipulationUpdated += OnManipulationUpdated;

                this._handlersRegistered = true;
            }
            else if (!register && _handlersRegistered)
            {
                this._gestureRecognizer.ManipulationUpdated -= OnManipulationUpdated;

                this._handlersRegistered = false;
            }
        }
    }
}
