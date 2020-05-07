using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Processes;

namespace TimeRecorder.Domain.UseCase.Processes
{
    /// <summary>
    /// 工程 に関するPresentation層との相互作用を実装します
    /// </summary>
    public class ProcessApplicationService
    {
        private readonly IProcessRepository _ProcessRepository;
        private readonly ProcessService _ProcessService;

        public ProcessApplicationService(IProcessRepository processRepository)
        {
            _ProcessRepository = processRepository;
            _ProcessService = new ProcessService(processRepository);
        }

        public Process[] GetProcesses()
        {
            return _ProcessRepository.SelectAll();
        }

        public Process Regist(string title)
        {
            var process = new Process(title);

            if (_ProcessService.IsDuplicated(process))
            {
                throw new Exception("重複しています");
            }
            else
            {
                return _ProcessRepository.Regist(process);
            }
        }
    }
}
