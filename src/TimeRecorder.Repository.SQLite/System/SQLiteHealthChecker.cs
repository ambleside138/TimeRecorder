using Dapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.System;
using TimeRecorder.Repository.SQLite.System.Versions;

namespace TimeRecorder.Repository.SQLite.System
{
    public class SQLiteHealthChecker : IHealthChecker
    {
        public SystemStatus CheckStatus()
        {
            // ファイル存在有無
            if (File.Exists(ConnectionFactory.DbFileName) == false)
                return SystemStatus.NotInitialized;

            // バージョン一致確認
            var dbVersion = GetSystemVersion();
            if (dbVersion != VersionManager.CurrentVersion)
                return SystemStatus.InvalidVersion;

            return SystemStatus.OK;
        }

        private bool ExistVersionTable()
        {
            #region SQL
            const string sql = @"
SELECT 
  COUNT(1) as cnt
FROM 
  sqlite_master 
WHERE 
  TYPE = 'table'
  AND name = 'systemversionlog'
";
            #endregion

            var existVersionTable = false;
            RepositoryAction.Query(c =>
            {
                var result = c.Query(sql);
                existVersionTable = result.FirstOrDefault()?.cnt > 0;
            });

            return existVersionTable;
        }

        public string GetSystemVersion()
        {
            #region SQL
            const string sql = @"
SELECT 
  MAX(version) version
FROM 
  systemversionlog 
";
            #endregion

            var version = "";
            RepositoryAction.Query(c =>
            {
                var result = c.Query(sql);
                version = result.FirstOrDefault()?.version ?? "";
            });

            return version;
        }
    }
}
