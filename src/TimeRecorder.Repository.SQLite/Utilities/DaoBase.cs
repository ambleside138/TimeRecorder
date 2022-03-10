using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace TimeRecorder.Repository.SQLite.Utilities;

abstract class DaoBase
{
    public DaoBase(SQLiteConnection connection, SQLiteTransaction transaction)
    {
        Connection = connection;
        Transaction = transaction;
    }

    public SQLiteConnection Connection { get; }
    public SQLiteTransaction Transaction { get; }
}
