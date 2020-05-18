using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.UseCase.Tracking.Reports;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Tracking.Reports
{
    /// <summary>
    /// 日々記録していく作業単位
    /// </summary>
    public class DailyWorkRecordHeader
    {
        /// <summary>
        /// 勤務日 [ymd]
        /// </summary>
        public string WorkYmd { get; set; }

        private readonly List<DailyWorkTaskUnit> _DailyWorkTaskUnits = new List<DailyWorkTaskUnit>();

        public IReadOnlyList<DailyWorkTaskUnit> DailyWorkTaskUnits => _DailyWorkTaskUnits;

        public int WorkMinutesForDay => _DailyWorkTaskUnits.Sum(u => u.TotalWorkMinutes);

        public void AddWorkTask(WorkingTimeRecordForReport workingTime)
        {
            var targetTask = _DailyWorkTaskUnits.FirstOrDefault(t => t.TaskId == workingTime.WorkTaskId);
            if(targetTask == null)
            {
                targetTask = new DailyWorkTaskUnit(workingTime);
                _DailyWorkTaskUnits.Add(targetTask);
            }

            targetTask.AddWorkingTime(workingTime.ConvertToWorkingTimeRange());
        }
    }
}
