using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Utility.SystemClocks
{
    public class SystemClockServiceLocator
    {
        public static ISystemClock Current { get; private set; } = new DefaultSystemClock();

        public static void SetSystemClock(ISystemClock clock)
        {
            Current = clock;
        }
    }
}
