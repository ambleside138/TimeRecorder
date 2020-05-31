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


        public ReactiveProperty<WorkingTimeForTimelineDto> DoingTask { get; } = new ReactiveProperty<WorkingTimeForTimelineDto>();

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

        public void LoadWorkingTime()
        {
            var list = _GetWorkingTimeForTimelineUseCase.SelectByYmd(TargetYmd.Value);

            WorkingTimes.Clear();
            WorkingTimes.AddRange(list);

            DoingTask.Value = WorkingTimes.FirstOrDefault(w => w.EndDateTime.HasValue == false);
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
