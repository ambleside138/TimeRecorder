using Livet;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.UseCase.Tracking;
using TimeRecorder.Domain.Utility.SystemClocks;
using TimeRecorder.Host;

namespace TimeRecorder.Contents.WorkUnitRecorder.Timeline
{
    /// <summary>
    /// 時系列エリアに表示するための作業時間ViewModelを表します
    /// </summary>
    public class TimelineWorkingTimeCardViewModel : ViewModel
    {
        private readonly ISystemClock _SystemClock = SystemClockServiceLocator.Current;


        public TimelineWorkingTimeCardViewModel(WorkingTimeForTimelineDto workingTimeRange)
        {
            DomainModel = workingTimeRange;

            // 1sスパンで更新する
            var timer = new ReactiveTimer(TimeSpan.FromSeconds(1), new SynchronizationContextScheduler(SynchronizationContext.Current));
            timer.Subscribe(_ => UpdateDurationTime());
            timer.AddTo(CompositeDisposable);
            timer.Start();
     
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

            StartHHmm = workingTimeRange.TimePeriod.StartDateTime.ToString("HHmm");
            EndHHmm = workingTimeRange.TimePeriod.EndDateTime?.ToString("HHmm") ?? "";

            CanvasTop = CalcTop();
            ActualHeight.Value = CalcActualHeight();

        }

        public void UpdateDurationTime()
        {
            if (DomainModel == null)
                return;

            var diff = _SystemClock.Now - DomainModel.TimePeriod.StartDateTime;
            DurationTimeText.Value = $"{diff.Hours:00}:{diff.Minutes:00}:{diff.Seconds:00}";

            ActualHeight.Value = CalcActualHeight();
        }

        private int CalcTop()
        {
            if (DomainModel == null)
                return 0;

            // とりあえず0時開始として考える

            var hourHeight = TimelineProperties.Current.HourHeight;

            var result = hourHeight * DomainModel.TimePeriod.StartDateTime.Hour;
            result += (hourHeight / 60) * DomainModel.TimePeriod.StartDateTime.Minute;
            result -= hourHeight * TimelineProperties.Current.StartHour;

            return result;
        }

        private int CalcActualHeight()
        {
            if (DomainModel == null)
                return 0;

            var endDateTime = DomainModel.TimePeriod.EndDateTime ?? _SystemClock.Now;

            var d = endDateTime - DomainModel.TimePeriod.StartDateTime;
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
