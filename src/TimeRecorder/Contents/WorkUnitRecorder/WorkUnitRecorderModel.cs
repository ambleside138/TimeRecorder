using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Joins;
using System.Text;
using Livet;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.UseCase.Tasks;
using TimeRecorder.Domain.UseCase.Tracking;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Helpers;

namespace TimeRecorder.Contents.WorkUnitRecorder
{
    public class WorkUnitRecorderModel : NotificationObject
    {

        #region TargetYmd変更通知プロパティ
        private string _TargetYmd = DateTime.Today.ToString("yyyyMMdd");

        public string TargetYmd
        {
            get => _TargetYmd;
            set => RaisePropertyChangedIfSet(ref _TargetYmd, value);
        }
        #endregion

        public ObservableCollection<WorkTask> PlanedTaskModels { get; } = new ObservableCollection<WorkTask>();


        #region UseCases
        private readonly WorkTaskUseCase _WorkTaskUseCase;
        private readonly WorkingTimeRangeUseCase _WorkingTimeRangeUseCase;
        private readonly GetDailyWorkRecordUseCase _GetDailyWorkRecordUseCase; 
        #endregion

        public WorkUnitRecorderModel()
        {
            _WorkTaskUseCase = new WorkTaskUseCase(ContainerHelper.Resolver.Resolve<IWorkTaskRepository>());
            _WorkingTimeRangeUseCase = new WorkingTimeRangeUseCase(ContainerHelper.Resolver.Resolve<IWorkingTimeRangeRepository>());
            _GetDailyWorkRecordUseCase = new GetDailyWorkRecordUseCase(ContainerHelper.Resolver.Resolve<IDailyWorkRecordQueryService>());
        }

        public void Load()
        {
            var list = _WorkTaskUseCase.GetPlanedTasks();

            PlanedTaskModels.Clear();
            PlanedTaskModels.AddRange(list);

            var workingtimeheader = _GetDailyWorkRecordUseCase.Select(TargetYmd);
        }

        public void AddWorkTask(WorkTask workTask)
        {
            var registed =_WorkTaskUseCase.Add(workTask);

            PlanedTaskModels.Add(registed);
        }

        public void EditWorkTask(WorkTask workTask)
        {
            _WorkTaskUseCase.Edit(workTask);

            Load();
        }

        public WorkTask SelectWorkTask(Identity<WorkTask> identity)
        {
            return _WorkTaskUseCase.SelectById(identity);
        }
    }
}
