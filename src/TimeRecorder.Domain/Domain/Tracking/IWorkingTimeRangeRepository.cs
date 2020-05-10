using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Tracking
{
    public interface IWorkingTimeRangeRepository
    {
        WorkingTimeRange Add(WorkingTimeRange workingTimeRange);

        void Edit(WorkingTimeRange workingTimeRange);

        void Remove(Identity<WorkingTimeRange> identity);

        WorkingTimeRange[] SelectByYmd(string ymd);
    }
}
