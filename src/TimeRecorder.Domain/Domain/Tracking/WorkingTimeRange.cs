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
    public class WorkingTimeRange : NotificationDomainModel
    {
        public Identity<WorkingTimeRange> Id { get; private set; }

        public Identity<WorkTask> TaskId { get; set; }

        #region TimePeriod変更通知プロパティ
        private TimePeriod _TimePeriod;

        public TimePeriod TimePeriod
        {
            get => _TimePeriod;
            set => RaisePropertyChangedIfSet(ref _TimePeriod, value);
        }
        #endregion

        public bool IsDoing => TimePeriod.WithinRangeAtCurrentTime;

        public int WorkSpanMinutes => TimePeriod.CalcWorkTimeMinutes();

        private ISystemClock _SystemClock => SystemClockServiceLocator.Current;

        public static WorkingTimeRange ForStart(Identity<WorkTask> taskId)
        {
            return new WorkingTimeRange 
            { 
                Id = Identity<WorkingTimeRange>.Temporary, 
                TaskId = taskId,
                TimePeriod = TimePeriod.CreateForStart(),
            };
        }

        public static WorkingTimeRange FromScheduledEvent(Identity<WorkTask> taskId, ScheduledEvent scheduledEvent)
        {
            return new WorkingTimeRange
            {
                Id = Identity<WorkingTimeRange>.Temporary,
                TaskId = taskId,
                TimePeriod = new TimePeriod(scheduledEvent.StartTime,  scheduledEvent.EndTime),
            };
        }

        private WorkingTimeRange() { }

        public WorkingTimeRange(Identity<WorkingTimeRange> identity, Identity<WorkTask> taskId, DateTime startDateTime, DateTime? endDateTime)
        {
            Id = identity;
            TaskId = taskId;
            TimePeriod = new TimePeriod(startDateTime, endDateTime);
        }


        public void Stop()
        {
            TimePeriod = new TimePeriod(TimePeriod.StartDateTime, _SystemClock.Now);
        }

        public void EditTimes(DateTime start, DateTime? end)
        {
            if(end.HasValue
                && start > end.Value)
            {
                throw new SpecificationCheckException("終了時刻は開始時刻以降である必要があります");
            }

            TimePeriod = new TimePeriod(start, end);
        }
    }
}
