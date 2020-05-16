using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Tasks
{
    // RepositoryはList<T>のような使い心地・実装を目指す

    public interface IWorkTaskRepository : IRepository
    {
        WorkTask Add(WorkTask task);

        void Edit(WorkTask task);

        void Delete(Identity<WorkTask> identity);

        WorkTask SelectById(Identity<WorkTask> identity);

        WorkTask[] SelectToDo();
    }
}
