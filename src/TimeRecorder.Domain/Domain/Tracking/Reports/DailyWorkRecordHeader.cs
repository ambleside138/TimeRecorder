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

        public DateTime StartTime => _DailyWorkTaskUnits.SelectMany(u => u.WorkingTimeRanges).Min(t => t.TimePeriod.StartDateTime);

        public DateTime EndTime => _DailyWorkTaskUnits.SelectMany(u => u.WorkingTimeRanges).Where(t => t.TimePeriod.IsStopped).Max(t => t.TimePeriod.EndDateTime.Value);

        public TimeSpan CalcExpectedTotalWorkTimeSpan()
        {
            var workingTime = EndTime - StartTime;

            // 労働基準法第34条で、
            // 労働時間が6時間を超え、8時間以下の場合は少なくとも45分
            // 8時間を超える場合は、少なくとも1時間の休憩を与えなければならない、と定めています

            if (workingTime.Hours > 8)
            {
                return workingTime - TimeSpan.FromHours(1); // １時間の休憩
            }
            else if(workingTime.Hours > 6)
            {
                return workingTime - TimeSpan.FromMinutes(45); // １時間の休憩
            }
            else
            {
                return workingTime;
            }
        }

        public void AddWorkTask(WorkingTimeRecordForReport workingTime)
        {
            var targetTask = _DailyWorkTaskUnits.FirstOrDefault(t => t.NeedToSummarize(workingTime));
            if(targetTask == null)
            {
                targetTask = new DailyWorkTaskUnit(workingTime);
                _DailyWorkTaskUnits.Add(targetTask);
            }

            targetTask.AddWorkingTime(workingTime.ConvertToWorkingTimeRange());
        }
    }
}
