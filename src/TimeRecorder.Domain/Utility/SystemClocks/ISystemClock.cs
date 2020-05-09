using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Utility.SystemClocks
{
    /// <summary>
    /// 現在時刻を取得するプロパティを提供します
    /// </summary>
    public interface ISystemClock
    {
        DateTime Now { get; }
    }
}
