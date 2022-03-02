using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain;
using TimeRecorder.Domain.Domain.Tracking;

namespace TimeRecorder.Domain.UseCase.Tracking.Reports;

/// <summary>
/// 指定された月の工数を取得するメソッドを提供します
/// </summary>
public interface IDailyWorkRecordQueryService
{
    DailyWorkResults SelectByYearMonth(YearMonth yearMonth);
}

public class DailyWorkResults
{
    public WorkingTimeRecordForReport[] WorkingTimeRecordForReports { get; set; }

    public WorkingHour[] WorkingHours { get; set; }
}
