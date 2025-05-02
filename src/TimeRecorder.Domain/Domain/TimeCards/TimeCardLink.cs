using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Domain.Domain.TimeCards;
public class TimeCardLink
{
    public YearMonth YearMonth { get; }

    public string LinkURL { get; }

    public TimeCardLink(YearMonth yearMonth, string linkURL)
    {
        YearMonth = yearMonth;
        LinkURL = linkURL;
    }
}
