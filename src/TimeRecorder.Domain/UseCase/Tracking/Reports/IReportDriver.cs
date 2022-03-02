using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tracking.Reports;

namespace TimeRecorder.Domain.UseCase.Tracking.Reports;

public interface IReportDriver
{
    ExportResult ExportMonthlyReport(DailyWorkRecordHeader[] dailyWorkRecordHeaders, string filePath, bool autoAdjust);
}

public class ExportResult
{
    public bool IsSuccessed { get; set; }

    public string Message { get; set; }
}
