using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.UseCase.Clients;
using TimeRecorder.Repository.InMemory;

namespace TimeRecorder.Domain.Test.UseCase.Clients;

[TestFixture]
class ClientUseCaseTest
{

    [Test]
    public void 病院一覧の取得()
    {
        var interactor = new ClientUseCase(new ClientRepository());

        var list = interactor.GetClients();

        Assert.IsNotNull(list);

        Assert.IsTrue(list.Length == 4);
    }
}
