using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.UseCase.Tracking.Reports;

namespace TimeRecorder.Domain.Domain.Tracking.Reports
{
    /// <summary>
    /// 月次の工数集計処理を行うメソッドを提供します
    /// </summary>
    internal class MonthlyReportBuilder
    {
        private readonly YearMonth _YearMonth;

        public MonthlyReportBuilder(YearMonth yearMonth)
        {
            _YearMonth = yearMonth;
        }

        public DailyWorkRecordHeader[] Build(DailyWorkResults results)
        {
            var records = results.WorkingTimeRecordForReports;

            var list = new List<DailyWorkRecordHeader>();
            if (records.Length == 0)
                return new DailyWorkRecordHeader[0];

            var date = _YearMonth.StartDate;
            while(date.Month == _YearMonth.StartDate.Month)
            {
                // 日付のループ
                var oHeader = new DailyWorkRecordHeader 
                { 
                    WorkYmd = date.ToString("yyyyMMdd"),
                    WorkingHour = results.WorkingHours.FirstOrDefault(h => h.Ymd.Equals(new YmdString(date))),
                };

                foreach(var record in records.Where(r => r.Ymd.ToDateTime().Value.Day == date.Day))
                {
                    oHeader.AddWorkTask(record);
                }

                list.Add(oHeader);
                date = date.AddDays(1);
            }

            return list.ToArray();
        }
    }
}
