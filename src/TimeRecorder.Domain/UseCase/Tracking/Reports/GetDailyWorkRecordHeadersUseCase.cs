using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain;
using TimeRecorder.Domain.Domain.Tracking.Reports;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.UseCase.Tracking.Reports
{
    public class GetDailyWorkRecordHeadersUseCase
    {
        private IDailyWorkRecordQueryService _DailyWorkRecordQueryService;

        public GetDailyWorkRecordHeadersUseCase(IDailyWorkRecordQueryService dailyWorkRecordQueryService)
        {
            _DailyWorkRecordQueryService = dailyWorkRecordQueryService;
        }

        public WorkingTimeRecordForReport[] Get(YmdString ymdString)
        {
            var yearMonth = YearMonth.FromYmdString(ymdString);

            var headers = _DailyWorkRecordQueryService.SelectByYearMonth(yearMonth);

            return headers.Where(h => h.Ymd.Equals(ymdString))
                          .OrderBy(h => h.StartDateTime)
                          .ToArray();
        }
    }
}
