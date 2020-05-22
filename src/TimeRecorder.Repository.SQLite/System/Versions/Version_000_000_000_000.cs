using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Repository.SQLite.System.Versions
{
    class Version_000_000_000_000 : IVersion
    {
        // dapperを利用する場合、Enumはintで定義するとマッピングしてくれる
        // SQLite は VARCHAR(N) のように N を指定したとしても、TEXT 型として解釈する

        public string CommandQuery => @"
create table 
  worktasks
(
  id INTEGER PRIMARY KEY AUTOINCREMENT
  , title varchar(64)
  , taskcategory int 
  , productId int
  , clientId int
  , processId int
  , remarks varchar(128)
  , planedStartDateTime datetime
  , planedEndDateTime datetime
  , actualStartDateTime datetime
  , actualEndDateTime datetime
);

create table
  workingtimes
(
  id INTEGER PRIMARY KEY AUTOINCREMENT
  , taskid int
  , ymd varchar(8)
  , starttime varchar(6)
  , endtime varchar(6)
);

CREATE TABLE 
  processes
(
  id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT
  , title varchar(64)
);

CREATE TABLE 
  clients
(
  id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT
  , name varchar(128)
  , kananame varchar(128)
);

CREATE TABLE 
  products 
(
  id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  name  varchar(32),
  shortname varchar(8)
);

CREATE TABLE 
  systemversionlog 
(
  id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  version  char(15),
  updatetime datetime
);

";

        public string Version => "000.000.000.000";
    }
}
