using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Utility.SystemClocks;

public class DefaultSystemClock : ISystemClock
{
    public DateTime Now => DateTime.Now;
}
