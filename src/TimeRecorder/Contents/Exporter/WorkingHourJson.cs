using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Contents.Exporter
{
    class WorkingHourJson
    {
        public WorkingHourJsonRecord[] records { get; set; }
    }

    class WorkingHourJsonRecord
    {
        public string ymd { get; set; }

        public string start { get; set; }

        public string end { get; set; }

        public WorkingHour ConvertToDomainModel()
        {
            var startDateTime = DateTimeParser.ConvertFromYmdHHmmss(ymd, start);
            var endDateTime = DateTimeParser.ConvertFromYmdHHmmss(ymd, end);
            return new WorkingHour(new YmdString(ymd), startDateTime, endDateTime);
        }
    }
}
