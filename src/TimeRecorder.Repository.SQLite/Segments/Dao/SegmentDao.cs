using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Repository.SQLite.Clients.Dao;
using TimeRecorder.Repository.SQLite.Utilities;

namespace TimeRecorder.Repository.SQLite.Segments.Dao;
internal class SegmentDao : DaoBase
{
    public SegmentDao(SQLiteConnection connection, SQLiteTransaction transaction)
        : base(connection, transaction) { }

    public SegmentTableRow[] SelectAll()
    {
        #region SQL
        const string sql = @"
SELECT
  id
  , name
FROM
  segments
ORDER BY 
  displayorder
";
        #endregion

        return Connection.Query<SegmentTableRow>(sql).ToArray();
    }
}
