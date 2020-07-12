using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.UseCase.Tracking.Reports;

namespace TimeRecorder.Driver.CsvDriver.Import
{
    public class CsvWorkingHourImportDriver : IWorkingHourImportDriver
    {
        public WorkingHour[] Import(string param)
        {
            // .Net CoreでSJISを扱うために呼ぶ必要がある
            // パッケージも必要: System.Text.Encoding.CodePages
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (var streamReader = new StreamReader(param, Encoding.GetEncoding("shift_jis")))
            using (var csv = new CsvReader(streamReader, CultureInfo.CurrentCulture))
            {
                csv.Configuration.HasHeaderRecord = false;

                return csv.GetRecords<WorkingHourRow>()
                          .Select(r => r.ConvertToDomainModel())
                          .ToArray();
            }
        }
    }
}
