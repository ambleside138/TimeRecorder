using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain;
using TimeRecorder.Domain.Domain.Calendar;
using TimeRecorder.Domain.Domain.Segments;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.UseCase.Tasks;

/// <summary>
/// カレンダーから予定を取り込み、タスクに登録します
/// </summary>
public class ImportTaskFromCalendarUseCase
{
    private static readonly NLog.Logger _Logger = NLog.LogManager.GetCurrentClassLogger();

    private readonly IWorkTaskRepository _WorkTaskRepository;
    private readonly IScheduledEventRepository _ScheduledEventRepository;
    private readonly IWorkingTimeRangeRepository _WorkingTimeRangeRepository;
    private readonly ISegmentRepository _SegmentRepository;
    private readonly WorkTaskBuilderConfig _WorkTaskBuilderConfig;
    private readonly ScheduleTitleMap[] _ScheduleTitleMaps;

    public ImportTaskFromCalendarUseCase(
        IWorkTaskRepository workTaskRepository,
        IScheduledEventRepository scheduledEventRepository,
        IWorkingTimeRangeRepository workingTimeRangeRepository,
        WorkTaskBuilderConfig workTaskBuilderConfig,
        ScheduleTitleMap[] scheduleTitleMaps,
        ISegmentRepository segmentRepository)
    {
        _WorkTaskRepository = workTaskRepository;
        _ScheduledEventRepository = scheduledEventRepository;
        _WorkingTimeRangeRepository = workingTimeRangeRepository;
        _WorkTaskBuilderConfig = workTaskBuilderConfig;
        _ScheduleTitleMaps = scheduleTitleMaps;
        _SegmentRepository = segmentRepository;
    }

    /// <summary>
    /// 指定した日付の予定をタスクとして取り込みます
    /// </summary>
    /// <param name="ymdString">取り込み対象日付</param>
    /// <returns></returns>
    public async Task<WorkTask[]> ImportToTaskAsync(YmdString ymdString)
    {
        try
        {
            _Logger.Info($"[ScheduleImporter] ▼スケジュール取り込み開始　target=[{ymdString}]");

            // イベントの取得
            var fromDateTime = ymdString.ToDateTime().Value;
            var toDateTime = fromDateTime.AddDays(1).AddMinutes(-1);
            var targetKinds = _WorkTaskBuilderConfig.EventMappers.Select(e => e.EventKind).ToArray();
            var events = await _ScheduledEventRepository.FetchScheduledEventsAsync(targetKinds, fromDateTime, toDateTime);
            if (events == null)
            {
                _Logger.Error("[ScheduleImporter] unknown error");
                return Array.Empty<WorkTask>();
            }

            // 未登録のイベントを取り込み
            var registedWorkTasks = _WorkTaskRepository.SelectByImportKeys(events.Select(e => e.Id).ToArray());

            var list = new List<WorkTask>();
            var builder = new WorkTaskBuilder(_WorkTaskBuilderConfig, _ScheduleTitleMaps, _SegmentRepository);
            foreach (var @event in events)
            {
                var target = registedWorkTasks.FirstOrDefault(t => t.ImportKey == @event.Id);

                if (target != null)
                {
                    // 登録済みなら時間とタイトルを更新
                    var workTask = _WorkTaskRepository.SelectById(new Identity<WorkTask>(target.WorkTaskId));
                    if(workTask != null)
                    {
                        workTask.Title = @event.Title;
                        _WorkTaskRepository.Edit(workTask);

                        var workingTime = _WorkingTimeRangeRepository.SelectByTaskId(workTask.Id).FirstOrDefault();

                        if(workingTime != null)
                        {
                            if(workingTime.TimePeriod.StartDateTime == @event.StartTime
                                && workingTime.TimePeriod.EndDateTime == @event.EndTime)
                            {
                                continue;
                            }

                            workingTime.TimePeriod = new TimePeriod(@event.StartTime, @event.EndTime);
                            _WorkingTimeRangeRepository.Edit(workingTime);
                        }
                        else
                        {
                            var newWorkingTime = WorkingTimeRange.FromScheduledEvent(workTask.Id, @event);
                            _WorkingTimeRangeRepository.Add(newWorkingTime);
                        }

                        list.Add(workTask);
                    }
                }
                else
                {
                    // 未登録ならスケジュールに合わせて登録
                    (WorkTask workTask, ImportedTask importedTask) = builder.Build(@event);
                    workTask = _WorkTaskRepository.AddForSchedule(workTask, importedTask);
                    list.Add(workTask);

                    var newWorkingTime = WorkingTimeRange.FromScheduledEvent(workTask.Id, @event);
                    _WorkingTimeRangeRepository.Add(newWorkingTime);
                }    
            }

            return list.ToArray();
        }
        finally
        {
            _Logger.Info($"[ScheduleImporter] ▲スケジュール取り込み終了　target=[{ymdString}]");
        }

    }
}
