using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Repository.SQLite.Utilities;

namespace TimeRecorder.Repository.SQLite.Tasks.Dao
{
    class WorkTaskCompletedDao : DaoBase
    {
        public WorkTaskCompletedDao(SQLiteConnection connection, SQLiteTransaction transaction)
            : base(connection, transaction)
        {
        }

        public void InsertIfNotExist(int worktaskId)
        {
            #region SQL
            const string sql = @"
INSERT INTO worktaskscompleted
(
  worktaskid,
  registdatetime
)
VALUES
(
  @worktaskid,
  @registdatetime
)
";
            #endregion

            var row = new 
            {
                WorkTaskId = worktaskId,
                RegistDateTime = DateTime.Now,
            };

            Connection.Execute(sql, row, Transaction);
        }

        public void DeleteByWorkTaskId(int worktaskId)
        {
            #region SQL
            const string sql = @"
DELETE FROM 
  worktaskscompleted
WHERE
  worktaskid = @worktaskid
  ";
            #endregion

            Connection.Execute(sql, new { worktaskId }, Transaction);
        }

        public bool IsCompleted(int workTaskId)
        {
            #region SQL
            const string sql = @"
SELECT
  1
FROM
  worktaskscompleted
WHERE
  worktaskid = @worktaskid
";
            #endregion

            return Connection.QuerySingleOrDefault(sql, new { workTaskId }) != null;
        }

        public int[] SelectCompleted(int[] worktaskIds)
        {
            #region SQL
            const string sql = @"
SELECT
  worktaskid
FROM
  worktaskscompleted
WHERE
  worktaskid in @worktaskids
";
            #endregion

            return Connection.Query<int>(sql, new { worktaskIds }).ToArray();
        }
    }
}
