using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Repository.SQLite.System.Versions
{
    class Version_000_001_000_000 : IVersion
    {
        public string CommandQuery => @"
CREATE TABLE 
  configs
(
  configkey varchar(128)
  , jsonvalue text
);
";

        public string Version => "000.001.000.000";
    }
}
