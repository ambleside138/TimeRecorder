using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Repository.SQLite.Tracking
{
    class SQLiteWorkingHoursRepository : IWorkingHourRepository
    {
        public void Add(WorkingHour workingHour)
        {
            throw new NotImplementedException();
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
}
