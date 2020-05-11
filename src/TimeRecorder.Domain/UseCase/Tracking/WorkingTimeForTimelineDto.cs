using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tasks.Definitions;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.UseCase.Tracking
{
    public class WorkingTimeForTimelineDto 
    {
        public Identity<WorkingTimeRange> WorkingTimeId { get; set; }
        public Identity<WorkTask> WorkTaskId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string TaskTitle { get; set; }
        public string TaskRemarks { get; set; }
        public TaskCategory TaskCategory { get; set; }
    }
}
