using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Repository.SQLite.Tracking.Dao;

namespace TimeRecorder.Repository.SQLite.Tracking
{

    public class SQLiteWorkingTimeRangeRepository : IWorkingTimeRangeRepository
    {
        public WorkingTimeRange Add(WorkingTimeRange workingTimeRange)
        {
            var row = WorkingTimeTableRow.FromDomainObject(workingTimeRange);

            RepositoryAction.Transaction((c, t) =>
            {
                var dao = new WorkingTimeDao(c, t);
                var id = dao.Insert(row);

                // ID採番結果
                row.Id = id;
            });

            return row.ConvertToDomainObject();
        }

        public void Edit(WorkingTimeRange workingTimeRange)
        {
            RepositoryAction.Transaction((c, t) =>
            {
                var row = WorkingTimeTableRow.FromDomainObject(workingTimeRange);
                var dao = new WorkingTimeDao(c, t);
                dao.Update(row);
            });
        }

        public void Remove(Identity<WorkingTimeRange> identity)
        {
            RepositoryAction.Transaction((c, t) =>
            {
                new WorkingTimeDao(c, t).Delete(identity.Value);
            });
        }

        public WorkingTimeRange[] SelectByYmd(string ymd)
        {
            WorkingTimeRange[] results = null;

            RepositoryAction.Query(c =>
            {
                var dao = new WorkingTimeDao(c, null);

                results = dao.SelectYmd(ymd)
                             .Select(r => r.ConvertToDomainObject())
                             .OrderBy(r => r.StartDateTime)
                             .ToArray();
            });

            return results;
        }

        public WorkingTimeRange SelectById(Identity<WorkingTimeRange> id)
        {
            WorkingTimeRange result = null;

            RepositoryAction.Query(c =>
            {
                var dao = new WorkingTimeDao(c, null);

                result = dao.SelectId(id.Value)?.ConvertToDomainObject();
            });

            return result;
        }
    }
}
