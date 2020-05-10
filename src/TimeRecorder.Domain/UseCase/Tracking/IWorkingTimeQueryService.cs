using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.UseCase.Tracking
{
    // QueryServiceのIF・戻りの型はUseCase層で定義する

    public interface IWorkingTimeQueryService
    {
        WorkingTimeForTimelineDto[] SelectByYmd(string ymd);
    }
}
