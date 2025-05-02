using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Repository.SQLite.System.Versions;
internal class Version_000_017_000_001 : IVersion
{
    public string CommandQuery => @"
CREATE TABLE
  timecardlinks
(
  year int
  , month int
  , linkurl varchar(256)
  ,  PRIMARY KEY ( year, month )
);
";

    public string Version => "000.017.000.001";
}
