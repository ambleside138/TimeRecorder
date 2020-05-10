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

        public void AddWorkingTimeRange(WorkingTimeRange workingTimeRange)
        {
            var validationResult = _WorkingTimeRegistSpecification.IsSatisfiedBy(workingTimeRange);
            if(string.IsNullOrEmpty( validationResult.ErrorMessage) == false)
            {
                throw new SpecificationCheckException(validationResult);
            }

            _WorkingTimeRangeRepository.Add(workingTimeRange);
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
