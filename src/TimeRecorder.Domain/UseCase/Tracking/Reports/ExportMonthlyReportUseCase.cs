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

        public ExportMonthlyReportUseCase(IDailyWorkRecordQueryService dailyWorkRecordQueryService)
        {
            _DailyWorkRecordQueryService = dailyWorkRecordQueryService;
        }

        public void Export(YearMonth yearMonth)
        {
            var list = _DailyWorkRecordQueryService.SelectByYearMonth(yearMonth);

            var builder = new MonthlyReportBuilder(yearMonth);
        }
    }
}
