using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.WorkProcesses;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Repository.SQLite.WorkProcesses.Dao;

namespace TimeRecorder.Repository.SQLite.WorkProcesses;

public class SQLiteWorkProcessRepository : IWorkProcessRepository
{
    public WorkProcess Regist(WorkProcess process)
    {
        throw new NotImplementedException();
    }

    public WorkProcess[] SelectAll()
    {
        var list = new List<WorkProcess>();

        RepositoryAction.Query(c =>
        {
            var rows = new WorkProcessDao(c, null).SelectAll();
            list.AddRange(rows.Select(r => r.ToDomainObject()));
        });

        return list.ToArray();
    }


}
