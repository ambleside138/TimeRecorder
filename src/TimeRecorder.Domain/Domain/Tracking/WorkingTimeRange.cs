using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Domain.Tracking
{
    /// <summary>
    /// 作業時間
    /// </summary>
    struct WorkingTimeRange
    {
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
    }
}
