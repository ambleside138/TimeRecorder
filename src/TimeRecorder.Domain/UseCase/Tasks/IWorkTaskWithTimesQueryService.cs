using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tasks.Definitions;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.UseCase.Tasks
{
    /// <summary>
    /// 作業時間を伴って作業内容を取得するメソッドを提供します
    /// </summary>
    public interface IWorkTaskWithTimesQueryService : IQueryService
    {
        WorkTaskWithTimesDto[] SelectByYmd(YmdString ymd, bool containsCompleted);
    }

    public class WorkTaskWithTimesDto : NotificationDomainModel
    {
        public Identity<WorkTask> TaskId { get; set; }

        public string Title { get; set; }

        public TaskCategory TaskCategory { get; set; }

        public string ProductName { get; set; }

        public string ClientName { get; set; }

        public string ProcessName { get; set; }

        public string Remarks { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsScheduled { get; set; }

        #region WorkingTimes変更通知プロパティ
        private WorkingTimeRange[] _WorkingTimes;

        public WorkingTimeRange[] WorkingTimes
        {
            get => _WorkingTimes;
            set => RaisePropertyChangedIfSet(ref _WorkingTimes, value);
        }
        #endregion

    }
}
