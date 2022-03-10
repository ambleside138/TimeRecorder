using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Helpers;
using TimeRecorder.UseCase.Clients;

namespace TimeRecorder.Contents.Shared.Clients;
internal class ClientsMaintenanceModel
{
    private readonly ClientUseCase _ClientUseCase;

    public ObservableCollection<Client> Clients { get; } = new();


    public ClientsMaintenanceModel()
    {
        _ClientUseCase = new ClientUseCase(ContainerHelper.GetRequiredService<IClientRepository>());
    }

    public void FetchClients()
    {
        Clients.Clear();
        Clients.AddRange(_ClientUseCase.GetClients());
    }
}
