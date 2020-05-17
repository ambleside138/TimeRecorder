using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using TimeRecorder.Domain.Domain.Tracking.Reports;
using TimeRecorder.Domain.UseCase.Tracking;
using TimeRecorder.Domain.UseCase.Tracking.Reports;

namespace TimeRecorder.Driver.CsvExporter
{
    public class CsvReportDriver : IReportDriver
    {
        public void ExportMonthlyReport(DailyWorkRecordHeader[] dailyWorkRecordHeaders, string filePath)
        {
            using (var sw = new StreamWriter(filePath, true, Encoding.GetEncoding("SHIFT_JIS")))
            using (var csv = new CsvHelper.CsvWriter(sw, CultureInfo.CurrentCulture))
            {
                //// ヘッダーあり
                //csv.Configuration.HasHeaderRecord = true;
                //// マッパーを登録
                //csv.Configuration.RegisterClassMap<PersonMapper>();
                //// データを読み出し
                //csv.WriteRecords(Personのリスト);
            }
        }
    }
}
