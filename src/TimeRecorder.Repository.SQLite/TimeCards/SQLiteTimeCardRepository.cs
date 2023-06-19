using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.TimeCards;
using TimeRecorder.Repository.SQLite.Tasks;
using TimeRecorder.Repository.SQLite.Tasks.Dao;
using TimeRecorder.Repository.SQLite.TimeCards.Dao;

namespace TimeRecorder.Repository.SQLite.TimeCards;
public class SQLiteTimeCardRepository : ITimeCardRepository
{
    public void Put(TimeCardLink timeCardLink)
    {
        var row = TimeCardLinkTableRow.FromDomainObject(timeCardLink);

        RepositoryAction.Transaction((c, t) =>
        {
            var dao = new TimeCardLinkDao(c, t);
            dao.Upsert(row);
        });
    }

    public TimeCardLink SelectByYearMonth(YearMonth yearMonth)
    {
        TimeCardLink result = null;

        RepositoryAction.Query(c =>
        {
            var dao = new TimeCardLinkDao(c, null);
            result = dao.Select(yearMonth)?.ConvertToDomainObject() ?? new TimeCardLink(yearMonth, "");
        });

        return result;
    }
}
