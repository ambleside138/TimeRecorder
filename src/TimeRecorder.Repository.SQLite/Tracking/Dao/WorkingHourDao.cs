using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Repository.SQLite.Utilities;

namespace TimeRecorder.Repository.SQLite.Tracking.Dao
{
    class WorkingHourDao : DaoBase
    {
        public WorkingHourDao(SQLiteConnection connection, SQLiteTransaction transaction) : base(connection, transaction)
        {
        }

        public void Insert(WorkingHourTableRow row)
        {
            #region SQL
            const string sql = @"
INSERT INTO
  workinghours
(
  ymd
  , starttime
  , endtime
)
VALUES
(
  @ymd
  , @starttime
  , @endtime
)
";
            #endregion

            Connection.Execute(sql, row);
        }

        public void Update(WorkingHourTableRow row)
        {
            #region SQL
            const string sql = @"
UPDATE
  workinghours
SET
  starttime = @starttime
  , endtime = @endtime
WHERE
  ymd = @ymd
";
            #endregion

            Connection.Execute(sql, row);
        }

        public WorkingHourTableRow SelectByYmd(string ymd)
        {
            #region SQL
            const string sql = @"
SELECT
  ymd
  , starttime
  , endtime
FROM  
  workinghours
WHERE
  ymd = @ymd
";
            #endregion

            return Connection.Query<WorkingHourTableRow>(sql, new { ymd }).FirstOrDefault();
        }

        public WorkingHourTableRow[] SelectByYmdRange(string startymd, string endymd)
        {
            #region SQL
            const string sql = @"
SELECT
  ymd
  , starttime
  , endtime
FROM  
  workinghours
WHERE
  ymd BETWEEN @startymd AND @endymd
";
            #endregion

            return Connection.Query<WorkingHourTableRow>(sql, new { startymd, endymd }).ToArray();
        }
    }
}
