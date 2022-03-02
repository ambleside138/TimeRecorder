using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Repository.SQLite.Tasks.Dao;

namespace TimeRecorder.Repository.SQLite.Tasks;

static class WorkTaskFactory
{
    public static WorkTask Create(WorkTaskTableRow workTaskTableRow, bool isCompleted)
    {
        if (workTaskTableRow == null)
            return null;

        return new WorkTask(
                 new Identity<WorkTask>(workTaskTableRow.Id)
                 , workTaskTableRow.Title
                 , workTaskTableRow.TaskCategory
                 , new Identity<Domain.Domain.Products.Product>(workTaskTableRow.ProductId)
                 , new Identity<Domain.Domain.Clients.Client>(workTaskTableRow.ClientId)
                 , new Identity<Domain.Domain.WorkProcesses.WorkProcess>(workTaskTableRow.ProcessId)
                 , workTaskTableRow.TaskSource
                 , isCompleted);

    }
}
