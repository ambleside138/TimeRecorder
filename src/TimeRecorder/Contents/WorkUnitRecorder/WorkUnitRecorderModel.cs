using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Joins;
using System.Text;
using System.Threading.Tasks;
using Livet;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using TimeRecorder.Domain.Domain.Calendar;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.UseCase.Tasks;
using TimeRecorder.Domain.UseCase.Tracking;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Domain.Utility.SystemClocks;
using TimeRecorder.Helpers;
using TimeRecorder.Host;

namespace TimeRecorder.Contents.WorkUnitRecorder
{
    public class WorkUnitRecorderModel : NotificationObject, IDisposable
    {
        public ReactivePropertySlim<DateTime> TargetDate { get; }

        public YmdString TargetYmd => new YmdString(TargetDate.Value.ToString("yyyyMMdd"));

        public ObservableCollection<WorkTaskWithTimesDto> PlanedTaskModels { get; } = new ObservableCollection<WorkTaskWithTimesDto>();

        public ObservableCollection<WorkingTimeForTimelineDto> WorkingTimes { get; } = new ObservableCollection<WorkingTimeForTimelineDto>();


        public ReactiveProperty<WorkingTimeForTimelineDto> DoingTask { get; } = new ReactiveProperty<WorkingTimeForTimelineDto>(null, ReactivePropertyMode.Default, new WorkingTimeForTimelineDtoEqualityComparer());

        private LivetCompositeDisposable _Disposables = new LivetCompositeDisposable();


        #region UseCases
        private readonly WorkTaskUseCase _WorkTaskUseCase;
        private readonly GetWorkTaskWithTimesUseCase _GetWorkTaskWithTimesUseCase;
        private readonly WorkingTimeRangeUseCase _WorkingTimeRangeUseCase;
        private readonly GetWorkingTimeForTimelineUseCase _GetWorkingTimeForTimelineUseCase;
        private readonly ImportTaskFromCalendarUseCase _ImportTaskFromCalendarUseCase;
        #endregion

        public WorkUnitRecorderModel()
        {
            _WorkTaskUseCase = new WorkTaskUseCase(
                                        ContainerHelper.Resolver.Resolve<IWorkTaskRepository>(), 
                                        ContainerHelper.Resolver.Resolve<IWorkingTimeRangeRepository>());
            _WorkingTimeRangeUseCase = new WorkingTimeRangeUseCase(
                                            ContainerHelper.Resolver.Resolve<IWorkingTimeRangeRepository>(),
                                            ContainerHelper.Resolver.Resolve<IWorkTaskRepository>());
            _GetWorkingTimeForTimelineUseCase = new GetWorkingTimeForTimelineUseCase(ContainerHelper.Resolver.Resolve<IWorkingTimeQueryService>());
            _GetWorkTaskWithTimesUseCase = new GetWorkTaskWithTimesUseCase(ContainerHelper.Resolver.Resolve<IWorkTaskWithTimesQueryService>());

            var config = JsonFileIO.Deserialize<TimeRecorderConfiguration>("TimeRecorderConfiguration.json") ?? new TimeRecorderConfiguration();

            _ImportTaskFromCalendarUseCase = new ImportTaskFromCalendarUseCase(
                ContainerHelper.Resolver.Resolve<IWorkTaskRepository>(),
                ContainerHelper.Resolver.Resolve<IScheduledEventRepository>(),
                ContainerHelper.Resolver.Resolve<IWorkingTimeRangeRepository>(),
                config.WorkTaskBuilderConfig);

            ObjectChangedNotificator.Instance.WorkTaskEdited += Load;

            TargetDate = new ReactivePropertySlim<DateTime>(DateTime.Today);
            TargetDate.Subscribe(_ => Load()).AddTo(_Disposables);
        }

        public void Load()
        {
            var list = _GetWorkTaskWithTimesUseCase.GetByYmd(TargetYmd);

            PlanedTaskModels.Clear();
            PlanedTaskModels.AddRange(list);

            LoadWorkingTime();
        }

        private void LoadWorkingTime()
        {
            var list = _GetWorkingTimeForTimelineUseCase.SelectByYmd(TargetYmd.Value);

            WorkingTimes.Clear();
            WorkingTimes.AddRange(list);

           SetDoingTask();
        }

        public void UpdateDoingTask()
        {
            if (DoingTask == null)
            {
                // 登録済みの予定が存在する可能性がある
                SetDoingTask();
            }
            else
            {
                // 実行中の作業がある場合
                //  A.そのまま続行
                //  B.次のスケジュールが埋まっている or 予定終了時間を超えた場合は自動終了する
                // いずれかの処理が必要
                if (AutoStopCurrentTaskIfNeeded(out string message))
                {
                    NotificationService.Current.Info("作業タスク 更新のお知らせ", message);
                }
            }
        }


        public void SetDoingTask()
        {
            var target = WorkingTimes.Where(w => w.TimePeriod.WithinRangeAtCurrentTime)
                                     .FirstOrDefault();

            DoingTask.Value = target;
        }

        public bool AutoStopCurrentTaskIfNeeded(out string message)
        {
            message = "";

            if (DoingTask.Value == null)
                return false;

            // 終了予定時刻ありの場合
            if (DoingTask.Value.TimePeriod.EndDateTime.HasValue)
            {
                // 現在のタスクが終了時刻を迎えたら終了
                if (DoingTask.Value.TimePeriod.WithinRangeAtCurrentTime == false)
                {
                    SetDoingTask();
                    message = "終了予定時刻となったため 現在の作業を終了しました";
                    return true;
                }
            }
            // 終了予定時刻なしの場合
            else
            {
                // 予定タスクがあれば自動移行
                var otherPlanedTask = WorkingTimes.Where(t => t.WorkingTimeId != DoingTask.Value.WorkingTimeId)
                                                       .Where(w => w.TimePeriod.WithinRangeAtCurrentTime)
                                                       .FirstOrDefault();
                if (otherPlanedTask != null)
                {
                    // 現在タスクの終了
                    // ＃終了処理のなかで再読み込みも行われる
                    StopCurrentTask();
                    message = $"タスク [ {otherPlanedTask.TaskTitle} ] の開始予定時刻となったため現在の作業を終了しました";
                    return true;
                }
            }

            return false;

        }

        public void AddWorkTask(WorkTask workTask)
        {
            _WorkTaskUseCase.Add(workTask);

            Load();
        }

        public void StopCurrentTask()
        {
            if (DoingTask.Value == null)
                return;

            _WorkingTimeRangeUseCase.StopWorking(DoingTask.Value.WorkingTimeId);

            Load();
        }

        public async Task<WorkTask[]> ImportFromCalendarAsync()
        {
            var importedWorkTasks = await _ImportTaskFromCalendarUseCase.ImportToTaskAsync(TargetYmd);

            if(importedWorkTasks.Any())
            {
                Load();
            }

            return importedWorkTasks;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Mangedリソースの解放
                    ObjectChangedNotificator.Instance.WorkTaskEdited -= Load;
                    _Disposables.Dispose();
                    _Disposables = null;
                }

                // TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~WorkUnitRecorderModel()
        // {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
