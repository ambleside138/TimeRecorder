using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.UseCase.Tasks
{
    public class GetWorkTaskWithTimesUseCase
    {
        private readonly IWorkTaskWithTimesQueryService _WorkTaskWithTimesQueryService;

        public GetWorkTaskWithTimesUseCase(IWorkTaskWithTimesQueryService workTaskWithTimesQueryService)
        {
            _WorkTaskWithTimesQueryService = workTaskWithTimesQueryService;
        }

        public WorkTaskWithTimesDto[] GetByYmd(YmdString ymdString, bool containsCompleted)
        {
            return _WorkTaskWithTimesQueryService.SelectByYmd(ymdString, containsCompleted);
        }
    }
}
