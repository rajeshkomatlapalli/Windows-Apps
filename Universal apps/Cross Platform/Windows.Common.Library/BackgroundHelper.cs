using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
#if WINDOWS_PHONE_APP
//using System.Windows.Threading;
using System.Threading;
using Windows.UI.Xaml;
#endif
#if WINDOWS_APP
using Windows.UI.Xaml;
#endif
namespace Common.Library
{
    public class BackgroundHelper
    {
#if WINDOWS_APP && NOTANDROID
        BackgroundWorker bw = new BackgroundWorker();
        private List<BackgroundWorker> backgroundWorkers;
        private Dictionary<int, DispatcherTimer> scheduledTasks;
        private List<object> inputArguments;

        bool m_bReportsProgress;
        public bool ReportsProgress
        {
            get { return m_bReportsProgress; }
            set { m_bReportsProgress = value; }
        }

        bool m_bSupportsCancellation;
        public bool SupportsCancellation
        {
            get { return m_bSupportsCancellation; }
            set { m_bSupportsCancellation = value; }
        }

        public BackgroundHelper()
        {
            ReportsProgress = true;
            SupportsCancellation = true;

            backgroundWorkers = new List<BackgroundWorker>();
            scheduledTasks = new Dictionary<int, DispatcherTimer>();
            inputArguments = new List<object>();
        }

        public void AddBackgroundTask(DoWorkEventHandler work, RunWorkerCompletedEventHandler callback)
        {
            AddBackgroundTask(work, callback, null);
        }

        public void AddBackgroundTask(DoWorkEventHandler work, RunWorkerCompletedEventHandler callback, object argument)
        {
            BackgroundWorker worker = new BackgroundWorker();

            worker.WorkerReportsProgress = ReportsProgress;
            worker.WorkerSupportsCancellation = SupportsCancellation;

            if (work != null)
                worker.DoWork += new DoWorkEventHandler(work);

            if (callback != null)
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(callback);

            inputArguments.Add(argument);

            backgroundWorkers.Add(worker);
        }

        public void AddScheduledBackgroundTask(EventHandler<object> scheduledWork, TimeSpan interval)
        {
            AddScheduledBackgroundTask(scheduledWork, interval, scheduledTasks.Count);
        }

        public void AddScheduledBackgroundTask(EventHandler<object> scheduledWork, TimeSpan interval, int index)
        {
            DispatcherTimer scheduledTimer = new DispatcherTimer();
           
            scheduledTimer.Interval = interval;

            if (scheduledWork != null)
                scheduledTimer.Tick += scheduledWork;

            if (!scheduledTasks.ContainsKey(index))
                scheduledTasks.Add(index, scheduledTimer);
        }

        public void RunBackgroundWorkers()
        {
            int argumentIndex = 0;
            foreach (var worker in backgroundWorkers)
                worker.RunWorkerAsync(inputArguments[argumentIndex++]);
        }

        public void StartScheduledTasks()
        {
            foreach (var task in scheduledTasks)
                task.Value.Start();
        }

        public void StopScheduledTasks(int index)
        {
            scheduledTasks[index].Stop();
        }

        public void StartScheduledTasks(int index)
        {
            scheduledTasks[index].Start();
        }
#endif
#if WINDOWS_PHONE_APP && NOTANDROID
        BackgroundWorker bw = new BackgroundWorker();
        private List<BackgroundWorker> backgroundWorkers;
        private Dictionary<int, DispatcherTimer> scheduledTasks;
        private List<object> inputArguments;

        bool m_bReportsProgress;
        public bool ReportsProgress
        {
            get { return m_bReportsProgress; }
            set { m_bReportsProgress = value; }
        }

        bool m_bSupportsCancellation;
        public bool SupportsCancellation
        {
            get { return m_bSupportsCancellation; }
            set { m_bSupportsCancellation = value; }
        }

        public BackgroundHelper()
        {
            ReportsProgress = true;
            SupportsCancellation = true;

            backgroundWorkers = new List<BackgroundWorker>();
            scheduledTasks = new Dictionary<int, DispatcherTimer>();
            inputArguments = new List<object>();
        }

        public void AddBackgroundTask(DoWorkEventHandler work, RunWorkerCompletedEventHandler callback)
        {
            AddBackgroundTask(work, callback, null);
        }

        public void AddBackgroundTask(DoWorkEventHandler work, RunWorkerCompletedEventHandler callback, object argument)
        {
            BackgroundWorker worker = new BackgroundWorker();

            worker.WorkerReportsProgress = ReportsProgress;
            worker.WorkerSupportsCancellation = SupportsCancellation;

            if (work != null)
                worker.DoWork += new DoWorkEventHandler(work);

            if (callback != null)
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(callback);

            inputArguments.Add(argument);

            backgroundWorkers.Add(worker);
        }

        public void AddScheduledBackgroundTask(EventHandler<object> scheduledWork, TimeSpan interval)
        {
            AddScheduledBackgroundTask(scheduledWork, interval, scheduledTasks.Count);
        }

        public void AddScheduledBackgroundTask(EventHandler<object> scheduledWork, TimeSpan interval, int index)
        {
            DispatcherTimer scheduledTimer = new DispatcherTimer();

            scheduledTimer.Interval = interval;

            if (scheduledWork != null)
                scheduledTimer.Tick += new EventHandler<object>(scheduledWork);

            if (!scheduledTasks.ContainsKey(index))
                scheduledTasks.Add(index, scheduledTimer);
        }

        public void RunBackgroundWorkers()
        {
            int argumentIndex = 0;
            foreach (var worker in backgroundWorkers)
                worker.RunWorkerAsync(inputArguments[argumentIndex++]);
        }

        public void StartScheduledTasks()
        {
            foreach (var task in scheduledTasks)
                task.Value.Start();
        }

        public void StopScheduledTasks(int index)
        {
            scheduledTasks[index].Stop();
        }

        public void StartScheduledTasks(int index)
        {
            scheduledTasks[index].Start();
        }
#endif

#if ANDROID
		
		private List<BackgroundWorker> backgroundWorkers;
		private Dictionary<int,DispatcherTimer> scheduledTasks;
		private  List<object> inputArguments;

		bool m_bReportsProgress;
		public bool ReportsProgress
		{
			get { return m_bReportsProgress; }
			set { m_bReportsProgress = value; }
		}

		bool m_bSupportsCancellation;
		public bool SupportsCancellation
		{
			get { return m_bSupportsCancellation; }
			set { m_bSupportsCancellation = value; }
		}

		public BackgroundHelper()
		{
			ReportsProgress = true;
			SupportsCancellation = true;

			backgroundWorkers = new List<BackgroundWorker>();
			scheduledTasks = new Dictionary<int, DispatcherTimer>();
			inputArguments = new List<object>();
		}

		public void AddBackgroundTask(DoWorkEventHandler work, RunWorkerCompletedEventHandler callback)
		{
			AddBackgroundTask(work, callback, null);
		}

		public void AddBackgroundTask(DoWorkEventHandler work, RunWorkerCompletedEventHandler callback, object argument)
		{
			BackgroundWorker worker = new BackgroundWorker();

			worker.WorkerReportsProgress = ReportsProgress;
			worker.WorkerSupportsCancellation = SupportsCancellation;

			if (work != null)
				worker.DoWork += new DoWorkEventHandler(work);

			if (callback != null)
				worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(callback);

			inputArguments.Add(argument);

			backgroundWorkers.Add(worker);
		}

		public void AddScheduledBackgroundTask(EventHandler scheduledWork, TimeSpan interval)
		{
			AddScheduledBackgroundTask(scheduledWork, interval, scheduledTasks.Count);
		}

		public void AddScheduledBackgroundTask(EventHandler scheduledWork, TimeSpan interval,int index)
		{

			DispatcherTimer scheduledTimer = new DispatcherTimer();
			scheduledTimer.Interval = interval;

			if (scheduledWork != null)
				scheduledTimer.Tick += new EventHandler(scheduledWork);

			if (!scheduledTasks.ContainsKey(index))
				scheduledTasks.Add(index, scheduledTimer);
		}


		public void RunBackgroundWorkers()
		{
			int argumentIndex = 0;
			foreach (var worker in backgroundWorkers)
				worker.RunWorkerAsync(inputArguments[argumentIndex++]);
		}

		public void StartScheduledTasks()
		{
			foreach (var task in scheduledTasks)
				task.Value.Start();
		}

		public void StopScheduledTasks(int index)
		{
			scheduledTasks[index].Stop();
		}

		public void StartScheduledTasks(int index)
		{
			scheduledTasks[index].Start();
		}
#endif
    }
}
