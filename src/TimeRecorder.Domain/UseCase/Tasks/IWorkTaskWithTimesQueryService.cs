using System;
using System.Collections.Generic;
using System.Text;
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
        WorkTaskWithTimesDto[] SelectByYmd(YmdString ymd);
    }

    public class WorkTaskWithTimesDto
    {
        public Identity<WorkTask> TaskId { get; set; }

        public string Title { get; set; }

        public TaskCategory TaskCategory { get; set; }

        public Product Product { get; set; }

        public string ClientName { get; set; }

        public string ProcessName { get; set; }

        public string Remarks { get; set; }

        public WorkingTimeRange[] WorkingTimes { get; set; }
    }
}
