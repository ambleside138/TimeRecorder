using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Repository.SQLite.System.Versions;

public interface IVersion
{
    string CommandQuery { get; }

    string Version { get; }
}
