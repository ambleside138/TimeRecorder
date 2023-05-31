using System;
using TimeRecorder.Domain;
using TimeRecorder.Domain.Domain.Clients;

namespace TimeRecorder.Repository.InMemory;

public class ClientRepository : IClientRepository
{
    public void Add(Client client) => throw new NotImplementedException();

    public Client[] SelectAll()
    {
        return new Client[]
        {
                new Client(new Identity<Client>(1), "朽木病院","クチキビョウイン"),
                new Client(new Identity<Client>(2), "黒崎眼科", "クロサキガンカ"),
                new Client(new Identity<Client>(3), "井上病院", "イノウエビョウイン"),
                new Client(new Identity<Client>(4), "石田クリニック", "イシダクリニック"),
        };
    }
}
