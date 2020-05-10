using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tracking;

namespace TimeRecorder.Repository.SQLite.Tracking
{
    public class SQLiteDailyWorkRecordQueryService : IDailyWorkRecordQueryService
    {
        public DailyWorkRecordHeader SelectByYmd(string ymd)
        {
            throw new NotImplementedException();
        }
    }
}
