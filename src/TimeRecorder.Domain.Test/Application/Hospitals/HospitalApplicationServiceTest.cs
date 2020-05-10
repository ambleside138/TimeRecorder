using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.UseCase.Hospitals;
using TimeRecorder.Repository.InMemory;

namespace TimeRecorder.Domain.Test.UseCase.Hospitals
{
    [TestFixture]
    class HospitalApplicationServiceTest
    {

        [Test]
        public void 病院一覧の取得()
        {
            var interactor = new HospitalUseCase(new HospitalRepository());

            var list = interactor.GetHospitals();

            Assert.IsNotNull(list);

            Assert.IsTrue(list.Length == 4);
        }
    }
}
