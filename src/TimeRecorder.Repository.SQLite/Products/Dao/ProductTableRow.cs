using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Products;
using TimeRecorder.Domain;

namespace TimeRecorder.Repository.SQLite.Products.Dao;

class ProductTableRow
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string ShortName { get; set; }

    public string Invalid { get; set; }

    public Product ToDomainObject()
    {
        return new Product(new Identity<Product>(Id), Name, ShortName, Invalid == "1");
    }
}
