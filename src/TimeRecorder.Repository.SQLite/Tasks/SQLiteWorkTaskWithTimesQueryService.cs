using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.UseCase.Tasks;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Repository.SQLite.Tasks.Dao;
using TimeRecorder.Repository.SQLite.Tracking.Dao;

namespace TimeRecorder.Repository.SQLite.Tasks
{
    public class SQLiteWorkTaskWithTimesQueryService : IWorkTaskWithTimesQueryService
    {
        public WorkTaskWithTimesDto[] SelectByYmd(YmdString ymd)
        {
            var list = new List<WorkTaskWithTimesDto>();

            RepositoryAction.Query(c =>
            {
                var workTaskDao = new WorkTaskDao(c, null);
                var workingTimeDao = new WorkingTimeDao(c, null);

                var tasks = workTaskDao.SelectPlaned(ymd);
                var times = workingTimeDao.SelectYmd(ymd.Value);

                foreach(var task in tasks)
                {
                    var dto = new WorkTaskWithTimesDto
                    {
                        TaskId = new Identity<Domain.Domain.Tasks.WorkTask>(task.Id),
                        ClientName = "",
                        ProcessName = "",
                        Product = task.Product,
                        Remarks = task.Remarks,
                        TaskCategory = task.TaskCategory,
                        Title = task.Title,
                    };

                    dto.WorkingTimes = times.Where(t => t.TaskId == task.Id)
                                            .Select(t => t.ToDomainObject())
                                            .ToArray();

                    list.Add(dto);
                }
            });

            return list.OrderBy(i => i.TaskId.Value).ToArray();
        }
    }
}
