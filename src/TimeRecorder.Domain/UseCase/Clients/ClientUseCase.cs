using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Clients;

namespace TimeRecorder.UseCase.Clients
{
    /// <summary>
    /// 病院（＝案件） に関するPresentation層との相互作用を実装します
    /// </summary>
    public class ClientUseCase
    {
        private readonly IClientRepository _ClientRepository;

        public ClientUseCase(IClientRepository ClientRepository)
        {
            _ClientRepository = ClientRepository;
        }

        public Client[] GetClients()
        {
            return _ClientRepository.SelectAll();
        }
    }
}
