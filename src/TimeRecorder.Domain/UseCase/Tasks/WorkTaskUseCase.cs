using MicroResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Domain.Domain.Processes;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.UseCase.Tasks
{
    public class WorkTaskUseCase
    {
        private readonly IWorkTaskRepository _TaskRepository;
        //private IClientRepository _ClientRepository;
        //private IProcessRepository _ProcessRepository;

        public WorkTaskUseCase(IWorkTaskRepository taskRepository)
        {
            _TaskRepository = taskRepository;
        }

        public WorkTask Add(WorkTask workTask)
        {
            return _TaskRepository.Add(workTask);
        }

        public void Edit(WorkTask workTask)
        {
            _TaskRepository.Edit(workTask);
        }

        public void Delete(WorkTask workTask)
        {
            _TaskRepository.Delete(workTask.Id);
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

        public WorkTask[] GetPlanedTasks()
        {
            try
            {
                var list = _TaskRepository.SelectToDo();

                //var listClient = _ClientRepository.SelectAll();
                //var listProcess = _ProcessRepository.SelectAll();


                return list;
            }
            catch (Exception)
            {
                return new WorkTask[0];
            }

        }
    }
}
