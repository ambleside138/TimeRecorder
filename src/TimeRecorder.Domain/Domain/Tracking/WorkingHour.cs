using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Tracking
{
    /// <summary>
    /// 勤務時間を表します
    /// </summary>
    public class WorkingHour
    {
        public YmdString Ymd { get; private set; }

        public DateTime? StartTime { get; private set; }

        public DateTime? EndTime { get; private set; }

        public WorkingHour(YmdString ymd, DateTime? startTime, DateTime? endTime)
        {
            Ymd = ymd;
            StartTime = startTime;
            EndTime = endTime;
        }

        private WorkingHour()
        {

        }

        public static WorkingHour CreateForStart(DateTime dateTime)
        {
            return new WorkingHour
            {
                Ymd = new YmdString(dateTime),
                StartTime = dateTime,
                EndTime = null,
            };
        }

        public void End(DateTime dateTime)
        {
            EndTime = dateTime;
        }

        public bool IsEmpty => StartTime == null && EndTime == null;
    }
}
