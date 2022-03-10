using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Domain.System;

public interface IHealthChecker
{
    SystemStatus CheckStatus();
}
