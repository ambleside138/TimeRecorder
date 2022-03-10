using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Repository.SQLite.System.Versions;

class Version_000_009_000_000 : IVersion
{
    // インポート済みタスク
    public string CommandQuery => @"
create table 
  importedtasks
(
  importkey varchar(1024) PRIMARY KEY
  , title varchar(64)
  , source varchar(64)
  , createdatetime datetime
);

";

    public string Version => "000.009.000.000";
}
