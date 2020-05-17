using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Domain.Domain.WorkProcesses;
using TimeRecorder.Domain.UseCase.WorkProcesses;
using TimeRecorder.UseCase.Clients;

namespace TimeRecorder.Contents.WorkUnitRecorder.Tasks.Editor
{
    class WorkTaskEditDialogModel
    {
        private readonly WorkProcessUseCase _ProcessUseCase;
        private readonly ClientUseCase _ClientUseCase;

        public WorkTaskEditDialogModel()
        {
            _ProcessUseCase = new WorkProcessUseCase(ContainerHelper.Resolver.Resolve<IWorkProcessRepository>());
            _ClientUseCase = new ClientUseCase(ContainerHelper.Resolver.Resolve<IClientRepository>());
        }

        public WorkProcess[] GetProcesses()
        {
            return _ProcessUseCase.GetProcesses();
        }

        public Client[] GetClients()
        {
            var list = new List<Client> { Client.Empty };
            list.AddRange(_ClientUseCase.GetClients());
            return list.ToArray();
        }
    }
}
