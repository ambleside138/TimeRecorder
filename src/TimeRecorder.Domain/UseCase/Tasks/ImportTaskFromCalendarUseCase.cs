using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Calendar;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.UseCase.Tasks
{
    /// <summary>
    /// カレンダーから予定を取り込み、タスクに登録します
    /// </summary>
    public class ImportTaskFromCalendarUseCase
    {
        private readonly IWorkTaskRepository _WorkTaskRepository;
        private readonly IScheduledEventRepository _ScheduledEventRepository;
        private readonly IWorkingTimeRangeRepository _WorkingTimeRangeRepository;
        private readonly WorkTaskBuilderConfig _WorkTaskBuilderConfig;
        private readonly ScheduleTitleMap[] _ScheduleTitleMaps;

        public ImportTaskFromCalendarUseCase(
            IWorkTaskRepository workTaskRepository, 
            IScheduledEventRepository scheduledEventRepository,
            IWorkingTimeRangeRepository workingTimeRangeRepository,
            WorkTaskBuilderConfig workTaskBuilderConfig,
            ScheduleTitleMap[] scheduleTitleMaps)
        {
            _WorkTaskRepository = workTaskRepository;
            _ScheduledEventRepository = scheduledEventRepository;
            _WorkingTimeRangeRepository = workingTimeRangeRepository;
            _WorkTaskBuilderConfig = workTaskBuilderConfig;
            _ScheduleTitleMaps = scheduleTitleMaps;
        }

        public async Task<WorkTask[]> ImportToTaskAsync(YmdString ymdString)
        {
            // イベントの取得
            var fromDateTime = ymdString.ToDateTime().Value;
            var toDateTime = fromDateTime.AddDays(1).AddMinutes(-1);
            var targetKinds = _WorkTaskBuilderConfig.EventMappers.Select(e => e.EventKind).ToArray();
            var events = await _ScheduledEventRepository.FetchScheduledEventsAsync(targetKinds, fromDateTime, toDateTime);
            if (events == null)
                return new WorkTask[0];

            // 未登録のイベントを取り込み
            var registedWorkTasks = _WorkTaskRepository.SelectByImportKeys(events.Select(e => e.Id).ToArray());

            var list = new List<WorkTask>();
            var builder = new WorkTaskBuilder(_WorkTaskBuilderConfig, _ScheduleTitleMaps);
            foreach(var @event in events)
            {
                // 登録済みは無視する
                if (registedWorkTasks.Any(t => t.ImportSource.Key == @event.Id))
                    continue;

                // 未登録ならスケジュールに合わせて登録
                var workTask = builder.Build(@event);
                workTask = _WorkTaskRepository.Add(workTask);
                list.Add(workTask);

                var newWorkingTime = WorkingTimeRange.FromScheduledEvent(workTask.Id, @event);
                _WorkingTimeRangeRepository.Add(newWorkingTime);
            }

            return list.ToArray();
        }
    }
}
