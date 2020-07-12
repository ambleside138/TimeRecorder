using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Domain.Products
{
    public interface IProductRepository : IRepository
    {
        Product[] SelectAll();
    }
}
