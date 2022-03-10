using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Utility.SystemClocks;

public class FixedSystemClock : ISystemClock
{
    public DateTime Now { get; }

    public FixedSystemClock(DateTime fixedTime)
    {
        Now = fixedTime;
    }
}
