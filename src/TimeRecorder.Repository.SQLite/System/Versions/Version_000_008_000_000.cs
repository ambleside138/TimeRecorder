using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Repository.SQLite.System.Versions;

class Version_000_008_000_000 : IVersion
{
    public string CommandQuery => @"
CREATE TABLE workinghours (
  ymd char(8)  PRIMARY KEY,
  starttime varchar(6),
  endtime varchar(6)
)
";

    public string Version => "000.008.000.000";
}
