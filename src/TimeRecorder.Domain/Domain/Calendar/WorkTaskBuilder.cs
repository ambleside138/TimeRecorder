using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Calendar
{
    class WorkTaskBuilder
    {
        private readonly WorkTaskBuilderConfig _Config;

        public WorkTaskBuilder(WorkTaskBuilderConfig config)
        {
            _Config = config;
        }

        public WorkTask Build(ScheduledEvent scheduledEvent)
        {
            var oTask = WorkTask.FromScheduledEvent(scheduledEvent);

            var eventConfig = _Config.EventMappers.FirstOrDefault(t => t.EventKind == scheduledEvent.Kind);
            if(eventConfig != null)
            {
                oTask.ProcessId = new Identity<WorkProcesses.WorkProcess>(eventConfig.WorkProcessId);
            }

            var targetConfig = _Config.TitleMappers.FirstOrDefault(t => t.ScheduleTitle == oTask.Title);
            if(targetConfig != null)
            {
                oTask.TaskCategory = targetConfig.TaskCategory;
                oTask.ProductId = new Identity<Products.Product>(targetConfig.ProductId);
                oTask.ProcessId = new Identity<WorkProcesses.WorkProcess>(targetConfig.WorkProcessId);
            }

            return oTask;
        }
    }
}
