using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace TimeRecorder.Domain.Domain.Tracking.Specifications
{
    /// <summary>
    /// 作業時間の登録に関する仕様を表します
    /// </summary>
    class WorkingTimeRegistSpecification
    {
        private readonly IWorkingTimeRangeRepository _WorkingTimeRangeRepository;

        public WorkingTimeRegistSpecification(IWorkingTimeRangeRepository workingTimeRangeRepository)
        {
            _WorkingTimeRangeRepository = workingTimeRangeRepository;
        }

        public ValidationResult IsSatisfiedBy(WorkingTimeRange workingTimeRange)
        {
            if(workingTimeRange.TaskId == null)
            {
                return new ValidationResult("タスクが割り当てられていません");
            }

            var timeRangesAtSameday = _WorkingTimeRangeRepository.SelectByYmd(workingTimeRange.TimePeriod.TargetYmd)
                                                                 .Where(t => t.Id != workingTimeRange.Id)
                                                                 .ToArray();

            // 調整中...
            // 時間の重複チェック
            // 自身以外の作業時間と重複していないか
            //var overlapTimes = timeRangesAtSameday.Where(t => t.TimePeriod.IsOverlapped(workingTimeRange.TimePeriod)).ToArray();
            //if (overlapTimes.Any())
            //{

            //    return new ValidationResult("");
            //}

            // 新規登録の場合のみ
            if (workingTimeRange.Id.IsTemporary)
            {
                // 終了時間未設定のタスクがあればNG
                if (timeRangesAtSameday.Any(t => t.TimePeriod.IsStopped == false))
                    return new ValidationResult("未完了のタスクが残っています");
            }

            return ValidationResult.Success;
        }
    }
}
