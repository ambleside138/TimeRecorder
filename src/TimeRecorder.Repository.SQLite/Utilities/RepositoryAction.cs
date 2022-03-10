using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace TimeRecorder.Repository.SQLite;

class RepositoryAction
{
    public static void Query(Action<SQLiteConnection> action)
    {
        using var conn = ConnectionFactory.Create();
        action(conn);
    }

    public static void Transaction(Action<SQLiteConnection, SQLiteTransaction> action)
    {
        using var connection = ConnectionFactory.Create();
        using var tran = connection.BeginTransaction();
        try
        {
            action(connection, tran);

            tran.Commit();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            tran.Rollback();
        }
    }
}
