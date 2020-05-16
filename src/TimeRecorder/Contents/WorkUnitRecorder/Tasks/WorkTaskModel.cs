using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.UseCase.Tasks;
using TimeRecorder.Domain.UseCase.Tracking;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Contents.WorkUnitRecorder.Tasks
{
    public class WorkTaskModel
    {
        private readonly WorkTaskUseCase _WorkTaskUseCase;
        private readonly WorkingTimeRangeUseCase _WorkingTimeRangeUseCase;

        public WorkTaskModel()
        {
            _WorkTaskUseCase = new WorkTaskUseCase(ContainerHelper.Resolver.Resolve<IWorkTaskRepository>());

            _WorkingTimeRangeUseCase = new WorkingTimeRangeUseCase(
                                            ContainerHelper.Resolver.Resolve<IWorkingTimeRangeRepository>(),
                                            ContainerHelper.Resolver.Resolve<IWorkTaskRepository>());
        }

        public void EditWorkTask(WorkTask workTask)
        {
            _WorkTaskUseCase.Edit(workTask);
            ObjectChangedNotificator.Instance.NotifyWorkTaskEdited();
        }

        public void StartWorking(Identity<WorkTask> id)
        {
            _WorkingTimeRangeUseCase.StartWorking(id);
            ObjectChangedNotificator.Instance.NotifyWorkTaskEdited();
        }

        public WorkTask SelectWorkTask(Identity<WorkTask> identity)
        {
            return _WorkTaskUseCase.SelectById(identity);
        }
    }
}
