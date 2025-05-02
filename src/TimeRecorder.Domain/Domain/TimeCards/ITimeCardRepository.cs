using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Domain.Domain.TimeCards;
public interface ITimeCardRepository
{
    TimeCardLink SelectByYearMonth(YearMonth yearMonth);

    void Put(TimeCardLink timeCardLink);
}
