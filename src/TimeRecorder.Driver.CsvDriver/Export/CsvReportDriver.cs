using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Tracking.Reports;
using TimeRecorder.Domain.UseCase.Tracking;
using TimeRecorder.Domain.UseCase.Tracking.Reports;

namespace TimeRecorder.Driver.CsvDriver
{
    public class CsvReportDriver : IReportDriver
    {
        private DailyWorkRecordHeaderToWorkTimeRowConverter _Converter = new();
        
        public ExportResult ExportMonthlyReport(DailyWorkRecordHeader[] dailyWorkRecordHeaders, string filePath, bool autoAdjust)
        {
            var rows = dailyWorkRecordHeaders.SelectMany(h => _Converter.Convert(h))
                                             .Where(r => r.ManHour != "0")
                                             .ToArray();

            if(autoAdjust)
            {
                WorkingHourAdjustor.AdjustTimes(dailyWorkRecordHeaders, rows);
            }


            // .Net CoreでSJISを扱うために呼ぶ必要がある
            // パッケージも必要: System.Text.Encoding.CodePages
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // ヘッダーなし
            var config = new CsvConfiguration(CultureInfo.CurrentCulture) { HasHeaderRecord = false, };

            using (var sw = new StreamWriter(filePath, false, Encoding.GetEncoding("shift_jis")))
            using (var csv = new CsvHelper.CsvWriter(sw, config))
            {
                // データを読み出し
                csv.WriteRecords(rows);
            }


            var listMessage = rows.Select(r => r.GetWaringMessage())
                                    .Where(r => string.IsNullOrEmpty(r) == false)
                                    .Distinct()
                                    .ToArray();

            var message = new StringBuilder()
                            .AppendLine("※ 下記の日付に入力不備があります。修正してください。")
                            .AppendLine("----")
                            .AppendLine(string.Join(Environment.NewLine, listMessage));

            return new ExportResult
            {
                IsSuccessed = listMessage.Length == 0,
                Message = listMessage.Length > 0 ? message.ToString() : "",
            };
        }

    }
}
