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

namespace TimeRecorder.Domain.UseCase.Tasks
{
    public class WorkTaskUseCase
    {
        private readonly IWorkTaskRepository _TaskRepository;
        private readonly IWorkingTimeRangeRepository _WorkingTimeRangeRepository;

        public WorkTaskUseCase(IWorkTaskRepository taskRepository, IWorkingTimeRangeRepository workingTimeRangeRepository)
        {
            _TaskRepository = taskRepository;
            _WorkingTimeRangeRepository = workingTimeRangeRepository;
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
        }

        public void Complete(Identity<WorkTask> id)
        {
            var target = _TaskRepository.SelectById(id);

            if (target == null)
            {
                throw new NotFoundException("完了対象がみつかりませんでした");
            }

            var spec = new WorkTaskCompletionSpecification(_WorkingTimeRangeRepository);

            var result = spec.IsSatisfiedBy(target);
            if(result != ValidationResult.Success)
            {
                throw new SpecificationCheckException(result);
            }

            spec.EditActualTimes(target);

            _TaskRepository.Edit(target);
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

        //public WorkTask[] GetPlanedTasks()
        //{
        //    try
        //    {
        //        var list = _TaskRepository.SelectToDo();

        //        //var listClient = _ClientRepository.SelectAll();
        //        //var listProcess = _ProcessRepository.SelectAll();


        //        return list;
        //    }
        //    catch (Exception)
        //    {
        //        return new WorkTask[0];
        //    }

        //}
    }
}
