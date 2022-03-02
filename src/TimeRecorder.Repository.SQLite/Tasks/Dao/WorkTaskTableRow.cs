using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain;

namespace TimeRecorder.Repository.SQLite.Tasks.Dao;

class WorkTaskTableRow
{
    public int Id { get; set; }

    public string Title { get; set; }

    public TaskCategory TaskCategory { get; set; }

    public int ProductId { get; set; }

    public int ClientId { get; set; }

    public int ProcessId { get; set; }

    public TaskSource TaskSource { get; set; }


    public static WorkTaskTableRow FromDomainObject(WorkTask workTask)
    {
        return new WorkTaskTableRow
        {
            Id = workTask.Id.Value,
            Title = workTask.Title,
            TaskCategory = workTask.TaskCategory,
            ProductId = workTask.ProductId.Value,
            ClientId = workTask.ClientId.Value,
            ProcessId = workTask.ProcessId.Value,
            TaskSource = workTask.TaskSource,
        };
    }
}
