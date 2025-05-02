using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Domain.Clients;

public interface IClientRepository : IRepository
{
    Client[] SelectAll();

    void Add(Client client);
}
