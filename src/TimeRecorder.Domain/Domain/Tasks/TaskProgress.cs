using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Domain.Tasks
{
    /// <summary>
    /// タスクの進捗
    /// </summary>
    public struct TaskProgress
    {
        /// <summary>
        /// 予定期間
        /// </summary>
        public DateTimePeriod PlanedPeriod { get; internal set; }

        /// <summary>
        /// 実績期間
        /// </summary>
        public DateTimePeriod ActualPeriod { get; internal set; }

        public TaskProgress(DateTimePeriod planedPeriod, DateTimePeriod actualPeriod)
        {
            PlanedPeriod = planedPeriod;
            ActualPeriod = actualPeriod;
        }
    }
}
