using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.UseCase.Tracking.Reports;
using TimeRecorder.Domain.Utility.SystemClocks;

namespace TimeRecorder.Domain.Domain.Tracking
{
    public static class WorkTimeRuleDomainService
    {
        // 7時を基準に日付変更
        /// <summary>
        /// 工数上の日付変更基準時間
        /// </summary>
        public const int SwitchHour = 7;

        public static bool IsMatchDate(DateTime time, DateTime targetDate)
        {
            return GetWorkingDate(time) == targetDate.Date;
        }

        public static DateTime GetWorkingDate(DateTime time)
        {
            var targetDate = time.Date;

            if(time.Hour < 7)
            {
                targetDate = targetDate.AddDays(-1);
            }

            return targetDate;
        }
    }
}
