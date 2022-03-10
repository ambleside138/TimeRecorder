using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Repository.SQLite.System.Versions;

class Version_000_012_001_000 : IVersion
{
    public string CommandQuery => @"
UPDATE
  processes
SET
  displayorder = id
  , invalid = '0';

UPDATE
  products
SET
  displayorder = id
  , invalid = '0';
";

    public string Version => "000.012.001.000";
}
