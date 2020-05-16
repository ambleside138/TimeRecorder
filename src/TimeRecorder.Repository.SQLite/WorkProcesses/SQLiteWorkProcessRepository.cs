using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.WorkProcesses;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Repository.SQLite.WorkProcesses
{
    public class SQLiteWorkProcessRepository : IWorkProcessRepository
    {
        public WorkProcess Regist(WorkProcess process)
        {
            throw new NotImplementedException();
        }

        public WorkProcess[] SelectAll()
        {
            #region SQL
            const string sql = @"
SELECT
  id
  , title
FROM
  processes
";
            #endregion

            var list = new List<WorkProcess>();

            RepositoryAction.Query(c =>
            {
                list.AddRange(c.Query<ProcessTableRow>(sql).Select(r => new WorkProcess(new Identity<WorkProcess>(r.Id), r.Title)));
            });

            return list.ToArray();
        }

        class ProcessTableRow
        {
            public int Id { get; set; }

            public string Title { get; set; }
        }
    }
}
