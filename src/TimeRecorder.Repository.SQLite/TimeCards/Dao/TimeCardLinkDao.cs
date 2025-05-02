using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain;
using TimeRecorder.Repository.SQLite.Tasks.Dao;
using TimeRecorder.Repository.SQLite.Utilities;

namespace TimeRecorder.Repository.SQLite.TimeCards.Dao;
internal class TimeCardLinkDao : DaoBase
{
    public TimeCardLinkDao(SQLiteConnection connection, SQLiteTransaction transaction)
        : base(connection, transaction) { }

    public void Upsert(TimeCardLinkTableRow row)
    {
        #region sql
        const string sql = @"
replace into timecardlinks
(
  year
  , month
  , linkurl
)
values
(
  @year
  , @month
  , @linkurl
)
";

        #endregion

        Connection.Execute(sql, row, Transaction);
    }

    public TimeCardLinkTableRow Select(YearMonth key)
    {
        #region SQL
        const string sql = @"
SELECT
  year
  , month
  , linkurl
FROM
  timecardlinks
WHERE
  year = @year
  AND month = @month
";
        #endregion

        return Connection.Query<TimeCardLinkTableRow>(sql, new { Year = key.Year, Month = key.Month }).FirstOrDefault();
    }
}
