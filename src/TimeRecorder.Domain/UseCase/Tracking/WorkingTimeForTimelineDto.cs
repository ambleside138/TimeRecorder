using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.UseCase.Tracking
{
    public class WorkingTimeForTimelineDto 
    {
        public Identity<WorkingTimeRange> WorkingTimeId { get; set; }
        public Identity<WorkTask> WorkTaskId { get; set; }
        public TimePeriod TimePeriod { get; set; }
        public string TaskTitle { get; set; }
        public TaskCategory TaskCategory { get; set; }
        public string WorkProcessName { get; set; }
    }

    public class WorkingTimeForTimelineDtoEqualityComparer : IEqualityComparer<WorkingTimeForTimelineDto>
    {
        public bool Equals(WorkingTimeForTimelineDto b1, WorkingTimeForTimelineDto b2)
        {
            if (b2 == null && b1 == null)
                return true;
            else if (b1 == null || b2 == null)
                return false;
            else if (b1.WorkingTimeId == b2.WorkingTimeId)
                return true;
            else
                return false;
        }

        public int GetHashCode(WorkingTimeForTimelineDto bx)
        {
            return bx.WorkingTimeId.GetHashCode();
        }
    }
}
