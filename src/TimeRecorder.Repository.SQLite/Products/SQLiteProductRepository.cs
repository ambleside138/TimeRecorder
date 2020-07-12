using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Products;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Repository.SQLite.Products.Dao;

namespace TimeRecorder.Repository.SQLite.Products
{
    public class SQLiteProductRepository : IProductRepository
    {
 
        public Product[] SelectAll()
        {
            var list = new List<Product>();

            RepositoryAction.Query(c =>
            {
                var rows = new ProductDao(c, null).SelectAll();
                list.AddRange(rows.Select(r => r.ToDomainObject()));
            });

            return list.ToArray();
        }
    }
}
