using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Repository.SQLite.System.Versions;

internal class Version_000_016_000_000: IVersion
{
    public string CommandQuery => @"
ALTER TABLE
  products
ADD COLUMN
  reportnameonly char(1);
";

    public string Version => "000.016.001.000";
}
