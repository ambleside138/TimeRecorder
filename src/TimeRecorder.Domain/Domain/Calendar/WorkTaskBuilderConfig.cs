using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Products;
using TimeRecorder.Domain.Domain.Tasks.Definitions;
using TimeRecorder.Domain.Domain.WorkProcesses;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Calendar
{
    public class WorkTaskBuilderConfig
    {
        public List<EventMapper> EventMappers { get; set; } = new List<EventMapper>();

        public List<TitleMapper> TitleMappers { get; set; } = new List<TitleMapper>();
    }

    public class EventMapper
    {
        public string EventKind { get; set; }

        public int WorkProcessId{ get; set; }
    }

    public class TitleMapper
    {
        public string ScheduleTitle { get; set; }

        public TaskCategory TaskCategory { get; set; }

        public int ProductId { get; set; }

        public int WorkProcessId { get; set; }
    }
}
