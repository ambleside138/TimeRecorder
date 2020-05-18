using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain;
using TimeRecorder.Domain.Domain.Tracking.Reports;

namespace TimeRecorder.Domain.UseCase.Tracking.Reports
{
    public class ExportMonthlyReportUseCase
    {
        private IDailyWorkRecordQueryService _DailyWorkRecordQueryService;
        private IReportDriver _ReportDriver;

        public ExportMonthlyReportUseCase(
                IDailyWorkRecordQueryService dailyWorkRecordQueryService,
                IReportDriver reportDriver)
        {
            _DailyWorkRecordQueryService = dailyWorkRecordQueryService;
            _ReportDriver = reportDriver;
        }

        public void Export(YearMonth yearMonth, string path)
        {
            var builder = new MonthlyReportBuilder(yearMonth);
            var workRecords = _DailyWorkRecordQueryService.SelectByYearMonth(yearMonth);
            var summary = builder.Build(workRecords);

            _ReportDriver.ExportMonthlyReport(summary, path);
        }
    }
}
