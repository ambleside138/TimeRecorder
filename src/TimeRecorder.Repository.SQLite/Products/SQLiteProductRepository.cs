using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Products;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Repository.SQLite.Products
{
    public class SQLiteProductRepository : IProductRepository
    {
 
        public Product[] SelectAll()
        {
            #region SQL
            const string sql = @"
SELECT
  id
  , name
  , shortname
FROM
  products
";
            #endregion

            var list = new List<Product>();

            RepositoryAction.Query(c =>
            {
                list.AddRange(c.Query<ProductTableRow>(sql).Select(r => new Product(new Identity<Product>(r.Id), r.Name, r.ShortName)));
            });

            return list.ToArray();
        }

        class ProductTableRow
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string ShortName { get; set; }
        }
    }
}
