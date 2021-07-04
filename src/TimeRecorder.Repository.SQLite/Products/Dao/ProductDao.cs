using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Repository.SQLite.Utilities;

namespace TimeRecorder.Repository.SQLite.Products.Dao
{
    class ProductDao : DaoBase
    {
        public ProductDao(SQLiteConnection connection, SQLiteTransaction transaction)
            : base(connection, transaction) { }

        public ProductTableRow[] SelectAll()
        {
            #region SQL
            const string sql = @"
SELECT
  id
  , name
  , shortname
  , invalid
FROM
  products
ORDER BY
  displayorder
";
            #endregion

            return Connection.Query<ProductTableRow>(sql).ToArray();
        }
    }
}
