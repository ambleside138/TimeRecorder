using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Repository.SQLite.Utilities;

namespace TimeRecorder.Repository.SQLite.Tracking.Dao
{
    class WorkingTimeDao : DaoBase
    {
        public WorkingTimeDao(SQLiteConnection connection, SQLiteTransaction transaction)
            : base(connection, transaction) { }

        public int Insert(WorkingTimeTableRow row)
        {
            #region SQL
            const string sql = @"
INSERT INTO 
  workingtimes
(
  taskid
  , ymd
  , starttime
  , endtime
)
values
(
  @taskid
  , @ymd
  , @starttime
  , @endtime
)
";
            #endregion

            Connection.Execute(sql, row, Transaction);

            return (int)Connection.LastInsertRowId;
        }

        public void Update(WorkingTimeTableRow row)
        {
            #region SQL
            const string sql = @"
UPDATE
  workingtimes
SET
  taskid = @taskid
  , ymd = @ymd
  , starttime = @starttime
  , endtime = @endtime
WHERE
  id = @id
";
            #endregion

            Connection.Execute(sql, row, Transaction);
        }

        public void Delete(int wokingTimeId)
        {
            #region SQL
            const string sql = @"
DELETE FROM
  workintimes
WHERE
  id = @id
";
            #endregion

            Connection.Execute(sql, new { id = wokingTimeId }, Transaction);
        }

        public WorkingTimeTableRow[] SelectYmd(string ymd)
        {
            #region SQL
            const string sql = @"
SELECT
  id
  , taskid
  , ymd
  , starttime
  , endtime
FROM
  workingtimes
WHERE
  ymd = @ymd
";
            #endregion

            return Connection.Query<WorkingTimeTableRow>(sql, new { ymd }).ToArray();
        }

        public WorkingTimeTableRow SelectId(int id)
        {
            #region SQL
            const string sql = @"
SELECT
  id
  , taskid
  , ymd
  , starttime
  , endtime
FROM
  workingtimes
WHERE
  id = @id
";
            #endregion

            return Connection.Query<WorkingTimeTableRow>(sql, new { id }).FirstOrDefault();
        }

        public WorkingTimeTableRow[] SelectByTaskIds(int[] taskids)
        {
            #region SQL
            const string sql = @"
SELECT
  id
  , taskid
  , ymd
  , starttime
  , endtime
FROM
  workingtimes
WHERE
  taskid in @taskids
";
            #endregion

            // DapperならIN句も配列を使える
            // ＃実装は、配列の数だけプレースホルダを用意する仕掛けのようなのでパフォーマンス的にはびみょうなのかもしれない
            return Connection.Query<WorkingTimeTableRow>(sql, new { taskids }).ToArray();
        }
    }
}
