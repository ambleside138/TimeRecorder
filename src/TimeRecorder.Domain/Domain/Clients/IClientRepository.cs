using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Domain.Clients
{
    public interface IClientRepository
    {
        Client[] SelectAll();
    }
}
