using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace TimeRecorder.Repository.SQLite
{
    static class ConnectionFactory
    {
        public const string DbFileName = "timeRecorder.sqlite";

        public static SQLiteConnection Create()
        {
            var connection = new SQLiteConnection($"Data Source={DbFileName};Version=3;");
            connection.Open();

            return connection;
        }
    }
}
