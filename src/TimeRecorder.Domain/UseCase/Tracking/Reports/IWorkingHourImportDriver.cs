using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tracking;

namespace TimeRecorder.Domain.UseCase.Tracking.Reports;

public interface IWorkingHourImportDriver
{
    public WorkingHour[] Import(string param);
}
