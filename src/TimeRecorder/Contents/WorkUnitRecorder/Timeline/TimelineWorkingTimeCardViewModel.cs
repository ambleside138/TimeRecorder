using Livet;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tasks.Definitions;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.UseCase.Tracking;
using TimeRecorder.Domain.Utility.SystemClocks;
using TimeRecorder.Host;

namespace TimeRecorder.Contents.WorkUnitRecorder.Timeline
{
    /// <summary>
    /// 時系列エリアに表示するためのViewModelを表します
    /// </summary>
    public class TimelineWorkingTimeCardViewModel : ViewModel
    {
        private readonly ISystemClock _SystemClock = SystemClockServiceLocator.Current;

        private int _NoValueCount = 0;

        private const int _AlertCount = 60 * 5;

        public TimelineWorkingTimeCardViewModel(WorkingTimeForTimelineDto workingTimeRange)
        {
            DomainModel = workingTimeRange;

            if (DomainModel == null)
            {
                TaskTitle = "お休み中";
                TaskCategory = TaskCategory.Develop;
                WorkProcessName = "‐‐‐";
                ShowStopButton.Value = false;
                return;
            }

            TaskTitle = workingTimeRange.TaskTitle;
            TaskCategory = workingTimeRange.TaskCategory;
            WorkProcessName = workingTimeRange.WorkProcessName;

            StartHHmm = workingTimeRange.StartDateTime.ToString("HHmm");
            EndHHmm = workingTimeRange.EndDateTime?.ToString("HHmm") ?? "";

            CanvasTop = CalcTop();
            ActualHeight.Value = CalcActualHeight();
                
            if(workingTimeRange.EndDateTime == null)
            {
                 // 1minスパンで更新する
                 new ReactiveTimer(TimeSpan.FromMinutes(1), new SynchronizationContextScheduler(SynchronizationContext.Current)) 
                            .Subscribe(_ => ActualHeight.Value = CalcActualHeight())
                            .AddTo(CompositeDisposable); 
            }

        }

        public void UpdateDurationTime()
        {
            if (DomainModel == null)
            {
                // 一定時間タスク開始していないなら通知する
                _NoValueCount++;
                if(_NoValueCount > 0 && _NoValueCount % _AlertCount == 0)
                {
                    NotificationService.Current.Info("⏰ TimeRecorder ⏰", "作業タスクが設定されていません");
                }

                return;
            }

            _NoValueCount = 0;

            if(_SystemClock.Now < DomainModel.StartDateTime)
            {
                DurationTimeText.Value = "--:--:--";
                return;
            }

            var diff = _SystemClock.Now - DomainModel.StartDateTime;
            DurationTimeText.Value = $"{diff.Hours:00}:{diff.Minutes:00}:{diff.Seconds:00}";
        }

        private int CalcTop()
        {
            if (DomainModel == null)
                return 0;

            // とりあえず0時開始として考える

            var hourHeight = TimelineProperties.Current.HourHeight;

            var result = hourHeight * DomainModel.StartDateTime.Hour;
            result += (hourHeight / 60) * DomainModel.StartDateTime.Minute;

            return result;
        }

        private int CalcActualHeight()
        {
            if (DomainModel == null)
                return 0;

            var endDateTime = DomainModel.EndDateTime ?? _SystemClock.Now;

            var d = endDateTime - DomainModel.StartDateTime;
            var hourHeight = TimelineProperties.Current.HourHeight;
            return (hourHeight / 60) * (int)d.TotalMinutes;
        }


        public WorkingTimeForTimelineDto DomainModel { get; }

        public string TaskTitle { get; }

        public TaskCategory TaskCategory { get; }

        public string WorkProcessName { get; }

        public string StartHHmm { get; set; } = "";

        public string EndHHmm { get; set; } = "";

        public int CanvasTop { get; }

        /// <summary>
        /// 実際の時間を反映した高さ
        /// </summary>
        public ReactivePropertySlim<int> ActualHeight { get; } = new ReactivePropertySlim<int>();


        public ReactivePropertySlim<string> DurationTimeText { get; } = new ReactivePropertySlim<string>();

        public ReactivePropertySlim<bool> ShowStopButton { get; } = new ReactivePropertySlim<bool>(true);
    }
}
