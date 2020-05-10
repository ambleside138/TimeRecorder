using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.UseCase.Tracking
{
    public interface IDailyWorkRecordQueryService
    {
        DailyWorkRecordHeader SelectByYmd(string ymd);
    }
}
