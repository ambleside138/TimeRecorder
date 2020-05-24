using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Tracking
{
    public interface IWorkingTimeRangeRepository : IRepository
    {
        WorkingTimeRange Add(WorkingTimeRange workingTimeRange);

        void Edit(WorkingTimeRange workingTimeRange);

        void Remove(Identity<WorkingTimeRange> identity);

        WorkingTimeRange[] SelectByYmd(string ymd);

        WorkingTimeRange SelectById(Identity<WorkingTimeRange> identity);

        WorkingTimeRange[] SelectByTaskId(Identity<WorkTask> taskId);

    }
}
