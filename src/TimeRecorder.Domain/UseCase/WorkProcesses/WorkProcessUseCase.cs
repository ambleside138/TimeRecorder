using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.WorkProcesses;

namespace TimeRecorder.Domain.UseCase.WorkProcesses
{
    // １クラスに複数メソッドを定義するときは「関心事」＋UseCaseと命名

    /// <summary>
    /// 工程 に関するPresentation層との相互作用を実装します
    /// </summary>
    public class WorkProcessUseCase
    {
        private readonly IWorkProcessRepository _ProcessRepository;
        private readonly Domain.WorkProcesses.WorkProcessService _ProcessService;

        public WorkProcessUseCase(IWorkProcessRepository processRepository)
        {
            _ProcessRepository = processRepository;
            _ProcessService = new Domain.WorkProcesses.WorkProcessService(processRepository);
        }

        public WorkProcess[] GetProcesses()
        {
            return _ProcessRepository.SelectAll();
        }

        public WorkProcess Regist(string title)
        {
            var process = new WorkProcess(title);

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
