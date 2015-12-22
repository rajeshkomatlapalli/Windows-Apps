using System;
using System.Diagnostics;
using System.Threading;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Storage;
using Windows.System.Threading;
using System.Threading.Tasks;
using Common.Library;
using OnlineVideos;

namespace Tasks
{
    public sealed class SampleBackgroundTask : IBackgroundTask
    {
        BackgroundTaskDeferral _deferral = null;
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            try
            {
                if (Task.Run(async () => await Storage.FileExists("Historys.xml")).Result)
                {
                    SendLocalImageTile s = new SendLocalImageTile();
                    s.UpdateTileWithImage();
                    _deferral.Complete();
                }
                else
                {
                    _deferral.Complete();
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Run Method In SampleBackgroundTask.cs file", ex);
            }
        }
    }
}