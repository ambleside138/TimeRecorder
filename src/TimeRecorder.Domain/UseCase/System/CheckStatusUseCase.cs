using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.System;

namespace TimeRecorder.Domain.UseCase.System;

public class CheckStatusUseCase
{
    private readonly IHealthChecker _HealthChecker;

    public CheckStatusUseCase(IHealthChecker healthChecker)
    {
        _HealthChecker = healthChecker;
    }

    public SystemStatus CheckSystemStatus()
    {
        return _HealthChecker.CheckStatus();
    }
}
