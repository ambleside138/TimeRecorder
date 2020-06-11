using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Tracking.Reports;
using TimeRecorder.Domain.UseCase.Tracking;
using TimeRecorder.Domain.UseCase.Tracking.Reports;

namespace TimeRecorder.Driver.CsvExporter
{
    public class CsvReportDriver : IReportDriver
    {
        private DailyWorkRecordHeaderToWorkTimeRowConverter _Converter = new DailyWorkRecordHeaderToWorkTimeRowConverter();
        
        public void ExportMonthlyReport(DailyWorkRecordHeader[] dailyWorkRecordHeaders, string filePath)
        {
            var rows = dailyWorkRecordHeaders.SelectMany(h => _Converter.Convert(h))
                                             .Where(r => r.ManHour != "0")
                                             .ToArray();

            // .Net CoreでSJISを扱うために呼ぶ必要がある
            // パッケージも必要: System.Text.Encoding.CodePages
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (var sw = new StreamWriter(filePath, false, Encoding.GetEncoding("shift_jis")))
            using (var csv = new CsvHelper.CsvWriter(sw, CultureInfo.CurrentCulture))
            {
                // ヘッダーなし
                csv.Configuration.HasHeaderRecord = false;

                // データを読み出し
                csv.WriteRecords(rows);
            }
        }
    }
}
