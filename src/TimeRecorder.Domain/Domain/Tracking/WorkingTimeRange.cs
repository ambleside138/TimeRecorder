using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Domain.Utility.SystemClocks;

namespace TimeRecorder.Domain.Domain.Tracking
{
    /// <summary>
    /// 作業時間の最小単位を表します
    /// </summary>
    public class WorkingTimeRange
    {
        public Identity<WorkingTimeRange> Id { get; private set; }

        public Identity<Task> TaskId { get; set; }

        public DateTime StartDateTime { get; private set; }

        public DateTime? EndDateTime { get; private set; }

        public string TargetYmd => StartDateTime.ToString("yyyyMMdd");

        public bool IsStopped => EndDateTime.HasValue;

        private static readonly ISystemClock _SystemClock = SystemClockServiceLocator.Current;

        public static WorkingTimeRange ForNew()
        {
            return new WorkingTimeRange 
            { 
                Id = Identity<WorkingTimeRange>.Temporary, 
                StartDateTime =  _SystemClock.Now,
            };
        }

        private WorkingTimeRange() { }

        public WorkingTimeRange(Identity<WorkingTimeRange> identity, Identity<Task> taskId, DateTime startDateTime, DateTime? endDateTime)
        {
            Id = identity;
            TaskId = taskId;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
        }

        public void Start()
        {
            StartDateTime = _SystemClock.Now;
        }

        public void Stop()
        {
            EndDateTime = _SystemClock.Now;
        }

        public void EditTimes(DateTime start, DateTime? end)
        {
            StartDateTime = start;
            EndDateTime = end;
        }
    }
}
