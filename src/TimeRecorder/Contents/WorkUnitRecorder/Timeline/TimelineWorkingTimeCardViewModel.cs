using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tasks.Definitions;
using TimeRecorder.Domain.Domain.Tracking;

namespace TimeRecorder.Contents.WorkUnitRecorder.Timeline
{
    public class WorkingTimeCardViewModel
    {

        public WorkingTimeCardViewModel(WorkTask task, WorkingTimeRange workingTimeRange)
        {
            DomainModel = workingTimeRange;

            TaskTitle = task.Title;
            TaskCategory = task.TaskCategory;

            StartHHmm = workingTimeRange.StartDateTime.ToString("HHmm");
            EndHHmm = workingTimeRange.EndDateTime?.ToString("HHmm") ?? "";

            CanvasTop = CalcTop();
            Height = CalcHeight();
        }


        private int CalcTop()
        {
            // とりあえず0時開始として考える

            var hourHeight = TimelineProperties.Current.HourHeight;

            var result = hourHeight * DomainModel.StartDateTime.Hour;
            result += (hourHeight / 60) * DomainModel.StartDateTime.Minute;

            return result;
        }

        private int CalcHeight()
        {
            var hourHeight = TimelineProperties.Current.HourHeight;

            if (DomainModel.EndDateTime.HasValue == false)
                return hourHeight;

            var d = DomainModel.EndDateTime.Value - DomainModel.StartDateTime;
            return (hourHeight / 60) * (int)d.TotalMinutes;
        }


        public WorkingTimeRange DomainModel { get; }

        public string TaskTitle { get; }

        public TaskCategory TaskCategory { get; }

        public string StartHHmm { get; set; }

        public string EndHHmm { get; set; }

        public int CanvasTop { get; }

        public int Height { get; }
    }
}
