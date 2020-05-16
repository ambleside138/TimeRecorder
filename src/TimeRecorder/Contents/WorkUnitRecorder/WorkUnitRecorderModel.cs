using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Joins;
using System.Text;
using Livet;
using Reactive.Bindings;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.UseCase.Tasks;
using TimeRecorder.Domain.UseCase.Tracking;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Domain.Utility.SystemClocks;
using TimeRecorder.Helpers;

namespace TimeRecorder.Contents.WorkUnitRecorder
{
    public class WorkUnitRecorderModel : NotificationObject, IDisposable
    {

        #region TargetYmd変更通知プロパティ
        private string _TargetYmd = DateTime.Today.ToString("yyyyMMdd");

        public string TargetYmd
        {
            get => _TargetYmd;
            set => RaisePropertyChangedIfSet(ref _TargetYmd, value);
        }
        #endregion

        public ObservableCollection<WorkTaskWithTimesDto> PlanedTaskModels { get; } = new ObservableCollection<WorkTaskWithTimesDto>();

        public ObservableCollection<WorkingTimeForTimelineDto> WorkingTimes { get; } = new ObservableCollection<WorkingTimeForTimelineDto>();


        public ReactiveProperty<WorkingTimeForTimelineDto> DoingTask { get; } = new ReactiveProperty<WorkingTimeForTimelineDto>();


        #region UseCases
        private readonly WorkTaskUseCase _WorkTaskUseCase;
        private readonly GetWorkTaskWithTimesUseCase _GetWorkTaskWithTimesUseCase;
        private readonly WorkingTimeRangeUseCase _WorkingTimeRangeUseCase;
        private readonly GetWorkingTimeForTimelineUseCase _GetWorkingTimeForTimelineUseCase;
        #endregion

        public WorkUnitRecorderModel()
        {
            _WorkTaskUseCase = new WorkTaskUseCase(ContainerHelper.Resolver.Resolve<IWorkTaskRepository>());
            _WorkingTimeRangeUseCase = new WorkingTimeRangeUseCase(
                                            ContainerHelper.Resolver.Resolve<IWorkingTimeRangeRepository>(),
                                            ContainerHelper.Resolver.Resolve<IWorkTaskRepository>());
            _GetWorkingTimeForTimelineUseCase = new GetWorkingTimeForTimelineUseCase(ContainerHelper.Resolver.Resolve<IWorkingTimeQueryService>());
            _GetWorkTaskWithTimesUseCase = new GetWorkTaskWithTimesUseCase(ContainerHelper.Resolver.Resolve<IWorkTaskWithTimesQueryService>());

            ObjectChangedNotificator.Instance.WorkTaskEdited += Load;
        }

        public void Load()
        {
            var list = _GetWorkTaskWithTimesUseCase.GetByYmd(new YmdString(TargetYmd));

            PlanedTaskModels.Clear();
            PlanedTaskModels.AddRange(list);

            LoadWorkingTime();
        }

        public void LoadWorkingTime()
        {
            var list = _GetWorkingTimeForTimelineUseCase.SelectByYmd(TargetYmd);

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
