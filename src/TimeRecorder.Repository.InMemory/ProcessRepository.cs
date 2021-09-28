using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.WorkProcesses;

namespace TimeRecorder.Repository.InMemory
{
    public class ProcessRepository : IWorkProcessRepository
    {
        private readonly List<WorkProcess> _ListProcess = new()
        {
            new WorkProcess(new Domain.Identity<WorkProcess>(1), "コーディング"),
            new WorkProcess(new Domain.Identity<WorkProcess>(2), "テスト"),
        };

        public WorkProcess Regist(WorkProcess process)
        {
            var newId = 1;
            if(_ListProcess.Any())
            {
                newId = _ListProcess.Max(i => i.Id.Value) + 1;
            }

            var newProcess = new WorkProcess(new Domain.Identity<WorkProcess>(newId), process.Title);
            _ListProcess.Add(newProcess);
            
            return newProcess;
        }

        public WorkProcess[] SelectAll()
        {
            return _ListProcess.ToArray();
        }
    }
}
