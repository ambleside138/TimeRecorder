using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Domain.Tracking.Specifications;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Domain.Utility.Exceptions;
using TimeRecorder.Domain.Utility.SystemClocks;

namespace TimeRecorder.Domain.UseCase.Tracking
{
    /// <summary>
    /// 作業時間 に関連するUseCaseを表します
    /// </summary>
    public class WorkingTimeRangeUseCase
    {
        private readonly IWorkingTimeRangeRepository _WorkingTimeRangeRepository;
        private readonly IWorkTaskRepository _WorkTaskRepository;

        private readonly WorkingTimeRegistSpecification _WorkingTimeRegistSpecification;
         

        public WorkingTimeRangeUseCase(IWorkingTimeRangeRepository workingTimeRangeRepository, IWorkTaskRepository workTaskRepository)
        {
            _WorkingTimeRangeRepository = workingTimeRangeRepository;
            _WorkingTimeRegistSpecification = new WorkingTimeRegistSpecification(workingTimeRangeRepository);
            _WorkTaskRepository = workTaskRepository;
        }

        public WorkingTimeRange StartWorking(Identity<WorkTask> id)
        {
            var targetTask = _WorkTaskRepository.SelectById(id);
            if (targetTask == null)
                throw new NotFoundException("開始対象のタスクがみつかりませんでした");

            // 実行中のタスクがあれば終了する
            var today = SystemClockServiceLocator.Current.Now.ToString("yyyyMMdd");
            var working = _WorkingTimeRangeRepository.SelectByYmd(today).FirstOrDefault(w => w.IsDoing);
            if (working != null)
            {
                StopWorkingCore(working);
            }

            var newWorkingTime = WorkingTimeRange.ForStart(id);

            var validationResult = _WorkingTimeRegistSpecification.IsSatisfiedBy(newWorkingTime);
            if(validationResult != null)
            {
                throw new SpecificationCheckException(validationResult);
            }

            return _WorkingTimeRangeRepository.Add(newWorkingTime);
        }

        public void StopWorking(Identity<WorkingTimeRange> id)
        {
            var target = _WorkingTimeRangeRepository.SelectById(id);

            if(target == null)
            {
                throw new NotFoundException("終了対象の作業がみつかりませんでした");
            }

            StopWorkingCore(target);
        }

        private void StopWorkingCore(WorkingTimeRange target)
        {
            target.Stop();

            _WorkingTimeRangeRepository.Edit(target);
        }
        
        public void EditWorkingTimeRange(WorkingTimeRange workingTimeRange)
        {
            var validationResult = _WorkingTimeRegistSpecification.IsSatisfiedBy(workingTimeRange);
            if (string.IsNullOrEmpty(validationResult.ErrorMessage) == false)
            {
                throw new SpecificationCheckException(validationResult);
            }

            _WorkingTimeRangeRepository.Edit(workingTimeRange);
        }

        public void DeleteWorkingTimeRange(Identity<WorkingTimeRange> identity)
        {
            _WorkingTimeRangeRepository.Remove(identity);
        }
    }
}
