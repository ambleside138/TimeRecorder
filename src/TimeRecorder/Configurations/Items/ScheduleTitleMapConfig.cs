using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using TimeRecorder.Domain.Domain.Calendar;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tasks.Definitions;

namespace TimeRecorder.Configurations.Items
{
    class ScheduleTitleMapConfig : ConfigItemBase
    {
        public ScheduleTitleMap[] ScheduleTitleMaps { get; set; }

        internal override ConfigKey Key => ConfigKey.ScheduleTitleMap;
    }
}
