using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Calendar;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Domain.Utility.Exceptions;
using TimeRecorder.Domain.Utility.SystemClocks;

namespace TimeRecorder.Domain.Domain.Tracking
{
    /// <summary>
    /// 作業時間の最小単位を表します
    /// </summary>
    public class WorkingTimeRange
    {
        public Identity<WorkingTimeRange> Id { get; private set; }

        public Identity<WorkTask> TaskId { get; set; }

        public DateTime StartDateTime { get; private set; }

        public DateTime? EndDateTime { get; private set; }

        public string TargetYmd => StartDateTime.ToString("yyyyMMdd");

        public bool IsStopped => EndDateTime.HasValue;

        public bool IsDoing => EndDateTime.HasValue == false;

        public int WorkSpan => EndDateTime.HasValue ? (int)(EndDateTime.Value - StartDateTime).TotalMinutes : 0;

        private static readonly ISystemClock _SystemClock = SystemClockServiceLocator.Current;

        public static WorkingTimeRange ForStart(Identity<WorkTask> taskId)
        {
            return new WorkingTimeRange 
            { 
                Id = Identity<WorkingTimeRange>.Temporary, 
                TaskId = taskId,
                StartDateTime =  _SystemClock.Now,
                EndDateTime = null,
            };
        }

        public static WorkingTimeRange FromScheduledEvent(Identity<WorkTask> taskId, ScheduledEvent scheduledEvent)
        {
            return new WorkingTimeRange
            {
                Id = Identity<WorkingTimeRange>.Temporary,
                TaskId = taskId,
                StartDateTime = scheduledEvent.StartTime,
                EndDateTime = scheduledEvent.EndTime,
            };
        }

        private WorkingTimeRange() { }

        public WorkingTimeRange(Identity<WorkingTimeRange> identity, Identity<WorkTask> taskId, DateTime startDateTime, DateTime? endDateTime)
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
            if(end.HasValue
                && start > end.Value)
            {
                throw new SpecificationCheckException("終了時刻は開始時刻以降である必要があります");
            }

            StartDateTime = start;
            EndDateTime = end;
        }
    }
}
