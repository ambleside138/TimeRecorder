﻿using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Clients;

/// <summary>
/// ユーザ を表します
/// </summary>
public class Client : Entity<Client>
{
    public Identity<Client> Id { get; set; }

    public string Name { get; set; }

    public string KanaName { get; set; }

    public string InitialLetters => string.IsNullOrEmpty(KanaName) ? "" :  KanaName.Substring(0, 1);

    public static Client Empty => new(Identity<Client>.Empty, "未選択", "");

    public Client(Identity<Client> id, string name, string kanaName)
    {
        Id = id;
        Name = name;
        KanaName = kanaName;
    }

    public static Client ForSource(string name, string kanaName = "")
        => new Client(Identity<Client>.Temporary, name, kanaName);

    protected override IEnumerable<object> GetIdentityValues()
    {
        yield return Id;
    }
}
