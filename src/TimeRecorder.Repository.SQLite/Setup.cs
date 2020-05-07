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
                // Enumはintで定義するとマッピングしてくれる
                var sql = @"
create table 
  worktasks
(
  id INTEGER PRIMARY KEY AUTOINCREMENT
  , title varchar(64)
  , taskcategory int 
  , product int
  , hospitalId int
  , processId int
  , remarks varchar(128)
  , planedStartDateTime datetime
  , planedEndDateTime datetime
  , actualStartDateTime datetime
  , actualEndDateTime datetime
)";

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }

        }
    }
}
