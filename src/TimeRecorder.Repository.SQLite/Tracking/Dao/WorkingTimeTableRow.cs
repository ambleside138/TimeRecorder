using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tracking;

namespace TimeRecorder.Repository.SQLite.Tracking.Dao;

class WorkingTimeTableRow
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public string Ymd { get; set; }

    public string StartTime { get; set; }

    public string EndTime { get; set; }

    public WorkingTimeRange ToDomainObject()
    {
        var startDateTime = ToDateTime(StartTime);
        var endDateTime = ToDateTime(EndTime);

        return new WorkingTimeRange(
            new Domain.Identity<WorkingTimeRange>(Id),
            new Domain.Identity<WorkTask>(TaskId),
            startDateTime.Value,
            endDateTime);
    }

    public static WorkingTimeTableRow FromDomainObject(WorkingTimeRange workingTimeRange)
    {
        return new WorkingTimeTableRow
        {
            Id = workingTimeRange.Id.Value,
            TaskId = workingTimeRange.TaskId.Value,
            Ymd = workingTimeRange.TimePeriod.TargetYmd,
            StartTime = workingTimeRange.TimePeriod.StartDateTime.ToString("HHmmss"),
            EndTime = workingTimeRange.TimePeriod.EndDateTime?.ToString("HHmmss") ?? "",
        };
    }

    public DateTime? ToDateTime(string hhmm)
    {
        if (string.IsNullOrEmpty(hhmm))
            return null;

        if (DateTime.TryParseExact(Ymd + hhmm, "yyyyMMddHHmmss", null, DateTimeStyles.AssumeLocal, out DateTime result))
            return result;

        return null;
    }
}
