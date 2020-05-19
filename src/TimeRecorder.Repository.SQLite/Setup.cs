using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using TimeRecorder.Repository.SQLite.System.Versions;

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
            foreach (var command in VersionManager.Versions)
            {
                RepositoryAction.Transaction((connection, transaction) =>
                {
                    using (var cmd = new SQLiteCommand(command.CommandQuery, connection, transaction))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    const string sql = "INSERT INTO systemversionlog (version, updatetime) VALUES (@version, @now)";
                    connection.Execute(sql, new { version = command.Version, now = DateTime.Now }, transaction);
                });
   
            }

        }
    }
}
