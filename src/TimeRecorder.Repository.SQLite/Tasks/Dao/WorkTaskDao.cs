using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Utility;
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
  , productid
  , clientId 
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
  , @productId
  , @clientId 
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
  , productId = @productId
  , clientId = @clientId
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

        /// <summary>
        /// 作業時間の記録に利用するためのタスクを取得します
        /// </summary>
        /// <param name="ymd">日付</param>
        /// <returns></returns>
        public WorkTaskTableRow[] SelectPlaned(YmdString ymd)
        {
            var where = @"
actualenddatetime IS NULL
";
            return SelectCore(where, new { ymd = ymd.Value }).ToArray();
        }

        public WorkTaskTableRow SelectById(int taskId)
        {
            return SelectCore("id = @Id", new { Id = taskId }).FirstOrDefault();
        }

        public IEnumerable<WorkTaskTableRow> SelectCore(string whereQuery, object param = null)
        {
            #region sql
            const string sql = @"
SELECT
  id
  , title 
  , taskcategory 
  , productId
  , clientId 
  , processId 
  , remarks
  , planedStartDateTime 
  , planedEndDateTime 
  , actualStartDateTime 
  , actualEndDateTime 
FROM
  worktasks
WHERE
  1 = 1
";
            #endregion

            var query = sql;
            if(string.IsNullOrEmpty(whereQuery) == false)
            {
                query += "AND " + whereQuery;
            }

            return Connection.Query<WorkTaskTableRow>(query, param ?? new object());
        }

    }
}
