using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks.Specifications;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Domain.Utility.Exceptions;

namespace TimeRecorder.Domain.Domain.Tasks.Commands
{
    /// <summary>
    /// WorkTaskに完了処理を行うためのメソッドを提供します
    /// </summary>
    class WorkTaskCompletionCommand
    {
        private readonly IWorkingTimeRangeRepository _WorkingTimeRangeRepository;
        private readonly IWorkTaskRepository _WorkTaskRepository;

        public WorkTaskCompletionCommand(IWorkTaskRepository workTaskRepository, IWorkingTimeRangeRepository workingTimeRangeRepository)
        {
            _WorkTaskRepository = workTaskRepository;
            _WorkingTimeRangeRepository = workingTimeRangeRepository;
        }

        public void CompleteWorkTask(Identity<WorkTask> id)
        {
            var target = _WorkTaskRepository.SelectById(id);

            if (target == null)
            {
                throw new NotFoundException("完了対象がみつかりませんでした");
            }

            var spec = new WorkTaskCompletionSpecification(_WorkingTimeRangeRepository);

            var result = spec.IsSatisfiedBy(target);
            if (result != ValidationResult.Success)
            {
                throw new SpecificationCheckException(result);
            }

            target.Complete();

            _WorkTaskRepository.Edit(target);
        }

        public void ReStartTask(Identity<WorkTask> id)
        {
            var target = _WorkTaskRepository.SelectById(id);

            if (target == null)
            {
                throw new NotFoundException("再開対象がみつかりませんでした");
            }

            target.ReStart();

            _WorkTaskRepository.Edit(target);
        }
    }
}
