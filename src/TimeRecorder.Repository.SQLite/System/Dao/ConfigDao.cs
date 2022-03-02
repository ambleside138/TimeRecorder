using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using TimeRecorder.Repository.SQLite.Utilities;

namespace TimeRecorder.Repository.SQLite.System.Dao;

class ConfigDao : DaoBase
{
    public ConfigDao(SQLiteConnection connection, SQLiteTransaction transaction)
       : base(connection, transaction) { }

    public void DeleteInsert(ConfigTableRow row)
    {
        #region SQL
        const string sql = @"
delete from 
  configs 
where 
  configkey  = @configkey;

insert into configs
(
  configkey
  , jsonvalue
)
values
(
  @configkey
  , @jsonvalue
)
";
        #endregion

        Connection.Execute(sql, row, Transaction);
    }


    public ConfigTableRow[] SelectAll()
    {
        #region SQL
        const string sql = @"
SELECT * FROM configs";
        #endregion

        return Connection.Query<ConfigTableRow>(sql).ToArray();
    }
}
