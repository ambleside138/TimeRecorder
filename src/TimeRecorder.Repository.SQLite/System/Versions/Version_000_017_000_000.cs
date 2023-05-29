using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Repository.SQLite.System.Versions;
internal class Version_000_017_000_000 : IVersion
{
    public string CommandQuery => @"
CREATE TABLE
  segments
(
  id INTEGER PRIMARY KEY AUTOINCREMENT
  , name varchar(32)
  , displayorder int
);

ALTER TABLE
  worktasks
ADD COLUMN
  segmentid int
";

    public string Version => "000.017.000.000";
}
