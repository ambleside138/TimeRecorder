using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Products
{
    /// <summary>
    /// 製品 を表します
    /// </summary>
    public class Product : Entity<Product>
    {
        public Identity<Product> Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public static Product Empty => new Product(Identity<Product>.Empty, "未選択", "ミセンタク");

        public Product(Identity<Product> id, string name, string shortName)
        {
            Id = id;
            Name = name;
            ShortName = shortName;
        }

        protected override IEnumerable<object> GetIdentityValues()
        {
            yield return Id;
        }
    }
}
