//# define DEBUG_AGENT
using System;
using Common.Library;

namespace OnlineVideos.Utilities
{
    public class RegisterAgent
    {
        public void backagent()
        {
            try
            {
                //var oldTask = ScheduledActionService.Find(ResourceHelper.ProjectName) as PeriodicTask;
                //if (oldTask != null)
                //    ScheduledActionService.Remove(ResourceHelper.ProjectName);

                //PeriodicTask task = new PeriodicTask(ResourceHelper.ProjectName);

                //task.Description = "Periodically Updates Primary Tile for " + ResourceHelper.ProjectName;

                //ScheduledActionService.Add(task);
//#if(DEBUG_AGENT)
//                ScheduledActionService.LaunchForTest(ResourceHelper.ProjectName, TimeSpan.FromSeconds(6));
//#endif

            }
            catch (InvalidOperationException ex)
            {
                AppSettings.BackgroundAgenError = ex.StackTrace;
                Exceptions.SaveOrSendExceptions("Exception in backagent Method In RegisterAgent.cs file.", ex);
            }
        }
    }
}
