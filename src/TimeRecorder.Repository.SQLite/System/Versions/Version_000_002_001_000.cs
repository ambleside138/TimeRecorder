using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Repository.SQLite.System.Versions;

class Version_000_002_001_000 : IVersion
{
    public string CommandQuery => @"
CREATE INDEX 
  idx_importkey
ON
  worktasks(importkey);
";

    public string Version => "000.002.001.000";
}
