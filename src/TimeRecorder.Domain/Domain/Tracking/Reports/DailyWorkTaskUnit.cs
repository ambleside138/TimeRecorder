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

        public bool IsTemporary { get; set; }

        private readonly List<WorkingTimeRange> _WorkingTimeRanges = new List<WorkingTimeRange>();

        public IReadOnlyList<WorkingTimeRange> WorkingTimeRanges => _WorkingTimeRanges;

        /// <summary>
        /// 総工数[分]
        /// </summary>
        public int TotalWorkMinutes => WorkingTimeRanges.Sum(w => w.WorkSpanMinutes);


        public DailyWorkTaskUnit(WorkingTimeRecordForReport workingTime)
        {
            TaskId = workingTime.WorkTaskId;
            TaskCategory = workingTime.TaskCategory;
            Product = workingTime.Product;
            Client = workingTime.Client;
            WorkProcess = workingTime.WorkProcess;
            Title = workingTime.Title;
            IsTemporary = workingTime.IsTemporary;
        }

        public void AddWorkingTime(WorkingTimeRange timeRange)
        {
            _WorkingTimeRanges.Add(timeRange);
        }

        public bool NeedToSummarize(WorkingTimeRecordForReport dailyWorkTaskUnit)
        {
            if (dailyWorkTaskUnit.WorkTaskId == TaskId)
                return true;
                    
            if(IsTemporary && dailyWorkTaskUnit.IsTemporary)
            {
                if(TaskCategory == dailyWorkTaskUnit.TaskCategory
                    && Product.Id == dailyWorkTaskUnit.Product.Id
                    && Client.Id == dailyWorkTaskUnit.Client.Id
                    && WorkProcess.Id == dailyWorkTaskUnit.WorkProcess.Id
                    && Title == dailyWorkTaskUnit.Title  )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
