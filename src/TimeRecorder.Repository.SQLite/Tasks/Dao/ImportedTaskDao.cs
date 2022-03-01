using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using TimeRecorder.Repository.SQLite.Utilities;

namespace TimeRecorder.Repository.SQLite.Tasks.Dao;

class ImportedTaskDao : DaoBase
{
    public ImportedTaskDao(SQLiteConnection connection, SQLiteTransaction transaction)
        : base(connection, transaction)
    {
    }

    public void Insert(ImportedTaskTableRow row)
    {
        #region SQL
        const string sql = @"
INSERT INTO importedtasks
(
  importkey
  , title
  , source
  , createdatetime
  , worktaskid
)
VALUES
(
  @importkey
  , @title
  , @source
  , @createdatetime
  , @worktaskid
)
";
        #endregion

        row.CreateDateTime = DateTime.Now;

        Connection.Execute(sql, row, Transaction);
    }

    public ImportedTaskTableRow[] SelectByImportKeys(string[] keys)
    {
        #region SQL
        const string sql = @"
SELECT
  importkey
  , title
  , source
  , createdatetime
  , worktaskid
FROM
  importedtasks
WHERE
  importkey in @keys
";
        #endregion

        return Connection.Query<ImportedTaskTableRow>(sql, new { keys }).ToArray();
    }
}
