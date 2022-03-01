using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Domain.Utility.Exceptions;

namespace TimeRecorder.Domain.UseCase.Tracking;

public class WorkingHourUseCase
{
    private readonly IWorkingHourRepository _WorkingHourRepository;

    public WorkingHourUseCase(IWorkingHourRepository workingHourRepository)
    {
        _WorkingHourRepository = workingHourRepository;
    }

    public void Import(WorkingHour[] workingHours)
    {
        _WorkingHourRepository.AddRange(workingHours);
    }


    public void StartWorking(DateTime time)
    {
        _WorkingHourRepository.Add(WorkingHour.CreateForStart(time));
    }

    public void EndWorking(DateTime time)
    {
        var target = _WorkingHourRepository.SelectYmd(new YmdString(time));
        if (target == null)
            throw new NotFoundException("対応する始業時間が登録されていません");

        target.End(time);

        _WorkingHourRepository.Edit(target);
    }

    public WorkingHour GetWorkingHour(YmdString ymd)
    {
        return _WorkingHourRepository.SelectYmd(ymd);
    }
}
