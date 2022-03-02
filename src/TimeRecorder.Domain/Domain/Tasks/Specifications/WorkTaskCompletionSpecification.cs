using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Tracking;

namespace TimeRecorder.Domain.Domain.Tasks.Specifications;

/// <summary>
/// タスク完了時に満たすべき仕様を表します
/// </summary>
class WorkTaskCompletionSpecification
{
    private readonly IWorkingTimeRangeRepository _WorkingTimeRangeRepository;

    public WorkTaskCompletionSpecification(IWorkingTimeRangeRepository workingTimeRangeRepository)
    {
        _WorkingTimeRangeRepository = workingTimeRangeRepository;
    }

    public ValidationResult IsSatisfiedBy(WorkTask workTask)
    {
        var times = _WorkingTimeRangeRepository.SelectByTaskId(workTask.Id);

        if (times.Length == 0)
            return new ValidationResult("作業時間が登録されていません");

        if (times.Any(t => t.IsDoing))
            return new ValidationResult("作業中なので完了できません");

        return ValidationResult.Success;
    }
}
