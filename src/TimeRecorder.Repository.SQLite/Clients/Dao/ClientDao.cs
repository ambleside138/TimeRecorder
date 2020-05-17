using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Repository.SQLite.Utilities;
namespace TimeRecorder.Repository.SQLite.Clients.Dao
{
    class ClientDao : DaoBase
    {
        public ClientDao(SQLiteConnection connection, SQLiteTransaction transaction)
            : base(connection, transaction) { }

        public ClientTableRow[] SelectAll()
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

            return Connection.Query<ClientTableRow>(sql).ToArray();
        }
    }
}
