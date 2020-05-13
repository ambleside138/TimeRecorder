using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Domain.Tracking.Specifications;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Domain.Utility.Exceptions;

namespace TimeRecorder.Domain.UseCase.Tracking
{
    /// <summary>
    /// 作業時間 に関連するUseCaseを表します
    /// </summary>
    public class WorkingTimeRangeUseCase
    {
        private readonly IWorkingTimeRangeRepository _WorkingTimeRangeRepository;

        private readonly WorkingTimeRegistSpecification _WorkingTimeRegistSpecification;
         

        public WorkingTimeRangeUseCase(IWorkingTimeRangeRepository workingTimeRangeRepository)
        {
            _WorkingTimeRangeRepository = workingTimeRangeRepository;
            _WorkingTimeRegistSpecification = new WorkingTimeRegistSpecification(workingTimeRangeRepository);
        }

        public WorkingTimeRange AddWorkingTimeRange(WorkingTimeRange workingTimeRange)
        {
            var validationResult = _WorkingTimeRegistSpecification.IsSatisfiedBy(workingTimeRange);
            if(validationResult != null)
            {
                throw new SpecificationCheckException(validationResult);
            }

            return _WorkingTimeRangeRepository.Add(workingTimeRange);
        }

        public void StopWorkingTimeRange(Identity<WorkingTimeRange> id)
        {
            var target = _WorkingTimeRangeRepository.SelectById(id);

            if(target == null)
            {
                throw new NotFoundException("終了対象の作業がみつかりませんでした");
            }

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
