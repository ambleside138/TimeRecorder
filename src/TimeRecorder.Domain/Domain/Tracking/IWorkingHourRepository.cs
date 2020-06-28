using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Tracking
{
    public interface IWorkingHourRepository
    {
        public void Add(WorkingHour workingHour);

        public void Edit(WorkingHour workingHour);

        public WorkingHour SelectYmd(YmdString ymd);

        public WorkingHour[] SelectByYearMonth(YearMonth yearMonth);
    }
}
