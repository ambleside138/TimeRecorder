using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Repository.SQLite.Utilities;

namespace TimeRecorder.Repository.SQLite.WorkProcesses.Dao
{
    class WorkProcessDao : DaoBase
    {
        public WorkProcessDao(SQLiteConnection connection, SQLiteTransaction transaction)
            : base(connection, transaction) { }

        public WorkProcessTableRow[] SelectAll()
        {
            #region SQL
            const string sql = @"
SELECT
  id
  , title
FROM
  processes
";
            #endregion

            return Connection.Query<WorkProcessTableRow>(sql).ToArray();
        }
    }
}
