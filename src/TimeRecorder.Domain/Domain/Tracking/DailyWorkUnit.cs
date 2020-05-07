using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Tracking
{
    /// <summary>
    /// 日々記録していく作業単位
    /// </summary>
    public class DailyWorkUnit
    {
        public Identity<WorkTask> TaskId { get; set; }


    }
}
