using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Calendar;

namespace TimeRecorder.Host
{
    public class TimeRecorderConfiguration
    {
        public WorkTaskBuilderConfig WorkTaskBuilderConfig { get; set; } = new WorkTaskBuilderConfig();
    }
}
