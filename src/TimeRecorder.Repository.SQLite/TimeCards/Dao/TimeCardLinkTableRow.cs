using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.TimeCards;

namespace TimeRecorder.Repository.SQLite.TimeCards.Dao;
internal class TimeCardLinkTableRow
{
    public int Year { get; set; }
    public int Month { get; set; }

    public string LinkURL { get; set; }

    public static TimeCardLinkTableRow FromDomainObject(TimeCardLink timeCardLink)
        => new TimeCardLinkTableRow { Year = timeCardLink.YearMonth.Year, Month = timeCardLink.YearMonth.Month, LinkURL = timeCardLink.LinkURL, };

    public TimeCardLink ConvertToDomainObject()
        => new TimeCardLink(new Domain.Domain.YearMonth(Year, Month), LinkURL);
}
