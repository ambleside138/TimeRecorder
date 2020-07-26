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

        // WorkTask以外の引数を受けるのはセオリーから外れる気がするが...
        // 正解がわからないのでこれで。
        WorkTask AddForSchedule(WorkTask task, ImportedTask workTaskImportSource);

        void Edit(WorkTask task);

        void Delete(Identity<WorkTask> identity);

        WorkTask SelectById(Identity<WorkTask> identity);

        ImportedTask[] SelectByImportKeys(string[] keys);
    }
}
