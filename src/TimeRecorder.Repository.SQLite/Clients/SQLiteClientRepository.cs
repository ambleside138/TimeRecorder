using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Repository.SQLite.Clients.Dao;

namespace TimeRecorder.Repository.SQLite.Clients;

public class SQLiteClientRepository : IClientRepository
{
    public Client[] SelectAll()
    {
        var list = new List<Client>();

        RepositoryAction.Query(c =>
        {
            var listRow = new ClientDao(c, null).SelectAll();
            list.AddRange(listRow.Select(r => r.ToDomainObject()));
        });

        return list.OrderBy(i => i.KanaName)
                   .ThenBy(i => i.Id.Value)
                   .ToArray();
    }
}
