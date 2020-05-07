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

        public Product Product { get; set; }

        public int HospitalId { get; set; }

        public int ProcessId { get; set; }

        public string Remarks { get; set; }

        public DateTime? PlanedStartDateTime { get; set; }

        public DateTime? PlanedEndDateTime { get; set; }

        public DateTime? ActualStartDateTime { get; set; }

        public DateTime? ActualEndDateTime { get; set; }

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
                , Product
                , new Identity<Domain.Domain.Hospitals.Hospital>(HospitalId)
                , new Identity<Domain.Domain.Processes.Process>(ProcessId)
                , Remarks
                , taskProgress);
        }

        public static WorkTaskTableRow FromDomainObject(WorkTask workTask)
        {
            return new WorkTaskTableRow
            {
                Id = workTask.Id.Value,
                Title = workTask.Title,
                TaskCategory = workTask.TaskCategory,
                Product = workTask.Product,
                HospitalId = workTask.HospitalId.Value,
                ProcessId = workTask.ProcessId.Value,
                Remarks = workTask.Remarks,
                PlanedStartDateTime = workTask.TaskProgress.PlanedPeriod.Start,
                PlanedEndDateTime = workTask.TaskProgress.PlanedPeriod.End,
                ActualStartDateTime = workTask.TaskProgress.PlanedPeriod.Start,
                ActualEndDateTime = workTask.TaskProgress.PlanedPeriod.End,
            };
        }
    }
}
