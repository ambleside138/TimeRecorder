using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tracking;

namespace TimeRecorder.Repository.SQLite.Tracking.Dao
{
    class WorkingTimeTableRow
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        public string Ymd { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public WorkingTimeRange ConvertToDomainObject()
        {
            var startDateTime = ToDateTime(StartTime);
            var endDateTime = ToDateTime(EndTime);

            return new WorkingTimeRange(
                new Domain.Utility.Identity<WorkingTimeRange>(Id),
                new Domain.Utility.Identity<System.Threading.Tasks.Task>(TaskId),
                startDateTime.Value,
                endDateTime );
        }

        public static WorkingTimeTableRow FromDomainObject(WorkingTimeRange workingTimeRange)
        {
            return new WorkingTimeTableRow
            {
                Id = workingTimeRange.Id.Value,
                TaskId = workingTimeRange.TaskId.Value,
                Ymd = workingTimeRange.TargetYmd,
                StartTime = workingTimeRange.StartDateTime.ToString("HHmm"),
                EndTime = workingTimeRange.EndDateTime?.ToString("HHmm") ?? "",
            };
        }

        public DateTime? ToDateTime(string hhmm)
        {
            if (string.IsNullOrEmpty(hhmm))
                return null;

            if (DateTime.TryParseExact(Ymd + hhmm, "yyyyMMddHHmm", null, System.Globalization.DateTimeStyles.AssumeLocal, out DateTime result))
                return result;

            return null;
        }
    }
}
