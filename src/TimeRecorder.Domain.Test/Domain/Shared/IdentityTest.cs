using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Test.Domain.Shared
{
    [TestFixture]
    class IdentityTest
    {
        [Test]
        public void 同一性の確認()
        {
            var key1 = new Identity<Staff>(1);
            var key1Copy = new Identity<Staff>(1);

            Assert.IsTrue(key1.Equals(key1Copy));
            Assert.IsTrue(key1 == key1Copy);
        }

        [Test]
        public void 非同一性の確認()
        {
            var key1 = new Identity<Staff>(1);
            var key2 = new Identity<Staff>(2);

            var key3 = new Identity<Customer>(1); 

            Assert.IsFalse(key1.Equals(key2));
            Assert.IsFalse(key1 == key2);

            Assert.IsFalse(key1.Equals(key3));
            //Assert.IsFalse(key1 == key3); //compile error
        }


        class Staff
        {

        }

        class Customer
        {

        }
    }


}
