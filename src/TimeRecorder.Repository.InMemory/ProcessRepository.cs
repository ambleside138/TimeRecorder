using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Processes;

namespace TimeRecorder.Repository.InMemory
{
    public class ProcessRepository : IProcessRepository
    {
        private readonly List<Process> _ListProcess = new List<Process>()
        {
            new Process(new Domain.Utility.Identity<Process>(1), "コーディング"),
            new Process(new Domain.Utility.Identity<Process>(2), "テスト"),
        };

        public Process Regist(Process process)
        {
            var newId = 1;
            if(_ListProcess.Any())
            {
                newId = _ListProcess.Max(i => i.Id.Value) + 1;
            }

            var newProcess = new Process(new Domain.Utility.Identity<Process>(newId), process.Title);
            _ListProcess.Add(newProcess);
            
            return newProcess;
        }

        public Process[] SelectAll()
        {
            return _ListProcess.ToArray();
        }
    }
}
