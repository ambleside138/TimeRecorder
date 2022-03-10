using Livet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Clients;

namespace TimeRecorder.Contents.Shared.Clients;
internal class ClientsMaintenanceViewModel : ViewModel
{
    private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

    private readonly ClientsMaintenanceModel _model = new();
    public ObservableCollection<Client> Clients { get; }

    public ClientsMaintenanceViewModel()
    {
        Clients = new ObservableCollection<Client>();
    }

    public void OnLoaded()
    {
        _model.FetchClients();
    }


}
