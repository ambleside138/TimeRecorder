using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Repository.SQLite.System.Versions;

class Version_000_002_000_000 : IVersion
{
    // 列の複数追加はサポートされていないよう
    public string CommandQuery => @"
ALTER TABLE 
  worktasks
ADD COLUMN
  source varchar(64);

ALTER TABLE 
  worktasks
ADD COLUMN
  importkey varchar(1024)
";

    public string Version => "000.002.000.000";
}
