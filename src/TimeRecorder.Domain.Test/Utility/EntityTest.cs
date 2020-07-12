using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Test.Utility
{
    [TestFixture]
    class EntityTest
    {
        [Test]
        public void 同一性の確認()
        {
            var t1 = new SampleEntity
            {
                Id = new Identity<SampleEntity>(1),
                Name = "test",
                Value = 123
            };

            var t2 = new SampleEntity
            {
                Id = new Identity<SampleEntity>(2),
                Name = "test",
                Value = 123
            };

            var t1_copy = new SampleEntity
            {
                Id = new Identity<SampleEntity>(1),
                Name = "hogehoge",
                Value = 123456
            };

            Assert.IsTrue(t1.Equals(t1_copy));
            Assert.IsFalse(t1.Equals(t2));
        }

        [Test]
        public void 同一性の確認_NULL()
        {
            var t1 = new SampleEntity
            {
                Id = new Identity<SampleEntity>(1),
                Name = "test",
                Value = 123
            };

            Assert.IsFalse(t1.Equals(null));
        }
    }

    class SampleEntity : Entity<SampleEntity>
    {
        public Identity<SampleEntity> Id { get; set; }

        public string Name { get; set; }

        public int Value { get; set; }

        protected override IEnumerable<object> GetIdentityValues()
        {
            yield return Id;
        }
    }
}
