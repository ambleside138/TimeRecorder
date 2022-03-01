using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeRecorder.Repository.SQLite.System.Versions;

public static class VersionManager
{
    public static string CurrentVersion => Versions.Max(v => v.Version);

    public static IEnumerable<IVersion> Versions => new IVersion[]
    {
            new Version_000_000_000_000(),
            new Version_000_001_000_000(),
            new Version_000_002_000_000(),
            new Version_000_003_000_000(),
            new Version_000_008_000_000(),
            new Version_000_009_000_000(),
            new Version_000_009_001_000(),
            new Version_000_012_000_000(),
            new Version_000_012_001_000(),
    };
}
