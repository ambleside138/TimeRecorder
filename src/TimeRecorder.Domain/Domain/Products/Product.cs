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

        /// <summary>
        /// 無効フラグ
        /// </summary>
        public bool Invalid { get; }

        public static Product Empty => new(Identity<Product>.Empty, "未選択", "ミセンタク", false);

        public Product(Identity<Product> id, string name, string shortName, bool invalid = false)
        {
            Id = id;
            Name = name;
            ShortName = shortName;
            Invalid = invalid;
        }

        protected override IEnumerable<object> GetIdentityValues()
        {
            yield return Id;
        }
    }
}
