using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.UseCase.Tracking
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


    }
}
