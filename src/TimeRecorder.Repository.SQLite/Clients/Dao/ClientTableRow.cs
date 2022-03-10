using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Domain;

namespace TimeRecorder.Repository.SQLite.Clients.Dao;

class ClientTableRow
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string KanaName { get; set; }

    public Client ToDomainObject()
    {
        return new Client(new Identity<Client>(Id), Name, KanaName);
    }

}
