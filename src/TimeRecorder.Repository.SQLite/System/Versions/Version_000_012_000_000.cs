using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Repository.SQLite.System.Versions
{
    class Version_000_012_000_000 : IVersion
    {
        public string CommandQuery => @"
ALTER TABLE
  processes
ADD COLUMN
  displayorder int;

ALTER TABLE
  processes
ADD COLUMN
  invalid char(1);

ALTER TABLE
  products
ADD COLUMN
  displayorder int;

ALTER TABLE
  products
ADD COLUMN
  invalid char(1);
";

        public string Version => "000.012.000.000";
    }
}
