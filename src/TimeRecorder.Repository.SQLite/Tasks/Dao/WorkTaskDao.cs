using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using TimeRecorder.Repository.SQLite.Utilities;

namespace TimeRecorder.Repository.SQLite.Tasks.Dao
{
    class WorkTaskDao : DaoBase
    {
        public WorkTaskDao(SQLiteConnection connection, SQLiteTransaction transaction)
            : base(connection, transaction) { }

        public int Insert(WorkTaskTableRow row)
        {
            #region sql
            const string sql = @"
insert into worktasks
(
  title 
  , taskcategory 
  , product
  , ClientId 
  , processId 
  , remarks
  , planedStartDateTime 
  , planedEndDateTime 
  , actualStartDateTime 
  , actualEndDateTime 
)
values
(
  @title 
  , @taskcategory 
  , @product
  , @ClientId 
  , @processId 
  , @remarks
  , @planedStartDateTime 
  , @planedEndDateTime 
  , @actualStartDateTime 
  , @actualEndDateTime 
)
";
            #endregion

            Connection.Execute(sql, row, Transaction);

            return (int)Connection.LastInsertRowId;
        }

        public void Delete(int taskId)
        {
            #region SQL
            const string sql = @"
DELETE FROM
  worktasks
WHERE
  id = @id
";
            #endregion

            Connection.Execute(sql, new { id = taskId }, Transaction);
        }

        public void Update(WorkTaskTableRow row)
        {
            #region SQL
            const string sql = @"
update worktasks
set
  title = @title
  , taskcategory = @taskcategory 
  , product = @product
  , ClientId = @ClientId
  , processId = @processId
  , remarks = @remarks
  , planedStartDateTime = @planedStartDateTime 
  , planedEndDateTime  = @planedEndDateTime
  , actualStartDateTime = @actualStartDateTime
  , actualEndDateTime = @actualEndDateTime
where
  id = @id
";
            #endregion

            Connection.Execute(sql, row, Transaction);
        }

        public WorkTaskTableRow[] SelectPlaned()
        {
            #region sql
            const string sql = @"
SELECT
  id
  , title 
  , taskcategory 
  , product
  , ClientId 
  , processId 
  , remarks
  , planedStartDateTime 
  , planedEndDateTime 
  , actualStartDateTime 
  , actualEndDateTime 
FROM
  worktasks
WHERE
  actualenddatetime IS NULL
";
            #endregion

            return Connection.Query<WorkTaskTableRow>(sql).ToArray();
        }

        public WorkTaskTableRow SelectById(int taskId)
        {
            #region sql
            const string sql = @"
SELECT
  id
  , title 
  , taskcategory 
  , product
  , ClientId 
  , processId 
  , remarks
  , planedStartDateTime 
  , planedEndDateTime 
  , actualStartDateTime 
  , actualEndDateTime 
FROM
  worktasks
WHERE
  id = @Id
";
            #endregion

            return Connection.QueryFirstOrDefault<WorkTaskTableRow>(sql, new { Id = taskId });
        }
    }
}
