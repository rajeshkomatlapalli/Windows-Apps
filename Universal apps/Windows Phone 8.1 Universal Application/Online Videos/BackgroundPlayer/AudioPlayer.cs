using Common.Library;
//using OnlineVideos.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Playback;

namespace BackgroundPlayer
{
    public sealed class AudioPlayer : IBackgroundTask
    {
        //AppInsights insights = new AppInsights();
        Stopwatch stopwatch = new Stopwatch();
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _systemMediaTransportControl = SystemMediaTransportControls.GetForCurrentView();
            _systemMediaTransportControl.IsEnabled = true;
            _systemMediaTransportControl.IsPlayEnabled = true;
            _systemMediaTransportControl.IsPauseEnabled = true;
            _systemMediaTransportControl.IsPreviousEnabled = true;
            _systemMediaTransportControl.IsNextEnabled = true;


            BackgroundMediaPlayer.MessageReceivedFromForeground += MessageReceivedFromForeground;
            BackgroundMediaPlayer.Current.CurrentStateChanged += BackgroundMediaPlayerCurrentStateChanged;

            // Associate a cancellation and completed handlers with the background task.
            taskInstance.Canceled += OnCanceled;
            taskInstance.Task.Completed += Taskcompleted;
            //stopwatch = System.Diagnostics.Stopwatch.StartNew();
            //var properties = new Dictionary<string, string> { { AppSettings.LinkTitle, "View" } };
            //var metrics = new Dictionary<string, double> { { "Duration", stopwatch.Elapsed.Seconds } };
            //insights.Event("Audio", properties, metrics);
            _deferral = taskInstance.GetDeferral();
        }

        private SystemMediaTransportControls _systemMediaTransportControl;
        private BackgroundTaskDeferral _deferral;
        private void Taskcompleted(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            BackgroundMediaPlayer.Shutdown();
            _deferral.Complete();
            //var properties = new Dictionary<string, string> { { AppSettings.LinkTitle, "failed" } };
            //var metrics = new Dictionary<string, double> { { "", 0.0 } };
            //insights.Event("Audio Failed", properties, metrics);
        }

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            // You get some time here to save your state before process and resources are reclaimed
            BackgroundMediaPlayer.Shutdown();
            _deferral.Complete();
        }
        private void MessageReceivedFromForeground(object sender, MediaPlayerDataReceivedEventArgs e)
        {
            ValueSet valueSet = e.Data;
            foreach (string key in valueSet.Keys)
            {
                switch (key)
                {
                    case "Play":
                        Debug.WriteLine("Starting Playback");
                        Play(valueSet[key].ToString());
                        break;
                }
            }
        }
        private void Play(string toPlay)
        {
            var mediaPlayer = BackgroundMediaPlayer.Current;
            mediaPlayer.AutoPlay = true;
            mediaPlayer.SetUriSource(new Uri(toPlay));

            //Update the universal volume control
            _systemMediaTransportControl.ButtonPressed += MediaTransportControlButtonPressed;
            _systemMediaTransportControl.IsPauseEnabled = true;
            _systemMediaTransportControl.IsPlayEnabled = true;
            _systemMediaTransportControl.DisplayUpdater.Type = MediaPlaybackType.Music;
            _systemMediaTransportControl.DisplayUpdater.MusicProperties.Title = AppSettings.AudioTitle;
            //_systemMediaTransportControl.DisplayUpdater.MusicProperties.Artist = "Test Artist";
            _systemMediaTransportControl.DisplayUpdater.Update();
        }
        private void BackgroundMediaPlayerCurrentStateChanged(MediaPlayer sender, object args)
        {
            if (sender.CurrentState == MediaPlayerState.Playing)
            {
                _systemMediaTransportControl.PlaybackStatus = MediaPlaybackStatus.Playing;
            }
            else if (sender.CurrentState == MediaPlayerState.Paused)
            {
                _systemMediaTransportControl.PlaybackStatus = MediaPlaybackStatus.Paused;
            }
        }
        private void MediaTransportControlButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            switch (args.Button)
            {
                case SystemMediaTransportControlsButton.Play:
                    BackgroundMediaPlayer.Current.Play();
                    break;
                case SystemMediaTransportControlsButton.Pause:
                    BackgroundMediaPlayer.Current.Pause();
                    break;
            }
        }
    }
}