using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tracking.Reports;

namespace TimeRecorder.Domain.UseCase.Tracking.Reports
{
    public interface IReportDriver
    {
        void ExportMonthlyReport(DailyWorkRecordHeader[] dailyWorkRecordHeaders, string filePath);
    }
}
