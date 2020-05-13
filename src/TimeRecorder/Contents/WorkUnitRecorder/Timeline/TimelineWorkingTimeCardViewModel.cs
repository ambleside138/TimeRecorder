using Livet;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tasks.Definitions;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.UseCase.Tracking;
using TimeRecorder.Domain.Utility.SystemClocks;

namespace TimeRecorder.Contents.WorkUnitRecorder.Timeline
{
    public class WorkingTimeCardViewModel : ViewModel
    {
        private readonly ISystemClock _SystemClock = SystemClockServiceLocator.Current;

        public WorkingTimeCardViewModel(WorkingTimeForTimelineDto workingTimeRange)
        {
            DomainModel = workingTimeRange;

            if (DomainModel == null)
            {
                TaskTitle = "お休み中";
                TaskCategory = TaskCategory.Develop;
                return;
            }

            TaskTitle = workingTimeRange.TaskTitle;
            TaskCategory = workingTimeRange.TaskCategory;

            StartHHmm = workingTimeRange.StartDateTime.ToString("HHmm");
            EndHHmm = workingTimeRange.EndDateTime?.ToString("HHmm") ?? "";

            CanvasTop = CalcTop();
            Height = CalcHeight();

            var timer = new ReactiveTimer(TimeSpan.FromSeconds(1)).AddTo(CompositeDisposable); // 1秒スパン
            timer.Subscribe(_ => UpdateDurationTime());
            timer.Start();
        }

        private void UpdateDurationTime()
        {
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

        private int CalcHeight()
        {
            if (DomainModel == null)
                return 0;

            var hourHeight = TimelineProperties.Current.HourHeight;

            if (DomainModel.EndDateTime.HasValue == false)
                return hourHeight;

            var d = DomainModel.EndDateTime.Value - DomainModel.StartDateTime;
            return (hourHeight / 60) * (int)d.TotalMinutes;
        }


        public WorkingTimeForTimelineDto DomainModel { get; }

        public string TaskTitle { get; }

        public TaskCategory TaskCategory { get; }

        public string StartHHmm { get; set; } = "";

        public string EndHHmm { get; set; } = "";

        public int CanvasTop { get; }

        public int Height { get; }

        public ReactiveProperty<string> DurationTimeText { get; } = new ReactiveProperty<string>();
    }
}
