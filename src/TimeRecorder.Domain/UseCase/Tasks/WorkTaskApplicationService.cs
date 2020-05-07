using MicroResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Hospitals;
using TimeRecorder.Domain.Domain.Processes;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.UseCase.Tasks
{
    public class WorkTaskApplicationService
    {
        private readonly IWorkTaskRepository _TaskRepository;
        //private IHospitalRepository _HospitalRepository;
        //private IProcessRepository _ProcessRepository;

        public WorkTaskApplicationService(IWorkTaskRepository taskRepository)
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

                //var listHospital = _HospitalRepository.SelectAll();
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
