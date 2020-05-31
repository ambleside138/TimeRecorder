using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tasks.Definitions;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Repository.SQLite.Tasks.Dao
{
    class WorkTaskTableRow
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public TaskCategory TaskCategory { get; set; }

        public int ProductId { get; set; }

        public int ClientId { get; set; }

        public int ProcessId { get; set; }

        public string Remarks { get; set; }

        public DateTime? PlanedStartDateTime { get; set; }

        public DateTime? PlanedEndDateTime { get; set; }

        public DateTime? ActualStartDateTime { get; set; }

        public DateTime? ActualEndDateTime { get; set; }

        public string Source { get; set; }

        public string ImportKey { get; set; }

        public WorkTask ConvertToDomainObject()
        {
            var taskProgress = new TaskProgress
             (
                 new Domain.Domain.DateTimePeriod { Start = PlanedStartDateTime, End = PlanedEndDateTime },
                 new Domain.Domain.DateTimePeriod { Start = ActualStartDateTime, End = ActualEndDateTime, }
             );

            return new WorkTask(
                new Identity<WorkTask>(Id)
                , Title
                , TaskCategory
                , new Identity<Domain.Domain.Products.Product>(ProductId)
                , new Identity<Domain.Domain.Clients.Client>(ClientId)
                , new Identity<Domain.Domain.WorkProcesses.WorkProcess>(ProcessId)
                , Remarks
                , taskProgress
                , new WorkTaskImportSource(ImportKey, Source));
        }

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
                Remarks = workTask.Remarks,
                PlanedStartDateTime = workTask.TaskProgress.PlanedPeriod.Start,
                PlanedEndDateTime = workTask.TaskProgress.PlanedPeriod.End,
                ActualStartDateTime = workTask.TaskProgress.ActualPeriod.Start,
                ActualEndDateTime = workTask.TaskProgress.ActualPeriod.End,
                Source = workTask.ImportSource.Kind,
                ImportKey = workTask.ImportSource.Key,
            };
        }
    }
}
