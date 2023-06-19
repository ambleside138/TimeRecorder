using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Repository.SQLite.System.Versions;
internal class Version_000_017_000_002 : IVersion
{
    public string CommandQuery => @"
ALTER TABLE
  processes
ADD COLUMN
  taskcategoryfilters;

ALTER TABLE
  products
ADD COLUMN
  taskcategoryfilters;

DROP TABLE
  worktasksarchive;
";

    public string Version => "000.017.000.002";
}
