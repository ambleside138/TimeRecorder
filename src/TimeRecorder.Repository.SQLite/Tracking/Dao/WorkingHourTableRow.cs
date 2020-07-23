using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Repository.SQLite.Tracking.Dao
{
    class WorkingHourTableRow
    {
        public string Ymd { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public static WorkingHourTableRow FromDomainObjects(WorkingHour workingHour)
        {
            return new WorkingHourTableRow
            {
                Ymd = workingHour.Ymd.Value,
                StartTime = workingHour.StartTime?.ToString("HHmmss") ?? "",
                EndTime = workingHour.EndTime?.ToString("HHmmss") ?? "",
            };
        }

        public WorkingHour ConvertToDomainObjects()
        {
            return new WorkingHour(
                new YmdString(Ymd),
                DateTimeParser.ConvertFromYmdHHmmss(Ymd, StartTime),
                DateTimeParser.ConvertFromYmdHHmmss(Ymd, EndTime)
                );
        }
    }
}
