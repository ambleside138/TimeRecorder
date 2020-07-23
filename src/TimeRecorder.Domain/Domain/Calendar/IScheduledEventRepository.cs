using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Calendar
{
    public interface IScheduledEventRepository : IRepository
    {
        /// <summary>
        /// 指定期間のスケジュールを取得します
        /// </summary>
        /// <param name="from">検索開始日時</param>
        /// <param name="to">検索終了日時</param>
        /// <returns></returns>
        public Task<ScheduledEvent[]> FetchScheduledEventsAsync(string[] targetKinds, DateTime from, DateTime to);
    }
}
