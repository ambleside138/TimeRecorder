using System;
using System.Collections.Generic;
using System.Linq;
using TimeRecorder.Domain.UseCase.Tasks;
using TimeRecorder.Domain;
using TimeRecorder.Repository.SQLite.Clients.Dao;
using TimeRecorder.Repository.SQLite.Products.Dao;
using TimeRecorder.Repository.SQLite.Tasks.Dao;
using TimeRecorder.Repository.SQLite.Tracking.Dao;
using TimeRecorder.Repository.SQLite.WorkProcesses.Dao;
using TimeRecorder.Domain.Domain;

namespace TimeRecorder.Repository.SQLite.Tasks;

public class SQLiteWorkTaskWithTimesQueryService : IWorkTaskWithTimesQueryService
{

    public WorkTaskWithTimesDto[] SelectByYmd(YmdString ymd, bool containsCompleted)
    {
        var list = new List<WorkTaskWithTimesDto>();

        RepositoryAction.Query(c =>
        {
            var workTaskDao = new WorkTaskDao(c, null);
            var workingTimeDao = new WorkingTimeDao(c, null);
            var processes = new WorkProcessDao(c, null).SelectAll();
            var products = new ProductDao(c, null).SelectAll();
            var clients = new ClientDao(c, null).SelectAll();
            var completedDao = new WorkTaskCompletedDao(c, null);

            var tasks = workTaskDao.SelectPlaned(ymd, containsCompleted);
            var times = workingTimeDao.SelectByTaskIds(tasks.Select(t => t.Id).Distinct().ToArray());
            var completed = completedDao.SelectCompleted(tasks.Select(t => t.Id).Distinct().ToArray());

            foreach (var task in tasks)
            {
                var dto = new WorkTaskWithTimesDto
                {
                    TaskId = new Identity<Domain.Domain.Tasks.WorkTask>(task.Id),
                    ClientName = clients.FirstOrDefault(c => c.Id == task.ClientId)?.Name ?? "",
                    ProcessName = processes.FirstOrDefault(p => p.Id == task.ProcessId)?.Title ?? "",
                    ProductName = products.FirstOrDefault(p => p.Id == task.ProductId)?.Name ?? "",
                    TaskCategory = task.TaskCategory,
                    Title = task.Title,
                    IsCompleted = completed.Any(c => c == task.Id),
                    IsScheduled = task.TaskSource == Domain.Domain.Tasks.TaskSource.Schedule,
                };

                dto.WorkingTimes = times.Where(t => t.TaskId == task.Id)
                                        .Select(t => t.ToDomainObject())
                                        .OrderBy(t => t.TimePeriod.StartDateTime)
                                        .ThenBy(t => t.Id)
                                        .ToArray();

                list.Add(dto);
            }
        });

        try
        {
            return list.OrderBy(i => i.WorkingTimes.Any(t => t.IsDoing) ? 0 : 1)
                   .ThenByDescending(i => i.WorkingTimes.Where(t => t.TimePeriod.IsFuture == false).Any() ? i.WorkingTimes.Where(t => t.TimePeriod.IsFuture == false).Max(t => t.TimePeriod.StartDateTime) : DateTime.MinValue)
                   .ThenBy(i => i.WorkingTimes.Where(t => t.TimePeriod.IsFuture).Any() ? i.WorkingTimes.Where(t => t.TimePeriod.IsFuture).Min(t => t.TimePeriod.StartDateTime) : DateTime.MaxValue)
                   .ThenBy(i => i.TaskId.Value).ToArray();
        }
        catch (Exception)
        {
            return list.ToArray();
        }

    }
}
