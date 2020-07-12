using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using TimeRecorder.Repository.SQLite.System;
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
            ExecuteVersionUpQuery(VersionManager.Versions);
        }

        public static void VersionUp()
        {
            var healthChecker = new SQLiteHealthChecker();
            var currentVer = healthChecker.GetSystemVersion();

            ExecuteVersionUpQuery(VersionManager.Versions.Where(v => v.Version.CompareTo(currentVer) > 0));
        }

        private static void ExecuteVersionUpQuery(IEnumerable<IVersion> versions)
        {
            foreach (var command in versions)
            {
                RepositoryAction.Transaction((connection, transaction) =>
                {
                    using (var cmd = new SQLiteCommand(command.CommandQuery, connection, transaction))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // バージョン情報の書き込み
                    const string sql = "INSERT INTO systemversionlog (version, updatetime) VALUES (@version, @now)";
                    connection.Execute(sql, new { version = command.Version, now = DateTime.Now }, transaction);
                });
   
            }

        }
    }
}
