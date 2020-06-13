using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Repository.SQLite.System.Versions
{
    class Version_000_003_000_000 : IVersion
    {
        public string CommandQuery => @"
ALTER TABLE 
  worktasks
ADD COLUMN
  istemporary char(1);

CREATE TABLE worktasksarchive (
	id	INTEGER PRIMARY KEY AUTOINCREMENT,
	title	varchar(64),
	taskcategory	int,
	productid	int,
	clientId	int,
	processId	int,
	remarks	varchar(128),
	planedStartDateTime	datetime,
	planedEndDateTime	datetime,
	actualStartDateTime	datetime,
	actualEndDateTime	datetime,
	source	varchar(64),
	importkey	varchar(1024),
    istemporary char(1)
)
";

        public string Version => "000.003.000.000";
    }
}
