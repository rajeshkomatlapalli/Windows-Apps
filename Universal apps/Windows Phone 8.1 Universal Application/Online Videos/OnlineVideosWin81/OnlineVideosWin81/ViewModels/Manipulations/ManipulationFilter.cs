using Common.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideos.ViewModels.Manipulations
{
    public class ManipulationFilter
    {
        private static float TargetMinSize = 247.5F;
        private static float TargetMaxSize = 1250F;
        private static float TargetMinInside = 247.5F;

        public static void Clamp(object sender, Manipulations.FilterManipulationEventArgs args)
        {
            var inputProcessor = sender as Manipulations.InputProcessor;
            var target = inputProcessor.Target;
            var container = inputProcessor.Reference;
            var rect = target.RenderTransform.TransformBounds(
                new Windows.Foundation.Rect(0, 0, target.ActualWidth, target.ActualHeight));
            Constants.ImageRectangle = rect;
            Constants.CurrentImageWidth = rect.Width;
            Constants.CurrentImageHeight = rect.Height;

            var translate = new Windows.Foundation.Point
            {
                X = args.Delta.Translation.X,
                Y = args.Delta.Translation.Y
            };
            if ((args.Delta.Translation.X > 0 && args.Delta.Translation.X > container.ActualWidth - rect.Left - ManipulationFilter.TargetMinInside) ||
                (args.Delta.Translation.X < 0 && args.Delta.Translation.X < ManipulationFilter.TargetMinInside - rect.Right) ||
                (args.Delta.Translation.Y > 0 && args.Delta.Translation.Y > container.ActualHeight - rect.Top - 243.75) ||
                (args.Delta.Translation.Y < 0 && args.Delta.Translation.Y < 243.75 - rect.Bottom))
            {
                translate.X = 0;
                translate.Y = 0;
            }

            float scale = args.Delta.Scale < 1F ?
                (float)System.Math.Max(ManipulationFilter.TargetMinSize / System.Math.Min(rect.Width, rect.Height), args.Delta.Scale) :
                (float)System.Math.Min(ManipulationFilter.TargetMaxSize / System.Math.Max(rect.Width, rect.Height), args.Delta.Scale);

            args.Delta = new Windows.UI.Input.ManipulationDelta
            {
                Rotation = args.Delta.Rotation,
                Scale = scale,
                Translation = translate
            };
        }
    }
}