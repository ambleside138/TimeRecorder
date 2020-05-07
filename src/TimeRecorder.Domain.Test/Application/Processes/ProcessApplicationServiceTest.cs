using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.UseCase.Processes;
using TimeRecorder.Repository.InMemory;

namespace TimeRecorder.Domain.Test.Application.Processes
{
    [TestFixture]
    class ProcessApplicationServiceTest
    {
        private ProcessApplicationService _Service;
 
        [SetUp]
        public void SetUp()
        {
            _Service = new ProcessApplicationService(new ProcessRepository());
        }

        [Test]
        public void 工程の登録()
        {
            var registedItem = _Service.Regist("test");

            Assert.IsNotNull(registedItem);
            Assert.IsTrue(registedItem.Title == "test");

            var list = _Service.GetProcesses();
            Assert.IsTrue(list.ToList().Last().Title == "test");
        }

        [Test]
        public void 工程の登録_重複()
        {
            var registedItem = _Service.Regist("test");

            var list = _Service.GetProcesses();
            Assert.IsTrue(list.ToList().Last().Title == "test");

            var ex = Assert.Throws<Exception>(() => _Service.Regist("test"));
        }

        [Test]
        public void 工程の一覧取得()
        {
            var list = _Service.GetProcesses();

            Assert.IsNotNull(list);

            Assert.IsTrue(list.Length == 2);
        }
    }
}
