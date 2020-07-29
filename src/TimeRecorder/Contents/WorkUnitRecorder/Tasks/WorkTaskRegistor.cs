using Reactive.Bindings.Notifiers;
using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.UseCase.Tasks;
using TimeRecorder.Domain.UseCase.Tracking;
using TimeRecorder.Domain;

namespace TimeRecorder.Contents.WorkUnitRecorder.Tasks
{
    /// <summary>
    /// WorkTaskの新規登録に関するメソッドを提供します
    /// </summary>
    class WorkTaskRegistor
    {
        private readonly WorkTaskUseCase _WorkTaskUseCase;
        private readonly WorkingTimeRangeUseCase _WorkingTimeRangeUseCase;

        public WorkTaskRegistor()
        {
            _WorkTaskUseCase = new WorkTaskUseCase(
                                        ContainerHelper.Resolver.Resolve<IWorkTaskRepository>(),
                                        ContainerHelper.Resolver.Resolve<IWorkingTimeRangeRepository>());
            _WorkingTimeRangeUseCase = new WorkingTimeRangeUseCase(
                                            ContainerHelper.Resolver.Resolve<IWorkingTimeRangeRepository>(),
                                            ContainerHelper.Resolver.Resolve<IWorkTaskRepository>());

        }

        public void AddWorkTask(WorkTask workTask, bool needStart)
        {
            var task = _WorkTaskUseCase.Add(workTask);
            if (needStart)
            {
                _WorkingTimeRangeUseCase.StartWorking(task.Id);
            }

            MessageBroker.Default.Publish(new WorkTaskRegistedEventArg { WorkTaskId = task.Id });
        }
    }

    class WorkTaskRegistedEventArg
    {
        public Identity<WorkTask> WorkTaskId { get; set; }
    }
}
