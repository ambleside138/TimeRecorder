using MicroResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Domain.Domain.WorkProcesses;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Domain.Utility.Exceptions;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Domain.Tasks.Specifications;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using TimeRecorder.Domain.Domain.Tasks.Commands;

namespace TimeRecorder.Domain.UseCase.Tasks
{
    public class WorkTaskUseCase
    {
        private readonly IWorkTaskRepository _TaskRepository;
        private readonly IWorkingTimeRangeRepository _WorkingTimeRangeRepository;

        private readonly WorkTaskCompletionCommand _WorkTaskCompletionCommand;

        public WorkTaskUseCase(IWorkTaskRepository taskRepository, IWorkingTimeRangeRepository workingTimeRangeRepository)
        {
            _TaskRepository = taskRepository;
            _WorkingTimeRangeRepository = workingTimeRangeRepository;
            _WorkTaskCompletionCommand = new WorkTaskCompletionCommand(taskRepository, workingTimeRangeRepository);
        }

        public WorkTask Add(WorkTask workTask)
        {
            return _TaskRepository.Add(workTask);
        }

        public void Edit(WorkTask workTask)
        {
            _TaskRepository.Edit(workTask);
        }

        public void Delete(Identity<WorkTask> workTaskId)
        {
            _TaskRepository.Delete(workTaskId);
            _WorkingTimeRangeRepository.RemoveByTaskId(workTaskId);
        }

        public void Complete(Identity<WorkTask> id)
        {
            _WorkTaskCompletionCommand.CompleteWorkTask(id);
        }

        public void UnComplete(Identity<WorkTask> id)
        {
            _WorkTaskCompletionCommand.ReStartTask(id);
        }

        public WorkTask SelectById(Identity<WorkTask> workTaskId)
        {
            var target = _TaskRepository.SelectById(workTaskId);

            if(target == null)
            {
                throw new Exception("みつかりませんでした");
            }

            return target;
        }
    }
}
