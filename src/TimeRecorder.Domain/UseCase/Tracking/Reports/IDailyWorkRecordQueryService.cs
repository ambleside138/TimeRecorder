using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain;

namespace TimeRecorder.Domain.UseCase.Tracking.Reports
{
    /// <summary>
    /// 指定された月の工数を取得するメソッドを提供します
    /// </summary>
    public interface IDailyWorkRecordQueryService
    {
        WorkingTimeRecordForReport[] SelectByYearMonth(YearMonth yearMonth);
    }
}
