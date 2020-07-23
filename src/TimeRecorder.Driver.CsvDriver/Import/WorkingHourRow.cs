using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Driver.CsvDriver.Import
{
    class WorkingHourRow
    {
        public string Ymd { get; set; }

        public string StartYmdHms { get; set; }

        public string EndYmdHms { get; set; }


        public WorkingHour ConvertToDomainModel()
        {
            return new WorkingHour
            (
                new YmdString(Ymd),
                DateTimeParser.ConvertFromYmdHHmmss(StartYmdHms),
                DateTimeParser.ConvertFromYmdHHmmss(EndYmdHms)
            );
        }
    }
}
