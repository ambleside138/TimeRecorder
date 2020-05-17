using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Repository.SQLite.Clients
{
    public class SQLiteClientRepository : IClientRepository
    {
        public Client[] SelectAll()
        {
            #region SQL
            const string sql = @"
SELECT
  id
  , name
  , kananame
FROM
  clients
";
            #endregion

            var list = new List<Client>();

            RepositoryAction.Query(c =>
            {
                list.AddRange(c.Query<ClientTableRow>(sql).Select(r => new Client(new Identity<Client>(r.Id), r.Name, r.KanaName)));
            });

            return list.OrderBy(i => i.KanaName)
                       .ThenBy(i => i.Id.Value)
                       .ToArray();
        }

        class ClientTableRow
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string KanaName { get; set; }
        }
    }
}
