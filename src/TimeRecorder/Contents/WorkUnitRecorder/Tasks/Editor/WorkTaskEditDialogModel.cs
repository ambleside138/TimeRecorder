using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.WorkProcesses;
using TimeRecorder.Domain.UseCase.WorkProcesses;

namespace TimeRecorder.Contents.WorkUnitRecorder.Tasks.Editor
{
    class WorkTaskEditDialogModel
    {
        private readonly WorkProcessUseCase _ProcessUseCase;

        public WorkTaskEditDialogModel()
        {
            _ProcessUseCase = new WorkProcessUseCase(ContainerHelper.Resolver.Resolve<IWorkProcessRepository>());
        }

        public WorkProcess[] GetProcesses()
        {
            return _ProcessUseCase.GetProcesses();
        }
    }
}
