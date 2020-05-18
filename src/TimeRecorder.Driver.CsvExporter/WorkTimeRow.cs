using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Driver.CsvExporter
{
    class WorkTimeRow
    {
        public string DateText { get; set; }

        public string TaskCategory { get; set; }

        public string ProductOrClient { get; set; }

        public string TaskProcess { get; set; }

        public string Remarks { get; set; }

        public string ManHour { get; set; }
    }
}
