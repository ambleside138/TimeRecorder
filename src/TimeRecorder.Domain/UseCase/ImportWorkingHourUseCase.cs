using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.UseCase.Tracking.Reports;

namespace TimeRecorder.Domain.UseCase;

public class ImportWorkingHourUseCase
{
    private readonly IWorkingHourRepository _WorkingHourRepository;
    private readonly IWorkingHourImportDriver _WorkingHourImportDriver;

    public ImportWorkingHourUseCase(IWorkingHourRepository workingHourRepository, IWorkingHourImportDriver workingHourImportDriver)
    {
        _WorkingHourRepository = workingHourRepository;
        _WorkingHourImportDriver = workingHourImportDriver;
    }

    public WorkingHour[] Import(string param)
    {
        var records = _WorkingHourImportDriver.Import(param);
        _WorkingHourRepository.AddRange(records);

        return records;
    }
}
