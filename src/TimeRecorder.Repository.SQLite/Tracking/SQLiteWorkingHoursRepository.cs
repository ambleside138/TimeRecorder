using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Repository.SQLite.Tracking.Dao;

namespace TimeRecorder.Repository.SQLite.Tracking;

public class SQLiteWorkingHoursRepository : IWorkingHourRepository
{
    public void Add(WorkingHour workingHour)
    {
        throw new NotImplementedException();
    }

    public void AddRange(WorkingHour[] workingHours)
    {
        RepositoryAction.Transaction((c, t) =>
        {
            var dao = new WorkingHourDao(c, t);

            dao.Delete(workingHours.Select(w => w.Ymd.Value).Distinct().ToArray());

            foreach (var row in workingHours.Where(w => w.IsEmpty == false))
            {
                dao.Insert(WorkingHourTableRow.FromDomainObjects(row));
            }
        });
    }

    public void Edit(WorkingHour workingHour)
    {
        throw new NotImplementedException();
    }

    public WorkingHour[] SelectByYearMonth(YearMonth yearMonth)
    {
        throw new NotImplementedException();
    }

    public WorkingHour SelectYmd(YmdString ymd)
    {
        throw new NotImplementedException();
    }
}
