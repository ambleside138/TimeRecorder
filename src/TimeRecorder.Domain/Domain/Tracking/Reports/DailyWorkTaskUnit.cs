using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Domain.Domain.Products;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tasks.Definitions;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Domain.WorkProcesses;
using TimeRecorder.Domain.UseCase.Tracking.Reports;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Tracking.Reports
{
    public class DailyWorkTaskUnit
    {
        public Identity<WorkTask> TaskId { get; }
        public TaskCategory TaskCategory { get; }
        public Product Product { get; }
        public Client Client { get; }
        public WorkProcess WorkProcess { get; }
        public string Title { get; }

        private readonly List<WorkingTimeRange> _WorkingTimeRanges = new List<WorkingTimeRange>();

        public IReadOnlyList<WorkingTimeRange> WorkingTimeRanges => _WorkingTimeRanges;

        /// <summary>
        /// 総工数[分]
        /// </summary>
        public int TotalWorkMinutes => WorkingTimeRanges.Sum(w => w.WorkSpan);


        public DailyWorkTaskUnit(WorkingTimeRecordForReport workingTime)
        {
            TaskId = workingTime.WorkTaskId;
            TaskCategory = workingTime.TaskCategory;
            Product = workingTime.Product;
            Client = workingTime.Client;
            WorkProcess = workingTime.WorkProcess;
            Title = workingTime.Title;
        }

        public void AddWorkingTime(WorkingTimeRange timeRange)
        {
            _WorkingTimeRanges.Add(timeRange);
        }
    }
}
