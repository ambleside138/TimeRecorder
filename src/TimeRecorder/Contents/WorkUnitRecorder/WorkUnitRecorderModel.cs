using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Joins;
using System.Text;
using Livet;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.UseCase.Tasks;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Helpers;

namespace TimeRecorder.Contents.WorkUnitRecorder
{
    public class WorkUnitRecorderModel : NotificationObject
    {
        public ObservableCollection<WorkTask> PlanedTaskModels { get; } = new ObservableCollection<WorkTask>();

        private readonly WorkTaskApplicationService _WorkTaskApplicationService;

        public WorkUnitRecorderModel()
        {
            _WorkTaskApplicationService = new WorkTaskApplicationService(ContainerHelper.Resolver.Resolve<IWorkTaskRepository>());
        }

        public void Load()
        {
            var list = _WorkTaskApplicationService.GetPlanedTasks();

            PlanedTaskModels.Clear();
            PlanedTaskModels.AddRange(list);
        }

        public void AddWorkTask(WorkTask workTask)
        {
            var registed =_WorkTaskApplicationService.Add(workTask);

            PlanedTaskModels.Add(registed);
        }

        public void EditWorkTask(WorkTask workTask)
        {
            _WorkTaskApplicationService.Edit(workTask);

            Load();
        }

        public WorkTask SelectWorkTask(Identity<WorkTask> identity)
        {
            return _WorkTaskApplicationService.SelectById(identity);
        }
    }
}
