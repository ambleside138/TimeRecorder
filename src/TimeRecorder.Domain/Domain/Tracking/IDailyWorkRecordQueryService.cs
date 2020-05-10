using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Domain.Tracking
{
    public interface IDailyWorkRecordQueryService
    {
        DailyWorkRecordHeader SelectByYmd(string ymd);
    }
}
