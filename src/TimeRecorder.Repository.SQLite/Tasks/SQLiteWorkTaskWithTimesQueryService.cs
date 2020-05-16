using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.UseCase.Tasks;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Repository.SQLite.Tasks.Dao;
using TimeRecorder.Repository.SQLite.Tracking.Dao;
using TimeRecorder.Repository.SQLite.WorkProcesses;

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
                var processes = new SQLiteWorkProcessRepository().SelectAll();

                var tasks = workTaskDao.SelectPlaned(ymd);
                var times = workingTimeDao.SelectYmd(ymd.Value);

                foreach(var task in tasks)
                {
                    var dto = new WorkTaskWithTimesDto
                    {
                        TaskId = new Identity<Domain.Domain.Tasks.WorkTask>(task.Id),
                        ClientName = "",
                        ProcessName = processes.FirstOrDefault(p => p.Id.Value == task.ProcessId)?.Title ?? "",
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
