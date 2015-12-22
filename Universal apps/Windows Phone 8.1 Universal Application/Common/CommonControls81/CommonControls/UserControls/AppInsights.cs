using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVideos.Views
{
   public class AppInsights
    {
       private TelemetryClient telemetry = new TelemetryClient();

       public void pageview(string pagename)
       {
           telemetry.TrackPageView(pagename);
       }

       public void Event(EventTelemetry name)
       {
           telemetry.TrackEvent(name);
           
       }
        public void Dependency(DependencyTelemetry name)
        {
            telemetry.TrackDependency(name);
        }
        public void Dependency(string dependencyName, string commandName, DateTimeOffset startTime, TimeSpan duration, bool success)
        {
            telemetry.TrackDependency(dependencyName, commandName, startTime, duration, success);
        }
        public void Event(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
       {
            telemetry.TrackEvent(eventName, properties, metrics);
       }
       public void Exception(ExceptionTelemetry ex)
       {
           telemetry.TrackException(ex);
       }
       public void Exception(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
       {
           telemetry.TrackException(exception, properties, metrics);
       }
       public void Metric(MetricTelemetry name)
       {
           telemetry.TrackMetric(name);
       }
       public void Metric(string name, double value, IDictionary<string, string> properties = null)
       {
           telemetry.TrackMetric(name, value, properties);
       }
       public void Request(RequestTelemetry request)
       {
           telemetry.TrackRequest(request);
       }
       public void Request(string name, DateTimeOffset timestamp, TimeSpan duration, string responseCode, bool success)
       {
           telemetry.TrackRequest(name, timestamp, duration, responseCode, success);
       }
       public void Trace(string message)
       {
           telemetry.TrackTrace(message);
       }
       public void Trace(TraceTelemetry message)
       {
           telemetry.TrackTrace(message);
       }
       public void Trace(string message, IDictionary<string, string> properties)
       {
           telemetry.TrackTrace(message, properties);
       }
       public void Trace(string message, SeverityLevel severityLevel)
       {
           telemetry.TrackTrace(message, severityLevel);
       }
       public void Trace(string message, SeverityLevel severityLevel, IDictionary<string, string> properties)
       {
           telemetry.TrackTrace(message, severityLevel, properties);
       }
    }
}