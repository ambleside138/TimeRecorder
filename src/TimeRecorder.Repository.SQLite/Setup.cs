using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace TimeRecorder.Repository.SQLite
{
    public static class Setup
    {

        public static void CreateDatabaseFile()
        {
            // DBファイルの作成
            SQLiteConnection.CreateFile(ConnectionFactory.DbFileName);

            // テーブルの作成
            CreateTables();
        }

        private static void CreateTables()
        {
            using (var con = ConnectionFactory.Create())
            {
                // dapperを利用する場合、Enumはintで定義するとマッピングしてくれる
                var sql = @"
create table 
  worktasks
(
  id INTEGER PRIMARY KEY AUTOINCREMENT
  , title varchar(64)
  , taskcategory int 
  , product int
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
)

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
";

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }

        }
    }
}
