using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Clients
{
    public class Client
    {
        public Identity<Client> Id { get; set; }

        public string Name { get; set; }

        public string KanaName { get; set; }

        public static Client Empty => new Client(Identity<Client>.Empty, "未選択", "ミセンタク");

        public Client(Identity<Client> id, string name, string kanaName)
        {
            Id = id;
            Name = name;
            KanaName = kanaName;
        }
    }
}
