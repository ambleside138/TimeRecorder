using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Tracking.Reports
{
    public class DailyWorkTaskUnit
    {
        public Identity<WorkTask> TaskId { get; set; }

        private readonly List<WorkingTimeRange> _WorkingTimeRanges = new List<WorkingTimeRange>();

        public IReadOnlyList<WorkingTimeRange> WorkingTimeRanges => _WorkingTimeRanges;

        public string Title { get; set; }

        public int WorkMinutes { get; set; }

    }
}
