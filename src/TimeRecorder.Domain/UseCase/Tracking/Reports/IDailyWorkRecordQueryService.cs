using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain;

namespace TimeRecorder.Domain.UseCase.Tracking.Reports
{
    public interface IDailyWorkRecordQueryService
    {
        WorkingTimeRecordForReport[] SelectByYearMonth(YearMonth yearMonth);
    }
}
