using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Domain.Calendar;

/// <summary>
/// カレンダー上の予定を表します
/// </summary>
public class ScheduledEvent
{
    public string Id { get; set; }

    public string Source { get; set; }

    public string Title { get; set; }

    public string Remarks { get; set; }

    public string Kind { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

}
