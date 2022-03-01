using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.UseCase.Tracking;

public class GetWorkingTimeForTimelineUseCase
{
    private readonly IWorkingTimeQueryService _WorkingTimeQueryService;

    public GetWorkingTimeForTimelineUseCase(IWorkingTimeQueryService workingTimeQueryService)
    {
        _WorkingTimeQueryService = workingTimeQueryService;
    }

    public WorkingTimeForTimelineDto[] SelectByYmd(string ymd)
    {
        return _WorkingTimeQueryService.SelectByYmd(ymd);
    }
}
