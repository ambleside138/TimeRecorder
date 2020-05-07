using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Tasks
{
    public interface IWorkTaskRepository
    {
        WorkTask Add(WorkTask task);

        void Edit(WorkTask task);

        void Delete(Identity<WorkTask> identity);

        WorkTask SelectById(Identity<WorkTask> identity);

        WorkTask[] SelectToDo();
    }
}
