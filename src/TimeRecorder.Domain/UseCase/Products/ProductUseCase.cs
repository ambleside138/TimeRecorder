using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Products;

namespace TimeRecorder.Domain.UseCase.Products
{
    public class ProductUseCase
    {
        private readonly IProductRepository _ProductRepository;

        public ProductUseCase(IProductRepository ProductRepository)
        {
            _ProductRepository = ProductRepository;
        }

        public Product[] GetProducts()
        {
            return _ProductRepository.SelectAll();
        }
    }
}
